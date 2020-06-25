using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportManagement.Data;
using SportManagement.Exceptions;
using SportManagement.WebApi.Model;
using SportManagement.WebApi.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SportManagement.WebApi.Controllers
{
    [Route("api/games")]
    public class GamesController : Controller
    {
        private IGameService gameService;

        public GamesController(IGameService service)
        {
            gameService = service;
        }


        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Post([FromBody] GameModelIn gameIn)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Game game = gameIn.TransformToEntity();
                    game = gameService.CreateGame(game);
                    GameModelOut gameModelOut = new GameModelOut(game);
                    return CreatedAtRoute("GetGame", new { id = game.GameId }, gameModelOut);
                }
                else
                {
                    return BadRequest(ModelState);
                }

            }
            catch (DateFormatException ex)
            {
                return BadRequest(ex.Message);

            }
            catch (GameAtSameDateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (WrongParametersException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotExistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("{id}", Name = "GetGame")]
        [Authorize]
        public IActionResult Get(int id)
        {
            Game game = gameService.GetGameById(id);
            if (game != null)
            {
                GameModelOut modelOut = new GameModelOut(game);
                return Ok(modelOut);
            }
            else
            {
                return BadRequest("No existe el partido buscado");
            }
        }


        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            try { 
            IEnumerable<Game> games = gameService.GetAllGames();
           
            if (games.Count() > 0)
            {
                List<GameModelOut> gamesModel = new List<GameModelOut>();
                foreach (Game game in games)
                {
                    GameModelOut modelOut = new GameModelOut(game);
                    gamesModel.Add(modelOut);
                }

                return Ok(gamesModel);

            }
            else
            {
                return BadRequest("No hay partidos en el sistema");
            }
           }catch(Exception ex )
            {
                return BadRequest("Ha ocurrido un error: "+ ex.Message);
            }
        }




        [HttpPost("algorithm")]
        [Authorize(Roles = "Administrator")]
        public IActionResult PostGamesWithAlgorithm([FromBody] GameAlgorithmModelIn model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    DateTime dateAndTime = model.ParseDate();
                    bool gamesWhereGenerated = gameService.GenerateGamesWithAlgorithm(model.SportId, dateAndTime, model.Algorithm);
                    if (gamesWhereGenerated)
                        return Ok("Se generaron los partidos correctamente");
                    else
                        return BadRequest("No se pudieron generar los partidos vuelva a intentarlo");
                }
                else
                {
                    return BadRequest(ModelState);
                }
            } catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            bool deleted = gameService.DeleteGame(id);
            if (deleted)
                return Ok("El partido ha sido eliminado");
            else
                return BadRequest("No existe el partido que se desea borrar");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Put(int id, [FromBody]GameModelInUpdate modelIn)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Game game = modelIn.TransformToEntity();
                    game = gameService.UpdateGame(id, game);
                    return Ok("El partido ha sido modificado");

                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (DateFormatException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (GameAtSameDateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (WrongParametersException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotExistsException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("sport/{sportId}")]
        [Authorize]
        public IActionResult GetGamesForSport(int sportId)
        {
            try
            {
                List<GameModelOut> gamesOut = new List<GameModelOut>();
                IEnumerable<Game> games = gameService.GetGamesForSport(sportId);
                if (games.Count() > 0)
                {
                    foreach (Game game in games)
                    {
                        GameModelOut model = new GameModelOut(game);
                        gamesOut.Add(model);

                    }
                    return Ok(gamesOut);
                }
                else
                {
                    return Ok("No hay partidos para el deporte");
                }
               

            }catch(NotExistsException ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        [HttpGet("team/{teamId}")]
        [Authorize]
        public IActionResult GetGamesForTeam(int teamId)
        {
            try
            {
                List<GameModelOut> gamesOut = new List<GameModelOut>();
                IEnumerable<Game> games = gameService.GetGamesForTeam(teamId);
                if (games.Count() > 0)
                {
                    foreach (Game game in games)
                    {
                        GameModelOut model = new GameModelOut(game);
                        gamesOut.Add(model);

                    }
                    return Ok(gamesOut);
                }
                else
                {
                    return Ok("No hay partidos para el equipo");
                }
            }
            catch (NotExistsException ex)
            {
                return BadRequest(ex.Message);
            }

        }



    }
}
