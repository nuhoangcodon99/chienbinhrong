using System;
using System.Linq;
using NgocRongGold.DatabaseManager;
using NgocRongGold.Model.Item;
using NgocRongGold.Model.Template;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Interfaces.Map;
using NgocRongGold.Application.Handlers.Item;
using NgocRongGold.Application.Main;
using NgocRongGold.Model.Character;
using System.Collections.Generic;
using NgocRongGold.Model.Option;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Helper;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Net.Http.Headers;

namespace NgocRongGold.Application.Constants
{
    public static class ItemCache
    {
        public readonly static List<short> ItemGoiQuaDacBiet2024 = new List<short>() { 1185, 1186, 1074, 1075, 1076, 1079, 1080, 1081, 1143, 1021, 1022, 1359, 213, 214, 215, 216, 217, 218, 219, 76};
        public readonly static List<short> ItemCapsuleTet2024 = new List<short>() { 1074, 1075, 1076, 1079, 1080, 1081,  76, 1066, 1067, 1068, 1069, 1070 , 849, 852,  1543, 1544, 1545, 942, 943, 944  };

        public readonly static List<short> ItemPhongBiTet2024 = new List<short>() { 213, 214, 215, 216, 217, 218, 219, 342, 343, 344,345, 1201, 945, 951,1023, 1024, 1522, 1074, 1075, 1076, 1079, 1080, 1081 , 459, 1542, 1527, 1412, 1421, 1422};
        public readonly static List<short> ItemThiepChucMungLongChau = new List<short>() { 76, 1185, 1186,1523,1087, 1088, 1089, 1090, 1091 };
        public readonly static List<short> ItemHopQuaTet2024 = new List<short>() { 441, 442, 443, 444, 445, 446, 447, 1074, 1075, 1076, 1077, 1078, 1079, 1080, 1081, 1082, 1083, 227, 1142, 1144, 381, 382, 383, 384, 385, 933, 1259, 1527, 1528 };

        public readonly static List<short> ItemAmulet = new List<short>() { 213, 214, 215, 216, 217, 218, 219 };
        public static Item GetItemDefault(short id, int quantity = 1)
        {
            var itemTemplate = Cache.Gi().ITEM_TEMPLATES.Values.FirstOrDefault(item => item.Id == id);
            if (itemTemplate == null) return null;
            var item = new Item();
            item.Id = itemTemplate.Id;
            //item.Options= itemTemplate.Options;
            item.Quantity = quantity;
            item.BuyPotential = 0;
            item.SaleCoin = itemTemplate.SaleCoin;
            item.Options = itemTemplate.Options.Copy();
            return item;
        }
        public static Item GetItemDefaultExpire(short id, int quantity = 1, int days = 0)
        {
            var itemTemplate = Cache.Gi().ITEM_TEMPLATES.Values.FirstOrDefault(item => item.Id == id);
            if (itemTemplate == null) return null;
            var item = new Item();
            item.Id = itemTemplate.Id;
            //item.Options= itemTemplate.Options;
            item.Quantity = quantity;
            item.BuyPotential = 0;
            item.SaleCoin = itemTemplate.SaleCoin;
            item.Options = itemTemplate.Options.Copy();
            if (days > 0)
            {

                if (item.Options.FirstOrDefault(i => i.Id == 73) != null)
                {
                    item.Options.FirstOrDefault(i => i.Id == 73).Param = days;
                }
                else
                {
                    item.Options.Add(new OptionItem()
                    {
                        Id = 73,
                        Param = days,
                    });
                }
            } 
            return item;
        }
        public static Item GetItemDefault(short id, List<OptionItem> optionItems,int quantity = 1)
        {
            var itemTemplate = Cache.Gi().ITEM_TEMPLATES.Values.FirstOrDefault(item => item.Id == id);
            if (itemTemplate == null) return null;
            var item = new Item();
            item.Id = itemTemplate.Id;
            //item.Options= itemTemplate.Options;
            item.Quantity = quantity;
            item.BuyPotential = 0;
            item.SaleCoin = itemTemplate.SaleCoin;
            item.Options.AddRange(optionItems.ToList());
            return ItemHandler.Clone(item);
        }
        public static Item GetBongTaiCap2(short id, int quantity = 1)
        {
            var itemTemplate = Cache.Gi().ITEM_TEMPLATES.Values.FirstOrDefault(item => item.Id == id);
            if (itemTemplate == null) return null;
            var item = new Item();
            item.Id = itemTemplate.Id;
            //item.Options= itemTemplate.Options;
            item.Quantity = quantity;
            item.BuyPotential = 0;
            item.SaleCoin = itemTemplate.SaleCoin;
            item.Options = new List<OptionItem>();

            var porataOption = DataCache.OptionPorata2[ServerUtils.RandomNumber(DataCache.OptionPorata2.Count)];
            item.Options.Add(new OptionItem()
            {
                Id = porataOption[0],
                Param = ServerUtils.RandomNumber(porataOption[1], porataOption[2]),
            });
            return ItemHandler.Clone(item);
        }
       
        public static void GetItem(Character character, short id, int quantity = 1)
        {
            var itemId = GetItemDefault(id);
            itemId.Quantity = quantity;
            character.CharacterHandler.AddItemToBag(true, itemId, "Get Item From ItemCache");
            character.CharacterHandler.SendMessage(Service.SendBag(character));
        }
      

        public static bool IsPetItem(short id)
        {
            switch(id)
            {
                case 892 : return true;//Thỏ xám
                case 893 : return true;//Thỏ trắng
                case 908 : return true;//Ma phong ba
                case 909 : return true;//Thần chết cute
                case 910 : return true;//Bí ngô nhí nhảnh
                case 916 : return true;//Lính Tam Giác
                case 917 : return true;//lính vuông
                case 918 : return true;//lính tròn
                case 919 : return true;//búp bê
                case 936 : return true;//tuần lộc nhí
                case 942 : return true;//hổ mặp vàng
                case 943 : return true;//hổ mặp trắng
                case 944 : return true;//hỏ mặp xanh
                case 967 : return true;//sao la
                case 1008 : return true;//cua đỏ
                case 1039 : return true;//Thỏ ốm
                case 1040 : return true;//Thỏ mập
                case 1046 : return true;//Khỉ bong bóng
                case 1107: return true; // Bí Ma Zương  
                case 1114: return true; // Phù Thủy Da Zàng
                case 1202: return true; // Mèo Trắng Đuôi Vàng
                case 1203: return true; // Mèo Trắng Đuôi Vàng
                default: return false;
            }
        }

        public static bool IsSpecialAmountItem(short id)
        {
            switch(id)
            {
                case 933:
                case 590:
                    return true;
                default: return false;
            }
        }

        public static bool IsUnlimitItem(short id)
        {
            switch(id)
            {
                case 590:
                case 1066:
                case 1067:
                case 1068:
                case 1069:
                case 1070:
                case 457:
              //  case 933:
                case 934:
                case 1235:
                case 1549:
                case >= 14 and <= 20:
                case >= 220 and <= 224:
                {
                    return true;
                }                
                default: return false;
            }
        }

        public static bool IsItemSellOnlyOne(short id)
        {
            switch(id)
            {
                case 457:
                case 1343:
                {
                    return true;
                }
                default: return false;
            }
        }

        public static ItemTemplate ItemTemplate(short id)
        {
            return Cache.Gi().ITEM_TEMPLATES.Values.FirstOrDefault(item => item.Id == id);
        }
       

        public static ItemOptionTemplate ItemOptionTemplate(int id)
        {
            return Cache.Gi().ITEM_OPTION_TEMPLATES.FirstOrDefault(option => option.Id == id);
        }

        public static bool IsCaiTrang(int id)
        {
            return DataCache.CaiTrang.Contains(id);
        }
        
        public static bool IsAvatar(int id)
        {
            return DataCache.Avatar.Contains(id);
        }
       
        public static bool GetCaiTrangById(int id)
        {
            var temp = Cache.Gi().CaiTrangTemplate.Values.FirstOrDefault(i => i.IdTemp == id);
            return temp != null && !temp.isAvatar;
        }
        public static bool GetAvatarById(int id)
        {
            var temp = Cache.Gi().CaiTrangTemplate.Values.FirstOrDefault(i => i.IdTemp == id);
            return temp != null && temp.isAvatar;
        }
        public static short GetHeadByCaiTrangid(int id)
        {
            var temp = Cache.Gi().CaiTrangTemplate.Values.FirstOrDefault(i => i.IdTemp == id);
            if (temp != null)
            {
                return (short)temp.Head;
            }
            return -1;
        }
        public static short GetBodyByCaiTrangid(int id)
        {
            var temp = Cache.Gi().CaiTrangTemplate.Values.FirstOrDefault(i => i.IdTemp == id);
            if (temp != null)
            {                
               return (short)temp.Body; 
            }
            return -1;
        }
        public static short GetLegByCaiTrangid(int id)
        {
            var temp = Cache.Gi().CaiTrangTemplate.Values.FirstOrDefault(i => i.IdTemp == id);
            if (temp != null)
            {               
                    return (short)temp.Leg;   
            }
            return -1;
        }
        public static short PartNotAvatar(int id) {
            switch (id) {
                case 282: {
                    return 98;
                }
                case 283: {
                    return 77;
                }
                case 285: {
                    return 89;
                }
                case 286: {
                    return 86;
                }
                case 287: {
                    return 83;
                }
                case 288: {
                    return 180;
                }
                case 289: {
                    return 162;
                }
                case 291: {
                    return 123;
                }
                case 290:
                case 431: {
                    return 171;
                }
                case 292:
                case 430: {
                    return 174;
                }
                case 1162:
                case 1163:
                    return 1167;
                case 1164:
                    return 1168;
                //case 1161:
                //    return 1172;
            }
            return -1;
        }
        // điền case + id Head , return To Body hoặc To leg || case la part
        public static short PartHeadToBody(short partHead) {
            switch (partHead)
            {
                case >= 192 and <= 200:
                    return 193;
                case 309 or 310:
                    return 307;
                case 460 or 461:
                    return 458;
                case >= 526 and <= 529:
                    return 525;
                case 536:
                    return 476;
                case 538 or 539 or 542 or 543:
                    return 474;
            }

            switch (partHead)
            {
                case 543:
                    return 523;
                case 545 or 546:
                    return 548;
                case 553:
                    return 555;
                case 569:
                    return 472;
                case 808 or 809 or 810:
                    return 806;
                case 831 or 832:
                    return 829;
                case 836 or 837:
                    return 834;
                case 906:
                    return 880;
                case 1128:
                    return 1129;
                case 1119:
                    return 1120;
               case 1122:
                    return 1123;
              case 1125:
                    return 1126;
              case 1131:
                    return 1132;
              case 1140:
                    return 1141;
               case  1146:
                    return 1147;
                case 1149:
                    return 1150;
                case 1152:
                    return 1153;
                case 1169:
                    return 880;
                case 1170:
                    return 1171;
                case 1173:

                    return 1174;
                case 1176:
                    return 1177;
                case 1186:
                    return 1190;
                case 1187: return 1193;
                case 1188: return 1196;
                case 1198: return 1199;
            }

            if(!IsCaiTrang(partHead))
                return (short)(partHead + 1);
            return -1;
        }
        
        public static short PartHeadToLeg(short partHead) {
            switch (partHead)
            {
                case >= 192 and <= 200:
                    return 194;
                case 309 or 310:
                    return 308;
                case 460 or 461:
                    return 459;
                case >= 526 and <= 529 or 543:
                    return 524;
                case 536:
                    return 477;
                case 538 or 539 or 542 or 543:
                    return 475;
                case 545 or 546:
                    return 549;
                case 553:
                    return 556;
                case 569:
                    return 473;
                case 808 or 809 or 810:
                    return 807;
                case 831 or 832:
                    return 830;
                case 836 or 837:
                    return 835;
                case 906:
                    return 881;
                case 1128:
                    return 1130;
                case 1119:
                    return 1121;
                case 1122:
                    return 1124;
              case 1125:
                    return 1127;
              case 1131:
                    return 1133;
              case 1140:
                    return 1142;
               case  1146:
                    return 1148;
                case 1149:
                    return 1151;
                case 1152:
                    return 1154;
                case 1169:
                    return 881;
                case 1170:
                    return 1172;
                case 1173:

                    return 1175;
                case 1176:
                    return 1178;
                case 1186:
                    return 1191;
                case 1187: return 1194 ;
                case 1188: return 1197;
                case 1198: return 1200;
            }

            if(!IsCaiTrang(partHead))
                return (short)(partHead + 2);
            return -1;
        }

        #region Giap Luyen Tap
        public static bool ItemIsGiapLuyenTap(int itemId) {
            return ((itemId >= 529 && itemId <= 531) || (itemId >= 534 && itemId <= 536));
        }

        public static int GetGiapLuyenTapLevel(int itemId)
        {
            switch (itemId)
            {
                case 529:
                case 534:
                {
                    return 1;
                }
                case 530:
                case 535:
                {
                    return 2;
                }
                case 531:
                case 536:
                {
                    return 3;
                }
            }
            return 1;
        }

        public static int GetGiapLuyenTapPTSucManh(int itemId)
        {
            switch (itemId)
            {
                case 529:
                case 534:
                {
                    return 10;
                }
                case 530:
                case 535:
                {
                    return 20;
                }
                case 531:
                case 536:
                {
                    return 30;
                }
            }
            return 10;
        }

        public static int GetGiapLuyenTapLimit(int itemId)
        {
            switch (itemId)
            {
                case 529:
                case 534:
                {
                    return 100;
                }
                case 530:
                case 535:
                {
                    return 1000;
                }
                case 531:
                case 536:
                {
                    return 10000;
                }
            }
            return 100;
        }
        #endregion

    }
}