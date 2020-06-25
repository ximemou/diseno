using SportManagement.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SportManagement.WebApi.Model
{
    public class TeamModelInUpdatecs
    {

        [Required]
        public string Name { get; set; }


        public Team TransformToEntity()
        {
            try
            {
                Team team = new Team();
                team.Name = this.Name;                          
                return team;
            }
            catch (Exception)
            {
                throw new ArgumentException("Ha ocurrido un error con la imagen. Debe ingresar una ruta valida");
            }

        }

        public List<Team> TransformListToEntity(List<TeamModelIn> teams)
        {
            List<Team> transformedTeams = new List<Team>();
            foreach (TeamModelIn teamModel in teams)
            {
                Team team = teamModel.TransformToEntity();
                transformedTeams.Add(team);

            }
            return transformedTeams;
        }

    }


}

