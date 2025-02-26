using System;
using System.Collections.Generic;

namespace BookLibraryProject.Models;

public partial class FineRule
{
    public int Id { get; set; }

    public int DaysLate { get; set; }

    public decimal FinePerDay { get; set; }
}
