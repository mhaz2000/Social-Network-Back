using Microsoft.EntityFrameworkCore;
using Social.Network.Domain.Entities;
using Social.Network.Message.Commands;
using Social.Network.Repository.Repositories;
using System;
using System.Threading.Tasks;

namespace Social.Network.Repository.Implementation.Implementations
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(Context context) : base(context)
        {

        }
        public async Task<Guid> CreateComment(CreateCommentCommand command, Guid userId)
        {
            var comment = new Comment()
            {
                CommentOwnerId = userId,
                Content = command.Content,
            };

            var post = await Context.Posts.Include(c => c.Comments).FirstOrDefaultAsync(c => c.Id == command.PostId);
            post.Comments.Add(comment);
            await Context.Comments.AddAsync(comment);

            return comment.Id;
        }

        public Task DeleteComment(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
