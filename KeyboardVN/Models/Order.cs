using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Display(Name = "Receiver")]
        public string? Receiver { get; set; }
        [Display(Name = "Street")]
        public string? ShipStreet { get; set; }

        [Display(Name = "City")]
        public string? ShipCity { get; set; }

        [Display(Name = "Province")]
        public string? ShipProvince { get; set; }

        [Display(Name = "Country")]
        public string? ShipCountry { get; set; }

        [Display(Name = "Email Address")]
        public string? ShipEmail { get; set; }

        [Display(Name = "Phone")]
        public string? ShipPhone { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; } = null!;

        [Display(Name = "Created Time")]
        public DateTime? CreatedTime { get; set; }

        public virtual ICollection<Feedback>? Feedbacks { get; set; }
        public virtual User? User { get; set; } = null!;
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
