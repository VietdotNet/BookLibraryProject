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

        public async Task<List<BorrowRecord>> GetListRequestBorrowByStaff()
        {
            return await _repo.GetListRequestBorrowByStaff();
        }

        public async Task<BorrowRecord?> GetBorrowRecordById(Guid id)
        {
            return await _repo.GetBorrowRecordById(id);
        }

        public async Task Update(BorrowRecord record)
        {
           await _repo.Update(record);
        }

        public async Task<List<BorrowRecord>> GetListApprovedByStaffAsync()
        {
            return await _repo.GetListApprovedByStaffAsync();
        }

        public async Task<List<BorrowRecord>> GetListRejectedByStaffAsync()
        {
            return await _repo.GetListRejectedByStaffAsync();
        }
    }
}
