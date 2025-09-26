using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECPAPI.Data.Configs
{
    public class RolePrivilegeConfig : IEntityTypeConfiguration<RolePrivilege>
    {
        public void Configure(EntityTypeBuilder<RolePrivilege> builder)
        {
            builder.ToTable("RolePrivileges");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.RolePrivilegeName).HasMaxLength(250).IsRequired();

            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.IsDeleted).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.CreationDate).IsRequired();

            builder.HasOne(x => x.Role).WithMany(x => x.RolePrivileges)
                .HasForeignKey(x => x.RoleId)
                .HasConstraintName("FK_RolePrivileges_Roles");
        }
    }
}
