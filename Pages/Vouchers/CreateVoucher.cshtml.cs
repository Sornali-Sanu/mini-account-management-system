using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniAccountSystem.Models;
using MiniAccountSystem.Models.VoucherModels;
using MiniAccountSystem.Services;

namespace MiniAccountSystem.Pages.Vouchers
{
    [Authorize(Roles = "Admin,Accountant")]
    public class CreateVoucherModel : PageModel
    {
        private readonly VoucherService _voucherService;
        private readonly AppDbContext _context;

        public CreateVoucherModel(VoucherService voucherService, AppDbContext context)
        {
            _voucherService = voucherService;
            _context = context;
        }
        [BindProperty]
        public Voucher Voucher { get; set; }

        public SelectList VoucherTypes { get; set; }
        public SelectList Accounts { get; set; }

        public void OnGet()
        {
            VoucherTypes = new SelectList(new List<string> { "Journal", "Payment", "Receipt" });
            Accounts = new SelectList(_context.Accounts.Where(a => a.IsActive), "AccountID", "AccountName");

            
            Voucher = new Voucher();
            Voucher.Entries.Add(new VoucherEntry());
        }
        public async Task<IActionResult> OnPostAsync()
        {
            VoucherTypes = new SelectList(new List<string> { "Journal", "Payment", "Receipt" });
            Accounts = new SelectList(_context.Accounts.Where(a => a.IsActive), "AccountID", "AccountName");

        
            if (Voucher.Entries == null || !Voucher.Entries.Any(e => e.DebitAmount > 0 || e.CreditAmount > 0))
            {
                ModelState.AddModelError(string.Empty, "Please enter at least one voucher entry with debit or credit amount.");
            }

            
            decimal totalDebit = Voucher.Entries.Sum(e => e.DebitAmount);
            decimal totalCredit = Voucher.Entries.Sum(e => e.CreditAmount);

            if (totalDebit != totalCredit)
            {
                ModelState.AddModelError(string.Empty, "Total debit and credit amounts must be equal.");
            }

            if (!ModelState.IsValid)
            {
              
            }

            
            Voucher.CreatedBy = 1;
            Voucher.CreatedAt = System.DateTime.Now;

            await _voucherService.SaveVoucherAsync(Voucher);

            return RedirectToPage("./Index"); 
        }
    }
}
