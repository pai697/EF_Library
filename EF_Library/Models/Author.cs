using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EF_Library.Models;

public partial class Author
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    public string MiddleName { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public virtual List<Book> Books { get; set; } = new List<Book>();
}
