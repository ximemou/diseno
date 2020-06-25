using SportManagement.Data;
using SportManagement.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportManagement.WebApi.Model
{
    public class GameModelInUpdate
    {
        [Required]
        public string Date { get; set; }
        [Required]
        public int LocalTeamId { get; set; }
        [Required]
        public int VisitingTeamId { get; set; }
     
        public Game TransformToEntity()
        {

            try
            {
                DateTime dateAndTime = DateTime.ParseExact(this.Date, "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                Game game = new Game();
                game.Date = dateAndTime;
                Team localTeam = new Team();
                localTeam.TeamId = this.LocalTeamId;
                Team visitingTeam = new Team();
                visitingTeam.TeamId = this.VisitingTeamId;             
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
