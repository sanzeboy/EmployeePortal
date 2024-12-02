using EmployeePortal.Application.Services.AppUsers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeePortal.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAppUserService _appUserService;

        public AccountController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        public IActionResult Login()
        {
            return View();
        }
        // POST: Login
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var loginResponse = await _appUserService.Login(new Application.Services.AppUsers.Models.Dtos.LoginDto
            {
                Username = username,
                Password = password,
            });

            if (loginResponse.IsSuccess)
            {
                // Create the user's claims
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, loginResponse.AppUserId?.ToString()),
                new Claim(ClaimTypes.Role, "Admin")
            };

                // Create the identity
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Create the principal
                var principal = new ClaimsPrincipal(identity);

                // Sign in the user
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        // GET: Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
