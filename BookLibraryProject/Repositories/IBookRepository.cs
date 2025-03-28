using BookLibraryProject.Models;
using BookLibraryProject.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BookLibraryProject.Repositories
{
    public interface IBookRepository
    {
        Task AddBookAsync(Book book);

        Task UpdateBookAsync(Book book);

        void DeleteBook(int id);

        Task<Book?> GetBookById(Guid id);
    }
}
