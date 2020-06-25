using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SportManagement.Data
{
    public class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
    
        public int SportId { get; set; }

        public virtual Sport Sport { get; set; }

        public virtual List<UserTeam> FollowingUsers { get; set; }
      
        public byte[] Photo { get; set; }

        
       
        public Team(string name)
        {
            this.Name = name;         
            this.FollowingUsers = new List<UserTeam>();
        }

  
        public Team()
        {
            this.Name = "";
            this.FollowingUsers = new List<UserTeam>();
        }

        public Team(int teamId)
        {
            this.TeamId = teamId;
            this.FollowingUsers = new List<UserTeam>();
        }

        public Team Update(Team entity)
        {
            Name = entity.Name;         
            return this;
        }


    }
}
