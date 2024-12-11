using System;
using System.Collections.Generic;

namespace Hotel_Management.Model;

public partial class Service
{
    public int ServiceId { get; set; }

    public string ServiceName { get; set; } = null!;

    public decimal ServiceCost { get; set; }

    public virtual ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
}
