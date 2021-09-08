using ProjectA.Factory;
using ProjectA.Models.StateOfChatModels.Enums;
using ProjectA.Services.Handlers;
using ProjectA.Services.PlayersSuggestion;
using ProjectA.Services.StateProvider;
using ProjectA.Services.Statistics;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProjectA.Handlers
{
    public class TelegramHandler : ITelegramUpdateHandler
    {
        private readonly IStateTeamService _handlerTeamService;
        private readonly ICosmosDbStateProviderService _stateProvider;
        private readonly IStateFactory _stateFactory;
        public TelegramHandler( 
            IStateTeamService handlerTeamService,
            ICosmosDbStateProviderService stateProvider, 
            IStateFactory stateFactory)
        {
            _handlerTeamService = handlerTeamService;
            _stateProvider = stateProvider;
            _stateFactory = stateFactory;
        }

        public  Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message == null && update.CallbackQuery == null)
            {
                return;
            }

            var chatId = update.Message != null ? update.Message.Chat.Id : update.CallbackQuery.Message.Chat.Id;
            var state =  _stateFactory.GetState(_stateProvider.GetChatStateAsync(chatId).Result.Current_State);

            var handler = update.Type switch
            {
                UpdateType.Message => state.BotOnMessageReceived(botClient, update.Message).Result,
                UpdateType.CallbackQuery => state.BotOnCallBackQueryReceived(botClient, update.CallbackQuery).Result,
                _ => UnknownUpdateHandlerAsync(botClient, update).Result
            };

            try
            {
                var nextState = handler;
                var chat = await _stateProvider.GetChatStateAsync(chatId);
                chat.Current_State = nextState;
                await _stateProvider.UpdateChatStateAsync(chat);
                await _stateFactory.GetState(nextState).BotSendMessage(botClient, chatId);
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken).ConfigureAwait(false);
            }
        }

        //TODO: Delete after second interal demo
        private static async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            if (message.Type != MessageType.Text)
                return;

            var action = message.Text switch
            {
               "/FPL" => SendBaseMenu(botClient, message),
                _ => Usage(botClient, message)
            };

            await action;

            
            static async Task<Message> Usage(ITelegramBotClient botClient, Message message)
            {
                const string usage = "Usage:\n" +
                                     "/inline   - send inline keyboard\n" +
                                     "/keyboard - send custom keyboard\n";

                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: usage,
                                                            replyMarkup: new ReplyKeyboardRemove());
            }
        }

        //TODO: Delete after second interal demo
        private async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {

            if (callbackQuery.Data=="Team Statistics")
            {
             await SendInlineTeamMenu(botClient, callbackQuery.Message);
            }
            if (callbackQuery.Data =="Base Menu")
            {
                await SendBaseMenu(botClient, callbackQuery.Message);
            }
             var result =await TeamsMenuFunctions(callbackQuery.Data, botClient);         


            await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: $"{result}");
        }

        //TODO: Delete after second interal demo
        static async Task<Message> SendBaseMenu(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

            // Simulate longer running task
            await Task.Delay(500);

            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Player Statistics"),
                        InlineKeyboardButton.WithCallbackData("Player Suggestions" )
                    },
                    //second row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Team Statistics")

                    },
                });

            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                        text: "Choose",
                                                        replyMarkup: inlineKeyboard);
        }

        //TODO: Delete after second interal demo
        static async Task<Message> SendInlineTeamMenu(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

            // Simulate longer running task
            await Task.Delay(500);

            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Top 3 Teams" ),
                        InlineKeyboardButton.WithCallbackData("All Teams" ),

                    },
                    //second row
                    new []
                    {
                         InlineKeyboardButton.WithCallbackData("Strongest Team Home" ),
                        InlineKeyboardButton.WithCallbackData("Strongest Team Away"),

                    },
                    new []
                    {
                         InlineKeyboardButton.WithCallbackData("Most Wins Team" ),
                        InlineKeyboardButton.WithCallbackData("Most Losses Team"),

                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Base Menu")
                    }
                });

            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                        text: "Choose",
                                                        replyMarkup: inlineKeyboard);

        }
        //TODO: Delete after second interal demo
        private async Task<string> TeamsMenuFunctions(string callbackQueryData, ITelegramBotClient botClient)
        {

            var result = callbackQueryData switch
            {
                "Top 3 Teams" => await _handlerTeamService.GetTopThreeStrongestTeamsAsync(),

                "Most Wins Team" => await _handlerTeamService.GetTeamsWithMostWinsAsync(),

                "Most Losses Team" => await _handlerTeamService.GetTeamWithMostLossesAsync(),

                "All Teams" => await _handlerTeamService.GetAllTeamsAsync(),

                "Strongest Team Home" => await _handlerTeamService.GetStrongestTeamHomeAsync(),

                "Strongest Team Away"  => await _handlerTeamService.GetStrongestTeamAwayAsync()

            };

            return result;
        }

        //TODO: Delete after second interal demo
        private async Task BotOnInlineQueryReceived(ITelegramBotClient botClient, InlineQuery inlineQuery)
        {
            Console.WriteLine($"Received inline query from: {inlineQuery.From.Id}");

            InlineQueryResultBase[] results = {
                // displayed result
                new InlineQueryResultArticle(
                    id: "3",
                    title: "TgBots",
                    inputMessageContent: new InputTextMessageContent(
                        "hello"
                    )
                )
            };

            await botClient.AnswerInlineQueryAsync(
                inlineQueryId: inlineQuery.Id,
                results: results,
                isPersonal: true,
                cacheTime: 0);
        }

        //TODO: Delete after second interal demo
        private static Task BotOnChosenInlineResultReceived(ITelegramBotClient botClient, ChosenInlineResult chosenInlineResult)
        {
            Console.WriteLine($"Received inline result: {chosenInlineResult.ResultId}");
            return Task.CompletedTask;
        }

        private Task<StateType> UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {
            botClient.SendTextMessageAsync(update.Message.Chat.Id, "Something went wrong! Please try again");

            return Task.Run(() => StateType.MainState);
        }

    }
}
