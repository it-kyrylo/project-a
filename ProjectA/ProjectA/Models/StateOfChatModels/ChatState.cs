using Newtonsoft.Json;
using ProjectA.Models.StateOfChatModels.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectA.Models.StateOfChatModels
{
    public class ChatState
    {
        public ChatState(long chatId, StateType state = StateType.MainState)
        {
            Id = Guid.NewGuid().ToString();
            Chat_Id = chatId;
            Current_State = state;
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [Required]
        [JsonProperty("chat_id")]
        public long Chat_Id { get; set; }

        [Required]
        [JsonProperty("current_state")]
        public StateType Current_State { get; set; }
    }
}
