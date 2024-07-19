using Application.Interfaces.Map;
using Application.Interfaces.Zone;
using Model.Zone;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Extension.Bosses.Tau77;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.DatabaseManager;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Item;
using NgocRongGold.Model.Map;
using Org.BouncyCastle.Math.Field;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Security;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Map
{
    public class MapOffline : IMap
    {
        public int Id { get; set; }
        public List<IZone> Zones { get; set; }
        public Dictionary<int, IZone> ZoneOfflines { get; set; }
        public List<Boss> BossOfflines { get; set; }
        public TileMap TileMap { get; set; }
        public long TimeMap { get; set; }
        public bool IsRunning { get; set; }
        public bool IsStop { get; set; }
        public Task HandleZone { get; set; }
        public IZone zone { get; set; }
        public MapOffline(int id)
        {
            Id = id;
            zone = null;
            Zones = null;
            ZoneOfflines = new Dictionary<int, IZone>();
            BossOfflines = new List<Boss>();
            TimeMap = -1;
            IsRunning = true;
            IsStop = false;
            TileMap = Cache.Gi().TILE_MAPS.FirstOrDefault(t => t.Id == Id);
            StartHandleZone();
        }
        public void SetUpZone(int id)
        {
            var zone = GetZoneById(id);
            switch (this.Id)
            {
                case 22:
                    {
                        if (zone.ItemMaps.Count > 0) break;
                        var item = ItemCache.GetItemDefault(74);//74
                        short x = 0;
                        short y = 0;
                        x = 55;
                        y = 325;
                        zone.ItemMaps.TryAdd(0, new ItemMap(-1)
                        {
                            Id = 0,
                            PlayerId = -1,
                            Item = item,
                            X = x,
                            Y = y
                        });
                        break;
                    }
                case 21:
                case 23:
                    {
                        if (zone.ItemMaps.Count > 0) break;
                        var item = ItemCache.GetItemDefault(74);//74
                        short x = 0;
                        short y = 0;
                        x = 632;
                        y = 325;
                        zone.ItemMaps.TryAdd(0, new ItemMap(-1)
                        {
                            Id = 0,
                            PlayerId = -1,
                            Item = item,
                            X = x,
                            Y = y
                        });
                        break;
                    }
                case 45:
                    {
                        var mrpopo = new Boss();
                        mrpopo.CreateBossNoAttack(90, 301, 408);
                        mrpopo.CharacterHandler.SetUpInfo();
                        zone.ZoneHandler.AddBoss(mrpopo);
                    }
                    break;
                case 46:
                    {
                        var yajiro = new Boss();
                        yajiro.CreateBossNoAttack(88, 300, 408);
                        yajiro.CharacterHandler.SetUpInfo();
                        zone.ZoneHandler.AddBoss(yajiro);
                    }
                    break;
                case 48:
                    {
                        var bubble = new Boss();
                        bubble.CreateBossNoAttack(91, 315, 240);
                        bubble.CharacterHandler.SetUpInfo();
                        zone.ZoneHandler.AddBoss(bubble);
                    }
                    break;
                case 49:
                    {
                        var thuongde = new Boss();
                        thuongde.CreateBossNoAttack(89, 829, 456);
                        thuongde.CharacterHandler.SetUpInfo();
                        zone.ZoneHandler.AddBoss(thuongde);
                    }
                    break;
                case 111:
                    {
                        var tau77 = new Boss();
                        tau77.CreateBossNoAttack(86, 824, 336);
                        tau77.CharacterHandler = new Tau77Handler(tau77);
                        tau77.CharacterHandler.SetUpInfo();
                        zone.ZoneHandler.AddBoss(tau77);
                    }
                    break;
            }
        }
        public Task UpdateBoss(long timeServer)
        {
            try
            {
                for (int i = 0; i < BossOfflines.Count; i++)
                {
                    var boss = BossOfflines[i];
                    if (boss != null)
                    {
                        if (boss.Zone == null)
                        {
                            boss.isBossOfflineWorking = false;
                        }
                        else if (boss.Zone.Characters.IsEmpty && boss.isBossOfflineWorking)
                        {
                            boss.isBossOfflineWorking = false;
                            boss.Zone.ZoneHandler.RemoveBoss(boss);
                        }
                        else
                        {
                            boss.CharacterHandler.Update();
                        }
                    }
                    else
                    {
                        BossOfflines.Remove(boss);
                    }
                }
            }
            catch(Exception e)
            {
                Server.Gi().Logger.Print(e.StackTrace + "\n" + e.Message);
            }
            return Task.CompletedTask;
        }
        public Task HandleBossCloseZone(IZone zone)
        {
            if (zone.Bosses.Count > 0)
            {
                foreach (var boss in zone.Bosses.Values)
                {
                    boss.isBossOfflineWorking = false;
                    zone.ZoneHandler.RemoveBoss(boss);
                }
            }
            return Task.CompletedTask;
        }
        public void StartHandleZone()
        {
            async void Update()
            {
                while (IsRunning)
                {
                    try
                    {
                        var timeServer = ServerUtils.CurrentTimeMillis();
                        //await UpdateBoss(timeServer);
                        var temp = ZoneOfflines.Values.ToList();
                        for (int i = 0; i < temp.Count;i++)
                        {
                            var zoneOffline = temp[i];
                            //if (zoneOffline.Time < timeServer)
                            //{
                            //    if (zoneOffline.Characters.Count > 0)
                            //    {
                            //        zoneOffline.Time = DataCache._1MINUTES * 10 + ServerUtils.CurrentTimeMillis();
                            //        break;
                            //    }
                            //    await HandleBossCloseZone(zone);
                            //    zoneOffline.ZoneHandler.Close();
                            //    ZoneOfflines.Remove(zoneOffline.Id);
                            //    break;
                            //}
                            await zoneOffline.ZoneHandler.Update();
                        }
                        await Task.Delay(1500);

                    }
                    catch (Exception e)
                    {
                        Server.Gi().Logger.Print("Close map offline: " + e.StackTrace + " \n" + e.Message);

                    }
                }
                await HandleZone;
                foreach (IZone zone in ZoneOfflines.Values)
                {
                    zone.ZoneHandler.Close();
                }
                ZoneOfflines.Clear();
                ZoneOfflines = null;
                TileMap = null;
                HandleZone.Dispose();
                HandleZone = null;
                GC.SuppressFinalize(this);
            }
            HandleZone = new Task(Update);
            HandleZone.Start();

        }
        public void Close()
        {
            IsRunning = false;
        }
        public void OutZone(ICharacter character)
        {
            GetZoneById(character.Id).ZoneHandler.OutZone(character);
        }
        public void AddBossWorking(Boss boss)
        {
            boss.isBossOfflineWorking = true;
            BossOfflines.Add(boss);
        }
        public Boss GetBoss(int type)
        {
            foreach (var boss in BossOfflines)
            {
                if (boss.Type == type && !boss.isBossOfflineWorking)
                {
                    boss.InfoDelayBoss.DelayRemove = 300000 + ServerUtils.CurrentTimeMillis();
                    return boss;
                }
            }
            return null;
        }
        public void RemoveBossWorking(Boss boss)
        {
            BossOfflines.Remove(boss);
        }
        public void JoinZone(Character character, int id, bool isDefault = false, bool isTeleport = false, int typeTeleport = 0)
        {
            var zone = GetZoneById(id);
            if (zone.Map.Id is 47)
            {
                switch (character.InfoTask.Id)
                {
                    case <= 11:

                        switch (character.InfoTask.Index)
                        {
                            case 0:
                                {
                                    var tau77 = new Boss();
                                   
                                        tau77 = new Boss();
                                        tau77.CreateBossNoAttack(94, 523, 336);
                                        tau77.CharacterHandler = new Tau77Handler(tau77);
                                        tau77.InfoDelayBoss.ChangeMode = 1000 + ServerUtils.CurrentTimeMillis();
                                        tau77.CharacterHandler.SetUpInfo();
                                        zone.ZoneHandler.AddBoss(tau77);
                                        AddBossWorking(tau77);
                                    
                                   
                                }
                                break;
                            case 1:
                                {

                                    var tau77 = new Boss();
                                    
                                        tau77 = new Boss();
                                        tau77.CreateBossNoAttack(85, 523, 336);
                                        tau77.CharacterHandler = new Tau77Handler(tau77);
                                        tau77.InfoDelayBoss.ChangeMode = 1000 + ServerUtils.CurrentTimeMillis();
                                        tau77.CharacterHandler.SetUpInfo();
                                        zone.ZoneHandler.AddBoss(tau77);
                                        AddBossWorking(tau77);
                                    
                                    //else
                                    //{
                                    //    tau77.InfoChar.TypePk = 0;
                                    //    tau77.InfoDelayBoss.ChangeMode = 1000 + ServerUtils.CurrentTimeMillis();
                                    //    tau77.CharacterHandler.SetUpInfo();
                                    //    zone.ZoneHandler.AddBoss(tau77);
                                    //}
                                }
                                break;
                        }
                        break;
                }
            }
            var mapOld = character.InfoChar.MapId;
            MapManager.Get(mapOld).OutZone(character);
            character.CharacterHandler.SetUpPosition(mapOld, this.Id);
            zone.ZoneHandler.JoinZone(character, isDefault, isTeleport, typeTeleport);
            if (zone.Map.Id is (21 or 22 or 23))
            {
                SetUpZone(zone.Id);
            }
           
        }
        public bool IsMapNotChangeZone()
        {
            return false;
        }
        public void JoinRandomZone(Character character, int id, bool isDefault = false, bool isTeleport = false, int typeTeleport = 0)
        {
            
        }
        #region Ingored
        public IZone GetZoneNotMaxPlayer()
        {
            return null;
        }

        public IZone GetZonePlayer()
        {
            return null;
        }
        public IZone GetZoneNotBoss()
        {
            return null;
        }
        public IZone MobAlive()
        {
            return null;
        }
        public IZone MobDieAll()
        {
            return null;
        }

        public Character GetChar(int id)
        {
            return null;

        }

        public IZone GetZoneNotMaxPlayer(int id)
        {
            return null;
        }
      
        public IZone GetZoneById(int id)
        {
            try
            {
                // Check if the zone with the specified ID already exists in the collection
                if (!ZoneOfflines.TryGetValue(id, out IZone zone))
                {
                    // If the zone doesn't exist, create a new instance of ZoneMapOffline
                    zone = new ZoneMapOffline(id, this);

                    // Add the newly created zone to the collection
                    ZoneOfflines[id] = zone;
                    SetUpZone(id);
                }
                var zoneOffline = (ZoneMapOffline)zone;
                zoneOffline.Time = DataCache._1MINUTES * 10 + ServerUtils.CurrentTimeMillis();
                // Return the retrieved or newly created zone
                return zone;
            }
            catch(Exception ex)
            {
                Server.Gi().Logger.Print(ex.Message + "\n" + ex.StackTrace, "red");
            }
            return null;
        }
        public IZone GetZoneRandomNotHasPlayer()
        {
            return null;
        }
        #endregion
    }
}
