using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace KeyboardVN.Models
{
    public partial class UserRole : IdentityUserRole<int>
    {
        public override int UserId { get; set; }
        public override int RoleId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
