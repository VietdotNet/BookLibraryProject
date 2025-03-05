using BookLibraryProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookLibraryProject.Repositories
{
    public interface IBookRepository
    {
        Task AddBookAsync(Book book);

        void EditBook(Book book);

        void DeleteBook(int id);
    }
}
