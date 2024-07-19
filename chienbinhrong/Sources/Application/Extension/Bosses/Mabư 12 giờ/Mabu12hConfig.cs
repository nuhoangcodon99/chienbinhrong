using NgocRongGold.Application.IO;
using NgocRongGold.Model.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.Bosses.Mabu12Gio
{
    public class Mabu12hConfig
    {
        public static readonly int TimeStart = 12;
        public static long Delay = ServerUtils.CurrentTimeMillis();
        public static Mabu12hStatus Status = Mabu12hStatus.CLOSE;
        public static readonly List<int> Bosses = new List<int> { 36, 37, 37, 38, 36, 36 };
        public static readonly List<Boss> DataBosses = new List<Boss>();
    }
}
