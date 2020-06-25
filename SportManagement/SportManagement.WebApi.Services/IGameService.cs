using SportManagement.Data;
using SportManagement.FixtureAlgorithm;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportManagement.WebApi.Services
{
    public interface IGameService
    {
        Game GetGameById(int gameId);
        IEnumerable<Game> GetAllGames();
        Game CreateGame(Game gameToCreate);
        Game UpdateGame(int gameId, Game gameToUpdate);
        bool DeleteGame(int gameId);

        bool GenerateGamesWithAlgorithm(int sportId, DateTime date, string algorithm);

        IEnumerable<Game> GetGamesForSport(int sportId);
        IEnumerable<Game> GetGamesForTeam(int teamId);
    }
}
