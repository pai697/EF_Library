using System;
using System.Collections.Generic;

namespace EF_Library.Models;

public partial class RentedBook
{
    public int ReaderId { get; set; }
    public int BookId { get; set; }
    public DateTime BeginDate { get; set; }
    public DateTime EndDate { get; set; }
    public int WorkerId { get; set; }
    // navigation properties
    public Book Book { get; set; }
    public Reader Reader { get; set; }
    public Worker Worker { get; set; }
}
