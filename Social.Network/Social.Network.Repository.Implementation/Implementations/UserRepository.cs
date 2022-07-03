using Microsoft.AspNetCore.Identity;
using Social.Network.Domain.Entities;
using Social.Network.Message.Commands;
using Social.Network.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Social.Network.Repository.Implementation.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(Context context) : base(context) { }

        public async Task<string> CreateAsync(RegisterCommand command)
        {
            var _passwordHasher = new PasswordHasher<User>();

            User user = new User()
            {
                Email = command.Email,
                Id = Guid.NewGuid().ToString(),
                UserName = command.Name,
                Posts = new List<Post>()
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, command.Password);
            await Context.Users.AddAsync(user);

            return user.Id;
        }

        public async Task UpdateUserAsync(UserUpdateCommand command, Guid userId)
        {
            var user =  await Context.Users.FindAsync(userId.ToString());

            user.FirstName = command.FirstName;
            user.Address = command.Address;
            user.Avatar = command.Avatar;
            user.Country = command.Country;
            user.Description = command.Description;
            user.LastName = command.LastName;
            user.PhoneNumber = command.PhoneNumber;
            user.PostCode = command.PostCode;
            user.TownOrCity = command.TownOrCity;
        }
    }
}
