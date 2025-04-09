using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using mpit.DataAccess;
using mpit.mpit.Core.Enums;
using mpit.mpit.DataAccess.Entities;

namespace mpit.mpit.DataAccess.DbContexts;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IOptions<AuthorizationOptions> authOptions
) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<InfoEntity> Infos { get; set; }

    //public DbSet<HelpEntity> Helps { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        //modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(authOptions.Value));
        modelBuilder.Entity<RolePermissionEntity>().HasKey(r => new { r.RoleId, r.PermissionId });
        modelBuilder.Entity<RolePermissionEntity>().HasData(ParseRolePermissions());

        modelBuilder
            .Entity<UserEntity>()
            .HasOne(u => u.Info)
            .WithOne(i => i.User)
            .HasForeignKey<InfoEntity>(i => i.UserId);
        base.OnModelCreating(modelBuilder);
    }

    private RolePermissionEntity[] ParseRolePermissions() =>
        authOptions
            .Value.RolePermissions.SelectMany(rp =>
                rp.Permissions.Select(p => new RolePermissionEntity
                {
                    RoleId = (int)Enum.Parse<Role>(rp.Role),
                    PermissionId = (int)Enum.Parse<Permission>(p),
                })
            )
            .ToArray();
}
