using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using mpit.mpit.Core.Enums;
using mpit.mpit.DataAccess.Entities;

namespace mpit.mpit.DataAccess.Configurations
{
    public partial class PermissionConfiguration : IEntityTypeConfiguration<PermissionEntity>
    {
        public void Configure(EntityTypeBuilder<PermissionEntity> builder)
        {
            builder.HasKey(p => p.Id);

            var permissions = Enum.GetValues<Permission>()
                .Select(p => new PermissionEntity { Id = (int)p, Name = p.ToString() });

            builder.HasData(permissions);
        }
    }
}
