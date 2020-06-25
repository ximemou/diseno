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
    [Route("api/comments")]
    public class CommentController : Controller
    {

        private ICommentService commentService;

        public CommentController(ICommentService service)
        {
            this.commentService = service;
        }


        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody]CommentModelIn commentModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Comment comment = commentModel.TransformToEntity();
                    comment = commentService.CreateComment(comment);
                    CommentModelOut modelOut = new CommentModelOut(comment);
                    return CreatedAtRoute("GetComment", new { id = comment.Id }, modelOut);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            } catch (NotExistsException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}", Name = "GetComment")]
        [Authorize]
        public IActionResult Get(int id)
        {
            Comment comment = commentService.GetCommentById(id);
            if (comment != null)
            {
                CommentModelOut modelOut = new CommentModelOut(comment);
                return Ok(modelOut);
            }
            else
            {
                return BadRequest("No existe el comentario buscado");
            }

        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            IEnumerable<Comment> comments = commentService.GetAllComments();
            
            if (comments.Count() > 0)
            {
                List<CommentModelOut> commentsOut = new List<CommentModelOut>();
                foreach (Comment comment in comments)
                {
                    CommentModelOut modelOut = new CommentModelOut(comment);
                    commentsOut.Add(modelOut);
                }
                return Ok(commentsOut);
            }
            else
            {
                return Ok("No existen comentarios en el sistema");
            }

        }

        [HttpGet("game/{gameId}")]
        [Authorize]
        public IActionResult GetGameComments(int gameId)
        {
            try
            {
                IEnumerable<Comment> comments = commentService.GetGameComments(gameId);
                List<CommentModelOut> commentsOut = new List<CommentModelOut>();
                if (comments.Count() > 0)
                {
                    foreach (Comment comment in comments)
                    {
                        CommentModelOut model = new CommentModelOut(comment);
                        commentsOut.Add(model);
                    }
                    return Ok(commentsOut);
                }
                else
                {
                    return Ok(" NO existen comentarios para el equipo");
                }
            } catch (NotExistsException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("user/{userId}")]
        [Authorize]
        public IActionResult GetUserComments( int userId)
        {
            try
            {
                IEnumerable<Comment> comments = commentService.GetUserComments(userId);
                List<CommentModelOut> commentsOut = new List<CommentModelOut>();
                if (comments.Count() > 0)
                {
                    foreach (Comment comment in comments)
                    {
                        CommentModelOut model = new CommentModelOut(comment);
                        commentsOut.Add(model);
                    }
                    return Ok(commentsOut);
                }
                else
                {
                    return Ok(" No existen comentarios para el equipo");
                }
            } catch (NotExistsException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user/{userId}/game/{gameId}")]
        [Authorize]
        public IActionResult GetUserCommentsForGame( int userId, int gameId)
        {
            try
            {
                IEnumerable<Comment> comments = commentService.GetUserCommentsForAGame(userId, gameId);
                List<CommentModelOut> commentsOut = new List<CommentModelOut>();
                if (comments.Count() > 0)
                {
                    foreach (Comment comment in comments)
                    {
                        CommentModelOut model = new CommentModelOut(comment);
                        commentsOut.Add(model);
                    }
                    return Ok(commentsOut);
                }
                else
                {
                    return Ok("No existen comentarios para el equipo");
                }
            }
            catch (NotExistsException ex)
            {
                return BadRequest(ex.Message);
            }


        }  
        

        [HttpGet("game/user/{userId}/")]
        [Authorize]
        public IActionResult GetCommentsOfTeamsUserFollows(int userId)
        {
            try
            {
                IEnumerable<Comment> comments = commentService.GetCommentsOfTeamsUserFollows(userId);
                if (comments.Count() > 0)
                {
                    List<CommentModelOut> commentsOut = new List<CommentModelOut>();
                    foreach (Comment comment in comments)
                    {
                        CommentModelOut modelOut = new CommentModelOut(comment);
                        commentsOut.Add(modelOut);
                    }
                    return Ok(commentsOut);
                }
                else
                {
                    return Ok("No existen comentarios para los equipos que el usuario sigue");
                }
               
            }
            catch (NotExistsException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }    
}
