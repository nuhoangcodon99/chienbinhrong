﻿using System.Collections;
using System.Collections.Generic;
using NgocRongGold.Model.Item;

namespace NgocRongGold.Model.Template
{
    public class ShopTemplate
    {
        public int Id { get; set; }
        public byte Type { get; set; }
        public string Name { get; set; }
        public List<ItemShop> ItemShops { get; set; }

        public ShopTemplate()
        {
            ItemShops = new List<ItemShop>();
        }
    }
}