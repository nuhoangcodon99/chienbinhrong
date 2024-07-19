using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Model.Info.Skill
{
    public class SocolaMabu
    {
        public long Time { get; set; }
        public bool isSocola { get; set; }
        public SocolaMabu()
        {
            Time = -1;
            isSocola = false;
        }
    }
}
