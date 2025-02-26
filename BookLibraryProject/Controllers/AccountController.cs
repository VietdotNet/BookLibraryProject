using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using BookLibraryProject.Models;

namespace BookLibraryProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly BookLibraryManagementProjectContext _context;

        public AccountController(BookLibraryManagementProjectContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Account");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;

            // Lấy thông tin từ Google
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (email == null) return RedirectToAction("Index", "Home");

            // Kiểm tra người dùng có tồn tại trong database không
            var user = GetUserFromDatabase(email);

            if (user == null)
            {
                user = CreateUser(email, name);
            }

            // Tạo ClaimsIdentity và đăng nhập
            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, "User")
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private User GetUserFromDatabase(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        private User CreateUser(string email, string name)
        {
            var newUser = new User
            {
                FullName = name,
                Email = email
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();
            return newUser;
        }
    }
}
