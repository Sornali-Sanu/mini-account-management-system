using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MiniAccountSystem.Models.VoucherModels
{
    public class Voucher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VoucherID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Voucher Type")]
        public string VoucherType { get; set; } // e.g., Journal, Payment, Receipt

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Voucher Date")]
        public DateTime VoucherDate { get; set; }

       
        [StringLength(50)]
        [Display(Name = "Reference No")]
        public string ReferenceNo { get; set; }

        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }

        [Display(Name = "Created At")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation property: One voucher has many entries
        public List<VoucherEntry> Entries { get; set; } = new List<VoucherEntry>();

    }
}
