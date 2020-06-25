using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SportManagement.Data;
using SportManagement.Data.Repository;
using SportManagement.Exceptions;
using SportManagement.FixtureAlgorithm;

namespace SportManagement.WebApi.Services
{
    public class GameService : IGameService
    {


        private readonly IUnitOfWork unitOfWork;

        public GameService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Game CreateGame(Game gameToCreate)
        {    
            Sport sport = GetSport(gameToCreate.Sport.SportId);
            gameToCreate.Sport = sport;
            
            if (sport != null)
            {

                if (CorrectDate(gameToCreate.Date))
                {
                    if (TeamsBelongToSport(sport.SportId, gameToCreate.LocalTeam.TeamId, gameToCreate.VisitingTeam.TeamId))
                    {
                        Team localTeam = unitOfWork.TeamRepository.GetByID(gameToCreate.LocalTeam.TeamId);
                        Team visitingTeam = unitOfWork.TeamRepository.GetByID(gameToCreate.VisitingTeam.TeamId);
                        gameToCreate.LocalTeam = localTeam;
                        gameToCreate.VisitingTeam = visitingTeam;

                        if (!GameAtSameDate(gameToCreate,localTeam.TeamId) && !GameAtSameDate(gameToCreate,visitingTeam.TeamId))
                        {
                            unitOfWork.GameRepository.Insert(gameToCreate);
                            unitOfWork.Save();
                            return gameToCreate;
                        }
                        else
                        {
                            throw new GameAtSameDateException("Ya existe un partido para estos equipos en el dia ingresado");
                        }
                    }
                    else
                    {
                        throw new WrongParametersException(" Los equipos ingresados deben pertenecer al deporte");
                    }
                }
                else
                {
                    throw new ArgumentException("La fecha del partido debe ser posterior a la fecha actual");
                }
            }
            else
            {
                throw new NotExistsException("No existe el deporte del partido ingresado");
            }    
        }



        private bool CorrectDate(DateTime date)
        {
            int result = DateTime.Compare(date, DateTime.Now);
            if (result > 0)
                return true;
            else
                return false;
        }

         private bool GameAtSameDate(Game game,int teamId)
        {     
            IEnumerable<Game> games = unitOfWork.GameRepository.Get(g =>g.LocalTeam.TeamId==teamId || g.VisitingTeam.TeamId==teamId);
            if (games.Count() == 0)
                return false;
            else
            {
                foreach(Game g in games)
                {

                    int day = g.Date.Day;
                    int month = g.Date.Month;
                    int year = g.Date.Year;

                    DateTime date1 = new DateTime(year, month, day, 0, 0, 0);
                    DateTime date2 = new DateTime(game.Date.Year, game.Date.Month, game.Date.Day, 0, 0, 0);
                    int result = DateTime.Compare(date1, date2);
                    if (result == 0)
                        return true;
                }
            }
            return false; 

        }

        private bool TeamsBelongToSport(int sportId, int localTeamId, int visitingTeamId)
        {
                    
            Team localTeam= unitOfWork.TeamRepository.GetByID(localTeamId);
            Team visitingTeam = unitOfWork.TeamRepository.GetByID(visitingTeamId);
            if (localTeam!=null && visitingTeam != null)
            {
                if (localTeam.SportId == sportId && visitingTeam.SportId == sportId)
                    return true;
                else
                    return false;
           
            }
            return false;
        }


        private Sport GetSport(int sportId)
        {
            Sport sport = unitOfWork.SportRepository.GetByID(sportId);
            return sport;
        }

        public bool DeleteGame(int gameId)
        {
            Game game = GetGameById(gameId);
            if (game != null)
            {
                DeleteComments(game);
                unitOfWork.GameRepository.Delete(game);
                unitOfWork.Save();
                return true;
            }
            return false;
        }

        private void DeleteComments(Game game)
        {
            IEnumerable<Comment> comments = unitOfWork.CommentRepository.Get(c => c.Game.GameId == game.GameId);
            foreach(Comment comment in comments)
            {
                unitOfWork.CommentRepository.Delete(comment);
            }
        }

        public IEnumerable<Game> GetAllGames()
        {
            IEnumerable<Game> games = unitOfWork.GameRepository.Get();
            return games;
        }

        public Game GetGameById(int gameId)
        {
            Game game = unitOfWork.GameRepository.GetByID(gameId);
            return game;
        }

        public Game UpdateGame(int gameId, Game gameToUpdate)
        {
            Game game = GetGameById(gameId);
            if (game != null)
            {
                if (CorrectDate(gameToUpdate.Date))
                {
                    Team localTeam = unitOfWork.TeamRepository.GetByID(gameToUpdate.LocalTeam.TeamId);
                    Team visitingTeam = unitOfWork.TeamRepository.GetByID(gameToUpdate.VisitingTeam.TeamId);
                    if (TeamsBelongToSport(game.Sport.SportId, localTeam.TeamId, visitingTeam.TeamId))
                    {
                        gameToUpdate.LocalTeam = localTeam;
                        gameToUpdate.VisitingTeam = visitingTeam;

                        if (!GameAtSameDateWhenUpdate(gameToUpdate, localTeam.TeamId,gameId) && !GameAtSameDateWhenUpdate(gameToUpdate, visitingTeam.TeamId,gameId))
                        {
                            game.Update(gameToUpdate);
                            unitOfWork.GameRepository.Update(game);
                            unitOfWork.Save();
                            return gameToUpdate;
                        }
                        else
                        {
                            throw new GameAtSameDateException("Ya existe un partido para estos equipos en el dia ingresado");
                        }
                    }
                    else
                    {
                        throw new WrongParametersException(" Los equipos ingresados deben pertenecer al deporte");
                    }
                }
                else
                {
                    throw new ArgumentException("La fecha del partido debe ser posterior a la fecha actual");
                }
            }
            else
            {
                throw new NotExistsException("No existe el partido que se quiere modificar");
            }
        }



        private bool GameAtSameDateWhenUpdate(Game game, int teamId,int gameId)
        {
            IEnumerable<Game> games = unitOfWork.GameRepository.Get(g => (g.LocalTeam.TeamId == teamId || g.VisitingTeam.TeamId == teamId) && g.GameId!=gameId);
            if (games.Count() == 0)
                return false;
            else
            {
                foreach (Game g in games)
                {

                    int day = g.Date.Day;
                    int month = g.Date.Month;
                    int year = g.Date.Year;

                    DateTime date1 = new DateTime(year, month, day, 0, 0, 0);
                    DateTime date2 = new DateTime(game.Date.Year, game.Date.Month, game.Date.Day, 0, 0, 0);
                    int result = DateTime.Compare(date1, date2);
                    if (result == 0)
                        return true;
                }
            }
            return false;

        }

        public bool GenerateGamesWithAlgorithm(int sportId, DateTime date, string algorithm)
        {
       
                Sport sport = GetSport(sportId);
            if (sport != null)
            {
                IEnumerable<Team> teams = unitOfWork.TeamRepository.Get(t => t.SportId == sport.SportId);
                ConvertFixtureAlgorithm fixtureAlgorithm = new ConvertFixtureAlgorithm();
                IFixtureAlgorithm fixtAlg = fixtureAlgorithm.ConvertAlgorithm(algorithm);
                List<Game> games = fixtAlg.GenerateGames(teams, date,sport);
                foreach (Game game in games) {

                    if (GameAtSameDate(game, game.LocalTeam.TeamId) || GameAtSameDate(game, game.VisitingTeam.TeamId))
                    {
                        return false;
                    }

                    unitOfWork.GameRepository.Insert(game);
                }

                unitOfWork.Save();
                return true;
            }           
                else
                {
                    throw new NotExistsException("No existe el deporte ingresado");
                }
             
        }

        public IEnumerable<Game> GetGamesForSport(int sportId)
        {
            Sport sport = GetSport(sportId);
            if(sport != null)
            {
                IEnumerable<Game> games = unitOfWork.GameRepository.Get(s => s.Sport.SportId == sportId);
                return games;
            }
            else
            {
                throw new NotExistsException("No existe el deporte ingresado");
            }
        }

        public IEnumerable<Game> GetGamesForTeam(int teamId)
        {
            Team team = unitOfWork.TeamRepository.GetByID(teamId);
            if (team != null)
            {
                IEnumerable<Game> games = unitOfWork.GameRepository.Get(g => g.LocalTeam.TeamId == teamId || g.VisitingTeam.TeamId == teamId);
                return games;
            }
            else
            {
                throw new NotExistsException("No existe el equipo ingresado");
            }
        }
    }
}
