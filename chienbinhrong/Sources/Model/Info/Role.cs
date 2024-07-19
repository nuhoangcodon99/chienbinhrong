using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using NgocRongGold.Model.Option;
using NgocRongGold.Model.Template;

namespace NgocRongGold.Model.Info
{
    public class Role2//role từ client
    {
        public short Small { get; set; }
        public int Frame { get; set; }
        public short Dx { get; set; }
        public short Dy { get; set; } = 0;
        public List<OptionItem> RoleOptions { get; set; }
    }
    public class Role1//role từ effect server
    {
        public List<Role1Template> Roles { get; set; } = new List<Role1Template>();
        public Role1Template RoleUsed { get; set; }
    }
}
