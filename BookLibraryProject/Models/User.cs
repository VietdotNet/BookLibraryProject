using System;
using System.Collections.Generic;

namespace BookLibraryProject.Models;

public partial class User
{
    public string Id { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? StudentCode { get; set; }

    public string? PhoneNumber { get; set; }

    public DateOnly? Dob { get; set; }

    public int RoleId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();

    public virtual ICollection<BorrowRecord> BorrowRecordStaffs { get; set; } = new List<BorrowRecord>();

    public virtual ICollection<BorrowRecord> BorrowRecordUsers { get; set; } = new List<BorrowRecord>();

    public virtual ICollection<News> NewsCreatedByStaffs { get; set; } = new List<News>();

    public virtual ICollection<News> NewsUpdatedByStaffs { get; set; } = new List<News>();

    public virtual Role Role { get; set; } = null!;
}
