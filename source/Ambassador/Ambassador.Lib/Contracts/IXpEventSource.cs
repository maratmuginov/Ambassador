using System.Threading.Tasks;
using Ambassador.Lib.Models;

namespace Ambassador.Lib.Contracts
{
    public delegate Task XpChanged(XpChangedArgs e);

    public interface IXpEventSource
    {
        event XpChanged XpChanged;
    }
}