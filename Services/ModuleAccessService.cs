using Microsoft.EntityFrameworkCore;
using MiniAccountSystem.Models;

namespace MiniAccountSystem.Services
{
    public class ModuleAccessService
    {
        private readonly AppDbContext _context;

        public ModuleAccessService(AppDbContext context)
        {
            _context = context;
        }

        public async Task GrantAccessAsync(string roleName, string moduleName)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC GrantModuleAccess @p0, @p1", roleName, moduleName);
        }

        public async Task RevokeAccessAsync(string roleName, string moduleName)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC RevokeModuleAccess @p0, @p1", roleName, moduleName);
        }

        public async Task<bool> HasAccessAsync(string roleName, string moduleName)
        {
            if (roleName == "Admin")
            {
                return true; 
            }
            var result = _context.RoleModuleAccess
                .FromSqlRaw("EXEC CheckModuleAccess @p0, @p1", roleName, moduleName)
                .AsNoTracking()
                .AsEnumerable()
                .FirstOrDefault();

            return result?.HasAccess ?? false;
        }
    }
}
