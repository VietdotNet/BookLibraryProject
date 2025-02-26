using System;
using System.Collections.Generic;

namespace BookLibraryProject.Models;

public partial class BorrowStatus
{
    public int Id { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
}
