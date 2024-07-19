using NgocRongGold.Application.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.Bosses.Mabu2Gio
{
    public class Mabu2hConfig
    {
        public static readonly int TimeStart = 14;
        public static long Delay = ServerUtils.CurrentTimeMillis();
        public static Mabu2hStatus Status = Mabu2hStatus.CLOSE;
    }
}