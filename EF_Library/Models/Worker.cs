using System;
using System.Collections.Generic;

namespace EF_Library.Models;

public partial class Worker
{
    public int WorkerId { get; set; }
    public string Position { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public List<ReadingRoom> ReadingRooms { get; set; }
    public List<RentedBook> RentedBooks { get; set; }
}
