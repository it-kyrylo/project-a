using Newtonsoft.Json;


namespace ProjectA.Models.Teams
{
    public class Team
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("win")]
        public int Win { get; set; }

        [JsonProperty("loss")]
        public int Loss { get; set; }

        [JsonProperty("draw")]
        public int Draw { get; set; }

        [JsonProperty("strength")]
        public int Strength { get; set; }

        [JsonProperty("strength_overall_home")]
        public int StrengthHome { get; set; }

        [JsonProperty("strength_overall_away")]
        public int StrengthAway { get; set; }
    }
}
