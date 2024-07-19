using System.Collections.Concurrent;
using System.Collections.Generic;
using Application.Interfaces.Map;
using Application.Interfaces.Zone;
using NgocRongGold.Application.Handlers.Map;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Info.Effect;
using NgocRongGold.Model.Item;
using NgocRongGold.Model.Monster;

namespace NgocRongGold.Model.Map
{
    public class Zone : IZone
    {
        public int Id { get; set; }
        public int ItemMapId { get; set; }
        public IMap Map { get; set; }
        public ConcurrentDictionary<int, Character.Character> Characters { get; set; }
        public ConcurrentDictionary<int, ICharacter> ICharacters { get; set; }
        public List<Npc> Npcs { get; set; }

        public ConcurrentDictionary<int, Disciple> Disciples { get; set; }
        public ConcurrentDictionary<int, Boss> Bosses { get; set; }
        public ConcurrentDictionary<int, Pet> Pets { get; set; }
        public List<MonsterMap> MonsterMaps { get; set; }
        public ConcurrentDictionary<int, MonsterPet> MonsterPets { get; set; }
        public ConcurrentDictionary<int, ItemMap> ItemMaps { get; set; }
        public ConcurrentDictionary<int, Pet2> Pets2 { get; set; }
        public List<Model.Info.Effect.Effect> Effects { get; set; }
        public ZoneHandler ZoneHandler { get; set; }
        public ConcurrentDictionary<int, PhanThan> PhanThans { get; set; }

        public Zone()
        {

        }
        public Zone(int id, Application.Threading.Map map)
        {
            Id = id;
            ItemMapId = 0;
            Map = map;
            PhanThans = new ConcurrentDictionary<int, PhanThan>();
            Characters = new ConcurrentDictionary<int, Character.Character>();
            ICharacters = new ConcurrentDictionary<int, ICharacter>();
            Disciples = new ConcurrentDictionary<int, Disciple>();
            Pets = new ConcurrentDictionary<int, Pet>();
            Bosses = new ConcurrentDictionary<int, Boss>();
            Npcs = new List<Npc>();
            MonsterMaps = new List<MonsterMap>();
            Pets2 = new ConcurrentDictionary<int, Pet2>();
            ItemMaps = new ConcurrentDictionary<int, ItemMap>();
            MonsterPets = new ConcurrentDictionary<int, MonsterPet>();
            Effects = new List<Info.Effect.Effect>();
            ZoneHandler = new ZoneHandler(this);
            ZoneHandler.InitMob();
        }
    }
}