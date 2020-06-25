using SportManagement.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportManagement.WebApi.Model
{
    public class SportModelIn
    {
        [Required]
        public string Name { get; set; }

        public Sport TransformToEntity()
        {
            Sport sport = new Sport(this.Name);
            return sport;
        }
    }
}
