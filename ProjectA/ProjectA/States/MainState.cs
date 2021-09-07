using ProjectA.Models.StateOfChatModels.Enums;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ProjectA.States
{
    public class MainState : IState
    {
        public async Task<StateType> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            if (message.Type != MessageType.Text)
            {
                return StateType.MainState;
            }

            return message.Text switch
            {
                //TODO: Add players Statitics and Teams Statistics menue states
                //TODO: Introduce StatesConstants class

                "/PlayersSuggestion" => StateType.SuggestionsMenuState,
                //"/PlayersStatistics" => StateType.GetSuggestion,
                //"/TeamsStatistics" => StateType.GetSuggestion,
                _ => await PrintMessage(botClient, message.Chat.Id, "Please choose on of the options", StateType.MainState)
            };
        }

        public Task<StateType> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
            => Task.FromResult(StateType.MainState);

        public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        {
            //TODO: Introduce StatesConstants class
            await botClient.SendTextMessageAsync(chatId, $"Please choose on of the options:\n" +
                                                   $"/PlayersSuggestion\n" +
                                                   $"/PlayersStatistics\n" +
                                                   $"/TeamsStatistics");
        }

        private static async Task<StateType> PrintMessage(ITelegramBotClient botClient, long chatId, string message, StateType returnState)
        {
            await botClient.SendTextMessageAsync(chatId, message);

            return returnState;
        }
    }
}
