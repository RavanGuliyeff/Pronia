namespace ProniaWebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;

        public ProductController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _db.Products
                .Include(p => p.ProductCategories)
                    .ThenInclude(tp => tp.Category)
                .Include(p => p.ProductTags)
                    .ThenInclude(tp => tp.Tag)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            var categoryIds = product.ProductCategories?.Select(c => c.CategoryId).ToList() ?? new List<int>();

            ViewBag.ReProduct = await _db.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductCategories)
                    .ThenInclude(tp => tp.Category)
                .Where(p => p.ProductCategories.Any(pc => categoryIds.Contains(pc.CategoryId))
                            && p.Id != id)
                .ToListAsync();

            return View(product);
        }
    }
}
