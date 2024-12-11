using System;
using System.Collections.Generic;

namespace Hotel_Management.Model;

public partial class Room
{
    public int RoomId { get; set; }

    public string RoomNumber { get; set; } = null!;

    public string RoomType { get; set; } = null!;

    public decimal PricePerNight { get; set; }

    public string AvailabilityStatus { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
