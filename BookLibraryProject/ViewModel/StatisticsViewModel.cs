namespace BookLibraryProject.ViewModel
{
    public class StatisticsViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalBooks { get; set; }
        public decimal TotalFineAmount { get; set; }

        public int TotalBookCategories { get; set; } = 0;
    }
}
