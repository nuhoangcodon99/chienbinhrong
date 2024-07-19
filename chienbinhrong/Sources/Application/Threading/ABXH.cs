using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Manager;
using NgocRongGold.DatabaseManager;
using NgocRongGold.DatabaseManager.Player;
using NgocRongGold.Model;
using NgocRongGold.Model.BangXepHang;
using NgocRongGold.Model.Character;

namespace NgocRongGold.Application.Threading
{
    public class ABXH
    {
        public List<BangXepHang.TopPower> TopPowers { get; set; }
        public List<BangXepHang.TopNap> TopNaps{get;set;}
        public List<BangXepHang.TopTask> TopTasks {get;set;}
        public List<BangXepHang.TopWhis> TopWhises { get; set; }

        public List<BangXepHang.TopClanCDRD> TopClanCDRDs {get;set;}
        public List<BangXepHang.TopClanKhiGas> TopClanKhiGas{get;set;}
        public List<BangXepHang.TopEvent> TopEvent { get; set; }
        public List<BangXepHang.TopQuayThuong> TopQuayThuong { get; set; }
        public List<BangXepHang.TopSanBoss> TopSanBoss { get; set; }
        public List<BangXepHang.TopDisciple> TopDisciples { get; set; }
        public List<BangXepHang.TopClan> TopClans { get; set; }


        public Task RunTime { get; set; }


        public ABXH()
        {
            TopWhises = new List<BangXepHang.TopWhis>();
            TopDisciples = new List<BangXepHang.TopDisciple>();
            TopPowers = new List<BangXepHang.TopPower>();
            TopNaps = new List<BangXepHang.TopNap>();
            TopTasks = new List<BangXepHang.TopTask>();
            TopClanCDRDs = new List<BangXepHang.TopClanCDRD>();
            TopClanKhiGas = new List<BangXepHang.TopClanKhiGas>();
            TopEvent = new List<BangXepHang.TopEvent>();
            TopQuayThuong = new List<BangXepHang.TopQuayThuong>();
            TopSanBoss = new List<BangXepHang.TopSanBoss>();
            TopClans = new List<BangXepHang.TopClan>();
        }
        public void Start()
        {
            new Thread(new ThreadStart(Action)).Start();
        }
        public void Flush()
        {
            TopPowers.Clear();
            TopNaps.Clear();
            TopClanCDRDs.Clear();
            TopClanKhiGas.Clear();
            TopTasks.Clear();
            TopEvent.Clear();
            TopQuayThuong.Clear();
            TopSanBoss.Clear();
            TopDisciples.Clear();
            TopClans.Clear();
            TopWhises.Clear();
        }
        private long timeDelay = 300000 + ServerUtils.CurrentTimeMillis();
        private void Action()
        {
            while (Server.Gi().IsRunning)
            {
                var timeserver = ServerUtils.CurrentTimeMillis();
                if (timeDelay < timeserver)
                {
                    Flush();
                    UpdateBangXepHang();
                    Server.Gi().Logger.Print("UPDATE Top Bang Xep Hang", "yellow");
                    timeDelay = timeserver + 300000;
                }
                Thread.Sleep(1000);
                
            }
            Server.Gi().Logger.Print("Close Update Bang Xep Hang Success", "red");

        }
        public static void UpdateBangXepHang(){
            BXHDatabase.SelectTopKhiGas(100);
            BXHDatabase.SelectTopPower(100);
            BXHDatabase.SelectTopTask(100);
            BXHDatabase.SelectTopEvent(100);
            BXHDatabase.SelectTopQuayThuong(100);
            BXHDatabase.SelectTopSanBoss(100);
            BXHDatabase.SelectTopNapThe(100);
            BXHDatabase.SelectTopDisciple(100);
            BXHDatabase.SelectTopClan(10);
            BXHDatabase.SelectTopWhis(100);
        }
        public Message ListTopInfoKhiGas()
        {
            var timeNow = ServerUtils.CurrentTimeMillis();
            var message = new Message(-96);
            message.Writer.WriteByte(0);
            message.Writer.WriteUTF("Top 100");
            message.Writer.WriteByte(TopEvent.Count > 100 ? 100 : TopClanKhiGas.Count);
            TopClanKhiGas.ForEach(i =>
            {
                message.Writer.WriteInt(i.Top); // rank
                message.Writer.WriteInt(-1); // pl id
                message.Writer.WriteShort(i.Head); // head id
                message.Writer.WriteShort(-1); // head icon
                message.Writer.WriteShort(i.Body); // body
                message.Writer.WriteShort(i.Leg); // leg
                message.Writer.WriteUTF(i.ClanName);  // name   
                message.Writer.WriteUTF($"Lv: {i.Level} ({ServerUtils.GetTimeInPast(timeNow, i.LastTimeRecord)})"); // name
                message.Writer.WriteUTF($"Bang chủ {i.LeaderName}\n[{ServerUtils.GetTime3(i.Record)}]"); // info 3
            });
            return message;
        }
        public Message ListTopInfoEvent()
        {
            var message = new Message(-96);
            message.Writer.WriteByte(1);
            message.Writer.WriteUTF("Top 100 Hoa Quả Sơn");
            message.Writer.WriteByte(TopEvent.Count > 100 ? 100 : TopEvent.Count);
            TopEvent.ForEach(i =>
            {
                message.Writer.WriteInt(i.Top); // rank
                message.Writer.WriteInt(i.PlId); // pl id
                message.Writer.WriteShort(i.Head); // head id
                message.Writer.WriteShort(-1); // head icon
                message.Writer.WriteShort(i.Body); // body
                message.Writer.WriteShort(i.Leg); // leg
                message.Writer.WriteUTF(i.Name);  // name   
                message.Writer.WriteUTF(i.Point.ToString()); // name
                message.Writer.WriteUTF(i.Point.ToString()); // info 3
            });
            return message;
        }
        public Message ListTopWhises()
        {
            var timeServer = ServerUtils.CurrentTimeMillis();
            var message = new Message(-96);
            message.Writer.WriteByte(0);
            message.Writer.WriteUTF("Top 100 Thách Đấu Whis");
            message.Writer.WriteByte(TopWhises.Count > 100 ? 100 : TopWhises.Count);
            TopWhises.ForEach(i =>
            {
                message.Writer.WriteInt(i.Top); // rank
                message.Writer.WriteInt(i.Id); // pl id
                message.Writer.WriteShort(i.Head); // head id
                message.Writer.WriteShort(-1); // head icon
                message.Writer.WriteShort(i.Body); // body
                message.Writer.WriteShort(i.Leg); // leg
                message.Writer.WriteUTF(i.Name);  // name   
                message.Writer.WriteUTF($"LV:{i.Level} với {i.Score} giây"); // name
                message.Writer.WriteUTF($"{ServerUtils.GetTimeInPast(timeServer, i.TimeSetScore)}"); // info 3
            });
            return message;
        }
        public Message ListTopDisciple()
        {
            var message = new Message(-96);
            message.Writer.WriteByte(1);
            message.Writer.WriteUTF("Top 100 Thiên Kiêu");
            message.Writer.WriteByte(TopDisciples.Count > 100 ? 100 : TopDisciples.Count);
            TopDisciples.ForEach(i =>
            {
                message.Writer.WriteInt(i.I); // rank
                message.Writer.WriteInt(i.Id); // pl id
                message.Writer.WriteShort(i.Head); // head id
                message.Writer.WriteShort(-1); // head icon
                message.Writer.WriteShort(i.Body); // body
                message.Writer.WriteShort(i.Leg); // leg
                message.Writer.WriteUTF("Sư phụ: "+i.MasterName);  // name   
                message.Writer.WriteUTF(i.Power.ToString()); // name
                message.Writer.WriteUTF(i.Name); // info 3
            });
            return message;
        }
        public Message ListTopClanes()
        {
            var message = new Message(-96);
            message.Writer.WriteByte(0);
            message.Writer.WriteUTF("Top 100 Môn Phái");
            message.Writer.WriteByte(TopClans.Count > 100 ? 100 : TopClans.Count);
            TopClans.ForEach(i =>
            {
                var clanIcon = Cache.Gi().CLAN_IMAGES[i.ImageId];
                message.Writer.WriteInt(i.Top); // rank
                message.Writer.WriteInt(0); // pl id
                message.Writer.WriteShort(i.Head); // head id
                message.Writer.WriteShort(clanIcon.Data[0]); // head icon
                message.Writer.WriteShort(i.Body); // body
                message.Writer.WriteShort(i.Leg); // leg
                message.Writer.WriteUTF(i.ClanName);  // name   
                message.Writer.WriteUTF($"Level: {i.Level}, Capsule Bang: {i.Capsule}"); // name
                message.Writer.WriteUTF($"Bang chủ: {i.LeaderName}\nThành viên {i.CurrentMember}/{i.MaxMember}"); // info 3
            });
            return message;
        }
        public Message ListTopQuayThuong()
        {
            var message = new Message(-96);
            message.Writer.WriteByte(1);
            message.Writer.WriteUTF("Top 100 Quay Thưởng");
            message.Writer.WriteByte(TopQuayThuong.Count > 100 ? 100 : TopQuayThuong.Count);
            TopQuayThuong.ForEach(i =>
            {
                message.Writer.WriteInt(i.Top); // rank
                message.Writer.WriteInt(i.PlId); // pl id
                message.Writer.WriteShort(i.Head); // head id
                message.Writer.WriteShort(-1); // head icon
                message.Writer.WriteShort(i.Body); // body
                message.Writer.WriteShort(i.Leg); // leg
                message.Writer.WriteUTF(i.Name);  // name
                message.Writer.WriteUTF(ServerUtils.GetMoneys(i.Point)); // name
                message.Writer.WriteUTF(ServerUtils.GetMoneys(i.Point)); // info 3
            });
            return message;
        }
        public Message ListTopSanBoss()
        {
            var message = new Message(-96);
            message.Writer.WriteByte(1);
            message.Writer.WriteUTF("Top 100 Sát Thần");
            message.Writer.WriteByte(TopSanBoss.Count > 100 ? 100 : TopSanBoss.Count);
            TopSanBoss.ForEach(i =>
            {
                message.Writer.WriteInt(i.Top); // rank
                message.Writer.WriteInt(i.PlId); // pl id
                message.Writer.WriteShort(i.Head); // head id
                message.Writer.WriteShort(-1); // head icon
                message.Writer.WriteShort(i.Body); // body
                message.Writer.WriteShort(i.Leg); // leg
                message.Writer.WriteUTF(i.Name);  // name
                message.Writer.WriteUTF(ServerUtils.GetMoneys(i.Point)); // name
                message.Writer.WriteUTF(ServerUtils.GetMoneys(i.Point)); // info 3
            });
            return message;
        }
        public Message ListTopInfoPower(){
            var message = new Message(-96);
            message.Writer.WriteByte(0);
            message.Writer.WriteUTF("Phong Thần Bảng");
            message.Writer.WriteByte(TopPowers.Count > 100 ? 100 : TopPowers.Count);
            TopPowers.ForEach(i=>
            {
                message.Writer.WriteInt(i.I); // rank
                message.Writer.WriteInt(i.Id); // pl id
                message.Writer.WriteShort(i.Head); // head id
                message.Writer.WriteShort(-1); // head icon
                message.Writer.WriteShort(i.Body); // body
                message.Writer.WriteShort(i.Leg); // leg
                message.Writer.WriteUTF(i.Name);  // name
                message.Writer.WriteUTF(ServerUtils.GetMoneys(i.Power)); // name
                message.Writer.WriteUTF(ServerUtils.GetMoneys(i.Power) + "\nTiềm năng: " + ServerUtils.GetMoneys(i.TotalPotential)); // info 3
            });
            return message;
        }
        public Message ListTopInfoPower2()
        {
            var message = new Message(-96);
            message.Writer.WriteByte(1);
            message.Writer.WriteUTF("Phong Thần Bảng");
            message.Writer.WriteByte(TopPowers.Count > 100 ? 100 : TopPowers.Count);
            TopPowers.ForEach(i =>
            {
                message.Writer.WriteInt(i.I); // rank
                message.Writer.WriteInt(i.Id); // pl id
                message.Writer.WriteShort(i.Head); // head id
                message.Writer.WriteShort(-1); // head icon
                message.Writer.WriteShort(i.Body); // body
                message.Writer.WriteShort(i.Leg); // leg
                message.Writer.WriteUTF(i.Name);  // name
                message.Writer.WriteUTF(ServerUtils.GetMoneys(i.Power)); // name
                message.Writer.WriteUTF(ServerUtils.GetMoneys(i.Power) + "\nTiềm năng: " + ServerUtils.GetMoneys(i.TotalPotential)); // info 3
            });
            return message;
        }
        public Message ListTopInfoNapThe(){
            var message = new Message(-96);
            message.Writer.WriteByte(1);
            message.Writer.WriteUTF("Tỷ Phú Bảng");
            message.Writer.WriteByte(TopNaps.Count > 100 ? 100 : TopNaps.Count);
            TopNaps.ForEach(i=>
            {
                message.Writer.WriteInt(i.I);
                message.Writer.WriteInt(i.Id);
                message.Writer.WriteShort(i.Head);
                message.Writer.WriteShort(-1);
                message.Writer.WriteShort(i.Body);
                message.Writer.WriteShort(i.Leg);
                message.Writer.WriteUTF(i.Name);
                message.Writer.WriteUTF("Đã nạp: " + ServerUtils.GetMoneys(i.TongNap));
                message.Writer.WriteUTF("Đã nạp: " + ServerUtils.GetMoneys(i.TongNap));
            });
            return message;
        }
       public Message ListTopInfoTask(){
            var message = new Message(-96);
            message.Writer.WriteByte(0);
            message.Writer.WriteUTF("Thiên Bảng");
            message.Writer.WriteByte(TopTasks.Count > 100 ? 100 : TopTasks.Count);
            TopTasks.ForEach(i=>
            {
                message.Writer.WriteInt(i.I);
                message.Writer.WriteInt(i.Id);
                message.Writer.WriteShort(i.Head);
                message.Writer.WriteShort(-1);
                message.Writer.WriteShort(i.Body);
                message.Writer.WriteShort(i.Leg);
                message.Writer.WriteUTF(i.Name);
                message.Writer.WriteUTF("Nhiệm vụ thứ " +i.TaskId);
                message.Writer.WriteUTF("Nhiệm vụ thứ " +i.TaskId + "\nGiai đoạn " + i.Index + " [" + i.Count + "]");
            });
            return message;
        }
    }
}