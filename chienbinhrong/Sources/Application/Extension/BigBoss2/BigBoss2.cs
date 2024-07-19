using NgocRongGold.Application.Interfaces.Monster;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.DatabaseManager;
using Org.BouncyCastle.Math.EC.Multiplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.BigBoss2
{
    public class BigBoss2
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public readonly Dictionary<int, List<int>> MapsSpawned = null;//map, list zone
        public readonly List<int> MapsSpawn = new List<int>();
        public long Time { get; set; }
        public long TimeResummon { get; set; }
        public Task Runtime { get; set; }
        public List<IMonster> Monsters { get; set; }
        public BigBoss2(short tempId, List<int> Maps)
        {
            Id = tempId;
            Name = Cache.Gi().MONSTER_TEMPLATES.FirstOrDefault(x => x.Id == Id).Name;
            MapsSpawn = Maps;
            MapsSpawned = new Dictionary<int, List<int>>();
            Time = 0;
            TimeResummon = 0;
            Monsters = new List<IMonster>();
        }
        public void Spawn()
        {
            var map = MapsSpawn[0];
            //Handle Summon
            for (int zone = 0; zone <= 19; zone++)
            {
                var monsterSummon = MapManager.Get(map).Zones[zone].MonsterMaps.FirstOrDefault(m => m.Id == Id);
                if (monsterSummon != null)
                {
                    monsterSummon.Status = 5;
                    monsterSummon.IsDie = false;
                    monsterSummon.MonsterHandler.SetUpMonster();
                    Monsters.Add(monsterSummon);
                }
            }
        }
        public void AutoSpawn()
        {
            Runtime = Task.Factory.StartNew(() =>
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
            });
        }
        public void Dispose()
        {
            Runtime.Dispose();
            GC.SuppressFinalize(this);
        }
        public void Reset(int map, int zone)
        {

        }
        public void AddMapsSpawn(int map, int zone)
        {
            
        }
    }
}
