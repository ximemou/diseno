using System;
using System.Collections.Generic;
using System.Text;

namespace SportManagement.Data
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public  virtual Game Game { get; set; }
        public  virtual User User { get; set; }

        public int GameId { get; set; }
        public int UserId { get; set; }


        public Comment (string text,Game game,User user)
        {
            this.Text = text;
            this.Game = game;
            this.User=user;
        }

        public Comment(string text)
        {
            this.Text = text;
        }
    }
}
