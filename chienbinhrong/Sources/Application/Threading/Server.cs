using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Extensions.Configuration;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.IO;
using NgocRongGold.Logging;
using NgocRongGold.Model.BangXepHang;
using InitData = NgocRongGold.DatabaseManager.InitData;
using Task = System.Threading.Tasks.Task;
using NgocRongGold.Application;
using NgocRongGold.Application.Extension;
using NgocRongGold.Application.Extension.ChampionShip;
using NgocRongGold.Application.Extension.Yardat;
using NgocRongGold.Application.Extension.Event;
using NgocRongGold.Application.Extension.BlackballWar;
using NgocRongGold.Application.Extension.Bosses.Mabu12Gio;
using NgocRongGold.Application.Extension.Bosses.Mabu2Gio;

using NgocRongGold.Application.Extension.Namecball;
using System.Text;
using System.Collections.Generic;
using NgocRongGold.Application.Extension.Ký_gửi;
using NgocRongGold.Application.Extension.NamecballWar;
using System.Runtime.InteropServices;
using System.Linq;
using NgocRongGold.DatabaseManager.Player;
using NgocRongGold.Model.Character;
using Figgle;
using NgocRongGold.Application.Extension.ConSoMayMan;
using NgocRongGold.Application.Extension.ChonAiDay;
using NgocRongGold.Application.Extension.DaiHoiVoThuat;
using NgocRongGold.Application.Extension.DiedRing;
using NgocRongGold.Model.Task;
using Org.BouncyCastle.Bcpg;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using NgocRongGold.DatabaseManager;
using System.Net.WebSockets;
using NgocRongGold.Application.Extension.Super_Champion;
using NgocRongGold.Application.Extension.DaiHoiVoThuat.SuperChampion;
using NgocRongGold.Application.Extension.BigBoss;
using MySqlX.XDevAPI;
using NgocRongGold.Application.Extension.NamecBattlefield;
using Microsoft.Owin.Hosting;
using Application.Constants;
using System.IO.Pipes;
using System.IO;
using NgocRongGold.Application.Extension.Dragon;

namespace NgocRongGold.Application.Threading
{
    public class Server
    {
        private static Server Instance { get; set; } = null;
        public static readonly object SQLLOCK = new object();
        public static readonly object IPLOCK = new object();
        private IPAddress IpAddress { get; set; }
        private TcpListener Listener { get; set; }
        public bool IsRunning { get; set; }
        public bool IsSaving { get; set; }
        private Thread RunServer { get; set; }
        public IServerLogger Logger { get; set; }
        public IConfiguration Config { get; set; }

        private DatabaseManager.InitData _initData;
        private Thread _serverRun;

        private ClanRunTime _clanRun;
        private MagicTreeRunTime _magicTreeRun;

        public ABXH BangXepHang;

        public ABoss ABoss;
        public AutoSpawn ABigBoss;
        public APet2 APet2;
        public Backup _Backup;
        public GameCache GameCache;

        public NamecBattlefield_Server NamecBattlefield;
        public long DelayLogin { get; set; }

        public long StartServerTime { get; set; }
        public int CountLogin { get; set; }
        public bool LockCloneGiaoDich { get; set; }

        public readonly string DROP_KEY = "baodeptrai";

        public static Server Gi()
        {
            return Instance ??= new Server();
        }

        public Server()
        {
            IpAddress = IPAddress.Parse(DatabaseManager.ConfigManager.gI().ServerHost);
            Listener = new TcpListener(IpAddress, DatabaseManager.ConfigManager.gI().ServerPort);
            RunServer = null;
        }


        public void InitServerRuntime()
        {
            //APet2.StartUpdate();
            SuKienHungVuong.Runtime();
            ABigBoss.Start();
            _clanRun.StartClan();
            _magicTreeRun.StartMagicTree();
            Yardat.Init();
            BangXepHang.Start();
            Mabu12h.gI().Start();
            Mabu2h.gI().Start();
            ConSoMayManHandler.gI().Start();
            ChonAiDay_Handler.gI().Start();
            ChampionShip.gI().Start();
            NamecBattlefield.Start();
            Init.gI().Start();
            ABoss.Start();
            _Backup.Start();
            GameCache.Update();
            BlackBallRuntime.gI().StartRunTime();
            DiedRing_Handler.gI().Start();
            ShenlongDragon.gI().StartUpdate();
            // new Thread(new ThreadStart(SaveDataAllPlayer));
        }
        private void InitServer()
        {
            _initData = new DatabaseManager.InitData();
            if (_Backup == null)
            {
                _Backup = new Backup();
            }
            if (_clanRun == null)
            {
                _clanRun = new ClanRunTime();
            }
            if (GameCache == null)
            {
                GameCache = new GameCache();
            }
            if (_magicTreeRun == null)
            {
                _magicTreeRun = new MagicTreeRunTime();
            }
            if (APet2 == null)
            {
                APet2 = new APet2();
            }
            if (NamecBattlefield == null)
            {
                NamecBattlefield = new NamecBattlefield_Server();
            }
            if (BangXepHang == null)
            {
                BangXepHang = new ABXH();
            }
            if (ABoss == null)
            {
                ABoss = new ABoss();
            }
            if (ABigBoss == null)
            {
                ABigBoss = new AutoSpawn();
            }
        }


        public ConcurrentDictionary<IPAddress, int> _connectionCounts = new ConcurrentDictionary<IPAddress, int>();
        public int _maxConnections = 15; // Giới hạn số lượng kết nối
        public TimeSpan _rateLimitPeriod = TimeSpan.FromSeconds(10); // Khoảng thời gian giới hạn yêu cầu
        private Task _serverAccecpt;

        public void StartServer(bool running, IServerLogger logger, IConfiguration config, bool isRestart)
        {
            Logger = logger;
            Config = config;
            IsRunning = running;
            StartServerTime = ServerUtils.CurrentTimeMillis();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
     FiggleFonts.Ogre.Render("SNRO"));
            Console.ResetColor();
            if (!IsRunning) return;
            InitServer();
            Logger.Print($"Start Server Success [{ConfigManager.gI().ServerPort}]!", "red");
            Listener.Start();
            InitServerRuntime();

            Socket_Server.SetupServer();
            Task.Run(() =>
                {
                    while (IsRunning)
                    {
                        try
                        {
                            var client = Listener.AcceptSocket();

                            HandleAccecptClient(client);



                            /*Thread.Sleep(50);*/
                        }
                        catch (Exception)
                        {
                            IsRunning = false;
                        }
                    }
                    SaveData();
                    IsSaving = false;
                    Task.Run(() =>
                    {
                        while (!_magicTreeRun.IsStop || !_clanRun.IsStop || !ABoss.IsStop)
                        {
                            //Ignore
                        }
                        Logger.Print("Server Shutdown Success!");
                    });
                });
        }

        public void HandleAccecptClient(Socket client)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            //bool shouldRejectConnection = false;

            var clientIPAddress = IPAddress.Parse(client.RemoteEndPoint.ToString().Split(':')[0]);
            //if (_connectionCounts.TryGetValue(clientIPAddress, out int connectionCount))
            //{
            //    if (connectionCount >= _maxConnections)
            //    {
            //        // Đạt đến giới hạn yêu cầu, từ chối kết nối mới
            //        shouldRejectConnection = true;
            //        Console.WriteLine($"Connection from {clientIPAddress} rejected. Rate limit exceeded.");
            //    }
            //}

            //if (!shouldRejectConnection)
            //{
            //    _connectionCounts.AddOrUpdate(clientIPAddress, 1, (key, count) => count + 1);
            //    Timer timer = new Timer(state =>
            //    {
            //        // Giảm số lượng kết nối của địa chỉ IP đi 1
            //        _connectionCounts.AddOrUpdate(clientIPAddress, 0, (key, count) => count - 1);
            //    }, null, _rateLimitPeriod, TimeSpan.FromMilliseconds(-1));
            Session_ME session = new Session_ME(client, clientIPAddress.ToString());
            ClientManager.Gi().Add(session);
            stopwatch.Stop();
            Logger.Info($"Accpet Session: {session.Id} | {clientIPAddress} Successful in {stopwatch.ElapsedMilliseconds} ms");
            //}
            //else
            //{
            //    client.Close();
            //}
        }
        public void StopServer()
        {
            Listener.Stop();
            IsSaving = true;
        }
        private long DelaySaveDataAllServer = 900000 + ServerUtils.CurrentTimeMillis();
        private void SaveDataAllPlayer()
        {
            while (IsRunning)
            {

                Stopwatch stp = new Stopwatch();
                Server.Gi().Logger.Print("Start Save Data All Server", "red");

                ClientManager.Gi().Players.Values.ToList().ForEach(player =>
                {
                    try
                    {
                        if (player != null)
                        {
                            CharacterDB.Update((Character)player.Character);
                        }
                    }
                    catch (Exception e)
                    {
                        ServerUtils.WriteLog("TraceLog", e.Message + "\n" + e.StackTrace + "\n ---------------- \n");
                    }
                });
                Server.Gi().Logger.Print($"Save Data All Server Success in {stp.ElapsedMilliseconds}", "red");
                DelaySaveDataAllServer = 900000 + ServerUtils.CurrentTimeMillis();

                Thread.Sleep(120000);
            }
        }
        private void SaveDataAllServer(long TimeServer)
        {
            Task.Factory.StartNew(() =>
            {
                while (IsRunning)
                {
                    if (TimeServer > DelaySaveDataAllServer)
                    {
                        Stopwatch stp = new Stopwatch();
                        Server.Gi().Logger.Print("Start Save Data All Server", "red");

                        ClientManager.Gi().Players.Values.ToList().ForEach(player =>
                        {
                            try
                            {
                                if (player != null)
                                {
                                    CharacterDB.Update((Character)player.Character);
                                }
                            }
                            catch (Exception e)
                            {
                                ServerUtils.WriteLog("TraceLog", e.Message + "\n" + e.StackTrace + "\n ---------------- \n");
                            }
                        });
                        Server.Gi().Logger.Print($"Save Data All Server Success in {stp.ElapsedMilliseconds}", "red");
                        DelaySaveDataAllServer = 900000 + ServerUtils.CurrentTimeMillis();
                    }
                    Thread.Sleep(1000);
                }
            });
        }
        private void SaveData()
        {
            ClientManager.Gi().Clear();
            Logger.Print("Save DATA Player Server Sucess!!!", "red");
            KyGUIMySQL.UpdateAllItem();
            Logger.Print("Save DATA ITEM KY GUI Server Sucess!!!", "red");
            SuperChampion_Database.Update();
            Logger.Print("Save DATA SUPER CHAMPIONER Server Sucess!!!", "red");

        }

        public void RestartServer()
        {

            StopServer();
            Task.Run(() =>
            {
                while (IsSaving || !_magicTreeRun.IsStop || !_clanRun.IsStop || !ABoss.IsStop)
                {
                    continue;
                }
                StartServer(true, Logger, Config, true);
                Logger.Print("Server Restart Success!", "red");
            });
        }
    }
}