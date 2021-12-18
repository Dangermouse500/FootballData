using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballData.Models
{
    public class MatchData
    {
        public int FTAG { get; set; }
        public int FTHG { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public DateTime MatchDate { get; set; }
    }
}
