using Ambassador.Lib.Contracts;
using Ambassador.Lib.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambassador.Lib.Services
{
    public class XpService : IXpService
    {
        public event LeveledUp LeveledUp;

        private readonly IReadOnlyList<int> _xpTresholds;
        private readonly ConcurrentDictionary<ulong, byte> _userLevels = new();

        public XpService(IXpEventSource eventSource, XpThresholdsConfig xpThresholdsConfig)
        {
            _xpTresholds = xpThresholdsConfig.Tresholds;
            eventSource.XpChanged += OnXpChanged;
        }

        private Task OnXpChanged(XpChangedArgs xpChangedInfo)
        {
            byte oldLevel = _userLevels.GetOrAdd(xpChangedInfo.UserId, 1);

            var newLevel = GetNewLevel(xpChangedInfo);

            if (newLevel > oldLevel)
            {
                var info = new LeveledUpArgs
                {
                    UserId = xpChangedInfo.UserId,
                    ChannelId = xpChangedInfo.ChannelId,
                    NewLevel = newLevel
                };
                _userLevels.AddOrUpdate(info.UserId, _ => info.NewLevel, (_, _) => info.NewLevel);
                LeveledUp?.Invoke(info);
            }

            return Task.CompletedTask;
        }

        private byte GetNewLevel(XpChangedArgs xpChangedInfo)
        {
            byte newLevel = 1;
            foreach (var xpTreshold in _xpTresholds)
            {
                if (xpTreshold > xpChangedInfo.TotalXp)
                    break;
                newLevel++;
            }
            return newLevel;
        }
    }
}
