using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SportManagement.Data;

namespace SportManagement.FixtureAlgorithm
{
    public class RandomAlgorithm : IFixtureAlgorithm
    {

        static Random randomNumber = new Random();
       
        public List<Game> GenerateGames(IEnumerable<Team> teams, DateTime date, Sport sport)
        {  
            List<Game> games = new List<Game>();
            Team[] teamsAsArray = teams.ToArray();
            int numberOfTeams = teams.Count();

            int gamesPerTeam = (numberOfTeams - 1) / 2; 
           for(int team=0; team< teamsAsArray.Length; team++)
            {

                
                DateTime newDate = date;
                for (int i = 0; i <= gamesPerTeam; i++)
                {

                    
                    int opponent = randomNumber.Next(numberOfTeams);
                    while (opponent == team)
                    {
                        opponent = randomNumber.Next(numberOfTeams);
                    }

                    Game game = new Game();
                    game.LocalTeam = teamsAsArray[team];
                    game.VisitingTeam = teamsAsArray[opponent];
                    game.Sport = sport;
                    game.Date = newDate.AddDays(3);
                    while (GamesOnSameDate(game.LocalTeam.TeamId,game.VisitingTeam.TeamId,games,game.Date))
                    {
                        game.Date = game.Date.AddDays(1);
                    }
                    games.Add(game);
                    newDate = game.Date;

                }
            }
            return games;
        }


        private bool GamesOnSameDate(int localTeamId,int visitingTeamId,List<Game> games,DateTime date)
        {
            foreach(Game game in games)
            {
                if((game.LocalTeam.TeamId==localTeamId || game.VisitingTeam.TeamId == localTeamId) || (game.LocalTeam.TeamId==visitingTeamId || game.VisitingTeam.TeamId==visitingTeamId))
                {
                    int result = DateTime.Compare(date, game.Date);
                    if(result == 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


    }
}
