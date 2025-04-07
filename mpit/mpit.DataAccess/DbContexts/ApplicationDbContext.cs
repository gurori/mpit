using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using mpit.DataAccess;
using mpit.mpit.DataAccess.Configurations;
using mpit.mpit.DataAccess.Entities;

namespace mpit.mpit.DataAccess.DbContexts;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IOptions<AuthorizationOptions> authOptions
) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(authOptions.Value));
    }
}
