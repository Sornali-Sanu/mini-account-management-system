using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniAccountSystem.Models
{
    [Table("RoleModuleAccess")]
    public class RoleModuleAccess
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string RoleName { get; set; }

      
        [StringLength(100)]
        public string ModuleName { get; set; }

        public bool HasAccess { get; set; }
    }
}
