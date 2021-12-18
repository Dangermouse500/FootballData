using FootballData.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.Caching;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FootballData.Interfaces;
using FootballData.Helpers;

namespace FootballData.Pages
{
    public class IndexModel : PageModel
    {
        private static string _dTClubsCacheKey = "ClubsDataTableCache";
        private static string _dictClubsCacheKey = "ClubsDictCache";
        private static string _leagueTableResultsCacheKey = "LeagueTableResultsCache";
        private static string _statsCacheKey = "StatsCache";
        private static string _matchDataCacheKey = "MatchDataCache";
        private Dictionary<int, string> _clubs;
        private DataTable _dtClubs;
        private IMatchData _matchData;

        public List<LeagueTable> _leagueTableResults { get; set; }
        public List<LeagueTable> LeagueTableResults { get; set; }
        private List<Stats> Stats { get; set; }

        private List<MatchData> MatchData { get; set; }

        public IndexModel(IMatchData matchData)
        {
            _matchData = matchData;
        }

        public void OnGet()
        {
            if (DataInMemoryCache.GetFromCache(_statsCacheKey) != null)
            {
                Stats = (List<Stats>)DataInMemoryCache.GetFromCache(_statsCacheKey);
            }
            else
            {
                Stats = _matchData.ReadJsonPremiershipMatchDataFile();
            }
            if (DataInMemoryCache.GetFromCache(_matchDataCacheKey) != null)
            {
                MatchData = (List<MatchData>)DataInMemoryCache.GetFromCache(_matchDataCacheKey);
            }
            else
            {
                MatchData = _matchData.ReadJsonPremiershipMatchDataFileAndReturnMatchData();
            }

            var clubs = Stats.OrderBy(o => o.HomeTeam).Select(x => x.HomeTeam).Distinct().ToList();
            PopulateClubDataStructures(clubs);
            DataInMemoryCache.AddToCache(_dictClubsCacheKey, _clubs);
            DataInMemoryCache.AddToCache(_dTClubsCacheKey, _dtClubs);

            LeagueTableResults = ProcessData.CalculateLeagueTableBasedOnMatches(MatchData);
            DataInMemoryCache.AddToCache(_statsCacheKey, Stats);
            DataInMemoryCache.AddToCache(_matchDataCacheKey, MatchData);
            DataInMemoryCache.AddToCache(_leagueTableResultsCacheKey, LeagueTableResults);
        }

        private void PopulateClubDataStructures(List<string> clubs)
        {
            _dtClubs = new DataTable();
            _dtClubs.Columns.Add("ClubId");
            _dtClubs.Columns.Add("ClubName");
            _clubs = new Dictionary<int, string>();
            for (int i = 0; i < clubs.Count; i++)
            {
                _dtClubs.Rows.Add(i + 1, clubs[i]);
                _clubs.Add(i + 1, clubs[i]);
            }
        }

        public string GetClubName(int clubID)
        {
            if (DataInMemoryCache.GetFromCache(_dictClubsCacheKey) != null)
            {
                var dict = (Dictionary<int, string>)DataInMemoryCache.GetFromCache(_dictClubsCacheKey);
                return dict[clubID];
            }

            return string.Empty;
        }                
    }
}