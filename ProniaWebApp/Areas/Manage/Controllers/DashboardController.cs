using Microsoft.AspNetCore.Authorization;

namespace ProniaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    public class DashboardController : ManageBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
