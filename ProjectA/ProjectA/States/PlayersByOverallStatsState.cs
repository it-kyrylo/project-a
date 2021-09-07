using ProjectA.Models.StateOfChatModels;
using ProjectA.Models.StateOfChatModels.Enums;
using ProjectA.Services.PlayersSuggestion;
using ProjectA.Services.StateProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ProjectA.States
{
    public class PlayersByOverallStatsState : IState
    {
        private readonly ICosmosDbStateProviderService _stateProvider;
        private readonly IPlayerSuggestionService players;

        public PlayersByOverallStatsState(
            ICosmosDbStateProviderService stateProvider, 
            IPlayerSuggestionService players)
        {
            _stateProvider = stateProvider;
            this.players = players;
        }

        public async Task<StateType> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            if (message.Text == null)
            {
                return await PrintMessage(botClient, message.Chat.Id, "Please insert your preferences");
            }

            var userInputParsed = ParseUserInput(message.Text);

            if (userInputParsed.Length != 3 ||
                !double.TryParse(userInputParsed[1], out double minPrice) ||
                !double.TryParse(userInputParsed[2], out double maxPrice))
            {
                return await PrintMessage(botClient, message.Chat.Id, "Wrong preferences format");
            }

            var position = userInputParsed[0];

            var chat = await _stateProvider.GetChatStateAsync(message.Chat.Id);

            await _stateProvider.UpdateChatStateAsync(chat);
            var suggestionResult = await GetSuggestionAsStringAsync(minPrice, maxPrice, position);

            await PrintMessage(botClient, message.Chat.Id, suggestionResult);

            return StateType.MainState;
        }

        private async Task<string> GetSuggestionAsStringAsync(double minPrice, double maxPrice, string position)
        {
            var suggestedPlayers = await this.players.GetByOverallStats(position, minPrice, maxPrice);

            var sb = new StringBuilder();

            foreach (var player in suggestedPlayers)
            {
                sb.AppendLine(player.ToString());
            }

            return sb.ToString();
        }

        public async Task<StateType> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(callbackQueryId: callbackQuery.Id);

            return StateType.PlayersByOverallStatsState;
        }

        public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        {
            await botClient.SendTextMessageAsync(chatId, $"Please enter your preferences:\n(position/min Price/max Price)");
        }

        private static async Task<StateType> PrintMessage(ITelegramBotClient botClient, long chatId, string message)
        {
            await botClient.SendTextMessageAsync(chatId, message);

            return StateType.PlayersByOverallStatsState;
        }

        private string[] ParseUserInput(string userInput) 
            => userInput
                .Split('/', StringSplitOptions.RemoveEmptyEntries)
                .ToArray();
    }
}
