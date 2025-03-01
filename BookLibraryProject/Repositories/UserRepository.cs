using BookLibraryProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryProject.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BookLibraryManagementProjectContext _context;

        public UserRepository(BookLibraryManagementProjectContext context)
        {
            _context = context;
        }

        public User? GetUserFromDB(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public void InsertUser(string email, string name)
        {
             _context.Database.ExecuteSqlRaw("INSERT INTO Users (FullName, Email, RoleId) VALUES ({1}, {2}, {3}, {4})", name, email, 2);
        }
    }
}
