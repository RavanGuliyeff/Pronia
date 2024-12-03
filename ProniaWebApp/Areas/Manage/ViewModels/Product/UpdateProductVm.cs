namespace ProniaWebApp.Areas.Manage.ViewModels.Product
{
    public record UpdateProductVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string SKU { get; set; }
        public double Price { get; set; }
        public IFormFile? MainPhoto { get; set; }
        public List<IFormFile>? Images { get; set; }
        public List<int> CategoryIds { get; set; }
        public List<int> TagIds { get; set; }

        public List<ProductImagesVm>? ProductImagesVms { get; set; }
        public List<string> ImageUrls { get; set; }
    }

    public record ProductImagesVm
    {
        public string ImgUrl { get; set; }
        public IFormFile Image { get; set; }
        public bool Primary { get; set; }
    }
}
