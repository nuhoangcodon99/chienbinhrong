using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace SignalR
{
    public class MyHub : Hub
    {
        public void SendMessage(string message)
        {
            Clients.All.ReceiveMessage(message);
        }
    }
}
