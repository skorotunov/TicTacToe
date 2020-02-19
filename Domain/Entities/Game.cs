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
            CrossPlayerGameTiles = new List<GameTile>();
            CrossPlayerGameWinConditions = new List<GameWinCondition>();
            NoughtPlayerGameTiles = new List<GameTile>();
            NoughtPlayerGameWinConditions = new List<GameWinCondition>();
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
        public IList<GameTile> CrossPlayerGameTiles { get; set; }

        /// <summary>
        /// Current winning conditions of the TicTacToeUser that is playing with cross character.
        /// </summary>
        public IList<GameWinCondition> CrossPlayerGameWinConditions { get; set; }

        /// <summary>
        /// ID of the TicTacToeUser that is playing with nought character.
        /// </summary>
        public string NoughtPlayerId { get; set; }

        /// <summary>
        /// Tiles that were placed on the board by the TicTacToeUser that is playing with nought character.
        /// </summary>
        public IList<GameTile> NoughtPlayerGameTiles { get; set; }

        /// <summary>
        /// Current winning conditions of the TicTacToeUser that is playing with nought character.
        /// </summary>
        public IList<GameWinCondition> NoughtPlayerGameWinConditions { get; set; }

        /// <summary>
        /// Result of the game for the player which played with cross character - win, loss, draw, still in progress
        /// </summary>
        public GameResult Result { get; set; }
    }
}
