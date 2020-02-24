﻿using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TicTacToe.Application.Common.Interfaces;

namespace TicTacToe.WebUI.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            UserName = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
        }

        public string UserId { get; }

        public string UserName { get; }
    }
}
