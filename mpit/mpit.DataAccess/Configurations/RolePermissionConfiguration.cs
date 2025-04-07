using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mpit.DataAccess;
using mpit.mpit.Core.Enums;
using mpit.mpit.DataAccess.Entities;

namespace mpit.mpit.DataAccess.Configurations
{
    public partial class RolePermissionConfiguration(AuthorizationOptions authorizationOptions)
        : IEntityTypeConfiguration<RolePermissionEntity>
    {
        private readonly AuthorizationOptions _authorizationOptions = authorizationOptions;

        public void Configure(EntityTypeBuilder<RolePermissionEntity> builder)
        {
            builder.HasKey(r => new { r.RoleId, r.PermissionId });

            builder.HasData(ParseRolePermissions());
        }

        private RolePermissionEntity[] ParseRolePermissions() =>
            _authorizationOptions
                .RolePermissions.SelectMany(rp =>
                    rp.Permissions.Select(p => new RolePermissionEntity
                    {
                        RoleId = (int)Enum.Parse<Role>(rp.Role),
                        PermissionId = (int)Enum.Parse<Permission>(p),
                    })
                )
                .ToArray();
    }
}
