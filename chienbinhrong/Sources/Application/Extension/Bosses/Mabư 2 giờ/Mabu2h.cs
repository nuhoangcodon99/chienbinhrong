using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.Model.Character;
using NgocRongGold.Application.Extension.Bosses.Mabu12Gio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace NgocRongGold.Application.Extension.Bosses.Mabu2Gio
{
    public class Mabu2h
    {
        public Task Runtime { get; set; }
        public static Mabu2h instance;
        public static Mabu2h gI()
        {
            if (instance == null)
            {
                instance = new Mabu2h();
            }
            return instance;
        }
        public void Start()
        {
            Task.Factory.StartNew(AsyncAutoInit);
        }
        public void Join(Character character)
        {
            MapManager.JoinMap(character, 127, ServerUtils.RandomNumber(20), false, false, 0);
        }
        public async void AsyncAutoInit()
        {
            while (Server.Gi().IsRunning)
            {
                switch (Mabu2hConfig.Status)
                {
                    case Mabu2hStatus.CLOSE:
                        if (CheckStart())
                        {
                            Mabu2hConfig.Status = Mabu2hStatus.OPEN;
                        }
                        break;
                    case Mabu2hStatus.OPEN:
                        Mabu2hConfig.Status = Mabu2hStatus.DURING;
                        Init();
                        break;
                    case Mabu2hStatus.DURING:
                        if (!CheckStart())
                        {
                            Mabu2hConfig.Status = Mabu2hStatus.CLOSE;
                            Dispose();
                        }
                        break;
                }
                await Task.Delay(1000);
            }
        }
        public bool CheckStart()
        {
            return ServerUtils.TimeNow().Hour == Mabu2hConfig.TimeStart;
        }
        public async void Dispose()
        {
            var mapDispose = MapManager.Get(127);
            foreach(var zone in mapDispose.Zones)
            {
                foreach(var boss in zone.Bosses.Values)
                {
                    boss.CharacterHandler.SendDie();
                    await Task.Delay(10);
                }
                await Task.Delay(100);
            }
            Server.Gi().Logger.PrintColor("Mabu 2gio Dispose Success !!!", "red");

        }
        public async void Init()
        {
            var mapInit = MapManager.Get(127);
            foreach (var zone in mapInit.Zones)
            {
                var mabu = new Boss();
                mabu.CreateBoss(43);
                mabu.CharacterHandler.SetUpInfo();
                zone.ZoneHandler.AddBoss(mabu);
                await Task.Delay(100);
            }
            Server.Gi().Logger.PrintColor("Mabu 2gio status: Open", "red");
            ClientManager.Gi().SendMessageCharacter(Service.ServerMessage("Hoạt động Mabư 2h đã xuất hiện,các cụt thủ mau tham gia !"));

        }
    }
}
