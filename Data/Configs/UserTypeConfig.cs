using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECPAPI.Data.Configs
{
    public class UserTypeConfig : IEntityTypeConfiguration<UserType>
    {
        public void Configure(EntityTypeBuilder<UserType> builder)
        {
            builder.ToTable("UserTypes");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(1500);
            builder.HasData(new List<UserType>()
            {

                new UserType
                {
                    Id = 1,
                    Name = "Guest",
                    Description = "For View",
                },
                new UserType
                {
                    Id= 2,
                    Name = "Admin",
                    Description = "For Changes"
                },
                new UserType
                {
                    Id= 3,
                    Name = "User",
                    Description = "For Order",
                }
            });
        }
    }
}
