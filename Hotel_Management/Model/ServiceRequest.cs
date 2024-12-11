using System;
using System.Collections.Generic;

namespace Hotel_Management.Model;

public partial class ServiceRequest
{
    public int RequestId { get; set; }

    public int BookingId { get; set; }

    public int ServiceId { get; set; }

    public DateTime RequestDate { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
