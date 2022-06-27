using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Social.Network.Domain.Entities;

namespace Social.Network.Repository.Implementation
{
    public class Context : IdentityDbContext<User, IdentityRole, string>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {


        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var connectionStr = "server=localhost; port=3306; database=socialNetworkDB; user=root; password=1234; Persist Security Info=False; Connect Timeout=3000;";
        //    optionsBuilder.UseMySql(connectionStr, ServerVersion.AutoDetect(connectionStr));
        //    base.OnConfiguring(optionsBuilder);
        //}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(builder1 =>
            {
                builder1.Metadata.RemoveIndex(new[] { builder1.Property(u => u.NormalizedUserName).Metadata });
                builder1.Metadata.RemoveIndex(new[] { builder1.Property(u => u.UserName).Metadata });

                var index = builder.Entity<User>()
                .HasIndex(u => new { u.UserName }).Metadata;
                builder.Entity<User>().Metadata.RemoveIndex(index);

                var index2 = builder.Entity<User>()
                .HasIndex(u => new { u.UserName }).Metadata;
                builder.Entity<User>().Metadata.RemoveIndex(index2.Properties);
            });

            base.OnModelCreating(builder);
        }
    }
}
