
namespace ProniaWebApp.Controllers
{
	public class ProductController : Controller
	{
		AppDbContext _db;

		public ProductController(AppDbContext db)
		{
			_db = db;
		}
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null) return NotFound();

			var product = await _db.Products.Include(p => p.ProductCategories).ThenInclude(tp => tp.Category)
				.Include(p => p.ProductTags).ThenInclude(tp => tp.Tag)
				.Include(p => p.ProductImages)
			.FirstOrDefaultAsync(p => p.Id == id);



			return View(product);
		}
	}
}
