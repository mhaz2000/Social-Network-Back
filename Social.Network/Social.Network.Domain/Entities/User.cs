using Microsoft.AspNetCore.Identity;
using System;

namespace Social.Network.Domain.Entities
{
    public class User : IdentityUser
    {
        public User(string firstName, string lastName, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
