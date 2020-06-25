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
    [Route("api/sports")]
    public class SportsController : Controller
    {
        private ISportService sportService;

        public SportsController(ISportService service)
        {
            sportService = service;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            IEnumerable<Sport> sports = sportService.GetAllSports();
            if (sports.Count() > 0)
            {
                List<SportModelOut> modelsOut = new List<SportModelOut>();
                foreach (Sport sport in sports)
                {
                    SportModelOut model = new SportModelOut(sport);
                    modelsOut.Add(model);
                }

                return Ok(modelsOut);

            }
            return Ok("No hay deportes ingresados al sistema");

        }

        [HttpGet("{id}", Name = "GetSport")]
        [Authorize]
        public IActionResult Get(int id)
        {
            Sport sport = sportService.GetSportById(id);
            if (sport!=null)
            {
                SportModelOut modelOut = new SportModelOut(sport);
                return Ok(modelOut);
            }
            else
            {
                return BadRequest("No existe el deporte buscado");
            }
            
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Post([FromBody] SportModelIn sportIn)
        {

            try
            {

                if (ModelState.IsValid)
                {
                    Sport sport = sportIn.TransformToEntity();
                    sport = sportService.CreateSport(sport);
                    SportModelOut modelOut = new SportModelOut(sport)
;                   return CreatedAtRoute("GetSport", new { id = sport.SportId }, modelOut);
                }
                else
                {
                    return BadRequest(ModelState);
                }

            }
            catch (NotUniqueException ex)
            {
                return BadRequest(ex.Message);
            }
        }

       


        [Route("{sportId}/teams")]
        [Authorize]
        public IActionResult GetAllTeamsInSport(int sportId)
        {
            try
            {
                IEnumerable<Team> teams = sportService.GetAllTeamsInSport(sportId);
                List<TeamModelOut> modelTeams = new List<TeamModelOut>();
                foreach (Team team in teams)
                {
                    TeamModelOut model = new TeamModelOut(team);
                    modelTeams.Add(model);
                }
                return Ok(modelTeams);

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteSport(int id)
        {
           
                bool deleted=sportService.DeleteSport(id);
                if (deleted)
                    return Ok("El deporte ha sido eliminado");
                else
                    return BadRequest("No existe el deporte a eliminar");
            
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult PutSport(int id,[FromBody] SportModelIn modelIn)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    Sport sport = modelIn.TransformToEntity();
                    sport = sportService.UpdateSport(id,sport);
                    return Ok("El deporte ha sido actualizado");
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
           catch(NotExistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(NotUniqueException ex)
            {
                return BadRequest(ex.Message);
            }

        }


    

    }
}
