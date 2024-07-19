using System.Collections.Concurrent;
using System.Collections.Generic;
using NgocRongGold.Application.Handlers.Character;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.IO;
using NgocRongGold.Model.Info;
using NgocRongGold.Model.Clan;
using NgocRongGold.Application.Threading;
using NgocRongGold.Application.Extension.BlackballWar;
using System;
using System.Linq;
using NgocRongGold.Application.Main;
using System.Drawing;

namespace NgocRongGold.Model.Clan
{
    public class Clan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Khẩu_hiệu { get; set; }
        public int ImgId { get; set; }
        public long Điểm_thành_tích { get; set; }
        public string LeaderName { get; set; }
        public int Điểm_Danh_Vọng { get; set; }
        public ClanLeader Leader {get;set;}
        public int Thành_viên_hiện_tại { get; set; }
        public int Tối_đa_thành_viên { get; set; }
        public int Cấp_Độ { get; set; }
        public int Capsule_Bang { get; set; }
        public int Thời_gian_tạo_bang { get; set; }
        public DateTime DateClanCreate { get; set; }
        public DateTime DateClanReset { get; set; }

        public List<ClanMember> Thành_viên { get; set; }
        public List<ClanMessage> Messages { get; set; }
        public IClanHandler ClanHandler { get; set; }
        public List<Item.Item> ClanBox { get; set; }
        public List<CharacterPea> CharacterPeas { get; set; }
        public long DelayUpdate { get; set; }
        public bool IsSave { get; set; }
        public int ClanFlag { get; set; }
        public ClanDungeon ClanDungeon { get; set; }
        //public Treasure bdkb;
        //public ConDuongRanDoc CDRD;
        //public Reddot Reddot { get; set; }
        //public Gas Gas;
        public BlackBallHandler.ForClan.ClanManagerr DataBlackBall{get;set;}
        public ClanZone ClanZone { get; set; }
        public ClanBoss.ClanBoss ClanBoss { get; set; }
        public string shortName { get; set; }
        public Clan()
        {
            ClanFlag = ServerUtils.RandomNumber(1, 8);
            shortName = "CBNR";
            ClanBoss = new ClanBoss.ClanBoss();
            ClanZone = new ClanZone();
            ClanDungeon = new ClanDungeon();
            DataBlackBall = new BlackBallHandler.ForClan.ClanManagerr();
            Khẩu_hiệu = "";
            Cấp_Độ = 1;
            Capsule_Bang = 0;
            Thành_viên = new List<ClanMember>();
            Messages = new List<ClanMessage>();
            CharacterPeas = new List<CharacterPea>();
            ClanHandler = new ClanHandler(this);
            DelayUpdate = 300000 + ServerUtils.CurrentTimeMillis();
            IsSave = true;
            //Reddot = new Reddot();
            //Gas = new Gas();
            //bdkb = new Treasure();
            ClanBox = new List<Item.Item>();
            //bdkb = new Treasure();
            //CDRD = new ConDuongRanDoc();
            //doanhtrai = new DoanhTrai();
            //khigas = new Gas();
            //bdkb.MapBDKB = new List<Application.Threading.Map>(1);

            //   cdrd.MapCDRD = new List<Application.Threading.Map>(1);
            //   doanhtrai.ReddotMaps = new List<Application.Threading.Map>(1);
            //   khigas.MapKhiGas = new List<Application.Threading.Map>(1);

        }
        public bool CondititonToJoinDungeon(Character.Character character, Clan clan, short npcId = 0)
        {
            if (!character.Player.IsActive)
            {
                character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Yêu cầu ngươi phải mở thành viên"));
                return false;
            }
            var dayClanCreate = (ServerUtils.TimeNow().Date - clan.DateClanCreate).TotalDays;
            var dayMemberJoinClan = (ServerUtils.TimeNow().Date - clan.DateClanCreate).TotalDays;
            if (dayClanCreate < 2)
            {
                character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Yêu cầu bang hội thành lập trên 2 ngày"));
                Server.Gi().Logger.Debug("Clan is create soon by 1 days, [" + dayClanCreate + "]");
                return false;
            }
            else if (dayMemberJoinClan < 2)
            {
                character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Yêu cầu tham gia bang hội trên 2 ngày"));
                Server.Gi().Logger.Debug("Member is join soon by 1 days, [" + dayMemberJoinClan + "]");
                return false;
            }
            return true;
        }
        public void Reset()
        {
            ClanDungeon.BanDoKhoBau.Reset(true);
            ClanDungeon.DoanhTraiDocNhan.Reset(true);
            ClanDungeon.KhiGasHuyDiet.Reset(true);
            ClanDungeon.ConDuongRanDoc.Reset(true);
            DataBlackBall.ListCurrentBlackball.Clear();
            ClanBoss.Reset();
        }
    }
}