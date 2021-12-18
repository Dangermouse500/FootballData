using FootballData.Helpers;
using FootballData.Interfaces;
using FootballData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballData.Pages
{
    public class ClubFormGuideModel : PageModel
    {
        private IMatchData _matchData;
        private IHtmlHelper _htmlHelper;

        private static string _dictClubsCacheKey = "ClubsDictCache";
        private static string _matchDataCacheKey = "MatchDataCache";
        private static string _statsCacheKey = "StatsCache";

        [BindProperty]
        public Clubs Club { get; set; }
        public List<SelectListItem> Clubs { get; set; }

        private Dictionary<int, string> _clubsDictionary;
        public List<string> ClubForm { get; set; }
        public Dictionary<int, string> ClubsDictionary
        {
            get
            {
                if (DataInMemoryCache.GetFromCache(_dictClubsCacheKey) != null)
                {
                    return (Dictionary<int, string>)DataInMemoryCache.GetFromCache(_dictClubsCacheKey);
                }
                else
                {
                    var clubs = Stats.OrderBy(o => o.HomeTeam).Select(x => x.HomeTeam).Distinct().ToList();
                    for (int i = 0; i < clubs.Count; i++)
                    {
                        _clubsDictionary.Add(i + 1, clubs[i]);
                    }
                    DataInMemoryCache.AddToCache(_dictClubsCacheKey, _clubsDictionary);

                    return _clubsDictionary;
                }
            }
        }

        public List<Stats> Stats
        {
            get
            {
                if (DataInMemoryCache.GetFromCache(_statsCacheKey) != null)
                {
                    return (List<Stats>)DataInMemoryCache.GetFromCache(_statsCacheKey);
                }
                else
                {
                    var stats = _matchData.ReadJsonPremiershipMatchDataFile();
                    DataInMemoryCache.AddToCache(_statsCacheKey, stats);

                    return stats;
                }
            }
        }

        public List<MatchData> Matches
        {
            get
            {
                if (DataInMemoryCache.GetFromCache(_matchDataCacheKey) != null)
                {
                    return (List<MatchData>)DataInMemoryCache.GetFromCache(_matchDataCacheKey);
                }
                else
                {
                    var matchData = _matchData.ReadJsonPremiershipMatchDataFileAndReturnMatchData();
                    DataInMemoryCache.AddToCache(_matchDataCacheKey, matchData);

                    return matchData;
                }
            }
        }

        public List<ClubsForm> _allClubsForm { get; set; }
        public int ClubID { get; set; }

        [BindProperty]
        public string selectedFilter { get; set; }

        public ClubFormGuideModel(IMatchData matchData, IHtmlHelper htmlHelper)
        {
            _matchData = matchData;
            _htmlHelper = htmlHelper;
        }

        public void OnGet(int clubId)
        {
            ClubID = clubId;
            Clubs = new List<SelectListItem>();
            foreach (var clubData in ClubsDictionary)
            {
                Clubs.Add(new SelectListItem { Value = clubData.Key.ToString(), Text = clubData.Value });
            }
            if (clubId > 0)
            {
                Clubs.Where(x => x.Value == clubId.ToString()).FirstOrDefault().Selected = true;

                _allClubsForm = ProcessData.CalculateClubsFormBasedOnMatches(Matches);

                ClubForm = _allClubsForm.Where(c => c.ClubID == clubId).Select(cl => cl.Results).ToList().FirstOrDefault();
            }
        }

        public PartialViewResult OnGetUpdateClubFormForPartialView()
        {
            var queryString = QueryHelpers.ParseQuery(Request.QueryString.Value);
            var selectedClubID = Convert.ToInt16(queryString.Where(x => x.Key == "selectedClubID").FirstOrDefault().Value.ToString());

            _allClubsForm = ProcessData.CalculateClubsFormBasedOnMatches(Matches);
            ClubForm = _allClubsForm.Where(c => c.ClubID == selectedClubID).Select(cl => cl.Results).ToList().FirstOrDefault();

            return Partial("ClubForm", ClubForm);
        }

        /// <summary>
        /// This was only used when using the Script tag in markup and Post method
        /// </summary>
        public void OnPost()
        {
            var json = Request.Form.Keys.FirstOrDefault();
            var clubResponse = JsonConvert.DeserializeObject<ClubResponse>(json);

            _allClubsForm = ProcessData.CalculateClubsFormBasedOnMatches(Matches);
            ClubForm = _allClubsForm.Where(c => c.ClubID == clubResponse.SelectedClubID).Select(cl => cl.Results).ToList().FirstOrDefault();
        }
    }
}