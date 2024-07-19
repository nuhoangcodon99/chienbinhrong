using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using NgocRongGold.Application.IO;
namespace NgocRongGold.Application.Menu
{
    public class MenuNpc
    {
        private static MenuNpc _instance;

        public MenuNpc()
        {

        }

        public static MenuNpc Gi()
        {
            if (_instance == null) _instance = new MenuNpc();
            return _instance;
        }
        public List<string> TextMenuChienTruongNamec = new List<string>
        {
            "Ngọc rồng Namec đang bị 2 thế lực tranh giành",
            "Ngọc rồng Namec đang bị 2 thế lực tranh giành\nHãy chọn cấp độ tham gia tùy theo sức mạnh của bản thân",
            "Con đã chọn Tự Do\nTiếp theo hãy chọn bên mà con muốn hỗ trợ\nCa Đíc: {0}\nFide: {1}",
            "Con đã chọn Tự Do và hỗ trợ cho {0}\nCa đíc: {1}\nFide: {2}\nLúc 18h35 sẽ bắt đầu chiến trường Namec khốc liệt",
            "Cướp hết ngọc rồng đem về đây cho ta"
        };
        
        public List<List<string>> MenuChienTruongNamec = new List<List<String>>()
        {
            new List<string>()
            {
                "Tranh ngọc\nrồng Namec",
            },
            new List<string>()
             {
                 "Hướng dẫn",
                 "Đổi điểm\nthưởng\n[{0}]",
                 "Bảng\nxếp hạng",
                 "Hạng\ntự do",
                 "Từ chối",
             },
             new List<string>()
             {
                 "Hướng dẫn",
                 "Đổi điểm\nthưởng\n[{0}]",
                 "Bảng\nxếp hạng",
                 "Từ chối",
             }, 
             new List<string>()
             {
                 "Hỗ trợ\nCa Đíc",
                 "Hỗ trợ\nFide"
             },
              new List<string>()
             {
                "Hủy\nđăng kí",
                "Đóng",
             },
              new List<string>()
             {
                "OK",
             }
        };
        public List<string> TextMenuJaco = new List<string>
        {
            "Gô Tên,Calích và Monaka đang gặp chuyện ở hành tinh Potaufeu\nHãy đến đó ngay !",
            "Bạn cần giúp gì à?"
        };

        public List<List<string>> MenuJaco = new List<List<String>>()
        {
           
                new List<string> { "Đến\nPotaufeu","Từ chối" },
            
            new List<string>()
             {
                "Quay về",
                "Từ chối"
             },
            
        };


        public List<string> TextBaOngGia = new List<string>()
        {
            "Ta có thể giúp gì cho con?\nCon đang có {0} VND, Con đã nạp: {1} VND.\nMã quà tặng riêng của con là:\n|8|{2}",
            "Con đã học kỹ năng thành công",
            "Hãy chọn máy chủ con muốn đổi"
        };
        public List<List<string>> MenuBroly = new List<List<string>>()
        {
            new List<string>()
            {
                "VIP 1",
                "VIP 2",
                "VIP 3",
                                "VIP 4",
                "Từ chối",
            },
             new List<string>()
            {
               "600K VND",
               "Từ chối",
            },
             new List<string>()
            {
                 "1.400K VND",
                 "Từ chối",
            },
              new List<string>()
            {
                 "2M VND",
                 "Từ chối",
            },
               new List<string>()
            {
                 "4M VND",
                 "Từ chối",
            }
        };
        public List<string> TextToriBot = new List<string>()
        {
            $"{ServerUtils.ColorNotSpace("red")}Trong thời gian mùa 7 diễn ra{ServerUtils.Color("red")}(từ 13/3/2024 đến hết 28/4/2024){ServerUtils.Color("brown")}Tạo nhân vật mới sẽ được X2 kinh nghiệm toàn mùa{ServerUtils.Color("brown")}Nếu nâng cấp VIP sẽ được nhận{ServerUtils.Color("brown")}nhiều ưu đãi hơn nữa.{ServerUtils.Color("brown")}Lưu ý: nâng cấp VIP chỉ được 3 lần mỗi mùa",
            $"{ServerUtils.ColorNotSpace("red")}Nâng cấp VIP 1 bạn sẽ được{ServerUtils.Color("brown")}Nhận ngay 14 thỏi vàng, 10 phiếu giảm giá 80%{ServerUtils.Color("brown")}X3 Kinh nghiệm toàn mùa. Tặng 1 đệ tử{ServerUtils.Color("brown")}Tàu ngầm 19 Cam 50 ngày.{ServerUtils.Color("brown")}Cải trang Goku SSJ4 30 ngày.{ServerUtils.Color("brown")}Thanh Long Đao 30 Ngày. Pet Godzilla 30 ngày.{ServerUtils.Color("brown")}Ngẫu nhiên 3 viên đá may mắn hoặc nâng cấp{ServerUtils.Color("brown")}5 Đá bảo vệ, 10 cuốn sách cũ",
            $"{ServerUtils.ColorNotSpace("red")}Nâng cấp VIP 2 bạn sẽ được{ServerUtils.Color("brown")}Nhận ngay 28 thỏi vàng, 10 phiếu giảm giá 80%{ServerUtils.Color("brown")}X3 Kinh nghiệm toàn mùa. Tặng 1 đệ tử Cumber{ServerUtils.Color("brown")}5 thẻ Đội trưởng vàng.{ServerUtils.Color("brown")}Tàu ngầm 19 Vàng 60 ngày.{ServerUtils.Color("brown")}Ngẫu nhiên 5 viên đá may mắn hoặc đá nâng cấp{ServerUtils.Color("brown")}Cải Trang Goku SSJ4 90 ngày{ServerUtils.Color("brown")}Pet Kong 60 ngày.{ServerUtils.Color("brown")}CT Hắc Mị Nương 90 ngày.{ServerUtils.Color("brown")}Thanh Long Đao 90 ngày.{ServerUtils.Color("brown")}10 Đá bảo vệ khóa, 15 cuốn sách cũ.",
            $"{ServerUtils.ColorNotSpace("red")}Nâng cấp VIP 3 bạn sẽ được{ServerUtils.Color("brown")}Nhận ngay 55 thỏi vàng, 10 phiếu giảm giá 80%{ServerUtils.Color("brown")}X3 Kinh nghiệm toàn mùa. Tặng 1 đệ tử Billy {ServerUtils.Color("brown")}Pet Kong và Godzilla vĩnh viễn.{ServerUtils.Color("brown")}10 Thẻ rồng thần Namec.{ServerUtils.Color("brown")}Ngẫu nhiên 10 viên đá may mắn hoặc đá nâng cấp{ServerUtils.Color("brown")}Cải Trang Goku SSJ4 180 Ngày{ServerUtils.Color("brown")}Cải trang Hắc Mị Nương và Mị Nương vĩnh viễn.{ServerUtils.Color("brown")}Cải trang Kakarot MUI 180 ngày.{ServerUtils.Color("brown")}Thú cưỡi Phượng Hoàng Băng vĩnh viễn.{ServerUtils.Color("brown")}Tàu ngầm 19 Vàng vĩnh viễn.{ServerUtils.Color("brown")}30 Đá bảo vệ khóa, 30 cuốn sách cũ.",
        };
        public List<List<string>> MenuToriBot = new List<List<string>>()
        {
            new List<string>()
            {
                "Vip 1",
                "Vip 2",
                "Vip 3",
                "Đóng",
            },
            new List<string>()
            {
                "500\nđiểm mùa\n[{0}]",
                "Đóng",
            },
            new List<string>()
            {
                "800\nđiểm mùa\n[{0}]",
                "Đóng",
            },
            new List<string>()
            {
                "2000\nđiểm mùa\n[{0}]",
                "Đóng",
            }
        };  
        public List<List<string>> MenuBaOngGia = new List<List<string>>()
        {
            new List<string>()
            {

                "Giftcode",
             "BXH\nSức mạnh",
           "Nhận\nNgọc xanh",
           //"Nhận\nĐệ tử",
           "Quy đổi\nvàng",
           "Đổi\nMật khẩu",
           "Mở\nthành viên"
            },

        };
        public List<string> TextMeo = new List<string>()
        {
            "Bạn có muốn nâng cấp đậu thần không",
            "Bạn có muốn huỷ nâng cấp đậu thần không (nhận lại 50% vàng)",
            "Bạn có muốn kết bạn với {0} không?",
            "Bạn có muốn xoá kết bạn với {0} không?",
            "Bạn có muốn dịch chuyển tới người chơi {0} không?\bTốn 10 ngọc để dịch chuyển và thời gian tự do là 20 phút",
            "Bạn có chắc chắn muốn rời bang hội ?",
            "{0} (sức mạnh {1})\nBạn muốn cược bao nhiêu vàng?",
            "Bạn có muốn xoá {0} khỏi danh sách thù địch không?",
            "Bạn chưa từng kích hoạt mã bảo vệ lần nào\nBạn có muốn dùng 50k vàng để kích hoạt không, mã bảo vệ của bạn là: {0}",
            "Bạn có muốn mở khoá rương không?",
            "Bạn có muốn khoá rương lại không?",
            "Bạn có muốn mua lại {0} từ {1} không?\nGiá ở cửa hàng là {2} thỏi vàng\nGiá tại đây là {3} thỏi vàng\nBạn cũng có thể nhận được {4} thỏi vàng khi người khác mua từ bạn (Cải trang của bạn sẽ không mất)",
            "Bạn có muốn Ban nhân vật {0}\nTài khoản là {1} không?",
            "Hãy chọn mức cược",
            "Bạn có muốn làm đệ tử của {0} không?",
            "Đợi {0} quyết định",
            "{0} muốn làm đệ tử của bạn, bạn có đồng ý không?",
             "Bạn có chắc chắn muốn trồng cây dưa hấu hộ {0} Không?\n(Hành động này tiêu hao 1 trái dưa hấu)",
        };
        public List<List<string>> MenuHeThong = new List<List<string>>()
        {
            new List<string>()
            {
                "Hủy",
                "Trang\nSau",
            },
             new List<string>()
            {
                "Hủy",
               // "Trang\nSau",
            },
        };
        public List<List<string>> MenuOsin = new List<List<string>>()
        {
            new List<string>()
            {
                "Đến Kaio",
                "Đến\nHành tinh\nbill",
                "Từ chối",
            },
             new List<string>()
            {
              "Cừa hàng",
                "Đến\nhành tinh\nngục tù",
             "Từ chối",
            },
              new List<string>()
            {
              "Đến\nHành tinh\nBill",
              "Từ chối",
            },
        };
        public List<List<string>> MenuAdmin = new List<List<string>>()
        {
             new List<string>()
            {
                "Check\nGifcode",
                "BUFF",
                "NRSD",
                "DHVT",
                "Mabu 12h",
                "Mabu 2h",
                "RESET\nDungeon"
            },
             new List<string>()
            {
                "ME",
                "FIND PLAYER:\n{0}",
            },
            new List<string>()
            {
                "Item",
                "Boss",
                "Task",
                "DeathNote",
                "Map",
                "Ban",
                "Potential",
                "Origanal\nPoint",
                "Money",
            },
            new List<string>()
            {
                "SET",
                "PLUS",
            },
         };
        public List<List<string>> MenuRobo = new List<List<string>>()
        {
            new List<string>()
            {
                "OK",
                "Từ chối",
            },

        };
        public List<List<string>> MenuMeo = new List<List<string>> ()
        {
            new List<string>()
            {
                "OK", 
                "Huỷ",
            },
            new List<string>()
            {
                "Đồng ý", 
                "Huỷ",
            },
            new List<string>()
            {
                "1,000\nvàng", 
                "10,000\nvàng", 
                "100,000\nvàng"
            },
            new List<string>()
            {
                "1tr\nvàng",
                "10tr\nvàng",
                "100tr\nvàng",
                "1ty\nvàng",
            },
            new List<string>()
            {
               "OK",
            },
        };

        public List<string> TextNoiBanh= new List<string>()
        {
            "Bạn muốn nấu bánh bằng gì?",
            "Để nấu bánh trung thu cần 10 trứng vịt muối, x10 bột mì, x10 gà quay, x10 đậu xanh + 100 ngọc. Bạn có đồng ý không?",
            "Để nấu bánh trung thu cần 10 trứng vịt muối, x10 bột mì, x10 gà quay, x10 đậu xanh + 25tr vàng. Bạn có đồng ý không?",
            "Xin chào {0}\nTôi là nồi nấu bánh\nTôi có thể giúp gì cho bạn?",
            "Hãy tìm đủ nguyên liệu và loại bánh muốn nấu",
            "Bánh đã nấu xong, hãy lấy bánh đi nào",
            "Bánh đang nấu, còn {0} có thể lấy bánh.",
        };
        
        public List<List<string>> MenuNoiBanh = new List<List<string>>()
        {
            new List<string>()
            {
                "Bằng ngọc", 
                "Bằng vàng",
            },
            new List<string>()
            {
                "Đồng ý", 
                "Hủy",
            },
             new List<string>()
            {
                "Từ chối",
                
            },
             new List<string>()
            {
                "Tự nấu\nbánh",
                "Từ chối",
            },
              new List<string>()
            {
                "Nấu\nBánh tét",
                "Nấu\nBánh chưng",
                "Từ chối",
            },
        new List<string>()
            {
                "OK",

            },
         new List<string>()
            {
                "Nấu\nBánh dầy",
                "Nấu\nBánh chưng\nLang liêu",
                "Từ chối",
            },
        };
        public List<List<string>> MenuGokuVoThan = new List<List<string>>()
        {
            new List<string>()
            {
                "Nâng Áo",
                "Nâng Quần",
                 "Nâng Găng",
                  "Nâng Giày",
                   "Nâng Rada",
            },
             new List<string>()
            {
               "Nâng cấp",
               "Từ chối",
            },
             new List<string>()
            {
              "Hủy",
             },

        };

        public List<string> TextThanMeo= new List<string>()
        {
            "Muốn uống nước thánh không cưng?",
            $"{ServerUtils.Color("red")}Bảng đổi điểm sự kiện xem tại thông báo của NRO Thần Long \b"+
            // $"{ServerUtils.Color("blue")}300 điểm : x10 đá bảo vệ, 5 viên cs trung thu, 10 thỏi vàng\b" + 
            // $"{ServerUtils.Color("blue")}500 điểm : x15 đá bảo vệ, 10 viên cs trung thu, 10 thỏi vàng\b" + 
            // $"{ServerUtils.Color("blue")}1000 điểm : x20 đá bảo vệ, 25 viên cs trung thu, item Lồng đèn, 20 thỏi vàng\b" + 
            // $"{ServerUtils.Color("blue")}3000 điểm : x30 đá bảo vệ, x99 viên cs trung thu, item lồng đèn, 30 thỏi vàng\b" + 
            // $"{ServerUtils.Color("blue")}5000 điểm : x35 đá bảo vệ, x99 viên cs trung thu, item lồng đèn, 35 thỏi vàng, ngẫu nhiên (phóng lợn, mèo mun, v.v)\b" + 
            // $"{ServerUtils.Color("blue")}5000 điểm : x35 đá bảo vệ, x99 viên cs trung thu, item lồng đèn, 35 thỏi vàng, ngẫu nhiên (phóng lợn, mèo mun, v.v)\b" + 
            // $"{ServerUtils.Color("blue")}7000 điểm : x50 đá bảo vệ, x99 viên cs trung thu, item lồng đèn, 40 thỏi vàng, ngẫu nhiên item, trang bị hủy diệt 10% trở lên\b" + 
            // $"{ServerUtils.Color("blue")}10000 điểm : x99 đá bảo vệ, x99 viên cs trung thu, item lồng đèn, 50 thỏi vàng, ngẫu nhiên item, trang bị hủy diệt 12% trở lên\b" + 
            $"{ServerUtils.Color("red")}BẠN CHỈ CÓ THỂ ĐỔI ĐIỂM DUY NHẤT MỘT LẦN",
            "Vui lòng chọn loại lồng đèn muốn nhận?",
        };
        
        public List<List<string>> MenuThanMeo = new List<List<string>>()
        {
            new List<string>()
            {
                "Đổi quà sự kiện", 
                "Đổi quà tích nạp",
            },
            //đổi sự kiện
            new List<string>()
            {
                "Đổi", 
                "Đóng",
            },
            //chọn lồng đèn
            new List<string>()
            {
                "SỨC ĐÁNH", 
                "HP",
                "KI",
                "CHÍ MẠNG",
                "GIÁP",
            },
        };

        public List<string> TextBaHatMit = new List<string>()
        {
            "Ngươi tìm ta có việc gì ?",
            "|2|Con muốn biến 10 mảnh đá vụn thành\n1 viên đá nâng cấp ngẫu nhiên\b|1|Cần 10 Mảnh đá vụ\nCần 1 bình nước phép\b|2|Cần 2 k vàng",
            "Ngươi Muốn pha lê hoá trang bị bằng cách nào",
            "Ta sẽ biến trang bị mới cấp cao hơn của ngươi thành trang bị có cấp độ và sao pha lê của trang bị cũ",
            "Ngươi muốn đổi Capsule World Cup ?\n|0|Ngươi đang có {0} thẻ Fan gà nửa mùa\n{1} thẻ Fan cuồng bóng đá\n|6|Đổi Capsule thường: cần 10 thẻ Fan gà nửa mùa và 1tr vàng\n2)Đổi Capsule VIP:cần 10 thẻ Fan cuồng bóng đá và 500 ngọc",
            "Pháp sư hóa trang bị sẽ được thêm chỉ số Pháp sư\nGiải pháp sư sẽ xóa tất cả chỉ số Pháp Sư\nNgươi muốn làm gì?",
        };
        public List<string> TextNgoKhong = new List<string>()
        {
            "Cha mi ngu",
        };
         public List<List<string>> MenuNgoKhong = new List<List<string>>()
        {
            new List<string>()
            {
                "Tặng quả\nHồng Đào",
                "Tặng quả\nHồng Đào\nChín",
            },
            
        };
        public List<string> TextDuongTang = new List<string>()
        {
            "A mi phò phò, thí chủ hãy giúp giải cứu đồ đệ của bần tăng đang bị\nphong ấn tại ngũ hành sơn.\nLưu ý: Tiềm năng khi đánh quái trong Ngũ Hành Sơn là X2",
            "Thí chủ muốn trở về sao ?",
            "A mi phò phò, thí chủ thu nhập bùa '[Giải Khai Phong Ấn]',Mỗi chữ 10 cái.\n" + $"{ServerUtils.ColorNotSpace("red")}(Chi tiết xem tại diễn đàn, fanpage)",
                        "A mi phò phò, thí chủ đang có {0} điểm, hãy chọn 1 phần quà\n500 cộng thêm 7 ngày sử dụng Cải Trang đang dùng\n600 cộng thêm 9 ngày sử dụng Cải Trang đệ tử đang dùng\n700 cải trang thành Bát Giới (dành riêng cho đệ tử)\n1.000 cải trang thành Tôn Ngộ Không (dành riêng cho đệ tử)\n1.300 cải trang thành Sa Tăng (dành riêng cho đệ tử)",
        };
        public List<List<string>> MenuDuongTang = new List<List<string>>()
        {
            new List<string>()
            {
                "Đồng ý",
                "Nhiệm vụ\nhộ tống",
                "Từ chối",
                "Nhận thưởng",
            },
            new List<string>()
            {
                "Đồng ý",
                "Từ chối",
            },
            new List<string> ()
            {
            "Giải\nPhong Ấn",
            "Top\nHoa Quả",
            },
            new List<string> ()
            {
            "500",
            "600",
            "700",
            "1.000",
            "1.300",
            }
        };
        public List<List<string>> MenuBaHatMit = new List<List<string>>()
        {
            new List<string>() // 0
            {
                "Thưởng Bùa Ngẫu Nhiên",
                "Sách\nTuyệt Kỹ",
                "Cửa hàng Bùa",
                "Nâng cấp Vật phẩm",
                "Làm phép Nhập đá",
                "Nhập Ngọc Rồng",
                "Nâng cấp\nBông tai\nPorata"
            },
            new List<string>() // 1
            {
                "Sách\nTuyệt Kỹ",
                "Cửa hàng Bùa",
                "Nâng cấp Vật phẩm",
                "Làm phép Nhập đá",
                "Nhập Ngọc Rồng",
                "Nâng cấp\nBông tai\nPorata"
            },
            new List<string>() // 2
            {
                "Bùa 1h",
                "Bùa 8h",
                "Bùa\n1 Tháng"
            },
            new List<string>() // 3
            {
               // "Ép sao\ntrang bị", 
               // "Pha lê\nhoá\ntrang bị", 
               "Ghép chữ\nđầu năm",
               "Bày mâm\nngũ quả",
               "Chức năng\nPha lê",
                "Chuyển hoá\nTrang bị",
               "Chức năng\nTinh chế",
               "Chức năng\nPháp sư",
              //  "Tinh chế\ntrang bị",
             //   "Phân giải\ntinh chế",
             "Võ đài sinh tử",
            },
            new List<string>() // 4
            {
                "Vào hành trang\nChọn trang bị\n(Áo,quần,găng,giày hoặc rada)\nChọn loại đá để nâng cấp\nSau đó chọn 'Nâng cấp'", 
                "Ta sẽ phù phép\ncho trang bị của ngươi\ntrở nên mạnh mẽ"
            },
            new List<string>() // 5
            {
                "Vào hành trang\nChọn 10 mảnh đá vụn\nChọn 1 bình nước phép\nSau đó chọn 'Làm phép'", 
                "Ta sẽ phù phép\ncho 10 mảnh đá vụn\ntrở thành 1 đá nâng cấp "
            },
            new List<string>() // 6
            {
                "Vào hành trang\nChọn 7 viên ngọc cùng sao\nSau đó chọn 'Làm phép'", 
                "Ta sẽ phù phép\ncho 7 viên Ngọc Rồng\nthành 1 viên Ngọc Rồng cấp cao"
            },
            new List<string>() // 7
            {
                "Làm phép\n2k vàng", 
                "Từ chối"
            },
            new List<string>()  // 8
            {
                "Vào hành trang\nChọn trang bị\n(Áo,quần,găng,giày hoặc rada)\nSau đó chọn 'Nâng cấp'", 
                "Ta sẽ phù phép\ncho trang bị của ngươi\ntrở thành trang bị pha lê"
            },
 

            new List<string>()//9
            {
                "Bằng ngọc", 
                "Từ chối"
            }, 

            new List<string>() // 10 
            {
                "Chuyển hoá\nDùng vàng", 
                "Chuyển hoá\nDùng ngọc"
            },

            new List<string>() // 11
            {
                "Vào hành trang\nChọn trang bị\n(Áo,quần,găng,giày hoặc rada)có ô đặt\n sao pha lê\n Chọn loại sao pha lê\nSau đó chọn 'Nâng cấp'",
                "Ta sẽ phù phép\ncho trang bị của ngươi\ntrở nên mạnh mẽ"
            },

            new List<string>() // 12
            {
                "Vào hành trang\n Chọn trang bị gốc\n (Áo,quần,găng,giày,rada) \ntừ cấp 4 trở lên\nChọn tiếp trang bị mới\nchưa nâng cấp cần nhập thể\nsau đó chọn 'Nâng cấp'",
                "Lưu ý trang bị mới\n phải hơn trang bị cũ 1 bậc"
            },
            new List<string>()  // 13 nâng cấp porata cấp 2
            {
                "Vào hành trang\nChọn bông tai Porata\nChọn mảnh vỡ bông tai để nâng cấp, số lượng\n9999 cái\nSau đó chọn 'Nâng cấp'", 
                "Ta sẽ phù phép\ncho bông tai Porata của người\nthành cấp 2"
            },
            new List<string>()  // 14 mở chỉ số porata cấp 2
            {
                "Vào hành trang\nChọn bông tai Porata cấp 2\nChọn mảnh hồn porata số lượng 99 cái và\nđá xanh lam để nâng cấp.\nSau đó chọn 'Nâng cấp'", 
                "Ta sẽ phù phép\ncho bông tai Porata cấp 2 của người\ncó 1 chỉ số ngẫu nhiên"
            },

            new List<string>() // 15
            {
                "Nở trứng\nLinh thú", 
                "Mở chỉ số\nLinh thú", 
                "Đổi chỉ số\nLinh thú", 
                "Nâng cấp\nLinh thú",
            },
            new List<string>() // 16
            {
                "Vào hành trang\nChọn 1 trứng linh thú\nChọn 99 hồn linh thú\n(Chọn 5 thỏi vàng nếu ngươi muốn nở nhanh)\nSau đó chọn 'Nâng cấp'", 
                "Ta sẽ làm phép\n cho trứng của ngươi sẽ nở"
            },
            new List<string>() // 17
            {
                "Vào hành trang\nChọn linh thú hạng C\nChọn 2x99 hồn linh thú\nChọn 2 thỏi vàng\nSau đó chọn 'Nâng cấp'", 
                "Ta sẽ làm phép\n mở chỉ số cho linh thú của ngươi"
            },
            new List<string>() // 18
            {
                "Vào hành trang\nChọn 1 linh thú có chỉ số\nChọn 99 hồn linh thú\nChọn 1 thỏi vàng\nSau đó chọn 'Nâng cấp'", 
                "Ta sẽ làm phép\n đổi chỉ số cho linh thú của ngươi"
            },
            new List<string>() // 19
            {
                "Vào hành trang\nChọn 2 linh thú cùng bậc thường để nâng cấp CỘNG\nHOẶC\n1 linh thú cấp CỘNG và 1 linh thú cấp thường để nâng hạng\n(Yêu cầu cùng loại linh thú\nvà cùng loại chỉ số)\nSau đó chọn 'Nâng cấp'", 
                "Ta sẽ làm phép\n linh thú của ngươi sẽ tiến hóa"
            },
            new List<string>() // 20
            {
                "Vào hành trang\nChọn 1 trang bị thiên sứ, hủy diệt, thần linh\nSau đó chọn x4 đá ngũ sắc\nSau đó chọn 'Nâng cấp'",
                "Trang bị kích hoạt\nnhận được ngẫu nhiên"
            },
            new List<string>() // 21
            {
                "Vào hành trang\nChọn 4 trang bị thần linh\nSau đó chọn 'Nâng cấp'",
                "Trang bị kích hoạt\nnhận được ngẫu nhiên"
            },
            new List<string>() // 22
            {
                "Vào hành trang\nChọn 1 trang bị thần linh\nSau đó chọn 'Nâng cấp'",
                "Nâng cấp thần linh -> hủy diệt"
            },
            new List<string> // 23
            {
                "Vào hành trang\nChọn mảnh đồ thiên sứ và công thức\nĐá nâng cấp (Nếu có)\nĐá may mắn(nếu có)\nthêm theo thứ tự",
                "Ta sẽ chế tạo\nTrang bị thiên sứ cho ngươi",
            },
            new List<string> // 24
            {
                "Vào hành trang\nChọn Trang bị và Đá Ngũ Sắc\nthêm theo thứ tự",
                "Ta sẽ tinh chế\nTrang bị của ngươi !",
            },
            new List<string> //25
            {
                "Cần 1 công thức\nMảnh trang bị tương ứng\n1 đá nâng cấp(tùy chọn)\n1 đá may mắn(tùy chọn)",
                "Chế tạo\ntrang bị thiên sứ",
                
            },
            new List<string>() // 26
            {
                "Ép sao\ntrang bị",
                "Pha lê\nhoá\ntrang bị",
                "Chuyển hoá\nTrang bị",
                "Nâng cấp\nhủy diệt",
                "Nâng cấp\nthần linh",
                "Tinh chế\ntrang bị",
                "Thông tin\nSự kiện"
            },
            new List<string>() // 27
            {
                "Chế tạo Bồn\nTắm gỗ",
                "Chế tạo Bồn\nTắm vàng"
            },
             new List<string>() // 28
            {
             "Vào hành trang\nChọn Trang bị và 2 Đá Ngũ Sắc\nthêm theo thứ tự",
                "Ta sẽ xóa tinh chế\nTrang bị của ngươi !",
            },
             new List<string>() // 29
            {
             "Vào hành trang\nChọn Mắt hỗn mang và đá ngũ sắc",
                "Ta sẽ nâng cấp mắt của ngươi !",
            },
              new List<string>() // 30
            {
                "Ép sao\ntrang bị",
                "Pha lê\nhoá\ntrang bị",
                "Chuyển hoá\nTrang bị",

                "Tinh chế\ntrang bị",
                "Phân giải\ntinh chế",
             //   "Chế tạo\nbồn tắm"
             "Võ đài sinh tử",
             "Thông tin\nsự kiện",
            },
               new List<string>() // 31
            {
                "Ép sao\ntrang bị",
                "Pha lê\nhoá\ntrang bị",
                "Nâng cấp\nSao pha lê",
                "Đánh bóng\nSao pha lê",
                "Cường hóa\nLỗ sao\npha lê",
                "Tạo đá\nHematite",
            },
                new List<string>() // 32
            {
                "Tinh chế\nTrang bị",
                "Phân giải\nTinh chế",
                
            },
                new List<string>() // 33
            {
                "Vào hành trang\nChọn đá Hematite\nChọn loại sao pha lê (cấp 1)\nSau đó chọn 'Nâng cấp''",
                "Ta sẽ phù phép\nnâng cấp Sao Pha lê\nthành cấp 2",

            },
                 new List<string>() // 34
            {
                "Vào hành trang\nChọn loại sao pha lê cấp 2 có từ 2 viên trở lên\nChọn 1 đá mài\nSau đó chọn 'Đánh bóng'",
                "Đánh bóng\nSao pha lê cấp 2",

            },
                 new List<string>() // 35
            {
                "Vào hành trang\nChọn trang bị có Ô sao thứ 8 trở lên chưa cường hóa\nChọn đá Hematite\nChọn dùi đục\nSau đó chọn 'Cường hóa'",
                "Cường hóa\nÔ Sao Pha Lê",

            },
                  new List<string>() // 36
            {
                "Vào hành trang\nChọn 5 sao pha lê cấp 2 cùng màu\nChọn 'Tạo đá Hematite'",
                "Ta sẽ phù phép\ntạo đá Hematite",

            },
                  new List<string>() // 37
                  {
                      "Đóng thành\nSách cũ",
                      "Đổi Sách\nTuyệt kỹ",
                      "Giám định\nSách",
                      "Tẩy\nSách",
                      "Nâng cấp\nSách\nTuyệt kỹ",
                      "Hồi phục\nSách",
                  },
                  new List<string>() // 38
                  {
                      "Đồng ý",
                      "Từ chối",
                  },
                  new List<string>() // 39
                  {
                      "Từ chối",
                  },
                   new List<string>() // 40
                  {
                     "Vào hành trang chọn\n1 sách cần giám định",
                "Ta sẽ phù phép\ngiám định sách đó cho ngươi",
                  },
                    new List<string>() // 41
                  {
                     "Vào hành trang chọn\n1 sách cần tẩy",
                "Ta sẽ phù phép\ntẩy sách đó cho ngươi",
                  },
                     new List<string>() // 42
                  {
                     "Vào hành trang chọn\nSách Tuyệt Kĩ 1 cần nâng cấp và\n10 kìm bấm giấy",
                "Ta sẽ phù phép\nnâng cấp Sách Tuyệt Kĩ",
                  },
                        new List<string>() // 43
                  {
                     "Vào hành trang chọn\nSách Tuyệt Kĩ cần phục hồi",
                "Ta sẽ phù phép\nphục hồi sách cho ngươi",
                  },
                        new List<string>() // 44
                  {
                     "Vào hành trang chọn\n1 sách cần phân rã",
                "Ta sẽ phù phép\nphân rã sách đó cho ngươi",
                  },
                         new List<string>() // 45
                  {
                     "Pháp sư\nhóa\nTrang bị",
                     "Giải\nPháp sư\nTrang bị",
                  },
                         new List<string>() // 47
                  {
                     "Ta sẽ Pháp Sư Hóa\ncho trang bị của ngươi\ntrở thành trang bị pháp sư",
                     "Vào hành trang\nChọn trang bị\nChọn Đá Pháp sư\nSau đó chọn 'Nâng cấp'",
                  },


                       new List<string>() // 46
                  {
                     "Ta sẽ Giải Pháp Sư\ncho trang bị của ngươi\ntrở thành trang bị nguyên bản",
                     "Vào hành trang\nChọn trang bị\nChọn bùa giải Pháp sư\nSau đó chọn 'Nâng cấp'",
                  },
        };
        public List<string> TextCayThongNoel = new List<string>()
        {
            "|0|Đang có {0} lượt trang trí\nTrang trí 2.000 lượt sẽ tặng: x2 exp toàn máy chủ trong 12 tiếng\nTrang trí 4.000 lượt sẽ tặng: x2 exp toàn máy chủ trong 24 tiếng\nTrang trí 7.000 lượt sẽ tặng: x3 exp toàn máy chủ trong 24 tiếng\nTrang trí 10.000 lượt sẽ tặng: x3 exp toàn máy chủ trong 36 tiếng\nTrang trí 18.000 lượt sẽ tặng: x4 exp toàn máy chủ trong 48 tiếng {1}",
        };
            public List<List<string>> MenuCayThongNoel = new List<List<string>>()
        {
            new List<string>()
            {
                "Trang trí",
                "Trang trí\nVIP",
                "Đóng",
            },
            new List<string>()
            {
                "Đồng ý",
                "Từ chối",
            },
            new List<string>()
            {
                
                "Từ chối",
            },
        };
    
        public List<string> TextBumma = new List<string>()
        {
            "Cưng cần trang bị gì cứ đến chỗ chị nhé",
            "Chị chỉ bán đồ cho người Trái Đất thôi nha cưng!",
        };
        
        public List<List<string>> MenuShopDistrict = new List<List<string>>()
        {
            new List<string>()
            {
                "Cửa hàng",
            },
            new List<string>()
            {
                "Cửa hàng", 
                "Từ chối",
            },
            new List<string>()
            {
                "Cửa hàng",
                "Mua lại\nvật phẩm\nđã bán"
            },
        };
        public List<string> TextBoMong = new List<string>()
        {
            "Ngươi muốn có thêm ngọc, có nhiều cách, nạp thẻ cào là nhanh nhất, còn không\nthì chịu khó làm vài nhiệm vụ sẽ được ngọc thưởng",
            "Bạn còn lại {0} nhiệm vụ chưa nhận\nPhần thưởng sẽ tương xứng tùy vào nhiệm vụ\nBạn muốn chọn nhiệm vụ khó hay dễ?"
        };

        public List<List<string>> MenuBoMong = new List<List<string>>()
        {
            new List<string> { "Nạp\nHồng ngọc", "Nhận ngọc\nMiễn phí", "Nhiệm vụ\nHằng ngày", "Quy đổi\nhồng ngọc" },
            new List<string>()
            {
                "Dễ",
                "Thường",
                "Khó",
                "Siêu khó",
            },
             new List<string>()
            {
                "Đóng",
            },
        };

        public List<string> TextDende = new List<string>()
        {
            "Anh, chị cần trang bị gì cứ đến chỗ em nhé",
            "Em... Chỉ bán đồ cho người Namếc thôi...!",
        };
        public List<string> TextAppule = new List<string>()
        {
            "Cậu cần trang bị gì cứ đến chỗ tôi nhé",
            "Ta chỉ bán đồ cho người Xayda siêu mạnh thôi, cút về hành tinh của ngươi mà mua đi!",
        };
        
        public List<string> TextBrief= new List<string>()
        {
            "Tàu vũ trụ Trái Đất sử dụng công nghệ mới nhất, có thể đưa ngươi đi bất kì đâu, miễn có tiền trả là được.",
            "Ta sẽ đưa ngươi trở về hành tinh của mình an toàn!",
        };
        
        public List<List<string>> MenuBrief = new List<List<string>>()
        {
            new List<string>()
            {
                "Đến Xayda", 
                "Đến Namếc", 
                "Siêu thị",
            },
            new List<string>()
            {
                "Đến Xayda", 
                "Đến Namếc",
            },
        };
        
        public List<string> TextCargo= new List<string>()
        {
            "Tàu vũ trụ Namec sử dụng công nghệ mới nhất, có thể đưa ngươi đi bất kì đâu, miễn có tiền trả là được.",
        };
        
        public List<List<string>> MenuCargo = new List<List<string>>()
        {
            new List<string>()
            {
                "Đến\nTrái Đất", 
                "Đến Xayda", 
                "Siêu thị",
            },
            new List<string>()
            {
                "Đến\nTrái Đất", 
                "Đến Xayda", 
            },
        };
        
        public List<string> TextCui = new List<string>()
        {
            "Tàu vũ trụ Xayda sử dụng công nghệ mới nhất, có thể đưa ngươi đi bất kì đâu, miễn có tiền trả là được.",
            "Đội quân Fide đang ở thung lũng Nappa, ta sẽ đưa ngươi đến đó",
            "Ngươi muốn về thành phố Vegeta ?",
        };
        
        public List<List<string>> MenuCui = new List<List<string>>()
        {
            new List<string>()
            {
                "Đến\nTrái Đất", "Đến Namếc", "Siêu thị",
            },
            new List<string>()
            {
                "Đến\nTrái Đất", "Đến Namếc", 
            },
            new List<string>()
            {
                "Đến Cold", "Đến Nappa", "Từ Chối"
            },
            new List<string>()
            {
                "Đồng ý", "Từ Chối"
            },
        };
        public List<string> TextHungVuong = new List<string>()
        {
            "Người tìm ta có việc gì?",
        };

        public List<List<string>> MenuHungVuong = new List<List<string>>()
        {
            new List<string>()
            {
                "Dâng\nsính lễ",  "Dâng\nsính lễ\nxịn", "Dâng\nBánh dầy","Dâng\nBánh chưng\nLang Liêu"
            },
           new List<string>()
            {
                "Đồng ý",
                "Từ chối",
            },
           new List<string>()
            {
                "Từ chối"
            },
        };

        public List<string> TextQuyLao = new List<string>()
        {
            "Con muốn hỏi gì nào?",
            "Chào con, to rất vui khi gặp con\nCon muốn làm gì nào?",
            "Mở vào ngày 10/11",
            "Con có muốn huỷ học kỹ năng này và nhận lại 50% số tiềm năng không ?",
            "|2|Con đang có: {0} Điểm Sự Kiện\n" +"Con có chắc muốn đổi 50 Điểm Sự Kiện lấy:\n|2|Cải trang [Diệt Quỷ]\n" + "|7|Cơ hội nhận được [Đá bảo vệ]\n bảo vệ trang bị không bị rớt cấp\n khi nâng cấp thất bại",
            "Con có muốn dùng 10 chữ,mỗi chữ 3 cái\n để đổi lấy vật phẩm đặc biệt không?\n1) Fan gà nửa mùa với 200 triệu vàng\n2) Fan cuồng bóng đá với 1000 ngọc",
             "Ta sẽ cho con chữ, con hãy chọn thiệp để nhận chữ\nPhí là 1 ngọc nhé",
             "Con đang có {0} phiếu bé ngoan\nCon muốn đổi quà gì nào ?\n30 phiếu: Mèo mun đột biến (đeo lưng) hạn sử dụng 15 ngày\n50 phiếu: Xí muội Hoa Đào, Xí muội Hoa Mai\n70 phiếu: Bóng heo hồng hạn sử dụng 30 ngày\n99 phiếu: Cờ dây pháo (đeo lưng) Hạn sử dụng 15,30 ngày",
             "Sự kiện đua Top nhận quà khủng\nTop phong bì Tết: 12h00 15/2/2024\nTop pháo hoa: Bắt đầu lúc 12h00 15/2/2024\nKết thúc và trao giải lúc 23:59:59 22/2024 (hết mùng 12 tết)\n(Bắt đầu sau {0} ngày nữa)\nHạn chót nhận giải là hết ngày 27/2/2024\nĐến gặp ta để nhận giải nhé\nChi tiết xem tại diễn đàn, Fanpage",
             "Con có chắc muốn đổi 10 điểm pháo bông lấy ngẫu nhiên:\nCải trang Kimono Hạn sử dụng, Vĩnh viễn\nCải trang Goku, Cadic, Pocolo thần tài Hạn sử dụng\nCải trang Bunma Thanh Lịch Hạn sử dụng\nCải trang Mèo Karin Kid Lân Hạn sử dụng\nThú cưỡi cá chép râu rồng Hạn sử dụng, Vĩnh viễn"

        };
        public List<string> TextOsins = new List<string>()
        {
            "Bây giờ tôi sẽ bí mât...\nđuổi theo 2 tên đồ tể...Quý vị nào muốn theo thì xin mời!",
            "Mabư đã thoát khỏi vỏ bọc\nMau đi cùng ta ngăn chặn hắn lại\ntrước khi hắn tàn phá Trái Đất này",
            "vào lúc 12h tôi sẽ bí mât...\nđuổi theo 2 tên đồ tể...Quý vị nào muốn theo thì xin mời!",
            "Cút về phe của ngươi mà thể hiện !",
            "Đừng vội xem thường Babiđây, ngay cả cha hắn là ma thần đạo sĩ\nBibiđây khi còn sống cũng phải sợ hắn !",
            "Ta có thể giúp gì cho ngươi ?",
            "Ta sẽ phù hộ ngươi bằng\nnguồn sức mạnh của Thần Kaio\n+1 triệu HP, +1 triệu KI, +10k Sức đánh\nLưu ý:sức mạnh này sẽ biến mất khi ngươi rời khỏi đây",
                       
        };
        public List<List<string>> MenuOsins = new List<List<string>>()
        {
           new List<string> { "OK", "Từ chối" },
           new List<string> { "Hướng\ndẫn\nthêm", "Giải trừ\nphép thuật\n1 ngọc", "Xuống\nTầng dưới" },
           new List<string> { "Phù hộ\n10 Ngọc", "Từ chối", "Về\nĐại hội\nVõ thuật" },
        };
        public List<List<string>> MenuQuyLao = new List<List<string>>()
        {
            new List<string>()  
            {
             //   "Nói\nchuyện", "Kho báu\ndưới biển","Bỏ qua\nNV\nTrung úy","Bỏ qua\nNV\nĐHVT",
                          //  "Nói\nchuyện", "Kho báu\ndưới biển", "Sự kiện\nĐua Top", "Đổi điểm\nPháo bông\n({0}/10)", "Xin chữ\nđầu năm", "Đổi phiếu\nbé ngoan\nlấy quà",
                          "Nói\nchuyện", "Kho báu\ndưới biển", "Đổi điểm\nsự kiện\n[{0}]", "Sự kiện\nĐua Top",
            },
            new List<string>()
            {
                "Nhiệm vụ", "Học\nKỹ năng"
            },
            new List<string>()
            {
                "Top\nBang hội", "Thành\ntích bang", "Chọn\ncấp độ", "Từ chối"
            },
            new List<string>()
            {
                "Đổi","Từ chối"
            },
            new List<string>()
            {
                "Tùy chọn\n1","Tùy chọn\n2","Đóng"
            },
             new List<string>()
            {
                "Nhiệm vụ", "Học\nKỹ năng", "Giải tán\nBang hội", "Về khu\nvực bang"
            },
             new List<string>()
            {
                "Top 100\nPháo hoa", "Top 100\nMở phong bì\nTết 2024", "Xem điểm", "Đóng",
            },
             new List<string>()
            {
                "Đồng ý","Từ chối"
            },
             new List<string>()
            {
                "Xin bằng\nThiệp đỏ","Xin bằng\nThiệp đỏ VIP","Từ chối"
            },
              new List<string>()
            {
               "30", "50", "70", "99", "Đóng"
            },
              new List<string>()
            {
               "Top dâng\nbánh dầy", "Top mở\nhộp quà\ncao cấp", "Xem điểm", "Đóng",
            },
               new List<string>()
            {
               "Đóng",
            },
        };

        public List<string> TextSanta = new List<string>()
        {
            "Xin chào, ta có một số vật phẩm đặc biệt, cậu có muốn xem không?",
            "Giới hạn vàng của bạn đang là {0} vàng, bạn có muốn nâng thêm 200Tr không?",
            "|0|Con đang có: {0} VND\nTổng Nạp: {1} VND",
            "|7|Con đang có: {0} VND",
            "|7|Con đang có: {0} VND\n|6|Muốn kích hoạt thành viên cần 20.000 VND",
            "|7|Con đang có: {0} VND\n|6|Con muốn đổi vàng hay ngọc ?",
        };
        
        public List<List<string>> MenuSanta = new List<List<string>>()
        {
            new List<string>()
            {
                 "Cửa\nhàng", "Mở rộng\nHành tranh\nRương đồ","Nhập mã\nquà tặng","Cửa hàng\nHạn sử dụng","Tiệm\nHớt tóc","Danh\nhiệu"//,//"Tặng\nMâm ngũ\nQuả"
            },
            new List<string>()
            {
                "Nâng 200Tr\nGiá 200Tr", "Từ chối"
            },
            new List<string>()
            {
                "Đổi vàng\nngọc", "Kích hoạt\nThành viên", "Từ chối"
            },
            new List<string>()
            {
                "Đổi vàng","Đổi Ngọc",
            },
            new List<string>()
            {
                "10K\n10 Thỏi","20K\n20 Thỏi", "50K\n50 Thỏi", "100k\n100 Thỏi", "200k\n200 Thỏi", "500k\n500 Thỏi"
            },
            new List<string>()
            {
                "10K\n20K Ngọc","20K\n40K Ngọc", "50K\n100K Ngọc", "100k\n200K Ngọc", "200k\n400k Ngọc", "500k\n1m Ngọc"
            },
            new List<string>()
            {
                "OK","Hủy"
            }
        };
        
        public List<string> TextQuocVuong = new List<string>()
        {
            "Con muốn nâng giới hạn sức mạnh\ncho bản thân hay đệ tử?"
        };
        
        public List<List<string>> MenuQuocVuong = new List<List<string>>()
        {
            new List<string>()
            {
                "Bản thân", "Đệ tử", "Từ chối"
            },
            new List<string>()
            {
                "Nâng\nGiới hạn\nSức mạnh","Nâng ngọc","OK"
            },
            new List<string>()
            {
                "Nâng ngay\ncho đệ tử\n%d ngọc","OK"
            },
        };

        public List<string> TextThuongDe = new List<string>()
        {
            "",
            "Con đã mạnh hơn ta, ta sẽ chỉ đường cho con đến Kaio để gặp thần Vũ Trụ Phương Bắc\nNgài là thần cai quản vũ trụ này, hãy theo ngài ấy học võ công",
            "Con có thể chọn từ 1 đến 7 viên\ngiá mỗi viên là 4 ngọc\nƯu tiên dùng vé quay trước.",
            "Con có chắc muốn xóa tất cả vật phẩm trong rương phụ không?"
        };

        public List<List<string>> MenuThuongDe = new List<List<string>>()
        {
            new List<string>()
            {
                "Bản thân"
            },
            new List<string>()
            {
                "{0}",
                "Tập luyện\nvới\nMr.Pôpô",
                "Tập luyện\nvới\nThuợng Đế",
                "Đến\nKaio",
                "Vòng quay\nMay mắn",
            },
            new List<string>()
            {
                "Vòng quay\nMay mắn",
                "Vòng quay\nĐặc biệt\nSự kiện",
              
            },
            new List<string>()
            {
                "Xóa",
                "Từ chối",
            },
        };

        public List<string> TextThanVuTru = new List<string>()
        {
            "Con muốn điều gì?",
        };

        public List<List<string>> MenuThanVuTru = new List<List<string>>()
        {
            new List<string>()
            {
                "Đăng ký\ntập\ntự động",
                "Tập luyện\nvới\nBubbles",
                "Tập luyện\nvới\nThần Vũ\nTrụ",
                "Di chuyển"
            },
        };

        // Rồng thần
        public string TextRongThan = "Ta sẽ ban cho ngươi 1 điều ước, hãy suy nghĩ thật kỹ trước khi quyết định";

        public List<string> MenuDieuUocRongThan = new List<string>()
        {
            "Găng tay\nđang mang\nlên 1 cấp",
            "Chí mạng\nGốc +2%",
            "Thay\nChiêu 2-3\nĐệ tử",
            "Găng tay\nĐệ tử\nđang mang\nlên 1 cấp",
            "Điều ước\nkhác",
        };
        public List<string> MenuDieuUocRongThanOther = new List<string>()
        {
            "Đẹp trai\nnhất\nVũ trụ",
            "Ước Capsule\n1 Sao",
            //"+200 Tr\nSức mạnh\nvà tiềm\nnăng",
            "Giàu có\n+2 Tỷ Vàng",
            "Giàu có\n+50 Hồng ngọc",
            "Rương sao\nPha lê",
            "Điều ước\nkhác",
        };
        // Trứng ma bư
        public List<string> TextQuaTrung = new List<string>()
        {
            "Bạn có chắc chắn thay thế để tự hiện tại thành đệ tử Ma Bư?",
            "Hãy chọn hành tinh cho đệ tử Ma Bư của bạn.",
        };

        public List<List<string>> MenuQuaTrung = new List<List<string>>()
        {
            new List<string>()
            {
                "{0}",
                "Nở Ngay\n1,1 Tỷ Vàng",
                "Đóng",
            },
            new List<string>()
            {
                "Nở",
                "Đóng",
            },
            new List<string>()
            {
                "Trái Đất",
                "Namếc",
                "Xayda"
            },
        };
        public List<string> TextObito = new List<string>()
        {
            "Ta là nhẫn giả từ thế giới Nhẫn Đạo Naruto, ngươi muốn gì?",
        };

        public List<List<string>> MenuObito = new List<List<string>>()
        {
             new List<string> { "Nâng cấp\nmắt\nhỗn mang","Chế tác\nmắt\nhỗn mang", "Pha lê\nHóa cải\nTrang Obito" },
                          new List<string> { "Từ chối" },
                                                    new List<string> { "Đồng ý", "Từ chối" },


        };
        public List<string> TextBBGolden = new List<string>()
        {
            "Ta là Bông băng Golden, ngươi cần ta giúp quy đổi gì?",
        };

        public List<List<string>> MenuBBGolden = new List<List<string>>()
        {
             new List<string> { "Hũ vàng","Thỏi vàng", "Hồng ngọc" },
                         

        };
        // Text nạp card
        public List<string> TextNapThe = new List<string>()
        {
            "Con hãy chọn loại thẻ mà con muốn nạp.",
            "Tốt lắm, giờ con hãy chọn mệnh giá thẻ nạp trước khi nhập mã nha\nLưu ý: Khi chọn sai mệnh giá sẽ bị trừ 50%."
        };


        public List<List<string>> MenuNapThe = new List<List<string>>()
        {
            new List<string>()
            {
                "Viettel",
                "Vinaphone",
                "Mobifone",
                "Zing",
                "Hủy"
            },
            new List<string>()
            {
                "10,000đ",
                "20,000đ",
                "30,000đ",
                "50,000đ",
                "Mệnh giá\nkhác",
                "Hủy",
            },
            new List<string>()
            {
                "100,000đ",
                "200,000đ",
                "300,000đ",
                "500,000đ",
                "1,000,000đ",
                "Hủy",
            },
            new List<string>()
            {
                "WORLD",
               
                "Hủy"
            },
        };

        // Text nội tại
        public List<string> TextNoiTai = new List<string>()
        {
            "Nội tại là một kỹ năng bị động hỗ trợ đặc biệt\nBạn có muốn mở hoặc thay đổi nội tại không?",
            "Bạn có muốn đổi Nội Tại khác với giá là {0} ngọc?",
            "Bạn có muốn mở Nội Tại Bằng Vàng với giá là {0} vàng?"
        };

        public List<List<string>> MenuNoiTai = new List<List<string>>() 
        {
            new List<string>()
            {
                "Xem\ntất cả\nNội tại",
                "Mở VIP",
                "Mở\nNội tại",
                "Từ chối"
            },
            new List<string>()
            {
                "Mở Nội Tại",
                "Từ chối"
            },
            new List<string>()
            {
                "Mở Bằng Vàng",
                "Từ chối"
            },
        };

        // Ca lich
        public List<string> TextCalich = new List<string>()
        {
            "Chào chú, cháu có thể giúp gì?",
            @"20 năm trước bọn Android sát thủ đã đánh bại nhóm bảo vệ trái đất của Sôngoku và Cađíc, Pôcôlô..
            Riêng Sôngoku vì bệnh tim đã chết trước đó nên không thể tham gia trận đánh...
            Từ đó đến nay bọn chúng tàn phá Trái Đất không hề thương tiếc. Cháu và mẹ may mắn sống sót nhờ lẩn trốn tại tầng hầm của công ty Capsule...
            Cháu tuy cũng là siêu Xayda nhưng cũng không thể làm gì được bọn Android sát thủ...
            Chỉ có Sôngoku mới có thể đánh bại bọn chúng, mẹ cháu đã chế tạo thành công cỗ máy thời gian và cháu quay về quá khứ để cứu Sôngoku...
            Bệnh của Gôku ở quá khứ là nan y, nhưng với trình độ y học tương lai chỉ cần uống thuốc là khỏi...
            Hãy đi theo cháu đến tương lai giúp nhóm Gôku đánh bại bọn Android sát thủ. Khi nào chú cần sự giúp đỡ của cháu hãy đến đây nhé.",
        };

        public List<List<string>> MenuCalich = new List<List<string>>() 
        {
            new List<string>()
            {
                "Kể chuyện",
                "Đi đến\nTương lai",
                "Từ chối",
            },
            new List<string>()
            {
                "Quay về\nQuá khứ",
                "Từ chối",
            },
        };

        public List<string> TextGiuMa = new List<string>()
        {
            "Ngươi đang muốn tìm mảnh vỡ và mảnh hồn bông tai Porata trong truyền thuyết, ta sẽ đưa ngươi đến đó.",
            "Thời gian khiên chiến Boss là 30 phút.\nTop 5 đánh boss +15 Capsule Bang\nTop 10 đánh boss +10 Capsule Bang\nTop 11 trở lên đánh boss +5 Capsule Bang\nNgười đánh boss cuối cùng sẽ được thưởng thêm 10 Capsule Bang\nMở cửa vào ngày thứ 7, chủ nhật hàng tuần",
        };

        public List<List<string>> MenuGiuMa = new List<List<string>>() 
        {
            new List<string>()
            {
               "Khiêu chiến\nBoss",
               "Điểm danh\n+1 Capsule\nBang",
               "OK",
               "Đóng"
            },
            new List<string>()
            {
               "Khiêu chiến\nBoss",
               "OK",
               "Đóng"
            },
             new List<string>()
            {
               "Miễn phí",
               "Đóng",
            },
             new List<string>()
            {
               "100 Ngọc",
               "Đóng",
            },
             new List<string>()
            {
               "300 Ngọc",
               "Đóng",
            },
        };

        public List<string> TextBill = new List<string>()
        {
            "Ngươi tìm ta có việc gì.",
            "Ngươi trang bị đủ bộ 5 món trang bị Thần\nvà mang 99 phần đồ ăn tới đây...\nrồi ta nói chuyện tiếp.",
            "Đói bụng quá...ngươi mang cho ta 99 phần đồ ăn, ta sẽ đổi cho một món đồ Hủy Diệt bằng THỎI VÀNG.\nNếu tâm trạng ta vui ngươi có thể nhận được trang bị tăng đến 15%"
        };


        public List<List<string>> MenuBill = new List<List<string>>() 
        {
            new List<string>()
            {
                "Nói chuyện",
                "Từ chối",
            },
            new List<string>()
            {
                "OK",
            },
            new List<string>()
            {
                "OK",
                "Từ chối",
            },
        };

        public List<string> TextTrungThu= new List<string>()
        {
            "Vui trung thu cùng NRO GOLD để nhận được nhiều phần quà hấp dẫn.\nBạn muốn xem gì?",
        };
        
        public List<List<string>> MenuTrungThu = new List<List<string>>()
        {
            new List<string>()
            {
                 "Cửa hàng\nTrung thu",
                 "BXH\nSự kiện\nTrung thu", 
                //"BXH\nSự kiện\nTop nạp",
            },
        };
    }
}