using System;
using System.Collections.Generic;
using System.Text;

namespace SportManagement.Data
{
    public class UserTeam
    {

        public int TeamId { get; set; }
        public int UserId { get; set; }
        public virtual Team Team { get; set; }
        public virtual User User { get; set; }

        public UserTeam()
        {

        }
    }
}
