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
    public class PlayersByITCRank : IState
    {
        private readonly ICosmosDbStateProviderService _stateProvider;
        private readonly IPlayerSuggestionService playerSuggestionService;

        public PlayersByITCRank(
            ICosmosDbStateProviderService stateProvider,
            IPlayerSuggestionService players)
        {
            _stateProvider = stateProvider;
            this.playerSuggestionService = players;
        }

        public async Task<StateType> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            var isUserInputNull = Guard.AgainstNull(botClient, message);

            if (isUserInputNull)
            {
                await InteractionHelper.PrintMessage(botClient, message.Chat.Id, InsertPlayersSuggestionsPreferences);

                return StateType.PlayersByITCRank;
            }

            var isUserInputInFormat = Guard.AgainstWrongFormat(botClient, message);

            if (!isUserInputInFormat)
            {
                await InteractionHelper.PrintMessage(botClient, message.Chat.Id, WrongInputFormat);

                return StateType.PlayersByITCRank;
            }

            var userInputParsed = InteractionHelper.ProcessUserInput(message.Text);

            InteractionHelper.GetUserPreferences(
                userInputParsed,
                out string position,
                out double minPrice,
                out double maxPrice);

            var arePricesInCorrectRange = Guard.AgainstInvalidPrices(botClient, message, minPrice, maxPrice);

            if (!arePricesInCorrectRange)
            {
                await InteractionHelper.PrintMessage(botClient, message.Chat.Id, MinPriceSmallerThanMaxPrice);

                return StateType.PlayersByITCRank;
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
            var suggestedPlayers = await this.playerSuggestionService.GetByInfluenceThreatCreativityRank(position, minPrice, maxPrice);

            var sb = new StringBuilder();

            if (suggestedPlayers == null)
            {
                return sb
                        .AppendLine("No such result")
                        .ToString();
            }

            sb
                .AppendLine(PlayersByITC)
                .AppendLine();

            foreach (var player in suggestedPlayers)
            {
                sb.AppendLine(player.ToString());
            }

            return sb.ToString();
        }
    }
}
