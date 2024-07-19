using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.SideQuest.HangNgay
{
    public class HangNgayQuest_Quest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public HangNgayQuest_Difficult Difficult { get; set; }
        public HangNgayQuest_Type Type { get; set; }
        public int MaxProgress { get; set; }
    }
}
