using NgocRongGold.Application.Handlers.Skill;
using NgocRongGold.Application.Interfaces.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NgocRongGold.Model.Item;
using NgocRongGold.Application.IO;
using NgocRongGold.Model.SkillCharacter;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Manager;
using NgocRongGold.Model.Map;
using NgocRongGold.Application.Interfaces.Monster;
using NgocRongGold.Model.Character;
using NgocRongGold.Model;
using Application.Interfaces.Zone;

namespace NgocRongGold.Application.Extension.Bosses.Xinbato
{
    public class XinbatoHandler : ICharacterHandler
    {
        public int GetAllQuantityItemBagById(int id)
        {
            return 0;
        }
        public void SetupAmulet()
        {

        }
        public void SetEnhancedOptionCard()
        {

        }
        public void UpdateEffectTemporary(long timeServer)
        {

        }   
        public void QueryItem()
        {

        }
        public Item RemoveItemGiftBox(int index, bool isReset = true)
        {
            return null;

        }
        public void UpdateOther(long timeServer)
        {
        }
        public Boss Boss { get; set; }
        public Item GetItemClanBoxByIndex(int index)
        {
            return null;
        }
        public Model.Item.Item RemoveItemClanBox(int index, bool isReset = true)
        {
            return null;

        }
        public void OpenUiSay(string say)
        {

        }
        public void SendServerMessage(string say)
        {

        }
        public void CreatePetNormal()
        {

        }
        public void UpdatePet()
        {
            //ingored
        }
        public void UpdateItem10()
        {
            //ingored
        }
        public XinbatoHandler(Boss boss)
        {
            Boss = boss;
        }
        public void PlusDiamondLock(int diamond)
        {
            //ingored
        }
        public void SetUpPhoBan()
        {

        }
        public int GetThoiVangInRuong()
        {
            return -1;
            //ingored
        }
        public int GetThoiVangInBag()
        {
            return -1;
            //ingored
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void SendZoneMessage(Message message)
        {
            Boss?.Zone?.ZoneHandler.SendMessage(message);
        }
        public void Update()
        {
            lock (Boss)
            {
                var timeServer = ServerUtils.CurrentTimeMillis();



                if (!Boss.InfoChar.IsDie)
                {
                    
                    AutoBoss(timeServer);
                }
                if (Boss.InfoChar.IsDie && Boss.InfoDelayBoss.LeaveDead <= timeServer)
                {
                    Boss.InfoDelayBoss.LeaveDead = -1;
                    LeaveFromDead();
                }
            }
        }

        private void AutoBoss(long timeServer)
        {
            if (Boss.IsDontMove()) return;

            AutoMoveMap(timeServer);
        }

        private void HandleBossAction()
        {
           
        }

       
        #region boss style attack
        private void HandleUseSkill(ICharacter character)
        {



        }

        private void AttackPlayer(ICharacter character, SkillCharacter skillChar)
        {
           
        }
        #endregion

        private void AutoMoveMap(long timeServer)
        {
            if (Boss.InfoDelayBoss.AutoMove <= timeServer)
            {
                Boss.InfoChar.X = (short)ServerUtils.RandomNumber(Boss.BasePositionX - 50,
                    Boss.BasePositionX + 50);
                SendZoneMessage(Service.PlayerMove(Boss.Id, Boss.InfoChar.X, Boss.InfoChar.Y));
                
                Boss.InfoDelayBoss.AutoMove = timeServer + ServerUtils.RandomNumber(2000, 4000);
            }

            if (Boss.InfoDelayBoss.AutoChat <= timeServer)
            {
                IdleChatMessage(timeServer);
            }
           
        }

        private void AutoChangeMap(long timeServer)
        {
            
        }

        private void SendBossChat(string text)
        {
            
                SendZoneMessage(Service.PublicChat(Boss.Id, text));
            
        }

        private void CombatChatMessage(long timeServer)
        {            
        }
       

        private void IdleChatMessage(long timeServer)
        {
            
            SendBossChat(TextServer.gI().XINBATO_CHAT_IDLE[ServerUtils.RandomNumber(TextServer.gI().XINBATO_CHAT_IDLE.Count)]);
            Boss.InfoDelayBoss.AutoChat = timeServer + ServerUtils.RandomNumber(3000, 5000);
            Boss.Zone.ZoneHandler.SendMessage(Service.ServerMessage("Hãy tìm đủ 99 bình nước và tìm Xinbato để cho"));
        }

        private void DieChatMessage()
        {
           
              
            SendBossChat(TextServer.gI().XINBATO_CHAT_IDLE[ServerUtils.RandomNumber(TextServer.gI().XINBATO_CHAT_IDLE.Count)]);
                  
        }

        private bool CheckNearWaypoint(ICharacter character)
        {
            var checkWP = MapManager.Get(character.InfoChar.MapId)?.TileMap.WayPoints.FirstOrDefault(waypoint => CheckTrueWaypoint(character, waypoint));
            if (checkWP != null)
            {
                return true;
            }
            return false;
        }

        private bool CheckTrueWaypoint(ICharacter character, WayPoint waypoint, int size = 0)
        {
            if (waypoint.IsEnter)
            {
                return character.InfoChar.X >= waypoint.MinX - size && character.InfoChar.X <= waypoint.MaxX + size &&
                       character.InfoChar.Y <= waypoint.MaxY && character.InfoChar.Y >= waypoint.MinY;
            }

            if (waypoint.MinX == 0)
            {
                return character.InfoChar.X <= waypoint.MaxX + 100 + size && character.InfoChar.Y <= waypoint.MaxY &&
                       character.InfoChar.Y >= waypoint.MinY;
            }

            return character.InfoChar.X >= waypoint.MinX - size && character.InfoChar.Y <= waypoint.MaxY &&
                   character.InfoChar.Y >= waypoint.MinY;
        }

        public void Close()
        {
            Clear();
        }

        public void Clear()
        {
            GC.SuppressFinalize(this);
        }

        public void UpdateInfo(bool QueryItem = false)
        {
            SetUpInfo();
            SendZoneMessage(Service.UpdateBody(Boss));
        }

        public void SetUpPosition(int mapOld = -1, int mapNew = -1, bool isRandom = false)
        {
            // PositionHandler.SetUpPosition(Boss, mapOld, mapNew);
            if (Boss.BasePositionX != 0 && Boss.BasePositionY != 0) return;
            switch (mapNew)
            {
                case 166:
                    Boss.BasePositionX = 540;
                    Boss.BasePositionY = 816;
                    break;
                case 5://Đảo Kame
                    {
                        Boss.BasePositionX = 593;
                        Boss.BasePositionY = 384;
                        break;
                    }
                case 29://Nam Kame
                    {
                        Boss.BasePositionX = 1006;
                        Boss.BasePositionY = 360;
                        break;
                    }
                case 13://Đảo Guru
                    {
                        Boss.BasePositionX = 1230;
                        Boss.BasePositionY = 384;
                        break;
                    }
                case 20://Vách núi đen
                    {
                        Boss.BasePositionX = 533;
                        Boss.BasePositionY = 360;
                        break;
                    }
                case 33://Nam Guru
                    {
                        Boss.BasePositionX = 973;
                        Boss.BasePositionY = 288;
                        break;
                    }
                case 44:
                case 42:
                case 43:
                    Boss.BasePositionX = 450;
                    Boss.BasePositionY = 432;
                    break;
                case 36://Rừng đá
                    {
                        Boss.BasePositionX = 696;
                        Boss.BasePositionY = 288;
                        break;
                    }
                case 38://Bờ vực đen
                    {
                        Boss.BasePositionX = 516;
                        Boss.BasePositionY = 240;
                        break;
                    }
                case 68://Thung lũng Nappa
                    {
                        Boss.BasePositionX = 920;
                        Boss.BasePositionY = 312;
                        break;
                    }
                case 69://Vực cấm
                    {
                        Boss.BasePositionX = 773;
                        Boss.BasePositionY = 384;
                        break;
                    }
                case 70://Núi Appule
                    {
                        Boss.BasePositionX = 800;
                        Boss.BasePositionY = 360;
                        break;
                    }
                case 71://Căn cứ Ras
                    {
                        Boss.BasePositionX = 526;
                        Boss.BasePositionY = 624;
                        break;
                    }
                case 72://Thung lũng Ras
                    {
                        Boss.BasePositionX = 964;
                        Boss.BasePositionY = 312;
                        break;
                    }
                case 92://Thành phố phía đông
                    {
                        Boss.BasePositionX = 704;
                        Boss.BasePositionY = 360;
                        break;
                    }
                case 93://Thành phố phía nam
                    {
                        Boss.BasePositionX = 784;
                        Boss.BasePositionY = 360;
                        break;
                    }
                case 79:
                    {
                        Boss.BasePositionX = 232;
                        Boss.BasePositionY = 360;

                    }
                    break;

                case 94:
                    Boss.BasePositionX = 910;
                    Boss.BasePositionY = 384;
                    break;
                case 96:
                    Boss.BasePositionX = 696;
                    Boss.BasePositionY = 288;
                    break;
                case 97:
                    Boss.BasePositionX = 558;
                    Boss.BasePositionY = 384;
                    break;
                case 98:
                    Boss.BasePositionX = 379;
                    Boss.BasePositionY = 312;
                    break;
                case 82:
                    Boss.BasePositionX = 691;
                    Boss.BasePositionY = 384;
                    break;
                case 83:
                    Boss.BasePositionX = 578;
                    Boss.BasePositionY = 144;
                    break;
                case 103: //võ đài xên bọ hung
                    {
                        Boss.BasePositionX = 414;
                        Boss.BasePositionY = 288;
                        break;
                    }
                case 107:
                    {
                        Boss.BasePositionX = 401;
                        Boss.BasePositionY = 696;
                        break;
                    }
                case 108:
                    {
                        Boss.BasePositionX = 445;
                        Boss.BasePositionY = 360;
                        break;
                    }
                case 110:
                    {
                        Boss.BasePositionX = 438;
                        Boss.BasePositionY = 432;
                        break;
                    }
                case 161://HTTV
                    {
                        Boss.BasePositionX = 831;
                        Boss.BasePositionY = 144;
                        break;
                    }
                case 154:
                    Boss.BasePositionX = 404;
                    Boss.BasePositionY = 360;
                    break;
                case 4:
                    Boss.BasePositionX = 516;
                    Boss.BasePositionY = 336;
                    break;
                case 3:
                    Boss.BasePositionX = 661;
                    Boss.BasePositionY = 408;
                    break;
                case 27:
                    Boss.BasePositionX = 719;
                    Boss.BasePositionY = 336;
                    break;
                case 28:
                    Boss.BasePositionX = 416;
                    Boss.BasePositionY = 360;
                    break;
                case 30:
                    Boss.BasePositionX = 346;
                    Boss.BasePositionY = 288;
                    break;
                case 6:
                    Boss.BasePositionX = 346;
                    Boss.BasePositionY = 336;
                    break;
                case 10:
                    Boss.BasePositionX = 696;
                    Boss.BasePositionY = 288;
                    break;
                case 45:
                    Boss.BasePositionX = 301;
                    Boss.BasePositionY = 408;
                    break;
                case 46:
                    Boss.BasePositionX = 300;
                    Boss.BasePositionY = 408;
                    break;
                case 48:
                    Boss.BasePositionX = 315;
                    Boss.BasePositionY = 240;
                    break;
                case 49:
                    Boss.BasePositionX = 829;
                    Boss.BasePositionY = 456;
                    break;
                default:
                    {
                        Boss.BasePositionX = (short)ServerUtils.RandomNumber(250, 450);
                        Boss.BasePositionY = 0;
                        break;
                    }
            }
            Boss.InfoChar.X = Boss.BasePositionX;
            Boss.InfoChar.Y = Boss.BasePositionY;
            SendZoneMessage(Service.SendPos(Boss, 1));

        }

        public void SendInfo()
        {
            SetUpInfo();
        }
        public void UpdateEffectCharacter()
        {
            //ingored
        }
        public void SendDie()
        {
            lock (Boss)
            {
                //if (Boss.Type == 70 || Boss.Type == 71 || Boss.Type == 72 || Boss.Type == 73 || Boss.Type == 74)
                //{
                //    //SkillHandler.BossSkillNotFocus(Boss, 14, 7);
                //    Boss.Zone.ZoneHandler.SendMessage(Service.SkillNotFocus7(Boss.Id, 141, 3000));

                //    Console.WriteLine("tusat");
                //    Boss.InfoDelayBoss.LeaveDead = ServerUtils.CurrentTimeMillis() + 3500;
                //    return;
                //}
               
                        RemoveSkill(ServerUtils.CurrentTimeMillis(), true);
                        Boss.InfoChar.IsDie = true;
                        Boss.InfoSkill.Monkey.MonkeyId = 0;
                        SetUpInfo();
                        SendZoneMessage(Service.PlayerDie(Boss));
                        DieChatMessage();
                       
                
            }
        }
        public Model.Item.Item GetItemBodyByIndex(int index)
        {
            return null;
        }
        public void UpdateEffective()
        {

        }
        public void LeaveItem(ICharacter character)
        {
            

        }

        public int GetParamItem(int id)
        {
            return Boss.ItemBody.Where(item => item != null).Select(item => item.Options.Where(option => option.Id == id).ToList()).Select(option => option.Sum(optionItem => optionItem.Param)).Sum();
        }

        public List<int> GetListParamItem(int id)
        {
            var param = new List<int>();
            foreach (var item in Boss.ItemBody.Where(item => item != null))
            {
                var option = item.Options.Where(option => option.Id == id).ToList();
                param.AddRange(option.Select(optionItem => optionItem.Param));
            }
            return param;
        }

        public void SetUpInfo(bool queryItem = false)
        {
            SetHpFull();
            SetMpFull();
            SetDamageFull();
            SetDefenceFull();
            SetCritFull();
            SetSpeed();
            SetHpPlusFromDamage();
            SetMpPlusFromDamage();
            SetBuffMp1s();
            SetBuffHp5s();
            SetBuffHp10s();
            SetBuffHp30s();
        }

        public void SetHpFull()
        {
            var hp = Boss.InfoChar.OriginalHp;
            hp += GetParamItem(2) * 100;
            hp += GetParamItem(6);
            hp += GetParamItem(22) * 1000;
            hp += GetParamItem(48);
            GetListParamItem(77).ForEach(param => hp += hp * param / 100);
            GetListParamItem(109).ForEach(param => hp -= hp * param / 100);
            if (Boss.InfoSkill.Monkey.MonkeyId != 0) hp += hp * Boss.InfoSkill.Monkey.Hp / 100;
            if (Boss.InfoSkill.HuytSao.IsHuytSao) hp += hp * Boss.InfoSkill.HuytSao.Percent / 100;
            Boss.HpFull = hp;
        }

        public void SetMpFull()
        {
            var mp = Boss.InfoChar.OriginalMp;
            mp += GetParamItem(2) * 100;
            mp += GetParamItem(7);
            mp += GetParamItem(23) * 1000;
            mp += GetParamItem(48);
            GetListParamItem(103).ForEach(param => mp += mp * param / 100);
            Boss.MpFull = mp;
        }

        public void SetDamageFull()
        {
            var damage = Boss.InfoChar.OriginalDamage;
            damage += GetParamItem(0);
            GetListParamItem(50).ForEach(param => damage += damage * param / 100);
            GetListParamItem(147).ForEach(param => damage += damage * param / 100);
            if (Boss.InfoSkill.Monkey.MonkeyId != 0) damage += damage * Boss.InfoSkill.Monkey.Damage / 100;
            Boss.DamageFull = damage;
        }

        public void SetDefenceFull()
        {
            var defence = Boss.InfoChar.OriginalDefence * 4;
            defence += GetParamItem(47);
            GetListParamItem(94).ForEach(param => defence += defence * param / 100);
            Boss.DefenceFull = Math.Abs(defence);
        }

        public void SetCritFull()
        {
            int crtCal;
            if (Boss.InfoSkill.Monkey.MonkeyId != 0)
            {
                crtCal = 115;
            }
            else
            {
                crtCal = Boss.InfoChar.OriginalCrit;
                crtCal += GetParamItem(14);
            }
            Boss.CritFull = crtCal;
        }

        public void SetHpPlusFromDamage()
        {
            var hpPlus = GetParamItem(95);
            Boss.HpPlusFromDamage = hpPlus;
        }

        public void SetMpPlusFromDamage()
        {
            var mpPlus = GetParamItem(96);
            Boss.MpPlusFromDamage = mpPlus;
        }

        public void SetSpeed()
        {
            var speed = 10;
            if (Boss.InfoSkill.Monkey.MonkeyId != 0) speed = 10;
            var plus = speed * (GetParamItem(148) + GetParamItem(16) + GetParamItem(114)) / 100;
            switch (plus)
            {
                case <= 1:
                    speed += 1;
                    break;
                case > 1 and <= 2:
                    speed += 2;
                    break;
            }

            Boss.InfoChar.Speed = (sbyte)speed;
        }

        public void SetBuffHp30s()
        {
            var hpPlus = GetParamItem(27);
            Boss.Effect.BuffHp30S.Value = hpPlus;
            if (Boss.Effect.BuffHp30S.Time == -1)
            {
                Boss.Effect.BuffHp30S.Time = 30000 + ServerUtils.CurrentTimeMillis();
            }

        }

        public void SetBuffMp1s()
        {
            var mpPlus = (int)Boss.MpFull * GetParamItem(162) / 100;
            Boss.Effect.BuffKi1s.Value = mpPlus;
            if (Boss.Effect.BuffKi1s.Time == -1)
            {
                Boss.Effect.BuffKi1s.Time = 1500 + ServerUtils.CurrentTimeMillis();
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
            if (Boss.IsDontMove()) return;

            var compare = Math.Abs(Boss.InfoChar.X - toX);
            if (compare >= 50)
            {
                if (Boss.InfoChar.X < toX)
                {
                    Boss.InfoChar.X = compare switch
                    {
                        >= 150 => (short)(toX - 70),
                        _ => (short)(toX - 50)
                    };
                }
                else
                {
                    Boss.InfoChar.X = compare switch
                    {
                        >= 150 => (short)(toX + 70),
                        _ => (short)(toX + 50)
                    };
                }

                if (toY != Boss.InfoChar.Y)
                {
                    Boss.InfoChar.Y = toY;
                }

                SendZoneMessage(Service.PlayerMove(Boss.Id, Boss.InfoChar.X, Boss.InfoChar.Y));
                if (Boss.InfoSkill.MeTroi.IsMeTroi && Boss.InfoSkill.MeTroi.DelayStart <= timeServer)
                {
                    SkillHandler.RemoveTroi(Boss);
                }
            }
        }

        public void PlusHp(int hp)
        {
            lock (Boss.InfoChar)
            {
                if (Boss.InfoChar.IsDie) return;
                Boss.InfoChar.Hp += hp;
                if (Boss.InfoChar.Hp >= Boss.HpFull) Boss.InfoChar.Hp = Boss.HpFull;
            }
        }

        public void MineHp(long hp)
        {
            lock (Boss.InfoChar)
            {
                if (Boss.InfoChar.IsDie || hp <= 0) return;

                if (hp > Boss.InfoChar.Hp)
                {
                    Boss.InfoChar.Hp = 0;
                }
                else
                {
                    Boss.InfoChar.Hp -= hp;
                }

                if (Boss.InfoChar.Hp <= 0)
                {
                    Boss.InfoChar.IsDie = true;
                    Boss.InfoChar.Hp = 0;
                }
            }
        }

        public void PlusMp(int mp)
        {
            lock (Boss.InfoChar)
            {
                if (Boss.InfoChar.IsDie) return;
                Boss.InfoChar.Mp += mp;
                if (Boss.InfoChar.Mp >= Boss.MpFull) Boss.InfoChar.Mp = Boss.MpFull;
            }
        }

        public void MineMp(int mp)
        {
            // lock (Boss.InfoChar)
            // {
            //     if(Boss.InfoChar.IsDie) return;
            //     Boss.InfoChar.Mp -= mp;
            //     if (Boss.InfoChar.Mp <= 0) Boss.InfoChar.Mp = 0;
            // }
        }

        public void PlusStamina(int stamina)
        {
            lock (Boss.InfoChar)
            {
                Boss.InfoChar.Stamina += (short)stamina;
                if (Boss.InfoChar.Stamina > 1250) Boss.InfoChar.Stamina = 1250;
            }
        }

        public void MineStamina(int stamina)
        {
            // lock (Boss.InfoChar)
            // {
            //     Boss.InfoChar.Stamina -= (short)stamina;
            //     if (Boss.InfoChar.Stamina <= 0) Boss.InfoChar.Stamina = 0;
            // }
        }

        public void PlusPower(long power)
        {
            // Ignore
        }

        public void PlusPotential(long potential)
        {
            // Ignore
        }

        public Model.Item.Item RemoveItemBody(int index)
        {
            Model.Item.Item item;
            lock (Boss.ItemBody)
            {
                item = Boss.ItemBody[index];
                if (item == null) return null;
                Boss.ItemBody[index] = null;
                UpdateInfo();
            }
            return item;
        }

        public void AddItemToBody(Model.Item.Item item, int index)
        {
            if (item == null) return;
            item.IndexUI = index;
            Boss.ItemBody[index] = item;
        }

        public void RemoveMonsterMe()
        {
            var skillEgg = Boss.InfoSkill.Egg;
            if (skillEgg.Monster is { IsDie: true })
            {
                SendZoneMessage(Service.UpdateMonsterMe7(skillEgg.Monster.Id));
                Boss.Zone.ZoneHandler.RemoveMonsterMe(skillEgg.Monster.Id);
                SkillHandler.RemoveMonsterPet(Boss);
            }
        }

        public void PlusTiemNang(IMonster monster, int damage)
        {
        }

        public void PlusTiemNang(long power, long potential, bool isAll)
        {
        }
        public void Heal()
        {
            SendMessage(Service.MeLoadInfo(Boss));
            Boss.InfoChar.IsDie = false;
            Boss.InfoChar.Hp = Boss.HpFull;
            Boss.InfoChar.Mp = Boss.MpFull;
            SendMessage(Service.MeLive());
            SendZoneMessage(Service.ReturnPointMap(Boss));
            SendZoneMessage(Service.PlayerLoadLive(Boss));
        }
        public void LeaveFromDead(bool isHeal = false)
        {

            lock (Boss)
            {
                Boss.Zone.ZoneHandler.RemoveBoss(Boss);
                UpdateInfo();
                Boss.InfoChar.IsDie = false;
                Boss.InfoChar.Hp = Boss.HpFull;
                Boss.InfoChar.Mp = Boss.MpFull;
                //SendZoneMessage(Service.ReturnPointMap(Boss));
                //SendZoneMessage(Service.PlayerLoadLive(Boss));
                Boss = null;
                Dispose();
            }
        }
      
        public void RemoveSkill(long timeServer, bool globalReset = false)
        {
            var infoSkill = Boss.InfoSkill;
            if ((infoSkill.TaiTaoNangLuong.IsTTNL &&
                 infoSkill.TaiTaoNangLuong.DelayTTNL <= timeServer) || globalReset)
            {
                SkillHandler.RemoveTTNL(Boss);
            }

            if (infoSkill.Monkey.MonkeyId == 1 && infoSkill.Monkey.TimeMonkey <= timeServer || globalReset)
            {
                SkillHandler.HandleMonkey(Boss, false);
            }

            if (infoSkill.Protect.IsProtect && infoSkill.Protect.Time <= timeServer || globalReset)
            {
                SkillHandler.RemoveProtect(Boss);
            }

            if (infoSkill.HuytSao.IsHuytSao && infoSkill.HuytSao.Time <= timeServer)
            {
                SkillHandler.RemoveHuytSao(Boss);
            }
            if (infoSkill.MaPhongBa.isMaPhongBa && infoSkill.MaPhongBa.timeMaPhongBa <= timeServer || globalReset)
            {
                RemoveMaPhongBa();
            }
            if (infoSkill.MeTroi.IsMeTroi)
            {
                var monsterMap = infoSkill.MeTroi.Monster;
                var charTemp = infoSkill.MeTroi.Character;
                if (globalReset)
                {
                    SkillHandler.RemoveTroi(Boss);
                }
                if (monsterMap is { IsDie: true })
                {
                    SkillHandler.RemoveTroi(Boss);
                }
                else if (charTemp != null && charTemp.InfoChar.IsDie)
                {
                    SkillHandler.RemoveTroi(Boss);
                }
                else if (infoSkill.MeTroi.TimeTroi <= timeServer || monsterMap is { IsDie: true } || charTemp != null && charTemp.InfoChar.IsDie)
                {
                    SkillHandler.RemoveTroi(Boss);
                }
            }

            if (infoSkill.PlayerTroi.IsPlayerTroi || globalReset) // mình là người bị trói
            {
                if (globalReset && infoSkill.PlayerTroi.IsPlayerTroi)
                {
                    List<int> PlayerID = new List<int>();

                    foreach (var id in new List<int>(infoSkill.PlayerTroi.PlayerId))
                    {
                        SkillHandler.RemoveTroi(Boss.Zone.ZoneHandler.GetCharacter(id));
                    }
                    // infoSkill.PlayerTroi.PlayerId.ForEach(i => SkillHandler.RemoveTroi(Boss.Zone.ZoneHandler.GetCharacter(i)));
                }
                else if (infoSkill.PlayerTroi.IsPlayerTroi && infoSkill.PlayerTroi.TimeTroi <= timeServer)
                {
                    infoSkill.PlayerTroi.IsPlayerTroi = false;
                    infoSkill.PlayerTroi.TimeTroi = -1;
                    infoSkill.PlayerTroi.PlayerId.Clear();
                    SkillHandler.RemoveTroi(Boss);
                }
            }


            if (infoSkill.DichChuyen.IsStun && infoSkill.DichChuyen.Time <= timeServer || globalReset)
            {
                SkillHandler.RemoveDichChuyen(Boss);
            }

            if (infoSkill.ThaiDuongHanSan.IsStun && infoSkill.ThaiDuongHanSan.Time <= timeServer || globalReset)
            {
                SkillHandler.RemoveThaiDuongHanSan(Boss);
            }

            if (infoSkill.ThoiMien.IsThoiMien && infoSkill.ThoiMien.Time <= timeServer || globalReset)
            {
                ///  Server.Gi().Logger.Print($"remove thoi mien, global reset: {globalReset}, [timeserver: {timeServer} - time: {infoSkill.ThoiMien.Time} = {timeServer - infoSkill.ThoiMien.Time}], thoimien stt: {infoSkill.ThoiMien.IsThoiMien}", "yellow");
                SkillHandler.RemoveThoiMien(Boss);
            }

            if (infoSkill.Socola.IsSocola && infoSkill.Socola.Time <= timeServer || globalReset)
            {
                SkillHandler.RemoveSocola(Boss);
            }
        }

        public void UpdateEffect(long timeServer)
        {
            var effect = Boss.Effect;
            if (effect.BuffHp30S.Value > 0 && effect.BuffHp30S.Time <= timeServer && Boss.InfoChar.Hp < Boss.HpFull)
            {
                PlusHp(effect.BuffHp30S.Value);
                SendZoneMessage(Service.PlayerLevel(Boss));
                effect.BuffHp30S.Time = 30000 + timeServer;
            }

            if (effect.BuffKi1s.Value > 0 && effect.BuffKi1s.Time <= timeServer && Boss.InfoChar.Mp < Boss.MpFull)
            {
                PlusMp(effect.BuffKi1s.Value);
                effect.BuffKi1s.Time = 1500 + timeServer;
            }
        }
        public void RemoveMaPhongBa()
        {
            var maphongba = Boss.InfoSkill.MaPhongBa;
            maphongba.isMaPhongBa = false;
            maphongba.timeMaPhongBa = -1;
            Boss.Zone.ZoneHandler.SendMessage(Service.UpdateBody(Boss));
        }
        public void RemoveTroi(int charId)
        {
            var infoSkill = Boss.InfoSkill.PlayerTroi;
            if (infoSkill.IsPlayerTroi)
            {
                infoSkill.PlayerId.RemoveAll(i => i == charId);
                if (infoSkill.PlayerId.Count <= 0)
                {
                    infoSkill.IsPlayerTroi = false;
                    infoSkill.TimeTroi = -1;
                    infoSkill.PlayerId.Clear();
                    SendZoneMessage(Service.SkillEffectPlayer(charId, Boss.Id, 2, 32));
                }
            }
        }

        #region Ignored Function
        public void UpdateMask()
        {

        }

        public void UpdateAutoPlay(long timeServer)
        {

        }

        public void UpdateLuyenTap()
        {

        }

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
            //Boss join map
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
            Heal();
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

        #endregion
    }
}

