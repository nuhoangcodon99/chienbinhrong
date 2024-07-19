using NgocRongGold.Application.Constants;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.Model.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.BlackballWar
{
    public class BlackBallRuntime
    {
        public Task Runtime { get; set; }
        public static BlackBallRuntime instance;
        public static BlackBallRuntime gI()
        {
            if (instance == null)
            {
                instance = new BlackBallRuntime();
            }
            return instance;
        }
        public void StartRunTime()
        {
            new Thread(new ThreadStart(RuntimeClass)).Start();
        }
        public void RuntimeClass()
        {
            while (Server.Gi().IsRunning)
            {
                switch (BlackballCache.Status)
                {
                    case BlackballStatus.CLOSE:
                        if (CheckStart())
                        {
                            BlackballCache.currTimeCanPick = DataCache._1MINUTES * 30 + ServerUtils.CurrentTimeMillis();
                            ChangeStatus(BlackballStatus.OPEN);
                            InitBlackball();
                            Server.Gi().Logger.Print("Blackball status: init black ball", "red");
                        }
                        break;
                    case BlackballStatus.OPEN:
                        if (!CheckStart())
                        {
                            ChangeStatus(BlackballStatus.REWARD);
                            BlackballCache.Delay = DataCache._1MINUTES * 10 + ServerUtils.CurrentTimeMillis();
                            Server.Gi().Logger.Print("Blackball status: reward", "red");

                        }
                        break;
                    case BlackballStatus.REWARD:
                        if (BlackballCache.Delay < ServerUtils.CurrentTimeMillis())
                        {
                            ChangeStatus(BlackballStatus.CLOSE);
                            Server.Gi().Logger.Print("Blackball status: close", "red");

                        }
                        break;
                }
                Thread.Sleep(1000);
            }
        }
        public async void InitBlackball()
        {
            for (int i = 0; i < BlackballCache.ListMapNRSD.Count; i++)
            {
                var item = ItemCache.GetItemDefault((short)BlackballCache.ListNRSD[i]);
                var mapBlackball = MapManager.Get(BlackballCache.ListMapNRSD[i]);
                foreach (var zone in mapBlackball.Zones)
                {

                    if (zone.ItemMaps.Count > 0)
                    {
                        foreach (var itemMap2 in zone.ItemMaps.Values)
                        {
                            zone.ZoneHandler.RemoveItemMap(itemMap2.Id);
                            await Task.Delay(100);
                        }
                    }
                    var itemMap = new ItemMap(-1, item);
                    switch (i)
                    {
                        case 3:
                            itemMap.X = 1031;
                            itemMap.X = 336;
                            break;
                        case 5:
                            itemMap.X = 760;
                            itemMap.Y = 240;
                            break;
                        default:
                            itemMap.X = 1031;
                            itemMap.X = 360;
                            break;
                    }
                    zone.ZoneHandler.LeaveItemMap(itemMap);
                    await Task.Delay(10);
                }
                
                await Task.Delay(1000);
            }
        }
        public bool CheckStart()
        {
            return BlackballCache.TimeStart == ServerUtils.TimeNow().Hour;
        }
        public void ChangeStatus(BlackballStatus status)
        {
            BlackballCache.Status = status;
        }
    }
}
