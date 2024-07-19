    
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.Zone;
using Figgle;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Extension.DaiHoiVoThuat;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.DatabaseManager.Player;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Map;
using Ubiety.Dns.Core.Records.NotUsed;

namespace NgocRongGold.Application.Extension.DiedRing
{
    public class DiedRing_Handler
    {
        public static DiedRing_Handler instance;
        public static DiedRing_Handler gI()
        {
            if (instance == null) instance = new DiedRing_Handler();
            return instance;
        }
        public Task Runtimee { get; set; }
        public void Start()
        {
            new Thread(new ThreadStart(Runtime)).Start();
        }
        public void Runtime()
        {
            while (Server.Gi().IsRunning)
            {
                try {
                    var timeServer = ServerUtils.CurrentTimeMillis();
                    switch (DiedRing_Cache.diedRing_Status)
                    {
                        case DiedRing_Status.WAIT:
                            {
                                if (DiedRing_Cache.ListCharacter.Count >= 1)
                                {
                                    var character = (Model.Character.Character)ClientManager.Gi().GetCharacter(DiedRing_Cache.ListCharacter[0]);
                                    if (character != null)
                                    {
                                        Fight(character);
                                        DiedRing_Cache.diedRing_Status = DiedRing_Status.FIGHT;
                                        break;
                                    }
                                    DiedRing_Cache.ListCharacter.Remove(DiedRing_Cache.ListCharacter[0]);
                                }
                                break;
                            }
                        case DiedRing_Status.FIGHT:
                            {
                                if (DiedRing_Cache.ListCharacter.Count >= 1)
                                {
                                    var character = (Model.Character.Character)ClientManager.Gi().GetCharacter(DiedRing_Cache.ListCharacter[0]);
                                    if (character != null)
                                    {
                                        Fighting(character, timeServer);
                                        break;
                                    }
                                    DiedRing_Cache.ListCharacter.Remove(DiedRing_Cache.ListCharacter[0]);
                                    DiedRing_Cache.diedRing_Status = DiedRing_Status.WAIT;
                                }
                                else
                                {
                                    DiedRing_Cache.diedRing_Status = DiedRing_Status.WAIT;
                                }

                                break;
                            }
                    }
                    Thread.Sleep(1000);
                }catch(Exception e)
                {
                    Server.Gi().Logger.Print(e.Message + "\n" + e.StackTrace);
                }
                }
        }
        public void Kill(Character character)
        {
            character.DataVoDaiSinhTu.Round++;
            if (character.DataVoDaiSinhTu.Round >= DiedRing_Cache.ListBosses.Count - 1)
            {
                //win
                character.DataVoDaiSinhTu.Win = DiedRing_Character_Win.WIN;
                DiedRing_Service.SendPosistion((Character)character, 467, 408);
                character.CharacterHandler.SendMessage(Service.NpcChat(21, "Chúc m?ng " + character.Name + " ?ã chi?n th?ng"));
                return;
            }
            Fight(character);
        }
        public void OutMapOrDie(Character character)
        {
            if (character == null) return;
            SetTypePK(character, 0);
            DiedRing_Cache.ListCharacter.Remove(character.Id);
            if (character.InfoChar.MapId == 112)
            {
                character.CharacterHandler.LeaveFromDead(true);
                DiedRing_Service.SendPosistion((Character)character, 467, 408);
                character.CharacterHandler.SendMessage(Service.NpcChat(21, character.Name + " y?u ?u?i quá !"));
            }
            RemoveBoss(character.Zone);
            character.DataVoDaiSinhTu.Status = DiedRing_Character_Status.NORMAL;
            //remove boss
        }
        public void RemoveBoss(IZone zone)
        {
            foreach(var boss in zone.Bosses.ToList())
            {
                boss.Value.CharacterHandler.SendDie();
            }
        }
        public Boss SummonBoss(ICharacter character, int type)
        {
            var boss = new Boss();
            boss.CreateBossHasCharFocus(type, character, 497, 336, character.Id);
            boss.CharacterHandler.SetUpInfo();
            character.Zone.ZoneHandler.AddBoss(boss);
            return boss;
        }
        public void ThoiMien(ICharacter character)
        {
            character.InfoSkill.ThoiMien.IsThoiMien = true;
            character.InfoSkill.ThoiMien.Time = DiedRing_Cache.TimeThoiMien + ServerUtils.CurrentTimeMillis();
            character.CharacterHandler.SendMessage(Service.ItemTime(DataCache.TimeThoiMien[0], DiedRing_Cache.TimeThoiMien / 1000));
            character.CharacterHandler.SendMessage(Service.SkillEffectPlayer(character.Id, character.Id, 1, 40));
        }
        public void SetTypePK(ICharacter character, sbyte typePk)
        {
            character.InfoChar.TypePk = typePk;
            character.Zone.ZoneHandler.SendMessage(Service.ChangeTypePk(character.Id, typePk));
        }
        public async void Fight(Character character)
        {
            if (character.DataVoDaiSinhTu.Round >= DiedRing_Cache.ListBosses.Count - 1) return;
            DiedRing_Cache.Delay = 180000 + ServerUtils.CurrentTimeMillis();
            character.DataVoDaiSinhTu.Status = DiedRing_Character_Status.FIGHTING;
            DiedRing_Service.SendPosistion((Character)character, 401, 336);
            var boss = SummonBoss(character, DiedRing_Cache.ListBosses[character.DataVoDaiSinhTu.Round]);
            ThoiMien(character);
            ThoiMien(boss);
            await Task.Delay(5000);
            SetTypePK(character, 3);
            SetTypePK(boss, 3);
            //summon boss
        }
        public void Fighting(Character character, long timeServer)
        {
            if (timeServer > DiedRing_Cache.Delay)
            {
                OutMapOrDie(character);
                DiedRing_Cache.diedRing_Status = DiedRing_Status.WAIT;
            }
        }
    }
}