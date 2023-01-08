using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EF_Library.Models;

public partial class Author
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string MiddleName { get; set; }
    public string Surname { get; set; }
    public List<Book> Books { get; set; }
}
