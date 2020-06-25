using SportManagement.FixtureAlgorithm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportManagement.WebApi.Model
{
    public class GameAlgorithmModelIn
    {

        [Required]
        public string DateAndTime { get; set; }
        [Required]
        public int SportId { get; set; }
        [Required]
        public string Algorithm { get; set; }

        public DateTime ParseDate()
        {

            DateTime dateParsed= DateTime.ParseExact(this.DateAndTime, "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);

            return dateParsed;
        }
    }
}
