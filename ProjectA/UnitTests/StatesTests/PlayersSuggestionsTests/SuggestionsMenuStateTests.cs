using Moq;
using NUnit.Framework;
using ProjectA.Models.StateOfChatModels.Enums;
using ProjectA.Services.StateProvider;
using ProjectA.States;
using Telegram.Bot;
using Telegram.Bot.Types;
using static ProjectA.States.StateConstants;

namespace UnitTests.StatesTests.PlayersSuggestionsTests
{
    [TestFixture]
    public class SuggestionsMenuStateTests
    {
        private Mock<SuggestionsMenuState> state;
        private Mock<ITelegramBotClient> bot;
        private Mock<ICosmosDbStateProviderService> stateProvider;
        private Mock<Message> message;
        private Mock<CallbackQuery> callbackQuery;
        private Mock<Chat> chat;

        [SetUp]
        public void Setup()
        {
            stateProvider = new Mock<ICosmosDbStateProviderService>();
            state = new Mock<SuggestionsMenuState>(stateProvider.Object);
            bot = new Mock<ITelegramBotClient>();
            message = new Mock<Message>();
            callbackQuery = new Mock<CallbackQuery>();
            chat = new Mock<Chat>();
        }

        [Test]
        [TestCase("/start")]
        [TestCase("hello")]
        [TestCase("57")]
        public void BotOnMessageReceivedShouldReturnCorrectState(string input)
        {
            //Arrange
            message.Object.Text = input;
            var expectedResult = StateType.SuggestionsMenuState;

            //Act
            var actualResult = state.Object.BotOnMessageReceived(bot.Object, message.Object).Result;

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [TestCase(Suggestions.PointsPerGameCriteria, StateType.PlayersByPointsPerGameState, 12345)]
        [TestCase(Suggestions.CurrentFormCriteria, StateType.PlayersByFormState, 12344567)]
        [TestCase(Suggestions.ITCRankCriteria, StateType.PlayersByITCRank, 12344567)]
        [TestCase(Suggestions.PointsPerPriceCriteria, StateType.PlayersByPointsPerPriceState, 12344567)]
        [TestCase(Suggestions.OverallStatsCriteria, StateType.PlayersByOverallStatsState, 12344567)]
        [TestCase(Suggestions.BackToPreviousMenu, StateType.MainState, 213)]
        public void BotOnCallBackQueryReceivedShouldReturnCorrectState(string queryData, StateType stateType, long chatId)
        {
            //Arrange
            callbackQuery.Object.Data = queryData;
            callbackQuery.Object.Message = message.Object;
            chat.Object.Id = chatId;
            callbackQuery.Object.Message.Chat = chat.Object;
            var expectedResult = stateType;

            //Act
            var actualResult = state.Object.BotOnCallBackQueryReceived(bot.Object, callbackQuery.Object).Result;

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [TestCase("Get", StateType.MainState, 12345)]
        [TestCase("hello", StateType.MainState, 9876612)]
        public void BotOnCallBackQueryReceivedShouldReturnCorrectStateWhenUnknownDataIsRecieved(string queryData, StateType stateType, long chatId)
        {
            //Arrange
            callbackQuery.Object.Data = queryData;
            callbackQuery.Object.Message = message.Object;
            chat.Object.Id = chatId;
            callbackQuery.Object.Message.Chat = chat.Object;
            var expectedResult = stateType;

            //Act
            var actualResult = state.Object.BotOnCallBackQueryReceived(bot.Object, callbackQuery.Object).Result;

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
