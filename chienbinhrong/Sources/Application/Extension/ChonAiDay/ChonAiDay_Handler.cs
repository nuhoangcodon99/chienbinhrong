using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.Model.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.ChonAiDay
{
    public class ChonAiDay_Handler
    {
        public static ChonAiDay_Info chonAiDay_Info = new ChonAiDay_Info();
        public static readonly string TheLe = "Mỗi lượt chơi có 6 giải thưởng\nĐựợc chọn thoải mái\nThời gian 1 lượt chọn tối đa là 5 phút\nKhi hết giờ, hệ thống sẽ chọn ngẫu nhiên 1 người may mắn của từng\ngiải và trao thưởng.";
        public static ChonAiDay_Handler instance;
        public static ChonAiDay_Handler gI()
        {
            if (instance == null) instance = new ChonAiDay_Handler();
            return instance;
        }
        public void Clear()
        {
            chonAiDay_Info.TongGiaiThuongNgocDo[0] = chonAiDay_Info.TongGiaiThuongNgocDo[1] = 0;
            chonAiDay_Info.TongGiaiThuongNgocXanh[0] = chonAiDay_Info.TongGiaiThuongNgocXanh[1] = 0;
            chonAiDay_Info.TongGiaiThuongVang[0] = chonAiDay_Info.TongGiaiThuongVang[1] = 0;
            chonAiDay_Info.TongThamGiaVang[0].Clear();
            chonAiDay_Info.TongThamGiaVang[1].Clear();
            chonAiDay_Info.TongThamGiaNgocDo[0].Clear();
            chonAiDay_Info.TongThamGiaNgocDo[1].Clear();
            chonAiDay_Info.TongThamGiaNgocXanh[0].Clear();
            chonAiDay_Info.TongThamGiaNgocXanh[1].Clear();
        }
        public void DatCuoc(ICharacter character, int type)
        {
            if (!character.Player.IsActive)
            {
                character.CharacterHandler.SendMessage(Service.ServerMessage("Vui lòng mở thành viên để kích hoạt chức năng này"));
                return;
            }
            if (chonAiDay_Info.Status != ChonAiDay_Status.PICK) return;
           if (!chonAiDay_Info.TongThamGiaVang[type].Contains(character.Id))
            {
                chonAiDay_Info.TongThamGiaVang[type].Add(character.Id);
            }
            
            var datcuoc = 0;
            switch (type)
            {
                case 0:
                    datcuoc = 10;
                    break;
                case 1:
                    datcuoc = 100;
                    break;
            }
            if (character.CharacterHandler.GetItemBagById(457) == null || character.CharacterHandler.GetItemBagById(457).Quantity < datcuoc)
            {
                character.CharacterHandler.SendMessage(Service.DialogMessage("Không đủ thỏi vàng !"));
                return;
            }
            character.CharacterHandler.RemoveItemBagById(457, datcuoc);
            character.CharacterHandler.SendMessage(Service.SendBag(character));
            character.CharacterHandler.SendMessage(Service.ServerMessage("Đặt cược thành côngg!"));
            chonAiDay_Info.TongGiaiThuongVang[type] += datcuoc;
            var time = (ChonAiDay_Handler.chonAiDay_Info.TimeEnd - ServerUtils.CurrentTimeMillis()) / 1000;
            var text = $"Tổng giải thường: {ChonAiDay_Handler.chonAiDay_Info.TongGiaiThuongVang[0]}, cơ hội trúng của bạn là: {ChonAiDay_Handler.gI().Getpercent(character, 0)}" +
                $"\nTổng giải VIP: {ChonAiDay_Handler.chonAiDay_Info.TongGiaiThuongVang[1]}, cơ hội trúng của bạn là: {ChonAiDay_Handler.gI().Getpercent(character, 1)}" +
                $"\nThời gian còn lại: {time} giây.";
            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(54, text, new List<string> { "Cập nhật", "Thường\n1 thỏi vàng", "VIP\n10 thỏi vàng", "Đóng" }, character.InfoChar.Gender));
            ((Character)character).TypeMenu = 3;
        }
        public void Reward(ICharacter character, long value)
        {
            value -= value / 100 * 10;
            character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã may mắn nhận được " + value + " thỏi vàng với Chọn Ai Đây"));
            ClientManager.Gi().SendMessage(Service.ServerChat("Chúc mừng " + character.Name + " đã nhận được " + value + " thỏi vàng với Chọn Ai Đây"));
            character.CharacterHandler.AddItemToBag(true, ItemCache.GetItemDefault(457, (int)value));
        }
        public Task Runtime { get; set; }
        public void Start()
        {
            new Thread(new ThreadStart(AutoResult)).Start();
        }
        public int Getpercent(ICharacter character, int type)
        {
            if (!chonAiDay_Info.TongThamGiaVang[type].Contains(character.Id))
            {
                return 0;
            }
            return 100 / chonAiDay_Info.TongThamGiaVang[type].Count ;
        }
        public void AutoResult()
        {
            while (Server.Gi().IsRunning)
            {
                var timeserver = ServerUtils.CurrentTimeMillis();
                switch (chonAiDay_Info.Status)
                {
                    case ChonAiDay_Status.PICK:
                        if (chonAiDay_Info.TimeEnd < timeserver)
                        {
                            chonAiDay_Info.Status = ChonAiDay_Status.RESULT;
                            Server.Gi().Logger.Print("SETUP CHON_AI_DAY | STATUS: RESULT", "red");
                        }
                        break;
                    case ChonAiDay_Status.RESULT:
                        for (int i = 0; i <= 1; i++)
                        {
                            if (chonAiDay_Info.TongThamGiaVang[i].Count < 1) continue;
                            var selectPlayerWin = ServerUtils.RandomNumberByListLong(chonAiDay_Info.TongThamGiaVang[i]);
                            var playerWin = ClientManager.Gi().GetCharacter((int)selectPlayerWin);
                            var valueGet = chonAiDay_Info.TongGiaiThuongVang[i];
                            if (playerWin != null)
                            {
                                Reward(playerWin, valueGet);
                                chonAiDay_Info.Name += $"{playerWin.Name} + {valueGet} thỏi vàng\n";
                            }
                        }
                        Server.Gi().Logger.Print("SETUP CHON_AI_DAY | STATUS: TRAO THUONG SUCCESS", "red");

                        //var selectPlayerWin = ServerUtils.RandomNumberByListLong(chonAiDay_Info.TongThamGiaVang[0]);
                        //var playerWin = ClientManager.Gi().GetCharacter((int)selectPlayerWin);
                        //var valueGet = chonAiDay_Info.TongGiaiThuongVang[0];
                        //if (playerWin != null)
                        //{
                        //    Reward(playerWin, valueGet);
                        //}
                        //selectPlayerWin = ServerUtils.RandomNumberByListLong(chonAiDay_Info.TongThamGiaVang[1]);
                        //playerWin = ClientManager.Gi().GetCharacter((int)selectPlayerWin);
                        //valueGet = chonAiDay_Info.TongGiaiThuongVang[1];
                        //if (playerWin != null)
                        //{
                        //    Reward(playerWin, valueGet);
                        //}
                        chonAiDay_Info.TimeEnd = 60000 + timeserver;
                        chonAiDay_Info.Status = ChonAiDay_Status.DELAY;
                        break;
                    case ChonAiDay_Status.DELAY:
                        if (chonAiDay_Info.TimeEnd < timeserver)
                        {
                            Clear();
                            chonAiDay_Info.TimeEnd = 300000 + timeserver;
                            chonAiDay_Info.Status = ChonAiDay_Status.PICK;
                            Server.Gi().Logger.Print("SETUP CHON_AI_DAY | STATUS: CLEAR", "red");
                        }
                        break;
                }
                Thread.Sleep(1000);
            }
            Server.Gi().Logger.Print("Close Chon Ai Day Success", "red");
        }
    }
}
