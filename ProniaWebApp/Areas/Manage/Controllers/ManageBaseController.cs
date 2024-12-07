
namespace ProniaWebApp.Areas.Manage.Controllers
{
    [Area(nameof(Manage))]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public abstract class ManageBaseController : Controller
    {
    }
}
