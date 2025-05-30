using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MiniAccountSystem.Models;
using MiniAccountSystem.Models.VoucherModels;

namespace MiniAccountSystem.Pages.Vouchers
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }
        public IList<Voucher> Vouchers { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Vouchers = await _context.Vouchers
                .Include(v => v.Entries)  
                .ToListAsync();
        }

    }
}
