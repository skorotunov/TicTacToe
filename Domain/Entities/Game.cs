using System;
using System.Collections.Generic;
using TicTacToe.Domain.Enums;

namespace TicTacToe.Domain.Entities
{
    /// <summary>
    /// Entity that represents Tic-Tac-Toe game process.
    /// </summary>
    public class Game
    {
        public Game()
        {
            Result = GameResult.InProgress;

            // initialize collections with default values in order to prevent null checks
            CrossPlayerGameTiles = new List<CrossPlayerGameTile>();
            CrossPlayerGameWinConditions = new List<CrossPlayerGameWinCondition>();
            NoughtPlayerGameTiles = new List<NoughtPlayerGameTile>();
            NoughtPlayerGameWinConditions = new List<NoughtPlayerGameWinCondition>();
        }

        /// <summary>
        /// Primary key.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The date when game was staerted.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// ID of the TicTacToeUser that is playing with cross character.
        /// </summary>
        public string CrossPlayerId { get; set; }

        /// <summary>
        /// Tiles that were placed on the board by the TicTacToeUser that is playing with cross character.
        /// </summary>
        public IList<CrossPlayerGameTile> CrossPlayerGameTiles { get; set; }

        /// <summary>
        /// Current winning conditions of the TicTacToeUser that is playing with cross character.
        /// </summary>
        public IList<CrossPlayerGameWinCondition> CrossPlayerGameWinConditions { get; set; }

        /// <summary>
        /// ID of the TicTacToeUser that is playing with nought character.
        /// </summary>
        public string NoughtPlayerId { get; set; }

        /// <summary>
        /// Tiles that were placed on the board by the TicTacToeUser that is playing with nought character.
        /// </summary>
        public IList<NoughtPlayerGameTile> NoughtPlayerGameTiles { get; set; }

        /// <summary>
        /// Current winning conditions of the TicTacToeUser that is playing with nought character.
        /// </summary>
        public IList<NoughtPlayerGameWinCondition> NoughtPlayerGameWinConditions { get; set; }

        /// <summary>
        /// Result of the game for the player which played with cross character - win, loss, draw, still in progress
        /// </summary>
        public GameResult Result { get; set; }
    }
}
