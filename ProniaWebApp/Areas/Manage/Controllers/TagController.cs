
namespace ProniaWebApp.Areas.Manage.Controllers
{
    public class TagController : ManageBaseController
    {
        AppDbContext _db;

		public TagController(AppDbContext db)
		{
			_db = db;
		}

		public async Task<IActionResult> Index()
        {
            List<Tag> tags = await _db.Tags
                .Include(t => t.ProductTags)
                .ToListAsync();

            return View(tags);
        }


        public async Task<IActionResult> Create()
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTagVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            Tag tag = new Tag()
            {
                Name = vm.Name,
            };
            
            await _db.Tags.AddAsync(tag);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int? id)
        {
            if(id == null || !(_db.Tags.Any(t=> t.Id == id)))
            {
                return BadRequest();
            }

            Tag tag = await _db.Tags.FirstOrDefaultAsync(t => t.Id == id);
            UpdateTagVm vm = new UpdateTagVm()
            {
                Id = tag.Id,
                Name = tag.Name
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateTagVm vm)
        {
            if(vm == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            Tag oldTag = await _db.Tags.FirstOrDefaultAsync(t => t.Id == vm.Id);

            Tag tag = new Tag()
            {
                Name = vm.Name,
            };

            oldTag.Name = vm.Name;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null || !(_db.Tags.Any(t=> t.Id == id)))
            {
                return BadRequest();
            }

            Tag tag = await _db.Tags.FirstOrDefaultAsync(t => t.Id == id);

            _db.Tags.Remove(tag);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
