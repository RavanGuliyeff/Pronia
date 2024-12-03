
namespace ProniaWebApp.Controllers
{
	public class HomeController : Controller
	{
		AppDbContext _db;

		public HomeController(AppDbContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			List<Product> products = _db.Products
				.Include(p=> p.ProductImages)
				.ToList();
			List<HomeProductVm> productVms = new List<HomeProductVm>();
			foreach (Product product in products)
			{
				HomeProductVm vm = new HomeProductVm()
				{
					Id = product.Id,
					Name = product.Name,
					Price = product.Price,
					ProductImages = product.ProductImages
				};
				productVms.Add(vm);
			}

			List<Slider> sliders = _db.Sliders.ToList();
			List<SliderVm> sliderVms = new List<SliderVm>();

			foreach(Slider slider in sliders)
			{
				SliderVm vm = new SliderVm()
				{
					UpTitle = slider.UpTitle,
					DownTitle = slider.DownTitle,
					Description = slider.Description,
					ImgUrl = slider.ImgUrl,
				};

				sliderVms.Add(vm);
			}

			HomeVm homeVm = new HomeVm()
			{
				SliderVms = sliderVms,
				HomeProductVms = productVms
			};

			return View(homeVm);
		}

	}
}
