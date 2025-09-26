using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics;

namespace ECPAPI.Data.Configs
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.UserNameOrEmail).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.PasswordSalt).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.IsDeleted).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.UserTypeId).IsRequired();

            builder.HasData(
                new User
                {
                    Id = 1,
                    UserNameOrEmail = "Lasha13",
                    Email = "lashalevera13@gmail.com",
                    //Password: Lasha123
                    Password = "KGxwZt9V6Q9xi3KBxjud5HR1FUIudVy6ZCxKVR4+Sx4=",
                    PasswordSalt = "/QkrQC3HMguvuwKh9qHWeQ==",
                    UserTypeId = 2,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedDate = new DateTime(2025, 8, 21),
                    ModifiedDate = new DateTime(2025, 08, 21)
                } 
            );

            builder.HasOne(x => x.UserType).WithMany(x => x.Users)
                   .HasForeignKey(x => x.UserTypeId).HasConstraintName("FK_Users_UserTypes");
        }
    }
}
