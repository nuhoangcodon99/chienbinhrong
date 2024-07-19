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
    public class MapTreasure : IMap
    {
        public int Id { get; set; }
        public List<IZone> Zones { get; set; }
        public Dictionary<int, IZone> ZoneTreasure { get; set; }
        public List<Boss> BossTreasure { get; set; }
        public TileMap TileMap { get; set; }
        public long TimeMap { get; set; }
        public bool IsRunning { get; set; }
        public bool IsStop { get; set; }
        public Task HandleZone { get; set; }
        public IZone zone { get; set; }
        public MapTreasure(int id)
        {
            Id = id;
            zone = null;
            Zones = null;
            ZoneTreasure = new Dictionary<int, IZone>();
            BossTreasure = new List<Boss>();
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
                if (monster.Id != 71 || monster.Id != 72)
                {
                    monster.OriginalHp = monster.HpMax * 20 * clan.ClanDungeon.BanDoKhoBau.Level;
                }
                else monster.OriginalHp = monster.HpMax * clan.ClanDungeon.BanDoKhoBau.Level;

                monster.MonsterHandler.SetUpMonster();
            }
            switch (Id)
            {
                case 138:
                    var TU_XL = new Boss();
                    TU_XL.CreateBossSetHp(48, level: clan.ClanDungeon.BanDoKhoBau.Level);
                    TU_XL.CharacterHandler.SetUpInfo();
                    zone.ZoneHandler.AddBoss(TU_XL);
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
                Server.Gi().Logger.Print("Close boss map treasure: " + e.StackTrace + " \n" + e.Message);


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
                        character.CharacterHandler.SendMessage(Service.ServerMessage("Doanh trại độc nhãn đã kết thúc, hãy quay lại vào ngày mai"));
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
                        var temp = ZoneTreasure.Values.ToList();

                        for (int i = 0; i < temp.Count; i++)
                        {
                            var zoneTreasure = (ZoneTreasure)temp[i];
                            if (zoneTreasure.Time < t1)
                            {
                                if (zoneTreasure.Characters.Count > 0)
                                {
                                    //handle kick out player
                                    await HandlePlayerCloseZone(zoneTreasure);
                                    continue;
                                }
                                await HandleBossCloseZone(zoneTreasure);
                                zoneTreasure.ZoneHandler.Close();
                                ZoneTreasure.Remove(zoneTreasure.Id);
                                continue;
                            }
                            await zoneTreasure.ZoneHandler.Update();
                        }
                        t2 = ServerUtils.CurrentTimeMillis() - t1;
                        await Task.Delay((int)Math.Abs(1000 - t2));

                    }
                    catch (Exception e)
                    {

                        Server.Gi().Logger.Print("Close map treasure: " + e.StackTrace + " \n" + e.Message);

                    }
                }
                await HandleZone;
                foreach (IZone zone in ZoneTreasure.Values)
                {
                    zone.ZoneHandler.Close();
                }
                ZoneTreasure.Clear();
                ZoneTreasure = null;
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
            BossTreasure.Add(boss);
        }
        public Boss GetBoss(int type)
        {
            foreach (var boss in BossTreasure)
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
            BossTreasure.Remove(boss);
        }
        public void JoinZone(Character character, int id, bool isDefault = false, bool isTeleport = false, int typeTeleport = 0)
        {
            var zone = GetZoneById(id);
            var mapOld = character.InfoChar.MapId;
            if (DataCache.IdMapBDKB.Contains(mapOld) && DataCache.IdMapBDKB.Contains(this.Id))
            {
                var zoneOld = (ZoneTreasure)MapManager.Get(mapOld).GetZoneById(id);
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

                if (!ZoneTreasure.TryGetValue(id, out IZone zone))
                {
                    // If the zone doesn't exist, create a new instance of ZoneMapOffline
                    zone = new ZoneTreasure(id, this);
                    // Add the newly created zone to the collection
                    var zoneTreasure = (ZoneTreasure)zone;
                    zoneTreasure.Time = DataCache._1MINUTES * 30 + ServerUtils.CurrentTimeMillis();
                    ZoneTreasure[id] = zone;
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
