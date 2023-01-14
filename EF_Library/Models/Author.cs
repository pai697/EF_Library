using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EF_Library.Models;

public partial class Author : Person
{
    public virtual List<Book> Books { get; set; } = new List<Book>();
}
