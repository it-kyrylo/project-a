using Newtonsoft.Json;

namespace ProjectA.Models
{
    public class Player
    {
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("first_name")]
		public string FirstName { get; set; }

		[JsonProperty("second_name")]
		public string LastName { get; set; }

		[JsonProperty("element_type")]
		public int GamePosition { get; set; }
	}
}
