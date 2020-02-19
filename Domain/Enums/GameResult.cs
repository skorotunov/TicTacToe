namespace TicTacToe.Domain.Enums
{
    /// <summary>
    /// Enumeration that describes result of the game.
    /// </summary>
    public enum GameResult
    {
        /// <summary>
        /// This indicates that the game is in progress.
        /// </summary>
        InProgress,

        /// <summary>
        /// This indicates that cross player has won the game.
        /// </summary>
        Win,

        /// <summary>
        /// This indicates that cross player has lost the game.
        /// </summary>
        Loss,

        /// <summary>
        /// This indicates that the game has ended in a draw.
        /// </summary>
        Draw
    }
}
