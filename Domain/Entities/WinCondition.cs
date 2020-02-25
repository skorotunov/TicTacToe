using System.Collections.Generic;

namespace TicTacToe.Domain.Entities
{
    /// <summary>
    /// Entity that represents one of the possible winning conditions in the game.
    /// </summary>
    public class WinCondition
    {
        public WinCondition()
        {
            // initialize collections with default values in order to prevent null checks
            WinConditionTiles = new List<WinConditionTile>();
        }

        /// <summary>
        /// Primary key.
        /// </summary>
        public byte Id { get; set; }

        public IList<WinConditionTile> WinConditionTiles { get; private set; }
    }
}
