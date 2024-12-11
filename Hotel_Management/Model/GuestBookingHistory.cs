using System;
using System.Collections.Generic;

namespace Hotel_Management.Model;

public partial class GuestBookingHistory
{
    public string GuestName { get; set; } = null!;

    public int RoomId { get; set; }

    public DateTime CheckInDate { get; set; }

    public DateTime CheckOutDate { get; set; }
}
