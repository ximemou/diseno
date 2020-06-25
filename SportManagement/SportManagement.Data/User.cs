using System;
using System.Collections.Generic;
using System.Text;

namespace SportManagement.Data
{
    public class User
    {

        public int UserId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsAdministrator { get; set; }
        public string Email { get; set; }

        public virtual List<UserTeam> FavouriteTeams { get; set; }


        public User()
        {
            this.Name = "";
            this.LastName = "";
            this.UserName = "";
            this.Password = "";
            this.IsAdministrator = false;
            this.Email = "";
            FavouriteTeams = new List<UserTeam>();



        }

        public User(string name, string lastName, string userName, string password, string email,bool isAdmin)
        {

            this.Name = name;
            this.LastName = lastName;
            this.UserName = userName;
            this.Password = password;
            this.Email = email;
            this.IsAdministrator = isAdmin;
            this.FavouriteTeams = new List<UserTeam>();

     
            

        }

        public User Update(User entity)
        {
            Name = entity.Name;
            LastName = entity.LastName;
            UserName = entity.UserName;
            Password = entity.Password;
            Email = entity.Email;
            IsAdministrator = entity.IsAdministrator;
            return this;
        }
        

        }
       
}
