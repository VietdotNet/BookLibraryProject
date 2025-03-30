using BookLibraryProject.Models;
using BookLibraryProject.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryProject.Services
{
    public class UserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public User? GetUserFromDB(string email)
        {
            return _repo.GetUserFromDB(email);
        }

        public void InsertUser(string email, string name)
        {
           _repo.InsertUser(email, name);
        }

        public async Task<List<User>> GetUserisStaffOrManager()
        {
            return await _repo.GetUserisStaffOrManager();
        }

        public void CreateUser(string name, string email, int roldId)
        {
            _repo.CreateUser(name, email, roldId);
        }

        public void Delete(User user)
        {
            _repo.Delete(user);
        }

        public User? GetUserById(string Id)
        {
            return _repo.GetUserById(Id);
        }
    }
}
