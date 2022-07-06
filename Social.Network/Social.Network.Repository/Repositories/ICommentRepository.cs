using Social.Network.Domain.Entities;
using Social.Network.Message.Commands;
using System;
using System.Threading.Tasks;

namespace Social.Network.Repository.Repositories
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<Guid> CreateComment(CreateCommentCommand command, Guid userId);
        Task DeleteComment(Guid id);
    }
}
