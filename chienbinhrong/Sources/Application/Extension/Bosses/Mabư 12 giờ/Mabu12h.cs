using NgocRongGold.Application.Constants;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.Model.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NgocRongGold.Application.MainTasks;

namespace NgocRongGold.Application.Extension.Bosses.Mabu12Gio
{

    public class Mabu12h
    {
        public static Mabu12h instance;
        public Task RunTime { get; set; }
        public static Mabu12h gI()
        {
            if (instance == null)
            {
                instance = new Mabu12h();
            }
            return instance;
        }
        public void Start()
        {
            Task.Factory.StartNew(AsyncAutoInit);

        }
        public async void AsyncAutoInit()
        {
            while (Server.Gi().IsRunning)
            {
                switch (Mabu12hConfig.Status)
                {
                    case Mabu12hStatus.CLOSE:
                        if (CheckStart())
                        {
                            Mabu12hConfig.Status = Mabu12hStatus.OPEN;
                            Server.Gi().Logger.PrintColor("Mabu 12gio status: Open", "red");
                            break;
                        }
                        break;
                    case Mabu12hStatus.OPEN:
                        Init();
                        Mabu12hConfig.Status = Mabu12hStatus.DURING;
                        break;
                    case Mabu12hStatus.DURING:
                        if (!CheckStart())
                        {
                            Mabu12hConfig.Status = Mabu12hStatus.CLOSE;
                            var task = new Task(Dispose);
                            task.Start();
                            Server.Gi().Logger.PrintColor("Mabu 12gio status: Close", "red");
                            break;
                        }
                        break;
                }
                await Task.Delay(1000);
            }
        }
        public bool CheckStart()
        {
            return ServerUtils.TimeNow().Hour == Mabu12hConfig.TimeStart;
        }
       
        
        public async void Dispose()
        {
            foreach (var mapId in DataCache.IdMapMabu)
            {
                var map = MapManager.Get(mapId);
                foreach (var zone in map.Zones)
                {
                    foreach(var boss in zone.Bosses.Values)
                    {
                        boss.CharacterHandler.SendDie();
                    }
                    await Task.Delay(100);
                }
                await Task.Delay(1000);
            }
            Server.Gi().Logger.Print("Dispose Mabu12gio Success !!", "red");
        }
        
        public void JoinMap(Character character)
        {
            if (TaskHandler.CheckTask(character, 31, 0)) TaskHandler.gI().PlusSubTask(character, 1);
            MapManager.JoinMap(character, 114, ServerUtils.RandomNumber(20), false, false, 0);
            character.CharacterHandler.SendMessage(Mabu12hService.sendPowerInfo("TL", 0, 50, 10));
            character.PPower = 0;
            var Flag = ServerUtils.RandomNumber(9, 10);
            character.Flag = (sbyte)Flag;
            character.CharacterHandler.SendMessage(Service.ChangeFlag1(character.Id, Flag));
        }
        public void InitMabu(int zonee)
        {
            var mapInit = MapManager.Get(120);
            var zone = mapInit.GetZoneById(zonee);
            var boss = new Boss();
            boss.CreateBoss(39);
            boss.CharacterHandler.SetUpInfo();
            zone.ZoneHandler.AddBoss(boss);
        }
        public void Init()
        {
            for (int i =0;i < DataCache.IdMapMabu.Count;i++)
            {
                var mapId = DataCache.IdMapMabu[i];
                var map = MapManager.Get(mapId);
                foreach (var zone in map.Zones)
                {
                    var boss = new Boss();
                    boss.CreateBoss(Mabu12hConfig.Bosses[i]);
                    boss.CharacterHandler.SetUpInfo();
                    zone.ZoneHandler.AddBoss(boss);
                }
            }
            Server.Gi().Logger.PrintColor("Mabu 12gio status: During", "red");
            ClientManager.Gi().SendMessageCharacter(Service.ServerMessage("Hoạt động Mabư 12h đã xuất hiện,các cụt thủ mau tham gia !"));
          
        }
    }
}
