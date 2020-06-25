using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SportManagement.Data;
using SportManagement.Data.Repository;
using SportManagement.Exceptions;

namespace SportManagement.WebApi.Services
{
    public class SportService : ISportService
    {

        private readonly IUnitOfWork unitOfWork;

        public SportService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Sport CreateSport(Sport sportToCreate)
        {
            if (UniqueSportName(sportToCreate.Name))
            {

                    unitOfWork.SportRepository.Insert(sportToCreate);
                    unitOfWork.Save();
                    return sportToCreate;
            }
            else
            {
                throw new NotUniqueException("Ya existe un deporte con el nombre ingresado");
            }
        }

        private bool UniqueSportName(string name)
        {
            IEnumerable<Sport> sports = unitOfWork.SportRepository.Get(s => s.Name == name);
            if (sports.Count() == 0)
                return true;
            return false;
        }

        public bool DeleteSport(int sportId)
        {
            Sport sport = unitOfWork.SportRepository.GetByID(sportId);
            if (sport != null)
            {
                DeleteGames(sportId);
                unitOfWork.SportRepository.Delete(sportId);
                unitOfWork.Save();
                return true;
            }
            else
                return false;
        }

        private void DeleteGames(int sportId)
        {
            IEnumerable<Game> games = unitOfWork.GameRepository.Get(g => g.Sport.SportId == sportId);
            foreach(Game game in games)
            {
                unitOfWork.GameRepository.Delete(game);
               
            }
        }

        public IEnumerable<Sport> GetAllSports()
        {
            return unitOfWork.SportRepository.Get();
        }

        public Sport GetSportById(int sportId)
        {
            return unitOfWork.SportRepository.GetByID(sportId);
        }

        public Sport UpdateSport(int sportId, Sport sportToUpdate)
        {
            Sport sport = GetSportById(sportId);
            if (sport != null)
            {
                if (UniqueSportNameWhenUpdating(sportToUpdate.Name,sportId))
                {
                    sport.Update(sportToUpdate);
                    unitOfWork.SportRepository.Update(sport);
                    unitOfWork.Save();
                    return sport;
                }
                else
                {
                    throw new NotUniqueException("Ya existe un equipo con el mismo nombre.");
                }
                
            }
            else
            {
                throw new NotExistsException("No existe el deporte que se quiere borrar");
            }

        }

        private bool UniqueSportNameWhenUpdating(string name,int sportId)
        {
            IEnumerable<Sport> sports = unitOfWork.SportRepository.Get(s => s.Name == name && s.SportId!=sportId);
            if (sports.Count() == 0)
                return true;
            return false;
        }

        public bool AddTeamsToSport(int sportId, List<Team> teams)
        {
            if (ExistsSport(sportId))
            {
                Sport sport = GetSportById(sportId);
                foreach(Team t in teams)
                {
                    Team team = unitOfWork.TeamRepository.GetByID(t.TeamId);                 
                    
                    unitOfWork.SportRepository.Update(sport);
                    unitOfWork.Save();
                }
               
            
                 
            }
            return true;
        }

        private void AddToDataBase(int sportId)
        {
            Sport sport = unitOfWork.SportRepository.GetByID(sportId);
        }

        private bool ExistsSport(int sportId)
        {
            Sport sport=unitOfWork.SportRepository.GetByID(sportId);
            if (sport != null)
                return true;
            return false;
        }

        public IEnumerable<Team> GetAllTeamsInSport(int sportId)
        {
            if (ExistsSport(sportId))
            {
                IEnumerable<Team> teams = unitOfWork.TeamRepository.Get(t => t.SportId == sportId);
                return teams;
            }
            else
            {
                throw new ArgumentException("No existe el deporte buscado");
            }
        }

       
    }
}
