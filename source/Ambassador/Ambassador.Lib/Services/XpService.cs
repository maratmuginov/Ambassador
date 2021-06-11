using System.Threading.Tasks;
using Ambassador.Lib.Contracts;
using Ambassador.Lib.Models;

namespace Ambassador.Lib.Services
{
    public class XpService : IXpService
    {
        public event LeveledUp LeveledUp;

        private readonly IXpEventSource _eventSource;

        public XpService(IXpEventSource eventSource)
        {
            _eventSource = eventSource;
            _eventSource.XpChanged += OnXpChanged;
        }

        private Task OnXpChanged(XpChangedInfo xpChangedInfo)
        {
            var leveledUpInfo = new LeveledUpInfo();
            LeveledUp?.Invoke(leveledUpInfo);
            return Task.CompletedTask;
        }
    }
}
