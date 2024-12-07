
using ProniaWebApp.Models;

namespace ProniaWebApp.Areas.Manage.Controllers
{
	public class ProductController : ManageBaseController
    {
		AppDbContext _db;
		private readonly IWebHostEnvironment _env;

		public ProductController(AppDbContext db, IWebHostEnvironment env)
		{
			_db = db;
			_env = env;
		}

		public async Task<IActionResult> Index()
		{
			List<Product> products = await _db.Products
				.Include(p => p.ProductCategories)
				.ThenInclude(pc => pc.Category)
				.Include(p => p.ProductTags)
				.ThenInclude(pt => pt.Tag)
				.Include(p => p.ProductImages)
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

			if (vm.CategoryIds != null)
			{
				List<ProductCategory> productCategories = new List<ProductCategory>();
				foreach (int id in vm.CategoryIds)
				{
					if (!(await _db.Categories.AnyAsync(c => c.Id == id)))
					{
						ModelState.AddModelError("CategoryIds", $"Category with id '{id}' does not exist");
						return View(vm);
					}

					ProductCategory productCategory = new ProductCategory()
					{
						CategoryId = id,
						Product = product
					};
					productCategories.Add(productCategory);
				}

				await _db.ProductCategories.AddRangeAsync(productCategories);

			}

			if (vm.TagIds != null)
			{
				List<ProductTag> productTags = new List<ProductTag>();
				foreach (int id in vm.TagIds)
				{
					if (!(await _db.Tags.AnyAsync(t => t.Id == id)))
					{
						ModelState.AddModelError("TagIds", $"Tag with id '{id}' does not exist");
						return View(vm);
					}

					ProductTag productTag = new ProductTag()
					{
						TagId = id,
						Product = product
					};
					productTags.Add(productTag);
				}
				await _db.ProductTags.AddRangeAsync(productTags);
			}

			if (!vm.MainPhoto.ContentType.Contains("image"))
			{
				ModelState.AddModelError("Image", "Wrong type");
				return View();
			}

			if (vm.MainPhoto.Length > 2097152)
			{
				ModelState.AddModelError("Image", "Image size must be under of 2gb");
				return View();
			}

			ProductImage productImage = (new()
			{
				ImgUrl = vm.MainPhoto.Upload(_env.WebRootPath, "Upload/Product"),
				Primary = true,
				Product = product
			});

			await _db.ProductImages.AddAsync(productImage);


			if (vm.Images != null)
			{
				List<ProductImage> productImages = new List<ProductImage>();
				foreach (IFormFile img in vm.Images)
				{
					if (!vm.MainPhoto.ContentType.Contains("image"))
					{
						ModelState.AddModelError("Image", "Wrong type");
						return View();
					}

					if (vm.MainPhoto.Length > 2097152)
					{
						ModelState.AddModelError("Image", "Image size must be under of 2gb");
						return View();
					}

					productImage = (new()
					{
						ImgUrl = img.Upload(_env.WebRootPath, "Upload/Product"),
						Primary = false,
						Product = product
					});
					productImages.Add(productImage);
				}
				await _db.ProductImages.AddRangeAsync(productImages);
			}

			await _db.Products.AddAsync(product);
			await _db.SaveChangesAsync();


			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Update(int? id)
		{
			ViewBag.Categories = await _db.Categories.ToListAsync();
			ViewBag.Tags = await _db.Tags.ToListAsync();

			if (id == null || !(_db.Products.Any(p => p.Id == id)))
			{
				return BadRequest();
			}

			Product? product = await _db.Products
				.Include(p => p.ProductImages)
				.Include(p => p.ProductCategories)
				.ThenInclude(pc => pc.Category)
				.Include(p => p.ProductTags)
				.ThenInclude(pt => pt.Tag)
				.FirstOrDefaultAsync(p => p.Id == id);

			UpdateProductVm vm = new UpdateProductVm()
			{
				Id = product.Id,
				Name = product.Name,
				Description = product.Description,
				SKU = product.SKU,
				Price = product.Price,
				CategoryIds = new List<int>(),
				TagIds = new List<int>(),
				ProductImagesVms = new List<ProductImagesVm>()
			};

			foreach (ProductCategory pc in product.ProductCategories)
			{
				vm.CategoryIds.Add(pc.CategoryId);
			}

			foreach (ProductTag pt in product.ProductTags)
			{
				vm.TagIds.Add(pt.TagId);
			}

			if(product.ProductImages != null)
			{
				foreach (ProductImage pi in product.ProductImages)
				{
					vm.ProductImagesVms.Add(new()
					{
						ImgUrl = pi.ImgUrl,
						Primary = pi.Primary
					});
				}
			}


			return View(vm);
		}

		[HttpPost]
		public async Task<IActionResult> Update(UpdateProductVm vm)
		{
			ViewBag.Categories = await _db.Categories.ToListAsync();
			ViewBag.Tags = await _db.Tags.ToListAsync();

			if (vm == null || !(_db.Products.Any(p => p.Id == vm.Id)))
			{
				return BadRequest();
			}

			if (!ModelState.IsValid)
			{
				return View(vm);
			}


			Product oldProduct = await _db.Products
				.Include(p => p.ProductCategories)
				.Include(p => p.ProductTags)
				.Include(p => p.ProductImages)
				.FirstOrDefaultAsync(p => p.Id == vm.Id);



			if (vm.CategoryIds != null)
			{
				_db.ProductCategories.RemoveRange(oldProduct.ProductCategories);
				List<ProductCategory> productCategories = new List<ProductCategory>();
				foreach (int id in vm.CategoryIds)
				{
					if (!(await _db.Categories.AnyAsync(c => c.Id == id)))
					{
						ModelState.AddModelError("CategoryIds", $"Category with id '{id}' does not exist");
						return View(vm);
					}

					ProductCategory productCategory = new ProductCategory()
					{
						CategoryId = id,
						Product = oldProduct
					};
					productCategories.Add(productCategory);
				}

				await _db.ProductCategories.AddRangeAsync(productCategories);

			}

			if (vm.TagIds != null)
			{
				_db.ProductTags.RemoveRange(oldProduct.ProductTags);
				List<ProductTag> productTags = new List<ProductTag>();
				foreach (int id in vm.TagIds)
				{
					if (!(await _db.Tags.AnyAsync(t => t.Id == id)))
					{
						ModelState.AddModelError("TagIds", $"Tag with id '{id}' does not exist");
						return View(vm);
					}

					ProductTag productTag = new ProductTag()
					{
						TagId = id,
						Product = oldProduct
					};
					productTags.Add(productTag);
				}
				await _db.ProductTags.AddRangeAsync(productTags);
			}


			if (vm.MainPhoto != null)
			{

				if (!vm.MainPhoto.ContentType.Contains("image"))
				{
					ModelState.AddModelError("Image", "Wrong type");
					return View();
				}

				if (vm.MainPhoto.Length > 2097152)
				{
					ModelState.AddModelError("Image", "Image size must be under of 2gb");
					return View();
				}
				ProductImage oldImage = oldProduct.ProductImages.FirstOrDefault(p => p.Primary);


                FileExtensions.Delete(_env.WebRootPath, "Upload/Product", oldImage.ImgUrl);
				_db.ProductImages.Remove(oldImage);

				ProductImage productImage = new ProductImage()
				{
					Primary = true,
					ImgUrl = vm.MainPhoto.Upload(_env.WebRootPath, "Upload/Product"),
					ProductId = oldProduct.Id,
					Product = oldProduct
				};
				await _db.ProductImages.AddAsync(productImage);

			}

			if(vm.ImageUrls != null)
			{
				
				foreach (ProductImage productImage in oldProduct.ProductImages.Where(pi=> !pi.Primary))
				{

                    if (!vm.ImageUrls.Any(iu=> iu == productImage.ImgUrl))
					{
                        FileExtensions.Delete(_env.WebRootPath, "Upload/Product", productImage.ImgUrl);
                        _db.ProductImages.Remove(productImage);
					}
				}


			}
			else
			{
				foreach (ProductImage productImage in oldProduct.ProductImages.Where(pi => !pi.Primary))
				{
                    FileExtensions.Delete(_env.WebRootPath, "Upload/Product", productImage.ImgUrl);
					_db.ProductImages.Remove(productImage);
                }
            }
			oldProduct.Name = vm.Name;
			oldProduct.Description = vm.Description;
			oldProduct.Price = vm.Price;

			await _db.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || !(_db.Products.Any(p => p.Id == id)))
			{
				return BadRequest();
			}

			Product? product = await _db.Products
				.Include(p=> p.ProductImages)
				.FirstOrDefaultAsync(p => p.Id == id);

			List<ProductImage> productImages = product.ProductImages;
			_db.ProductImages.RemoveRange(productImages);
			_db.Products.Remove(product);
			await _db.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}
	}
}
