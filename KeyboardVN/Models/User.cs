using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace KeyboardVN.Models
{
    public partial class User : IdentityUser<int>
    {
        public User()
        {
            Carts = new HashSet<Cart>();
            Orders = new HashSet<Order>();
            UserClaims = new HashSet<UserClaim>();
            UserLogins = new HashSet<UserLogin>();
            UserRoles = new HashSet<UserRole>();
            UserTokens = new HashSet<UserToken>();
        }

        [PersonalData]
        public override int Id { get; set; }

        [PersonalData]
        public string? FirstName { get; set; }

        [PersonalData]
        public string? LastName { get; set; }

        [PersonalData]
        public string? Street { get; set; }

        [PersonalData]
        public string? City { get; set; }

        [PersonalData]
        public string? Province { get; set; }

        [PersonalData]
        public string? Country { get; set; }

        [ProtectedPersonalData]
        public override string? UserName { get; set; }
        public override string? NormalizedUserName { get; set; }

        [ProtectedPersonalData]
        public override string? Email { get; set; }
        public override string? NormalizedEmail { get; set; }

        [PersonalData]
        public override bool EmailConfirmed { get; set; }
        public override string? PasswordHash { get; set; }
        public override string? SecurityStamp { get; set; }
        public override string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        [ProtectedPersonalData]
        public override string? PhoneNumber { get; set; }

        [PersonalData]
        public override bool PhoneNumberConfirmed { get; set; }

        [PersonalData]
        public override bool TwoFactorEnabled { get; set; }
        public override DateTimeOffset? LockoutEnd { get; set; }
        public override bool LockoutEnabled { get; set; }
        public override int AccessFailedCount { get; set; }

        public virtual ICollection<Feedback>? CustomerFeedbacks { get; set; }

        // Navigation property for seller feedbacks
        public virtual ICollection<Feedback>? SellerFeedbacks { get; set; }
        public virtual ICollection<Cart>? Carts { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<UserClaim>? UserClaims { get; set; }
        public virtual ICollection<UserLogin>? UserLogins { get; set; }
        public virtual ICollection<UserRole>? UserRoles { get; set; }
        public virtual ICollection<UserToken>? UserTokens { get; set; }
    }
}
