using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicTacToe.Application.Games.Commands.CreateGame;

namespace TicTacToe.WebUI.Controllers
{
    public class GamesController : ApiController
    {
        public GamesController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateGameCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
