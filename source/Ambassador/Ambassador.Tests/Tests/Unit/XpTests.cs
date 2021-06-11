using Ambassador.Lib.Contracts;
using Ambassador.Lib.Models;
using Ambassador.Lib.Services;
using System.Threading.Tasks;
using Ambassador.Tests.Fixtures;
using Xunit;

namespace Ambassador.Tests.Tests.Unit
{
    public class XpTests
    {
        [Fact]
        public void LevelUpIsRaisedWhenMemberLevelsUp()
        {
            var eventSource = new TestXpEventSource();
            IXpService sut = new XpService(eventSource);
            sut.LeveledUp += OnLevelUp;
            bool raised = false;

            eventSource.RaiseXpChanged();

            Task OnLevelUp(LeveledUpInfo levelupinfo)
            {
                raised = true;
                return Task.CompletedTask;
            }

            Assert.True(raised);
        }

    }
}
