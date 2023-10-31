using System;
using System.Collections.Generic;

namespace KeyboardVN.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Receiver { get; set; }
        public string? ShipStreet { get; set; }
        public string? ShipCity { get; set; }
        public string? ShipProvince { get; set; }
        public string? ShipCountry { get; set; }
        public string? ShipEmail { get; set; }
        public string? ShipPhone { get; set; }
        public string Status { get; set; } = null!;
        public DateTime? CreatedTime { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
