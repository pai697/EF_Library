using System;
using System.Collections.Generic;

namespace EF_Library.Models;

public partial class Location
{
    public int LocationId { get; set; }
    public int Room { get; set; }
    public int Shelf { get; set; }
    public virtual List<Book> Books { get; set; }
}
