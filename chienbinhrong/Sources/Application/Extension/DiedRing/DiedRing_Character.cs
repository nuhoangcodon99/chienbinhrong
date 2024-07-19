
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.Model.Character;
namespace NgocRongGold.Application.Extension.DiedRing{
    public enum DiedRing_Character_Status
    {
        NORMAL = 0,
        FIGHTING = 1,
    }
    public enum DiedRing_Character_Win
    {
        LOSE = 0,
        WIN = 1,
    }
    public class DiedRing_Character{
        public int Round { get; set; }
        public bool Reward { get; set;}
        public DiedRing_Character_Win Win { get; set; } = DiedRing_Character_Win.LOSE;
        public DiedRing_Character_Status Status { get; set; } = DiedRing_Character_Status.NORMAL;
        public int Count { get; set; }
    }
}