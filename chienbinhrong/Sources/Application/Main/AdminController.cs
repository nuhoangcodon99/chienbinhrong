using Newtonsoft.Json;
using NgocRongGold.Application.Interfaces.Client;
using NgocRongGold.Application.Interfaces.Map;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  NgocRongGold.Application.Main
{
    public class AdminController : IMessageHandler
    {
        private readonly ISession_ME _session;

        public AdminController(ISession_ME client)
        {
            _session = client;
        }
        public void OnConnectionFail(ISession_ME client, bool isMain)
        {

        }

        public void OnConnectOK(ISession_ME client, bool isMain)
        {

        }

        public void OnDisconnected(ISession_ME client, bool isMain)
        {

        }

        public Task OnMessage(Message message)
        {

            Server.Gi().Logger.Print("ADMIN CONTROLLER ON MESSAGE: " + message.Command, "manager");
            switch (message.Command)
            {
                case 44:
                    Server.Gi().Logger.Print("ADMIN CONTROLLER CHAT: " + message.Reader.ReadUTF(), "manager");
                    break;
                case 2:
                    WritePlayer();
                    Server.Gi().Logger.Print("SEND PLAYER", "manager");

                    break;
                default:

                    break;
            }
            message?.CleanUp();
            return Task.CompletedTask;
        }
        public Task WritePlayer()
        {
            
            var msg = new Message(2);
            msg.Writer.WriteUTF(JsonConvert.SerializeObject(ClientManager.Gi().Players));
            return Task.CompletedTask;
        }
    }
}
