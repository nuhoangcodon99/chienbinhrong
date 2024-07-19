using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Model.Template
{
    public enum MenuPlayerEnum
    {
        NULL = 0,
        BUY_AVATAR = 1,
        PLEASE_DISCIPLE = 3,
        OAN_TU_TI = 8,
        BAN_ACC = 10,
        TRONG_DUA_HAU = 11,
    }
    public class MenuPlayerTemplate
    {
        public string Caption { get; set;}
        public string Caption2 { get; set; }
        public short MenuSelect { get; set; }
    }
}
