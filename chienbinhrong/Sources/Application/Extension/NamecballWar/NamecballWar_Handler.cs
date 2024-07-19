using NgocRongGold.Application.IO;
using NgocRongGold.Application.Manager;
using NgocRongGold.Model.Clan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NgocRongGold.Model.Character;
using NgocRongGold.Application.Main;

namespace NgocRongGold.Application.Extension.NamecballWar
{
    public class NamecballWar_Handler
    {
          
        public static void ConfirmMenu(short npcId,Character character,int select)
        {
            switch (character.TypeMenu)
            {
                case 0:
                    {
                        switch (select)
                        {
                            case 0:
                                var menu = NamecballWar.ListMenus[1];
                                string.Format(menu[1], character.InfoChar.TotalPotential);
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Ngọc rồng Namec đang bị 2 thế lực tranh giành\nHãy chọn cấp độ tham gia tùy chọn theo sức mạnh bản thân", menu, character.InfoChar.Gender));
                                character.TypeMenu = 1;
                                break;
                        }
                    }
                    break;
                case 1:
                    {
                        switch (select)
                        {
                            case 0:
                                break;
                            case 1:

                                break;
                        }
                    }
                    break;
            }
        }
       
    }
}
