using FootballData.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballData.Interfaces
{
    public class InMemoryMatchData : IMatchData
    {                
        private const string pathToJsonFile = @"/Resources/premierLeague.json";

        public List<Stats> ReadJsonPremiershipMatchDataFile()
        {
            string fullPathToJsonFile = System.IO.Directory.GetCurrentDirectory() + pathToJsonFile;
            string jsonString = System.IO.File.ReadAllText(fullPathToJsonFile);
            List<Stats> stats = JsonConvert.DeserializeObject<List<Stats>>(jsonString);
            return stats;
        }

        public List<MatchData> ReadJsonPremiershipMatchDataFileAndReturnMatchData()
        {
            string fullPathToJsonFile = System.IO.Directory.GetCurrentDirectory() + pathToJsonFile;
            string jsonString = System.IO.File.ReadAllText(fullPathToJsonFile);
            List<Stats> stats = JsonConvert.DeserializeObject<List<Stats>>(jsonString);
            return stats.Select(s => new MatchData { FTAG = s.FTAG, FTHG = s.FTHG, MatchDate = DateTime.Parse(s.Date), HomeTeam = s.HomeTeam, AwayTeam = s.AwayTeam }).ToList(); ;
        }
    }
}