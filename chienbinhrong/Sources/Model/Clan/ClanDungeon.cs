using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Model.Clan
{
    public class ClanDungeon
    {
        public ConDuongRanDoc ConDuongRanDoc { get; set; }
        public DoanhTraiDocNhan DoanhTraiDocNhan{  get; set; }
        public BanDoKhoBau BanDoKhoBau { get;set; }
        public KhiGasHuyDiet KhiGasHuyDiet { get; set; }
        public ClanDungeon() 
        {
            ConDuongRanDoc = new ConDuongRanDoc();
            DoanhTraiDocNhan = new DoanhTraiDocNhan();
            BanDoKhoBau = new BanDoKhoBau();
            KhiGasHuyDiet = new KhiGasHuyDiet();  
        }
    }
}
