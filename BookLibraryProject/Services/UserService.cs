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
    }
}
