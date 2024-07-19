using NgocRongGold.Application.Interfaces.Monster;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.DatabaseManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.BigBoss
{
    public class BigBoss
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public readonly Dictionary<int, List<int>> MapsSpawned = new Dictionary<int, List<int>>(); // Khởi tạo từ đầu
        public readonly List<int> MapsSpawn = new List<int>();
        public long Time { get; set; }
        public long TimeResummon { get; set; }
        public Task Runtime { get; set; }
        public List<IMonster> Monsters { get; set; }

        public BigBoss(short tempId, List<int> maps)
        {
            Id = tempId;
            Name = Cache.Gi().MONSTER_TEMPLATES.FirstOrDefault(x => x.Id == Id)?.Name; // Sử dụng ?. để tránh null reference
            MapsSpawn = maps;
            Time = 0;
            TimeResummon = 0;
            Monsters = new List<IMonster>();
        }

        public void Spawn()
        {
            var randomMapIndex = ServerUtils.RandomNumber(MapsSpawn.Count);
            var randomZone = ServerUtils.RandomNumber(19);
            AddMapsSpawn(MapsSpawn[randomMapIndex], randomZone);

            var map = MapManager.Get(MapsSpawn[randomMapIndex]);
            var zone = map?.Zones[randomZone];
            var monsterSummon = zone?.MonsterMaps.FirstOrDefault(m => m.Id == Id);

            if (monsterSummon != null)
            {
                monsterSummon.Status = 5;
                monsterSummon.IsDie = false;
                monsterSummon.MonsterHandler.SetUpMonster();
                Monsters.Add(monsterSummon);
            }
        }

        public void AutoSpawn()
        {
            new Thread(new ThreadStart(() =>
            {
                while (Server.Gi().IsRunning)
                {
                    var timeServer = ServerUtils.CurrentTimeMillis();
                    if (Time < timeServer)
                    {
                        Time = TimeResummon + timeServer;
                        Spawn();
                    }
                    Thread.Sleep(1000);
                }
                Dispose();
            })).Start();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Reset(int map, int zone)
        {
            // Implement reset logic here
        }

        public void AddMapsSpawn(int map, int zone)
        {
            if (MapsSpawned.ContainsKey(map))
            {
                if (!MapsSpawned[map].Contains(zone))
                {
                    MapsSpawned[map].Add(zone);
                    NotifyBossSpawn(map, zone);
                }
            }
            else
            {
                MapsSpawned.Add(map, new List<int>() { zone });
                NotifyBossSpawn(map, zone);
            }
        }

        private void NotifyBossSpawn(int mapId, int zoneId)
        {
            var map = MapManager.Get(mapId)?.TileMap?.Name;
            if (map != null)
            {
                ClientManager.Gi().SendMessage(Service.ServerMessage($"{Name} vừa xuất hiện tại {map}"));
                Server.Gi().Logger.Print($"BigBoss {(BigBoss_Template)Id} Spawn: [Mapid: {mapId}, Zone: {zoneId}]", "red");
            }
        }
    }
}
