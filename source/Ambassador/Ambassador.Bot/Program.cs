using Ambassador.Bot.Commands;
using Ambassador.Bot.Services;
using Ambassador.Lib.Contracts;
using Ambassador.Lib.Helpers;
using Ambassador.Lib.Models;
using Ambassador.Lib.Services;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ambassador.Bot
{
    internal class Program
    {
        private static async Task Main()
        {
            var services = ConfigureServices();

            var discordClient = services.GetRequiredService<DiscordClient>();

            var xpService = services.GetRequiredService<IXpService>();
            xpService.LeveledUp += async info =>
            {
                var channel = await discordClient.GetChannelAsync(info.ChannelId);
                var user = await discordClient.GetUserAsync(info.UserId);
                await channel.SendMessageAsync($"{user.Mention} Reached level {info.NewLevel}");
            };

            await discordClient.ConnectAsync();
            await Task.Delay(Timeout.Infinite);
        }

        private static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddScoped<DiscordConfiguration>(BuildDiscordConfig)
                .AddSingleton<BotConfig>(_ => JsonHelper.DeserializeFile<BotConfig>("botConfig.json"))
                .AddSingleton<DiscordClient>(BuildDiscordClient)
                .AddSingleton<IXpEventSource, MessageXpEventSource>()
                .AddScoped<IXpService, XpService>()
                .AddScoped<XpThrottleConfig>(_ => GetXpThrottleConfig())
                .AddScoped<XpThresholdsConfig>(_ => GetXpThresholdsConfig())
                .BuildServiceProvider();
        }

        private static XpThresholdsConfig GetXpThresholdsConfig()
        {
            return new XpThresholdsConfig
            {
                Tresholds = Enumerable.Range(8, 32).Select(power => 1 << power).ToList()
            };
        }

        private static XpThrottleConfig GetXpThrottleConfig()
        {
            return new XpThrottleConfig
            {
                Count = 10,
                Duration = TimeSpan.FromMinutes(1)
            };
        }

        private static DiscordClient BuildDiscordClient(IServiceProvider services)
        {
            var botConfig = services.GetRequiredService<BotConfig>();
            var discordConfig = services.GetRequiredService<DiscordConfiguration>();
            var discordClient = new DiscordClient(discordConfig);

            var commandsNextConfig = new CommandsNextConfiguration
            {
                StringPrefixes = botConfig.Prefixes,
                Services = services
            };

            var commandsNext = discordClient.UseCommandsNext(commandsNextConfig);
            commandsNext.RegisterCommands<ModeratorCommands>();

            return discordClient;
        }

        private static DiscordConfiguration BuildDiscordConfig(IServiceProvider services)
        {
            var botConfig = services.GetRequiredService<BotConfig>();
            return new DiscordConfiguration
            {
                Token = botConfig.Token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged,
                AutoReconnect = true
            };
        }
    }
}
