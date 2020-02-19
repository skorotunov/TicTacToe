namespace TicTacToe.Domain.Entities
{
    /// <summary>
    /// Entity that represents many-to-many relation between WinCondition and Tile.
    /// </summary>
    public class WinConditionTile
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        public byte Id { get; set; }

        /// <summary>
        /// Foreign key to the WinCondition entity.
        /// </summary>
        public byte WinConditionId { get; set; }

        public WinCondition WinCondition { get; set; }

        /// <summary>
        /// Foreign key to the Tile entity.
        /// </summary>
        public byte TileId { get; set; }

        public Tile Tile { get; set; }
    }
}
