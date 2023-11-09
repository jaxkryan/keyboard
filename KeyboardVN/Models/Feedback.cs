using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KeyboardVN.Models
{
    public partial class Feedback
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? CustomerId { get; set; }
        public int? ProductId { get; set; }
        public int? SellerId { get; set; }
        public string? Content { get; set; }
        public string? Reply { get; set; }
        public DateTime? FeedbackDate { get; set; }
        public DateTime? ReplyDate { get; set; }
        public bool? Checked { get; set; }

        public virtual User? Customer { get; set; }
        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
        public virtual User? Seller { get; set; }
    }
}
