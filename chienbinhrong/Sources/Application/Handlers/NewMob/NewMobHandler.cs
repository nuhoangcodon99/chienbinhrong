using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Handlers.Item;
using NgocRongGold.Application.Handlers.Skill;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.Interfaces.Monster;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Model.Monster;
using NgocRongGold.Model.Character;
using NgocRongGold.Application.Threading;
using NgocRongGold.Application.Manager;
using NgocRongGold.Model.Item;
using NgocRongGold.DatabaseManager;
using System.Threading.Tasks;
using NgocRongGold.Application.MainTasks;
using Org.BouncyCastle.Math.Field;
using System.Net.WebSockets;
using System.Drawing;
using NgocRongGold.Model.Info.Skill;
using System.ComponentModel.DataAnnotations;
using NgocRongGold.DatabaseManager.Player;

namespace NgocRongGold.Application.Handlers.Monster
{

    public class NewMobHandler : IMonsterHandler
    {
        public IMonster Monster { get; set; }

        public void SetUpMonster(bool isDie = false)
        {
            Monster.IsDie = isDie;
            SetClassNewMonster();
            SetHpNewMonster();
            SetLevelBossNewMonster();
            SetDamageBossNewMonster();
        }

        public void SetClassNewMonster()
        {
            lock (Monster)
            {
                switch (Monster.Id)
                {

                    default:
                        Monster.Sys = (sbyte)ServerUtils.RandomNumber(1, 3);
                        break;
                }

            }
        }

        public void SetHpNewMonster()
        {
            Monster.Hp = Monster.HpMax = GetMonsterHP();
            Monster.MaxExp = (int)Monster.OriginalHp;
        }

        private long GetMonsterHP()
        {
            switch (Monster.LvBoss)
            {
                //case >= 1:
                    //return Monster.OriginalHp = Monster.OriginalHp * Monster.LvBoss;
                default:
                    return Monster.OriginalHp;
            }

        }

        public void SetLevelBossNewMonster()
        {
            Monster.LvBoss = 0;
        }

        public void SetDamageBossNewMonster()
        {
            Monster.Damage = (int)Monster.HpMax * 5 / 100;
        }

        public void Recovery()
        {

            RemoveEffect(ServerUtils.CurrentTimeMillis(), globalReset: true);
            Monster.IsDie = false;
            SetClassNewMonster();
            SetHpNewMonster();
            SetLevelBossNewMonster();
            SetDamageBossNewMonster();
            Monster.Status = 5;
            Monster.IsDontMove = false;
            Monster.Zone.ZoneHandler.SendMessage(Service.MobLive(Monster));
            Monster.CharacterAttack.Clear();
            Monster.SessionAttack.Clear();

        }

        public int UpdateHp(long damage, int charId, bool isMaxHp = false)
        {
            if (damage < 0) damage = Math.Abs(damage);

            if (Monster.Id == 0)
            {
                damage = 10;
            }
            else
            {
                if (Monster.Hp == Monster.HpMax && damage >= Monster.HpMax && !isMaxHp)
                {
                    damage = Monster.HpMax - 1;
                }
                else if (damage >= Monster.Hp)
                {
                    damage = Monster.Hp;
                }
            }

            Monster.Hp -= damage;
            if (Monster.Hp <= 0)
            {
                StartDie();
            }
            return (int)damage;
        }

        private void StartDie()
        {
            Monster.Zone.ZoneHandler.SendMessage(Service.MonsterDie(Monster.IdMap));
            Monster.Hp = 0;
            Monster.IsDie = true;
            Monster.Status = 0;
            RemoveEffect(ServerUtils.CurrentTimeMillis(), true);
            if (Monster.IsRefresh) Monster.TimeRefresh = ServerUtils.CurrentTimeMillis() + 7000;
        }
        public void LeaveItem(ICharacter character)
        {
            if (!Monster.IsDie) return;

            var plusGoldPercent = 0;
            Model.Character.Character charRel = null;
            if (character.Id > 0)
            {
                charRel = (Model.Character.Character)character;
                var specialId = charRel.SpecialSkill.Id;
                if (specialId != -1 && (specialId == 7 || specialId == 18 || specialId == 28)) //Đã có nội tại rơi vàng cộng thêm từ quái
                {
                    plusGoldPercent += charRel.SpecialSkill.Value;
                }
            }

            plusGoldPercent += character.InfoOption.PhanTramVangTuQuai;


            var itemMap = LeaveItemHandler.LeaveMonsterItemRecode(character, Monster.LeaveItemType, plusGoldPercent, character.Zone.Map.Id, Monster.Id);
            if (itemMap == null) return;
            itemMap.X = Monster.X;
            itemMap.Y = Monster.Y;
            Monster.Zone.ZoneHandler.LeaveItemMap(itemMap, (MonsterMap)Monster);


            // Kiểm tra có bùa thu hút ko
            if (character.Id > 0)
            {
                if (charRel.InfoMore.BuaThuHut)
                {
                    if (charRel.InfoMore.BuaThuHutTime > ServerUtils.CurrentTimeMillis())
                    {
                        charRel.CharacterHandler.PickItemMap((short)itemMap.Id);
                    }
                    else
                    {
                        charRel.InfoMore.BuaThuHut = false;
                    }
                }
            }
            else //Đệ 
            {
                var disciple = (Model.Character.Disciple)character;
                if (disciple.Character.InfoMore.BuaThuHut)
                {
                    if (disciple.Character.InfoMore.BuaThuHutTime > ServerUtils.CurrentTimeMillis())
                    {
                        disciple.Character.CharacterHandler.PickItemMap((short)itemMap.Id);
                    }
                    else
                    {
                        disciple.Character.InfoMore.BuaThuHut = false;
                    }
                }
            }

        }

        public void MonsterAttack(long timeServer)
        {

           
            foreach (var character in Monster.Zone.Characters.Values.ToList())
            {
                // Quái không tự đánh
                if (!character.InfoChar.IsDie && Math.Abs(character.InfoChar.X - Monster.X) <= 150 &&
                    Math.Abs(character.InfoChar.Y - Monster.Y) <= 40)
                {
                    Characters.Add(character);
                }

            }
            if (Characters.Count > 0)
            {
                HandlerAttackCharacter(Characters, timeServer);
            }
        }
        public List<ICharacter> Characters = new List<ICharacter>();
        public void HandlerAttackCharacter(List<ICharacter> characters, long timeServer)
        {
            List<int> Damages = new List<int>();
            Monster.TimeAttack = 2000 + timeServer;
            foreach (var character in characters)
            {
                var damage = ServerUtils.RandomNumber(Monster.Damage * 9 / 10, Monster.Damage * 11 / 10);

                if (Monster.LvBoss == 1 && !Monster.IsBoss)
                {
                    if (damage < character.HpFull)
                    {
                        damage = (int)character.HpFull / 10;
                    }
                }

                damage -= character.DefenceFull;

                if (Monster.InfoSkill.ThoiMien.TimePercent > 0 && Monster.InfoSkill.ThoiMien.TimePercent > timeServer)
                {
                    damage -= damage * Monster.InfoSkill.ThoiMien.Percent / 100;
                }

                if (Monster.InfoSkill.Socola.IsSocola && Monster.LvBoss == 0 && !Monster.IsBoss && Monster.InfoSkill.Socola.CharacterId == character.Id)
                {
                    damage = 1;
                }
                if (Monster.InfoSkill.MaPhongBa.isMaPhongBa)
                {
                    damage = 1;
                }
                else
                {
                    if (Monster.InfoSkill.Socola.IsSocola)
                    {
                        damage -= damage * Monster.InfoSkill.Socola.Percent / 100;
                    }

                    if (ServerUtils.RandomNumber(100) < 50 && damage <= 0)
                    {
                        damage = 1;
                    }
                    else if (character.InfoSkill.Protect.IsProtect)
                    {
                        if (character.HpFull <= damage)
                        {
                            //HANDLE REMOVE SKILL PROTECT
                            if (character.Id > 0)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().DROP_PROTECT));
                                character.CharacterHandler.SendMessage(Service.ItemTime(DataCache.TimeProtect[0], 0));
                            }
                            SkillHandler.RemoveProtect(character);
                        }
                        damage = 1;
                    }
                }

                // Xử lý phản phần trăm sát thương
                int phanTramPhanSatThuong = character.InfoOption.PhanPercentSatThuong;

                if (damage > 0 && phanTramPhanSatThuong > 0)
                {
                    int phanDamage = damage * phanTramPhanSatThuong / 100;
                    UpdateHp(phanDamage, character.Id);
                    character.CharacterHandler.SendZoneMessage(Service.MonsterHp(Monster, false, phanDamage, -1));
                }

                // Kiểm tra phải đệ tử nhận damage không
                // Kiểm tra xem có bùa đệ tử không
                if (character.Id < 0 && damage > 0)
                {
                    var discipleReal = (Model.Character.Disciple)character;
                    if (discipleReal.Character.InfoMore.BuaDeTu)
                    {
                        if (discipleReal.Character.InfoMore.BuaDeTuTime > timeServer)
                        {
                            damage /= 2;
                        }
                        else
                        {
                            discipleReal.Character.InfoMore.BuaDeTu = false;
                        }
                    }
                }

                if (character.Id > 0)
                {
                    var charRel = (Model.Character.Character)character;

                    // Kiểm tra xem có bùa da trâu không
                    if (charRel.InfoMore.BuaDaTrau)
                    {
                        if (charRel.InfoMore.BuaDaTrauTime > ServerUtils.CurrentTimeMillis())
                        {
                            damage /= 2;
                        }
                        else
                        {
                            charRel.InfoMore.BuaDaTrau = false;
                        }
                    }

                    if (charRel.InfoMore.BuaBatTu)
                    {
                        if (charRel.InfoMore.BuaBatTuTime > timeServer)
                        {
                            // Neus damage lớn hơn máu thì set máu bằng 1
                            if (character.InfoChar.Hp - damage <= 1)
                            {
                                character.InfoChar.Hp = 1;
                                character.CharacterHandler.SendMessage(Service.SendHp((int)character.InfoChar.Hp));
                                damage = 0;
                            }
                        }
                        else
                        {
                            charRel.InfoMore.BuaBatTu = false;
                        }
                    }

                    if (charRel.InfoMore.IsNearAuraPhongThuItem)
                    {
                        if (charRel.InfoMore.AuraPhongThuTime > timeServer)
                        {
                            damage -= damage * 20 / 100;
                        }
                        else
                        {
                            charRel.InfoMore.IsNearAuraPhongThuItem = false;
                        }
                    }

                    // Giáp xên
                    if (damage > 0 && charRel.InfoBuff.GiapXen)
                    {
                        damage -= (damage * 50 / 100);
                    }
                    if (damage > 0 && charRel.InfoBuff.GiapXen2)
                    {
                        damage -= (damage * 60 / 100);
                    }
                    if (damage > 0 && charRel.InfoBuff.BinhChuaCommeson)
                    {
                        damage -= (damage * 90 / 100);
                    }
                }

                if (damage < 0)
                {
                    damage = 1;
                }
                character.CharacterHandler.MineHp(damage);

                Monster.Zone.ZoneHandler.SendMessage(Service.MonsterAttackPlayer(Monster.IdMap, character));
                if (character.Id > 0)
                {
                    character.CharacterHandler.SendMessage(Service.MonsterAttackMe(Monster.IdMap, damage, 0));
                }

                if (character.InfoChar.IsDie)
                {
                    character.CharacterHandler.SendDie();
                }
                Damages.Add(damage);
            }
            if (Characters[0] != null)
            {
                Monster.X = Characters[0].InfoChar.X;
                Monster.Y = Characters[0].InfoChar.Y;
                Monster.Zone.ZoneHandler.SendMessage(NewMobMove(Characters[0].InfoChar.X, Characters[0].InfoChar.Y));

            }
            Monster.Zone.ZoneHandler.SendMessage(NewMobAttack((byte)(11 + count), Characters, Damages));
            
            Characters.Clear();
            count++;
            if(count >= 2)
            {
                count = 0;
            }
        }
        int count = 0;
        public Message NewMobAttack(byte type, List<ICharacter> characters,List<int> damage)
        {
            var msg = new Message(102);
            msg.Writer.WriteByte(type);
            msg.Writer.WriteByte(Monster.IdMap);//idmap
            msg.Writer.WriteByte(characters.Count); //count pl in map
            for( int i = 0; i < characters.Count;i++)
            {
                var temp = characters[i];
                msg.Writer.WriteInt(temp.Id);
                msg.Writer.WriteInt(damage[i]);
            }
            msg.Writer.WriteByte(0);
            Server.Gi().Logger.Debug($"{Cache.Gi().MONSTER_TEMPLATES[Monster.Id].Name} Attack [{characters.Count}] Type: {type}");
            return msg;

        }
        public Message NewMobMove(short x, short y)
        {
            var msg = new Message(102);
            msg.Writer.WriteByte(10);
            msg.Writer.WriteByte(Monster.IdMap);//idmap
            msg.Writer.WriteShort(x);
            msg.Writer.WriteShort(y);
            return msg;

        }
        public Message NewMobFly(short x, short y)
        {
            var msg = new Message(102);
            msg.Writer.WriteByte(21);
            msg.Writer.WriteByte(Monster.IdMap);//idmap
            msg.Writer.WriteShort(x);
            msg.Writer.WriteShort(y);
            return msg;

        }
        public Message NewMobDie()
        {
            var msg = new Message(102);
            msg.Writer.WriteByte(23);
            msg.Writer.WriteByte(Monster.IdMap);//idmap
            return msg;
        }
        public int PetAttackMonster(IMonster monster)
        {
            //Ignored
            return 0;
        }

        public void PetAttackPlayer(ICharacter character)
        {
            //Ignoredf
        }

        public void MonsterAttack(ICharacter temp, ICharacter character)
        {
            //Ignored
        }
        public Boolean CheckBulonInReddot()
        {
            if (DataCache.IdMapSpecial.Contains(Monster.Zone.Map.Id) && Monster.Id == 22)
            {
                if (Monster.Zone.ZoneHandler.GetBoss(47) != null)
                {
                    return true;
                }
            }
            return false;
        }
        public void Update()
        {
            try
            {
                var timeServer = ServerUtils.CurrentTimeMillis();
                //if (Monster.Id == 77 && !Monster.IsDie&&Monster.Zone.Map.Id != ThiefBear.gI().CurrentMapsSpawn && Monster.Zone.Id != ThiefBear.gI().CurrentZonesSpawn)
                //{
                //    Monster.Hp = 0;
                //    Monster.IsDie = true;
                //    Monster.Status = 0;
                //    return;
                //}
                if (Monster.IsDie && Monster.IsRefresh && Monster.TimeRefresh > 0 && Monster.TimeRefresh <= timeServer && (!DataCache.IdMapSpecial.Contains(Monster.Zone.Map.Id) || CheckBulonInReddot()))
                {
                    Recovery();
                }
                if (!Monster.IsDie)
                {
                    RemoveEffect(timeServer);
                }

                if (Monster.InfoSkill.PlayerTroi.IsPlayerTroi || Monster.InfoSkill.DichChuyen.IsStun) return;

                if (!Monster.IsDie && Monster.Id != 0)
                {
                    if (Monster.TimeHp <= timeServer && Monster.Hp < Monster.HpMax && Monster.TimeAttack <= timeServer)
                    {
                        if (Monster.Hp + (Monster.HpMax / 10) > Monster.HpMax)
                        {
                            Monster.Hp += Monster.HpMax - Monster.Hp;
                        }
                        else
                        {
                            Monster.Hp += Monster.HpMax / 10;
                        }
                        Monster.TimeHp = 5000 + timeServer;
                        Monster.Zone.ZoneHandler.SendMessage(Service.MonsterHp(Monster));
                    }
                    MonsterAttack(timeServer);
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error UpdateMonsterMap in ZoneHandler.cs: {e.Message} \n {e.StackTrace}", e);
            }
        }

        public void AddPlayerAttack(ICharacter character, int damage = 0)
        {
            if (!Monster.CharacterAttack.Contains(character.Id))
            {
                Monster.CharacterAttack.Add(character.Id);
            }

            //var sessId = character.Player.Session.Id;

            //if (!Monster.SessionAttack.TryAdd(sessId, damage))
            //{
            //    var dmg = Monster.SessionAttack[sessId];
            //    dmg += damage;
            //    Monster.SessionAttack[sessId] = dmg;
            //};
        }

        public void RemoveTroi(int charId)
        {
            try
            {
                lock (Monster.InfoSkill.PlayerTroi)
                {
                    var infoSkill = Monster.InfoSkill.PlayerTroi;
                    if (infoSkill.IsPlayerTroi)
                    {
                        infoSkill.PlayerId.RemoveAll(i => i == charId);
                        if (infoSkill.PlayerId.Count <= 0)
                        {
                            infoSkill.IsPlayerTroi = false;
                            infoSkill.TimeTroi = -1;
                            infoSkill.PlayerId.Clear();
                            Monster.Zone.ZoneHandler.SendMessage(Service.SkillEffectMonster(-1, Monster.IdMap, 0, 32));
                            if (Monster.IsDontMove)
                            {
                                Monster.IsDontMove = false;
                                Monster.Zone.ZoneHandler.SendMessage(Service.MonsterDontMove(Monster.IdMap, false));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error("Error RemoveTroi", e);
            }
        }

        public void RemoveDichChuyen(long timeServer, bool globalReset)
        {
            lock (Monster.InfoSkill.DichChuyen)
            {
                if (Monster.InfoSkill.DichChuyen.IsStun && Monster.InfoSkill.DichChuyen.Time <= timeServer || globalReset)
                {
                    Monster.InfoSkill.DichChuyen.IsStun = false;
                    Monster.InfoSkill.DichChuyen.Time = -1;
                    Monster.Zone.ZoneHandler.SendMessage(Service.SkillEffectMonster(-1, Monster.IdMap, 0, 40));
                    if (Monster.IsDontMove)
                    {
                        Monster.IsDontMove = false;
                        Monster.Zone.ZoneHandler.SendMessage(Service.MonsterDontMove(Monster.IdMap, false));
                    }
                }
            }
        }

        public void RemoveThoiMien(long timeServer, bool globalReset)
        {
            lock (Monster.InfoSkill.ThoiMien)
            {
                if (Monster.InfoSkill.ThoiMien.IsThoiMien && Monster.InfoSkill.ThoiMien.Time <= timeServer || globalReset)
                {
                    Monster.InfoSkill.ThoiMien.IsThoiMien = false;
                    Monster.InfoSkill.ThoiMien.Time = -1;
                    Monster.InfoSkill.ThoiMien.TimePercent = 10000 + timeServer;
                    Monster.Zone.ZoneHandler.SendMessage(Service.SkillEffectMonster(-1, Monster.IdMap, 0, 41));
                    if (Monster.IsDontMove)
                    {
                        Monster.IsDontMove = false;
                        Monster.Zone.ZoneHandler.SendMessage(Service.MonsterDontMove(Monster.IdMap, false));
                    }
                }
            }
        }

        public void RemoveThoiMien2(long timeServer, bool globalReset)
        {
            lock (Monster.InfoSkill.ThoiMien)
            {
                if (Monster.InfoSkill.ThoiMien.TimePercent > 0 && Monster.InfoSkill.ThoiMien.TimePercent <= timeServer || globalReset)
                {
                    Monster.InfoSkill.ThoiMien.TimePercent = -1;
                    Monster.InfoSkill.ThoiMien.Percent = 0;
                }
            }
        }

        public void RemoveThaiDuongHanSan(long timeServer, bool globalReset)
        {
            lock (Monster.InfoSkill.ThaiDuongHanSan)
            {
                if (Monster.InfoSkill.ThaiDuongHanSan.IsStun && Monster.InfoSkill.ThaiDuongHanSan.Time <= timeServer || globalReset)
                {
                    Monster.InfoSkill.ThaiDuongHanSan.IsStun = false;
                    Monster.InfoSkill.ThaiDuongHanSan.Time = -1;
                    Monster.InfoSkill.ThaiDuongHanSan.TimeReal = 0;
                    Monster.Zone.ZoneHandler.SendMessage(Service.SkillEffectMonster(-1, Monster.IdMap, 0, 40));
                    if (Monster.IsDontMove)
                    {
                        Monster.IsDontMove = false;
                        Monster.Zone.ZoneHandler.SendMessage(Service.MonsterDontMove(Monster.IdMap, false));
                    }
                }
            }
        }

        public void RemoveTroi(long timeServer, bool globalReset)
        {
            try
            {
                lock (Monster.InfoSkill.PlayerTroi)
                {
                    var infoSkill = Monster.InfoSkill.PlayerTroi;
                    if ((infoSkill.IsPlayerTroi && infoSkill.TimeTroi < timeServer) || globalReset)
                    {
                        infoSkill.PlayerId.Clear();

                        infoSkill.IsPlayerTroi = false;
                        infoSkill.TimeTroi = -1;
                        infoSkill.PlayerId.Clear();
                        Monster.Zone.ZoneHandler.SendMessage(Service.SkillEffectMonster(-1, Monster.IdMap, 0, 32));
                        if (Monster.IsDontMove)
                        {
                            Monster.IsDontMove = false;
                            Monster.Zone.ZoneHandler.SendMessage(Service.MonsterDontMove(Monster.IdMap, false));
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error("Error RemoveTroi", e);
            }
        }

        public void RemoveSocola(long timeServer, bool globalReset)
        {
            lock (Monster.InfoSkill.Socola)
            {
                var infoSkill = Monster.InfoSkill.Socola;
                if (infoSkill.IsSocola && infoSkill.Time <= timeServer || globalReset)
                {
                    infoSkill.IsSocola = false;
                    infoSkill.Time = -1;
                    infoSkill.CharacterId = -1;
                    infoSkill.Fight = 0;
                    infoSkill.Percent = 0;
                    Monster.Zone.ZoneHandler.SendMessage(Service.ChangeMonsterBody(0, Monster.IdMap, -1));
                }
            }
        }
        public void RemoveMaPhongBa(long timeServer, bool globalReset)
        {
            lock (Monster.InfoSkill.MaPhongBa)
            {
                var infoSkill = Monster.InfoSkill.MaPhongBa;
                if (infoSkill.isMaPhongBa && infoSkill.timeMaPhongBa <= timeServer || globalReset)
                {
                    infoSkill.isMaPhongBa = false;
                    infoSkill.timeMaPhongBa = -1;
                    Monster.Zone.ZoneHandler.SendMessage(Service.ChangeMonsterBody(0, Monster.IdMap, -1));
                }
            }
        }
        public void RemoveEffect(long timeServer, bool globalReset = false)
        {
            RemoveMaPhongBa(timeServer, globalReset);
            RemoveDichChuyen(timeServer, globalReset);
            RemoveThoiMien(timeServer, globalReset);
            RemoveThoiMien2(timeServer, globalReset);
            RemoveThaiDuongHanSan(timeServer, globalReset);
            RemoveTroi(timeServer, globalReset);
            RemoveSocola(timeServer, globalReset);
        }

        public NewMobHandler(IMonster monster)
        {
            Monster = monster;
        }
    }
}