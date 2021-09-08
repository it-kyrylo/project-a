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
    public class TeamsMenuState :IState
    {
        private readonly ICosmosDbStateProviderService _stateProvider;

        public TeamsMenuState(ICosmosDbStateProviderService stateProvider)
        {
            _stateProvider = stateProvider;
        }

        public async Task<StateType> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(callbackQueryId: callbackQuery.Id);

            return callbackQuery.Data switch
            {
                TeamStatistics.TopThreeTeams => StateType.TopThreeTeamsState,
                TeamStatistics.AllTeams => StateType.AllTeamsState,
                TeamStatistics.StrongestTeamHome => StateType.StrongestTeamHomeState,
                TeamStatistics.StrongestTeamAway => StateType.StrongestTeamAwayState,
                TeamStatistics.MostWinsTeam => StateType.MostWinsTeamState,
                TeamStatistics.MostLossesTeam => StateType.MostLossesTeamState,
                TeamStatistics.SearchTeam => StateType.SearchTeamState,
                "Back" or _ => MoveBack(callbackQuery.Message.Chat.Id)
            };
        }

        private StateType MoveBack(long chatId)
        {
            var chat = _stateProvider.GetChatStateAsync(chatId).Result;

            _stateProvider.UpdateChatStateAsync(chat);

            return StateType.MainState;
        }

        public Task<StateType> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            return Task.FromResult(StateType.TeamsMenuState);
        }

        public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        {
            var options = new InlineKeyboardMarkup(new[]
          {
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(TeamStatistics.TopThreeTeams,TeamStatistics.TopThreeTeams),
                        InlineKeyboardButton.WithCallbackData(TeamStatistics.AllTeams,TeamStatistics.AllTeams),

                    },
                    //second row
                    new []
                    {
                         InlineKeyboardButton.WithCallbackData(TeamStatistics.StrongestTeamHome,TeamStatistics.StrongestTeamHome),
                        InlineKeyboardButton.WithCallbackData(TeamStatistics.StrongestTeamAway,TeamStatistics.StrongestTeamAway),

                    },
                    new []
                    {
                         InlineKeyboardButton.WithCallbackData(TeamStatistics.MostWinsTeam,TeamStatistics.MostWinsTeam),
                        InlineKeyboardButton.WithCallbackData(TeamStatistics.MostLossesTeam,TeamStatistics.MostLossesTeam),

                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(TeamStatistics.SearchTeam,TeamStatistics.SearchTeam),
                        InlineKeyboardButton.WithCallbackData(TeamStatistics.BackToPreviousMenu,TeamStatistics.BackToPreviousMenu)
                    }
                });
            var message = "Please choose button";

            await InteractionHelper.SendInlineKeyboard(botClient, chatId, message, options);
        }
    }
}

