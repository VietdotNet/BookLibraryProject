using System.ComponentModel.DataAnnotations;

namespace BookLibraryProject.ViewModel
{
    public class UserProfile
    {
        [Display(Name ="Tên người dùng")]
        public string FullName { get; set; } = null!;

        [Key]
        public string Email { get; set; } = null!;

        [Display(Name = "Mã sinh viên")]
        public string? StudentCode { get; set; }

        [Display(Name = "Số điện thoại")]
        [Phone]
        public string? PhoneNumber { get; set; }
        [Display(Name = "Ngày sinh")]
        public DateOnly? Dob { get; set; }
        [Display(Name = "Ngày tạo")]
        public DateTime? CreatedAt { get; set; }
    }
}
