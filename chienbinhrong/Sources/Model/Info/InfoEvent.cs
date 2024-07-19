using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Model.Info
{
    public class InfoEvent
    {
        public int PhieuBeNgoan { get; set; }
        public int PhaoBong { get; set; }
        public List<int> XinChuDauNam { get; set; }
        public int PhongBiTet2024 { get; set; }
        public int PointSuKienHungVuong { get; set; }

        public InfoEvent()
        {
            PointSuKienHungVuong = 0;
        }
    }
}
