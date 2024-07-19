using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.BigBoss
{
    public class AutoSpawn
    {
        public BigBoss GauTuongCuop = new BigBoss((int)BigBoss_Template.GAU_TUONG_CUOP,BigBoss_Cache.Maps_GauTuongCuop) { TimeResummon = 1800000 };
        public BigBoss CoMayHuyDiet = new BigBoss((int)BigBoss_Template.CO_MAY_HUY_DIET, BigBoss_Cache.Maps_GauTuongCuop) { TimeResummon = 1800000 };
        public BigBoss BachTuoc = new BigBoss((int)BigBoss_Template.VUA_BACH_TUOC, BigBoss_Cache.Maps_BachTuoc) { TimeResummon = 1800000 };
        public BigBoss Piano = new BigBoss((int)BigBoss_Template.PIANO, BigBoss_Cache.Maps_Piano) { TimeResummon = 1800000 };

        public void Start()
        {
            GauTuongCuop.AutoSpawn();
            CoMayHuyDiet.AutoSpawn();
            BachTuoc.AutoSpawn();
            Piano.AutoSpawn();
        }

    }
}
