using ProjectA.Models.StateOfChatModels.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProjectA.Helpers
{
    public static class InteractionHelper
    {
        public static async Task<Message> SendInlineKeyboard(ITelegramBotClient botClient, long chatId, string prompt, InlineKeyboardMarkup options)
        {
            await botClient.SendChatActionAsync(chatId, ChatAction.Typing);

            return await botClient.SendTextMessageAsync(chatId: chatId,
                                                        text: prompt,
                                                        replyMarkup: options);
        }

        public static string[] ProcessUserInput(string userInput)
               => userInput
                   .Split('/', StringSplitOptions.RemoveEmptyEntries)
                   .ToArray();

        public static async Task<StateType> PrintMessage(ITelegramBotClient botClient, long chatId, string message)
        {
            await botClient.SendTextMessageAsync(chatId, message);

            return StateType.PlayersByOverallStatsState;

        }

        public static void GetUserPreferences(
            string[] userInputParsed,
            out string position,
            out double minPrice,
            out double maxPrice)
        {
            position = userInputParsed[0];
            minPrice = double.Parse(userInputParsed[1]);
            maxPrice = double.Parse(userInputParsed[2]);
        }
    }
}