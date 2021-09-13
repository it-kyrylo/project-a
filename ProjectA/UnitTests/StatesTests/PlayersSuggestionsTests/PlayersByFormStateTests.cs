using FluentAssertions;
using Moq;
using NUnit.Framework;
using ProjectA.Models.StateOfChatModels.Enums;
using ProjectA.Services.PlayersSuggestion;
using ProjectA.Services.StateProvider;
using ProjectA.States;
using ProjectA.States.PlayersSuggestion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace UnitTests.StatesTests.PlayersSuggestionsTests
{
    [TestFixture]
    public class PlayersByFormStateTests
    {
        private Mock<PlayersByFormState> state;
        private Mock<ITelegramBotClient> bot;
        private Mock<ICosmosDbStateProviderService> stateProvider;
        private Mock<Message> message;
        private Mock<CallbackQuery> callbackQuery;
        private Mock<Chat> chat;
        private Mock<IPlayerSuggestionService> players;

        [SetUp]
        public void Setup()
        {
            stateProvider = new Mock<ICosmosDbStateProviderService>();
            bot = new Mock<ITelegramBotClient>();
            message = new Mock<Message>();
            callbackQuery = new Mock<CallbackQuery>();
            chat = new Mock<Chat>();
            players = new Mock<IPlayerSuggestionService>();
            state = new Mock<PlayersByFormState>(stateProvider.Object, players.Object);
        }

        [Test]
        public void BotOnCallBackQueryReceivedShouldReturnCorrectState()
        {
            //Arrange
            var expectedResult = StateType.PlayersByFormState;

            //Act
            var actualResult = state.Object.BotOnCallBackQueryReceived(bot.Object, callbackQuery.Object).Result;

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [TestCase(1234556789)]
        [TestCase(987654321010)]
        public void BotOnMessageReceivedShouldReturnCorrectStateIfMessageIsNull(long chatId)
        {
            //Arrange
            chat.Object.Id = chatId;
            message.Object.Chat = chat.Object;
            var expectedResult = StateType.PlayersByFormState;

            //Act
            var actualResult = state.Object.BotOnMessageReceived(bot.Object, message.Object).Result;

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [TestCase(1234556789, "/defender/5/5.5")]
        [TestCase(987654321010, "defender/defender/7")]
        [TestCase(987654321010, "defender/5/defender")]
        [TestCase(987654321010, "5/5/7")]
        [TestCase(987654321010, "///")]
        [TestCase(987654321010, ".")]
        [TestCase(1234556789, "defender/-1/5.5")]
        [TestCase(987654321010, "defender/5/3")]
        public void BotOnMessageReceivedShouldReturnCorrectStateIfMessageIsNotInCorrectFormat(long chatId, string userInput)
        {
            //Arrange
            chat.Object.Id = chatId;
            message.Object.Chat = chat.Object;
            message.Object.Text = userInput;
            var expectedResult = StateType.PlayersByFormState;

            //Act
            var actualResult = state.Object.BotOnMessageReceived(bot.Object, message.Object).Result;

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        [TestCase(1234556789, "defender/4.5/5.5")]
        [TestCase(1234556789, "deFENDer/4.5/5.5")]
        [TestCase(1234556789, "forward/4.5/6.5")]
        [TestCase(987654321010, "FORWARD/5/8")]
        [TestCase(987654321010, "midfielder/5/8")]
        [TestCase(987654321010, "Goalkeeper/5/7")]
        public void BotOnMessageReceivedShouldReturnCorrectStateIfUserInputPricesAreInvalid(long chatId, string userInput)
        {
            //Arrange
            chat.Object.Id = chatId;
            message.Object.Chat = chat.Object;
            message.Object.Text = userInput;
            var expectedResult = StateType.SuggestionsMenuState;

            //Act
            var actualResult = state.Object.BotOnMessageReceived(bot.Object, message.Object).Result;

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
