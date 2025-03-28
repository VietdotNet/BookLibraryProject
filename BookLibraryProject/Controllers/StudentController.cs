using BookLibraryProject.Models;
using BookLibraryProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookLibraryProject.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly BorrowRecordService _borrowRecordService;

        public StudentController(BorrowRecordService borrowRecordService)
        {
            _borrowRecordService = borrowRecordService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListBorrowRequestAsync()
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var borrowRequests = await _borrowRecordService.GetListRequestBorrowByStudent(Id);
            return PartialView("_BorrowRequestList", borrowRequests);
        }

        [HttpPost]
        public async Task<IActionResult> CancelRequestBorrowByStudentAsync([FromBody] BorrowRecord record)
        {
            var borrowRecord = await _borrowRecordService.GetBorrowRecordById(record.Id);

            if (borrowRecord == null)
            {
                return Json(new { success = false, message = "Yêu cầu mượn không tồn tại!" });
            }

            borrowRecord.StatusId = 7; // 'Đã hủy yêu cầu'
            borrowRecord.Reason = record.Reason;

            await _borrowRecordService.Update(borrowRecord);

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> ListCanceledBorrowByStudentAsync()
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var canceledBorrows = await _borrowRecordService.GetListCanceledBorrowByStudentAsync(Id);
            return PartialView("_CanceledBorrowList", canceledBorrows);
        }

        [HttpGet]
        public async Task<IActionResult> ListApprovedBorrowByStudentAsync()
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var approvedBorrows = await _borrowRecordService.GetListApprovedBorrowByStudentAsync(Id);
            return PartialView("_ApprovedBorrowList", approvedBorrows);
        }

        [HttpGet]
        public async Task<IActionResult> ListBorrowingByStudentAsync()
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var borrowingList = await _borrowRecordService.GetListBorrowingByStudentAsync(Id);
            return PartialView("_BorrowingList", borrowingList);
        }

        [HttpGet]
        public async Task<IActionResult> ListOverdueByStudentAsync()
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var overdueList = await _borrowRecordService.GetListOverdueByStudentAsync(Id);
            return PartialView("_OverdueList", overdueList);
        }

        [HttpGet]
        public async Task<IActionResult> ListReturnedByStudentAsync()
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var returnedList = await _borrowRecordService.GetListReturnedByStudentAsync(Id);
            return PartialView("_ReturnedList", returnedList);
        }

        [HttpGet]
        public async Task<IActionResult> ListRejectedByStaffAsync()
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var rejectedList = await _borrowRecordService.GetListRejectedByStaffAsync(Id);
            return PartialView("_RejectedBorrowList", rejectedList);
        }
    }
}
