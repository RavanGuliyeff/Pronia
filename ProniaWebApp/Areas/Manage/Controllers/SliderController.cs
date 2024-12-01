using Microsoft.AspNetCore.Mvc;

namespace ProniaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
