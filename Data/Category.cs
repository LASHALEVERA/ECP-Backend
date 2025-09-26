using System.Text.Json;

namespace ECPAPI.Data
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public string Description { get; set; }
        public string image { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        //public List<string> Images { get; set; } = new();
        //public string ImagesJson { get; set; } = "[]";

        //public List<string> Images
        //{
        //    get => JsonSerializer.Deserialize<List<string>>(ImagesJson) ?? new();
        //    set => ImagesJson = JsonSerializer.Serialize(value);
        //}

    }
}
