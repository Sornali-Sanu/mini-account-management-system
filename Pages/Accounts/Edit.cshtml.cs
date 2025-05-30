using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MiniAccountSystem.Models;
using MiniAccountSystem.Models.ChartOfAccount;

namespace MiniAccountSystem.Pages.Accounts
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public AccountViewModel Account { get; set; }

        public SelectList AccountTypeList { get; set; }
        public SelectList ParentAccountList { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var account = await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.AccountID == id);

            if (account == null)
            {
                return NotFound();
            }

            Account = new AccountViewModel
            {
                AccountID = account.AccountID,
                AccountName = account.AccountName,
                AccountCode = account.AccountCode,
                AccountTypeID = account.AccountTypeID,
                ParentAccountID = account.ParentAccountID,
                Description = account.Description,
                IsActive = account.IsActive,
                CreatedDate = account.CreatedDate,
                UpdatedDate = account.UpdatedDate
            };

            await LoadSelectLists();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectLists();
                
            }

            var parameters = new[]
            {
                new SqlParameter("@Action", "Update"),
                new SqlParameter("@AccountID", Account.AccountID),
                new SqlParameter("@AccountName", Account.AccountName ?? (object)DBNull.Value),
                new SqlParameter("@AccountCode", Account.AccountCode ?? (object)DBNull.Value),
                new SqlParameter("@AccountTypeID", Account.AccountTypeID),
                new SqlParameter("@ParentAccountID", (object?)Account.ParentAccountID ?? DBNull.Value),
                new SqlParameter("@Description", Account.Description ?? (object)DBNull.Value),
                new SqlParameter("@IsActive", Account.IsActive),
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC sp_ManageChartOfAccounts @Action, @AccountID, @AccountName, @AccountCode, @AccountTypeID, @ParentAccountID, @Description, @IsActive", parameters);

            return RedirectToPage("./Index");
        }

        private async Task LoadSelectLists()
        {
            AccountTypeList = new SelectList(await _context.AccountTypes.ToListAsync(), "AccountTypeID", "AccountTypeName");
            ParentAccountList = new SelectList(await _context.Accounts.ToListAsync(), "AccountID", "AccountName");
        }
    }
}
