    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.KeoBuaBao
{
    public class KeoBuaBao_Cache
    {
        public static readonly List<string> TextMenu = new List<string>()
        {
            "Hãy chọn mức cược",
            "|1|Mức thỏi vàng cược: {0}\n|1|Hãy chọn Kéo,Búa hoặc Bao\n|7|Thời gian 15 giây bắt đầu",
            "|1|Bạn ra cái {0}\n|1|Tôi ra cái {1}\n|1|Bạn thắng rồi huhu\n|1|Bạn nhận được {2} thỏi vàng",
                        "|7|Bạn ra cái {0}\n|7|Tôi ra cái {1}\n|7|Tôi thắng nhé <lêu lêu>\n|1|Bạn đã bị mất {2} thỏi vàng",
        };
        public static readonly List<List<string>> Menu = new List<List<string>>()
        {
            new List<string>{"1 thỏi\nvàng", "5 thỏi\nvàng", "10 thỏi\nvàng"},
            new List<string>{"Kẽo", "Búa", "Bao", "Đổi\nmức cược", "Nghỉ chơi"}
        };
    }
}
