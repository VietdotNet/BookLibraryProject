using BookLibraryProject.Models;
using BookLibraryProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookLibraryProject.Controllers
{
    public class StaffController : Controller
    {
        private readonly BorrowRecordService _borrowRecordService;

        public StaffController(BorrowRecordService borrowRecordService)
        {
            _borrowRecordService = borrowRecordService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ListBorrowRequestAsync()
        {
            var borrowRequests = await _borrowRecordService.GetListRequestBorrowByStaff();
            return PartialView("_BorrowRequestList", borrowRequests);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveBorrowByStaffAsync(Guid id)
        {
            var record = await _borrowRecordService.GetBorrowRecordById(id);
            if (record == null || record.StatusId != 1) // Kiểm tra trạng thái "Đăng ký"
            {
                return NotFound(new { success = false, message = "Không tìm thấy yêu cầu hoặc trạng thái không hợp lệ." });
            }

            record.StatusId = 6; // Cập nhật trạng thái "Đã duyệt"
            record.ApprovedDate = DateTime.Now;
            record.StaffId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _borrowRecordService.Update(record);

            return Json(new { success = true, message = "Duyệt thành công!" });
        }

        [HttpGet]
        public async Task<IActionResult> ListApprovedBorrowByStaffAsync()
        {
            var approvedBorrows = await _borrowRecordService.GetListApprovedByStaffAsync();

            return PartialView("_ApprovedBorrowList", approvedBorrows);
        }

    }
}
