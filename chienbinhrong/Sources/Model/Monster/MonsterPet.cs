
using System.Security.Policy;
using Application.Interfaces.Zone;
using NgocRongGold.Application.Handlers.Monster;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Model.ModelBase;
using Zone = NgocRongGold.Model.Map.Zone;

namespace NgocRongGold.Model.Monster
{
    public class MonsterPet : MonsterBase
    {
        public MonsterPet(ICharacter character, IZone zone, short template, long hp, int damage)
        {
            IdMap = character.Id;
            Id = template;
            X = character.InfoChar.X;
            Y = character.InfoChar.Y;
            Zone = zone;
            Character = character;
            IsMobMe = true;
            HpMax = hp;
            OriginalHp = hp;
            Hp = hp;
            Damage = damage;
            IsDie = false;
            MonsterHandler = new MonsterPetHandler(this);
        }
    }
}