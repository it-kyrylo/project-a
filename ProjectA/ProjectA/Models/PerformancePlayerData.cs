using Newtonsoft.Json;

namespace ProjectA.Models
{
    public class PerformancePlayerData : Player
    {
        [JsonProperty("form")]
        public double Form { get; set; }
        
        [JsonProperty("points_per_game")]
        public double PointsPerGame { get; set; }
        
        [JsonProperty("total_points")]
        public int TotalPoints { get; set; }
        
        [JsonProperty("influence_rank_type")]
        public int Influence { get; set; }
        
        [JsonProperty("creativity_rank_type")]
        public int Creativity { get; set; }
        
        [JsonProperty("threat_rank_type")]
        public int Threat { get; set; }
        
        [JsonProperty("ict_index_rank_type")]
        public int IndexRank { get; set; }

        [JsonProperty("now_cost")]
        public int Price { get; set; }
    }
}
