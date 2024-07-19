using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Model.Info
{
    public class LuckyNumber
    {
        public List<string> Number = new List<string> { ""};
        public int JoinId = -1;
        public int NumberWin = -1;
        public int NumberBuy = 1;
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
