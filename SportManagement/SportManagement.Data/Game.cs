using System;
using System.Collections.Generic;
using System.Text;

namespace SportManagement.Data
{
    public class Game
    {

        public int GameId { get; set; }
        public DateTime Date { get; set; }

        public  virtual Sport Sport { get; set; }

        public virtual Team LocalTeam { get; set; }

        public virtual Team VisitingTeam { get; set; }

        public Game(DateTime date, Sport sport,Team localTeam,Team visitingTeam)
        {
            this.Date = date;
            this.Sport = sport;
            this.LocalTeam = localTeam;
            this.VisitingTeam = visitingTeam;
        }

        public Game(DateTime date)
        {
            this.Date = date;
            
        }

        public Game()
        {

        }

        public Game Update(Game entity)
        {
            this.Date = entity.Date;
            this.LocalTeam = entity.LocalTeam;
            this.VisitingTeam = entity.VisitingTeam;
            return this;
        }
    }
}
