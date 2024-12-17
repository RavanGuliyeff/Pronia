using Azure;
using Newtonsoft.Json;
using ProniaWebApp.ViewModels.Basket;

namespace ProniaWebApp.ViewComponents
{
    public class BasketViewComponent:ViewComponent
    {
        AppDbContext _db;

        public BasketViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var json = Request.Cookies["Basket"];
            List<CookieitemVm> cookieList = new List<CookieitemVm>();

            if (json != null)
            {
                cookieList = JsonConvert.DeserializeObject<List<CookieitemVm>>(json);
            }

            List<CartVm> cartList = new List<CartVm>();
            List<CookieitemVm> deletedCookieItem = new List<CookieitemVm>();

            if (cookieList.Count > 0)
            {
                foreach (var cookie in cookieList)
                {
                    var product = await _db.Products
                        .Include(p => p.ProductImages)
                        .FirstOrDefaultAsync(p => p.Id == cookie.Id);

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
                        ImgUrl = product.ProductImages.FirstOrDefault(pi => pi.Primary).ImgUrl,
                        Count = cookie.Count
                    });

                }
                if (deletedCookieItem.Count > 0)
                {
                    deletedCookieItem.ForEach(d =>
                    {
                        cookieList.Remove(d);
                    });
                    HttpContext.Response.Cookies.Append("Basket", JsonConvert.SerializeObject(cookieList));
                }
            }

            return View(cartList);
        }
    }
}
