using NgocRongGold.Application.IO;
using NgocRongGold.Application.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.BlackballWar
{
    public class BlackballCache
    {
        public static readonly int TimeStart = 20;
        public static BlackballStatus Status = BlackballStatus.CLOSE;
        public static readonly List<int> ListNRSD = new List<int> { 372, 373, 374, 375, 376, 377, 378 };
        public static readonly List<int> ListMapNRSD = new List<int> { 85, 86, 87, 88, 89, 90, 91 };
        public static readonly String textWaitForPick = "Chưa thể nhặt lúc này,hãy đợi {0} giây nữa";
        public static readonly String textWaitToWin = "Cố giữ ngọc thêm {0} giây nữa sẽ thắng";
        public static readonly String textClanReward = "Chúc mừng bạn đã dành được Ngọc Rồng {0} sao đen cho Bang";
        public static readonly String textEnd = "Trò chơi đã kết thúc.Hẹn gặp lại vào 20h ngày mai ";
        public static readonly List<int> CostToPlusHp = new List<int>() { 10, 20, 30 };
        public static readonly List<int> PercentPlusHp = new List<int>() { 3, 5, 7 };
        public static long currTimeEndBlackBall = 21;
        public static long currTimeStartBlackBall = 20;
        public static long currTimeCanPick = -1;
        public static long Delay = ServerUtils.CurrentTimeMillis();

        public static List<List<string>> Menus = new List<List<string>>()
                {
                    new List<String> { "Hướng dẫn\nthêm", "Tham gia", "Từ chối" },
                    new List<String> { "Hướng dẫn\nthêm", "Từ chối" },
                };
        public static string Tutorial = "Mỗi ngày từ 20h -> 21h các hành tinh có Ngọc Rồng Sao Đen sẽ xảy ra 1 cuộc đại chiến\nNgười nào tìm thấy và giữ được Ngọc Rồng Sao Đen sẽ mang phần thưởng về cho bang của mình trong 1 ngày"
                                + "\nLưu ý mỗi bang có thể chiếm hữu nhiều viên khác nhau nhưng nếu cùng loại cũng chỉ nhận được 1 lần phần thưởng đó.\nCó 2 cách để thắng:\n1) Giữ ngọc sao đen trong 5 phút\n2)Sau 30 phút tham gia tàu sẽ đón về và đang giữ ngọc sao đen trên người\n"
                                + "Các phần thưởng như sau:\n1 sao đen : +21% sức đánh toàn bang\n2 sao đen: +35% Hp toàn bang\n3 sao đen: +35% Ki toàn bang\n4 sao đen: +14% giáp cho toàn bang\n 5 sao đen: + 14% né cho toàn bang\n6 sao đen: +35% tiềm năng sức mạnh nhận được cho toàn bang\n7 sao đen: +35% tiềm năng sức mạnh nhận được cho toàn bang";
    }
}
