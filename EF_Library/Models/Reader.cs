using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EF_Library.Models;

public partial class Reader
{
    [Key]
    public int ReaderId { get; set; }
    public string Name { get; set; }
    public string MiddleName { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    [Required][MaxLength(13)] public string Number { get; set; }
    public virtual List<RentedBook> RentedBooks { get; set; }
}
