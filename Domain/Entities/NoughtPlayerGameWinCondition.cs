namespace TicTacToe.Domain.Entities
{
    /// <summary>
    /// Entity that represents many-to-many relation between Game and WinCondition for nought player.
    /// </summary>
    public class NoughtPlayerGameWinCondition
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to the Game entity.
        /// </summary>
        public int GameId { get; set; }

        public Game Game { get; set; }

        /// <summary>
        /// Foreign key to the WinCondition entity.
        /// </summary>
        public byte WinConditionId { get; set; }

        public WinCondition WinCondition { get; set; }
    }
}
