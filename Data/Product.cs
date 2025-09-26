using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text.Json;
using ECPAPI.Data.Enums;

namespace ECPAPI.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public int? CategoryId { get; set; }
        public int Stock { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = "";
        public List<string> Images { get; set; } = new();
        public virtual Category? Category { get; set; }

        internal static void OrderProduct(int productId, int quantity)
        {
            throw new NotImplementedException("Added");
        }

    }
    //public enum ProductStatus
    //{
    //    Active,
    //    Inactive
    //}
}

//public ICollection<Image> Images { get; set; } = new List<Image>();
//public Image images { get; set; }
//public List<string> Images { get; set; } = new();
//public string ImagesJson { get; set; } = "[]";

//public List<string> Images
//{
//    get => JsonSerializer.Deserialize<List<string>>(ImagesJson) ?? new();
//    set => ImagesJson = JsonSerializer.Serialize(value);
//}

//[NotMapped]
//public List<string> ImageUrls
//{
//    get => JsonSerializer.Deserialize<List<string>>(Images) ?? new();
//    set => Images = JsonSerializer.Serialize(value);
//}