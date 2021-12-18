
using FootballData.Models;
using System.Collections.Generic;

namespace FootballData.Interfaces
{
    public interface IMatchData
    {
        List<Stats> ReadJsonPremiershipMatchDataFile();
        List<MatchData> ReadJsonPremiershipMatchDataFileAndReturnMatchData();
    }
}