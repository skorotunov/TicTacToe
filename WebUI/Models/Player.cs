using System.Collections.Generic;

namespace TicTacToe.WebUI.Models
{
    /// <summary>
    /// Entity that represents player in the system.
    /// </summary>
    public class Player
    {
        public Player(string id, string name)
        {
            Id = id;
            Name = name;
            ConnectionIds = new HashSet<string>();
            Actions = new List<SynchronizationAction>();
        }

        /// <summary>
        /// ID of the player. It is the same as ID of the user in the database.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of the player. It is the same as name of the user in the database.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of the group in which all conections of the two players that are in the game stored.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// ID of the game in the database if player is actually in the game.
        /// </summary>
        public int GameId { get; set; }

        /// <summary>
        /// Property which shows if player plays with X. It meand that this player started game first.
        /// </summary>
        public bool IsCrossPlayer { get; set; }

        /// <summary>
        /// All current player connections. e.g. different browser tabs, etc.
        /// </summary>
        public HashSet<string> ConnectionIds { get; set; }

        /// <summary>
        /// Collection of the actions done by the user. Used to sync UI of all connections even if one connection was added in the middle of some action.
        /// </summary>
        public List<SynchronizationAction> Actions { get; set; }
    }
}
