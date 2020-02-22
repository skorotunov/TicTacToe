using System.Collections.Generic;

namespace TicTacToe.Application.Players.Queries.GetPlayerGames
{
    public class PlayerGamesVM
    {
        public IList<GameDTO> Games { get; set; }
    }
}
