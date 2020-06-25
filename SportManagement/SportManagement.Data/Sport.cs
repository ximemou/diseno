using System;
using System.Collections.Generic;
using System.Text;

namespace SportManagement.Data
{
    public class Sport
    {
        public int SportId { get; set; }
        public string Name { get; set; }


        public virtual List<Team> Teams { get; set; }

        public Sport(string name)
        {
            this.Name = name;
            this.Teams = new List<Team>();
               
        }

        public Sport()
        {
            this.Teams = new List<Team>();
        }

        public Sport Update(Sport sport)
        {
            this.Name = sport.Name;
            return this;
        }
    }
}
