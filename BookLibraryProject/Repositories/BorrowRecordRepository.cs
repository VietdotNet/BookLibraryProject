using BookLibraryProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryProject.Repositories
{
    public class BorrowRecordRepository : IBorrowRecordRepository
    {
        private readonly BookLibraryManagementProjectContext _context; 

        public BorrowRecordRepository(BookLibraryManagementProjectContext context)
        {
            _context = context;
        }

        public async Task CreateRequestBorrow(BorrowRecord record)
        {
            _context.BorrowRecords.Add(record);
            await _context.SaveChangesAsync();
        }

        public async Task<BorrowRecord?> GetBorrowRecordById(Guid id)
        {
            return await _context.BorrowRecords
                .Include(b => b.Book)
                .Include(b => b.User)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<BorrowRecord>> GetListApprovedBorrowByStudentAsync(string Id)
        {
            return await _context.BorrowRecords
              .Include(b => b.Book)
              .Include(b => b.User)
              .Include(b => b.Status)
              .Where(br => br.StatusId == 6) // Lọc các yêu cầu chưa được xử lý bởi nhân viên
              .Where(br => br.UserId == Id)
              .ToListAsync();
        }

        public async Task<List<BorrowRecord>> GetListApprovedByStaffAsync()
        {
            return await _context.BorrowRecords
                .Include(b => b.Book)
                .Include(b => b.User)
                .Include(b => b.Status)
                .Where(b => b.StatusId == 6) //đã xét duyệt
                .ToListAsync();
        }

        public async Task<List<BorrowRecord>> GetListBorrowingByStaffAsync()
        {
            return await _context.BorrowRecords
                .Include(b => b.Book)
                .Include(b => b.User)
                .Include(b => b.Status)
                .Where(b => b.StatusId == 2) //Đang mượn
                .ToListAsync();
        }

        public async Task<List<BorrowRecord>> GetListBorrowingByStudentAsync(string Id)
        {
            return await _context.BorrowRecords
                .Include(b => b.Book)
                .Include(b => b.User)
                .Include(b => b.Status)
                .Where(b => b.StatusId == 2) //Đang mượn
                .Where(br => br.UserId == Id)
                .ToListAsync();
        }

        public async Task<List<BorrowRecord>> GetListCanceledBorrowByStudentAsync(string Id)
        {
            return await _context.BorrowRecords
              .Include(b => b.Book)
              .Include(b => b.User)
              .Include(b => b.Status)
              .Where(br => br.StatusId == 7) // Lọc các yêu cầu chưa được xử lý bởi nhân viên
              .Where(br => br.UserId == Id)
              .ToListAsync();
        }

        public async Task<List<BorrowRecord>> GetListOverdueByStaffAsync()
        {
            return await _context.BorrowRecords
               .Include(b => b.Book)
               .Include(b => b.User)
               .Include(b => b.Status)
               .Where(b => b.StatusId == 5) // Xác nhận đã trả
               .ToListAsync();
        }

        public async Task<List<BorrowRecord>> GetListOverdueByStudentAsync(string Id)
        {
            return await _context.BorrowRecords
             .Include(b => b.Book)
             .Include(b => b.User)
             .Include(b => b.Status)
             .Where(br => br.StatusId == 5)
             .Where(br => br.UserId == Id)
             .ToListAsync();
        }

        public async Task<List<BorrowRecord>> GetListRejectedByStaffAsync()
        {
            return await _context.BorrowRecords
                .Include(b => b.Book)
                .Include(b => b.User)
                .Include(b => b.Status)
                .Where(b => b.StatusId == 4 || b.StatusId == 8)
                .ToListAsync();
        }

        public async Task<List<BorrowRecord>> GetListRejectedByStaffAsync(string Id)
        {
            return await _context.BorrowRecords
               .Include(b => b.Book)
               .Include(b => b.User)
               .Include(b => b.Status)
               .Where(b => b.StatusId == 4 || b.StatusId == 8)
               .Where(br => br.UserId == Id)
               .ToListAsync();
        }

        public async Task<List<BorrowRecord>> GetListRequestBorrowByStaff()
        {
            return await _context.BorrowRecords
                .Include(b => b.Book)
                .Include(b => b.User)
                .Include(b => b.Status)
                .Where(br => br.StaffId == null) // Lọc các yêu cầu chưa được xử lý bởi nhân viên
                .Where(br => br.StatusId == 1)
                .OrderBy(br => br.BorrowDate) // Sắp xếp theo BorrowDate giảm dần
                .ToListAsync();
        }

        public async Task<List<BorrowRecord>> GetListRequestBorrowByStudentAsync(string Id)
        {
            return await _context.BorrowRecords
               .Include(b => b.Book)
               .Include(b => b.User)
               .Include(b => b.Status)
               .Where(br => br.StaffId == null) // Lọc các yêu cầu chưa được xử lý bởi nhân viên
               .Where(br => br.UserId == Id)
               .Where(br => br.StatusId == 1)
               .OrderBy(br => br.BorrowDate) // Sắp xếp theo BorrowDate giảm dần
               .ToListAsync();
        }

        public async Task<List<BorrowRecord>> GetListReturnedByStaffAsync()
        {
            return await _context.BorrowRecords
                .Include(b => b.Book)
                .Include(b => b.User)
                .Include(b => b.Status)
                .Where(b => b.StatusId == 3) // Xác nhận đã trả
                .ToListAsync();
        }

        public async Task<List<BorrowRecord>> GetListReturnedByStudentAsync(string Id)
        {
            return await _context.BorrowRecords
            .Include(b => b.Book)
            .Include(b => b.User)
            .Include(b => b.Status)
            .Where(br => br.StatusId == 3) //Đã trả
            .Where(br => br.UserId == Id)
            .ToListAsync();
        }

        public async Task Update(BorrowRecord record)
        {
             _context.BorrowRecords.Update(record);
            await _context.SaveChangesAsync();
        }
    }
}
