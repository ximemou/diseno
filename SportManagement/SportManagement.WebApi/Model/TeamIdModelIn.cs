using SportManagement.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportManagement.WebApi.Model
{
    public class TeamIdModelIn
    {
        [Required]
        public int TeamId { get; set; }

        public Team TransformToEntity()
        {
            return new Team
            {
                TeamId = this.TeamId
            };
        }

    }
}
