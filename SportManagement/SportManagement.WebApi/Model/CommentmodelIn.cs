using SportManagement.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportManagement.WebApi.Model
{
    public class CommentModelIn
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public int GameId { get; set; }
        [Required]
        public int UserId { get; set; }

        public Comment TransformToEntity()
        {
            Comment comment = new Comment(Text);
            comment.UserId = this.UserId;
            comment.GameId = this.GameId;
            return comment;
        }
    }
}
