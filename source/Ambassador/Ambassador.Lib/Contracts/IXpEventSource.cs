using System.Threading.Tasks;
using Ambassador.Lib.Models;

namespace Ambassador.Lib.Contracts
{
    public delegate Task XpChanged(XpChangedInfo xpChangedInfo);

    public interface IXpEventSource
    {
        event XpChanged XpChanged;
    }
}