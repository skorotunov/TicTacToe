using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicTacToe.Infrastructure.Identity;
using TicTacToe.Infrastructure.Persistence;

namespace TicTacToe.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddDbContext<TicTacToeDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly(typeof(TicTacToeDbContext).Assembly.FullName)));

            services.AddDefaultIdentity<TicTacToeUser>()
                .AddEntityFrameworkStores<TicTacToeDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<TicTacToeUser, TicTacToeDbContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            return services;
        }
    }
}
