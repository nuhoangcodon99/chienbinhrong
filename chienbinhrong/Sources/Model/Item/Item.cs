using System;
using NgocRongGold.Model.ModelBase;

namespace NgocRongGold.Model.Item
{
    public class Item : ItemBase, IDisposable
    {
        public int IndexUI { get; set; }
        public int SaleCoin { get; set; } = 1;
        public long BuyPotential { get; set; }

        public Item() : base()
        {
            
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

       
    }
}