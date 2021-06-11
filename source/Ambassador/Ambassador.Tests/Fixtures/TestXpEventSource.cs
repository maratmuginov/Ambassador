using Ambassador.Lib.Contracts;
using Ambassador.Lib.Models;

namespace Ambassador.Tests.Fixtures
{
    public class TestXpEventSource : IXpEventSource
    {
        public event XpChanged XpChanged;

        public void RaiseXpChanged()
        {
            var xpChangedInfo = new XpChangedInfo();
            XpChanged?.Invoke(xpChangedInfo);
        }
    }
}
