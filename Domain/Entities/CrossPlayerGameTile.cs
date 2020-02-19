namespace TicTacToe.Domain.Entities
{
    /// <summary>
    /// Entity that represents many-to-many relation between Game and Tile for cross player.
    /// </summary>
    public class CrossPlayerGameTile
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
        /// Foreign key to the Tile entity.
        /// </summary>
        public byte TileId { get; set; }

        public Tile Tile { get; set; }
    }
}
