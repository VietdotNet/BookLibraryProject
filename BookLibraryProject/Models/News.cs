using System;
using System.Collections.Generic;

namespace BookLibraryProject.Models;

public partial class News
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public string CreatedByStaffId { get; set; } = null!;

    public int? NewsStatus { get; set; }

    public string? UpdatedByStaffId { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual User CreatedByStaff { get; set; } = null!;

    public virtual NewsStatus? NewsStatusNavigation { get; set; }

    public virtual User? UpdatedByStaff { get; set; }
}
