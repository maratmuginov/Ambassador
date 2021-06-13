using Ambassador.Lib.Contracts;
using Ambassador.Lib.Models;

namespace Ambassador.Tests.Fixtures
{
    public class TestXpEventSource : IXpEventSource
    {
        public event XpChanged XpChanged;

        public void RaiseXpChanged(int totalXp)
        {
            var xpChangedInfo = new XpChangedArgs
            {
                UserId = default,
                ChannelId = default,
                TotalXp = totalXp
            };
            XpChanged?.Invoke(xpChangedInfo);
        }
    }
}
