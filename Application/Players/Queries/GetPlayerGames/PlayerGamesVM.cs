using System.Collections.Generic;

namespace TicTacToe.Application.Players.Queries.GetPlayerGames
{
    /// <summary>
    /// Output get player games data object.
    /// </summary>
    public class PlayerGamesVM
    {
        /// <summary>
        /// All games that exist between two players.
        /// </summary>
        public IList<GameDTO> Games { get; set; }
    }
}
