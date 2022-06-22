using Microsoft.EntityFrameworkCore;
using Social.Network.Domain.Entities;

namespace Social.Network.Repository.Implementation
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }
    }
}
