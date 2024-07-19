using Application.Interfaces.Zone;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Map
{
    public interface IMap
    {
        public int Id { get; set; }
        public  List<IZone> Zones { get; set; }
        public TileMap TileMap { get; set; }
        public long TimeMap { get; set; }
        public bool IsRunning { get; set; }
        public bool IsStop { get; set; }
        public Task HandleZone { get; set; }
        public IZone zone { get; set; }
        public bool IsMapNotChangeZone();
        public void OutZone(ICharacter character);
        public void JoinZone(Character character, int id, bool isDefault = false, bool isTeleport = false, int typeTeleport = 0);
        public void JoinRandomZone(Character character, int id, bool isDefault = false, bool isTeleport = false, int typeTeleport = 0);
        public IZone GetZoneNotMaxPlayer();
        public IZone GetZonePlayer();
        public IZone GetZoneNotBoss();
        public IZone MobAlive();
        public IZone MobDieAll();
        public Character GetChar(int id);
        public IZone GetZoneNotMaxPlayer(int id);
        public IZone GetZoneById(int id);
        public IZone GetZoneRandomNotHasPlayer();
        public void Close();
    }

}
