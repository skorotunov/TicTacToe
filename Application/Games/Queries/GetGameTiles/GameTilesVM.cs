using System.Collections.Generic;

namespace TicTacToe.Application.Games.Queries.GetGameTiles
{
    /// <summary>
    /// Output get game tiles data object.
    /// </summary>
    public class GameTilesVM
    {
        /// <summary>
        /// Return value which shows if opponent playes with X (started first).
        /// </summary>
        public bool IsOpponentCrossPlayer { get; set; }

        public IList<CrossPlayerTileDTO> CrossPlayerTiles { get; set; }

        public IList<NoughtPlayerTileDTO> NoughtPlayerTiles { get; set; }
    }
}
