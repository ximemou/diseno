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
    [Route("api/users")]
    public class UsersController : Controller
    {


        private IUserService userService;

        public UsersController(IUserService aUserService)
        {
            userService = aUserService;
        }


        [HttpPost]
        [Authorize(Roles= "Administrator")]
        public IActionResult Post([FromBody] UserModelIn userIn)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    User user = userIn.TransformToEntity();
                    user = userService.CreateUser(user);
                    UserModelOut userModel = new UserModelOut(user);

                    return CreatedAtRoute("Get", new { id = user.UserId }, userModel);
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
            catch (EmailFormatException ex)
            {
                return BadRequest(ex.Message);
            }
        }

       
        [HttpGet("{id}", Name = "Get"),Authorize(Roles="Administrator")]
        [Authorize(Roles= "Administrator")]
        public IActionResult Get(int id)
        {
            
            User user = userService.GetUserById(id);
            if (user != null)
            {
                UserModelOut model = new UserModelOut(user);
                return Ok(model);
            }
            else
            {
                return BadRequest("No existe el usuario buscado");
            }
            

        }


        [HttpPost("{id}/teams")]
        [Authorize]
        public IActionResult PostFavouriteTeams(int id, [FromBody] List<TeamIdModelIn> teamsIn)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<Team> teams = new List<Team>();
                    foreach (TeamIdModelIn model in teamsIn)
                    {
                        Team team = model.TransformToEntity();
                        teams.Add(team);
                    }
                    userService.AddFavouriteTeams(id, teams);
                    return Ok("Equipos favoritos agregados");

                }
                else
                {
                    return BadRequest(ModelState);
                }
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

        [HttpGet("{id}/teams")]
        [Authorize]
        public IActionResult GetUserFavouritTeams(int id)
        {
            try
            {
                List<Team> teams = userService.GetUserFavouriteTeams(id);
                if (teams.Count > 0)
                {
                    List<TeamModelOut> teamsOut = new List<TeamModelOut>();
                    foreach (Team team in teams)
                    {
                        TeamModelOut model = new TeamModelOut(team);
                        teamsOut.Add(model);
                    }
                    return Ok(teamsOut);
                }
                else
                {
                    return Ok("El usuario no sigue a ningun equipo");
                }

              
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{userId}/teams/{teamId}")]
        [Authorize]
        public IActionResult DeleteFavouriteTeam(int userId, int teamId)
        {
            try
            {
                userService.StopFollowingTeam(userId, teamId);
                return Ok("Ha dejado de seguir al equipo");

            } catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            if (userService.DeleteUser(id))
            {
                return Ok("El usuario ha sido eliminado");

            }
            return BadRequest("No existe el usuario a eliminar");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult Get()
        {
            IEnumerable<User> users = userService.GetAllUsers();
            if (users.Count() > 0)
            {
                List<UserModelOut> modelUsers = new List<UserModelOut>();
                foreach (User user in users)
                {
                    UserModelOut model = new UserModelOut(user);
                    modelUsers.Add(model);
                }
                return Ok(modelUsers);
            }
            else
            {
                return Ok("No existen usuarios en el sistema");
            }
            

        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Put(int id, [FromBody]UserModelIn userToUpdate)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    User user = userToUpdate.TransformToEntity();
                    User userUpdated = userService.UpdateUser(id, user);
                    return Ok("El usuario ha sido modificado ");

                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);

            } catch (EmailFormatException ex) {
                return BadRequest(ex.Message);

            } catch (NotUniqueException ex) {


                return BadRequest(ex.Message);
            }

        }

 
    }
}
