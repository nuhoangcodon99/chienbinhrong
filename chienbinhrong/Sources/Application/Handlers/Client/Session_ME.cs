using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.InteropServices;
using NgocRongGold.Application.Interfaces.Client;
using NgocRongGold.Application.Interfaces.Map;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.DatabaseManager.Player;
using NgocRongGold.Model;
using NgocRongGold.Application.Constants;
using NgocRongGold;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using MySqlX.XDevAPI;
using Google.Protobuf.WellKnownTypes;
using System.Threading.Channels;
using NgocRongGold.Model.Character;
using System.Numerics;
using Org.BouncyCastle.Crypto.Paddings;
using Newtonsoft.Json.Linq;

namespace NgocRongGold.Application.Constants
{
    public class Session_ME : ISession_ME
    {
        public static int BaseId { get; set; }
        public int Id { get; set; }
        public int recieveByteCount = 0;
        public int sendByteCount = 0;

        private readonly string KEY = "baodeptrai";
        public bool IsDisconnected { get; set; }
        private bool IsGetKeyComplete { get; set; }
        private sbyte CurR { get; set; }
        private sbyte CurW { get; set; }
        private BinaryReader Reader { get; set; } //dis
        private BinaryWriter Writer { get; set; } //dos
        private Socket Client { get; set; }
        public List<Player> Players { get; set; }
        public Player Player { get; set; }
        private sbyte Type { get; set; }
        public sbyte ZoomLevel { get; set; }
        private bool IsGPS { get; set; }
        private int Width { get; set; }
        private int Height { get; set; }
        private bool IsQwerty { get; set; }
        private bool IsTouch { get; set; }
        private string PlastForm { get; set; }
        public string Version { get; set; }
        private sbyte LanguageId { get; set; }
        private int Provider { get; set; }
        private string Agent { get; set; }
        public string IpV4 { get; set; }
        public bool IsLogin { get; set; }
        public IMessageHandler MessageHandler { get; set; }
        public MessageCollector HandlerMsg { get; set; }
        public Sender HandlerSender { get; set; }
        public bool IsNewVersion { get; set; }
        public long TimeConnected { get; set; }

        public Session_ME(Socket client, string ipV4)
        {
            Id = BaseId++;
            Client = client;
            var stream = new NetworkStream(client);
            Reader = new BinaryReader(stream);
            Writer = new BinaryWriter(stream);
            MessageHandler = new Controller(this);
            IsDisconnected = false;
            IsGetKeyComplete = false;
            CurR = 0;
            CurW = 0;
            Type = 0;
            ZoomLevel = 2;
            IsQwerty = false;
            IsTouch = false;
            IsGPS = false;
            PlastForm = null;
            Version = null;
            LanguageId = 0;
            Provider = 0;
            Agent = null;
            IpV4 = ipV4;
            IsLogin = false;
            HandlerMsg = new MessageCollector(this);
            HandlerSender = new Sender(this);
            IsNewVersion = false;
            TimeConnected = ServerUtils.CurrentTimeMillis();
        }

        public void StartSession()
        {

           // HandlerMsg.Start();
           // HandlerSender.Start();
        }

        public bool IsConnected()
        {
            return !IsDisconnected;
        }

        public void SendMessage(Message message)
        {
            try
            {
                if (IsConnected() && message != null && HandlerSender != null)
                {
                    HandlerSender.AddMessage(message);
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error SendMessage in Session_ME.cs: {e.Message}\n{e.StackTrace}", e);
            }   
        }
        private void DoSendMessage2(int size)
        {
            var sizeTemp = 0;
            var a = size;
            var b = 0;
            var c = 0;
            if (size > 256)
            {
                a = size % 256;
                sizeTemp = (size - a) / 256;
                if (sizeTemp <= 256)
                {
                    b = sizeTemp;
                    c = 0;
                }
                else
                {
                    b = sizeTemp % 256;
                    c = (sizeTemp - b) / 256;
                }
            }
            Writer.Write(WriteKey((sbyte)(a - 128)));
            Writer.Write(WriteKey((sbyte)(b - 128)));
            Writer.Write(WriteKey((sbyte)(c - 128)));
        }
        private void DoSendMessage(Message message)
        {
            try
            {
                var cmd = message.Command;
                var data = message.GetData();
                Writer?.Write(IsGetKeyComplete ? WriteKey(cmd) : cmd);
                if (data != null)
                {
                    var size = data.Length;
                    if (cmd is -32 or -66 or 11 or -67 or -74 or -87 or 66)
                    {
                        DoSendMessage2(size);
                    }
                    else if (IsGetKeyComplete)
                    {
                        Writer.Write(WriteKey((sbyte)(size >> 8)));
                        Writer.Write(WriteKey((sbyte)(size & 0xFF)));
                    }
                    else
                    {
                          Writer.Write((sbyte)(size & 65280));
                          Writer.Write((sbyte)(size & 0xFF));
                    }

                    if (IsGetKeyComplete)
                    {
                        for (var i = 0; i < size; ++i)
                        {
                            data[i] = WriteKey(data[i]);
                        }
                    }
                    Writer.Write(ServerUtils.ConvertSbyteToByte(data));
                }
                else
                {
                    Writer.Write((short)0);
                }
                Writer.Flush();
            }
            catch (Exception)
            {
                //Server.Gi().Logger.Print("Close message because SendMessage Error in Session_Me.cs", "manager");
                CloseMessage();
            }

        }

        private sbyte ReadKey(sbyte b)
        {
            sbyte[] bytes = ServerUtils.ConvertArrayByteToSByte(Encoding.ASCII.GetBytes(KEY));
            sbyte i = (sbyte)((bytes[CurR++] & 0xFF) ^ (b & 0xFF));
            if (CurR >= (sbyte)bytes.Length)
            {
                CurR %= (sbyte)bytes.Length;
            }   
            return i;
        }

        private sbyte WriteKey(sbyte b)
        {
            sbyte[] bytes = ServerUtils.ConvertArrayByteToSByte(Encoding.ASCII.GetBytes(KEY));
            sbyte i = (sbyte)((bytes[CurW++] & 0xFF) ^ (b & 0xFF));
            if (CurW >= bytes.Length)
            {
                CurW %= (sbyte)bytes.Length;
            }
            return i;
        }
        public void SetupAdmin()
        {
            MessageHandler = new AdminController(this);
            var m = new Message(1);
            m.Writer.WriteInt(1);
            DoSendMessage(m);
            IsGetKeyComplete = true;
            Server.Gi().Logger.Print("Manager Server Connected Form IpAddress: " + IpV4);
        }
        public byte[] BlockOtherClient(Message message)
        {
            try
            {
                var slogan = message.Reader.ReadUTF();
                if (!slogan.ToLower().Equals(DataCache.SLOGAN_SERVER))
                {
                    CloseMessage();
                    Server.Gi().Logger.Print($"Connection from {this.IpV4} rejected. Client Block | Reason: (Not Client Server).", "manager");
                    return null;
                }
                var length = message.Reader.ReadInt();
                var key = new byte[length];
                for (int i = 0; i < length; i++)
                {
                    key[i] = (byte)message.Reader.ReadByte();
                }
                if (key == null || !ServerUtils.ConvertArrayByteToString(key).ToLower().Equals(DataCache.KEY_SERVER))
                {
                    CloseMessage();
                    Server.Gi().Logger.Print($"Connection from {this.IpV4} rejected. Client Block | Reason: (Not Client Server).", "manager");
                    return null;
                }
                return key;
            }
            finally
            {
                message.CleanUp();
            }
        }
        public void HansakeMessage()
        {
            try
            {
                
                var bytes = ServerUtils.ConvertArrayByteToSByte(Encoding.ASCII.GetBytes(KEY));
                var m = new Message((sbyte)-27);
                m.Writer.WriteByte(bytes.Length);
                m.Writer.WriteByte(bytes[0]);
                for (int i = 1; i < bytes.Length; i++)
                {
                    m.Writer.WriteByte(bytes[i] ^ bytes[i - 1]);
                }
                m.Writer.WriteUTF("");
                m.Writer.WriteInt(0);
                m.Writer.WriteByte(0);
                DoSendMessage(m);
                IsGetKeyComplete = true;
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error Hansake Message in Session_ME.cs: {e.StackTrace}", e);
            }
        }

        public void SetConnect(Message message)
        {
            try
            {
                Type = message.Reader.ReadByte();
                ZoomLevel = message.Reader.ReadByte();
                IsGPS = message.Reader.ReadBoolean();
                Width = message.Reader.ReadInt();
                Height = message.Reader.ReadInt();
                IsQwerty = message.Reader.ReadBoolean();
                IsTouch = message.Reader.ReadBoolean();
                PlastForm = message.Reader.ReadUTF();
               // BlockOtherClient(message);
            }
            catch (Exception)
            {
                CloseMessage();
            }
            finally
            {
                message?.CleanUp();
            }
        }
        
        public bool LoginGame(string c_username, string c_password, string c_version, sbyte c_type, Message message)
        {
            if (IsLogin) return false;
            try
            {

                var username = c_username;
                var password = c_password;
                Version = c_version;
                Type = c_type;
                if (int.Parse(c_version.Replace(".", "")) > 213)
                {
                    IsNewVersion = true;
                }
                var players = UserDB.Login(username, password);
                if (players != null)
                {
                    UserDB.UpdateLogin(players.Id, 1);
                    Player = players;
                    Player.Session = this;
                    IsLogin = true;
                    Server.Gi().Logger.Info($"Username: {username} - Login game success version: {Version} ");
                    return true;
                }
                else
                {
                    IsLogin = false;
                    Server.Gi().Logger.Info($"Username: {username} - Login game failed password: {password} ");
                    return false;

                }

            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error Login in Session_ME.cs: {e.Message} \n {e.StackTrace}", e);
                IsLogin = false;
                return false;
            }
            finally
            {
                message?.CleanUp();
            }
        }

        public void Disconnect()
        {
            if (!IsConnected()) return;
            MessageHandler.OnDisconnected(this, true);
            ClearNetwork();
            Server.Gi().Logger.Info($"Session: {Id} Disconnecd.!");
        }

        public void CloseMessage()
        {
            if (IsConnected())
            {
                ClientManager.Gi().KickSession(this);
            }
        }

        public void ClearNetwork()
        {
            IsDisconnected = true;
            IsLogin = false;
            Reader.Close();
            Writer.Close();
            Client.Close();
            Client = null;
            Player = null;
            MessageHandler = null;
            Dispose();
            
        }

        public class MessageCollector
        {
            private readonly Task _Collector;
            public MessageCollector(Session_ME session)
            {
                //new Thread(new ThreadStart(() =>
                //{
                _Collector = Task.Run(() =>
                {
                    while (session.IsConnected())
                    {

                        Message message = ReadMessage(session);
                        if (message == null)
                        {
                            break;
                        }
                        try
                        {
                            session.MessageHandler.OnMessage(message);  
                           // Thread.Sleep(1);
                        }
                        catch (Exception e)
                        {
                            Server.Gi().Logger.Error($"Error Message Collector in Session_ME.cs: {e.Message}\n{e.StackTrace}", e);
                        }

                    }
                    session.CloseMessage();
                    //_Collector.Dispose();
                    GC.SuppressFinalize(this);
                    // return Task.CompletedTask;
                });
            }





            private Message ReadMessage(Session_ME session)
            {
                try
                {
                    var cmd = session.Reader.ReadSByte();
                    if (session.IsGetKeyComplete)
                    {
                        cmd = session.ReadKey(cmd);
                    }
                    var size = 0;
                    if (session.IsGetKeyComplete)
                    {
                        var b1 = session.Reader.ReadSByte();
                        var b2 = session.Reader.ReadSByte();
                        size = (session.ReadKey(b1) & 255) << 8 | session.ReadKey(b2) & 255;
                    }
                    else
                    {
                        int num2 = (int)session.Reader.ReadSByte();
                        sbyte b4 = session.Reader.ReadSByte();
                        size = ((num2 & 65280) | ((int)b4 & 255));
                    }
                    // if (size > 2000) //500
                    // {
                    //     Server.Gi().Logger.Info($"IP co dau hieu bat thuong ----------- Ban IP: {session.IpV4}...");
                    //     FireWall.BanIp(session.IpV4);
                    //     return null;
                    // }
                    var data = new sbyte[size];
                    var src = session.Reader.ReadBytes(size);
                    Buffer.BlockCopy(src, 0, data, 0, size);
                    //     session.recieveByteCount += 5 + size;
                    //      int num3 = session.recieveByteCount + session.sendByteCount;
                    //    Server.Gi().Logger.Print(string.Concat(new object[]
                    //    {
                    //        "Recieve: ",
                    //num3 / 1024,
                    //".",
                    //num3 % 1024 / 102,
                    //"Kb"
                    //    }));
                    if (session.IsGetKeyComplete)
                    {
                        for (var i = 0; i < data.Length; i++)
                        {
                            data[i] = session.ReadKey(data[i]);
                        }
                    }
                    return new Message(cmd, data);
                }
                catch (Exception)
                {
                    return null;
                }
            }

        }



        public class Sender
        {
            private readonly Task _Sender;
            private readonly ConcurrentQueue<Message> _listMessage;

            public Sender(Session_ME session)
            {
                _listMessage = new ConcurrentQueue<Message>();
                _Sender = Task.Run(() =>
                {
                    while (session != null && session.IsConnected())
                    {
                        try
                        {

                            if (session.IsGetKeyComplete)
                            {
                                while (_listMessage.Count > 0)
                                {
                                    _listMessage.TryDequeue(out Message messaged);
                                    session.DoSendMessage(messaged);
                                }
                            }


                        }
                        catch (Exception)
                        {
                            // ignored
                        }

                        Thread.Sleep(1);

                    }
                    //_Sender.Dispose();
                    GC.SuppressFinalize(this);
                    //return Task.CompletedTask;
                });
            }


           public void AddMessage(Message message)
            {
                // lock (_listMessage)
                // {
                _listMessage.Enqueue(message);
                // }
            }


        }
    

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}