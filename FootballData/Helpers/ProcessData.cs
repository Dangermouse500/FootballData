using FootballData.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FootballData.Helpers
{
    public static class ProcessData
    {
        private static string _dTClubsCacheKey = "ClubsDataTableCache";

        public static List<LeagueTable> CalculateLeagueTableBasedOnMatches(List<MatchData> matches)
        {
            var leagueTableResults = new List<LeagueTable>();
            foreach (var match in matches)
            {
                ProcessHomeTeamResult(match, ref leagueTableResults);
                ProcessAwayTeamResult(match, ref leagueTableResults);
            }
            var LeagueTableResults = leagueTableResults.OrderByDescending(p => p.Points).ThenByDescending(g => g.GoalDifference).ToList();
            return LeagueTableResults;
        }

        private static void ProcessHomeTeamResult(MatchData match, ref List<LeagueTable> leagueTableResults)
        {
            LeagueTable lt;
            var dtClubs = new DataTable();
            if (DataInMemoryCache.GetFromCache(_dTClubsCacheKey) != null)
            {
                dtClubs = (DataTable)DataInMemoryCache.GetFromCache(_dTClubsCacheKey);
            }

            DataView dvClubs = new DataView(dtClubs);
            dvClubs.RowFilter = $"ClubName = '{match.HomeTeam}'";

            int clubId = Convert.ToInt16(dvClubs[0]["ClubId"]);
            if (!leagueTableResults.Any(res => res.ClubID == clubId))
            {
                lt = new LeagueTable();
                lt.ClubID = clubId;
                lt.SeasonID = 1;
            }
            else
            {
                lt = leagueTableResults.Where(res => res.ClubID == clubId).First();
            }

            lt.Played++;
            lt.HomeFor += Convert.ToInt16(match.FTHG);
            lt.HomeAgainst += Convert.ToInt16(match.FTAG);

            int homeGoals = Convert.ToInt16(match.FTHG);
            int awayGoals = Convert.ToInt16(match.FTAG);

            if (homeGoals > awayGoals)
            {
                lt.HomeWon++;
                lt.Points += 3;
            }
            else if (homeGoals == awayGoals)
            {
                lt.HomeDraw++;
                lt.Points += 1;
            }
            else
            {
                lt.HomeLoss++;
            }

            lt.GoalDifference += (match.FTHG - match.FTAG);

            if (!leagueTableResults.Any(res => res.ClubID == clubId))
            {
                leagueTableResults.Add(lt);
            }
        }

        private static void ProcessAwayTeamResult(MatchData match, ref List<LeagueTable> leagueTableResults)
        {
            LeagueTable lt;

            var dtClubs = new DataTable();
            if (DataInMemoryCache.GetFromCache(_dTClubsCacheKey) != null)
            {
                dtClubs = (DataTable)DataInMemoryCache.GetFromCache(_dTClubsCacheKey);
            }
            DataView dvClubs = new DataView(dtClubs);
            dvClubs.RowFilter = $"ClubName = '{match.AwayTeam}'";

            int clubId = Convert.ToInt16(dvClubs[0]["ClubId"]);
            if (!leagueTableResults.Any(res => res.ClubID == clubId))
            {
                lt = new LeagueTable();
                lt.ClubID = clubId;
                lt.SeasonID = 1;
            }
            else
            {
                lt = leagueTableResults.Where(res => res.ClubID == clubId).First();
            }

            lt.Played++;
            lt.AwayFor += Convert.ToInt16(match.FTAG);
            lt.AwayAgainst += Convert.ToInt16(match.FTHG);

            int homeGoals = Convert.ToInt16(match.FTHG);
            int awayGoals = Convert.ToInt16(match.FTAG);

            if (awayGoals > homeGoals)
            {
                lt.AwayWon++;
                lt.Points += 3;
            }
            else if (homeGoals == awayGoals)
            {
                lt.AwayDraw++;
                lt.Points += 1;
            }
            else
            {
                lt.AwayLoss++;
            }

            lt.GoalDifference += (match.FTAG - match.FTHG);

            if (!leagueTableResults.Any(res => res.ClubID == clubId))
            {
                leagueTableResults.Add(lt);
            }
        }

        public static int GetClubIDFromClubName(string clubName)
        {
            var dtClubs = new DataTable();
            if (DataInMemoryCache.GetFromCache(_dTClubsCacheKey) != null)
            {
                dtClubs = (DataTable)DataInMemoryCache.GetFromCache(_dTClubsCacheKey);
            }

            DataView dvClubs = new DataView(dtClubs);
            dvClubs.RowFilter = $"ClubName = '{clubName}'";

            return Convert.ToInt16(dvClubs[0]["ClubId"]);
        }

        public static List<ClubsForm> CalculateClubsFormBasedOnMatches(List<MatchData> matches)
        {
            var clubsForm = new List<ClubsForm>();
            foreach (var match in matches)
            {
                ProcessHomeTeamForm(match, ref clubsForm);
                ProcessAwayTeamForm(match, ref clubsForm);
            }
            
            return clubsForm;
        }

        private static void ProcessHomeTeamForm(MatchData md, ref List<ClubsForm> clubsForm)
        {
            ClubsForm cf;

            int clubId = ProcessData.GetClubIDFromClubName(md.HomeTeam);
            if (!clubsForm.Any(res => res.ClubID == clubId))
            {
                cf = new ClubsForm();
                cf.ClubID = clubId;
                cf.Results = new List<string>();
            }
            else
            {
                cf = clubsForm.Where(res => res.ClubID == clubId).First();
            }

            if (md.FTHG > md.FTAG)
            {
                cf.Results.Add("W");
            }
            else if (md.FTHG == md.FTAG)
            {
                cf.Results.Add("D");
            }
            else
            {
                cf.Results.Add("L");
            }

            if (!clubsForm.Any(res => res.ClubID == clubId))
            {
                clubsForm.Add(cf);
            }
        }

        private static void ProcessAwayTeamForm(MatchData md, ref List<ClubsForm> clubsForm)
        {
            ClubsForm cf;

            int clubId = ProcessData.GetClubIDFromClubName(md.AwayTeam);
            if (!clubsForm.Any(res => res.ClubID == clubId))
            {
                cf = new ClubsForm();
                cf.ClubID = clubId;
                cf.Results = new List<string>();
            }
            else
            {
                cf = clubsForm.Where(res => res.ClubID == clubId).First();
            }

            if (md.FTAG > md.FTHG)
            {
                cf.Results.Add("W");
            }
            else if (md.FTAG == md.FTHG)
            {
                cf.Results.Add("D");
            }
            else
            {
                cf.Results.Add("L");
            }

            if (!clubsForm.Any(res => res.ClubID == clubId))
            {
                clubsForm.Add(cf);
            }
        }
    }
}