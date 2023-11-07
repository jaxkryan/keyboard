using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace KeyboardVN.Models
{
    public partial class UserLogin : IdentityUserLogin<int>
    {
        public override string LoginProvider { get; set; } = null!;
        public override string ProviderKey { get; set; } = null!;
        public override string? ProviderDisplayName { get; set; }
        public override int UserId { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
