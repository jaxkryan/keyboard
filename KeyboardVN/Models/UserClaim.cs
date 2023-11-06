using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace KeyboardVN.Models
{
    public partial class UserClaim : IdentityUserClaim<int>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? ClaimType { get; set; }
        public string? ClaimValue { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
