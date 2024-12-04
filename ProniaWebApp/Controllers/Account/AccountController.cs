using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ProniaWebApp.Controllers.Account
{
    public class AccountController : Controller
    {
        AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(AppDbContext db, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser user = new AppUser()
            {
                Name = vm.Name,
                Surname = vm.Surname,
                UserName = vm.Username,
                Email = vm.Email,
            };
            var existingUserByUsername = await _userManager.FindByNameAsync(vm.Username);

            if (existingUserByUsername != null)
            {
                ModelState.AddModelError("Username", "Username is already taken.");
            }

            var result = await _userManager.CreateAsync(user, vm.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            if (vm.LogIn)
            {
                await _signInManager.SignInAsync(user, true);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction(nameof(Login));
        }


        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser user = await _userManager.FindByEmailAsync(vm.EmailOrUsername) 
                ?? await _userManager.FindByNameAsync(vm.EmailOrUsername);

            if (user == null)
            {
                ModelState.AddModelError("", "Account not founded");
                return View();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, vm.Password, true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Try again later. You are banned");
                return View();
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Account not founded");
                return View();
            }

            await _signInManager.SignInAsync(user, vm.RememberMe);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }


}
