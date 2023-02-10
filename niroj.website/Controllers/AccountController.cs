using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using niroj.website.ViewModels;
using Personal.Domain.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace niroj.website.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null &&
                    await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                    identity.AddClaim(new Claim("user_id", user.Id.ToString()));

                    await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme,
                        new ClaimsPrincipal(identity));
                    return Redirect("/admin/home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid UserName or Password");
                    return View();
                }
            }
            catch (System.Exception)
            {
                ModelState.AddModelError("", "Failed to login");
                return View();
            }           
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> logout()
        {
            await HttpContext.SignOutAsync();

            return Redirect("/account/login");
        }
    }
}
