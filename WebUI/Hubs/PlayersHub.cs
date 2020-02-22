using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToe.Application.Common.Interfaces;
using TicTacToe.WebUI.Models;

namespace TicTacToe.WebUI.Hubs
{
    [Authorize]
    public class PlayersHub : Hub
    {
        // static fields because every hub method call is executed on a new hub instance according to docs
        private static readonly object NotInGameLockObject = new object();
        private static readonly object InGameLockObject = new object();

        private static readonly ConcurrentDictionary<string, Player> PlayersNotInTheGame = new ConcurrentDictionary<string, Player>();
        private static readonly ConcurrentDictionary<string, Player> PlayersInTheGame = new ConcurrentDictionary<string, Player>();

        private readonly ICurrentUserService currentUserService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public PlayersHub(ICurrentUserService currentUserService, IHttpContextAccessor httpContextAccessor)
        {
            this.currentUserService = currentUserService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public override async Task OnConnectedAsync()
        {
            bool sendPlayerConnectedCallback = false;
            bool sendPlayerConnectedSelfCallback = false;
            IEnumerable<Player> values = null;
            string playerId = currentUserService.UserId;
            string playerName = httpContextAccessor.HttpContext.User.Identity.Name;
            string connectionId = Context.ConnectionId;

            // check if connected player is in the game
            if (PlayersInTheGame.TryGetValue(playerId, out Player playerInTheGame))
            {
                lock (InGameLockObject)
                {
                    playerInTheGame.ConnectionIds.Add(connectionId);
                }
            }
            else
            {
                // player not in the game, search in collection
                Player playerNotInTheGame = PlayersNotInTheGame.GetOrAdd(playerId, _ => new Player(playerId, playerName));
                lock (NotInGameLockObject)
                {
                    playerNotInTheGame.ConnectionIds.Add(connectionId);

                    // always broadcast this to show players list for the new connection
                    sendPlayerConnectedSelfCallback = true;
                    values = PlayersNotInTheGame.Where(x => x.Key != playerNotInTheGame.Id).Select(x => x.Value);

                    // only broadcast this info if this is the first connection of the user
                    sendPlayerConnectedCallback = playerNotInTheGame.ConnectionIds.Count == 1;
                }

                if (sendPlayerConnectedSelfCallback)
                {
                    await Clients.Caller.SendAsync("PlayerConnectedSelf", values);
                }

                if (sendPlayerConnectedCallback)
                {
                    await Clients.Others.SendAsync("PlayerConnected", playerId, playerName);
                }
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            bool sendCallback = false;
            string playerId = currentUserService.UserId;
            string playerName = httpContextAccessor.HttpContext.User.Identity.Name;
            string connectionId = Context.ConnectionId;

            // check if connected player is in the game
            if (PlayersInTheGame.TryGetValue(playerId, out Player playerInTheGame))
            {
                lock (InGameLockObject)
                {
                    playerInTheGame.ConnectionIds.RemoveWhere(x => x.Equals(connectionId));
                    if (!playerInTheGame.ConnectionIds.Any())
                    {
                        PlayersInTheGame.TryRemove(playerId, out Player _);

                        // TODO: inform another player that this player has left the game
                    }
                }
            }
            else if (PlayersNotInTheGame.TryGetValue(playerId, out Player playerNotInTheGame))
            {
                lock (NotInGameLockObject)
                {
                    playerNotInTheGame.ConnectionIds.RemoveWhere(x => x.Equals(connectionId));
                    if (!playerNotInTheGame.ConnectionIds.Any())
                    {
                        PlayersNotInTheGame.TryRemove(playerId, out Player _);

                        // only broadcast this info if this is the last connection of the user and the user actual is now disconnected from all connections
                        sendCallback = true;
                    }
                }

                if (sendCallback)
                {
                    await Clients.Others.SendAsync("PlayerDisconnected", playerId);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
