using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Social.Network
{
    public static class WebHostExtension
    {
        private static readonly MyDbContextFactory MyDbContextFactory = new MyDbContextFactory();
        public static IWebHost Seed(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                var context = MyDbContextFactory.CreateDbContext(new[] { string.Empty });
                context.Database.Migrate();
            }

            return host;
        }
    }
}
