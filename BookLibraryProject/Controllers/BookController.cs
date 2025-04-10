﻿using BookLibraryProject.Models;
using BookLibraryProject.Services;
using BookLibraryProject.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        public async Task<IActionResult> DisplayBooks(int page = 1, string search = "", int? categoryId = null, string language = "")
        {
            int pageSize = 12;
            var books = await _bookService.GetBooksIsActiveAsync(page, pageSize, search, categoryId, language);
            ViewBag.RootCategories = _categoryService.GetCategories();
            return View(books);
        }


        [HttpGet]
        public async Task<IActionResult> DisplayBooksOfManager(int page = 1, string search = "", int? categoryId = null, string language = "")
        {
            int pageSize = 12;
            var books = await _bookService.GetBooksAsync(page, pageSize, search, categoryId, language);
            ViewBag.RootCategories = _categoryService.GetCategories();
            return View("DisplayBooks",books);
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

        public async Task<IActionResult> DetailBook(Guid id) 
        {
            var book = await _bookService.GetBookById(id);
            ViewBag.RootCategories = _categoryService.GetCategories();
            return View(book);
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


        [HttpPost]
        public async Task<IActionResult> UpdateBookAsync([FromBody] Book updatedBook)
        {
            if (updatedBook == null)
            {
                return BadRequest(new { message = "Dữ liệu sách không hợp lệ", error = "Request body is null" });
            }

            Console.WriteLine($"Received Book Data: {JsonConvert.SerializeObject(updatedBook)}"); // Debug log

            var existingBook = await _bookService.GetBookById(updatedBook.Id);
            if (existingBook == null)
            {
                return NotFound(new { message = "Không tìm thấy sách" });
            }

            // Update book properties
            existingBook.Title = updatedBook.Title;
            existingBook.Author = updatedBook.Author;
            existingBook.PublishedYear = updatedBook.PublishedYear;
            existingBook.Description = updatedBook.Description;
            existingBook.CategoryId = updatedBook.CategoryId;
            existingBook.Quantity = updatedBook.Quantity;
            existingBook.Price = updatedBook.Price;
            existingBook.Language = updatedBook.Language;

            await _bookService.UpdateBookAsync(existingBook);

            return Ok(new { message = "Cập nhật sách thành công!" });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatusAsync([FromBody] dynamic request)
        {
            if (request == null || request.id == null)
            {
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ!" });
            }

            Guid bookId = request.id;
            Console.WriteLine($"Received ID: {bookId}");

            var book = await _bookService.GetBookById(bookId);
            if (book == null)
            {
                return NotFound(new { success = false, message = "Không tìm thấy sách!" });
            }

            book.IsActive = !book.IsActive; // Đảo trạng thái
            await _bookService.UpdateBookAsync(book);

            return Json(new { success = true, status = book.IsActive });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleNewsStatusAsync(Guid id)
        {
            var book = await _bookService.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }

            // Đảo ngược trạng thái NewsStatus
            book.IsActive = !book.IsActive;

            await _bookService.UpdateBookAsync(book);
            ViewBag.RootCategories = _categoryService.GetCategories();

            return View("DetailBook", book);
        }



    }




}
