using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EF_Library.Models;

public partial class Reader
{
    [Key]
    public int ReaderId { get; set; }
    public string Name { get; set; } = null!;
    public string MiddleName { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Email { get; set; } = null!;
    [Required][MaxLength(13)] public string Number { get; set; } = null!;
    public virtual List<RentedBook> RentedBooks { get; set; } = new List<RentedBook>();
}
