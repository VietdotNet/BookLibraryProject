
namespace BookLibraryProject.Services
{
    public class OverdueCheckService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OverdueCheckService> _logger;

        public OverdueCheckService(IServiceProvider serviceProvider, ILogger<OverdueCheckService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var borrowService = scope.ServiceProvider.GetRequiredService<BorrowRecordService>();
                        var mailService = scope.ServiceProvider.GetRequiredService<SendMailService>();

                        var overdueBorrows = await borrowService.GetOverdueBorrowsAsync();

                        foreach (var record in overdueBorrows)
                        {
                            record.StatusId = 8; // Cập nhật trạng thái thành "Quá hạn lấy"
                            record.Reason = "Quá hạn lấy sách";
                            await borrowService.Update(record);

                            // Gửi email thông báo
                            var mailContent = new MailContent
                            {
                                To = record.User.Email,
                                Subject = "Thông báo: Yêu cầu mượn sách bị hủy do quá hạn lấy",
                                Body = $"<h3>Yêu cầu mượn sách của bạn đã bị HỦY do bạn không đến lấy sách đúng hạn.</h3>" +
                                       "<br>Thông tin chi tiết: " +
                                       $"<br>- <b>Tên sách:</b> {record.Book.Title}" +
                                       $"<br>- <b>Tác giả:</b> {record.Book.Author}" +
                                       $"<br>- <b>Ngày đăng ký:</b> {record.BorrowDate:dd/MM/yyyy}" +
                                       $"<br>- <b>Hạn đến lấy sách:</b> {record.PickupDeadline:dd/MM/yyyy}" +
                                       "<br>Vui lòng kiểm tra lại thời gian mượn sách trong tương lai." +
                                       "<p>Nếu có thắc mắc, liên hệ thư viện: vietnq021103@gmail.com</p>"
                            };

                            await mailService.SendMailAsync(mailContent);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Lỗi khi kiểm tra sách quá hạn: {ex.Message}");
                }

                // Chờ 1 ngày trước khi kiểm tra lại (tối ưu tài nguyên)
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }
}
