using System;
using System.Collections.Generic;
using NgocRongGold.Model;
using NgocRongGold.Model.Info;
using NgocRongGold.Model.Map;
using NgocRongGold.Model.SkillCharacter;
using NgocRongGold.Application.Extension;
using NgocRongGold.Application.Extension.Chẵn_Lẻ_Momo;
using NgocRongGold.Model.Task;
using NgocRongGold.Application.Extension.ChampionShip.ChampionShip_23;
using NgocRongGold.Application.Extension.Namecball;
using NgocRongGold.Application.Extension.Transfaction.Thách_Đấu;
using NgocRongGold.Application.Extension.DaiHoiVoThuat;
using NgocRongGold.Application.Extension.DiedRing;
using NgocRongGold.Application.Extension.NamecBattlefield;
using NgocRongGold.Application.Extension.DaiHoiVoThuat.SuperChampion;
using NgocRongGold.Model.Clan;
using NgocRongGold.Application.Extension.SideQuest.BoMong;
using Application.Interfaces.Zone;

namespace NgocRongGold.Application.Interfaces.Character
{
    public interface ICharacter : IDisposable
    {
        ICharacterHandler CharacterHandler { get; set; }
         InfoSkill InfoSkill { get; set; }
         IZone Zone { get; set; }
         int Id { get; set; }
       
        int ClanId { get; set; }
        Clan Clan { get; set; }
         Player Player { get; set; }
         string Name { get; set; }
         InfoChar InfoChar { get; set; }
        TaskInfo InfoTask { get; set; }
        long DameBossBangHoi { get; set; }
        int TypeTeleport { get; set; }
         sbyte Flag { get; set; }
         long HpFull { get; set; }
         long MpFull { get; set; }
         int DamageFull { get; set; }
         int DefenceFull { get; set; }
         int CritFull { get; set; }
         int HpPlusFromDamage { get; set; }
         int MpPlusFromDamage { get; set; }
         int HpPlusFromDamageMonster { get; set; }
         bool IsGetHpFull { get; set; }
         bool IsGetMpFull { get; set; }
         bool IsGetDamageFull { get; set; }
         bool IsGetDefenceFull { get; set; }
         bool IsGetCritFull { get; set; }
         bool IsHpPlusFromDamage { get; set; }
         bool IsMpPlusFromDamage { get; set; }
         int DiemSuKien { get; set; }
        
         List<SkillCharacter> Skills { get; set; }
         List<Model.Item.Item> ItemBody { get; set; }
         List<Model.Item.Item> ItemBag { get; set; }
         List<Model.Item.Item> ItemBox { get; set; }
         InfoOption InfoOption { get; set; }
         InfoSet InfoSet { get; set; }
      
         /// <summary>
        int TypeDragon { get; set; }
         /// </summary>
         /// <returns></returns>
         bool IsInvisible();
         short GetHead(bool isMonkey = true);
         short GetBody(bool isMonkey = true);
         short GetLeg(bool isMonkey = true);
         void PlusGold(int gold);
         short GetBag();
         void SetGetFull(bool isGet);
         int LengthBagNull();
         int LengthBoxNull();

         int BagLength();
         int BoxLength();
         int BodyLength();
         bool CheckLockInventory();
         bool IsDontMove();

    }
}