using System;
using System.Collections.Generic;
using System.Linq;
using Linq.Extras;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Handlers.Skill;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.Interfaces.Monster;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Handlers.Item;
using NgocRongGold.Application.Threading;
using NgocRongGold.DatabaseManager;
using NgocRongGold.DatabaseManager.Player;
using NgocRongGold.Model;
using NgocRongGold.Model.Info;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Map;
using NgocRongGold.Model.SkillCharacter;
using Org.BouncyCastle.Math.Field;
using static System.GC;
using System.IO;
using System.Runtime.InteropServices;
using Application.Interfaces.Zone;

namespace NgocRongGold.Application.Handlers.Character
{
    public class PhanThanHandler : ICharacterHandler
    {
        public int GetAllQuantityItemBagById(int id)
        {
            return 0;
        }
        public void OpenUiSay(string say)
        {

        }
        public void SendServerMessage(string say)
        {

        }
        public void SetEnhancedOptionCard()
        {

        }

        public void SetupAmulet()
        {

        }
        public void UpdateOther(long timeServer)
        {
        }
        public PhanThan Disciple { get; set; }
        public Model.Item.Item GetItemClanBoxByIndex(int index)
        {
            return null;
        }
        public void CreatePetNormal()
        {

        }
        public void UpdatePet()
        {
            //ingored
        }
        public Model.Item.Item RemoveItemClanBox(int index, bool isReset = true)
        {
            return null;

        }
        public PhanThanHandler(PhanThan disciple)
        {
            Disciple = disciple;
        }
        public void PlusDiamondLock(int diamond)
        {
            //ingored
        }
        public int GetThoiVangInRuong()
        {
            return -1;
            //ingored
        }
        public void UpdateItem10()
        {
            //ingored
        }
        public int GetThoiVangInBag()
        {
            return -1;
            //ingored
        }
        public void UpdateEffectCharacter()
        {
            //ingored
        }
        public void SetUpPhoBan()
        {

        }
        public void Dispose()
        {
            SuppressFinalize(this);
        }

        public void SendZoneMessage(Message message)
        {
            Disciple?.Zone?.ZoneHandler.SendMessage(message);
        }

        public void Update()
        {
            lock (Disciple)
            {
                try
                {

                    var timeServer = ServerUtils.CurrentTimeMillis();
                    AutoRemove(timeServer);
                    RemoveSkill(timeServer);

                    if (!Disciple.InfoChar.IsDie)
                    {
                        AutoDisciple(timeServer);
                    }
                }
                catch (Exception e)
                {
                   
                }

            }
        }

        private void BuaDeTu()
        {
           

        }
        private void AutoRemove(long timeServer)
        {

            if (Disciple.InfoDelayDisciple.TimePhanThan < timeServer || Disciple.InfoChar.IsDie)
            {
                Close();
            }
        }
        private void AutoDisciple(long timeServer)
        {
            if (Disciple.IsDontMove()) return;
            switch (Disciple.Status)
            {
                //Đi theo
                case 0:
                    {
                        if (Math.Abs(Disciple.InfoChar.X - Disciple.Character.InfoChar.X) >= 60)
                        {
                            SetUpPosition(isRandom: true);
                            SendZoneMessage(Service.PlayerMove(Disciple.Id, Disciple.InfoChar.X, Disciple.InfoChar.Y));
                        }
                        AutoMoveMap(timeServer);
                        break;
                    }
                //Bảo vệ
                case 1:
                    {
                        
                                HandleUseSkill();
                        break;
                    }
                // Tấn công
                case 2:
                    {
                        
                        HandleUseSkill();
                        

                        break;
                    }
            }
        }

        private void HandleUseSkill(bool isAuto = true, int charId = -1, int modId = -1)
        {
            if (!Disciple.IsFire) return;
            if (Disciple.CharacterFocus != null)
            {
                if (Disciple.Zone.ZoneHandler.GetICharacter(Disciple.CharacterFocus.Id) == null)
                {
                    Disciple.CharacterFocus = null;
                    return;
                }
                var infoSkill = Disciple.InfoSkill;
                var timeServer = ServerUtils.CurrentTimeMillis();
                // Tái tạo năng lượng
                // Đang tái tạo năng lượng sẽ không bị xóa
                if (infoSkill.TaiTaoNangLuong.IsTTNL &&
                    infoSkill.TaiTaoNangLuong.DelayTTNL > timeServer)
                {
                    if (Disciple.InfoDelayDisciple.TTNL <= timeServer)
                    {
                        // Xử lý tái tạo năng lượng
                        SkillHandler.SkillNotFocus(Disciple, 8, 2);
                        Disciple.InfoDelayDisciple.TTNL = timeServer + 30000;
                    }
                    return;
                }
                var enemy = Disciple.CharacterFocus;
                SkillCharacter skillChar = null;
                var dX = 0;
                var dY = 0;
                bool isMoveToPlayer = false;
                //  try {

                // Kiểm tra khoản cách giữa quái và đệ
                var bossDistance = Math.Abs(enemy.InfoChar.X - Disciple.InfoChar.X);
                var bossDistanceY = Math.Abs(enemy.InfoChar.Y - Disciple.InfoChar.Y);
                // for skill từ trên xuống dưới
                for (int i = Disciple.Skills.Count - 1; i >= 0; i--)
                {
                    skillChar = Disciple.Skills[i];

                    if (skillChar == null)
                    {
                        continue;
                    }

                    var skillTemplate = Cache.Gi().SKILL_TEMPLATES.FirstOrDefault(sk => sk.Id == skillChar.Id);
                    var skillDataTemplate = skillTemplate?.SkillDataTemplates.FirstOrDefault(so => so.SkillId == skillChar.SkillId);
                    if (skillDataTemplate == null)
                    {
                        skillChar = null;
                        continue;
                    }

                    //Check mana
                    var manaUse = skillDataTemplate.ManaUse;
                    var manaUseType = skillTemplate.ManaUseType;
                    var manaChar = Disciple.InfoChar.Mp;
                    manaUse = manaUseType switch
                    {
                        1 => manaUse * (int)Disciple.MpFull / 100,
                        2 => (int)manaChar,
                        _ => manaUse
                    };

                    if (manaUse > manaChar || skillChar.CoolDown > timeServer)
                    {
                        skillChar = null;
                        continue;
                    }

                    dX = skillDataTemplate.Dx;
                    dY = skillDataTemplate.Dy;
                    // Nếu skill 3,4 thỏa mãn đk thì lấy
                    if (i == 3 || i == 2)
                    {
                        if (skillChar.Id == 8)
                        {
                            //   if (ServerUtils.RandomNumber(0, 100) < 80) continue;
                            var hpMine = Disciple.HpFull * 0.6;
                            if (hpMine < Disciple.InfoChar.Hp)
                            {
                                skillChar = null;
                                continue;
                            }

                        }
                        else if (skillChar.Id == 9)
                        {
                            if (ServerUtils.RandomNumber(0, 100) < 80) continue;
                            var hpMine = Disciple.HpFull / 10;
                            if (hpMine >= Disciple.InfoChar.Hp)
                            {
                                skillChar = null;
                                continue;
                            }

                        }
                        break;
                    }
                    // Nếu skill 2 khoản cách lớn hơn >36 thì lấy\
                    if (i == 1 && (((ServerUtils.RandomNumber(100) < 50) && bossDistance <= dX) && (bossDistanceY <= dY)))
                    {
                        break;
                    }
                    else if (i == 0)
                    {
                        if ((bossDistance <= dX && bossDistanceY <= dY) || Disciple.Skills.Count == 1)
                        {
                            break;
                        }
                        else
                        {
                            isMoveToPlayer = true;
                            break;
                        }
                    }
                }

                if (skillChar == null)
                {
                    return;
                }


                if (skillChar.Id == 8)
                {
                    // Bắt đầu dùng tái tạo năng lượng
                    return;
                }

                // Thái dương hạ sang
                if (skillChar.Id == 6)
                {
                    SkillHandler.SkillNotFocus(Disciple, skillChar.Id, 0);
                    return;
                }
                if (skillChar.Id == 14)
                {
                    SkillHandler.SkillNotFocus(Disciple, skillChar.Id, 7);

                    return;
                }
                // Khiên năng lượng
                if (skillChar.Id == 19)
                {
                    SkillHandler.SkillNotFocus(Disciple, skillChar.Id, 9);
                    return;
                }

                if (skillChar.Id == 0 || skillChar.Id == 2 || skillChar.Id == 4 || skillChar.Id == 9 || skillChar.Id == 17 || isMoveToPlayer)
                {
                    if (enemy.InfoChar.X > Disciple.InfoChar.X)
                    {
                        Disciple.InfoChar.X = (short)(enemy.InfoChar.X - dX);
                    }
                    else
                    {
                        Disciple.InfoChar.X = (short)(enemy.InfoChar.X + dX);
                    }


                    Disciple.InfoChar.Y = enemy.InfoChar.Y;
                    isMoveToPlayer = false;

                    SendZoneMessage(Service.PlayerMove(Disciple.Id, Disciple.InfoChar.X, Disciple.InfoChar.Y));
                }
                else if ((skillChar.Id == 1 || skillChar.Id == 3 || skillChar.Id == 5) && Disciple.InfoChar.Y > enemy.InfoChar.Y)
                {
                    if (ServerUtils.RandomNumber(100) < 35)
                    {
                        Disciple.InfoChar.Y = (short)(enemy.InfoChar.Y - ServerUtils.RandomNumber(0, 40));
                    }
                    else
                    {
                        Disciple.InfoChar.Y = enemy.InfoChar.Y;
                    }
                    SendZoneMessage(Service.PlayerMove(Disciple.Id, Disciple.InfoChar.X, Disciple.InfoChar.Y));
                }
                SkillHandler.DiscipleAttackPlayer(Disciple, skillChar, enemy.Id);
            }
            else if (isAuto)
            {
                var infoSkill = Disciple.InfoSkill;
                var timeServer = ServerUtils.CurrentTimeMillis();
                // Tái tạo năng lượng
                // Đang tái tạo năng lượng sẽ không bị xóa
                if (infoSkill.TaiTaoNangLuong.IsTTNL &&
                    infoSkill.TaiTaoNangLuong.DelayTTNL > timeServer)
                {
                    if (Disciple.InfoDelayDisciple.TTNL <= timeServer)
                    {
                        // Xử lý tái tạo năng lượng
                        SkillHandler.SkillNotFocus(Disciple, 8, 2);
                        Disciple.InfoDelayDisciple.TTNL = timeServer + 2000;
                    }
                    return;
                }

                // Tìm quái
                var monster = Disciple.MonsterFocus;

                var checkSize = 220;

                if (Disciple.Status == 2)
                {
                    checkSize = 800;
                    if (monster == null || monster.IsDie || Math.Abs(monster.X - Disciple.InfoChar.X) > checkSize || Math.Abs(monster.Y - Disciple.InfoChar.Y) > 600)
                    {
                        Disciple.MonsterFocus = monster = Disciple.Zone.MonsterMaps.FirstOrDefault(m =>
                            m is { IsDie: false } && Math.Abs(m.X - Disciple.InfoChar.X) <= checkSize && Math.Abs(m.Y - Disciple.InfoChar.Y) <= 600);
                    }
                }
                else if (Disciple.Status == 1)
                {
                    if (monster == null || monster.IsDie || Math.Abs(monster.Y - Disciple.InfoChar.Y) > 600)
                    {
                        var findMonster = Disciple.Zone.MonsterMaps.Where(m =>
                            m is { IsDie: false } && Math.Abs(m.X - Disciple.InfoChar.X) <= checkSize && Math.Abs(m.Y - Disciple.InfoChar.Y) <= 600);
                        if (findMonster != null && findMonster.Count() > 0)
                        {
                            Disciple.MonsterFocus = monster = findMonster.MinBy(m => Math.Abs(m.X - Disciple.InfoChar.X));
                        }
                        else
                        {
                            Disciple.MonsterFocus = monster = null;
                        }
                    }
                }

                if (monster == null)
                {
                    return;
                }

                SkillCharacter skillChar = null;
                var dX = 0;
                var dY = 0;
                try
                {
                    // Kiểm tra khoản cách giữa quái và đệ
                    var monsterDistance = Math.Abs(monster.X - Disciple.InfoChar.X);
                    var monsterDistanceY = Math.Abs(monster.Y - Disciple.InfoChar.Y);
                    // for skill từ trên xuống dưới
                    for (int i = Disciple.Skills.Count - 1; i >= 0; i--)
                    {
                        skillChar = Disciple.Skills[i];

                        if (skillChar == null)
                        {
                            continue;
                        }

                        var skillTemplate = Cache.Gi().SKILL_TEMPLATES.FirstOrDefault(sk => sk.Id == skillChar.Id);
                        var skillDataTemplate = skillTemplate?.SkillDataTemplates.FirstOrDefault(so => so.SkillId == skillChar.SkillId);
                        if (skillDataTemplate == null)
                        {
                            skillChar = null;
                            continue;
                        }

                        //Check mana
                        var manaUse = skillDataTemplate.ManaUse;
                        var manaUseType = skillTemplate.ManaUseType;
                        var manaChar = Disciple.InfoChar.Mp;
                        manaUse = manaUseType switch
                        {
                            1 => manaUse * (int)Disciple.MpFull / 100,
                            2 => (int)manaChar,
                            _ => manaUse
                        };

                        if (manaUse > manaChar || skillChar.CoolDown > timeServer)
                        {
                            skillChar = null;
                            continue;
                        }

                        dX = skillDataTemplate.Dx;
                        dY = skillDataTemplate.Dy;
                        // Nếu skill 3,4 thỏa mãn đk thì lấy
                        if (i == 4)
                        {
                            if (skillChar.Id == 24)
                            {
                                SkillHandler.SkillNotFocus(Disciple, skillChar.Id, 0);
                            }
                        }
                        if (i == 3 || i == 2)
                        {
                            if (skillChar.Id == 9)
                            {
                                if (ServerUtils.RandomNumber(0, 100) < 80) continue;
                                var hpMine = Disciple.HpFull / 10;
                                if (hpMine >= Disciple.InfoChar.Hp)
                                {
                                    skillChar = null;
                                    continue;
                                }

                            }
                            break;
                        }
                        // Nếu skill 2 khoản cách lớn hơn >36 thì lấy
                        else if ((i == 1 || i == 0) && Disciple.Status == 2)
                        {
                            break;
                        }
                        else if (Disciple.Status == 1)
                        {
                            if (i == 1 && ((monsterDistance > 44 && monsterDistance <= dX) || (monsterDistanceY > 44 && monsterDistanceY <= dY)))
                            {
                                break;
                            }
                            else if (i == 0)
                            {
                                if ((monsterDistance <= dX && monsterDistanceY <= dY) || Disciple.Skills.Count == 1)
                                {
                                    // Bảo vệ đủ khoản cách đấm hoặc chỉ có 1 chiêu
                                    break;
                                }
                                else
                                {
                                    skillChar = null;
                                    Disciple.MonsterFocus = null;
                                    break;
                                }
                            }
                        }
                        // Nếu skill 1
                        // Nếu chiêu là chiêu đánh, chưởng, hoặc liên hoàn
                        // Kiểm tra khoản cách giữa quái

                        // Dùng chiêu 2 trước

                        // Kiểm tra nếu gần quá thì dùng chiêu 1
                        // Random nhảy tới dứt
                        // Không thì thấp nhất bởi cooldown
                        // Nếu chiêu là các chiêu biến hình, ttnl, khiên năng lượng
                    }

                    if (skillChar == null)
                    {
                        return;
                    }
                    // skillChar = Disciple.Skills.Where(s => s.CoolDown <= ServerUtils.CurrentTimeMillis()).MinBy(s => s.CoolDown);

                    if (skillChar.Id == 8)
                    {
                        // Bắt đầu dùng tái tạo năng lượng
                        SkillHandler.SkillNotFocus(Disciple, skillChar.Id, 1);
                        return;
                    }

                    // Thái dương hạ sang
                    if (skillChar.Id == 6)
                    {
                        SkillHandler.SkillNotFocus(Disciple, skillChar.Id, 0);
                        return;
                    }

                    // Khiên năng lượng
                    if (skillChar.Id == 19)
                    {
                        SkillHandler.SkillNotFocus(Disciple, skillChar.Id, 9);
                        return;
                    }
                    //Đẻ trứng
                    if (skillChar.Id == 12)
                    {
                        SkillHandler.SkillNotFocus(Disciple, skillChar.Id, 8);
                        return;
                    }

                    //Biến khỉ
                    if (skillChar.Id == 13)
                    {
                        SkillHandler.SkillNotFocus(Disciple, skillChar.Id, 6);
                        return;
                    }

                    if (monster is { IsDie: false })
                    {
                        if (skillChar.Id == 0 || skillChar.Id == 2 || skillChar.Id == 4 || skillChar.Id == 9 || Disciple.Status == 2)
                        {
                            if (monster.X > Disciple.InfoChar.X)
                            {
                                Disciple.InfoChar.X = (short)(monster.X - dX);
                            }
                            else
                            {
                                Disciple.InfoChar.X = (short)(monster.X + dX);
                            }

                            Disciple.InfoChar.Y = monster.Y;
                            SendZoneMessage(Service.PlayerMove(Disciple.Id, Disciple.InfoChar.X, Disciple.InfoChar.Y));
                        }
                        //SendZoneMessage(Service.SendPos(Disciple, 0));
                        SkillHandler.AttackMonster(Disciple, skillChar, monster.IdMap);
                    }
                }
                catch (Exception)
                {
                    // Ignore
                    return;
                }
            }
            else
            {
                if (charId != -1)
                {

                }
                else if (modId != -1)
                {

                }
            }

        }

        private void AutoMoveMap(long timeServer, bool isForce = false)
        {
            if ((Disciple.IsFire && Disciple.InfoDelayDisciple.AutoMove <= timeServer) || isForce)
            {
                Disciple.InfoChar.X = (short)ServerUtils.RandomNumber(Disciple.Character.InfoChar.X - 30,
                    Disciple.Character.InfoChar.X + 30);
                SendZoneMessage(Service.PlayerMove(Disciple.Id, Disciple.InfoChar.X, Disciple.InfoChar.Y));
                if (Disciple.InfoSkill.MeTroi.IsMeTroi &&
                    Disciple.InfoSkill.MeTroi.DelayStart <= timeServer)
                {
                    SkillHandler.RemoveTroi(Disciple);
                }
                Disciple.InfoDelayDisciple.AutoMove = timeServer + ServerUtils.RandomNumber(10000, 20000);
            }
        }


        private void AutoPlusPoint(long timeServer)
        {
           
        }

        private void PlusPointOrignal(int type)
        {
            lock (Disciple.InfoChar)
            {
                var infoChar = Disciple.InfoChar;
                switch (type)
                {
                    case 0:
                        {
                            var x10 = 10 * (2 * (infoChar.OriginalHp + 1000) + 180) / 2;

                            if (infoChar.Potential >= x10)
                            {
                                infoChar.OriginalHp += (infoChar.HpFrom1000 * 10);
                                infoChar.Potential -= x10;
                            }
                            else
                            {
                                infoChar.OriginalHp += infoChar.HpFrom1000;
                                infoChar.Potential -= Disciple.PlusPoint.PointNext;
                            }
                            break;
                        }
                    case 1:
                        {
                            var x10 = 10 * (2 * (infoChar.OriginalMp + 1000) + 180) / 2;
                            if (infoChar.Potential >= x10)
                            {
                                infoChar.OriginalMp += (infoChar.MpFrom1000 * 10);
                                infoChar.Potential -= x10;
                            }
                            else
                            {
                                infoChar.OriginalMp += infoChar.MpFrom1000;
                                infoChar.Potential -= Disciple.PlusPoint.PointNext;
                            }
                            break;
                        }
                    case 2:
                        {
                            var x10 = 10 * (2 * infoChar.OriginalDamage + 9) / 2 * 100;
                            if (infoChar.Potential >= x10)
                            {

                                infoChar.OriginalDamage += (infoChar.DamageFrom1000 * 10);
                                infoChar.Potential -= x10;
                            }
                            else
                            {
                                infoChar.OriginalDamage += infoChar.DamageFrom1000;
                                infoChar.Potential -= Disciple.PlusPoint.PointNext;
                            }

                            break;
                        }
                    case 3:
                        {
                            infoChar.OriginalDefence += 1;
                            infoChar.Potential -= Disciple.PlusPoint.PointNext;
                            break;
                        }
                    case 4:
                        {
                            infoChar.OriginalCrit += 1;
                            infoChar.Potential -= Disciple.PlusPoint.PointNext;
                            break;
                        }
                }
            }
        }

        private void AutoAddSkill()
        {
            
        }

        private void SendChatForSp(string text)
        {
            if (Disciple.Status < 3)
            {
                Disciple.Character.CharacterHandler.SendMessage(Service.PublicChat(Disciple.Id, text));
            };
        }

        public void Close()
        {
            Clear();
        }

        public void Clear()
        {
            SuppressFinalize(this);
        }

        public void UpdateInfo(bool QueryItem = false)
        {
            SetUpInfo(QueryItem);
            SendZoneMessage(Service.UpdateBody(Disciple));
        }

        public void SetUpPosition(int mapOld = -1, int mapNew = -1, bool isRandom = false)
        {
            if (isRandom)
            {
                Disciple.InfoChar.X = (short)(Disciple.Character.InfoChar.X + 15);
            }
            else
            {
                Disciple.InfoChar.X = Disciple.Character.InfoChar.X;
            }
            Disciple.InfoChar.Y = Disciple.Character.InfoChar.Y;
            //Todo lỗi Touch không có map
            // if (Disciple.InfoChar.Y <= 10)
            // {
            //     Disciple.InfoChar.Y = Disciple.Zone.Map.TileMap.TouchY(Disciple.InfoChar.X, Disciple.InfoChar.Y);
            // }
        }

        public void SendInfo()
        {
            SetUpInfo(true);
        }

        public void SendDie()
        {
            lock (Disciple)
            {
                RemoveSkill(ServerUtils.CurrentTimeMillis(), true);
                Disciple.InfoChar.IsDie = true;
                Disciple.InfoSkill.Monkey.MonkeyId = 0;
                SetUpInfo();
                // SendZoneMessage(Service.UpdateBody(Disciple));
                SendZoneMessage(Service.PlayerDie(Disciple));
                Disciple.InfoDelayDisciple.LeaveDead = ServerUtils.CurrentTimeMillis() + 30000;
            }
        }

        public int GetParamItem(int id)
        {
            return Disciple.ItemBody.Where(item => item != null).Select(item => item.Options.Where(option => option.Id == id).ToList()).Select(option => option.Sum(optionItem => optionItem.Param)).Sum();
        }
        public void QueryItem()
        {
            var Milis = ServerUtils.CurrentTimeMillis();
            Disciple.InfoOption.Reset();
            Disciple.InfoSet.Reset();
            RestPlusHpMpFromDamage();
            Disciple.ItemBody.Where(item => item != null).ToList().ForEach(item =>
            {
                SetEnhancedOption(item, Milis, 0, true);
            });
        }
        public List<int> GetListParamItem(int id)
        {
            var param = new List<int>();
            foreach (var item in Disciple.ItemBody.Where(item => item != null))
            {
                var option = item.Options.Where(option => option.Id == id).ToList();
                param.AddRange(option.Select(optionItem => optionItem.Param));
            }
            return param;
        }
        public void RestPlusHpMpFromDamage()
        {
            Disciple.MpPlusFromDamage = 0;
            Disciple.HpPlusFromDamage = 0;
            Disciple.HpPlusFromDamageMonster = 0;
        }
        public void UpdateEffectTemporary(long timeServer)
        {

        }
        public void SetEnhancedOption(Model.Item.Item item, long Milis = 0, int type = 0, bool plusOption = false)
        {

            var ItemTemplate = ItemCache.ItemTemplate(item.Id);
            switch (ItemTemplate.Level)
            {
                case 13:
                    Disciple.InfoSet.CountSetThanLinh++;
                    if (Disciple.InfoSet.CountSetThanLinh >= 5) Disciple.InfoSet.IsFullSetThanLinh = true;
                    break;
                case 14:
                    Disciple.InfoSet.CountSetHuyDiet++;
                    if (Disciple.InfoSet.CountSetHuyDiet >= 5) Disciple.InfoSet.IsFullSetHuyDiet = true;
                    break;
                default:
                    break;
            }
            item.Options.ForEach(option =>
            {
                switch (option.Id)
                {
                    case 159:
                        Disciple.InfoOption.PercentPlusDameKamejoko += option.Param;
                        break;
                    case 34:
                        Disciple.InfoSet.CountSetTinhAn++;
                        if (Disciple.InfoSet.CountSetTinhAn >= 5) Disciple.InfoSet.IsFullSetTinhAn = true;
                        break;
                    case 35:
                        Disciple.InfoSet.CountSetNguyetAn++;
                        if (Disciple.InfoSet.CountSetNguyetAn >= 5) Disciple.InfoSet.IsFullSetTinhAn = true;
                        break;
                    case 36:
                        Disciple.InfoSet.CountSetNhatAn++;
                        if (Disciple.InfoSet.CountSetNhatAn >= 5) Disciple.InfoSet.IsFullSetNhatAn = true;
                        break;
                    case 127:
                        Disciple.InfoSet.CountSetThienXinHang++;
                        if (Disciple.InfoSet.CountSetThienXinHang >= 5) Disciple.InfoSet.IsFullSetThienXinHang = true;
                        break;
                    case 128:
                        Disciple.InfoSet.CountSetKirin++;
                        if (Disciple.InfoSet.CountSetKirin >= 5) Disciple.InfoSet.IsFullSetKirin = true;
                        break;
                   
                    case 129:
                        Disciple.InfoSet.CountSetSongoku++;
                        if (Disciple.InfoSet.CountSetSongoku >= 5) Disciple.InfoSet.IsFullSetSongoku = true;
                        break;
                    case 130:
                        Disciple.InfoSet.CountSetPicolo++;
                        if (Disciple.InfoSet.CountSetPicolo >= 5) Disciple.InfoSet.IsFullSetPicolo = true;
                        break;
                    case 131:
                        Disciple.InfoSet.CountSetOcTieu++;
                        if (Disciple.InfoSet.CountSetOcTieu >= 5) Disciple.InfoSet.IsFullSetOcTieu = true;
                        break;
                    case 132:
                        Disciple.InfoSet.CountSetPikkoro++;
                        if (Disciple.InfoSet.CountSetPikkoro >= 5) Disciple.InfoSet.IsFullSetPikkoro = true;
                        break;
                    case 133:
                        Disciple.InfoSet.CountSetKakarot++;
                        if (Disciple.InfoSet.CountSetKakarot >= 5) Disciple.InfoSet.IsFullSetKakarot = true;
                        break;

                    case 134:
                        Disciple.InfoSet.CountSetCadic++;
                        if (Disciple.InfoSet.CountSetCadic >= 5) Disciple.InfoSet.IsFullSetCadic = true;
                        break;
                    case 135:
                        Disciple.InfoSet.CountSetNappa++;
                        if (Disciple.InfoSet.CountSetNappa >= 5) Disciple.InfoSet.IsFullSetNappa = true;
                        break;
                    case 213:
                        Disciple.InfoSet.CountSetZelot++;
                        if (Disciple.InfoSet.CountSetZelot >= 5) Disciple.InfoSet.IsFullSetZelot = true;
                        break;
                    //-----------------------------------------------------------------------------------------//
                    case 80:
                        Disciple.InfoOption.PhanTramPlusHp30Second += option.Param;
                        break;
                    case 81:
                        Disciple.InfoOption.PhanTramPlusMp30Second += option.Param;
                        break;
                    case 2:
                        Disciple.InfoOption.HundredHPMP += option.Param;
                        break;
                    case 27:
                        Disciple.InfoOption.PlusHp30Second += option.Param;
                        break;
                    case 28:
                        Disciple.InfoOption.PlusMp30Second += option.Param;
                        break;
                    case 162:
                        Disciple.InfoOption.PlusMpEverySecond += option.Param;
                        break;
                    case 6:
                        Disciple.InfoOption.Hp += option.Param;
                        break;
                    case 22:
                        Disciple.InfoOption.ThoundsandHP += option.Param;
                        break;
                    case 23:
                        Disciple.InfoOption.ThoundsandMP += option.Param;
                        break;
                    case 48:
                        Disciple.InfoOption.HpMp += option.Param;
                        break;
                    case 0:
                        Disciple.InfoOption.Damage += option.Param;
                        break;
                    case 50:
                        Disciple.InfoOption.PhanTramDamage += option.Param;
                        break;
                    case 147:
                        Disciple.InfoOption.PhanTramDamage2 += option.Param;
                        break;
                    case 97:
                        Disciple.InfoOption.PhanPercentSatThuong += option.Param;
                        break;
                    case 98:
                        Disciple.InfoOption.PhanTramXuyenGiapChuong += option.Param;
                        break;
                    case 99:
                        Disciple.InfoOption.PhanTramXuyenGiapCanChien += option.Param;
                        break;
                    case 108:
                        Disciple.InfoOption.PhanTramNeDon += option.Param;
                        break;
                    case 100:
                        Disciple.InfoOption.PhanTramVangTuQuai += option.Param;
                        break;
                    case 101:
                        Disciple.InfoOption.PhanTramTNSM += option.Param;
                        break;
                    case 10:
                        Disciple.InfoOption.PercentChinhXac += option.Param;
                        break;
                    case 77:
                        Disciple.InfoOption.PhanTramHp += option.Param;
                        break;
                    case 5:
                        Disciple.InfoOption.PhanTramSatThuongChiMang += option.Param;
                        break;
                    case 3:
                        Disciple.InfoOption.PhanTramXuyenGiapChuong += option.Param;
                        break;
                   
                    case 103:
                        Disciple.InfoOption.PhanTramKi += option.Param;
                        break;
                    
                    case 155:
                        Disciple.InfoOption.X2TiemNang = true;
                        break;
                    case 95:
                        Disciple.HpPlusFromDamage += option.Param;
                        break;
                    case 96:
                        Disciple.MpPlusFromDamage += option.Param;
                        break;
                    case 14:
                    case 192:

                        Disciple.InfoOption.Crit += option.Param;
                        break;
                    case 104:
                        Disciple.HpPlusFromDamageMonster += option.Param;
                        break;
                    case 109:
                        Disciple.InfoOption.PhanTramGiamHp += option.Param;
                        break;
                    case 47:
                        Disciple.InfoOption.Defence += option.Param;
                        break;
                    case 94:
                        Disciple.InfoOption.PhanTramDefence += option.Param;
                        break;
                    case 148:
                    case 114:
                    case 16:
                        Disciple.InfoOption.PhanTramSpeed += option.Param;
                        break;
                    case 7:
                        Disciple.InfoOption.Ki += option.Param;
                        break;
                    case 178:
                        Disciple.InfoOption.PhanTramPlusMp10Second += option.Param;
                        break;
                    case 93:
                        var expireTimeOption = item.Options.FirstOrDefault(option2 => (option2.Id == 73));
                        if (expireTimeOption != null)
                        {
                            if (expireTimeOption.Param < Milis)
                            {
                                switch (type)
                                {
                                    case 0://body
                                        {
                                            RemoveItemBody(item.IndexUI);
                                            break;
                                        }
                                    case 1:
                                        {
                                            RemoveItemBagByIndex(item.IndexUI, item.Quantity, false, reason: "Item hết hạn sử dụng");
                                            break;
                                        }
                                    case 2:
                                        {
                                            RemoveItemBoxByIndex(item.IndexUI, item.Quantity, false);
                                            break;
                                        }
                                    case 3:
                                        {
                                            RemoveItemLuckyBox(item.IndexUI, false);
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                // tính ngày từ giây
                                var leftTime = expireTimeOption.Param - Milis;
                                option.Param = ServerUtils.ConvertMilisToDay(leftTime);
                            }
                        }
                        break;
                }
            });
        }
        public void SetUpInfo(bool queryItem = false)
        {
            if (queryItem) QueryItem();
            SetHpFull();
            SetMpFull();
            SetDamageFull();
            SetDefenceFull();
            SetCritFull();
            SetSpeed();

            SetBuffMp1s();
            SetBuffHp5s();
            SetBuffHp10s();
            SetBuffHp30s();
            // SetEnhancedOption();
        }
        public void UpdateEffective()
        {

        }
        public Model.Item.Item GetItemBodyByIndex(int index)
        {
            return Disciple.ItemBody.FirstOrDefault(item => item.IndexUI == index);
        }
        public int GetCountItemLevel(byte level)
        {
            var count = 0;
            var itemsBody = CollectionsMarshal.AsSpan(Disciple.ItemBody.Where(item => item != null).ToList());
            for (int i = 0; i < itemsBody.Length; i++)
            {
                var item = itemsBody[i];
                if (ItemCache.ItemTemplate(item.Id).Level == level) count++;

            }
            return count;
        }
        public void SetInfoSet()
        {
            Disciple.InfoSet.Reset();
            Disciple.InfoSet.IsFullSetThanLinh = GetCountItemLevel(13) >= 5;
            Disciple.InfoSet.IsFullSetHuyDiet = GetCountItemLevel(14) >= 5;
            Disciple.InfoSet.IsFullSetTinhAn = GetParamItemExistCount(34) >= 5;
            Disciple.InfoSet.IsFullSetNguyetAn = GetParamItemExistCount(35) >= 5;
            Disciple.InfoSet.IsFullSetNhatAn = GetParamItemExistCount(36) >= 5;
            switch (Disciple.InfoChar.Gender)
            {
                case 0:
                    {
                        Disciple.InfoSet.IsFullSetThienXinHang = GetParamItemExistCount(127) >= 5;
                        Disciple.InfoSet.IsFullSetKirin = GetParamItemExistCount(128) >= 5;
                        Disciple.InfoSet.IsFullSetSongoku = GetParamItemExistCount(129) >= 5;
                        break;
                    }
                case 1:
                    {
                        Disciple.InfoSet.IsFullSetPicolo = GetParamItemExistCount(130) >= 5;
                        Disciple.InfoSet.IsFullSetOcTieu = GetParamItemExistCount(131) >= 5;
                        Disciple.InfoSet.IsFullSetPikkoro = GetParamItemExistCount(132) >= 5;
                        Disciple.InfoSet.IsFullSetZelot = GetParamItemExistCount(213) >= 5;
                    }
                    break;
                case 2:
                    {
                        Disciple.InfoSet.IsFullSetKakarot = GetParamItemExistCount(133) >= 5;
                        Disciple.InfoSet.IsFullSetCadic = GetParamItemExistCount(134) >= 5;
                        Disciple.InfoSet.IsFullSetNappa = GetParamItemExistCount(135) >= 5;
                    }
                    break;
            }
        }
        public int GetParamItemExistCount(int id)
        {
            var count = 0;
            var itemsBody = CollectionsMarshal.AsSpan(Disciple.ItemBody.Where(item => item != null).ToList());
            for (int i = 0; i < itemsBody.Length; i++)
            {
                var item = itemsBody[i];
                if (item.isHaveOption(id)) count++;

            }
            return count;
        }

        public Model.Item.Item RemoveItemGiftBox(int index, bool isReset = true)
        {
            return null;

        }
        public void SetHpFull()
        {
            var hp = Disciple.InfoChar.OriginalHp;
            hp += Disciple.InfoOption.HundredHPMP * 100;
            hp += Disciple.InfoOption.Hp;
            hp += Disciple.InfoOption.ThoundsandHP * 1000;
            hp += Disciple.InfoOption.HpMp;
            hp += hp * Disciple.InfoOption.PhanTramHp / 100;
            hp -= hp * (Disciple.InfoOption.PhanTramGiamHp / 100);

            // Nappa
            if (Disciple.InfoSet.IsFullSetNappa)
            {
                hp += hp * 80 / 100;
            }
            if (Disciple.InfoSet.IsFullSetNguyetAn)
            {
                hp += hp * 30 / 100;
            }
            //if (Disciple.Character.InfoChar.isColer)
            //{
            //    hp -= hp / 2;
            //}
            if (Disciple.InfoSkill.Monkey.MonkeyId != 0) hp += hp * Disciple.InfoSkill.Monkey.Hp / 100;
            if (Disciple.InfoSkill.HuytSao.IsHuytSao) hp += hp * Disciple.InfoSkill.HuytSao.Percent / 100;

            Disciple.HpFull = hp;
        }

        public void SetMpFull()
        {
            var mp = Disciple.InfoChar.OriginalMp;
            mp += Disciple.InfoOption.HundredHPMP * 100;
            mp += Disciple.InfoOption.Ki;
            mp += Disciple.InfoOption.ThoundsandMP * 1000;
            mp += Disciple.InfoOption.HpMp;
            mp += Disciple.InfoOption.PhanTramKi * mp / 100;
            if (Disciple.InfoSet.IsFullSetNhatAn)
            {
                mp += mp * 30 / 100;
            }
            Disciple.MpFull = mp;
            //if (Disciple.Character.InfoChar.isColer)
            //{
            //    mp -= mp / 2;
            //}
        }

        public void SetDamageFull()
        {
            var damage = Disciple.InfoChar.OriginalDamage;
            damage += (int)Disciple.InfoOption.Damage;
            damage += Disciple.InfoOption.PhanTramDamage * damage / 100;
            damage += Disciple.InfoOption.PhanTramDamage2 * damage / 100;
            if (Disciple.InfoSkill.Monkey.MonkeyId != 0) damage += damage * Disciple.InfoSkill.Monkey.Damage / 100;
            if (Disciple.InfoSet.IsFullSetTinhAn)
            {
                damage += damage * 30 / 100;
            }
            //if (Disciple.Character.InfoChar.isColer)
            //{
            //    damage -= damage / 2;
            //}
            Disciple.DamageFull = damage;
        }

        public void SetDefenceFull()
        {
            var defence = Disciple.InfoChar.OriginalDefence * 4;
            defence += Disciple.InfoOption.Defence;
            defence += defence * (Disciple.InfoOption.PhanTramDefence / 100);
            Disciple.DefenceFull = Math.Abs(defence);
        }

        public void SetCritFull()
        {
            int crtCal;
            if (Disciple.InfoSkill.Monkey.MonkeyId != 0)
            {
                crtCal = 115;
            }
            else
            {
                crtCal = Disciple.InfoChar.OriginalCrit;
                crtCal += Disciple.InfoOption.Crit;
            }
            Disciple.CritFull = crtCal;
        }

        public void SetHpPlusFromDamage()
        {

        }

        public void SetMpPlusFromDamage()
        {

        }

        public void SetSpeed()
        {
            var speed = 5;
            if (Disciple.InfoSkill.Monkey.MonkeyId != 0) speed = 7;
            var plus = speed * (Disciple.InfoOption.PhanTramSpeed) / 100;
            switch (plus)
            {
                case <= 1:
                    speed += 1;
                    break;
                case > 1 and <= 2:
                    speed += 2;
                    break;
                case > 2:
                    speed += plus;
                    break;
            }
            Disciple.InfoChar.Speed = (sbyte)speed;
        }


        public void SetBuffHp30s()
        {
            var hpPlus = Disciple.InfoOption.PlusHp30Second;
            hpPlus += hpPlus * (Disciple.InfoOption.PhanTramPlusHp30Second / 100);
            Disciple.Effect.BuffHp30S.Value = hpPlus;
            if (Disciple.Effect.BuffHp30S.Time == -1)
            {
                Disciple.Effect.BuffHp30S.Time = 30000 + ServerUtils.CurrentTimeMillis();
            }

        }
        public void SetBuffMp30s()
        {
            var mpPlus = Disciple.InfoOption.PlusMp30Second;
            mpPlus += mpPlus * (Disciple.InfoOption.PhanTramPlusMp30Second / 100);
            Disciple.Effect.BuffKi30S.Value = mpPlus;
            if (Disciple.Effect.BuffKi30S.Time == -1)
            {
                Disciple.Effect.BuffKi30S.Time = 30000 + ServerUtils.CurrentTimeMillis();
            }

        }

        public void SetBuffMp1s()
        {
            var mpPlus = (int)Disciple.MpFull * Disciple.InfoOption.PlusMpEverySecond / 100;
            Disciple.Effect.BuffKi1s.Value = mpPlus;
            if (Disciple.Effect.BuffKi1s.Time == -1)
            {
                Disciple.Effect.BuffKi1s.Time = 1500 + ServerUtils.CurrentTimeMillis();
            }
        }
        public void SetBuffHp5s()
        {
            //TODO set buff 5s
        }

        public void SetBuffHp10s()
        {
            //TODO set buff 10s
        }

        public void MoveMap(short toX, short toY, int type = 0)
        {
            var timeServer = ServerUtils.CurrentTimeMillis();
            if (Disciple.IsDontMove()) return;

            var compare = Math.Abs(Disciple.InfoChar.X - toX);
            if (compare >= 50)
            {
                Disciple.IsFire = false;
                Disciple.InfoDelayDisciple.Fire = timeServer + 1500;
                if (Disciple.InfoChar.X < toX)
                {
                    Disciple.InfoChar.X = compare switch
                    {
                        >= 150 => (short)(toX - 70),
                        _ => (short)(toX - 50)
                    };
                }
                else
                {
                    Disciple.InfoChar.X = compare switch
                    {
                        >= 150 => (short)(toX + 70),
                        _ => (short)(toX + 50)
                    };
                }

                if (toY != Disciple.InfoChar.Y)
                {
                    Disciple.InfoChar.Y = toY;
                }

                SendZoneMessage(Service.PlayerMove(Disciple.Id, Disciple.InfoChar.X, Disciple.InfoChar.Y));
                if (Disciple.InfoSkill.MeTroi.IsMeTroi && Disciple.InfoSkill.MeTroi.DelayStart <= timeServer)
                {
                    SkillHandler.RemoveTroi(Disciple);
                }
            }
        }

        public void PlusHp(int hp)
        {
            lock (Disciple.InfoChar)
            {
                if (Disciple.InfoChar.IsDie) return;
                Disciple.InfoChar.Hp += hp;
                if (Disciple.InfoChar.Hp >= Disciple.HpFull) Disciple.InfoChar.Hp = Disciple.HpFull;
            }
        }

        public void MineHp(long hp)
        {
            lock (Disciple.InfoChar)
            {
                if (Disciple.InfoChar.IsDie || hp <= 0) return;
                if (hp > Disciple.InfoChar.Hp)
                {
                    Disciple.InfoChar.Hp = 0;
                }
                else
                {
                    Disciple.InfoChar.Hp -= hp;
                }

                if (Disciple.InfoChar.Hp <= 0)
                {
                    Disciple.InfoChar.IsDie = true;
                    Disciple.InfoChar.Hp = 0;
                }
            }
        }

        public void PlusMp(int mp)
        {
            lock (Disciple.InfoChar)
            {
                if (Disciple.InfoChar.IsDie) return;
                Disciple.InfoChar.Mp += mp;
                if (Disciple.InfoChar.Mp >= Disciple.MpFull) Disciple.InfoChar.Mp = Disciple.MpFull;
            }
        }

        public void MineMp(int mp)
        {
            lock (Disciple.InfoChar)
            {
                if (Disciple.InfoChar.IsDie || mp < 0) return;
                Disciple.InfoChar.Mp -= mp;
                if (Disciple.InfoChar.Mp <= 0) Disciple.InfoChar.Mp = 0;
            }
        }

        public void PlusStamina(int stamina)
        {
            lock (Disciple.InfoChar)
            {
                Disciple.InfoChar.Stamina += (short)stamina;
                if (Disciple.InfoChar.Stamina > 1250) Disciple.InfoChar.Stamina = 1250;
            }
        }

        public void MineStamina(int stamina)
        {
            lock (Disciple.InfoChar)
            {
                if (stamina < 0) return;
                Disciple.InfoChar.Stamina -= (short)stamina;
                if (Disciple.InfoChar.Stamina <= 0) Disciple.InfoChar.Stamina = 0;
            }
        }

        public void PlusPower(long power)
        {
            lock (Disciple.InfoChar)
            {
                Disciple.InfoChar.Power += power;
                Disciple.InfoChar.Level = (sbyte)(Cache.Gi().EXPS.Count(exp => exp < Disciple.InfoChar.Power) - 1);
                if (Cache.Gi().LIMIT_POWERS[Disciple.InfoChar.LitmitPower].Power > Disciple.InfoChar.Power)
                {
                    Disciple.InfoChar.IsPower = true;
                }
                else
                {
                    Disciple.InfoChar.IsPower = false;
                }
            }
        }

        public void PlusPotential(long potential)
        {
            lock (Disciple.InfoChar)
            {
                Disciple.InfoChar.Potential += potential;
            }
        }

        public Model.Item.Item RemoveItemBody(int index)
        {
            Model.Item.Item item;
            lock (Disciple.ItemBody)
            {
                item = Disciple.ItemBody[index];
                if (item == null) return null;
                Disciple.ItemBody[index] = null;
                UpdateInfo(true);
            }
            return item;
        }

        public void AddItemToBody(Model.Item.Item item, int index)
        {
            if (item == null) return;
            item.IndexUI = index;
            Disciple.ItemBody[index] = item;
        }

        public void RemoveMonsterMe()
        {
            var skillEgg = Disciple.InfoSkill.Egg;
            if (skillEgg.Monster is { IsDie: true })
            {
                SendZoneMessage(Service.UpdateMonsterMe7(skillEgg.Monster.Id));
                Disciple.Zone.ZoneHandler.RemoveMonsterMe(skillEgg.Monster.Id);
                SkillHandler.RemoveMonsterPet(Disciple);
            }
        }

        public void PlusTiemNang(IMonster monster, int damage)
        {
            if (monster.IsMobMe && monster.Character != null) return;
            if (damage <= 0) return;

            long fixDmg = (long)((damage) + (monster.OriginalHp * 0.00125));
            long damagePlusPoint = fixDmg;

            if (damagePlusPoint <= 0)
            {
                damagePlusPoint = 2;
            }

            if (monster.Id != 0)
            {
                var levelChar = Disciple.InfoChar.Level;
                var levelMonster = monster.Level;
                var checkLevel = Math.Abs(levelChar - levelMonster);
                if ((checkLevel > 5 && levelChar > levelMonster) || (levelMonster > levelChar && levelMonster - levelChar > 5))
                {
                    damagePlusPoint = 1;
                }
                else if (monster.Id is (77 or 70 or 85 or 72))
                {
                    damagePlusPoint = 1;
                }


            }

            switch (Disciple.Flag)
            {
                case 0: break;
                case 8:
                    damagePlusPoint += damagePlusPoint * 10 / 100;
                    break;
                default:
                    damagePlusPoint += damagePlusPoint * 5 / 100;
                    break;
            }

            if (DatabaseManager.ConfigManager.gI().ExpUp > 1)
            {
                if (Disciple.InfoChar.Power < DatabaseManager.ConfigManager.gI().LimitPowerExpUp)
                {
                    damagePlusPoint *= DatabaseManager.ConfigManager.gI().ExpUp;
                }
                else
                {
                    damagePlusPoint *= (DatabaseManager.ConfigManager.gI().ExpUp / 2);
                }
            }

            
            damagePlusPoint += damagePlusPoint * Disciple.Character.InfoOption.PercentPlusPotenialForDisciple / 100;
            if (Disciple.Character.InfoChar.Power > DatabaseManager.ConfigManager.gI().LimitPowerExpUp)
            {
                if (Disciple.Character.InfoChar.IsPower)
                {
                    Disciple.Character.CharacterHandler.PlusTiemNang((long)(damagePlusPoint / 5), (long)(damagePlusPoint / 2), true);
                }
                else
                {
                    Disciple.Character.CharacterHandler.PlusTiemNang(0, (long)(damagePlusPoint / 2), false);
                }
            }
            else
            {
                if (Disciple.Character.InfoChar.IsPower)
                {
                    Disciple.Character.CharacterHandler.PlusTiemNang((long)(damagePlusPoint / 1.5), (long)(damagePlusPoint / 1.5), true);
                }
                else
                {
                    Disciple.Character.CharacterHandler.PlusTiemNang(0, (long)(damagePlusPoint / 1.5), false);
                }
            }
        }

        public void PlusTiemNang(long power, long potential, bool isAll)
        {
           
            if (isAll && power > 0 && potential > 0)
            {
                PlusPower(power);
                PlusPotential(potential);
            }
            else
            {
                if (power > 0)
                {
                    PlusPower(power);
                }

                if (potential > 0)
                {
                    PlusPotential(potential);
                }
            }
        }

        public void LeaveFromDead(bool isHeal = false)
        {
            lock (Disciple)
            {
                UpdateInfo();
                Disciple.InfoChar.IsDie = false;
                Disciple.InfoChar.Hp = Disciple.HpFull;
                Disciple.InfoChar.Mp = Disciple.MpFull;
                SendZoneMessage(Service.ReturnPointMap(Disciple));
                SendZoneMessage(Service.PlayerLoadLive(Disciple));
            }
        }

        public void RemoveSkill(long timeServer, bool globalReset = false)
        {
            var infoSkill = Disciple.InfoSkill;
            if ((infoSkill.TaiTaoNangLuong.IsTTNL &&
                 infoSkill.TaiTaoNangLuong.DelayTTNL <= timeServer) || globalReset)
            {
                SkillHandler.RemoveTTNL(Disciple);
            }

            if (infoSkill.Monkey.MonkeyId == 1 && infoSkill.Monkey.TimeMonkey <= timeServer || globalReset)
            {
                SkillHandler.HandleMonkey(Disciple, false);
            }

            if (infoSkill.Protect.IsProtect && infoSkill.Protect.Time <= timeServer || globalReset)
            {
                SkillHandler.RemoveProtect(Disciple);
            }

            if (infoSkill.HuytSao.IsHuytSao && infoSkill.HuytSao.Time <= timeServer)
            {
                SkillHandler.RemoveHuytSao(Disciple);
            }

            if (infoSkill.PlayerTroi.IsPlayerTroi || globalReset)
            {
                if (globalReset && infoSkill.PlayerTroi.IsPlayerTroi)
                {
                    infoSkill.PlayerTroi.PlayerId.ForEach(i => SkillHandler.RemoveTroi(Disciple.Zone.ZoneHandler.GetCharacter(i)));
                }
            }

            if (infoSkill.DichChuyen.IsStun && infoSkill.DichChuyen.Time <= timeServer || globalReset)
            {
                SkillHandler.RemoveDichChuyen(Disciple);
            }

            if (infoSkill.ThaiDuongHanSan.IsStun && infoSkill.ThaiDuongHanSan.Time <= timeServer || globalReset)
            {
                SkillHandler.RemoveThaiDuongHanSan(Disciple);
            }

            if (infoSkill.ThoiMien.IsThoiMien && infoSkill.ThoiMien.Time <= timeServer || globalReset)
            {
                SkillHandler.RemoveThoiMien(Disciple);
            }

            if (infoSkill.Socola.IsSocola && infoSkill.Socola.Time <= timeServer || globalReset)
            {
                SkillHandler.RemoveSocola(Disciple);
            }
        }

        public void UpdateEffect(long timeServer)
        {
            var effect = Disciple.Effect;
            if (effect.BuffHp30S.Value > 0 && effect.BuffHp30S.Time <= timeServer && Disciple.InfoChar.Hp < Disciple.HpFull)
            {
                PlusHp(effect.BuffHp30S.Value);
                SendZoneMessage(Service.PlayerLevel(Disciple));
                effect.BuffHp30S.Time = 30000 + timeServer;
            }

            if (effect.BuffKi1s.Value > 0 && effect.BuffKi1s.Time <= timeServer && Disciple.InfoChar.Mp < Disciple.MpFull)
            {
                PlusMp(effect.BuffKi1s.Value);
                effect.BuffKi1s.Time = 1500 + timeServer;
            }
        }

        public void UpdateMask()
        {
            var item = Disciple.ItemBody[5];
            if (item == null) return;
            /*switch (item.Id)
            {
                //TODO handle logic for mask
            }*/
        }

        public void UpdateAutoPlay(long timeServer)
        {

        }

        public void UpdateLuyenTap()
        {

        }

        public void RemoveTroi(int charId)
        {
            var infoSkill = Disciple.InfoSkill.PlayerTroi;
            if (infoSkill.IsPlayerTroi)
            {
                infoSkill.PlayerId.RemoveAll(i => i == charId);
                if (infoSkill.PlayerId.Count <= 0)
                {
                    infoSkill.IsPlayerTroi = false;
                    infoSkill.TimeTroi = -1;
                    infoSkill.PlayerId.Clear();
                    SendZoneMessage(Service.SkillEffectPlayer(charId, Disciple.Id, 2, 32));
                }
            }
        }

        #region Ignored Function
        public void SetPlayer(Player player)
        {
            //Set player
        }

        public void SendMessage(Message message)
        {
            //ignore
        }

        public void SendMeMessage(Message message)
        {
            //ignore
        }
        public void HandleJoinMap(IZone zone)
        {
            //Disciple join map
        }

        public void BagSort()
        {
            //ignore
        }

        public void BoxSort()
        {
            //ignore
        }
        public void Upindex(int index)
        {
            //ignore
        }
        public bool AddItemToBag(bool isUpToUp, Model.Item.Item item, string reason = "")
        {
            //ignore
            return false;
        }

        public bool AddItemToBox(bool isUpToUp, Model.Item.Item item)
        {
            //ignore
            return false;
        }

        public void ClearTest()
        {
            //Clear DoanhTrai
        }

        public void DropItemBody(int index)
        {
            //ignore
        }

        public void DropItemBag(int index)
        {
            //ignore
        }

        public void PickItemMap(short id)
        {
            //ignore
        }

        public void UpdateMountId()
        {
            //ignore
        }
        public void UpdatePhukien()
        {
            //ignore
        }
        public Model.Item.Item GetItemBagByIndex(int index)
        {
            //ignore
            return null;
        }

        public Model.Item.Item GetItemBagById(int id)
        {
            //ignore
            return null;
        }

        public Model.Item.Item GetItemBoxByIndex(int index)
        {
            //ignore
            return null;
        }
        public Model.Item.Item GetItemLuckyBoxByIndex(int index)
        {
            //ignore
            return null;
        }
        public Model.Item.Item GetItemBoxById(int id)
        {
            //ignore
            return null;
        }


        public void BackHome()
        {
            //Ignore
        }

        public void RemoveItemBagById(short id, int quantity, string reason = "")
        {
            //ignore
        }

        public void RemoveItemBagByIndex(int index, int quantity, bool reset = true, string reason = "")
        {
            //ignore
        }

        public void RemoveItemBoxByIndex(int index, int quantity, bool reset = true)
        {
            //ignore
        }

        public Model.Item.Item RemoveItemBag(int index, bool isReset = true, string reason = "")
        {
            return null;
        }



        public Model.Item.Item ItemBagNotMaxQuantity(short id)
        {
            //ignore
            return null;
        }

        public Model.Item.Item RemoveItemBox(int index, bool isReset = true)
        {
            return null;
        }
        public Model.Item.Item RemoveItemLuckyBox(int index, bool isReset = true)
        {
            return null;
        }

        public void SetUpFriend()
        {
            //Ignore
        }

        public void LeaveItem(ICharacter character)
        {
            // Ignore
        }

        #endregion
    }
}