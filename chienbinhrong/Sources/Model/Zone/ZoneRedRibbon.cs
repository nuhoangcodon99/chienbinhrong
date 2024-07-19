using Application.Interfaces.Map;
using Application.Interfaces.Zone;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Handlers.Map;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Threading;
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

namespace Model.Zone
{
    public class ZoneRedRibbon : IZone
    {

        public int Id { get; set; }
        public int ItemMapId { get; set; }
        public IMap Map { get; set; }
        public ConcurrentDictionary<int, Character> Characters { get; set; }
        public ConcurrentDictionary<int, ICharacter> ICharacters { get; set; }
        public List<NgocRongGold.Model.Info.Effect.Effect> Effects { get; set; }
        public ConcurrentDictionary<int, PhanThan> PhanThans { get; set; }
        public List<Npc> Npcs{get;set;}

        public long Time { get; set; }
        public ConcurrentDictionary<int, Disciple> Disciples { get; set; }
        public ConcurrentDictionary<int, Boss> Bosses { get; set; }
        public ConcurrentDictionary<int, Pet> Pets { get; set; }
        public List<MonsterMap> MonsterMaps { get; set; }
        public ConcurrentDictionary<int, MonsterPet> MonsterPets { get; set; }
        public ConcurrentDictionary<int, ItemMap> ItemMaps { get; set; }
        public ConcurrentDictionary<int, Pet2> Pets2 { get; set; }
        public ZoneHandler ZoneHandler { get; set; }
        public ZoneRedRibbon(int id, IMap map)
        {
            Id = id;
            Time = -1;
            Map = map;
            PhanThans = new ConcurrentDictionary<int, PhanThan>();
            Characters = new ConcurrentDictionary<int, Character>();
            Bosses = new ConcurrentDictionary<int, Boss>();
            MonsterPets = new ConcurrentDictionary<int, MonsterPet>();
            ICharacters = new ConcurrentDictionary<int, ICharacter>();
            Disciples = new ConcurrentDictionary<int, Disciple>();
            Pets = new ConcurrentDictionary<int, Pet>();
                        Npcs = new List<Npc>();

            //Npcs = new ConcurrentDictionary<int, Npc>();
            MonsterMaps = new List<MonsterMap>();
            Pets2 = new ConcurrentDictionary<int, Pet2>();
            ItemMaps = new ConcurrentDictionary<int, ItemMap>();
            MonsterPets = new ConcurrentDictionary<int, MonsterPet>();
            ZoneHandler = new ZoneHandler(this);
            ZoneHandler.InitMob();


        }
    }
}
