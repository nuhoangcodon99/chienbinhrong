using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Info.Skill;
using NgocRongGold.Model.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.NamecBattlefield
{
    public class NamecBattlefield_Handler
    {
        public static void SendMessage(List<int> Characters, Message msg)
        {
            foreach (var id in Characters)
            {
                var character = ClientManager.Gi().GetCharacter(id);
                if (character != null)
                {
                    character.CharacterHandler.SendMessage(msg);
                }
            }
        }
        public static void ChangeFlag(Character character, sbyte cflag)
        {
            character.Flag = cflag;
            
        }
        public static void GetPointTemporary(Character character)
        {
            var DragonballX = 0;
            var DragonballY = 0;
            character.DataNamecBattlefield.PointTemporary += 2;
            character.CharacterHandler.SendMessage(Service.ServerMessage("+2"));
            var cadic = Server.Gi().NamecBattlefield.Cadic;
            var fide = Server.Gi().NamecBattlefield.Fide;
            switch (character.DataNamecBattlefield.TeamId)
            {
                case 1://cadic
                    cadic.Point++;
                    SendMessage(cadic.Characters, NamecBattlefield_Service.updatePoint(cadic.Point, fide.Point));
                    SendMessage(fide.Characters, NamecBattlefield_Service.updatePoint(cadic.Point, fide.Point));
                    DragonballX = 248 + ServerUtils.RandomNumber(50);
                    DragonballY = 144;
                    break;
                case 2://fikde
                    fide.Point++;
                    SendMessage(cadic.Characters, NamecBattlefield_Service.updatePoint(cadic.Point, fide.Point));
                    SendMessage(fide.Characters, NamecBattlefield_Service.updatePoint(cadic.Point, fide.Point));
                    DragonballX = 2218 - ServerUtils.RandomNumber(50);
                    DragonballY = 600;
                    break;
            }
            var item = new ItemMap(-1, ItemCache.GetItemDefault((short)(353 + character.DataNamecBattlefield.Star - 1)), true);
            item.X = (short)DragonballX;
            item.Y = (short)DragonballY;
            character.Zone.ZoneHandler.LeaveItemMap(item);
            character.DataNamecBattlefield.Star = 0;
        }
        public static void OutOrDie(Character character)
        {
            if (character.DataNamecBattlefield.Star != 0)
            {
                var item = new ItemMap(-1, ItemCache.GetItemDefault((short)(353 + character.DataNamecBattlefield.Star - 1)), true);
                item.X = character.InfoChar.X;
                item.Y = character.InfoChar.Y;
                character.Zone.ZoneHandler.LeaveItemMap(item);
                character.DataNamecBattlefield.Star = 0;
            }
            character.DataNamecBattlefield.Status = NamecBattlefield_Character_Status.NORMAL;

        }
        public static void GoBackAndGetPoint(Character character, int point)
        {
            character.DataNamecBattlefield.Point += point;
            character.DataNamecBattlefield.PointTemporary = 0;
            character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + point + " điểm danh vọng"));
            MapManager.JoinMap((Character)character, 24 + character.InfoChar.Gender, ServerUtils.RandomNumber(20), true, true, character.TypeTeleport);
            character.Zone.ZoneHandler.SendMessage(Service.SendTeleport(character.Id, character.TypeTeleport));
        }
        public List<Character> GetCharacters(List<int> Characters)
        {
           var CharactersReal = new List<Character>();
            foreach (var id in Characters)
            {
                var Character =(Model.Character.Character) ClientManager.Gi().GetCharacter(Characters[id]);
                CharactersReal.Add(Character);
            }
            return CharactersReal;
        }
    }
}
