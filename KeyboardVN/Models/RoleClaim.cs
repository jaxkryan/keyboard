using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace KeyboardVN.Models
{
    public partial class RoleClaim : IdentityRoleClaim<int>
    {
        public override int Id { get; set; }
        public override int RoleId { get; set; }
        public override string? ClaimType { get; set; }
        public override string? ClaimValue { get; set; }

        public virtual Role Role { get; set; } = null!;
    }
}
