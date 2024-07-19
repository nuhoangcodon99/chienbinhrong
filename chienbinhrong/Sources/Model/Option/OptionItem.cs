using System;
using NgocRongGold.Model.ModelBase;

namespace NgocRongGold.Model.Option
{
    public class OptionItem : OptionBase
    {
        public OptionItem()
        {
            
        }

        public override object Clone()
        {
            return new OptionItem()
            {
                Id = Id,
                Param = Param
            };
        }
     
    }
}