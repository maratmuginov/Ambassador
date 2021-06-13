using System;

namespace Ambassador.Lib.Models
{
    public class XpThrottleConfig
    {
        public int Count { get; init; }
        public TimeSpan Duration { get; init; }
    }
}
