using System.Threading.Tasks;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.IO;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Item;
using NgocRongGold.Model.Map;
using NgocRongGold.Model.Monster;
using NgocRongGold.Model;
using Application.Interfaces.Zone;

namespace NgocRongGold.Application.Interfaces.Map
{
    public interface IZoneHandler
    {
        IZone Zone { get; set; }
        void JoinZone(Model.Character.Character _char, bool isDefault, bool isTeleport, int typeTeleport);
        void OutZone(ICharacter _char);
        void InitMob();
        Task Update();
        void Close();
        void SendMessage(Message message, bool isSkillMessage);
        void SendMessage(Message message, int id);
        void LeaveItemMap(ItemMap itemMap);
        void LeaveItemMap(ItemMap itemMap, MonsterMap monster);
        void RemoveItemMap(int id);
        int GetItemMapNotId();
        void RemoveCharacter();
        void AddDisciple(Disciple disciple);
        void RemoveDisciple(Disciple disciple);
        void AddPet(Pet pet);
        void AddPet(Pet2 pet);
        void RemovePet(Pet2 pet);
        void RemovePet(Pet pet);
        void RemoveMonsterMe(int id);
        ICharacter GetCharacter(int id);
        MonsterMap GetMonsterMap(int id);
    }
}