using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.IO;

namespace NgocRongGold.Application.Interfaces.Monster
{
    public interface IMonsterHandler
    {
        IMonster Monster { get; set; }
        void SetUpMonster(bool isDie = false);
        void Recovery();
        int UpdateHp(long damage, int charId, bool isMaxHp = false);  
        void LeaveItem(ICharacter character);
        int PetAttackMonster(IMonster monster);  
        void PetAttackPlayer(ICharacter character);  
        void MonsterAttack(ICharacter temp, ICharacter character);
        void Update();
        void AddPlayerAttack(ICharacter character, int damage);
        void RemoveTroi(int charId);
        void RemoveEffect(long timeServer, bool globalReset = false);
    }
}