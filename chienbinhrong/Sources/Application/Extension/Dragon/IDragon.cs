using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.Dragon
{
    public interface IDragon : IDisposable
    {
        public string Info { get; set; }
        public long Delay { get; set; }
        public long TimeDisappearing { get; set; }
        public long TimeAppearing { get; set; }
        public bool isApprearing { get; set; }

        public bool ConditionWish();
        public void Wish();
    }
}
