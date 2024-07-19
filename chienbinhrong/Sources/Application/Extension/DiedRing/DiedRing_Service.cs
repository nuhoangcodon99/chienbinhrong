
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.Model.Character;
namespace NgocRongGold.Application.Extension.DiedRing{
    public class DiedRing_Service{
        public static void Register(Character character){
            if (DiedRing_Cache.ListCharacter.Contains(character.Id))
            {
                return;
            }
            DiedRing_Cache.ListCharacter.Add(character.Id);
        }
        public static void SendPosistion(Character character,short X, short Y){
              character.InfoChar.X = X;
              character.InfoChar.Y = Y;
              character.CharacterHandler.SendZoneMessage(Service.SendPos(character, 1));
        }
        public static void RemoveBoss(Boss boss){
            if (boss != null){
                boss.CharacterHandler.SendDie();
            }
        }
    }
}