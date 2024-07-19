using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NgocRongGold.Model.BangXepHang.BangXepHang;

namespace NgocRongGold.Application.Extension.Event
{
    public class DuaTopSuKien
    {
        public class DuaTopCharacterInfo
        {
            public ICharacter Character { get; set; }
            public long Time { get; set; }
            public int Point { get; set; }
            public int Top { get; set; }

        }
        public static DateTime TimeEnd = new DateTime(2024, 4, 25, 0, 0, 00, DateTimeKind.Utc);
        public static List<string> InfoSuKien = new List<string>()
        {
            "Top 100 dâng bánh dầy",
            "Top 100 mở hộp quà cao cấp",
        };
        public static List<string> MenuSuKien = new List<string>()
        {
            "Top dâng\nbánh dầy",
            "Top mở\nhộp quà\ncao cấp",
            "Xem điểm",
            "Đóng",
        };

        public static List<DuaTopCharacterInfo> InfosTop100DangBanhGiay = new List<DuaTopCharacterInfo>();
        public static List<DuaTopCharacterInfo> InfosTop100MoHopQuaCaoCap = new List<DuaTopCharacterInfo>();
        public static void Save()
        {
            File.WriteAllText("Top\\InfosTop100DangBanhGiay.txt", JsonConvert.SerializeObject(InfosTop100DangBanhGiay));
            File.WriteAllText("Top\\InfosTop100MoHopQuaCaoCap.txt", JsonConvert.SerializeObject(InfosTop100MoHopQuaCaoCap));

        }
        public static void Load()
        {
            InfosTop100DangBanhGiay = JsonConvert.DeserializeObject<List<DuaTopCharacterInfo>>(File.ReadAllText("Top\\InfosTop100DangBanhGiay.txt"));
            InfosTop100MoHopQuaCaoCap = JsonConvert.DeserializeObject<List<DuaTopCharacterInfo>>(File.ReadAllText("Top\\InfosTop100DInfosTop100MoHopQuaCaoCap.txt"));

        }
        public static void OpenMenuSuKien(short npcId, Model.Character.Character character)
        {
            var text = $"Sự kiện đua {InfoSuKien.Count} Top nhận quà khủng";
            foreach (var info in InfoSuKien)
            {
                text += $"\n{info}";

            }
            text += $"\nKết thúc và trao giải lúc 0:0:0 25/4/2024";
            text += $"\nĐến gặp ta để nhận giải nhé\nChi tiết xem tại diễn đàn, Fanpage, Box Zalo";
            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, text, MenuSuKien, character.InfoChar.Gender));
        }
        public Task Runtime { get; set; }
        public void TraoGiai()
        {

        }
        public static void UpdatePoint(ICharacter character, int point, List<DuaTopCharacterInfo> ts)
        {
            if (ts.FirstOrDefault(i => i.Character.Id == character.Id) != null)
            {
                var temp = ts.FirstOrDefault(i => i.Character.Id == character.Id);
                temp.Point += point;
                List<DuaTopCharacterInfo> increasedInfos = SelectInfosWithIncreasedPosition(ts);
                ts = increasedInfos;
                foreach (var info in increasedInfos)
                {
                    info.Time = ServerUtils.CurrentTimeMillis();
                    Console.WriteLine($"Character: {info.Character}, Time: {info.Time - ServerUtils.CurrentTimeMillis()}, Point: {info.Point}, Top: {info.Top}");
                }
            }
            else
            {
                ts.Add(new DuaTopCharacterInfo()
                {
                    Character = character,
                    Time = ServerUtils.CurrentTimeMillis(),
                    Point = point,
                    Top = ts.Count + 1,
                });
            }

        }
        public static List<DuaTopCharacterInfo> SelectInfosWithIncreasedPosition(List<DuaTopCharacterInfo> infos)
        {
            // Sắp xếp danh sách giảm dần theo thuộc tính Point.
            var sortedInfos = infos.OrderByDescending(i => i.Point).ToList();

            List<DuaTopCharacterInfo> increasedInfos = new List<DuaTopCharacterInfo>();

            // Tìm các phần tử có vị trí tăng sau khi sắp xếp.
            for (int i = 0; i < infos.Count; i++)
            {
                var info = infos[i];
                var indexOriginal = infos.IndexOf(info);
                var indexSorted = sortedInfos.IndexOf(info);
                if (indexSorted > indexOriginal)
                {
                    increasedInfos.Add(info);
                }
            }

            return increasedInfos;
        }

       

    public void UpdateTop(List<DuaTopCharacterInfo> ts)
        {
            ts.OrderByDescending(i => i.Point);
           

        }
        public static Message ShowTopType1(List<DuaTopCharacterInfo> ts, Message message, string Name)
        {
            message.Writer.WriteByte(1);
            message.Writer.WriteUTF($"{Name}");
            message.Writer.WriteByte(ts.Count);
            ts.ForEach(i =>
            {
                message.Writer.WriteInt(i.Top); // rank
                message.Writer.WriteInt(i.Character.Id); // pl id
                message.Writer.WriteShort(i.Character.GetHead()); // head id
                message.Writer.WriteShort(-1); // head icon
                message.Writer.WriteShort(i.Character.GetBody()); // body
                message.Writer.WriteShort(i.Character.GetLeg()); // leg
                message.Writer.WriteUTF(i.Character.Name);  // name   
                message.Writer.WriteUTF(i.Point.ToString()); // name
                message.Writer.WriteUTF(i.Point.ToString()); // info 3
            });
            return message;
        }
    }
}
