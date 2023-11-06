using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace KeyboardVN.Models
{
    public partial class RoleClaim : IdentityRoleClaim<int>
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string? ClaimType { get; set; }
        public string? ClaimValue { get; set; }

        public virtual Role Role { get; set; } = null!;
    }
}
