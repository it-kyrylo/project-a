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
    public class StatisticsMenuState : IState
    {
        private readonly ICosmosDbStateProviderService _stateProvider;

        public StatisticsMenuState(ICosmosDbStateProviderService stateProvider)
        {
            this._stateProvider = stateProvider;
        }

        public async Task<StateType> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(callbackQueryId: callbackQuery.Id);

            return callbackQuery.Data switch
            {
                Statistics.PlayersData => StateType.PlayerDataState,
                Statistics.TopScorersLeague => StateType.TopScorersState,
                Statistics.TopScorersTeam => StateType.TopScorersInTeamMenuState,
                Statistics.PlayersInTeamFromPosition => StateType.PlayersOfPositionInTeamState,
                Statistics.PlayerInDreamtem => StateType.TimesPlayerHasBeenInDreamTeamState,
                Statistics.PlayersFromTeamInDreamteam => StateType.PlayersInDreamTeamOfTeamState,
                Suggestions.BackToPreviousMenu or _ => MoveBack(callbackQuery.Message.Chat.Id)
            };
        }

        public async Task<StateType> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            return await Task.FromResult(StateType.StatisticsMenuState);
        }

        public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        {
            var options = new InlineKeyboardMarkup(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Player's data", "Player's data"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Top scorers of the championship", "Top scorers of the championship"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Top scorers in a team", "Top scorers in a team"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Player in a team of position", "Player in a team of position"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Times player has been in dream team", "Times player has been in dream team"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Team's players in dream teams", "Team's players in dream teams"),
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Back", "Back"),
                }
            });

            await InteractionHelper.SendInlineKeyboard(botClient, chatId, StateMessages.ChooseCategory, options);
        }

        private StateType MoveBack(long chatId)
        {
            //var chat = _stateProvider.GetChatStateAsync(chatId).Result;
            //chat.Current_State = StateType.MainState;
            //_stateProvider.UpdateChatStateAsync(chat);

            return StateType.MainState;
        }
    }
}
