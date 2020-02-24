using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using TicTacToe.Application.Common.Interfaces;
using TicTacToe.WebUI.Common;
using TicTacToe.WebUI.Models;

namespace TicTacToe.WebUI.Hubs
{
    [Authorize]
    public class TicTacToeHub : Hub
    {
        // every hub method call is executed on a new hub instance according to docs
        private static readonly object NotInGameLockObject = new object();
        private static readonly object InGameLockObject = new object();
        private static readonly SemaphoreLocker NotInGameLocker = new SemaphoreLocker();
        private static readonly SemaphoreLocker InGameLocker = new SemaphoreLocker();

        private static readonly ConcurrentDictionary<string, Player> PlayersNotInTheGame = new ConcurrentDictionary<string, Player>();
        private static readonly ConcurrentDictionary<string, Player> PlayersInTheGame = new ConcurrentDictionary<string, Player>();

        private readonly ICurrentUserService currentUserService;

        public TicTacToeHub(ICurrentUserService currentUserService)
        {
            this.currentUserService = currentUserService;
        }

        public override async Task OnConnectedAsync()
        {
            string playerId = currentUserService.UserId;
            string playerName = currentUserService.UserName;
            string connectionId = Context.ConnectionId;

            // check if connected player is in the game
            if (PlayersInTheGame.TryGetValue(playerId, out Player playerInTheGame))
            {
                lock (InGameLockObject)
                {
                    playerInTheGame.ConnectionIds.Add(connectionId);

                    // TODO: actions for connection to process
                }
            }
            else
            {
                // player not in the game, search in the collection
                Player playerNotInTheGame = PlayersNotInTheGame.GetOrAdd(playerId, _ => new Player(playerId, playerName));
                await NotInGameLocker.LockAsync(async () =>
                {
                    playerNotInTheGame.ConnectionIds.Add(connectionId);

                    // always broadcast this to show players list for the new connection
                    await Clients.Caller.SendAsync("PlayerConnectHandle", PlayersNotInTheGame.Where(x => x.Key != playerNotInTheGame.Id).Select(x => x.Value));

                    // check if this is a first connection of the user
                    if (playerNotInTheGame.ConnectionIds.Count == 1)
                    {
                        // only broadcast this info if this is the first connection of the user
                        await Clients.Others.SendAsync("PlayerFirstTimeConnectHandle", playerId, playerName);
                    }
                    else
                    {
                        // because this is not the first user's connection we want to sync UI
                        foreach (SynchronizationAction action in playerNotInTheGame.Actions)
                        {
                            switch (action.Parameters.Count())
                            {
                                case 0:
                                    await Clients.Caller.SendAsync(action.Name);
                                    break;
                                case 1:
                                    await Clients.Caller.SendAsync(action.Name, action.Parameters[0]);
                                    break;
                                case 2:
                                    await Clients.Caller.SendAsync(action.Name, action.Parameters[0], action.Parameters[1]);
                                    break;
                            }
                        }
                    }
                });
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string playerId = currentUserService.UserId;
            string playerName = currentUserService.UserName;
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
                await NotInGameLocker.LockAsync(async () =>
                {
                    playerNotInTheGame.ConnectionIds.RemoveWhere(x => x.Equals(connectionId));
                    if (!playerNotInTheGame.ConnectionIds.Any())
                    {
                        PlayersNotInTheGame.TryRemove(playerId, out Player _);

                        // only broadcast this info if this is the last connection of the user and the user actual is now disconnected from all connections
                        await Clients.Others.SendAsync("PlayerDisconnectHandle", playerId);
                    }
                });
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task OnPatientGamesFirstTimeOpen(string playerId)
        {
            if (PlayersNotInTheGame.TryGetValue(currentUserService.UserId, out Player currentPlayer))
            {
                lock (NotInGameLockObject)
                {
                    currentPlayer.Actions.Add(new SynchronizationAction("PatientGamesFirstTimeOpenHandle", new object[] { playerId }));
                }
            }

            await Clients.User(currentUserService.UserId).SendAsync("PatientGamesFirstTimeOpenHandle", playerId);
        }

        public async Task OnPatientGamesOpen(string playerId)
        {
            if (PlayersNotInTheGame.TryGetValue(currentUserService.UserId, out Player currentPlayer))
            {
                lock (NotInGameLockObject)
                {
                    currentPlayer.Actions.Add(new SynchronizationAction("PatientGamesFirstTimeOpenHandle", new object[] { playerId }));
                }
            }

            await Clients.User(currentUserService.UserId).SendAsync("PatientGamesOpenHandle", playerId);
        }

        public async Task OnPatientGamesClose(string playerId)
        {
            if (PlayersNotInTheGame.TryGetValue(currentUserService.UserId, out Player currentPlayer))
            {
                lock (NotInGameLockObject)
                {
                    currentPlayer.Actions.RemoveAll(x => x.Name == "PatientGamesFirstTimeOpenHandle" && x.Parameters.First().ToString() == playerId);
                }
            }

            await Clients.User(currentUserService.UserId).SendAsync("PatientGamesCloseHandle", playerId);
        }

        public async Task OnNewGameStartCaller(string playerId)
        {
            if (PlayersNotInTheGame.TryGetValue(currentUserService.UserId, out Player currentPlayer))
            {
                lock (NotInGameLockObject)
                {
                    currentPlayer.Actions.Add(new SynchronizationAction("NewGameStartCallerHandle", new object[] { playerId }));
                }
            }

            await Clients.User(currentUserService.UserId).SendAsync("NewGameStartCallerHandle", playerId);
        }

        public async Task OnNewGameStartReceiver(string playerId)
        {
            // in order to prevent setting UI in disabled state we need to specifically process exception case
            try
            {
                if (PlayersNotInTheGame.TryGetValue(playerId, out Player player))
                {
                    await NotInGameLocker.LockAsync(async () =>
                    {
                        // check if target player is waiting for another game
                        if (player.Actions.Where(x => x.Name == "NewGameStartCallerHandle").Any())
                        {
                            if (PlayersNotInTheGame.TryGetValue(currentUserService.UserId, out Player currentPlayer)
                             && !currentPlayer.Actions.Where(x => x.Name == "NewGameFailureCallerHandle").Any())
                            {
                                currentPlayer.Actions.Add(new SynchronizationAction("NewGameFailureCallerHandle"));
                            }

                            await Clients.User(currentUserService.UserId).SendAsync("NewGameFailureCallerHandle", playerId, $"{player.Name} is busy at the moment. Please try later.");
                        }
                        else
                        {
                            player.Actions.Add(new SynchronizationAction("NewGameStartReceiverHandle", new object[] { currentUserService.UserId, currentUserService.UserName }));
                            await Clients.User(playerId).SendAsync("NewGameStartReceiverHandle", currentUserService.UserId, currentUserService.UserName);
                        }
                    });
                }
            }
            catch
            {
                if (PlayersNotInTheGame.TryGetValue(currentUserService.UserId, out Player currentPlayer))
                {
                    lock (NotInGameLockObject)
                    {
                        currentPlayer.Actions.RemoveAll(x => x.Name == "NewGameStartCallerHandle" && x.Parameters.First().ToString() == playerId);
                        if (!currentPlayer.Actions.Where(x => x.Name == "NewGameFailureCallerHandle").Any())
                        {
                            currentPlayer.Actions.Add(new SynchronizationAction("NewGameFailureCallerHandle"));
                        }
                    }
                }

                await Clients.User(currentUserService.UserId).SendAsync("NewGameFailureCallerHandle", playerId, string.Empty);
                throw;
            }
        }

        /// <summary>
        /// Attended player has declined game offer. Therefore this action should be processed both on the caller and receiver sides.
        /// </summary>
        /// <param name="playerId">Id of the player who has ofered game. In the terms of current method - receiver.</param>
        /// <returns></returns>
        public async Task OnNewGameDecline(string playerId)
        {
            // process receiver
            if (PlayersNotInTheGame.TryGetValue(playerId, out Player receiver))
            {
                // try to add action for the receiver (this is required for correct work of multiple UI sessions.
                // For example - user sent game request to another player. Then he opens new browser tab. UI should display correct state - waiting for another player to respond.)
                lock (NotInGameLockObject)
                {
                    // add single NewGameFailureCallerHandle method that will enable UI for new user's connections
                    if (!receiver.Actions.Where(x => x.Name == "NewGameFailureCallerHandle").Any())
                    {
                        receiver.Actions.Add(new SynchronizationAction("NewGameFailureCallerHandle"));
                    }
                }

                // call specific NewGameFailureCallerHandle method to show alert for existing user's connections
                await Clients.User(receiver.Id).SendAsync("NewGameFailureCallerHandle", receiver.Id, $"{receiver.Name} does not want to play at the moment. Please try later.");
            }

            // process caller
            if (PlayersNotInTheGame.TryGetValue(currentUserService.UserId, out Player caller))
            {
                // try to add action for the caller
                lock (NotInGameLockObject)
                {
                    // remove NewGameStartReceiverHandle method instances aimed to receiver. They intended for opening modal windows
                    caller.Actions.RemoveAll(x => x.Name == "NewGameStartReceiverHandle" && x.Parameters.Contains(playerId));
                }

                // call method that will close open modal
                await Clients.User(caller.Id).SendAsync("NewGameDeclineHandle", playerId);
            }
        }
    }
}
