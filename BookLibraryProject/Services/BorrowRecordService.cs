using BookLibraryProject.Models;
using BookLibraryProject.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryProject.Services
{
    public class BorrowRecordService
    {
        private readonly IBorrowRecordRepository _repo;
        private readonly BookLibraryManagementProjectContext _context;

        public BorrowRecordService(IBorrowRecordRepository repo, BookLibraryManagementProjectContext context)
        {
            _repo = repo;
            _context = context;
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

        public async Task<List<BorrowRecord>> GetListBorrowingByStaffAsync()
        {
            return await _repo.GetListBorrowingByStaffAsync();
        }

        public async Task<List<BorrowRecord>> GetOverdueBorrowsAsync()
        {
            return await _context.BorrowRecords
                .Include(b => b.User)
                .Include(b => b.Book)
                .Where(b => b.StatusId == 6 && b.ApprovedDate.HasValue && b.ApprovedDate.Value.AddDays(3) < DateTime.Now)
                .ToListAsync();
        }

        public async Task<List<BorrowRecord>> GetListOverdueAsync()
        {
            return await _context.BorrowRecords
                .Include(b => b.User)
                .Include(b => b.Book)
                .Where(b => b.StatusId == 2 && b.PickupDeadline < DateTime.Now)
                .ToListAsync();
        }

        public async Task<List<BorrowRecord>> GetListReturnedByStaffAsync()
        {
            return await _repo.GetListReturnedByStaffAsync();
        }

        public async Task<List<BorrowRecord>> GetListOverdueByStaffAsync()
        {
            return await _repo.GetListOverdueByStaffAsync();
        }



    }
}
