using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MiniAccountSystem.Models;
using MiniAccountSystem.Models.ChartOfAccount;

namespace MiniAccountSystem.Pages.Accounts
{
    [Authorize(Roles = "Admin,Accountant,Viewer")]
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;

        public DetailsModel(AppDbContext context)
        {
            _context = context;
        }

        public Account Account { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Account = await _context.Accounts
                .Include(a => a.AccountType)
                .Include(a => a.ParentAccount)
                .FirstOrDefaultAsync(m => m.AccountID == id);

            if (Account == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
