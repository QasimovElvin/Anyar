using Elvi.Az.Models;
using Elvi.Az.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Elvi.Az.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();
            ApplicationUser user=new ApplicationUser()
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                UserName=registerVM.Username,
                Email = registerVM.Email,
            };
            var result=await _userManager.CreateAsync(user,registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                    return View();
                }
            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View();
            if (loginVM.Password == null)
            {
                ModelState.AddModelError("", "User is null");
                return View();
            }
            if (loginVM.EmailOrUsername.Contains("@"))
            {
                var user = await _userManager.FindByEmailAsync(loginVM.EmailOrUsername);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user,loginVM.Password,false,false);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Password is Wrong");
                        return View();
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                var user = await _userManager.FindByEmailAsync(loginVM.EmailOrUsername);
                if (user != null)
                {
                    var result = await _userManager.CreateAsync(user, loginVM.Password);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Password is Wrong");
                        return View();
                    }
                    return RedirectToAction("Index", "Home");
                }
            }return View();
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
