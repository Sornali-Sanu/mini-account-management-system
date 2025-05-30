using MiniAccountSystem.Models.ChartOfAccount;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MiniAccountSystem.Models.VoucherModels
{
    public class VoucherEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EntryID { get; set; }

     
        [ForeignKey("Voucher")]
        public int VoucherID { get; set; }

       
        [ForeignKey("Account")]
        public int AccountID { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Debit Amount")]
        public decimal DebitAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Credit Amount")]
        public decimal CreditAmount { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        // Navigation Properties
        public Voucher Voucher { get; set; }
        public Account Account { get; set; }
    }
}
