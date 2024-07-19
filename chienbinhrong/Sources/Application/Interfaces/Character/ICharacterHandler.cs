using System;
using System.Collections.Generic;
using Application.Interfaces.Zone;
using NgocRongGold.Application.Interfaces.Monster;
using NgocRongGold.Application.IO;
using NgocRongGold.Model;
using NgocRongGold.Model.Map;

namespace NgocRongGold.Application.Interfaces.Character
{
    public interface ICharacterHandler : IDisposable
    {
        void UpdateEffectTemporary(long timeServer);

        void SetPlayer(Player player);
        void SendMessage(Message message);
        void SendZoneMessage(Message message);
        void SendMeMessage(Message message);
        void Update();
        void SetUpPhoBan();
        void Close();
        void UpdateInfo(bool QueryItem = false);
        void OpenUiSay(string say);
        void SendServerMessage(string say);
        void SetUpPosition(int mapOld = -1, int mapNew = -1, bool isRandom = false);
        public int GetThoiVangInBag();
        public int GetThoiVangInRuong();
        void SendInfo();
        void SendDie();
        public int GetParamItem(int id);
        void UpdateEffective();
        public List<int> GetListParamItem(int id);
        public void CreatePetNormal();
        public void SetUpFriend();
        public void SetUpInfo(bool queryItem = false);
        public void SetEnhancedOptionCard();
        public void QueryItem();
        public void SetHpFull();
        public void SetupAmulet();

        public void SetMpFull();

        public void SetDamageFull();
        public void SetDefenceFull();
        public void SetCritFull();
        public void SetHpPlusFromDamage();
        public void SetMpPlusFromDamage();

        public void HandleJoinMap(IZone zone);

        public void SetSpeed();

        public void BagSort();
        public void BoxSort();
        public void Upindex(int index);
        bool AddItemToBag(bool isUpToUp, Model.Item.Item item, string reason = "");
        
        void AddItemToBody(Model.Item.Item item, int index);

        bool AddItemToBox(bool isUpToUp, Model.Item.Item item);
        Model.Item.Item ItemBagNotMaxQuantity(short id);
        void RemoveItemBagById(short id, int quantity, string reason = "");
        void RemoveItemBagByIndex(int index, int quantity, bool reset = true, string reason = "");
        void RemoveItemBoxByIndex(int index, int quantity, bool reset = true);
        Model.Item.Item RemoveItemBag(int index, bool isReset = true, string reason = "");
        Model.Item.Item RemoveItemBox(int index, bool isReset = true);
        Model.Item.Item RemoveItemLuckyBox(int index, bool isReset = true);
        Model.Item.Item RemoveItemGiftBox(int index, bool isReset = true);

        Model.Item.Item RemoveItemClanBox(int index, bool isReset = true);
        void MoveMap(short toX, short toY, int type = 0);
        void PlusHp(int hp);
        void MineHp(long hp);
        void PlusMp(int mp);
        void MineMp(int mp);
        void PlusStamina(int stamina);
        void UpdateEffectCharacter();
        void UpdatePet();
        void UpdateItem10();
        void MineStamina(int stamina);
        void PlusPower(long power);
        void PlusPotential(long potential);
        Model.Item.Item RemoveItemBody(int index);
        void DropItemBody(int index);
        void DropItemBag(int index);
        void PickItemMap(short id);
        void UpdateMountId();
        void UpdatePhukien();

        Model.Item.Item GetItemBagByIndex(int index);
        Model.Item.Item GetItemBodyByIndex(int index);
        Model.Item.Item GetItemBagById(int id);
        int GetAllQuantityItemBagById(int id);

        Model.Item.Item GetItemBoxByIndex(int index);
        Model.Item.Item GetItemLuckyBoxByIndex(int index);
        Model.Item.Item GetItemClanBoxByIndex(int index);
        Model.Item.Item GetItemBoxById(int id);
        void RemoveMonsterMe();
        void PlusTiemNang(IMonster monster, int damage);
        void PlusTiemNang(long power, long potential, bool isAll);
        void LeaveFromDead(bool isHeal = false);
        void LeaveItem(ICharacter character);
        void BackHome();
        void UpdateOther(long timeServer);
        void RemoveSkill(long timeServer, bool globalReset = false);
        void UpdateEffect(long timeServer);
        void UpdateMask();
        void UpdateAutoPlay(long timeServer);
        void UpdateLuyenTap();

        void ClearTest();
        void RemoveTroi(int charId);
        void SetBuffHp30s();
        void SetBuffMp1s();
        void SetBuffHp5s();
        void SetBuffHp10s();
        void Clear();
        void PlusDiamondLock(int diamond);
    }
}