using Microsoft.AspNetCore.Identity;
using System;

namespace Social.Network.Domain.Entities
{
    public class User : IdentityUser
    {
        public User() { }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
