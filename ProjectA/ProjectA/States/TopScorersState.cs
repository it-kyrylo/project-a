using ProjectA.Models.StateOfChatModels.Enums;
using ProjectA.Services.StateProvider;
using ProjectA.Services.Statistics;
using ProjectA.Infrastructure;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Text;
using ProjectA.Services.Statistics.ServiceModels;

namespace ProjectA.States
{
    public class TopScorersState : IState
    {
        private readonly ICosmosDbStateProviderService _stateProvider;
        private readonly IStatisticsService _statisticsService;

        public TopScorersState(ICosmosDbStateProviderService stateProvider, IStatisticsService statisticsService)
        {
            this._stateProvider = stateProvider;
            this._statisticsService = statisticsService;
        }

        private async Task HandleRequest(ITelegramBotClient botClient, Message message, int topScorers)
        {
            var result = await this._statisticsService.GetTopScorersAsync(topScorers);
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

            if (!int.TryParse(message.Text, out int topScorers))
            {
                return await BotPrintMessage.PrintMessage(botClient, message.Chat.Id, "Wrong preferences format");
            }

            var chat = await _stateProvider.GetChatStateAsync(message.Chat.Id);

            await _stateProvider.UpdateChatStateAsync(chat);

            await this.HandleRequest(botClient, message, topScorers);

            return StateType.MainState;
        }

        public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        {
            await botClient.SendTextMessageAsync(chatId, $"Please enter your preferences:\n(position/min Price/max Price)");
        }
    }
}