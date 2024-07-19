using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.NamecBattlefield
{
    public class NamecBattlefield_Character
    {
        public int Point { get; set; } = 0;
        public int PointTemporary { get; set; }= 0;
        public NamecBattlefield_Character_Status Status { get; set; } = NamecBattlefield_Character_Status.NORMAL;
        public int Star { get; set;  } = 0;
        public int TeamId { get; set; } // 1 = cadic, 2 = fide
    }
}
