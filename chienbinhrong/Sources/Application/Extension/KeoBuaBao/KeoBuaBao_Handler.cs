using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Extension.KeoBuaBao;
using NgocRongGold.Application.Main;
using NgocRongGold.Model.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.KeoBuaBao
{
    public class KeoBuaBao_Handler  
    {
        public static KeoBuaBao_Type GetWin(KeoBuaBao_Type type)
        {
            switch (type)
            {
                case KeoBuaBao_Type.Kéo:
                    return KeoBuaBao_Type.Bao;
                case KeoBuaBao_Type.Búa:
                    return KeoBuaBao_Type.Kéo;
                case KeoBuaBao_Type.Bao:
                    return KeoBuaBao_Type.Búa;
                default:
                    return KeoBuaBao_Type.Kéo;
            }
        }
        public static KeoBuaBao_Type GetLose(KeoBuaBao_Type type)
        {
            switch (type)
            {
                case KeoBuaBao_Type.Kéo:
                    return KeoBuaBao_Type.Búa;
                case KeoBuaBao_Type.Búa:
                    return KeoBuaBao_Type.Bao;
                case KeoBuaBao_Type.Bao:
                    return KeoBuaBao_Type.Kéo;
                default:
                    return KeoBuaBao_Type.Kéo;
            }
        }
        public static void Thang(Character character)
        {
            //   var menu = string.Format(KeoBuaBao_Cache.TextMenu[2], character.InfoChar.KeoBuaBao.Me_Type.ToString(), character.InfoChar.KeoBuaBao.KeoBuaBao_Type.ToString());
            var menu = $"|1|Bạn ra cái <{character.InfoChar.KeoBuaBao.Me_Type.ToString()}>\n|1|Tôi ra cái <{character.InfoChar.KeoBuaBao.KeoBuaBao_Type.ToString()}>\n|1|Bạn thắng rồi huhu\n|1|Bạn nhận được {character.InfoChar.KeoBuaBao.MucDatCuoc*2} thỏi vàng";
            character.CharacterHandler.AddItemToBag(true, ItemCache.GetItemDefault(457, character.InfoChar.KeoBuaBao.MucDatCuoc * 2));
            character.CharacterHandler.SendMessage(Service.SendBag(character));
            character.CharacterHandler.SendMessage(Service.ServerMessage($"Bạn đã nhận được {character.InfoChar.KeoBuaBao.MucDatCuoc*2} thỏi vàng"));
            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(54, menu, KeoBuaBao_Cache.Menu[1], character.InfoChar.Gender));
            character.TypeMenu = 5;
        }
        public static void Thua(Character character)
        {
            character.CharacterHandler.RemoveItemBagById(457, character.InfoChar.KeoBuaBao.MucDatCuoc);
            character.CharacterHandler.SendMessage(Service.SendBag(character));
            var menu = $"|7|Bạn ra cái <{character.InfoChar.KeoBuaBao.Me_Type.ToString()}>\n|7|Tôi ra cái <{character.InfoChar.KeoBuaBao.KeoBuaBao_Type.ToString()}>\n|7|Tôi thắng nhé <lêu lêu>\n|1|Bạn đã bị mất  {character.InfoChar.KeoBuaBao.MucDatCuoc} thỏi vàng";
            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(54, menu, KeoBuaBao_Cache.Menu[1], character.InfoChar.Gender));
            character.TypeMenu = 5;

        }
    }
}
