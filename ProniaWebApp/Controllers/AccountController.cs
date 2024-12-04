
namespace ProniaWebApp.Areas.Manage.Controllers
{
    public class AccountController : Controller
    {
        AppDbContext _db;

        public AccountController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult LoginRegister()
        {
            AuthVm model = new AuthVm
            {
                Login = new LoginVm(),
                Register = new RegisterVm()
            };
            return View(model);
        }


        [HttpPost]
        public IActionResult Register(RegisterVm vm)
        {

            return View();
        }

    }
}
