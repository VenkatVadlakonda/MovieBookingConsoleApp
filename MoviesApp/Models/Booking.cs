using System;
using System.Collections.Generic;

namespace MoviesApp.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int? UserId { get; set; }

    public DateTime? BookingTime { get; set; }

    public decimal? Totalprice { get; set; }

    public int? MovieId { get; set; }

    public string? ShowTime { get; set; }

    public virtual Movie? Movie { get; set; }

    public virtual User? User { get; set; }
}
