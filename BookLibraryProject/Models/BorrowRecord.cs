using System;
using System.Collections.Generic;

namespace BookLibraryProject.Models;

public partial class BorrowRecord
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = null!;

    public Guid BookId { get; set; }

    public DateTime? BorrowDate { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? PickupDeadline { get; set; }

    public DateTime? ReturnDate { get; set; }

    public decimal? FineAmount { get; set; }

    public int StatusId { get; set; }

    public string? StaffId { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual User? Staff { get; set; }

    public virtual BorrowStatus Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
