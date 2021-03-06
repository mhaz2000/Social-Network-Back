using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Social.Network.Domain.Entities
{
    public class User : IdentityUser
    {
        public User() { }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string TownOrCity { get; set; }
        public string PostCode { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public ICollection<User> Friends { get; set; }
        public ICollection<Post> Posts { get; set; }

    }
}
