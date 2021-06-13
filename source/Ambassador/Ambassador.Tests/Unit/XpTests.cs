using System;
using Ambassador.Lib.Contracts;
using Ambassador.Lib.Models;
using Ambassador.Lib.Services;
using Ambassador.Tests.Fixtures;
using System.Threading.Tasks;
using Ambassador.Bot.Services;
using DSharpPlus;
using DSharpPlus.EventArgs;
using Moq;
using Xunit;

namespace Ambassador.Tests.Unit
{
    public class XpTests
    {
        [Fact]
        public void LevelUpIsFiredWhenExperienceTresholdIsSurpassed()
        {
            var eventSource = new TestXpEventSource();
            var xpThresholdsConfig = new XpThresholdsConfig
            {
                Tresholds = new[] { 1 }
            };
            IXpService sut = new XpService(eventSource, xpThresholdsConfig);
            sut.LeveledUp += OnLevelUp;
            bool raised = false;

            eventSource.RaiseXpChanged(2);

            Task OnLevelUp(LeveledUpArgs levelupinfo)
            {
                raised = true;
                return Task.CompletedTask;
            }

            Assert.True(raised);
        }

        [Fact]
        public void MessageXpEventSourceThrottles()
        {
            //Arrange
            var xpThrottleConfig = new XpThrottleConfig
            {
                Count = 10,
                Duration = TimeSpan.FromMilliseconds(10)
            };
            var userMessageSource = new TestUserMessageSource();
            var sut = new MessageXpEventSource(userMessageSource, xpThrottleConfig);
            int actual = 0;
            sut.XpChanged += _ =>
            {
                actual++;
                return Task.CompletedTask;
            };
            int expected = xpThrottleConfig.Count;

            //Act
            for (int i = 0; i < xpThrottleConfig.Count * 2; i++)
                userMessageSource.RaiseUserMessageReceived("someMessageContent");

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MessageXpEventSourceClearsCache()
        {
            //Arrange
            var xpThrottleConfig = new XpThrottleConfig
            {
                Count = 10,
                Duration = TimeSpan.FromMilliseconds(1)
            };
            var userMessageSource = new TestUserMessageSource();
            var sut = new MessageXpEventSource(userMessageSource, xpThrottleConfig);
            int actual = 0;
            sut.XpChanged += _ =>
            {
                actual++;
                return Task.CompletedTask;
            };
            int expected = xpThrottleConfig.Count * 2;

            //Act
            for (int i = 0; i < xpThrottleConfig.Count * 2; i++)
                userMessageSource.RaiseUserMessageReceived("someMessageContent");
            await Task.Delay(TimeSpan.FromMilliseconds(20));
            for (int i = 0; i < xpThrottleConfig.Count * 2; i++)
                userMessageSource.RaiseUserMessageReceived("someMessageContent");

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}
