using System.Collections.Generic;

namespace Ambassador.Lib.Models
{
    public class BotConfig
    {
        public string Token { get; init; }
        public IReadOnlyList<string> Prefixes { get; init; }
    }
}
