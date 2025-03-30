using BookLibraryProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

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

        public async Task<List<User>> GetUserisStaffOrManager()
        {
            return await _context.Users
                .Where(u => u.Id.StartsWith("QL") || u.Id.StartsWith("NV"))
                .Include(x => x.Role)
                .ToListAsync();
        }

        public void InsertUser(string email, string name)
        {
             _context.Database.ExecuteSqlRaw("INSERT INTO Users (FullName, Email, RoleId) VALUES ({0}, {1}, {2})", name, email, 2);
        }

        public void CreateUser(string name, string email, int roldId)
        {
            _context.Database.ExecuteSqlRaw("INSERT INTO Users (FullName, Email, RoleId) VALUES ({0}, {1}, {2})", name, email, roldId);
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public User? GetUserById(string Id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == Id);
        }
    }
}
