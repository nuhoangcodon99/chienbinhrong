using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Handlers.Item;
using NgocRongGold.Application.Main;
using NgocRongGold.Model.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.Event
{
    public class MenuNpc8Thang3
    {
        public static MenuNpc8Thang3 instance = new MenuNpc8Thang3();
        public static MenuNpc8Thang3 gI()
        {
            return instance;
        }

        public readonly List<string> TextMenu8Thang3 = new List<string>()
        {
            "Bạn muốn gói loại nào ?",
            "Bạn muốn tặng quà cho mình ư ?"
        };
        public readonly List<List<string>> Menu8Thang3 = new List<List<string>>()
        {
            new List<string>()
            {
                "Gói hộp quà\nnhẹ nhàng",
                "Gói hộp quà\nchỉn chu",
                "Đóng",
            },
            new List<string>()
            {
                "Chế tạo",
                "Đóng"
            },
            new List<string>()
            {
                "Đóng"
            },
            new List<string>()
            {
                "Tặng\nHộp quà chỉn\nchu",
                "Tặng\nHộp quà nhẹ\nnhàng",
                "Tặng\nBông hoa\nhồng",
                "Đóng"
            },
        };
    }
    public class SuKien8Thang3
    {
        public static readonly List<List<int>> ItemMakeEasyGift = new List<List<int>>()
        {
            new List<int>() { 1556,  1555, 1554},
            new List<int>() { 30, 5, 1},
        };
        public static readonly List<List<int>> ItemMakeAdvancedGift = new List<List<int>>()
        {
            new List<int>() { 1556,  1555, 1554, 1557},
            new List<int>() { 30, 5, 1, 1},
        };
        public static void OpenMenuGoiHopQua(Character character)
        {
            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, MenuNpc8Thang3.gI().TextMenu8Thang3[0], MenuNpc8Thang3.gI().Menu8Thang3[0], character.InfoChar.Gender));
            character.TypeMenu = 20;
        }
        public static void OpenMenuTangQua(Character character, short npcId, int typeMenu)
        {
            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc8Thang3.gI().TextMenu8Thang3[1], MenuNpc8Thang3.gI().Menu8Thang3[3], character.InfoChar.Gender));
            character.TypeMenu = typeMenu;
        }
        public static void SelectTangQua(Character character, short id, short npcId)
        {
            var count = character.CharacterHandler.GetAllQuantityItemBagById(id);
            if (count <= 0)
            {
                character.CharacterHandler.SendMessage(Service.ServerMessage("Cần 1 " + ItemCache.ItemTemplate(id).Name));
                return;
            }
            character.CharacterHandler.RemoveItemBagById(id, 1);
            character.CharacterHandler.SendZoneMessage(Service.NpcChat(npcId, "Cảm ơn " + character.Name + " đã tặng cho tui 1 " + ItemCache.ItemTemplate(id).Name + " mãi iu iu " + character.Name));
            HandleTangQua(character);
            character.CharacterHandler.SendMessage(Service.SendBag(character));
        }
        public static void HandleTangQua(Character character)
        {

        }
        public static void SelectGoiHopQuaNheNhang(Character character)
        {
            var text = DoiThuongHandler.DoiThuong(character, "Gói hộp quà nhẹ nhàng", ItemMakeEasyGift[0], ItemMakeEasyGift[1], (int)DoiThuongType.HOP_QUA_NHE_NHANG);
            var menu = MenuNpc8Thang3.gI().Menu8Thang3[1];
            if (character.TypeDoiThuong != (int)DoiThuongType.HOP_QUA_NHE_NHANG) menu = MenuNpc8Thang3.gI().Menu8Thang3[2];
            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, text, menu, character.InfoChar.Gender));
            character.TypeMenu = 21;
        }
        public static void SelectGoiHopQuaChinChu(Character character)
        {
            var text = DoiThuongHandler.DoiThuong(character, "Gói hộp quà chỉn chu", ItemMakeAdvancedGift[0], ItemMakeAdvancedGift[1], (int)DoiThuongType.HOP_QUA_CHIN_CHU);
            var menu = MenuNpc8Thang3.gI().Menu8Thang3[1];
            if (character.TypeDoiThuong != (int)DoiThuongType.HOP_QUA_CHIN_CHU) menu = MenuNpc8Thang3.gI().Menu8Thang3[2];
            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, text, menu, character.InfoChar.Gender));
            character.TypeMenu = 21;
        }
    }
}
