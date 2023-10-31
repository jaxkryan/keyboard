using System;
using System.Collections.Generic;

namespace KeyboardVN.Models
{
    public partial class Brand
    {
        public Brand()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Image { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
