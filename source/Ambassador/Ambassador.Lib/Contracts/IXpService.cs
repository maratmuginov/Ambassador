using Ambassador.Lib.Models;
using System.Threading.Tasks;

namespace Ambassador.Lib.Contracts
{
    public delegate Task LeveledUp(LeveledUpArgs leveledUpInfo);

    public interface IXpService
    {
        event LeveledUp LeveledUp;
    }
}