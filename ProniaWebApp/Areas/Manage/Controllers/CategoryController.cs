namespace ProniaWebApp.Areas.Manage.Controllers
{
    public class CategoryController : ManageBaseController
    {
        AppDbContext _db;

        public CategoryController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _db.Categories
                .Include(c=> c.ProductCategories)           
                .ToListAsync();
            return View(categories);

        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryVm vm)
        {
            if(vm == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }


            Category category = new Category()
            {
                Name = vm.Name
            };

            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || !(_db.Categories.Any(c=> c.Id == id)))
            {
                return BadRequest();
            }

            Category category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id);

            UpdateCategoryVm vm = new UpdateCategoryVm()
            {
                Id = category.Id,
                Name = category.Name
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCategoryVm vm)
        {
            if(vm == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            Category oldCategory = await _db.Categories.FirstOrDefaultAsync(c => c.Id == vm.Id);

            Category category = new Category()
            {
                Name = vm.Name
            };

            oldCategory.Name = vm.Name;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<ActionResult> Delete(int? id)
        {
            if(id == null || !(_db.Categories.Any(c=> c.Id == id)))
            {
                return BadRequest();
            }

            Category category = _db.Categories.FirstOrDefault(c => c.Id == id);

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
