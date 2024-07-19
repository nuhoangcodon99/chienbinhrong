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
using Application.Interfaces.Zone;

namespace NgocRongGold.Application.Handlers.Character
{
    public class Pet2Handler : ICharacterHandler
    {
        public int GetAllQuantityItemBagById(int id)
        {
            return 0;
        }
        public Pet2 Pet { get; set; }
        public Model.Item.Item GetItemClanBoxByIndex(int index)
        {
            return null;
        }
        public void UpdateEffectTemporary(long timeServer)
        {

        }
        public void SetEnhancedOptionCard()
        {

        }
        public void QueryItem()
        {

        }
        public void UpdateEffective()
        {

        }
        public void OpenUiSay(string say)
        {

        }
        public void SendServerMessage(string say)
        {

        }
        public void UpdateOther(long timeServer){   
                }
        public void SetupAmulet()
        {

        }
        public void CreatePetNormal()
        {

        }
        public Model.Item.Item RemoveItemClanBox(int index, bool isReset = true)
        {
            return null;

        }
        public Model.Item.Item RemoveItemGiftBox(int index, bool isReset = true)
        {
            return null;

        }
        public void UpdatePet()
        {
            //ingored
        }
        public Pet2Handler(Pet2 pet)
        {
            Pet = pet;
        }
        public int GetThoiVangInRuong()
        {
            return -1;
            //ingored
        }
        
        public void SetUpPhoBan()
        {

        }
        public void UpdateEffectCharacter()
        {
            //ingored
        }
        public void UpdateItem10()
        {
            //ingored
        }
        public Model.Item.Item GetItemBodyByIndex(int index)
        {
            return null;
        }
        public int GetThoiVangInBag()
        {
            return -1;
            //ingored
        }
        public void PlusDiamondLock(int diamond)
        {
            //ingored
        }
        public void Dispose()
        {
            SuppressFinalize(this);
        }

        public void SendZoneMessage(Message message)
        {
            Pet?.Zone?.ZoneHandler.SendMessage(message);
        }

        public void Update()
        {
            lock (Pet)
            {
                var timeServer = ServerUtils.CurrentTimeMillis();
                AutoPet(timeServer);
            }
        }

        private void AutoPet(long timeServer)
        {
            if(Pet.IsDontMove()) return;
            //if (Pet.DelayAutoMove < timeServer)
            //{
            //    SetUpPosition(isRandom:true);
            //    SendZoneMessage(Service.PlayerMove(Pet.Id, Pet.InfoChar.X, Pet.InfoChar.Y));
            //}
            AutoMoveMap(timeServer);
        }

        private void AutoMoveMap(long timeServer, bool isForce = false)
        {
            if (Pet.Character != null)
            {
                int distance = Math.Abs(Pet.InfoChar.X - Pet.Character.InfoChar.X);
                if (distance > 300)
                {
                    // Đặt Character thành null
                    Pet.Character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã đi quá xa kì lân nên nó không thèm đi nữa, hãy dắt lại nào"));
                    Pet.Character = null;
                    SendChatAll("Xa quá, không thèm đi nữa đâu");
                }
                else
                {
                    if ((Pet.DelayAutoMove <= timeServer) || isForce)
                    {
                        // Tính toán khoảng cách giữa vị trí hiện tại của Pet và vị trí của Character

                        // Nếu khoảng cách vẫn lớn hơn một ngưỡng cho trước
                        if (distance > 30)
                        {
                            // Xác định hướng di chuyển của Pet (phải hoặc trái)
                            int direction = Math.Sign(Pet.Character.InfoChar.X - Pet.InfoChar.X);

                            // Cập nhật vị trí của Pet dần dần tới vị trí của Character
                            Pet.InfoChar.X += (short)(direction * 30);

                            // Gửi message về việc di chuyển của Pet
                            SendZoneMessage(Service.PlayerMove(Pet.Id, Pet.InfoChar.X, Pet.InfoChar.Y));

                            // Thiết lập thời gian chờ cho lần di chuyển tiếp theo của Pet
                            Pet.DelayAutoMove = timeServer + ServerUtils.RandomNumber(50, 200);
                        }
                    }
                }
            }
            else
            {
                if ((Pet.DelayAutoMove <= timeServer) || isForce)
                {
                    Pet.InfoChar.X = (short)ServerUtils.RandomNumber(Pet.InfoChar.X - 30,
                        Pet.InfoChar.X + 30);
                    SendZoneMessage(Service.PlayerMove(Pet.Id, Pet.InfoChar.X, Pet.InfoChar.Y));
                    Pet.DelayAutoMove = timeServer + ServerUtils.RandomNumber(1000, 3000);
                    SendChatAll(" Ngọc Rồng chúc mừng năm mới !!!");
                }
            }
        }

        private void SendChatForSp(string text)
        {
        }
        private void SendChatAll(string text)
        {
            SendZoneMessage(Service.PublicChat(Pet.Id,text));
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
            SendZoneMessage(Service.UpdateBody(Pet));
        }

        public void SetUpPosition(int mapOld = -1, int mapNew = -1, bool isRandom = false)
        {
            if (Pet.Character != null)
            {
                if (isRandom)
                {
                    Pet.InfoChar.X = (short)(Pet.Character.InfoChar.X + 15);
                }
                else
                {
                    Pet.InfoChar.X = (short)(Pet.Character.InfoChar.X);
                }
                Pet.InfoChar.Y = Pet.Character.InfoChar.Y;
            }
            else
            {
                if (isRandom)
                {
                    Pet.InfoChar.X = (short)(Pet.InfoChar.X + 15);
                }
                else
                {
                    Pet.InfoChar.X = Pet.InfoChar.X;
                }
                Pet.InfoChar.Y = Pet.InfoChar.Y;
            }
        }
        

        public void PlusHp(int hp)
        {
            lock (Pet.InfoChar)
            {
                if(Pet.InfoChar.IsDie) return;
                Pet.InfoChar.Hp += hp;
                if (Pet.InfoChar.Hp >= Pet.HpFull) Pet.InfoChar.Hp = Pet.HpFull;
            }
        }

        public void MineHp(long hp)
        {
            lock (Pet.InfoChar)
            {
                if(Pet.InfoChar.IsDie || hp <= 0) return;
                if (hp > Pet.InfoChar.Hp)
                {
                    Pet.InfoChar.Hp = 0;
                }
                else 
                {
                    Pet.InfoChar.Hp -= hp;
                }

                if (Pet.InfoChar.Hp <= 0)
                {
                    Pet.InfoChar.IsDie = true;
                    Pet.InfoChar.Hp = 0;
                }
            }
        }

        
        public void MoveMap(short toX, short toY, int type = 0)
        {
            var timeServer = ServerUtils.CurrentTimeMillis();
            if(Pet.IsDontMove()) return;

            var compare = Math.Abs(Pet.InfoChar.X - toX);
            if (compare >= 50)
            {
                if (Pet.InfoChar.X < toX)
                {
                    Pet.InfoChar.X = compare switch
                    {
                        >= 150 => (short) (toX - 50),
                        _ => (short) (toX - 30)
                    };
                }
                else
                {
                    Pet.InfoChar.X = compare switch
                    {
                        >= 150 => (short) (toX + 50),
                        _ => (short) (toX + 30)
                    };
                }

                if (toY != Pet.InfoChar.Y)
                {
                    Pet.InfoChar.Y = toY;
                }

                SendZoneMessage(Service.PlayerMove(Pet.Id, Pet.InfoChar.X, Pet.InfoChar.Y));
                if (Pet.InfoSkill.MeTroi.IsMeTroi && Pet.InfoSkill.MeTroi.DelayStart <= timeServer)
                {
                    SkillHandler.RemoveTroi(Pet);
                }
            }
        }


        #region Ignored Function
        private void HandleUseSkill(bool isAuto = true, int charId = -1, int modId = -1)
        {
            

        }

        public void SendDie()
        {

        }

        public void SendInfo()
        {
        }

        public int GetParamItem(int id)
        {
            return 0;
        }

        public List<int> GetListParamItem(int id)
        {
            return null;
        }

        public void SetUpInfo(bool queryItem = false)
        {
        }

        public void SetInfoSet()
        {
        }

        public void LeaveFromDead(bool isHeal = false)
        {

        }

        public void SetEnhancedOption()
        {
        }

        public void SetHpFull()
        {
        }

        public void SetMpFull()
        {
        }

        public void SetDamageFull()
        {
        }

        public void SetDefenceFull()
        {
        }

        public void SetCritFull()
        {
        }

        public void SetHpPlusFromDamage()
        {
        }

        public void SetMpPlusFromDamage()
        {
        }

        public void SetSpeed()
        {
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
        public void PlusMp(int mp)
        {
        }

        public void MineMp(int mp)
        {
        }

        public void PlusStamina(int stamina)
        {
        }

        public void MineStamina(int stamina)
        {
        }

        public void PlusPower(long power)
        {
        }

        public void PlusPotential(long potential)
        {
        }

        public Model.Item.Item RemoveItemBody(int index)
        {
            return null;
        }

        public void AddItemToBody(Model.Item.Item item, int index)
        {
        }

        public void RemoveMonsterMe()
        {
        }
        public void PlusTiemNang(IMonster monster, int damage)
        {
            
        }

        public void PlusTiemNang(long power, long potential, bool isAll)
        {

        }

        public void RemoveSkill(long timeServer, bool globalReset = false)
        {
            // ignore
        }

        public void UpdateEffect(long timeServer)
        {
            // ignore
        }

        public void UpdateMask()
        {
            // ignore
        }

        public void UpdateAutoPlay(long timeServer)
        {
            
        }

        public void UpdateLuyenTap()
        {
            
        }

        public void RemoveTroi(int charId)
        {
            // ignore
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
            //Pet join map
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