using System.ComponentModel.DataAnnotations;

namespace MiniAccountSystem.Models.ChartOfAccount
{
    public class AccountViewModel
    {
        public int AccountID { get; set; }

        [Required]
        [StringLength(100)]
        public string AccountName { get; set; }

        [StringLength(20)]
        public string AccountCode { get; set; }

 
        public int AccountTypeID { get; set; }

        public string AccountTypeName { get; set; } 

        public int? ParentAccountID { get; set; }

        public string ParentAccountName { get; set; } 

        [StringLength(500)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
