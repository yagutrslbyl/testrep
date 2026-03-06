using Eat.Models;
using Eat.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eat.Controllers
{
    public class AccountController : Controller
    {
        UserManager<AppUser> _userManager;
        RoleManager<IdentityRole> _roleManager;
        SignInManager<AppUser> _singInManager;

        public AccountController(UserManager<AppUser> user, RoleManager<IdentityRole> role, SignInManager<AppUser> sing)
        {
            _userManager = user;
            _roleManager = role;
            _singInManager = sing;
              
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            AppUser user = new()
            {
                Name = vm.Name,
                Surname = vm.Surname,
                UserName = vm.UserName,
                Email = vm.Email

            };

            IdentityResult result = await _userManager.CreateAsync(user, vm.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(vm);
            }

            await _userManager.AddToRoleAsync(user, "user");
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            AppUser user = await _userManager.FindByNameAsync(vm.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "Username is wrong");
                return View(vm);
            }

            var result = await _singInManager.PasswordSignInAsync(user, vm.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Password or username is wrong");
                return View(vm);

            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("Index", "Home");

            }


        }

        //public async Task<IActionResult> SeedRoles()
        //{
        //    var roles = Enum.GetNames(typeof(Roles));
        //    foreach(var role in roles)
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole(role));

        //    }
        //    return Content("Rollar yaradildi");

        //}

        public async Task<IActionResult> Logout()
        {
            await _singInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
