using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ProjectA.Helpers
{
    public class Guard
    {
        public static async Task AgainstNull(ITelegramBotClient botClient, Message message, string feedbackMessage)
        {
            if (message.Text == null)
            {
                await InteractionHelper.PrintMessage(botClient, message.Chat.Id, feedbackMessage);
            }
        }
    }
}
