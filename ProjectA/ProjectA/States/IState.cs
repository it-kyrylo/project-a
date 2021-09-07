using ProjectA.Models.StateOfChatModels.Enums;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ProjectA.States
{
    public interface IState
    {
        Task<StateType> BotOnMessageReceived(ITelegramBotClient botClient, Message message);

        Task<StateType> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery);

        Task BotSendMessage(ITelegramBotClient botClient, long chatId);
    }
}
