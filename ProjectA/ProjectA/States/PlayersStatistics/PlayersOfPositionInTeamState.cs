using ProjectA.Models.StateOfChatModels.Enums;
using ProjectA.Services.StateProvider;
using ProjectA.Services.Statistics;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Text;
using ProjectA.Helpers;
using static ProjectA.States.StateConstants;
using System.Linq;
using ProjectA.Models.PlayersModels;
using ProjectA.Infrastructure;

namespace ProjectA.States.PlayersStatistics
{
    public class PlayersOfPositionInTeamState : IState
    {
        private readonly ICosmosDbStateProviderService _stateProvider;
        private readonly IStatisticsService _statisticsService;

        public PlayersOfPositionInTeamState(ICosmosDbStateProviderService stateProvider, IStatisticsService statisticsService)
        {
            this._stateProvider = stateProvider;
            this._statisticsService = statisticsService;
        }

        private async Task HandleRequest(ITelegramBotClient botClient, Message message, string teamName, string position)
        {
            var result = await this._statisticsService.GetPLayersOfPositionInTeamAsync(teamName, position);
            if (result == null)
            {
                await InteractionHelper.PrintMessage(botClient, message.Chat.Id, "Wrong team name or position");
            }

            StringBuilder stringBuilder = new StringBuilder();
            int counter = 1;
            stringBuilder.Append($"Player Name");
            foreach (Element player in result)
            {
                stringBuilder.Append($"{counter}. {KeyBuilder.Build(player.First_Name, player.Second_Name)}");
                stringBuilder.AppendLine();
                counter++;
            }

            await InteractionHelper.PrintMessage(botClient, message.Chat.Id, stringBuilder.ToString());
        }

        private string[] HandleInput(string inputText)
        {
            string[] splited = inputText.Split(' ');
            string[] result = new string[2];
            result[0] = string.Join(" ", splited.Take(splited.Length - 1));
            result[1] = splited.Last();
            return result;
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

            string[] splittedInput = this.HandleInput(message.Text);
            string teamName = splittedInput[0];
            string position = splittedInput[1];

            //var chat = await _stateProvider.GetChatStateAsync(message.Chat.Id);

            //await _stateProvider.UpdateChatStateAsync(chat);

            await this.HandleRequest(botClient, message, teamName, position);

            return StateType.StatisticsMenuState;
        }

        public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        {
            await botClient.SendTextMessageAsync(chatId, StateMessages.InsertPlayersSuggestionsPreferences);
        }
    }
}
