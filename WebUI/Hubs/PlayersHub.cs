using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace TicTacToe.WebUI.Hubs
{
    public class PlayersHub : Hub
    {
        public async Task Echo(string message)
        {
            await Clients.All.SendAsync("Receive", message);
        }
    }
}
