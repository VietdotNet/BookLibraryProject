using BookLibraryProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryProject.Repositories
{
    public interface IBorrowRecordRepository
    {
        Task CreateRequestBorrow(BorrowRecord record);
    }
}
