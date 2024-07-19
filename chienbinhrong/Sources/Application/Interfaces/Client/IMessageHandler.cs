using System.Threading.Tasks;
using NgocRongGold.Application.Interfaces.Map;
using NgocRongGold.Application.IO;

namespace NgocRongGold.Application.Interfaces.Client
{
    public interface IMessageHandler
    {
        void OnConnectionFail(ISession_ME client, bool isMain);

        void OnConnectOK(ISession_ME client, bool isMain);

        void OnDisconnected(ISession_ME client, bool isMain);

        Task OnMessage(Message message);
    }
}