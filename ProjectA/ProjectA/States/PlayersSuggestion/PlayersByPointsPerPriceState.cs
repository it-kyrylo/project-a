using ProjectA.Helpers;
using ProjectA.Models.StateOfChatModels.Enums;
using ProjectA.Services.PlayersSuggestion;
using ProjectA.Services.StateProvider;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using static ProjectA.States.StateConstants.StateMessages;
using static ProjectA.States.StateConstants.Suggestions;

namespace ProjectA.States.PlayersSuggestion
{
    public class PlayersByPointsPerPriceState : IState
    {
        private readonly ICosmosDbStateProviderService _stateProvider;
        private readonly IPlayerSuggestionService players;

        public PlayersByPointsPerPriceState(
            ICosmosDbStateProviderService stateProvider,
            IPlayerSuggestionService players)
        {
            _stateProvider = stateProvider;
            this.players = players;
        }

        public async Task<StateType> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            var isUserInputNull = await Guard.AgainstNull(botClient, message, InsertPlayersSuggestionsPreferences);

            if (isUserInputNull)
            {
                return StateType.PlayersByPointsPerPriceState;
            }

            var isUserInputInFormat = await Guard.AgainstWrongFormat(botClient, message, WrongInputFormat);

            if (!isUserInputInFormat)
            {
                return StateType.PlayersByPointsPerPriceState;
            }

            var userInputParsed = InteractionHelper.ProcessUserInput(message.Text);

            InteractionHelper.GetUserPreferences(
                userInputParsed,
                out string position,
                out double minPrice,
                out double maxPrice);

            var arePricesInCorrectRange = await Guard.AgainstInvalidPrices(botClient, message, MinPriceSmallerThanMaxPrice, minPrice, maxPrice);

            if (!arePricesInCorrectRange)
            {
                return StateType.PlayersByFormState;
            }

            var chat = await _stateProvider.GetChatStateAsync(message.Chat.Id);

            await _stateProvider.UpdateChatStateAsync(chat);

            var suggestionResult = await GetSuggestionAsStringAsync(minPrice, maxPrice, position);

            await InteractionHelper.PrintMessage(botClient, message.Chat.Id, suggestionResult);

            return StateType.SuggestionsMenuState;
        }

        public async Task<StateType> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(callbackQueryId: callbackQuery.Id);

            return StateType.PlayersByOverallStatsState;
        }

        public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        {
            await botClient.SendTextMessageAsync(chatId, $"{InsertPlayersSuggestionsPreferences}\n{PlayersSuggestionPreferencesFormat}");
        }

        private async Task<string> GetSuggestionAsStringAsync(double minPrice, double maxPrice, string position)
        {
            var suggestedPlayers = await this.players.GetByPointsPerPrice(position, minPrice, maxPrice);

            var sb = new StringBuilder();

            if (suggestedPlayers == null)
            {
                return sb
                        .AppendLine("No such result")
                        .ToString();
            }

            sb
                .AppendLine(PlayersByPointsPerPrice)
                .AppendLine();

            foreach (var player in suggestedPlayers)
            {
                sb.AppendLine(player.ToString());
            }

            return sb.ToString();
        }
    }
}
