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
    public class MapGodzillaAndKong : IMap
    {
        public int Id { get; set; }
        public List<IZone> Zones { get; set; }
        public Dictionary<int, IZone> ZonesGodzillaAndKong { get; set; }
        public List<Boss> BossGodzillaAndKong { get; set; }
        public TileMap TileMap { get; set; }
        public long TimeMap { get; set; }
        public bool IsRunning { get; set; }
        public bool IsStop { get; set; }
        public Task HandleZone { get; set; }
        public IZone zone { get; set; }
        public MapGodzillaAndKong(int id)
        {
            Id = id;
            zone = null;
            Zones = null;
            ZonesGodzillaAndKong = new Dictionary<int, IZone>();
            BossGodzillaAndKong = new List<Boss>();
            TimeMap = -1;
            IsRunning = true;
            IsStop = false;
            TileMap = Cache.Gi().TILE_MAPS.FirstOrDefault(t => t.Id == Id);
            StartHandleZone();
        }
        public void SetUpZone(int id)
        {
            
        }
        public Task UpdateBoss(long timeServer)
        {

            return Task.CompletedTask;
        }
        public Task HandleBossCloseZone(IZone zone)
        {
            return Task.CompletedTask;
        }
        public Task HandlePlayerCloseZone(IZone zone)
        {
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
                        var temp = ZonesGodzillaAndKong.Values.ToList();

                        for (int i = 0; i < temp.Count; i++)
                        {
                            var zoneGodzillaAndKong = (ZoneGodzillaAndKong)temp[i];
                            if (zoneGodzillaAndKong.Time < t1)
                            {
                                if (zoneGodzillaAndKong.Characters.Count > 0)
                                {
                                    //handle kick out player
                                    await HandlePlayerCloseZone(zoneGodzillaAndKong);
                                    continue;
                                }
                                zoneGodzillaAndKong.ZoneHandler.Close();
                                ZonesGodzillaAndKong.Remove(zoneGodzillaAndKong.Id);
                                continue;
                            }
                            await zoneGodzillaAndKong.ZoneHandler.Update();
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
                foreach (IZone zone in ZonesGodzillaAndKong.Values)
                {
                    zone.ZoneHandler.Close();
                }
                ZonesGodzillaAndKong.Clear();
                ZonesGodzillaAndKong = null;
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
        public void JoinZone(Character character, int id, bool isDefault = false, bool isTeleport = false, int typeTeleport = 0)
        {
            var zone = GetZoneById(id);
            var mapOld = character.InfoChar.MapId;
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

                if (!ZonesGodzillaAndKong.TryGetValue(id, out IZone zone))
                {
                    // If the zone doesn't exist, create a new instance of ZoneMapOffline
                    zone = new ZoneGodzillaAndKong(id, this);
                    // Add the newly created zone to the collection
                    var zoneGodzillaAndKong = (ZoneGodzillaAndKong)zone;
                    zoneGodzillaAndKong.Time = DataCache._1MINUTES * 30 + ServerUtils.CurrentTimeMillis();
                    ZonesGodzillaAndKong[id] = zone;
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
