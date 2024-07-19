using NgocRongGold.Application.Constants;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Clan.ClanBoss;
using NgocRongGold.Model.Template;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NgocRongGold.Model.Clan.ClanBoss
{
    public class ClanBoss
    {
        public int Level { get; set; } = 0;
        public int Count { get; set; } = 0;
        public ClanBoss_Status Status = ClanBoss_Status.CLOSE;
        public long Time { get; set; } = 0;
        public Application.Threading.Map Map { get; set; }
        public ClanBoss() 
        {
            Level = 0;
            Count = 0;
            Status = ClanBoss_Status.CLOSE;
            Time = -1;
            Map = null;
        }
        public void Reset()
        {
            Level = 0;
            Count = 0;
            Status = ClanBoss_Status.CLOSE;
            Time = -1;
        }
        public void Join(Character.Character character, int mapOld, bool isCapsule = false)
        {
            var currentMap = MapManager.Get(character.InfoChar.MapId);

            if (isCapsule)
            {
                character.CharacterHandler.SendZoneMessage(
                                            Service.SendTeleport(character.Id, character.InfoChar.Teleport));
                currentMap.OutZone(character);
                character.MapIdOld = currentMap.Id;
                character.SetOldMap();

            }
            else
            {
                currentMap.OutZone(character);
            }
            character.CharacterHandler.SetUpPosition(character.InfoChar.MapId, 165);
            Map.JoinZone(character, 0);
        }
        public void Update(long timeServer)
        {
            try
            {
                switch (Status)
                {
                    case ClanBoss_Status.CLOSE:
                        break;
                    case ClanBoss_Status.OPEN:
                        if (Time < timeServer)
                        {
                            Status = ClanBoss_Status.CLOSE;
                            var characters = Map.Zones[0].ZoneHandler.CharacterInMap();
                            if (characters.Count > 0)
                            {
                                var clan = ClanManager.Get(characters[0].ClanId);
                                for (int i = 0; i < characters.Count; i++)
                                {
                                    var character = characters[i];
                                    if (character != null)
                                    {
                                        Map.OutZone(character);
                                        clan.ClanZone.Map.JoinZone((Character.Character)character, 0);
                                    }
                                }
                            }
                            break;
                        }
                        else if (Time - timeServer <= DataCache._1MINUTES)
                        {
                            var characters = Map.Zones[0].ZoneHandler.CharacterInMap();
                            for (int i = 0; i < characters.Count; i++)
                            {
                                var character = characters[i];
                                if (character != null)
                                {
                                    var sc = (Time - timeServer) / 1000;
                                    character.CharacterHandler.SendMessage(Service.ServerMessage($"về khu vực bang trong {sc} giây nữa"));
                                }
                            }
                        }
                        break;
                    case ClanBoss_Status.END:
                        break;
                }
            }catch(Exception e)
            {
                Server.Gi().Logger.Print(e.Message + "\n" + e.StackTrace);
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
                    member.CharacterHandler.SendMessage(Service.ItemTimeWithMessage($"Hạ Boss Bang Hội [lần thứ {clan.ClanBoss.Level + 1}] thời gian:", (int)TextTime.BOSS_BANG_HOI, (int)(Time / 1000)));
                }
            }
            if (character != null)character.CharacterHandler.SendMessage(Service.ItemTimeWithMessage($"Hạ Boss Bang Hội [lần thứ {clan.ClanBoss.Level + 1}] thời gian:", (int)TextTime.BOSS_BANG_HOI, (int)(Time / 1000)));
        }
        public void Start()
        {
            if (Map == null)Map = new Application.Threading.Map(165, null);
            else if (Map.Zones[0].Bosses.Count > 0)
            {
                foreach(var bossOld in Map.Zones[0].ZoneHandler.BossInMap())
                {
                    bossOld.CharacterHandler.SendDie();
                }
            }
            var boss = new Boss();
            boss.CreateBoss(95 + Level, 505, 384);
            boss.CharacterHandler.SetUpInfo();
            Map.Zones[0].ZoneHandler.AddBoss(boss);
            var timeServer = ServerUtils.CurrentTimeMillis();
            Time = timeServer + DataCache._1MINUTES * 30;
            Count++;
            Status = ClanBoss_Status.OPEN;
        }
    }
}
