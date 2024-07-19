using Application.Interfaces.Map;
using Application.Interfaces.Zone;
using Chiến_Binh_Rồng.Sources.Application.Manager;
using Model.Zone;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Extension.Bosses.Tau77;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.DatabaseManager;
using NgocRongGold.DatabaseManager.Player;
using NgocRongGold.Model;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Clan;
using NgocRongGold.Model.Item;
using NgocRongGold.Model.Map;
using Org.BouncyCastle.Math.Field;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class MapSnakeRoad : IMap
    {
        public int Id { get; set; }
        public List<IZone> Zones { get; set; }
        public Dictionary<int, IZone> ZoneSnakeRoad { get; set; }
        public List<Boss> BossSnakeRoad { get; set; }
        public TileMap TileMap { get; set; }
        public long TimeMap { get; set; }
        public bool IsRunning { get; set; }
        public bool IsStop { get; set; }
        public Task HandleZone { get; set; }
        public IZone zone { get; set; }
        public MapSnakeRoad(int id)
        {
            Id = id;
            zone = null;
            Zones = null;
            ZoneSnakeRoad = new Dictionary<int, IZone>();
            BossSnakeRoad = new List<Boss>();
            TimeMap = -1;
            IsRunning = true;
            IsStop = false;
            TileMap = Cache.Gi().TILE_MAPS.FirstOrDefault(t => t.Id == Id);
            StartHandleZone();
        }
        public void SetUpZone(int id)
        {
            var clan = ClanManager.Get(id);
            var zone = GetZoneById(id);
            for (int mob = 0; mob < zone.MonsterMaps.Count; mob++)
            {
                var monster = zone.MonsterMaps[mob];
                monster.OriginalHp = monster.HpMax * 20 * clan.ClanDungeon.ConDuongRanDoc.Level;
                monster.MonsterHandler.SetUpMonster();

                monster.MonsterHandler.SetUpMonster();
            }
            switch (Id)
            {
                case 144:
                    for (int i = 0; i < 5; i++)
                    {
                        //
                        var saibamen = new Boss();
                        if (i == 0) saibamen.CreateBossSetHp(73, level: clan.ClanDungeon.ConDuongRanDoc.Level, typePk: 5);
                        else saibamen.CreateBossSetHp(73 + i, level: clan.ClanDungeon.ConDuongRanDoc.Level, typePk: 0);
                        saibamen.CharacterHandler.SetUpInfo();
                        zone.ZoneHandler.AddBoss(saibamen);
                        BossSnakeRoad.Add(saibamen);
                    }
                    //
                    var nappa = new Boss();
                    nappa.CreateBossSetHp(78, level: clan.ClanDungeon.ConDuongRanDoc.Level, typePk: 0);
                    nappa.CharacterHandler.SetUpInfo();
                    zone.ZoneHandler.AddBoss(nappa);
                    BossSnakeRoad.Add(nappa);
                    //
                    var cadic = new Boss();
                    cadic.CreateBossSetHp(79, level: clan.ClanDungeon.ConDuongRanDoc.Level, typePk: 0);
                    cadic.CharacterHandler.SetUpInfo();
                    zone.ZoneHandler.AddBoss(cadic);
                    BossSnakeRoad.Add(cadic);
                    break;
            }
        }
        public Task UpdateBoss(long timeServer)
        {

            return Task.CompletedTask;
        }
        public Task HandleBossCloseZone(IZone zone)
        {
            try
            {
                if (zone.Bosses.Count > 0)
                {
                    foreach (var boss in zone.Bosses.Values)
                    {
                        boss.CharacterHandler.SendDie();
                    }
                    Task.Delay(5000);
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Print("Close boss map cdrd: " + e.StackTrace + " \n" + e.Message);


            }
            return Task.CompletedTask;
        }
        public Task HandlePlayerCloseZone(IZone zone)
        {
            try
            {
                if (zone.Characters.Count > 0)
                {
                    var temp = zone.Characters.Values.ToList();
                    for (int i = 0; i < temp.Count; i++)
                    {
                        var character = temp[i];
                        if (character == null) continue;
                        character.CharacterHandler.BackHome();
                        character.CharacterHandler.SendMessage(Service.ServerMessage("Con đường rắn độc đã kết thúc, hãy quay lại vào ngày mai"));
                    }
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Print("Close player map treasure: " + e.StackTrace + " \n" + e.Message);


            }
            return Task.CompletedTask;
        }
        public void StartHandleZone()
        {
            async void Update()
            {
                long t1, t2;
                while (IsRunning)
                {
                    try
                    {
                        t1 = ServerUtils.CurrentTimeMillis();
                        //await UpdateBoss(timeServer);
                        var temp = ZoneSnakeRoad.Values.ToList();

                        for (int i = 0; i < temp.Count; i++)
                        {
                            var zoneSnakeRoad = (ZoneSnakeRoad)temp[i];
                            if (zoneSnakeRoad.Time < t1)
                            {
                                if (zoneSnakeRoad.Characters.Count > 0)
                                {
                                    //handle kick out player
                                    await HandlePlayerCloseZone(zoneSnakeRoad);
                                    continue;
                                }
                                await HandleBossCloseZone(zoneSnakeRoad);
                                zoneSnakeRoad.ZoneHandler.Close();
                                ZoneSnakeRoad.Remove(zoneSnakeRoad.Id);
                                continue;
                            }
                            await zoneSnakeRoad.ZoneHandler.Update();
                        }
                        t2 = ServerUtils.CurrentTimeMillis() - t1;
                        await Task.Delay((int)Math.Abs(1000 - t2));

                    }
                    catch (Exception e)
                    {

                        Server.Gi().Logger.Print("Close map cdrd: " + e.StackTrace + " \n" + e.Message);

                    }
                }
                await HandleZone;
                foreach (IZone zone in ZoneSnakeRoad.Values)
                {
                    zone.ZoneHandler.Close();
                }
                ZoneSnakeRoad.Clear();
                ZoneSnakeRoad = null;
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
            GetZoneById(character.ClanId).ZoneHandler.OutZone(character);
        }
        public void AddBossWorking(Boss boss)
        {
            boss.isBossOfflineWorking = true;
            BossSnakeRoad.Add(boss);
        }
        public Boss GetBoss(int type)
        {
            foreach (var boss in BossSnakeRoad)
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
            BossSnakeRoad.Remove(boss);
        }
        public void JoinZone(Character character, int id, bool isDefault = false, bool isTeleport = false, int typeTeleport = 0)
        {
            var zone = GetZoneById(id);
            var mapOld = character.InfoChar.MapId;
            if (DataCache.IdMapCDRD.Contains(mapOld) && DataCache.IdMapCDRD.Contains(this.Id))
            {
                var zoneOld = (ZoneSnakeRoad)MapManager.Get(mapOld).GetZoneById(id);
                var countMob = zoneOld.MonsterMaps.Where(mob => mob.IsDie is false).Count();
                if (countMob > 0)
                {
                    MapManager.Get(mapOld).OutZone(character);
                    character.CharacterHandler.SetUpPosition(this.Id, mapOld);
                    zoneOld.ZoneHandler.JoinZone(character, isDefault, isTeleport, typeTeleport);
                    character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn phải đánh hết quái mới được đi qua khu vực này"));
                    return;
                }
            }
            MapManager.Get(mapOld).OutZone(character);
            character.CharacterHandler.SetUpPosition(mapOld, this.Id);
            zone.ZoneHandler.JoinZone(character, isDefault, isTeleport, typeTeleport);


        }
        public bool IsMapNotChangeZone()
        {
            return false;
        }
        public void JoinRandomZone(Character character, int id, bool isDefault = false, bool isTeleport = false, int typeTeleport = 0)
        {

        }
        public int StatementZone(Character character)
        {
            var zoneSelect = 0;
            for (int zone = 0; zone < Zones.Count; zone++)
            {
                if (zone == Zones.Count - 1) zoneSelect = 0;
                else if (Zones[zone + 1].Characters.Count < Zones[zone].Characters.Count) zoneSelect = zone++;

            }
            return zoneSelect;
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

                if (!ZoneSnakeRoad.TryGetValue(id, out IZone zone))
                {
                    // If the zone doesn't exist, create a new instance of ZoneMapOffline
                    zone = new ZoneSnakeRoad(id, this);
                    // Add the newly created zone to the collection
                    var zoneSnakeRoad = (ZoneSnakeRoad)zone;
                    zoneSnakeRoad.Time = DataCache._1MINUTES * 30 + ServerUtils.CurrentTimeMillis();
                    ZoneSnakeRoad[id] = zone;
                    SetUpZone(id);
                }
                // Return the retrieved or newly created zone
                return zone;
            }
            catch (Exception ex)
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
