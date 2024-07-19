using NgocRongGold.Model.Info.Skill;
using NgocRongGold.Sources.Base.Info.Skill;

namespace NgocRongGold.Model.Info
{
    public class InfoSkill
    {
        public TaiTaoNangLuong TaiTaoNangLuong { get; set; }
        public Monkey Monkey { get; set; }
        public TuSat TuSat { get; set; }
        public Protect Protect { get; set; }
        public HuytSao HuytSao { get; set; }
        public MeTroi MeTroi { get; set; }
        public SuperKamejoko SuperKamejoko { get; set; }
        public CadicLienHoanChuong CadicLienHoanChuong{get;set;}
        public PlayerTroi PlayerTroi { get; set; }
        public DichChuyen DichChuyen { get; set; }
        public ThoiMien ThoiMien { get; set; }
        public ThaiDuongHanSan ThaiDuongHanSan { get; set; }
        public QCKK Qckk { get; set; }
        public Laze Laze { get; set; }
        public Egg Egg { get; set; }
        public Socola Socola { get; set; }
        public HoaDa HoaDa { get; set; }
        public HoaBang HoaBang { get; set; }
        public MaPhongBa MaPhongBa { get; set; }
        public SocolaMabu socolaMabu { get; set; }
        public TrungMabu TrungMabu { get; set; }
        public InfoSkill()
        {
            TrungMabu = new TrungMabu();
            socolaMabu = new SocolaMabu();
            MaPhongBa = new MaPhongBa();
            HoaBang = new HoaBang();
            TaiTaoNangLuong = new TaiTaoNangLuong();
            SuperKamejoko = new SuperKamejoko();
            Monkey = new Monkey();
            TuSat = new TuSat();
            Protect = new Protect();
            HuytSao = new HuytSao();
            MeTroi = new MeTroi();
            PlayerTroi = new PlayerTroi();
            DichChuyen = new DichChuyen();
            ThoiMien = new ThoiMien();
            ThaiDuongHanSan = new ThaiDuongHanSan();
            CadicLienHoanChuong = new CadicLienHoanChuong();
            Qckk = new QCKK();
            Laze = new Laze();
            Egg = new Egg();
            Socola = new Socola();
            HoaDa = new HoaDa();
        }
    }
}