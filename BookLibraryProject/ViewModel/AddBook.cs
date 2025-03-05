using BookLibraryProject.Models;
using System.ComponentModel.DataAnnotations;

namespace BookLibraryProject.ViewModel
{
    public class AddBook
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Hình ảnh")]
        public string? Image { get; set; }

        [Display(Name ="Tên sách")]
        [Required]
        public string Title { get; set; } = null!;
        [Display(Name = "Tác giả")]
        [Required]
        public string Author { get; set; } = null!;
        [Display(Name = "Năm xuất bản")]
        [Required]
        public int? PublishedYear { get; set; }
        [Display(Name = "Mô tả")]
        [Required]
        public string Description { get; set; } = null!;

        [Display(Name = "Hạng mục")]
        [Required]
        public int CategoryId { get; set; }
        [Display(Name = "Số lượng")]
        [Required]
        public int? Quantity { get; set; }
        [Display(Name = "Giá")]
        [Required]
        public decimal Price { get; set; }
        [Display(Name = "Ngày thêm")]
        public DateTime? CreatedAt { get; set; }
        [Display(Name = "Được thêm bởi(ID)")]
        public string? AddedById { get; set; }

        // ✅ Thêm trường Ngôn ngữ
        [Display(Name = "Ngôn ngữ")]
        [Required]
        public string Language { get; set; } = "Tiếng Việt"; // Giá trị mặc định

    }
}
