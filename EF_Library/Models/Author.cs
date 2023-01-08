using System;
using System.Collections.Generic;

namespace EF_Library.Models;

public partial class Author
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string MiddleName { get; set; }
    public string Surname { get; set; }
    public List<Book> Books { get; set; }
}
