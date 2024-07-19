using NgocRongGold.Model.Option;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Model.Template
{
    public class Role1Template
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<OptionItem> Options { get; set; }
        public long Delay { get; set; }
        public int Temp { get; set; }
        public short Second { get; set; }

    }
}
