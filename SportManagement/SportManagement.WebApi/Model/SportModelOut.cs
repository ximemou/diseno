using SportManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportManagement.WebApi.Model
{
    public class SportModelOut
    {
        public string Name { get; set; }

        public int SportId { get; set; }

        public List<TeamModelOut> Teams { get; set; }

        

        public SportModelOut(Sport sport)
        {
            this.Name = sport.Name;
            this.SportId = sport.SportId;
            List<TeamModelOut> teamsOut = new List<TeamModelOut>();
            foreach(Team team in sport.Teams)
            {
                TeamModelOut model = new TeamModelOut(team);
                teamsOut.Add(model);
            }
            this.Teams = teamsOut;
           
        }
    }
}
