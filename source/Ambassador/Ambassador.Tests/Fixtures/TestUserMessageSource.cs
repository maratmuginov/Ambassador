using Ambassador.Lib.Contracts;
using Ambassador.Lib.Models;

namespace Ambassador.Tests.Fixtures
{
    public class TestUserMessageSource : IUserMessageSource
    {
        public event UserMessageReceived UserMessageReceived;

        public void RaiseUserMessageReceived(string messageContent)
        {
            var args = new UserMessageArgs
            {
                UserId = ulong.MaxValue,
                ChannelId = ulong.MaxValue,
                MessageContent = messageContent
            };
            UserMessageReceived?.Invoke(args);
        }
    }
}
