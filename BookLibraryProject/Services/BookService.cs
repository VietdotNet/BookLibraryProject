using BookLibraryProject.Models;
using BookLibraryProject.Repositories;
using BookLibraryProject.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryProject.Services
{
    public class BookService
    {
        private readonly IBookRepository _repo;

        public BookService(IBookRepository repo)
        {
            _repo = repo;
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
    }
}
