using NgocRongGold.Model.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Model.Info.Effect
{
    public class EffectChar
    {
        public static EffectTemplate[] effTemplates;

        // Token: 0x04000502 RID: 1282
        public static sbyte EFF_ME;

        // Token: 0x04000503 RID: 1283
        public static sbyte EFF_FRIEND = 1;

        // Token: 0x04000504 RID: 1284
        public int timeStart;

        // Token: 0x04000505 RID: 1285
        public int timeLenght;

        // Token: 0x04000506 RID: 1286
        public short param;

        // Token: 0x04000507 RID: 1287
        public EffectTemplate template;
    }
}
