using Social.Network.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Social.Network.Repository.Repositories
{
    public interface IPostRepository:IRepository<Post>
    {
        Task<Guid> CreatePost();
        Task DeletePost();
        Task UpdatePost();
    }
}
