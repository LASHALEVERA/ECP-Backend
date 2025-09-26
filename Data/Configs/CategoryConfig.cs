using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace ECPAPI.Data.Configs
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");
            builder.HasKey(x => x.CategoryId);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.ParentId);
            builder.Property(x => x.Description).HasMaxLength(250);
            //builder.Property(x => x.image);
            //builder.Property(x => x.Images).HasConversion(v => JsonSerializer.Serialize(
            //    v, (JsonSerializerOptions)null), v => JsonSerializer.Deserialize<List<string>>(
            //    v, (JsonSerializerOptions)null)).HasColumnType("nvarchar(max)");

            builder.HasData(new List<Category>()
            {
                new Category
                {
                    CategoryId = 1,
                    Name = "Mobile Phone",
                    ParentId = 0,
                    Description = "CellPhones Category",
                    image = "default.jpg"
                    //Images = new List<string>
                    //{"https://iplus.com.ge/images/detailed/11/iPhone_16_Pro_Max_Black_Titanium_PDP_Image_Position_1__ce-WW_gcrc-kg.jpg"
                    //}
                },
                new Category
                {
                    CategoryId = 2,
                    Name = "Laptop",
                    ParentId = 0,
                    Description = "Laptop Category",
                    image = "default.jpg"
                },

                new Category
                {
                    CategoryId = 3,
                    Name = "Watch",
                    ParentId = 0,
                    Description = "Smart Watch Category",
                    image = "default.jpg"
                }

           });
        }
    }
}
