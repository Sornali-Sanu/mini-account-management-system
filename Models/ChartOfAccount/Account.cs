using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MiniAccountSystem.Models.ChartOfAccount
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Account Name")]
        public string AccountName { get; set; }

        [StringLength(20)]
        [Display(Name = "Account Code")]
        public string AccountCode { get; set; }

        [Required]
        [Display(Name = "Account Type")]
        public int AccountTypeID { get; set; } 

        
        [ForeignKey("AccountTypeID")] 
        public AccountType AccountType { get; set; }

        [Display(Name = "Parent Account")]
        public int? ParentAccountID { get; set; } 

       
        [ForeignKey("ParentAccountID")]
        public Account ParentAccount { get; set; }

       
        public ICollection<Account> ChildrenAccounts { get; set; } = new List<Account>();

        [StringLength(500)]
        public string Description { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true; 

        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Updated Date")]
        [DataType(DataType.DateTime)]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}

