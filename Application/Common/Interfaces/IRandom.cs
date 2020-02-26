namespace TicTacToe.Application.Common.Interfaces
{
    /// <summary>
    /// Service which encapsulates creation of the random number.
    /// </summary>
    public interface IRandom
    {
        /// <summary>
        /// Get random number in a range.
        /// </summary>
        /// <param name="minValue">Min value of the outcome.</param>
        /// <param name="maxValue">Max value of the outcome.</param>
        /// <returns>Created random number.</returns>
        int Next(int minValue, int maxValue);
    }
}
