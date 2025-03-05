using BookLibraryProject.Models;

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

        public void EditBook(Book book)
        {
            throw new NotImplementedException();
        }
    }
}
