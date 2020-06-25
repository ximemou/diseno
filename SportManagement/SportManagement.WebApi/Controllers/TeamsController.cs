using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportManagement.Data;
using SportManagement.Exceptions;
using SportManagement.WebApi.Model;
using SportManagement.WebApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SportManagement.WebApi.Controllers
{
    [Route("api/teams")]
    public class TeamsController : Controller
    {


        private ITeamService teamService;

        public TeamsController(ITeamService aTeamService)
        {
            teamService = aTeamService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Team> teams = teamService.GetAllTeams();
            if (teams.Count() > 0)
            {
                List<TeamModelOut> teamsModel = new List<TeamModelOut>();
                foreach (Team team in teams)
                {
                    TeamModelOut modelOut = new TeamModelOut(team);
                    teamsModel.Add(modelOut);
                }
                return Ok(teamsModel);
            }
            else
            {
                return Ok("No existen equipos");
            }
            
        }

        [HttpGet("{id}", Name = "GetTeam")]
        [Authorize]
        public IActionResult Get(int id)
        {
            Team team = teamService.GetTeamById(id);
            if (team != null)
            {
                TeamModelOut modelOut = new TeamModelOut(team);
                return Ok(modelOut);
            }
            else
            {
                return BadRequest("No existe el equipo buscado");
            }
            
            
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Post([FromBody] TeamModelIn teamIn)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    Team team = teamIn.TransformToEntity();
                    team = teamService.CreateTeam(team);
                    TeamModelOut modelOut = new TeamModelOut(team);

                    return CreatedAtRoute("Get", new { id = team.TeamId }, modelOut);
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
            catch (Exception ex)
            {
                return BadRequest("Ha ocurrido un error"+ex.Message);
            }
        }


        [HttpPut("{id}/image")]
        [Authorize(Roles = "Administrator")]
        public IActionResult PutImage(int id, IFormFile file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    teamService.AddImageFile(id,file);
                    return Ok("La imagen ha sido agregada");
                }
                else
                {
                    return BadRequest("Debe enviar una imagen como string");
                }
            } catch (NotExistsException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{id}/image")]
        [Authorize]
        public IActionResult GetTeamPhoto(int id)
        {
            try
            {
                byte[] photo = teamService.GetTeamPhoto(id);
                if (photo != null)
                {
                    return Ok(photo);
                }
                else
                {
                    return Ok("El equipo no tiene una imagen");
                }
            }catch(NotExistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
               return BadRequest("Ha ocurrido un error: " + ex.Message);
            }
            
        }  
        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Put(int id, [FromBody]TeamModelIn modelIn)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    Team team = modelIn.TransformToEntity();
                    teamService.UpdateTeam(id, team);
                    return Ok("El equipo ha sido modificado");
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }catch(NotExistsException ex)
            {
                return BadRequest(ex.Message);
            }catch(NotUniqueException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            bool deleted = teamService.DeleteTeam(id);
            if (deleted)
                return Ok("El equipo ha sido eliminado");
            else
                return BadRequest("No existe el equipo que se quiere eliminar");
        }

    }
}
