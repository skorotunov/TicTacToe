﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicTacToe.Application.Common.Interfaces;
using TicTacToe.Infrastructure.Identity;
using TicTacToe.Infrastructure.Persistence;
using TicTacToe.Infrastructure.Services;

namespace TicTacToe.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TicTacToeDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(TicTacToeDbContext).Assembly.FullName)));

            services.AddDefaultIdentity<TicTacToeUser>()
                .AddEntityFrameworkStores<TicTacToeDbContext>();

            services.AddTransient<IDateTime, DateTimeService>();

            return services;
        }
    }
}
