using System;
using System.Collections.Generic;

namespace Hotel_Management.Model;

public partial class RoomsOverview
{
    public string RoomNumber { get; set; } = null!;

    public string RoomType { get; set; } = null!;

    public decimal PricePerNight { get; set; }

    public string AvailabilityStatus { get; set; } = null!;
}
