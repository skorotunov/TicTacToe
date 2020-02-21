using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TicTacToe.Application.Common.Interfaces;

namespace TicTacToe.WebUI.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ICurrentUserService currentUserService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ChatHub(ICurrentUserService currentUserService, IHttpContextAccessor httpContextAccessor)
        {
            this.currentUserService = currentUserService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task Send(string message, string userName)
        {
            var test = httpContextAccessor.HttpContext.User.Identity.Name;
            var test2 = currentUserService.UserId;

            await Clients.All.SendAsync("Receive", message, userName);
        }

        [Authorize(Roles = "admin")]
        public async Task Notify(string message, string userName)
        {
            await Clients.All.SendAsync("Receive", message, userName);
        }
    }
}
