using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicTacToe.Application.CrossPlayerGameTiles.Commands.CreateCrossPlayerGameTile;

namespace TicTacToe.WebUI.Controllers
{
    public class CrossPlayerGameTilesController : ApiController
    {
        public CrossPlayerGameTilesController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateCrossPlayerGameTileCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
