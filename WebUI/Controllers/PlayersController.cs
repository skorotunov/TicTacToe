using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicTacToe.Application.Common.Interfaces;
using TicTacToe.Application.Players.Queries.GetPlayerGames;

namespace TicTacToe.WebUI.Controllers
{
    public class PlayersController : ApiController
    {
        private readonly ICurrentUserService currentUserService;

        public PlayersController(IMediator mediator, ICurrentUserService currentUserService)
            : base(mediator)
        {
            this.currentUserService = currentUserService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerGamesVM>> Get(string id)
        {
            return await Mediator.Send(new GetPlayerGamesQuery(id, currentUserService.UserId));
        }
    }
}
