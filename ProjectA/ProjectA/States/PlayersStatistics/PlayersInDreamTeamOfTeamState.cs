using ProjectA.Models.StateOfChatModels.Enums;
using ProjectA.Services.StateProvider;
using ProjectA.Services.Statistics;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Text;
using static ProjectA.States.StateConstants;
using ProjectA.Services.Statistics.ServiceModels;

namespace ProjectA.States.PlayersStatistics
{
    public class PlayersInDreamTeamOfTeamState : IState
    {
        private readonly ICosmosDbStateProviderService _stateProvider;
        private readonly IStatisticsService _statisticsService;

        public PlayersInDreamTeamOfTeamState(ICosmosDbStateProviderService stateProvider, IStatisticsService statisticsService)
        {
            this._stateProvider = stateProvider;
            this._statisticsService = statisticsService;
        }

        private async Task<string> HandleRequest(ITelegramBotClient botClient, Message message, string teamName)
        {
            var result = await this._statisticsService.PlayersInDreamTeamOfTeamAsync(teamName);
            if (result == null)
            {
                return "Wrong team name";
            }

            StringBuilder stringBuilder = new StringBuilder();
            int counter = 1;
            stringBuilder.Append($"Player Name - Position - In dreamteam");
            stringBuilder.AppendLine();
            foreach (PlayerDreamTeamData player in result)
            {
                stringBuilder.Append($"{counter}. {player.PlayerName} - {player.PlayerPosition} - {player.DreamTeamCount}");
                stringBuilder.AppendLine();
                counter++;
            }

            return stringBuilder.ToString();
        }

        public async Task<StateType> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(callbackQueryId: callbackQuery.Id);

            return StateType.PlayersInDreamTeamOfTeamState;
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
