using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EF_Library.Models;

public partial class Book
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public int AuthorId { get; set; }
    public virtual List<Author> Authors { get; set; } = null!;
    public string Genre { get; set; } = null!;
    public DateTime DateTaken { get; set; }
    public int Location { get; set; }
    public virtual Location LocationNavigation { get; set; } = null!;
    public string SerialNumber { get; set; } = null!;
    public virtual List<RentedBook> RentedBooks { get; set; } = new List<RentedBook>();
}
