using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.Model.Item;

namespace NgocRongGold.Application.Extension.Namecball
{
    public class Init
    {
        public static List<int> CountNamecBall = new List<int>();
        public static List<NamecBall> NamecBalls = new List<NamecBall>();
        public static List<int> MapInit = new List<int> { 7, 43, 8, 9, 25, 11, 12, 13, 10, 33, 34, 32, 31 };
        public static List<int> PosistionX = new List<int> { 854, 1052, 822, 551, 442, 711, 925, 1148, 698, 1334, 488, 433, 591 };
        public static List<int> PosistionY = new List<int> { 432, 432, 360, 384, 336, 336, 408, 384, 288, 360, 312, 384, 312 };
        public Task Runtime { get; set; }
        public static Init instance;
        public static Init gI()
        {
            if (instance == null) instance = new Init();
            return instance;
        }
        public void Start()
        {
            new Thread(new ThreadStart(AutoInit)).Start();
        }
        public static int PickCountNamecBall()
        {
            var id = 0;
            for (int i = 0; i < 7;i++)
            {
                if (CountNamecBall.Contains(i)) continue;
                id = i;
                break;
            }
            return id;
        }
        public  void AutoInit()
        {
            while (Server.Gi().IsRunning)
            {
                var timeserver = ServerUtils.CurrentTimeMillis();
                if (DelayInit < timeserver && CountNamecBall.Count < 7)
                {
                    RoiNgocRong();
                }
            }
        }
        public static long DelayInit = 3000 + ServerUtils.CurrentTimeMillis();
        public async void AutoInitAsync()
        {
            while (Server.Gi().IsRunning)
            {
                var timeserver = ServerUtils.CurrentTimeMillis();
                if (DelayInit < timeserver && CountNamecBall.Count < 7)
                {
                    RoiNgocRong();
                }
                await Task.Delay(1000);
            }
            Server.Gi().Logger.Print("Close Namec Ball Success", "red");
        }
        public static void RoiNgocRong()
        {
            var idPick = PickCountNamecBall();
            var itemDrop = ItemCache.GetItemDefault((short)(353+ idPick));
            var randomIndex = ServerUtils.RandomNumber(MapInit.Count);
            var Maps = MapManager.Get(MapInit[randomIndex]);
            var ToaDoX = PosistionX[randomIndex];
            var ToaDoY = PosistionY[randomIndex];
            var Zone = Maps.Zones[0];
            var itemMapDrop = new ItemMap(-1, itemDrop);
            itemMapDrop.X = (short)ToaDoX;
            itemMapDrop.Y = (short)ToaDoY;
            Zone.ZoneHandler.LeaveItemMap(itemMapDrop);
            NamecBalls.Add(new NamecBall()
            {
                Id = itemDrop.Id,
                MapName = Maps.TileMap.Name,
                PlayerPick = -1,
                ZoneId = Zone.Id,
                MapId = Maps.Id,
                X = (short)ToaDoX,
                Y = (short)ToaDoY
            }) ;
            Server.Gi().Logger.Print("INIT ITEM: " + itemDrop.Id + " | Maps: " + Maps.Id + " | Zone: " + Zone.Id + " | X: " + ToaDoX + " | Y: " + ToaDoY, "cyan");
            CountNamecBall.Add(idPick);
            
        }
    }
}
