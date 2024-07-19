
using System.Collections.Concurrent;
using System.Collections.Generic;
using NgocRongGold.Application.Handlers.Character;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.IO;
using NgocRongGold.Model.Info;
using NgocRongGold.Model.Clan;
using NgocRongGold.Application.Threading;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Constants;
using NgocRongGold.Model.Character;
using System;
using System.Linq;
using NgocRongGold.Model.Template;
using Application.Interfaces.Map;

namespace NgocRongGold.Model.Clan
{
    public class KhiGasHuyDiet : PhoBanBase
    {
        public long Time { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int Level { get; set; }
        // public DateTime DateTime { get; set; }
        public PhoBanStatus Status { get; set; } // trang thai pho ban
        public List<Boss> Bosses { get; set; }
        public long Record { get; set; }
        public List<long> LastRecord { get; set; } // max 5
        public long LastTimeRecord { get; set; } // thoi gian lap ki luc
        public IList<IMap> Maps { get; set; }
        public KhiGasHuyDiet()
        {
            Time = -1;
            Name = "Khí Gas Huỷ Diệt";
            Status = PhoBanStatus.CLOSE;
            Bosses = null;
            Record = -1;
            LastRecord = null;
            LastTimeRecord = -1;
            Maps = new List<IMap>();
            Count = 3;
            // DateTime = DateTime.Today;
        }
        public IMap GetMap(int obj)
        {   
            return Maps.FirstOrDefault(map => map.Id == obj);
        }
        public void JoinMap(Character.Character character, int MapNext, bool isCapsule = false)
        {
            var currentMap = GetMap(character.InfoChar.MapId);
            if (currentMap == null)
            {
                currentMap = MapManager.Get(character.InfoChar.MapId);
            }
            else if (isCapsule)
            {
                character.CharacterHandler.SendZoneMessage(
                                            Service.SendTeleport(character.Id, character.InfoChar.Teleport));
                currentMap.OutZone(character);
                character.MapIdOld = currentMap.Id;
                character.SetOldMap();

            }
            else if (currentMap.Zones[0].ZoneHandler.GetCountMob() > 1)
            {

                currentMap.OutZone(character);
                character.CharacterHandler.SetUpPosition(MapNext, character.InfoChar.MapId);
                currentMap.JoinZone(character, 0);
                return;
            }
            else
            {
                currentMap.OutZone(character);

            }
            character.CharacterHandler.SetUpPosition(character.InfoChar.MapId, MapNext);
            GetMap(MapNext).JoinZone(character, 0);

        }
        public void JoinMap(Character.Character character, int MapNext)
        {
            var CurrentMap = GetMap(character.InfoChar.MapId);
            if (CurrentMap == null)
            {
                CurrentMap = MapManager.Get(character.InfoChar.MapId);
            }
            else if (CurrentMap.Zones[0].ZoneHandler.GetCountMob() > 0)//
            {
                CurrentMap.OutZone(character);
                character.CharacterHandler.SetUpPosition(MapNext, CurrentMap.Id);
                CurrentMap.JoinZone(character, 0);
                character.CharacterHandler.SendMessage(Service.OpenUiSay(5, "Phải tiêu diệt hết quái mới được qua"));
                return;
            }
            CurrentMap.OutZone(character);
            character.CharacterHandler.SetUpPosition(CurrentMap.Id, MapNext);
            GetMap(MapNext).JoinZone(character, 0);
        }
      
        public void Reset(bool newDay = false)
        {
            Time = -1;
            Name = "Khí Gas Huỷ Diệt";
            Status = PhoBanStatus.CLOSE;
            Bosses = null;
            Maps.Clear();
            if (newDay)
            {
                Count = 3;
            }
        }
        public void SendTextTime(Clan clan = null, Character.Character character = null)
        {
            if (clan != null)
            {
                foreach (var clanMember in clan.Thành_viên)
                {
                    var member = ClientManager.Gi().GetCharacter(clanMember.Id);
                    if (member == null) continue;
                    member.CharacterHandler.SendMessage(Service.ItemTimeWithMessage(Name, (int)TextTime.KHI_GAS_HUY_DIET, (int)((Time - ServerUtils.CurrentTimeMillis()) / 1000)));
                }
            }
            if (character != null) character.CharacterHandler.SendMessage(Service.ItemTimeWithMessage(Name, (int)TextTime.KHI_GAS_HUY_DIET, (int)((Time - ServerUtils.CurrentTimeMillis()) / 1000)));
        }
        public void Update(long timeserver)
        {
            switch (Status)
            {

                case PhoBanStatus.OPEN:
                case PhoBanStatus.WIN:
                    if (timeserver > Time)
                    {
                        Close();
                    }
                    break;

            }
        }
        public void KickPlayer(Character.Character character)
        {
            GetMap(character.InfoChar.MapId).OutZone(character);
            character.Zone.ZoneHandler.SendMessage(Service.SendTeleport(character.Id, character.TypeTeleport));
            MapManager.JoinMap(character, 27, ServerUtils.RandomNumber(0, 19), true, true, character.TypeTeleport);
            character.CharacterHandler.SendMessage(Service.ServerMessage("Khí Gas Huỷ Diệt đã kết thúc."));

        }
        public void Close()
        {
            if (Status == PhoBanStatus.WIN)
            {
                var timeServer = ServerUtils.CurrentTimeMillis();
                if (Time - timeServer > Record)
                {
                    Record = Time - timeServer;
                }                                                                                           
                if (LastRecord.Count >= 5) LastRecord.RemoveAt(0);
                LastRecord.Add(Record);
                LastTimeRecord = timeServer;
            }
            foreach (var map in Maps)
            {
                map.Close();
            }
            Maps.Clear();
            Reset(false);
        }
        public void Open(Clan clan)
        {
            Count -= 1;
            Time = (DataCache._1MINUTES * 30) + ServerUtils.CurrentTimeMillis();
            Status = PhoBanStatus.OPEN;
            Maps.Add(new Application.Threading.Map(149, tileMap: null       )); // 0 
            Maps.Add(new Application.Threading.Map(147, tileMap: null)); // 1
            Maps.Add(new Application.Threading.Map(152, tileMap: null)); // 2
            Maps.Add(new Application.Threading.Map(151, tileMap: null)); // 3
            Maps.Add(new Application.Threading.Map(148, tileMap: null)); // 4
            InitMob(clan);
            InitBoss(clan);
            SendTextTime(clan, null);
        }
        public bool CheckOpen()
        {
            return Status is (PhoBanStatus.OPEN or PhoBanStatus.WIN or PhoBanStatus.SPECIAL);
        }
        public bool CheckClose()
        {
            return Status is PhoBanStatus.CLOSE;
        }
        public void InitMob(Clan clan)
        {
            for (int map = 0; map < Maps.Count; map++)
            {
                var zone = Maps[map].Zones[0];
                for (int m = 0; m < zone.MonsterMaps.Count; m++)
                {
                    var monster = zone.MonsterMaps[m];
                    monster.OriginalHp += monster.OriginalHp * 6 * Level;
                    monster.MonsterHandler.SetUpMonster();
                }
            }
        }
        public void InitBoss(Clan clan)
        {
            var boss = new Boss();
            boss.CreateBossSetHp(23, level: Level);
            boss.InfoChar.TypePk = 0;
            boss.CharacterHandler.SetUpInfo();
            Maps[4].Zones[0].ZoneHandler.AddBoss(boss);

            boss = new Boss();
            boss.CreateBossSetHp(67, level: Level);
            boss.InfoChar.TypePk = 0;
            boss.CharacterHandler.SetUpInfo();
            Maps[4].Zones[0].ZoneHandler.AddBoss(boss);
        }

    }
    }