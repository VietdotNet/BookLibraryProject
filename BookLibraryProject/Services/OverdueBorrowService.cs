
namespace BookLibraryProject.Services
{
    public class OverdueBorrowService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OverdueBorrowService> _logger;

        public OverdueBorrowService(IServiceProvider serviceProvider, ILogger<OverdueBorrowService> logger)
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

                        var overdueBorrows = await borrowService.GetListOverdueAsync();

                        foreach (var record in overdueBorrows)
                        {
                            record.StatusId = 5; // Cập nhật trạng thái thành "Quá hạn trả sách"
                            await borrowService.Update(record);
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
