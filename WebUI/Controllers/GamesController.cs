using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicTacToe.Application.Games.Commands.CreateGame;
using TicTacToe.Application.Games.Queries.GetGameTiles;

namespace TicTacToe.WebUI.Controllers
{
    public class GamesController : ApiController
    {
        public GamesController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameTilesVM>> Get(int id)
        {
            return await Mediator.Send(new GetGameTilesQuery(id));
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateGameCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
