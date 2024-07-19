using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Menu;
using NgocRongGold.Application.Threading;
using NgocRongGold.DatabaseManager.Player;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Item;
using NgocRongGold.Model.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Handlers.Client
{
    public class PlayerMenu
    {
        public static void Action(Model.Character.Character character, ICharacter player, short select)
        {
            if (player == null) return;
            character.IdPlayerAction = player.Id;
            switch ((MenuPlayerEnum)select)
            {
                case MenuPlayerEnum.NULL://Not Exist
                    break;
                case MenuPlayerEnum.BUY_AVATAR:
                    var item = player.ItemBody[5];
                    if (item == null)
                    {
                        character.CharacterHandler.SendMessage(Service.ServerMessage("Người chơi không mặc cải trang này nữa"));
                        break;
                    }
                    var itemTemplate = ItemCache.ItemTemplate(item.Id);
                    var itemCost = item.SaleCoin - (item.SaleCoin * 5 / 100);
                    var goldRecieve = item.SaleCoin / 5;
                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, string.Format(MenuNpc.Gi().TextMeo[11], itemTemplate.Name, player.Name, itemTemplate.SaleCoin, itemCost, goldRecieve), MenuNpc.Gi().MenuMeo[1], character.InfoChar.Gender));
                    character.TypeMenu = 15;
                    break;
                case MenuPlayerEnum.PLEASE_DISCIPLE:
                    if (ConditionToPleaseDisciple(character, player, true))
                    {
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, string.Format(MenuNpc.Gi().TextMeo[14], player.Name), MenuNpc.Gi().MenuMeo[1], character.InfoChar.Gender));
                        character.TypeMenu = 18;
                    }
                    break;
                case MenuPlayerEnum.OAN_TU_TI:
                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, string.Format(MenuNpc.Gi().TextMeo[13]), MenuNpc.Gi().MenuMeo[3], character.InfoChar.Gender));
                    character.TypeMenu = 17;
                    break;
                case MenuPlayerEnum.BAN_ACC:
                    if (!ConditionToBanAccount(character)) break;
                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, string.Format(MenuNpc.Gi().TextMeo[12], player.Name, player.Player.Username), MenuNpc.Gi().MenuMeo[1], character.InfoChar.Gender));
                    character.TypeMenu = 16;
                    break;
                case MenuPlayerEnum.TRONG_DUA_HAU:
                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, string.Format(MenuNpc.Gi().TextMeo[17], player.Name), MenuNpc.Gi().MenuMeo[1], character.InfoChar.Gender));
                    character.TypeMenu = 22;
                    break;
            }
        }
        public static bool ConditionToBanAccount(Model.Character.Character character)
        {
            return character.Player.Role is 1;
        }
        public static bool ConditionToBuyAvatar(Model.Character.Character character, ICharacter player)
        {
            var itemAvatar = player.ItemBody[5];
            if (itemAvatar == null) return false;
            if (!character.Player.IsActive || character.InfoChar.Power < 50_000_000_000)
            {
                return false;
            }
            return itemAvatar.SaleCoin > 0;
        }
        public static int SaleCoinBuyAvatar(Model.Item.Item item)
        {
            return item.SaleCoin - (item.SaleCoin * 5 / 100);
        }
        public static bool ConditionToPleaseDisciple(Model.Character.Character character, ICharacter player, bool message = false)
        {
            var mePower = character.InfoChar.Power;
            var playerPower = player.InfoChar.Power;
            if (message)
            {
                if (mePower < 150_000_000 || mePower > 1_500_000_000)
                {
                    character.CharacterHandler.SendMessage(Service.ServerMessage("Yêu cầu sức mạnh lớn hơn 150tr và nhỏ hơn 1ty5"));
                    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                    character.CharacterHandler.SendMessage(Service.ClosePanel());
                    return false;
                }
                else if (playerPower < 1_500_000_000)
                {
                    character.CharacterHandler.SendMessage(Service.ServerMessage("Sức mạnh của người bạn muốn làm đệ tử phải lớn hơn 1ty5"));
                    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                    character.CharacterHandler.SendMessage(Service.ClosePanel());
                    return false;
                }
                else if (character.InfoChar.MasterId != -1)
                {
                    character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã có sư phụ rồi"));
                    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                    character.CharacterHandler.SendMessage(Service.ClosePanel());
                    return false;
                }
                else if (player.InfoChar.DiscipleId != -1)
                {
                    character.CharacterHandler.SendMessage(Service.ServerMessage("Người này đã có đệ tử"));
                    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                    character.CharacterHandler.SendMessage(Service.ClosePanel());
                    return false;
                }
            }
            else
            {
                if (mePower < 150_000_000 || mePower > 1_500_000_000)
                {
                    return false;
                }
                else if (playerPower < 1_500_000_000)
                {
                    return false;
                }
                else if (character.InfoChar.MasterId != -1)
                {
                    return false;
                }
                else if (player.InfoChar.DiscipleId != -1)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
