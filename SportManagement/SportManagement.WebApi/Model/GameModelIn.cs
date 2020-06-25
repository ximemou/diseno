using SportManagement.Data;
using SportManagement.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SportManagement.WebApi.Model
{
    public class GameModelIn
    {
        [Required]
        public string Date { get; set; }
        [Required]
        public int LocalTeamId { get; set; }
        [Required]
        public int VisitingTeamId { get; set; }
        [Required]
        public int SportId { get; set; }





        public Game TransformToEntity()
        {

            try
            {
                DateTime dateAndTime = DateTime.ParseExact(this.Date, "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                Game game = new Game();
                game.Date = dateAndTime;
                Sport sport = new Sport();
                sport.SportId = this.SportId;
                Team localTeam = new Team();
                localTeam.TeamId = this.LocalTeamId;
                Team visitingTeam = new Team();
                visitingTeam.TeamId = this.VisitingTeamId;
                game.Sport = sport;
                game.VisitingTeam = visitingTeam;
                game.LocalTeam = localTeam;
                return game;
            }
            catch
            {
                throw new DateFormatException("El formato de la fecha y hora del partido es yyyy-MM-dd HH:mm");
            }

        }


    }
}
