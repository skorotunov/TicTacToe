using System.Collections.Concurrent;

namespace TicTacToe.WebUI.Models
{
    /// <summary>
    /// Object that contains two player collections. ConcurrentDictionary was choosed as a store to use async abilities of the SignalR.
    /// Later with hundreds or thousands of users that can be moved to persistence in order to minimize memory usage.
    /// But currently, use of the database seems to be not optimal from perfomance point of view. 
    /// </summary>
    public static class PlayersCollection
    {
        /// <summary>
        /// Available players collection. All players that are not in the game are stored here.
        /// </summary>
        public static readonly ConcurrentDictionary<string, Player> AvailablePlayers = new ConcurrentDictionary<string, Player>();

        /// <summary>
        /// Players that are in the game now collection. All players that are in the game are stored here.
        /// </summary>
        public static readonly ConcurrentDictionary<string, Player> InTheGamePlayers = new ConcurrentDictionary<string, Player>();
    }
}
