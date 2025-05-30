using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniAccountSystem.Models.ChartOfAccount;

namespace MiniAccountSystem.Models
{
    public class AppDbContext:IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>op):base(op)
        {
            
        }
        public virtual DbSet<Account>Accounts { get; set; }
        public virtual DbSet<AccountType> AccountTypes { get; set; }
        public virtual DbSet<RoleModuleAccess> RoleModuleAccess { get; set; }

    }
}
