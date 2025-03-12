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
    }
}
