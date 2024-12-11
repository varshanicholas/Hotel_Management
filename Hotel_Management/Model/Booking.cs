using System;
using System.Collections.Generic;

namespace Hotel_Management.Model;

public partial class Booking
{
    public int BookingId { get; set; }

    public int GuestId { get; set; }

    public int RoomId { get; set; }

    public DateTime CheckInDate { get; set; }

    public DateTime CheckOutDate { get; set; }

    public string BookingStatus { get; set; } = null!;

    public virtual Guest Guest { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;

    public virtual ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
}
