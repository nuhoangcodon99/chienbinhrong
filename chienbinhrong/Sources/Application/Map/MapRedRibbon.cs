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
    public class MapRedRibbon : IMap
    {
        public int Id { get; set; }
        public List<IZone> Zones { get; set; }
        public Dictionary<int, IZone> ZoneRedRibbons { get; set; }
        public List<Boss> BossRedRibbons { get; set; }
        public TileMap TileMap { get; set; }
        public long TimeMap { get; set; }
        public bool IsRunning { get; set; }
        public bool IsStop { get; set; }
        public Task HandleZone { get; set; }
        public IZone zone { get; set; }
        public MapRedRibbon(int id)
        {
            Id = id;
            zone = null;
            Zones = null;
            ZoneRedRibbons = new Dictionary<int, IZone>();
            BossRedRibbons = new List<Boss>();
            TimeMap = -1;
            IsRunning = true;
            IsStop = false;
            TileMap = Cache.Gi().TILE_MAPS.FirstOrDefault(t => t.Id == Id);
            StartHandleZone();
        }
        public void SetUpZone(int id)
        {
            long PercentHp = ClanManager.Get(id).ClanDungeon.DoanhTraiDocNhan.PercentHp;
            if (PercentHp / 50000 < 1) PercentHp = 50000;
            var zone = GetZoneById(id);
            for (int m = 0; m < zone.MonsterMaps.Count; m++)
            {
                var monster = zone.MonsterMaps[m];
                monster.OriginalHp += monster.OriginalHp * 6 * (PercentHp / 50000);
                monster.MonsterHandler.SetUpMonster();
            }
            var hpBoss = PercentHp / 50000;
            switch (Id)
            {
                case 59:
                    Boss _TUT = new Boss();
                    _TUT.CreateBossDoanhTrai(47, 923, 384, (int)hpBoss);
                    _TUT.CharacterHandler.SetUpInfo();
                    zone.ZoneHandler.AddBoss(_TUT);
                    break;
                ///
                case 62:
                    Boss _TUXL = new Boss();
                    _TUXL.CreateBossDoanhTrai(48, 1088, 384, (int)hpBoss);
                    _TUXL.CharacterHandler.SetUpInfo();
                    zone.ZoneHandler.AddBoss(_TUXL);
                    break;
                ///
                case 55:
                    Boss _TUTHEP = new Boss();
                    _TUTHEP.CreateBossDoanhTrai(49, 830, 312, (int)hpBoss);
                    _TUTHEP.CharacterHandler.SetUpInfo();
                    zone.ZoneHandler.AddBoss(_TUTHEP);
                    break;
                ///
                case 54:
                    Boss _NJAOTIM = new Boss();
                    _NJAOTIM.CreateBossDoanhTrai(50, 994, 312, (int)hpBoss);
                    _NJAOTIM.CharacterHandler.SetUpInfo();
                    zone.ZoneHandler.AddBoss(_NJAOTIM);
                    break;
                //// --- VE SI 1
                case 57:
                    Boss _VESI = new Boss();
                    _VESI.CreateBossDoanhTrai(51, 1443, 312, (int)hpBoss);
                    _VESI.CharacterHandler.SetUpInfo();
                    zone.ZoneHandler.AddBoss(_VESI);
                    /// --- VE SI 2
                    Boss _VESI2 = new Boss();
                    _VESI2.CreateBossDoanhTrai(51, 1493, 312, (int)hpBoss);
                    _VESI2.CharacterHandler.SetUpInfo();
                    zone.ZoneHandler.AddBoss(_VESI2);
                    /// --- VE SI 3
                    Boss _VESI3 = new Boss();
                    _VESI3.CreateBossDoanhTrai(51, 1393, 312, (int)hpBoss);
                    _VESI3.CharacterHandler.SetUpInfo();
                    zone.ZoneHandler.AddBoss(_VESI3);
                    /// --- VE SI 4
                    Boss _VESI4 = new Boss();
                    _VESI4.CreateBossDoanhTrai(51, 1343, 312, (int)hpBoss);
                    _VESI4.CharacterHandler.SetUpInfo();
                    zone.ZoneHandler.AddBoss(_VESI4);
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
                Server.Gi().Logger.Print("Close boss red ribioon: " + e.StackTrace + " \n" + e.Message);


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
            catch(Exception e)
            {
                Server.Gi().Logger.Print("Close player red ribioon: " + e.StackTrace + " \n" + e.Message);


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
                        var temp = ZoneRedRibbons.Values.ToList();

                        for (int i = 0; i < temp.Count;i++)
                        {
                            var ZoneRedRibbon = (ZoneRedRibbon)temp[i];
                            if (ZoneRedRibbon.Time < t1)
                            {
                                if (ZoneRedRibbon.Characters.Count > 0)
                                {
                                    //handle kick out player
                                    await HandlePlayerCloseZone(ZoneRedRibbon);
                                    continue;
                                }
                                await HandleBossCloseZone(ZoneRedRibbon);
                                ZoneRedRibbon.ZoneHandler.Close();
                                ZoneRedRibbons.Remove(ZoneRedRibbon.Id);
                                continue;
                            }
                            await ZoneRedRibbon.ZoneHandler.Update();
                        }
                        t2 = ServerUtils.CurrentTimeMillis() - t1;
                        await Task.Delay((int)Math.Abs(1000 - t2));

                    }
                    catch (Exception e)
                    {
                       
                        Server.Gi().Logger.Print("Close map red ribioon: " + e.StackTrace + " \n" + e.Message);

                    }
                }
                await HandleZone;
                foreach (IZone zone in ZoneRedRibbons.Values)
                {
                    zone.ZoneHandler.Close();
                }
                ZoneRedRibbons.Clear();
                ZoneRedRibbons = null;
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
            BossRedRibbons.Add(boss);
        }
        public Boss GetBoss(int type)
        {
            foreach (var boss in BossRedRibbons)
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
            BossRedRibbons.Remove(boss);
        }
        public void JoinZone(Character character, int id, bool isDefault = false, bool isTeleport = false, int typeTeleport = 0)
        {
            var zone = GetZoneById(id);
            var mapOld = character.InfoChar.MapId;
            if (DataCache.IdMapReddot.Contains(mapOld) && DataCache.IdMapReddot.Contains(this.Id))
            {
                var zoneOld = (ZoneRedRibbon)MapManager.Get(mapOld).GetZoneById(id);
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
                
                if (!ZoneRedRibbons.TryGetValue(id, out IZone zone))
                {
                    // If the zone doesn't exist, create a new instance of ZoneMapOffline
                    zone = new ZoneRedRibbon(id, this);
                    // Add the newly created zone to the collection
                    var zoneRedRibbon = (ZoneRedRibbon)zone;
                    zoneRedRibbon.Time = DataCache._1MINUTES * 30 + ServerUtils.CurrentTimeMillis();
                    ZoneRedRibbons[id] = zone;
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
