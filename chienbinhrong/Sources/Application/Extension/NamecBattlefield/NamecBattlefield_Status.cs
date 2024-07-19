using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.NamecBattlefield
{
    public enum NamecBattlefield_Status
    {
         CLOSE = 0,
         REGISTER  = 1,
         OPEN = 2,
         NOT_ENOUGH_CHARACTER = 3,
    }
}
