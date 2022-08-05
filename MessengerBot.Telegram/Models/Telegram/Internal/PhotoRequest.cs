using System.Text.Json.Serialization;

namespace MessengerBot.Telegram.Models.Telegram.Internal
{
    internal class PhotoRequest
    {
        [JsonPropertyName("chat_id")]
        public string ChatId { get; set; }

        [JsonPropertyName("photo")]
        public string Photo { get; set; }
    }
}