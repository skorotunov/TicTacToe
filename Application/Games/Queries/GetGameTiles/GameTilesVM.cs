using System.Collections.Generic;

namespace TicTacToe.Application.Games.Queries.GetGameTiles
{
    public class GameTilesVM
    {
        public IList<CrossPlayerTileDTO> CrossPlayerTiles { get; set; }

        public IList<NoughtPlayerTileDTO> NoughtPlayerTiles { get; set; }
    }
}
