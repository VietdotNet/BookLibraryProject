using System;
using System.Collections.Generic;

namespace BookLibraryProject.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual UserIdtracker? UserIdtracker { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
