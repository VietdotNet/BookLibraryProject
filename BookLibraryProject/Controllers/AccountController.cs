using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using BookLibraryProject.Models;
using Microsoft.EntityFrameworkCore;
using BookLibraryProject.Services;
using BookLibraryProject.ViewModel;
using System.Text.RegularExpressions;

namespace BookLibraryProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;

        public AccountController(UserService userService)
        {
            _userService = userService;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult SignIn()
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
                return RedirectToAction("Login", "Account");
            }

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;

            // Lấy thông tin từ Google
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (email == null) return RedirectToAction("Login", "Account");

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
                new Claim(ClaimTypes.NameIdentifier, user.Id),
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

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private User? GetUserFromDatabase(string email)
        {
            return _userService.GetUserFromDB(email);
        }

        public User CreateUser(string email, string name)
        {
            // Chèn user bằng lệnh SQL để kích hoạt trigger
            _userService.InsertUser(email, name);
            // Truy vấn lại user vừa tạo
            var newUser = _userService.GetUserFromDB(email);

            if (newUser == null)
                throw new Exception("User creation failed!");

            return newUser;
        }

        [HttpGet]
        public IActionResult ShowProfile()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Index", "Home");
            }

            var getUser = _userService.GetUserFromDB(email);
            if (getUser == null)
            {
                return RedirectToAction("Index", "Home"); // Hoặc trang lỗi
            }

            var user = new UserProfile
            {
                Email = email,
                CreatedAt = getUser.CreatedAt,
                PhoneNumber = getUser.PhoneNumber,
                FullName = getUser.FullName,
                StudentCode = ExtractStudentCode(email)
            };

            return View("EditProfile", user);
        }

        public static string ExtractStudentCode(string email)
        {
            // Tìm chuỗi bắt đầu bằng "he" + số (không phân biệt hoa thường)
            Match match = Regex.Match(email, @"he\d+", RegexOptions.IgnoreCase);
            return match.Success ? match.Value.ToUpper() : "Không xác định"; // Giá trị mặc định nếu không tìm thấy
        }


    }
}
