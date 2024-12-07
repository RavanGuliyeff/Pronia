
namespace ProniaWebApp.Areas.Manage.Controllers
{
    public class SliderController : ManageBaseController
    {
        AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Slider> sliderList = await _db.Sliders.ToListAsync();

            return View(sliderList);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSliderVm vm)
        {
            if (vm == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (!vm.Image.ContentType.Contains("image"))
            {
                ModelState.AddModelError("Image", "Wrong type");
                return View();
            }

            if (vm.Image.Length > 2097152)
            {
                ModelState.AddModelError("Image", "Image size must be under of 2gb");
                return View();
            }


            Slider slider = (new()
            {
                UpTitle = vm.UpTitle,
                DownTitle = vm.DownTitle,
                Description = vm.Description,
                ImgUrl = vm.Image.Upload(_env.WebRootPath, "Upload/Slider")
            });

            await _db.Sliders.AddAsync(slider);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Slider slider = await _db.Sliders.FirstOrDefaultAsync(s => s.Id == id);

            if(slider == null)
            {
                return NotFound();
            }

            UpdateSliderVm vm = (new()
            {
                UpTitle = slider.UpTitle,
                DownTitle = slider.DownTitle,
                Description = slider.Description,
                ImgUrl = slider.ImgUrl
            });

            if (vm == null)
            {
                return BadRequest();
            }

            return View(vm);

        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateSliderVm vm)
        {
            if (vm == null)
            {
                return BadRequest();
            }
            if(!ModelState.IsValid)
            {
                return View(vm);
            }

            Slider oldSlider = await _db.Sliders.FirstOrDefaultAsync(s => s.Id == vm.Id);

            oldSlider.UpTitle = vm.UpTitle;
            oldSlider.DownTitle = vm.DownTitle;
            oldSlider.Description = vm.Description;

            if (vm.Image != null)
            {
				if (!vm.Image.ContentType.Contains("image"))
				{
					ModelState.AddModelError("Image", "Wrong type");
					return View();
				}

				if (vm.Image.Length > 2097152)
				{
					ModelState.AddModelError("Image", "Image size must be under of 2gb");
					return View();
				}
				if (!string.IsNullOrEmpty(oldSlider.ImgUrl))
				{
					FileExtensions.Delete(_env.WebRootPath, "Upload/Slider", oldSlider.ImgUrl);
				}
				oldSlider.ImgUrl = vm.Image.Upload(_env.WebRootPath, "Upload/Slider");
			}

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

			Slider slider = await _db.Sliders.FirstOrDefaultAsync(s => s.Id == id);

			if (slider == null)
			{
				return NotFound();
			}
            FileExtensions.Delete(_env.WebRootPath, "Upload/Slider", slider.ImgUrl);
            _db.Sliders.Remove(slider);
            await _db.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
        }
    }


}
