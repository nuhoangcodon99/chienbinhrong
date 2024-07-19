    using NgocRongGold.Application.Constants;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Handlers.Item
{
    public class DoiThuongCache
    {
        public static readonly List<List<int>> DoiCa = new List<List<int>> { new List<int> { 1002, 1003, 1004, -1 }, new List<int> { 10, 10, 10, 500000000 } };
        public static readonly List<List<short>> RewardDoiCa = new List<List<short>> { new List<short> { 1142, 1149, 1110, 1111, 1108, 1197, 1219, 1220, 1229, 1310, 1336 }, new List<short> { 909, 916, 917, 918, 942, 943, 944, 1250, 1251, 1230, 1231, 1232 }, new List<short> { 733, 734, 735, 743, 744, 746, 795, 849, 897, 920, 1092, 1131, 1144, 1172, 1259, 1260, 1308, 1309 }, new List<short> { 1311, 1312, 1301, 1278, 1279 } };
        public static readonly List<List<string>> Combines = new List<List<string>> { new List<string> { "Chế tạo" }, new List<string> { "Hủy" } };
        public static readonly List<List<string>> OKS = new List<List<string>> { new List<string> { "OK" }, new List<string> { "Hủy" } };
    }
    public enum DoiThuongType
    {
        BON_TAM_GO = 1,
        BON_TAM_VANG = 2,
        CAI_TRANG_SIEU_CAP = 3,
        XO_CA = 4,
        CHE_TAO_SACH_CU = 5,
        DOI_SACH_TUYET_KI_1 = 6,
        GHEP_CHU_DAU_NAM = 7,
        NAU_BANH_TET = 8,
        NAU_BANH_CHUNG = 9,
        TRANG_TRI_CAY_TET_NORMAL = 10,
        TRANG_TRI_CAY_TET_VIP = 11,
        CHE_TAC_MAT_HON_MANG = 12,
        HOP_QUA_NHE_NHANG = 13,
        HOP_QUA_CHIN_CHU = 14,
        NAU_BANH_DAY = 15,
        NAU_BANH_CHUNG_LANG_LIEU = 16,
        DANG_SINH_LE = 17,
        DANG_SINH_LE_XIN = 18,
        DANG_SINH_LE2 = 19,// dâng bằng bánh dầy
        DANG_SINH_LE3 = 20,//dâng bằng bánh chưng Lang Liêu
    }
    public class DoiThuongHandler
    {
        public static void Reward(Model.Character.Character character, List<List<short>> paramt)
        {
            var randIndex = ServerUtils.RandomNumber(paramt.Count);
            var item = ItemCache.GetItemDefault(paramt[randIndex][ServerUtils.RandomNumber(paramt[randIndex].Count)]);
            var itemTemp = ItemCache.ItemTemplate(item.Id);
            var text = $"Chúc mừng bạn đã nhận được:{ServerUtils.Color("green")}{itemTemp.Name}";
            character.CharacterHandler.AddItemToBag(true,item);
            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, text, DoiThuongCache.OKS[0], character.InfoChar.Gender));
            character.TypeMenu = 68;
        }
        public static string DoiThuong(Model.Character.Character character, string Title, List<int> ItemId, List<int> Quantitys, int TypeDoiThuong, params string[] text)
        {
            if (ItemId.Count != Quantitys.Count) return "";

            StringBuilder sb = new StringBuilder(Title);
            List<string> ListText = new List<string>();

            for (int i = 0; i < ItemId.Count; i++)
            {
                string colorBlue = ServerUtils.Color("blue");
                string colorRed = ServerUtils.Color("red");
                string itemName = "";

                if (ItemId[i] == -1)
                {
                    var item = character.InfoChar.Gold;
                    string moneyParse = ServerUtils.GetMoneyParse(Quantitys[i]);
                    if (item >= Quantitys[i])
                        ListText.Add($"{colorBlue}Giá vàng: {moneyParse}");
                    else
                    {
                        TypeDoiThuong = -1;
                        ListText.Add($"{colorRed}Giá vàng: {moneyParse}");
                    }
                }
                else if (ItemId[i] == -2)
                {
                    var item = character.InfoChar.Diamond;
                    string moneyParse = ServerUtils.GetMoneyParse(Quantitys[i]);
                    if (item >= Quantitys[i])
                        ListText.Add($"{colorBlue}Giá ngọc: {moneyParse}");
                    else
                    {
                        TypeDoiThuong = -1;
                        ListText.Add($"{colorRed}Giá ngọc: {moneyParse}");
                    }
                }
                else
                {
                    var quantity = character.CharacterHandler.GetAllQuantityItemBagById(ItemId[i]);
                    if (quantity > 0)
                    {
                        itemName = ItemCache.ItemTemplate((short)ItemId[i]).Name;
                        if (quantity >= Quantitys[i])
                        {
                            ListText.Add($"{colorBlue}{itemName} {quantity}/{Quantitys[i]}");
                            Server.Gi().Logger.Debug($"{itemName} found {quantity}");
                        }
                        else
                        {
                            Server.Gi().Logger.Debug("Not enough quantity");
                            TypeDoiThuong = -1;
                            ListText.Add($"{colorRed}{itemName} {quantity}/{Quantitys[i]}");
                        }
                    }
                    else
                    {
                        Server.Gi().Logger.Debug("Not found");
                        itemName = ItemCache.ItemTemplate((short)ItemId[i]).Name;
                        ListText.Add($"{colorRed}{itemName} Không có/{Quantitys[i]}");
                        TypeDoiThuong = -1;
                    }
                }
            }

            character.TypeDoiThuong = TypeDoiThuong;

            foreach (string textItem in ListText)
            {
                sb.Append(textItem);
            }

            foreach (string additionalText in text)
            {
                sb.Append(additionalText);
            }

            return sb.ToString();
        }
    }
}
