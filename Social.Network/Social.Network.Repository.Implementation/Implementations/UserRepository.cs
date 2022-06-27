using Microsoft.AspNetCore.Identity;
using Social.Network.Domain.Entities;
using Social.Network.Message.Commands;
using Social.Network.Repository.Repositories;
using System;
using System.Threading.Tasks;

namespace Social.Network.Repository.Implementation.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(Context context) : base(context) { }

        public async Task CreateAsync(RegisterCommand command)
        {
            var _passwordHasher = new PasswordHasher<User>();

            User user = new User()
            {
                Email = command.Email,
                Id = Guid.NewGuid().ToString(),
                UserName = command.Name
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, command.Password);
            await Context.Users.AddAsync(user);
        }
    }
}
