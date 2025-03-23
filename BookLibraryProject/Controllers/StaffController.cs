using BookLibraryProject.Models;
using BookLibraryProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace BookLibraryProject.Controllers
{
    public class StaffController : Controller
    {
        private readonly BorrowRecordService _borrowRecordService;
        private readonly SendMailService _sendMailService;

        public StaffController(BorrowRecordService borrowRecordService, SendMailService sendMailService)
        {
            _borrowRecordService = borrowRecordService;
            _sendMailService = sendMailService;
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
            record.Book.BorrowedQuantity += 1;

            var deadline = DateTime.Now.AddDays(3);
            string formattedDate = deadline.ToString("dd/MM/yyyy");
            string formattedPickupDeadline = record.PickupDeadline?.ToString("dd/MM/yyyy") ?? "N/A";


            //borrowRecord.PickupDeadline = now.AddDays(15).Date;
            var mailContent = new MailContent();
            mailContent.To = record.User.Email;
            mailContent.Subject = "Thông báo về yêu cầu mượn sách";
            mailContent.Body = "<h3>Yêu cầu mượn sách của bạn đã được XÉT DUYỆT!</h3>" +
                "<br>Thông tin chi tiết: " +
                "<br>- <b>Tên sách:</b> " + record.Book.Title +
                "<br>- <b>Tác giả:</b> " + record.Book.Author +
                "<br>- <b>Ngày tạo đơn mượn:</b> " + record.BorrowDate +
                "<br>- <b>Hạn trả sách:</b> " + formattedPickupDeadline +
                "<br>Vui lòng đến quầy tại thư viện để lấy sách trước <b>17h ngày " + formattedDate + "</b>. Nếu sau thời gian đó mà bạn chưa lấy sách thì yêu cầu mượn sách của bạn sẽ bị hủy!" +
                "<br>Quy định mượn/trả sách của thư viện: <a href='https://localhost:7260/Home/Index" + "'>Xem chi tiết</a>";

            await _sendMailService.SendMailAsync(mailContent);

            await _borrowRecordService.Update(record);

            return Json(new { success = true, message = "Duyệt thành công!" });
        }

        [HttpGet]
        public async Task<IActionResult> ListApprovedBorrowByStaffAsync()
        {
            var approvedBorrows = await _borrowRecordService.GetListApprovedByStaffAsync();

            return PartialView("_ApprovedBorrowList", approvedBorrows);
        }

        [HttpPost]
        public async Task<IActionResult> RejectBorrowByStaffAsync([FromBody] BorrowRecord record)
        {
            var borrowRecord = await _borrowRecordService.GetBorrowRecordById(record.Id);

            if (borrowRecord == null)
            {
                return Json(new { success = false, message = "Yêu cầu mượn không tồn tại!" });
            }

            borrowRecord.StatusId = 4; // 'Bị từ chối'
            borrowRecord.Reason = record.Reason;
            borrowRecord.ApprovedDate = DateTime.Now;
            borrowRecord.StaffId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _borrowRecordService.Update(borrowRecord);

            string formattedPickupDeadline = borrowRecord.PickupDeadline?.ToString("dd/MM/yyyy") ?? "N/A";
            // Gửi email thông báo từ chối
            var mailContent = new MailContent();

            mailContent.To = borrowRecord.User.Email;
            mailContent.Subject = "Thông báo về yêu cầu mượn sách";
            mailContent.Body = $"<h3>Yêu cầu mượn sách của bạn đã bị TỪ CHỐI với lý do: <b>{record.Reason}.</b> </h3>" +
                    "<br>Thông tin chi tiết: " +
                    "<br>- <b>Tên sách:</b> " + borrowRecord.Book.Title +
                    "<br>- <b>Tác giả:</b> " + borrowRecord.Book.Author +
                    "<br>- <b>Ngày tạo đơn mượn:</b> " + borrowRecord.BorrowDate +
                    "<br>- <b>Hạn trả sách:</b> " + formattedPickupDeadline +
                    "<br>Quy định mượn/trả sách của thư viện: <a href='https://localhost:7260/Home/Index" + "'>Xem chi tiết</a>" +
                    "<p>Nếu bạn có bất kỳ thắc mắc nào, vui lòng liên hệ với thư viện qua:</p>" +
                    "<p><b>Email:</b> <a href='#'>vietnq021103@gmail.com</a></p>" +
                    "<p><b>Điện thoại:</b> +84 123 456 789</p>" +
                    "<p><b>Địa chỉ:</b> Tầng 1, Tòa Delta, Đại học FPT Hà Nội</p>";
          
            await _sendMailService.SendMailAsync(mailContent);

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> ListRejectedBorrowByStaffAsync()
        {
            var rejectedBorrows = await _borrowRecordService.GetListRejectedByStaffAsync();
            return PartialView("_RejectedBorrowList", rejectedBorrows);
        }


    }
}
