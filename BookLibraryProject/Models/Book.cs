using System;
using System.Collections.Generic;

namespace BookLibraryProject.Models;

public partial class Book
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public int? PublishedYear { get; set; }

    public string Description { get; set; } = null!;

    public int CategoryId { get; set; }

    public int? Quantity { get; set; }

    public int? BorrowedQuantity { get; set; }

    public int? AvailableQuantity { get; set; }

    public decimal Price { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? AddedById { get; set; }

    public virtual User? AddedBy { get; set; }

    public virtual ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();

    public virtual BookCategory Category { get; set; } = null!;
}
