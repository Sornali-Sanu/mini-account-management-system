using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MiniAccountSystem.Models.ChartOfAccount
{
    public class AccountType
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountTypeID { get; set; }

        [Required] 
        [StringLength(50)] 
        [Display(Name = "Account Type Name")]
        public string AccountTypeName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; } = DateTime.Now; 

        [Display(Name = "Updated Date")]
        [DataType(DataType.DateTime)]
        public DateTime UpdatedDate { get; set; } = DateTime.Now; 

        
        public ICollection<Account> Accounts { get; set; }
    }
}
