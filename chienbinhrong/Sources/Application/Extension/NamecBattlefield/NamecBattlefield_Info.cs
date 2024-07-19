using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.NamecBattlefield
{
    public enum NamecBattlefield_Message
    {
        NEW_INFO = 0,
        UPDATE_POINT = 1,
        ADD_EFFECT = 2,
        UPDATE_LIFE = 4,
        UPDATE_TIME = 5,
    }
    public class NamecBattlefield_Info
    {
        public List<int> Characters = new List<int>();
        public int Point { get; set; } = 0;
        public int Life { get; set; } = 0;
        public int TeamId { get; set; } = 0;
        public int PointTemporary { get; set; } = 0;
        public void Clear()
        {
            PointTemporary = 0;
            Characters.Clear();
            Point = 0;
            Life = 0;
        }
    }
    
}
