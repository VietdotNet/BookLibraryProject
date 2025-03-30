using BookLibraryProject.Models;
using BookLibraryProject.Services;
using BookLibraryProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace BookLibraryProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserService _userService;
        private readonly BookLibraryManagementProjectContext _context;

        public AdminController(UserService userService, BookLibraryManagementProjectContext context)
        {
            _userService = userService;
            _context = context;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var list = await _userService.GetUserisStaffOrManager();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Statistics()
        {
            var totalUsers = _context.Users.Where(x => x.RoleId == 2).Count();
            var totalBooks = _context.Books.Sum(b => b.Quantity ?? 0);
            var totalFineAmount = _context.BorrowRecords.Sum(br => br.FineAmount ?? 0);
            var totalBookCategories = _context.Books.Select(b => b.CategoryId).Distinct().Count();


            var model = new StatisticsViewModel
            {
                TotalUsers = totalUsers,
                TotalBooks = totalBooks,
                TotalFineAmount = totalFineAmount,
                TotalBookCategories = totalBookCategories
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(User user)
        {
            Console.WriteLine($"ID: {user.Id}, FullName: {user.FullName}, Email: {user.Email}, RoleId: {user.RoleId}"); // Debug

            _userService.CreateUser(user.FullName, user.Email, user.RoleId);

            TempData["SuccessMessage"] = "Tạo người dùng thành công!";
            return RedirectToAction(nameof(Create)); // Quay lại trang tạo
        }

        [HttpGet]
        public IActionResult Delete(string Id)
        {
            var user = _userService.GetUserById(Id);
            if (user == null)
            {
                return NotFound();
            }
            _userService.Delete(user);
            TempData["SuccessMessage"] = "Xóa người dùng thành công!";
            return RedirectToAction("Index");
        }

    }
}
