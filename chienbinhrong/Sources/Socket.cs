using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;
using Microsoft.Extensions.Primitives;
using NgocRongGold.Application.Interfaces.Character;
using System.Collections.Concurrent;

namespace NgocRongGold
{
	public class Socket_Client
	{
		
		// Token: 0x020000C4 RID: 196
		
	}
    
    public class Socket_Server
	{
        #region classOther
        public class vSocket
        {
            public Socket _Socket { get; set; }
            public string _Name { get; set; }
            public vSocket(Socket socket)
            {
                this._Socket = socket;
            }
        }
        public class vMessage
        {
            // Token: 0x0400003D RID: 61
            public int cmd;

            // Token: 0x0400003E RID: 62
            public object data;
        }
        public class Socket_Server_Service
        {
            public static Socket_Server_Service instance;
            public static Socket_Server_Service gI()
            {
                if (instance == null) instance = new Socket_Server_Service();
                return instance;
            }
            public ConcurrentDictionary<int, string> SendListInfoPlayers()
            {
                var data = new ConcurrentDictionary<int, string>();
                var characters = ClientManager.Gi().Characters;
                foreach (var character in characters)
                {
                    data.TryAdd(character.Key, character.Value.Name);
                }
                return data;
            }
        }
        #endregion
        #region Main
        public static void SetupServer()
        {
          
            
        }
        private static void onMessage(string data, Socket socket)
        {
            Socket_Server.vMessage vMessage = JsonConvert.DeserializeObject<Socket_Server.vMessage>(data);
            int cmd = vMessage.cmd;
            switch (cmd)
            {
                case 0:
                    break;
                case 1://manager yêu cầu gửi danh sách tài khoản hiện tại
                    break;
                case 2://manager yêu cầu gửi danh sách nhân vật hiện tại 
                    sendData(new vMessage() {
                        cmd = 2,
                        data = JsonConvert.SerializeObject(Socket_Server_Service.gI().SendListInfoPlayers()),
                        });

                    break;
                default:
                    break;
            }
            Server.Gi().Logger.Print(data, "manager");
        }
        public static void sendData(object data)
        {
            new Thread(delegate ()
            {
                byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
                bool connected = Socket_Server.__ClientSockets._Socket.Connected;
                if (connected)
                {
                    Socket_Server.sendDataSelectSocket(Socket_Server.__ClientSockets._Socket, bytes);
                }
            })
            {
                IsBackground = true
            }.Start();
        }
        private static void ReceiveCallback(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            bool connected = socket.Connected;
            if (connected)
            {
                int num = 0;
                try
                {
                    num = socket.EndReceive(ar);
                }
                catch
                {
                }
                bool flag = num != 0;
                if (flag)
                {
                    try
                    {
                        byte[] array = new byte[num];
                        Array.Copy(Socket_Server._buffer, array, num);
                        string @string = Encoding.UTF8.GetString(array);
                        Socket_Server.onMessage(@string, socket);
                        socket.BeginReceive(Socket_Server._buffer, 0, Socket_Server._buffer.Length, SocketFlags.None, new AsyncCallback(Socket_Server.ReceiveCallback), socket);
                    }
                    catch (Exception ex)
                    {
                        Server.Gi().Logger.Print("ReceiveCallback Error: " + ex.ToString(), "manager");
                        goto CloseSocket;
                    }
                    return;
                }
            }
        CloseSocket:

            try
            {
                Socket_Server.vSocket vSocket = Socket_Server.__ClientSockets;
                if (vSocket._Socket.RemoteEndPoint.ToString().Equals(socket.RemoteEndPoint.ToString()))
                {
                    try
                    {
                        vSocket._Socket.Close();
                    }
                    catch
                    {
                    }
                    Socket_Server.__ClientSockets = null;
                }
            }
            catch
            {
            }

        }
        private static void sendDataSelectSocket(Socket socket, byte[] b)
        {
            socket.BeginSend(b, 0, b.Length, SocketFlags.None, new AsyncCallback(Socket_Server.SendCallback), socket);
            Socket_Server._serverSocket.BeginAccept(new AsyncCallback(Socket_Server.AppceptCallback), null);
        }

        private static void AppceptCallback(IAsyncResult ar)
        {
            Socket socket = Socket_Server._serverSocket.EndAccept(ar);
            Socket_Server.__ClientSockets = new Socket_Server.vSocket(socket);
            socket.BeginReceive(Socket_Server._buffer, 0, Socket_Server._buffer.Length, SocketFlags.None, new AsyncCallback(Socket_Server.ReceiveCallback), socket);
            Socket_Server._serverSocket.BeginAccept(new AsyncCallback(Socket_Server.AppceptCallback), null);
        }

        private static void SendCallback(IAsyncResult AR)
        {
            ((Socket)AR.AsyncState).EndSend(AR);
        }

        #endregion
        #region Propreties
        private static byte[] _buffer = new byte[2048];

        private static Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static Socket_Server.vSocket __ClientSockets { get; set; }
        #endregion 
    }
}
