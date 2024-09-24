using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAttendace.Config;

namespace StudentAttendace.Controllers;

public class LoginController(ILogger<LoginController> logger, ApplicationDbContext context) : Controller
{
    private readonly ILogger<LoginController> _logger = logger;

    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ModelState.AddModelError(string.Empty, "Invalid Credentials");
            return View();
        }

        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        if (user != null)
        {
            var claims = new List<Claim>
            {
                new Claim("Email", user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                RedirectUri = Url.Action("Index", "Home")
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction("Index", "Home");

        }
        
        ModelState.AddModelError(string.Empty, "Invalid Credentials");
        return View();
    }
    
}