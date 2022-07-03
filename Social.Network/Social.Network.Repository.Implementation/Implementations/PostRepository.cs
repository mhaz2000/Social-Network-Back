using Microsoft.EntityFrameworkCore;
using Social.Network.Domain.Entities;
using Social.Network.Message.Commands;
using Social.Network.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Network.Repository.Implementation.Implementations
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(Context context): base(context)
        {

        }
        public async Task<Guid> CreatePost(CreatePostCommand command, Guid userId)
        {
            var post = new Post()
            {
                Comments = new HashSet<Comment>(),
                Content = command.Content,
                CreationDate = DateTime.Now,
                Image = Guid.Parse(command.Image),
                PostOwnerId = userId
            };

            await Context.Posts.AddAsync(post);

            var users = Context.Users.Include(c => c.Posts);
            (await users.FirstOrDefaultAsync(c=>c.Id == userId.ToString())).Posts.Add(post);

            return post.Id;
        }

        public async Task DeletePost(Guid postId)
        {
            var post = await Context.Posts.FindAsync(postId);
            Context.Posts.Remove(post);
        }

        public Task UpdatePost()
        {
            throw new NotImplementedException();
        }
    }
}
