using ProjectA.Models.StateOfChatModels.Enums;
using ProjectA.Services.StateProvider;
using ProjectA.Services.Statistics;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Text;
using ProjectA.Helpers;
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

        private async Task HandleRequest(ITelegramBotClient botClient, Message message, string playerName)
        {
            var result = await this._statisticsService.TimesPlayerHasBeenInDreamTeamAsync(playerName);
            if (result == -1)
            {
                await InteractionHelper.PrintMessage(botClient, message.Chat.Id, "Wrong player name");
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"{playerName}: {result}");
            stringBuilder.AppendLine();

            await InteractionHelper.PrintMessage(botClient, message.Chat.Id, stringBuilder.ToString());
        }

        public async Task<StateType> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(callbackQueryId: callbackQuery.Id);

            return StateType.TopScorersState;
        }

        public async Task<StateType> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            if (message.Text == null)
            {
                return await InteractionHelper.PrintMessage(botClient, message.Chat.Id, StateMessages.InsertPlayersSuggestionsPreferences);
            }

            //var chat = await _stateProvider.GetChatStateAsync(message.Chat.Id);

            //await _stateProvider.UpdateChatStateAsync(chat);

            await this.HandleRequest(botClient, message, message.Text);

            return StateType.StatisticsMenuState;
        }

        public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        {
            await botClient.SendTextMessageAsync(chatId, StateMessages.InsertPlayersSuggestionsPreferences);
        }
    }
}
