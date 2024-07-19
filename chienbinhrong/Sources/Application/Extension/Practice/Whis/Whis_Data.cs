using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.Practice.Whis
{
    public class Whis_Data
    {
        public int Level { get; set; } = 1;
        public Whis_Status Status = Whis_Status.LIVE;
        public double HighScore { get; set; } = 0;
        public long Time { get; set;} = 0;//
        public long TimeSetScore { get;set; } = 0;
    }
}
