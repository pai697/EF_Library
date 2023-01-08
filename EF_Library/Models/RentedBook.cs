using System;
using System.Collections.Generic;

namespace EF_Library.Models;

public partial class RentedBook
{
    public int ReaderId { get; set; }

    public int BookId { get; set; }

    public DateTime? BeginDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? WorkerId { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Reader Reader { get; set; } = null!;

    public virtual Worker? Worker { get; set; }
}
