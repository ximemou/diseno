using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SportManagement.Data;
using SportManagement.Data.Repository;
using SportManagement.Exceptions;

namespace SportManagement.WebApi.Services
{
    public class CommentService : ICommentService
    {

        private readonly IUnitOfWork unitOfWork;

        public CommentService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public Comment CreateComment(Comment commentToCreate)
        {
            Game game = unitOfWork.GameRepository.GetByID(commentToCreate.GameId);
            User user = unitOfWork.UserRepository.GetByID(commentToCreate.UserId);
            if(game!=null && user != null)
            {
                unitOfWork.CommentRepository.Insert(commentToCreate);
                unitOfWork.Save();
                return commentToCreate;
            }
            else
            {
                string message = "";
                if (game == null)
                    message = "No existe el encuentro al cual se le quiere realizar un comentario\n";
                if (user == null)
                    message += "No existe el usuario que quiere realizar el comentario";
                throw new NotExistsException(message);
            }
        }
       
        public IEnumerable<Comment> GetAllComments()
        {
            IEnumerable<Comment> comments = unitOfWork.CommentRepository.Get();
            return comments;
        }

        public Comment GetCommentById(int commentId)
        {
            Comment comment = unitOfWork.CommentRepository.GetByID(commentId);
            return comment;
        }

        public IEnumerable<Comment> GetCommentsOfTeamsUserFollows(int userId)
        {
            User user = unitOfWork.UserRepository.GetByID(userId);
            List<Comment> commentsToReturn = new List<Comment>();
            if (user != null)
            {
                IEnumerable<UserTeam> userTeams = unitOfWork.UserTeamRepository.Get(u => u.UserId == userId);             
                foreach(UserTeam userTeam in userTeams)
                {
                    IEnumerable<Game> games = unitOfWork.GameRepository.Get(g => g.LocalTeam.TeamId == userTeam.TeamId || g.VisitingTeam.TeamId == userTeam.TeamId);
                    foreach(Game game in games)
                    {
                        IEnumerable<Comment> comments = unitOfWork.CommentRepository.Get(c => c.Game.GameId == game.GameId);
                        if (comments.Count() > 0)
                        {
                            commentsToReturn.AddRange(comments);
                        }
                    }
                }
                return commentsToReturn;
            }
            else
            {
                throw new NotExistsException("No existe el usuario ingresado");
            }
        }

        public IEnumerable<Comment> GetGameComments(int gameId)
        {
            Game game = unitOfWork.GameRepository.GetByID(gameId);
            if (game != null)
            {
                IEnumerable<Comment> comments = unitOfWork.CommentRepository.Get(c => c.Game.GameId == gameId);
                return comments;
            }
            else
            {
                throw new NotExistsException("No existe el encuentro ingresado");
            }
            
        }

        public IEnumerable<Comment> GetUserComments(int userId)
        {
            User user = unitOfWork.UserRepository.GetByID(userId);
            if (user != null)
            {
                IEnumerable<Comment> comments = unitOfWork.CommentRepository.Get(c => c.User.UserId == userId);
                return comments;
            }
            else
            {
                throw new NotExistsException("No existe el usuario");
            }
            
        }

        public IEnumerable<Comment> GetUserCommentsForAGame(int userId, int gameId)
        {
            User user = unitOfWork.UserRepository.GetByID(userId);
            Game game = unitOfWork.GameRepository.GetByID(gameId);

            if(user!=null && game != null)
            {
                IEnumerable<Comment> comments = unitOfWork.CommentRepository.Get(c => c.User.UserId == userId && c.Game.GameId == gameId);
                return comments;
            }
            else
            {
                string message = "";
                if (user == null)
                {
                    message += " No existe el usuario ingresado\n";
                }
                if (game == null)
                {
                    message += "No existe el encuentro ingresado";
                }
                throw new NotExistsException(message);
            }

           
        }
    }
}
