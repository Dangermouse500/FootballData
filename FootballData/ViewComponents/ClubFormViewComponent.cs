using FootballData.Helpers;
using FootballData.Interfaces;
using FootballData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballData.ViewComponents
{
    public class ClubFormViewComponent: ViewComponent
    {
        private IMatchData _matchData;
        private IHtmlHelper _htmlHelper;
        private static string _matchDataCacheKey = "MatchDataCache";
        public List<string> ClubForm { get; set; }
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

        public ClubFormViewComponent(IMatchData matchData, IHtmlHelper htmlHelper)
        {
            _matchData = matchData;
            _htmlHelper = htmlHelper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int clubID)
        {
            var queryString = QueryHelpers.ParseQuery(Request.QueryString.Value);
            var selectedClubID = Convert.ToInt16(queryString.Where(x => x.Key == "clubId").FirstOrDefault().Value.ToString());

            _allClubsForm = ProcessData.CalculateClubsFormBasedOnMatches(Matches);
            ClubForm = _allClubsForm.Where(c => c.ClubID == selectedClubID).Select(cl => cl.Results).ToList().FirstOrDefault();

            return View("Default", ClubForm);
        }

        public IViewComponentResult OnGetClubFormDetailsComponent(int clubID)
        {
            var queryString = QueryHelpers.ParseQuery(Request.QueryString.Value);
            var selectedClubID = Convert.ToInt16(queryString.Where(x => x.Key == "clubId").FirstOrDefault().Value.ToString());

            _allClubsForm = ProcessData.CalculateClubsFormBasedOnMatches(Matches);
            ClubForm = _allClubsForm.Where(c => c.ClubID == selectedClubID).Select(cl => cl.Results).ToList().FirstOrDefault();

            return View("Default", ClubForm);
        }
    }
}