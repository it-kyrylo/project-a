using Moq;
using NUnit.Framework;
using ProjectA.Models.StateOfChatModels;
using ProjectA.Services.StateProvider;
using ProjectA.States;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using ProjectA.Models.StateOfChatModels.Enums;
using static ProjectA.States.StateConstants;

namespace UnitTests.StatisticsStateTests
{
    [TestFixture]
    public class StatisticsMenuStateTests
    {
        private readonly ICosmosDbStateProviderService _stateProviderMock;
        private readonly ITelegramBotClient _botClientMock;
        private readonly Message _messageMock;
        private readonly ChatState _chatStateMock;
        private readonly CallbackQuery _callbackQueryMock;
        private readonly IState _statisticsMenuState;

        public StatisticsMenuStateTests()
        {
            this._stateProviderMock = new Mock<ICosmosDbStateProviderService>().Object;
            this._statisticsMenuState = new StatisticsMenuState(this._stateProviderMock);

            this._botClientMock = new Mock<ITelegramBotClient>().Object;
            this._messageMock = new Message();
            this._messageMock.Chat = new Chat();
            this._callbackQueryMock = new CallbackQuery();
            this._chatStateMock = new ChatState(1234);
        }

        [Test]
        [TestCase(Statistics.PlayersData, StateType.PlayerDataState, 123456789)]
        [TestCase(Statistics.PlayersFromTeamInDreamteam, StateType.PlayersInDreamTeamOfTeamState, 123456789)]
        [TestCase(Statistics.PlayerInDreamtem, StateType.TimesPlayerHasBeenInDreamTeamState, 123456789)]
        [TestCase(Statistics.PlayersInTeamFromPosition, StateType.PlayersOfPositionInTeamState, 123456789)]
        [TestCase(Statistics.TopScorersLeague, StateType.TopScorersState, 123456789)]
        [TestCase(Statistics.TopScorersTeam, StateType.TopScorersInTeamState, 123456789)]
        [TestCase(Suggestions.BackToPreviousMenu, StateType.MainState, 123456789)]
        public async Task BotOnCallBackQueryReceived_ShouldReturnCorrectState_AsCallbackQueryData(string stateStr, StateType state, long chatId)
        {
            //Arrange
            var expectedResult = state;
            this._callbackQueryMock.Data = stateStr;
            this._messageMock.Chat.Id = chatId;
            this._callbackQueryMock.Message = this._messageMock;

            //Act
            var actualResult = await this._statisticsMenuState.BotOnCallBackQueryReceived(this._botClientMock, this._callbackQueryMock);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [TestCase(1234556789)]
        public async Task BotOnMessageReceived_ShouldReturnCorrectState(long chatId)
        {
            //Arrange
            this._messageMock.Chat.Id = chatId;
            this._messageMock.Text = null;
            var expectedResult = StateType.StatisticsMenuState;

            //Act
            var actualResult = await this._statisticsMenuState.BotOnMessageReceived(this._botClientMock, this._messageMock);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
