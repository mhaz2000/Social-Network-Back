using Social.Network.Domain.Entities;
using Social.Network.Repository.Repositories;

namespace Social.Network.Repository.Implementation.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(Context context) : base(context)
        {

        }
    }
}
