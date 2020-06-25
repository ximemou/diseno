using SportManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportManagement.WebApi.Model
{
    public class UserModelOut
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public bool IsAdministrator { get; set; }
        public string Email { get; set; }

        public UserModelOut(User user)
        {
            this.UserId = user.UserId;
            this.Name = user.LastName;
            this.LastName = user.LastName;
            this.UserName = user.UserName;
            this.IsAdministrator = user.IsAdministrator;
            this.Email = user.Email;
        }

        
    }
}
