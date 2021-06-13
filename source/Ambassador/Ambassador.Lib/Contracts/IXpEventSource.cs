using System.Threading.Tasks;
using Ambassador.Lib.Models;

namespace Ambassador.Lib.Contracts
{
    public delegate Task XpChanged(XpChangedArgs xpChangedInfo);

    public interface IXpEventSource
    {
        event XpChanged XpChanged;
    }
}