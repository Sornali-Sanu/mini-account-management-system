using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using MiniAccountSystem.Models.ChartOfAccount;
using MiniAccountSystem.Models;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MiniAccountSystem.Pages.Accounts
{
    [Authorize(Roles = "Admin,Accountant")]
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AccountViewModel Account { get; set; }

        public SelectList AccountTypes { get; set; }
        public SelectList ParentAccounts { get; set; }

        public async Task OnGetAsync()
        {
            
            var accountTypes = await _context.AccountTypes.ToListAsync();
            AccountTypes = new SelectList(accountTypes, "AccountTypeID", "AccountTypeName");

            var parentAccounts = await _context.Accounts.ToListAsync();
            ParentAccounts = new SelectList(parentAccounts, "AccountID", "AccountName");
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"Model error in {entry.Key}: {error.ErrorMessage}");
                    }
                }
            }
            

            var connection = _context.Database.GetDbConnection();
            try
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "sp_ManageChartOfAccounts";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Action", "Insert"));
                    command.Parameters.Add(new SqlParameter("@AccountID", SqlDbType.Int) { Direction = ParameterDirection.Output });
                    command.Parameters.Add(new SqlParameter("@AccountName", Account.AccountName ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@AccountCode", Account.AccountCode ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@AccountTypeID", Account.AccountTypeID));
                    command.Parameters.Add(new SqlParameter("@ParentAccountID", Account.ParentAccountID.HasValue ? (object)Account.ParentAccountID.Value : DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@Description", Account.Description ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@IsActive", Account.IsActive));


                    
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    Console.WriteLine("Rows affected: " + rowsAffected);

                    var newId = (int)command.Parameters["@AccountID"].Value;
                    if (newId <= 0)
                    {
                        ModelState.AddModelError(string.Empty, "Failed to create new account.");
                        await OnGetAsync();
                        return Page();
                    }
                   
                }
            }
            

            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error creating account: " + ex.Message);
                await OnGetAsync();
                return Page();
            }
            finally
            {
                await connection.CloseAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

