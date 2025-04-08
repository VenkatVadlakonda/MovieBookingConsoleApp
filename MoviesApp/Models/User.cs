using System;
using System.Collections.Generic;

namespace MoviesApp.Models;

public partial class User
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string Role { get; set; } = null!;

    public string? UserName { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
