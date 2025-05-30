using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MiniAccountSystem.Models;
using MiniAccountSystem.Models.ChartOfAccount;
using MiniAccountSystem.Services;

namespace MiniAccountSystem.Pages.Accounts
{

    [Authorize(Roles = "Admin,Accountant,Viewer")]
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ModuleAccessService _accessService;
        private readonly UserManager<IdentityUser> _userManager;
        public IndexModel(AppDbContext context, ModuleAccessService accessService, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _accessService = accessService;
            _userManager = userManager;
        }

       
        

        public List<Account> AccountsList { get; set; }=new List<Account>();

        private async Task LoadAccountsAsync()
        {
            AccountsList = await _context.Accounts
                .Include(a => a.AccountType)
                .Include(a => a.ParentAccount)
                .ToListAsync();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            //await LoadAccountsAsync();
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            var hasAccess = await _accessService.HasAccessAsync(role, "Index");

            if (!hasAccess)
            {
                return Forbid();
            }

            await LoadAccountsAsync();
            return Page();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var connection = _context.Database.GetDbConnection();

            try
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "sp_ManageChartOfAccounts";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Action", "Delete"));

                    var idParam = new SqlParameter("@AccountID", id)
                    {
                        Direction = System.Data.ParameterDirection.Input
                    };
                    command.Parameters.Add(idParam);

                    
                    command.Parameters.Add(new SqlParameter("@AccountName", DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@AccountCode", DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@AccountTypeID", DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@ParentAccountID", DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@Description", DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@IsActive", DBNull.Value));

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error deleting account: {ex.Message}");
            }
            finally
            {
                await connection.CloseAsync();
            }

            await LoadAccountsAsync();
            return Page();


        }
    }
}
