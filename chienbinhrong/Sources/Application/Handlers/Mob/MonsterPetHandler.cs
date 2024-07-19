using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Extension.Bosses.Mabu12Gio;
using NgocRongGold.Application.Extension.DiedRing;
using NgocRongGold.Application.Extension.Practice.Whis;
using NgocRongGold.Application.Handlers.Skill;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.Interfaces.Monster;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Item;

namespace NgocRongGold.Application.Handlers.Monster
{
    public class MonsterPetHandler : IMonsterHandler
    {
        public IMonster Monster { get; set; }
        public void SetUpMonster(bool isDie = false)
        {
             //ignore
        }
        public Message Hidru(byte type)
        {
            //return;
            return null;
        }
        public void RemoveEffect(long timeServer, bool globalReset = false)
        {
        }
        public void Recovery()
        {
            //Ignore
        }

        public int UpdateHp(long damage, int charId, bool isMaxHp = false)
        {
            if (Monster.Character.InfoSet.IsFullSetPikkoro)
            {
                return 0;
            }

            if(damage >= Monster.Hp)
            {
                damage = Monster.Hp;
            }
            Monster.Hp -= damage;
            if(Monster.Hp <= 0) StartDie();
            return (int)damage;
        }

        private void StartDie()
        {
            Monster.Zone.ZoneHandler.SendMessage(Service.UpdateMonsterMe6(Monster.IdMap));
            Monster.Hp = 0;
            Monster.IsDie = true;
            Monster.Status = 0;
            Monster.Character?.CharacterHandler.RemoveMonsterMe();
        }

        public void LeaveItem(ICharacter character)
        {
            //ignore
            Server.Gi().Logger.Debug($"Monster Pet Handler Leave Item");
        }

        public int PetAttackMonster(IMonster monster)
        {
            long damage = ServerUtils.RandomNumber(Monster.Damage * 9 / 10, Monster.Damage);
            var damageMonsterAfterHandle = 0;

            if (Monster.Character.InfoSet.IsFullSetPikkoro)
            {
                damage*=3;
            }

            if (ServerUtils.RandomNumber(100) <= 25)
            {
                damageMonsterAfterHandle = 0;
            }
            else
            {
                damageMonsterAfterHandle = monster.MonsterHandler.UpdateHp(damage, Monster.IdMap, true);
            }
            // Monster.Zone.ZoneHandler.SendMessage(Service.UpdateMonsterMe1(Monster.IdMap, monster.IdMap));
            // Hút máu, ki
            if (damageMonsterAfterHandle > 0)
            {
                var hpPlus = damageMonsterAfterHandle * Monster.Character.HpPlusFromDamage / 100;
                var mpPlus = damageMonsterAfterHandle * Monster.Character.MpPlusFromDamage / 100;
                var hpPlusMonster = damageMonsterAfterHandle * Monster.Character.HpPlusFromDamageMonster / 100;

                hpPlus += hpPlusMonster > 0 ? hpPlusMonster : 0;
                
                if(hpPlus > 0) {
                    Monster.Character.CharacterHandler.PlusHp(hpPlus);
                    if (Monster.Character.Id > 0)
                    {
                        Monster.Character.CharacterHandler.SendMessage(Service.SendHp((int)Monster.Character.InfoChar.Hp));
                    }
                    Monster.Character.CharacterHandler.SendZoneMessage(Service.PlayerLevel(Monster.Character));
                }

                if(mpPlus > 0) {
                    Monster.Character.CharacterHandler.PlusMp(mpPlus);
                    if (Monster.Character.Id > 0)
                    {
                        Monster.Character.CharacterHandler.SendMessage(Service.SendMp((int)Monster.Character.InfoChar.Mp));
                    }
                }
            }

            Monster.Zone.ZoneHandler.SendMessage(Service.UpdateMonsterMe3(Monster.IdMap, monster.IdMap, (int)monster.Hp, damageMonsterAfterHandle));
            Monster.Zone.ZoneHandler.SendMessage(Service.MonsterHp(monster, false, damageMonsterAfterHandle, -1));
            return damageMonsterAfterHandle;
        }

        public void PetAttackPlayer(ICharacter character)
        {
            if(character == null) return;
            try
            {
                var damage = Monster.Damage;
                var damageReal = ServerUtils.RandomNumber(damage * 9 / 10, damage);
                damageReal -= character.DefenceFull;
                if (ServerUtils.RandomNumber(100) < 20)
                {
                    damage = 0;
                }
                
                character.CharacterHandler.MineHp(damageReal);
                Monster.Zone.ZoneHandler.SendMessage(Service.UpdateMonsterMe2(Monster.IdMap, character.Id, damage, (int)character.InfoChar.Hp));
                if (SkillHandler.CharIdIsBoss(character.Id) && character.InfoChar.IsDie)
                {
                    var charReal = (Model.Character.Character)Monster.Character;
                    var bossReal = (Model.Character.Boss)character;
                    switch (bossReal.Type)
                    {
                        case 64:
                            bossReal.InfoChar.TypePk = 0;
                            bossReal.CharacterHandler.SendZoneMessage(Service.ChangeTypePk(bossReal.Id, 0));
                            var item = ItemCache.GetItemDefault(590);
                            var itemmap = new ItemMap(character.Id, item);
                            itemmap.X = bossReal.InfoChar.X;
                            itemmap.Y = bossReal.InfoChar.Y;
                            bossReal.Zone.ZoneHandler.LeaveItemMap(itemmap);
                            bossReal.InfoDelayBoss.AutoPlusHP = 3000 + ServerUtils.CurrentTimeMillis();
                            break;
                        case 114:
                            charReal.DataSieuHang.Handler.Win(charReal);
                            break;
                        case >= 87 and <= 92:
                            charReal.DataPractice.Handler.Kill(bossReal, charReal, bossReal.Type);
                            break;
                        case 107:

                            break;
                        case 73:
                        case 74:
                        case 75:
                        case 76:
                        case 77:
                            async void ActionSaibamen()
                            {
                                charReal.DataDaiHoiVoThuat23.Handler.ThoiMien(character, 3000);
                                var bossNext = bossReal.Zone.ZoneHandler.BossInMap()[1];
                                bossReal.Zone.ZoneHandler.SendMessage(Service.PublicChat(bossReal.Id, "zzzz"));
                                await Task.Delay(1000);
                                SkillHandler.HandleTuSat(bossReal, null, null);
                                await Task.Delay(1000);
                                bossNext.InfoChar.TypePk = 5;
                                bossReal.Zone.ZoneHandler.SendMessage(Service.ChangeTypePk(bossNext.Id, 5));
                            }
                            var task = new Task(ActionSaibamen);
                            task.Start();
                            break;
                        case 78:
                            async void ActionNappa()
                            {
                                charReal.DataDaiHoiVoThuat23.Handler.ThoiMien(character, 3000);
                                var bossNext = bossReal.Zone.ZoneHandler.BossInMap()[1];
                                bossReal.Zone.ZoneHandler.SendMessage(Service.PublicChat(bossReal.Id, "Very good, seen you soon"));
                                await Task.Delay(1000);
                                SkillHandler.HandleTuSat(bossReal, null, null);
                                await Task.Delay(1000);
                                bossNext.InfoChar.TypePk = 5;
                                bossReal.Zone.ZoneHandler.SendMessage(Service.ChangeTypePk(bossNext.Id, 5));
                            }
                            var task4 = new Task(ActionNappa);
                            task4.Start();
                            break;
                        case 79:
                            async void ActionCadic()
                            {
                                charReal.DataDaiHoiVoThuat23.Handler.ThoiMien(character, 3000);
                                bossReal.Zone.ZoneHandler.SendMessage(Service.PublicChat(bossReal.Id, "Arghhhhhh"));
                                await Task.Delay(1000);
                                SkillHandler.HandleTuSat(bossReal, null, null);
                                await Task.Delay(1000);

                            }
                            var task5 = new Task(ActionCadic);
                            task5.Start();
                            break;
                        case 95:
                        case 96:
                        case 97:
                            var clan = ClanManager.Get(character.ClanId);
                            if (clan != null)
                            {
                                clan.ClanBoss.Time = 30000 + ServerUtils.CurrentTimeMillis();
                                clan.ClanBoss.Level++;
                                if (clan.ClanBoss.Level >= 3)
                                {
                                    clan.ClanBoss.Status = Model.Clan.ClanBoss.ClanBoss_Status.END;
                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã chiến thắng, mai hãy quay lại đánh boss tiếp nhé"));
                                }
                                clan.Capsule_Bang += 1000;
                                List<List<long>> Object = new List<List<long>>();

                                foreach (var memberId in clan.Thành_viên.Select(member => member.Id))
                                {
                                    var clanMember = ClientManager.Gi().GetCharacter(memberId);
                                    if (clanMember != null)
                                    {
                                        Object.Add(new List<long> { clanMember.Id, clanMember.DameBossBangHoi });
                                    }
                                }

                                Object.Sort((g2, g1) => g2[1].CompareTo(g1[1]));

                                foreach (var obj in Object.Take(Object.Count - 1)) // Assuming processing for top 9 members
                                {
                                    var idObject = obj[0];
                                    var CurrentObject = ClientManager.Gi().GetCharacter((int)idObject);
                                    if (CurrentObject != null)
                                    {
                                        var CurrentObjectClanMember = ClanManager.Get(CurrentObject.ClanId).Thành_viên.FirstOrDefault(i => i.Id == idObject);
                                        var i2 = Object.IndexOf(obj);
                                        var CSBCollect = 5 * (i2 <= 4 ? 3 : i2 <= 8 ? 2 : 1) + (idObject == bossReal.KillerId ? 10 : 0);
                                        CurrentObjectClanMember.Capsule_Bang = CurrentObjectClanMember.Capsule_Cá_Nhân += CSBCollect;
                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + CSBCollect + " Capsule Bang"));
                                    }
                                }


                            }
                            break;
                        case 68:
                        case 69:
                        case 70:
                        case 71:
                        case 72:
                            // Died_Ring.gI().Kill((Model.Character.Character)character);
                            DiedRing_Handler.gI().Kill(charReal);
                            break;

                        case 36:
                            async void SpawnMabu()
                            {
                                for (int i = 0; i < 11; i++)
                                {
                                    bossReal.Zone.ZoneHandler.SendMessage(Mabu12hService.NoTrungMabu((byte)(10 * i)));
                                    await Task.Delay(500);
                                }
                                await Task.Delay(1500);
                                Mabu12h.gI().InitMabu(bossReal.Zone.Id);
                            }
                            var task3 = new Task(SpawnMabu);
                            task3.Start();
                            break;
                        case 37:
                        case 38:
                        case 39:
                            if (DataCache.IdMapMabu.Contains(bossReal.Zone.Map.Id))
                            {
                                charReal.PPower += 50;
                                character.CharacterHandler.SendMessage(Mabu12hService.sendPowerInfo("TL", (short)charReal.PPower, 50, 10));
                            }
                            Thread.Sleep(30000);
                            var bossAgain = new Boss();
                            bossAgain.CreateBoss(bossReal.Type);
                            bossAgain.CharacterHandler.SetUpInfo();
                            bossReal.Zone.ZoneHandler.AddBoss(bossAgain);
                            break;


                        case 1:

                            if (charReal.Disciple == null)
                            {
                                Menu.Menu.CreatePetNormal(charReal);
                            }
                            break;
                        case 23:
                            async void Hachijack()  
                            {
                                var dr = character.Zone.ZoneHandler.BossInMap()[1];
                                dr.Zone.ZoneHandler.SendMessage(Service.PublicChat(dr.Id, "Ồ, ngay cả Dr.Lychee cũng không thể hạ gục nguưi"));
                                await Task.Delay(1000);
                                dr.Zone.ZoneHandler.SendMessage(Service.PublicChat(dr.Id, "Tiếp theo đây, để ta xem thực lực ngươi tới đâu"));
                                await Task.Delay(1000);
                                dr.Zone.ZoneHandler.SendMessage(Service.PublicChat(dr.Id, "Muahahahahha"));
                                dr.InfoChar.TypePk = 5;
                                dr.Zone.ZoneHandler.SendMessage(Service.ChangeTypePk(dr.Id, 5));
                            }
                            var task2 = new Task(Hachijack);
                            task2.Start();
                            break;
                        case 52:
                        case 53:
                        case 54:
                        case 55:
                        case 56:
                        case 57:
                        case 58:
                        case 59:
                        case 60:
                        case 61:
                        case 62:
                        case 112:
                            charReal.DataDaiHoiVoThuat23.Handler.Fight(charReal, true);
                            break;
                        case 113:
                            Whis_Practice.gI().Kill((Model.Character.Character)character);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error HandleTroiMonster in SkillHandler.cs: {e.Message} \n {e.StackTrace}", e);
            }
        }

        public void MonsterAttack(ICharacter temp, ICharacter character)
        {
            //Ignore
        }

        public void Update()
        {
            var timeServer = ServerUtils.CurrentTimeMillis();
            if (!Monster.IsDie)
            {
                if (Monster.Character?.InfoSkill.Egg.Time <= timeServer)
                {
                    StartDie();
                }
            }
        }

        public void AddPlayerAttack(ICharacter character, int damage)
        {
            //Ignore
        }

        public void RemoveTroi(int charId)
        {
            //Ignore
        }

        public MonsterPetHandler(IMonster monster)
        {
            Monster = monster;
        }
    }
}