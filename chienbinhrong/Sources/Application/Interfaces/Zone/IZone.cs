using Application.Interfaces.Map;
using NgocRongGold.Application.Handlers.Map;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Item;
using NgocRongGold.Model.Map;
using NgocRongGold.Model.Monster;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Zone
{
    public interface IZone
    {

        public int Id { get; set; }
        public int ItemMapId { get; set; }
        public IMap Map { get; set; }
        public ConcurrentDictionary<int, Character> Characters { get; set; }
        public ConcurrentDictionary<int, ICharacter> ICharacters { get; set; }
        public List<NgocRongGold.Model.Info.Effect.Effect> Effects { get; set; }
        public ConcurrentDictionary<int, PhanThan> PhanThans { get; set; }

        public ConcurrentDictionary<int, Disciple> Disciples { get; set; }
        public ConcurrentDictionary<int, Boss> Bosses { get; set; }
        public ConcurrentDictionary<int, Pet> Pets { get; set; }
        public List<MonsterMap> MonsterMaps { get; set; }
        public List<Npc> Npcs { get; set; }

        public ConcurrentDictionary<int, MonsterPet> MonsterPets { get; set; }
        public ConcurrentDictionary<int, ItemMap> ItemMaps { get; set; }
        public ConcurrentDictionary<int, Pet2> Pets2 { get; set; }
        public ZoneHandler ZoneHandler { get; set; }
        
    }
}
