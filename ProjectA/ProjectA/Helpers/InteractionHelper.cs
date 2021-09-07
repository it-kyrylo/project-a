using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ProjectA.Models
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
    }
}