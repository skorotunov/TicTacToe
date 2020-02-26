using System;

namespace TicTacToe.Application.Common.Interfaces
{
    /// <summary>
    /// Service which encapsulates creation of the Guid.
    /// </summary>
    public interface IGuid
    {
        /// <summary>
        /// Create new Guid.
        /// </summary>
        /// <returns>Guid.</returns>
        Guid NewGuid();
    }
}
