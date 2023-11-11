using System;
using System.Collections.Generic;

namespace KeyboardVN.Models
{
    public partial class Product
    {
        public Product()
        {
            CartItems = new HashSet<CartItem>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public double? Discount { get; set; }
        public int UnitInStock { get; set; }

        public virtual Brand? Brand { get; set; } = null!;
        public virtual Category? Category { get; set; } = null!;
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<Feedback>? Feedbacks { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
