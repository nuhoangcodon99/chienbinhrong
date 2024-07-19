using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Handlers.Item;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Threading;
using NgocRongGold.Model.Item;
using NgocRongGold.Model.ModelBase;
using NgocRongGold.Model.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sources.Model.Clan
{
    public class ClanBox
    {
        public List<Item> ItemBoxClan { get; set; }
        public ClanBox()
        {
            ItemBoxClan = new List<Item>(2);
        }
        public int BoxLength()
        {
            return 20;
        }

    }
}
