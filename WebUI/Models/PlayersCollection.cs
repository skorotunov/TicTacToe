using System.Collections.Concurrent;

namespace TicTacToe.WebUI.Models
{
    public static class PlayersCollection
    {
        public static readonly ConcurrentDictionary<string, Player> AvailablePlayers = new ConcurrentDictionary<string, Player>();
        public static readonly ConcurrentDictionary<string, Player> InTheGamePlayers = new ConcurrentDictionary<string, Player>();
    }
}
