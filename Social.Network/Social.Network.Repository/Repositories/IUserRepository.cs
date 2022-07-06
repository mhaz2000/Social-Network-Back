using Social.Network.Domain.Entities;
using Social.Network.Message.Commands;
using System;
using System.Threading.Tasks;

namespace Social.Network.Repository.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<string> CreateAsync(RegisterCommand command);
        Task UpdateUserAsync(UserUpdateCommand command,Guid userId);
        Task AddFriendAsync(Guid userId, Guid id);
        Task RemoveFriendAsync(Guid userId, Guid id);
    }
}
