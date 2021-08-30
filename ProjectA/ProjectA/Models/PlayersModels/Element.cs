using Newtonsoft.Json;

namespace ProjectA.Models.PlayersModels
{
    public class Element
    {
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("first_name")]
		public string FirstName { get; set; }

		[JsonProperty("second_name")]
		public string LastName { get; set; }

		[JsonProperty("element_type")]
		public int GamePosition { get; set; }

		[JsonProperty("minutes")]
		public int MinutesInPlay { get; set; }

		[JsonProperty("team")]
		public int CurrentTeam { get; set; }

		[JsonProperty("goals_scored")]
		public int GoalsScored { get; set; }

		[JsonProperty("assists")]
		public int Assists { get; set; }

		[JsonProperty("yellow_cards")]
		public int YellowCards { get; set; }

		[JsonProperty("red_cards")]
		public int RedCards { get; set; }

		[JsonProperty("form")]
		private string FormStr
        {
            set
            {
				Form = double.Parse(value);
			}
        }

		[JsonIgnore]
		public double Form { get; set; }

		[JsonProperty("points_per_game")]
		private string PointsPerGameStr 
		{
            set
            {
				PointsPerGame = double.Parse(value);
            }
		}

		[JsonIgnore]
		public double PointsPerGame  { get; set; }

		[JsonProperty("total_points")]
		public int TotalPoints { get; set; }

		[JsonProperty("influence_rank_type")]
		private string InfluenceStr 
		{
            set
            {
				Influence = int.Parse(value);
            }
		}

		[JsonIgnore]
		public int Influence { get; set; }

		[JsonProperty("creativity_rank_type")]
		private string CreativityStr 
		{
            set
            {
				Creativity = int.Parse(value);

			}
		}

		[JsonIgnore]
		public int Creativity { get; set; }

		[JsonProperty("threat_rank_type")]
		private string ThreatStr 
		{
            set
            {
				Threat = int.Parse(value);
            }
		}

		[JsonIgnore]
		public int Threat { get; set; }

		[JsonProperty("ict_index_rank_type")]
		public int IndexRank { get; set; }

		[JsonProperty("now_cost")]
		public int Price { get; set; }
    }
}
