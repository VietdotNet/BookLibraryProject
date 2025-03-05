using BookLibraryProject.Models;

namespace BookLibraryProject.Repositories
{
    public class BookCategoryRepository : IBookCategoryRepository
    {
        private readonly BookLibraryManagementProjectContext _context;

        public BookCategoryRepository(BookLibraryManagementProjectContext context)
        {
            _context = context;
        }

        public List<BookCategory> GetCategories()
        {
            return _context.BookCategories
                           .Where(c => c.ParentId == null) // Lấy danh mục không có Parent
                           .OrderBy(c => c.Name)
                           .ToList();
        }

    }
}
