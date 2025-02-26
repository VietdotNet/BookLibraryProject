using System;
using System.Collections.Generic;

namespace BookLibraryProject.Models;

public partial class BookCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? ParentId { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();

    public virtual ICollection<BookCategory> InverseParent { get; set; } = new List<BookCategory>();

    public virtual BookCategory? Parent { get; set; }
}
