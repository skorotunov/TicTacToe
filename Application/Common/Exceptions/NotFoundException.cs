using System;

namespace TicTacToe.Application.Common.Exceptions
{
    /// <summary>
    /// Custom exception aimed to represent the fact that entity was not found.
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}
