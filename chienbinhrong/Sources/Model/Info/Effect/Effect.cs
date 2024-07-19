using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Model.Info.Effect
{
    public class Effect
    {
        public int effId { get; set; }

        public int x { get; set; }

        public int y { get; set; }

        public int loop { get; set; } // số lần lặp
        public int tLoopCount { get; set; } // = 0 (delay)

        public int tLoop { get; set; } // thời gian của 1 lặp (0-100)

        public int layer{ get; set; }

    }
    /*
     * 
									this.tLoopCount++;
									if (this.tLoopCount == this.tLoop)
									{
										this.tLoopCount = 0;
										this.loop--;
										this.t = 0;
										if (this.loop == 0)
										{
											this.c.removeEffChar(0, this.effId);
										}
									}
    */
}

