namespace NgocRongGold.Model.Info
{
    public class InfoOption
    {
        public int PhanPercentSatThuong { get; set; }
        public int PhanTramXuyenGiapChuong { get; set; }
        public int PhanTramXuyenGiapCanChien { get; set; }
        public int PhanTramNeDon { get; set; }
        public int PhanTramVangTuQuai { get; set; }
        public int PhanTramTNSM { get; set; }
       
        public int PercentChinhXac { get; set; }
        public int PhanTramSatThuongChiMang { get; set; }
        public int VoHieuHoaChuong { get; set; }
        public bool X2TiemNang { get; set; }
        public int PhanTramTangSatThuongDam { get; set; }
        public int PhanTramHp { get; set; }
        public int PhanTramKi { get; set; }
        public long Hp { get; set; }
        public long Ki { get; set; }
        public int PhanTramGiamHp { get; set; }
        public long ThoundsandHP { get; set; }
        public long ThoundsandMP { get; set; }
      
        public int HundredHPMP { get; set; }
        public long HpMp { get; set; }

        public int Defence { get; set; }
        public int PhanTramDefence { get; set; }
        public int Crit { get; set; }
        public int PhanTramSpeed { get; set; }
        public long Damage { get; set; }

        public int PhanTramDamage { get; set; }
        public int PhanTramDamage2 { get; set; }
        public int PlusHp30Second { get; set; }
        public int PlusMp30Second { get; set; }
        public int PlusMpEverySecond { get; set; }
        public int PhanTramPlusHp30Second { get; set; }
        public int PhanTramPlusMp30Second { get; set; }
        public int PhanTramPlusMp10Second { get; set; }
        public int PercentPlusPotenialForDisciple { get; set; }
        public int PercentPlusDameKamejoko { get; set; }
        public bool ChongLanh { get; set; }
        public int PlusDameToMonster { get; set; }
        public bool CongDonDam { get; set; }
      

        public int PhanTramDamageCard { get; set; }
        public int PhanTramHpCard { get; set; }
        public int PhanTramMpCard { get; set; }
        public InfoOption()
        {
            PhanTramDamageCard = 0;
            PhanTramHpCard = 0;
            PhanTramMpCard = 0;
            CongDonDam = false;
            PlusDameToMonster = 0;
            PercentPlusDameKamejoko = 0;
            PercentPlusPotenialForDisciple = 0;
            PhanTramPlusMp10Second = 0;
            PhanTramPlusMp30Second = 0;
            PhanTramPlusHp30Second = 0;
            PlusHp30Second = 0;
            PlusMp30Second = 0;
            PlusMpEverySecond = 0;
            Defence = 0;
            PhanTramDefence = 0;
            Crit = 0;
            PhanTramSpeed = 0;
            Damage = 0;
            PhanTramHp = 0;
            PhanTramKi = 0;
            Hp = 0;
            Ki = 0;
            PhanTramGiamHp = 0;
            ThoundsandHP = 0;
            ThoundsandMP = 0;
            HpMp = 0;
            PhanTramDamage = 0;
            PhanTramDamage2 = 0;
            HundredHPMP = 0;
            PhanPercentSatThuong = 0;
            PhanTramXuyenGiapChuong = 0;
            PhanTramXuyenGiapCanChien = 0;
            PhanTramNeDon = 0;
            PhanTramVangTuQuai = 0;
            PhanTramTNSM = 0;
           
            PercentChinhXac = 0;
            PhanTramSatThuongChiMang = 0;
            VoHieuHoaChuong = 0;
            X2TiemNang = false;
            PhanTramTangSatThuongDam = 0;
            ChongLanh = false;

        }
        public void ResetOptionCard()
        {
            PhanTramHpCard = PhanTramMpCard = PhanTramDamageCard = 0;
        }
        public void ResetOptionRole()
        {
        }
        public void Reset()
        {
            CongDonDam = false;
            PlusDameToMonster = 0;
            ChongLanh = false;
            PercentPlusPotenialForDisciple = 0;
            PercentPlusDameKamejoko = 0;
            PhanTramPlusMp10Second = 0;
            PhanTramPlusMp30Second = 0;
            PhanTramPlusHp30Second = 0;
            PlusHp30Second = 0;
            PlusMp30Second = 0;
            PlusMpEverySecond = 0;
            PhanTramDamage2 = 0;
            PhanTramHp = 0;
            PhanTramKi = 0;
            Hp = 0;
            Ki = 0;
            PhanTramGiamHp = 0;
            ThoundsandHP = 0;
            ThoundsandMP = 0;
            HpMp = 0;
            PhanTramDamage = 0;
            HundredHPMP = 0;
            PhanPercentSatThuong = 0;
            PhanTramXuyenGiapChuong = 0;
            PhanTramXuyenGiapCanChien = 0;
            PhanTramNeDon = 0;
            PhanTramVangTuQuai = 0;
            PhanTramTNSM = 0;
            Defence = 0;
            PhanTramDefence = 0;
            Crit = 0;
            PhanTramSpeed = 0;
            Damage = 0;
            PercentChinhXac = 0;
            PhanTramSatThuongChiMang = 0;
            VoHieuHoaChuong = 0;
            X2TiemNang = false;
            PhanTramTangSatThuongDam = 0;
        }
    }
}