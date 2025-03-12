using BookLibraryProject.Models;
using BookLibraryProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookLibraryProject.Controllers
{
    [Authorize]
    public class BorrowBooksController : Controller
    {
        private readonly BorrowRecordService _borrowService;
        private readonly BookService _bookService; // Thêm BookService để kiểm tra số lượng sách

        public BorrowBooksController(BorrowRecordService borrowService, BookService bookService)
        {
            _borrowService = borrowService;
            _bookService = bookService;
        }

        [HttpPost]
        public async Task<IActionResult> Borrow([FromBody] BorrowRecord borrowRecord)
        {
            if (borrowRecord == null || borrowRecord.BookId == Guid.Empty)
            {
                return BadRequest(new { message = "Dữ liệu mượn sách không hợp lệ!" });
            }

            try
            {
                //// Lấy thông tin sách từ BookService để kiểm tra số lượng còn lại
                //var book = await _bookService.GetBookByIdAsync(borrowRecord.BookId);
                //if (book == null)
                //{
                //    return NotFound(new { message = "Không tìm thấy sách!" });
                //}

                //if (book.AvailableQuantity <= 0)
                //{
                //    return BadRequest(new { message = "Sách đã hết, không thể mượn!" });
                //}

                // Gán thông tin người dùng đang đăng nhập
                borrowRecord.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                borrowRecord.StatusId = 1; // 1 = Chờ duyệt

                // Xử lý ngày mượn & hạn trả hợp lệ
                DateTime now = DateTime.Now;
                borrowRecord.BorrowDate = now;
                borrowRecord.PickupDeadline = now.AddDays(15).Date; // 15 ngày sau

                // Log kiểm tra
                Console.WriteLine($"Mượn sách: {borrowRecord.BookId}, Người dùng: {borrowRecord.UserId}, Hạn trả: {borrowRecord.PickupDeadline}");

                // Ghi nhận yêu cầu mượn sách vào DB
                await _borrowService.CreateRequestBorrow(borrowRecord);

                // Cập nhật số lượng sách sau khi mượn
                //book.AvailableQuantity -= 1;
                //book.BorrowedQuantity += 1;
                //await _bookService.UpdateBookAsync(book);

                return Ok(new { message = "Mượn sách thành công!", borrowRecord });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xử lý yêu cầu mượn sách: {ex.Message}");
                return StatusCode(500, new { message = "Lỗi hệ thống khi mượn sách!", details = ex.Message });
            }
        }
    }
}
