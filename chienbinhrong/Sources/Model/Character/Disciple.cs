using System;
using System.Linq;
using Linq.Extras;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Handlers.Character;
using NgocRongGold.Application.Interfaces.Monster;
using NgocRongGold.Application.IO;
using NgocRongGold.DatabaseManager;
using NgocRongGold.Model.Info;
using NgocRongGold.Model.Info.InfoDiscipler;
using NgocRongGold.Model.ModelBase;
using NgocRongGold.Model.Data;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Interfaces.Character;

namespace NgocRongGold.Model.Character
{
    public class Disciple : CharacterBase
    {
        public Character Character { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public long LevelPercent { get; set; }
        public bool IsFire { get; set; }
        public bool IsBienHinh { get; set; }
        public IMonster MonsterFocus { get; set; }
        public InfoDelayDisciple InfoDelayDisciple { get; set; }
        public PlusPoint PlusPoint { get; set; }
        public ICharacter CharacterFocus { get; set; }
        public Disciple(Character character)
        {
            Status = 0;
            InfoChar.Power = 2000;
            Name = "Đệ tử";
            Type = 1;
            Id = -character.Id;
            Character = character;
            InfoChar.Stamina = 1250;
            InfoChar.MaxStamina = 1250;
            IsFire = true;
            MonsterFocus = null;
            IsBienHinh = false;
            PlusPoint = new PlusPoint();
            InfoDelayDisciple = new InfoDelayDisciple();
            CharacterHandler = new DiscipleHandler(this);
        }

        public Disciple()
        {
            Status = 0;
            InfoChar.Power = 2000;
            Name = "Đệ tử";
            Type = 1;
            InfoChar.Stamina = 1250;
            InfoChar.MaxStamina = 1250;
            IsFire = true;
            IsBienHinh = false;
            MonsterFocus = null;
            PlusPoint = new PlusPoint();
            InfoDelayDisciple = new InfoDelayDisciple();
            CharacterHandler = new DiscipleHandler(this);
        }

        public void CreateNewDisciple(Character character, int gender)
        {
            InfoChar.Gender = (sbyte)gender;
            InfoChar.Power = 2000;
            InfoChar.Level = (sbyte)Cache.Gi().EXPS.Count(exp => exp < InfoChar.Power);
            Status = 0;
            Name = "Đệ tử";
            Type = 1;
            Id = -character.Id;
            Character = character;
            Zone = character.Zone;
            InfoChar.Stamina = 1250;
            InfoChar.MaxStamina = 1250;
            IsFire = true;
            InfoChar.OriginalHp = InfoChar.Hp = ServerUtils.RandomNumber(980, 2200);
            InfoChar.OriginalMp = InfoChar.Mp = ServerUtils.RandomNumber(980, 2200);
            InfoChar.OriginalDamage = ServerUtils.RandomNumber(20, 60);
            InfoChar.OriginalDefence = ServerUtils.RandomNumber(20, 50);
            InfoChar.OriginalCrit = ServerUtils.RandomNumber(100) < 10 ? ServerUtils.RandomNumber(3, 6) : ServerUtils.RandomNumber(1, 4);

            var randomSkill = DataCache.IdSkillDisciple1[ServerUtils.RandomNumber(DataCache.IdSkillDisciple1.Count)];
            Skills.Add(new SkillCharacter.SkillCharacter()
            {
                Id = randomSkill,
                SkillId = GetSkillId(randomSkill),
                Point = 1,
            });
            PlusPoint = new PlusPoint();
            InfoDelayDisciple = new InfoDelayDisciple();
            CharacterHandler = new DiscipleHandler(this);
            Info = new InfoFriend(this);
        }
        public void CreatePet(Character character, int type,sbyte gender)
        {
            InfoChar.Gender =gender;
            InfoChar.Power = 2000;
            InfoChar.Level = (sbyte)Cache.Gi().EXPS.Count(exp => exp < InfoChar.Power);
            Status = 0;
            if (type == 1) { 
            Name = "Đệ tử";
            Type = 1;
            }
             else if (type == 1) { 
            Name = "Đệ tử Mabư";
            Type = 2;
            }else if (type == 2){
            Name = "Đệ tử Cumber";
            Type = 3;
            }else if (type == 3)
            {
                Name = "Đệ tử Cumber";
                Type = 4;
            }
            else if (type == 4)
            {
                Name = "Đệ tử Billy";
                Type = 5;
            }
            Id = -character.Id;
            Character = character;
            Zone = character.Zone;
            InfoChar.Stamina = 1250;
            InfoChar.MaxStamina = 1250;
            IsFire = true;
            InfoChar.OriginalHp = InfoChar.Hp = ServerUtils.RandomNumber(980, 2200);
            InfoChar.OriginalMp = InfoChar.Mp = ServerUtils.RandomNumber(980, 2200);
            InfoChar.OriginalDamage = ServerUtils.RandomNumber(20, 60);
            InfoChar.OriginalDefence = ServerUtils.RandomNumber(20, 50);
            InfoChar.OriginalCrit = ServerUtils.RandomNumber(100) < 10 ? ServerUtils.RandomNumber(3, 6) : ServerUtils.RandomNumber(1, 4);

            var randomSkill = DataCache.IdSkillDisciple1[ServerUtils.RandomNumber(DataCache.IdSkillDisciple1.Count)];
            Skills.Add(new SkillCharacter.SkillCharacter()
            {
                Id = randomSkill,
                SkillId = GetSkillId(randomSkill),
                Point = 1,
            });
            PlusPoint = new PlusPoint();
            InfoDelayDisciple = new InfoDelayDisciple();
            CharacterHandler = new DiscipleHandler(this);
        }
        public void CreateNewMaBuDisciple(Character character, sbyte gender)
        {
            InfoChar.Gender = gender;
            InfoChar.Power = 1500000;
            InfoChar.Level = (sbyte)Cache.Gi().EXPS.Count(exp => exp < InfoChar.Power);
            Status = 0;
            Name = "Đệ tử Mabư";
            Type = 2;
            Id = -character.Id;
            Character = character;
            Zone = character.Zone;
            InfoChar.Stamina = 2400;
            InfoChar.MaxStamina = 2400;
            IsFire = true;
            InfoChar.OriginalHp = InfoChar.Hp = ServerUtils.RandomNumber(2200, 3200);
            InfoChar.OriginalMp = InfoChar.Mp = ServerUtils.RandomNumber(2200, 3200);
            InfoChar.OriginalDamage = ServerUtils.RandomNumber(20, 60);
            InfoChar.OriginalDefence = ServerUtils.RandomNumber(20, 50);
            InfoChar.OriginalCrit = ServerUtils.RandomNumber(100) < 10 ? ServerUtils.RandomNumber(4, 8) : ServerUtils.RandomNumber(2, 5);
            var randomSkill = DataCache.IdSkillDisciple1[ServerUtils.RandomNumber(DataCache.IdSkillDisciple1.Count)];
            Skills.Add(new SkillCharacter.SkillCharacter()
            {
                Id = randomSkill,
                SkillId = GetSkillId(randomSkill),
                Point = 1,
            });
            PlusPoint = new PlusPoint();
            InfoDelayDisciple = new InfoDelayDisciple();
            CharacterHandler = new DiscipleHandler(this);
            Info = new InfoFriend(this);
        }
        public void CreateNewCumberDisciple(Character character, sbyte gender)
        {
            InfoChar.Gender = gender;
            InfoChar.Power = 2000;
            InfoChar.Level = (sbyte)Cache.Gi().EXPS.Count(exp => exp < InfoChar.Power);
            Status = 0;
            Name = "Đệ tử Cumber";
            Type = 4;
            Id = -character.Id;
            Character = character;
            Zone = character.Zone;
            InfoChar.Stamina = 2400;
            InfoChar.MaxStamina = 2400;
            IsFire = true;
            InfoChar.OriginalHp = InfoChar.Hp = ServerUtils.RandomNumber(2200, 3200);
            InfoChar.OriginalMp = InfoChar.Mp = ServerUtils.RandomNumber(2200, 3200);
            InfoChar.OriginalDamage = ServerUtils.RandomNumber(20, 60);
            InfoChar.OriginalDefence = ServerUtils.RandomNumber(20, 50);
            InfoChar.OriginalCrit = ServerUtils.RandomNumber(100) < 10 ? ServerUtils.RandomNumber(4, 8) : ServerUtils.RandomNumber(2, 5);
            var randomSkill = DataCache.IdSkillDisciple1[ServerUtils.RandomNumber(DataCache.IdSkillDisciple1.Count)];
            Skills.Add(new SkillCharacter.SkillCharacter()
            {
                Id = randomSkill,
                SkillId = GetSkillId(randomSkill),
                Point = 1,
            });
            PlusPoint = new PlusPoint();
            InfoDelayDisciple = new InfoDelayDisciple();
            CharacterHandler = new DiscipleHandler(this);
            Info = new InfoFriend(this);
        }
        public void CreateDiscipleTemplate(Character character,int id ,sbyte gender)
        {
            var temp = Cache.Gi().DISCIPLE_TEMPLATE.Values.FirstOrDefault(i => i.Id == id);
            if (temp != null)
            {
                InfoChar.Gender = gender;
                InfoChar.Power = 1500000;
                InfoChar.Level = (sbyte)Cache.Gi().EXPS.Count(exp => exp < InfoChar.Power);
                Status = 0;
                Name = temp.Name;
                Type = temp.Type;
                Id = -character.Id;
                Character = character;
                Zone = character.Zone;
                InfoChar.Stamina = 2400;
                InfoChar.MaxStamina = 2400;
                IsFire = true;
                InfoChar.OriginalHp = InfoChar.Hp = temp.Hp;
                InfoChar.OriginalMp = InfoChar.Mp = temp.Mp;
                InfoChar.OriginalDamage = (int)temp.Damage;
                InfoChar.OriginalDefence = (int)temp.Defend;
                InfoChar.OriginalCrit = temp.Critical;
                var randomSkill = DataCache.IdSkillDisciple1[ServerUtils.RandomNumber(DataCache.IdSkillDisciple1.Count)];
                Skills.Add(new SkillCharacter.SkillCharacter()
                {
                    Id = randomSkill,
                    SkillId = GetSkillId(randomSkill),
                    Point = 1,
                });
                PlusPoint = new PlusPoint();
                InfoDelayDisciple = new InfoDelayDisciple();
                CharacterHandler = new DiscipleHandler(this);
            }
            else{
                Character.CharacterHandler.SendMessage(Service.DialogMessage("NOT FIND PET WITH TYPE: " + id + "IN TEMPlATE !"));
            }
        }
        public static int GetSkillId(int id)
        {
            return id switch
            {
                19 => 121,
                99 => 693,
                _ => id * 7
            };
        }

        private short HeadLevel()
        {
            //var temp = Cache.gI().DISCIPLE_TEMPLATE.Values.FirstOrDefault(i => i.Id == Type);
            //if (temp != null)
            //{
            //    return (short)temp.Head;
            //}
            //else
            //{
                // Ma bư
                if (Type == 2)
                {
                    return 297;
                }
                if (Type == 3)
                {
                    return 989;
                }
                if (Type == 4)
            {
                return (short)(InfoChar.Power > 150000000 ? 1266 : 1263);
            }
                if (Type == 5)
            {
                return 1454;
            }
            return InfoChar.Gender switch
            {
                0 => InfoChar.Power <= 1500000 ? (short)285 : (short)304,
                1 => InfoChar.Power <= 1500000 ? (short)288 : (short)305,
                _ => InfoChar.Power <= 1500000 ? (short)282 : (short)303
            };
         
        }

        private short BodyLevel()
        {
          
                if (Type == 2)
                {
                    return 298;
                }
                if (Type == 3)
                {
                    return 990;
                }
            if (Type == 4)
            {
                return 1264;
            }
            if (Type == 5)
            {
                return 1455;
            }
            return InfoChar.Gender switch
                {
                    0 => 286,
                    1 => 289,
                    _ => 283
                };
          
        }

        private short LegLevel()
        {
          
                if (Type == 2)
                {
                    return 299;
                }

                if (Type == 3)
                {
                    return 991;
                }
            if (Type == 4)
            {
                return 1265;
            }
            if (Type == 5)
            {
                return 1456;
            }
            return InfoChar.Gender switch
                {
                    0 => 287,
                    1 => 290,
                    _ => 284
                };
           
        }

        public override short GetHead(bool isMonkey = true)
        {
            switch (isMonkey)
            {
                case true when InfoSkill.Socola.IsSocola:
                    return 609;
                case true when InfoSkill.Monkey.HeadMonkey != -1:
                    return InfoSkill.Monkey.HeadMonkey;
            }

            if (InfoChar.Fusion.IsFusion) return 383;
            var item = ItemBody[5];
            if (item == null || (DataCache.TypeDiscipleBienHinh.Contains(Type) && !IsBienHinh)) return HeadLevel();

            var itemTemplate = ItemCache.ItemTemplate(item.Id);
            var part = itemTemplate.Part;
            //Check part #1
            if (ItemCache.GetCaiTrangById(item.Id))
            {
                return ItemCache.GetHeadByCaiTrangid(item.Id);
            }
            return ItemCache.ItemTemplate(item.Id).Part != -1 ? ItemCache.ItemTemplate(item.Id).Part : ItemCache.GetAvatarById(item.Id) ? ItemCache.GetHeadByCaiTrangid(item.Id) : HeadLevel();
        }

        public override short GetBody(bool isMonkey = true)
        {
            switch (isMonkey)
            {
                case true when InfoSkill.Socola.IsCarot:
                    return 407;
                case true when InfoSkill.Socola.IsSocola:
                    return 413;
                case true when InfoSkill.Monkey.BodyMonkey != -1:
                    return InfoSkill.Monkey.BodyMonkey;
            }

            if (InfoChar.Fusion.IsFusion) return 384;

            if (DataCache.TypeDiscipleBienHinh.Contains(Type) && !IsBienHinh) return BodyLevel();

            var headPart = GetHead();
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
            return BodyLevel();
        }

        public override short GetLeg(bool isMonkey = true)
        {
            switch (isMonkey)
            {
                case true when InfoSkill.Socola.IsSocola:
                    return 611;
                case true when InfoSkill.Monkey.LegMonkey != -1:
                    return InfoSkill.Monkey.LegMonkey;
            }

            if (InfoChar.Fusion.IsFusion) return 385;

            if (DataCache.TypeDiscipleBienHinh.Contains(Type) && !IsBienHinh) return LegLevel();

            var headPart = GetHead();
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
            return LegLevel();
        }

        public string CurrStrLevel()
        {
            GetDataLevel();
            var levels = Cache.Gi().LEVELS.Where(x => x.Gender == InfoChar.Gender).ToList();
            return $"{levels[InfoChar.Level].Name} {LevelPercent/100}.{LevelPercent%100}%" ;
        }

        private void GetDataLevel()
        {
            try
            {
                var num = 1L;
                var num2 = 0L;
                var num3 = 0;
                for (var num4 = Cache.Gi().EXPS.Count - 1; num4 >= 0; num4--)
                {
                    if (InfoChar.Power < Cache.Gi().EXPS[num4]) continue;
                    num = ((num4 != Cache.Gi().EXPS.Count - 1) ? (Cache.Gi().EXPS[num4 + 1] - Cache.Gi().EXPS[num4]) : 1);
                    num2 = InfoChar.Power - Cache.Gi().EXPS[num4];
                    num3 = num4;
                    break;
                }
                InfoChar.Level = (sbyte)num3;
                LevelPercent = (int)(num2 * 10000 / num);
            }
            catch (Exception)
            {
                //Ignored
            }
        }
    }
}