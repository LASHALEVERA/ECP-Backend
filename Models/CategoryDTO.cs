namespace ECPAPI.Models
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public string Description { get; set; }
        public string image { get; set; } 

    }
}
