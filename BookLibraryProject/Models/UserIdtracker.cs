using System;
using System.Collections.Generic;

namespace BookLibraryProject.Models;

public partial class UserIdtracker
{
    public int RoleId { get; set; }

    public string Prefix { get; set; } = null!;

    public int? LastId { get; set; }

    public virtual Role Role { get; set; } = null!;
}
