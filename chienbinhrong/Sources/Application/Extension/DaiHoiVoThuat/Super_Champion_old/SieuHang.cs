using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Threading;
using NgocRongGold.DatabaseManager;
using NgocRongGold.Model.Character;

namespace NgocRongGold.Application.Extension.Super_Champion
{
    public class SieuHang
    {
        public class InfoRank
        {
            public int Top { get; set; }
            public int PlayerId { get; set; }
            public string PlayerName { get; set; }
            public short Head { get; set; }
            public short Body { get; set; }
            public short Leg { get; set; }
            public int Gem { get; set; }
            public History History { get; set; }
            public Point Point { get; set; }

        }
        public class History
        {
            public string[][] Text { get; set; }
            public History()
            {
                Text = new string[0][];
            }
        }

        public class Point
        {
            public long HP { get; set; }
            public long SD { get; set; }
            public long Amor { get; set; }
            public int Win { get; set; }
            public int Lose { get; set; }
            public Point()
            {
                HP = -1;
                SD = -1;
                Amor = -1;
                Win = 0;
                Lose = 0;
            }
            public Point(long hp, long sd, long amor, int win, int lose)
            {
                HP = hp;
                SD = sd;
                Amor = amor;
                Win = win;
                Lose = lose;
            }
        }
        public class InfoPlayer
        {
            public int Ticket { get; set; }
            public int Top { get; set; }
            public History History { get; set; }
            public int Gem { get; set; }
            public Point point { get; set; }
            public InfoPlayer()
            {
                Ticket = 3;
                //   Top = getTop();
                History = new History();
                Gem = 100;
                point = new Point();

            }
            //   public int getTop()
            //   {
            //     try
            //      {
            // return Cache.Gi().InfoRankSieuHang.Max().Key + 1;
            //      }
            //      catch
            //      {
            //         return 0;
            //      }
            //  }
            // }
            public class MySql
            {

            }
            public static void ThachDau(Character character, int playerThachDauId)
            {
                //var player = Cache.Gi().InfoRankSieuHang.FirstOrDefault(i=>i.Value.PlayerId == playerThachDauId);
            }
            public static List<InfoRank> ListInfoRank(Character character)
            {
                List<InfoRank> Infos = new List<InfoRank>();

                return Infos;
            }
            //public static List<InfoRank> SelectRanks(int from, int to)
            // {
            //var rankCollections = CollectionsMarshal.AsSpan(Cache.Gi().InfoRankSieuHang.Values.Where(rank => rank.Top >= from && rank.Top <= to).ToList());
            //var ranks = new List<InfoRank>();
            //for (int i = 0; i < rankCollections.Length; i++)
            //{
            //    ranks.Add(rankCollections[i]);
            //}
            //return ranks;
            // }
            //public static Message ListRankCaoThu(Character character)
            //{
            //    var message = new Message(-96);
            //    message.Writer.WriteByte(0);
            //    message.Writer.WriteUTF("Top 100 Cao Thủ");
            //    message.Writer.WriteByte(Cache.Gi().InfoRankSieuHang.Count);
            //    var collections = CollectionsMarshal.AsSpan(SelectRanks(1, 100));
            //    for (int i = 0; i < collections.Length; i++)
            //    {
            //        var time = ServerUtils.CurrentTimeMillis();
            //        var rank = Cache.Gi().InfoRankSieuHang[i];
            //        var info = "";

            //        message.Writer.WriteInt(i + 1); // rank
            //        message.Writer.WriteInt(rank.PlayerId); // pl id
            //        message.Writer.WriteShort(rank.Head); // head id
            //        message.Writer.WriteShort(-1); // head icon
            //        message.Writer.WriteShort(rank.Body); // body
            //        message.Writer.WriteShort(rank.Leg); // leg
            //        message.Writer.WriteUTF(rank.PlayerName);  // name
            //        message.Writer.WriteUTF(""); // phía bên ngoài | khi bật menu lên là chữ màu xanh lá
            //        for (int infoRank = 0; infoRank < rank.History.Text.Length; infoRank++)
            //        {
            //            info += rank.History.Text[infoRank][0] + $"{(time - long.Parse(rank.History.Text[infoRank][1]) > 1 ? "" : "" + ServerUtils.GetTime(time - long.Parse(rank.History.Text[infoRank][1])))}" + (rank.History.Text.Length == 1 ? "" : "\n");
            //        }
            //        for (int history = 0; history < character.DataSieuHang.History.Text.Length; history++)
            //        {
            //            info += character.DataSieuHang.History.Text[history][0] + (time - long.Parse(character.DataSieuHang.History.Text[history][1]) > 1 ? "" : $" {ServerUtils.GetTime(time - long.Parse(character.DataSieuHang.History.Text[history][1]))}") + (character.DataSieuHang.History.Text.Length == 1 ? "" : "\n");
            //        }
            //        message.Writer.WriteUTF((rank.PlayerId == character.Id ?
            //        $"HP {character.DataSieuHang.point.HP}\nSức đánh {character.DataSieuHang.point.SD}\nGiáp {character.DataSieuHang.point.Amor}\n{character.DataSieuHang.point.Win}:{character.DataSieuHang.point.Lose}\n"
            //        : $"HP {rank.Point.HP}\nSức đánh {rank.Point.SD}\nGiáp {rank.Point.Amor}\n{rank.Point.Win}:{rank.Point.Lose}")
            //        + $"{info}"); // khi bật menu lên là chữ màu xanh dương
            //    }
            //    //character.CharacterHandler.SendMessage(message);
            //    return message;
            //}
        }
    }
}