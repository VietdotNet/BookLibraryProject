using BookLibraryProject.Models;
using BookLibraryProject.Repositories;
using BookLibraryProject.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;
using X.PagedList.Mvc.Core;

namespace BookLibraryProject.Services
{
    public class BookService
    {
        private readonly IBookRepository _repo;
        private readonly BookLibraryManagementProjectContext _context;

        public BookService(IBookRepository repo, BookLibraryManagementProjectContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task AddBookAsync(AddBook book, IFormFile file)
        {
            var newBook = new Book
            {
                Title = book.Title,
                Author = book.Author,
                CreatedAt = book.CreatedAt,
                PublishedYear = book.PublishedYear,
                Description = book.Description,
                CategoryId = book.CategoryId,
                Quantity = book.Quantity,
                Price = book.Price,
                AddedById = book.AddedById,
                Image = book.Image, // ✅ Sử dụng ảnh đã lưu từ Controller
                Language = book.Language
            };

            await _repo.AddBookAsync(newBook);
        }

        //public IPagedList<Book> GetListBooks(int? page)
        //{
        //    int pageSize = 12; // Số sách mỗi trang
        //    int pageNumber = page ?? 1;

        //    var books = _context.Books
        //                        .Include(b => b.Category) // Load danh mục sách
        //                        .OrderByDescending(b => b.CreatedAt)
        //                        .ToPagedList(pageNumber, pageSize); // Phân trang

        //    return books; // 
        //}

        public async Task<IPagedList<Book>> GetBooksAsync(int page, int pageSize, string search, int? categoryId, string language)
        {
            var query = _context.Books.Include(b => b.Category).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b => b.Title.Contains(search) || b.Author.Contains(search));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(b => b.Category.Id == categoryId.Value);
            }

            if (!string.IsNullOrEmpty(language))
            {
                query = query.Where(b => b.Language == language);
            }

            return query.OrderBy(b => b.Title).ToPagedList(page, pageSize);

        }

        public async Task<Book?> GetBookById(Guid id)
        {
            return await _repo.GetBookById(id);
        }
    }
}
