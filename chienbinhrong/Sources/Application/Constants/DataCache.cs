using System.Collections.Generic;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using NgocRongGold.DatabaseManager;
using NgocRongGold.Model.Map;
using NgocRongGold.Model.SkillCharacter;
using NgocRongGold.Model.Template;
using Org.BouncyCastle.Asn1.Mozilla;
using Org.BouncyCastle.Bcpg;

namespace NgocRongGold.Application.Constants
{
    public class DataCache
    {
        public static JsonSerializerSettings SettingNull = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };
        public static readonly List<List<double>> Exchange = new List<List<double>>
        {
            new List<double> { 10000, 5},
            new List<double> { 20000, 20},
            new List<double> { 30000, 30},
            new List<double> { 50000, 50 },
                        new List<double> { 100000, 100},
                                                new List<double> { 200000, 200},
                                                                                                new List<double> { 500000, 500},
                                                                                                                                                                                                new List<double> { 1000000, 1000},

        };
        public static readonly string SLOGAN_SERVER = "caubengocrong.com";
        public static readonly string KEY_SERVER = "dungdeptrai";
        public static readonly List<List<int>> ExchangeHongNgoc = new List<List<int>>
        {
            new List<int> { 10000, 10},
            new List<int> { 20000, 24},
            new List<int> { 30000, 36},
            new List<int> { 50000, 60 },
                        new List<int> { 100000, 120},
                                                new List<int> { 200000, 240},
                                                                                                new List<int> { 500000, 600},
                                                                                                                                                                                                new List<int> { 1000000, 1200},

        };
        public static List<int> TypeDiscipleBienHinh = new List<int>() { 2, 3, 4 }; 
        #region other
        public static readonly List<int> SoChan = new List<int>
        {
            0,
2,
4,
6,
8,
10,
12,
14,
16,
18,
20,
22,
24,
26,
28,
30,
32,
34,
36,
38,
40,
42,
44,
46,
48,
50,
52,
54,
56,
58,
60,
62,
64,
66,
68,
70,
72,
74,
76,
78,
80,
82,
84,
86,
88,
90,
92,
94,
96,
98,
        };
        public static readonly List<int> SoLe = new List<int>
        {
            1,
3,
5,
7,
9,
11,
13,
15,
17,
19,
21,
23,
25,
27,
29,
31,
33,
35,
37,
39,
41,
43,
45,
47,
49,
51,
53,
55,
57,
59,
61,
63,
65,
67,
69,
71,
73,
75,
77,
79,
81,
83,
85,
87,
89,
91,
93,
95,
97,
99,
        };
        public static readonly List<string> varchars = new List<string>() { "q", "w", "e", "r", "t", "0","1", "2", "3", "4", "5", "6", "7", "8", "9", "y", "u","y", "i", "o", "p", "a", "s", "d", "f", "g", "h", "j", "n", "m", "b", "x", "z", "v", "c"};
        #endregion
        public static List<int> listItemCapsuleCaiTrang = new List<int> { 405,406,407,408,409,410,411,421,422,423,424,425,426,427,428,429,430,431,432,433,452,463,464 };
        public static List<int> listAvatarForDisciple = new List<int> { 547, 548, 932, 1337,1412,1421,1422 };

        public static List<int> listXDoanhTrai = new List<int> { 316, 647, 437, 485, 1148, 613, 655, 711, 577, 1252 };
        public static List<int> listYDoanhTrai = new List<int> { 384, 336, 384, 288, 216, 384, 240, 312, 312, 312 };
        public static List<int> IdManhAngel = new List<int> { 1066,1067,1068,1069,1070 };
        public static List<int> IdAllCongThuc = new List<int> { 1071,1072,1073,1084,1085,1086 };
        public static List<int> IdDaNangCap = new List<int> { 1074,1075,1076,1077,1078 };
        public static List<int> IdDaMayMan = new List<int> { 1079,1080,1081,1082,1083 };
        public static List<int> OptionChiSoAn = new List<int> { 50, 77, 103, 5 , 14};
        public static readonly  List<int> listMobBay = new List<int> { 28,29,30,31,7,8,9,10,11,12,37,43,50};
        public static readonly List<short> listItemCSKB = new List<short> { 381, 382, 383, 384, 385 };
        public static readonly List<List<short>> ListChooseGender = new List<List<short>>()
        {
            new List<short>() { 64, 472, 473},
            new List<short>() { 9, 476, 477},
            new List<short>() { 6, 474, 475},

        };
        public static List<List<int>> hairID = new List<List<int>> //gender, index hair
    {
        new List<int>
        {
            64,
            30,
            31
        },
        new List<int>
        {
            9,
            29,
            32
        },
        new List<int>
        {
            6,
            27,
            28
        }
    };

        public static readonly List<int> IdMapNguHanhSon = new List<int> { 122, 123, 124 };
        public static readonly long _1HOUR = 3660000L;
        public static readonly long _8HOURS = 28860000L;
        public static readonly long _1DAY = 86580000L;
        public static readonly long _1DAYBYSECOND = 86400L;
        public static readonly long _1MONTH = 2629860000L;
        public static readonly long _1MINUTES = 60000L;
        public static readonly long _1SECOND = 1000L;
        public static readonly List<string> OngGiaNoelChat = new List<string> { "Hô hô, giáng sinh an lành", "Noel rồi không đi ôm gấu đi mà lại chơi game à?", "Noel cô đơn, hô hô", "Mùa đông và nỗi nhớ tựa như tri kỉ" };

        public static readonly long MAX_LIMIT_GOLD = 5000000000;
        public static readonly long PREMIUM_LIMIT_UP_POWER = 80000000000;

        public static readonly long TRUNG_MA_BU_TIME = 86400000;
        public static readonly long DUA_HAU_TIME = 3600000;
        public static readonly int GIA_NO_TRUNG_MA_BU = 1100000000; //1.1ty

        public static readonly short SHOP_ID_NAPTHE = 3000; //1.1ty

        public static readonly int LIMIT_TRAIN_MANH_VO_BONG_TAI_NGAY = 10000; //1 ngày chỉ train 400 mảnh
        public static readonly int LIMIT_TRAIN_MANH_HON_BONG_TAI_NGAY = 396;
        public static readonly long LITMIT_GOLD = 500000000000L;
        public static readonly int LIMIT_SLOT_RUONG_PHU_THUONG_DE = 100;
        public static readonly List<int> IdBossDied = new List<int> { 52,53,54,55,56,57,58,59,60,61,62,65, 68, 69, 70, 71, 72, 85, 86,87,88,89,90,91,92,112,113 };
        public static readonly List<int> IdBossPractice = new List<int> { 88,90,91,89 };

        public static List<short> LIMIT_SO_LAN_GOI_RONG = new List<short> { 2, 10 };//tài khoản thường, tài khoản premium

        public static readonly int LIMIT_NOT_PREMIUM_TRADE_DAY = 5;//ngày giao dịch 5 lần

        public static readonly long LIMIT_NOT_PREMIUM_TRADE_TIME = 600000;//10p giao dich 1 lan
        // Max cấp bậc sức mạnh
        public static readonly int MAX_LIMIT_POWER_LEVEL = 16; // mốc giới hạn


        public static readonly int LITMIT_OPEN_NEED_GOD_ITEM = 5;
        // Mở khóa sức mạnh mặc định
        public static readonly int DEFAULT_LIMIT_POWER_LEVEL = 0;

        //Giá 1 ngọc trong vòng quay
        public static readonly int CRACK_BALL_PRICE = 25000000;
        public static readonly int CRACK_BALL_PRICE_DIAMOND_LOCK = 10;

        //Chưa có VIP thì chỉ giao dịch max 100tr 1 lần
        public static readonly long LIMIT_NOT_PREMIUM_TRADE_GOLD_AMOUNT = 100000000;

        // Giá mở nội tại
        public static readonly int PRICE_UNLOCK_SPECIAL_SKILL = 50000000; //vang
        public static readonly int PRICE_UNLOCK_SPECIAL_SKILL_VIP = 100; //ngoc

        public static readonly long MAX_PLUS_BAG = 40;

        public static readonly int GiaBanThoiVang = 500000000;
        public static readonly List<short> ListItemGiftWhis = new List<short> { 880, 881, 882 };

        public static readonly List<int> TypeNewBoss_1 = new List<int> { 76, 92, 93 };
        public static readonly List<int> TypeNewBoss_2 = new List<int> { 77 , 85, 88, 89, 90, 91};
        public static readonly List<int> ListActionHidru = new List<int> { 0,   1, 2, 3 };
        public static readonly List<int> ListActionMoveHidru = new List<int> { 8 };

        public static readonly List<short> IdCrackBall = new List<short> { 419, 420, 421, 422, 423, 424, 425 };
        public static readonly List<int> IdMapCustom = new List<int> { };
        public static readonly List<int> IdMapMabu = new List<int> { 114,115,117,118,119,120 };//120
        public static readonly List<int> IdMapMabu2 = new List<int> { 127 };//120
        public static readonly List<List<short>> SetDoTanThu = new List<List<short>> { new List<short> { 0, 6, 12, 21, 27 }, new List<short>() { 1, 7, 12, 22, 28 }, new List<short>() { 2, 8, 12, 23, 29 } };

        public static readonly List<int> IdMapDHVT = new List<int> { 113, 129, 112};
        public static readonly List<MenuPlayerTemplate> MenuPlayers = new List<MenuPlayerTemplate>()
        {
            new MenuPlayerTemplate() { Caption = "", Caption2 = "", MenuSelect = 0},
            new MenuPlayerTemplate() { Caption = "Cải trang", Caption2 = "{0} thỏi vàng", MenuSelect = 1},
            new MenuPlayerTemplate() { Caption = "Đồ sát", Caption2 = "", MenuSelect = 2},
            new MenuPlayerTemplate() { Caption = "Xin làm đệ tử", Caption2 = "", MenuSelect = 3},
                 new MenuPlayerTemplate() { Caption = "Mời chơi oẳn tù tì", Caption2 = "Được ăn cả, ngã ăn cứt", MenuSelect = 8},
            new MenuPlayerTemplate() { Caption = "Ban tài khoản", Caption2 = "", MenuSelect = 10},
                        new MenuPlayerTemplate() { Caption = "Trồng dưa hấu", Caption2 = "", MenuSelect = 11},
        };
        public static readonly List<MenuPlayerTemplate> MenuPlayersNull = new List<MenuPlayerTemplate>()
        {
            
        };
        public static readonly List<int> IdMapNotChangeZone = new List<int> { 112, 135, 136, 137, 138, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 141, 142, 143, 144, 147, 148, 149, 151, 152, 165, 39, 40, 41, 45, 46, 47, 48, 49, 50, 111, 154, 21, 22, 23, 139, 140, 114, 115, 117, 118, 119, 120, 127 };//147,148,149,151,152
        public static readonly List<int> IdMapSpecial = new List<int> {112, 135,136,137,138,53,54,55,56,57,58,59,60,61,62,141,142,143,144,147,148,149,151,152,139,140, 165, 85, 86, 87, 88, 89, 90, 91, 45, 46, 47, 48, 49, 111, 154, 21, 22, 23, 114, 115, 117, 118, 119, 120, 127 };//147,148,149,151,152
        public static readonly List<int> IdMapKarin = new List<int> { 39,40, 41, 45, 46, 47,48, 49, 50,111, 154,21,22,23 , 139,140};
        public static readonly List<int> IdMapReddot = new List<int> { 53, 58, 59, 60, 61, 62, 55, 56,54, 57 };
        public static readonly List<int> IdMapGas = new List<int> { 147,148,149,151,152 };
        public static readonly List<int> IdMapCDRD = new List<int> { 141,142,143};
        public static readonly List<int> IdMapBDKB = new List<int> { 135,136,137,138 };
        public static readonly List<int> IdMapCold = new List<int> { 105, 106, 107, 108, 109, 110 };
        public static readonly List<int> IdMapNRSD = new List<int> {  85,86,87,88,89,90,91};
        public static readonly List<int> IdMapTDST = new List<int>{82,83,79};
        public static readonly List<int> IdOptionSpecialEffectTemporary = new List<int>() { 117, 145, 146, 149, 150, 151, 176, 177, 179, 180, 184, 186, 193, 194, 195, 198, 201, 202, 203, 205, 226, 229, 26, 173, 186 , 109};
        public static readonly List<int> IdMapTuongLai = new List<int> { 92, 93, 94, 96, 97, 98,99, 100, 102, 103};

        public static readonly List<int> IdMapThanhDia = new List<int> { 156, 157, 158, 159 };

        public static readonly List<int> IdMapThucVat = new List<int> { 160, 161, 162, 163 };


        public static readonly List<int> DefaultHair = new List<int> { 64, 30, 31, 6, 27, 28, 9, 29, 32 };

        public static List<List<short>> IdMob = new List<List<short>> {
           new List<short>() {281, 361, 351},
           new List<short>() {512, 513, 536},
           new List<short>() {514, 515, 537},
        };

        
        public static readonly List<int> CaiTrang = new List<int> { 126, 127, 128, 273, 274, 275, 276, 277, 278, 279, 280, 281, 303, 304, 305, 389, 390, 560, 561, 562, 668, 769, 770, 771, 775, 776, 777, 825, 826, 827, 853, 854, 855, 856, 857, 900, 901, 902,1104,1109,1114 ,1169,1170,1173,1176,1186,1187,1189,1198}; // part
        public static readonly List<int> Avatar = new List<int> { 0, 6, 9, 27, 28, 29, 30, 31, 32, 64, 101, 102, 103, 104, 105, 107, 776, 112, 111, 113, 106, 108, 109, 110, 1168,1167}; // part
        public static readonly List<int> IdVeTinh = new List<int> { 343, 344, 345, 346 };
        public static bool IsIdItemNotTrade(int id)
        {
            return id >= 650 && id <= 667;
        }

        public static List<short> ItemThanhDia = new List<short> { 933,934,935 };
        public static List<int> ItemNotSell = new List<int> { 1549 };
        public static List<int> ItemNotDrop = new List<int> { 1549, 457 };

        public static List<int> ItemPremiumTrade = new List<int> { 457, 934, 886, 887, 888, 889, 987, 1048 };
        public static List<int> ItemNormalTrade = new List<int> { 379, 381, 382, 383, 384, 385 };
        public static List<int> TypeItemTrade = new List<int> { 0, 1, 2, 3, 4, 12, 14, 30 };
        public static List<int> IdAmulet = new List<int> { 213, 214, 215, 216, 217, 218, 219 };
        public static List<short> IdFlag = new List<short> { 363, 364, 365, 366, 367, 368, 369, 370, 371, 519, 520, 747 };
        public static List<int> IdDauThan = new List<int> { 13, 60, 61, 62, 63, 64, 65, 352, 523, 595 };
        public static List<int> IdDauThanx30 = new List<int> { 293, 294, 295, 296, 297, 298, 299, 596, 597 };
        public static List<long> UpgradeDauThanTime = new List<long> { 600000, 1800000, 3600000, 10800000, 43200000, 129600000, 432000000, 1296000000, 3456000000, 4456000000 };
        public static List<int> UpgradeDauThanGold = new List<int> { 5000, 25000, 75000, 450000, 1500000, 9000000, 45000000, 125000000, 300000000, 400000000 };

        public static List<int> TypeItemRemove = new List<int> { 6, 7, 5, 8, 9, 10, 11, 12, 27, 13, 15, 16, 22, 23, 24, 25, 28, 29, 30, 31, 33 };
        public static List<short> IdMount = new List<short> { 349, 350, 351, 396, 532, 346, 347, 348, 733, 734, 735, 743, 744, 746, 795, 849, 897, 920,1142,1160,1188 };

        public static List<MapTranspot> GMapTranspots = new List<MapTranspot>()
        {
            null,   //0
            new MapTranspot() {Id = 47, Info = "Rừng Karin", Name = "Trái Đất"},
            new MapTranspot() {Id = 45, Info = "Thần Điện", Name = "Trái Đất"},
            new MapTranspot() {Id = 0, Info = "Làng Aru", Name = "Trái Đất"},
            new MapTranspot() {Id = 7, Info = "Làng Moori", Name = "Namếc"},
            new MapTranspot() {Id = 14, Info = "Làng Kakarot", Name = "Xay da"},
            new MapTranspot() {Id = 5, Info = "Đảo Kamê", Name = "Trái Đất"},
            new MapTranspot() {Id = 20, Info = "Vách núi đen", Name = "Xay da"},
            new MapTranspot() {Id = 13, Info = "Đảo Guru", Name = "Namếc"},
            null,    //9
            new MapTranspot() {Id = 27, Info = "Rừng Bamboo", Name = "Trái Đất"},
            new MapTranspot() {Id = 19, Info = "Thành phố Vegeta", Name = "Xay da"},
            new MapTranspot() {Id = 79, Info = "Núi khỉ đỏ", Name = "Fide"},
            new MapTranspot() {Id = 84, Info = "Siêu thị", Name = "Thiên đường tung tăng mua sắm"},
                        new MapTranspot() {Id = 154, Info = "Hành tinh Bill", Name = "Nơi ở của Binz ham ăn bánh Flan"},
                                                new MapTranspot() {Id = 52, Info = "Đại hội võ thuật", Name = "Tranh phong với các thiên kiêu chi tử, chiếu cáo thiên hạ"},

        };
        public static List<MapTranspot> NMapTranspots = new List<MapTranspot>()
        {
        //    null,   //0
            new MapTranspot() {Id = 85, Info = "Hành tinh M-2", Name = "Ngọc rồng sao đen"},
            new MapTranspot() {Id = 86, Info = "Hành tinh Polaris", Name = "Ngọc rồng sao đen"},
            new MapTranspot() {Id = 87, Info = "Hành tinh Cretaceous", Name = "Ngọc rồng sao đen"},
            new MapTranspot() {Id = 88, Info = "Hành tinh Monmaasu", Name = "Ngọc rồng sao đen"},
            new MapTranspot() {Id = 89, Info = "Hành tinh Rudeeze", Name = "Ngọc rồng sao đen"},
            new MapTranspot() {Id = 90, Info = "Hành tinh Gelbo", Name = "Ngọc rồng sao đen"},
            new MapTranspot() {Id = 91, Info = "Hành tinh Tigere", Name = "Ngọc rồng sao đen"},
        };
        public static List<long> TimeUseSkill = new List<long>() { 900000, 1800000, 3600000, 86400000, 259200000, 604800000, 1296000000 };
        public static List<short> ListDragonball = new List<short>() { 19,20 };
        public static List<short> ListAllDragonball = new List<short>() {14,15,16,17, 18, 19, 20 };

        public static List<short> ListDaNangCap = new List<short>() { 220, 221, 222, 223, 224 };
        public static List<short> ListSaoPhaLe = new List<short>() { 441, 442, 443, 444, 445, 446, 447 };

        public static List<short> ListSaoPhaLeLevel2 = new List<short>() {1457, 1458, 1459, 1460, 1461, 1462, 1463 };
        public static List<short> ListItemSaoPhaLeUpgrade = new List<short>() { 1457, 1458, 1459, 1460, 1461, 1462, 1463, 1464 };

        public static List<short> ListDoHuyDiet = new List<short>() { 650, 651, 652, 653, 654, 655, 656, 657, 658, 659, 660, 661, 662 };
        public static List<short> ListEventTrungThu = new List<short>() { 886, 887, 888, 889 };
        public static List<short> ListSach1 = new List<short>() { 1509, 1510, 1511 };
        public static List<short> ListSach2 = new List<short>() { 1313,1314,1315 };
        public static Dictionary<int, List<int>> OptionGodItem = new Dictionary<int, List<int>>()
        {
            {47, new List<int> (){800, 920, 850, 980 ,900, 1035 } },
            {0, new List<int> (){4300, 5060, 4400, 4945, 4500, 5175 } },
            {6, new List<int> (){ 48000, 59800, 50000, 57500    , 52000, 55200} },
                        {7, new List<int> (){48000, 55200, 50000, 57500, 46000, 52900} },
                        {14, new List<int> (){12, 12, 12, 12, 12, 12}}
        };

        //562 564 566 561
        public static List<short> ListBuaNguHanhSon = new List<short>() { 537, 538, 539, 540 };
        public static List<short> ListDoThanLinh = new List<short>() { 555, 556, 557, 558, 559, 560, 561,562, 563, 564, 565, 566, 567, 567 };
        public static List<short> ListDoHiem = new List<short>() { 241, 253, 265, 277, 233, 245, 257, 269, 237, 249, 261, 273 };
        public static List<short> ListThucAn = new List<short>() { 663, 664, 665, 666, 667 };
        public static List<short> ListManhAngel = new List<short>() { 1066, 1067, 1068, 1069, 1070};
        public static List<int> TypeItemExpire = new List<int>() { 5, 27, 11, 23, 24 };
        public static readonly List<short> VanSuNhuY2024 = new List<short>() { 1533, 1534, 1535, 1536, 1537 };
        public static List<short> ListPetID = new List<short>(){
                892,//Thỏ xám
                893,//Thỏ trắng
                908,//Ma phong ba
                909,//Thần chết cute
                910,//Bí ngô nhí nhảnh
                892,//Thỏ xám
                893,//Thỏ trắng
                908,//Ma phong ba
                909,//Thần chết cute
                910,//Bí ngô nhí nhảnh
                916,//Lính Tam Giác
                917,//lính vuông
                918,//lính tròn
                919,//búp bê
                936,//tuần lộc nhí
                919,//búp bê
                936,//tuần lộc nhí
                919,//búp bê
                936,//tuần lộc nhí
                942,//hổ mặp vàng
                943,//hổ mặp trắng
                944,//hỏ mặp xanh
                967,//sao la
                1008,//cua đỏ
                967,//sao la
                1008,//cua đỏ
                967,//sao la
                1008,//cua đỏ
                1039,//Thỏ ốm
                1040,//thỏ mặp
                1046,//khỉ bong bóng
            1107,
            1114,
            1188,
            1202,
            1203,
            1230,
            1231,
            1232,
            1250,
            1251,
            1292,
            1359 ,
                1388 ,
                1455 ,
                1476 ,
                1493 ,
                1499 ,
                1404,//PIKA
                1542 ,
                1527,
                1599,
                1600,
                };
        
        public static List<int> PetIdOptionGoc = new List<int>() { 197,199,201,203,205 };
        
        public static List<int> PetIdOptionPlus = new List<int>() { 198,200,202,204,206 };

        public static List<int> PetTierIndex = new List<int>() { 197,198,199,200,201,202,203,204,205,206 };

        public static List<int> PetMaxChiSoTier = new List<int>() { 0,0,5,8,10,13,16,20,25,30 };
        #region Combinne
        public static readonly long MAX_LIMIT_UPGRADE = 8;
        public static readonly long MAX_LIMIT_SPL = 8;
        public static readonly int DIV_FAKE_PERCENT_PL = 6;//Chia tỉ lệ phần trăm thành công khi đục lổ trang bị, 50%/3 = 15% thành công
        public static readonly int DIV_FAKE_PERCENT_UPGRADE = 3;//

        public static List<string> TextColor = new List<string> { "brown", "green", "blue", "light-red", "light-green", "light-blue", "red" };

        public static List<int> IdOptionGoc = new List<int>() { 0, 6, 7, 22, 23, 14, 27, 28, 47 };
        public static List<int> IdOptionPlus = new List<int>() { 50, 77, 80, 81, 94, 95, 96, 97, 98, 99, 100, 101, 103, 108 };

        public static List<List<int>> OptionBall = new List<List<int>>{
            new List<int> {108, 2},
            new List<int> {94, 2},
            new List<int> {50, 3},
            new List<int> {81, 5},
            new List<int> {80, 5},
            new List<int> {103, 5},
            new List<int> {77, 5},
        };
        public static int[][] PercentEyes = new int[][]{
           //vàng * 1tr, 50%, 10 ngọc
            new int[] {30, 80, 10}, // 0
            new int[] {30, 80, 10}, // 1
            new int[] {60, 50, 20},//2
            new int[] {90, 40, 30},//3
            new int[] {120, 20, 40},//4
            new int[] {15, 10, 50},//5
            new int[] {180, 8, 55},//6
            new int[] {240, 5, 60},//7
            new int[] {270, 3, 65},//8
            new int[] {300, 1, 80},//9
        };

        public static List<List<int>> OptionSPL = new List<List<int>>(){
            new List<int>() {95, 5},
            new List<int>() {96, 5},
            new List<int>() {97, 5},
            new List<int>() {98, 3},
            new List<int>() {99, 3},
            new List<int>() {100, 3},
            new List<int>() {101, 5},
            new List<int>() {191, 2},
            new List<int>() {192, 2},
        };
        public static List<List<int>> OptionSPL2 = new List<List<int>>(){
            new List<int>() {95, 6},
            new List<int>() {96, 6},
            new List<int>() {97, 6},
            new List<int>() {98, 5},
            new List<int>() {99, 5},
            new List<int>() {100, 5},
            new List<int>() {101, 6},
                      
        };
        public static List<List<int>> OptionSPLLapLanh = new List<List<int>>(){
            new List<int>() {95, 10},
            new List<int>() {96, 10},
            new List<int>() {97, 10},
            new List<int>() {98, 7},
            new List<int>() {99, 7},
            new List<int>() {100, 7},
            new List<int>() {101, 10},
                        new List<int>() {192, 4},
            new List<int>() {191, 4},

        };
        public static List<int> GetOptionSQL(short itemId)
        {
            switch (itemId)
            {
                case >= 14 and <= 20:
                    return OptionBall[itemId - 14];
                case 964:
                    return OptionSPL[7];
                case 965:
                    return OptionSPL[8];
                case >= 1457 and <= 1463:
                    return OptionSPL2[itemId - 1457];
                case >= 1467 and <= 1475:
                    return OptionSPLLapLanh[itemId - 1467];
                default:
                    return OptionSPL[itemId - 441];
            }
        }
        public static double[][] PercentPhaLe = new double[][]{
            //vàng * 1tr, 50%, 10 ngọc
            new double[] {10, 80, 10},
            new double[] {20, 50, 20},
            new double[] {40, 40, 30},
            new double[] {80, 20, 40},
            new double[] {90, 10, 50},
            new double[] {120, 1, 55},
            new double[] {180, 0.5, 60},
            new double[] {240, 0.3, 65},
            new double[] {300, 0.1, 80},
            new double[] {350, 0.01, 80},
            new double[] {500, 0.01, 80}

        };
        public static int[][] VongQuayLTN = new int[][]{
            //thỏi vàng, số lần quay
            new int[] {1, 1 },
            new int[] {5, 5 },
            new int[] {10, 10 },
            new int[] {48, 50},
            new int[] {95, 100},
           
        };
        public static List<int> ItemVongQuayLiTieuNuongNormal = new List<int>() { 441, 442, 443, 444, 445, 446 };
        public static List<int> ItemVongQuayLiTieuNuongRare = new List<int>() { 16, 17, 1150, 1151, 1152, 1153, 1154 };
        public static List<int> ItemVongQuayLiTieuNuongEpic = new List<int>() { 1263, 1264, 1195, 1196};

        public static List<List<int>> PercentUpgrade = new List<List<int>>{
            //số đá, vàng, %
            new List<int>(){2, 10, 80},
            new List<int>(){3, 20, 60},
            new List<int>(){4, 30, 40},
            new List<int>(){5, 40, 20},
            new List<int>(){6, 50, 10},
            new List<int>(){7, 60, 5},
            new List<int>(){8, 70, 3},
            new List<int>(){9, 80, 1}
        };

        public static List<int> PercentUpgradePorata2 = new List<int> { 100, 500000000, 50 };

        public static List<int> PercentUpgradePorata2Option = new List<int> { 100, 200000000, 40 };

        public static List<List<int>> OptionPorata2 = new List<List<int>>(){
            // option id , min, max
            new List<int>() {50, 7, 20},
            new List<int>() {77, 10, 20},
            new List<int>() {108, 5, 20},//né đòn
            new List<int>() {103, 10, 20},
            new List<int>() {94, 5, 15},
            new List<int>() {14, 2, 5   },
        };
        public static List<List<int>> OptionPhapSuTrangBi = new List<List<int>>(){
            // option id , max
            new List<int>() {232, 3},
            new List<int>() {233, 3},
            new List<int>() {234, 3},//né đòn
            new List<int>() {235,3},
            new List<int>() {236, 3},
            new List<int>() {237, 3},
                        new List<int>() {238, 3},
        };
        public static List<int> OptionIdPhapSuTrangBi = new List<int>(){
            231,
            232,
            233,
            234,
            235,
            236,
            237,
            238,
        };
        public static List<List<int>> OptionSachTuyetKy = new List<List<int>>(){
            // option id , min, max
            new List<int>() {50, 7, 20},
            new List<int>() {77, 10, 20},
            new List<int>() {108, 5, 20},//né đòn
            new List<int>() {103, 10, 20},
            new List<int>() {94, 5, 15},
            new List<int>() {14, 2, 10},
            new List<int>() {214, 2, 20},
        };
        public static List<int> CheckTypeUpgrade = new List<int>() { 223, 222, 224, 221, 220 };
        #endregion


        #region Skill
        public static List<short> TimeProtect = new List<short> { 3784, 150, 200, 250, 300, 350, 400, 450 };
        public static List<short> TimeHuytSao = new List<short> { 3781, 300 };
        public static List<short> TimeTroi = new List<short> { 3779, 50, 100, 150, 200, 250, 300, 360 };
        public static List<short> TimeDichChuyen = new List<short> { 3783, 10, 15, 20, 25, 30, 35, 40 };
        public static List<short> TimeStun = new List<short> { 3783, 3, 4, 5, 6, 7, 8, 9 };
        public static List<short> TimeThoiMien = new List<short> { 3782, 50, 60, 70, 80, 90, 100, 110 };
        public static List<short> IdMonsterPet = new List<short> { 8, 11, 32, 25, 43, 49, 50 };

        public static List<short> IdSkillDisciple1 = new List<short>() {0,2,4};
        public static List<short> IdSkillDisciple2 = new List<short>() {1,3,5};
        public static List<short> IdSkillDisciple3 = new List<short>() {6, 8, 9 };
        public static List<short> IdSkillDisciple4 = new List<short>() { 12, 12, 12, 12, 12, 19, 19, 13, 19 };
        public static List<short> RandomPointDisciple = new List<short>() { 0, 0, 0, 0, 0, 1, 1, 1, 0, 2, 2, 2, 2, 2, 2, 1, 1, 0, 1, 0, 0, 0, 2, 0, 2, 0, 2, 2, 1, 1, 0, 1, 0, 2, 1, 2, 0, 1, 0, 1, 0, 2, 1, 2 };

        public static List<SkillMonkey> SkillMonkeys = new List<SkillMonkey>()
        {
            new SkillMonkey() {Id = 192, Time = 60000, Hp = 40, Damage = 109},
            new SkillMonkey() {Id = 195, Time = 70000, Hp = 50, Damage = 110},
            new SkillMonkey() {Id = 196, Time = 80000, Hp = 60, Damage = 111},
            new SkillMonkey() {Id = 197, Time = 90000, Hp = 70, Damage = 112},
            new SkillMonkey() {Id = 198, Time = 100000, Hp = 80, Damage = 113},
            new SkillMonkey() {Id = 199, Time = 110000, Hp = 90, Damage = 114},
            new SkillMonkey() {Id = 200, Time = 120000, Hp = 100, Damage = 115},
        };

        public static List<int> SkillIdChuong = new List<int>() { 7, 8, 9, 10, 11, 12, 13, 21, 22, 23, 24, 25, 26, 27, 35, 36, 37, 38, 39, 40, 41 };
        public static List<int> SkillIdCanChien = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 28, 29, 30, 31, 32, 33, 34, 63, 64, 65, 66, 67, 68, 69 };
        #endregion

        #region Boss
        // ID BOSS
        public static readonly int BOSS_SUPER_BROLY_TYPE = 1;
        public static readonly int BOSS_BLACK_GOKU_TYPE = 2;
        public static readonly int BOSS_SUPER_BLACK_GOKU_TYPE = 3;

        public static readonly int BOSS_FIDE_01_TYPE = 4;
        public static readonly int BOSS_FIDE_02_TYPE = 5;
        public static readonly int BOSS_FIDE_03_TYPE = 6;

        public static readonly int BOSS_CELL_01_TYPE = 7;
        public static readonly int BOSS_CELL_02_TYPE = 8;
        public static readonly int BOSS_CELL_03_TYPE = 9;

        public static readonly int BOSS_COOLER_01_TYPE = 10;
        public static readonly int BOSS_COOLER_02_TYPE = 11;
        public static readonly int BOSS_THO_PHE_CO_TYPE = 12;
        public static readonly int BOSS_THO_DAI_CA_TYPE = 13;
        public static readonly int BOSS_CHILLED_TYPE = 14;
        public static readonly int BASE_BOSS_ID = -1000000;
        public static readonly int BOSS_GOKU_THIEN_SU_TYPE = 15;
        public static readonly int so4 = 16;
        public static readonly int so3 = 17;
        public static readonly int so1= 18;
        public static readonly int tieudoitruong = 19;
        public static readonly int tau77normal = 20;


        public static readonly int MAX_BASE_BOSS_ID = -500000;

        public static int CURRENT_BOSS_ID = -1000000;

        #endregion
        #region Lucky Box
        public static List<short> LuckBoxEpic = new List<short>() { 342, 343, 344, 345 };
        public static List<short> LuckBoxCommon = new List<short>() { 190, 441, 190, 442, 190, 443, 190, 190, 444, 190, 445, 190, 190, 446, 447 };
        public static List<short> LuckyBoxItems = new List<short>() {1547, 1546, 1545,1544, 1543, 1542,1538,1532,1531,1530,1529,1528,1527,1524,1523,1522,1521,1514,1515,1516};
        public static List<short> LuckyBoxRare = new List<short>() { 381, 382, 383, 384, 385, 18, 19, 20, 828, 829, 830, 831, 832,833,834,835,836,837,838,839,840,841,842, 956,1204, 1150, 1151, 1152, 1153, 1154, 441, 442, 443, 444, 445, 446, 447, 1274, 1235, 1195, 1196 };
        public static List<short> LuckyBoxItemExpire = new List<short>() { 1,3,5,7,10 };

        //{  1155, 1156, 1157, 1201, 1202, 1203, 1204, 1205, 1206, 1229, 1230, 1231, 1232, 1236, 195, 1196, 1197, 1188, 1185, 1186, 381, 382, 383, 384, 385, 1107 , 1106, 1105, 998, 997, 999, 964, 965, 951, 952, 953, 954, 955, 956, 957, 958, 959, 948, 942, 943, 944};
        public static List<short> LuckyBoxItemSpecial = new List<short>() { };
        #endregion

        #region Special Skill
        public static List<int> SpecialSkillTSD = new List<int>() { 0, 1, 2, 3, 4, 5, 9, 13, 17 };
        public static List<int> SpecialSkillTGHP = new List<int>() { 7, 11, 12, 14, 10, 19, 2 };
        #endregion

        public static List<int> LogTheoDoi = new List<int>() { 23213 };

    }
}