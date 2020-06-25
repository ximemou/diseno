using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SportManagement.Data;
using SportManagement.Data.Repository;
using SportManagement.Exceptions;

namespace SportManagement.WebApi.Services
{
    public class UserService : IUserService
    {
   
        private readonly IUnitOfWork unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public User CreateUser(User user)
        {
            if (ValidateUniqueUserName(user.UserName)){

                if (ValidateEmailFormat(user.Email)){

                    unitOfWork.UserRepository.Insert(user);
                    unitOfWork.Save();
                    return user;

                }
                else
                {
                    throw new EmailFormatException("Formato de email incorrecto");
                }         
            }
            else
            {
                throw new NotUniqueException("Ya existe un usuario con el nombre de usuario ingresado");
            }
        }

        public bool DeleteUser(int userId)
        {
            User user = unitOfWork.UserRepository.GetByID(userId);
            if (user != null)
            {
                
                unitOfWork.UserRepository.Delete(user);
                //fijarem si borra en la tabla userteam
                unitOfWork.Save();
                return true;
            }

            return false;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return unitOfWork.UserRepository.Get();
        }

        public User GetUserById(int userId)
        {
            return unitOfWork.UserRepository.GetByID(userId);
        }

        public User UpdateUser(int userId, User userToUpdate)
        {
            User user = unitOfWork.UserRepository.GetByID(userId);
            if (ExistsUser(userId))
            {
                if (!ExistsUserWithSameUserNameWhenUpdate(user.UserId, userToUpdate.UserName))
                {
                    if (ValidateEmailFormat(userToUpdate.Email))
                    {
                        user.Update(userToUpdate);
                        unitOfWork.UserRepository.Update(user);
                        unitOfWork.Save();
                    
                        return user;
                    }
                    else
                    {
                        throw new EmailFormatException("Formato de mail invalido");
                    }
                }
                else
                {
                    throw new NotUniqueException("Ya existe un usuario con ese nombre de usuario");
                }   

            }
            else
            {
                throw new ArgumentException("El usuario no existe");
            }    
        }
        private bool ExistsUserWithSameUserNameWhenUpdate(int userId, string userName)
        {

            IEnumerable<User> users = unitOfWork.UserRepository.Get(u => u.UserName == userName && u.UserId != userId);
            return users.Count() > 0;
        }

        private bool ExistsUser(int userId)
        {
            User user = unitOfWork.UserRepository.GetByID(userId);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        private static void ThrowErrorIfItsNull(User user)
        {
            if (user == null)
            {
                throw new ArgumentException("El usuario no existe");
            }
        }


        private bool ExistsUserWithSameUserName(string userName)
        {

            IEnumerable<User> users = unitOfWork.UserRepository.Get(u => u.UserName == userName);
            return users.Count() > 0;
            
        }

        private bool ValidateNewUser(User User)
        {
            bool correct;


            correct = ValidateCompleteName(User.Name, User.LastName, User.UserName)
                   && ValidatePassword(User.Password);

            return correct;
        }

        private bool ValidateCompleteName(string name, string lastName, string userName)
        {
            return (name.Length != 0 && lastName.Length != 0 && userName.Length != 0);
        }

        //POS: retorna true si no es vacio
        private bool ValidatePassword(string password)
        {
            return password.Length != 0;
        }

        private bool ValidateUniqueUserName(string userName)
        {
            IEnumerable<User> users =unitOfWork.UserRepository.Get(u => u.UserName == userName);
            if (users.Count() > 0)
                return false;
            else
                return true;
        }

        private bool ValidateUniqueEmail(string email) {

            IEnumerable<User> users = unitOfWork.UserRepository.Get(u => u.Email == email);
            if (users.Count() > 0)
                return false;
            else
                return true;

        }

        private bool ValidateEmailFormat(string email)
        {   
            string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";

            if (Regex.IsMatch(email, pattern))
            {
                return true;
            }
            return false;
        }


        public void StopFollowingTeam(int userId, int teamId)
        { 
            UserTeam userTeamToDelete = unitOfWork.UserTeamRepository.Get(u => u.UserId == userId && u.TeamId == teamId).FirstOrDefault();
            if (userTeamToDelete != null) {
                 unitOfWork.UserTeamRepository.Delete(userTeamToDelete);
                 unitOfWork.Save();
            }
            else
            {
                throw new ArgumentException("El usuario no sigue al equipo. Verifique los datos ingresados");
            }
        }

       

        public void AddFavouriteTeams(int userId,List<Team> teams)
        {
            if(ExistsUser(userId))
            {
                if (AllTeamsExists(teams))
                {
                
                    foreach (Team t in teams)
                    {
                        Team team = unitOfWork.TeamRepository.GetByID(t.TeamId);
                        User user = unitOfWork.UserRepository.GetByID(userId);
                        if (team != null)
                        {
                            if (!AlreadyFollowingTeam(userId, team.TeamId))
                            {
                                UserTeam userTeam = new UserTeam();
                                userTeam.TeamId = team.TeamId;
                                userTeam.UserId = userId;
                                unitOfWork.UserTeamRepository.Insert(userTeam);
                            }
                        }
                    }
                    unitOfWork.Save();
                }
                else
                {
                    throw new NotExistsException("Los datos de los equipos favoritos no son correctos , vuelva a ingresarlos");
                }
            }
            else
            {
                throw new ArgumentException("No existe el usuario");
            }
           
        }

        private bool AllTeamsExists(List<Team> teams)
        {
            foreach(Team team in teams)
            {
                Team teamFromRepo = unitOfWork.TeamRepository.GetByID(team.TeamId);
                if (teamFromRepo == null)
                    return false;

            }
            return true;
        }


        

        private bool AlreadyFollowingTeam(int userId, int teamId)
        {
            UserTeam userTeam = unitOfWork.UserTeamRepository.Get(u => u.UserId == userId && u.TeamId == teamId).FirstOrDefault();
            if (userTeam != null)
                return true;
            return false;
        }

        public List<Team> GetUserFavouriteTeams(int userId)
        {

            if (ExistsUser(userId))
            {
                IEnumerable<UserTeam> userTeams = unitOfWork.UserTeamRepository.Get(u => u.UserId == userId);

                List<Team> teams = new List<Team>();
                foreach (UserTeam userTeam in userTeams)
                {
                    teams.Add(userTeam.Team);
                }
                return teams;
            }
            else{
                throw new ArgumentException("No existe el usuario");
            }
            
        }
    }
}
