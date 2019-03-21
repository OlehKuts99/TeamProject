using System;
using Microsoft.AspNetCore.Identity;

namespace Store.Models
{
    /// <summary>
    /// Model for user's account.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreateDate { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
