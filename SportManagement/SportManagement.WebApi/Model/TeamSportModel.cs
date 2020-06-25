using SportManagement.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportManagement.WebApi.Model
{
    public class TeamSportModel
    {
        [Required]
        public int TeamId { get; set; }

        public Team TransformToEntity()
        {
            Team team = new Team(this.TeamId);
            return team;
        }
    }
}
