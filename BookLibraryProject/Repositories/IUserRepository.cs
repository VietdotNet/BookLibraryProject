using BookLibraryProject.Models;

namespace BookLibraryProject.Repositories
{
    public interface IUserRepository
    {
        User? GetUserFromDB(string email);

        User? GetUserById(string Id);

        void InsertUser(string email, string name);

        Task<List<User>> GetUserisStaffOrManager();

        void CreateUser(string name, string email, int roldId);

        void Delete(User user);
    }
}
