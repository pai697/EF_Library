using System;
using System.Collections.Generic;

namespace EF_Library.Models;

public partial class Book
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? AuthorId { get; set; }
    public Author? Author { get; set; }
    public string? Genre { get; set; }
    public DateTime? DateTaken { get; set; }
    public int? Location { get; set; }
    public Location? LocationNavigation { get; set; }
    public string? SerialNumber { get; set; }
    public List<RentedBook> RentedBooks { get; set; }
}
