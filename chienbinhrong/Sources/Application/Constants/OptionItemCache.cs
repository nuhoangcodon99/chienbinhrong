using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NgocRongGold.Model.Option;

namespace NgocRongGold.Application.Constants
{
    public class OptionItemCache
    {
        public static List<OptionItem> GetOptionItems(List<List<int>> options)
        {
            var listOpt = new List<OptionItem>();
            options.ForEach(num =>
            {
                listOpt.Add(new OptionItem()
                {
                    Id = num[0],
                    Param = num[1],
                });
            });
            return listOpt;
        }
    }
}
