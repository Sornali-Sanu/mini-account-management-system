using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MiniAccountSystem.Models;
using MiniAccountSystem.Models.ChartOfAccount;

namespace MiniAccountSystem.Pages.Accounts
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

       
        

        public List<Account> AccountsList { get; set; }=new List<Account>();

        private async Task LoadAccountsAsync()
        {
            AccountsList = await _context.Accounts
                .Include(a => a.AccountType)
                .Include(a => a.ParentAccount)
                .ToListAsync();
        }

        public async Task OnGetAsync()
        {
         await LoadAccountsAsync();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
          
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
