using BookLibraryProject.Models;
using BookLibraryProject.Services;
using BookLibraryProject.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookLibraryProject.Controllers
{
    public class BookController : Controller
    {
        private readonly BookCategoryService _categoryService;
        private readonly BookService _bookService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookController(BookCategoryService categoryService, BookService bookService, IWebHostEnvironment webHostEnvironment)
        {
            _categoryService = categoryService;
            _bookService = bookService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Add()
        {
            var model = new AddBook
            {
                CreatedAt = DateTime.Now,
                AddedById = User.FindFirstValue(ClaimTypes.NameIdentifier),
            };
            ViewBag.RootCategories = _categoryService.GetCategories();

            return View(model);
        }

        [HttpGet]
        public JsonResult GetChildCategories(int parentId)
        {
            var categories = _categoryService.GetChildCategories(parentId);
            return Json(categories);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(AddBook book, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder); // ✅ Tạo thư mục nếu chưa có
                }

                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                book.Image = "/images/" + fileName;
            }
            else
            {
                ModelState.AddModelError("Image", "Vui lòng chọn ảnh."); // ✅ Báo lỗi nếu không có ảnh
            }

            if (!ModelState.IsValid)
            {
                ViewBag.RootCategories = _categoryService.GetCategories(); // ✅ Cập nhật danh mục
                return View(book);
            }

            await _bookService.AddBookAsync(book, file);
            // ✅ Thêm thông báo vào TempData
            TempData["SuccessMessage"] = "Thêm sách mới thành công!";

            return RedirectToAction("Add");
        }


    }




}
