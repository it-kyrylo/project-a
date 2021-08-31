using Newtonsoft.Json;

namespace ProjectA.Models.PlayersModels
{
    public class Element
    {
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("first_name")]
		public string First_Name { get; set; }

		[JsonProperty("second_name")]
		public string Second_Name { get; set; }

		[JsonProperty("element_type")]
		public int Element_Type { get; set; }

		[JsonProperty("minutes")]
		public int Minutes { get; set; }

		[JsonProperty("team")]
		public int Team { get; set; }

		[JsonProperty("goals_scored")]
		public int Goals_Scored { get; set; }

		[JsonProperty("assists")]
		public int Assists { get; set; }

		[JsonProperty("yellow_cards")]
		public int Yellow_Cards { get; set; }

		[JsonProperty("red_cards")]
		public int Red_Cards { get; set; }

		[JsonProperty("form")]
		public string Form { get; set; }

		[JsonProperty("points_per_game")]
		public string Points_Per_Game { get; set; }

		[JsonProperty("total_points")]
		public int Total_Points { get; set; }

		[JsonProperty("influence_rank_type")]
		public int Influence_Rank_Type { get; set; }

		[JsonProperty("creativity_rank_type")]
		public int Creativity_Rank_Type { get; set; }

		[JsonProperty("threat_rank_type")]
		public int Threat_Rank_Type { get; set; }

		[JsonProperty("ict_index_rank_type")]
		public int Ict_Index_Rank_Type { get; set; }

		[JsonProperty("now_cost")]
		public int Now_Cost { get; set; }

		[JsonProperty("dreamteam_count")]
		public int Dreamteam_Count { get; set; }
	}
}
