using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECPAPI.Data.Configs
{
    public class UserRoleMappingConfig : IEntityTypeConfiguration<UserRoleMapping> 
    {
        public void Configure(EntityTypeBuilder<UserRoleMapping> builder)
        {
            builder.ToTable("UserRoleMappings");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasIndex(x => new { x.UserId, x.RoleId }, "UK_UserRoleMapping").IsUnique();

            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.RoleId).IsRequired();

            builder.HasOne(x => x.Role)
                .WithMany(x => x.UserRoleMappings)
                .HasForeignKey(x => x.RoleId)
                .HasConstraintName("FK_UserMapping_Role");

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserRoleMappings)
                .HasForeignKey(x => x.UserId)
                .HasConstraintName("FK_UserMappings_User");
        }

    }
}
