using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TicTacToe.WebUI.Controllers
{
    public class GameController : Controller
    {
        private readonly ILogger<GameController> logger;

        public GameController(ILogger<GameController> logger)
        {
            this.logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}