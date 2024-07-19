using NgocRongGold.Application.IO;
using NgocRongGold.Application.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace NgocRongGold.Application.Extension.DiedRing{
    public class DiedRing_Cache{
        public static readonly List<int> ListBosses = new List<int>{ 68, 69, 70, 71, 72 };
        public static DiedRing_Status diedRing_Status = DiedRing_Status.WAIT;
        public static List<int> ListCharacter = new List<int>();
        public static long Delay = 180000 + ServerUtils.CurrentTimeMillis();
        public static int TimeThoiMien = 5000;
    }
}