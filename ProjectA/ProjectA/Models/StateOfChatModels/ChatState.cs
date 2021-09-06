using Newtonsoft.Json;
using ProjectA.Models.StateOfChatModels.Enums;

namespace ProjectA.Models.StateOfChatModels
{
    public class ChatState
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("chat_id")]
        public long Chat_Id { get; set; }

        [JsonProperty("current_state")]
        public StateTypes Current_State { get; set; }
    }
}
