using System;
using System.Collections.Generic;

namespace MoviesApp.Models;

public partial class Movie
{
    public int MovieId { get; set; }

    public string MovieName { get; set; } = null!;

    public string Genre { get; set; } = null!;

    public string? Duration { get; set; }

    public int AgeRestriction { get; set; }

    public string ShowTime { get; set; } = null!;

    public decimal Price { get; set; }

    public int NumberOfTickets { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
