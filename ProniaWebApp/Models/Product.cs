
namespace ProniaWebApp.Models
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string SKU { get; set; }
        public double Price { get; set; }
        public List<ProductImage>? ProductImages { get; set; }
        public List<ProductCategory>? ProductCategories { get; set; }
        public List<ProductTag>? ProductTags { get; set; }
    }
}
