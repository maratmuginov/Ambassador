using Ambassador.Lib.Contracts;
using Ambassador.Lib.Models;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Ambassador.Bot.Services
{
    public class MessageXpEventSource : IXpEventSource
    {
        public event XpChanged XpChanged;

        private readonly ConcurrentDictionary<ulong, int> _userXp = new();
        private readonly ConcurrentDictionary<ulong, int> _messagesSentLastMinute = new();
        private readonly XpThrottleConfig _xpThrottleConfig;

        public MessageXpEventSource(IUserMessageSource userMessageSource, XpThrottleConfig xpThrottleConfig)
        {
            _xpThrottleConfig = xpThrottleConfig;
            userMessageSource.UserMessageReceived += OnUserMessageReceived;
        }

        private Task OnUserMessageReceived(UserMessageArgs e)
        {
            int messagesCount = GetMessagesSentLastMinute(e.UserId);

            if (messagesCount > _xpThrottleConfig.Count)
                return Task.CompletedTask;

            int addXp = e.MessageContent.Length;
            //TODO: Make _userXp persistent.
            int totalXp = _userXp.AddOrUpdate(e.UserId, addXp, (_, oldValue) => oldValue + addXp);

            var args = new XpChangedArgs { UserId = e.UserId, ChannelId = e.ChannelId, TotalXp = totalXp };
            XpChanged?.Invoke(args);

            return Task.CompletedTask;
        }

        private int GetMessagesSentLastMinute(ulong userId)
        {
            int messagesCount = _messagesSentLastMinute.GetOrAdd(userId, 0);
            messagesCount = _messagesSentLastMinute.AddOrUpdate(userId, ++messagesCount, (_, oldValue) => ++oldValue);

            void DecrementMessageCount(Task _)
            {
                _messagesSentLastMinute.AddOrUpdate(userId, _ => 0, (_, oldValue) => oldValue > 0 ? --oldValue : 0);
            }

            Task.Delay(_xpThrottleConfig.Duration).ContinueWith(DecrementMessageCount);
            return messagesCount;
        }
    }
}
