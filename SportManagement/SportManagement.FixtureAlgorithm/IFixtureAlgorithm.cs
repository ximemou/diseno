using SportManagement.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportManagement.FixtureAlgorithm
{
    public interface IFixtureAlgorithm
    {
        List<Game> GenerateGames(IEnumerable<Team> teams, DateTime date, Sport sport);
    }
}
