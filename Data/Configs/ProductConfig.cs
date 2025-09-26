using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;
using static System.Net.WebRequestMethods;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Drawing;
using Microsoft.AspNetCore.Http.HttpResults;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace ECPAPI.Data.Configs
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.CategoryId).IsRequired();
            builder.Property(x => x.Stock).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.Status).HasConversion<string>().IsRequired();
            builder.Property(x => x.Images).HasConversion(x => JsonSerializer.Serialize(x, (JsonSerializerOptions)null),
               x => JsonSerializer.Deserialize<List<string>>(x, (JsonSerializerOptions)null) ?? new List<string>()).IsRequired();

            //builder.Property(p => p.Images).HasConversion(v => JsonSerializer.Serialize(
            //    v, (JsonSerializerOptions)null), v => JsonSerializer.Deserialize<List<string>>(
            //    v, (JsonSerializerOptions)null)).HasColumnType("nvarchar(max)");

            builder.HasData(new List<Product>()
            {
                new Product
                {
                    Id = 1,
                    Name = "Apple IPHONE 16Pro",
                    Description = "Apple IPHONE 16Pro",
                    Price = 2999,
                    CategoryId = 1,
                    Stock = 100,
                    CreatedAt = DateTime.UtcNow,
                    Status = "Active",
                    Images = new List<string>{"https://iplus.com.ge/images/detailed/11/iPhone_16_Pro_Max_Black_Titanium_PDP_Image_Position_1__ce-WW_gcrc-kg.jpg",
                    "https://iplus.com.ge/images/thumbnails/4889/4000/detailed/11/iPhone_16_Pro_Max_Black_Titanium_PDP_Image_Position_1b__ce-WW.jpg",
                    "https://iplus.com.ge/images/thumbnails/4889/4000/detailed/11/iPhone_16_Pro_Max_Black_Titanium_PDP_Image_Position_2__ce-WW.jpg"
                    }
                },
                
                new Product
                {
                    Id = 2,
                    Name = "Apple MacBookAir 15",
                    Description = "15-inch MacBook Air: Apple M4 chip with 10-core CPU and 10-core GPU, 24GB, 512GB SSD - Sky Blue MC7D4",
                    Price = 5499,
                    CategoryId = 2,
                    Stock = 1000,
                    CreatedAt = DateTime.UtcNow,
                    Status = "Active",
                    Images = new List<string>{ "https://iplus.com.ge/images/thumbnails/1100/900/detailed/12/sk.bl_7f88-gy.png",
                    "https://iplus.com.ge/images/thumbnails/1100/900/detailed/12/sk,%E1%83%93_j3ao-6y.png",}
                },

                new Product
                {
                    Id = 3,
                    Name = "Apple Watch 10 GPS 46mm",
                    Description = "Apple Watch Series 10 GPS 46mm Jet Black Aluminum Case with Black Sport Band",
                    Price = 1399,
                    CategoryId = 3,
                    Stock = 1000,
                    CreatedAt = DateTime.UtcNow,
                    Status = "Active",
                    Images = new List<string>{ "https://iplus.com.ge/images/thumbnails/1100/900/detailed/11/Apple_Watch_Series_10_42mm_GPS_Jet_Black_Aluminum_Sport_Band_Black_PDP_Image_Position_2__ce-WW_4ixe-va.jpg",
                    "https://iplus.com.ge/images/thumbnails/1100/900/detailed/11/Apple_Watch_Series_10_42mm_GPS_Jet_Black_Aluminum_Sport_Band_Black_PDP_Image_Position_1__ce-WW_s9un-1a.jpg", }
                },


            });
            builder.HasOne(x => x.Category).WithMany(x => x.Products).
                HasForeignKey(x => x.CategoryId).HasConstraintName("FK_Products_Category");
        }
    }
}


//ImagesJson = "[\"https://iplus.com.ge/images/detailed/11/iPhone_16_Pro_Max_Black_Titanium_PDP_Image_Position_1__ce-WW_gcrc-kg.jpg\"]"