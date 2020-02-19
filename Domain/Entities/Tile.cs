using System.Collections.Generic;

namespace TicTacToe.Domain.Entities
{
    /// <summary>
    /// Entity that represents single tile on the Tic-Tac-Toe board.
    /// </summary>
    public class Tile
    {
        public Tile()
        {
            // initialize collections with default values in order to prevent null checks
            CrossPlayerGameTiles = new List<CrossPlayerGameTile>();
            NoughtPlayerGameTiles = new List<NoughtPlayerGameTile>();
            WinConditionTiles = new List<WinConditionTile>();
        }

        /// <summary>
        /// Primary key.
        /// </summary>
        public byte Id { get; set; }

        /// <summary>
        /// X coordinate of the tile on the board.
        /// </summary>
        public byte X { get; set; }

        /// <summary>
        /// Y coordinate of the tile on the board.
        /// </summary>
        public byte Y { get; set; }

        public IList<CrossPlayerGameTile> CrossPlayerGameTiles { get; private set; }

        public IList<NoughtPlayerGameTile> NoughtPlayerGameTiles { get; private set; }

        public IList<WinConditionTile> WinConditionTiles { get; private set; }
    }
}
