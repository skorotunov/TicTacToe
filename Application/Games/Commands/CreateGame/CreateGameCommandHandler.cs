using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Application.Common.Interfaces;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.Games.Commands.CreateGame
{
    /// <summary>
    /// Handler which inserts game to the database.
    /// </summary>
    public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, int>
    {
        private readonly ITicTacToeDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IDateTime dateTime;

        public CreateGameCommandHandler(ITicTacToeDbContext context, ICurrentUserService currentUserService, IDateTime dateTime)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.dateTime = dateTime;
        }

        public async Task<int> Handle(CreateGameCommand request, CancellationToken cancellationToken)
        {
            string currentUserId = currentUserService.UserId;
            var entity = new Game
            {
                StartDate = dateTime.Now,
                CrossPlayerId = request.IsOpponentCrossPlayer ? request.OpponentId : currentUserId,
                NoughtPlayerId = request.IsOpponentCrossPlayer ? currentUserId : request.OpponentId
            };
            context.Games.Add(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}
