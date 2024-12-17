
namespace ProniaWebApp.Controllers.Account
{
    public class AccountController : Controller
    {
        AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IMailService _mailService;

		public AccountController(AppDbContext db
            , UserManager<AppUser> userManager
            , SignInManager<AppUser> signInManager
            , RoleManager<IdentityRole> roleManager
            , IMailService mailService)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
			_mailService = mailService;
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

            await _userManager.AddToRoleAsync(user, UserRoles.Member.ToString());


            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);


			object userStat = new
			{
				userid = user.Id,
				token = token
			};


			var link = Url.Action("ConfirmEmail", "Account", userStat, HttpContext.Request.Scheme);


            string confirmKey = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();

            user.ConfirmationKey = confirmKey;

			await _userManager.UpdateAsync(user);

			MailRequest mailRequest = new MailRequest()
			{
				Subject = "Confirm Email",
				Body = $"<h2>Your security key is {confirmKey}</h2><a href='{link}'>Click for confirm your email.</a>",
				ToEmail = vm.Email
			};

			await _mailService.SendEmailAsync(mailRequest);

			return RedirectToAction(nameof(Login));
        }


        public IActionResult ConfirmEmail()
        {
            return View();
        }

		[HttpPost]
		public async Task<IActionResult> ConfirmEmail(string userId,string token, ConfirmEmailVm vm)
		{
			var user = await _userManager.FindByIdAsync(userId);

            string key = vm.Key;


			if (user == null || user.ConfirmationKey != key)
			{
				ModelState.AddModelError("Key", "Invalid confirmation key.");
				return View(vm);
			}

			user.EmailConfirmed = true;
			user.ConfirmationKey = null;
			await _userManager.UpdateAsync(user);

			return RedirectToAction("Login");
		}



		public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginVm vm, string? ReturnUrl)
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


            if(ReturnUrl != null)
            {
                return Redirect(ReturnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }



        public async Task<IActionResult> CreateRoles()
        {
            foreach(var role in Enum.GetValues(typeof(UserRoles)))
            {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = role.ToString()
                });
            }

            return RedirectToAction(nameof(Index), "Home");
        }



        public async Task<IActionResult> ForgetPassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            AppUser user = await _userManager.FindByEmailAsync(vm.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "User not founded");
                return View();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);


            object userStat = new
            {
                userid = user.Id,
                token = token
            };

            var link = Url.Action("ResetPassword", "Account", userStat, HttpContext.Request.Scheme);

            MailRequest mailRequest = new MailRequest()
            {
                Subject = "Reset Password",
                Body = $"<a href='{link}'>Click for reset your password.</a>",
                ToEmail = vm.Email
            };

            await _mailService.SendEmailAsync(mailRequest);

            return RedirectToAction(nameof(Login));
        }


        public async Task<IActionResult> ResetPassword(string userId, string token)
        {
            if(userId == null)
            {
                return BadRequest();
            }

			ResetPasswordVm resetPasswordVm = new ResetPasswordVm()
			{
				UserId = userId,
                Token = token
			};

			return View(resetPasswordVm);
        }

        [HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordVm vm)
		{
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = await _userManager.FindByIdAsync(vm.UserId);

            if(user == null)
            {
                return BadRequest();
            }


            var result = await _userManager.ResetPasswordAsync(user, vm.Token, vm.Password);

            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(vm);
            }

			return RedirectToAction(nameof(Login));
		}

	}
}
