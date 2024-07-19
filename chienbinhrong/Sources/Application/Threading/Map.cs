using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.Interfaces.Map;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Manager;
using NgocRongGold.DatabaseManager;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Item;
using NgocRongGold.Model.Map;
using static System.GC;
using NgocRongGold.Model.Monster;
using NgocRongGold.Model.Template;
using NgocRongGold.Model.Info;
using NgocRongGold.Model.Clan;
using NgocRongGold.Application.Extension.ChampionShip;
using Application.Interfaces.Zone;
using Application.Interfaces.Map;

namespace NgocRongGold.Application.Threading
{
    public class Map : IMap
    {
        public int Id { get; set; }
        public List<IZone> Zones { get; set; }
        public TileMap TileMap { get; set; }
        public long TimeMap { get; set; }
        public bool IsRunning { get; set; }
        public bool IsStop { get; set; }
        public Task HandleZone { get; set; }
        public IZone zone { get; set; }

        public Map()
        {

        }

        public Map(int id, TileMap tileMap, bool isStart = true)
        {
            Id = id;
            Zones = new List<IZone>();
            TimeMap = -1;
            IsRunning = false;
            IsStop = false;
            TileMap = tileMap ?? Cache.Gi().TILE_MAPS.FirstOrDefault(t => t.Id == Id);
            SetZone();
            if (isStart) Start();
        }

        public void Start()
        {
            if (IsRunning) return;
            IsRunning = true;
            StartHanleZone();
        }
        public bool IsMapNotChangeZone()
        {
            return true;
        }

        public void StartHanleZone()
        {
            long t1, t2;
            async void Action()
            {
                while (IsRunning)
                {
                    try
                    {
                        t1 = ServerUtils.CurrentTimeMillis();
                        foreach(var zone in Zones.Where(zone=> zone.Characters.Count > 0))
                        {
                            await zone.ZoneHandler.Update();
                        }
                        t2 = ServerUtils.CurrentTimeMillis() - t1;
                        await Task.Delay((int)Math.Abs(1000 - t2));
                    }
                    catch(Exception e)
                    {
                        Server.Gi().Logger.Print(e.StackTrace + "\n" + e.Message);
                    }
                }
                await HandleZone;
                Zones.ForEach(zone => zone.ZoneHandler.Close());
                Zones.Clear();
                Zones = null;
                TileMap = null;
                HandleZone.Dispose();
                HandleZone = null;
                SuppressFinalize(this);
            }
            HandleZone = new Task(Action);
            HandleZone.Start();
        }

        private async Task Update()
        {
            Parallel.ForEach(Zones, ZoneUpdate);
            await Task.Delay(10);
        }

        private async void ZoneUpdate(IZone zone)
        {
            if (zone.Characters.Count > 0)
            {
                await zone.ZoneHandler.Update();
            }
        }

        public void Close()
        {
            IsRunning = false;
        }

        public void SetZone()
        {
            for (var i = 0; i < TileMap.ZoneNumbers; i++)
            {
                Zones.Add(new Zone(i, this));
               
            }

        }


        public  void JoinZone(Character character, int id, bool isDefault = false, bool isTeleport = false, int typeTeleport = 0)
        {
            
            if (id == -1)
            { 
                GetZoneNotMaxPlayer()?.ZoneHandler.JoinZone(character, isDefault, isTeleport, typeTeleport);
            }
            else
            {
                GetZoneById(id)?.ZoneHandler.JoinZone(character, isDefault, isTeleport, typeTeleport);
            }
        }
        public void JoinRandomZone(Character character, int id, bool isDefault = false, bool isTeleport = false, int typeTeleport = 0)
        {
            if (GetZoneById(id).Characters.Count >= 15)
            {
                GetZoneNotMaxPlayer().ZoneHandler.JoinZone(character, isDefault, isTeleport, typeTeleport);
            }
            else
            {

                GetZoneById(id)?.ZoneHandler.JoinZone(character, isDefault, isTeleport, typeTeleport);
            }
            
        }
       
        public IZone GetZoneNotMaxPlayer()
        {
            return (Zone)Zones.FirstOrDefault(x => x.Characters.Count < TileMap.MaxPlayers);
        }

        public IZone GetZonePlayer()
        {
            return Zones.FirstOrDefault(x => x.Characters.Count >= 1);//4
        }
        public IZone GetZoneNotBoss()
        {
            return Zones.FirstOrDefault(x => x.Bosses.Count == 0);
        }
        public IZone MobAlive()
        {
            return Zones.FirstOrDefault(x => x.MonsterMaps.Count >= 0);//4
        }
        public IZone MobDieAll()
        {
            return Zones.FirstOrDefault(x => x.MonsterMaps.Count ==0);//4
        }
      
        public Character GetChar(int id)
        {
            var zonn = Zones.FirstOrDefault(x => x.Id == id);
            return zonn == null ? null : zonn.Characters.FirstOrDefault(c => c.Key > 0).Value;
        }

        public IZone GetZoneNotMaxPlayer(int id)
        {
            return (Zone)Zones.FirstOrDefault(x => x.Id == id && x.Characters.Count < TileMap.MaxPlayers);
        }
        
        public IZone GetZoneById(int id)
        {
            return Zones.FirstOrDefault(x => x.Id == id);
        }
        public IZone GetZoneRandomNotHasPlayer()
        {
            return Zones.FirstOrDefault(x => x.Characters.Count == 0);
        }
       
        public void OutZone(ICharacter character)
        {
            Zones.FirstOrDefault(x => x.Id == character.InfoChar.ZoneId)?.ZoneHandler?.OutZone(character);
        }
        
        
    }
}