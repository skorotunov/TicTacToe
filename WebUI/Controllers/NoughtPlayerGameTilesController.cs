using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicTacToe.Application.NoughtPlayerGameTiles.Commands.CreateNoughtPlayerGameTile;

namespace TicTacToe.WebUI.Controllers
{
    public class NoughtPlayerGameTilesController : ApiController
    {
        public NoughtPlayerGameTilesController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateNoughtPlayerGameTileCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
