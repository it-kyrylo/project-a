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
    public class PlayerDataState : IState
    {
        private readonly ICosmosDbStateProviderService _stateProvider;
        private readonly IStatisticsService _statisticsService;

        public PlayerDataState(ICosmosDbStateProviderService stateProvider, IStatisticsService statisticsService)
        {
            this._stateProvider = stateProvider;
            this._statisticsService = statisticsService;
        }

        private string GetPositionName(int positionIndex)
        {
            switch (positionIndex)
            {
                case 1:
                    return "Goalkeeper";
                case 2:
                    return "Defender";
                case 3:
                    return "Midfielder";
                case 4:
                    return "Forward";
                default:
                    return "No such position";
            }
        }

        private async Task HandleRequest(ITelegramBotClient botClient, Message message, string playerName)
        {
            var result = await this._statisticsService.GetPayerData(playerName);
            if (result == null)
            {
                await InteractionHelper.PrintMessage(botClient, message.Chat.Id, "Wrong player's name");
                return;
            }

            string position = GetPositionName(result.Element_Type);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Name: {playerName}");
            stringBuilder.AppendLine();
            stringBuilder.Append($"Player's position: {position}");
            stringBuilder.AppendLine();
            stringBuilder.Append($"Minutes played: {result.Minutes}");
            stringBuilder.AppendLine();
            stringBuilder.Append($"Scored goals: {result.Goals_Scored}");
            stringBuilder.AppendLine();
            stringBuilder.Append($"Assists: {result.Assists}");
            stringBuilder.AppendLine();
            stringBuilder.Append($"Red cards: {result.Red_Cards}");
            stringBuilder.AppendLine();
            stringBuilder.Append($"Yellow cards: {result.Yellow_Cards}");
            stringBuilder.AppendLine();
            stringBuilder.Append($"Times in dreamteam: {result.Dreamteam_Count}");
            stringBuilder.AppendLine();

            await InteractionHelper.PrintMessage(botClient, message.Chat.Id, stringBuilder.ToString());
        }

        public async Task<StateType> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(callbackQueryId: callbackQuery.Id);

            return StateType.PlayerDataState;
        }

        public async Task<StateType> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            if (message.Text == null)
            {
                return await InteractionHelper.PrintMessage(botClient, message.Chat.Id, StateMessages.InsertPlayersSuggestionsPreferences);
            }

            await this.HandleRequest(botClient, message, message.Text);

            return StateType.StatisticsMenuState;
        }

        public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        {
            await botClient.SendTextMessageAsync(chatId, StateMessages.InsertPlayersSuggestionsPreferences);
        }
    }
}
