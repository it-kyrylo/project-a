using ProjectA.Helpers;
using ProjectA.Models.StateOfChatModels.Enums;
using ProjectA.Services.Handlers;
using ProjectA.Services.StateProvider;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ProjectA.States.TeamsStatistic
{
    public class StrongestHomeState : IState
    {
        private readonly IStateTeamService _handlerTeamService;
        private readonly ICosmosDbStateProviderService _stateProvider;
        public StrongestHomeState(ICosmosDbStateProviderService stateProvider, IStateTeamService handlerTeamService)
        {
            _handlerTeamService = handlerTeamService;
            _stateProvider = stateProvider;
        }
        public async Task<StateType> BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            var chat = await _stateProvider.GetChatStateAsync(message.Chat.Id);

            await _stateProvider.UpdateChatStateAsync(chat);


            return StateType.TeamsMenuState;
        }
        public async Task<StateType> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(callbackQueryId: callbackQuery.Id);

            return StateType.StrongestTeamHomeState;
        }



        public async Task BotSendMessage(ITelegramBotClient botClient, long chatId)
        {
            await botClient.SendTextMessageAsync(chatId, "Current strongest Team at home");

            var teams = await _handlerTeamService.GetStrongestTeamHomeAsync();

            await InteractionHelper.PrintMessage(botClient, chatId, teams);

            await botClient.SendTextMessageAsync(chatId, "Type 'menu' for Teams Menu");

        }
    }
}
