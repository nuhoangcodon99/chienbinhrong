using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Model.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Model.Clan
{
    public class ClanZone
    {
        public Application.Threading.Map Map { get; set; }
        public ClanZone()
        {
            Map = new Application.Threading.Map(153, tileMap: null);
           // Maps.Add(new Application.Threading.Map(165, tileMap: null, mapCustom: null));
        }
        public void Join(Character.Character character,int mapOld, bool isCapsule = false)
        {
            var currentMap = MapManager.Get(character.InfoChar.MapId);

            if (isCapsule)
            {
                character.CharacterHandler.SendZoneMessage(
                                            Service.SendTeleport(character.Id, character.InfoChar.Teleport));
                currentMap.OutZone(character);
                character.MapIdOld = currentMap.Id;
                character.SetOldMap();

            }
            else
            {
                currentMap.OutZone(character);
            }
            character.CharacterHandler.SetUpPosition(character.InfoChar.MapId, 153);
            Map.JoinZone(character, 0);
        }
    }
}
