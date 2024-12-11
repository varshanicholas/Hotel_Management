using System;
using System.Collections.Generic;

namespace Hotel_Management.Model;

public partial class CurrentBooking
{
    public string GuestName { get; set; } = null!;

    public string RoomNumber { get; set; } = null!;

    public DateTime CheckInDate { get; set; }
}
