using Newtonsoft.Json;
using ProjectA.Models.StateOfChatModels.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjectA.Models.StateOfChatModels
{
    public class ChatState
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [Required]
        [JsonProperty("chat_id")]
        public long Chat_Id { get; set; }

        [Required]
        [JsonProperty("current_state")]
        public StateTypes Current_State { get; set; }
    }
}
