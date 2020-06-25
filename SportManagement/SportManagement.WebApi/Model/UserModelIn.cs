using SportManagement.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportManagement.WebApi.Model
{
    public class UserModelIn
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public bool IsAdministrator { get; set; }
        [Required]
        public string Email { get; set; }


        public User TransformToEntity()
        {

            return new User
            {
                Name = this.Name,
                LastName = this.LastName,
                UserName = this.UserName,
                IsAdministrator= this.IsAdministrator,
                Password = this.Password,
                Email=this.Email
                
            };


        }

    }
}
