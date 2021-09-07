using ProjectA.Models.StateOfChatModels.Enums;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ProjectA.States
{
    public interface IState
    {
        Task<StateTypes> BotOnMessageReceived(ITelegramBotClient botClient, Message message);

        Task<StateTypes> BotOnCallBackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery);

        void BotSendMessage(ITelegramBotClient botClient, long chatId);
    }
}
