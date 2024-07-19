using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.IO;
using NgocRongGold.Model.Character;

namespace NgocRongGold.Model.Info
{
    public class InfoFriend
    {
        public int Id { get; set; } 
        public short Head { get; set; }
        public short Body { get; set; }
        public short Leg { get; set; }
        public sbyte Bag { get; set; }
        public string Name { get; set; }
        public bool IsOnline { get; set; }
        public long Power { get; set; }
        public long HpFull { get; set; }
        public long DefenceFull { get; set; }
        public long DamageFull { get; set; }
        public InfoFriend()
        {
            Id = -1;
            Head = -1;
            Leg = -1;
            Body = -1;
            Bag = -1;
            Name = "";
            IsOnline = true;
            Power = 0;

            HpFull = 0;
            DefenceFull = 0;
            DamageFull = 0;
        }

        public InfoFriend(Character.Character character)
        {
            Id = character.Id;
            Head = character.GetHead();
            Leg = character.GetLeg();
            Body = character.GetBody();
            Bag = character.InfoChar.Bag;
            Name = character.Name;
            IsOnline = true;
            Power = character.InfoChar.Power;
            HpFull = character.HpFull;
            DefenceFull = character.DefenceFull;
            DamageFull = character.DamageFull;
        }
        public void CloseAndUpdate(Character.Character character)
        {
            Id = character.Id;
            Head = character.GetHead();
            Leg = character.GetLeg();
            Body = character.GetBody();
            Bag = character.InfoChar.Bag;
            Name = character.Name;
            IsOnline = false;
            Power = character.InfoChar.Power;
            HpFull = character.HpFull;
            DefenceFull = character.DefenceFull;
            DamageFull = character.DamageFull;
        }
        public InfoFriend(ICharacter character)
        {
            Id = character.Id;
            Head = character.GetHead(false);
            Leg = character.GetLeg(false);
            Body = character.GetBody(false);
            Bag = character.InfoChar.Bag;
            Name = character.Name;
            IsOnline = true;
            Power = character.InfoChar.Power;
            HpFull = character.HpFull;
            DefenceFull = character.DefenceFull;
            DamageFull = character.DamageFull;
        }
    }
}