using System;
using System.Collections.Generic;

namespace EF_Library.Models;

public partial class ReadingRoom
{
    public int ReadingRoomId { get; set; }
    public int? WorkerId { get; set; }
    public Worker? Worker { get; set; }
    public int? Room { get; set; }
}
