using System;
using System.Collections.Generic;

namespace EF_Library.Models;

public partial class Reader
{
    public int ReaderId { get; set; }
    public string Name { get; set; }
    public string MiddleName { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Number { get; set; }
    public List<RentedBook> RentedBooks { get; set; }
}
