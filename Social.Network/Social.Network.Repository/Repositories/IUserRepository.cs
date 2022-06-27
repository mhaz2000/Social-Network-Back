using Social.Network.Domain.Entities;
using Social.Network.Message.Commands;
using System.Threading.Tasks;

namespace Social.Network.Repository.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task CreateAsync(RegisterCommand command);
    }
}
