
namespace ProniaWebApp.ViewModels
{
	public class HomeProductVm
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public List<ProductImage> ProductImages { get; set; }
    }
}
