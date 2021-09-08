using ProjectA.Models.StateOfChatModels.Enums;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static ProjectA.States.StateConstants;

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

                Suggestions.PlayersSuggestions => StateType.SuggestionsMenuState,
                Statistics.PlayersStatistics => StateType.StatisticsMenuState,
                //Statistics.TeamStatistics => StateType.GetSuggestion,
                _ => await PrintMessage(botClient, message.Chat.Id, StateMessages.ChooseOptionMainState, StateType.MainState)
            };
        }

        public Task<StateType> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
            => Task.FromResult(StateType.MainState);

        public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        {
            //TODO: Introduce StatesConstants class
            await botClient.SendTextMessageAsync(chatId, $"{StateMessages.ChooseOptionMainState}\n" +
                                                   $"{Suggestions.PlayersSuggestions}\n" +
                                                   $"{Statistics.PlayersStatistics}\n" +
                                                   $"{Statistics.TeamStatistics}");
        }

        private static async Task<StateType> PrintMessage(ITelegramBotClient botClient, long chatId, string message, StateType returnState)
        {
            await botClient.SendTextMessageAsync(chatId, message);

            return returnState;
        }
    }
}
