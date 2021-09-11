using ProjectA.Models.StateOfChatModels.Enums;
using ProjectA.Services.StateProvider;
using ProjectA.Services.Statistics;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Text;
using static ProjectA.States.StateConstants;

namespace ProjectA.States.PlayersStatistics
{
    public class TimesPlayerHasBeenInDreamTeamState : IState
    {
        private readonly ICosmosDbStateProviderService _stateProvider;
        private readonly IStatisticsService _statisticsService;

        public TimesPlayerHasBeenInDreamTeamState(ICosmosDbStateProviderService stateProvider, IStatisticsService statisticsService)
        {
            this._stateProvider = stateProvider;
            this._statisticsService = statisticsService;
        }

        private async Task<string> HandleRequest(ITelegramBotClient botClient, Message message, string playerName)
        {
            var result = await this._statisticsService.TimesPlayerHasBeenInDreamTeamAsync(playerName);
            if (result == -1)
            {
                return "Wrong player name";
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"{playerName}: {result}");
            stringBuilder.AppendLine();

            return stringBuilder.ToString();
        }

        public async Task<StateType> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(callbackQueryId: callbackQuery.Id);

            return StateType.TimesPlayerHasBeenInDreamTeamState;
        }

        public async Task<StateType> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            if (message.Text == null)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, StateMessages.InsertPlayersSuggestionsPreferences);
                return StateType.StatisticsMenuState;
            }

            string result = await this.HandleRequest(botClient, message, message.Text);
            await botClient.SendTextMessageAsync(message.Chat.Id, result);

            return StateType.StatisticsMenuState;
        }

        public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        {
            await botClient.SendTextMessageAsync(chatId, StateMessages.InsertPlayersSuggestionsPreferences);
        }
    }
}
