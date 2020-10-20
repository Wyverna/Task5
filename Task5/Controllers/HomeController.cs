using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Task4.Models;
using Microsoft.AspNetCore.Authorization;
using Task4.ViewModels;

namespace Task4.Controllers
{
    public class HomeController : Controller
    {
        static List<Game> games = new List<Game>();
        private readonly ILogger<HomeController> _logger;
        readonly MainContext db;        

        public HomeController(ILogger<HomeController> logger, MainContext context)
        {
            _logger = logger;
            db = context;            
        }

        [Authorize]
        public IActionResult Index()
        {            
            ViewBag.Games = db.Games;
            return View();
        }           

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateNewGame(NewGameModel model)
        {
            if (ModelState.IsValid)
            {
                Game game = new Game { Name = model.Name, Tags = model.Tags };
                db.Games.Add(game);
                games.Add(game);
                await db.SaveChangesAsync();                
                return AddPlayer(game, true);
            }
            return RedirectToAction("Index", "Home", model);
        }

        [Authorize]
        public IActionResult AddPlayer(Game game, bool flag)
        {
            string TypeOfMove;            
            AddPlayerToGame(game, User.Identity.Name, out TypeOfMove);
            if (flag)
            {
                return RedirectToAction("Field", "Game", new { name = game.Name, move = TypeOfMove, join = "new" });
            }
            else
            {
                return RedirectToAction("Field", "Game", new { name = game.Name, move = TypeOfMove, join = "join" });
            }
            
        }

        [Authorize]
        public IActionResult JoinTheGame(string name)
        {
            Game game = db.Games.FirstOrDefault(u => u.Name == name);
            if (game.Player1 == null || game.Player2 == null)
            {                
                    return AddPlayer(game, false);
            }
            else
            {
                return RedirectToAction("index");
            }
        }

        [Authorize]
        public void AddPlayerToGame(Game game, string name, out string TypeOfMove)
        {            
            if(game.Player1 == null)
            {
                game.Player1 = name;
                TypeOfMove = "x";                            
            }
            else if (game.Player2 == null)
            {
                game.Player2 = name;
                TypeOfMove = "o";                
            }
            else
            {
                TypeOfMove = "";
            }
            db.SaveChanges();
        }

        [Authorize]        
        public IActionResult RemovePlayerFromGame(string name)
        {            
            Game CurrentGame = db.Games.FirstOrDefault(g => g.Name == name);
            if(CurrentGame.Player1 == User.Identity.Name)
            {
                CurrentGame.Player1 = null;                
            } else if (CurrentGame.Player2 == User.Identity.Name)
            {
                CurrentGame.Player2 = null;                
            }
            db.SaveChanges();
            CheckOnEmptyGame(CurrentGame);            
            return RedirectToAction("Index", "Home");
        }        

        public void CheckOnEmptyGame(Game game)
        {
            if(game.Player1 == null && game.Player2 == null)
            {
                db.Games.Remove(game);
                games.Remove(game);
                db.SaveChanges();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
