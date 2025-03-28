﻿using BookLibraryProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryProject.Repositories
{
    public interface IBorrowRecordRepository
    {
        Task CreateRequestBorrow(BorrowRecord record);

        Task<List<BorrowRecord>> GetListRequestBorrowByStaff();

        Task<BorrowRecord?> GetBorrowRecordById(Guid id);

        Task Update(BorrowRecord record);

        Task<List<BorrowRecord>> GetListApprovedByStaffAsync();

        Task<List<BorrowRecord>> GetListRejectedByStaffAsync();

        Task<List<BorrowRecord>> GetListBorrowingByStaffAsync();

        Task<List<BorrowRecord>> GetListReturnedByStaffAsync();

        Task<List<BorrowRecord>> GetListOverdueByStaffAsync();
    }


}
