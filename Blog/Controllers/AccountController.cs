using Blog.Models;
using Blog.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        //REGISTER
        public async Task<IActionResult> Register(string? returnUrl = null)
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("Moderator"));
            }
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem()
            {
                Value = "Admin",
                Text = "Admin"
            });
            listItems.Add(new SelectListItem()
            {
                Value = "Moderator",
                Text = "Moderator"
            });
            RegisterViewModel registerviewModel = new RegisterViewModel();
            registerviewModel.RoleList = listItems;
            registerviewModel.ReturnUrl = returnUrl;
            return View(registerviewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel, string? returnUrl = null)
        {
            registerViewModel.ReturnUrl = returnUrl; ;
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    Email = registerViewModel.Email,
                    UserName = registerViewModel.Username
                };
                var result = await _userManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded)
                {
                    if (registerViewModel.RoleSelected != null && registerViewModel.RoleSelected.Length > 0 && registerViewModel.RoleSelected == "Admin")
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "Moderator");
                    }
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                ModelState.AddModelError("Password", "User could not be created. Password not Unique enough.");
            }
            return View(registerViewModel);
        }

        //LOGIN
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            loginViewModel.ReturnUrl = returnUrl ?? Url.Content("~/");
            return View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName,
                    loginViewModel.Password,
                    loginViewModel.RememberMe,
                    lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                if (result.IsLockedOut)
                {
                    return View("LockOut");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt");
                    return View(loginViewModel);
                }
            }
            return View(loginViewModel);
        }

        //LOGOFF
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
