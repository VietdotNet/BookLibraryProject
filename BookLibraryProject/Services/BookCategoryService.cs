using BookLibraryProject.Models;
using BookLibraryProject.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryProject.Services
{
    public class BookCategoryService
    {
        private readonly IBookCategoryRepository _repo;
        private readonly BookLibraryManagementProjectContext _context;

        public BookCategoryService(IBookCategoryRepository repo, BookLibraryManagementProjectContext context)
        {
            _repo = repo;
            _context = context;
        }

        public List<BookCategory> GetCategories()
        {
            return _repo.GetCategories();
        }

        public List<BookCategory> GetChildCategories(int parentId)
        {
            var childCategories = _context.BookCategories
                                          .Where(c => c.ParentId == parentId)
                                          .OrderBy(c => c.Name)
                                          .ToList();

            return childCategories;
        }
    }
}
