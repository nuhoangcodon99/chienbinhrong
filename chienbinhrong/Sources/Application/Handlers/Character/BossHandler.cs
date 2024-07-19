using System;
using System.Collections.Generic;
using System.Linq;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Handlers.Item;
using NgocRongGold.Application.Handlers.Skill;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.Interfaces.Monster;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.DatabaseManager;
using NgocRongGold.DatabaseManager.Player;
using NgocRongGold.Model;
using NgocRongGold.Model.Map;
using NgocRongGold.Model.SkillCharacter;
using NgocRongGold.Model.Info;
using NgocRongGold.Model.Template;
using NgocRongGold.Model.Item;
using NgocRongGold.Model.Option;
using static System.GC;
using System.Threading;
using System.Threading.Tasks;
using NgocRongGold.Model.Character;
using NgocRongGold.Application.Threading;
using NgocRongGold.Application.Extension;
using Application.Interfaces.Zone;

namespace NgocRongGold.Application.Handlers.Character
{
    public class BossHandler : ICharacterHandler
    {
        public int GetAllQuantityItemBagById(int id)
        {
            return 0;
        }
        public void UpdateEffectTemporary(long timeServer)
        {
            
        }
        public void SetupAmulet()
        {

        }
        public void SetEnhancedOptionCard()
        {

        }
        public void QueryItem()
        {

        }
        public Model.Item.Item RemoveItemGiftBox(int index, bool isReset = true)
        {
            return null;

        }
        public void UpdateOther(long timeServer){   
                }
        public Boss Boss { get; set; }
        public Model.Item.Item GetItemClanBoxByIndex(int index)
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
        public BossHandler(Boss boss)
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
            SuppressFinalize(this);
        }

        public void SendZoneMessage(Message message)
        {
            Boss?.Zone?.ZoneHandler.SendMessage(message);
        }
        public static int countXenCon = 0;
         public void Update()
        {
            lock (Boss)
            {
                var timeServer = ServerUtils.CurrentTimeMillis();
                switch (Boss.Type)
                {
                    case 9:
                        if (Boss.InfoChar.IsDie && Boss.InfoDelayBoss.LeaveDead <= timeServer)
                        {
                            Boss.InfoDelayBoss.LeaveDead = -1;
                            LeaveFromDead();
                        }
                        if (Boss.InfoChar.TypePk != 0)
                        {
                            RemoveSkill(timeServer);

                            if (!Boss.InfoChar.IsDie)
                            {
                                AutoBoss(timeServer);
                                UpdateEffect(timeServer);
                            }
                        }
                        if (Boss.InfoDelayBoss.DelayRemove < timeServer && !Boss.InfoChar.IsDie && Boss.Zone.ZoneHandler.GetBossInMap(8).Count < 1)
                        {
                            Server.Gi().ABoss.SpawnXenBoHung = false;
                            Server.Gi().ABoss.SpawnXenBoHung = false;
                            LeaveFromDead();
                        }

                        return;
                    case 8:
                        if (Boss.InfoChar.IsDie && Boss.InfoDelayBoss.LeaveDead <= timeServer)
                        {
                            Boss.InfoDelayBoss.LeaveDead = -1;
                            LeaveFromDead();
                        }
                        if (Boss.InfoChar.TypePk != 0)
                        {
                            RemoveSkill(timeServer);

                            if (!Boss.InfoChar.IsDie)
                            {
                                AutoBoss(timeServer);
                                UpdateEffect(timeServer);
                            }
                        }
                        if (Boss.InfoDelayBoss.DelayRemove < timeServer && !Boss.InfoChar.IsDie && Boss.Zone.ZoneHandler.GetBossInMap(7).Count < 1)
                        {
                                                        Server.Gi().ABoss.SpawnXenBoHung = false;
                                                        Server.Gi().ABoss.DelayXenBoHung = 125000 + timeServer;
                            LeaveFromDead();
                        }

                        return;
                    case 6:
                        if (Boss.InfoChar.IsDie && Boss.InfoDelayBoss.LeaveDead <= timeServer)
                        {
                            Boss.InfoDelayBoss.LeaveDead = -1;
                            LeaveFromDead();
                        }
                        if (Boss.InfoChar.TypePk != 0)
                        {
                            RemoveSkill(timeServer);

                            if (!Boss.InfoChar.IsDie)
                            {
                                AutoBoss(timeServer);
                                UpdateEffect(timeServer);
                            }
                        }
                        if (Boss.InfoDelayBoss.DelayRemove < timeServer && !Boss.InfoChar.IsDie && Boss.Zone.ZoneHandler.GetBossInMap(5).Count < 1)
                        {
                                                        Server.Gi().ABoss.Fide1Spawn = false;
                                                        Server.Gi().ABoss.DelayFide1 = 121000 + timeServer;
                            LeaveFromDead();
                        }

                        return;
                    case 5:
                        if (Boss.InfoChar.IsDie && Boss.InfoDelayBoss.LeaveDead <= timeServer)
                        {
                            Boss.InfoDelayBoss.LeaveDead = -1;
                            LeaveFromDead();
                        }
                        if (Boss.InfoChar.TypePk != 0)
                        {
                            RemoveSkill(timeServer);

                            if (!Boss.InfoChar.IsDie)
                            {
                                AutoBoss(timeServer);
                                UpdateEffect(timeServer);
                            }
                        }
                        if (Boss.InfoDelayBoss.DelayRemove < timeServer && !Boss.InfoChar.IsDie && Boss.Zone.ZoneHandler.GetBossInMap(4).Count < 1)
                        {
                                                        Server.Gi().ABoss.Fide1Spawn = false;
                                                        Server.Gi().ABoss.DelayFide1 = 121000 + timeServer;
                            LeaveFromDead();
                        }

                        return;
                    case 31:
                        if (Boss.InfoChar.IsDie && Boss.InfoDelayBoss.LeaveDead <= timeServer)
                        {
                            Boss.InfoDelayBoss.LeaveDead = -1;
                            LeaveFromDead();
                        }
                        if (Boss.InfoChar.TypePk != 0)
                        {
                            RemoveSkill(timeServer);

                            if (!Boss.InfoChar.IsDie)
                            {
                                AutoBoss(timeServer);
                                UpdateEffect(timeServer);
                            }
                        }
                        if (Boss.InfoDelayBoss.DelayRemove < timeServer && !Boss.InfoChar.IsDie && Boss.Zone.ZoneHandler.GetBossInMap(30).Count < 1)
                        {
                                                        Server.Gi().ABoss.SpawnSatThu1 = false;
                                                        Server.Gi().ABoss.DelaySatThu1 = 122000 + timeServer;
                            LeaveFromDead();
                        }

                        return;
                    case 29:
                        if (Boss.InfoChar.IsDie && Boss.InfoDelayBoss.LeaveDead <= timeServer)
                        {
                            Boss.InfoDelayBoss.LeaveDead = -1;
                            LeaveFromDead();
                        }
                        if (Boss.InfoChar.TypePk != 0)
                        {
                            RemoveSkill(timeServer);

                            if (!Boss.InfoChar.IsDie)
                            {
                                AutoBoss(timeServer);
                                UpdateEffect(timeServer);
                            }
                        }
                        if (Boss.InfoDelayBoss.DelayRemove < timeServer && !Boss.InfoChar.IsDie && Boss.Zone.ZoneHandler.GetBossInMap(27).Count < 1)
                        {
                                                        Server.Gi().ABoss.SpawnSatThu3 = false;
                                                        Server.Gi().ABoss.DelaySatThu3 = 123000 + timeServer;
                            LeaveFromDead();
                        }

                        return;
                    case 27:
                        if (Boss.InfoChar.IsDie && Boss.InfoDelayBoss.LeaveDead <= timeServer)
                        {
                            Boss.InfoDelayBoss.LeaveDead = -1;
                            LeaveFromDead();
                        }
                        if (Boss.InfoChar.TypePk != 0)
                        {
                            RemoveSkill(timeServer);

                            if (!Boss.InfoChar.IsDie)
                            {
                                AutoBoss(timeServer);
                                UpdateEffect(timeServer);
                            }
                        }
                        if (Boss.InfoDelayBoss.DelayRemove < timeServer && !Boss.InfoChar.IsDie && Boss.Zone.ZoneHandler.GetBossInMap(28).Count < 1)
                        {
                                                        Server.Gi().ABoss.SpawnSatThu3 = false;
                                                        Server.Gi().ABoss.DelaySatThu3 = 123000 + timeServer;
                            LeaveFromDead();
                        }

                        return;
                    case 32:
                        if (Boss.InfoChar.IsDie && Boss.InfoDelayBoss.LeaveDead <= timeServer)
                        {
                            Boss.InfoDelayBoss.LeaveDead = -1;
                            LeaveFromDead();
                        }
                        if (Boss.InfoChar.TypePk != 0)
                        {
                            RemoveSkill(timeServer);

                            if (!Boss.InfoChar.IsDie)
                            {
                                AutoBoss(timeServer);
                                UpdateEffect(timeServer);
                            }
                        }
                        if (Boss.InfoDelayBoss.DelayRemove < timeServer && !Boss.InfoChar.IsDie && Boss.Zone.ZoneHandler.GetBossInMap(33).Count < 1)
                        {
                                                        Server.Gi().ABoss.SpawnSatThu2 = false;
                                                        Server.Gi().ABoss.DelaySatThu2 = 124000 + timeServer;
                            LeaveFromDead();
                        }

                        return;
                    case 33:
                        if (Boss.InfoChar.IsDie && Boss.InfoDelayBoss.LeaveDead <= timeServer)
                        {
                            Boss.InfoDelayBoss.LeaveDead = -1;
                            LeaveFromDead();
                        }
                        if (Boss.InfoChar.TypePk != 0)
                        {
                            RemoveSkill(timeServer);

                            if (!Boss.InfoChar.IsDie)
                            {
                                AutoBoss(timeServer);
                                UpdateEffect(timeServer);
                            }
                        }
                        if (Boss.InfoDelayBoss.DelayRemove < timeServer && !Boss.InfoChar.IsDie && Boss.Zone.ZoneHandler.GetBossInMap(34).Count < 1)
                        {
                                                        Server.Gi().ABoss.SpawnSatThu2 = false;
                                                        Server.Gi().ABoss.DelaySatThu2 = 124000 + timeServer;
                            LeaveFromDead();
                        }

                        return;
                    case 19: // tdt
                        if (Boss.InfoChar.IsDie && Boss.InfoDelayBoss.LeaveDead <= timeServer)
                        {
                            Boss.InfoDelayBoss.LeaveDead = -1;
                            LeaveFromDead();
                        }
                        if (Boss.InfoChar.TypePk != 0)
                        {
                            RemoveSkill(timeServer);

                            if (!Boss.InfoChar.IsDie)
                            {
                                AutoBoss(timeServer);
                                UpdateEffect(timeServer);
                            }
                        }
                        if (Boss.InfoDelayBoss.DelayRemove < timeServer && Boss.Zone.ZoneHandler.GetBossInMap(18).Count < 1 && !Boss.InfoChar.IsDie)
                        {

                                                        Server.Gi().ABoss.DelayTDST = 15000 + timeServer;
                                                        Server.Gi().ABoss.SpawnTDST = false;

                            Server.Gi().Logger.Print("CLEAR TDST!", "red");
                            LeaveFromDead();
                        }

                        return;
                    case 18://so1
                        if (Boss.InfoChar.IsDie && Boss.InfoDelayBoss.LeaveDead <= timeServer)
                        {
                            Boss.InfoDelayBoss.LeaveDead = -1;
                            LeaveFromDead();
                        }
                        if (Boss.InfoChar.TypePk != 0)
                        {
                            RemoveSkill(timeServer);

                            if (!Boss.InfoChar.IsDie)
                            {
                                AutoBoss(timeServer);
                                UpdateEffect(timeServer);
                            }
                        }
                        if (Boss.InfoDelayBoss.DelayRemove < timeServer && Boss.Zone.ZoneHandler.GetBossInMap(93).Count < 1 && !Boss.InfoChar.IsDie)
                        {
                            LeaveFromDead();

                        }

                        return;
                    case 93://so2
                        if (Boss.InfoChar.IsDie && Boss.InfoDelayBoss.LeaveDead <= timeServer)
                        {
                            Boss.InfoDelayBoss.LeaveDead = -1;
                            LeaveFromDead();
                        }
                        if (Boss.InfoChar.TypePk != 0)
                        {
                            RemoveSkill(timeServer);

                            if (!Boss.InfoChar.IsDie)
                            {
                                AutoBoss(timeServer);
                                UpdateEffect(timeServer);
                            }
                        }
                        if (Boss.InfoDelayBoss.DelayRemove < timeServer && Boss.Zone.ZoneHandler.GetBossInMap(17).Count < 1 && !Boss.InfoChar.IsDie)
                        {

                            LeaveFromDead();

                        }

                        return;
                    case 17://so3
                        if (Boss.InfoChar.IsDie && Boss.InfoDelayBoss.LeaveDead <= timeServer)
                        {
                            Boss.InfoDelayBoss.LeaveDead = -1;
                            LeaveFromDead();
                        }
                        if (Boss.InfoChar.TypePk != 0)
                        {
                            RemoveSkill(timeServer);

                            if (!Boss.InfoChar.IsDie)
                            {
                                AutoBoss(timeServer);
                                UpdateEffect(timeServer);
                            }
                        }
                        if (Boss.InfoDelayBoss.DelayRemove < timeServer && Boss.Zone.ZoneHandler.GetBossInMap(16).Count < 1 && !Boss.InfoChar.IsDie)
                        {
                            LeaveFromDead();

                        }

                        return;                    
                    case 50:
                        if (!Boss.isPhanThan && Boss.InfoChar.Hp < Boss.HpFull)
                        {
                            async void Action()
                            {
                                Boss.isPhanThan = true;
                                SendBossChat("Ảnh phân thân đa trọng ảnh");
                                await Task.Delay(3000);
                                for (int i = 0; i < 5; i++)
                                {
                                    var randomX = ServerUtils.RandomNumber((short)Boss.InfoChar.X, (short)Boss.InfoChar.X + 20);
                                    var boss = new Boss();
                                    boss.CreateBossPhanThan(50, (short)randomX, Boss.InfoChar.Y);
                                    boss.CharacterHandler.SetUpInfo();
                                    Boss.Zone.ZoneHandler.AddBoss(boss);
                                }
                            }
                            var task = new Task(Action);
                            task.Start();
                        }
                        break;
                    case 63:
                        if (Boss.InfoChar.IsDie && Boss.InfoDelayBoss.LeaveDead <= timeServer)
                        {
                            Boss.InfoDelayBoss.LeaveDead = -1;
                            LeaveFromDead();
                        }
                        if (!Boss.InfoChar.IsDie)
                        {
                            if (Boss.InfoChar.Hp < Boss.HpFull / 2)
                            {
                                if (Boss.InfoChar.TypePk != 0 && Boss.isSpawnXenCon == false && Boss.InfoDelayBoss.AutoSpawnXenCon <= timeServer)
                                {
                                    SendBossChat("Hahaha khá đấy tụi nhóc, thử đấm nhau với 7 thằng đệ tao nè !");
                                    Boss.InfoChar.TypePk = 0;
                                    Boss.CharacterHandler.SendZoneMessage(Service.ChangeTypePk(Boss.Id, 0));
                                    Boss.isSpawnXenCon = true;
                                    Boss.InfoDelayBoss.AutoSpawnXenCon = 120000 + timeServer;
                                }                                
                            }
                            else if (Boss.isSpawnXenCon)
                            {
                                Boss.isSpawnXenCon = false;
                                for (int i = 0; i < 7; i++)
                                {
                                    var xencon = new Boss();
                                    xencon.CreateXenCon(i);
                                    xencon.CharacterHandler.SetUpInfo();
                                    Boss.Zone.ZoneHandler.AddBoss(xencon);
                                }



                            }
                            else if (Boss.isSpawnXenCon == false && Boss.InfoChar.TypePk == 0)
                            {
                                if (Boss.Zone.Bosses.Count == 1)
                                {
                                    SendBossChat("Anh đã quay trở lại rồiii !");
                                    Boss.InfoChar.TypePk = 5;
                                    Boss.CharacterHandler.SendZoneMessage(Service.ChangeTypePk(Boss.Id, 5));
                                }
                            }
                            RemoveSkill(timeServer);
                            AutoBoss(timeServer);
                            UpdateEffect(timeServer);
                            
                        }
                        break;

                    case 40:
                        if (Boss.InfoChar.TypePk == 5)
                        {
                            Boss.InfoChar.TypePk = 0;
                        }
                        if (!Boss.InfoChar.IsDie)
                        {
                            if (Boss.InfoDelayBoss.AutoDie <= timeServer)
                            {
                                LeaveFromDead();
                                return;
                            }
                            if (Boss.InfoDelayBoss.AutoRotHopQua <= timeServer)
                            {
                                AutoBoss(timeServer);
                                UpdateEffect(timeServer);
                                SendBossChat("Hô hô");
                                var hopqua = ItemCache.GetItemDefault(648);
                                var drop = new ItemMap(-1, hopqua);
                                drop.X = Boss.InfoChar.X;
                                drop.Y = Boss.InfoChar.Y;
                                Boss.Zone.ZoneHandler.LeaveItemMap(drop);
                                Boss.InfoDelayBoss.AutoRotHopQua = 15000 + timeServer;
                            }

                        }
                        else
                        {
                            if (Boss.InfoDelayBoss.LeaveDead <= timeServer)
                            {
                                Boss.InfoDelayBoss.LeaveDead = -1;
                                LeaveFromDead();

                            }
                        }
                        break;

                    default:
                        if (Boss.isYardat && Boss.InfoDelayBoss.AutoPlusHP <= timeServer)
                        {
                            if (Boss.InfoChar.TypePk == 0)
                            {
                                if (Boss.InfoChar.Hp < Boss.HpFull)
                                {
                                    Boss.InfoChar.Hp += Boss.HpFull / (long)2.5;
                                    Boss.InfoDelayBoss.AutoPlusHP = 600 + timeServer;
                                }
                                else
                                {
                                    Boss.InfoChar.TypePk = 5;
                                    Boss.CharacterHandler.SendZoneMessage(Service.ChangeTypePk(Boss.Id, 5));
                                    Boss.InfoDelayBoss.AutoPlusHP = 5000 + timeServer;
                                }
                            }
                        }
                        if (!Boss.InfoChar.IsDie)
                        {
                            if (Boss.Status >= 3) return;
                            if (Boss.InfoChar.TypePk == 0 && Boss.Type != 47 && Boss.Type != 83 && Boss.Type != 84) return;
                            RemoveSkill(timeServer);



                            AutoBoss(timeServer);
                            UpdateEffect(timeServer);
                        }
                        if (Boss.InfoChar.IsDie && Boss.InfoDelayBoss.LeaveDead <= timeServer)
                        {
                            Boss.InfoDelayBoss.LeaveDead = -1;
                            LeaveFromDead();
                        }
                        break;
                }



            }
        }

        private void AutoBoss(long timeServer)
        {
            if(Boss.IsDontMove()) return;
           
            // if (Boss.InfoChar.Stamina <= 0)
            // {
            //     Boss.CharacterHandler.SendZoneMessage(Service.PlayerMove(Boss.Id, Boss.InfoChar.X, Boss.InfoChar.Y));
            // }
            // else
            // {
            // }
            HandleBossAction();
        }

        private void HandleBossAction()
        {
            var timeServer = ServerUtils.CurrentTimeMillis();
            var checkSize = 1000;

            if (Boss.CharacterAttack.Count > 0)
            {
                foreach (var id in Boss.CharacterAttack.ToList())
                {
                    var character = Boss.Zone.ZoneHandler.GetCharacter(id);

                    if (character == null)
                    {
                        Boss.CharacterAttack.RemoveAll(i => i == id);
                        continue;
                    }
                    
                    if (character.InfoSkill.Socola.IsSocola)
                    {
                        Boss.CharacterAttack.RemoveAll(i => i == id);
                        continue;
                    }
                    if (character.InfoChar.IsDie)
                    {
                        Boss.CharacterAttack.RemoveAll(i => i == id);
                        continue;
                    }
                    var distance = Math.Abs(character.InfoChar.X - Boss.InfoChar.X);
                    if (!character.InfoSkill.Socola.IsSocola && !character.InfoChar.IsDie && distance <= checkSize && Math.Abs(character.InfoChar.Y - Boss.InfoChar.Y) <= checkSize)
                    {
                        HandleUseSkill(character);
                        break;
                    }
                    Boss.CharacterAttack.RemoveAll(i => i == id);
                }
            }
            else 
            {
                checkSize = 300;
                var character = Boss.CharacterFocus;
                if (character == null || 
                 Boss.Zone.ZoneHandler.GetCharacter(character.Id) == null || 
                 character.InfoChar.IsDie || 
                 Math.Abs(character.InfoChar.X - Boss.InfoChar.X) > checkSize || 
                 Math.Abs(character.InfoChar.Y - Boss.InfoChar.Y) > 600)
                {
                    
                        Boss.CharacterFocus = character =
                            Boss.Zone.Characters.Values.ToList().FirstOrDefault(m => !m.InfoChar.IsDie && !m.InfoSkill.Socola.IsSocola &&
                            !CheckNearWaypoint(m) &&
                            Math.Abs(m.InfoChar.X - Boss.InfoChar.X) <= checkSize &&
                            Math.Abs(m.InfoChar.Y - Boss.InfoChar.Y) <= 600);
                    
                }

                if (character == null)
                {
                    if (Boss.InfoChar.Hp != Boss.HpFull)
                    {
                        if (Boss.InfoDelayBoss.TTNL <= timeServer)
                        {
                            // Xử lý tái tạo năng lượng
                            SkillHandler.BossSkillNotFocus(Boss, 8, 2);
                            
                            Boss.InfoDelayBoss.TTNL = timeServer + 15000;
                        }
                    }
                    AutoMoveMap(timeServer);
                }
                else
                {
                    if (character.InfoSkill.Socola.IsSocola)
                    {
                        AutoMoveMap(timeServer);
                        return;
                    }
                    HandleUseSkill(character);
                }
            }
        }

        public Message MessageSkillMabu(byte skillId, short xTo, short yTo)
        {
            var msg = new Message(51);
            msg.Writer.WriteInt(Boss.Id);
            msg.Writer.WriteByte(skillId);
            msg.Writer.WriteShort(xTo);
            msg.Writer.WriteShort(yTo);
            msg.Writer.WriteByte(Boss.Zone.Characters.Count);
            for (int i = 0; i < Boss.Zone.Characters.Count; i++)
            {
                msg.Writer.WriteInt(Boss.Zone.ZoneHandler.CharacterInMap()[i].Id);
                msg.Writer.WriteInt(Boss.DamageFull);
            }
            return msg;
        }
        public Message MessageSkillMabu2(byte skillType, Model.Character.Character character)
        {
            var msg = new Message(52);
            msg.Writer.WriteByte(skillType);
            switch (skillType)
            {
                case 0:
                case 1:
                    msg.Writer.WriteInt(character.Id);
                    if (skillType is 0)
                    {
                        msg.Writer.WriteShort(character.InfoChar.X);
                        msg.Writer.WriteShort(character.InfoChar.Y);
                    }
                    break;
                case 2:
                    msg.Writer.WriteInt(Boss.Id);
                    msg.Writer.WriteInt(character.Id);
                    character.InfoSkill.TrungMabu.Eat = Model.Info.Skill.Active.ACTIVE;
                    async void Action()
                    {
                        //handle go map bung bu
                        await Task.Delay(1000);
                        character.CharacterHandler.SendMessage(MessageSkillMabu2(1, character));
                        character.InfoSkill.TrungMabu.Active = Model.Info.Skill.Active.ACTIVE;
                        character.InfoSkill.TrungMabu.Time = DataCache._1MINUTES * 5;
                    }
                    var task = new Task(Action);
                    task.Start();
                    break;
                default:
                    msg.Writer.WriteInt((int)(Boss.InfoChar.Hp % Boss.HpFull));
                    break;
            }
          
            return msg;
        }
        #region boss style attack
        private void HandleUseSkill(ICharacter character)
        {
           
            
            var infoSkill = Boss.InfoSkill;
            var timeServer = ServerUtils.CurrentTimeMillis();
            switch (Boss.Type)
            {

                case 45 or 46 or 43 or 44:
                    byte skillId = (byte)ServerUtils.RandomNumber(0, 5);
                    //if (skillId is 5)
                    //{
                    //    Boss.Zone.ZoneHandler.SendMessage(MessageSkillMabu2(2, (Model.Character.Character)character));
                    //    break;
                    //}
                    Boss.Zone.ZoneHandler.SendMessage(MessageSkillMabu(skillId, character.InfoChar.X, character.InfoChar.Y));
                    // Server.Gi().Logger.Print($"skillId:  {skillId}, {character.InfoChar.X}, {character.InfoChar.Y}");
                    break;


            }
            // Tái tạo năng lượng
            // Đang tái tạo năng lượng sẽ không bị xóa
            if (infoSkill.TaiTaoNangLuong.IsTTNL &&
                 infoSkill.TaiTaoNangLuong.DelayTTNL > timeServer)
            {
                if (Boss.InfoDelayBoss.TTNL <= timeServer)
                {
                    if (Boss.Type == 41 && Boss.InfoDelayBoss.AutoPlusHP < timeServer)
                    {
                        if (Boss.HpFull <= 17000000)
                        {
                            if (Boss.HpFull < 50000)
                            {
                                Boss.HpFull += Boss.HpFull * 2;
                            }
                            else if (Boss.HpFull < 100000)
                            {
                                Boss.HpFull += Boss.HpFull / 10;
                            }
                            else if (Boss.HpFull < 250000)
                            {
                                Boss.HpFull += Boss.HpFull / 30;
                            }
                            else if (Boss.HpFull < 340000)
                            {
                                Boss.HpFull += Boss.HpFull / 50;
                            }
                            else
                            {
                                Boss.HpFull += Boss.HpFull / 80;
                            }
                        }
                        Boss.HpFull += Boss.HpPst;
                        Boss.HpPst = 0;
                        Boss.InfoDelayBoss.AutoPlusHP = timeServer + Boss.Type == 41? 5000: 60000;
                        //Server.Gi().Logger.Print("HP: " + Boss.HpFull, "cyan");
                    }
                    // Xử lý tái tạo năng lượng
                    SkillHandler.BossSkillNotFocus(Boss, 8, 2);
                   
                    Boss.InfoDelayBoss.TTNL = timeServer + 15000;
                }
                if (infoSkill.TaiTaoNangLuong.IsTTNL)
                {
                    return;
                }
                return;
            }

            // Tìm chiêu để sử dụng
            SkillCharacter skillChar = null;
            var dX = 0;
            var dY = 0;
            bool isMoveToPlayer = false;
          //  try {

                // Kiểm tra khoản cách giữa quái và đệ
                var bossDistance = Math.Abs(character.InfoChar.X - Boss.InfoChar.X);
                var bossDistanceY = Math.Abs(character.InfoChar.Y - Boss.InfoChar.Y);
                // for skill từ trên xuống dưới
                for (int i = Boss.Skills.Count - 1; i >= 0; i--)
                {
                    skillChar = Boss.Skills[i];
                    
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
                    var manaChar = Boss.InfoChar.Mp;
                    manaUse = manaUseType switch
                    {
                        1 => manaUse * (int) Boss.MpFull / 100,
                        2 => (int) manaChar,
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
                            var hpMine = Boss.HpFull*0.6;
                            if (hpMine < Boss.InfoChar.Hp)
                            {
                                skillChar = null;
                                continue;
                            }
                        
                        }
                        else if (skillChar.Id == 9)
                        {
                            if (ServerUtils.RandomNumber(0, 100) < 80) continue;
                            var hpMine = Boss.HpFull / 10;
                            if (hpMine >= Boss.InfoChar.Hp)
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
                        if ((bossDistance <= dX && bossDistanceY <= dY) || Boss.Skills.Count == 1)
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
                if (Boss.InfoDelayBoss.AutoChat <= timeServer)
                {
                    if (Boss.Type != 40)
                    {
                        CombatChatMessage(timeServer);
                    }
                    else
                    {
                        NoelChatMessage(timeServer);
                    }
                }

                if (skillChar.Id == 8)
                {
                    // Bắt đầu dùng tái tạo năng lượng
                    SkillHandler.BossSkillNotFocus(Boss, skillChar.Id, 1);
                    return;
                }

                // Thái dương hạ sang
                if (skillChar.Id == 6)
                {
                    SkillHandler.BossSkillNotFocus(Boss, skillChar.Id, 0);
                    return;
                }
                if (skillChar.Id == 14)
                {
                    SkillHandler.BossSkillNotFocus(Boss, skillChar.Id, 7);

                    return;
                }
                // Khiên năng lượng
                if (skillChar.Id == 19)
                {
                    SkillHandler.BossSkillNotFocus(Boss, skillChar.Id, 9);
                    return;
                }

                if (skillChar.Id == 0 || skillChar.Id == 2 || skillChar.Id == 4 || skillChar.Id == 9 || skillChar.Id == 17 || isMoveToPlayer)
                {
                    if (character.InfoChar.X > Boss.InfoChar.X)
                    {
                        Boss.InfoChar.X = (short)(character.InfoChar.X - dX);
                    }
                    else
                    {
                        Boss.InfoChar.X = (short)(character.InfoChar.X + dX);
                    }

                    
                    Boss.InfoChar.Y = character.InfoChar.Y;
                    isMoveToPlayer = false;

                    SendZoneMessage(Service.PlayerMove(Boss.Id, Boss.InfoChar.X, Boss.InfoChar.Y));
                }
                else if ((skillChar.Id == 1 || skillChar.Id == 3 || skillChar.Id == 5) && Boss.InfoChar.Y > character.InfoChar.Y)
                {
                    if (ServerUtils.RandomNumber(100) < 35)
                    {
                        Boss.InfoChar.Y = (short)(character.InfoChar.Y - ServerUtils.RandomNumber(0, 40));
                    }
                    else
                    {
                        Boss.InfoChar.Y = character.InfoChar.Y;
                    }
                    SendZoneMessage(Service.PlayerMove(Boss.Id, Boss.InfoChar.X, Boss.InfoChar.Y));
                }
                AttackPlayer(character, skillChar);

                // 
                if (Boss.Type == DataCache.BOSS_THO_PHE_CO_TYPE)
                {
                    Boss.InfoDelayBoss.AutoChangeMap = timeServer + 500000;
                }
         //   }
         //   catch (Exception)
         //   {
                // Ignore
         //       return;
          //  }

        }
        
        private void AttackPlayer(ICharacter character, SkillCharacter skillChar)
        {
            if (!character.InfoChar.IsDie)
            {
                var isNearWaypoint = CheckNearWaypoint(Boss);
                if (isNearWaypoint)
                {
                    Boss.CharacterFocus = null;
                    MoveMap(Boss.BasePositionX, Boss.BasePositionY);
                }
                else 
                {
                    SkillHandler.BossAttackPlayer(Boss, skillChar, character.Id);
                }
            }
        }
        #endregion

        private void AutoMoveMap(long timeServer)
        {
            if (Boss.InfoDelayBoss.AutoMove <= timeServer)
            {
                Boss.InfoChar.X = (short)ServerUtils.RandomNumber(Boss.BasePositionX - 50,
                    Boss.BasePositionX + 50);
                SendZoneMessage(Service.PlayerMove(Boss.Id, Boss.InfoChar.X, Boss.InfoChar.Y));
                if (Boss.InfoSkill.MeTroi.IsMeTroi &&
                    Boss.InfoSkill.MeTroi.DelayStart <= timeServer)
                {
                    SkillHandler.RemoveTroi(Boss);
                }
                Boss.InfoDelayBoss.AutoMove = timeServer + ServerUtils.RandomNumber(2000, 4000);
            }

            if (Boss.InfoDelayBoss.AutoChat <= timeServer)
            {
                IdleChatMessage(timeServer);
            }
            // Boss thỏ phê cỏ tự đổi map tìm người
            if (Boss.InfoDelayBoss.AutoChangeMap <= timeServer)
            {
                AutoChangeMap(timeServer);
            }
        }

        private void AutoChangeMap(long timeServer)
        {
            Boss.InfoDelayBoss.AutoChangeMap = timeServer + 500000;
            if (Boss.Type == DataCache.BOSS_THO_PHE_CO_TYPE)
            {
                var randChar = ClientManager.Gi().GetRandomCharacter();
                if (randChar != null) 
                {
                    var zone = randChar.Zone;
                    if (zone != null)
                    {
                        Boss.Zone.ZoneHandler.RemoveBoss(Boss);
                        Boss.CharacterHandler.SetUpInfo();
                        Boss.InfoChar.X = randChar.InfoChar.X;
                        Boss.InfoChar.Y = randChar.InfoChar.Y;
                        Boss.BasePositionX = randChar.InfoChar.X;
                        Boss.BasePositionY = randChar.InfoChar.Y;
                        zone.ZoneHandler.AddBoss(Boss);
                        ClientManager.Gi().SendMessageCharacter(Service.ServerChat("BOSS Thỏ Phê Cỏ " + Boss.Id + " vừa xuất hiện tại " + zone.Map.TileMap.Name));
                    }
                }
                // tự đổi map khác
            }
        }

        private void SendBossChat(string text)
        {
            if (Boss.Status < 3)
            {
                SendZoneMessage(Service.PublicChat(Boss.Id, text));
            };
        }

        private void CombatChatMessage(long timeServer)
        {
            if (DatabaseManager.ConfigManager.gI().SuKienTrungThu)
            {
                SendBossChat(TextServer.gI().BOSS_MOON_CHAT_COMBAT[ServerUtils.RandomNumber(TextServer.gI().BOSS_MOON_CHAT_COMBAT.Count)]);
            }
            else 
            {
                SendBossChat(TextServer.gI().BOSS_CHAT_COMBAT[ServerUtils.RandomNumber(TextServer.gI().BOSS_CHAT_COMBAT.Count)]);
            }
            Boss.InfoDelayBoss.AutoChat = timeServer + ServerUtils.RandomNumber(3000, 5000);
        }
        private void NoelChatMessage(long timeServer)
        {
            if (DatabaseManager.ConfigManager.gI().SuKienTrungThu)
            {
                SendBossChat(TextServer.gI().BOSS_MOON_CHAT_COMBAT[ServerUtils.RandomNumber(TextServer.gI().BOSS_MOON_CHAT_COMBAT.Count)]);
            }
            else
            {
                SendBossChat(TextServer.gI().BOSS_CHAT_TRANG[ServerUtils.RandomNumber(TextServer.gI().BOSS_CHAT_TRANG.Count)]);
            }
            Boss.InfoDelayBoss.AutoChat = timeServer + ServerUtils.RandomNumber(3000, 5000);
        }

        private void IdleChatMessage(long timeServer)
        {
            if (DatabaseManager.ConfigManager.gI().SuKienTrungThu)
            {
                SendBossChat(TextServer.gI().BOSS_MOON_CHAT_IDLE[ServerUtils.RandomNumber(TextServer.gI().BOSS_MOON_CHAT_IDLE.Count)]);
            }
            else 
            {
                SendBossChat(TextServer.gI().BOSS_CHAT_IDLE[ServerUtils.RandomNumber(TextServer.gI().BOSS_CHAT_IDLE.Count)]);
            }
            Boss.InfoDelayBoss.AutoChat = timeServer + ServerUtils.RandomNumber(3000, 5000);
        }

        private void DieChatMessage()
        {
            switch (Boss.Type)
            {

                case (>= 85 and <= 92) or 94:
                    SendBossChat("Ok, ta chịu thua");
                    break;
                default:
                    if (DatabaseManager.ConfigManager.gI().SuKienTrungThu)
                    {
                        SendBossChat(TextServer.gI().BOSS_MOON_CHAT_DIE[ServerUtils.RandomNumber(TextServer.gI().BOSS_MOON_CHAT_DIE.Count)]);
                    }
                    else
                    {
                        SendBossChat(TextServer.gI().BOSS_CHAT_DIE[ServerUtils.RandomNumber(TextServer.gI().BOSS_CHAT_DIE.Count)]);
                    }
                    break;
            }
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
            SuppressFinalize(this);
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
                    Boss.BasePositionX = 450    ;
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
                    Boss.BasePositionX = (short)ServerUtils.RandomNumber(250,450);
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
                switch (Boss.Type)
                {
                    case int i when DataCache.IdBossPractice.Contains(i):
                        Boss.InfoChar.TypePk = 0;
                        Boss.Zone.ZoneHandler.SendMessage(Service.ChangeTypePk(Boss.Id, 0));
                        SetUpInfo();
                        Boss.CharacterHandler.BackHome();
                        Boss.CharacterAttack.Clear();
                        DieChatMessage();
                        SetUpPosition(-1, Boss.Zone.Map.Id);
                        break;
                    default:
                        Server.Gi().ABoss.BossDied(Boss);
                        RemoveSkill(ServerUtils.CurrentTimeMillis(), true);
                        Boss.InfoChar.IsDie = true;
                        Boss.InfoSkill.Monkey.MonkeyId = 0;
                        SetUpInfo();
                        SendZoneMessage(Service.PlayerDie(Boss));
                        DieChatMessage();
                        Boss.CharacterAttack.Clear();
                        switch (Boss.Type)
                        {
                            case int i when DataCache.IdBossDied.Contains(i):
                                Boss.InfoDelayBoss.LeaveDead = -1;
                                break;
                            default:
                                Boss.InfoDelayBoss.LeaveDead = ServerUtils.CurrentTimeMillis() + 5000;
                                break;
                        }
                        break;
                }
                //Boss.Zone.ZoneHandler.RemoveBoss(Boss);
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
            var playerKillId = Math.Abs(character.Id);
            ClientManager.Gi().SendMessageCharacter(Service.ServerChat(character.Name + ": Đã tiêu diệt được " + Boss.Name + " mọi người đều ngưỡng mộ."));
            //  Server.Gi().Logger.Print($"{character.Name} HAS BEEN KILLED BOSS {Boss.Name}", "red");

            if (DataCache.IdMapMabu.Contains(Boss.Zone.Map.Id) || DataCache.IdMapMabu2.Contains(Boss.Zone.Map.Id))
            {
                var random = ServerUtils.RandomNumber(25, 40);
                for (int cgold = 0; cgold < random; cgold++)
                {
                    var itemGold = ItemCache.GetItemDefault(457);
                    var goldItemMap = new ItemMap(playerKillId, itemGold);
                    goldItemMap.X = (short)(Boss.InfoChar.X + (10 * cgold));
                    goldItemMap.Y = Boss.InfoChar.Y;
                    Boss.Zone.ZoneHandler.LeaveItemMap(goldItemMap);
                }
            }
            switch (Boss.Type)
            {
                case 109:
                    {
                        if (ServerUtils.RandomNumber(50) < 15)
                        {
                            var item = ItemCache.GetItemDefault(1291);
                            var itemMap = new ItemMap(playerKillId, item);
                            itemMap.X = Boss.InfoChar.X;
                            itemMap.Y = Boss.InfoChar.Y;
                            Boss.Zone.ZoneHandler.LeaveItemMap(itemMap);
                        }
                    }
                    break;
                case 110:
                    {
                        if (ServerUtils.RandomNumber(50) < 35)
                        {
                            var item = ItemCache.GetItemDefault(898);
                            var itemMap = new ItemMap(playerKillId, item);
                            itemMap.X = Boss.InfoChar.X;
                            itemMap.Y = Boss.InfoChar.Y;
                            Boss.Zone.ZoneHandler.LeaveItemMap(itemMap);
                        }
                    }
                    break;
                case 111:
                    {
                        if (ServerUtils.RandomNumber(50) < 15)
                        {
                            var item = ItemCache.GetItemDefault(1293);
                            var itemMap = new ItemMap(playerKillId, item);
                            itemMap.X = Boss.InfoChar.X;
                            itemMap.Y = Boss.InfoChar.Y;
                            Boss.Zone.ZoneHandler.LeaveItemMap(itemMap);
                        }
                    }
                    break;
                case 105:
                case 106:
                    {
                        if (ServerUtils.RandomNumber(50) < 15)
                        {
                            var item2 = ItemCache.GetItemDefault((short)16);
                            var itemMap2 = new ItemMap(playerKillId, item2);
                            itemMap2.X = Boss.InfoChar.X;
                            itemMap2.Y = Boss.InfoChar.Y;
                            Boss.Zone.ZoneHandler.LeaveItemMap(itemMap2);
                        }
                        else if (ServerUtils.RandomNumber(100) < 15)
                        {
                            var goditem = LeaveItemHandler.LeaveGodItem(character, "đánh boss");
                            goditem.X = Boss.InfoChar.X;
                            goditem.Y = Boss.InfoChar.Y;
                            Boss.Zone.ZoneHandler.LeaveItemMap(goditem);
                        }
                        else if (ServerUtils.RandomNumber(50) <  15)
                        {
                            var item = ItemCache.GetItemDefault((short)457);
                            for (int i = 0; i <= 2; i++)
                            {
                                var itemMap = new ItemMap(playerKillId, item);
                                itemMap.X = (short)(Boss.InfoChar.X + (10 * i));
                                itemMap.Y = (short)(Boss.InfoChar.Y + (10 * i));
                                Boss.Zone.ZoneHandler.LeaveItemMap(itemMap);
                            }
                        }
                        break;
                    }
                case 98:
                    {
                        //character.InfoChar.PointSanBoss++;
                        //character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã tích lũy được 1 điểm săn boss"));
                        if (ServerUtils.RandomNumber(100) < 20)
                        {
                            var item = ItemCache.GetItemDefault(1243);
                            item.Options.ForEach(opt =>
                            {
                                if (opt.Id == 93) opt.Param = ServerUtils.RandomNumber(3, 7);
                                opt.Param = ServerUtils.RandomNumber(17, 25);
                            });
                            var itemMap = new ItemMap(playerKillId, item);
                            itemMap.X = Boss.InfoChar.X;
                            itemMap.Y = Boss.InfoChar.Y;
                            Boss.Zone.ZoneHandler.LeaveItemMap(itemMap);

                            for (int i = 0; i < 10; i++)
                            {
                                var item2 = ItemCache.GetItemDefault(190);
                                item2.Quantity = ServerUtils.RandomNumber(100000, 200000);
                                var itemMap2 = new ItemMap(Boss.KillerId, item2);
                                itemMap2.X = (short)(Boss.InfoChar.X + ServerUtils.RandomNumber(-50, 50));
                                itemMap2.Y = Boss.InfoChar.Y;
                                Boss.Zone.ZoneHandler.LeaveItemMap(itemMap2);
                            }
                            for (int i2 = 0; i2 < 10; i2++)
                            {
                                var item3 = ItemCache.GetItemDefault(381);
                                var itemMap3 = new ItemMap(Boss.KillerId, item3);
                                itemMap3.X = (short)(Boss.InfoChar.X + (i2 * 10));
                                itemMap3.Y = Boss.InfoChar.Y;
                                Boss.Zone.ZoneHandler.LeaveItemMap(itemMap3);
                            }
                        }
                    }
                    break;
                case 100:
                case 101:
                case 102:
                    {
                        var item = ItemCache.GetItemDefault((short)(Boss.Type == 100 ? 730 : Boss.Type == 101 ? 731 : 732));
                        item.Options.Add(new OptionItem()
                        {
                            Id = 93,
                            Param = ServerUtils.RandomNumber(1, 7)
                        });
                        var itemMap = new ItemMap(playerKillId, item);
                        itemMap.X = Boss.InfoChar.X;
                        itemMap.Y = Boss.InfoChar.Y;
                        Boss.Zone.ZoneHandler.LeaveItemMap(itemMap);
                        
                    }
                    break;
                case 99:
                    {
                       
                        if (ServerUtils.RandomNumber(100) < 20)
                        {
                            var item = ItemCache.GetItemDefault(1241);
                            item.Options.Add(new OptionItem()
                            {
                                Id = 50,
                                Param = ServerUtils.RandomNumber(17, 23),
                            });
                            item.Options.Add(new OptionItem()
                            {
                                Id = 77,
                                Param = ServerUtils.RandomNumber(17, 23),
                            });
                            item.Options.Add(new OptionItem()
                            {
                                Id = 103,
                                Param = ServerUtils.RandomNumber(17, 23),
                            });
                            item.Options.Add(new OptionItem()
                            {
                                Id = 14,
                                Param = ServerUtils.RandomNumber(17, 23),
                            });
                            item.Options.Add(new OptionItem()
                            {
                                Id = 5,
                                Param = ServerUtils.RandomNumber(17, 23),
                            });
                            item.Options.Add(new OptionItem()
                            {
                                Id = 31,
                                Param = 0,
                            });
                            item.Options.Add(new OptionItem()
                            {
                                Id = 93,
                                Param = ServerUtils.RandomNumber(3, 7),
                            });
                            var itemMap = new ItemMap(playerKillId, item);
                            itemMap.X = Boss.InfoChar.X;
                            itemMap.Y = Boss.InfoChar.Y;
                            Boss.Zone.ZoneHandler.LeaveItemMap(itemMap);

                            for (int i = 0; i < 10; i++)
                            {
                                var item2 = ItemCache.GetItemDefault(190);
                                item2.Quantity = ServerUtils.RandomNumber(100000, 200000);
                                var itemMap2 = new ItemMap(Boss.KillerId, item2);
                                itemMap2.X = (short)(Boss.InfoChar.X + ServerUtils.RandomNumber(-50, 50));
                                itemMap2.Y = Boss.InfoChar.Y;
                                Boss.Zone.ZoneHandler.LeaveItemMap(itemMap2);
                            }
                            for (int i2 = 0; i2 < 10; i2++)
                            {
                                var item3 = ItemCache.GetItemDefault(381);
                                var itemMap3 = new ItemMap(Boss.KillerId, item3);
                                itemMap3.X = (short)(Boss.InfoChar.X + (i2 * 10));
                                itemMap3.Y = Boss.InfoChar.Y;
                                Boss.Zone.ZoneHandler.LeaveItemMap(itemMap3);
                            }
                        }
                    }
                    break;
                
                case 95:
                case 97:
                    var countItemDrop = ServerUtils.RandomNumber(2, 9);
                    List<short> ListItemDrop = new List<short> { 1150, 1151, 1152, 1153, 1154 };
                    for (int i = 0; i < countItemDrop; i++)
                    {
                        var ItemDrop = ItemCache.GetItemDefault(ListItemDrop[ServerUtils.RandomNumber(ListItemDrop.Count)]);
                        var ItemMapDrop = new ItemMap(playerKillId, ItemDrop);
                        ItemMapDrop.X = (short)(Boss.InfoChar.X - (ServerUtils.RandomNumber(-50, 50)));
                        ItemMapDrop.Y = Boss.InfoChar.Y;
                        Boss.Zone.ZoneHandler.LeaveItemMap(ItemMapDrop);
                    }
                    break;
                case 80:
                case 81:
                case 64:
                    {
                        async void Yardat()
                        {
                            await Task.Delay(2500);
                            var yardat = new Boss();
                            yardat.CreateBossYardat(Boss.Type, Boss.Name, Boss.BasePositionX, Boss.BasePositionY);
                            yardat.CharacterHandler.SetUpInfo();
                            Boss.Zone.ZoneHandler.AddBoss(yardat);
                        }
                        var task = new Task(Yardat);
                        task.Start();
                        break;
                    }
                
                case 36:
                case 37:
                case 38:
                case 39:
                case 43:
                case 44:
                case 45:
                case 46:
                    {
                        var randomPc = ServerUtils.RandomNumber(100);
                        if (randomPc <= 15)
                        {

                            var itemMap = LeaveItemHandler.LeaveGodItem(character, "đánh boss");
                            itemMap.X = (short)(Boss.InfoChar.X + ServerUtils.RandomNumber(-10, 10));
                            itemMap.Y = Boss.InfoChar.Y;
                            Boss.Zone.ZoneHandler.LeaveItemMap(itemMap);
                        }
                    }
                    break;
                case 23:
                    {
                        async void SpawnHachijack()
                        {
                            await Task.Delay(1500);
                            var item = ItemCache.GetItemDefault(738);
                            item.Options.Add(new OptionItem()
                            {
                                Id = 93,
                                Param = ServerUtils.RandomNumber(1, 5)
                            });
                            for (int i = 0; i < 6; i++)
                            {
                                item.Options.FirstOrDefault(i => i.Id == 50).Param = (ServerUtils.RandomNumber(12, 25));
                                item.Options.FirstOrDefault(i => i.Id == 77).Param = (ServerUtils.RandomNumber(12, 25));
                                item.Options.FirstOrDefault(i => i.Id == 103).Param = (ServerUtils.RandomNumber(12, 25));

                                var itemMap = new ItemMap(playerKillId, item);
                                itemMap.X = (short)(Boss.InfoChar.X + ServerUtils.RandomNumber(-i * 10, i * 10));
                                itemMap.Y = Boss.InfoChar.Y;
                                Boss.Zone.ZoneHandler.LeaveItemMap(itemMap);
                            }
                        }
                        var task = new Task(SpawnHachijack);
                        task.Start();
                        break;
                    }
                case 67:
                    {
                        var clan = ClanManager.Get(character.ClanId);
                        var item = ItemCache.GetItemDefault(729);
                        item.Options.Add(new OptionItem()
                        {
                            Id = 93,
                            Param = ServerUtils.RandomNumber(1, 5)
                        });
                        for (int i = 0; i < 6; i++)
                        {
                            item.Options.FirstOrDefault(i => i.Id == 50).Param = (ServerUtils.RandomNumber(12, 25));
                            item.Options.FirstOrDefault(i => i.Id == 77).Param = (ServerUtils.RandomNumber(12, 25));
                            item.Options.FirstOrDefault(i => i.Id == 103).Param =(ServerUtils.RandomNumber(12, 25));
                            
                            var itemMap = new ItemMap(playerKillId, item);
                            itemMap.X = (short)(Boss.InfoChar.X + ServerUtils.RandomNumber(-i*10, i*10));
                            itemMap.Y = Boss.InfoChar.Y;
                            Boss.Zone.ZoneHandler.LeaveItemMap(itemMap);
                        }
                    }
                    break;
                case 47:
                case 48:
                case 49:
                case 50:
                case 51:
                    {

                        var itemMap = LeaveItemHandler.LeaveDoII(character, "đánh boss");
                        itemMap.X = (short)(Boss.InfoChar.X + ServerUtils.RandomNumber(-10, 10));
                        itemMap.Y = Boss.InfoChar.Y;
                        Boss.Zone.ZoneHandler.LeaveItemMap(itemMap);
                        if (ServerUtils.RandomNumber(100) < 15)
                        {
                            var item = ItemCache.GetItemDefault(611);
                            itemMap = new ItemMap(playerKillId, item);
                            itemMap.X = Boss.InfoChar.X;
                            itemMap.Y = Boss.InfoChar.Y;
                            Boss.Zone.ZoneHandler.LeaveItemMap(itemMap);
                        }
                    }
                    break;
                case 65:
                    {
                        var item = ItemCache.GetItemDefault(638);
                        item.Options.Add(new OptionItem()
                        {
                            Id = 30,
                            Param = 0,
                        });
                        item.Options.Add(new OptionItem()
                        {
                            Id = 93,
                            Param = 30,
                        });
                        var itemMap = new ItemMap(playerKillId, item);
                        itemMap.X = (short)(Boss.InfoChar.X + ServerUtils.RandomNumber(-10, 10));
                        itemMap.Y = Boss.InfoChar.Y;
                        Boss.Zone.ZoneHandler.LeaveItemMap(itemMap);
                        break;
                    }
                case 2://black goku
                case 3://super black goku
                {
                    var randomPercent = ServerUtils.RandomNumber(100);
                    if (randomPercent < 85)//20
                    {
                            var item = ItemCache.GetItemDefault(190);
                            randomPercent = ServerUtils.RandomNumber(100);
                            if (randomPercent <= 35) //giày thần 5 sao //25
                            {
                                item = ItemCache.GetItemDefault(LeaveItemHandler.LeaveGodItem(character, "đánh boss").Item.Id);
                                var soSaoGiayThan = ServerUtils.RandomNumber(1, 5);
                                item.Options.Add(new OptionItem()
                                {
                                    Id = 107,
                                    Param = soSaoGiayThan
                                });
                            }

                            else //Ngọc rồng 
                            {

                                item = ItemCache.GetItemDefault(16);


                            }

                        if (randomPercent < 60)
                        {
                            var itemNhan = ItemCache.GetItemDefault(992);
                            var nhanthoikhong = new ItemMap(playerKillId, itemNhan);
                            nhanthoikhong.X = Boss.InfoChar.X;
                            nhanthoikhong.Y = Boss.InfoChar.Y;
                            Boss.Zone.ZoneHandler.LeaveItemMap(nhanthoikhong);
                        }
                         var itemMap2 = new ItemMap(playerKillId, item);
                        itemMap2.X = Boss.InfoChar.X;
                        itemMap2.Y = Boss.InfoChar.Y;
                        Boss.Zone.ZoneHandler.LeaveItemMap(itemMap2);
                       
                    }
                    
                    // Rớt vàng 100tr
                    for (int i = 0; i < 10; i++)
                    {
                        var item2 = ItemCache.GetItemDefault(190);
                        item2.Quantity = ServerUtils.RandomNumber(5000000, 10000000);
                        var itemMap2 = new ItemMap(playerKillId, item2);
                        itemMap2.X = (short)(Boss.InfoChar.X + ServerUtils.RandomNumber(-10, 10));
                        itemMap2.Y = Boss.InfoChar.Y;
                        Boss.Zone.ZoneHandler.LeaveItemMap(itemMap2);
                    }



                        var itemMap3 = LeaveItemHandler.LeaveDoII(character, "đánh boss");
                            itemMap3.X = (short)(Boss.InfoChar.X + ServerUtils.RandomNumber(-10, 10));
                            itemMap3.Y = Boss.InfoChar.Y;
                            Boss.Zone.ZoneHandler.LeaveItemMap(itemMap3);

                        break;
                }
                case 10:
                case 11:
               case 63:
                    {
                        var randomPercent = ServerUtils.RandomNumber(100);
                        if (randomPercent < 80)
                        {
                            var randomPc = ServerUtils.RandomNumber(100);
                            if (randomPc <= 135)
                            {
                               
                                Boss.Zone.ZoneHandler.LeaveItemMap(LeaveItemHandler.LeaveGodItem(character, "đánh boss"));
                            }

                          

                            
                        }

                        // Rớt vàng 100tr
                        for (int i = 0; i < 10; i++)
                        {
                            var item = ItemCache.GetItemDefault(190);
                            item.Quantity = ServerUtils.RandomNumber(5000000, 10000000);
                            var itemMap = new ItemMap(playerKillId, item);
                            itemMap.X = (short)(Boss.InfoChar.X + ServerUtils.RandomNumber(-10, 10));
                            itemMap.Y = Boss.InfoChar.Y;
                            Boss.Zone.ZoneHandler.LeaveItemMap(itemMap);
                        }
                    }
                    break;
                case 24://kuku
                case 25://mdđ
                case 26://rambo
                case 27://pic
                case 28://poc
                case 29://kingkong
                case 30://and 19
                case 31://dr kore
                case 4://fide 1
                case 5://fide 1 2
                case 6://fide 3
                    {
                        var itemMapJean = LeaveItemHandler.LeaveDoII(character, "đánh boss");
                        itemMapJean.X = Boss.InfoChar.X;
                        itemMapJean.Y = Boss.InfoChar.Y;
                        Boss.Zone.ZoneHandler.LeaveItemMap(itemMapJean);
                        var phanTramRoiDo = ServerUtils.RandomNumber(0, 100);
                        if (phanTramRoiDo <= 20)//35
                        {
                            var item = ItemCache.GetItemDefault(16);
                            var itemMap = new ItemMap(playerKillId, item);
                            itemMap.X = Boss.InfoChar.X;
                            itemMap.Y = Boss.InfoChar.Y;
                            Boss.Zone.ZoneHandler.LeaveItemMap(itemMap);
                        }
                        // Rớt vàng 20-30
                        for (int i = 0; i < 10; i++)
                        {
                            var item = ItemCache.GetItemDefault(190);
                            item.Quantity = ServerUtils.RandomNumber(2000000, 3000000);
                            var itemMap = new ItemMap(playerKillId, item);
                            itemMap.X = (short)(Boss.InfoChar.X + ServerUtils.RandomNumber(-10, 10));
                            itemMap.Y = Boss.InfoChar.Y;
                            Boss.Zone.ZoneHandler.LeaveItemMap(itemMap);
                        }

                        var item2 = ItemCache.GetItemDefault(1517);
                        var itemMap2 = new ItemMap(playerKillId, item2);
                        itemMap2.X = (short)(Boss.InfoChar.X + ServerUtils.RandomNumber(-10, 10));
                        itemMap2.Y = Boss.InfoChar.Y;
                        Boss.Zone.ZoneHandler.LeaveItemMap(itemMap2);
                        break;
                    }
                case 7:
                case 8:
                case 9:
                {
                    var randomPercent = ServerUtils.RandomNumber(100);
                    if (randomPercent < 25)
                    {
                        var item = ItemCache.GetItemDefault(190);
                        var listQuanThan = new List<short>(){556,558,560};
                        var listAoThan = new List<short>(){555,557,559};
                        randomPercent = ServerUtils.RandomNumber(100); 
                        if (randomPercent <= 35) //quan thần 5 sao
                        {
                            item = ItemCache.GetItemDefault(listQuanThan[ServerUtils.RandomNumber(listQuanThan.Count)]);
                        }
                        else if (randomPercent <= 55) //ao thần 5 sao
                        {
                            item = ItemCache.GetItemDefault(listAoThan[ServerUtils.RandomNumber(listAoThan.Count)]);
                        }
                            else //Ngọc rồng 
                            {
                                if (ServerUtils.RandomNumber(100) < 40) //2s
                                {
                                    item = ItemCache.GetItemDefault(15);
                                }
                                else //3s 
                                {
                                    item = ItemCache.GetItemDefault(16);
                                }
                            }

                            var itemMap = new ItemMap(playerKillId, item);
                        itemMap.X = Boss.InfoChar.X;
                        itemMap.Y = Boss.InfoChar.Y;
                        Boss.Zone.ZoneHandler.LeaveItemMap(itemMap);
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        var item = ItemCache.GetItemDefault(190);
                        item.Quantity = ServerUtils.RandomNumber(1000000, 5000000);
                        var itemMap = new ItemMap(playerKillId, item);
                        itemMap.X = (short)(Boss.InfoChar.X + ServerUtils.RandomNumber(-10, 10));
                        itemMap.Y = Boss.InfoChar.Y;
                        Boss.Zone.ZoneHandler.LeaveItemMap(itemMap);
                    }
                    break;
                }
             
              
                case 12://tho phe co
                {
                    var item = ItemCache.GetItemDefault(670);
                    var itemMap = new ItemMap(-1, item);
                    itemMap.X = Boss.InfoChar.X;
                    itemMap.Y = Boss.InfoChar.Y;
                    Boss.Zone.ZoneHandler.LeaveItemMap(itemMap);
                    break;
                }
                case 13://tho đại ca
                {
                    var listItem = new List<short>(){462,886,887,888,889,891,462,886,887,888,889};
                    for (int i = 0; i < ServerUtils.RandomNumber(10, 20); i++)
                    {
                        var item = ItemCache.GetItemDefault(listItem[ServerUtils.RandomNumber(listItem.Count)]);
                        item.Quantity = ServerUtils.RandomNumber(1, 5);
                        var itemMap = new ItemMap(-1, item);
                        itemMap.X = (short)(Boss.InfoChar.X + ServerUtils.RandomNumber(-100, 100));
                        itemMap.Y = Boss.InfoChar.Y;
                        Boss.Zone.ZoneHandler.LeaveItemMap(itemMap);
                    }
                    break;
                }
                case 14:
                {
                   

                    var randomPercent = ServerUtils.RandomNumber(100);
                    
                     if (randomPercent <= 20) // rơi cải trang ngày
                    {
                        var timeServer = ServerUtils.CurrentTimeSecond();
                        var expireDay = ServerUtils.RandomNumber(1, 3);
                        var expireTime = timeServer + (expireDay*86400);
                        var itemCaiTrang = ItemCache.GetItemDefault(985);
                                    

                        itemCaiTrang.Options.Add(new OptionItem()
                        {
                            Id = 93,
                            Param = expireDay,
                        });
                        var optionHiden = itemCaiTrang.Options.FirstOrDefault(option => option.Id == 73);
                        
                        if (optionHiden != null) 
                        {
                            optionHiden.Param = expireTime;
                        }
                        else 
                        {
                            itemCaiTrang.Options.Add(new OptionItem()
                            {
                                Id = 73,
                                Param = expireTime,
                            });
                        }
                        
                        var itemMapThucVat = new ItemMap(playerKillId, itemCaiTrang);
                        itemMapThucVat.X = Boss.InfoChar.X;
                        itemMapThucVat.Y = Boss.InfoChar.Y;
                        Boss.Zone.ZoneHandler.LeaveItemMap(itemMapThucVat);
                    }
                    else if (randomPercent <= 50)
                    {
                        var doJean = ItemCache.GetItemDefault(DataCache.ListDoHiem[ServerUtils.RandomNumber(DataCache.ListDoHiem.Count)]);
                        var soSao = ServerUtils.RandomNumber(0, 5);
                        if (soSao > 0)
                        {
                            doJean.Options.Add(new OptionItem()
                            {
                                Id = 107,
                                Param = soSao
                            });
                        }
                        var itemMapJean = new ItemMap(playerKillId, doJean);
                        itemMapJean.X = Boss.InfoChar.X;
                        itemMapJean.Y = Boss.InfoChar.Y;
                        Boss.Zone.ZoneHandler.LeaveItemMap(itemMapJean);
                    }
                    else // hồn
                    {
                        var itemHonLinhThu = ItemCache.GetItemDefault(1152);
                        itemHonLinhThu.Quantity = ServerUtils.RandomNumber(5, 10);
                        var itemMapThucVat = new ItemMap(playerKillId, itemHonLinhThu);
                        itemMapThucVat.X = Boss.InfoChar.X;
                        itemMapThucVat.Y = Boss.InfoChar.Y;
                        Boss.Zone.ZoneHandler.LeaveItemMap(itemMapThucVat);
                    }
                    break;
                }
            }

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
            
            Boss.HpFull = hp;
        }

        public void SetMpFull()
        {
            var mp = Boss.InfoChar.OriginalMp;
            
            Boss.MpFull = mp;
        }

        public void SetDamageFull()
        {
            var damage = Boss.InfoChar.OriginalDamage;
           
            if (Boss.InfoSkill.Monkey.MonkeyId != 0) damage += damage * Boss.InfoSkill.Monkey.Damage / 100;
            Boss.DamageFull = damage;
        }

        public void SetDefenceFull()
        {
            var defence = Boss.InfoChar.OriginalDefence * 4;
           
            Boss.DefenceFull = Math.Abs(defence);
        }

        public void SetCritFull()
        {
            int crtCal;
            
                crtCal = Boss.InfoChar.OriginalCrit;
            
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
            

            Boss.InfoChar.Speed = (sbyte)speed;
        }

        public void SetBuffHp30s()
        {
           
            
        }

        public void SetBuffMp1s()
        {
            
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
            if(Boss.IsDontMove()) return;

            var compare = Math.Abs(Boss.InfoChar.X - toX);
            if (compare >= 50)
            {
                if (Boss.InfoChar.X < toX)
                {
                    Boss.InfoChar.X = compare switch
                    {
                        >= 150 => (short) (toX - 70),
                        _ => (short) (toX - 50)
                    };
                }
                else
                {
                    Boss.InfoChar.X = compare switch
                    {
                        >= 150 => (short) (toX + 70),
                        _ => (short) (toX + 50)
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
                if(Boss.InfoChar.IsDie) return;
                Boss.InfoChar.Hp += hp;
                if (Boss.InfoChar.Hp >= Boss.HpFull) Boss.InfoChar.Hp = Boss.HpFull;
            }
        }

        public void MineHp(long hp)
        {
            lock (Boss.InfoChar)
            {
                if(Boss.InfoChar.IsDie || hp <= 0) return;

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
                if(Boss.InfoChar.IsDie) return;
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
            if (skillEgg.Monster is {IsDie: true})
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
        public void Leave()
        {
            Boss.Zone.ZoneHandler.RemoveBoss(Boss);
            Boss = null;
            Dispose();
            Boss.InfoChar.IsDie = true;
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
                SkillHandler.HandleMonkey(Boss,false);
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
                if (monsterMap is {IsDie: true})
                {
                    SkillHandler.RemoveTroi(Boss);
                }
                else if (charTemp != null && charTemp.InfoChar.IsDie)
                {
                    SkillHandler.RemoveTroi(Boss);
                }
                else if (infoSkill.MeTroi.TimeTroi <= timeServer || monsterMap is {IsDie: true} || charTemp != null && charTemp.InfoChar.IsDie)
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