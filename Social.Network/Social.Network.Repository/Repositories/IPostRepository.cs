using Social.Network.Domain.Entities;
using Social.Network.Message.Commands;
using System;
using System.Threading.Tasks;

namespace Social.Network.Repository.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<Guid> CreatePost(CreatePostCommand command, Guid userId);
        Task DeletePost(Guid postId);
        Task UpdatePost();
    }
}
