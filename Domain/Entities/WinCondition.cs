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
            CrossPlayerGameWinConditions = new List<CrossPlayerGameWinCondition>();
            NoughtPlayerGameWinConditions = new List<NoughtPlayerGameWinCondition>();
        }

        /// <summary>
        /// Primary key.
        /// </summary>
        public byte Id { get; set; }

        public IList<WinConditionTile> WinConditionTiles { get; private set; }

        /// <summary>
        /// Current winning conditions of the TicTacToeUser that is playing with cross character.
        /// </summary>
        public IList<CrossPlayerGameWinCondition> CrossPlayerGameWinConditions { get; set; }

        /// <summary>
        /// Current winning conditions of the TicTacToeUser that is playing with nought character.
        /// </summary>
        public IList<NoughtPlayerGameWinCondition> NoughtPlayerGameWinConditions { get; set; }
    }
}
