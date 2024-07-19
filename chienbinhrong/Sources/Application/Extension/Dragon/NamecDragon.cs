using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Model.Character;

namespace NgocRongGold.Application.Extension.Dragon
{
    public class Rồng_Namec
    {
        public string textMenu = "Ta sẽ ban cho cả bang hội ngươi 1 điều ước, ngươi có 5 phút, hãy suy\nnghĩ thật kỹ trước khi quyết định";
        public List<string> Menus = new List<string> { "Bùa trí tuệ\n7 ngày","Bùa mạnh mẽ\n7 ngày","Bùa da trâu\n7 ngày","Bùa thu hút\n7 ngày","Bùa trí tuệ\nX3\n7 ngày", "Bùa trí tuệ\nX3\n7 ngày" };
        public static Rồng_Namec instance;
        public static Rồng_Namec gI()
        {
            if (instance == null) instance = new Rồng_Namec();
            return instance;
        }
        public void ShowMenu(Character character)
        {
            MapManager.SetDragonAppeared(true);
            character.CharacterHandler.SendMessage(Service.SendBag(character));
            character.CharacterHandler.SendZoneMessage(Service.CallDragon(character, 1));
            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(24, textMenu, Menus, 3));
        }
        public void ConfirmMenu(Character character, int npcId, int select)
        {
            var clan = ClanManager.Get(character.ClanId);
            short amulet = 1;
            switch (select)
            {
                case 0:
                    amulet = 213;
                    break;
                case 1:
                    amulet = 214;
                    break;
                case 2:
                    amulet = 215;
                    break;
                case 3:
                    amulet = 219;
                    break;
                case 4:
                    amulet = 671;
                    break;
                case 5:
                    amulet = 672;
                    break;
            }
            var itemAmulet = ItemCache.ItemTemplate(amulet);
            for (int i = 0; i < clan.Thành_viên.Count; i++)
            {
                var ICharacter = ClientManager.Gi().GetCharacter(clan.Thành_viên[i].Id);
                if (ICharacter == null) continue;
                if (ICharacter.InfoChar.ItemAmulet.ContainsKey(amulet))
                {
                    if (ICharacter.InfoChar.ItemAmulet[amulet] < ServerUtils.CurrentTimeMillis())
                    {
                        ICharacter.InfoChar.ItemAmulet[amulet] = DataCache._8HOURS + ServerUtils.CurrentTimeMillis();
                    }
                    else
                    {
                        ICharacter.InfoChar.ItemAmulet[amulet] += DataCache._8HOURS;
                    }
                }
                else
                {
                    ICharacter.InfoChar.ItemAmulet.TryAdd(amulet, DataCache._1HOUR + ServerUtils.CurrentTimeMillis());
                }
                ICharacter.CharacterHandler.SetupAmulet();
                ICharacter.CharacterHandler.SendMessage(Service.ServerMessage("Chúc mừng Bang hội của bạn đã nhận được " + itemAmulet.Name + " trong 7 Ngày"));
            }
            MapManager.SetDragonAppeared(false);

        }
    }
}
