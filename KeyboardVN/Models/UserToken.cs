using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace KeyboardVN.Models
{
    public partial class UserToken : IdentityUserToken<int>
    {
        public override int UserId { get; set; }
        public override string LoginProvider { get; set; } = null!;
        public override string Name { get; set; } = null!;
        public override string? Value { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
