using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ProjectA.Helpers
{
    public static class Guard
    {
        public static bool AgainstNull(ITelegramBotClient botClient, Message message)
        {
            if (message?.Text == null)
            {
                return true;
            }

            return false;
        }

        public static bool AgainstWrongFormat(ITelegramBotClient botClient, Message message)
        {
            string pattern = @"^[a-zA-z]{7,}\/\d{1,2}.{0,1}\d{0,1}\/\d{1,2}.{0,1}\d{0,1}$";

            if (!Regex.IsMatch(message.Text, pattern))
            {
                return false;
            }

            return true;
        }

        public static bool AgainstInvalidPrices (ITelegramBotClient botClient, Message message, double minPrice, double maxPrice)
        {
            if (minPrice < 0 || minPrice > maxPrice)
            {
                return false;
            }

            return true;
        }

    }
}
