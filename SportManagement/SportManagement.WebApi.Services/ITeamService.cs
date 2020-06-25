using Microsoft.AspNetCore.Http;
using SportManagement.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SportManagement.WebApi.Services
{
    public interface ITeamService
    {
        Team GetTeamById(int teamId);
        IEnumerable<Team> GetAllTeams();
        Team CreateTeam(Team teamToCreate);
        Team UpdateTeam(int teamId, Team teamToUpdate);
        bool DeleteTeam(int teamId);     
        byte[] GetTeamPhoto(int teamId);
        IEnumerable<Team> GetAllTeamsInSport(int sportId);
        void AddImageFile(int teamdId, IFormFile file);

       

    }
}
