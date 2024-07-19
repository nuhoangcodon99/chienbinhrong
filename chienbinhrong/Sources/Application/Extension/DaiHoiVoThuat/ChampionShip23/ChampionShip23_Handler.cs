using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Zone;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.DatabaseManager.Player;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Map;
using Org.BouncyCastle.Math.Field;

namespace NgocRongGold.Application.Extension.DaiHoiVoThuat
{
    public class ChampionShip23_Handler
    {
        //public static ChampionShip23_Handler instance;
        //public static ChampionShip23_Handler gI()
        //{
        //    if (instance == null) instance = new ChampionShip23_Handler();
        //    return instance;
        //}

        //public Task Runtimee { get; set; }
        //public void Start()
        //{
        //    Runtimee = new Task(Runtime);
        //    Runtimee.Start();
        //}
        //public async void Runtime()
        //{
        //    while (Server.Gi().IsRunning)
        //    {
        //        if (ChampionShip23Cache.ListPlayer.Count >= 1)
        //        {
        //            var timeServer = ServerUtils.CurrentTimeMillis();
        //            for (int i = 0; i < ChampionShip23Cache.ListPlayer.Count; i++)
        //            {
        //                var Id = ChampionShip23Cache.ListPlayer[i];
        //                var character = ClientManager.Gi().GetCharacter(Id);
        //                if (character == null)
        //                {
        //                    ChampionShip23Cache.ListPlayer.Remove(Id);
        //                    break;
        //                }

        //                switch (character.DataDaiHoiVoThuat23.Status)
        //                {
        //                    case ChampionerCS23_Status.NORMAL:
        //                        if (character.Zone.Characters.Count > 1)
        //                        {
        //                            async void Action()
        //                            {
        //                                var zoneJoin = MapManager.Get(character.InfoChar.MapId).GetZoneNotMaxPlayer();
        //                                zoneJoin.ZoneHandler.JoinZone((Character)character, false, false, 0);
        //                                await Task.Delay(200);
        //                                Fight(character);
        //                            }
        //                            var task = new Task(Action);
        //                            task.Start();
        //                        }
        //                        else
        //                        {
        //                            Fight(character);
        //                        }
        //                        break;
        //                    case ChampionerCS23_Status.FIGHITING:

        //                        if (CheckPosistion(character))
        //                        {
        //                            Failed(character, "Bạn đã thất bại vì vi phạm quy chế thi đấu, chúc bạn may mắn lần sau");
        //                        }
        //                        else if (timeServer > character.DataDaiHoiVoThuat23.Delay)
        //                        {
        //                            Failed(character, "Bạn đã thất bại vì hết thời gian thi đấu, chúc bạn may mắn lần sau");
        //                        }
        //                        break;
        //                    case ChampionerCS23_Status.END:
        //                        ChampionShip23Cache.ListPlayer.Remove(character.Id);
        //                        break;
        //                }
        //            }
        //        }
        //        await Task.Delay(1000);
        //    }
        //}
        public void Update(Character character, long timeServer)
        {
            switch (character.DataDaiHoiVoThuat23.Status)
            {
                case ChampionerCS23_Status.NORMAL:
                    break;
                case ChampionerCS23_Status.REGISTER:
                    if (character.Zone.Characters.Count > 1)
                    {
                        var zoneJoin = MapManager.Get(character.InfoChar.MapId).GetZoneNotMaxPlayer();
                        zoneJoin.ZoneHandler.JoinZone((Character)character, false, false, 0);
                        Fight(character);
                    }
                    else
                    {
                        Fight(character);
                    }
                    break;
                case ChampionerCS23_Status.FIGHITING:

                    if (CheckPosistion(character))
                    {
                        Failed(character, "Bạn đã thất bại vì vi phạm quy chế thi đấu, chúc bạn may mắn lần sau");
                    }
                    else if (timeServer > character.DataDaiHoiVoThuat23.Delay)
                    {
                        Failed(character, "Bạn đã thất bại vì hết thời gian thi đấu, chúc bạn may mắn lần sau");
                    }
                    break;
                case ChampionerCS23_Status.END:
                    break;
            }
        }
            public Boss GetBoss(Character character,ChampionShip23_Boss Boss)
        {
            var boss = new Boss();
            boss.CreateBossHasCharFocus(ChampionShip23_Cache.ListBosses[(int)Boss], character, 497, 264, character.Id);
            boss.CharacterHandler.SetUpInfo();
            return boss;
        }
        public void Register(Character character)
        {
            if (character.DataDaiHoiVoThuat23.Status == ChampionerCS23_Status.NORMAL)
            {
                character.DataDaiHoiVoThuat23.Status = ChampionerCS23_Status.REGISTER;
            }

        }
        public ChampionShip23_Boss GetBoss(int type)
        {
            switch (type)
            {
                case 52:
                    return ChampionShip23_Boss.SOI_HEC_QUYN;
                case 53:
                    return ChampionShip23_Boss.O_DO;
                case 54:
                    return ChampionShip23_Boss.XIN_BA_TO;
                case 55:
                    return ChampionShip23_Boss.CHA_PA;
                case 56:
                    return ChampionShip23_Boss.PON_PUI;
                case 57:
                    return ChampionShip23_Boss.CHAN_XU;
                case 58:
                    return ChampionShip23_Boss.TAU_PAY_PAY;
                case 59:
                    return ChampionShip23_Boss.YAM_CHA;
                case 60:
                    return ChampionShip23_Boss.JACKY_CHUN;
                case 61:
                    return ChampionShip23_Boss.THIEN_XIN_HANG;
                case 62:
                    return ChampionShip23_Boss.LIU_LIU;
                case 112:
                    return ChampionShip23_Boss.PICOLO;
                default:
                    return ChampionShip23_Boss.SOI_HEC_QUYN;
            }
            
        }
        public void SetTypePk(ICharacter character, sbyte typePk)
        {
            character.InfoChar.TypePk = typePk;
            character.CharacterHandler.SendZoneMessage(Service.ChangeTypePk(character.Id, typePk));
        }
        public void ThoiMien(ICharacter character, long TimeThoiMien = 0)
        {
            character.InfoSkill.ThoiMien.IsThoiMien = true;
            character.InfoSkill.ThoiMien.Time = TimeThoiMien + ServerUtils.CurrentTimeMillis();
            character.CharacterHandler.SendMessage(Service.ItemTime(DataCache.TimeThoiMien[0], (int)TimeThoiMien / 1000));
            character.CharacterHandler.SendMessage(Service.SkillEffectPlayer(character.Id, character.Id, 1, 40));
        }
        public bool CheckPosistion(Character character)
        {
            return ((character.InfoChar.X >= 0 && character.InfoChar.X <= 157) || (character.InfoChar.X >= 611 && character.InfoChar.X <= 733) || character.InfoChar.Y > 264);
        }
        public bool CheckStatusPk(Character character)
        {
            return character.DataDaiHoiVoThuat23.Status is (ChampionerCS23_Status.FIGHITING or ChampionerCS23_Status.REGISTER);
        }
        public async void Fight(Character character, bool win = false)
        {
            var round = character.DataDaiHoiVoThuat23.Round;
            if (win)
            {
                round++;
                character.DataDaiHoiVoThuat23.WoodChestLevel++;
            }
            if (round >= ChampionShip23_Cache.ListBosses.Count)
            {
                //win
                SendPosistion(character, 373, 360);
                character.DataDaiHoiVoThuat23.Status = ChampionerCS23_Status.NORMAL;
                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã thắng đại hội võ thuật lần thứ 23"));
                return;
            }
            var listBoss = ChampionShip23_Cache.ListBosses[round];
            character.DataDaiHoiVoThuat23.Round = round;
            character.DataDaiHoiVoThuat23.Delay = 180000 + ServerUtils.CurrentTimeMillis();
            character.DataDaiHoiVoThuat23.Zone = character.Zone.Id;
            character.DataDaiHoiVoThuat23.Status = ChampionerCS23_Status.FIGHITING;
            ReleaseInfo(character);
            ReleaseSkill(character);
            SetTypePk(character, 0);
            ThoiMien(character, ChampionShip23_Cache.TimeThoiMien);
            SendPosistion(character, 334, 264);
            var bossSummon = SummonBoss(character, GetBoss(listBoss));
            if (bossSummon != null)
            {
                ThoiMien(bossSummon, ChampionShip23_Cache.TimeThoiMien);
                SetTypePk(bossSummon, 0);
            }
            await Task.Delay(10000);
            SetTypePk(character, 3);
            SetTypePk(bossSummon, 3);
            SendPosistion(character, 334, 264);
            }
        public void WinRound(Character character)
        {
            

        }
        public void Reset(Character character, bool newDay = false)
        {
            if (newDay)
            {
                character.DataDaiHoiVoThuat23.WoodChestCollect = false;
                character.DataDaiHoiVoThuat23.WoodChestLevel = 0;
                character.DataDaiHoiVoThuat23.Round = 0;
                character.DataDaiHoiVoThuat23.Count = 0;
            }
            character.DataDaiHoiVoThuat23.Status = ChampionerCS23_Status.NORMAL;
        }
        public void Failed(Character character, string reason ="")
        {
            
            character.CharacterHandler.LeaveFromDead(true);
            SetTypePk(character, 0);
            SendPosistion(character, 389, 360);
            character.CharacterHandler.SendMessage(Service.ServerMessage(reason));
            if (character.Zone.Bosses.Count >= 1)DeleteBoss(character.Zone, GetBoss(ChampionShip23_Cache.ListBosses[character.DataDaiHoiVoThuat23.Round]));
            Reset(character, false);
            character.DataDaiHoiVoThuat23.Status = ChampionerCS23_Status.END;

            // ChampionShip23Cache.ListPlayer.Remove(character.Id);
        }
        public void Outmap(Character character, string reason = "")
        {
            if (character.InfoChar.MapId == 129)
            {
                SendPosistion(character, 389, 360);
            }
            ChampionShip23_Cache.ListPlayer.Remove(character.Id);
            Reset(character, false);
            SetTypePk(character, 0);
            character.CharacterHandler.SendMessage(Service.ServerMessage(reason));
            DeleteBoss(MapManager.Get(129).Zones[character.DataDaiHoiVoThuat23.Zone], ChampionShip23_Cache.ListBosses[character.DataDaiHoiVoThuat23.Round]);
        }
        public void ReleaseSkill(Character character)
        {
            character.Skills.ForEach(skill =>
            {
                skill.CoolDown = 1000 + ServerUtils.CurrentTimeMillis();
            });
            character.CharacterHandler.SendMessage(Service.UpdateCooldown(character));
        }
        public void ReleaseInfo(Character character)
        {
            character.CharacterHandler.PlusHp((int)character.HpFull);
            character.CharacterHandler.PlusMp((int)character.MpFull);
            character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
        }
        public void SendPosistion(Character character, short toX, short toY)
        {
            character.InfoChar.X = toX;
            character.InfoChar.Y = toY;
            character.Zone.ZoneHandler.SendMessage(Service.SendPos(character, 0));
        }
        public Boss SummonBoss(Character character, ChampionShip23_Boss Boss)
        {
            var boss = GetBoss(character,Boss);

            character.Zone.ZoneHandler.AddBoss(boss);
            return boss;
        }
        public void DeleteBoss(IZone zone, ChampionShip23_Boss Boss)
        {
            if (zone.ZoneHandler.GetBossInMap(ChampionShip23_Cache.ListBosses[(int)Boss]).Count < 1) return;
            var boss = zone.ZoneHandler.GetBossInMap(ChampionShip23_Cache.ListBosses[(int)Boss])[0];
            if (boss == null) return;
            boss.CharacterHandler.SendDie();
        }
        public void DeleteBoss(IZone zone,int type)
        {
            var boss = zone.ZoneHandler.GetBossInMap(type)[0];
            if (boss == null) return;
            boss.CharacterHandler.SendDie();
        }
    }

}