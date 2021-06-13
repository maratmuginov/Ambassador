using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Threading.Tasks;

namespace Ambassador.Bot.Commands
{
    public class ModeratorCommands : BaseCommandModule
    {
        [Command("Sweep")]
        public async Task SweepAsync(CommandContext context, int messageCount)
        {
            if (messageCount < 1)
            {
                await context.Message.RespondAsync($"{nameof(messageCount)} cannot be less than 1");
                return;
            }

            var messages = await context.Channel.GetMessagesAsync(messageCount);
            await context.Channel.DeleteMessagesAsync(messages);
            var confirmationMessage = await context.Channel.SendMessageAsync($"Deleted {messages.Count} messages.");
            await Task.Delay(TimeSpan.FromSeconds(5));
            await confirmationMessage.DeleteAsync();
        }
    }
}
