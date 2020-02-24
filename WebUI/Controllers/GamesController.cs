using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicTacToe.Application.Common.Interfaces;
using TicTacToe.Application.Games.Commands.CreateGame;

namespace TicTacToe.WebUI.Controllers
{
    public class GamesController : ApiController
    {
        private readonly ICurrentUserService currentUserService;

        public GamesController(IMediator mediator, ICurrentUserService currentUserService)
            : base(mediator)
        {
            this.currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateGameCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
