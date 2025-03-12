using BookLibraryProject.Models;
using BookLibraryProject.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryProject.Services
{
    public class BorrowRecordService
    {
        private readonly IBorrowRecordRepository _repo;

        public BorrowRecordService(IBorrowRecordRepository repo)
        {
            _repo = repo;
        }

        public async Task CreateRequestBorrow(BorrowRecord record)
        {
            await _repo.CreateRequestBorrow(record);
        }
    }
}
