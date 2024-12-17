using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using ProniaWebApp.ViewModels.Basket;

namespace ProniaWebApp.Controllers
{
    public class CartController : Controller
    {
        AppDbContext _db;

        public CartController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var json = Request.Cookies["Basket"];
            List<CookieitemVm> cookieList = new List<CookieitemVm>();

            if (json != null)
            {
            cookieList = JsonConvert.DeserializeObject<List<CookieitemVm>>(json);
            }

            List<CartVm> cartList = new List<CartVm>();
            List<CookieitemVm> deletedCookieItem = new List<CookieitemVm>();

            if(cookieList.Count > 0)
            {
                foreach (var cookie in cookieList)
                {
                    var product = await _db.Products
                        .Include(p=> p.ProductImages)
                        .FirstOrDefaultAsync(p=> p.Id == cookie.Id);

                    if (product == null)
                    {
                        deletedCookieItem.Add(cookie);
                        continue;
                    }

                    cartList.Add(new CartVm()
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        ImgUrl = product.ProductImages.FirstOrDefault(pi=> pi.Primary).ImgUrl,
                        Count = cookie.Count
                    });

                }
                if(deletedCookieItem.Count > 0)
                {
                    deletedCookieItem.ForEach(d =>
                    {
                        cookieList.Remove(d);
                    });
                    Response.Cookies.Append("Basket", JsonConvert.SerializeObject(cookieList));
                }
            }

            return View(cartList);
        }

        public async Task<IActionResult> AddBasket(int id)
        {
            Product product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);

            if(product == null) return NotFound();


            List<CookieitemVm> cookiesList;

            var basket = Request.Cookies["Basket"];

            if(basket != null)
            {
                cookiesList = JsonConvert.DeserializeObject<List<CookieitemVm>>(basket);
                var existproduct = cookiesList.FirstOrDefault(c=> c.Id == id);


                if (existproduct != null)
                {
                    existproduct.Count += 1;
                }
                else
                {
                    cookiesList.Add(new CookieitemVm() { Id = id});
                }
            }
            else
            {
                cookiesList = new List<CookieitemVm>();
                cookiesList.Add(new CookieitemVm() { Id = id });
            }

            Response.Cookies.Append("Basket", JsonConvert.SerializeObject(cookiesList));



            return RedirectToAction("Index", "Home");
        }


    }
}
