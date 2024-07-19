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
using NgocRongGold.Model.Item;
using NgocRongGold.DatabaseManager.Player;

namespace NgocRongGold.Model.Character
{
    public class PhanThan : CharacterBase
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
        public PhanThan(Character character)
        {
            Status = 0;
            InfoChar.Power = 2000;
            Name = "Phân thân của " + character.Name;
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
            CharacterHandler = new PhanThanHandler(this);
        }

        public PhanThan()
        {
            Status = 0;
            InfoChar.Power = 2000;
            Name = "Phân thân";
            Type = 1;
            InfoChar.Stamina = 1250;
            InfoChar.MaxStamina = 1250;
            IsFire = true;
            IsBienHinh = false;
            MonsterFocus = null;
            CharacterHandler = new PhanThanHandler(this);
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

       

        public override short GetHead(bool isMonkey = true)
        {
            return Character.GetHead();

        }

        public override short GetBody(bool isMonkey = true)
        {
            return Character.GetBody();

        }

        public override short GetLeg(bool isMonkey = true)
        {
            return Character.GetLeg();

        }

        public string CurrStrLevel()
        {
            GetDataLevel();
            var levels = Cache.Gi().LEVELS.Where(x => x.Gender == InfoChar.Gender).ToList();
            return $"{levels[InfoChar.Level].Name} {LevelPercent / 100}.{LevelPercent % 100}%";
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