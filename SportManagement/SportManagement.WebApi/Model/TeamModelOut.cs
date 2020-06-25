using SportManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportManagement.WebApi.Model
{
    public class TeamModelOut
    {
        public string Name { get; set; }
       // public SportModelOut Sport { get; set; }

        //public string Photo { get; set; }

        public int TeamId { get; set; }

        public int SportId { get; set; }

        public TeamModelOut(Team team)
        {
            this.Name = team.Name;
            this.SportId = team.SportId;
       
          //  if (team.Image != null)
            //    this.Photo = team.Image;
            this.TeamId = team.TeamId;
           
        }

        public TeamModelOut()
        {
        }
    }
}
