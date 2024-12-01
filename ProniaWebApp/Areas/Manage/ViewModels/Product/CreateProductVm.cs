using ProniaWebApp.Models;

namespace ProniaWebApp.Areas.Manage.ViewModels.Product
{
	public record CreateProductVm
	{
		public string Name { get; set; }
		public string? Description { get; set; }
		public string SKU { get; set; }
		public double Price { get; set; }
		//public List<int>? ImageIds { get; set; }
		public List<int>? CategoryIds { get; set; }
		public List<int>? TagIds { get; set; }
	}
}
