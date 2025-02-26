using System;
using System.Collections.Generic;

namespace BookLibraryProject.Models;

public partial class NewsStatus
{
    public int Id { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<News> News { get; set; } = new List<News>();
}
