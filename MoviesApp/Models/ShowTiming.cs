using System;
using System.Collections.Generic;

namespace MoviesApp.Models;

public partial class ShowTiming
{
    public int ShowId { get; set; }

    public int? MovieId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Movie? Movie { get; set; }
}
