using SportManagement.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportManagement.WebApi.Services
{
    public interface ISportService
    {
        Sport GetSportById(int sportId);
        IEnumerable<Sport> GetAllSports();
        Sport CreateSport(Sport sportToCreate);
        Sport UpdateSport(int sportId, Sport sportToUpdate);
        bool DeleteSport(int sportId);

        bool AddTeamsToSport(int sportId, List<Team> teams);
        IEnumerable<Team> GetAllTeamsInSport(int sportId);
        
    }
}
