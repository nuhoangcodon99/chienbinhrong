using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Handlers.Item
{
    public class CombineHandler
    {
        public static bool NeedGood(Model.Character.Character character, long gold)
        {
            return character.InfoChar.Gold >= gold;
        }
        public static bool NeedDiamond(Model.Character.Character character, long diamond)
        {
            return character.AllDiamond() >= diamond;
        }
        public static int ConvertLevel2SaoPhaLeToVipSaoPhaLe(int id)
        {
            id = id switch
            {
                964 => 1474,
                965 => 1475,
                >= 1457 and <= 1463 => id + 10,

                _ => id,
            } ;
            return id;
        }
        public static int GetNgocRongUp(int id)
        {
            id = id switch
            {
                > 14 and <= 20 => id - 1,
                14 => 1015,

                _ => id,
            };
            return id;
        }
        public static int ConvertLevel1SaoPhaLeToLevel2SaoPhaLe(int id)
        {
            id = id switch
            {
               441 => 1457,//hut hp
               442 => 1458,// hut ki
               443 => 1459,//phan sat thuong
               444 => 1460,//xuyen giap % chuong
               445 => 1561,//xuyen giap % dam
               446 => 1562,//% vang
               447 => 1563,//%tnsm
                _ => id,
            };
            return id;
        }
    }
}
