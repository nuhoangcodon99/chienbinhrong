using NgocRongGold.Application.IO;
using NgocRongGold.Model.Clan;
using NgocRongGold.Model.Info;

namespace NgocRongGold.Application.Interfaces.Character
{
    public interface IClanHandler
    {
        Clan Clan { get; set; }
        void Update(int id);
        void Flush();
        bool AddMember(Model.Character.Character character, int role = 0, bool isFlush = true);
        void AddCharacterPea(CharacterPea characterPea);
        bool RemoveMember(int id);
        ClanMember GetMember(int id);
        ClanMessage GetMessage(int id);
        void SendMessage(Message message);
        void UpdateClanId();
        void SendUpdateClan();
        void Chat(ClanMessage message);
    }
}