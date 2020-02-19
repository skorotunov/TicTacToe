using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Security.Claims;
using TicTacToe.Application.Common.Interfaces;
using TicTacToe.Infrastructure.Identity;
using TicTacToe.Infrastructure.Persistence;
using TicTacToe.Infrastructure.Services;

namespace TicTacToe.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddDbContext<TicTacToeDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(TicTacToeDbContext).Assembly.FullName)));

            services.AddDefaultIdentity<TicTacToeUser>()
                .AddEntityFrameworkStores<TicTacToeDbContext>();

            if (environment.IsEnvironment("Test"))
            {
                services.AddIdentityServer()
                    .AddApiAuthorization<TicTacToeUser, TicTacToeDbContext>(options =>
                    {
                        options.Clients.Add(new Client
                        {
                            ClientId = "TicTacToe.IntegrationTests",
                            AllowedGrantTypes = { GrantType.ResourceOwnerPassword },
                            ClientSecrets = { new Secret("secret".Sha256()) },
                            AllowedScopes = { "TicTacToe.WebUIAPI", "openid", "profile" }
                        });
                    }).AddTestUsers(new List<TestUser>
                    {
                        new TestUser
                        {
                            SubjectId = "7d02cd1c-dfa3-40da-9f39-ca205da84e68",
                            Username = "test@test.com",
                            Password = "TicTacToe!",
                            Claims = new List<Claim>
                            {
                                new Claim(JwtClaimTypes.Email, "test@test.com")
                            }
                        }
                    });
            }
            else
            {
                services.AddIdentityServer()
                    .AddApiAuthorization<TicTacToeUser, TicTacToeDbContext>();

                services.AddTransient<IDateTime, DateTimeService>();
            }

            services.AddAuthentication()
                .AddIdentityServerJwt();

            return services;
        }
    }
}
