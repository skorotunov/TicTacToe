namespace TicTacToe.Application.Common.Interfaces
{
    /// <summary>
    /// Service which represents current user information.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Current user ID.
        /// </summary>
        string UserId { get; }

        /// <summary>
        /// Current user name.
        /// </summary>
        string UserName { get; }
    }
}
