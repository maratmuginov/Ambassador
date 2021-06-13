using Ambassador.Lib.Contracts;
using Ambassador.Lib.Models;
using DSharpPlus;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace Ambassador.Bot.Services
{
    public class UserMessageSource : IUserMessageSource
    {
        public event UserMessageReceived UserMessageReceived;

        public UserMessageSource(DiscordClient discordClient)
        {
            discordClient.MessageCreated += OnMessageCreated;
        }

        private Task OnMessageCreated(DiscordClient _, MessageCreateEventArgs e)
        {
            if (e.Author.IsBot)
                return Task.CompletedTask;

            var args = new UserMessageArgs
            {
                UserId = e.Author.Id,
                ChannelId = e.Channel.Id,
                MessageContent = e.Message.Content
            };
            UserMessageReceived?.Invoke(args);
            return Task.CompletedTask;
        }
    }
}
