using Ambassador.Lib.Models;
using System.Threading.Tasks;

namespace Ambassador.Lib.Contracts
{
    public delegate Task UserMessageReceived(UserMessageArgs e);
    public interface IUserMessageSource
    {
        event UserMessageReceived UserMessageReceived;
    }
}