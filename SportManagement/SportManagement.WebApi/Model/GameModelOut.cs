using SportManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportManagement.WebApi.Model
{
    public class GameModelOut
    {

        public int GameId { get; set; }
        public DateTime Date { get; set; }

        public int  SportId { get; set; }
        public string SportName { get; set; }

        public int LocalTeamId { get; set; }
        public string LocalTeamName { get; set; }

        public int VisitingTeamId { get; set; }
        public string VisitingTeamName { get; set; }

        public GameModelOut(Game game)
        {
            this.GameId = game.GameId;
            this.Date = game.Date;
            this.SportId = game.Sport.SportId;
            this.SportName = game.Sport.Name;
            this.LocalTeamId = game.LocalTeam.TeamId;
            this.LocalTeamName = game.LocalTeam.Name;
            this.VisitingTeamId = game.VisitingTeam.TeamId;
            this.VisitingTeamName = game.VisitingTeam.Name;
        }



    }
}
