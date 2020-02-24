﻿using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe.Application.Common.Interfaces;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.Games.Commands.CreateGame
{
    public class CreateGameCommand : IRequest<int>
    {
        public string PlayerId { get; set; }

        public bool IsCrossPlayer { get; set; }

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
                    CrossPlayerId = request.IsCrossPlayer ? request.PlayerId : currentUserId,
                    NoughtPlayerId = request.IsCrossPlayer ? currentUserId : request.PlayerId
                };
                context.Games.Add(entity);
                await context.SaveChangesAsync(cancellationToken);
                return entity.Id;
            }
        }
    }
}
