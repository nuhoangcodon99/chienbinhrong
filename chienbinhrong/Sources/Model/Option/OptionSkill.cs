using System;
using NgocRongGold.Model.ModelBase;

namespace NgocRongGold.Model.Option
{
    public class OptionSkill : OptionBase
    {
        public override object Clone()
        {
            return new OptionSkill()
            {
                Id = Id,
                Param = Param
            };
        }
    }
}