using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballData.Models
{
    public class LeagueTable
    {
        public int SeasonID { get; set; }
        public int ClubID { get; set; }
        public int Played { get; set; }
        public int HomeWon { get; set; }
        public int HomeDraw { get; set; }
        public int HomeLoss { get; set; }
        public int HomeFor { get; set; }
        public int HomeAgainst { get; set; }
        public int AwayWon { get; set; }
        public int AwayDraw { get; set; }
        public int AwayLoss { get; set; }
        public int AwayFor { get; set; }
        public int AwayAgainst { get; set; }
        public int GoalDifference { get; set; }
        public int Points { get; set; }
        public DateTime MatchDate { get; set; }
    }
}
