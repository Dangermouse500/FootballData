using Newtonsoft.Json;
using System.Collections.Generic;

namespace FootballData.Models
{
    public class ClubResponse
    {
        [JsonProperty("selectedClubID")]
        public int SelectedClubID { get; set; }
    }
}