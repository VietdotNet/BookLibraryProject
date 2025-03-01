using BookLibraryProject.Models;

namespace BookLibraryProject.Repositories
{
    public interface IUserRepository
    {
        User? GetUserFromDB(string email);

        void InsertUser(string email, string name);
    }
}
