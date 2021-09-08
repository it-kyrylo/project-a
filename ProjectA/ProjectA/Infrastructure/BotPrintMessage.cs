using ProjectA.Models.StateOfChatModels.Enums;
using System.Threading.Tasks;
using Telegram.Bot;

namespace ProjectA.Infrastructure
{
    public static class BotPrintMessage
    {
        public static async Task<StateType> PrintMessage(ITelegramBotClient botClient, long chatId, string message)
        {
            await botClient.SendTextMessageAsync(chatId, message);

            return StateType.PlayersByOverallStatsState;
        }
    }
}
