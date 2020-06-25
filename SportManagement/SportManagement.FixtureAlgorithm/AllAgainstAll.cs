using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SportManagement.Data;

namespace SportManagement.FixtureAlgorithm
{
    public class AllAgainstAll : IFixtureAlgorithm
    {

        private const int BYE = -1;
        public List<Game> GenerateGames(IEnumerable<Team> teams,DateTime date, Sport sport)
        {
            List<Game> gamesGenerated = new List<Game>();

            Team[] teamsAsArray = teams.ToArray();

            int[,] generateRounds;

            if (teams.Count() % 2 == 0)
            {
                generateRounds = GenerateRoundRobinEven(teams.Count());
            }
            else{
                generateRounds = GenerateRoundRobinOdd(teams.Count());
            }

            int sumDay = 1;
                for (int j = 0; j < generateRounds.GetLength(1); j++)
                {

                for (int i = 0; i < generateRounds.GetLength(0); i++)
                {
                    if(generateRounds[i, j] != -1)
                    {
                        Game game = new Game();
                        game.LocalTeam = teamsAsArray[i];
                        int visitingTeamId = generateRounds[i, j];
                        game.VisitingTeam = teamsAsArray[visitingTeamId];
                        DateTime gameTime = date.AddDays(sumDay + 1);
                        game.Date = gameTime;
                        game.Sport = sport;
                        gamesGenerated.Add(game);

                    }
                    sumDay++;
                    
                }
                
                

                }
                    return gamesGenerated;
        }



        private int[,] GenerateRoundRobinEven(int num_teams)
        {
            // Generate the result for one fewer teams.
            int[,] results = GenerateRoundRobinOdd(num_teams - 1);

            // Copy the results into a bigger array,
            // replacing the byes with the extra team.
            int[,] results2 = new int[num_teams, num_teams - 1];
            for (int team = 0; team < num_teams - 1; team++)
            {
                for (int round = 0; round < num_teams - 1; round++)
                {
                    if (results[team, round] == BYE)
                    {
                        // Change the bye to the new team.
                        results2[team, round] = num_teams - 1;
                        results2[num_teams - 1, round] = team;
                    }
                    else
                    {
                        results2[team, round] = results[team, round];
                    }
                }
            }

            return results2;
        }


        private int[,] GenerateRoundRobinOdd(int num_teams)
        {
            int n2 = (int)((num_teams - 1) / 2);
            int[,] results = new int[num_teams, num_teams];

            // Initialize the list of teams.
            int[] teams = new int[num_teams];
            for (int i = 0; i < num_teams; i++) teams[i] = i;

            // Start the rounds.
            for (int round = 0; round < num_teams; round++)
            {
                for (int i = 0; i < n2; i++)
                {
                    int team1 = teams[n2 - i];
                    int team2 = teams[n2 + i + 1];
                    results[team1, round] = team2;
                    results[team2, round] = team1;
                }

                // Set the team with the bye.
                results[teams[0], round] = BYE;

                // Rotate the array.
                RotateArray(teams);
            }

            return results;
        }

        // Rotate the entries one position.
        private void RotateArray(int[] teams)
        {
            int tmp = teams[teams.Length - 1];
            Array.Copy(teams, 0, teams, 1, teams.Length - 1);
            teams[0] = tmp;
        }

    }
}
