using Social.Network.Repository.Implementation.Implementations;
using Social.Network.Repository.Repositories;
using System;
using System.Threading.Tasks;

namespace Social.Network.Repository.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _context;
        private UserRepository _userRepository;
        private PostRepository _postRepository;

        public UnitOfWork(Context context)
        {
            _context = context;
        }

        public IUserRepository UserRepository => _userRepository = _userRepository ?? new UserRepository(_context);

        public IPostRepository PostRepository => _postRepository = _postRepository ?? new PostRepository(_context);

        public ICommentRepository CommentRepository => throw new NotImplementedException();

        public async Task<int> CommitAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
