using ProjectA.Helpers;
using ProjectA.Models.StateOfChatModels.Enums;
using ProjectA.Services.StateProvider;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static ProjectA.States.StateConstants;

namespace ProjectA.States
{
    public class SuggestionsMenuState : IState
    {
        private readonly ICosmosDbStateProviderService _stateProvider;

        public SuggestionsMenuState(ICosmosDbStateProviderService stateProvider)
        {
            _stateProvider = stateProvider;
        }

        public Task<StateType> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
            => Task.FromResult(StateType.SuggestionsMenuState);

        public async Task<StateType> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(callbackQueryId: callbackQuery.Id);

            return callbackQuery.Data switch
            {
                Suggestions.PointsPerGameCriteria=> StateType.PlayersByPointsPerGameState,
                Suggestions.CurrentFormCriteria => StateType.PlayersByFormState,
                Suggestions.ITCRankCriteria => StateType.PlayersByITCRank,
                Suggestions.PointsPerPriceCriteria => StateType.PlayersByPointsPerPriceState,
                Suggestions.OverallStatsCriteria => StateType.PlayersByOverallStatsState,
                Suggestions.BackToPreviousMenu or _ => MoveBack(callbackQuery.Message.Chat.Id)
            };
        }

        public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        {
            var options = new InlineKeyboardMarkup(new[]
            {
                new [] 
                { 
                    InlineKeyboardButton.WithCallbackData(Suggestions.PointsPerGameCriteria, Suggestions.PointsPerGameCriteria),
                    InlineKeyboardButton.WithCallbackData(Suggestions.CurrentFormCriteria, Suggestions.CurrentFormCriteria)
                },
                new [] 
                { 
                    InlineKeyboardButton.WithCallbackData(Suggestions.ITCRankCriteria, Suggestions.ITCRankCriteria), 
                    InlineKeyboardButton.WithCallbackData(Suggestions.PointsPerPriceCriteria, Suggestions.PointsPerPriceCriteria) 
                },
                new [] 
                {
                    InlineKeyboardButton.WithCallbackData(Suggestions.OverallStatsCriteria, Suggestions.OverallStatsCriteria),
                    InlineKeyboardButton.WithCallbackData(Suggestions.BackToPreviousMenu, Suggestions.BackToPreviousMenu) 
                }
            });

            var message = StateMessages.GetPlayersSuggestionMessage;

            await InteractionHelper.SendInlineKeyboard(botClient, chatId, message, options);
        }

        private StateType MoveBack(long chatId)
        {
            var chat = _stateProvider.GetChatStateAsync(chatId).Result;
            _stateProvider.UpdateChatStateAsync(chat);

            return StateType.MainState;
        }
    }
}
