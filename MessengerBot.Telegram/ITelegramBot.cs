using MessengerBot.Telegram.Models;
using MessengerBot.Telegram.Models.Telegram;
using System.Threading.Tasks;

namespace MessengerBot.Telegram
{
    public interface ITelegramBot
    {
        Task<User> GetMeAsync();
        Task<Message> SendAsync(MessageType messageType, MessageInfo message);
        Task<Message> SendMessageAsync(MessageInfo message);
        Task<Message> SendPhotoAsync(MessageInfo message);
        Task<Message> SendDocumentAsync(MessageInfo message);
    }
}