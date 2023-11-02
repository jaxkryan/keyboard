using System;
using System.Collections.Generic;

namespace KeyboardVN.Models
{
    public partial class Cart
    {
        public Cart()
        {
            CartItems = new HashSet<CartItem>();
        }

        public int Id { get; set; }
        public int? UserId { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
