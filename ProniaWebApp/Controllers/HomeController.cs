using Microsoft.AspNetCore.Mvc;

namespace ProniaWebApp.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
