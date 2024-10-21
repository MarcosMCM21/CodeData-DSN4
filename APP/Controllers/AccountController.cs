using CodeData_Connection.Areas.Identity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CodeData_Connection.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult LockScreen(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            // Salvar na sessão que o usuário está bloqueado
            HttpContext.Session.SetString("IsLocked", "true");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UnlockScreen(LockScreenViewModel model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return View("LockScreen", model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("Idenity/Account/Login");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                // Salvar na sessão que o usuário está bloqueado
                HttpContext.Session.SetString("IsLocked", "false");

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return LocalRedirect(returnUrl); // Redireciona para a URL original
                } 
                else
                {
                    return RedirectToAction("Index", "Home"); // Caso não haja returnUrl, redireciona para a home
                }
            }

            return View("LockScreen", model);
        }
    }

    public class LockScreenViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
