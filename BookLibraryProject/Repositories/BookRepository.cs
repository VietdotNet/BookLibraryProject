using BookLibraryProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryProject.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookLibraryManagementProjectContext _context;

        public BookRepository(BookLibraryManagementProjectContext context)
        {
            _context = context;
        }
        public async Task AddBookAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public void DeleteBook(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateBookAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task<Book?> GetBookById(Guid id)
        {
            return await _context.Books
                                 .Include(x => x.Category)
                                 .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
