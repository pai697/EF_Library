using System;
using System.Collections.Generic;

namespace EF_Library.Models;

public partial class Worker
{
    public int WorkerId { get; set; }
    public string Position { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public virtual List<ReadingRoom> ReadingRooms { get; set; } = new List<ReadingRoom>();
    public virtual List<RentedBook> RentedBooks { get; set; } = new List<RentedBook>();
}
