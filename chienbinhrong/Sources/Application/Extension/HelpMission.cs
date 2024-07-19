using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NgocRongGold.Application.MainTasks;
using NgocRongGold.Model.Character;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Threading;

namespace NgocRongGold.Application.Extension
{
    public class HelpMission
    {
        public static Boolean InitHoTroNhiemVu = true;
        public static Boolean Check(Character character)
        {
            if (TaskHandler.CheckTask(character, 22, 0))
            {
                return true;
            }
            if (TaskHandler.CheckTask(character, 22, 1))
            {
                return true;
            }
            if (TaskHandler.CheckTask(character, 22, 2))
            {
                return true;
            }
            return false;
        }
        public static String Menu (Character character)
        {
            
            if (TaskHandler.CheckTask(character, 22, 0))
            {
                return "Đội quân của Fide đang ở Thung lũng Nappa, ta sẽ đưa tới đó\nTa vừa nhìn thấy Kuku, có phải ngươi đang tìm hắn?";
            }
            else if (TaskHandler.CheckTask(character, 22, 1))
            {
                return "Đội quân của Fide đang ở Thung lũng Nappa, ta sẽ đưa tới đó\nTa vừa nhìn thấy Mập đầu đinh, có phải ngươi đang tìm hắn?";
            }
            else if (TaskHandler.CheckTask(character, 22, 2))
            {
                return "Đội quân của Fide đang ở Thung lũng Nappa, ta sẽ đưa tới đó\nTa vừa nhìn thấy Rambo, có phải ngươi đang tìm hắn?";
            }
            return "Đội quân Fide đang ở thung lũng Nappa, ta sẽ đưa ngươi đến đó";
        }
        public static void openMenuCui(Character character)
        {
            var g = character.InfoChar.Gender;
            if (TaskHandler.CheckTask(character, 22, 0))
            {
                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(12, Menu(character), new List<string> { "Đồng ý", "Từ chối", "Đến chỗ\nKuku\n500Tr vàng" }, g));
            }
            else if (TaskHandler.CheckTask(character, 22, 1))
            {
                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(12, Menu(character), new List<string> { "Đồng ý", "Từ chối", "Đến chỗ\nMdd\n500Tr vàng" }, g));
            }
            else if (TaskHandler.CheckTask(character, 22, 2))
            {
                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(12, Menu(character), new List<string> { "Đồng ý", "Từ chối", "Đến chỗ\nRambo\n500Tr vàng" }, g));
            }
            else
            {
                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(12, Menu(character), new List<string> {  "Đến Cold", "Đến Nappa", "Từ Chối" }, g));
                character.TypeMenu = 0;
            }
        }
        public static void HoTroNhiemVu(Character character, int taskIndex)
        {
            if (InitHoTroNhiemVu)
            {
                switch (taskIndex)
                {
                    case 0:
                        if (!Server.Gi().ABoss.KukuSpawn)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Hết Kuku!"));
                            return;
                        }
                        MapManager.JoinMap(character, Server.Gi().ABoss.oldMapKuku, Server.Gi().ABoss.oldZoneKuku, false, false, 0);
                        break;
                    case 1:
                        if (!Server.Gi().ABoss.MapDauDinhSpawn)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Hết MDD!"));
                            return;
                        }
                        MapManager.JoinMap(character, Server.Gi().ABoss.oldMapMapDauDinh, Server.Gi().ABoss.oldZoneMapDauDinh, false, false, 0);
                        break;
                    case 2:
                        if (!Server.Gi().ABoss.RamboSpawn)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Hết Rambo!"));
                            return;
                        }
                        MapManager.JoinMap(character, Server.Gi().ABoss.oldMapRambo, Server.Gi().ABoss.oldZoneRambo, false, false, 0);
                        break;
                }
            }
            else
            {
                character.CharacterHandler.SendMessage(Service.ServerMessage("Hỗ trợ nhiệm vụ Đang Tắt!"));
            }
        }
    }
}
