using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Model.Info;
using NgocRongGold.Model.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.Bosses.Xinbato
{
    public class Xinbato : CharacterBase
    {
        public int Status { get; set; }
        public int Type { get; set; }
        public ICharacter CharacterFocus { get; set; }
        public InfoDelayBoss InfoDelayBoss { get; set; }
        public short Hair { get; set; }
        public short Body { get; set; }
        public short Leg { get; set; }
        public short BasePositionX { get; set; }
        public short BasePositionY { get; set; }
        public short RangeMove { get; set; }
       
    }
}
