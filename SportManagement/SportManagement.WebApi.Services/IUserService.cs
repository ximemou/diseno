using SportManagement.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportManagement.WebApi.Services
{
    public interface IUserService
    {
        User GetUserById(int userId);
        IEnumerable<User> GetAllUsers();
        User CreateUser(User UserToCreate);
        User UpdateUser(int UserId, User UserToUpdate);
        bool DeleteUser(int UserId);
        void AddFavouriteTeams(int userId,List<Team> teams);
        List<Team> GetUserFavouriteTeams(int userId);
        void StopFollowingTeam(int userid, int teamId);
    }
}
