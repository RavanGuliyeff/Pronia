namespace ProniaWebApp.ViewComponents
{
	public class ProductViewComponent:ViewComponent
	{
        AppDbContext _db;

        public ProductViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync(string key)
		{

            List<Product> products = _db.Products
                .Include(p => p.ProductImages)
                .ToList();
            List<HomeProductVm> productVms = new List<HomeProductVm>();
            foreach (Product product in products)
            {
                HomeProductVm vm = new HomeProductVm()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Count = product.Count,
                    ProductImages = product.ProductImages
                };
                productVms.Add(vm);
            }

            List<HomeProductVm> productSender = new List<HomeProductVm>();

            switch (key.ToLower())
            {
                case "featured":
                    productSender = productVms.OrderByDescending(p => p.Price).ToList();
                    ProductTaker(ref productSender);
                    break;
                case "bestseller":
                     productSender = productVms.OrderBy(p => p.Count).ToList();
                    ProductTaker(ref productSender);
                    break;
                case "latest":
                    productSender = productVms.OrderByDescending(p => p.Id).ToList();
                    ProductTaker(ref productSender);
                    break;
                default:
                    break;
            }

            return View(productSender);
		}


        public void ProductTaker(ref List<HomeProductVm>? productSender)
        {
            productSender = productSender.Take(12).Any()
                        ? productSender.Take(12).ToList()
                        : productSender;
        }
	}
}
