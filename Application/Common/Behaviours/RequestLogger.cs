using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Application.Common.Interfaces;

namespace TicTacToe.Application.Common.Behaviours
{
    /// <summary>
    /// Behaviour which is aimed on logging each request action and user info.
    /// </summary>
    /// <typeparam name="TRequest">Type of the MediatR request.</typeparam>
    public class RequestLogger<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger logger;
        private readonly ICurrentUserService currentUserService;

        public RequestLogger(ILoggerFactory loggerFactory, ICurrentUserService currentUserService)
        {
            logger = loggerFactory.CreateLogger<RequestLogger<TRequest>>();
            this.currentUserService = currentUserService;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            string requestName = typeof(TRequest).Name;
            string userId = currentUserService.UserId ?? string.Empty;
            string userName = currentUserService.UserName ?? string.Empty;
            logger.LogInformation("Tic Tac Toe Request: {Name} {@UserId} {@UserName} {@Request}", requestName, userId, userName, request);
            return Task.CompletedTask;
        }
    }
}
