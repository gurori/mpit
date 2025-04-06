using Microsoft.EntityFrameworkCore;
using mpit.mpit.Application.Interfaces.Repositories;
using mpit.mpit.DataAccess.DbContexts;

namespace mpit.mpit.DataAccess.Repositories
{
    public sealed class RoleRepository(ApplicationDbContext context) : IRoleRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<HashSet<string>> GetPermissionsAsync(string roleName)
        {
            var permissions = await _context
                .Roles.AsNoTracking()
                .Include(r => r.Permissions)
                .Where(r => r.Name.ToLower() == roleName)
                .Select(r => r.Permissions)
                .ToArrayAsync();

            return permissions.SelectMany(p => p).Select(p => p.Name).ToHashSet();
        }
    }
}
