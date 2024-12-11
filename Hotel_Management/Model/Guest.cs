using System;
using System.Collections.Generic;

namespace Hotel_Management.Model;

public partial class Guest
{
    public int GuestId { get; set; }

    public string GuestName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? Email { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
