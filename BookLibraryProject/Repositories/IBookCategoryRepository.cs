using BookLibraryProject.Models;

namespace BookLibraryProject.Repositories
{
    public interface IBookCategoryRepository
    {
        List<BookCategory> GetCategories();


    }
}
