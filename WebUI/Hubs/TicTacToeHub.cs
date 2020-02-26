using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToe.Application.Common.Interfaces;
using TicTacToe.Application.Games.Commands.CheckAndUpdateGameWinConditions;
using TicTacToe.Domain.Enums;
using TicTacToe.WebUI.Models;

namespace TicTacToe.WebUI.Hubs
{
    [Authorize]
    public class TicTacToeHub : Hub
    {
        // every hub method call is executed on a new hub instance according to docs
        private static readonly object LockObject = new object();

        private readonly ICurrentUserService currentUserService;
        private readonly IGuid guid;
        private readonly IRandom random;
        private readonly IMediator mediator;

        public TicTacToeHub(ICurrentUserService currentUserService, IGuid guid, IRandom random, IMediator mediator)
        {
            this.currentUserService = currentUserService;
            this.guid = guid;
            this.random = random;
            this.mediator = mediator;
        }

        public override async Task OnConnectedAsync()
        {
            var tasks = new List<Task>();
            var actionTasks = new List<Task>();
            string playerId = currentUserService.UserId;
            string playerName = currentUserService.UserName;
            string connectionId = Context.ConnectionId;

            // check if connected player is in the game
            if (PlayersCollection.InTheGamePlayers.TryGetValue(playerId, out Player playerInTheGame))
            {
                lock (LockObject)
                {
                    playerInTheGame.ConnectionIds.Add(connectionId);
                    tasks.Add(Groups.AddToGroupAsync(connectionId, playerInTheGame.GroupName));

                    // always broadcast this to show players list for the new connection
                    var availablePlayers = PlayersCollection.AvailablePlayers.Select(x => x.Value).ToList();
                    tasks.Add(Clients.Caller.SendAsync("PlayerConnectHandle", availablePlayers));

                    // find opponent user
                    Player opponent = GetOpponent(playerInTheGame.Id, playerInTheGame.GroupName);
                    if (opponent != null)
                    {
                        tasks.Add(Clients.Caller.SendAsync("PlayerInGameConnectHandle", opponent));
                    }
                }
            }
            else
            {
                // player not in the game, search in the collection
                Player playerNotInTheGame = PlayersCollection.AvailablePlayers.GetOrAdd(playerId, _ => new Player(playerId, playerName));
                lock (LockObject)
                {
                    playerNotInTheGame.ConnectionIds.Add(connectionId);

                    // always broadcast this to show players list for the new connection
                    var availablePlayers = PlayersCollection.AvailablePlayers.Where(x => x.Key != playerNotInTheGame.Id).Select(x => x.Value).ToList();
                    tasks.Add(Clients.Caller.SendAsync("PlayerConnectHandle", availablePlayers));

                    // check if this is a first connection of the user
                    if (playerNotInTheGame.ConnectionIds.Count == 1)
                    {
                        // only broadcast this info if this is the first connection of the user
                        tasks.Add(Clients.Others.SendAsync("PlayerFirstTimeConnectHandle", playerNotInTheGame.Id, playerNotInTheGame.Name));
                    }
                    else
                    {
                        // because this is not the first user's connection we want to sync UI
                        foreach (SynchronizationAction action in playerNotInTheGame.Actions)
                        {
                            // due to the restrictions of the ClientProxyExtensions signature and without desire to use reflection
                            // we need to store parameters in the array and pass as separete items, not as a collection (up to 4 possible parameters)
                            switch (action.Parameters.Count())
                            {
                                case 0:
                                    actionTasks.Add(Clients.Caller.SendAsync(action.Name));
                                    break;
                                case 1:
                                    actionTasks.Add(Clients.Caller.SendAsync(action.Name, action.Parameters[0]));
                                    break;
                                case 2:
                                    actionTasks.Add(Clients.Caller.SendAsync(action.Name, action.Parameters[0], action.Parameters[1]));
                                    break;
                                case 3:
                                    actionTasks.Add(Clients.Caller.SendAsync(action.Name, action.Parameters[0], action.Parameters[1], action.Parameters[2]));
                                    break;
                                case 4:
                                    actionTasks.Add(Clients.Caller.SendAsync(action.Name, action.Parameters[0], action.Parameters[1], action.Parameters[2], action.Parameters[3]));
                                    break;
                            }
                        }
                    }
                }
            }

            await Task.WhenAll(tasks);

            // actions should be executed in specific order so WhenAll is not suitable
            foreach (Task actionTask in actionTasks)
            {
                await actionTask;
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var tasks = new List<Task>();
            string playerId = currentUserService.UserId;
            string playerName = currentUserService.UserName;
            string connectionId = Context.ConnectionId;

            // check if connected player is in the game
            if (PlayersCollection.InTheGamePlayers.TryGetValue(playerId, out Player playerInTheGame))
            {
                lock (LockObject)
                {
                    playerInTheGame.ConnectionIds.RemoveWhere(x => x.Equals(connectionId));
                    tasks.Add(Groups.RemoveFromGroupAsync(connectionId, playerInTheGame.GroupName));

                    // check that last player connection was disconnected. In that case we need to inform opponent and all other players
                    if (!playerInTheGame.ConnectionIds.Any())
                    {
                        tasks.Add(Clients.Group(playerInTheGame.GroupName).SendAsync("PlayerInGameDisconnectHandle"));

                        // find opponent user
                        Player opponent = GetOpponent(playerInTheGame.Id, playerInTheGame.GroupName);
                        if (opponent != null)
                        {
                            // informe other players that now opponent is available
                            tasks.Add(Clients.AllExcept(opponent.ConnectionIds.ToList()).SendAsync("PlayerFirstTimeConnectHandle", opponent.Id, opponent.Name));

                            // move opponent to available players list
                            if (PlayersCollection.InTheGamePlayers.TryRemove(opponent.Id, out Player _))
                            {
                                PlayersCollection.AvailablePlayers.TryAdd(opponent.Id, opponent);
                            }
                        }

                        PlayersCollection.InTheGamePlayers.TryRemove(playerInTheGame.Id, out Player _);
                    }
                }
            }
            else if (PlayersCollection.AvailablePlayers.TryGetValue(playerId, out Player playerNotInTheGame))
            {
                lock (LockObject)
                {
                    playerNotInTheGame.ConnectionIds.RemoveWhere(x => x.Equals(connectionId));
                    if (!playerNotInTheGame.ConnectionIds.Any())
                    {
                        if (PlayersCollection.AvailablePlayers.TryRemove(playerNotInTheGame.Id, out Player _))
                        {
                            // only broadcast this info if this is the last connection of the user and the user actual is now disconnected from all connections
                            tasks.Add(Clients.Others.SendAsync("PlayerDisconnectHandle", playerNotInTheGame.Id));
                        }
                    }
                }
            }

            await Task.WhenAll(tasks);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task OnPlayerGamesFirstTimeOpen(string playerId)
        {
            if (PlayersCollection.AvailablePlayers.TryGetValue(currentUserService.UserId, out Player caller))
            {
                lock (LockObject)
                {
                    caller.Actions.Add(new SynchronizationAction("PlayerGamesFirstTimeOpenHandle", new string[] { playerId }));
                }

                await Clients.User(currentUserService.UserId).SendAsync("PlayerGamesFirstTimeOpenHandle", playerId);
            }
        }

        public async Task OnPlayerGamesOpen(string playerId)
        {
            if (PlayersCollection.AvailablePlayers.TryGetValue(currentUserService.UserId, out Player caller))
            {
                lock (LockObject)
                {
                    caller.Actions.Add(new SynchronizationAction("PlayerGamesFirstTimeOpenHandle", new string[] { playerId }));
                }

                await Clients.User(currentUserService.UserId).SendAsync("PlayerGamesOpenHandle", playerId);
            }
        }

        public async Task OnPlayerGamesClose(string playerId)
        {
            if (PlayersCollection.AvailablePlayers.TryGetValue(currentUserService.UserId, out Player caller))
            {
                lock (LockObject)
                {
                    caller.Actions.RemoveAll(x => x.Name == "PlayerGamesFirstTimeOpenHandle" && x.Parameters.First() == playerId);
                }

                await Clients.User(currentUserService.UserId).SendAsync("PlayerGamesCloseHandle", playerId);
            }
        }

        public async Task OnNewGameStartCaller(string playerId, string gameId)
        {
            if (PlayersCollection.AvailablePlayers.TryGetValue(currentUserService.UserId, out Player caller))
            {
                lock (LockObject)
                {
                    caller.Actions.Add(new SynchronizationAction("NewGameStartCallerHandle", new string[] { playerId, gameId }));
                }

                await Clients.User(currentUserService.UserId).SendAsync("NewGameStartCallerHandle", playerId, gameId);
            }
        }

        public async Task OnNewGameStartReceiver(string playerId, string gameId, string gameDate)
        {
            if (PlayersCollection.AvailablePlayers.TryGetValue(currentUserService.UserId, out Player caller)
             && PlayersCollection.AvailablePlayers.TryGetValue(playerId, out Player receiver))
            {
                // in order to prevent setting UI in disabled state we need to specifically process exception case
                try
                {
                    Task task;
                    lock (LockObject)
                    {
                        // check if target player is waiting for another game
                        if (receiver.Actions.LastOrDefault()?.Name == "NewGameStartCallerHandle")
                        {
                            caller.Actions.RemoveAll(x => x.Name == "NewGameFailureHandle");
                            caller.Actions.Add(new SynchronizationAction("NewGameFailureHandle"));
                            task = Clients.User(caller.Id).SendAsync("NewGameFailureHandle", receiver.Id, $"{receiver.Name} is busy at the moment. Please try later.");
                        }
                        else
                        {
                            receiver.Actions.Add(new SynchronizationAction("NewGameStartReceiverHandle", new string[] { caller.Id, caller.Name, gameId, gameDate }));
                            task = Clients.User(receiver.Id).SendAsync("NewGameStartReceiverHandle", caller.Id, caller.Name, gameId, gameDate);
                        }
                    }

                    await task;
                }
                catch
                {
                    lock (LockObject)
                    {
                        caller.Actions.RemoveAll(x => x.Name == "NewGameStartCallerHandle" && x.Parameters.First() == receiver.Id);
                        caller.Actions.RemoveAll(x => x.Name == "NewGameFailureHandle");
                        caller.Actions.Add(new SynchronizationAction("NewGameFailureHandle"));
                    }

                    await Clients.User(caller.Id).SendAsync("NewGameFailureHandle", receiver.Id, string.Empty);
                    throw;
                }
            }
        }

        /// <summary>
        /// Attended player has declined game offer. Therefore this action should be processed both on the caller and receiver sides.
        /// </summary>
        /// <param name="playerId">Id of the player who has ofered game. In the terms of current method - receiver.</param>
        /// <returns></returns>
        public async Task OnNewGameDecline(string playerId)
        {
            if (PlayersCollection.AvailablePlayers.TryGetValue(currentUserService.UserId, out Player caller)
             && PlayersCollection.AvailablePlayers.TryGetValue(playerId, out Player receiver))
            {
                var tasks = new List<Task>();

                // try to add action for the caller and receiver (this is required for correct work of multiple UI sessions.
                // For example - user sent game request to another player. Then he opens new browser tab. UI should display correct state - waiting for another player to respond.)
                lock (LockObject)
                {
                    // remove NewGameStartReceiverHandle method instances aimed to receiver. They intended for opening modal windows
                    caller.Actions.RemoveAll(x => x.Name == "NewGameStartReceiverHandle" && x.Parameters.First() == receiver.Id);

                    // add single NewGameFailureHandle method at the end of the actions list. It will enable UI for new user's connections
                    receiver.Actions.RemoveAll(x => x.Name == "NewGameFailureHandle");
                    receiver.Actions.Add(new SynchronizationAction("NewGameFailureHandle"));

                    // call method that will close open modal
                    tasks.Add(Clients.User(caller.Id).SendAsync("NewGameDeclineHandle", receiver.Id));

                    // call specific NewGameFailureHandle method to show alert for existing user's connections
                    tasks.Add(Clients.User(receiver.Id).SendAsync("NewGameFailureHandle", caller.Id, $"{caller.Name} does not want to play at the moment. Please try later."));
                }

                await Task.WhenAll(tasks);
            }
        }

        /// <summary>
        /// Attended player has accepted game offer. Therefore this action should be processed both on the caller and receiver sides.
        /// Also all other players that are waiting for this player should be informed.
        /// </summary>
        /// <param name="playerId">Id of the player who has ofered game. In the terms of current method - receiver.</param>
        /// /// <param name="playerId">Id of the game to continue. If it is new game - 0.</param>
        /// <returns></returns>
        public async Task OnNewGameAccept(string playerId, int gameId)
        {
            if (PlayersCollection.AvailablePlayers.TryGetValue(currentUserService.UserId, out Player caller)
             && PlayersCollection.AvailablePlayers.TryGetValue(playerId, out Player receiver))
            {
                var tasks = new List<Task>();

                // try to add action for the users that are waiting for new game response (this is required for correct work of multiple UI sessions.
                // For example - user sent game request to another player. Then he opens new browser tab. UI should display correct state - waiting for another player to respond.)
                lock (LockObject)
                {
                    // process caller
                    // get all playerIds of the other players that are waiting for the response of the new game with caller.
                    // There should not be many players waiting for one specific player. Therefore, regular foreach should be more efficient than Parallel
                    foreach (SynchronizationAction action in caller.Actions.Where(x => x.Name == "NewGameStartReceiverHandle" && x.Parameters.First() != receiver.Id))
                    {
                        if (PlayersCollection.AvailablePlayers.TryGetValue(action.Parameters.First(), out Player waitingPlayer))
                        {
                            // add single NewGameFailureHandle method at the end of the actions list. It will enable UI for new user's connections
                            waitingPlayer.Actions.RemoveAll(x => x.Name == "NewGameFailureHandle");
                            waitingPlayer.Actions.Add(new SynchronizationAction("NewGameFailureHandle"));

                            // add specific NewGameFailureHandle method to show alert for existing user's connections
                            tasks.Add(Clients.User(waitingPlayer.Id).SendAsync("NewGameFailureHandle", caller.Id, $"Sorry, {caller.Name} has already started another game."));
                        }
                    }

                    // move to the game collection
                    if (PlayersCollection.AvailablePlayers.TryRemove(caller.Id, out Player _))
                    {
                        PlayersCollection.InTheGamePlayers.TryAdd(caller.Id, caller);
                        caller.Actions = new List<SynchronizationAction>();
                    }

                    if (PlayersCollection.AvailablePlayers.TryRemove(receiver.Id, out Player _))
                    {
                        PlayersCollection.InTheGamePlayers.TryAdd(receiver.Id, receiver);
                        receiver.Actions = new List<SynchronizationAction>();
                    }

                    // create game group
                    string guidString = guid.NewGuid().ToString("d");
                    caller.GroupName = guidString;
                    receiver.GroupName = guidString;

                    if (gameId > 0)
                    {
                        caller.GameId = gameId;
                        receiver.GameId = gameId;
                    }
                    else
                    {
                        // coin Toss
                        if (random.Next(0, 2) == 0)
                        {
                            // caller will make the first move and will play with cross
                            caller.IsCrossPlayer = true;
                            receiver.IsCrossPlayer = false;
                        }
                        else
                        {
                            // receiver make the first move and will play with cross
                            caller.IsCrossPlayer = false;
                            receiver.IsCrossPlayer = true;
                        }
                    }

                    foreach (string callerConnectionId in caller.ConnectionIds)
                    {
                        tasks.Add(Groups.AddToGroupAsync(callerConnectionId, guidString));
                    }

                    foreach (string receiverConnectionId in receiver.ConnectionIds)
                    {
                        tasks.Add(Groups.AddToGroupAsync(receiverConnectionId, guidString));
                    }

                    // inform others that they need to hide those players
                    foreach (KeyValuePair<string, Player> availablePlayer in PlayersCollection.AvailablePlayers)
                    {
                        tasks.Add(Clients.User(availablePlayer.Key).SendAsync("NewGameAcceptOthersHandle", receiver.Id));
                        tasks.Add(Clients.User(availablePlayer.Key).SendAsync("NewGameAcceptOthersHandle", caller.Id));
                    }

                    // add methods that will handler start of the new game
                    tasks.Add(Clients.User(caller.Id).SendAsync("NewGameAcceptCallerHandle", receiver));
                    tasks.Add(Clients.User(receiver.Id).SendAsync("NewGameAcceptReceiverHandle", caller));
                }

                await Task.WhenAll(tasks);
            }
        }

        /// <summary>
        /// New game was created. Therefore id of this game should be sent to the opponent.
        /// </summary>
        /// <param name="gameId">Id of the created game</param>
        /// <returns></returns>
        public async Task OnNewGameCreate(int gameId, string opponentId)
        {
            if (PlayersCollection.InTheGamePlayers.TryGetValue(currentUserService.UserId, out Player caller)
             && PlayersCollection.InTheGamePlayers.TryGetValue(opponentId, out Player opponent))
            {
                lock (LockObject)
                {
                    // set gameId for both players
                    caller.GameId = gameId;
                    opponent.GameId = gameId;
                }

                await Clients.OthersInGroup(caller.GroupName).SendAsync("NewGameCreateHandle", gameId);
            }
        }

        /// <summary>
        /// Turn was made by one of the players
        /// </summary>
        /// <param name="x">X coordinate of the cell</param>
        /// <param name="y">Y coordinate of the cell</param>
        /// <returns></returns>
        public async Task OnTurnComplete(int gameId, string opponentId, byte x, byte y)
        {
            var tasks = new List<Task>();

            // register turn of the game and check win conditions
            GameResult result = await mediator.Send(new CheckAndUpdateGameWinConditionsCommand(gameId));

            if (PlayersCollection.InTheGamePlayers.TryGetValue(currentUserService.UserId, out Player caller)
             && PlayersCollection.InTheGamePlayers.TryGetValue(opponentId, out Player opponent))
            {
                lock (LockObject)
                {
                    // add handle methods for caller and receiver connections
                    tasks.Add(Clients.GroupExcept(caller.GroupName, caller.ConnectionIds.ToList()).SendAsync("TurnCompleteReceiverHandle", x, y));
                    tasks.Add(Clients.GroupExcept(opponent.GroupName, opponent.ConnectionIds.ToList()).SendAsync("TurnCompleteCallerHandle", x, y));

                    // check if game has ended
                    if (result != GameResult.Active)
                    {
                        tasks.Add(Clients.Group(caller.GroupName).SendAsync("GameEndHandle", result.ToString()));

                        // move to the available collection
                        if (PlayersCollection.InTheGamePlayers.TryRemove(caller.Id, out Player _))
                        {
                            PlayersCollection.AvailablePlayers.TryAdd(caller.Id, caller);
                        }

                        if (PlayersCollection.InTheGamePlayers.TryRemove(opponent.Id, out Player _))
                        {
                            PlayersCollection.AvailablePlayers.TryAdd(opponent.Id, opponent);
                        }

                        // cleanup game data
                        caller.GroupName = null;
                        caller.IsCrossPlayer = false;
                        opponent.GroupName = null;
                        opponent.IsCrossPlayer = false;

                        // inform other players that game participants are now available for the game
                        tasks.Add(Clients.AllExcept(caller.ConnectionIds.ToList()).SendAsync("PlayerFirstTimeConnectHandle", caller.Id, caller.Name));
                        tasks.Add(Clients.AllExcept(opponent.ConnectionIds.ToList()).SendAsync("PlayerFirstTimeConnectHandle", opponent.Id, opponent.Name));
                    }
                }

                await Task.WhenAll(tasks);
            }
        }

        /// <summary>
        /// Get player's opponent entity
        /// </summary>
        /// <param name="playerId">player in the game whose opponent needs to be found</param>
        /// <param name="groupName">name of the players group</param>
        /// <returns></returns>
        private Player GetOpponent(string playerId, string groupName)
        {
            return PlayersCollection.InTheGamePlayers.FirstOrDefault(x => x.Key != playerId && x.Value.GroupName == groupName).Value;
        }
    }
}
