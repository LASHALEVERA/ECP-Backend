using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace ECPAPI.Models
{
    public class ProductDTO
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

        //public ICollection<Image> Images { get; set; } = new List<Image>();

    }
}
