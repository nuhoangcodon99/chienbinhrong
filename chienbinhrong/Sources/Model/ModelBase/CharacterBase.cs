using System;
using System.Collections.Generic;
using System.Linq;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.DatabaseManager;
using NgocRongGold.Model.Info;
using NgocRongGold.Model.Map;
using NgocRongGold.Application.Extension.Bosses;
using NgocRongGold.Sources.Base.Info;
using NgocRongGold.Application.Extension.Namecball;
using static System.GC;
using NgocRongGold.Model.Task;
using NgocRongGold.Application.Extension.BlackballWar;
using NgocRongGold.Application.Extension.Bo_Mong;
using NgocRongGold.Application.Extension.ChampionShip.ChampionShip_23;
using NgocRongGold.Application.Extension.Ký_gửi;
using NgocRongGold.Application.Extension;
using NgocRongGold.Application.Extension.Transfaction.Thách_Đấu;
using NgocRongGold.Application.Extension.Chẵn_Lẻ_Momo;
using static NgocRongGold.Application.Extension.Super_Champion.SieuHang;
using static NgocRongGold.Application.Extension.Namecball.NamecBallHandler;
using NgocRongGold.Application.Extension.DaiHoiVoThuat;
using NgocRongGold.Application.Extension.DiedRing;
using NgocRongGold.Application.Extension.NamecBattlefield;
using NgocRongGold.Application.Extension.DaiHoiVoThuat.SuperChampion;
using NgocRongGold.Application.Extension.Practice;
using NgocRongGold.Application.Extension.SideQuest.BoMong;
using NgocRongGold.Model.Character;
using NgocRongGold.Application.Extension.SideQuest.HangNgay;
using NgocRongGold.Application.Threading;
using Application.Interfaces.Zone;

namespace NgocRongGold.Model.ModelBase
{
    public class CharacterBase : ICharacter
    {
        public Model.Clan.Clan Clan { get; set; }
        public ICharacterHandler CharacterHandler { get; set; }
        public IZone Zone { get; set; }
        public int Id { get; set; }
        public int ClanId { get; set; }
        public Player Player { get; set; }
        public InfoFriend Info { get; set; }
        public string Name { get; set; }
        public InfoChar InfoChar { get; set; }
        
        public long DameBossBangHoi { get; set; }
        public int TypeTeleport { get; set; }
        public int TypeDragon { get; set; }
        public sbyte Flag { get; set; }
        public long HpFull { get; set; }
        public long MpFull { get; set; }
        public int DamageFull { get; set; }
        public int DefenceFull { get; set; }
        public int CritFull { get; set; }
        public int HpPlusFromDamage { get; set; }
        public int MpPlusFromDamage { get; set; }
        public int HpPlusFromDamageMonster { get; set; }
        public bool IsGetHpFull { get; set; }
        public bool IsGetMpFull { get; set; }
        public bool IsGetDamageFull { get; set; }
        public bool IsGetDefenceFull { get; set; }
        public bool IsGetCritFull { get; set; }
        public bool IsHpPlusFromDamage { get; set; }
        public bool IsMpPlusFromDamage { get; set; }
        public int DiemSuKien { get; set; }
        public InfoDelay Delay { get; set; }

        public List<SkillCharacter.SkillCharacter> Skills { get; set; }
        public List<Item.Item> ItemBody { get; set; }
        public List<Item.Item> ItemBag { get; set; }
        public List<Item.Item> ItemBox { get; set; }
        public InfoSkill InfoSkill { get; set; }
        public Effect.Effect Effect { get; set; }
        public InfoOption InfoOption { get; set; }
        public InfoSet InfoSet { get; set; }
        public InfoDamage InfoDamage { get; set; }
        public TaskInfo InfoTask { get; set; }
        public bool IsInvisible()
        {
            // return ItemBody[5] != null && ItemBody[5].Options.FirstOrDefault(i => i.Id == 105) != null;
            return false;
        }

        public CharacterBase()
        {
            //if (this.GetType() == typeof(Character.Character))
            //{
            //    //khi login game chứ không phải tạo acc mới
            //    DataPractice = new Practice_Data();
            //    DataNamecBattlefield = new NamecBattlefield_Character();
            //    InfoDamage = new InfoDamage();
            //    DataSideTask = new HangNgayQuest_Data();
            //    DameBossBangHoi = 0;
            //    DataSieuHang = new SuperChampion_Championer();
            //    DataSieuHang.Top = SuperChampion_Manager.Entrys.Count + 1;
            //    SuperChampion_Manager.Entrys.TryAdd(DataSieuHang.Top, DataSieuHang);
            //    Blackball = new BlackBallHandler.ForPlayer();
            //    DataBoMong = new BoMongQuest_Data();
            //    DataBoMong.Create();
            //    DataDaiHoiVoThuat23 = new ChampionerCS23();
            //    DataDaiHoiVoThuat = new Championer();
            //    DataVoDaiSinhTu = new DiedRing_Character();
            //    DataNgocRongNamek = new DataNgocRongNamek();
            //    InfoTask = new TaskInfo();
            //    DataEnchant = new Enchant();
            //    Server.Gi().Logger.Print("basezzz");
            //}
            Delay = new InfoDelay();
            InfoTask = new TaskInfo();
            Effect = new Effect.Effect();
            InfoSet = new InfoSet();
            InfoOption = new InfoOption();
            ClanId = -1;
            Flag = 0;
            InfoChar = new InfoChar();
            HpFull = InfoChar.OriginalHp;
            MpFull = InfoChar.OriginalMp;
            DamageFull = InfoChar.OriginalDamage;
            DefenceFull = InfoChar.OriginalDefence;
            CritFull = InfoChar.OriginalCrit;
            Skills = new List<SkillCharacter.SkillCharacter>();
            ItemBody = new List<Item.Item>(BodyLength());
            for(var i = 0; i < BodyLength(); i++) ItemBody.Add(null);
            ItemBag = new List<Item.Item>();
            ItemBox = new List<Item.Item>();
            InfoSkill = new InfoSkill();
           
            SetGetFull(true);
           
        }
        public void PlusGold(int gold)
        {
            
        }
        public virtual short GetHead(bool isMonkey = true)
        {
            if (InfoSkill.socolaMabu.isSocola) return 1285;
            else if (InfoSkill.MaPhongBa.isMaPhongBa) return 1221;
            else if (InfoSkill.HoaBang.isHoaBang) return 1210;
            else if (isMonkey && InfoSkill.Socola.IsCarot) return 406;
            else if (isMonkey && InfoSkill.Socola.IsSocola) return 412;
            else if (isMonkey && InfoSkill.HoaDa.IsHoaDa) return 454;
            else if (isMonkey && InfoSkill.Monkey.HeadMonkey != -1) return InfoSkill.Monkey.HeadMonkey;
            else
            {
                if (InfoChar.Fusion.IsFusion)
                {
                    if (InfoChar.Gender == 1 && !InfoChar.Fusion.IsPorata2)
                    {
                        return 391;
                    }

                    if (InfoChar.Fusion.IsPorata2)
                    {
                        switch (InfoChar.Gender)
                        {
                            case 0:
                                {
                                    return 870;
                                }
                            case 1:
                                {
                                    return 873;
                                }
                            case 2:
                                {
                                    return 867;
                                }
                        }
                    }
                    else if (InfoChar.Fusion.IsPorata)
                    {
                        return 383;
                    }
                    else
                    {
                        return 380;
                    }
                }

                var item = ItemBody[5];
                if (item == null) return InfoChar.Hair;
                if (ItemCache.GetCaiTrangById(item.Id))
                {
                    return ItemCache.GetHeadByCaiTrangid(item.Id);
                }
                return ItemCache.ItemTemplate(item.Id).Part != -1 ? ItemCache.ItemTemplate(item.Id).Part  : ItemCache.GetAvatarById(item.Id) ? ItemCache.GetHeadByCaiTrangid(item.Id) : InfoChar.Hair;
            }
        }

        public virtual short GetBody(bool isMonkey = true)
        {
            if (InfoSkill.socolaMabu.isSocola) return 1286;
            else if (InfoSkill.MaPhongBa.isMaPhongBa) return 1222;
            else if (InfoSkill.HoaBang.isHoaBang) return 1211;
            else if (isMonkey && InfoSkill.Socola.IsCarot) return 407;
            else if (isMonkey && InfoSkill.Socola.IsSocola) return 413;
            else if (isMonkey && InfoSkill.HoaDa.IsHoaDa) return 455;
            else if (isMonkey && InfoSkill.Monkey.BodyMonkey != -1) return InfoSkill.Monkey.BodyMonkey;
            else
            {
                var headPart = GetHead();
                if (InfoChar.Fusion.IsFusion)
                {
                    return (short)(headPart + 1);
                }
                var item = ItemBody[5];
                if (item != null)
                {
                    if (ItemCache.GetCaiTrangById(item.Id))
                    {
                        return ItemCache.GetBodyByCaiTrangid(item.Id);
                    }
                }

                item = ItemBody[0];
                if (item != null)
                {
                    return ItemCache.ItemTemplate(item.Id).Part;
                }
                return InfoChar.Gender == 1 ? (short)59 : (short)57;
            }
        }

        public virtual short GetLeg(bool isMonkey = true)
        {
            if (InfoSkill.socolaMabu.isSocola) return 1287;
            else if (InfoSkill.MaPhongBa.isMaPhongBa) return 1223;
            else if (InfoSkill.HoaBang.isHoaBang) return 1212;
            else if (isMonkey && InfoSkill.Socola.IsCarot) return 408;
            else if (isMonkey && InfoSkill.Socola.IsSocola) return 414;
            else if (isMonkey && InfoSkill.HoaDa.IsHoaDa) return 456;
            else if (isMonkey && InfoSkill.Monkey.LegMonkey != -1) return InfoSkill.Monkey.LegMonkey;
            else
            {
                var headPart = GetHead();
                if (InfoChar.Fusion.IsFusion)
                {
                    return (short)(headPart + 2);
                }
                var item = ItemBody[5];

                if (item != null)
                {
                    if (ItemCache.GetCaiTrangById(item.Id))
                    {
                        return ItemCache.GetLegByCaiTrangid(item.Id);
                    }
                }
                item = ItemBody[1];
                if (item != null)
                {
                    return ItemCache.ItemTemplate(item.Id).Part;
                }
                return InfoChar.Gender == 1 ? (short)60 : (short)58;
            }
        }

        public short GetBag()
        {
            return InfoChar.PhukienPart > 0 ? InfoChar.PhukienPart : InfoChar.Bag;
        }

        public void SetGetFull(bool isGet)
        {
            IsGetHpFull = isGet;
            IsGetMpFull = isGet;
            IsGetDamageFull = isGet;
            IsGetDefenceFull = isGet;
            IsGetCritFull = isGet;
            IsHpPlusFromDamage = isGet;
            IsMpPlusFromDamage = isGet;
        }

        public virtual int LengthBagNull()
        {
            return 20 - ItemBag.Count;
        }

        public virtual int LengthBoxNull()
        {
            return 20 - ItemBox.Count;
        }

        public virtual int BagLength()
        {
            return 20;
        }

        public virtual int BoxLength()
        {
            return 20;
        }
        
        public virtual int BodyLength()
        {
            return 12;
        }

        public bool CheckLockInventory()
        {
            if (InfoChar.LockInventory.IsLock)
            {
                CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().LOCK_INVENTORY));
                return false;
            }
            return true;
        }

        public bool IsDontMove()
        {
            var timeServer = ServerUtils.CurrentTimeMillis();
            if (InfoChar.IsDie || InfoSkill.ThaiDuongHanSan.IsStun || InfoSkill.DichChuyen.IsStun
                || InfoSkill.PlayerTroi.IsPlayerTroi
                || InfoSkill.ThoiMien.IsThoiMien
                || InfoSkill.HoaDa.IsHoaDa
                || InfoSkill.HoaBang.isHoaBang
                || InfoSkill.MeTroi.IsMeTroi
                || InfoSkill.Monkey.IsStart || InfoSkill.TuSat.Delay > timeServer || InfoSkill.Laze.Time > timeServer || InfoSkill.Qckk.Time > timeServer) return true;
            return false;
        }

        public void Clear() => SuppressFinalize(this);

        public void Dispose()
        {
            SuppressFinalize(this);
        }
    }
}