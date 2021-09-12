using Moq;
using NUnit.Framework;
using ProjectA.Models.StateOfChatModels;
using ProjectA.Services.StateProvider;
using ProjectA.Services.Statistics;
using ProjectA.States;
using ProjectA.States.PlayersStatistics;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using ProjectA.Models.StateOfChatModels.Enums;

namespace UnitTests.StatisticsStateTests
{
    [TestFixture]
    public class PlayersInDreamTeamOfTeamStateTests
    {
        private readonly ICosmosDbStateProviderService _stateProviderMock;
        private readonly IStatisticsService _statisticsServiceMock;
        private readonly ITelegramBotClient _botClientMock;
        private readonly Message _messageMock;
        private readonly ChatState _chatStateMock;
        private readonly CallbackQuery _callbackQueryMock;
        private readonly IState _playersInDreamTeamOfTeamState;

        public PlayersInDreamTeamOfTeamStateTests()
        {
            this._stateProviderMock = new Mock<ICosmosDbStateProviderService>().Object;
            this._statisticsServiceMock = new Mock<IStatisticsService>().Object;
            this._playersInDreamTeamOfTeamState = new PlayersInDreamTeamOfTeamState(this._stateProviderMock, this._statisticsServiceMock);

            this._botClientMock = new Mock<ITelegramBotClient>().Object;
            this._messageMock = new Message();
            this._messageMock.Chat = new Chat();
            this._callbackQueryMock = new CallbackQuery();
            this._chatStateMock = new ChatState(1234);
        }

        [Test]
        public async Task BotOnCallBackQueryReceived_ShouldReturnSameState()
        {
            //Arrange
            var expectedResult = StateType.PlayersInDreamTeamOfTeamState;

            //Act
            var actualResult = await this._playersInDreamTeamOfTeamState.BotOnCallBackQueryReceived(this._botClientMock, this._callbackQueryMock);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [TestCase(1234556789)]
        public async Task BotOnMessageReceived_ShouldReturnCorrectState_IfMessageIsNull(long chatId)
        {
            //Arrange
            this._messageMock.Chat.Id = chatId;
            this._messageMock.Text = null;
            var expectedResult = StateType.StatisticsMenuState;

            //Act
            var actualResult = await this._playersInDreamTeamOfTeamState.BotOnMessageReceived(this._botClientMock, this._messageMock);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }


        [Test]
        [TestCase(1234556789, "Liverpool")]
        [TestCase(1234556789, "Manchester United")]
        public async Task BotOnMessageReceived_ShouldReturnCorrectState_IfUserInputIsCorrect(long chatId, string userInput)
        {
            //Arrange
            this._messageMock.Chat.Id = chatId;
            this._messageMock.Text = userInput;
            var expectedResult = StateType.StatisticsMenuState;

            //Act
            var actualResult = await this._playersInDreamTeamOfTeamState.BotOnMessageReceived(this._botClientMock, this._messageMock);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [TestCase(1234556789, "liverpool")]
        public async Task BotOnMessageReceived_ShouldReturnCorrectState_IfUserInputIsIncorrect(long chatId, string userInput)
        {
            //Arrange
            this._messageMock.Chat.Id = chatId;
            this._messageMock.Text = userInput;
            var expectedResult = StateType.StatisticsMenuState;

            //Act
            var actualResult = await this._playersInDreamTeamOfTeamState.BotOnMessageReceived(this._botClientMock, this._messageMock);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
