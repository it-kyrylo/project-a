using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ProjectA.Helpers
{
    public class Guard
    {
        public static async Task<bool> AgainstNull(ITelegramBotClient botClient, Message message, string feedbackMessage)
        {
            if (message.Text == null)
            {
                await InteractionHelper.PrintMessage(botClient, message.Chat.Id, feedbackMessage);

                return true;
            }

            return false;
        }

        public static async Task<bool> AgainstWrongFormat(ITelegramBotClient botClient, Message message, string feedbackMessage)
        {
            //Checks for userInput format - postion/minPrice/maxPrice
            //Prices can be both integer or floating-point numbers
            string pattern = @"^[a-zA-z]{7,}\/\d{1,2}.{0,1}\d{0,1}\/\d{1,2}.{0,1}\d{0,1}$";

            if (!Regex.IsMatch(message.Text, pattern))
            {
                await InteractionHelper.PrintMessage(botClient, message.Chat.Id, feedbackMessage);

                return false;
            }

            return true;
        }

        public static async Task<bool> AgainstInvalidPrices (ITelegramBotClient botClient, Message message, string feedbackMessage, double minPrice, double maxPrice)
        {
            if (minPrice < 0 || minPrice > maxPrice)
            {
                await InteractionHelper.PrintMessage(botClient, message.Chat.Id, feedbackMessage);

                return false;
            }

            return true;
        }

    }
}
