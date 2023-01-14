using System;
using System.Collections.Generic;

namespace EF_Library.Models;

public partial class Worker : Person
{
    public string Position { get; set; } = null!;
    public virtual List<ReadingRoom> ReadingRooms { get; set; } = new List<ReadingRoom>();
    public virtual List<RentedBook> RentedBooks { get; set; } = new List<RentedBook>();
}
