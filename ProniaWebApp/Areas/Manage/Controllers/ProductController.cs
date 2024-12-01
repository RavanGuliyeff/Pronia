using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaWebApp.Areas.Manage.ViewModels.Product;
using ProniaWebApp.DAL;
using ProniaWebApp.Models;

namespace ProniaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        AppDbContext _db;

		public ProductController(AppDbContext db)
		{
			_db = db;
		}

		public async Task<IActionResult> Index()
        {
            List<Product> products = await _db.Products
                .Include(p=> p.ProductCategories)
                .ThenInclude(pc=> pc.Category)
                .Include(p=> p.ProductTags)
                .ThenInclude(pt=> pt.Tag)
                .ToListAsync();
            return View(products);
        }


        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            ViewBag.Tags = await _db.Tags.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVm vm)
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            ViewBag.Tags = await _db.Tags.ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }

            Product product = new Product()
            {
                Name = vm.Name,
                Description = vm.Description,
                SKU = vm.SKU,
                Price = vm.Price,
            };

            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            ViewBag.Tags = await _db.Tags.ToListAsync();

            if (id == null || !(_db.Products.Any(p=> p.Id == id)))
            {
                return BadRequest();
            }

            Product? product = await _db.Products
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .Include(p => p.ProductTags)
                .ThenInclude(pt => pt.Tag)
                .FirstOrDefaultAsync(p=> p.Id == id);

            UpdateProductVm vm = new UpdateProductVm()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                SKU = product.SKU,
                Price = product.Price,
            };


            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductVm vm)
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            ViewBag.Tags = await _db.Tags.ToListAsync();

            if (vm == null || !(_db.Products.Any(p=> p.Id == vm.Id)))
            {
                return BadRequest();
            }

            Product product = new Product()
            {
                Id = vm.Id,
                Name = vm.Name,
                Description = vm.Description,
                SKU = vm.SKU,
                Price = vm.Price
            };

            Product oldProduct = await _db.Products.FirstOrDefaultAsync(p => p.Id == vm.Id);

            oldProduct.Name = product.Name;
            oldProduct.Description = product.Description;
            oldProduct.Price = product.Price;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null || !(_db.Products.Any(p=> p.Id == id)))
            {
                return BadRequest();
            }

            Product? product = await _db.Products.FirstOrDefaultAsync(p=> p.Id == id);
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
