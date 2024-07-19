using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.DaiHoiVoThuat
{
    public class Reason
    {
        public static readonly string WIN_BECAUSE_ENEMY_OUT = "Đối thủ của bạn đã thoát, xin chờ tại đây ít phút để thi tiếp vòng sau";
        public static readonly string WIN_BECAUSE_ENEMY_DEAD = "Bạn đã thắng vòng này, xin chờ tại đây ít phút để thi tiếp vòng sau";
        public static readonly string FAIL_BECAUSE_OUT_MAP = "Bạn đã thất bại vì vi phạm quy chế thi đấu, hãy trở lại vào giải đấu lần sau";
        public static readonly string FAIL_BECAUSE_HAS_KILLED = "Bạn đã thua cuộc, hãy trở lại vào gỉải sau";
        public static readonly string FAIL_BECAUSE_NOT_FAIRPLAY= "Bạn đã thất bại vì vi phạm quy chế thi đấu, hãy trở lại vào giải đấu lần sau";
        public static readonly string WINNER = "Chúc mừng bạn đã vô địch giải đấu {0}";
        public static readonly string CONGRATULATION_WINNER = "{1} vừa vô địch giải đấu {0}";
    }
    public class ReasonResult
    {
    }
}
