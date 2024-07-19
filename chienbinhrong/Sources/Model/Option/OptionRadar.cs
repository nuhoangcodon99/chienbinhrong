using System;
using NgocRongGold.Model.ModelBase;

namespace NgocRongGold.Model.Option
{
    public class OptionRadar : OptionBase
    {
        public int ActiveCard { get; set; }

        public OptionRadar()
        {
            Id = 0;
            Param = 0;
            ActiveCard = 0;
        }

        public override object Clone()
        {
            return new OptionRadar()
            {
                Id = Id,
                Param = Param,
                ActiveCard = ActiveCard
            };
        }
    }
}