using ProjectA.Models.StateOfChatModels.Enums;
using ProjectA.Services.StateProvider;
using ProjectA.Services.Statistics;
using ProjectA.Infrastructure;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Text;
using ProjectA.Services.Statistics.ServiceModels;
using System.Linq;

namespace ProjectA.States
{
    public class TopScorersInTeamMenuState : IState
    {
        private readonly ICosmosDbStateProviderService _stateProvider;
        private readonly IStatisticsService _statisticsService;

        public TopScorersInTeamMenuState(ICosmosDbStateProviderService stateProvider, IStatisticsService statisticsService)
        {
            this._stateProvider = stateProvider;
            this._statisticsService = statisticsService;
        }

        private async Task HandleRequest(ITelegramBotClient botClient, Message message, string teamName, int topScorers)
        {
            var result = await this._statisticsService.GetTopScorersInATeamAsync(teamName, topScorers);
            if (result == null)
            {
                await BotPrintMessage.PrintMessage(botClient, message.Chat.Id, "Negative number or zero inputted");
            }
            StringBuilder stringBuilder = new StringBuilder();

            int counter = 1;
            stringBuilder.Append($"Player Name               Scored Goals");
            foreach (ScorersData scorer in result)
            {
                stringBuilder.Append($"{counter}. {scorer.PlayerName} - {scorer.ScoredGoals}");
                stringBuilder.AppendLine();
                counter++;
            }
            await BotPrintMessage.PrintMessage(botClient, message.Chat.Id, stringBuilder.ToString());
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
                return await BotPrintMessage.PrintMessage(botClient, message.Chat.Id, "Please insert your preferences");
            }

            string[] splittedInput = this.HandleInput(message.Text);
            if (!int.TryParse(splittedInput[1], out int topScorers))
            {
                return await BotPrintMessage.PrintMessage(botClient, message.Chat.Id, "Wrong preferences format");
            }
            string teamName = splittedInput[0];

            var chat = await _stateProvider.GetChatStateAsync(message.Chat.Id);

            await _stateProvider.UpdateChatStateAsync(chat);

            await this.HandleRequest(botClient, message, teamName, topScorers);

            return StateType.MainState;
        }

        public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        {
            await botClient.SendTextMessageAsync(chatId, $"Please enter your preferences:\n(position/min Price/max Price)");
        }
    }
}
