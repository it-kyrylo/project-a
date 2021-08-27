using Newtonsoft.Json;

namespace ProjectA.Models
{
	public class StatisticsPlayerData : Player
	{
		[JsonProperty("minutes")]
		public int MinutesInPlay { get; set; }

		[JsonProperty("goals_scored")]
		public int GoalsScored { get; set; }

		[JsonProperty("assists")]
		public int Assists { get; set; }

		[JsonProperty("yellow_cards")]
		public int YellowCards { get; set; }

		[JsonProperty("red_cards")]
		public int RedCards { get; set; }
	}
}
