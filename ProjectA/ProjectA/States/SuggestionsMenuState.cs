using ProjectA.Helpers;
using ProjectA.Models.StateOfChatModels.Enums;
using ProjectA.Services.StateProvider;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

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
                //TODO: Add states for each suggestions criteria - e.g. "Points per game" and etc.
                //TODO: Introduce StatesConstants class

                "Points Per Game" => StateType.SuggestionsMenuState,
                "Current Form" => StateType.SuggestionsMenuState,
                "ITC Rank" => StateType.SuggestionsMenuState,
                "Points Per Price" => StateType.SuggestionsMenuState,
                "Overall stats" => StateType.PlayersByOverallStatsState,
                "Back" or _ => MoveBack(callbackQuery.Message.Chat.Id)
            };
        }

        public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        {
            //TODO: Introduce StatesConstants class
            var options = new InlineKeyboardMarkup(new[]
            {
                new [] 
                { 
                    InlineKeyboardButton.WithCallbackData("Points Per Game", "Points Per Game"),
                    InlineKeyboardButton.WithCallbackData("Current Form", "Current Form")
                },
                new [] 
                { 
                    InlineKeyboardButton.WithCallbackData("ITC Rank", "ITC Rank"), 
                    InlineKeyboardButton.WithCallbackData("Points Per Price", "Points Per Price") 
                },
                new [] 
                {
                    InlineKeyboardButton.WithCallbackData("Overall stats", "Overall stats"),
                    InlineKeyboardButton.WithCallbackData("Back", "Back") 
                }
            });

            var message = "To get 5 suggested players, please choose a criteria:";

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
