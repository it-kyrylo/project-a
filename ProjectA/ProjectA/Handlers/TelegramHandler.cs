using ProjectA.Models.PlayersModels;
using ProjectA.Services.Handlers;
using ProjectA.Services.PlayersSuggestion;
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
        private readonly IPlayerSuggestionService _players;
        private readonly IStatisticsService _statisticsService;
        private readonly IHandlerTeamService _handlerTeamService;
        public TelegramHandler(IPlayerSuggestionService players, IStatisticsService statisticsService, IHandlerTeamService handlerTeamService)
        {
            _players = players;
            _handlerTeamService = handlerTeamService;
            _statisticsService = statisticsService;
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
            var handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageReceived(botClient, update.Message),
                UpdateType.EditedMessage => BotOnMessageReceived(botClient, update.EditedMessage),
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(botClient, update.CallbackQuery),
                UpdateType.InlineQuery => BotOnInlineQueryReceived(botClient, update.InlineQuery),
                UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(botClient, update.ChosenInlineResult),
                _ => UnknownUpdateHandlerAsync(botClient, update)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken);
            }
        }


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

        // Process Inline Keyboard callback data
        private  async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
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
        private  async Task<string> TeamsMenuFunctions(string callbackQueryData, ITelegramBotClient botClient)
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

        private static Task BotOnChosenInlineResultReceived(ITelegramBotClient botClient, ChosenInlineResult chosenInlineResult)
        {
            Console.WriteLine($"Received inline result: {chosenInlineResult.ResultId}");
            return Task.CompletedTask;
        }

        private static Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {
            Console.WriteLine($"Unknown update type: {update.Type}");
            return Task.CompletedTask;
        }

    }
}
