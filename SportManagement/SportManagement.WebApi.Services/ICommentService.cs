using SportManagement.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportManagement.WebApi.Services
{
    public interface ICommentService
    {
        Comment GetCommentById(int commentId);
        IEnumerable<Comment> GetAllComments();
        Comment CreateComment(Comment commentToCreate);
        IEnumerable<Comment> GetGameComments(int gameId);
        IEnumerable<Comment> GetUserCommentsForAGame(int userId, int gameId);
        IEnumerable<Comment> GetUserComments(int userId);

        IEnumerable<Comment> GetCommentsOfTeamsUserFollows(int userId);

    }
}
