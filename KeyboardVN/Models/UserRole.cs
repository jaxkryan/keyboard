using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace KeyboardVN.Models
{
    public partial class UserRole : IdentityUserRole<int>
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
