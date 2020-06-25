using SportManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportManagement.WebApi.Model
{
    public class CommentModelOut
    {
       
        public int CommentId { get; set; }
        public string Text { get; set; }

        public int GameId { get; set; }
        
        public int UserId { get; set; }

        public CommentModelOut(Comment comment)
        {
            this.CommentId = comment.Id;
            this.Text = comment.Text;
            this.GameId = comment.GameId;
            this.UserId = comment.UserId;
        }

    }
}
