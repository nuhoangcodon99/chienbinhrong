using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Handlers.Item;
using NgocRongGold.Application.Handlers.Monster;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.DatabaseManager;
using NgocRongGold.DatabaseManager.Player;
using NgocRongGold.Application.Menu;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Clan;
using NgocRongGold.Model.Info;
using NgocRongGold.Model.Option;
using NgocRongGold.Model.SkillCharacter;
using NgocRongGold.Model.Template;
using NgocRongGold.Application.MainTasks;
using NgocRongGold.Application.Extension;
using NgocRongGold.Application.Extension.Chẵn_Lẻ_Momo;
using NgocRongGold.Application.Extension.Bosses;
using NgocRongGold.Application.Extension.BlackballWar;
using NgocRongGold.Application.Extension.Dragon;
using NgocRongGold.Application.Extension.Event;
using NgocRongGold.Application.Extension.Ký_gửi;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Extension.Bosses.Mabu2Gio;
using NgocRongGold.Application.Extension.Bosses.Mabu12Gio;
using NgocRongGold.Application.Extension.Namecball;
using NgocRongGold.Application.Extension.Super_Champion;
using NgocRongGold.Application.Extension.Bo_Mong;
using NgocRongGold.Application.Extension.ConSoMayMan;
using NgocRongGold.Application.Extension.ChonAiDay;
using NgocRongGold.Application.Extension.DaiHoiVoThuat;
using NgocRongGold.Application.Extension.DiedRing;
using NgocRongGold.Application.Extension.KeoBuaBao;
using Serilog.Settings.Configuration;
using NgocRongGold.Application.Extension.NamecBattlefield;
using System.ComponentModel;
using Org.BouncyCastle.Math.Field;
using NgocRongGold.Application.Extension.Practice.Whis;
using NgocRongGold.Application.Extension.Practice;
using NgocRongGold.Application.Extension.SideQuest.BoMong;
using NgocRongGold.Model.Item;
using Linq.Extras;
using Org.BouncyCastle.Bcpg.OpenPgp;
using NgocRongGold.Application.Extension.SideQuest.HangNgay;
using Newtonsoft.Json;
using System.Drawing;
using System.Net.Security;
using NgocRongGold.Application.Interfaces.Character;
using Application.Interfaces.Map;
using NgocRongGold.Application.Extension.DaiHoiVoThuat.SuperChampion;
using Application.Map;
using Chiến_Binh_Rồng.Sources.Application.Manager;
using Application.Dungeon.RedRibbon;
using Application.Constants;
using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;

namespace NgocRongGold.Application.Menu
{
    public static class Menu
    {
        public static void OpenUiMenu(short npcId, Character character)
        {
           
            Server.Gi().Logger.Debug($"Menu NpcId Case 33: ------------------------------------ {npcId}");
            try
            {
                switch (npcId)
                {
                    case 84:
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextObito[0], MenuNpc.Gi().MenuObito[0], character.InfoChar.Gender));
                        character.TypeMenu = 0;

                        break;
                    case 83:
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextBBGolden[0], MenuNpc.Gi().MenuBBGolden[0], character.InfoChar.Gender));
                        character.TypeMenu = 0;

                        break;
                    case 81:
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, string.Format(MenuNpc.Gi().TextCayThongNoel[0], ServerUtils.Color("blue") + "Server đang thưởng X2 Exp trong 0 phút"), MenuNpc.Gi().MenuCayThongNoel[0], character.InfoChar.Gender));
                        break;
                    case 82:
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, string.Format(MenuNpc.Gi().TextCayThongNoel[0], GameCache.gI()._Decorative.cDecorative, ServerUtils.Color("red") + "Server đang thưởng X" + GameCache.gI()._Exp.currentUpExp + " Exp trong " + (GameCache.gI()._Exp.timeUpExp - ServerUtils.CurrentTimeMillis() > 0 ? ServerUtils.GetTime(GameCache.gI()._Exp.timeUpExp - ServerUtils.CurrentTimeMillis()) : 0) + " phút"), MenuNpc.Gi().MenuCayThongNoel[0], character.InfoChar.Gender));
                        character.TypeMenu = 0;
                        break;
                    case 78:
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Ngươi muốn gì?", new List<string> { "Quay về\nHành tinh\nngục tù" }, character.InfoChar.Gender));
                        character.TypeMenu = 0;
                        break;
                    case 28:
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Cửa hàng chúng tôi chuyên mua bán hàng hiệu, hàng độc, cảm ơn bạn đã\nghé thăm", new List<string> { "Hướng\ndẫn\nthêm", "Mua bán\nKý gửi", "Từ chối" }, character.InfoChar.Gender));
                        character.TypeMenu = 0;
                        break;
                    case 52:
                        switch (character.InfoChar.MapId)
                        {
                            case 0 or 7 or 14:
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextHungVuong[0], MenuNpc.Gi().MenuHungVuong[0], character.InfoChar.Gender));
                                character.TypeMenu = 3;

                                break;
                            default:
                                List<string> Menus = new List<string>();
                                for (int i = 0; i < SuKienHungVuong.DataTradeDuaHau[0].Count; i++)
                                {
                                    Menus.Add($"{SuKienHungVuong.DataTradeDuaHau[1][i]} ngọc\n{SuKienHungVuong.DataTradeDuaHau[0][i]} quả");
                                }
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Muốn đổi hồng ngọc thì mang dưa hấu tới đây.", Menus, character.InfoChar.Gender));
                                character.TypeMenu = 0;
                                break;
                        }
                        break;
                    case 51:
                        if (character.InfoChar.ThoiGianDuaHau != 0)
                        {
                            var second = (character.InfoChar.ThoiGianDuaHau - ServerUtils.CurrentTimeMillis()) / 1000;
                            if (second < 0)
                            {
                                second = 0;
                            }

                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Khi nào chín hãy thu hoạch và hãy mang [Dưa Hấu] đến gặp Vua Hùng\nđể đổi quà nhé", new List<string> { second == 0 ? "Thu hoạch" : "OK", "Từ chối" }, character.InfoChar.Gender));
                            character.TypeMenu = second == 0 ? 1 : 0;

                        }
                        break;
                    case 60:
                        if (character.InfoChar.MapId == 80)
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Ta mới hạ Fide, nhưng nó đã kịp đào 1 cái lỗ\nHành tinh này sắp nổ tung rồi\nMau lượn thôi", new List<string> { "Chuẩn" }, character.InfoChar.Gender));
                            character.TypeMenu = 0;
                        }
                        else
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Ngươi muốn quay về hả?", new List<string> { "Ờm" }, character.InfoChar.Gender));
                            character.TypeMenu = 1;
                        }
                        break;
                    case 61: // goku yaradt normal in yardat
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Hãy cố gắng tập luyện\nThu nhập 9.999 bí kiếp để đổi trang phục Yardat nhé !", new List<string> { "Nhận\nthưởng", "OK" }, character.InfoChar.Gender));
                        character.TypeMenu = 0;
                        break;
                    case 56:
                        switch (character.InfoChar.MapId)
                        {
                            case 5:
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Ta là Whis được Đại Thiên Sứ cử xuống trái đất để thu nhập lại các trang bị Thần\nLinh bị kẻ xấu xa đánh cắp.Ta sẽ ban lại cho ngươi các món đồ kích hoạt viễn cổ\nnếu ngươi giao cho ta trang bị thần linh", new List<string> { "Ghép trang bị\nkích hoạt", "Đóng" }, character.InfoChar.Gender));
                                character.TypeMenu = 0;//0

                                break;
                            case 48:
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Giải đấu liên vũ trụ sắp diễn ra, quy tụ cao thủ khắp các vũ trụ\nMau mau đăng ký để nhận giải thưởng siêu cấp về cho vũ trụ của mình", new List<string> { "Hướng dẫn", "Đóng" }, character.InfoChar.Gender));
                                character.TypeMenu = 10;
                                break;
                            default:
                                if (character.DataPractice.Whis.Status is Whis_Status.DIED)
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Ta đang đói bụng, nếu có đồ ăn ngon thì ta sẽ tiếp tục tập với ngươi.", new List<string> { "Nói chuyện", "Học\ntuyệt kĩ", "Top 100", "Tặng\nđồ ăn" }, character.InfoChar.Gender));

                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Thử đánh với ta xem nào.\nNgươi còn 1 lượt nữa cơ mà.", new List<string> { "Nói chuyện", "Học\ntuyệt kĩ", "Top 100", "[LV: " + character.DataPractice.Whis.Level + "]" }, character.InfoChar.Gender));
                                }
                                character.TypeMenu = 0;
                                break;
                        }
                        break;
                    case 30:
                    case 31:
                    case 32:
                    case 33:
                    case 34:
                    case 35:
                    case 36:
                        BlackBallHandler.ForNpc.Rong_nSaoDen.OpenMenuRong_nSaoDen(character, npcId);
                        break;
                    case 29:

                        BlackBallHandler.ForNpc.Omega_Dragon.OpenMenuOmega_Dragon(character, npcId);
                        break;
                    case 54:
                        //    character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "NPC TẠM ĐÓNG"));
                        // character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Mini game chơi vui có thưởng", new List<string> { "Con số\nmay mắn", "Chọn\nAi đây", "Kéo\nBúa\nBao", "Tặng quà 8-3" }, character.InfoChar.Gender));
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Mini game chơi vui có thưởng", new List<string> { "Con số\nmay mắn", "Chọn\nAi đây", "Kéo\nBúa\nBao" }, character.InfoChar.Gender));

                        character.TypeMenu = 0;
                        break;
                    case 74:
                        switch (character.InfoChar.MapId)
                        {
                            case 5:
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Cậu cần gì ở tôi?", new List<string> { "Shop\nThần bí", "Shop\nHồng ngọc" }, character.InfoChar.Gender));
                                character.TypeMenu = 0;
                                break;
                            case 0 or 7 or 14:
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextToriBot[0], MenuNpc.Gi().MenuToriBot[0], character.InfoChar.Gender));
                                character.TypeMenu = 1;
                                break;
                        }
                        break;
                    case 70:
                        if (!TaskHandler.gI().ReportTask(character, npcId))
                        {



                        }
                        break;
                    case 72:
                    case 73:
                        {
                            if (character.InfoChar.MapId == 164)
                            {
                                if (character.DataNamecBattlefield.Star != 0)
                                {
                                    NamecBattlefield_Handler.GetPointTemporary(character);
                                }
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextMenuChienTruongNamec[4], MenuNpc.Gi().MenuChienTruongNamec[0], character.InfoChar.Gender));
                                character.TypeMenu = 10;
                                break;
                            }
                            //character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Trong thời gian mùa TEST diễn ra\nTạo nhân vật mới tặng 500 thỏi vàng\nX2 kinh nghiệm toàn mùa\nNếu nâng cấp VIP sẽ nhận được nhiều ưu đãi hơn nữa.\n|7|Lưu ý: nâng cấp VIP chỉ được 1 lần mỗi mùa", MenuNpc.Gi().MenuBroly[0], character.InfoChar.Gender));
                            //character.TypeMenu = 1;
                        }
                        break;
                    //case 75:
                    //    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Kìa mây mây ngang đầu, kìa núi núi lô nhô\nCùng em trên con đường, Đường bé xíu lô nhô\n|7|                ~~~ Đen Vâu ~~~~", new List<string> { "Shop Hè", "Shop\nVật phẩm", "Nhập\nGift Code\nSự kiện hè", "Đổi quà sự kiện", "Tặng\nBọ Cánh\nCứng", "Tặng\nNgài Đêm", "Hoàn trả\ncải trang" }, character.InfoChar.Gender));
                    //    character.TypeMenu = 0;
                    //    break;
                    case 76:
                        character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId,TextServer.gI().UPDATING));
                    //case 75:
                        break;
                    //    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "|7|[CHUYỂN SINH]\n|0|Khi chuyển sinh, ngươi sẽ được tăng 10% tăng lên 5% với mỗi cấp chuyển sinh\ncác loại chỉ số gốc và sức mạnh\n|2|Bù lại ngươi phải trả nguyên liệu và chi phí và Giảm TNSM\nNgươi có thể chuyển sinh cho [SƯ PHỤ] hoặc [ĐỆ TỬ]\n|7|Lưu ý: Max Là 5 Lần Chuyển Sinh", new List<string>() { "Sư phụ", "Đệ tử", "Từ chối" }, character.InfoChar.Gender));
                    //    character.TypeMenu = 1;
                    //    break;
                    case 26:

                        if (character.Zone.ZoneHandler.GetCountMob() <= 0)
                        {

                            character.CharacterHandler.SendMessage(Service.ServerMessage("Trại Độc Nhãn đã bị tiêu diệt, bạn có 5 phút để tìm kiếm ngọc 4 sao trước khi phi thuyền đến đón"));
                            character.CharacterHandler.SendMessage(Service.OpenUiSay(5, "Ta chịu thua nhưng các ngươi đừng mong lấy được ngọc của ta!"));
                            character.CharacterHandler.SendMessage(Service.BuyItem(character));
                            var clan2 = ClanManager.Get(character.ClanId);
                            if (clan2 != null && clan2.ClanDungeon.DoanhTraiDocNhan.Status != PhoBanStatus.SPECIAL)
                            {
                                clan2.ClanDungeon.DoanhTraiDocNhan.Win(clan2);
                            }
                        }
                        else
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Tên gà mờ,\nvẫn chưa thắng được ta thì đừng mơ đụng vào ngọc rồng\ncủa ta.", new List<string> { "Hủy" }, 3));
                            break;



                        }
                        break;

                    case 46:
                        if (character.Flag == 10 && character.PPower < 20)
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Cút về phe của ngươi mà thể hiện !"));
                        }
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Bọn Kaio do nhóc con Osin cầm đầu đã có mặt tại đây...Hãy chuẩn bị 'Tiếp\nKhách' nhé !", new List<string> { "Hướng\ndẫn\nthêm", "Giải trừ\nphép thuật\n1 ngọc", "Xuống\nTầng dưới", "Về nhà" }, character.InfoChar.Gender));
                        break;
                    case 44:
                        switch (character.InfoChar.MapId)
                        {
                            case 0:
                            case 7:
                            case 14:
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Nhân dịp Mở Test, NRO TARA tổ chức sự kiện cho những cư dân săn được nhiều boss nhất\nKhi giết được 1 boss sẽ được 1 điểm sát thần\nKhi Open sẽ dùng điểm này để đổi các phần quà hấp dẫn\nBạn đang có: " + character.DiemSuKien + " điểm sát thần.", new List<string> { "Top 10", "Đóng" }, character.InfoChar.Gender));
                                break;
                            case 50:
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextOsins[5], MenuNpc.Gi().MenuOsin[0], character.InfoChar.Gender));
                                    break;
                                }
                            case 154:
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextOsins[5], MenuNpc.Gi().MenuOsin[1], character.InfoChar.Gender));
                                    break;
                                }
                            case 155:
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextOsins[5], MenuNpc.Gi().MenuOsin[2], character.InfoChar.Gender));
                                    break;
                                }
                            case 127:
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextOsins[6], MenuNpc.Gi().MenuOsins[2], character.InfoChar.Gender));
                                    character.TypeMenu = 0;
                                    break;
                                }

                            case 52:
                                {
                                    if (TaskHandler.gI().ReportTask(character, npcId)) break;
                                        if (Mabu12hConfig.Status == Mabu12hStatus.DURING)
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextOsins[0], MenuNpc.Gi().MenuOsins[0], character.InfoChar.Gender));
                                    }
                                    else if (Mabu2hConfig.Status == Mabu2hStatus.DURING)
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextOsins[1], MenuNpc.Gi().MenuOsins[0], character.InfoChar.Gender));
                                    }
                                    else
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextOsins[2], MenuNpc.Gi().MenuOsins[0], character.InfoChar.Gender));
                                    }
                                    character.TypeMenu = 0;
                                    break;
                                }
                            case 120:
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextOsins[4], MenuNpc.Gi().MenuOsins[2], character.InfoChar.Gender));

                                }
                                break;
                            default:
                                {
                                    if (character.Flag == 10 && character.PPower < 20) character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, MenuNpc.Gi().TextOsins[3]));
                                    else character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextOsins[4], MenuNpc.Gi().MenuOsins[2], character.InfoChar.Gender));
                                    break;
                                }

                        }
                        character.TypeMenu = 0;
                        break;

                    //3 ông già
                    case 64:
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "", new List<string> {  "TOP\nNhiệm vụ", "TOP\nNạp thẻ", "TOP\nĐệ tử", "TOP\nBang hội", "Đóng" }, character.InfoChar.Gender));
                        character.TypeMenu = 0;
                        break;
                    case 0:
                    case 1:
                    case 2:
                        {
                            //if (TaskHandler.CheckTaskFinish(character, npcId)){
                            //    TaskHandler.DoClickNpcToNextTaskWithList(character,npcId);
                            //    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                            //}else{
                            if (!TaskHandler.gI().ReportTask(character, npcId))
                            {
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, string.Format(MenuNpc.Gi().TextBaOngGia[0], ServerUtils.GetMoneyParse(UserDB.GetVND(character.Player)), ServerUtils.GetMoneyParse(UserDB.GetTongVND(character.Player)), UserDB.GetPrivateGitcode(character.Player)), MenuNpc.Gi().MenuBaOngGia[0], character.InfoChar.Gender));
                                character.TypeMenu = 0;
                            }
                            break;
                        }
                    //Rương đồ
                    case 3:
                        {
                            if (character.Zone.Map.TileMap.Npcs.FirstOrDefault(i => i.Id == npcId) == null)
                            {
                                character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Hay lắm nhaaaa"));
                                return;
                            }
                            if (character.InfoChar.MapId == 153)
                            {
                                character.ShopId = 2222;
                                //   character.CharacterHandler.SendMessage(Service.ClanBox(ClanManager.Get(character.ClanId).ClanBox));
                            }
                            else
                            {
                                character.CharacterHandler.SendMessage(Service.SendBox(character, 1));

                            }
                            break;
                        }
                    //Đậu thần
                    case 4:
                        {
                            var magicTree = MagicTreeManager.Get(character.Id);
                            if (magicTree == null) return;
                            var ngoc = magicTree.Diamond;
                            if (magicTree.IsUpdate)
                            {
                                character.CharacterHandler.SendMessage(Service.MagicTree1(new List<string>() { $"Nâng cấp\nnhanh\n{ngoc} ngọc", "Huỷ\nnâng cáp" }));
                            }
                            else
                            {
                                if (magicTree.Peas == magicTree.MaxPea)
                                {
                                    character.CharacterHandler.SendMessage(Service.MagicTree1(new List<string>()
                                    {"Thu hoạch", $"Nâng cấp\n{ServerUtils.ConvertMilisecond(DataCache.UpgradeDauThanTime[magicTree.Level - 1])}\n{ServerUtils.GetMoney(DataCache.UpgradeDauThanGold[magicTree.Level - 1])}\nvàng"}));
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(Service.MagicTree1(new List<string>() { "Thu hoạch", $"Nâng cấp\n{ServerUtils.ConvertMilisecond(DataCache.UpgradeDauThanTime[magicTree.Level - 1])}\n{ServerUtils.GetMoney(DataCache.UpgradeDauThanGold[magicTree.Level - 1])} \nvàng", $"Kết hạt\nnhanh\n{ngoc} ngọc" }));
                                }
                            }
                            break;
                        }
                    //Bumma
                    case 7:
                        {
                            //if (TaskHandler.CheckTaskFinish(character,npcId))
                            //{
                            //    TaskHandler.DoClickNpcToNextTaskWithList(character, npcId);
                            //    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                            //}
                            //else
                            //{
                            if (!TaskHandler.gI().ReportTask(character, npcId)) {
                                var menu = MenuNpc.Gi().MenuShopDistrict[0];
                                if (character.ItemSells.Count > 0)
                                {
                                    menu = MenuNpc.Gi().MenuShopDistrict[2];
                                }
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextBumma[0], menu, character.InfoChar.Gender));
                                character.TypeMenu = 0;
                            }
                            break;
                            //}
                        }
                    //Dende
                    case 8:
                        {
                            if (!TaskHandler.gI().ReportTask(character, npcId)) {
                                if (character.DataNgocRongNamek.AlreadyPick(character))
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Ồ, Ngọc Zồng Na Méc, bạn thật là may's mắn\nnếu tìm đủ 7 viên sẽ được [Zồng Thiên Na Méc] ban điều ước", new List<string> { "Hướng\ndẫn\nGọi Rồng", "Gọi Rồng", "Từ chối" }, character.InfoChar.Gender));
                                    character.TypeMenu = 2;
                                    break;
                                }
                                else
                                {
                                    //if (TaskHandler.CheckTaskFinish(character, npcId))
                                    //{
                                    //    TaskHandler.DoClickNpcToNextTaskWithList(character, npcId);
                                    //    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                    //}
                                    //else
                                    //{
                                    var menu = MenuNpc.Gi().MenuShopDistrict[0];
                                    if (character.ItemSells.Count > 0)
                                    {
                                        menu = MenuNpc.Gi().MenuShopDistrict[2];
                                    }
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextDende[0], menu, character.InfoChar.Gender));
                                    character.TypeMenu = 0;
                                    break;
                                    //}
                                }
                            }
                            break;
                        }
                    //Appule
                    case 9:
                        {
                            //if (TaskHandler.CheckTaskFinish(character, npcId))
                            //{
                            //    TaskHandler.DoClickNpcToNextTaskWithList(character, npcId);
                            //    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                            //}
                            //else
                            //{
                            if (!TaskHandler.gI().ReportTask(character, npcId)) {
                                var menu = MenuNpc.Gi().MenuShopDistrict[0];
                                if (character.ItemSells.Count > 0)
                                {
                                    menu = MenuNpc.Gi().MenuShopDistrict[2];
                                }
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextAppule[0], menu, character.InfoChar.Gender));
                                character.TypeMenu = 0;
                            }
                            break;
                            //}
                        }
                    //
                    //ef
                    case 10:
                        {
                            switch (character.InfoChar.MapId)
                            {

                                case 153:
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Ta có thể giúp gì cho bang hội của bạn?", new List<string> { "Chức năng\nbang hội", "Nhiệm vụ\nBang\n[5/5]", "Đảo Kame", "Đóng" }, character.InfoChar.Gender));
                                    // character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Ta có thể giúp gì cho bang hội của bạn?", new List<string> { "Đổi tên\ntên bang\nviết tắt", "Chọn ngẫu nhiên\ntên bang\nviết tắt", "Nâng cấp\nBang hội", "Đóng" }, character.InfoChar.Gender));
                                    character.TypeMenu = 1;
                                    break;
                                default:
                                    if (!TaskHandler.gI().ReportTask(character, npcId))
                                    {
                                        character.CharacterHandler.SendMessage(character.InfoChar.MapId == 84
                                        ? Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextBrief[1],
                                            new List<string>()
                                            {
                                    character.InfoChar.Gender != 0
                                        ? character.InfoChar.Gender != 1 ? "Về Xayda" : "Về Namếc"
                                        : "Về\nTrái Đất"
                                            }, character.InfoChar.Gender)
                                        : Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextBrief[0], MenuNpc.Gi().MenuBrief[0],
                                            character.InfoChar.Gender));
                                    }
                                    break;
                            }
                            break;
                        }
                    //Cargo
                    case 11:
                        {
                            if (!TaskHandler.gI().ReportTask(character, npcId))
                            {
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextCargo[0], MenuNpc.Gi().MenuCargo[0], character.InfoChar.Gender));
                                break;
                            }
                            break;
                        }
                    //Cui
                    case 12:
                        {
                            if (!TaskHandler.gI().ReportTask(character, npcId))
                            {
                                switch (character.InfoChar.MapId)
                                {
                                    case 19:
                                        if (character.InfoTask.Id >= 21)
                                        {
                                            
                                            HelpMission.openMenuCui(character);
                                        }
                                        else
                                        {
                                            character.CharacterHandler.SendMessage(Service.OpenUiSay(5, "Bạn phải hoàn thành nhiệm vụ mới có thể mở khóa chức năng này"));
                                        }
                                        break;
                                    case 68:
                                        if (character.InfoTask.Id >= 21)
                                        {
                                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextCui[2], MenuNpc.Gi().MenuCui[3], character.InfoChar.Gender));
                                        }
                                        else
                                        {
                                            character.CharacterHandler.SendMessage(Service.OpenUiSay(5, "Bạn phải hoàn thành nhiệm vụ mới có thể mở khóa chức năng này"));
                                        }
                                        break;
                                    default:
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextCui[0], MenuNpc.Gi().MenuCui[0], character.InfoChar.Gender));
                                        break;
                                }
                            }
                            break;

                        }
                    //Quy lão
                    case 13:
                        {
                            if (TaskHandler.CheckTask(character, 13, 0))
                            {
                                TaskHandler.gI().PlusSubTask(character, 1);
                                return;
                            }
                            if (!TaskHandler.gI().ReportTask(character, npcId))
                            {
                                if (character.InfoChar.LearnSkill != null)
                                {
                                    var itemAdd = character.InfoChar.LearnSkill.ItemSkill;
                                    var time = character.InfoChar.LearnSkill.Time - ServerUtils.CurrentTimeMillis();
                                    var skillTemplate = Cache.Gi().SKILL_TEMPLATES.FirstOrDefault(skill => skill.Id == character.InfoChar.LearnSkill.ItemTemplateSkillId);

                                    if (character.InfoChar.LearnSkill.Time <= ServerUtils.CurrentTimeMillis())
                                    {
                                        ItemHandler.AddLearnSkill(character, itemAdd, skillTemplate);
                                        character.InfoChar.LearnSkill = null;
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextQuyLao[0], MenuNpc.Gi().MenuQuyLao[0], character.InfoChar.Gender));
                                        character.TypeMenu = 0;
                                    }
                                    else
                                    {
                                        var itemTempalte = ItemCache.ItemTemplate(itemAdd.Id);
                                        var ngoc = 5;
                                        if (time / 600000 >= 2)
                                        {
                                            ngoc += (int)time / 600000;
                                        }

                                        var menu = string.Format(TextServer.gI().ADDING_SKILL, skillTemplate.Name,
                                            itemTempalte.Level, ServerUtils.GetTime(time));
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, menu, new List<string>() { $"Học\nCấp tốc\n{ngoc} ngọc", "Huỷ", "Bỏ qua" }, character.InfoChar.Gender));
                                        character.TypeMenu = 3;
                                    }
                                }
                                else
                                {
                                    var menu = MenuNpc.Gi().MenuQuyLao[0];
                                    menu[2] = string.Format(menu[2], character.InfoEvent.PointSuKienHungVuong);
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextQuyLao[0], menu, character.InfoChar.Gender));
                                    character.TypeMenu = 0;
                                }
                                break;
                            }
                            break;
                        }
                    //Trưởng lão Guru
                    case 14:
                        {

                            if (TaskHandler.CheckTask(character, 13, 0))
                            {
                                TaskHandler.gI().PlusSubTask(character, 1);
                                return;
                            }

                            if (!TaskHandler.gI().ReportTask(character, npcId))
                            {
                                if (Server.Gi().NamecBattlefield.CanAction())
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextMenuChienTruongNamec[0], MenuNpc.Gi().MenuChienTruongNamec[0], character.InfoChar.Gender));
                                    character.TypeMenu = 4;
                                }

                                else
                                {
                                    if (character.InfoChar.Gender != 1)
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Nơi đây chỉ dành cho những chiến binh Namếc, hãy về hành tinh của mình đi."));
                                        return;
                                    }
                                    if (character.InfoChar.LearnSkill != null)
                                    {
                                        var itemAdd = character.InfoChar.LearnSkill.ItemSkill;
                                        var time = character.InfoChar.LearnSkill.Time - ServerUtils.CurrentTimeMillis();
                                        var skillTemplate = Cache.Gi().SKILL_TEMPLATES.FirstOrDefault(skill => skill.Id == character.InfoChar.LearnSkill.ItemTemplateSkillId);

                                        if (character.InfoChar.LearnSkill.Time <= ServerUtils.CurrentTimeMillis())
                                        {
                                            ItemHandler.AddLearnSkill(character, itemAdd, skillTemplate);
                                            character.InfoChar.LearnSkill = null;
                                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextQuyLao[1], MenuNpc.Gi().MenuQuyLao[1], character.InfoChar.Gender));
                                            character.TypeMenu = 0;
                                        }
                                        else
                                        {
                                            var itemTempalte = ItemCache.ItemTemplate(itemAdd.Id);
                                            var ngoc = 5;
                                            if (time / 600000 >= 2)
                                            {
                                                ngoc += (int)time / 600000;
                                            }

                                            var menu = string.Format(TextServer.gI().ADDING_SKILL, skillTemplate.Name,
                                                itemTempalte.Level, ServerUtils.GetTime(time));
                                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, menu, new List<string>() { $"Học\nCấp tốc\n{ngoc} ngọc", "Huỷ", "Bỏ qua" }, character.InfoChar.Gender));
                                            character.TypeMenu = 2;
                                        }
                                    }
                                    else
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextQuyLao[0], MenuNpc.Gi().MenuQuyLao[1], character.InfoChar.Gender));
                                        character.TypeMenu = 0;
                                    }
                                }
                                break;
                            }
                            break;
                        }
                    //Vua vegeta
                    case 15:
                        {
                            if (TaskHandler.CheckTask(character, 13, 0))
                            {
                                TaskHandler.gI().PlusSubTask(character, 1);
                                return;
                            }
                            if (!TaskHandler.gI().ReportTask(character, npcId))
                            {
                                if (character.InfoChar.Gender != 2)
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Nơi đây chỉ dành cho những chiến binh Xayda, hãy về hành tinh của mình đi."));
                                    return;
                                }
                                if (character.InfoChar.LearnSkill != null)
                                {
                                    var itemAdd = character.InfoChar.LearnSkill.ItemSkill;
                                    var time = character.InfoChar.LearnSkill.Time - ServerUtils.CurrentTimeMillis();
                                    var skillTemplate = Cache.Gi().SKILL_TEMPLATES.FirstOrDefault(skill => skill.Id == character.InfoChar.LearnSkill.ItemTemplateSkillId);

                                    if (character.InfoChar.LearnSkill.Time <= ServerUtils.CurrentTimeMillis())
                                    {
                                        ItemHandler.AddLearnSkill(character, itemAdd, skillTemplate);
                                        character.InfoChar.LearnSkill = null;
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextQuyLao[1], MenuNpc.Gi().MenuQuyLao[1], character.InfoChar.Gender));
                                        character.TypeMenu = 0;
                                    }
                                    else
                                    {
                                        var itemTempalte = ItemCache.ItemTemplate(itemAdd.Id);
                                        var ngoc = 5;
                                        if (time / 600000 >= 2)
                                        {
                                            ngoc += (int)time / 600000;
                                        }

                                        var menu = string.Format(TextServer.gI().ADDING_SKILL, skillTemplate.Name,
                                            itemTempalte.Level, ServerUtils.GetTime(time));
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, menu, new List<string>() { $"Học\nCấp tốc\n{ngoc} ngọc", "Huỷ", "Bỏ qua" }, character.InfoChar.Gender));
                                        character.TypeMenu = 2;
                                    }
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextQuyLao[0], MenuNpc.Gi().MenuQuyLao[1], character.InfoChar.Gender));
                                    character.TypeMenu = 0;
                                }
                                break;
                            }
                            break;
                        }
                    //Uron
                    case 16:
                        {
                            var idShop = 15 + character.InfoChar.Gender;
                            character.CharacterHandler.SendMessage(Service.Shop(character, 0, idShop));
                            character.ShopId = idShop;
                            character.TypeShop = 0;
                            break;
                        }
                    //Thần mèo
                    case 18:
                        {
                            //if (TaskHandler.CheckTaskFinish(character, npcId) && character.InfoChar.OriginalDamage >= 10000)
                            //{
                            //    TaskHandler.DoClickNpcToNextTaskWithList(character, npcId);
                            //}
                            //else
                            //{
                            if (!TaskHandler.gI().ReportTask(character, npcId)) {
                                if (character.StatusCDRD is Character.StatusConDuongRanDoc.JOIN)
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Hãy cầm lấy hai hạt đậu cuối cùng của ta đây\nCố giữ mình nhé " + character.Name + "!", new List<string> { "Cảm ơn\nSư phụ" }, character.InfoChar.Gender));
                                    ItemCache.GetItem(character, 63);
                                    character.TypeMenu = 4;

                                }
                                else
                                {
                                    switch (character.DataPractice.Progress)
                                    {

                                        case Practice_Progress.THAN_MEO_KARIN:
                                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId,
                                             "Muốn chiến thắng Tàu Pảy Pảy phải đánh bại được ta đã", new List<string> { !character.DataPractice.isAutoTrain() ? "Đăng ký\ntập\ntự động" : "Hủy đăng\nký tập\ntự động", "Nhiệm vụ", "Tập luyện\nvới\nThần Mèo", "Thách đấu\nThần Mèo" }, character.InfoChar.Gender));
                                            character.TypeMenu = 0;
                                            break;
                                        case Practice_Progress.YAJIRO:
                                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId,
                                             "Hãy đánh bại Yajiro sau đó bay theo cây Gậy Như Ý trên đỉnh tháp để đến thần điện gặp Thượng Đế", new List<string> { !character.DataPractice.isAutoTrain() ? "Đăng ký\ntập\ntự động" : "Hủy đăng\nký tập\ntự động", "Tập luyện\nvới\nYajiro", "Thách đấu\nYajiro" }, character.InfoChar.Gender));
                                            character.TypeMenu = 9;
                                            break;
                                        default:
                                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Con hãy bay theo cây Gậy Như Ý trên đỉnh tháp để đến Thần Điện gặp Thượng Đế\nCon rất xứng đáng làm đệ tử ông ấy.", new List<string> { !character.DataPractice.isAutoTrain() ? "Đăng ký\ntập\ntự động" : "Hủy đăng\nký tập\ntự động", "Tập luyện\nvới\nThần Mèo", "Tập luyện\nvới\nYajirô" }, character.InfoChar.Gender));
                                            character.TypeMenu = 3;
                                            break;
                                    }
                                }
                            }
                            //}
                            break;
                        }
                    //Thượng đế
                    //Bò Mộng
                    case 17:
                        if (!TaskHandler.gI().ReportTask(character, npcId)) {
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextBoMong[0], MenuNpc.Gi().MenuBoMong[0], character.InfoChar.Gender));
                            character.TypeMenu = 0;

                        }
                        break;
                    case 19:
                        {
                            switch (character.InfoChar.MapId)
                            {
                                case 141:
                                    if (character.Zone.ZoneHandler.GetCountMob() <= 0)
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Hãy nắm lấy tay ta mau !", new List<string> { "Về\nthần điện" }, character.InfoChar.Gender));
                                        character.TypeMenu = 0;
                                    }
                                    else
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Đánh hết quái đi rồi tính tiếp !", new List<string> { "OK" }, character.InfoChar.Gender));
                                        character.TypeMenu = 10;
                                    }
                                    break;
                                default:
                                    {
                                        switch (character.DataPractice.Progress)
                                        {
                                            case Practice_Progress.MR_POPO: // mr po po
                                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Pôpô là đệ tử của ta, luyện tập với Pôpô con sẽ có thêm nhiều\nkinh nghiệm\nđánh bại được Pôpô ta sẽ dạy võ công cho con", new List<string> { character.DataPractice.isAutoTrain() ? "Hủy Đăng\nký tập\ntự động" : "Đăng ký\ntập\ntự động", "Tập luyện\nvới\nTpô", "Thách đấu\nvới\nMr.Pôpô" }, character.InfoChar.Gender));
                                                character.TypeMenu = 3;
                                                break;
                                            case Practice_Progress.THUONG_DE: // thuong de
                                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Từ nay con sẽ là đệ tử của ta. Ta sẽ truyền cho con tất cả tuyệt kĩ", new List<string> { character.DataPractice.isAutoTrain() ? "Hủy Đăng\nký tập\ntự động" : "Đăng ký\ntập\ntự động", "Tập luyện\nvới\nThượng Đế", "Thách đấu\nvới\nThượng Đế" }, character.InfoChar.Gender));
                                                character.TypeMenu = 4;
                                                break;
                                            default:
                                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextThuongDe[1], new List<string>{ character.DataPractice.isAutoTrain() ? "Hủy Đăng\nký tập\ntự động" : "Đăng ký\ntập\ntự động",
                                                "Tập luyện\nvới\nMr.Pôpô",
                                                "Tập luyện\nvới\nThuợng Đế",
                                                "Đến\nKaio",
                                                "Vòng quay\nMay mắn",
                                                "Top Vòng quay\nmay mắn" }, character.InfoChar.Gender));
                                                character.TypeMenu = 0;
                                                break;
                                        }

                                        break;
                                    }
                            }
                            break;
                        }
                    case 20:
                        {

                            switch (character.DataPractice.Progress)
                            {
                                case Practice_Progress.KHI_BUBBLES:
                                    {
                                        var menu = new List<string>()
                                         {
                                               character.DataPractice.isAutoTrain() ? "Hủy Đăng\nký\ntập tự\nđộng" : "Đăng ký\ntập\ntự động",
                                               "Tập luyện\nvới\nBubbles",
                                               "Thách đấu\nvới\nBubbles",
                                               "Di chuyển"
                                         };
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextThanVuTru[0], menu, character.InfoChar.Gender));
                                        character.TypeMenu = 4;
                                    }
                                    break;
                                case Practice_Progress.KAIO:
                                    {
                                        var menu = new List<string>()
                                         {
                                               character.DataPractice.isAutoTrain() ? "Hủy Đăng\nký\ntập tự\nđộng" : "Đăng ký\ntập\ntự động",
                                               "Tập luyện\nvới\nKaio",
                                               "Thách đấu\nvới\nKaio",
                                               "Di chuyển"
                                         };
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextThanVuTru[0], menu, character.InfoChar.Gender));
                                        character.TypeMenu = 5;
                                    }
                                    break;
                                default:
                                    {
                                        var menu = new List<string>()
                                         {
                                               character.DataPractice.isAutoTrain() ? "Hủy Đăng\nký\ntập tự\nđộng" : "Đăng ký\ntập\ntự động",
                                               "Tập luyện\nvới\nBubbles",
                                               "Tập luyện\nvới\nThần Vũ\nTrụ",
                                               "Di chuyển"
                                         };
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextThanVuTru[0], menu, character.InfoChar.Gender));
                                        character.TypeMenu = 0;
                                    }
                                    break;
                            }
                            break;
                        }
                    //Bà hạt mít
                    case 21:
                        {
                            switch (character.InfoChar.MapId)
                            {
                                case 5:
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextBaHatMit[0], MenuNpc.Gi().MenuBaHatMit[3], character.InfoChar.Gender));
                                        character.TypeMenu = 1;
                                        break;
                                    }
                                case 42:
                                case 43:
                                case 44:
                                case 84:
                                    {
                                        List<string> menuBaHatMit = new List<string>();
                                        var bongTaiPorata2 = character.CharacterHandler.GetItemBagById(921);

                                        menuBaHatMit = MenuNpc.Gi().MenuBaHatMit[(character.InfoChar.IsNhanBua ? 0 : 1)];

                                        if (bongTaiPorata2 != null)
                                        {
                                            menuBaHatMit[(character.InfoChar.IsNhanBua ? 6 : 5)] = "Mở chỉ số\nBông tai\nPorata cấp 2";
                                        }

                                        character.CharacterHandler.SendMessage(
                                            Service
                                                .OpenUiConfirm(npcId, MenuNpc.Gi().TextBaHatMit[0], menuBaHatMit, character.InfoChar.Gender));
                                        character.TypeMenu = 0;
                                        break;
                                    }
                                case 46:
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextBaHatMit[0], MenuNpc.Gi().MenuBaHatMit[15], character.InfoChar.Gender));
                                        character.TypeMenu = 14;
                                        break;
                                    }
                                case 112:
                                    if (character.DataVoDaiSinhTu.Win is DiedRing_Character_Win.WIN && !character.DataVoDaiSinhTu.Reward)
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId,
                                            "Đây là phần thưởng cho con!", new List<string> { "1 vệ tinh\nbất kì", "1 Bùa 1h\nbất kì" }, character.InfoChar.Gender));
                                        character.TypeMenu = 23;

                                    }
                                    else
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Ngươi muốn đăng ký thi đấu võ đài?\nNhiều phần thưởng giá trị đang đợi ngươi đó.", new List<string> { "Top 100", "Đồng ý", "Từ chối", "Về\nđảo rùa" }, character.InfoChar.Gender));
                                        character.TypeMenu = 22;
                                    }


                                    break;
                            }

                            break;
                        }
                    case 22:
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Đại hội võ thuật Siêu Hạng\ndiễn ra 24/7 kể cả ngày lễ và chủ nhật\nHãy thi đấu ngay để khẳng định đẳng cấp của mình nhé",
                        new List<string> { "Top 100\nCao Thủ", "Hướng\ndẫn\nthêm", $"Miễn phí\nCòn {character.DataSieuHang.Ticket} vé", "Ưu tiên\nđấu ngay", "Về\nĐại Hội\nVõ Thuật" }, character.InfoChar.Gender));
                        character.TypeMenu = 0;
                        break;
                    //Bumma TL
                    case 37:
                        {
                            if (!TaskHandler.gI().ReportTask(character, npcId))
                            {
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextBumma[0], MenuNpc.Gi().MenuShopDistrict[1], character.InfoChar.Gender));
                                character.TypeMenu = 0;
                            }
                            break;
                        }
                    // Ca lích
                    case 38:
                        {
                            if (!TaskHandler.gI().ReportTask(character, npcId))
                            {
                                switch (character.InfoChar.MapId)
                                {
                                    case 28:
                                        {
                                            if (character.InfoTask.Id >= 23)
                                            {
                                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextCalich[0], MenuNpc.Gi().MenuCalich[0], character.InfoChar.Gender));

                                            }
                                            else
                                            {
                                                character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Bạn phải hoàn thành nhiệm vụ Fide mới có thể qua tương lai"));
                                                break;
                                            }
                                            character.TypeMenu = 0;

                                            break;
                                        }
                                    case 102:
                                        {
                                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextCalich[0], MenuNpc.Gi().MenuCalich[1], character.InfoChar.Gender));
                                            character.TypeMenu = 1;
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    //Santa
                    case 39:
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextSanta[0], MenuNpc.Gi().MenuSanta[0], character.InfoChar.Gender));
                            character.TypeMenu = 0;
                            break;
                        }
                    // trung thu
                    case 41:
                        {
                            //character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextTrungThu[0], MenuNpc.Gi().MenuTrungThu[0], character.InfoChar.Gender));
                            //character.TypeMenu = 99;
                            break;
                        }
                    //Quốc vương
                    case 42:
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextQuocVuong[0], MenuNpc.Gi().MenuQuocVuong[0], character.InfoChar.Gender));
                            character.TypeMenu = 0;
                            break;
                        }
                    // Giu ma
                    case 47:
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextGiuMa[0], MenuNpc.Gi().MenuGiuMa[character.InfoChar.isDiemDanh ? 1 : 0], character.InfoChar.Gender));
                            character.TypeMenu = 0;
                            break;
                        }
                    case 48:
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextNgoKhong[0], MenuNpc.Gi().MenuNgoKhong[0], character.InfoChar.Gender));
                            character.TypeMenu = 0;
                            break;
                        }
                    case 49:
                        {
                            switch (character.InfoChar.MapId)
                            {
                                case 0:
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextDuongTang[0], MenuNpc.Gi().MenuDuongTang[0], character.InfoChar.Gender));
                                    character.TypeMenu = 0;
                                    break;
                                case 123:
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextDuongTang[1], MenuNpc.Gi().MenuDuongTang[1], character.InfoChar.Gender));
                                    character.TypeMenu = 1;
                                    break;
                                case 122:
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextDuongTang[2], MenuNpc.Gi().MenuDuongTang[2], character.InfoChar.Gender));
                                    character.TypeMenu = 2;
                                    break;
                            }
                            break;
                        }
                    // Quả trứng
                    case 50:
                        {
                            if (character.InfoChar.ThoiGianTrungMaBu <= 0) return;
                            var seconds = (character.InfoChar.ThoiGianTrungMaBu - ServerUtils.CurrentTimeMillis()) / 1000;
                            if (seconds > 0) //chưa đủ thời gian nở
                            {
                                MenuNpc.Gi().MenuQuaTrung[0][0] = "Chờ\n" + seconds + " giây nữa";
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextQuaTrung[0], MenuNpc.Gi().MenuQuaTrung[0], character.InfoChar.Gender));
                                character.TypeMenu = 0;
                            }
                            else
                            {
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextQuaTrung[0], MenuNpc.Gi().MenuQuaTrung[1], character.InfoChar.Gender));
                                character.TypeMenu = 1;
                            }
                            break;
                        }
                    // Bill
                    case 55:
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextBill[0], MenuNpc.Gi().MenuBill[0], character.InfoChar.Gender));
                            character.TypeMenu = 0;
                            break;
                        }
                    // Nồi bánh
                    case 66:
                        {
                            if (Server.Gi().GameCache.isDaNauXong(character.Id))
                            {
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextNoiBanh[5], MenuNpc.Gi().MenuNoiBanh[1], character.InfoChar.Gender));
                                character.TypeMenu = 3;
                            }
                            else if (Server.Gi().GameCache.isDangNau(character.Id))
                            {
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, string.Format(MenuNpc.Gi().TextNoiBanh[6], ServerUtils.GetTime(Server.Gi().GameCache.GetTimeBanhDangNau(character.Id) - ServerUtils.CurrentTimeMillis())), MenuNpc.Gi().MenuNoiBanh[5], character.InfoChar.Gender));
                                character.TypeMenu = 4;
                            }
                            else
                            {
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, string.Format(MenuNpc.Gi().TextNoiBanh[3], character.Name), MenuNpc.Gi().MenuNoiBanh[3], character.InfoChar.Gender));
                                character.TypeMenu = 0;
                            }
                            break;
                        }
                    // Mrpopo
                    case 67:
                        {
                            if (ClanManager.Get(character.ClanId) != null)
                            {
                                var clan3 = ClanManager.Get(character.ClanId);
                                if (clan3.ClanDungeon.KhiGasHuyDiet.CheckOpen())
                                {
                                    var time = (clan3.ClanDungeon.KhiGasHuyDiet.Time - ServerUtils.CurrentTimeMillis()) / 60000;
                                    if (time < 1)
                                    {

                                        time = (clan3.ClanDungeon.KhiGasHuyDiet.Time - ServerUtils.CurrentTimeMillis()) / 1000;
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Thượng Đế vừa phát hiện 1 loại khí đang âm thầm\nhủy diệt mọi mầm sống trên Trái Đất,\nnó được gọi là Destron Gas.\nTa sẽ đưa các cậu đến nơi ấy, các cậu sẵn sàng chưa?\nBang hội của con đang Tham gia Khí Gas Level: " + clan3.ClanDungeon.KhiGasHuyDiet.Level + ".\nCon có muốn tham gia không?\nCòn " + time + " giây nữa", new List<string> { "Thông tin\nchi tiết", "Top 100\nBang hội", "OK", "Từ chối" }, character.InfoChar.Gender));
                                    }
                                    else
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Thượng Đế vừa phát hiện 1 loại khí đang âm thầm\nhủy diệt mọi mầm sống trên Trái Đất,\nnó được gọi là Destron Gas.\nTa sẽ đưa các cậu đến nơi ấy, các cậu sẵn sàng chưa?\nBang hội của con đang Tham gia Khí Gas Level: " + clan3.ClanDungeon.KhiGasHuyDiet.Level + ".\nCon có muốn tham gia không?\nCòn " + time + " phút nữa", new List<string> { "Thông tin\nchi tiết", "Top 100\nBang hội", "OK", "Từ chối" }, character.InfoChar.Gender));
                                    }
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Thượng Đế vừa phát hiện 1 loại khí đang âm thầm\nhủy diệt mọi mầm sống trên Trái Đất,\nnó được gọi là Destron Gas.\nTa sẽ đưa các cậu đến nơi ấy, các cậu sẵn sàng chưa?", new List<string> { "Thông tin\nchi tiết", "Top 100\nBang hội", "OK", "Từ chối" }, character.InfoChar.Gender));
                                }
                            }
                            else
                            {
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Thượng Đế vừa phát hiện 1 loại khí đang âm thầm\nhủy diệt mọi mầm sống trên Trái Đất,\nnó được gọi là Destron Gas.\nTa sẽ đưa các cậu đến nơi ấy, các cậu sẵn sàng chưa?", new List<string> { "Thông tin\nchi tiết", "Top 100\nBang hội", "OK", "Từ chối" }, character.InfoChar.Gender));
                            }
                            character.TypeMenu = 0;
                            break;
                        }
                    case 53:
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Ác quỷ truyền thuyết Hirudegarn\nđã thoát khỏi phong ấn ngàn năm\nhãy giúp tôi chế ngự nó", new List<string> { "OKER", "Từ chối" }, character.InfoChar.Gender));
                        character.TypeMenu = 0;
                        break;
                    case 63:
                        switch (character.InfoChar.MapId)
                        {
                            case 139:
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextMenuJaco[1], MenuNpc.Gi().MenuJaco[1], character.InfoChar.Gender));
                                character.TypeMenu = 1;
                                break;
                            default:
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextMenuJaco[0], MenuNpc.Gi().MenuJaco[0], character.InfoChar.Gender));
                                character.TypeMenu = 0;
                                break;
                        }
                        break;
                    case 62:
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Hãy giúp ta đánh bại bản sao\nNgươi chỉ có 5 phút để hạ hắn\nPhần thưởng của ngươi là 1 bình Commeson", new List<string> { "Hướng\ndẫn\nthêm","OK", "Từ chối" }, character.InfoChar.Gender));
                        character.TypeMenu = 0;
                        break;
                    //case 76:
                    //    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "[NGƯƠI CÓ MUỐN ĐỘ KIẾP CHO TRANG BỊ CỦA NGƯƠI ?]\nTa sẽ Nâng cấp trang bị [Kích Hoạt] Của ngươi\nKhi nâng cấp xong trang bị của ngươi sẽ tăng 1 bậc\nTrở thành [Trang bị độ kiếp],\nNhưng ngươi sẽ phải ta Nguyên Liệu và Tiền Công", MenuNpc.Gi().MenuGokuVoThan[0], character.InfoChar.Gender));
                    //    character.TypeMenu = 0;
                    //    break;
                    case 71:
                        if (!TaskHandler.gI().ReportTask(character, npcId))
                        {
                        }
                            break;
                    case 77:
                     //   character.CharacterHandler.SendZoneMessage(Service.NpcChat(npcId, "Về nhà ăn tết, tết, tết!"));
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "|0|Xin chàoo,Tui là Trangg (Người yêu của Dev)\n|6|Tui bán tất cả mọi thứ", new List<string> { "Shop\nVán bay", "Shop\nThẻ Rada", "Shop\nCải Trang", "Shop\nRồng Băng","Shop\nSự kiện"}, character.InfoChar.Gender));
                        character.TypeMenu = 0;
                        break;
                    case 23:
                        // ChampionShip.gI().OpenMenuGhiDanh(character);
                        switch (character.InfoChar.MapId)
                        {
                            case 130:

                                break;
                            case 129:
                                //if (character.DataDaiHoiVoThuat23.WoodChestLevel != 0)
                                //{
                                //    var round = character.DataDaiHoiVoThuat23.Round;
                                //    var level = character.DataDaiHoiVoThuat23.WoodChestLevel;
                                //    var menu = ChampionShip23Cache.TextMenu[1];
                                //    string.Format(menu[1], 1 * (1 * round));
                                //    string.Format(menu[2], 1 * (1 * round));
                                //    string.Format(menu[3], level);
                                //    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(23, string.Format(ChampionShip23Cache.Text[1], level), menu, character.InfoChar.Gender));
                                //}
                                //else
                                //{
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(23, ChampionShip23_Cache.Text[0], ChampionShip23_Cache.TextMenu[0], character.InfoChar.Gender));
                                //}
                                character.TypeMenu = 0;
                                break;
                            default:
                                if (TaskHandler.CheckTask(character, 19, 1))
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Muốn bỏ qua nhiệm vụ Đại Hội Võ Thuật, bạn cần 10.000 Ngọc\nBạn có muốn bỏ qua không?", new List<String> { "Bỏ qua", "Từ chối" }, character.InfoChar.Gender));
                                    character.TypeMenu = 6;
                                    break;
                                }
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(23, "Chào mừng bạn đến với Đại Hội Võ Thuật\nGiải " + ChampionShip.gI().TextChampionShip[ChampionShip.gI().TypeChampionShip] + " đang có " + ChampionShip.gI().PlayersRegister.Count + " người đăng ký thi đấu", ChampionShipCache.MenuNpcs[0], character.InfoChar.Gender));
                                character.TypeMenu = 0;
                                break;
                        }

                       
                        break;
                    
                    case 25:
                        
                            var clan = ClanManager.Get(character.ClanId);
                        if (clan != null)
                        {
                            //if (ServerUtils.TimeNow().Day - clan.TimeClanCreate.Day < 2)
                            //{
                            //    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Yêu cầu bang hội thành lập trên 2 ngày",
                            //      new List<string> { "Hướng\ndẫn\nthêm" }, character.InfoChar.Gender));
                            //    character.TypeMenu = 2;
                            //}
                            //else if (ServerUtils.TimeNow().Day - clan.Thành_viên.FirstOrDefault(i=>i.Id == character.Id).DateJoin.Day < 2)
                            //{
                            //    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Ngươi phải tham gia bang hội trên 2 ngày thì mới được đi doanh trại",
                            //       new List<string> { "Hướng\ndẫn\nthêm" }, character.InfoChar.Gender));
                            //    character.TypeMenu = 2;
                            //}

                            //if (ServerUtils.TimeNow().Day - ClanManagerr.Get(character.ClanId).TimeClanCreate.Day  < 2)
                            //{
                            //    character.CharacterHandler.SendMessage(Service.ServerMessage("Yêu cầu bang hội được thành lập trên 2 ngày"));
                            //    return;
                            //}

                            //if (character.Zone.ZoneHandler.GetCharacterClanInMap(character.ClanId).Count < 2 && clan.ClanDungeon.DoanhTraiDocNhan.CheckClose())
                            //{
                            //    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Ít nhất phải có 1 đồng đội bên cạnh để vào",
                            //      new List<string> { "Hướng\ndẫn\nthêm" }, character.InfoChar.Gender));
                            //    character.TypeMenu = 1;
                            //    return;
                            //}
                            if (clan.ClanDungeon.DoanhTraiDocNhan.CheckOpen())
                            {
                                var time = (ClanManager.Get(character.ClanId).ClanDungeon.DoanhTraiDocNhan.Time - ServerUtils.CurrentTimeMillis())/60000;
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Đồng bang của ngươi đã vào trước rồi.Ngươi có muốn vào\n"
                                    + "không?\nCòn "+time+" phút nữa!",
                                    new List<string> { "Vào\n(miễn phí)", "Không", "Hướng\ndẫn\nthêm", "Nhiệm vụ" }, character.InfoChar.Gender));
                                character.TypeMenu = 0;
                                break;
                            }

                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Hôm nay bang hội của ngươi chưa vào trại lần nào.Ngươi có muốn vào\n"
                                    + "không?\nĐể vào, ta khuyên ngươi nên có 3-4 người cùng bang đi cùng",
                                    new List<string> { "Vào\n(miễn phí)", "Không", "Hướng\ndẫn\nthêm", "Nhiệm vụ" }, character.InfoChar.Gender));
                            character.TypeMenu = 0;
                          
                                
                            
                        }
                        else
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Chỉ tiếp các bang hội, miễn tiếp khách vãng lai",
                                    new List<string> { "Hướng\ndẫn\nthêm" }, character.InfoChar.Gender));
                            character.TypeMenu = 2;
                        }
                        break;
                    default:
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, TextServer.gI().UPDATING));
                            break;
                        }
                }
            }
            catch
            {
               // Server.Gi().Logger.Error($"Error OpenUiMenu in Menu.cs: {e.Message} \n {e.StackTrace}", e);
            }
        }
        
        public static void MenuHandler(Message message, Character character)
        {
            try
            {
                var npcId = message.Reader.ReadByte();
                var menuId = message.Reader.ReadByte();
                var optionId = message.Reader.ReadByte();
                Server.Gi().Logger.Debug($"Menu Handler --------------------------- {npcId} - {menuId} - {optionId}");
                switch (npcId)
                {
                    //Đậu thần
                    case 4:
                        {
                            MenuDauThan(character, npcId, menuId, optionId);
                            break;
                        }
                    default:
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, TextServer.gI().UPDATING));
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error Menu Handler in Menu.cs: {e.Message} \n {e.StackTrace}", e);
            }
            finally
            {
                message?.CleanUp();
            }
        }
        public static void ConfirmMenuTrongTai(Character character, int npcId, int select){
            switch(character.TypeMenu){
                case 0:
                switch(select){
                    case 0: // top 100 cao thu
                            // character.CharacterHandler.SendMessage(SieuHang.ListRankCaoThu(character));
                            character.DataSieuHang.Handler.TopInfo100(character);

                            break;
                    case 1: // huong dan them
                    string text = "Giải đấu thể hiện đẳng cấp thực sự\nCác trận đấu diễn ra liên tục bất kể ngày đêm\nBạn hãy tham gia thi đấu để nâng hạng\nvà nhận giải thưởng khủng nhé"
                    + "Cơ cấu giải thưởng như sau\n(chốt và trao giải ngẫu nhiên từ 20h-23h mỗi ngày)\nTop 1 thưởng 100 ngọc\nTop 2-10 thưởng 20 ngọc\nTop 11-100 thưởng 5 ngọc\nTop 101-1000 thưởng 1 ngọc"
                    + "Mỗi ngày các bạn được tặng 1 vé tham dự miễn phí\n(tích lũy tối đa 3 vé) khi thua sẽ mất đi 1 vé\nKhi hết vé bạn phải trả 1 ngọc để đấu tiếp\n(trừ ngọc khi trận đấu kết thúc)"
                    + "Bạn không thể thi đấu với đấu thủ\ncó hạng thấp hơn mình\nChúc bạn may mắn, chào đoàn kết và quyết thắng";
                    character.CharacterHandler.SendMeMessage(Service.OpenUiSay((short)npcId, text));
                    break;
                    case 2: // mien phi con $ ve
                        character.DataSieuHang.Handler.TopInfo(character);
                            break;
                    case 3: // uu tien dau ngay !
                            // character.CharacterHandler.SendMessage(SieuHang.ListRankCaoThu(character));
                            character.DataSieuHang.Handler.TopInfo(character);
                            break;
                    case 4: // ve dai hoi vo thuat 
                            character.InfoChar.X = 467;
                            character.InfoChar.Y = 336;
                            MapManager.JoinMap(character, 52, ServerUtils.RandomNumber(19), false, false, 0);
                    break;
                    
                }
                break;
            }
        }
        public static void confirmMenuAdmin(Character character, int npcId, int select)
        {
            switch (character.TypeMenu)
            {
                case 5: // dung de action menu k co action
                    break;
                case 3:
                    switch (select)
                    {
                        case 0:
                            var inputChiso = new List<InputBox>();
                            var inputHP = new InputBox()
                            {
                                Name = "Nhập HP",
                                Type = 1,
                            };
                            inputChiso.Add(inputHP);

                            var inputMP = new InputBox()
                            {
                                Name = "Nhập MP",
                                Type = 1,
                            };
                            inputChiso.Add(inputMP);

                            var inputSD = new InputBox()
                            {
                                Name = "Nhập SD",
                                Type = 1,
                            };
                            inputChiso.Add(inputSD);

                            var inputCM = new InputBox()
                            {
                                Name = "Nhập Chí Mạng",
                                Type = 1,
                            };
                            inputChiso.Add(inputCM);

                            var inputAmor = new InputBox()
                            {
                                Name = "Nhập Giáp",
                                Type = 1,
                            };
                            inputChiso.Add(inputAmor);

                            character.CharacterHandler.SendMessage(Service.ShowInput("Menu Buff Bẩn Chỉ Số", inputChiso));
                            character.TypeInput = 24;
                            break;
                        case 1:
                            var inputChiso2 = new List<InputBox>();
                            var inputHP2 = new InputBox()
                            {
                                Name = "Nhập HP",
                                Type = 1,
                            };
                            inputChiso2.Add(inputHP2);

                            var inputMP2 = new InputBox()
                            {
                                Name = "Nhập MP",
                                Type = 1,
                            };
                            inputChiso2.Add(inputMP2);

                            var inputSD2 = new InputBox()
                            {
                                Name = "Nhập SD",
                                Type = 1,
                            };
                            inputChiso2.Add(inputSD2);

                            var inputCM2 = new InputBox()
                            {
                                Name = "Nhập Chí Mạng",
                                Type = 1,
                            };
                            inputChiso2.Add(inputCM2);

                            var inputAmor2 = new InputBox()
                            {
                                Name = "Nhập Giáp",
                                Type = 1,
                            };
                            inputChiso2.Add(inputAmor2);

                            character.CharacterHandler.SendMessage(Service.ShowInput("Menu Buff Bẩn Chỉ Số", inputChiso2));
                            character.TypeInput = 25;
                            break;
                    }
                    break;
                case 2:
                    switch (select)
                    {
                        case 0:
                            var allPlayerInSever = "\nALL SESSION : " + ServerUtils.GetMoneys(ClientManager.Gi().Sessions.Count) + " | ALL PLAYER: " + ServerUtils.GetMoneys(ClientManager.Gi().Characters.Count);
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(41, "Xin Chào " + character.Name + ", " + allPlayerInSever +"\n|0|Sever Status: [ON]" + "\n|3|Chọn thể loại muốn Buff !", MenuNpc.Gi().MenuAdmin[2], character.InfoChar.Gender));
                            character.TypeMenu = 1;
                            MenuAdminRecode.gI().NameCharSelect = character.Name;
                            break;
                        case 1:
                            if (MenuAdminRecode.gI().NameCharSelect == "Unknow")
                            {
                                var inputCheckGifcode = new List<InputBox>();
                                var inputCode = new InputBox()
                                {
                                    Name = "INSERT NAME PLAYER BUFF",
                                    Type = 1,
                                };
                                inputCheckGifcode.Add(inputCode);
                                character.CharacterHandler.SendMessage(Service.ShowInput("FIND PLAYER BY NAME", inputCheckGifcode));
                                character.TypeInput = 23;
                            }
                            else
                            {
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(41, "|3|Vui lòng chọn đối tượng Buff !", new List<string> { MenuAdminRecode.gI().NameCharSelect != "Unknow" ? MenuAdminRecode.gI().NameCharSelect : "Unknow", "Set" }, character.InfoChar.Gender));
                                character.TypeMenu = 4;
                            }
                            
                            break;
                    }
                    break;
                case 4:
                    if (select == 0)
                    {
                        if (MenuAdminRecode.gI().NameCharSelect == "Unknow")
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Vui lòng chọn đối tượng !"));
                            return;
                        }
                        var allPlayerInSever = "\nALL SESSION : " + ServerUtils.GetMoneys(ClientManager.Gi().Sessions.Count) + " | ALL PLAYER: " + ServerUtils.GetMoneys(ClientManager.Gi().Characters.Count);
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(41, "Xin Chào " + character.Name + ", " + allPlayerInSever + "\n|0|Sever Status: [ON]" + "\n|3|Chọn thể loại muốn Buff !", MenuNpc.Gi().MenuAdmin[2], character.InfoChar.Gender));
                        character.TypeMenu = 1;
                    }
                    if (select == 1)
                    {
                        var inputCheckGifcode = new List<InputBox>();

                        var inputCode = new InputBox()
                        {
                            Name = "INSERT NAME PLAYER BUFF",
                            Type = 1,
                        };
                        inputCheckGifcode.Add(inputCode);
                        character.CharacterHandler.SendMessage(Service.ShowInput("FIND PLAYER BY NAME", inputCheckGifcode));
                        character.TypeInput = 23;
                        break;
                    }
                    break;
                case 0:
               switch (select)
                    {
                        case 0: // check gfc
                            {
                                var inputCheckGifcode = new List<InputBox>();

                                var inputCode = new InputBox()
                                {
                                    Name = "Nhập Giftcode",
                                    Type = 1,
                                };
                                inputCheckGifcode.Add(inputCode);
                                character.CharacterHandler.SendMessage(Service.ShowInput("Menu Check giftcode", inputCheckGifcode));
                                character.TypeInput = 12;
                            }
                            break;
                        case 1: // open menu buff
                            var allPlayerInSever = "\nALL SESSION : " + ServerUtils.GetMoneys(ClientManager.Gi().Sessions.Count) + " | ALL PLAYER: " + ServerUtils.GetMoneys(ClientManager.Gi().Characters.Count);
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(41, "Xin Chào " + character.Name + ", " + allPlayerInSever + "\n|0|Sever Status: [ON]", new List<string> { "ME", "FIND PLAYER:\n"+MenuAdminRecode.gI().NameCharSelect}, character.InfoChar.Gender));
                            character.TypeMenu = 2;
                            break;
                        case 2: // init nrsd
                            break;
                        case 3: // init dhvt
                            break;
                        case 4: // init mabu 12h
                            break;
                        case 5: // init mabu 2h
                            break;
                        case 6: // reset count pho ban
                            //for (int i = 0; i < ClanManager.Entrys.Count; i++)
                            //{
                            //    var clan = ClanManager.Entrys[i];
                            //    clan.cdrd.Count = 3;
                            //    clan.ClanDungeon.DoanhTrai.Count = 2;
                            //    clan.Gas.Count = 2;
                            //}
                            //Server.Gi().Logger.PrintColor("RESET COUNT PHO BAN", "red");
                            break;
                    }
                    break;
                case 6:
                    switch (select)
                    {
                        case 0:
                            character.MineDiamond(10);
                            character.InfoChar.X = Init.NamecBalls[0].X;
                            character.InfoChar.Y = Init.NamecBalls[0].Y;
                            MapManager.JoinMap(character, Init.NamecBalls[0].MapId, NgocRongGold.Application.Extension.Namecball.Init.NamecBalls[0].ZoneId, false, false, 0);
                            break;
                        case 1:
                            character.MineGold(100000);
                            character.InfoChar.X =Init.NamecBalls[0].X;
                            character.InfoChar.Y = Init.NamecBalls[0].Y;
                            MapManager.JoinMap(character, Init.NamecBalls[0].MapId, NgocRongGold.Application.Extension.Namecball.Init.NamecBalls[0].ZoneId, false, false, 0);
                            break;
                        case 2:
                            break;
                    }
                    break;

            case 1:

                    switch (select) {
                        case 0:
                            var inputItem = new List<InputBox>();

                            var inputIdItem = new InputBox()
                            {
                                Name = "Nhập ID Item",
                                Type = 1,
                            };
                            inputItem.Add(inputIdItem);

                            var inputSoLuong = new InputBox()
                            {
                                Name = "Nhập Số Lượng",
                                Type = 1,
                            };
                            inputItem.Add(inputSoLuong);

                            character.CharacterHandler.SendMessage(Service.ShowInput("Menu Buff Item", inputItem));
                            character.TypeInput = 4;
                            break;
                        case 1:
                            {
                                var inputBoss = new List<InputBox>();

                                var inputIdBoss = new InputBox()
                                {
                                    Name = "Nhập ID Boss",
                                    Type = 1,
                                };
                                inputBoss.Add(inputIdBoss);
                                character.CharacterHandler.SendMessage(Service.ShowInput("Menu Spawm Boss", inputBoss));
                                character.TypeInput = 5;
                               // break;
                            }
                            break;
                        //case 2:
                        //    {
                        //        var inputCheckGifcode = new List<InputBox>();

                        //        var inputCode = new InputBox()
                        //        {
                        //            Name = "Nhập Giftcode",
                        //            Type = 1,
                        //        };
                        //        inputCheckGifcode.Add(inputCode);
                        //        character.CharacterHandler.SendMessage(Service.ShowInput("Menu Check giftcode", inputCheckGifcode));
                        //        character.TypeInput = 12;
                        //    }
                        //    break;
                        case 2:
                            var inputTask = new List<InputBox>();                           
                            var inputId = new InputBox()
                            {
                                Name = "Nhập ID Nhiệm vụ",
                                Type = 1,
                            };
                            inputTask.Add(inputId);

                            var inputIndex = new InputBox()
                            {
                                Name = "Nhập Index Nhiệm vụ",
                                Type = 1,
                            };
                            inputTask.Add(inputIndex);

                            var inputCount = new InputBox()
                            {
                                Name = "Nhập Count Nhiệm vụ",
                                Type = 1,
                            };
                            inputTask.Add(inputCount);
                            character.CharacterHandler.SendMessage(Service.ShowInput("Menu Buff Nhiệm vụ", inputTask));
                            character.TypeInput = 9;
                            break;
                        case 3:
                            {
                                //death
                                character.TypeInput = 10;
                            }
                            break;
                        case 4:
                            {
                                var inputMap = new List<InputBox>();
                                var inputIdMap = new InputBox()
                                {
                                    Name = "Nhập ID Map",
                                    Type = 1,
                                };
                                inputMap.Add(inputIdMap);
                                character.CharacterHandler.SendMessage(Service.ShowInput("Menu Teleport To Map", inputMap));
                                character.TypeInput = 6;
                            }
                            break;
                        case 5:
                            {
                             //   var randChar = ClientManager.gI().GetRandomCharacter();
                             //   Console.WriteLine("randChar: " + randChar.Name);
                                var inputBanned = new List<InputBox>();                              
                                var inputReason = new InputBox()
                                {
                                    Name = "Nhập lý do khóa",
                                    Type = 1,
                                };
                                inputBanned.Add(inputReason);
                                character.CharacterHandler.SendMessage(Service.ShowInput("Khóa tài khoản", inputBanned));
                                character.TypeInput = 3;
                            }
                            break;
                        case 6:
                            {
                                var inputTiemNang = new List<InputBox>();

                                var inputType = new InputBox()
                                {
                                    Name = "Nhập Type, 0) Sức Mạnh, 1) Tiềm Năng, 2) Tiềm Năng và Sức Mạnh",
                                    Type = 1,
                                };
                                inputTiemNang.Add(inputType);

                                var inputTnsm = new InputBox()
                                {
                                    Name = "Nhập Tiềm Năng",
                                    Type = 1,
                                };
                                inputTiemNang.Add(inputTnsm);


                              //  character.CharacterHandler.SendMessage(Service.ShowInput("Menu Buff Tiềm Năng và Sức Mạnh\nType: \n0) Tiềm năng\n1) Sức mạnh\n 2) Cả 2", inputTiemNang));
                                character.CharacterHandler.SendMessage(Service.ShowInput("Menu Buff Tiềm Năng và Sức Mạnh", inputTiemNang));
                                character.TypeInput = 7;
                            }
                            break;
                        case 7:
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(41, "|0|Would You Want?\nSET OR PLUS ?", MenuNpc.Gi().MenuAdmin[3], character.InfoChar.Gender));
                            character.TypeMenu = 3;
                            //var inputChiso = new List<InputBox>();



                            //var inputHP = new InputBox()
                            //{
                            //    Name = "Nhập HP",
                            //    Type = 1,
                            //};
                            //inputChiso.Add(inputHP);

                            //var inputMP = new InputBox()
                            //{
                            //    Name = "Nhập MP",
                            //    Type = 1,
                            //};
                            //inputChiso.Add(inputMP);

                            //var inputSD = new InputBox()
                            //{
                            //    Name = "Nhập SD",
                            //    Type = 1,
                            //};
                            //inputChiso.Add(inputSD);

                            //var inputCM = new InputBox()
                            //{
                            //    Name = "Nhập Chí Mạng",
                            //    Type = 1,
                            //};
                            //inputChiso.Add(inputCM);

                            //var inputAmor = new InputBox()
                            //{
                            //    Name = "Nhập Giáp",
                            //    Type = 1,
                            //};
                            //inputChiso.Add(inputAmor);

                            //character.CharacterHandler.SendMessage(Service.ShowInput("Menu Buff Bẩn Chỉ Số", inputChiso));
                            //character.TypeInput = 8;
                            break;
                        case 8:
                            var inputCongTien = new List<InputBox>();
                            var inputMoney = new InputBox()
                            {
                                Name = "Nhập Số Tiền Muốn Cộng",
                                Type = 1,
                            };
                            inputCongTien.Add(inputMoney);
                            character.CharacterHandler.SendMessage(Service.ShowInput("Menu Buff Money", inputCongTien));
                            character.TypeInput = 13;
                            break;
                    }
                    break;
            }
                    
        }
        
        public static void UiConfirm(Message message, Character character)
        {
            try
            {
                var npcId = message.Reader.ReadShort();
                var select = message.Reader.ReadByte();
                switch (npcId)
                {
                    case 56:
                        switch (character.TypeMenu)
                        {
                            case 6:
                                if (select == character.CombinneIndex.Count + 1) return;
                                character.CharacterHandler.RemoveItemBagById((short)character.CombinneIndex[select], 1);
                                character.DataPractice.Whis.Status = Extension.Practice.Whis.Whis_Status.LIVE;
                                character.CharacterHandler.SendMessage(Service.SendBag(character));
                                break;
                            case 4:
                                {
                                    switch (select)
                                    {
                                        
                                        case 0:
                                            if (character.InfoChar.Gold < 2000000000)
                                            {
                                                character.CharacterHandler.SendMessage(Service.ServerMessage("Yêu cầu 2 tỉ vàng"));
                                                return;
                                            }
                                            var countTbThanLinh = 0;
                                            List<int> IndexTbi = new List<int>();
                                            for (int i = 0; i < character.ItemBody.Count; i++)
                                            {
                                                if (character.ItemBody[i] != null)
                                                {
                                                    var itemBody = character.ItemBody[i];
                                                    if (ItemCache.ItemTemplate(itemBody.Id).Level == 13)
                                                    {
                                                        countTbThanLinh++;
                                                        IndexTbi.Add(itemBody.IndexUI);
                                                    }
                                                }
                                            }
                                            for (int tbi = 0; tbi < IndexTbi.Count; tbi++)
                                            {
                                                var item = character.ItemBody[IndexTbi[tbi]];
                                                var itemTemp = ItemCache.ItemTemplate(character.ItemBody[IndexTbi[tbi]].Id);
                                                character.CharacterHandler.RemoveItemBody(IndexTbi[tbi]);
                                                HandlerGhepTrangBiHuyDietForBody(character, itemTemp.Type, character.InfoChar.Gender, item.IndexUI);

                                            }
                                            character.MineGold(2000000000);
                                            character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                            character.CharacterHandler.SendMessage(Service.SendBody(character));
                                            character.CharacterHandler.SendMessage(Service.UpdateBody(character));
                                            character.CharacterHandler.SendMessage(Service.ServerMessage("Hiến tế trang bị thành công !"));
                                            break;
                                        
                                    }
                                }
                                break;
                            case 5:
                                {
                                    if (character.InfoChar.Gold < 2000000000)
                                    {
                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Yêu cầu 2 tỉ vàng"));
                                        return;
                                    }
                                    var countTbThanLinh = 0;
                                    List<int> IndexTbi = new List<int>();
                                    for (int i = 0; i < character.Disciple.ItemBody.Count; i++)
                                    {
                                        if (character.Disciple.ItemBody[i] != null)
                                        {
                                            var itemBody = character.Disciple.ItemBody[i];
                                            if (ItemCache.ItemTemplate(itemBody.Id).Level == 13)
                                            {
                                                countTbThanLinh++;
                                                IndexTbi.Add(itemBody.IndexUI);
                                            }
                                        }
                                    }
                                    for (int tbi = 0; tbi < IndexTbi.Count; tbi++)
                                    {
                                        var item = character.Disciple.ItemBody[IndexTbi[tbi]];
                                        var itemTemp = ItemCache.ItemTemplate(character.Disciple.ItemBody[IndexTbi[tbi]].Id);
                                        character.Disciple.CharacterHandler.RemoveItemBody(IndexTbi[tbi]);
                                        HandlerGhepTrangBiHuyDietForBody(character.Disciple, itemTemp.Type, itemTemp.Gender, item.IndexUI);

                                    }
                                    character.MineGold(2000000000);
                                    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                    character.CharacterHandler.SendMessage(Service.SendBody(character));
                                    character.CharacterHandler.SendZoneMessage(Service.UpdateBody(character.Disciple));
                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Hiến tế trang bị thành công !"));
                                    break;
                                }
                            case 1:
                                switch (character.InfoChar.MapId)
                                {
                                    case 5:
                                        switch (select)
                                        {
                                            case 0:
                                                {
                                                    var countTbThanLinh = 0;
                                                    List<int> IndexTbi = new List<int>();
                                                    for (int i = 0; i < character.ItemBody.Count; i++)
                                                    {
                                                        if (character.ItemBody[i] != null)
                                                        {
                                                            var itemBody = character.ItemBody[i];
                                                            if (ItemCache.ItemTemplate(itemBody.Id).Level == 13)
                                                            {
                                                                countTbThanLinh++;
                                                                IndexTbi.Add(i);
                                                            }
                                                        }
                                                    }
                                                    if (countTbThanLinh == 0 || IndexTbi.Count == 0)
                                                    {
                                                        character.CharacterHandler.SendMessage(Service.OpenUiSay(5, "Yêu cầu ít nhất 1 món đồ thần linh đang mặc trên người"));
                                                        return;
                                                    }
                                                    var textMenu = "Danh sách vật phẩm hiến tế cho Whis:";
                                                    for (int tbi = 0; tbi < IndexTbi.Count; tbi++)
                                                    {
                                                        var item = character.ItemBody[IndexTbi[tbi]];
                                                        textMenu += $"{ServerUtils.Color("green")}{tbi + 1}. {ItemCache.ItemTemplate(item.Id).Name}";
                                                    }
                                                    textMenu += $"{ServerUtils.Color("red")}Ngươi sẽ nhận lại 1 trang bị kích hoạt tương xứng trong thời kì viễn cỗ";
                                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(56, textMenu, new List<string> { "Hiến tế\n(2 tỷ vàng)", "Từ chối" }, character.InfoChar.Gender));
                                                    character.TypeMenu = 4;
                                                    break;
                                                }
                                            case 1:
                                                {
                                                    var countTbThanLinh = 0;
                                                    List<int> IndexTbi = new List<int>();
                                                    for (int i = 0; i < character.Disciple.ItemBody.Count; i++)
                                                    {
                                                        if (character.Disciple.ItemBody[i] != null)
                                                        {
                                                            var itemBody = character.Disciple.ItemBody[i];
                                                            if (ItemCache.ItemTemplate(itemBody.Id).Level == 13)
                                                            {
                                                                countTbThanLinh++;
                                                                IndexTbi.Add(i);
                                                            }
                                                        }
                                                    }
                                                    if (countTbThanLinh == 0 || IndexTbi.Count == 0)
                                                    {
                                                        character.CharacterHandler.SendMessage(Service.OpenUiSay(5, "Yêu cầu ít nhất 1 món đồ thần linh đang mặc trên người đệ tử"));
                                                        return;
                                                    }
                                                    var textMenu = "Danh sách vật phẩm hiến tế cho Whis:";
                                                    for (int tbi = 0; tbi < IndexTbi.Count; tbi++)
                                                    {
                                                        var item = character.Disciple.ItemBody[IndexTbi[tbi]];
                                                        textMenu += $"{ServerUtils.Color("green")}{tbi + 1}. {ItemCache.ItemTemplate(item.Id).Name}";
                                                    }
                                                    textMenu += $"{ServerUtils.Color("red")}Ngươi sẽ nhận lại 1 trang bị kích hoạt tương xứng trong thời kì viễn cỗ";
                                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(56, textMenu, new List<string> { "Hiến tế\n(2 tỷ vàng)", "Từ chối" }, character.InfoChar.Gender));
                                                    character.TypeMenu = 5;
                                                    break;
                                                }
                                        }
                                        break;
                                    default:
                                        switch (select)
                                        {
                                            case 0:
                                                character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[25], 56));
                                                character.ShopId = 20;
                                                break;  
                                            //case 1:
                                            // //   if (character.Player.Role != 1) return;
                                            //    character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[29], 56));
                                            //    character.ShopId = 21;
                                            //    break;
                                            //case 2:
                                            ////    if (character.Player.Role != 1) return;
                                            //    var existMat = false;
                                            //    for (int i = 0; i < character.ItemBag.Count; i++)
                                            //    {
                                            //        var itmBag = character.ItemBag[i];
                                            //        if (itmBag.Id >= 1280 && itmBag.Id <= 1289) existMat = true;
                                            //    }
                                            //    if (existMat)
                                            //    {
                                            //        character.CharacterHandler.SendMessage(Service.ServerMessage("Ngươi đã có mắt hỗn mang rồi !"));
                                            //        return;
                                            //    }
                                            //    var item = ItemCache.GetItemDefault(1280);
                                            //    character.CharacterHandler.AddItemToBag(false, item);
                                            //    character.CharacterHandler.SendMessage(Service.SendBag(character));
                                            //    character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + ItemCache.ItemTemplate(item.Id).Name));

                                            //    break;
                                        }
                                        break;
                                }
                                break;
                            case 2:
                                switch (select)
                                {
                                    case 0:
                                        character.CharacterHandler.SendMessage(Service.SendCombinne6(11238, (short)(character.InfoChar.Gender == 0 ? 11162 : character.InfoChar.Gender == 1 ? 11194 : 11193), npcId));
                                        character.CharacterHandler.SendMessage(Service.NpcChat(npcId, "Bư cô la, ba cô la, bư ra bư zô, đút vào đút ra, ..."));
                                        character.CharacterHandler.RemoveItemBagById(1235, 9999);
                                        character.MineGold(10000000);
                                        character.MineDiamond(99);
                                        character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                                        character.Skills.Add(new SkillCharacter()
                                        {
                                            Id = character.InfoChar.Gender == 0 ? 24 : character.InfoChar.Gender == 1 ? 26 : 25,
                                            SkillId = character.InfoChar.Gender == 0 ? 156 : character.InfoChar.Gender == 1 ? 176 : 166,
                                            CoolDown = 0,
                                            Point = 1,
                                            CurrExp = 10,
                                        });
                                        character.CharacterHandler.SendMessage(Service.AddSkill((short)(character.InfoChar.Gender == 0 ? 156 : character.InfoChar.Gender == 1 ? 176 : 166)));
                                        break;
                                }
                                break;
                            case 0:
                                switch (character.InfoChar.MapId)
                                {

                                    case 5:
                                        switch (select)
                                        {
                                            //case 0://hien te than linh
                                            //    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(56, "Ngươi muốn hiến tế cho Bản Thân hay Đệ Tử?", new List<string> { "Bản thân", "Đệ tử", "Từ chối" }, character.InfoChar.Gender));
                                            //    character.TypeMenu = 1;
                                            //    break;
                                            case 0:
                                                character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[21], 56));
                                                character.ShopId = 11;
                                                break;
                                        }
                                        break;
                                    default:
                                        switch (select)
                                        {
                                            case 3:
                                                if (character.DataPractice.Whis.Status is Extension.Practice.Whis.Whis_Status.DIED)
                                                {
                                                    
                                                    List<string> Menu = new List<string>();
                                                    character.CombinneIndex = new List<int>();
                                                    foreach (var item in character.ItemBag)
                                                    {
                                                        if (DataCache.ListItemGiftWhis.Contains(item.Id))
                                                        {
                                                            Menu.Add(ItemCache.ItemTemplate(item.Id).Name);
                                                            character.CombinneIndex.Add(item.Id);
                                                        }
                                                    }
                                                    if (Menu.Count == 0)
                                                    {
                                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Cần 1 món đồ ăn cho Whis"));
                                                        return;
                                                    }
                                                    Menu.Add("Từ chối");
                                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Ta đang đói bụng, nếu có đồ ăn ngon thì ta sẽ tiếp tục tập với ngươi", Menu, character.InfoChar.Gender));
                                                    character.TypeMenu = 6;
                                                }
                                                else
                                                {
                                                    //thach dau whis
                                                    Whis_Practice.gI().Practice(character, character.DataPractice.Whis.Level);
                                                }
                                                break;
                                            case 0:
                                            // new List<string> { "Chế tạo\ntrang bị\nthiên sứ","Nâng cấp\nmắt\nhỗn mang","Nhận mắt\nhỗn mang" }
                                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Ta có thể giúp gì cho ngươi?", new List<string> { "Chế tạo\ntrang bị\nthiên sứ" }, character.InfoChar.Gender));
                                                character.TypeMenu = 1;
                                                break;
                                            case 1:
                                                var skillGender = character.InfoChar.Gender == 0 ? "Super Kamekameha" : character.InfoChar.Gender == 2 ? "Cadic liên hoàn chưởng" : "Ma Phong Ba";
                                                var textMenu = $"{ServerUtils.Color("green")}Ta sẽ dạy ngươi tuyệt kĩ {skillGender}\n";
                                                var Cannot = false;
                                                var checkBiKip = character.CharacterHandler.GetItemBagById(1235) != null;
                                                if (checkBiKip)
                                                {
                                                    textMenu += character.CharacterHandler.GetItemBagById(1235).Quantity >= 9999 ? $"{ServerUtils.Color("blue")}Bí kíp tuyệt kĩ {character.CharacterHandler.GetItemBagById(1235).Quantity}/9999" : $"{ServerUtils.Color("red")}Bí kíp tuyệt kĩ {character.CharacterHandler.GetItemBagById(1235).Quantity}/9999";
                                                    if   (character.CharacterHandler.GetItemBagById(1235).Quantity < 9999) Cannot = true;
                                                }
                                                else
                                                {
                                                    textMenu += $"{ServerUtils.Color("red")}Bí kíp tuyệt kĩ 0/9999";
                                                    Cannot = true;
                                                }
                                                textMenu += character.InfoChar.Gold >= 10000000 ? $"{ServerUtils.Color("blue")}Giá vàng: 10.000.000" : $"{ServerUtils.Color("red")}Giá vàng: 10.000.000";
                                                textMenu += $"{(character.AllDiamond() >= 99 ? ServerUtils.Color("blue") : ServerUtils.Color("red"))}Giá ngọc: 99";
                                                if (character.InfoChar.Gold < 10000000 || character.AllDiamond() < 99) Cannot = true;
                                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, textMenu, new List<string> { "Đồng ý", "Từ chối" }, character.InfoChar.Gender));
                                                character.TypeMenu = Cannot ? 3 : 2;
                                                break;
                                            case 2:
                                                character.CharacterHandler.SendMessage(Server.Gi().BangXepHang.ListTopWhises());
                                                break;
                                        }
                                        break;

                                }
                                break;
                        }
                        break;
                    case 30:
                    case 31:
                    case 32:
                    case 33:
                    case 34:
                    case 35:
                    case 36:
                        BlackBallHandler.ForNpc.Rong_nSaoDen.ConfirmRong_nSaoDen(character, npcId, select);
                        break;
                    case 29:
                        BlackBallHandler.ForNpc.Omega_Dragon.Confirm(character, npcId, select);
                        break;
                    case 22:
                        ConfirmMenuTrongTai(character, npcId, select);
                        break;
                    case 64:
                        switch (select)
                        {
                            
                            case 0:
                                character.CharacterHandler.SendMessage(Server.Gi().BangXepHang.ListTopInfoTask());
                                break;
                            case 1:
                                character.CharacterHandler.SendMessage(Server.Gi().BangXepHang.ListTopInfoNapThe());
                                break;
                            case 2:
                                character.CharacterHandler.SendMessage(Server.Gi().BangXepHang.ListTopDisciple());
                                break;
                            case 3:
                                character.CharacterHandler.SendMessage(Server.Gi().BangXepHang.ListTopClanes());
                                break;
                        }
                        break;
                    case 41:
                        if (character.TypeMenu == 99)
                        {
                            ConfirmTrungThu(character, npcId, select);
                        }
                        else
                        {
                            confirmMenuAdmin(character, npcId, select);
                        }
                        break;
                    case 46:
                        ComfirmBabiday(character, npcId, select);
                        break;
                    case 44:
                        ConfirmOsin(character, npcId, select);
                        break;
                    case 0:
                    case 1:
                    case 2:
                        {
                            // 3 ông già
                            ConfirmBaOngGia(character, npcId, select);
                            break;
                        }
                    case 5:
                        {
                            ConfirmMeo(character, npcId, select);
                            break;
                        }
                    case 7: {
                            ConfirmBumma(character, npcId, select);
                            break;
                        }
                    case 8: {
                            ConfirmDende(character, npcId, select);
                            break;
                        }
                    case 9: {
                            ConfirmAppule(character, npcId, select);
                            break;
                        }
                    case 10: {
                            ConfirmBrief(character, npcId, select);
                            break;
                        }
                    case 11: {
                            ConfirmCargo(character, npcId, select);
                            break;
                        }
                    case 12: {
                            ConfirmCui(character, npcId, select);
                            break;
                        }
                    case 78:
                        switch (select)
                        {
                            case 0:
                                MapManager.JoinMap(character, 155, ServerUtils.RandomNumber(0, 19), true, true, character.TypeTeleport);
                                break;
                        }
                        break;
                    case 13: {
                            ConfirmQuyLao(character, npcId, select);
                            break;
                        }
                    case 14: {
                            ConfirmTruongLaoGuru(character, npcId, select);
                            break;
                        }
                    case 15: {
                            ConfirmVuaVegeta(character, npcId, select);
                            break;
                        }
                    case 17:
                        ConfirmBoMong(character, npcId, select);
                        break;
                    case 54:
                        ConfirmLiTieuNuong(character, npcId, select);
                        break;
                        //switch (character.TypeMenu)
                        //{
                        //    case 1:
                        //        switch (select)
                        //        {
                        //            default:
                        //                var thoivang = 0;
                        //                character.ItemBag.Where(item => item != null).ToList().ForEach(item =>
                        //                {
                        //                    if (item.Id == 457) thoivang+= item.Quantity;
                        //                });
                                        
                        //                var current = DataCache.VongQuayLTN[select];
                        //                if (character.LuckyBox.Count + current[1] > 100)
                        //                {
                        //                    character.CharacterHandler.SendMessage(Service.ServerMessage("Vui lòng dọn dẹp lại rương chứa vật phẩm"));
                        //                }
                        //                if (thoivang < current[0])
                        //                {
                        //                    character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Bạn không đủ thỏi vàng !"));
                        //                    return;
                        //                }
                        //                character.InfoChar.PointQuayThuong += current[1];
                        //                for (int i =0; i < current[1]; i++)
                        //                {
                        //                    var random = ServerUtils.RandomNumber(100);
                        //                    var item = ItemCache.GetItemDefault(1);
                                            
                        //                    switch (random)
                        //                    {
                        //                        case <= 20:
                        //                            item = ItemCache.GetItemDefault((short)(DataCache.ItemVongQuayLiTieuNuongEpic[ServerUtils.RandomNumber(DataCache.ItemVongQuayLiTieuNuongEpic.Count)]));
                        //                            item.IndexUI = character.LuckyBox.Count;
                        //                            item.Reason = "Vòng quay Lí Tiểu Nương";
                        //                            item.Options.Add(new OptionItem()
                        //                            {
                        //                                Id = 93,
                        //                                Param = ServerUtils.RandomNumber(2, 7)
                        //                            });
                        //                            character.LuckyBox.Add(item);
                        //                            break;
                        //                        case <= 60:
                        //                            item = ItemCache.GetItemDefault((short)(DataCache.ItemVongQuayLiTieuNuongRare[ServerUtils.RandomNumber(DataCache.ItemVongQuayLiTieuNuongRare.Count)]));
                        //                            item.IndexUI = character.LuckyBox.Count;
                        //                            item.Reason = "Vòng quay Lí Tiểu Nương";
                        //                            character.LuckyBox.Add(item);

                        //                            break;
                        //                        default:
                        //                            item = ItemCache.GetItemDefault((short)(DataCache.ItemVongQuayLiTieuNuongNormal[ServerUtils.RandomNumber(DataCache.ItemVongQuayLiTieuNuongNormal.Count)]));
                        //                            item.IndexUI = character.LuckyBox.Count;
                        //                            item.Reason = "Vòng quay Lí Tiểu Nương";
                        //                            character.LuckyBox.Add(item);

                        //                            break;
                        //                    }
                        //                }
                        //                CharacterDB.SaveInventory(character, false, false, false, true);
                        //                character.CharacterHandler.RemoveItemBagById(457, current[0]);
                        //                character.CharacterHandler.SendMessage(Service.SendBag(character));
                        //                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã tích lũy được thêm " + current[1] + " điểm quay thưởng"));

                        //                break;
                        //        }
                        //        break;
                        //    case 0:
                        //        switch (select)
                        //        {
                        //            case 0:
                        //                List<string> Menus = new List<string>();
                        //                for (int i = 0; i < DataCache.VongQuayLTN.Length; i++)
                        //                {
                        //                    var current = DataCache.VongQuayLTN[i];
                        //                    if (current[0] <= 5)
                        //                    {
                        //                        Menus.Add($"Quay\n{current[1]} lần\n({current[0]} thỏi\nvàng)");
                        //                    }
                        //                    else
                        //                    {
                        //                        Menus.Add($"Quay\n{current[1]} lần\n({current[0]} thỏi vàng)");
                        //                    }
                        //                }
                        //                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Mời bạn chọn số lượng quay", Menus,character.InfoChar.Gender));
                        //                character.TypeMenu = 1;
                        //                break;
                        //            case 1:
                        //                character.CharacterHandler.SendMessage(Service.SubBox(character.LuckyBox));
                        //                character.ShopId = 1111;
                        //                break;
                        //            case 2:
                        //                character.CharacterHandler.SendMessage(Server.Gi().BangXepHang.ListTopQuayThuong());    
                        //                break;
                        //        }
                        //        break;
                        //}
                        //break;
                    case 18: {
                            ConfirmThanMeo(character, npcId, select);
                            break;
                        }
                    case 75:
                        switch (character.TypeMenu)
                        {
                            case 1:
                                switch (select)
                                {
                                    case 0:
                                        if (character.TypeDoiThuong == 3)
                                        {

                                            character.CharacterHandler.RemoveItemBagById(695, 99);
                                            character.CharacterHandler.RemoveItemBagById(696, 99);
                                            character.CharacterHandler.RemoveItemBagById(697, 99);
                                            character.CharacterHandler.RemoveItemBagById(698, 99);
                                            character.CharacterHandler.RemoveItemBagById(694, 99);
                                            character.MineGold(1000000000);

                                            var item = ItemCache.GetItemDefault((short)(ServerUtils.RandomNumber(1241, 1244)));
                                            item.Options.ForEach(option => {
                                                option.Param = ServerUtils.RandomNumber(20, 35);
                                            });
                                            item.Options.Add(new OptionItem()
                                            {
                                                Id = 5,
                                                Param = ServerUtils.RandomNumber(1, 10),
                                            });
                                            character.CharacterHandler.AddItemToBag(false, item);
                                            character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + ItemCache.ItemTemplate(item.Id).Name));
                                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                                            character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                                        }
                                        break;
                                }
                                break;
                            case 0:
                                switch (select)
                                {
                                    case 0:
                                        character.CharacterHandler.SendMessage(Service.Shop(character, 0, 37 + character.InfoChar.Gender));
                                        character.ShopId = 37 + character.InfoChar.Gender;
                                        character.TypeShop = 0;
                                        break;
                                    case 1:
                                        character.CharacterHandler.SendMessage(Service.Shop(character, 3, 36));
                                        character.ShopId = 36;
                                        character.TypeShop = 3;
                                        break;
                                    case 2:
                                        var inputGiftcode = new List<InputBox>();
                                        var inputCode = new InputBox()
                                        {
                                            Name = "Nhập mã quà tặng",
                                            Type = 1,
                                        };
                                        inputGiftcode.Add(inputCode);
                                        character.CharacterHandler.SendMessage(Service.ShowInput("Nhập Giftcode NROLOTUSMUAHESOIDONG để nhận quà", inputGiftcode));
                                        character.TypeInput = 1;
                                        break;
                                    case 3:
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, $"{ServerUtils.Color("green")}Chế tạo cải trang siêu cấp {ItemHandler.TrueItem(character, new List<int> { 695, 696, 697, 698, 694, -1 }, new List<int> { 99, 99, 99, 99, 99, 100000000 }, 3)}", new List<string> { character.TypeDoiThuong != 3 ? "Từ chối" : "Chế tạo" }, character.InfoChar.Gender));
                                        character.TypeMenu = 1;
                                        break;
                                    case 5: // tang ngai dem
                                        {
                                            if (character.CharacterHandler.GetItemBagById(1256) == null)
                                            {
                                                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn không có Ngài Đêm"));

                                                return;
                                            }
                                            character.CharacterHandler.RemoveItemBagById(1256, 1);
                                            character.CharacterHandler.PlusPotential(1000);
                                            character.CharacterHandler.PlusPower(1000);
                                            character.CharacterHandler.SendMessage(Service.UpdateExp(2, 1000));
                                            List<int> ListItem = new List<int> { 1259,1260,1250,1251   };
                                            var randomHSD = ServerUtils.RandomNumber(100);
                                            var item = ItemCache.GetItemDefault((short)(ListItem[ServerUtils.RandomNumber(ListItem.Count)]));
                                            switch (randomHSD)
                                            {
                                                case <= 5:

                                                    break;
                                                default:
                                                    item.Options.Add(new OptionItem()
                                                    {
                                                        Id = 93,
                                                        Param = ServerUtils.RandomNumber(1, 7)
                                                    });
                                                    break;
                                            }
                                            character.CharacterHandler.AddItemToBag(false, item);
                                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                                            break;
                                        }
                                    case 6:
                                        {
                                            
                                        }
                                        break;
                                    case 4:// tang bo canh cung
                                        {
                                            if (character.CharacterHandler.GetItemBagById(1255) == null)
                                            {
                                                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn không có Bọ cánh cứng"));
                                                return;
                                            }
                                            character.CharacterHandler.RemoveItemBagById(1255, 1);
                                            character.CharacterHandler.PlusPotential(1000);
                                            character.CharacterHandler.PlusPower(1000);
                                            character.CharacterHandler.SendMessage(Service.UpdateExp(2, 1000));
                                            List<int> ListItem = new List<int> { 1259, 1260, 1250, 1251 };
                                            var randomHSD = ServerUtils.RandomNumber(100);
                                            var item = ItemCache.GetItemDefault((short)(ListItem[ServerUtils.RandomNumber(ListItem.Count)]));
                                            switch (randomHSD)
                                            {
                                                case <= 5:

                                                    break;
                                                default:
                                                    item.Options.Add(new OptionItem()
                                                    {
                                                        Id = 93,
                                                        Param = ServerUtils.RandomNumber(1, 7)
                                                    });
                                                    break;
                                            }
                                            character.CharacterHandler.AddItemToBag(false, item);
                                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                                            break;
                                        }
                                }
                                break;
                        }
                        break;
                    case 19: {
                            ConfirmThuongDe(character, npcId, select);
                            break;
                        }
                    case 20: {
                            ConfirmThanVuTru(character, npcId, select);
                            break;
                        }
                    case 73:
                        ConfirmFide(character, npcId, select);
                        break;
                    case 51:
                        ConfirmDuaHau(character, npcId, select);
                        break;
                    case 21: {
                            ConfirmBaHatMit(character, npcId, select);
                            break;
                        }
                    case 23: {
                            ConfirmGhiDanh(character, npcId, select);
                            //ChampionShip.gI().HandlerMenu(character, npcId, select);
                            break;
                        }
                    case 24: {
                            ConfirmRongThan(character, npcId, select);
                            break;
                        }
                    case 25: {
                            //character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Sẽ mở vào ngày 10/11"));
                            ConfirmLinhCanh(character, npcId, select);
                            break;
                        }
                    case 83:
                        {
                         ConfirmBongBangGolden(character, npcId, select);
                            break;
                        }
                    case 84:
                        {
                            ConfirmObito(character, npcId, select);
                            break;
                        }
                    case 53:
                        switch (select)
                        {
                            case 0:
                                var now = ServerUtils.TimeNow().Hour;
                                if (now == 22)
                                    {
                                        var zoneJoin = MapManager.Get(126).GetZoneNotMaxPlayer();
                                        character.InfoMore.TransportMapId = 126;
                                        character.CharacterHandler.SendMessage(Service.Transport(20));
                                        break;
                                    }   
                                else
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiSay(5, "Cuộc chiến sẽ bắt đầu lúc 22h mỗi ngày", false, character.InfoChar.Gender));
                                    }
                                break;
                        }
                        break;
                    case 361:
                        ConfirmMayDo(character, npcId, select);
                        break;
                    case 37: {
                            ConfirmBummaTL(character, npcId, select);
                            break;
                        }
                    case 38: {
                            ConfirmCalich(character, npcId, select);
                            break;
                        }
                    case 39: {
                            ConfirmSanta(character, npcId, select);
                            break;
                        }
                    case 74:
                        {
                            switch (character.TypeMenu)
                            {
                                case 0:
                                    switch (select)
                                    {
                                        case 0:
                                            character.CharacterHandler.SendMessage(Service.Shop(character, 0, 29));
                                            character.TypeShop = 0;
                                            character.ShopId = 29;
                                            break;


                                        case 1:
                                            character.CharacterHandler.SendMessage(Service.Shop(character, 3, 40));
                                            character.TypeShop = 3;
                                            character.ShopId = 40;
                                            break;
                                    }
                                    break;
                                case 1:
                                    {
                                        switch (select)
                                        {
                                            case 0:
                                                {
                                                    var menu = new List<string>(){"50K VND\n[{0}]","Đóng",};
                                                    menu[0] = string.Format(menu[0], ServerUtils.ToKMB(UserDB.GetVND(character.Player)));
                                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextToriBot[1], menu, character.InfoChar.Gender));
                                                    character.TypeMenu = 2;
                                                    break;
                                                }
                                            case 1:
                                                {
                                                    var menu = new List<string>() { "150K VND\n[{0}]", "Đóng", };
                                                    menu[0] =  string.Format(menu[0], ServerUtils.ToKMB(UserDB.GetVND(character.Player)));
                                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextToriBot[2], menu, character.InfoChar.Gender));
                                                    character.TypeMenu = 3;
                                                    break;
                                                }
                                            case 2:
                                                {
                                                    var menu = new List<string>() { "500K VND\n[{0}]", "Đóng", };
                                                    menu[0] =  string.Format(menu[0], ServerUtils.ToKMB(UserDB.GetVND(character.Player)));
                                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextToriBot[3], menu, character.InfoChar.Gender));
                                                    character.TypeMenu = 4;
                                                    break;
                                                }
                                        }
                                        break;
                                    }
                                case 2:
                                    {
                                        switch (select)
                                        {
                                            case 0:
                                                {
                                                    if (UserDB.GetVND(character.Player) < 50000)
                                                    {
                                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Cần 50.000 VND để mua mốc VIP này"));
                                                        return;
                                                    }
                                                    if (character.LengthBagNull() < 10)
                                                    {
                                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Cần ít nhất 10 ô trống"));
                                                        return;
                                                    }
                                                    UserDB.MineVND(character.Player, 50000);
                                                    character.InfoMuaGiai.Add(ConfigManager.gI().MuaGiai, 1);
                                                    var item = ItemCache.GetItemDefault(457, 14);
                                                    character.CharacterHandler.AddItemToBag(true, item);
                                                    item = ItemCache.GetItemDefault(721, 10);
                                                    character.CharacterHandler.AddItemToBag(true, item);
                                                    var itemExpire = ItemCache.GetItemDefaultExpire(1603, 10, 50);
                                                    character.CharacterHandler.AddItemToBag(true, itemExpire);
                                                    itemExpire = ItemCache.GetItemDefaultExpire((short)(1602), 1, 30);
                                                    character.CharacterHandler.AddItemToBag(true, itemExpire);
                                                    itemExpire = ItemCache.GetItemDefaultExpire((short)(1599), 1, 30);
                                                    character.CharacterHandler.AddItemToBag(true, itemExpire);
                                                    itemExpire = ItemCache.GetItemDefaultExpire((short)(1547), 1, 30);
                                                    character.CharacterHandler.AddItemToBag(true, itemExpire);
                                                    item = ItemCache.GetItemDefault(987, 5);
                                                    character.CharacterHandler.AddItemToBag(true, item);
                                                    item = ItemCache.GetItemDefault(1318, 10);
                                                    character.CharacterHandler.AddItemToBag(true, item);
                                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Nâng cấp VIP 1 thành công."));
                                                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                                                    break;
                                                }
                                           
                                        }
                                        break;
                                    }
                                case 3:
                                    {
                                        switch (select)
                                        {
                                            case 0:
                                                {
                                                    if (UserDB.GetVND(character.Player) < 150000)
                                                    {
                                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Cần 150.000 VND để mua mốc VIP này"));
                                                        return;
                                                    }
                                                    if (character.LengthBagNull() < 10)
                                                    {
                                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Cần ít nhất 10 ô trống"));
                                                        return;
                                                    }
                                                    UserDB.MineVND(character.Player, 150000);
                                                    character.InfoMuaGiai.Add(ConfigManager.gI().MuaGiai, 2);
                                                    CreateDiscipleCumber(character, character.InfoChar.Gender);
                                                    var item = ItemCache.GetItemDefault(457, 28);
                                                    character.CharacterHandler.AddItemToBag(true, item);
                                                    item = ItemCache.GetItemDefault(721, 10);
                                                    character.CharacterHandler.AddItemToBag(true, item);
                                                    item = ItemCache.GetItemDefault(956, 5);
                                                    character.CharacterHandler.AddItemToBag(true, item);
                                                    var itemExpire = ItemCache.GetItemDefaultExpire(1604, 1, 60);
                                                    character.CharacterHandler.AddItemToBag(true, itemExpire);
                                                    itemExpire = ItemCache.GetItemDefaultExpire((short)(1602), 1,90);
                                                    character.CharacterHandler.AddItemToBag(true, itemExpire);
                                                    itemExpire = ItemCache.GetItemDefaultExpire((short)(1600), 1,60);
                                                    character.CharacterHandler.AddItemToBag(true, itemExpire);
                                                    itemExpire = ItemCache.GetItemDefaultExpire((short)(1606), 1,90);
                                                    character.CharacterHandler.AddItemToBag(true, itemExpire);
                                                    itemExpire = ItemCache.GetItemDefaultExpire((short)(1547), 1, 90);
                                                    character.CharacterHandler.AddItemToBag(true, itemExpire);
                                                    item = ItemCache.GetItemDefault(987, 10);
                                                    character.CharacterHandler.AddItemToBag(true, item);
                                                    item = ItemCache.GetItemDefault(1318, 15);
                                                    character.CharacterHandler.AddItemToBag(true, item);
                                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Nâng cấp VIP 2 thành công."));
                                                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                                                    break;
                                                }

                                        }
                                        break;
                                    }
                                case 4:
                                    {
                                        switch (select)
                                        {
                                            case 0:
                                                {
                                                    if (UserDB.GetVND(character.Player) < 500000)
                                                    {
                                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Cần 500.000 VND để mua mốc VIP này"));
                                                        return;
                                                    }
                                                    if (character.LengthBagNull() < 10)
                                                    {
                                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Cần ít nhất 10 ô trống"));
                                                        return;
                                                    }
                                                    UserDB.MineVND(character.Player, 500000);
                                                    character.InfoMuaGiai.Add(ConfigManager.gI().MuaGiai, 3);
                                                    CreateDiscipleBilly(character, character.InfoChar.Gender);
                                                    var item = ItemCache.GetItemDefault(457, 55);
                                                    character.CharacterHandler.AddItemToBag(true, item);
                                                    item = ItemCache.GetItemDefault(721, 10);
                                                    character.CharacterHandler.AddItemToBag(true, item);
                                                    item = ItemCache.GetItemDefault(1204, 10);
                                                    character.CharacterHandler.AddItemToBag(true, item);
                                                    item = ItemCache.GetItemDefaultExpire(1599, 1);
                                                    character.CharacterHandler.AddItemToBag(true, item);
                                                    item = ItemCache.GetItemDefaultExpire(1600, 1);
                                                    character.CharacterHandler.AddItemToBag(true, item);
                                                    var itemExpire = ItemCache.GetItemDefaultExpire((short)(1602), 1, 180);
                                                    character.CharacterHandler.AddItemToBag(true, itemExpire);
                                                    item = ItemCache.GetItemDefaultExpire((short)(1606), 1);
                                                    character.CharacterHandler.AddItemToBag(true, item);
                                                    item = ItemCache.GetItemDefaultExpire((short)(860), 1);
                                                    character.CharacterHandler.AddItemToBag(true, item);
                                                    itemExpire = ItemCache.GetItemDefaultExpire((short)(1580), 1, 180);
                                                    character.CharacterHandler.AddItemToBag(true, itemExpire);
                                                    item = ItemCache.GetItemDefault((short)(1484), 1);
                                                    character.CharacterHandler.AddItemToBag(true, itemExpire);
                                                    item = ItemCache.GetItemDefault((short)(1604), 1);
                                                    character.CharacterHandler.AddItemToBag(true, itemExpire);
                                                    item = ItemCache.GetItemDefault(987, 30);
                                                    character.CharacterHandler.AddItemToBag(true, item);
                                                    item = ItemCache.GetItemDefault(1318, 30);
                                                    character.CharacterHandler.AddItemToBag(true, item);
                                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Nâng cấp VIP 3 thành công."));
                                                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                                                    break;
                                                }

                                        }
                                        break;
                                    }
                            }
                        }
                        break;
                        
                    case 42: {
                            ConfirmQuocVuong(character, npcId, select);
                            break;
                        }
                    case 47:
                        {
                            ConfirmGiuMa(character, npcId, select);
                            break;
                        }
                    case 76:
                        switch (select)
                        {
                            case 1:
                                character.CharacterHandler.SendMessage(KyGUIService.OpenShopKiGui(character));
                                character.TypeMenu = 1;
                                break;
                        }
                        break;
                    case 48:
                        ConfirmNgoKhong(character, npcId, select);
                        break;
                    case 49:
                        ConfirmDuongTang(character, npcId, select);
                        break;
                    case 50: {
                            if (character.InfoChar.ThoiGianTrungMaBu <= 0)
                            {
                                 UserDB.BanUser(character.Player.Id);
                                ClientManager.Gi().KickSession(character.Player.Session);
                                ServerUtils.WriteLog("hacktrung", $"Tên tài khoản {character.Player.Username} (ID:{character.Player.Id}) hack trứng");

                                var temp = ClientManager.Gi().GetPlayer(character.Player.Id);
                                if (temp != null)
                                {
                                    ClientManager.Gi().KickSession(temp.Session);
                                }
                                return;
                            }
                            ConfirmQuaTrung(character, npcId, select);
                            break;
                        }
                    case 55: {
                            ConfirmBill(character, npcId, select);
                            break;
                        }
                    case 52:
                        ConfirmHungVuong(character, npcId, select);
                        break;
                    
                    case 77:
                        ConfirmTrang(character, npcId, select);
                        break;
                    case 62:
                        ConfirmPotage(character, npcId, select);
                        break;
                    case 63:
                        ConfirmJaco(character, npcId, select);
                        break;
                    case 60:
                        ConfirmGokuSSJ60(character, npcId, select);
                        break;
                    case 61:
                        ConfirmGokuSSJ61(character, npcId, select);
                        break;
                    case 67:
                        {
                            switch (select)
                            {
                                case 0:
                                    var uisay = "Chúng ta gặp rắc rối rồiThượng Đế nói với tôi rằng có 1 loại khígọi là Destron Gas, thứ này không thuộc về nơi đây"
                                  + "\nNó tích tụ trên Trái Đấtvà nó sẽ hủy diệt mọi mô tế bào sốngCó tất cả 4 địa điểm mà Thượng Đế bảo tôi nói với cậuCậu có thể đến kiểm tra..."
                                  + "\nĐầu tiên là Thành phố Santa tọa lạc ở phía tây nam của thủ đô ở Viễn Đông."
                                  + "\nThứ hai là gần Kim Tự Tháp ở vùng Sa Mạc viễn tây của thủ đô Phía Bắc."
                                  + "\nThứ ba Vùng Đất Băng Giá ở Phương Bắc xa xôi"
                                  + "\nThứ tư là Hành tinh Bóng Tối đang che phủ một phần địa cầu\nCậu đã hiểu rõ chưa ?";
                                    character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, uisay));
                                    break;
                                case 1:
                                    character.CharacterHandler.SendMessage(Server.Gi().BangXepHang.ListTopInfoKhiGas());
                                    break;
                                case 2:

                                    //if (character.ClanId == -1 || ClanManager.Get(character.ClanId) == null)
                                    if (character.ClanId == -1)
                                    {
                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn chưa có bang hội"));
                                        return;
                                    }

                                    var clan = ClanManager.Get(character.ClanId);
                                    //if (clan.CondititonToJoinDungeon(character, clan, npcId))
                                    //{
                                    if (character.InfoChar.Power < 80000000000)
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Yêu cầu sức mạnh trên 80 tỉ"));
                                    }
                                    else if (clan.ClanDungeon.KhiGasHuyDiet.CheckClose() && clan.ClanDungeon.KhiGasHuyDiet.Count == 0)
                                    {
                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Mai hãy quay lại, hết lượt vào rồi !"));
                                        return;
                                    }
                                    else if (clan.ClanDungeon.KhiGasHuyDiet.CheckClose())
                                    {
                                        var inputKhiGas = new List<InputBox>();
                                        var inputLevel = new InputBox()
                                        {
                                            Name = "(Nhập cấp độ từ 0 -> 110)",
                                            Type = 1,
                                        };
                                        inputKhiGas.Add(inputLevel);
                                        character.CharacterHandler.SendMessage(Service.ShowInput("Nhập cấp độ Khí Gas", inputKhiGas));
                                        character.TypeInput = 22;
                                    }
                                    else if (character.InfoChar.Power < 80000000000)
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Yêu cầu sức mạnh trên 80 tỉ"));
                                    }
                                    else
                                    {

                                        var mapOld = MapManager.Get(character.InfoChar.MapId);
                                        mapOld.OutZone(character);
                                        clan.ClanDungeon.KhiGasHuyDiet.JoinMap(character, 149);
                                    }


                                    break;
                            }
                            break;
                        }
                    case 28:
                    ConfirmKyGUI(character, npcId, select);
                    break;
                    case 66:
                        {
                            ConfirmNoiBanh(character, npcId, select);
                            break;
                        }
                    case 82:
                        ConfirmCayTet(character, npcId, select);
                        break;
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error Ui Confirm in Menu.cs: {e.Message} \n {e.StackTrace}", e);
            }
            finally
            {
                message?.CleanUp();
            }
        }

        #region Menu COFIRM
        private static void ConfirmTrang(Character character, short npcid, int select)
        {

            switch (select)
            {
                case 0:
                    character.CharacterHandler.SendMessage(Service.Shop(character, 3, 30)); // shop van bay
                    character.ShopId = 30;
                    character.TypeShop = 3;
                    break;
                case 1:
                    character.CharacterHandler.SendMessage(Service.Shop(character, 3, 31)); // shop rada
                    character.ShopId = 31;
                    character.TypeShop = 3;
                    break;
                case 2:
                    character.CharacterHandler.SendMessage(Service.Shop(character, 3, 32)); // shop cai trang
                    character.ShopId = 32;
                    character.TypeShop = 3;

                    break;
                case 3:
                    character.CharacterHandler.SendMessage(Service.Shop(character, 3, 33)); // shop rong bang
                    character.ShopId = 33;
                    character.TypeShop = 3;
                    break;
                case 4:

                    character.CharacterHandler.SendMessage(Service.Shop(character, 3, 34)); // shop su kien
                    character.ShopId = 34;
                    character.TypeShop = 3;
                    break;
            }

        }
        private static void ConfirmCayTet(Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                case 0:
                    switch (select)
                    {

                        case 0:
                            {
                                var text = DoiThuongHandler.DoiThuong(character, "Để trang trí cần", new List<int> { 1517, 1518, 1519, 1520, -1 }, new List<int>() { 2, 4, 20, 1, 5000000 }, (int)DoiThuongType.TRANG_TRI_CAY_TET_NORMAL);
                                var menu = MenuNpc.Gi().MenuCayThongNoel[1];
                                var typeMenu = 1;
                                if (character.TypeDoiThuong != (int)DoiThuongType.TRANG_TRI_CAY_TET_NORMAL)
                                {
                                    menu = MenuNpc.Gi().MenuCayThongNoel[2];
                                    typeMenu = 4;
                                }
                                character.CharacterHandler.SendMeMessage(Service.OpenUiConfirm(npcId, text, menu, character.InfoChar.Gender));
                                character.TypeMenu = typeMenu;
                                break;
                            }
                        case 1:
                            {
                                var text = DoiThuongHandler.DoiThuong(character, "Để trang trí cần", new List<int> { 1517, 1518, 1519, 1520, -1, -2 }, new List<int>() { 2, 4, 20, 1, 10000000, 5 }, (int)DoiThuongType.TRANG_TRI_CAY_TET_NORMAL);
                                var menu = MenuNpc.Gi().MenuCayThongNoel[1];
                                var typeMenu = 1;
                                if (character.TypeDoiThuong != (int)DoiThuongType.TRANG_TRI_CAY_TET_VIP)
                                {
                                    menu = MenuNpc.Gi().MenuCayThongNoel[2];
                                    typeMenu = 4;
                                }
                                character.CharacterHandler.SendMeMessage(Service.OpenUiConfirm(npcId, text, menu, character.InfoChar.Gender));
                                character.TypeMenu = typeMenu;
                                break;
                            }
                    }
                    break;
                case 1:
                    switch (character.TypeDoiThuong)
                    {
                        case (int)DoiThuongType.TRANG_TRI_CAY_TET_NORMAL:
                            {
                                character.CharacterHandler.RemoveItemBagById(1517, 2);
                                character.CharacterHandler.RemoveItemBagById(1518, 4);
                                character.CharacterHandler.RemoveItemBagById(1519, 20);
                                character.CharacterHandler.RemoveItemBagById(1520, 1);
                                character.MineGold(5000000);
                                List<int> itemId = new List<int>() { 381, 382, 383, 384, 385, 441, 442, 443, 444, 445, 446, 447, 964, 965, 220, 221, 222, 223, 224, 1074, 1075, 1076, 1080, 1081, 1079, 1202, 1197, 1174, 1175, 1176 };
                                var itemAdd = ItemCache.GetItemDefault((short)(itemId[ServerUtils.RandomNumber(itemId.Count)]));
                                var itemTemplate = ItemCache.ItemTemplate(itemAdd.Id);
                                if (itemTemplate.Type == 11 || (itemTemplate.Type == 27 && DataCache.ListPetID.Contains(itemAdd.Id)))
                                {
                                    ItemHandler.HandleExpireItem(itemAdd);
                                }
                                character.CharacterHandler.AddItemToBag(true, itemAdd, "trang trí cây tết thường");
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Trang trí cây thành công, bạn nhận được " + itemTemplate.Name));
                                break;
                            }
                        case (int)DoiThuongType.TRANG_TRI_CAY_TET_VIP:
                            {
                                character.CharacterHandler.RemoveItemBagById(1517, 2);
                                character.CharacterHandler.RemoveItemBagById(1518, 4);
                                character.CharacterHandler.RemoveItemBagById(1519, 20);
                                character.CharacterHandler.RemoveItemBagById(1520, 1);
                                character.MineGold(5000000);
                                character.MineDiamond(5);
                                List<int> itemId = new List<int>() { 1201, 1203, 1150, 1151, 1152, 1153, 1154, 987, 1174, 1175, 1176, 1514, 1515, 1516, 1260, 1523, 1494,1521 };
                                var itemAdd = ItemCache.GetItemDefault((short)(itemId[ServerUtils.RandomNumber(itemId.Count)]));
                                var itemTemplate = ItemCache.ItemTemplate(itemAdd.Id);
                                if (itemTemplate.Type == 11 || (itemTemplate.Type == 27 && DataCache.ListPetID.Contains(itemAdd.Id)) || itemTemplate.Type == 5)
                                {
                                    ItemHandler.HandleExpireItem(itemAdd);
                                }
                                character.CharacterHandler.AddItemToBag(true, itemAdd, "trang trí cây tết VIP");
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Trang trí cây thành công, bạn nhận được " + itemTemplate.Name));
                                break;
                            }
                    }
                    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                    GameCache.gI().Decorativity();
                    break;
                case 4:// ingored
                    break;
            }
        }
        private static void ConfirmHungVuong(Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                case 0:
                    if (SuKienHungVuong.TimeCollectHatGiong <= ServerUtils.CurrentTimeMillis())
                    {
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Hãy trồng dưa hấu và mang chúng đến gặp ta đổi hồng ngọc.", new List<string> { "Trồng dưa hấu" }, character.InfoChar.Gender));
                        character.TypeMenu = 1;
                        return;
                    }
                    if (character.CharacterHandler.GetAllQuantityItemBagById(569) < SuKienHungVuong.DataTradeDuaHau[select][0])
                    {
                        character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn còn thiếu " + (SuKienHungVuong.DataTradeDuaHau[select][0] - (character.CharacterHandler.GetItemBagById(569) != null ? character.CharacterHandler.GetItemBagById(569).Quantity : 0)) + " dưa hấu"));
                        return;
                    }
                    character.CharacterHandler.RemoveItemBagById(569, SuKienHungVuong.DataTradeDuaHau[select][0]);
                    character.PlusDiamondLock(SuKienHungVuong.DataTradeDuaHau[select][1]);
                    character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                    character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn vừa nhận được " + SuKienHungVuong.DataTradeDuaHau[select][1] + " hồng ngọc."));
                    break;
                case 1:
                    if (character.InfoChar.ThoiGianDuaHau != 0)
                    {
                        // has dua hau
                        character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã có cây dưa hấu rồi."));
                        return;
                    }
                    else
                    {
                        character.InfoChar.ThoiGianDuaHau = DataCache._1DAY + ServerUtils.CurrentTimeMillis();
                        character.CharacterHandler.SendMessage(Service.ServerMessage("Cây dưa hấu vừa được trồng ở nhà bạn."));
                        SuKienHungVuong.TimeCollectHatGiong = 300000 + ServerUtils.RandomNumber(300000);
                    }
                    break;
                case 3:
                    switch (select)
                    {
                        case 0:
                            {
                                var text = DoiThuongHandler.DoiThuong(character, "Con muốn dâng sính lễ?", new List<int> { 1226, 1227, 1228, -1 }, new List<int> { 9, 9, 9, 1000000 }, (int)DoiThuongType.DANG_SINH_LE);
                                var menu = character.TypeDoiThuong == (int)DoiThuongType.DANG_SINH_LE ? MenuNpc.Gi().MenuHungVuong[1] : MenuNpc.Gi().MenuHungVuong[2];
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, text, menu, character.InfoChar.Gender));
                                character.TypeMenu = 4;
                                break;
                            }
                        case 1:
                            {
                                var text = DoiThuongHandler.DoiThuong(character, "Con muốn dâng sính lễ xịn?", new List<int> { 1226, 1227, 1228, -2 }, new List<int> { 9, 9, 9, 10 }, (int)DoiThuongType.DANG_SINH_LE_XIN);
                                var menu = character.TypeDoiThuong == (int)DoiThuongType.DANG_SINH_LE_XIN ? MenuNpc.Gi().MenuHungVuong[1] : MenuNpc.Gi().MenuHungVuong[2];
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, text, menu, character.InfoChar.Gender));
                                character.TypeMenu = 4;
                                break;
                            }
                        case 2:
                            {
                                var text = DoiThuongHandler.DoiThuong(character, "Con muốn dâng sính lễ?", new List<int> { 1591 }, new List<int> { 1 }, (int)DoiThuongType.DANG_SINH_LE2);
                                var menu = character.TypeDoiThuong == (int)DoiThuongType.DANG_SINH_LE2 ? MenuNpc.Gi().MenuHungVuong[1] : MenuNpc.Gi().MenuHungVuong[2];
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, text, menu, character.InfoChar.Gender));
                                character.TypeMenu = 4;
                                break;
                            }
                        case 3:
                            {
                                var text = DoiThuongHandler.DoiThuong(character, "Con muốn dâng sính lễ?", new List<int> { 1605 }, new List<int> { 1}, (int)DoiThuongType.DANG_SINH_LE3);
                                var menu = character.TypeDoiThuong == (int)DoiThuongType.DANG_SINH_LE3 ? MenuNpc.Gi().MenuHungVuong[1] : MenuNpc.Gi().MenuHungVuong[2];
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, text, menu, character.InfoChar.Gender));
                                character.TypeMenu = 4;
                                break;
                            }
                    }
                    break;
                case 4:
                    if (select != 0) break;
                    switch (character.TypeDoiThuong)//handle xử lí đổi thưởng
                    {
                        case 0:
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Dâng sính lễ thành công."));
                            character.CharacterHandler.RemoveItemBagById(1226, 9);
                            character.CharacterHandler.RemoveItemBagById(1227, 9);
                            character.CharacterHandler.RemoveItemBagById(1228, 9);
                            character.MineGold(1_000_000);
                            character.CharacterHandler.SendMessage(Service.BuyItem(character));
                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                            break;
                        case 1:
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Dâng sính lễ thành công."));
                            character.CharacterHandler.RemoveItemBagById(1226, 9);
                            character.CharacterHandler.RemoveItemBagById(1227, 9);
                            character.CharacterHandler.RemoveItemBagById(1228, 9);
                            character.MineDiamond(10);
                            character.CharacterHandler.SendMessage(Service.BuyItem(character));
                            character.CharacterHandler.SendMessage(Service.SendBag(character)); 
                            break;
                        case 2:
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Dâng sính lễ thành công."));
                            character.CharacterHandler.RemoveItemBagById(1591, 1);
                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                            break;
                        case 3:
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Dâng sính lễ thành công."));
                            character.CharacterHandler.RemoveItemBagById(1605, 1);
                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                            break;
                    }
                    break;
            }
        }
        private static void ConfirmDuaHau(Character character, short npcid, int select)
        {
            switch (character.TypeMenu)
            {
                case 0:
                    switch (select)
                    {
                        case 0:

                            break;
                    }
                    break;
                case 1:
                    switch (select)
                    {
                        case 0:
                            character.InfoChar.ThoiGianDuaHau = DataCache._1DAY + ServerUtils.CurrentTimeMillis();
                            var duahau = ItemCache.GetItemDefault(569);
                            duahau.Options.Add(new OptionItem()
                            {
                                Id = 30,
                                Param = 0
                            });
                            duahau.Options.Add(new OptionItem()
                            {
                                Id = 93,
                                Param = 30
                            });
                            character.CharacterHandler.AddItemToBag(true, duahau);
                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn nhận được Dưa Hấu"));
                            character.CharacterHandler.SendMessage(Service.DuaHau(character));
                            break;
                    }
                    break;
            }
        }
        private static void ConfirmGokuSSJ60(Character character, short npcid, int select)
        {

            switch (select)
            {
                case 0:
                    if (character.InfoChar.MapId == 80) MapManager.JoinMap(character, 131, ServerUtils.RandomNumber(20), false, false, 0);
                    else MapManager.JoinMap(character, 80, ServerUtils.RandomNumber(20), false, false, 0);
                    break;
            }

        }
        private static void ConfirmGokuSSJ61(Character character, short npcid, int select)
        {

            switch (select)
            {
                case 0:
                    if (character.CharacterHandler.GetItemBagById(590) == null || character.CharacterHandler.GetItemBagById(590).Quantity < 9999)
                    {
                        character.CharacterHandler.SendMessage(Service.ServerMessage("Ngươi không có đủ 9.999 Bí Kiếp !"));
                        return;
                    }
                    var item = ItemCache.GetItemDefault((short)(592 + character.InfoChar.Gender));
                    character.CharacterHandler.AddItemToBag(false,item);
                    character.CharacterHandler.RemoveItemBagById(590, 9999);
                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                    character.CharacterHandler.SendMessage(Service.ServerMessage("Nhận thưởng thành công !"));
                    
                    break;
            }

        }
        private static void ConfirmPotage(Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                case 0:
                    switch (select)
                    {
                        case 0:

                            break;
                        case 1: // go verus
                            var boss = new Boss();
                            boss.CreateBossClone(character, character.HpFull, character.MpFull, character.DamageFull, character.DefenceFull);
                            boss.CharacterHandler.SetUpInfo();
                            character.Zone.ZoneHandler.AddBoss(boss);
                            break;
                    }
                    break;
                case 1:
                    break;
            }
        }
        private static void ConfirmJaco(Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                case 0:
                    switch (select)
                    {
                        case 0:
                            MapManager.OutMap(character, 140);// go map clone
                            MapManager.GetMapOffline(140).JoinZone(character, character.Id, true, true);

                            break;
                        
                    }
                    break;
                case 1:
                    switch (select)
                    {
                        case 0: // return
                            MapManager.GetMapOffline(140).OutZone(character);
                            MapManager.JoinMap(character, 24, ServerUtils.RandomNumber(0, 20), true, true, 0);
                            break;
                    }
                    break;
            }
        }
        private static void ConfirmFide(Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                //case 1:

                //    switch (select)
                //    {
                //        case 0:
                //            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "|7|Nâng cấp VIP 1 bạn sẽ được\n|3|Nhận ngay Cải trang Xe triều đình, 100 thỏi vàng", MenuNpc.Gi().MenuBroly[1], character.InfoChar.Gender));

                //            character.TypeMenu = 2;
                //            break;
                //        case 1:
                //            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "|7|Nâng cấp VIP 2 bạn sẽ được\n|3|Tặng 1 Đệ Cumber,Cải trang Mị Nương và 100 Thỏi Vàng\n|3|Nhận ngẫu nhiên từ 1 - 100 Hồng Ngọc", MenuNpc.Gi().MenuBroly[2], character.InfoChar.Gender));
                //            character.TypeMenu = 3;
                //            break;
                //        case 2:
                //            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "|7|Nâng cấp VIP 3 bạn sẽ được\n|3|Cải trang Siêu thần và 300 Thỏi Vàng\n|3|Nhận ngẫu nhiên từ 1 - 100 Hồng Ngọc", MenuNpc.Gi().MenuBroly[3], character.InfoChar.Gender));
                //            character.TypeMenu = 4;
                //            break;
                //        case 3:
                //            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "|7|Nâng cấp VIP 4 bạn sẽ được\n|3|Cải trang Obito độc quyền và 300 Thỏi Vàng\n|3|Nhận ngẫu nhiên từ 1 - 100 Hồng Ngọc", MenuNpc.Gi().MenuBroly[4], character.InfoChar.Gender));
                //            character.TypeMenu = 5;
                //            break;
                //    }

                //    break;
                case 2:

                    var vip = character.InfoChar.VIP;
                    var @char = character.InfoChar;

                    switch (select)
                    {

                        case 0:

                            if (vip == 0)
                            {
                                if (UserDB.GetVND(character.Player) >= 300000)
                                {
                                    if (GetItemNapVIP(character, 1))
                                    {
                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Đã mở khóa VIP 1 thành công !"));
                                    }
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Không đủ tiền, vui lòng nạp thêm"));
                                }
                            }
                            else
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Con đã mở khóa VIP 1 rồi"));
                            }
                            break;


                    }
                    break;
                case 3:
                    switch (select)
                    {
                        case 0:

                            vip = character.InfoChar.VIP;
                            if (vip == 2)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Con đã mở khóa VIP 2 rồi"));
                                return;
                            }
                            if (vip == 1)
                            {
                                if (UserDB.GetVND(character.Player) >= 700000)
                                {
                                    if (GetItemNapVIP(character, 2))
                                    {
                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Đã mở khóa VIP 2 thành công !"));
                                    }
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Không đủ tiền , vui lòng nạp thêm"));
                                }
                            }
                            else
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Phải mở khóa VIP 1 mới có thể mở VIP 2"));
                            }

                            break;
                    }
                    break;
                case 4:
                    switch (select)
                    {
                        case 0:

                            vip = character.InfoChar.VIP;
                            if (vip == 3)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Con đã mở khóa VIP 3 rồi"));
                                return;
                            }
                            if (vip == 2)
                            {
                                if (UserDB.GetVND(character.Player) >= 1000000)
                                {
                                    if (GetItemNapVIP(character, 3))
                                    {
                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Đã mở khóa VIP 3 thành công !"));
                                    }
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Không đủ tiền, vui lòng nạp thêm"));
                                }
                            }
                            else
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Phải mở khóa VIP 2 mới có thể mở VIP 2"));
                            }

                            break;
                    }
                    break;
                case 5:
                    switch (select)
                    {
                        case 0:

                            vip = character.InfoChar.VIP;
                            if (vip == 4)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Con đã mở khóa VIP 3 rồi"));
                                return;
                            }
                            if (vip == 3)
                            {
                                if (UserDB.GetVND(character.Player) >= 2000000)
                                {
                                    if (GetItemNapVIP(character, 4))
                                    {
                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Đã mở khóa VIP 4 thành công !"));
                                    }
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Không đủ tiền, vui lòng nạp thêm"));
                                }
                            }
                            else
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Phải mở khóa VIP 3 mới có thể mở VIP 4"));
                            }

                            break;
                    }
                    break;
            }
        }
        public static bool GetItemNapVIP(Character character, int vip)
        {
            switch (vip)
            {
                case 1:
                    if (character.LengthBagNull() < 2)
                    {
                        character.CharacterHandler.SendMessage(Service.ServerMessage("Yêu cầu trống 2 ô hành trang"));
                        return false;
                    }
                    //ItemCache.GetItem(character, 457, 10);
                    //ItemCache.GetItem(character, 711, 1);
                    var x4 = ItemCache.GetItemDefault(1293);
                    
                    character.CharacterHandler.AddItemToBag(false, x4, "Nap VIP 1");
                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                    UserDB.MineVND(character.Player,600000);
                    character.CharacterHandler.SetUpInfo(true);
                    character.InfoChar.VIP = vip;

                    return true ;
                case 2:
                    if (character.LengthBagNull() < 2)
                    {
                        character.CharacterHandler.SendMessage(Service.ServerMessage("Yêu cầu trống 2 ô hành trang"));
                        return false;
                    }
                    //ItemCache.GetItem(character, 457, 100);
                    //   ItemCache.GetItem(character, 459, 10);
                    //   ItemCache.GetItem(character, 860, 1);
                    var minuong = ItemCache.GetItemDefault(860);
                    
                    CreateDiscipleCumber(character, character.InfoChar.Gender);
                    var random = ServerUtils.RandomNumber(1, 100);
                    character.PlusDiamondLock(random);
                    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                    character.CharacterHandler.AddItemToBag(false, minuong, "Nap VIP 2");
                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                    UserDB.MineVND(character.Player,1400000);
                    character.CharacterHandler.SetUpInfo(true);
                    character.InfoChar.VIP = vip;

                    return true;
                case 3:
                    if (character.LengthBagNull() < 2)
                    {
                        character.CharacterHandler.SendMessage(Service.ServerMessage("Yêu cầu trống 2 ô hành trang"));
                        return false;
                    }
                    //ItemCache.GetItem(character, 457, 150);
                    //   ItemCache.GetItem(character, 459, 10);
                    //   ItemCache.GetItem(character, 860, 1);
                    short id = 0;
                    switch (character.InfoChar.Gender)
                    {
                        case 0:
                            id = 905;
                            break;
                        case 1:
                            id = 907;
                            break;
                        case 2:
                            id = 911    ;
                            break;
                    }
                    var sieuthan = ItemCache.GetItemDefault(id);

                    //CreateDiscipleCumber(character, character.InfoChar.Gender);
                    var rand = ServerUtils.RandomNumber(1, 100);
                    character.PlusDiamondLock(rand);
                    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                    character.CharacterHandler.AddItemToBag(false, sieuthan, "Nap VIP 3");
                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                    UserDB.MineVND(character.Player, 2000000);
                    character.CharacterHandler.SetUpInfo(true);
                    character.InfoChar.VIP = vip;
                    return true;
                case 4:
                    if (character.LengthBagNull() < 1)
                    {
                        character.CharacterHandler.SendMessage(Service.ServerMessage("Yêu cầu trống 1 ô hành trang"));
                        return false;
                    }
                    ItemCache.GetItem(character, 457, 1);
                    ItemCache.GetItem(character, 1548, 1);
                    UserDB.MineVND(character.Player, 4000000);
                    var rand2 = ServerUtils.RandomNumber(1, 100);
                    character.PlusDiamondLock(rand2);
                    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                    character.InfoChar.VIP = vip;
                    return true;
            }
            return false;
        }
        private static void ConfirmBoMong   (Character character,short npcId, int select)
        {
            var c = character.CharacterHandler;
            switch (character.TypeMenu)
            {
                case 0:
                    switch (select)
                    {
                        case 0: // nap hong ngoc
                            //var inputNap = new List<InputBox>();
                            //var inputHongNgoc= new InputBox()
                            //{
                            //    Name = "Nhập số hồng ngọc muốn quy đổi",
                            //    Type = 1,
                            //};
                            //inputNap.Add(inputHongNgoc);
                            //character.CharacterHandler.SendMessage(Service.ShowInput("Bạn đang có: " + ServerUtils.GetMoney(UserDB.GetHongNgoc(character.Player)) + " hồng ngọc", inputNap));
                            //character.TypeInput = 16;

                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextNapThe[0], MenuNpc.Gi().MenuNapThe[3], character.InfoChar.Gender));
                            character.TypeMenu = 2;
                            break;
                        case 1: //
                            character.CharacterHandler.SendMessage(BoMongQuest_Handler.gI().OpenHubTask(character));
                            break;
                        case 2: // nv hang ngay
                            
                            if (character.DataSideTask.HaveTask())
                            {
                                if (character.DataSideTask.IsDone())
                                {
                                    var powerBuff = character.InfoChar.Power * (((int)character.DataSideTask.Quest.Difficult < 1 ? 1 : (int)character.DataSideTask.Quest.Difficult) * 5) / 100;
                                    character.InfoChar.Power += powerBuff;
                                    character.InfoChar.Potential += powerBuff;
                                    character.InfoChar.NangDong++;
                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + powerBuff + " tiềm năng và sức mạnh"));
                                    character.DataSideTask.Quest = null;
                                    break;
                                }
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, character.DataSideTask.GetCurrentTask(), MenuNpc.Gi().MenuBoMong[2], character.InfoChar.Gender));
                                character.TypeMenu = 5;
                            }
                            else if (character.DataSideTask.Count <= 0)
                            {
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Hôm nay bạn đã làm hết nhiệm vụ rồi", MenuNpc.Gi().MenuBoMong[2], character.InfoChar.Gender));
                                character.TypeMenu = 5;
                            }
                            else
                            {
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, string.Format(MenuNpc.Gi().TextBoMong[1], character.DataSideTask.Count), MenuNpc.Gi().MenuBoMong[1], character.InfoChar.Gender));
                                character.TypeMenu = 4;
                            }
                            break;
                        case 3:
                            var text = $"|2|Bảng giá quy đổi hồng ngọc, Rate " + ConfigManager.gI().Rate_Hồng_Ngọc;
                            var texts = new List<string>();
                            foreach (var textt in DataCache.ExchangeHongNgoc)
                            {
                                text += $"{ServerUtils.Color("red")}{ServerUtils.GetMoneyParse((textt[0]))}đ - {(ServerUtils.GetMoneyParse(textt[1] * (int)ConfigManager.gI().Rate_Hồng_Ngọc))} hồng ngọc";
                                texts.Add($"Quy đổi\n{ServerUtils.GetMoneyParse(textt[0])}đ");
                            }
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, text, texts, character.InfoChar.Gender));
                            character.TypeMenu = 5;
                            break;
                    }
                    break;
                case 5:
                    {
                        var exchange = DataCache.ExchangeHongNgoc[select];
                        if (UserDB.GetVND(character.Player) < exchange[0])
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Tài khoản của bạn số dư không đủ, vui lòng nạp thêm để giao dịch"));
                            break;
                        }
                        var diamondLockGet = exchange[1] * ConfigManager.gI().Rate_Hồng_Ngọc;
                        UserDB.MineVND(character.Player, exchange[0]);
                        character.CharacterHandler.PlusDiamondLock((int)diamondLockGet);
                        character.PlusDiamond((int)diamondLockGet);

                        character.CharacterHandler.SendMessage(Service.BuyItem(character));
                        character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã quy đổi thành công " + diamondLockGet + " hồng ngọc"));
                        break;
                    }
                case 4:
                    {
                        HangNgayQuest_Handler.AccecptTask(character, (HangNgayQuest_Difficult)select);
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, character.DataSideTask.GetCurrentTask(), MenuNpc.Gi().MenuBoMong[2], character.InfoChar.Gender));
                        character.TypeMenu = 5;
                        break;
                    }
                case 2://Chọn danh sách loại thẻ nạp
                    {
                        switch (select)
                        {
                            case 0:
                                {
                                    character.NapTheTemp.LoaiThe = "7 SAO";//VIETTEL
                                    break;
                                }
                            //case 1:
                            //    {
                            //        character.NapTheTemp.LoaiThe = "VINAPHONE";
                            //        break;
                            //    }
                            //case 2:
                            //    {
                            //        character.NapTheTemp.LoaiThe = "MOBIFONE";
                            //        break;
                            //    }
                            //case 3:
                            //    {
                            //        character.NapTheTemp.LoaiThe = "ZING";
                            //        break;
                            //    }
                            //default:
                            //    {
                            //        character.NapTheTemp.LoaiThe = "";
                            //        return;
                            //    }
                        }
                        // Chọn mệnh giá
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextNapThe[1], MenuNpc.Gi().MenuNapThe[1], character.InfoChar.Gender));
                        character.TypeMenu = 3;
                        character.MenuPage = 0;//Page 0
                        break;
                    }
                case 3://Chọn mệnh giá thẻ nạp
                    {
                        switch (character.MenuPage)
                        {
                            case 0://page 0
                                {
                                    switch (select)
                                    {
                                        case 0:
                                            {
                                                //10k
                                                character.NapTheTemp.MenhGia = 10000;
                                                break;
                                            }
                                        case 1:
                                            {
                                                //20k
                                                character.NapTheTemp.MenhGia = 20000;
                                                break;
                                            }
                                        case 2:
                                            {
                                                //30k
                                                character.NapTheTemp.MenhGia = 30000;
                                                break;
                                            }
                                        case 3:
                                            {
                                                //50k
                                                character.NapTheTemp.MenhGia = 50000;
                                                break;
                                            }
                                        case 4:
                                            {
                                                //Mệnh giá khác
                                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextNapThe[1], MenuNpc.Gi().MenuNapThe[2], character.InfoChar.Gender));
                                                character.TypeMenu = 3;
                                                character.MenuPage = 1;//Page 1
                                                return;
                                            }
                                        default:
                                            {
                                                return;
                                            }
                                    }
                                    break;
                                }
                            case 1:
                                {
                                    switch (select)
                                    {
                                        case 0:
                                            {
                                                //100k
                                                character.NapTheTemp.MenhGia = 100000;
                                                break;
                                            }
                                        case 1:
                                            {
                                                //200k
                                                character.NapTheTemp.MenhGia = 200000;
                                                break;
                                            }
                                        case 2:
                                            {
                                                //300k
                                                character.NapTheTemp.MenhGia = 300000;
                                                break;
                                            }
                                        case 3:
                                            {
                                                //500k
                                                character.NapTheTemp.MenhGia = 500000;
                                                break;
                                            }
                                        case 4:
                                            {
                                                //1000k
                                                character.NapTheTemp.MenhGia = 1000000;
                                                break;
                                            }
                                        default:
                                            {
                                                return;
                                            }
                                    }
                                    break;
                                }
                        }
                        // Hiển thị menu input nhập seri và số thẻ
                        var inputNapThe = new List<InputBox>();
                        var inputSeri = new InputBox()
                        {
                            Name = TextServer.gI().INPUT_SERI_THE,
                            Type = 0,
                        };
                        inputNapThe.Add(inputSeri);
                        var inputPin = new InputBox()
                        {
                            Name = TextServer.gI().INPUT_PIN_THE,
                            Type = 0,
                        };
                        inputNapThe.Add(inputPin);
                        character.CharacterHandler.SendMessage(Service.ShowInput(TextServer.gI().NHAP_TT_THE, inputNapThe));
                        character.TypeInput = 0;
                        break;
                    }
            }
        }
        private static void ConfirmLinhCanh(Character character, short npcId, int select)
        {
            switch (character.TypeMenu) {
                case 0:
                    {
                        switch (select)
                        {
                            case 0: // not have reddotmaps so init and go
                                var clan = ClanManager.Get(character.ClanId);
                                if (clan == null) break;
                                if (clan.CondititonToJoinDungeon(character, clan, npcId))
                                {
                                    if (RedRibbonManager.gI().countRedRibbon >= ConstRedRibbon.N_MAX_RED_RIBBON)
                                    {
                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Doanh trại đã đầy, vui lòng quay trở lại sau tí nữa"));
                                    }
                                    else if (clan.ClanDungeon.DoanhTraiDocNhan.CheckOpen())
                                    {
                                        var mapOld = MapManager.Get(character.InfoChar.MapId);
                                        mapOld.OutZone(character);
                                        character.InfoChar.X = 63;
                                        character.InfoChar.Y = 432;
                                        MapManager.Get(53).GetZoneById(clan.Id).ZoneHandler.JoinZone(character, false, false, 0);
                                    }
                                    else if (clan.ClanDungeon.DoanhTraiDocNhan.Count == 0)
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Bang hội của ngươi hôm nay đã đi doanh trại rồi, hãy quay lại vào ngày mai"));
                                    }
                                    else 
                                    {
                                        RedRibbonManager.gI().countRedRibbon++;
                                        clan.ClanDungeon.DoanhTraiDocNhan.Open(clan);
                                        var mapOld = MapManager.Get(character.InfoChar.MapId);
                                        mapOld.OutZone(character);
                                        character.InfoChar.X = 63;
                                        character.InfoChar.Y = 432;
                                        MapManager.Get(53).GetZoneById(clan.Id).ZoneHandler.JoinZone(character, false, false, 0);
                                    }
                                }
                                break;
                            case 3:
                                if (TaskHandler.CheckTask(character, 20, 1))
                                {
                                    TaskHandler.gI().PlusSubTask(character, 1);
                                }
                                break;
                        }
                    }
                    break;
            }
        }

        private static void ConfirmBaOngGia(Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                case 0:
                    {
                        switch (select)
                        {
                            
                            case 0://Gift code
                                {


                                    var inputGiftcode = new List<InputBox>();
                                    var inputCode = new InputBox()
                                    {
                                        Name = "Nhập mã quà tặng",
                                        Type = 1,
                                    };
                                    inputGiftcode.Add(inputCode);
                                    character.CharacterHandler.SendMessage(Service.ShowInput("Giftcode Ngọc Rồng", inputGiftcode));
                                    character.TypeInput = 1;
                                    character.ShopId = npcId;
                                    break;
                                }
                            case 1:
                                // bang xep hang
                                character.CharacterHandler.SendMessage(Server.Gi().BangXepHang.ListTopInfoPower());
                                break;
                            case 2:
                                if (character.AllDiamond() < 2000000)
                                {
                                    character.PlusDiamond(200000000);
                                    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Đủ rồi, tham lam vừa thôi "));
                                }
                                break;
                            // case 3:
                            //    if (character.Disciple != null || DiscipleDB.IsAlreadyExist(-character.Id))
                            //    {
                            //        character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận đệ tử rồi !"));
                            //        return;
                            //    }
                            //    CreatePetNormal(character);
                            //    character.CharacterHandler.SendMessage(Service.ServerMessage("Chúc mừng bạn vừa thu nhận được đệ tử !"));
                            //    break;
                            case 3:
                                var text = $"|2|Bảng giá quy đổi hũ vàng";
                                var texts = new List<string>();
                                foreach (var textt in DataCache.Exchange)
                                {
                                    text += $"{ServerUtils.Color("red")}{ServerUtils.GetMoneyParse((textt[0]))}đ - {ServerUtils.GetMoneyParse(textt[1])} hũ vàng";
                                    texts.Add($"Quy đổi\n{ServerUtils.GetMoneyParse(textt[0])}đ");
                                }
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, text, texts, character.InfoChar.Gender));
                                character.TypeMenu = 1;
                                break;
                            case 4:
                                var inputChangePass = new List<InputBox>();
                                var inputPassNow = new InputBox()
                                {
                                    Name = "Nhập mật khẩu hiện tại",
                                    Type = 1,
                                };
                                inputChangePass.Add(inputPassNow);
                                var inputPassChange = new InputBox()
                                {
                                    Name = "Nhập mật khẩu muốn thay đổi",
                                    Type = 1,
                                };
                                inputChangePass.Add(inputPassChange);
                                var inputConfirmPassChange = new InputBox()
                                {
                                    Name = "Xác nhận mật khẩu muốn thay đổi",
                                    Type = 1,
                                };
                                inputChangePass.Add(inputConfirmPassChange);
                                character.CharacterHandler.SendMessage(Service.ShowInput("Đổi mật khẩu", inputChangePass));
                                character.TypeInput = 21;
                                break;
                            case 5:
                                if (UserDB.isActive(character.Player))
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiSay(5, "Bạn đã mở thành viên rồi"));
                                    return;
                                }
                                if (UserDB.GetVND(character.Player) < 10000)
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiSay(5, "Cần có 10k VND trong tài khoản để mở thành viên"));
                                    return;
                                }
                                character.Player.IsActive = true;
                                    //character.CharacterHandler.AddItemToBag(true, ItemCache.GetItemDefault(457, 500));
                                    //character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được 500 thỏi vàng"));
                                    UserDB.MineVND(character.Player, 10000);
                                    UserDB.Active(character.Player);
                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Kích hoạt tài khoản thành công !, vui lòng thoát ra vào lại để có hiệu lực"));
                                    
                                
                                
                                break;
                        }
                        break;
                    }
                case 1:
                    var money = UserDB.GetVND(character.Player);
                    var moneyExchange = DataCache.Exchange[select][0];
                    var goldExchange = DataCache.Exchange[select][1];
                   if (money < moneyExchange)
                    {
                        character.CharacterHandler.SendMessage(Service.OpenUiSay(5, "Bạn không có đủ tiền để quy đổi, vui lòng nạp thêm"));
                        return;
                    }
                    UserDB.MineVND(character.Player,(int) moneyExchange);
                    var gold = ItemCache.GetItemDefault(1549, (int)goldExchange);
                    character.CharacterHandler.AddItemToBag(true, gold);
                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                    character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã quy đổi thành công " + ServerUtils.GetMoney((int)goldExchange) + " hũ vàng"));
                        break;
                    
                // Các case nạp thẻ
                case 2://Chọn danh sách loại thẻ nạp
                    {
                        switch (select)
                        {
                            case 0:
                                {
                                    character.NapTheTemp.LoaiThe = "VIETTEL";
                                    break;
                                }
                            case 1:
                                {
                                    character.NapTheTemp.LoaiThe = "VINAPHONE";
                                    break;
                                }
                            case 2:
                                {
                                    character.NapTheTemp.LoaiThe = "MOBIFONE";
                                    break;
                                }
                            case 3:
                                {
                                    character.NapTheTemp.LoaiThe = "ZING";
                                    break;
                                }
                            default:
                                {
                                    character.NapTheTemp.LoaiThe = "";
                                    return;
                                }
                        }
                        // Chọn mệnh giá
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextNapThe[1], MenuNpc.Gi().MenuNapThe[1], character.InfoChar.Gender));
                        character.TypeMenu = 3;
                        character.MenuPage = 0;//Page 0
                        break;
                    }
                case 3://Chọn mệnh giá thẻ nạp
                    {
                        switch (character.MenuPage)
                        {
                            case 0://page 0
                                {
                                    switch (select)
                                    {
                                        case 0:
                                            {
                                                //10k
                                                character.NapTheTemp.MenhGia = 10000;
                                                break;
                                            }
                                        case 1:
                                            {
                                                //20k
                                                character.NapTheTemp.MenhGia = 20000;
                                                break;
                                            }
                                        case 2:
                                            {
                                                //30k
                                                character.NapTheTemp.MenhGia = 30000;
                                                break;
                                            }
                                        case 3:
                                            {
                                                //50k
                                                character.NapTheTemp.MenhGia = 50000;
                                                break;
                                            }
                                        case 4:
                                            {
                                                //Mệnh giá khác
                                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextNapThe[1], MenuNpc.Gi().MenuNapThe[2], character.InfoChar.Gender));
                                                character.TypeMenu = 3;
                                                character.MenuPage = 1;//Page 1
                                                return;
                                            }
                                        default:
                                            {
                                                return;
                                            }
                                    }
                                    break;
                                }
                            case 1:
                                {
                                    switch (select)
                                    {
                                        case 0:
                                            {
                                                //100k
                                                character.NapTheTemp.MenhGia = 100000;
                                                break;
                                            }
                                        case 1:
                                            {
                                                //200k
                                                character.NapTheTemp.MenhGia = 200000;
                                                break;
                                            }
                                        case 2:
                                            {
                                                //300k
                                                character.NapTheTemp.MenhGia = 300000;
                                                break;
                                            }
                                        case 3:
                                            {
                                                //500k
                                                character.NapTheTemp.MenhGia = 500000;
                                                break;
                                            }
                                        case 4:
                                            {
                                                //1000k
                                                character.NapTheTemp.MenhGia = 1000000;
                                                break;
                                            }
                                        default:
                                            {
                                                return;
                                            }
                                    }
                                    break;
                                }
                        }
                        // Hiển thị menu input nhập seri và số thẻ
                        var inputNapThe = new List<InputBox>();
                        var inputSeri = new InputBox() {
                            Name = TextServer.gI().INPUT_SERI_THE,
                            Type = 0,
                        };
                        inputNapThe.Add(inputSeri);
                        var inputPin = new InputBox() {
                            Name = TextServer.gI().INPUT_PIN_THE,
                            Type = 0,
                        };
                        inputNapThe.Add(inputPin);
                        character.CharacterHandler.SendMessage(Service.ShowInput(TextServer.gI().NHAP_TT_THE, inputNapThe));
                        character.TypeInput = 0;
                        break;
                    }
            }
        }
        public static void ConfrimLiTieuNuong(Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                case 0:

                    break;
                case 1:
                    break;
            }
        }
        private static void ConfirmMeo(Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                case 28:
                    {
                        var idopt = -1;
                        switch (character.InfoChar.Gender, select)
                        {
                            case (0, 0):
                                {
                                    idopt = 129;
                                    break;
                                }
                            case (0, 1):
                                {
                                    idopt = 128;
                                    break;
                                }
                            case (0, 2):
                                {
                                    idopt = 127;
                                    break;
                                }
                            case (1, 0):
                                {
                                    idopt = 130;
                                    break;
                                }
                            case (1, 1):
                                {
                                    idopt = 131;
                                    break;
                                }
                            case (1, 2):
                                {
                                    idopt = 132;
                                    break;
                                }
                            
                            case (2, 0):
                                {
                                    idopt = 133;
                                    break;
                                }
                            case (2, 1):
                                {
                                    idopt = 134;
                                    break;
                                }
                            case (2, 2):
                                { 
                                    idopt = 135;
                                    break;
                                }

                        }
                        if (idopt == -1) return;
                        var items = new List<List<short>> { new List<short> { 0, 6, 12, 21, 27 }, new List<short>() { 1, 7, 12, 22, 28 }, new List<short>() { 2, 8, 12, 23, 29 } };
                        for (int i = 0; i < items[character.InfoChar.Gender].Count; i++)
                        {
                            var IdItem = items[character.InfoChar.Gender][i];
                            var item = ItemCache.GetItemDefault(IdItem);
                            var idSKH = idopt;
                            item.Options.Add(new OptionItem()
                            {
                                Id = idSKH,
                                Param = 0,
                            });
                            item.Options.Add(new OptionItem()
                            {
                                Id = GetSKHDescOption(idSKH),
                                Param = 0,
                            });
                            item.Options.Add(new OptionItem()
                            {
                                Id = 107,
                                Param =9,
                            });
                            item.Options.Add(new OptionItem()
                            {
                                Id = 30,
                                Param = 0,
                            });
                            character.CharacterHandler.AddItemToBag(false, item);
                        }
                        character.CharacterHandler.RemoveItemBagById(1290, 1);
                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                        character.CharacterHandler.SendMessage(Service.ServerMessage("Nhận quà thành công"));


                    }
                    break;
                case 27:
                    switch (select)
                    {
                        case 0:
                            var oldDisciple = character.Disciple;
                            oldDisciple = new Disciple();
                            oldDisciple.CreatePet(character, 3, character.InfoChar.Gender);
                            oldDisciple.Player = character.Player;
                            oldDisciple.CharacterHandler.SetUpInfo();
                            character.Disciple = oldDisciple;
                            DiscipleDB.Update(oldDisciple);
                            break;
                    }
                    break;
                case 29:
                    switch (select)
                    {
                        case 0:
                            var oldDisciple = character.Disciple;
                            oldDisciple = new Disciple();
                            oldDisciple.CreatePet(character, 4, character.InfoChar.Gender);
                            oldDisciple.Player = character.Player;
                            oldDisciple.CharacterHandler.SetUpInfo();
                            character.Disciple = oldDisciple;
                            DiscipleDB.Update(oldDisciple);
                            break;
                    }
                    break;
                //Thách đấu
                case 0:
                    {
                       
                        var characterChallenge = (Model.Character.Character)character.Zone.ZoneHandler.GetCharacter(character.Challenge.PlayerChallengeID);
                        if (characterChallenge == null) character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_FOUND_CHAR_IN_MAP));
                        else
                        {
                            switch (select)
                            {
                                //1,000 vàng
                                case 0:
                                    {
                                        if (character.InfoChar.Gold < 1000)
                                        {
                                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_GOLD));
                                            character.Challenge.Gold = 0;
                                        }
                                        else
                                        {
                                            var text = string.Format(TextServer.gI().SEND_TEST, character.Name, ServerUtils.GetPower(character.InfoChar.Potential), 1000);
                                            character.Challenge.Gold = characterChallenge.Challenge.Gold = 1000;
                                            characterChallenge.CharacterHandler.SendMessage(Service.PlayerVsPLayer(3, character.Id, 1000, text));
                                        }
                                        break;
                                    }
                                //10,000 vàng
                                case 1:
                                    {
                                        if (character.InfoChar.Gold < 10000)
                                        {
                                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_GOLD));
                                            character.Challenge.Gold = 0;
                                        }
                                        else
                                        {
                                            var text = string.Format(TextServer.gI().SEND_TEST, character.Name, ServerUtils.GetPower(character.InfoChar.Potential), 10000);
                                            character.Challenge.Gold = characterChallenge.Challenge.Gold = 10000;
                                            characterChallenge.CharacterHandler.SendMessage(Service.PlayerVsPLayer(3, character.Id, 10000, text));
                                        }
                                        break;
                                    }
                                //100,000 vàng
                                case 2:
                                    {
                                        if (character.InfoChar.Gold < 100000)
                                        {
                                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_GOLD));
                                            character.Challenge.Gold = 0;
                                        }
                                        else
                                        {
                                            var text = string.Format(TextServer.gI().SEND_TEST, character.Name, ServerUtils.GetPower(character.InfoChar.Potential), 100000);
                                            character.Challenge.Gold = characterChallenge.Challenge.Gold = 100000;
                                            characterChallenge.CharacterHandler.SendMessage(Service.PlayerVsPLayer(3, character.Id, 100000, text));
                                        }
                                        break;
                                    }
                            }
                        }
                        break;
                    }
                //Nâng cấp đậu
                case 1:
                    {
                        var magicTree = MagicTreeManager.Get(character.Id);
                        if (magicTree == null || select == 1) return;
                        lock (magicTree)
                        {
                            var levelTree = magicTree.Level;
                            var gold = DataCache.UpgradeDauThanGold[levelTree - 1];
                            if (character.InfoChar.Gold < gold)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_GOLD));
                                return;
                            }
                            character.MineGold(gold);
                            magicTree.IsUpdate = true;
                            magicTree.Seconds = DataCache.UpgradeDauThanTime[levelTree - 1] + ServerUtils.CurrentTimeMillis();
                            magicTree.MagicTreeHandler.HandleNgoc();
                            character.CharacterHandler.SendMessage(Service.MagicTree0(magicTree));
                            character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                        }
                        break;
                    }
                //Huỷ nâng cấp đậu
                case 2:
                    {
                        var magicTree = MagicTreeManager.Get(character.Id);
                        if (magicTree == null || select == 1) return;
                        lock (magicTree)
                        {
                            var levelTree = magicTree.Level;
                            var gold = DataCache.UpgradeDauThanGold[levelTree - 1];
                            character.PlusGold(gold / 2);
                            magicTree.IsUpdate = false;
                            if (magicTree.Peas == magicTree.MaxPea)
                            {
                                magicTree.Seconds = 0;
                            }
                            else
                            {
                                magicTree.Seconds = 60000 * magicTree.Level + ServerUtils.CurrentTimeMillis();
                            }
                            magicTree.MagicTreeHandler.HandleNgoc();
                            character.CharacterHandler.SendMessage(Service.MagicTree0(magicTree));
                            character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                        }
                        break;
                    }
                //Kết bạn
                case 3:
                    {
                        if (select != 0 || character.FriendTemp == null) return;
                        character.Friends.Add(character.FriendTemp);
                        var @char = ClientManager.Gi().GetCharacter(character.FriendTemp.Id);
                        @char?.CharacterHandler.SendMessage(Service.WorldChat((Character)character, string.Format(TextServer.gI().ADD_FRIEND, character.Name, @char.Name), 1));
                        character.CharacterHandler.SendMessage(Service.ServerMessage(string.Format(TextServer.gI().ADD_FRIEND_2, character.FriendTemp.Name)));
                        character.FriendTemp = null;
                        break;
                    }
                //Xoá kết bạn
                case 4:
                    {
                        if (select != 0 || character.FriendTemp == null) return;
                        character.Friends.RemoveAll(friend => friend.Id == character.FriendTemp.Id);
                        character.CharacterHandler.SendMessage(Service.ListFriend2(character.FriendTemp.Id));
                        character.CharacterHandler.SendMessage(Service.ServerMessage(string.Format(TextServer.gI().FRIEND_DELETE, character.FriendTemp.Name)));
                        character.FriendTemp = null;
                        break;
                    }
                //Dịch chuyển tới người chơi
                case 5:
                    {
                        if (select != 0 || character.EnemyTemp == null) return;
                        var charCheck = (Character)ClientManager.Gi().GetCharacter(character.EnemyTemp.Id);
                        if (charCheck == null)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().USER_OFFLINE));
                        }
                        
                        else
                        {
                            var mapId = character.InfoChar.MapId;
                            switch (charCheck.InfoChar.MapId)
                            {
                                case 155:
                                    if (!character.InfoSet.IsFullSetHuyDiet)
                                    {
                                        character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().TELEPORT_ERROR));
                                        return;
                                    }
                                    break;
                                case 161 or 162 or 163 or 160:
                                    if (character.CharacterHandler.GetItemBagById(992) == null)
                                    {
                                        character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().TELEPORT_ERROR));
                                        return;
                                    }
                                    break;
                                case int i when DataCache.IdMapCold.Contains(i):
                                    if (character.InfoTask.Id < 30)
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiSay(5, "Bạn phải làm [nhiệm vụ Cuộc dạo chơi của Xên] để qua khu vực này", false, character.InfoChar.Gender));
                                       
                                        return;
                                    }
                                    break;
                                case int i when DataCache.IdMapKarin.Contains(i):
                                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().TELEPORT_ERROR));
                                    return;
                                case int i when DataCache.IdMapSpecial.Contains(i):
                                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().TELEPORT_ERROR));
                                    return;

                            }
                            
                                var mapEnemy = MapManager.Get(charCheck.Zone.Map.Id);
                                var mapNow = MapManager.Get(mapId);
                                mapNow.OutZone(character);
                                character.InfoChar.X = charCheck.InfoChar.X;
                                mapEnemy.JoinZone((Character)character, charCheck.Zone.Id);
                                
                                character.EnemyTemp = null;
                                break;
                            
                        }
                        break;
                    }
                //Rời bang
                case 6:
                    {
                        if (select != 0) return;
                        var clan = ClanManager.Get(character.ClanId);
                        if (clan == null) return;
                        var me = clan.ClanHandler.GetMember(character.Id);
                        if (clan.ClanHandler.RemoveMember(me.Id))
                        {
                            var lastMess = clan.Messages.LastOrDefault();
                            var id = lastMess != null ? lastMess.Id + 1 : 0;
                            clan.ClanHandler.Chat(new ClanMessage()
                            {
                                Type = 0,
                                Id = id,
                                PlayerId = -1,
                                PlayerName = "Thông báo",
                                Role = 0,
                                Time = ServerUtils.CurrentTimeSecond() - 1000000000,
                                Text = string.Format(TextServer.gI().LEAVE_CLAN, me.Name),
                                Color = 1,
                                NewMessage = true,
                            });
                            character.ClanId = -1;
                            character.InfoChar.Bag = -1;
                            clan.ClanHandler.SendUpdateClan();
                            if (character.InfoChar.PhukienPart == -1) character.CharacterHandler.SendZoneMessage(Service.SendImageBag(character.Id, -1));
                            character.CharacterHandler.SendMessage(Service.GetImageBag(null));
                            character.CharacterHandler.SendMessage(Service.MyClanInfo());
                            character.CharacterHandler.SendZoneMessage(Service.UpdateClanId(character.Id, -1));
                            clan.ClanHandler.UpdateClanId();
                            CharacterDB.Update(character);
                            ClanDB.Update(clan);
                        }
                        break;
                    }
                //Xoá thù địch
                case 7:
                    {
                        if (select != 0 || character.EnemyTemp == null) return;
                        character.Enemies.RemoveAll(enemy => enemy.Id == character.EnemyTemp.Id);
                        character.CharacterHandler.SendMessage(Service.ServerMessage(string.Format(TextServer.gI().ENEMY_DELETE, character.EnemyTemp.Name)));
                        character.EnemyTemp = null;
                        break;
                    }
                //Đồng ý kích hoạt mã
                case 8:
                    {
                        if (select != 0 || character.InfoChar.LockInventory.PassTemp == -1) return;
                        if (character.InfoChar.Gold < 50000)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_GOLD));
                            return;
                        }
                        character.MineGold(50000);
                        character.InfoChar.LockInventory.IsLock = true;
                        character.InfoChar.LockInventory.Pass = character.InfoChar.LockInventory.PassTemp;
                        character.InfoChar.LockInventory.PassTemp = -1;
                        character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().ACTIVE_LOCK_INVENTORY));
                        character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                        break;
                    }
                //Mở/Khoá rương
                case 9:
                    {
                        if (select != 0 || character.InfoChar.LockInventory.Pass == -1) return;
                        character.InfoChar.LockInventory.IsLock = !character.InfoChar.LockInventory.IsLock;
                        character.CharacterHandler.SendMessage(character.InfoChar.LockInventory.IsLock
                            ? Service.ServerMessage(TextServer.gI().SUCCESS_LOCK_INVENTORY)
                            : Service.ServerMessage(TextServer.gI().UNACTIVE_LOCK_INVENTORY));
                        break;
                    }
                // Nội tại
                case 10:
                    {
                        switch (select)
                        {
                            case 0: //Xem tất cả nội tại
                                {
                                    character.CharacterHandler.SendMessage(Service.SpeacialSkill(character, 1));
                                    break;
                                }
                            case 1: //Mở nội tại VIP
                                {
                                    if (character.InfoChar.Power < 10000000000)
                                    {
                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Yêu cầu sức mạnh phải đạt 10 tỉ"));
                                        break;
                                    }
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, string.Format(MenuNpc.Gi().TextNoiTai[1], 100),
                                            MenuNpc.Gi().MenuNoiTai[1], character.InfoChar.Gender));
                                    character.TypeMenu = 11;
                                    break;
                                }
                            case 2: //Mở nội tại NORMAL
                                {
                                    if (character.InfoChar.Power < 10000000000)
                                    {
                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Yêu cầu sức mạnh phải đạt 10 tỉ"));
                                        break;
                                    }
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, string.Format(MenuNpc.Gi().TextNoiTai[2], ServerUtils.GetMoney(50000000)),
                                            MenuNpc.Gi().MenuNoiTai[2], character.InfoChar.Gender));
                                    character.TypeMenu = 12;
                                    break;
                                }

                        }
                        break;
                    }
                case 11://mở nội tại VIP
                    {
                        switch (select)
                        {
                            case 0:

                                var specialSkillTemplate = Cache.Gi().SPECIAL_SKILL_TEMPLATES.FirstOrDefault(s => s.Key == character.InfoChar.Gender).Value;
                                if (specialSkillTemplate == null) return;
                                if (character.AllDiamond() < 100)
                                {
                                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_DIAMOND));
                                    return;
                                }
                                character.MineDiamond(100);

                                int RandomIndex = ServerUtils.RandomNumber(specialSkillTemplate.Count);
                                SpecialSkillTemplate SkillRandom = specialSkillTemplate[RandomIndex];

                                int ValueRandom = 0;

                               
                                    ValueRandom = ServerUtils.RandomNumber(SkillRandom.Min+10, SkillRandom.Max + 1);

                                string InfoRandom = SkillRandom.InfoFormat.Replace("#", ValueRandom + "");

                                character.SpecialSkill.Id = SkillRandom.Id;
                                character.SpecialSkill.Info = InfoRandom;
                                character.SpecialSkill.Img = SkillRandom.Img;
                                character.SpecialSkill.SkillId = SkillRandom.SkillId;
                                character.SpecialSkill.Value = ValueRandom;
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã mở nội tại " + InfoRandom));
                                character.CharacterHandler.SendMessage(Service.SpeacialSkill(character, 0));
                                character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                                break;
                        }
                        break;
                    }
                case 12://mở nội tại NORMAL
                    {
                        switch (select) {
                            case 0:
                     var specialSkillTemplate = Cache.Gi().SPECIAL_SKILL_TEMPLATES.FirstOrDefault(s => s.Key == character.InfoChar.Gender).Value;
                        if (specialSkillTemplate == null) return;
                        if (character.InfoChar.Gold < 50000000) {
                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_GOLD));
                            return;
                        }
                        character.MineGold(50000000);

                        int RandomIndex = ServerUtils.RandomNumber(specialSkillTemplate.Count);
                        SpecialSkillTemplate SkillRandom = specialSkillTemplate[RandomIndex];

                        int ValueRandom = 0;

                        ValueRandom = ServerUtils.RandomNumber(SkillRandom.Min, SkillRandom.Max + 1);
                                
                                String    InfoRandom = SkillRandom.InfoFormat.Replace("#", ValueRandom + "");
                                

                        character.SpecialSkill.Id = SkillRandom.Id;
                        character.SpecialSkill.Info = InfoRandom;
                        character.SpecialSkill.Img = SkillRandom.Img;
                        character.SpecialSkill.SkillId = SkillRandom.SkillId;
                        character.SpecialSkill.Value = ValueRandom;
                        character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã mở nội tại " + InfoRandom));
                        character.CharacterHandler.SendMessage(Service.SpeacialSkill(character, 0));
                        character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                        break;
                    }
                        break;
            }
                case 13://hộp siêu phàm
                    {
                        switch (select)
                        {
                            case 0:
                                {
                                    var ListDoThanLinh = new List<int> { 555, 556, 561, 562, 563 };
                                    var ListOption = new List<int> { 47, 6, 0, 7, 12 };
                                    var minParam = new List<int> { 730, 36000, 3600, 36000, 12 };
                                    var maxParam = new List<int> { 1200, 69999, 7000, 59000, 18 };
                                    for (int i2 = 0; i2 < ListDoThanLinh.Count; i2++)
                                    {
                                        var item = ItemCache.GetItemDefault((short)(ListDoThanLinh[i2]));
                                        // var ItemTemp = ItemCache.ItemTemplate(item.Id);
                                        // var option = item.Options.FirstOrDefault(i => i.Id == ListOption[ItemTemp.Type]);
                                        //option.Param = ServerUtils.RandomNumber(minParam[ItemTemp.Type], maxParam[ItemTemp.Type]);
                                        character.CharacterHandler.AddItemToBag(false, item);
                                        //  character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + ItemTemp.Name));
                                    }
                                    character.CharacterHandler.RemoveItemBagById(1269, 1);
                                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                                }
                                break;
                            case 1:
                                {
                                    var ListDoThanLinh = new List<int> { 557, 558, 561, 564, 565 };

                                    var ListOption = new List<int> { 47, 6, 0, 7, 12 };
                                    var minParam = new List<int> { 730, 36000, 3600, 36000, 12 };
                                    var maxParam = new List<int> { 1200, 69999, 7000, 59000, 18 };
                                    for (int i2 = 0; i2 < ListDoThanLinh.Count; i2++)
                                    {
                                        var item = ItemCache.GetItemDefault((short)(ListDoThanLinh[i2]));
                                        // var ItemTemp = ItemCache.ItemTemplate(item.Id);
                                        // var option = item.Options.FirstOrDefault(i => i.Id == ListOption[ItemTemp.Type]);
                                        //option.Param = ServerUtils.RandomNumber(minParam[ItemTemp.Type], maxParam[ItemTemp.Type]);
                                        character.CharacterHandler.AddItemToBag(false, item);
                                        //  character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + ItemTemp.Name));
                                    }
                                    character.CharacterHandler.RemoveItemBagById(1269, 1);
                                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                                }
                                break;
                            case 2:
                                {
                                    var ListDoThanLinh = new List<int> { 559, 560, 561, 566, 567 };

                                    var ListOption = new List<int> { 47, 6, 0, 7, 12 };
                                    var minParam = new List<int> { 730, 36000, 3600, 36000, 12 };
                                    var maxParam = new List<int> { 1200, 69999, 7000, 59000, 18 };
                                    for (int i2 = 0; i2 < ListDoThanLinh.Count; i2++)
                                    {
                                        var item = ItemCache.GetItemDefault((short)(ListDoThanLinh[i2]));
                                       // var ItemTemp = ItemCache.ItemTemplate(item.Id);
                                       // var option = item.Options.FirstOrDefault(i => i.Id == ListOption[ItemTemp.Type]);
                                        //option.Param = ServerUtils.RandomNumber(minParam[ItemTemp.Type], maxParam[ItemTemp.Type]);
                                        character.CharacterHandler.AddItemToBag(false, item);
                                      //  character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + ItemTemp.Name));
                                    }
                                    character.CharacterHandler.RemoveItemBagById(1269, 1);
                                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                                    break;
                                }
                        
                        }
                    }
                    break;
                case 14:// luyện tập
                    switch (select)
                    {
                        case 0:// ở lại đây
                            break;
                        case 1:// về chỗ cũ
                            var mapNow = MapManager.Get(character.InfoChar.MapId);
                            var mapOld = MapManager.Get(character.Id);
                            mapNow.OutZone(character);
                            mapOld.Zones[character.InfoChar.ZoneId].ZoneHandler.SendMessage(Service.SendTeleport(character.Id, character.TypeTeleport));
                            mapOld.JoinZone(character, character.InfoChar.ZoneId, true, true, character.TypeTeleport);
                            break;
                        
                    }
                    break;
                case 15:// mua lại cải trang
                    {
                        switch (select)
                        {   
                            case 0://đồng ý
                                var player = character.Zone.ZoneHandler.GetCharacter(character.IdPlayerAction);
                                if (player == null) break;
                                var item = player.ItemBody[5];
                                if (item == null)
                                {
                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Người chơi không mặc cải trang này nữa"));
                                    break;
                                }
                                var itemTemplate = ItemCache.ItemTemplate(item.Id);
                                var itemCost = itemTemplate.SaleCoin - (itemTemplate.SaleCoin * 5 / 100);
                                if (character.CharacterHandler.GetThoiVangInBag() < itemCost)
                                {
                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn không có đủ thỏi vàng"));
                                    break;
                                }
                                var goldRecieve = itemTemplate.SaleCoin / 5;
                                character.CharacterHandler.RemoveItemBagById(457, itemCost);
                                player.CharacterHandler.AddItemToBag(true, ItemCache.GetItemDefault(457 ,goldRecieve));
                                character.CharacterHandler.AddItemToBag(false, item, "Mua lại từ người khác");
                                character.CharacterHandler.SendMessage(Service.SendBag(character));
                                player.CharacterHandler.SendMessage(Service.SendBag(player));
                                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                character.CharacterHandler.SendMessage(Service.ServerMessage($"Bạn đã mua thành công {itemTemplate.Name} từ {player.Name}"));
                                player.CharacterHandler.SendMessage(Service.ServerMessage($"Bạn đã nhận được {goldRecieve} thỏi vàng"));
                                break;
                            case 1://từ chối
                                break;
                        }
                        break;
                    }
                case 16://Ban Acc
                    {
                        switch (select)
                        {
                            case 0://đồng ý
                                var player = character.Zone.ZoneHandler.GetCharacter(character.IdPlayerAction);
                                if (player == null) break;
                                UserDB.BanUser(player.Id);
                                ClientManager.Gi().KickSession(player.Player.Session);
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã ban nhân vật " + player.Name + " vĩnh viễn"));
                                ServerUtils.WriteLog("ban.txt", $"{character.Name} đã ban {player.Name}");
                                break;
                            case 1://từ chối
                                break;
                        }
                        break;
                    }
                case 17://kéo búa bao
                    {
                        break;
                    }
                case 18:// xin làm đệ tử
                    {
                        switch (select)
                        {
                            case 0://đồng ý
                                var player = character.Zone.ZoneHandler.GetCharacter(character.IdPlayerAction);
                                if (player == null) break;
                                var playerReal = (Model.Character.Character)player;
                                playerReal.IdPlayerAction = character.Id;
                                playerReal.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, string.Format(MenuNpc.Gi().TextMeo[16], character.Name), MenuNpc.Gi().MenuMeo[1], character.InfoChar.Gender));
                                playerReal.TypeMenu = 19;
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, string.Format(MenuNpc.Gi().TextMeo[15], player.Name), MenuNpc.Gi().MenuMeo[4], character.InfoChar.Gender));
                                character.TypeMenu = 99;
                                break;
                            case 1://từ chối
                                break;
                        }
                        break;
                    }
                case 19:// nhận đệ tử
                    {
                        switch (select)
                        {
                            case 0://đồng ý
                                {
                                    var player = character.Zone.ZoneHandler.GetCharacter(character.IdPlayerAction);
                                    if (player == null) break;
                                    var playerReal = (Model.Character.Character)player;
                                    playerReal.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, $"{character.Name} đã thu nhận bạn làm đệ tử", MenuNpc.Gi().MenuMeo[4], character.InfoChar.Gender));
                                    playerReal.TypeMenu = 99;
                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã thu nhận " + player.Name + " làm đệ tử"));
                                    character.InfoChar.DiscipleId = playerReal.Id;
                                    playerReal.InfoChar.MasterId = character.Id;
                                    break;
                                }
                            case 1://từ chối
                                {
                                    var player = character.Zone.ZoneHandler.GetCharacter(character.IdPlayerAction);
                                    if (player == null) break;
                                    var playerReal = (Model.Character.Character)player;
                                    playerReal.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, $"{character.Name} đã từ chối thu nhận bạn làm đệ tử", MenuNpc.Gi().MenuMeo[4], character.InfoChar.Gender));
                                    playerReal.TypeMenu = 99;
                                    break;
                                }
                        }
                        break;
                    }
                case 20://chọn gói hộp quà
                    {
                        switch (select)
                        {
                            case 0:
                                SuKien8Thang3.SelectGoiHopQuaNheNhang(character);
                                break;
                            case 1:
                                SuKien8Thang3.SelectGoiHopQuaChinChu(character);
                                break;
                        }
                        break;
                    }
                case 21:
                    {
                        switch (character.TypeDoiThuong, select)
                        {
                            case ((int)DoiThuongType.HOP_QUA_NHE_NHANG, 0):
                                character.CharacterHandler.RemoveItemBagById(1556, 30, "Sự kiện 8 tháng 3");
                                character.CharacterHandler.RemoveItemBagById(1555, 5, "Sự kiện 8 tháng 3");
                                character.CharacterHandler.RemoveItemBagById(1554, 1, "Sự kiện 8 tháng 3");
                                character.CharacterHandler.AddItemToBag(true, ItemCache.GetItemDefault(1558, 1), "Sự kiện 8 tháng 3");
                                character.CharacterHandler.SendMessage(Service.SendBag(character));
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được Hộp quà nhẹ nhàng"));
                                break;
                            case ((int)DoiThuongType.HOP_QUA_CHIN_CHU, 0):
                                character.CharacterHandler.RemoveItemBagById(1556, 30, "Sự kiện 8 tháng 3");
                                character.CharacterHandler.RemoveItemBagById(1555, 5, "Sự kiện 8 tháng 3");
                                character.CharacterHandler.RemoveItemBagById(1554, 1, "Sự kiện 8 tháng 3");
                                character.CharacterHandler.RemoveItemBagById(1557, 1, "Sự kiện 8 tháng 3");
                                character.CharacterHandler.AddItemToBag(true, ItemCache.GetItemDefault(1559, 1), "Sự kiện 8 tháng 3");
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được Hộp quà chỉn chu"));
                                break;
                        }
                        break;
                    }
                case 22:
                    switch (select)
                    {
                        case 0:

                            break;
                    }
                    break;
            }
    }
        private static void ConfirmMayDo(Character character, short npcId, int select)
        {
            switch (select)
            {
                case 0:
                    character.MineDiamond(10);
                    MapManager.JoinMap(character, Init.NamecBalls[0].MapId, Init.NamecBalls[0].ZoneId, false, false, 0);
                    break;
                case 1:
                    character.MineGold(100000);
                    MapManager.JoinMap(character, Init.NamecBalls[0].MapId, Init.NamecBalls[0].ZoneId, false, false, 0);
                    break;
                case 2: 
                    break;
            }
        }
        private static void ConfirmBumma(Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                case 0:
                {
                    if (character.InfoChar.Gender != 0)
                    {
                        character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, MenuNpc.Gi().TextBumma[1]));
                    }
                    else if (select == 0)
                    {
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextBumma[0], MenuNpc.Gi().MenuShopDistrict[1], character.InfoChar.Gender));
                        character.TypeMenu = 1;
                    }
                        else if (select == 1)
                        {
                            var idShop = 3333;
                            character.CharacterHandler.SendMessage(Service.Shop(character, 0, 3333));
                            character.ShopId = idShop;
                            character.TypeShop = 0;
                        }
                        break;
                }
                //Show shop
                case 1:
                {
                    if(select == 1) return;
                    var shopId = 12;
                    character.CharacterHandler.SendMessage(Service.Shop(character, 0, shopId));
                    character.ShopId = shopId;
                    character.TypeShop = 0;
                    break;
                }
            }
        }

        private static void ConfirmBummaTL(Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                case 0:
                {
                    if(select == 1) return;
                    var shopId = 22;
                    character.CharacterHandler.SendMessage(Service.Shop(character, 0, shopId));
                    character.ShopId = shopId;
                    character.TypeShop = 0;
                    break;
                }
            }
        }
        
        private static void ConfirmDende(Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                case 0:
                {
                    if (character.InfoChar.Gender != 1)
                    {
                        character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, MenuNpc.Gi().TextDende[1]));
                    }
                    else if (select == 0)
                    {
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextDende[0], MenuNpc.Gi().MenuShopDistrict[1], character.InfoChar.Gender));
                        character.TypeMenu = 1;
                    }else if (select == 1)
                        {
                            var idShop = 3333;
                            character.CharacterHandler.SendMessage(Service.Shop(character, 0, 3333));
                            character.ShopId = idShop;
                            character.TypeShop = 0;
                        }
                    break;
                }
                //Show shop
                case 1:
                {
                    if(select == 1) return;
                    var idShop = 13;
                    character.CharacterHandler.SendMessage(Service.Shop(character, 0, idShop));
                    character.ShopId = idShop;
                    character.TypeShop = 0;
                    break;
                }
                case 2:
                    switch (select)
                    {
                        case 0:
                            break;
                        case 1:
                            if (character.DataNgocRongNamek.AlreadyPick(character) && character.DataNgocRongNamek.DelayWish <= ServerUtils.CurrentTimeMillis())
                            {
                                if (character.ClanId == -1 || ClanManager.Get(character.ClanId) == null)
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Yêu cầu tham gia bang hội."));
                                    return;
                                }
                                if (character.Zone.ZoneHandler.GetCharacterClanHasNamecBallInMap(character.ClanId).Count < 7)
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Yêu cầu tập trung đủ 7 thành viên trong Bang hội\nMỗi thành viên phải đeo 1 viên Ngọc Rồng Namec", true, character.InfoChar.Gender));
                                    return;
                                }                               
                                character.CharacterHandler.SendZoneMessage(Service.PublicChat(character.Id, "TAKKARABUTO...POPPORUNGA...PUPIRITTOPA,Hỡi Zồng Thiên Ơii mau mau thức dậy."));
                                Rồng_Namec.gI().ShowMenu(character);
                            }else
                            {
                                var time = (character.DataNgocRongNamek.DelayWish - ServerUtils.CurrentTimeMillis())/60000;
                                if (time <= 1)
                                {
                                    time = (character.DataNgocRongNamek.DelayWish - ServerUtils.CurrentTimeMillis()) / 1000;
                                    character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, $"Ngọc bẩn quá, xin chờ em {time} giây nữa để lau bóng ngọc\ngọi Zồng mới hiển linh"));
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, $"Ngọc bẩn quá, xin chờ em {time} phút nữa để lau bóng ngọc\ngọi Zồng mới hiển linh"));

                                }
                            }
                            break;
                    }
                    break;
            }
        }
        
        private static void ConfirmAppule(Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                case 0:
                    {
                        if (character.InfoChar.Gender != 2)
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, MenuNpc.Gi().TextAppule[1]));
                        }
                        else if (select == 0)
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextAppule[0], MenuNpc.Gi().MenuShopDistrict[1], character.InfoChar.Gender));
                            character.TypeMenu = 1;
                        }
                        else if (select == 1)
                        {
                            var idShop = 3333;
                            character.CharacterHandler.SendMessage(Service.Shop(character, 0, 3333));
                            character.ShopId = idShop;
                            character.TypeShop = 0;
                        }
                        break;
                }
                //Show shop
                case 1:
                {
                    if(select == 1) return;
                    var idShop = 14;
                    character.CharacterHandler.SendMessage(Service.Shop(character, 0, idShop));
                    character.ShopId = idShop;
                    character.TypeShop = 0;
                    break;
                }
            }
        }

        private static void ConfirmBrief(Character character, short npcId, int select)
        {
            var map = MapManager.Get(character.InfoChar.MapId);
            switch (map.Id)
            {
                case 153:
                    {
                        switch (character.TypeMenu)
                        {
                            case 3:
                                switch (select)
                                {
                                    case 0:
                                        var clan = ClanManager.Get(character.ClanId);
                                        var cost = ClanManager.Get(character.ClanId).Cấp_Độ * 100;
                                        if (clan.Capsule_Bang < cost)
                                        {
                                            character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Không đủ capsule bang"));
                                            return;
                                        }
                                        clan.Capsule_Bang -= cost;
                                        clan.Cấp_Độ++;
                                        clan.Tối_đa_thành_viên++;
                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Chúc mừng bang hội của bạn đã lên cấp " + clan.Cấp_Độ));
                                        clan.ClanHandler.SendMessage(Service.MyClanInfo());
                                        ClanDB.Update(clan);
                                        break;
                                }
                                break;
                            case 2:
                                switch (select)
                                {
                                    case 0:
                                        var inputGiftcode = new List<InputBox>();
                                        var inputCode = new InputBox()
                                        {
                                            Name = "Nhập tên viết tắt",
                                            Type = 1,
                                        };
                                        inputGiftcode.Add(inputCode);
                                        character.CharacterHandler.SendMessage(Service.ShowInput("Tên viết tắt bang hội", inputGiftcode));
                                        character.TypeInput = 31;
                                        break;
                                    case 1:
                                        ClanManager.Get(character.ClanId).shortName = "CBNR";
                                        break;
                                    case 2:
                                        var clan = ClanManager.Get(character.ClanId);
                                        var cost = ClanManager.Get(character.ClanId).Cấp_Độ * 100;
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, $"Cần {cost} capsule bang [đang có {clan.Capsule_Bang} capsule bang] để nâng cấp bang hội" +
                                            $"\nlên cấp {clan.Cấp_Độ++}\n+1 tối đa số lượng thành viên\n+1 ô trống tối đa rương bang\n+Mở bán bùa bang cấp {clan.Cấp_Độ++}"
                                            , new List<string> { "Đồng ý", "Từ chối" }, character.InfoChar.Gender));
                                        character.TypeMenu = 3;
                                        break;
                                }
                                break;
                            
                            case 1:
                                {
                                    switch (select)
                                    {
                                        case 0:
                                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Ta có thể giúp gì cho bang hội của bạn?", new List<string> { "Đổi tên\ntên bang\nviết tắt", "Chọn ngẫu nhiên\ntên bang\nviết tắt", "Nâng cấp\nBang hội", "Đóng" }, character.InfoChar.Gender));
                                            character.TypeMenu = 2;
                                            break;
                                        case 2:
                                            {
                                                var map2 = MapManager.Get(character.InfoChar.MapId);
                                                var mapJoin2 = MapManager.Get(5);
                                                var zoneJoin2 = mapJoin2.GetZoneNotMaxPlayer();
                                                if (zoneJoin2!= null)
                                                {
                                                    character.CharacterHandler.SendZoneMessage(Service.SendTeleport(character.Id, character.InfoChar.Teleport));
                                                    map2.OutZone(character);
                                                    zoneJoin2.ZoneHandler.JoinZone(character, false, true, character.InfoChar.Teleport);
                                                }
                                                else
                                                {
                                                    character.CharacterHandler.SendMessage(Service.OpenUiSay(5, TextServer.gI().MAX_NUMCHARS, false, character.InfoChar.Gender));
                                                }
                                                break;
                                            }
                                    }
                                }
                                break;
                        }
                        break;
                    }
                default:

                    if (map == null) return;
                   IMap mapJoin;

                    if (map.Id == 84)
                    {
                        mapJoin = MapManager.Get(character.InfoChar.Gender + 24);
                    }
                    else
                    {
                        switch (select)
                        {
                            case 0:
                                {
                                    mapJoin = MapManager.Get(26);
                                    break;
                                }
                            case 1:
                                {
                                    mapJoin = MapManager.Get(25);
                                    break;
                                }
                            case 2:
                                {
                                    mapJoin = MapManager.Get(84);
                                    break;
                                }
                            default:
                                {
                                    return;
                                }
                        }
                    }

                    if (mapJoin == null) return;
                    var zoneJoin = mapJoin.GetZoneNotMaxPlayer();
                    if (zoneJoin != null)
                    {
                        character.CharacterHandler.SendZoneMessage(Service.SendTeleport(character.Id, character.InfoChar.Teleport));
                        map.OutZone(character);
                        zoneJoin.ZoneHandler.JoinZone(character, false, true, character.InfoChar.Teleport);
                    }
                    else
                    {
                        character.CharacterHandler.SendMessage(Service.OpenUiSay(5, TextServer.gI().MAX_NUMCHARS, false, character.InfoChar.Gender));
                    }
                    break;
            }

        }
        
        private static void ConfirmCargo(Character character, short npcId, int select)
        {
            var map = MapManager.Get(character.InfoChar.MapId);
            if (map == null) return;
           IMap mapJoin;
            switch (select)
            {
                case 0:
                {
                    mapJoin = MapManager.Get(24);
                    break;
                }
                case 1:
                {
                    mapJoin = MapManager.Get(26);
                    break;
                }
                case 2:
                {
                    mapJoin = MapManager.Get(84);
                    break;
                }
                default:
                {
                    return;
                }
            }

            if (mapJoin == null) return;
            var zoneJoin = mapJoin.GetZoneNotMaxPlayer();
            if (zoneJoin != null)
            {

                if (character.DataNgocRongNamek.AlreadyPick(character))
                {
                    var itm = new ItemMap(-1, ItemCache.GetItemDefault((short)(character.DataNgocRongNamek.IdNamekBall)));
                    itm.X = character.InfoChar.X;
                    itm.Y = character.InfoChar.Y;
                    character.Zone.ZoneHandler.LeaveItemMap(itm);
                    character.InfoChar.TypePk = 0;
                    character.DataNgocRongNamek.IdNamekBall = -1;
                    character.InfoChar.Bag = ClanManager.Get(character.ClanId) != null ? (sbyte)ClanManager.Get(character.ClanId).ImgId : (sbyte)-1;
                }
                
                character.CharacterHandler.SendZoneMessage(Service.SendTeleport(character.Id, character.InfoChar.Teleport));
                map.OutZone(character);
                zoneJoin.ZoneHandler.JoinZone(character, false, true, character.InfoChar.Teleport);
            }
            else
            {
                character.CharacterHandler.SendMessage(Service.OpenUiSay(5, TextServer.gI().MAX_NUMCHARS, false, character.InfoChar.Gender));
            }
        }
        
        private static void ConfirmCui(Character character, short npcId, int select)
        {
            var map = MapManager.Get(character.InfoChar.MapId);
            if (map == null) return;
           IMap mapJoin = null;
            switch (map.Id)
            {
                
               
                case 19:
                    {
                        if (HelpMission.Check(character))
                        {
                            if (select == 0)
                            {
                                mapJoin = MapManager.Get(68);
                            }
                            if (select == 2)
                            {
                                HelpMission.HoTroNhiemVu(character, character.InfoTask.Index);
                            }   
                        }
                        else
                        {
                            if (select == 0)
                            {
                                if (character.InfoTask.Id > 28)
                                {
                                    mapJoin = MapManager.Get(109);
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiSay(5, "Bạn phải hoàn thành nhiệm vụ để mở khóa chức năng này"));
                                }
                                break;
                            }
                            if (@select == 1)
                            {
                                mapJoin = MapManager.Get(68);
                            }
                        }
                    break;
                }
                case 68:
                {
                    if (@select == 0)
                    {
                        mapJoin = MapManager.Get(19);
                    }

                    break;
                }
                default:
                {
                    switch (@select)
                    {
                        case 0:
                        {
                            mapJoin = MapManager.Get(24);
                            break;
                        }
                        case 1:
                        {
                            mapJoin = MapManager.Get(25);
                            break;
                        }
                        case 2:
                        {
                            mapJoin = MapManager.Get(84);
                            break;
                        }
                        default:
                        {
                            return;
                        }
                    }

                    break;
                }
            }

            if (mapJoin == null) return;
            var zoneJoin = mapJoin.GetZoneNotMaxPlayer();
            if (zoneJoin != null)
            {
                character.CharacterHandler.SendZoneMessage(Service.SendTeleport(character.Id, character.InfoChar.Teleport));
                map.OutZone(character);
                zoneJoin.ZoneHandler.JoinZone(character, false, true, character.InfoChar.Teleport);
            }
            else
            {
                character.CharacterHandler.SendMessage(Service.OpenUiSay(5, TextServer.gI().MAX_NUMCHARS, false, character.InfoChar.Gender));
            }
        }
        private static void HandlerGhepTrangBiThanLinh(Character character, int typeItem, int gender, int id = 0)
        {
            switch (typeItem)
            {
                case 0:
                    {
                        switch (gender)
                        {
                            case 0:
                                {
                                    id = 650;
                                    break;
                                }
                            case 1:
                                {
                                    id = 652;
                                    break;
                                }
                            case 2:
                                {
                                    id = 654;
                                    break;
                                }
                        }
                        break;
                    }
                case 1:
                    {
                        switch (gender)
                        {
                            case 0:
                                {
                                    id = 651;
                                    break;
                                }
                            case 1:
                                {
                                    id = 653;
                                    break;
                                }
                            case 2:
                                {
                                    id = 655;
                                    break;
                                }
                        }
                        break;
                    }
                case 2:
                    {
                        switch (gender)
                        {
                            case 0:
                                {
                                    id = 657;
                                    break;
                                }
                            case 1:
                                {
                                    id = 659;
                                    break;
                                }
                            case 2:
                                {
                                    id = 661;
                                    break;
                                }
                        }
                        break;
                    }
                case 3:
                    {
                        switch (gender)
                        {
                            case 0:
                                {
                                    id = 658;
                                    break;
                                }
                            case 1:
                                {
                                    id = 660;
                                    break;
                                }
                            case 2:
                                {
                                    id = 662;
                                    break;
                                }
                        }
                        break;
                    }
                case 4:
                    {
                        id = 656;
                        break;
                    }
            }
            var item = ItemCache.GetItemDefault((short)id);
            character.CharacterHandler.AddItemToBag(false, item);
        }
        private static void HandlerGhepTrangBiHuyDiet(Character character, int typeItem, int gender, int type)
        {
            var listItem = new List<int>() { };
            var listOption = new List<int>() { };
            var listOption2 = new List<int>() { };
            switch (typeItem)
            {
                case 0: // ao
                    {
                        switch (gender)
                        {
                            case 0: // trai dat
                                {
                                    listItem = new List<int> { 0, 3, 33, 34, 136, 137, 138, 139, 230, 231, 232, 233, 555, 650 };
                                    break;
                                }
                            case 1: // namec
                                {
                                    listItem = new List<int> { 1, 4, 41, 42, 152, 153, 154, 155, 234, 235, 236, 237, 557,652 };
                                    break;
                                }
                            case 2: // xayda
                                {
                                    listItem = new List<int> { 2, 5, 49, 50, 168, 169, 170, 171, 238, 239, 240, 241, 559,654 };
                                    break;
                                }
                        }
                        break;
                    }
                case 1: // quan
                    {
                        switch (gender)
                        {
                            case 0: // trai dat
                                {
                                    listItem = new List<int> { 6, 9, 35, 36, 140, 141, 142, 143, 242, 243, 244, 245, 556,651 };
                                    break;
                                }
                            case 1: // namec
                                {
                                    listItem = new List<int> { 7, 10, 43, 44, 156, 157, 158, 159, 246, 247, 248, 249, 558, 653 };
                                    break;
                                }
                            case 2: // xayda
                                {
                                    listItem = new List<int> { 8, 11, 51, 52, 172, 173, 174, 175, 250, 251, 252, 253, 560, 655 };
                                    break;
                                }
                        }
                        break;
                    }
                case 2: // găng
                    {
                        switch (gender)
                        {
                            case 0: // trai dat
                                {
                                    listItem = new List<int> { 21, 24, 37, 38, 144, 145, 146, 147, 254, 255, 256, 257, 562 , 657};
                                    break;
                                }
                            case 1: // namec
                                {
                                    listItem = new List<int> { 22, 25, 45, 46, 160, 161, 162, 163, 258, 259, 260, 261, 564, 659 };
                                    break;
                                }
                            case 2: // xayda
                                {
                                    listItem = new List<int> { 23, 26, 53, 54, 176, 177, 178, 179, 262, 263, 264, 265, 566, 661 };
                                    break;
                                }
                        }
                        break;
                    }
                case 3: // giay
                    {
                        switch (gender)
                        {
                            case 0: // trai dat
                                {
                                    listItem = new List<int> { 27, 30, 39, 40, 148, 149, 150, 151, 266, 267, 268, 269, 563, 658 };
                                    break;
                                }
                            case 1: // namec
                                {
                                    listItem = new List<int> { 28, 31, 47, 48, 164, 165, 166, 167, 270, 271, 272, 273, 565, 660 };
                                    break;
                                }
                            case 2: // xayda
                                {
                                    listItem = new List<int> { 29, 32, 55, 56, 180, 181, 182, 183, 274, 275, 276, 277, 567, 662 };
                                    break;
                                }
                        }
                        break;
                    }
                case 4: // rada
                    {
                        listItem = new List<int> { 12, 57, 58, 59, 184, 185, 186, 187, 278, 279, 280, 281, 561, 656 };
                        break;
                    }

            }
            switch (gender)
            {
                case 0:
                    {
                        listOption = new List<int> { 127, 128, 129 };
                        listOption2 = new List<int> { 139, 140, 141 };
                        break;
                    }
                case 1:
                    {
                        listOption = new List<int> { 130, 131, 132 };
                        listOption2 = new List<int> { 142, 143, 144 };
                        break;
                    }
                case 2:
                    {
                        listOption = new List<int> { 130, 131, 132 };
                        listOption2 = new List<int> { 136, 137, 138 };
                        break;
                    }
            }
            var item = ItemCache.GetItemDefault(((short)listItem[ServerUtils.RandomNumber(listItem.Count)]));
            if (type == 0)//normal
            {
                item = ItemCache.GetItemDefault(((short)listItem[0]));
            }
            item.Options.Add(new OptionItem()
            {
                Id = listOption[ServerUtils.RandomNumber(listOption.Count)],
                Param = 0,
            });
            item.Options.Add(new OptionItem()
            {
                Id = listOption2[ServerUtils.RandomNumber(listOption2.Count)],
                Param = 0,
            });
            character.CharacterHandler.AddItemToBag(false, item);
        }
        private static void HandlerGhepTrangBiHuyDietForBody(Character character, int typeItem, int gender, int index)
        {
            var listItem = new List<int>();
            var listOption = new List<int>() { 127, 128, 129 };
            //  var listOption2 = new List<int>() { };
            switch (typeItem)
            {
                case 0: // ao
                    {
                        switch (gender)
                        {
                            case 0: // trai dat
                                {
                                    listItem = new List<int> { 0, 3, 33, 34, 136, 137, 138, 139, 230, 231, 232, 233, 555, 650 };
                                    break;
                                }
                            case 1: // namec
                                {
                                    listItem = new List<int> { 1, 4, 41, 42, 152, 153, 154, 155, 234, 235, 236, 237, 557, 652 };
                                    break;
                                }
                            case 2: // xayda
                                {
                                    listItem = new List<int> { 2, 5, 49, 50, 168, 169, 170, 171, 238, 239, 240, 241, 559, 654 };
                                    break;
                                }
                        }
                        break;
                    }
                case 1: // quan
                    {
                        switch (gender)
                        {
                            case 0: // trai dat
                                {
                                    listItem = new List<int> { 6, 9, 35, 36, 140, 141, 142, 143, 242, 243, 244, 245, 556, 651 };
                                    break;
                                }
                            case 1: // namec
                                {
                                    listItem = new List<int> { 7, 10, 43, 44, 156, 157, 158, 159, 246, 247, 248, 249, 558, 653 };
                                    break;
                                }
                            case 2: // xayda
                                {
                                    listItem = new List<int> { 8, 11, 51, 52, 172, 173, 174, 175, 250, 251, 252, 253, 560, 655 };
                                    break;
                                }
                        }
                        break;
                    }
                case 2: // găng
                    {
                        switch (gender)
                        {
                            case 0: // trai dat
                                {
                                    listItem = new List<int> { 21, 24, 37, 38, 144, 145, 146, 147, 254, 255, 256, 257, 562, 657 };
                                    break;
                                }
                            case 1: // namec
                                {
                                    listItem = new List<int> { 22, 25, 45, 46, 160, 161, 162, 163, 258, 259, 260, 261, 564, 659 };
                                    break;
                                }
                            case 2: // xayda
                                {
                                    listItem = new List<int> { 23, 26, 53, 54, 176, 177, 178, 179, 262, 263, 264, 265, 566, 661 };
                                    break;
                                }
                        }
                        break;
                    }
                case 3: // giay
                    {
                        switch (gender)
                        {
                            case 0: // trai dat
                                {
                                    listItem = new List<int> { 27, 30, 39, 40, 148, 149, 150, 151, 266, 267, 268, 269, 563, 658 };
                                    break;
                                }
                            case 1: // namec
                                {
                                    listItem = new List<int> { 28, 31, 47, 48, 164, 165, 166, 167, 270, 271, 272, 273, 565, 660 };
                                    break;
                                }
                            case 2: // xayda
                                {
                                    listItem = new List<int> { 29, 32, 55, 56, 180, 181, 182, 183, 274, 275, 276, 277, 567, 662 };
                                    break;
                                }
                        }
                        break;
                    }
                case 4: // rada
                    {
                        listItem = new List<int> { 12, 57, 58, 59, 184, 185, 186, 187, 278, 279, 280, 281, 561, 656 };
                        break;
                    }

            }
            switch (gender)
            {
                
                case 1:
                    {
                        listOption = new List<int>() { 131, 132, 130 };
                   //     listOption2 = new List<int> { 142, 143, 144, 214 };
                        break;
                    }
                case 2:
                    {
                        listOption = new List<int>() { 133, 134, 135 };
                        //         listOption2 = new List<int>() { 133, 134, 135 };
                        break;
                    }
            }
            if (listItem.Count == 0)
            {
                character.CharacterHandler.SendMessage(Service.OpenUiSay(5, "Vui lòng thử lại !"));
                return;
            }
            var item = ItemCache.GetItemDefault(((short)listItem[0]));
            var rand = ServerUtils.RandomNumber(listOption.Count);
            item.Options.Add(new OptionItem()
            {
                Id = listOption[rand],
                Param = 0,
            });
            item.Options.Add(new OptionItem()
            {
                Id = GetSKHDescOption(listOption[rand]),
                Param = 0,
            });
            item.Options.Add(new OptionItem()
            {
                Id = 30,
                Param = 0,
            });
            character.CharacterHandler.AddItemToBody(item, index);
        }
        public static int GetSKHDescOption(int skhId)
        {
            switch (skhId)
            {
                case 127: return 139;
                case 128: return 140;
                case 129: return 141;
                case 130: return 142;
                case 131: return 143;
                case 132: return 144;
                case 133: return 136;
                case 134: return 137;
                case 135: return 138;
            }
            return 73;
        }
        private static void HandlerGhepTrangBiHuyDietForBody(Disciple character, int typeItem, int gender, int index)
        {
            var listItem = new List<int>() { };
            var listOption = new List<int>() { 127, 128, 129 };
            switch (typeItem)
            {
                case 0: // ao
                    {
                        switch (gender)
                        {
                            case 0: // trai dat
                                {
                                    listItem = new List<int> { 0, 3, 33, 34, 136, 137, 138, 139, 230, 231, 232, 233, 555, 650 };
                                    break;
                                }
                            case 1: // namec
                                {
                                    listItem = new List<int> { 1, 4, 41, 42, 152, 153, 154, 155, 234, 235, 236, 237, 557, 652 };
                                    break;
                                }
                            case 2: // xayda
                                {
                                    listItem = new List<int> { 2, 5, 49, 50, 168, 169, 170, 171, 238, 239, 240, 241, 559, 654 };
                                    break;
                                }
                        }
                        break;
                    }
                case 1: // quan
                    {
                        switch (gender)
                        {
                            case 0: // trai dat
                                {
                                    listItem = new List<int> { 6, 9, 35, 36, 140, 141, 142, 143, 242, 243, 244, 245, 556, 651 };
                                    break;
                                }
                            case 1: // namec
                                {
                                    listItem = new List<int> { 7, 10, 43, 44, 156, 157, 158, 159, 246, 247, 248, 249, 558, 653 };
                                    break;
                                }
                            case 2: // xayda
                                {
                                    listItem = new List<int> { 8, 11, 51, 52, 172, 173, 174, 175, 250, 251, 252, 253, 560, 655 };
                                    break;
                                }
                        }
                        break;
                    }
                case 2: // găng
                    {
                        switch (gender)
                        {
                            case 0: // trai dat
                                {
                                    listItem = new List<int> { 21, 24, 37, 38, 144, 145, 146, 147, 254, 255, 256, 257, 562, 657 };
                                    break;
                                }
                            case 1: // namec
                                {
                                    listItem = new List<int> { 22, 25, 45, 46, 160, 161, 162, 163, 258, 259, 260, 261, 564, 659 };
                                    break;
                                }
                            case 2: // xayda
                                {
                                    listItem = new List<int> { 23, 26, 53, 54, 176, 177, 178, 179, 262, 263, 264, 265, 566, 661 };
                                    break;
                                }
                        }
                        break;
                    }
                case 3: // giay
                    {
                        switch (gender)
                        {
                            case 0: // trai dat
                                {
                                    listItem = new List<int> { 27, 30, 39, 40, 148, 149, 150, 151, 266, 267, 268, 269, 563, 658 };
                                    break;
                                }
                            case 1: // namec
                                {
                                    listItem = new List<int> { 28, 31, 47, 48, 164, 165, 166, 167, 270, 271, 272, 273, 565, 660 };
                                    break;
                                }
                            case 2: // xayda
                                {
                                    listItem = new List<int> { 29, 32, 55, 56, 180, 181, 182, 183, 274, 275, 276, 277, 567, 662 };
                                    break;
                                }
                        }
                        break;
                    }
                case 4: // rada
                    {
                        listItem = new List<int> { 12, 57, 58, 59, 184, 185, 186, 187, 278, 279, 280, 281, 561, 656 };
                        break;
                    }

            }
            switch (gender)
            {

                case 1:
                    {
                        listOption = new List<int>() { 131, 132, 213, 130 };
                        //     listOption2 = new List<int> { 142, 143, 144, 214 };
                        break;
                    }
                case 2:
                    {
                        listOption =  new List<int>() { 133, 134, 135 };
                        //         listOption2 = new List<int>() { 133, 134, 135 };
                        break;
                    }
            }
            var item = ItemCache.GetItemDefault(((short)listItem[0]));
            var rand = ServerUtils.RandomNumber(listOption.Count);
            item.Options.Add(new OptionItem()
            {
                Id = listOption[rand],
                Param = 0,
            });
            item.Options.Add(new OptionItem()
            {
                Id = GetSKHDescOption(listOption[rand]),
                Param = 0,
            });
            item.Options.Add(new OptionItem()
            {
                Id = 30,
                Param = 0,
            });
            character.CharacterHandler.AddItemToBody(item, index);
        }
        private static void  ConfirmQuyLao (Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                case 13:
                    switch (select)
                    {
                        case 0:
                            character.CharacterHandler.SendMessage(DuaTopSuKien.ShowTopType1(DuaTopSuKien.InfosTop100DangBanhGiay, new Message(-96), "Top dâng bánh giày"));
                            break;
                        case 1:
                            character.CharacterHandler.SendMessage(DuaTopSuKien.ShowTopType1(DuaTopSuKien.InfosTop100MoHopQuaCaoCap, new Message(-96), "Top mở hộp quà cao cấp"));
                            break;
                        case 2:
                            break;
                    }
                    break;
                case 12:
                    {
                        if (select is 2) break;
                        int id = 0;
                        switch (select)
                        {
                            case 0:
                                id = 1539;
                                break;
                            case 1:
                                id = 1540;
                                break;
                        }
                        if (character.AllDiamond() < 1)
                        {
                            break;
                        }
                        if (character.CharacterHandler.GetItemBagById(id) == null)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Cần " + ItemCache.ItemTemplate((short)id).Name));
                            break;
                        }
                        var item = ItemCache.GetItemDefault((short)(DataCache.VanSuNhuY2024[ServerUtils.RandomNumber(DataCache.VanSuNhuY2024.Count)]));
                        character.MineDiamond(1);
                        character.CharacterHandler.AddItemToBag(true, item);
                        character.CharacterHandler.RemoveItemBagById((short)id, 1);
                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                        character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã xin được chữ " + ItemCache.ItemTemplate(item.Id).Name));
                        character.CharacterHandler.SendMessage(Service.BuyItem(character));
                        break;
                    }
                case 11:
                    {

                        if (character.InfoEvent.PhaoBong < 10)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Con còn thiếu " + (10 - character.InfoEvent.PhaoBong) + " điểm nữa"));
                            break;
                        }
                        character.InfoEvent.PhaoBong -= 10;
                        var listItem = new List<short> { (short)(1174 + character.InfoChar.Gender), (short)(1529 + character.InfoChar.Gender), 1528, 1201 };
                        var item = ItemCache.GetItemDefault(listItem[ServerUtils.RandomNumber(listItem.Count)]);
                        var randomRate = ServerUtils.RandomNumber(0.0, 100.0);
                        int expireDay = 0;
                        int expireTime = 0;

                        switch (randomRate)
                        {
                            case < 20:
                                break;
                            case < 50:
                                expireDay = DataCache.LuckyBoxItemExpire[ServerUtils.RandomNumber(DataCache.LuckyBoxItemExpire.Count - 2, DataCache.LuckyBoxItemExpire.Count)];
                                break;
                            case < 70:
                                expireDay = DataCache.LuckyBoxItemExpire[ServerUtils.RandomNumber(3, 5)];
                                break;
                            default:
                                expireDay = DataCache.LuckyBoxItemExpire[ServerUtils.RandomNumber(1, 3)];
                                break;
                        }
                        var timeServer = ServerUtils.CurrentTimeMillis();
                        if (expireDay > 0)
                        {
                            expireTime = (int)(timeServer + (expireDay * DataCache._1DAY));
                        }
                        var optionHiden = item.Options.FirstOrDefault(option => option.Id == 73);
                        item.Options.Add(new OptionItem()
                        {
                            Id = 93,
                            Param = expireDay,
                        });

                        if (optionHiden != null)
                        {
                            optionHiden.Param = expireTime;
                        }
                        else
                        {
                            item.Options.Add(new OptionItem()
                            {
                                Id = 73,
                                Param = expireTime,
                            });
                        }

                        character.CharacterHandler.AddItemToBag(false, item);
                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                        character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + ItemCache.ItemTemplate(item.Id).Name));
                        break;
                    }
                //Open menu 1
                case 0:
                    {
                        switch (select)
                        {
                            //Nói chuyện
                            case 0: {
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextQuyLao[1], MenuNpc.Gi().MenuQuyLao[ClanManager.Get(character.ClanId) != null ? 5:1], character.InfoChar.Gender));
                                    character.TypeMenu = 1;
                                    break;
                                }
                            //Kho báo dưới biển
                            case 1:
                                {
                                    var clan = ClanManager.Get(character.ClanId);
                                    if (clan == null)
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Con chưa có bang hội"));
                                    }
                                   else if (clan.CondititonToJoinDungeon(character, clan, npcId))
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Đây là bản đồ kho báu hải tặc tí hon\nCác con cứ yên tâm lên đường\nỞ đây có ta lo\nNhớ chọn cấp độ vừa sức mình nhé", new List<string> { "Top\nBang hội", "Thành tích\nBang", clan.ClanDungeon.BanDoKhoBau.CheckOpen() ? "Tham gia" : "Chọn\ncấp độ", "Từ chối" }, character.InfoChar.Gender));
                                        character.TypeMenu = 7;
                                    }
                                    break;
                                }
                            case 2:// đổi điểm sự kiện TOP
                                {
                                    break;
                                }
                            case 3://Sự kiện đua top
                                {
                                    DuaTopSuKien.OpenMenuSuKien(npcId, character);
                                    character.TypeMenu = 13;
                                    break;
                                }
                            //case 2:// sự kiện đua top
                            //    {
                            //        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextQuyLao[8], MenuNpc.Gi().MenuQuyLao[6], character.InfoChar.Gender));
                            //        character.TypeMenu = 10;
                            //        break;
                            //    }
                            //case 3:// đổi điểm pháo bông
                            //    {
                            //        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextQuyLao[9], MenuNpc.Gi().MenuQuyLao[7], character.InfoChar.Gender));
                            //        character.TypeMenu = 11;
                            //        break;
                            //    }
                            //case 4://xin chữ đầu năm
                            //    {
                            //        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextQuyLao[6], MenuNpc.Gi().MenuQuyLao[8], character.InfoChar.Gender));
                            //        character.TypeMenu = 12;
                            //        break;
                            //    }
                            //case 5://đổi phiếu bé ngoan lấy quà
                            //    {
                            //        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextQuyLao[7], MenuNpc.Gi().MenuQuyLao[9], character.InfoChar.Gender));
                            //        character.TypeMenu = 13;
                            //        break;
                            //    }

                                //case 2:
                                //    {
                                //        if (TaskHandler.CheckTask(character, 20, 1))
                                //        {
                                //            TaskHandler.gI().PlusSubTask(character, 1);
                                //        }
                                //    //    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, string.Format(MenuNpc.Gi().TextQuyLao[4], character.DiemSuKien), MenuNpc.Gi().MenuQuyLao[3], character.InfoChar.Gender));
                                //    //    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, string.Format(MenuNpc.gI().TextQuyLao[4], character.DiemSuKien), MenuNpc.gI()).MenuQuyLao[3], character.InfoChar.Gender));
                                //    //    character.TypeMenu = 5;
                                //        break;
                                //    }
                                //case 3:
                                //   // if (ConfigManager.gI().SuKienWorldCup)
                                //    //{
                                //    //    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextQuyLao[5], MenuNpc.Gi().MenuQuyLao[4], character.InfoChar.Gender));
                                //    //}else if (ConfigManager.gI().SuKienNoel)
                                //    //{
                                //    //    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "|0|Hãy thu thập đủ x99 các loại vật phẩm sự kiện Noel\n|6|Để đổi lấy những phần quà hấp dẫn", new List<String> { "Đổi", "Từ chối" }, character.InfoChar.Gender));
                                //    //}
                                //    //else
                                //    //{
                                //        if (TaskHandler.CheckTask(character, 19, 1))
                                //        {
                                //            TaskHandler.gI().PlusSubTask(character, 1);
                                //        }
                                //    //}

                                //    character.TypeMenu = 6;
                                //    break;
                                //case 4:
                                //    character.CharacterHandler.SendMessage(Service.GiftBox(character.ItemGift));
                                //    character.ShopId = 2222;
                                //    break;
                                //case 5:
                                //    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId,DoiThuongHandler.DoiThuong(character, "|3|Xô cá", DoiThuongCache.DoiCa[0], DoiThuongCache.DoiCa[1], (int)DoiThuongType.XO_CA), (character.TypeDoiThuong != 4 ? DoiThuongCache.Combines[1] : DoiThuongCache.Combines[0]), character.InfoChar.Gender));
                                //    character.TypeMenu = 8;
                                //    break;
                        }
                        break;
                    }
                case 8:
                    switch (select)
                    {
                        case 0:
                            if (character.TypeDoiThuong != 4) return;
                            for (int i = 0; i < 3; i++)
                            {
                                character.CharacterHandler.RemoveItemBagById((short)DoiThuongCache.DoiCa[0][i], DoiThuongCache.DoiCa[1][i]);
                            }

                            character.CharacterHandler.AddItemToBag(true, ItemCache.GetItemDefault(1006));
                            character.MineGold(500000000);
                            character.CharacterHandler.SendMessage(Service.BuyItem(character));
                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                            character.CharacterHandler.SendMessage(Service.ServerMessage("bạn đã nhận được 1 xô cá vàng"));
                            break;
                    }
                    break;
                //Open menu Nói chuyện
                case 7:
                    switch (select)
                    {
                        case 0:
                           
                            break;
                        case 1:
                            if (character.ClanId == -1)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Hãy vào bang hội trước"));
                            }
                            else
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Chức năng đang được cập nhật"));
                            }
                            break;
                        
                        case 2:
                            if (character.ClanId == -1)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Hãy vào bang hội trước"));
                            }
                            else
                            {
                                var clan = ClanManager.Get(character.ClanId);
                                if (clan.ClanDungeon.BanDoKhoBau.CheckOpen())
                                {
                                    var time = (clan.ClanDungeon.BanDoKhoBau.Time - ServerUtils.CurrentTimeMillis()) / 60000;
                                    if (time <= 1)
                                    {
                                        time = (clan.ClanDungeon.BanDoKhoBau.Time - ServerUtils.CurrentTimeMillis()) / 1000;
                                    }
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Bang hội của ngươi đã mở Bản Đồ Kho Báu Level: " + clan.ClanDungeon.BanDoKhoBau.Level + "\nCòn " + time + (time <= 1 ? " giây nữa" : "' nũa"), new List<string> {"Tham gia\n(Miễn phí)","Hủy" }, character.InfoChar.Gender));
                                    character.TypeMenu = 9;
                                }
                                else if (character.CharacterHandler.GetItemBagById(611) == null)
                                {
                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Yêu cầu bản đồ kho báu"));
                                }
                                else
                                {
                                    var inputBDKB = new List<InputBox>();
                                    var inputLevel = new InputBox()
                                    {
                                        Name = "(Nhập cấp độ từ 0 -> 110)",
                                        Type = 1,
                                    };
                                    inputBDKB.Add(inputLevel);
                                    character.CharacterHandler.SendMessage(Service.ShowInput("Nhập cấp độ Bản Đồ Kho Báu", inputBDKB));
                                    character.TypeInput = 14;
                                }
                            }
                            break;

                    }
                    break;
                case 9:
                    {
                        switch (select)
                        {
                            case 0:
                                character.InfoChar.X = 78;
                                character.InfoChar.Y = 336;
                                MapManager.Get(135).GetZoneById(character.ClanId).ZoneHandler.JoinZone(character, false, false, 0);
                                break;
                        }
                    }
                    break;
                case 1:
                    {
                        switch (select)
                        {
                            case 0:
                                {

                                    var task = Cache.Gi().TASK_TEMPLATES_0.Values.FirstOrDefault(i => i.Id == character.InfoTask.Id).SubNames[character.InfoTask.Index];
                                    character.CharacterHandler.SendMessage(Service.OpenUiSay(5, task));
                                    break;
                                }
                            case 1:
                                {
                                    if (character.InfoChar.LearnSkill != null)
                                    {
                                        var itemAdd = character.InfoChar.LearnSkill.ItemSkill;
                                        var time = character.InfoChar.LearnSkill.Time - ServerUtils.CurrentTimeMillis();
                                        var skillTemplate = Cache.Gi().SKILL_TEMPLATES.FirstOrDefault(skill => skill.Id == character.InfoChar.LearnSkill.ItemTemplateSkillId);
                                        var itemTempalte = ItemCache.ItemTemplate(itemAdd.Id);
                                        var ngoc = 5;
                                        if (time / 600000 >= 2)
                                        {
                                            ngoc += (int)time / 600000;
                                        }

                                        var menu = string.Format(TextServer.gI().ADDING_SKILL, skillTemplate.Name,
                                            itemTempalte.Level, ServerUtils.GetTime(time));
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, menu, new List<string>() { $"Học\nCấp tốc\n{ngoc} ngọc", "Huỷ", "Bỏ qua" }, character.InfoChar.Gender));
                                        character.TypeMenu = 3;
                                    }
                                    else
                                    {
                                        var idShop = 7 + character.InfoChar.Gender;
                                        character.CharacterHandler.SendMessage(Service.Shop(character, 1, idShop));
                                        character.ShopId = idShop;
                                        character.TypeShop = 0;
                                    }
                                    break;
                                }
                            case 2:
                                if (character.ClanId == -1)
                                {
                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Hãy vào bang hội trước"));
                                }
                                else
                                {
                                    var clan = ClanManager.Get(character.ClanId);
                                    if (clan.Thành_viên.FirstOrDefault(i=> i.Id == character.Id).Role != 0)
                                    {
                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn không phải là chủ của bang hội"));
                                    }
                                    else
                                    {
                                        clan.Thành_viên.ForEach(member =>
                                        {
                                            var temp = ClientManager.Gi().GetCharacter(member.Id);
                                            temp.ClanId = -1;
                                            temp.CharacterHandler.SendMessage(Service.MyClanInfo());
                                            temp.CharacterHandler.SendMessage(Service.ServerMessage("Bang hội của bạn đã bị giải tán"));
                                            temp.CharacterHandler.UpdatePhukien();
                                        });
                                        ClanManager.Remove(clan);
                                    }
                                }
                                break;
                            case 3:
                                {
                                    var clan = ClanManager.Get(character.ClanId);
                                    if (clan != null)
                                    {
                                        MapManager.OutMap(character, clan.ClanZone.Map.Id);
                                        clan.ClanZone.Map.JoinZone(character, 0);
                                    }

                                    break;
                                }
                        }
                        break;
                    }
                //Học skill
                case 2:
                    {
                        switch (select)
                        {
                            //Đồng ý
                            case 0:
                                {
                                    if (character.InfoChar.LearnSkillTemp == null) return;
                                    var itemAdd = character.InfoChar.LearnSkillTemp.ItemSkill;
                                    var time = character.InfoChar.LearnSkillTemp.Time + ServerUtils.CurrentTimeMillis();
                                    var idSkill = character.InfoChar.LearnSkillTemp.ItemTemplateSkillId;
                                    character.InfoChar.Potential -= itemAdd.BuyPotential;
                                    character.InfoChar.LearnSkill = new LearnSkill()
                                    {
                                        ItemSkill = itemAdd,
                                        Time = time,
                                        ItemTemplateSkillId = idSkill,
                                        Potential = (int)itemAdd.BuyPotential
                                    };
                                    character.InfoChar.LearnSkillTemp = null;
                                    character.CharacterHandler.SendMessage(Service.MeLoadPoint(character));
                                    character.CharacterHandler.SendMessage(Service.ClosePanel());
                                    character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Con đã học thành công, hãy cố gắng chờ đợi nha"));
                                    break;
                                }
                            //Từ chối
                            case 1:
                                {
                                    character.InfoChar.LearnSkillTemp = null;
                                    break;
                                }
                        }
                        break;
                    }
                //Open menu with learn skill
                case 3:
                    {
                        switch (select)
                        {
                            //Đồng ý học nhanh
                            case 0:
                                {
                                    if (character.InfoChar.LearnSkill == null) return;
                                    var itemAdd = character.InfoChar.LearnSkill.ItemSkill;
                                    var time = character.InfoChar.LearnSkill.Time - ServerUtils.CurrentTimeMillis();
                                    var skillTemplate = Cache.Gi().SKILL_TEMPLATES.FirstOrDefault(skill => skill.Id == character.InfoChar.LearnSkill.ItemTemplateSkillId);
                                    if (skillTemplate == null) return;
                                    var ngoc = 5;
                                    if (time / 600000 >= 2)
                                    {
                                        ngoc += (int)time / 600000;
                                    }
                                    if (character.AllDiamond() < ngoc) {
                                        character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_DIAMOND));
                                        return;
                                    }
                                    character.MineDiamond(ngoc);
                                    character.InfoChar.LearnSkill = null;
                                    character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                                    ItemHandler.AddLearnSkill(character, itemAdd, skillTemplate);
                                    break;
                                }
                            //Huỷ học skill
                            case 1:
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextQuyLao[3], MenuNpc.Gi().MenuMeo[1], character.InfoChar.Gender));
                                    character.TypeMenu = 4;
                                    break;
                                }
                            //Open menu 1
                            case 2:
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextQuyLao[0], MenuNpc.Gi().MenuQuyLao[0], character.InfoChar.Gender));
                                    character.TypeMenu = 0;
                                    break;
                                }
                        }
                        break;
                    }
                //Huỷ học skill
                case 4:
                    {
                        if (select != 0) return;
                        var plusPoint = character.InfoChar.LearnSkill.Potential / 2;
                        character.CharacterHandler.PlusTiemNang(0, plusPoint, false);
                        character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().CANCEL_LEARN_SKILL));
                        character.InfoChar.LearnSkill = null;
                        character.InfoChar.LearnSkillTemp = null;
                        break;
                    }
                case 5:
                    {
                        switch (select) {
                            case 0:
                                {
                                    if (character.DiemSuKien >= 50) {
                                        var randomRate = ServerUtils.RandomNumber(0.0, 100.0);
                                        var itemAdd2 = ItemCache.GetItemDefault(1);

                                        if (randomRate <= 20.0)
                                        {
                                            itemAdd2 = ItemCache.GetItemDefault(1087);
                                        } else if (randomRate <= 40.0)
                                        {
                                            itemAdd2 = ItemCache.GetItemDefault(1088);
                                        } else if (randomRate <= 60.0)
                                        {
                                            itemAdd2 = itemAdd2 = ItemCache.GetItemDefault(1089);
                                        } else if (randomRate <= 80.0) {
                                            itemAdd2 = itemAdd2 = ItemCache.GetItemDefault(1090);
                                        } else
                                        {
                                            itemAdd2 = itemAdd2 = ItemCache.GetItemDefault(1091);
                                        }
                                        itemAdd2.Reason = "Quà Sự Kiện";
                                        itemAdd2.Options.Add(new OptionItem()
                                        {
                                            Id = 30,
                                            Param = 0,
                                        });
                                        itemAdd2.Quantity = 1;

                                        character.CharacterHandler.AddItemToBag(true, itemAdd2, "SuKien");
                                        character.DiemSuKien -= 50;
                                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                                        var template2 = ItemCache.ItemTemplate(itemAdd2.Id);
                                        character.CharacterHandler.SendMessage(
                                        Service.ServerMessage(string.Format(TextServer.gI().ADD_ITEM,
                                         $"x{itemAdd2.Quantity} {template2.Name}")));
                                        break;
                                    } else
                                    {
                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn còn thiếu " + (50 - character.DiemSuKien) + " Điểm Sự Kiện nữa"));
                                        break;
                                    }
                                }
                            case 1:
                                break;
                        }
                        break;
                    }
                case 6:
                    if (ConfigManager.gI().SuKienWorldCup)
                    {
                        switch (select)
                        {



                            case 0:
                                for (int dball = 1129; dball <= 1138; dball++)
                                {
                                    if (character.CharacterHandler.GetItemBagById(dball) == null || character.CharacterHandler.GetItemBagById(dball).Quantity < 10)
                                    {
                                        character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_GENDER));
                                        return;
                                    }
                                }
                                for (short dball = 1129; dball <= 1138; dball++)
                                {
                                    character.CharacterHandler.RemoveItemBagById(dball, 10, reason: "Menu 0 World CUP");
                                }
                                int gold = 200000000;
                                if (character.InfoChar.Gold < gold)
                                {

                                    character.CharacterHandler.SendMeMessage(Service.ServerMessage("Không đủ vàng"));
                                    return;
                                }
                                else
                                {

                                    character.MineGold(gold);
                                    var itemAdd2 = ItemCache.GetItemDefault(1144);
                                    itemAdd2.Options.Add(new OptionItem()
                                    {
                                        Id = 30,
                                        Param = 0,
                                    });
                                    itemAdd2.Quantity = 1;
                                    //  character.MineDiamond(diamond);
                                    character.CharacterHandler.AddItemToBag(true, itemAdd2, "World cup");
                                    character.CharacterHandler.SendMeMessage(Service.SendBag(character));
                                }
                                break;
                            case 1:
                                for (int dball = 1129; dball <= 1138; dball++)
                                {
                                    if (character.CharacterHandler.GetItemBagById(dball) == null || character.CharacterHandler.GetItemBagById(dball).Quantity < 10)
                                    {
                                        character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_GENDER));
                                        return;
                                    }
                                }
                                for (short dball = 1129; dball <= 1138; dball++)
                                {
                                    character.CharacterHandler.RemoveItemBagById(dball, 10, reason: "Menu 0 World CUP");
                                }
                                int diamond = 1000;
                                if (character.InfoChar.Diamond < diamond)
                                {

                                    character.CharacterHandler.SendMeMessage(Service.ServerMessage("Không đủ ngọc"));
                                    return;
                                }
                                else
                                {
                                    var itemAdd = ItemCache.GetItemDefault(1143);
                                    itemAdd.Options.Add(new OptionItem()
                                    {
                                        Id = 30,
                                        Param = 0,
                                    });
                                    itemAdd.Quantity = 1;
                                    character.MineDiamond(diamond);
                                    character.CharacterHandler.AddItemToBag(true, itemAdd, "World cup");
                                    character.CharacterHandler.SendMeMessage(Service.SendBag(character));
                                }

                                break;
                        }
                    } else if (ConfigManager.gI().SuKienNoel)
                    {
                        switch (select) {
                            case 0:
                        for (int dball = 1181; dball <= 1185; dball++)
                        {
                            if (character.CharacterHandler.GetItemBagById(dball) == null || character.CharacterHandler.GetItemBagById(dball).Quantity < 99)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Không đủ nguyên liệu"));
                                return;
                            }
                            else
                            {
                                character.CharacterHandler.RemoveItemBagById((short)dball, 99);
                            }
                        }

                                var tui7chulun = ItemCache.GetItemDefault((short)1187);
                                tui7chulun.Quantity = 1;
                                character.CharacterHandler.AddItemToBag(true, tui7chulun);
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Chúc mừng cu đã nhận được Túi 7 Thằng Lùn"));
                                character.CharacterHandler.SendMessage(Service.SendBag(character));
                                break;
                    }

                    }
        
                    break;

        }
        }
        private static void ConfirmKyGUI(Character character, short npcId, int select){
            switch(select){
                case 1:
                 //   character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId,"Chức năng tạm thời bảo trì đến khi Open"));
                    //character.TypeMenu = 0;
                    character.CharacterHandler.SendMessage(KyGUIService.OpenShopKiGui(character));
                    character.TypeShop = 0;
                    character.TypeMenu = 0;
                break;
            }
        }
        private static void ConfirmTruongLaoGuru(Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                //Open menu Nói chuyện
                case 0:
                {
                    switch (select)
                    {
                        case 0:
                        {
                                    
                                    break;
                        }
                        case 1:
                        {
                            if (character.InfoChar.LearnSkill != null)
                            {
                                var itemAdd = character.InfoChar.LearnSkill.ItemSkill;
                                var time = character.InfoChar.LearnSkill.Time - ServerUtils.CurrentTimeMillis();
                                var skillTemplate = Cache.Gi().SKILL_TEMPLATES.FirstOrDefault(skill => skill.Id == character.InfoChar.LearnSkill.ItemTemplateSkillId);
                                var itemTempalte = ItemCache.ItemTemplate(itemAdd.Id);
                                var ngoc = 5;
                                if (time / 600000 >= 2)
                                {
                                    ngoc += (int)time / 600000;
                                }

                                var menu = string.Format(TextServer.gI().ADDING_SKILL, skillTemplate.Name,
                                    itemTempalte.Level, ServerUtils.GetTime(time));
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, menu, new List<string>() {$"Học\nCấp tốc\n{ngoc} ngọc", "Huỷ","Bỏ qua"}, character.InfoChar.Gender));
                                character.TypeMenu = 2;
                            }
                            else
                            {
                                var idShop = 10;
                                character.CharacterHandler.SendMessage(Service.Shop(character, 1, idShop));
                                character.ShopId = idShop;
                                character.TypeShop = 1;
                            }
                            break;
                        }
                    }
                    break;
                }
                //Học skill
                case 1:
                {
                    switch (select)
                    {
                        //Đồng ý
                        case 0:
                        {
                            if(character.InfoChar.LearnSkillTemp == null) return;
                            var itemAdd = character.InfoChar.LearnSkillTemp.ItemSkill;
                            var time = character.InfoChar.LearnSkillTemp.Time + ServerUtils.CurrentTimeMillis();
                            var idSkill = character.InfoChar.LearnSkillTemp.ItemTemplateSkillId;
                            character.InfoChar.Potential -= itemAdd.BuyPotential;
                            character.InfoChar.LearnSkill = new LearnSkill()
                            {
                                ItemSkill = itemAdd,
                                Time = time,
                                ItemTemplateSkillId = idSkill,
                                Potential = (int)itemAdd.BuyPotential
                            };
                            character.InfoChar.LearnSkillTemp = null;
                            character.CharacterHandler.SendMessage(Service.MeLoadPoint(character));
                            character.CharacterHandler.SendMessage(Service.ClosePanel());
                            character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Con đã học thành công, hãy cố gắng chờ đợi nha"));
                            break;
                        }
                        //Từ chối
                        case 1:
                        {
                            character.InfoChar.LearnSkillTemp = null;
                            break;
                        }
                    }
                    break;
                }
                //Open menu with learn skill
                case 2:
                {
                    switch (select)
                    {
                        //Đồng ý học nhanh
                        case 0:
                        {
                            if(character.InfoChar.LearnSkill == null) return;
                            var itemAdd = character.InfoChar.LearnSkill.ItemSkill;
                            var time = character.InfoChar.LearnSkill.Time - ServerUtils.CurrentTimeMillis();
                            var skillTemplate = Cache.Gi().SKILL_TEMPLATES.FirstOrDefault(skill => skill.Id == character.InfoChar.LearnSkill.ItemTemplateSkillId);
                            if (skillTemplate == null) return;
                            var ngoc = 5;
                            if (time / 600000 >= 2)
                            {
                                ngoc += (int)time / 600000;
                            }
                            if(character.AllDiamond() < ngoc) {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_DIAMOND));
                                return;
                            }
                            character.MineDiamond(ngoc);
                            character.InfoChar.LearnSkill = null;
                            character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                            ItemHandler.AddLearnSkill(character, itemAdd, skillTemplate);
                            break;
                        }
                        //Huỷ học skill
                        case 1:
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextQuyLao[3], MenuNpc.Gi().MenuMeo[1], character.InfoChar.Gender));
                            character.TypeMenu = 3;
                            break;
                        }
                        //Open menu 1
                        case 2:
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextQuyLao[1], MenuNpc.Gi().MenuQuyLao[1], character.InfoChar.Gender));
                            character.TypeMenu = 0;
                            break;
                        }
                    }
                    break;
                }
                //Huỷ học skill
                case 3:
                {
                    if(select != 0) return;
                    var plusPoint = character.InfoChar.LearnSkill.Potential / 2;
                    character.CharacterHandler.PlusTiemNang(0, plusPoint, false);
                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().CANCEL_LEARN_SKILL));
                    character.InfoChar.LearnSkill = null;   
                    character.InfoChar.LearnSkillTemp = null;   
                    break;
                }
                case 4:
                    switch (select)
                    {
                        case 0:
                            {
                                var menu = MenuNpc.Gi().MenuChienTruongNamec[1];
                                var typeMenu = 5;
                                switch (Server.Gi().NamecBattlefield.Status)
                                {
                                    case NamecBattlefield_Status.OPEN:
                                        menu = MenuNpc.Gi().MenuChienTruongNamec[2];
                                        typeMenu = 6;
                                        break;
                                }
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextMenuChienTruongNamec[1], menu, character.InfoChar.Gender));
                                character.TypeMenu = typeMenu;
                                break;
                            }
                    }
                    break;
                case 5://register status 
                    switch (select)
                    {
                        case 0:
                        case 4:
                            break;
                        case 1:// doi diem thuong
                            break;
                        case 2:// bxh
                            break;
                        case 3:// hang tu do
                            var menu = MenuNpc.Gi().MenuChienTruongNamec[3];
                            var textMenu = string.Format(MenuNpc.Gi().TextMenuChienTruongNamec[2], Server.Gi().NamecBattlefield.Cadic.Characters.Count, Server.Gi().NamecBattlefield.Fide.Characters.Count);
                            var typeMenu = 8;
                            if (character.DataNamecBattlefield.Status == NamecBattlefield_Character_Status.REGISTER)
                            {
                                menu = MenuNpc.Gi().MenuChienTruongNamec[4];
                                var getTeam = character.DataNamecBattlefield.TeamId == 1 ? Server.Gi().NamecBattlefield.Cadic : Server.Gi().NamecBattlefield.Fide;
                                textMenu = string.Format(MenuNpc.Gi().TextMenuChienTruongNamec[3], getTeam.Characters.Count, Server.Gi().NamecBattlefield.Cadic.Characters.Count, Server.Gi().NamecBattlefield.Fide.Characters.Count);
                                typeMenu = 7;
                            }
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, textMenu, menu, character.InfoChar.Gender));
                            character.TypeMenu = typeMenu;
                            break;
                    }
                    break;
                case 6:// open status
                    switch (select)
                    {
                        case 0:
                        case 3:
                            break;
                        case 1:// doi diem thuong
                            break;
                        case 2:// bxh
                            break;
                    }
                    break;
                case 8:
                    switch (select)
                    {
                        case 0:
                            Server.Gi().NamecBattlefield.Cadic.Characters.Add(character.Id);
                            character.DataNamecBattlefield.Status = NamecBattlefield_Character_Status.REGISTER;
                            character.DataNamecBattlefield.TeamId = 1;
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Đăng kí hỗ trợ phe Ca Đíc thành công"));
                            break;
                        case 1:
                            Server.Gi().NamecBattlefield.Fide.Characters.Add(character.Id);
                            character.DataNamecBattlefield.Status = NamecBattlefield_Character_Status.REGISTER;
                            character.DataNamecBattlefield.TeamId = 2;
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Đăng kí hỗ trợ phe Ca Đíc thành công"));

                            break;
                    }
                    break;  
                case 7:// registered
                    switch (select)
                    {
                        case 0:
                            var team = Server.Gi().NamecBattlefield.Cadic;
                            switch (character.DataNamecBattlefield.TeamId)
                            {
                                case 1:
                                    break;
                                case 2:
                                    team = Server.Gi().NamecBattlefield.Fide;
                                    break;
                            }
                            team.Characters.Remove(character.Id);
                            character.DataNamecBattlefield.Status = NamecBattlefield_Character_Status.NORMAL;
                            break;
                    }
                    break;
            }
        }

        private static void ConfirmVuaVegeta(Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                //Open menu Nói chuyện
                case 0:
                {
                    switch (select)
                    {
                        case 0:
                        {

                                    break;
                        }
                        case 1:
                        {
                            if (character.InfoChar.LearnSkill != null)
                            {
                                var itemAdd = character.InfoChar.LearnSkill.ItemSkill;
                                var time = character.InfoChar.LearnSkill.Time - ServerUtils.CurrentTimeMillis();
                                var skillTemplate = Cache.Gi().SKILL_TEMPLATES.FirstOrDefault(skill => skill.Id == character.InfoChar.LearnSkill.ItemTemplateSkillId);
                                var itemTempalte = ItemCache.ItemTemplate(itemAdd.Id);
                                var ngoc = 5;
                                if (time / 600000 >= 2)
                                {
                                    ngoc += (int)time / 600000;
                                }

                                var menu = string.Format(TextServer.gI().ADDING_SKILL, skillTemplate.Name,
                                    itemTempalte.Level, ServerUtils.GetTime(time));
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, menu, new List<string>() {$"Học\nCấp tốc\n{ngoc} ngọc", "Huỷ","Bỏ qua"}, character.InfoChar.Gender));
                                character.TypeMenu = 2;
                            }
                            else
                            {
                                var idShop = 11;
                                character.CharacterHandler.SendMessage(Service.Shop(character, 1, idShop));
                                character.ShopId = idShop;
                                character.TypeShop = 2;
                            }
                            break;
                        }
                    }
                    break;
                }
                //Học skill
                case 1:
                {
                    switch (select)
                    {
                        //Đồng ý
                        case 0:
                        {
                            if(character.InfoChar.LearnSkillTemp == null) return;
                            var itemAdd = character.InfoChar.LearnSkillTemp.ItemSkill;
                            var time = character.InfoChar.LearnSkillTemp.Time + ServerUtils.CurrentTimeMillis();
                            var idSkill = character.InfoChar.LearnSkillTemp.ItemTemplateSkillId;
                            character.InfoChar.Potential -= itemAdd.BuyPotential;
                            character.InfoChar.LearnSkill = new LearnSkill()
                            {
                                ItemSkill = itemAdd,
                                Time = time,
                                ItemTemplateSkillId = idSkill,
                                Potential = (int)itemAdd.BuyPotential
                            };
                            character.InfoChar.LearnSkillTemp = null;
                            character.CharacterHandler.SendMessage(Service.MeLoadPoint(character));
                            character.CharacterHandler.SendMessage(Service.ClosePanel());
                            character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Con đã học thành công, hãy cố gắng chờ đợi nha"));
                            break;
                        }
                        //Từ chối
                        case 1:
                        {
                            character.InfoChar.LearnSkillTemp = null;
                            break;
                        }
                    }
                    break;
                }
                //Open menu with learn skill
                case 2:
                {
                    switch (select)
                    {
                        //Đồng ý học nhanh
                        case 0:
                        {
                            if(character.InfoChar.LearnSkill == null) return;
                            var itemAdd = character.InfoChar.LearnSkill.ItemSkill;
                            var time = character.InfoChar.LearnSkill.Time - ServerUtils.CurrentTimeMillis();
                            var skillTemplate = Cache.Gi().SKILL_TEMPLATES.FirstOrDefault(skill => skill.Id == character.InfoChar.LearnSkill.ItemTemplateSkillId);
                            if (skillTemplate == null) return;
                            var ngoc = 5;
                            if (time / 600000 >= 2)
                            {
                                ngoc += (int)time / 600000;
                            }
                            if(character.AllDiamond() < ngoc) {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_DIAMOND));
                                return;
                            }
                            character.MineDiamond(ngoc);
                            character.InfoChar.LearnSkill = null;
                            character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                            ItemHandler.AddLearnSkill(character, itemAdd, skillTemplate);
                            break;
                        }
                        //Huỷ học skill
                        case 1:
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextQuyLao[3], MenuNpc.Gi().MenuMeo[1], character.InfoChar.Gender));
                            character.TypeMenu = 3;
                            break;
                        }
                        //Open menu 1
                        case 2:
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextQuyLao[1], MenuNpc.Gi().MenuQuyLao[1], character.InfoChar.Gender));
                            character.TypeMenu = 0;
                            break;
                        }
                    }
                    break;
                }
                //Huỷ học skill
                case 3:
                {
                    if(select != 0) return;
                    var plusPoint = character.InfoChar.LearnSkill.Potential / 2;
                    character.CharacterHandler.PlusTiemNang(0, plusPoint, false);
                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().CANCEL_LEARN_SKILL));
                    character.InfoChar.LearnSkill = null;   
                    character.InfoChar.LearnSkillTemp = null;   
                    break;
                }
            }
        }

        private static void ConfirmThanMeo(Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                case 10:
                    switch (select)
                    {
                        case 0:
                            character.DataPractice.Handler.Practice(character, Practice_Progress.YAJIRO);
                            break;
                    }
                    break;
                case 11:
                    switch (select)
                    {
                        case 0:
                            character.DataPractice.Handler.Challenge(character, Practice_Progress.YAJIRO);
                            break;
                    }
                    break;
                case 9:
                    switch (select)
                    {
                        case 0://tdlt

                            if (character.DataPractice.isAutoTrain())
                            {
                                character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Con đã hủy thành công đăng ký tập tự động !\ntừ giờ còn muốn tập Offline hãy tự đến đây trước"));
                                character.DataPractice.TrainStatus = AutoTrain_Status.NORMAL;
                            }
                            else
                            {
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, $"Đăng ký để mỗi khi Offline quá 30 phút, con sẽ được tự động luyện tập với tốc độ\n{character.DataPractice.GetPotenial()} sức mạnh mỗi phút", new List<string> { "Hướng\ndẫn\nthêm", "Đồng ý\n1 ngọc\nmỗi lần", "Không\nĐồng ý" }, character.InfoChar.Gender));
                                character.TypeMenu = 5;
                            }
                            break;
                        case 1://tập luyện với yajiro
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, $"Con có chắc muốn tập luyện với Yajiro?"
                            + $"\nTập luyện với hắn sẽ tăng 40 sức mạnh mỗi phút", new List<string> { "Đồng ý\nluyện tập", "Không\nđồng ý" }, character.InfoChar.Gender));
                            character.TypeMenu = 10;
                            break;
                        case 2:// thách đấu với yajiro
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, $"Con có chắc muốn thách đấu với Yajiro?"
                            + $"\nNếu thắng hắn sẽ được bay lên Thần Điện, tăng 80 sức mạnh mỗi phút", new List<string> { "Đồng ý\ngiao đấu", "Không\nđồng ý" }, character.InfoChar.Gender));
                            character.TypeMenu = 11;

                            break;
                    }
                    break;
                case 5:
                switch(select){
                    case 0:
                    var huongdanthem = "Tập luyện vẫn tiếp tục và sức mạnh vẫn tăng khi đã Offline\n"
+ "Hiệu quả tập luyện như sau:\nThần Mèo: 20 sức mạnh mỗi phút\nYajirô: 40 sức mạnh mỗi phút\nMr.PôPô: 80 sức mạnh mỗi phút\nThượng đế: 160 sức mạnh mỗi phút"
+ "Khỉ Bubbles: 320 sức mạnh mỗi phút\nThần Vũ Trụ: 640 sức mạnh mỗi phút\nTổ sư Kaio: 1280 sức mạnh mỗi phút\n"
+ "Có thể tặng ngọc để thắng mà không cần thách đấu\n"
+ "Nếu đăng ký tập thường xuyên mỗi khi Offline không cần phải đến đây vẫn tập luyện được";
character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, huongdanthem, true));
break;
                       case 1:
                       character.DataPractice.TrainStatus =
                                AutoTrain_Status.AUTO_TRAIN;
                            character.DataPractice.MapPracticeId = character.InfoChar.MapId;
                       character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Từ giờ, quá 30p Offline con sẽ được tự động luyện tập !"));
                    break;
                }
                break;
            case 6:
            switch(select){
                case 0:
                            character.DataPractice.Handler.Practice(character, Practice_Progress.THAN_MEO_KARIN);
                break;
            }
            break;
            case 7:
            switch(select){
                case 0:
                            character.DataPractice.Handler.Challenge(character, Practice_Progress.THAN_MEO_KARIN);
                break;
            }
            break;
                case 0:
                {
                        switch (select)
                        {
                            case 0:
                                {
                                    if (character.DataPractice.isAutoTrain())
                                        {
                                            character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Con đã hủy thành công đăng ký tập tự động !\ntừ giờ còn muốn tập Offline hãy tự đến đây trước"));
                                        character.DataPractice.TrainStatus = AutoTrain_Status.NORMAL;
                                    }
                                    else
                                        {
                                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, $"Đăng ký để mỗi khi Offline quá 30 phút, con sẽ được tự động luyện tập với tốc độ\n{character.DataPractice.GetPotenial()} sức mạnh mỗi phút", new List<string> { "Hướng\ndẫn\nthêm", "Đồng ý\n1 ngọc\nmỗi lần", "Không\nĐồng ý" }, character.InfoChar.Gender));
                                            character.TypeMenu = 5;
                                        }
                                    
                                    break;
                                }
                            case 1: // nhiem vu
                                if (TaskHandler.CheckTask(character, 30, 0))
                                {
                                    TaskHandler.gI().PlusSubTask(character, 1);
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Mau tập luyện để đánh bại Tàu Pảy Pảy", true));
                                }
                                break;                       
                            case 2://tap luyen voi than meo
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, $"Con có chắc muốn tập luyện?"
                            +$"\nTập luyện với ta sẽ tăng 20 sức mạnh mỗi phút", new List<string>{"Đồng ý\nluyện tập", "Không\nđồng ý"}, character.InfoChar.Gender));
                            character.TypeMenu = 6;
                            break;
                            case 3://thach dau than meo
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, $"Con có chắc muốn thách đấu?"
                            +$"\nNếu thắng ta sẽ được tập với Yajiro, tăng 40 sức mạnh mỗi phút", new List<string>{"Đồng ý\ngiao đấu", "Không\nđồng ý"}, character.InfoChar.Gender));
                            character.TypeMenu = 7;
                            break;     
                        }
                    break;
                }
                case 1:
                {
                        switch (select)
                        {
                            case 0:
                                character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Tập luyện vẫn tiếp tục và sức mạnh vẫn tăng khi đã Offline"));
                                break;
                            case 1:
                                {
                                    //if (!character.DataTraining.DataTraining.isTraining)
                                    //{
                                    //    if (character.AllDiamondLock() < 1)
                                    //    {
                                    //        character.CharacterHandler.SendMessage(Service.ServerMessage("Yêu cầu có 1 hồng ngọc !"));
                                    //        return;
                                    //    }
                                    //    else
                                    //    {
                                    //        character.DataTraining.DataTraining.isTraining = true;
                                    //        character.DataTraining.DataTraining.Potetinal = 80;
                                    //        character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Từ giờ, quá 30p Offline con sẽ được tự động luyện tập !"));
                                    //    }
                                    //}

                                    break;
                                }
                        }
                        break;
                }
                case 2://tự chọn lồng đèn
                {
                    switch (select)
                    {
                        case 0:
                                {
                                    if (character.StatusCDRD is Character.StatusConDuongRanDoc.JOIN )
                                    {
                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Hãy mau bay xuống chân tháp Karin"));
                                        character.StatusCDRD = Character.StatusConDuongRanDoc.TALK_MEO_KARIN;
                                    }
                                    else
                                    {
                                        var ldKeoQuan = ItemCache.GetItemDefault((short)469);
                                        ldKeoQuan.Quantity = 1;
                                        character.CharacterHandler.AddItemToBag(true, ldKeoQuan, "Đổi điểm sự kiện tt tren 1k");
                                    }
                                    break;
                                }
                        case 1:
                        {
                            var ldOngSao = ItemCache.GetItemDefault((short)467);
                            ldOngSao.Quantity = 1;
                            character.CharacterHandler.AddItemToBag(true, ldOngSao, "Đổi điểm sự kiện tt tren 1k");
                            break;
                        }
                        case 2:
                        {
                            var ldCaChep = ItemCache.GetItemDefault((short)468);
                            ldCaChep.Quantity = 1;
                            character.CharacterHandler.AddItemToBag(true, ldCaChep, "Đổi điểm sự kiện tt tren 1k");
                            break;
                        }
                        case 3:
                        {
                            var ldConGa = ItemCache.GetItemDefault((short)802);
                            ldConGa.Quantity = 1;
                            character.CharacterHandler.AddItemToBag(true, ldConGa, "Đổi điểm sự kiện tt tren 1k");
                            break;
                        }
                        case 4:
                        {
                            var ldHoiAn = ItemCache.GetItemDefault((short)471);
                            ldHoiAn.Quantity = 1;
                            character.CharacterHandler.AddItemToBag(true, ldHoiAn, "Đổi điểm sự kiện tt tren 1k");
                            break;
                        }
                        default:
                        {
                            var ldKeoQuan = ItemCache.GetItemDefault((short)469);
                            ldKeoQuan.Quantity = 1;
                            character.CharacterHandler.AddItemToBag(true, ldKeoQuan, "Đổi điểm sự kiện tt tren 1k");
                            break;
                        }
                    }
                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                    break;
                }
            }
        }

        private static void ConfirmThuongDe(Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                case 5:
                    switch (select)
                    {
                        case 0:
                            var huongdanthem = "Tập luyện vẫn tiếp tục và sức mạnh vẫn tăng khi đã Offline\n"
        + "Hiệu quả tập luyện như sau:\nThần Mèo: 20 sức mạnh mỗi phút\nYajirô: 40 sức mạnh mỗi phút\nMr.PôPô: 80 sức mạnh mỗi phút\nThượng đế: 160 sức mạnh mỗi phút"
        + "Khỉ Bubbles: 320 sức mạnh mỗi phút\nThần Vũ Trụ: 640 sức mạnh mỗi phút\nTổ sư Kaio: 1280 sức mạnh mỗi phút\n"
        + "Có thể tặng ngọc để thắng mà không cần thách đấu\n"
        + "Nếu đăng ký tập thường xuyên mỗi khi Offline không cần phải đến đây vẫn tập luyện được";
                            character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, huongdanthem, true));
                            break;
                        case 1:
                            character.DataPractice.TrainStatus =
                                                           AutoTrain_Status.AUTO_TRAIN;
                            character.DataPractice.MapPracticeId = character.InfoChar.MapId;
                            character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Từ giờ, quá 30p Offline con sẽ được tự động luyện tập !"));
                            break;
                    }
                    break;
                //Menu ban đầu
                case 0:
                {
                    switch (select)
                    {
                            case 1:// tap luyen voi mr popo
                                character.DataPractice.Handler.Practice(character, Practice_Progress.MR_POPO);
                                break;
                            case 2:// tap luyen voi thuong de
                                character.DataPractice.Handler.Practice(character, Practice_Progress.THUONG_DE);

                                break;
                            case 0:
                                switch (character.InfoChar.MapId)
                                {
                                    case 141:

                                        MapManager.GetMapOffline(45).JoinZone(character, character.Id);
                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Hãy xuống gặp thần mèo Karin"));
                                        character.StatusCDRD = Character.StatusConDuongRanDoc.JOIN;
                                        break;
                                    default:
                                        if (character.DataPractice.isAutoTrain())
                                        {
                                            character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Con đã hủy thành công đăng ký tập tự động !\ntừ giờ còn muốn tập Offline hãy tự đến đây trước"));
                                            character.DataPractice.TrainStatus = AutoTrain_Status.NORMAL;
                                        }
                                        else
                                        {
                                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, $"Đăng ký để mỗi khi Offline quá 30 phút, con sẽ được tự động luyện tập với tốc độ\n{character.DataPractice.GetPotenial()} sức mạnh mỗi phút", new List<string> { "Hướng\ndẫn\nthêm", "Đồng ý\n1 ngọc\nmỗi lần", "Không\nĐồng ý" }, character.InfoChar.Gender));
                                            character.TypeMenu = 5;
                                        }
                                        break;
                                }
                                break;
                        //Quay ngọc may mắn
                        case 4:
                                {
                                    var menu = MenuNpc.Gi().MenuThuongDe[2].ToList();
                                    if (character.LuckyBox.Count > 0)
                                    {
                                        menu.Add($"Rương phụ\n{character.LuckyBox.Count}\nmón");
                                        menu.Add($"Đóng");
                                    }
                                    else
                                    {
                                        menu.Add($"Đóng");
                                    }
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextThuongDe[2], menu, character.InfoChar.Gender));
                                    character.TypeMenu = 1;
                                   // character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Chuc nang tam dong !"));
                                    break;
                                }
                        case 3://Đến Kaio
                        {
                                    MapManager.GetMapOffline(48).JoinZone(character, character.Id);
                            break;
                        }
                    }
                    break;
                }
                case 4:
                    {
                        switch (select)
                        {
                            case 1: // tap lueyn voi thuong de
                                character.DataPractice.Handler.Practice(character, Practice_Progress.THUONG_DE);
                                break;
                            case 2://thach dau voi thuong de
                                character.DataPractice.Handler.Challenge(character, Practice_Progress.THUONG_DE);

                                break;
                            case 0:
                                switch (character.InfoChar.MapId)
                                {
                                    case 141:

                                        MapManager.GetMapOffline(45).JoinZone(character, character.Id);
                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Hãy xuống gặp thần mèo Karin"));
                                        character.StatusCDRD = Character.StatusConDuongRanDoc.JOIN;
                                        break;
                                    default:
                                        if (character.DataPractice.isAutoTrain())
                                        {
                                            character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Con đã hủy thành công đăng ký tập tự động !\ntừ giờ còn muốn tập Offline hãy tự đến đây trước"));
                                        character.DataPractice.TrainStatus = AutoTrain_Status.NORMAL;
                                        }
                                        else
                                        {
                                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, $"Đăng ký để mỗi khi Offline quá 30 phút, con sẽ được tự động luyện tập với tốc độ\n{character.DataPractice.GetPotenial()} sức mạnh mỗi phút", new List<string> { "Hướng\ndẫn\nthêm", "Đồng ý\n1 ngọc\nmỗi lần", "Không\nĐồng ý" }, character.InfoChar.Gender));
                                            character.TypeMenu = 5;
                                        }
                                        break;
                                }
                                break;
                            //Quay ngọc may mắn
                            case 4:
                                {
                                    var menu = MenuNpc.Gi().MenuThuongDe[2].ToList();
                                    if (character.LuckyBox.Count > 0)
                                    {
                                        menu.Add($"Rương phụ\n{character.LuckyBox.Count}\nmón");
                                        menu.Add($"Đóng");
                                    }
                                    else
                                    {
                                        menu.Add($"Đóng");
                                    }
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextThuongDe[2], menu, character.InfoChar.Gender));
                                    character.TypeMenu = 1;

                                    break;
                                }
                            case 3://Đến Kaio
                                {
                                    MapManager.GetMapOffline(48).JoinZone(character, character.Id);
                                    break;
                                }
                            case 5:
                                character.CharacterHandler.SendMessage(Server.Gi().BangXepHang.ListTopQuayThuong());
                                break;
                        }
                        break;
                    }
                case 3:
                    {
                        switch (select)
                        {
                            case 1: // tap lueyn voi mr popo
                                character.DataPractice.Handler.Practice(character, Practice_Progress.MR_POPO);
                                break;
                            case 2://thach dau voi mr popo
                                character.DataPractice.Handler.Challenge(character, Practice_Progress.MR_POPO);

                                break;
                            case 0:
                                switch (character.InfoChar.MapId)
                                {
                                    case 141:

                                        MapManager.GetMapOffline(45).JoinZone(character, character.Id);
                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Hãy xuống gặp thần mèo Karin"));
                                        character.StatusCDRD = Character.StatusConDuongRanDoc.JOIN;
                                        break;
                                    default:
                                        if (character.DataPractice.isAutoTrain())
                                        {
                                            character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Con đã hủy thành công đăng ký tập tự động !\ntừ giờ còn muốn tập Offline hãy tự đến đây trước"));
                                        character.DataPractice.TrainStatus = AutoTrain_Status.NORMAL;
                                        }
                                        else
                                        {
                                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, $"Đăng ký để mỗi khi Offline quá 30 phút, con sẽ được tự động luyện tập với tốc độ\n{character.DataPractice.GetPotenial()} sức mạnh mỗi phút", new List<string> { "Hướng\ndẫn\nthêm", "Đồng ý\n1 ngọc\nmỗi lần", "Không\nĐồng ý" }, character.InfoChar.Gender));
                                            character.TypeMenu = 5;
                                        }
                                        break;
                                }
                                break;
                            //Quay ngọc may mắn
                            case 4:
                                {
                                    var menu = MenuNpc.Gi().MenuThuongDe[2].ToList();
                                    if (character.LuckyBox.Count > 0)
                                    {
                                        menu.Add($"Rương phụ\n{character.LuckyBox.Count}\nmón");
                                        menu.Add($"Đóng");
                                    }
                                    else
                                    {
                                        menu.Add($"Đóng");
                                    }
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextThuongDe[2], menu, character.InfoChar.Gender));
                                    character.TypeMenu = 1;

                                    break;
                                }
                            case 3://Đến Kaio
                                {
                                    MapManager.GetMapOffline(48).JoinZone(character, character.Id);

                                    break;
                                }
                        }
                        break;
                    }
                //Quay ngọc may mắn
                case 1:
                {
                    switch (select)
                    {
                        case 0:
                        {
                            if (character.LuckyBox.Count >= DataCache.LIMIT_SLOT_RUONG_PHU_THUONG_DE)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().FULL_LUCKY_BOX));
                                break;
                            }
                            character.CharacterHandler.SendMessage(Service.LuckRoll0());
                            character.ShopId = 0;
                            break;
                        }
                        case 1:
                        {
                                    if (character.LuckyBox.Count >= DataCache.LIMIT_SLOT_RUONG_PHU_THUONG_DE)
                                    {
                                        character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().FULL_LUCKY_BOX));
                                        break;
                                    }
                                    character.CharacterHandler.SendMessage(Service.LuckRoll0());
                                    character.ShopId = 0;
                                    break;
                         }
                        case 2:
                        {
                            var luckRoll = character.LuckyBox;
                            if (character.LuckyBox.Count > 0)
                            {
                                character.CharacterHandler.SendMessage(Service.SubBox(luckRoll));
                                character.ShopId = 1111;
                            } 
                            break;
                        }
                       
                    }

                    break;
                }
               
            }
        }
        
        private static void ConfirmThanVuTru(Character character, short npcId, int select)
        {
            Server.Gi().Logger.Print(character.TypeMenu + "");
            switch (character.TypeMenu)
            {
                //Menu ban đầu
                case 6:
                    switch (select)
                    {
                        case 0:
                            var huongdanthem = "Tập luyện vẫn tiếp tục và sức mạnh vẫn tăng khi đã Offline\n"
        + "Hiệu quả tập luyện như sau:\nThần Mèo: 20 sức mạnh mỗi phút\nYajirô: 40 sức mạnh mỗi phút\nMr.PôPô: 80 sức mạnh mỗi phút\nThượng đế: 160 sức mạnh mỗi phút"
        + "Khỉ Bubbles: 320 sức mạnh mỗi phút\nThần Vũ Trụ: 640 sức mạnh mỗi phút\nTổ sư Kaio: 1280 sức mạnh mỗi phút\n"
        + "Có thể tặng ngọc để thắng mà không cần thách đấu\n"
        + "Nếu đăng ký tập thường xuyên mỗi khi Offline không cần phải đến đây vẫn tập luyện được";
                            character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, huongdanthem, true));
                            break;
                        case 1:
                            character.DataPractice.TrainStatus =
                                                           AutoTrain_Status.AUTO_TRAIN;
                            character.DataPractice.MapPracticeId = character.InfoChar.MapId;
                            character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Từ giờ, quá 30p Offline con sẽ được tự động luyện tập !"));
                            break;
                    }
                    break;
                case 4:
                    switch (select)
                    {
                        case 0:
                            if (character.DataPractice.isAutoTrain())
                            {
                                character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Con đã hủy thành công đăng ký tập tự động !\ntừ giờ còn muốn tập Offline hãy tự đến đây trước"));
                                character.DataPractice.TrainStatus =
                                                           AutoTrain_Status.NORMAL;
                            }
                            else
                            {
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, $"Đăng ký để mỗi khi Offline quá 30 phút, con sẽ được tự động luyện tập với tốc độ\n{character.DataPractice.GetPotenial()} sức mạnh mỗi phút", new List<string> { "Hướng\ndẫn\nthêm", "Đồng ý\n1 ngọc\nmỗi lần", "Không\nĐồng ý" }, character.InfoChar.Gender));
                                character.TypeMenu = 6;
                            }
                            break;
                        case 1://tap luyen voi khi bubblesb
                            character.DataPractice.Handler.Practice(character, Practice_Progress.KHI_BUBBLES);

                            break;
                        case 2://thach dau voi khi bubbles
                            character.DataPractice.Handler.Challenge(character, Practice_Progress.KHI_BUBBLES);

                            break;
                        case 3:
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Ta sẽ đưa con đi", new List<string> { "Về\nthần điện", "Thánh địa\nKaio", "Con\nđường\nrắc độc", "Từ chối" }, character.InfoChar.Gender));
                            character.TypeMenu = 1;
                            break;
                    }
                    break;
                case 5:
                    switch (select)
                    {
                        case 0:
                            if (character.DataPractice.isAutoTrain())
                            {
                                character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Con đã hủy thành công đăng ký tập tự động !\ntừ giờ còn muốn tập Offline hãy tự đến đây trước"));
                                character.DataPractice.TrainStatus =
                                                                                          AutoTrain_Status.NORMAL;
                            }
                            else
                            {
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, $"Đăng ký để mỗi khi Offline quá 30 phút, con sẽ được tự động luyện tập với tốc độ\n{character.DataPractice.GetPotenial()} sức mạnh mỗi phút", new List<string> { "Hướng\ndẫn\nthêm", "Đồng ý\n1 ngọc\nmỗi lần", "Không\nĐồng ý" }, character.InfoChar.Gender));
                                character.TypeMenu = 6;
                            }
                            break;
                        case 1://tap luyen voi kaio
                            character.DataPractice.Handler.Practice(character, Practice_Progress.KAIO);

                            break;
                        case 2://thach dau voi kaio
                            character.DataPractice.Handler.Challenge(character, Practice_Progress.KAIO);

                            break;
                        case 3:
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Ta sẽ đưa con đi", new List<string> { "Về\nthần điện", "Thánh địa\nKaio", "Con\nđường\nrắc độc", "Từ chối" }, character.InfoChar.Gender));
                            character.TypeMenu = 1;
                            break;
                    }
                    break;
                case 0:
                    {
                        switch (select)
                        {
                            case 0://tu dong luyen tap
                                if (character.DataPractice.isAutoTrain())
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Con đã hủy thành công đăng ký tập tự động !\ntừ giờ còn muốn tập Offline hãy tự đến đây trước"));
                                    character.DataPractice.TrainStatus =
                                                                                               AutoTrain_Status.NORMAL;
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, $"Đăng ký để mỗi khi Offline quá 30 phút, con sẽ được tự động luyện tập với tốc độ\n{character.DataPractice.GetPotenial()} sức mạnh mỗi phút", new List<string> { "Hướng\ndẫn\nthêm", "Đồng ý\n1 ngọc\nmỗi lần", "Không\nĐồng ý" }, character.InfoChar.Gender));
                                    character.TypeMenu = 6;
                                }
                                break;
                            case 1://handle practice with khi bubbles
                                character.DataPractice.Handler.Practice(character, Practice_Progress.KHI_BUBBLES);
                                break;
                            case 2://handle practice with kaio
                                character.DataPractice.Handler.Practice(character, Practice_Progress.KAIO);

                                break;
                            case 3:
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Ta sẽ đưa con đi", new List<string> { "Về\nthần điện", "Thánh địa\nKaio", "Con\nđường\nrắc độc", "Từ chối" }, character.InfoChar.Gender));
                                character.TypeMenu = 1;
                                break;
                        }
                        break;
                    }
                
                case 1:
                    switch (select)
                    {
                        case 0:
                            {
                                MapManager.GetMapOffline(45).JoinZone(character, character.Id);

                            }
                            break;
                        case 1:
                            {
                                MapManager.GetMapOffline(50).JoinZone(character, character.Id);

                            }
                            break;
                        case 2:
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Hãy mau trở về bằng con đường rắn độc\nBọn xayda đã đến Trái Đất!.", new List<string> {"TOP\nBang hội", "Thành tích\nBang", "Chọn\nCấp độ", "Từ chối" }, character.InfoChar.Gender));
                            character.TypeMenu = 2;
                            break;
                    }
                    break;
                case 2:
                    switch (select)
                    {
                        case 2:
                            var clan = ClanManager.Get(character.ClanId);
                            if (clan.CondititonToJoinDungeon(character, clan, npcId))
                            {
                                if (character.InfoChar.Power < 80000000000)
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Yêu cầu sức mạnh trên 80 tỉ"));
                                }
                                else if (clan.ClanDungeon.ConDuongRanDoc.CheckOpen())
                                {
                                    var timing = (clan.ClanDungeon.ConDuongRanDoc.Time - ServerUtils.CurrentTimeMillis()) / 1000 >= 60 ? $"({(clan.ClanDungeon.ConDuongRanDoc.Time - ServerUtils.CurrentTimeMillis()) / 60000} phút trước)" : $"({(clan.ClanDungeon.ConDuongRanDoc.Time - ServerUtils.CurrentTimeMillis()) / 1000} giây trước)";
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Bang hội con đang ở con đường rắn độc cấp độ " + clan.ClanDungeon.ConDuongRanDoc.Time + "\nCon có muốn đi cùng họ không? " + timing, new List<string> { "Top\nBang hội", "Thành tích\nBang", "Đồng ý", "Từ chối" }, character.InfoChar.Gender));
                                    character.TypeMenu = 3;
                                }else if (clan.ClanDungeon.ConDuongRanDoc.Count == 0)
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Lượt đi hôm nay đã hết, mai hãy quay lại"));

                                }
                                else
                                {
                                    //   character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Chức năng sẽ mở khóa khi Open"));
                                    //return;
                                    var intputCDRD = new List<InputBox>();
                                    var inputLevel = new InputBox()
                                    {
                                        Name = "Cấp độ",
                                        Type = 1,
                                    };
                                    intputCDRD.Add(inputLevel);
                                    character.CharacterHandler.SendMessage(Service.ShowInput("Hãy chọn cấp từ 1 -> 110", intputCDRD));
                                    character.TypeInput = 14;
                                }
                            }
                            break;
                    }
                    break;
                case 3:
                    switch (select)
                    {
                        case 2:
                            var clan = ClanManager.Get(character.ClanId);
                            
                            character.InfoChar.X = 1103;
                            character.InfoChar.Y = 336;
                            var mapCDRD = MapManager.Get(143);
                            mapCDRD.JoinZone(character, character.ClanId);
                            break;
                    }
                    break;
            }
            
        }
        public static void HandlerGhepTrangBiThienSu(Character character, int idAngelPiece, int gender, int percentMayMan)
        {
            var Item = ItemCache.GetItemDefault(1);
            var type = 0;
            switch (idAngelPiece)
            {
                case 1066:
                    type = 0;
                    break;
                case 1067:
                    type = 1;
                       break;
                case 1068:
                    type = 3;
                    break;
                case 1069:
                    type = 4;
                    break;
                case 1070:
                    type = 2;
                    break;
                

            }
            switch (type)
            {
                case 0:
                    Item = ItemCache.GetItemDefault((short)(1048 + gender));
                    break;
                case 1:
                    Item = ItemCache.GetItemDefault((short)(1051 + gender));
                    break;
                case 2:
                    Item = ItemCache.GetItemDefault((short)(1054 + gender));
                    break;
                case 3:
                    Item = ItemCache.GetItemDefault((short)(1057 + gender));
                    break;
                case 4:
                    Item = ItemCache.GetItemDefault((short)(1060 + gender));
                    break;
            }
            var random = ServerUtils.RandomNumber(100);
            if (random <= percentMayMan)
            {
                if (percentMayMan >= 60)
                {
                    Item.Options[0].Param += (Item.Options[0].Param * ServerUtils.RandomNumber(20, 35)) / 100;
                }else
                Item.Options[0].Param += (Item.Options[0].Param * ServerUtils.RandomNumber(1, 35)) / 100;
            }
            var chisothuong = ServerUtils.RandomNumber(1, 5);
            Item.Options.Add(new OptionItem()
            {
                Id = 41,
                Param = chisothuong,
            });
            for (int i = 0; i < chisothuong; i++)
            {
                Item.Options.Add(new OptionItem()
                {
                    Id = ServerUtils.RandomNumber(42,47),
                    Param = ServerUtils.RandomNumber(1,6),
                });
            }
            character.CharacterHandler.RemoveItemBagById(457, 200);
            character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
            character.CharacterHandler.AddItemToBag(false, Item);
            character.CharacterHandler.SendMessage(Service.SendBag(character));
            character.CharacterHandler.SendMessage(Service.SendCombinne5(ItemCache.ItemTemplate(Item.Id).IconId));

            var ItemSave = new List<int>();
            ItemSave.Add(Item.IndexUI);
            character.CharacterHandler.SendMessage(Service.SendCombinne1(ItemSave));
        }
        private static void ConfirmBaHatMit(Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                case 23:
                    switch (select)
                    {
                        case 0:
                            character.DataVoDaiSinhTu.Reward = true;
                            var vetinh = ItemCache.GetItemDefault((short)(DataCache.IdVeTinh[ServerUtils.RandomNumber(DataCache.IdVeTinh.Count)]));
                            character.CharacterHandler.AddItemToBag(true, vetinh);
                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Chúc mừng bạn đã nhận được " + ItemCache.ItemTemplate(vetinh.Id).Name));
                            break;
                        case 1:
                            character.DataVoDaiSinhTu.Reward = true;
                            var amulet = (short)DataCache.IdAmulet[ServerUtils.RandomNumber(DataCache.IdAmulet.Count)];
                            var itemAmulet = ItemCache.ItemTemplate(amulet);
                            if (character.InfoChar.ItemAmulet.ContainsKey(amulet))
                            {
                                if (character.InfoChar.ItemAmulet[amulet] < ServerUtils.CurrentTimeMillis())
                                {
                                    character.InfoChar.ItemAmulet[amulet] = DataCache._1HOUR + ServerUtils.CurrentTimeMillis();
                                }
                                else
                                {
                                    character.InfoChar.ItemAmulet[amulet] += DataCache._1HOUR;
                                }
                            }
                            else
                            {
                                character.InfoChar.ItemAmulet.TryAdd(amulet, DataCache._1HOUR + ServerUtils.CurrentTimeMillis());
                            }
                            character.SetupAmulet();
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Chúc mừng bạn đã nhận được " + itemAmulet.Name));
                            break;
                    }
                    break;
                case 22:
                    switch (select)
                    {   
                        case 0: // top 100
                            break;
                        case 1: // dong y
                            DiedRing_Service.Register(character);
                            break;
                        case 2: // tu choi
                            break;
                        case 3: // ve dao kame
                            MapManager.JoinMap(character, 5, ServerUtils.RandomNumber(20), true, true, character.TypeTeleport);
                            break;
                    }
                    break;
                case 21:
                    {
                        switch (select)
                        {
                            case 0:
                                {
                                    var IndexAngelPiece = character.CharacterHandler.GetItemBagByIndex(character.CombinneIndex[0]);
                                    var IndexCongThuc = character.CharacterHandler.GetItemBagByIndex(character.CombinneIndex[1]);
                                    var UseDaNangCap = character.CombinneIndex[2] == 1;
                                    var UseDaMayMan = character.CombinneIndex[3] == 1;
                                    var IndexDaNangCap = character.CharacterHandler.GetItemBagByIndex(character.CombinneIndex[4]);
                                    var IndexDaMayMan = character.CharacterHandler.GetItemBagByIndex(character.CombinneIndex[5]);
                                    var PercentNangCap = character.CombinneIndex[6];
                                    var PercentMayMan = character.CombinneIndex[7];
                                    // index 2 = check use da nang cap
                                    // index 3 = check use da may man
                                    // index 4 = index da nang cap
                                    // index 5 = index da may man
                                    if (ServerUtils.RandomNumber(100) <= PercentNangCap)
                                    {
                                        character.CharacterHandler.RemoveItemBagByIndex(IndexAngelPiece.IndexUI, 9999);
                                        character.CharacterHandler.RemoveItemBagByIndex(IndexCongThuc.IndexUI, 1);
                                        if (UseDaNangCap) character.CharacterHandler.RemoveItemBagByIndex(IndexDaNangCap.IndexUI, 1);
                                        if (UseDaMayMan) character.CharacterHandler.RemoveItemBagByIndex(IndexDaMayMan.IndexUI, 1);
                                        HandlerGhepTrangBiThienSu(character, IndexAngelPiece.Id, ItemCache.ItemTemplate(IndexCongThuc.Id).Gender, PercentMayMan);
                                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                                    }
                                    else
                                    {
                                        character.CharacterHandler.RemoveItemBagByIndex(IndexAngelPiece.IndexUI, 999);
                                        character.CharacterHandler.RemoveItemBagByIndex(IndexCongThuc.IndexUI, 1);
                                        if (UseDaNangCap) character.CharacterHandler.RemoveItemBagByIndex(IndexDaNangCap.IndexUI, 1);
                                        if (UseDaMayMan) character.CharacterHandler.RemoveItemBagByIndex(IndexDaMayMan.IndexUI, 1);
                                        character.MineGold(200000000);
                                        character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                                        character.CharacterHandler.SendMessage(Service.SendCombinne3());
                                        character.CharacterHandler.SendMessage(Service.NpcChat(npcId, "Chúc con may mắn lần sau !"));
                                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                                        var itemSave = new List<int>();
                                        itemSave.Add(IndexAngelPiece.IndexUI);
                                        character.CharacterHandler.SendMessage(Service.SendCombinne1(itemSave));
                                    }
                                    character.CombinneIndex.Clear();
                                    character.CombinneIndex = null;
                                    break;
                                }

                        }
                        break;
                    }
                case 20:

                    {
                        switch (select)
                        {
                            case 0:
                                var listArray = character.CombinneIndex;
                                var item = character.CharacterHandler.GetItemBagByIndex(listArray[0]);
                                var dns = character.CharacterHandler.GetItemBagByIndex(listArray[1]);
                                character.CharacterHandler.RemoveItemBagById(dns.Id, 1);
                                character.CharacterHandler.SendMessage(Service.SendCombinne2());
                                var listOpt = new List<int> { 34, 35, 36 };

                                item.Options.Add(new OptionItem()
                                {
                                    Id = listOpt[ServerUtils.RandomNumber(listOpt.Count)],
                                    Param = 0
                                });
                                
                                character.CharacterHandler.SendMessage(Service.SendBag(character));
                                //character.CharacterHandler.SendMessage(Service.SendBody(character));
                                var listIndex = new List<int>();
                                listIndex.Add(item.IndexUI);
                                character.CharacterHandler.SendMessage(Service.SendCombinne1(listIndex));
                                character.CombinneIndex.Clear();
                                character.CombinneIndex = null;
                                break;
                        }
                    }
                    break;
                case 33:
                    {
                        switch (select)
                        {
                            case 0:
                                var indexSplC2 = character.CombinneIndex[0];
                                character.MineGold(1000000);
                                character.CharacterHandler.RemoveItemBagByIndex(indexSplC2, 5);
                                if (ServerUtils.RandomNumber(100) < 50)
                                {
                                    character.CharacterHandler.SendMessage(Service.SendCombinne2());
                                    character.CharacterHandler.AddItemToBag(true, ItemCache.GetItemDefault(1464));
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(Service.SendCombinne3());
                                }
                                character.CharacterHandler.SendMessage(Service.SendBag(character));

                                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                var listIndex = new List<int>();
                                listIndex.Add(indexSplC2);
                                character.CharacterHandler.SendMessage(Service.SendCombinne1(listIndex));
                                character.CombinneIndex.Clear();
                                character.CombinneIndex = null;
                                break;
                        }
                        break;
                    }
                case 34:
                    {
                        switch (select)
                        {
                            case 0:
                                var indexSplC1 = character.CombinneIndex[0];
                                var indexHametite = character.CombinneIndex[1];
                                character.MineGold(100000000);
                                character.MineDiamond(50);
                                character.CharacterHandler.RemoveItemBagByIndex(indexSplC1, 1);
                                character.CharacterHandler.RemoveItemBagByIndex(indexHametite, 1);
                                var listIndex = new List<int>();
                                if (ServerUtils.RandomNumber(100) < 50)
                                {
                                    var idSplC2 = CombineHandler.ConvertLevel1SaoPhaLeToLevel2SaoPhaLe(character.CharacterHandler.GetItemBagByIndex(indexSplC1).Id);
                                    var SplC2 = ItemCache.GetItemDefault((short)idSplC2);
                                    character.CharacterHandler.SendMessage(Service.SendCombinne2());
                                    character.CharacterHandler.AddItemToBag(true, SplC2);
                                    listIndex.Add(character.CharacterHandler.GetItemBagById(idSplC2).IndexUI);
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(Service.SendCombinne3());
                                }
                                character.CharacterHandler.SendMessage(Service.SendBag(character));

                                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                character.CharacterHandler.SendMessage(Service.SendCombinne1(listIndex));
                                character.CombinneIndex.Clear();
                                character.CombinneIndex = null;
                                break;
                        }
                        break;
                    }
                case 36:
                    {
                        switch (select)
                        {
                            case 0:
                                var indexSplC2 = character.CombinneIndex[0];
                                var indexDaMai = character.CombinneIndex[1];
                                character.MineGold(100000000);
                                character.MineDiamond(50);
                                character.CharacterHandler.RemoveItemBagByIndex(indexSplC2, 2);
                                character.CharacterHandler.RemoveItemBagByIndex(indexDaMai, 1);
                                var listIndex = new List<int>();
                                if (ServerUtils.RandomNumber(100) < 50)
                                {
                                    var idSplC2 = CombineHandler.ConvertLevel2SaoPhaLeToVipSaoPhaLe(character.CharacterHandler.GetItemBagByIndex(indexSplC2).Id);
                                    var SplC2 = ItemCache.GetItemDefault((short)idSplC2);
                                    character.CharacterHandler.SendMessage(Service.SendCombinne2());
                                    character.CharacterHandler.AddItemToBag(true, SplC2);
                                    listIndex.Add(character.CharacterHandler.GetItemBagById(idSplC2).IndexUI);
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(Service.SendCombinne3());
                                }
                                character.CharacterHandler.SendMessage(Service.SendBag(character));

                                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                character.CharacterHandler.SendMessage(Service.SendCombinne1(listIndex));
                                character.CombinneIndex.Clear();
                                character.CombinneIndex = null;
                                break;
                        }
                        break;
                    }
                case 35:
                    {
                        switch (select)
                        {
                            case 0:
                                var indexItem = character.CombinneIndex[0];
                                var indexHametite = character.CombinneIndex[1];
                                var indexDuiDuc = character.CombinneIndex[2];
                                var item = character.CharacterHandler.GetItemBagByIndex(indexItem);
                                character.MineGold(100000000);
                                character.MineDiamond(50);
                                character.CharacterHandler.RemoveItemBagByIndex(indexDuiDuc, 1);
                                character.CharacterHandler.RemoveItemBagByIndex(indexHametite, 1);
                                var listIndex = new List<int>();
                                listIndex.Add(indexItem);
                                if (ServerUtils.RandomNumber(100) < 50)
                                {
                                    if (!item.isHaveOption(228))
                                    {
                                        item.Options.Add(new OptionItem()
                                        {
                                            Id = 228,
                                            Param = 8,
                                        });
                                    }
                                    else
                                    {
                                        item.GetOption(228).Param++;
                                    }
                                    character.CharacterHandler.SendMessage(Service.SendCombinne2());
                                    
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(Service.SendCombinne3());
                                }
                                    character.CharacterHandler.SendMessage(Service.SendBag(character));

                                    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                    character.CharacterHandler.SendMessage(Service.SendCombinne1(listIndex));
                                    character.CombinneIndex.Clear();
                                    character.CombinneIndex = null;
                                    break;
                                }

                        }
                        break;
                    

                case 32:
                    {
                        switch (select)
                        {
                            case 0:
                                var indexMat = character.CombinneIndex[0];
                                var indexDNS = character.CombinneIndex[1];
                                var Vang = character.CombinneIndex[2];
                                var TiLe = character.CombinneIndex[3];
                                var Ngoc = character.CombinneIndex[4];
                                var Option = character.CombinneIndex[5];
                                if (character.CharacterHandler.GetAllQuantityItemBagById(1549) < Vang)
                                {
                                    character.CharacterHandler.SendMessage(Service.DialogMessage("Không đủ vàng !"));
                                    return;
                                }

                                if (character.AllDiamondLock() < Ngoc)
                                {
                                    character.CharacterHandler.SendMessage(Service.DialogMessage("Không đủ ngọc !"));
                                    return;
                                }
                                character.MineDiamond(Ngoc, 2);
                                character.CharacterHandler.RemoveItemBagById(1549, Vang);
                                character.CharacterHandler.RemoveItemBagByIndex(indexDNS, 1);
                                if (ServerUtils.RandomNumber(100) < TiLe)
                                {
                                    var item = ItemCache.GetItemDefault((short)(character.CharacterHandler.GetItemBagByIndex(indexMat).Id + 1));
                                    if (Option > 2)
                                    {
                                        var chisothuong = Option / 2;
                                        item.Options.Add(new OptionItem()
                                        {
                                            Id = 41,
                                            Param = chisothuong
                                        });
                                        for (int i = 0; i < chisothuong; i++)
                                        {
                                            var option = DataCache.OptionPorata2[ServerUtils.RandomNumber(DataCache.OptionPorata2.Count)];
                                            item.Options.Add(new OptionItem()
                                            {
                                                Id = option[0],
                                                Param = ServerUtils.RandomNumber(option[1], option[2]),
                                            });
                                        }
                                    }
                                    character.CharacterHandler.AddItemToBag(false, item);
                                    character.CharacterHandler.RemoveItemBagByIndex(indexMat, 1);
                                    character.CharacterHandler.SendMessage(Service.SendCombinne2());
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(Service.SendCombinne3());
                                }
                                character.CharacterHandler.SendMessage(Service.SendBag(character));
                                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[29], npcId));
                                character.CharacterHandler.SendMessage(Service.SendCombinne1(new List<int> { indexMat, indexDNS }));
                                break;
                        }
                        break;
                    }
                case 31:

                    {
                        switch (select)
                        {
                            case 0:
                                var listArray = character.CombinneIndex;
                                var item = character.CharacterHandler.GetItemBagByIndex(listArray[0]);
                                var dns = character.CharacterHandler.GetItemBagByIndex(listArray[1]);
                                character.CharacterHandler.RemoveItemBagById(dns.Id, 2);
                                character.CharacterHandler.SendMessage(Service.SendCombinne2());
                                for (int i = 0; i  < item.Options.Count; i++)
                                {
                                    if (item.Options[i].Id == 34 || item.Options[i].Id == 35 || item.Options[i].Id == 36)
                                    {
                                        item.Options.Remove(item.Options[i]);
                                    }
                                }
                                character.CharacterHandler.SendMessage(Service.SendBag(character));
                                //character.CharacterHandler.SendMessage(Service.SendBody(character));
                                var listIndex = new List<int>();
                                listIndex.Add(item.IndexUI);
                                character.CharacterHandler.SendMessage(Service.SendCombinne1(listIndex));
                                character.CombinneIndex.Clear();
                                character.CombinneIndex = null;
                                break;
                        }
                    }
                    break;
                case 16:
                    {
                        switch (select)
                        {

                            case 0:
                                {
                                    var listArray = character.CombinneIndex;
                                    var item1 = character.CharacterHandler.GetItemBagByIndex(listArray[0]);
                                    var item2 = character.CharacterHandler.GetItemBagByIndex(listArray[1]);
                                    var item3 = character.CharacterHandler.GetItemBagByIndex(listArray[2]);
                                                                        var item4 = character.CharacterHandler.GetItemBagByIndex(listArray[3]);

                                    character.CharacterHandler.SendMessage(Service.SendCombinne2());
                                    HandlerGhepTrangBiHuyDiet(character, ServerUtils.RandomNumber(1,4), character.InfoChar.Gender, 1);
                                    character.CharacterHandler.RemoveItemBagByIndex(item1.IndexUI, 1, false, reason: "Null");
                                    character.CharacterHandler.RemoveItemBagByIndex(item2.IndexUI, 1, false, reason: "Nukk");
                                    character.CharacterHandler.RemoveItemBagByIndex(item3.IndexUI, 1, false, reason: "Nukk");
                                                                        character.CharacterHandler.RemoveItemBagByIndex(item4.IndexUI, 4, false, reason: "Nukk");

                                    character.MineGold(500000000);
                                    character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                                    character.CombinneIndex.Clear();
                                    character.CombinneIndex = null;
                                    break;
                                }
                        }
                        break;
                    }
                case 17:
                    {
                        switch (select)
                        {

                            case 0:
                                {
                                    var listArray = character.CombinneIndex;
                                    var item1 = character.CharacterHandler.GetItemBagByIndex(listArray[0]);
                                                                        var item2 = character.CharacterHandler.GetItemBagByIndex(listArray[1]); var item3 = character.CharacterHandler.GetItemBagByIndex(listArray[2]); var item4 = character.CharacterHandler.GetItemBagByIndex(listArray[2]);

                                    character.CharacterHandler.SendMessage(Service.SendCombinne2());
                                    HandlerGhepTrangBiHuyDiet(character, ItemCache.ItemTemplate(item1.Id).Type, ItemCache.ItemTemplate(item1.Id).Gender, 0);
                                    character.CharacterHandler.RemoveItemBagByIndex(item1.IndexUI, 1, false, reason: "Null");
                                    character.CharacterHandler.RemoveItemBagByIndex(item2.IndexUI, 1, false, reason: "Null");
                                    character.CharacterHandler.RemoveItemBagByIndex(item3.IndexUI, 1, false, reason: "Null");
                                    character.CharacterHandler.RemoveItemBagByIndex(item4.IndexUI, 1, false, reason: "Null");

                                    character.MineGold(500000000);
                                    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                                    character.CombinneIndex.Clear();
                                    character.CombinneIndex = null;
                                    break;
                                }
                        }
                        break;
                    }
                case 18:
                    {
                        switch (select)
                        {

                            case 0:
                                {
                                    var listArray = character.CombinneIndex;
                                    var item1 = character.CharacterHandler.GetItemBagByIndex(listArray[0]);
                                    character.CharacterHandler.SendMessage(Service.SendCombinne2());
                                    HandlerGhepTrangBiThanLinh(character, ItemCache.ItemTemplate(item1.Id).Type, ItemCache.ItemTemplate(item1.Id).Gender);
                                    character.CharacterHandler.RemoveItemBagByIndex(item1.IndexUI, 1, false, reason: "Null");
                                    character.MineGold(500000000);
                                    character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                                    character.CombinneIndex.Clear();
                                    character.CombinneIndex = null;
                                    break;
                                }
                        }
                        break;
                    }
                case 38:
                    {
                        switch (select)
                        {
                            case 0:
                                if (character.TypeDoiThuong is 5)
                                {
                                    if (ServerUtils.RandomNumber(100) < 20)
                                    {
                                        character.CharacterHandler.RemoveItemBagById(1316, 9999);
                                        character.CharacterHandler.AddItemToBag(true, ItemCache.GetItemDefault(1318, 1));

                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Chế tạo cuốn sách cũ thành công"));
                                    }
                                    else//failed
                                    {
                                        character.CharacterHandler.RemoveItemBagById(1316, 99);
                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Chế tạo cuốn sách cũ thất bại"));
                                    }
                                    character.CharacterHandler.RemoveItemBagById(1317, 1);
                                    character.MineGold(200000000);
                                    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                                }
                                break;
                        }
                        break;
                    }
                case 39:
                    {
                        switch (select)
                        {
                            case 0:
                                if (character.TypeDoiThuong is 6)
                                {
                                    if (ServerUtils.RandomNumber(100) < 20)
                                    {
                                        character.CharacterHandler.AddItemToBag(true, ItemCache.GetItemDefault((short)(1509 + character.InfoChar.Gender), 1));

                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Chế tạo Sách Tuyệt Kĩ 1 thành công"));
                                    }
                                    else//failed
                                    {
                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Chế tạo Sách Tuyệt Kĩ 1 thất bại"));
                                    }
                                    character.CharacterHandler.RemoveItemBagById(1318, 10);
                                    character.CharacterHandler.RemoveItemBagById(1320, 1);
                                    character.MineGold(200000000);
                                    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                                }
                                break;
                        }
                        break;
                    }
                case 40:
                    {
                        switch (select)
                        {
                            case 0:
                                character.CharacterHandler.SendMessage(Service.SendCombinne2());
                                var index = character.CombinneIndex;
                                var Sach1 = character.CharacterHandler.GetItemBagByIndex(index[0]);
                                Sach1.Options.RemoveAt(0);// xóa option chưa giám định
                                var getOption = DataCache.OptionSachTuyetKy[ServerUtils.RandomNumber(DataCache.OptionSachTuyetKy.Count)];
                                Sach1.Options.Insert(0, new OptionItem()//đẩy option vào 
                                {
                                    Id = getOption[0],
                                    Param = ServerUtils.RandomNumber(getOption[1], getOption[2]),
                                });
                                Sach1.Options.FirstOrDefault(opt => opt.Id == 211).Param++;
                                character.CharacterHandler.RemoveItemBagByIndex(index[1], 1);
                                character.CharacterHandler.SendMessage(Service.SendBag(character));
                                character.CharacterHandler.SendMessage(Service.SendCombinne1(new List<int>()));
                                break;
                        }
                        break;
                    }
                case 41:
                    {
                        switch (select)
                        {
                            case 0:
                                character.CharacterHandler.SendMessage(Service.SendCombinne2());
                                var index = character.CombinneIndex;
                                var Sach = character.CharacterHandler.GetItemBagByIndex(index[0]);
                                Sach.Options.RemoveAt(0);// xóa option
                                Sach.Options.Insert(0, new OptionItem()//đẩy option vào 
                                {
                                    Id = 217,
                                    Param = 0,
                                });
                                Sach.Options.FirstOrDefault(opt => opt.Id == 219).Param--;
                                character.CharacterHandler.SendMessage(Service.SendBag(character));
                                character.CharacterHandler.SendMessage(Service.SendCombinne1(new List<int>()));
                                break;
                        }
                        break;
                    }
                case 42:
                    {
                        switch (select)
                        {
                            case 0:
                                var index = character.CombinneIndex;
                                var Sach = character.CharacterHandler.GetItemBagByIndex(index[0]);
                                if (ServerUtils.RandomNumber(100) < 20)
                                {
                                    character.CharacterHandler.RemoveItemBagByIndex(index[0], 1);
                                    character.CharacterHandler.RemoveItemBagByIndex(index[1], 10);
                                    character.CharacterHandler.AddItemToBag(false, ItemCache.GetItemDefault((short)(1313 + ItemCache.ItemTemplate(Sach.Id).Gender)  ));
                                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                                    character.CharacterHandler.SendMessage(Service.ClosePanel());
                                    character.CharacterHandler.SendMessage(Service.SendCombinne2());
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(Service.SendCombinne3());
                                    character.CharacterHandler.RemoveItemBagByIndex(index[1], 10);
                                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                                    character.CharacterHandler.SendMessage(Service.SendCombinne1(new List<int> { index[0], index[1] }));
                                }
                               
                                
                                break;
                        }
                        break;
                    }
                case 44:
                    {
                        switch (select)
                        {
                            case 0:
                                var index = character.CombinneIndex;
                                var trangbi = character.CharacterHandler.GetItemBagByIndex(index[0]);
                                
                                if (trangbi.isHaveOption(231))
                                {
                                    trangbi.Options.FirstOrDefault(opt => opt.Id == 231).Param++;

                                }
                                else
                                {
                                    trangbi.Options.Add(new OptionItem()
                                    {
                                        Id = 231,
                                        Param = 1,
                                    });
                                }
                                var option = DataCache.OptionPhapSuTrangBi[ServerUtils.RandomNumber(DataCache.OptionPhapSuTrangBi.Count)];// id, param
                                if (trangbi.isHaveOption(option[0]))
                                {
                                    trangbi.Options.FirstOrDefault(opt => opt.Id == option[0]).Param += option[1];
                                }
                                else
                                {
                                    trangbi.Options.Add(new OptionItem()
                                    {
                                        Id = option[0],
                                        Param = option[1],
                                    });
                                }
                                character.CharacterHandler.SendMessage(Service.SendBag(character));
                                character.CharacterHandler.SendMessage(Service.SendCombinne2());
                                character.CharacterHandler.SendMessage(Service.SendCombinne1(new List<int> { index[0], index[1] }));
                                
                                


                                break;
                        }
                        break;
                    }
                case 45:
                    {
                        switch (select)
                        {
                            case 0:
                                var index = character.CombinneIndex;
                                var trangbi = character.CharacterHandler.GetItemBagByIndex(index[0]);



                                for (int i2 = 0; i2 < trangbi.Options.Count; i2++)
                                {
                                    if (DataCache.OptionIdPhapSuTrangBi.Contains(trangbi.Options[i2].Id))
                                    {
                                        trangbi.Options.RemoveAt(i2);
                                    }
                                }
                                
                                
                                character.CharacterHandler.SendMessage(Service.SendBag(character));
                                character.CharacterHandler.SendMessage(Service.SendCombinne2());
                                character.CharacterHandler.SendMessage(Service.SendCombinne1(new List<int> { index[0], index[1] }));




                                break;
                        }
                        break;
                    }
                case 37://Menu tuyệt kĩ
                    {
                        switch (select)
                        {
                            case 0:
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, DoiThuongHandler.DoiThuong(character, ServerUtils.ColorNotSpace("green") + "Chế tạo cuốn sách cũ",new List<int> { 1316, 1317, -1 }, new List<int> { 9999, 1, 200000000 }, 5, "\n|6|Tỉ lệ thành công: 20%\n","|6|Thất bại mất 99 trang sách và 1 bìa sách"), character.TypeDoiThuong != 5 ? MenuNpc.Gi().MenuBaHatMit[39] : MenuNpc.Gi().MenuBaHatMit[38], character.InfoChar.Gender));
                                character.TypeMenu = 38;
                                break;
                            case 1:
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, DoiThuongHandler.DoiThuong(character, ServerUtils.ColorNotSpace("green") + "Đổi sách tuyệt kĩ 1", new List<int> { 1318, 1320, -1 }, new List<int> { 10, 1, 200000000 }, 6, "\n|6|Tỉ lệ thành công: 20%"), character.TypeDoiThuong != 6 ? MenuNpc.Gi().MenuBaHatMit[39] : MenuNpc.Gi().MenuBaHatMit[38], character.InfoChar.Gender));
                                character.TypeMenu = 39;
                                break;
                            case 2://giám định
                                character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[40]));
                                character.ShopId = 26;
                                break;
                            case 3://tẩy sách
                                character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[41]));
                                character.ShopId = 27;
                                break;
                            case 4://nâng cấp tuyệt kĩ 2
                                character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[42]));
                                character.ShopId = 28;
                                break;
                            case 5://phục hồi độ bền
                                character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[43]));
                                character.ShopId = 29;
                                break;
                            case 6://rã sách
                                character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[44]));
                                character.ShopId = 30;
                                break;
                        }
                    }
                    break;
                //Menu vách núi
                case 0:
                    {
                        if (!character.InfoChar.IsNhanBua) select += 1;
                        switch (select)
                        {
                            //Nhận bùa miễn phí
                            case 0:
                                {
                                    var idAmulet = (short)DataCache.IdAmulet[ServerUtils.RandomNumber(DataCache.IdAmulet.Count)];
                                    var timePlus = DataCache._1HOUR;
                                    if (character.InfoChar.ItemAmulet.ContainsKey(idAmulet))
                                    {
                                        character.InfoChar.ItemAmulet[idAmulet] += timePlus;
                                    }
                                    else
                                    {
                                        character.InfoChar.ItemAmulet.TryAdd(idAmulet, timePlus + ServerUtils.CurrentTimeMillis());
                                    }
                                    character.CharacterHandler.SendMessage(Service.ServerMessage(string.Format(TextServer.gI().ADD_AMULET, ItemCache.ItemTemplate(idAmulet).Name)));
                                    character.InfoChar.IsNhanBua = false;
                                    // Setup Bùa
                                    break;
                                }
                                // sách tuyệt kĩ
                            case 1:
                                {
                                    character.CharacterHandler.SendMessage(
                                        Service
                                            .OpenUiConfirm(npcId, MenuNpc.Gi().TextBaHatMit[0], MenuNpc.Gi().MenuBaHatMit[37], character.InfoChar.Gender));
                                    character.TypeMenu = 37;
                                    break;
                                }
                            //Cửa hàng bùa
                            case 2:
                                {
                                    character.CharacterHandler.SendMessage(
                                        Service
                                            .OpenUiConfirm(npcId, MenuNpc.Gi().TextBaHatMit[0], MenuNpc.Gi().MenuBaHatMit[2], character.InfoChar.Gender));
                                    character.TypeMenu = 2;
                                    break;
                                }
                            //Nâng cấp vật phẩm
                            case 3:
                                {
                                    character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[4], 21));
                                    character.ShopId = 0;
                                    break;
                                }
                            //Làm phép nhập đá
                            case 4:
                                {
                                    character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[5]));
                                    character.ShopId = 1;
                                    break;
                                }
                            //Nhập ngọc rồng
                            case 5:
                                {
                                    character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[6]));
                                    character.ShopId = 2;
                                    break;
                                }
                            //Nâng cấp bông tai porata
                            case 6:
                                {
                                    var bongTaiPorata2 = character.CharacterHandler.GetItemBagById(921);
                                    if (bongTaiPorata2 == null)
                                    {
                                        character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[13]));
                                        character.ShopId = 7;
                                    }
                                    else
                                    {
                                        character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[14]));//14
                                        character.ShopId = 8;//8
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                case 26:
                    switch (select)
                    {
                        case 0:
                            character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[46]));
                            character.ShopId = 13;
                            break;
                        case 1:
                            character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[47]));
                            character.ShopId = 14;
                            break;
                    }
                    break;
                case 25:
                    switch (select)
                    {
                        case 0:
                            character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[24]));
                            character.ShopId = 13;
                            break;
                        case 1:
                            character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[28]));
                            character.ShopId = 14;
                            break;
                    }
                    break;
                case 24:
                    switch (select)
                    {
                        //Ép sao trang bị
                        case 0:
                            {
                                character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[11]));
                                character.ShopId = 3;
                                break;
                            }
                        //MENU - Pha lê hoá trang bị
                        case 1:
                            {
                                character.CharacterHandler.SendMessage(
                                    Service
                                        .OpenUiConfirm(npcId, MenuNpc.Gi().TextBaHatMit[2], MenuNpc.Gi().MenuBaHatMit[9], character.InfoChar.Gender));
                                character.TypeMenu = 3;
                                break;
                            }
                        case 2://nâng cấp sao pha lê
                            {
                                character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[33]));
                                character.ShopId = 23;
                                break;
                            }
                        case 3://đánh bóng sao pha lê
                            {
                                character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[34]));
                                character.ShopId = 25;
                                break;
                            }
                        case 4://Cường hóa lỗ sao pha lê
                            {
                                character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[35]));
                                character.ShopId = 24;
                                break;
                            }
                        case 5://Tạo đá Hemematite
                            {
                                character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[36]));
                                character.ShopId = 22;
                                break;
                            }
                    }
                    break;
                    //xử lí đổi thưởng
                case 43:
                    switch (character.TypeDoiThuong)
                    {
                        case 7://ghép chữ đầu năm
                            {
                                for (short i = DataCache.VanSuNhuY2024[0]; i <= DataCache.VanSuNhuY2024[DataCache.VanSuNhuY2024.Count - 1]; i++)
                                {
                                    character.CharacterHandler.RemoveItemBagById(i, 1);
                                }
                                character.CharacterHandler.AddItemToBag(true, ItemCache.GetItemDefault(1538, 1), "Ghép chữ đầu năm [Bà Hạt Mít]");
                                character.CharacterHandler.SendMessage(Service.SendBag(character));
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Chúc mừng bạn đã nhận được Phong bì Tết 2024"));
                                break;

                            }
                        case 8://bày mâm ngũ quả (1182)
                            {
                                for (short i = 1177; i <= 1181; i++)
                                {
                                    character.CharacterHandler.RemoveItemBagById(i, 20);
                                }
                                character.MineGold(5000000);
                                character.CharacterHandler.RemoveItemBagById(1183, 1);
                                character.CharacterHandler.AddItemToBag(true, ItemCache.GetItemDefault(1182));
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được Mâm ngũ quả"));
                                character.CharacterHandler.SendMessage(Service.SendBag(character));
                                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                break;
                            }
                    }
                    break;
                //Đảo kame
                case 1:
                    {
                        switch (select)
                        {
                            ////Ghép chữ đầu năm
                            case 0:
                                {
                                    var text = DoiThuongHandler.DoiThuong(character, $"{ServerUtils.ColorNotSpace("green")}Ghép chữ đầu năm\n", new List<int> { 1533, 1534, 1535, 1536, 1537 }, new List<int> { 1, 1, 1, 1, 1 }, 7);
                                    var menus = new List<string>();
                                    if (character.TypeDoiThuong is 7)
                                    {
                                        menus.Add("Đồng ý");
                                    }
                                    else
                                    {
                                        menus.Add("Từ chối");

                                    }
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, text, menus, character.InfoChar.Gender));
                                    character.TypeMenu = 43;
                                    break;
                                }
                            //Bày mâm ngũ quả
                            case 1:
                                {
                                    var text = DoiThuongHandler.DoiThuong(character, $"{ServerUtils.ColorNotSpace("green")}Bày mâm ngũ quả\n", new List<int> { 1177, 1178, 1179, 1180, 1181, 1183, -1}, new List<int> { 20, 20, 20, 20, 20, 1, 5000000}, 8);
                                    var menus = new List<string>();
                                    if (character.TypeDoiThuong is 8)
                                    {
                                        menus.Add("Đồng ý");
                                    }
                                    else
                                    {
                                        menus.Add("Từ chối");

                                    }
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, text, menus, character.InfoChar.Gender));
                                    character.TypeMenu = 43;
                                    break;
                                }
                            case 2://MENU - PHA LÊ
                                {
                                    character.CharacterHandler.SendMessage(
                                        Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextBaHatMit[0], MenuNpc.Gi().MenuBaHatMit[31], character.InfoChar.Gender));
                                    character.TypeMenu = 24;
                                }
                                break;
                            //MENU - Chuyển hoá trang bị
                            case 3:
                                {
                                    character.CharacterHandler.SendMessage(
                                        Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextBaHatMit[3], MenuNpc.Gi().MenuBaHatMit[10], character.InfoChar.Gender));
                                    character.TypeMenu = 4;
                                    break;
                                }
                            case 4://MENU - Tinh chế
                                {
                                    character.CharacterHandler.SendMessage(
                                       Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextBaHatMit[0], MenuNpc.Gi().MenuBaHatMit[32], character.InfoChar.Gender));
                                    character.TypeMenu = 25;
                                    break;
                                }
                            case 5://MENU - Pháp Sư Trang Bị
                                {
                                    character.CharacterHandler.SendMessage(
                                       Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextBaHatMit[5], MenuNpc.Gi().MenuBaHatMit[45], character.InfoChar.Gender));
                                    character.TypeMenu = 26;
                                    break;
                                }
                            //case 2:
                            //    character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[24]));
                            //    character.ShopId = 13;
                            //    break;
                            //case 3:
                            //    character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[28]));
                            //    character.ShopId = 14;
                            //    break;
                            case 6:
                                character.InfoChar.X = 217;
                                character.InfoChar.Y = 408;
                                character.CharacterHandler.SendZoneMessage(Service.SendTeleport(character.Id, character.TypeTeleport));
                                MapManager.JoinMap(character, 112, 0, false, false, character.TypeTeleport);
                                break;
                            case 7:
                                break;
                            case 8:
                                if (ConfigManager.gI().SuKienHe)
                                {

                                }else if (ConfigManager.gI().SuKienVuLan)
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Ngươi muốn xuống địa ngục tìm Lích Tên à?\nNhớ mang theo bình phép chứa linh hồn\nTìm đủ 99 linh hồn thì gặp ta", new List<string> { "Xuống\nđịa ngục", "Hồi sinh\nLích tên", "Ráp\nhoa đăng" }, character.InfoChar.Gender));
                                    character.TypeMenu = 50;
                                }
                                else
                                {

                                }
                                break;
                        }
                        break;
                    }
                case 50:
                    switch (select)
                    {
                        case 0:
                            character.Zone.ZoneHandler.SendMessage(Service.SendTeleport(character.Id, character.TypeTeleport));
                            MapManager.JoinMap(character, 167, ServerUtils.RandomNumber(20), true, true, character.TypeTeleport);
                            break;
                        case 1:
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Sáng mai updatee"));
                            break;
                        case 2:
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Sáng mai updatee"));
                            break;
                    }
                    break;
                case 27:
                    {
                        switch (select)
                        {
                            case 0:
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(21, $"{ServerUtils.Color("green")}Chế tạo bồn tắm gỗ {ItemHandler.TrueItem(character, new List<int> { 1244, 1245, 1246, 1247, -1 }, new List<int> { 50, 20, 20, 2, 5000000 }, 1)}", new List<string> { character.TypeDoiThuong != 1 ? "Từ chối" : "Chế tạo"}, character.InfoChar.Gender));
                                character.TypeMenu = 28;
                            break;
                            case 1:
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(21, $"{ServerUtils.Color("green")}Chế tạo bồn tắm vàng {ItemHandler.TrueItem(character, new List<int> { 1244, 1245, 1246, 1247, -1, -2 }, new List<int> { 50, 20, 20, 2, 5000000, 1000 }, 2)}", new List<string> { character.TypeDoiThuong != 2 ? "Từ chối" : "Chế tạo" }, character.InfoChar.Gender));
                                character.TypeMenu = 28;
                                break;
                        }
                    }
                    break;
                case 28:
                    {
                        switch (select)
                        {
                            case 0:
                                switch (character.TypeDoiThuong)
                                {
                                    case 1:
                                        {
                                            character.CharacterHandler.RemoveItemBagById(1244, 50);
                                            character.CharacterHandler.RemoveItemBagById(1245, 20);
                                            character.CharacterHandler.RemoveItemBagById(1246, 20);
                                            character.CharacterHandler.RemoveItemBagById(1247, 2);
                                            character.MineGold(5000000);
                                            var item = ItemCache.GetItemDefault(1248);
                                            character.CharacterHandler.AddItemToBag(true, item);
                                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                                            character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                            character.CharacterHandler.SendMessage(Service.ServerMessage("Chúc mừng ngươi đã chế tạo thành công Bồn Tắm Gỗ"));
                                            break;
                                        }
                                    case 2:
                                        {
                                            character.CharacterHandler.RemoveItemBagById(1244, 50);
                                            character.CharacterHandler.RemoveItemBagById(1245, 20);
                                            character.CharacterHandler.RemoveItemBagById(1246, 20);
                                            character.CharacterHandler.RemoveItemBagById(1247, 2);
                                            character.MineGold(5000000);
                                            character.MineDiamond(2000);
                                            var item = ItemCache.GetItemDefault(1249);
                                            character.CharacterHandler.AddItemToBag(true, item);
                                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                                            character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                            character.CharacterHandler.SendMessage(Service.ServerMessage("Chúc mừng ngươi đã chế tạo thành công Bồn Tắm Vàng"));
                                            break;
                                        }
                                }
                                break;
                        }
                    }
                    break;
                case 19:
                    {
                        switch (select)
                        {
                            case 0:
                                {
                                    character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[21]));
                                    character.ShopId = 11;
                                    break;
                                }
                            case 1:
                                {
                                    character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[20]));
                                    character.ShopId = 10;
                                    break;

                                }
                        }
                        break;
                    }
                //Cửa hàng bùa
                case 2:
                    {
                        if (@select is < 0 or > 2) select = 0;
                        var idShop = select;
                        character.CharacterHandler.SendMessage(Service.Shop(character, 0, idShop));
                        character.ShopId = idShop;
                        character.TypeShop = 0;
                        break;
                    }
                //Menu Pha lê hoá
                case 3:
                    {
                        if (select != 0) return;
                        character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[8]));
                        character.ShopId = 4;
                        break;
                    }
                //MENU - Chuyển hoá trang bị
                case 4:
                    {
                        switch (select)
                        {
                            case 0:
                                {
                                    character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[12]));
                                    character.ShopId = 5;
                                    break;
                                }
                            case 1:
                                {
                                    character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[12]));
                                    character.ShopId = 6;
                                    break;
                                }
                        }
                        break;
                    }
                //Nâng cấp trang bị
                case 5:
                    {
                        var listArray = character.CombinneIndex;
                        var dungDaBaoVe = listArray[5];
                        var daBaoVeItemIndex = listArray[6];
                        var buaNangCap = listArray[7] == 1;
                        var daBaoVe = false;
                        var daBaoVeLock = listArray[8] == 1;
                        if (select == 1 && dungDaBaoVe == 1 && daBaoVeItemIndex != -1)
                        {
                            daBaoVe = true;
                          //  Console.WriteLine("Co su dung da bao ve");
                        }
                        else if (select != 0)
                        {
                            return;
                        }

                        var trangBi = character.CharacterHandler.GetItemBagByIndex(listArray[0]);
                        if (trangBi == null) return;
                        var da = character.CharacterHandler.GetItemBagByIndex(listArray[1]);
                        var soDaCanNangCap = listArray[2];
                        var gold = listArray[3];
                        var percentSuccess = listArray[4];
                        if (character.InfoChar.Gold < gold)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_GOLD));
                            return;
                        }
                        if (da.Quantity < soDaCanNangCap)
                        {
                            character.CharacterHandler.SendMessage(
                                Service.DialogMessage(TextServer.gI().NEED_ENOUGH_STONE));
                            return;
                        }

                        var optionCheck = trangBi.Options.FirstOrDefault(option => option.Id == 72);
                        var optionCheck2 = trangBi.Options.FirstOrDefault(option => option.Id == 209);
                        var percentRandom = ServerUtils.RandomNumber(100) < percentSuccess;
                        if (percentRandom)
                        {
                            if (optionCheck == null)
                            {
                                trangBi.Options.Add(new OptionItem()
                                {
                                    Id = 72,
                                    Param = 1
                                });
                            }
                            else
                            {
                                optionCheck.Param += 1;
                            }
                            trangBi.Options.Where(option => DataCache.IdOptionGoc.Contains(option.Id)).ToList().ForEach(
                                option =>
                                {
                                    option.Param += option.Param / 10;
                                });

                            character.CharacterHandler.SendMessage(Service.SendCombinne2());
                        }
                        else
                        {
                            if (optionCheck != null)
                            {
                                // – cấp 0 lên cấp 1 xịt hay lên ko ảnh hưởng gì hết. Xác suất 80%
                                // – cấp 1 lên cấp 2 xịt hay lên ko ảnh hưởng. Xác suất 50%
                                // – cấp 2 lên cấp 3 xịt bị rớt xuống cấp 1 và giảm 1% chỉ số. Xác suất 20%
                                // – cấp 3 lên 4 xịt k giảm cấp và chỉ số. Xác suất 10%
                                // – cấp 4 lên 5 xịt rớt xuống 3 giảm 1% chỉ số. Xác suất 5%
                                // – cấp 5 lên 6 xịt ko sao. Xác suất 2%
                                // – cấp 6 lên 7 xịt xuống 5 và giảm 1% chỉ số. Xác suất 1%

                                if (optionCheck.Param > 0 && optionCheck.Param % 2 == 0 && !daBaoVe)
                                {
                                    optionCheck.Param -= 1;
                                    trangBi.Options.Where(option => DataCache.IdOptionGoc.Contains(option.Id)).ToList().ForEach(
                                        option =>
                                        {
                                            option.Param -= option.Param / 10;
                                        });
                                    if (optionCheck2 == null)
                                    {
                                        trangBi.Options.Add(new OptionItem()
                                        {
                                            Id = 209,
                                            Param = 1
                                        });
                                    }
                                    else
                                    {
                                        optionCheck2.Param += 1;
                                    }
                                }
                                

                            }
                            character.CharacterHandler.SendMessage(Service.SendCombinne3());
                        }
                        character.MineGold(gold);
                        if (daBaoVe)
                        {

                            character.CharacterHandler.RemoveItemBagByIndex(daBaoVeItemIndex, 1, false, reason: "Dùng đá bảo vệ");
                           // Console.WriteLine("Xoa da bao ve");
                        }
                        if (buaNangCap)
                        {
                            character.CharacterHandler.RemoveItemBagById(1277, 1);
                        }
                        character.CharacterHandler.RemoveItemBagByIndex(da.IndexUI, soDaCanNangCap, reason: "Dùng đá nâng cấp");
                        character.CharacterHandler.SendMessage(Service.BuyItem(character));
                        character.CharacterHandler.SendMessage(Service.SendBag(character));

                        var checkDa = character.CharacterHandler.GetItemBagByIndex(listArray[1]);
                        var listIndexUi = new List<int>();
                        if (checkDa != null && checkDa.Id == da.Id)
                        {
                            listIndexUi.Add(trangBi.IndexUI);
                            listIndexUi.Add(da.IndexUI);
                        }
                        else
                        {
                            listIndexUi.Add(trangBi.IndexUI);
                        }
                        character.CharacterHandler.SendMessage(Service.SendCombinne1(listIndexUi));
                        character.CombinneIndex.Clear();
                        character.CombinneIndex = null;

                        break;
                    }
                //Nhập đá
                case 6:
                    {
                        if (select != 0) return;
                        var bagNull = character.LengthBagNull();
                        var listArray = character.CombinneIndex;
                        var item1 = character.CharacterHandler.GetItemBagByIndex(listArray[0]);
                        var item2 = character.CharacterHandler.GetItemBagByIndex(listArray[1]);
                        var idNew = (short)(220 + ServerUtils.RandomNumber(5));
                        var itemNew = ItemCache.GetItemDefault(idNew);

                        var itemBagNotMax = character.CharacterHandler.ItemBagNotMaxQuantity(itemNew.Id);
                        if (itemBagNotMax == null && bagNull < 1)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_BAG));
                            return;
                        }
                        switch (item1.Id)
                        {
                            case 225:
                                {
                                    character.CharacterHandler.RemoveItemBagByIndex(item1.IndexUI, 10, false, reason: "Nhập đá");
                                    character.CharacterHandler.RemoveItemBagByIndex(item2.IndexUI, 1, false, reason: "Nhập đá");
                                    break;
                                }
                            default:
                                {
                                    character.CharacterHandler.RemoveItemBagByIndex(item1.IndexUI, 1, false, reason: "Nhập đá");
                                    character.CharacterHandler.RemoveItemBagByIndex(item2.IndexUI, 10, false, reason: "Nhập đá");
                                    break;
                                }
                        }
                        character.MineGold(2000);
                        character.CharacterHandler.AddItemToBag(true, itemNew, "Nhập đá");
                        character.CharacterHandler.SendMessage(Service.BuyItem(character));
                        character.CharacterHandler.SendMessage(Service.SendBag(character));

                        var listIndexUi = new List<int>();
                        var itemReturn = character.CharacterHandler.GetItemBagByIndex(item1.IndexUI);
                        if (itemReturn != null && itemReturn.Id == item1.Id)
                        {
                            listIndexUi.Add(item1.IndexUI);
                        }
                        itemReturn = character.CharacterHandler.GetItemBagByIndex(item2.IndexUI);
                        if (itemReturn != null && itemReturn.Id == item2.Id)
                        {
                            listIndexUi.Add(item2.IndexUI);
                        }

                        character.CharacterHandler.SendMessage(Service.SendCombinne1(listIndexUi));
                        character.CharacterHandler.SendMessage(Service.SendCombinne4(ItemCache.ItemTemplate(itemNew.Id).IconId));
                        character.CombinneIndex.Clear();
                        character.CombinneIndex = null;
                        break;
                    }
                //Nhập ngọc rông
                case 7:
                    {
                        if (select != 0) return;
                        var bagNull = character.LengthBagNull();
                        var listArray = character.CombinneIndex;
                        if (listArray == null) return;
                        var ngocRong = character.CharacterHandler.GetItemBagByIndex(listArray[0]);
                        var idNew = CombineHandler.GetNgocRongUp(ngocRong.Id);
                        var itemNew = ItemCache.GetItemDefault((short)idNew);

                        var itemBagNotMax = character.CharacterHandler.ItemBagNotMaxQuantity(itemNew.Id);
                        if (itemBagNotMax == null && bagNull < 1)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_BAG));
                            return;
                        }
                        character.MineGold(2000);
                        character.CharacterHandler.RemoveItemBagByIndex(ngocRong.IndexUI, 7, reason: "Nhập ngọc");
                        character.CharacterHandler.AddItemToBag(true, itemNew, "Nhập ngọc");
                        character.CharacterHandler.SendMessage(Service.BuyItem(character));
                        character.CharacterHandler.SendMessage(Service.SendBag(character));

                        character.CharacterHandler.SendMessage(Service.SendCombinne5(ItemCache.ItemTemplate(itemNew.Id).IconId));

                        var listIndexUi = new List<int>();
                        var itemReturn = character.CharacterHandler.GetItemBagByIndex(ngocRong.IndexUI);
                        if (itemReturn != null && itemReturn.Id == ngocRong.Id)
                        {
                            listIndexUi.Add(ngocRong.IndexUI);
                        }

                        character.CharacterHandler.SendMessage(Service.SendCombinne1(listIndexUi));
                        character.CombinneIndex.Clear();
                        character.CombinneIndex = null;
                        break;
                    }
                //Ép sao trang bị
                case 8:
                    {
                        if (select != 0) return;
                        if (10 > character.AllDiamond())
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_DIAMOND));
                            return;
                        }
                        var bagNull = character.LengthBagNull();
                        var listArray = character.CombinneIndex;
                        if (listArray == null) return;
                        var trangBi = character.CharacterHandler.GetItemBagByIndex(listArray[0]);
                        var ngocRong = character.CharacterHandler.GetItemBagByIndex(listArray[1]);
                        var optionId = listArray[2];
                        var optionParam = listArray[3];
                        var isUpgrade = listArray[4] == 1;
                        if (trangBi == null || ngocRong == null) return;

                        var optionCheck = trangBi.Options.FirstOrDefault(opt => opt.Id == 102);
                        var optionUp = trangBi.Options.FirstOrDefault(opt => opt.Id == optionId);
                        var checkSql = trangBi.Options.FirstOrDefault(opt => opt.Id == 107);

                        var Cgold = 0;
                        if (optionCheck == null)
                        {
                            trangBi.Options.Add(new OptionItem()
                            {
                                Id = 102,
                                Param = 1
                            });
                            Cgold = 1;
                        }
                        else
                        {
                            optionCheck.Param++;
                            Cgold = optionCheck.Param;
                        }
                        if (!isUpgrade)
                        {
                            if (optionUp == null)
                            {
                                trangBi.Options.Add(new OptionItem()
                                {
                                    Id = optionId,
                                    Param = optionParam
                                });
                            }
                            else
                            {
                                optionUp.Param += optionParam;
                            }
                        }
                        else
                        {
                            var optionUpgradeUp = trangBi.Options.FirstOrDefault(opt => opt.Id == 218);
                            if (optionUpgradeUp == null)
                            {
                                trangBi.Options.Add(new OptionItem()
                                {
                                    Id = 218,
                                    Param = 1,
                                });
                            }
                            trangBi.Options.Add(new OptionItem()
                            {
                                Id = optionId,
                                Param = optionParam
                            });
                        }
                        character.MineDiamond(10);
                        character.MineGold((Cgold * 10000000));
                        character.CharacterHandler.SendMessage(Service.SendCombinne2());
                        character.CharacterHandler.RemoveItemBagByIndex(ngocRong.IndexUI, 1, reason: "Ép ngọc rồng");
                        character.CharacterHandler.SendMessage(Service.BuyItem(character));
                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                        
                        character.CharacterHandler.SendMessage(Service.SendCombinne1(new List<int>() { trangBi.IndexUI, ngocRong.IndexUI }));
                        character.CombinneIndex.Clear();
                        character.CombinneIndex = null;
                        break;
                    }
                //Pha lê hoá trang bị
                case 9:
                    {
                        var listArray = character.CombinneIndex;
                        if (listArray == null) return;
                        var itemBag = character.CharacterHandler.GetItemBagByIndex(listArray[0]);
                        var lvOption = listArray[1];
                        if (itemBag == null) return;
                        var percentPhaLe = DataCache.PercentPhaLe[lvOption];
                        long goldPhaLe = (long)percentPhaLe[0] * 1000000;
                        int diamondPhaLe = (int)percentPhaLe[2];
                        int solandap = 1;
                        switch (select)
                        {
                            case 2:
                                goldPhaLe *= 100;
                                diamondPhaLe *= 100;
                                solandap = 100;
                                break;
                            case 1:
                                goldPhaLe *= 10;
                                diamondPhaLe *= 10;
                                solandap = 10;
                                break;
                        }

                        if (character.InfoChar.Gold < goldPhaLe)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_GOLD));
                            return;
                        }
                        if (character.AllDiamond() < diamondPhaLe)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_DIAMOND));
                            return;
                        }
                        
                        var success = false;
                       
                        var FakePercent = 1;
                        if (lvOption >= 5)
                        {
                            FakePercent = 2 * (select == 0 ? 3 : select == 1 ? 2 : 1);
                        }
                        for (int i = 1; i <= solandap; i++)
                        {
                            var percent = ServerUtils.RandomNumber(150.000);//120
                            switch (lvOption)
                            {
                                case 9:
                                    percent = ServerUtils.RandomNumber(220.000);
                                    break;
                                case 10:
                                    percent = ServerUtils.RandomNumber(250.000);
                                    break;
                                case >= 5:
                                    percent = ServerUtils.RandomNumber(200.000);
                                    break;
                            }
                            var percentSuccess = percent <= percentPhaLe[1] / FakePercent;
                            if (percentSuccess)
                            {
                                success = true;
                                solandap = i;
                                goldPhaLe = (long)((long)percentPhaLe[0] * 1000000 * solandap);
                               // long GoldGet = (long)((long)percentPhaLe[0] * 1000000 * solandap);
                                diamondPhaLe = (int)percentPhaLe[2] * solandap;
                                break;
                            }
                        }
                        if (success)
                        {
                            var optionPlus = itemBag.Options.FirstOrDefault(option => option.Id == 107);
                            if (optionPlus != null && optionPlus.Param >= DataCache.MAX_LIMIT_SPL)
                            {
                                return;
                            }
                            if (optionPlus != null)
                            {
                                optionPlus.Param++;
                            }
                            else
                            {
                                itemBag.Options.Add(new OptionItem()
                                {
                                    Id = 107,
                                    Param = 1
                                });
                            }
                            character.CharacterHandler.SendMessage(Service.SendCombinne2());
                        }
                        else
                        {
                            character.CharacterHandler.SendMessage(Service.SendCombinne3());
                        }
                        character.MineGold(goldPhaLe);
                        character.MineDiamond(diamondPhaLe);
                        character.CharacterHandler.SendMessage(Service.BuyItem(character));
                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                        character.CharacterHandler.SendMessage(Service.SendCombinne1(new List<int>() { itemBag.IndexUI }));
                        character.CombinneIndex.Clear();
                        character.CombinneIndex = null;
                        break;
                    }
                //Chuyển hoá trang bị VÀNG / 10
                //Chuyển hoá trang bị NGỌC / 11
                case 10:
                case 11:
                    {
                        if (select != 0) return;
                        var listArray = character.CombinneIndex;
                        var itemLuongLong = character.CharacterHandler.GetItemBagByIndex(listArray[0]); //old
                        var itemThan = character.CharacterHandler.GetItemBagByIndex(listArray[1]); //new đồ thần
                        var levelUp = listArray[2];
                        var checkMoney = listArray[3];
                        if (itemLuongLong == null || itemThan == null) return;
                        switch (character.TypeMenu)
                        {
                            case 10:
                                {
                                    if (character.InfoChar.Gold < checkMoney)
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service.DialogMessage(TextServer.gI().NOT_ENOUGH_GOLD));
                                        return;
                                    }
                                    else
                                    {
                                        character.MineGold(checkMoney);
                                    }
                                    break;
                                }
                            case 11:
                                {
                                    if (character.AllDiamondLock() < checkMoney)
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service.DialogMessage(TextServer.gI().NOT_ENOUGH_DIAMOND));
                                        return;
                                    }
                                    else
                                    {
                                        character.MineDiamond(checkMoney, 2);
                                    }
                                    break;
                                }
                        }

                        var checkLevel = itemLuongLong.Options.FirstOrDefault(opt => opt.Id == 72)?.Param;

                        var listOptionLlGoc = itemLuongLong.Options.Where(opt => DataCache.IdOptionGoc.Contains(opt.Id)).ToList();
                        itemThan.Options.ForEach(opt =>
                        {
                            var paramNew = 0;
                            var optCheck = listOptionLlGoc.FirstOrDefault(o => o.Id == opt.Id);
                            if (optCheck == null) return;
                            if (checkLevel == levelUp)
                            {
                                paramNew += optCheck.Param;
                            }
                            else
                            {
                                paramNew += optCheck.Param - optCheck.Param / 10;
                            }
                            opt.Param += paramNew;
                        });
                        var listCheckPlus = itemLuongLong.Options.Where(opt => itemThan.Options.FirstOrDefault(o => o.Id == opt.Id) == null).ToList();
                        itemThan.Options.AddRange(listCheckPlus);

                        character.CharacterHandler.RemoveItemBag(itemLuongLong.IndexUI, reason: "Chuyển hóa");
                        character.CharacterHandler.SendMessage(Service.BuyItem(character));
                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                        character.CharacterHandler.SendMessage(Service.SendCombinne4(ItemCache.ItemTemplate(itemThan.Id).IconId));

                        var itemReturn = character.ItemBag.FirstOrDefault(item =>
                            item.Id == itemThan.Id && item.Options.Count == itemThan.Options.Count &&
                            item.IndexUI != itemThan.IndexUI) ?? itemThan;
                        character.CharacterHandler.SendMessage(Service.SendCombinne1(new List<int>() { itemReturn.IndexUI }));
                        character.CombinneIndex.Clear();
                        character.CombinneIndex = null;
                        break;
                    }
                //Nâng cấp porata
                case 12:
                    {
                        if (select != 0) return;

                        var listArray = character.CombinneIndex;
                        var bongTaiPorata = character.CharacterHandler.GetItemBagByIndex(listArray[0]);
                        var manhVoBongTai = character.CharacterHandler.GetItemBagByIndex(listArray[1]);
                        var soNgocCanNangCap = listArray[2];
                        var soVangCanNangCap = listArray[3];
                        var percentSuccess = listArray[4];
                        var isThanhCong = false;

                        if (character.InfoChar.Gold < soVangCanNangCap)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_GOLD));
                            return;
                        }
                        if (character.AllDiamond() < soNgocCanNangCap)
                        {
                            character.CharacterHandler.SendMessage(Service.DialogMessage(TextServer.gI().NOT_ENOUGH_DIAMOND));
                            return;
                        }

                        var optionCheck = manhVoBongTai.Options.FirstOrDefault(option => option.Id == 31);
                        var percentRandom = ServerUtils.RandomNumber(100) < percentSuccess;
                        if (percentRandom)
                        {
                            // Thành công thì xóa số lượng 9999 item, xóa item bông tai và thêm item bông tai 2
                            if (optionCheck != null)
                            {
                                optionCheck.Param -= 9999;
                                if (optionCheck.Param <= 0)
                                {
                                    character.CharacterHandler.RemoveItemBagByIndex(manhVoBongTai.IndexUI, 1, false, reason: "NC Porata");
                                }
                            }
                            character.CharacterHandler.RemoveItemBagByIndex(bongTaiPorata.IndexUI, 1, false, reason: "NC Porata");
                            var itemAdd = ItemCache.GetItemDefault(921);
                            itemAdd.Quantity = 1;
                            character.CharacterHandler.AddItemToBag(false, itemAdd, "Nâng cấp porata");
                            character.CharacterHandler.SendMessage(Service.SendCombinne2());
                            isThanhCong = true;
                        }
                        else
                        {
                           
                            character.CharacterHandler.SendMessage(Service.SendCombinne3());
                        }

                        character.MineGold(soVangCanNangCap);
                        character.MineDiamond(soNgocCanNangCap);
                        character.CharacterHandler.SendMessage(Service.BuyItem(character));
                        character.CharacterHandler.SendMessage(Service.SendBag(character));

                        var checkManhVoBongTai = character.CharacterHandler.GetItemBagByIndex(listArray[1]);
                        var listIndexUi = new List<int>();
                        if (!isThanhCong)
                        {
                            if (checkManhVoBongTai != null && checkManhVoBongTai.Id == manhVoBongTai.Id)
                            {
                                listIndexUi.Add(bongTaiPorata.IndexUI);
                                listIndexUi.Add(manhVoBongTai.IndexUI);
                            }
                            else
                            {
                                listIndexUi.Add(bongTaiPorata.IndexUI);
                            }
                        }
                        character.CharacterHandler.SendMessage(Service.SendCombinne1(listIndexUi));
                        character.CombinneIndex.Clear();
                        character.CombinneIndex = null;
                        break;
                    }
                // Mở option porata
                case 13:
                    {
                        if (select != 0) return;

                        var listArray = character.CombinneIndex;
                        var bongTaiPorata2 = character.CharacterHandler.GetItemBagByIndex(listArray[0]);
                        var manhHonBongTai = character.CharacterHandler.GetItemBagByIndex(listArray[1]);
                        var daXanhLam = character.CharacterHandler.GetItemBagByIndex(listArray[2]);
                        var soNgocCanNangCap = listArray[3];
                        var soVangCanNangCap = listArray[4];
                        var percentSuccess = listArray[5];

                        if (character.InfoChar.Gold < soVangCanNangCap)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_GOLD));
                            return;
                        }
                        if (character.AllDiamond() < soNgocCanNangCap)
                        {
                            character.CharacterHandler.SendMessage(Service.DialogMessage(TextServer.gI().NOT_ENOUGH_DIAMOND));
                            return;
                        }
                        // remove item đá xanh lam, và 99 cái mảnh hồn
                        character.CharacterHandler.RemoveItemBagByIndex(manhHonBongTai.IndexUI, 99, false, reason: "CS Porata");
                        character.CharacterHandler.RemoveItemBagByIndex(daXanhLam.IndexUI, 1, false, reason: "CS Porata");
                        // Remove tiền và ngọc
                        character.MineGold(soVangCanNangCap);
                        character.MineDiamond(soNgocCanNangCap);

                        var optionCheck = bongTaiPorata2.Options.FirstOrDefault(option => option.Id != 72);

                        var percentRandom = ServerUtils.RandomNumber(100) < 101;
                        if (percentRandom)
                        {
                            var optionRandom = DataCache.OptionPorata2[ServerUtils.RandomNumber(DataCache.OptionPorata2.Count)];
                            // Thành công thì lấy random option trong list
                            if (optionCheck != null)
                            {
                                optionCheck.Id = optionRandom[0];
                                optionCheck.Param = ServerUtils.RandomNumber(optionRandom[1], optionRandom[2]);
                            }
                            else
                            {
                                bongTaiPorata2.Options.Insert(0, new OptionItem()
                                {
                                    Id = optionRandom[0],
                                    Param = ServerUtils.RandomNumber(optionRandom[1], optionRandom[2])
                                });
                            }
                            character.CharacterHandler.SendMessage(Service.SendCombinne2());
                        }
                        else
                        {
                            character.CharacterHandler.SendMessage(Service.SendCombinne3());
                        }

                        character.CharacterHandler.SendMessage(Service.BuyItem(character));
                        character.CharacterHandler.SendMessage(Service.SendBag(character));

                        var checkBongTaiPorata2 = character.CharacterHandler.GetItemBagByIndex(listArray[0]);
                        var listIndexUi = new List<int>();
                        if (checkBongTaiPorata2 != null && checkBongTaiPorata2.Id == bongTaiPorata2.Id)
                        {
                            listIndexUi.Add(bongTaiPorata2.IndexUI);
                        }

                        character.CharacterHandler.SendMessage(Service.SendCombinne1(listIndexUi));
                        character.CombinneIndex.Clear();
                        character.CombinneIndex = null;
                        break;
                    }
                case 14:
                    {
                        //menu linh thú
                        switch (select)
                        {
                            case 0://nở trứng linh thú
                                {
                                    character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[16]));
                                    character.ShopId = 9;
                                    break;
                                }
                            case 1://nâng cấp linh thú
                                {
                                    character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[17]));
                                    character.ShopId = 10;
                                    break;
                                }
                            case 2://nâng cấp linh thú
                                {
                                    character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[18]));
                                    character.ShopId = 11;
                                    break;
                                }
                            case 3://nâng cấp linh thú
                                {
                                    character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[19]));
                                    character.ShopId = 12;
                                    break;
                                }
                        }
                        break;
                    }
                case 15://nở trứng
                    {
                        var listArray = character.CombinneIndex;

                        var trungLinhThu = character.CharacterHandler.GetItemBagByIndex(listArray[0]);
                        var honLinhThu = character.CharacterHandler.GetItemBagByIndex(listArray[1]);
                        short trungLinhThuIcon = ItemCache.ItemTemplate(trungLinhThu.Id).IconId;

                        character.CharacterHandler.RemoveItemBagByIndex(trungLinhThu.IndexUI, 1, false, reason: "Nở trứng");
                        character.CharacterHandler.RemoveItemBagByIndex(honLinhThu.IndexUI, 99, false, reason: "Nở trứng");

                        if (listArray.Count == 3)
                        {
                            var thoiVang = character.CharacterHandler.GetItemBagByIndex(listArray[2]);
                            character.CharacterHandler.RemoveItemBagByIndex(thoiVang.IndexUI, 5, false, reason: "Nở trứng nhanh");
                        }

                        var linhThuNgauNhien = DataCache.ListPetID[ServerUtils.RandomNumber(DataCache.ListPetID.Count)];
                        var itemLinhThu = ItemCache.GetItemDefault(linhThuNgauNhien);

                        var maSoLinhThu = ServerUtils.RandomNumber(100, 100000);
                        var optionHiden = itemLinhThu.Options.FirstOrDefault(option => option.Id == 73);

                        if (optionHiden != null)
                        {
                            optionHiden.Param = maSoLinhThu;
                        }
                        else
                        {
                            itemLinhThu.Options.Add(new OptionItem()
                            {
                                Id = 73,
                                Param = maSoLinhThu,
                            });
                        }

                        character.CharacterHandler.SendMessage(Service.NpcChat(npcId, TextServer.gI().RANDOM_LINH_THU));
                        character.CharacterHandler.SendMessage(Service.SendCombinne6(trungLinhThuIcon, ItemCache.ItemTemplate(itemLinhThu.Id).IconId));

                        character.CharacterHandler.AddItemToBag(false, itemLinhThu, "Nở trứng");
                        character.CharacterHandler.SendMessage(Service.BuyItem(character));
                        character.CharacterHandler.SendMessage(Service.SendBag(character));

                        character.CharacterHandler.SendMessage(Service.SendCombinne1(new List<int>()));
                        character.CombinneIndex.Clear();
                        character.CombinneIndex = null;
                        break;
                    }
                    //case 17:
                    //    switch (select)
                    //    {
                    //        case 0:
                    //            if (character.CharacterHandler.GetItemBagById(1199) == null || character.CharacterHandler.GetItemBagById(1199).Quantity < 1)
                    //            {
                    //                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn không có mâm ngũ quả"));
                    //                return;
                    //            }
                    //            var rand = ServerUtils.RandomNumber(120);
                    //            var item = ItemCache.GetItemDefault(0);
                    //            if (rand <= 30)
                    //            {
                    //                item = ItemCache.GetItemDefault(1205);
                    //            }else if (rand <= 60)
                    //            {
                    //                item = ItemCache.GetItemDefault(1219);
                    //            }
                    //            else if (rand <= 100)
                    //            {
                    //                item = ItemCache.GetItemDefault(1202);
                    //            }else if (rand <= 120)
                    //            {
                    //                item = ItemCache.GetItemDefault(758);
                    //            }
                    //            character.CharacterHandler.AddItemToBag(trfue, item, "Bay mam ngu qua");
                    //            character.CharacterHandler.RemoveItemBagById(1199, 1);
                    //            character.CharacterHandler.SendMessage(Service.SendBag(character));

                    //            character.CharacterHandler.SendMessage(Service.SendCombinne4(ItemCache.ItemTemplate(item.Id).IconId));
                    //             break;
                    //    }
                    //    break;
                    //case 16:
                    //    switch (select)
                    //    {
                    //        case 0:
                    //            int gold = 1000000;
                    //            if (character.InfoChar.Gold < gold)
                    //            {
                    //                character.CharacterHandler.SendMessage(Service.ServerMessage("Không đủ 1tr vàng"));
                    //                return;
                    //            }
                    //            else if (character.CharacterHandler.GetItemBagById(1144) == null || character.CharacterHandler.GetItemBagById(1144).Quantity < 10)
                    //            {
                    //                character.CharacterHandler.SendMessage(Service.ServerMessage("Không có thẻ Fan Gà Nửa Mùa hoặc không đủ"));
                    //                return;
                    //            }
                    //            else
                    //            {
                    //                var capsulethuong = ItemCache.GetItemDefault(1146);
                    //                capsulethuong.Quantity = 1;
                    //                capsulethuong.Options.Add(new OptionItem()
                    //                {
                    //                    Id = 30,
                    //                    Param = 0,
                    //                });
                    //                character.CharacterHandler.AddItemToBag(true,capsulethuong);
                    //                character.CharacterHandler.RemoveItemBagById(1144, 10);
                    //                character.MineGold(gold);
                    //                character.CharacterHandler.SendMessage(Service.SendBag(character));
                    //                character.CharacterHandler.SendMessage(Service.ServerMessage("Chúc mừng bạn đã nhận được Capsule thường"));
                    //            }
                    //            break;
                    //        case 1:
                    //            int gem = 1000;
                    //            if (character.InfoChar.Diamond < gem)
                    //            {
                    //                character.CharacterHandler.SendMessage(Service.ServerMessage("Không đủ 1000 ngọc, vui lòng nạp thêm"));
                    //                return;
                    //            }
                    //            else if (character.CharacterHandler.GetItemBagById(1143) == null || character.CharacterHandler.GetItemBagById(1143).Quantity < 10)
                    //            {
                    //                character.CharacterHandler.SendMessage(Service.ServerMessage("Không có thẻ Fan cuồng bóng đá hoặc không đủ"));
                    //                return;
                    //            }
                    //            else
                    //            {
                    //                var capsulevip = ItemCache.GetItemDefault(1147);
                    //                capsulevip.Quantity = 1;
                    //                capsulevip.Options.Add(new OptionItem()
                    //                {
                    //                    Id = 30,
                    //                    Param = 0,
                    //                });
                    //                character.CharacterHandler.AddItemToBag(true, capsulevip);
                    //                character.CharacterHandler.RemoveItemBagById(1143, 10);
                    //                character.MineDiamond(gem);
                    //                character.CharacterHandler.SendMessage(Service.SendBag(character));
                    //                character.CharacterHandler.SendMessage(Service.ServerMessage("Chúc mừng bạn đã nhận được Capsule VIP"));
                    //            }
                    //            break;
            //}
            //       break;
            }
        }

        private static void ConfirmGhiDanh(Character character, short npcId, int select)
        {
            switch (character.InfoChar.MapId)
            {

                case 129:
                    switch (character.TypeMenu)
                    {
                        case 0:
                            switch (select)
                            {
                                case 0:

                                    character.CharacterHandler.SendMessage(Service.OpenUiSay(23, ChampionShip23_Cache.textHuongDanThem));
                                    break;
                                case 1:
                                    character.DataDaiHoiVoThuat23.Handler.Register(character);
                                    break;
                                case 2:
                                    character.DataDaiHoiVoThuat23.Handler.Register(character);
                                    break;
                                case 3:
                                    if (!character.DataDaiHoiVoThuat23.WoodChestCollect && character.DataDaiHoiVoThuat23.WoodChestLevel != 0)
                                    {
                                        //menu nhận rương +[n]
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(23, string.Format(ChampionShip23_Cache.Text[1], character.DataDaiHoiVoThuat23.WoodChestLevel), ChampionShip23_Cache.TextMenu[2], character.InfoChar.Gender));
                                        character.TypeMenu = 2;
                                    }
                                    else
                                    {
                                        character.InfoChar.X = 467;
                                        character.InfoChar.Y = 336;
                                        MapManager.JoinMap(character, 52, ServerUtils.RandomNumber(20), false, false, 0);

                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case 52:
                    switch (character.TypeMenu)
                    {
                        case 6:
                            if (TaskHandler.CheckTask(character, 19, 1))
                            {
                                TaskHandler.gI().PlusSubTask(character, 1);
                            }
                            break;
                        case 0:
                            switch (select)
                            {
                                case 0:
                                    character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Lịch thi đấu trong ngày:\nGiải nhi đồng: 8,14,18h\nGiải Siêu cấp 1: 9,13,19h\nGiải Siêu cấp 2: 10,15,20h\nGiải siêu cấp 3: 11,16,21h\nGiải ngoại hạng: 12,17,22,23h"));
                                    character.TypeMenu = 0;
                                    break;
                                case 1:
                                    if (ChampionShip.gI().TypeChampionShip != 5)
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(23, "Hiện đang có giải đấu " + ChampionShip.gI().GetNameChampionShip() + " bạn có muốn đăng ký không?", new List<String> { "Giải\n" + ChampionShip.gI().GetNameChampionShip() + "\n(" + ChampionShip.gI().GetCostChampionShip() + " ngọc)" }, character.InfoChar.Gender));
                                        character.TypeMenu = 1;
                                    }
                                    else
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(23, "Hiện đang có giải đấu " + ChampionShip.gI().GetNameChampionShip() + " bạn có muốn đăng ký không?", new List<String> { "Giải\n" + ChampionShip.gI().GetNameChampionShip() + "\n(" + ChampionShip.gI().GetCostChampionShip() + " vàng)" }, character.InfoChar.Gender));
                                        character.TypeMenu = 1;
                                    }
                                    break;
                                case 2:// giải siêu hạng
                                    MapManager.JoinMap(character, 130, ServerUtils.RandomNumber(20), false, false, 0);
                                    break;
                                case 3:// đại hội võ thuật lần thứ 23
                                    MapManager.JoinMap(character, 129, ServerUtils.RandomNumber(20), false, false, 0);
                                    break;
                            }
                            break;
                        case 1:
                            switch (select)
                            {
                                case 0:// đăng ký đại hội võ thuật
                                    if (ChampionShip.gI().TypeChampionShip != 0)
                                    {
                                        ChampionShip.gI().Register(character);
                                    }
                                    break;
                            }
                        break;
                        case 2://nhận rương +[n]
                            character.DataDaiHoiVoThuat23.WoodChestCollect = true;
                            var woodChest = ItemCache.GetItemDefault(570);
                            woodChest.Options.Clear();
                            woodChest.Options.Add(new OptionItem()
                            {
                                Id = 72,
                                Param = character.DataDaiHoiVoThuat23.WoodChestLevel
                            });
                            character.CharacterHandler.AddItemToBag(false, woodChest, "DHVT 23");
                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn vừa nhận được Rương Gỗ Cấp " + character.DataDaiHoiVoThuat23.WoodChestLevel));
                            break;
                    }
                    break;
            }
        }
        private static void ConfirmLiTieuNuong(Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                case 6:
                    switch (select)
                    {
                        case 0:
                            SuKien8Thang3.SelectTangQua(character, 1558, npcId);
                            break;
                        case 1:
                            SuKien8Thang3.SelectTangQua(character, 1559, npcId);
                            break;
                        case 2:
                            SuKien8Thang3.SelectTangQua(character, 1578, npcId);
                            break;
                    }
                    break;
                case 0:
                    switch (select)
                    {
                        case 0: // csmm 
                            if (ConSoMayManHandler.gI().Config.ConSoMayManStatus == ConSoMayManStatus.DONE)
                            {
                                var last = (ConSoMayManHandler.gI().Config.timeRemain - ServerUtils.CurrentTimeMillis()) / 1000;
                                var text = $"Kết quả giải trước: {ConSoMayManHandler.gI().Config.LastResult}" +
                                    $"\nThắng giải trước: {ConSoMayManHandler.gI().Config.LastPlayersNameWinGame}" +
                                    $"\nBắt đầu sau {last}s";

                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, text, new List<string> { "OK" }, character.InfoChar.Gender));
                                character.TypeMenu = 1;
                            }
                            else
                            {
                                var last = (ConSoMayManHandler.gI().Config.timeRemain - ServerUtils.CurrentTimeMillis()) / 1000;
                                var text = $"Kết quả giải trước: {ConSoMayManHandler.gI().Config.LastResult}" +
                                    $"\nThắng giải trước: {ConSoMayManHandler.gI().Config.LastPlayersNameWinGame}" +
                                    $"\nTham gia: {ConSoMayManHandler.gI().Config.Joins[0]} tổng giải thưởng: {ConSoMayManHandler.gI().Config.PlayersGame.Count} thỏi vàng" +
                                    $"\n{last}s";

                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, text, new List<string> { "Cập nhật", "1 số\n5 thỏi vàng", "ngẫu nhiên\n1 số chẵn\n5 thỏi vàng", "ngẫu nhiên\n1 số lẻ\n5 thỏi vàng", "Hướng\ndẫn\nthêm", "Đóng" }, character.InfoChar.Gender));
                                character.TypeMenu = 1;
                            }
                            break;
                        case 1://chon ai day?
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Trò chơi Chọn Ai Đây đang được diễn ra, nếu bạn tin tưởng mình đang\ntràn đầy may mắn thì có thể tham gia thử", new List<string> { "Thể lệ", "Chọn\nthỏi vàng", "Chọn\nhồng ngọc" }, character.InfoChar.Gender));
                            character.TypeMenu = 2;
                            break;
                        case 2:
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, KeoBuaBao_Cache.TextMenu[0], KeoBuaBao_Cache.Menu[0], character.InfoChar.Gender));
                            character.TypeMenu = 4;
                            break;
                        case 3:
                            SuKien8Thang3.OpenMenuTangQua(character, npcId, 6);
                            break;
                    }

                    break;
                case 4:// keo bua bao
                    switch (select)
                    {
                        case 0:
                            character.InfoChar.KeoBuaBao.MucDatCuoc = 1;
                            break;
                        case 1:
                            character.InfoChar.KeoBuaBao.MucDatCuoc = 5;

                            break;
                        case 2:
                            character.InfoChar.KeoBuaBao.MucDatCuoc = 10;
                            break;
                    }
                    if (character.CharacterHandler.GetItemBagById(457).Quantity < character.InfoChar.KeoBuaBao.MucDatCuoc || character.CharacterHandler.GetItemBagById(457)== null)
                    {
                        character.CharacterHandler.SendMessage(Service.ServerMessage($"Bạn không có đủ thỏi vàng, hãy nạp thêm rồi quay lại."));
                        return;
                    }
                    character.CharacterHandler.RemoveItemBagById(457, character.InfoChar.KeoBuaBao.MucDatCuoc);
                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, string.Format(KeoBuaBao_Cache.TextMenu[1], character.InfoChar.KeoBuaBao.MucDatCuoc), KeoBuaBao_Cache.Menu[1], character.InfoChar.Gender));
                    character.TypeMenu = 5;
                    break;
                case 5://keo bua bao
                    switch (select)
                    {
                        case 0:
                            character.InfoChar.KeoBuaBao.Me_Type = KeoBuaBao_Type.Kéo;
                            break;
                        case 1:
                            character.InfoChar.KeoBuaBao.Me_Type = KeoBuaBao_Type.Búa;
                            break;
                        case 2:
                            character.InfoChar.KeoBuaBao.Me_Type = KeoBuaBao_Type.Bao;
                            break;
                        case 3:
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, KeoBuaBao_Cache.TextMenu[0], KeoBuaBao_Cache.Menu[0], character.InfoChar.Gender));
                            character.TypeMenu = 4;
                            break;
                    }
                    var random = ServerUtils.RandomNumber(100);
                    switch (random)
                    {
                        case < 10:
                            character.InfoChar.KeoBuaBao.KeoBuaBao_Type = KeoBuaBao_Handler.GetWin(character.InfoChar.KeoBuaBao.Me_Type);
                            KeoBuaBao_Handler.Thang(character);
                            break;
                        default:
                            character.InfoChar.KeoBuaBao.KeoBuaBao_Type = KeoBuaBao_Handler.GetLose(character.InfoChar.KeoBuaBao.Me_Type);
                            KeoBuaBao_Handler.Thua(character);
                            break;
                    }
                    break;
                case 1://con so may man
                    switch (select)
                    {
                        case 0://update
                            if (ConSoMayManHandler.gI().Config.ConSoMayManStatus == ConSoMayManStatus.DONE)
                            {
                                var last = (ConSoMayManHandler.gI().Config.timeRemain - ServerUtils.CurrentTimeMillis()) / 1000;
                                var text = $"Kết quả giải trước: {ConSoMayManHandler.gI().Config.LastResult}" +
                                    $"\nThắng giải trước: {ConSoMayManHandler.gI().Config.LastPlayersNameWinGame}" +
                                    $"\nBắt đầu sau {last}s";

                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, text, new List<string> { "OK" }, character.InfoChar.Gender));
                                character.TypeMenu = 1;
                            }
                            else
                            {
                                var last = (ConSoMayManHandler.gI().Config.timeRemain - ServerUtils.CurrentTimeMillis()) / 1000;
                                var text = $"Kết quả giải trước: {ConSoMayManHandler.gI().Config.LastResult}" +
                                    $"\nThắng giải trước: {ConSoMayManHandler.gI().Config.LastPlayersNameWinGame}" +
                                    $"\nTham gia: {ConSoMayManHandler.gI().Config.Joins[0]} tổng giải thưởng: {ConSoMayManHandler.gI().Config.PlayersGame.Count} thỏi vàng" +
                                    $"\n{last}s";

                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, text, new List<string> { "Cập nhật", "1 số\n5 thỏi vàng", "ngẫu nhiên\n1 số chẵn\n5 thỏi vàng", "ngẫu nhiên\n1 số lẻ\n5 thỏi vàng", "Hướng\ndẫn\nthêm", "Đóng" }, character.InfoChar.Gender));
                                character.TypeMenu = 1;
                            }
                            break;
                        case 1:
                            var listInput = new List<InputBox>();
                            listInput.Add(new InputBox()
                            {
                                Name = "Nhập con số bạn muốn chọn",
                                Type = 0,
                            });
                            character.TypeInput = 116;
                            character.CharacterHandler.SendMessage(Service.ShowInput("Con số may mắn", listInput));
                            break;
                        case 2:
                            ConSoMayManHandler.gI().BuyConSoMayMan(character, DataCache.SoChan[ServerUtils.RandomNumber(DataCache.SoChan.Count)]);
                            break;
                        case 3:
                            ConSoMayManHandler.gI().BuyConSoMayMan(character, DataCache.SoLe[ServerUtils.RandomNumber(DataCache.SoLe.Count)]);
                            break;
                        case 4:
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, ConSoMayManHandler.HuongDanThem, new List<string> { "OKE" }, character.InfoChar.Gender));
                            character.TypeMenu = 6;
                            break;

                    }
                    break;
                case 2:
                    switch (select)
                    {
                        case 0://the le
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, ChonAiDay_Handler.TheLe, new List<string> { "OK"}, character.InfoChar.Gender));
                            character.TypeMenu = 6;
                            break;
                        case 1://vang
                            if (ChonAiDay_Handler.chonAiDay_Info.Status == ChonAiDay_Status.DELAY)
                            {
                                var time = (ChonAiDay_Handler.chonAiDay_Info.TimeEnd - ServerUtils.CurrentTimeMillis()) / 1000;
                                var text = "Chúc mừng các bạn may mắn được chọn lần trước là: \n" +
                                    $"{ChonAiDay_Handler.chonAiDay_Info.Name}" +
                                    $"Trò chơi sẽ bắt đầu sau {time}s nữa";
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, text, new List<string> { "Thể lệ", "OK"}, character.InfoChar.Gender));
                                character.TypeMenu = 7;
                            }
                            else {
                                var time = (ChonAiDay_Handler.chonAiDay_Info.TimeEnd - ServerUtils.CurrentTimeMillis()) / 1000;
                                var text = $"Tổng giải thường: {ChonAiDay_Handler.chonAiDay_Info.TongGiaiThuongVang[0]}, cơ hội trúng của bạn là: {ChonAiDay_Handler.gI().Getpercent(character, 0)}" +
                                    $"\nTổng giải VIP: {ChonAiDay_Handler.chonAiDay_Info.TongGiaiThuongVang[1]}, cơ hội trúng của bạn là: {ChonAiDay_Handler.gI().Getpercent(character, 1)}" +
                                    $"\nThời gian còn lại: {time} giây.";
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, text, new List<string> { "Cập nhật", "Thường\n10 thỏi vàng", "VIP\n100 thỏi vàng", "Đóng" }, character.InfoChar.Gender));
                                character.TypeMenu = 3;
                            }
                            break;
                        case 2://ngoc
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Chức năng đang được phát triển"));
                            break;
                    }
                    break;
                case 3:
                    switch (select)
                    {
                        case 0://update
                            var time = (ChonAiDay_Handler.chonAiDay_Info.TimeEnd - ServerUtils.CurrentTimeMillis()) / 1000;
                            var text = $"Tổng giải thường: {ChonAiDay_Handler.chonAiDay_Info.TongGiaiThuongVang[0]}, cơ hội trúng của bạn là: {ChonAiDay_Handler.gI().Getpercent(character, 0)}" +
                                $"\nTổng giải VIP: {ChonAiDay_Handler.chonAiDay_Info.TongGiaiThuongVang[1]}, cơ hội trúng của bạn là: {ChonAiDay_Handler.gI().Getpercent(character, 1)}" +
                                $"\nThời gian còn lại: {time} giây.";
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, text, new List<string> { "Cập nhật", "Thường\n10 thỏi vàng", "VIP\n100 thỏi vàng", "Đóng" }, character.InfoChar.Gender));
                            character.TypeMenu = 3;
                            break;
                        case 1://normal
                            ChonAiDay_Handler.gI().DatCuoc(character, 0);
                            break;
                        case 2://vjp
                            ChonAiDay_Handler.gI().DatCuoc(character, 1);
                            break;
                    }
                    break;
                case 7:
                    switch (select)
                    {
                        case 0://the le chon ai day
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, ChonAiDay_Handler.TheLe, new List<string> { "OK" }, character.InfoChar.Gender));
                            character.TypeMenu = 6;
                            break;
                    }
                    break;
            }
        }
        private static void ConfirmRongThan(Character character, short npcId, int select)
        {
            // character.CharacterHandler.SendMessage(Service.ServerMessage("select: " + select));
            switch (character.TypeDragon)
            {
                case 0:
                    switch (character.TypeMenu)
                    {
                        case 0:
                            switch (select)
                            {

                                case 0://+1 gang tay tren nguoi
                                    {
                                        var trangBi = character.ItemBody[2];

                                        if (trangBi == null)
                                        {
                                            character.CharacterHandler.SendMessage(Service.ServerMessage("Trên người của bạn không có găng tay"));
                                            break;
                                        }

                                        var optionCheck = trangBi.Options.FirstOrDefault(option => option.Id == 72);
                                        if (optionCheck != null)
                                        {
                                            if (optionCheck.Param >= DataCache.MAX_LIMIT_UPGRADE)
                                            {
                                                character.CharacterHandler.SendMessage(Service.ServerMessage("Găng tay của bạn đã được nâng cấp đến mức tối đa"));
                                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(24, MenuNpc.Gi().TextRongThan, MenuNpc.Gi().MenuDieuUocRongThan, 3));
                                                character.TypeMenu = 0;
                                                break;
                                            }
                                            optionCheck.Param += 1;
                                            trangBi.Options.Where(option => DataCache.IdOptionGoc.Contains(option.Id)).ToList().ForEach(
                                                        option => option.Param += option.Param / 10);
                                            character.CharacterHandler.SendMessage(Service.SendBody(character));
                                        }
                                        else
                                        {
                                            trangBi.Options.Add(new OptionItem()
                                            {
                                                Id = 72,
                                                Param = 1
                                            });
                                            trangBi.Options.Where(option => DataCache.IdOptionGoc.Contains(option.Id)).ToList().ForEach(
                                                            option => option.Param += option.Param / 10);
                                            character.CharacterHandler.SendMessage(Service.SendBody(character));
                                        }
                                       
                                        character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Điều ước của ngươi đã trở thành sự thật\nHẹn gặp ngươi lần sau, ta đi ngủ đây, bái bai"));
                                        character.CharacterHandler.SetUpInfo(true);
                                        character.CharacterHandler.SendMessage(Service.MeLoadPoint(character));
                                        character.CharacterHandler.SendMessage(Service.CallDragon(1, 0, character));
                                                                               ShenlongDragon.gI().WishFinish(character.Id);
                                        
                                        break;
                                    }
                                case 1://chí mạng gốc +2%
                                    {
                                        if (character.InfoChar.OriginalCrit < 10)
                                        {
                                            character.InfoChar.OriginalCrit += 2;
                                            character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Điều ước của ngươi đã trở thành sự thật\nHẹn gặp ngươi lần sau, ta đi ngủ đây, bái bai"));
                                            character.CharacterHandler.SetUpInfo(true);
                                            character.CharacterHandler.SendMessage(Service.MeLoadPoint(character));
                                        }
                                        else
                                        {
                                            character.CharacterHandler.SendMessage(Service.ServerMessage("Chí mạng của ngươi đã quá cao nên ta không đủ quyền năng thực hiện"));
                                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(24, MenuNpc.Gi().TextRongThan, MenuNpc.Gi().MenuDieuUocRongThan, 3));
                                            character.TypeMenu = 0;
                                        }
                                        character.CharacterHandler.SendMessage(Service.CallDragon(1, 0, character));
                                                                               ShenlongDragon.gI().WishFinish(character.Id);
                                        
                                        break;
                                    }
                                case 2://Doi ky nang de tu
                                    {
                                        var disciple = character.Disciple;
                                        var disciplePower = disciple.InfoChar.Power;
                                        if (disciplePower >= 150000000 && disciple.Skills.Count >= 2)
                                        {
                                            var randomSkill = DataCache.IdSkillDisciple2[ServerUtils.RandomNumber(DataCache.IdSkillDisciple2.Count)];
                                            disciple.Skills[1] = new SkillCharacter() // skill 2
                                            {
                                                Id = randomSkill,
                                                SkillId = Disciple.GetSkillId(randomSkill),
                                                Point = 1,
                                            };
                                        }

                                        if (disciplePower >= 1500000000 && disciple.Skills.Count >= 3)
                                        {
                                            var randomSkill = DataCache.IdSkillDisciple3[ServerUtils.RandomNumber(DataCache.IdSkillDisciple3.Count)];
                                            disciple.Skills[2] = new SkillCharacter() // skill 3
                                            {
                                                Id = randomSkill,
                                                SkillId = Disciple.GetSkillId(randomSkill),
                                                Point = 1,
                                            };
                                        }
                                        character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Điều ước của ngươi đã trở thành sự thật\nHẹn gặp ngươi lần sau, ta đi ngủ đây, bái bai"));
                                        character.CharacterHandler.SendMessage(Service.CallDragon(1, 0, character));
                                                                               ShenlongDragon.gI().WishFinish(character.Id);
                                        
                                        break;
                                    }
                               
                                case 3://+1 găng tay trên người đệ tử
                                    {
                                        var trangBi = character.Disciple.ItemBody[2];

                                        if (trangBi == null)
                                        {
                                            character.CharacterHandler.SendMessage(Service.ServerMessage("Trên người của đệ tử ngươi không có găng tay"));
                                            break;
                                        }

                                        var optionCheck = trangBi.Options.FirstOrDefault(option => option.Id == 72);
                                        if (optionCheck != null)
                                        {
                                            if (optionCheck.Param >= DataCache.MAX_LIMIT_UPGRADE)
                                            {
                                                character.CharacterHandler.SendMessage(Service.ServerMessage("Găng tay của đệ tử ngươi đã được nâng cấp đến mức tối đa"));
                                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(24, MenuNpc.Gi().TextRongThan, MenuNpc.Gi().MenuDieuUocRongThan, 3));
                                                character.TypeMenu = 0;
                                                break;
                                            }
                                            optionCheck.Param += 1;
                                            trangBi.Options.Where(option => DataCache.IdOptionGoc.Contains(option.Id)).ToList().ForEach(
                                                        option => option.Param += option.Param / 10);
                                            character.CharacterHandler.SendMessage(Service.SendBody(character.Disciple));
                                        }
                                        else
                                        {
                                            trangBi.Options.Add(new OptionItem()
                                            {
                                                Id = 72,
                                                Param = 1
                                            });
                                            trangBi.Options.Where(option => DataCache.IdOptionGoc.Contains(option.Id)).ToList().ForEach(
                                                            option => option.Param += option.Param / 10);
                                            character.CharacterHandler.SendMessage(Service.SendBody(character.Disciple));
                                        }

                                        character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Điều ước của ngươi đã trở thành sự thật\nHẹn gặp ngươi lần sau, ta đi ngủ đây, bái bai"));
                                        character.Disciple.CharacterHandler.SetUpInfo(true);
                                        character.CharacterHandler.SendMessage(Service.CallDragon(1, 0, character));
                                        ShenlongDragon.gI().WishFinish(character.Id);
                                        break;
                                    }
                                case 4://other menu
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(24, MenuNpc.Gi().TextRongThan, MenuNpc.Gi().MenuDieuUocRongThanOther, 3)); 
                                        character.TypeMenu = 1;
                                        character.CharacterHandler.SendMessage(Service.CallDragon(1, 0, character));
                                                                               ShenlongDragon.gI().WishFinish(character.Id);
                                        
                                        break;
                                    }
                            }
                            // character.Zone.ZoneHandler.SendMessage(Service.CallDragon(1, 0, character));
                           
                            break;
                        case 1://menu 2
                            {
                                switch (select)
                                {
                                    case 0:// đẹp trai nhất vũ trụ
                                        {
                                            var itemId = (character.InfoChar.Gender + 227);
                                            var itemAdd = ItemCache.GetItemDefault((short)itemId);
                                            itemAdd.Quantity = 1;
                                            character.CharacterHandler.AddItemToBag(true, itemAdd, "Ước NR");
                                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                                            character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Điều ước của ngươi đã trở thành sự thật\nHẹn gặp ngươi lần sau, ta đi ngủ đây, bái bai"));
                                            character.CharacterHandler.SendMessage(Service.CallDragon(1, 0, character));
                                                                                   ShenlongDragon.gI().WishFinish(character.Id);
                                            
                                            break;
                                        }
                                    case 1://capsule 1 sao
                                        {
                                            var itemAdd = ItemCache.GetItemDefault((short)869);
                                            itemAdd.Quantity = 1;
                                            character.CharacterHandler.AddItemToBag(true, itemAdd, "Ước NR");
                                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                                            character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Trôn\nTrôn Vi en\nta đi ngủ đây\nbái bai"));
                                            character.CharacterHandler.SendMessage(Service.CallDragon(1, 0, character));
                                                                                   ShenlongDragon.gI().WishFinish(character.Id);
                                            
                                            break;
                                        }
                                    //case 2:
                                    //    var exp = 200_000_000;
                                    //    character.InfoChar.Power += exp;
                                    //    character.InfoChar.Potential += exp;
                                    //    character.CharacterHandler.SendMessage(Service.UpdateExp(2, exp));
                                    //    character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Điều ước của ngươi đã trở thành sự thật\nHẹn gặp ngươi lần sau, ta đi ngủ đây, bái bai"));
                                    //    character.CharacterHandler.SendMessage(Service.CallDragon(1, 0, character));
                                    //                                           ShenlongDragon.gI().WishFinish(character.Id);
                                        
                                    //    break;
                                    case 2:
                                        character.PlusGold(2_000_000_000);
                                        character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                        character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Điều ước của ngươi đã trở thành sự thật\nHẹn gặp ngươi lần sau, ta đi ngủ đây, bái bai"));
                                        character.CharacterHandler.SendMessage(Service.CallDragon(1, 0, character));
                                                                               ShenlongDragon.gI().WishFinish(character.Id);
                                        
                                        break;
                                    case 3:
                                        character.PlusDiamondLock(50);
                                        character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                        character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Điều ước của ngươi đã trở thành sự thật\nHẹn gặp ngươi lần sau, ta đi ngủ đây, bái bai"));
                                        character.CharacterHandler.SendMessage(Service.CallDragon(1, 0, character));
                                                                               ShenlongDragon.gI().WishFinish(character.Id);
                                        
                                        break;
                                    case 4:
                                        character.CharacterHandler.AddItemToBag(true, ItemCache.GetItemDefault(1481), "Ước rồng thần");
                                        character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Điều ước của ngươi đã trở thành sự thật\nHẹn gặp ngươi lần sau, ta đi ngủ đây, bái bai"));
                                        character.CharacterHandler.SendMessage(Service.CallDragon(1, 0, character));
                                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                                        ShenlongDragon.gI().WishFinish(character.Id);
                                        break;
                                    case 5:
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(24, MenuNpc.Gi().TextRongThan, MenuNpc.Gi().MenuDieuUocRongThan, 3));
                                        character.TypeMenu = 0;
                                        break;
                                }
                                
                            }
                            break; 
                    }
                    break;
                case 1:
                    Rồng_Namec.gI().ConfirmMenu(character, npcId,select);
                    break;
                case 2:
                    BoneDragon.gI().Ước(character, select);
                    break;
                case 3:
                    IceDragon.gI().Wish(character,select);
                    break;
            }
        }
        private static void ConfirmBongBangGolden(Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                case 0:
                    switch (select)
                    {
                        case 0:
                            {
                                var inputHuVang = new List<InputBox>();
                                var inputCount = new InputBox()
                                {
                                    Name = "Nhập số hũ vàng muốn quy đổi thành",
                                    Type = 1,
                                };
                                inputHuVang.Add(inputCount);
                                character.CharacterHandler.SendMessage(Service.ShowInput($"1000 thỏi vàng bằng 1 hũ vàng, ngươi đang có: {character.CharacterHandler.GetAllQuantityItemBagById(457)} thỏi vàng", inputHuVang));
                                character.TypeInput = 210;
                            }
                            break;
                        case 1:
                            {
                                var inputHuVang = new List<InputBox>();
                                var inputCount = new InputBox()
                                {
                                    Name = "Nhập số hũ vàng ngươi muốn đổi thành thỏi vàng",
                                    Type = 1,
                                };
                                inputHuVang.Add(inputCount);
                                character.CharacterHandler.SendMessage(Service.ShowInput($"1 hũ vàng bằng 900 thỏi vàng, ngươi đang có: {character.CharacterHandler.GetAllQuantityItemBagById(1549)} hũ vàng", inputHuVang));
                                character.TypeInput = 211;
                            }
                            break;
                        case 2:
                            {
                                var inputHuVang = new List<InputBox>();
                                var inputCount = new InputBox()
                                {
                                    Name = "Nhập số hũ vàng ngươi muốn đổi thành hồng ngọc",
                                    Type = 1,
                                };
                                inputHuVang.Add(inputCount);
                                character.CharacterHandler.SendMessage(Service.ShowInput($"1 hũ vàng bằng 3 hồng ngọc, ngươi đang có: {character.CharacterHandler.GetAllQuantityItemBagById(1549)} hũ vàng", inputHuVang));
                                character.TypeInput = 212;
                            }
                            break;
                    }
                    break;
            }
        }
        private static void ConfirmObito(Character character, short npcId, int select)
        {
            switch (character.TypeMenu) {
                case 0:
                    switch (select)
                    {
                        case 0:
                            //   if (character.Player.Role != 1) return;
                            character.CharacterHandler.SendMessage(Service.SendCombinne0(MenuNpc.Gi().MenuBaHatMit[29], npcId));
                            character.ShopId = 21;
                            break;
                        case 1:
                            var text = DoiThuongHandler.DoiThuong(character, "Chế tác mắt hỗn mang", new List<int> { 1549}, new List<int> {1 }, (int) DoiThuongType.CHE_TAC_MAT_HON_MANG);
                            var menu = MenuNpc.Gi().MenuObito[1];
                            if (character.TypeDoiThuong == (int)DoiThuongType.CHE_TAC_MAT_HON_MANG)
                            {
                                menu = MenuNpc.Gi().MenuObito[2];
                            }
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, text, menu, character.InfoChar.Gender));
                            character.TypeMenu = 1;
                            break;
                    }
                    break;
                case 1:
                    if (character.TypeDoiThuong != (int)DoiThuongType.CHE_TAC_MAT_HON_MANG || select == 1) break;
                    var item = ItemCache.GetItemDefault(1280);
                    character.CharacterHandler.RemoveItemBagById(1549, 1);
                    character.CharacterHandler.AddItemToBag(false, item);
                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                    character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + ItemCache.ItemTemplate(item.Id).Name));
                    break;
            }
        }
        private static void ConfirmCalich(Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                case 0: 
                {
                    switch(select)
                    {
                        case 0://Nói chuyện
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, MenuNpc.Gi().TextCalich[1]));
                            break;
                        }
                        case 1:
                        {
                                    if (character.InfoTask.Id >= 23)
                                    {
                                        character.InfoMore.TransportMapId = 102;
                                        character.CharacterHandler.SendMessage(Service.Transport(20));
                                    }
                                    else
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiSay(npcId, "Không thể qua tương lai khi chưa làm nhiệm vụ"));
                                        // đến tương lai
                                    }
                            break;
                        }
                    }
                    break;
                }
                case 1:
                {
                    if (select != 0) return;
                    character.InfoMore.TransportMapId = 24;
                    character.CharacterHandler.SendMessage(Service.Transport(20));
                    break;
                }
            }
        }

        private static void ConfirmSanta(Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                case 0: 
                {
                        switch (select)
                        {
                            //case 0:
                            //{
                            //    var inputGiftcode = new List<InputBox>();
                            //    var inputCode = new InputBox(){
                            //        Name = "Nhập mã quà tặng",
                            //        Type = 1,
                            //    };
                            //    inputGiftcode.Add(inputCode);
                            //    character.CharacterHandler.SendMessage(Service.ShowInput("Giftcode Ngọc Rồng Tiên Kiếm", inputGiftcode));
                            //    character.TypeInput = 1;
                            //    break;
                            //}
                            case 0:
                                {
                                    var idShop = 18 + character.InfoChar.Gender;
                                    character.CharacterHandler.SendMessage(Service.Shop(character, 0, idShop));
                                    character.ShopId = idShop;
                                    character.TypeShop = 0;
                                    break;
                                }
                            case 1:
                                {
                                    var idShop = 42;
                                    character.CharacterHandler.SendMessage(Service.Shop(character, 0, idShop));
                                    character.ShopId = idShop;
                                    character.TypeShop = 0;
                                    break;
                                }
                            case 2:
                                {
                                    //giftcode
                                    break;
                                }
                            case 3://shop hsd
                                {
                                    var idShop = 41;
                                    character.CharacterHandler.SendMessage(Service.Shop(character, 0, idShop));
                                    character.ShopId = idShop;
                                    character.TypeShop = 0;
                                    break;
                                }
                            case 4://cat toc
                                {
                                    var idShop = 3 + character.InfoChar.Gender;
                                    character.CharacterHandler.SendMessage(Service.Shop(character, 0, idShop));
                                    character.ShopId = idShop;
                                    character.TypeShop = 0;
                                    break;
                                }

                            case 5://danh hieu
                                {
                                    var idShop = 4444;
                                    character.CharacterHandler.SendMessage(Service.Shop(character, 3, idShop));
                                    character.ShopId = idShop;
                                    character.TypeShop = 3;
                                    break;
                                }
                            case 6:
                                {
                                    if (character.CharacterHandler.GetItemBagById(1182) == null)
                                    {
                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Cần Mâm ngũ quả"));
                                        return;
                                    }
                                    character.CharacterHandler.AddItemToBag(true, ItemCache.GetItemDefault(1184));
                                    character.CharacterHandler.RemoveItemBagById(1182, 1);
                                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được Gói quà đặc biệt 2024"));

                                    break;
                                }
                            default:
                                {
                                    character.CharacterHandler.SendMessage(Service.NpcChat(npcId, TextServer.gI().UPDATING));
                                    break;
                                }
                        }
                    break;
                }
              
                case 1:
                {
                    switch(select)
                    {
                        case 0:
                                
                                    /// doi vang + ngoc 
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, string.Format(MenuNpc.Gi().TextSanta[5], ServerUtils.GetMoneys(UserDB.GetVND(character.Player))), MenuNpc.Gi().MenuSanta[3], character.InfoChar.Gender));
                                    character.TypeMenu = 2;

                                    //if(character.CharacterHandler.AddItemToBag(true, itemNew, "Đổi vàng")) {
                                    //  character.CharacterHandler.SendMessage(Service.SendBag(character));
                                    //character.CharacterHandler.SendMessage(Service.ServerMessage(string.Format(TextServer.gI().GET_GOLD_BAR, soLuongThoiVang)));



                                
                           
                            break;
                        
                        case 1:
                                // kich hoat thanh vien
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, string.Format(MenuNpc.Gi().TextSanta[4], ServerUtils.GetMoneys(UserDB.GetVND(character.Player))), MenuNpc.Gi().MenuSanta[6], character.InfoChar.Gender));
                                character.TypeMenu = 5;

                                break;
                    }

                    break;
                }
                case 2:
                    switch (select)
                    {
                        case 0:
                        case 1:
                            if (DatabaseManager.ConfigManager.gI().Rate_Thỏi_Vàng <= 1)
                            {
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, string.Format(MenuNpc.Gi().TextSanta[3] + "\n|1|Tình Trạng Khuyến Mãi [Không có Khuyến Mãi]", ServerUtils.GetMoneys(UserDB.GetVND(character.Player))), MenuNpc.Gi().MenuSanta[4 + select], character.InfoChar.Gender));
                                character.TypeMenu = 3 + select;
                                break;
                            }
                            else
                            {
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, string.Format(MenuNpc.Gi().TextSanta[3] + "\n|1|TÌnh Trạng Khuyến Mãi [Đang X" + DatabaseManager.ConfigManager.gI().Rate_Thỏi_Vàng +"]", ServerUtils.GetMoneys(UserDB.GetVND(character.Player))), MenuNpc.Gi().MenuSanta[4 + select], character.InfoChar.Gender));
                                character.TypeMenu = 3 + select;
                                break;
                            }
                    }
                    break;
                case 3:
                    switch (select)
                    {
                        
                    }
                    break;
                case 5:
                    switch (select)
                    {
                        case 0:
                            if (UserDB.GetVND(character.Player) < 20000)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Không Đủ Tiền, Vui Lòng Nạp Thêm"));
                                return;
                            }
                            if (!character.InfoChar.IsPremium)
                            {
                                var thoivang = ItemCache.GetItemDefault(457);
                                thoivang.Quantity = 20;
                                character.PlusDiamond(100000);
                                var soluong = thoivang.Quantity;
                                character.CharacterHandler.AddItemToBag(true, thoivang, "Đổi vàng");
                                character.CharacterHandler.SendMessage(Service.SendBag(character));
                                character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                                //  character.CharacterHandler.SendMessage(Service.ServerMessage(string.Format(TextServer.gI().GET_GOLD_BAR, soluong)));
                                /// ------------->>>>
                                character.InfoChar.IsPremium = true;
                                UserDB.MineVND(character.Player, 20000);
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Kích hoạt thành viên thành công !!!, Chúc bạn chơi game vui vẻ"));
                                character.CharacterHandler.SendMessage(Service.MeLoadAll(character));
                            } else
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã kích hoạt thành viên rồi !!"));
                            }
                            break;
                    }
                    
                    break;
            }
        }

        private static void ConfirmTrungThu(Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                //Menu chính
                case 99:
                {
                    switch (select)
                    {
                        case 0://shop
                        {
                            character.CharacterHandler.SendMessage(Service.Shop(character, 3, 23));
                                    character.TypeShop = 3;
                            character.ShopId = 23;
                        
                            break;
                        }
                        case 1:
                                {
                                    break;
                                }
                        case 2:
                        {
                            //bang xep hang
                            break;
                        }
                    }
                    break;
                }
            }
        }
       
        

        private static void ConfirmQuocVuong(Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                //Menu chính
                case 0:
                    {
                        switch (select)
                        {
                            case 0:
                                {
                                    try
                                    {
                                        var limit = character.InfoChar.LitmitPower;
                                        if (limit >= DataCache.MAX_LIMIT_POWER_LEVEL)
                                        {
                                            character.CharacterHandler.SendMessage(Service.NpcChat(npcId, "Con đã đạt giới hạn tối đa"));
                                            return;
                                        }
                                        var LM = Cache.Gi().LIMIT_POWERS[limit];
                                        var ngoc = 100 * (limit + 1);
                                        var text = string.Format(TextServer.gI().UPGRADE_LEVEL_ME, ServerUtils.GetPower(LM.Power), ngoc);
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, text, MenuNpc.Gi().MenuQuocVuong[1], character.InfoChar.Gender));
                                        character.TypeMenu = 1;
                                    }
                                    catch (Exception)
                                    {
                                        character.CharacterHandler.SendMessage(Service.NpcChat(npcId, "Con đã đạt giới hạn tối đa"));
                                    }
                                    break;
                                }
                            case 1:
                                {
                                    try
                                    {
                                        if (character.Disciple == null)
                                        {
                                            character.CharacterHandler.SendMessage(Service.NpcChat(npcId, "Con chưa có đệ tử !"));
                                            return;
                                        }
                                        
                                        var limit = character.Disciple.InfoChar.LitmitPower;
                                        if (limit >= DataCache.MAX_LIMIT_POWER_LEVEL)
                                        {
                                            character.CharacterHandler.SendMessage(Service.NpcChat(npcId, "Đệ tử con đã đạt giới hạn tối đa"));
                                            return;
                                        }
                                        var LM = Cache.Gi().LIMIT_POWERS[limit];
                                        var ngoc = 100 * (limit + 1);
                                        var text = string.Format(TextServer.gI().UPGRADE_LEVEL_ME, ServerUtils.GetPower(LM.Power), ngoc);
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, text, MenuNpc.Gi().MenuQuocVuong[1], character.Disciple.InfoChar.Gender));
                                        character.TypeMenu = 2;
                                    }
                                    catch (Exception)
                                    {
                                        character.CharacterHandler.SendMessage(Service.NpcChat(npcId, "Đệ tử con đã đạt giới hạn tối đa"));
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                case 2:
                    {
                        var limit = character.Disciple.InfoChar.LitmitPower;
                        if (limit >= DataCache.LITMIT_OPEN_NEED_GOD_ITEM)
                        {
                            var countItem = 0;
                            foreach (var item in character.ItemBody)
                            {
                                if (item != null && ItemCache.ItemTemplate(item.Id).Level == 13)
                                {
                                    countItem++;
                                }
                            }
                            if (countItem == 0)
                            {
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Để mở giới hạn sức mạnh này con phải đạt cấp độ Giới Vương Thần\nvà mặc trên người ít nhất 1 trong 5 món trang bị Thần\ngồm Áo, Quần, Găng, Giày, Nhẫn", new List<string> { "OK" }, character.InfoChar.Gender));
                                character.TypeMenu = 3;
                                return;
                            }
                            else
                            {
                                var ngoc2 = 100 * (limit + 1);
                                if (ngoc2 > character.AllDiamond())
                                {
                                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_DIAMOND));
                                    return;
                                }

                                character.Disciple.InfoChar.IsPower = true;
                                character.Disciple.InfoChar.LitmitPower += 1;
                                character.MineDiamond(ngoc2);
                                character.Disciple.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                                character.Disciple.CharacterHandler.SendMessage(Service.NpcChat(npcId, "Chúc mừng con đạt tới sức mạnh mới"));
                            }
                        }else{
                        if (limit == DataCache.MAX_LIMIT_POWER_LEVEL)
                        {
                            character.CharacterHandler.SendMessage(Service.NpcChat(npcId, "Con đã đạt giới hạn tối đa"));
                            return;
                        }
                        if (character.InfoChar.Power < 17999999999)
                        {
                            character.CharacterHandler.SendMessage(Service.NpcChat(npcId, "Con chưa đủ sức mạnh để mới giới hạn"));
                            return;
                        }
                        var ngoc = 100 * (limit + 1);
                        if (ngoc > character.AllDiamond())
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_DIAMOND));
                            return;
                        }

                        character.Disciple.InfoChar.IsPower = true;
                        character.Disciple.InfoChar.LitmitPower += 1;
                        character.MineDiamond(ngoc);
                        character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                        character.CharacterHandler.SendMessage(Service.NpcChat(npcId, "Chúc mừng con đạt tới sức mạnh mới"));
                    }
                    }
                    break;
                case 1:
                {
                        switch (select)
                        {
                            case 0:
                                {
                                    break;
                                }
                            case 1:
                                {
                                    var limit = character.InfoChar.LitmitPower;
                                    if (limit >= DataCache.LITMIT_OPEN_NEED_GOD_ITEM)
                                    {
                                        var countItem = 0;
                                        foreach(var item in character.ItemBody)
                                        {
                                            if (item != null && ItemCache.ItemTemplate(item.Id).Level == 13)
                                            {
                                                countItem++;
                                            }
                                        }
                                        if (countItem == 0)
                                        {
                                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Để mở giới hạn sức mạnh này con phải đạt cấp độ Giới Vương Thần\nvà mặc trên người ít nhất 1 trong 5 món trang bị Thần\ngồm Áo, Quần, Găng, Giày, Nhẫn", new List<string> { "OK"}, character.InfoChar.Gender));
                                            character.TypeMenu = 3;
                                            return;
                                        }
                                        else
                                        {
                                            var ngoc2 = 100 * (limit + 1);
                                            if (ngoc2 > character.AllDiamond())
                                            {
                                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_DIAMOND));
                                                return;
                                            }

                                            character.InfoChar.IsPower = true;
                                            character.InfoChar.LitmitPower += 1;
                                            character.MineDiamond(ngoc2);
                                            character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                                            character.CharacterHandler.SendMessage(Service.NpcChat(npcId, "Chúc mừng con đạt tới sức mạnh mới"));
                                        }
                                    }else{
                                    if (limit == DataCache.MAX_LIMIT_POWER_LEVEL)
                                    {
                                        character.CharacterHandler.SendMessage(Service.NpcChat(npcId, "Con đã đạt giới hạn tối đa"));
                                        return;
                                    }
                                    if (character.InfoChar.Power < 17999999999)
                                    {
                                        character.CharacterHandler.SendMessage(Service.NpcChat(npcId, "Con chưa đủ sức mạnh để mới giới hạn"));
                                        return;
                                    }
                                    var ngoc = 100 * (limit + 1);
                                    if (ngoc > character.AllDiamond())
                                    {
                                        character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_DIAMOND));
                                        return;
                                    }

                                    character.InfoChar.IsPower = true;
                                    character.InfoChar.LitmitPower += 1;
                                    character.MineDiamond(ngoc);
                                    character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                                    character.CharacterHandler.SendMessage(Service.NpcChat(npcId, "Chúc mừng con đạt tới sức mạnh mới"));
                                    }
                                    break;
                                }
                        }
                    break;
                }
            }
        }

        private static void ConfirmGiuMa(Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                //Menu chính
                case 1:
                    {
                        switch (select)
                        {
                            case 0:
                               
                                var clan = ClanManager.Get(character.ClanId);
                                if (clan.ClanBoss.Status == Model.Clan.ClanBoss.ClanBoss_Status.OPEN || clan.ClanBoss.Status == Model.Clan.ClanBoss.ClanBoss_Status.END) return;
                                switch (clan.ClanBoss.Count)
                                {
                                    case 0:
                                        break;
                                    case 1:
                                        character.MineDiamond(100);
                                        break;
                                    case >= 2:
                                        character.MineDiamond(300);
                                        break;
                                }
                                clan.ClanBoss.Start();
                                clan.ClanZone.Map.OutZone(character);
                                clan.ClanBoss.Map.JoinZone(character, 0);
                                clan.ClanBoss.SendTextTime(clan, null);
                                break;
                        }
                        break;
                    }
                case 0:
                    {
                        switch (select)
                        {
                            case 0:
                                {
                                    var clan = ClanManager.Get(character.ClanId);
                                    if (clan.ClanBoss.Status == Model.Clan.ClanBoss.ClanBoss_Status.OPEN)
                                    {
                                        clan.ClanZone.Map.OutZone(character);
                                        clan.ClanBoss.Map.JoinZone(character, 0);
                                        character.CharacterHandler.SendMessage(Service.ItemTimeWithMessage($"Hạ Boss Bang Hội [lần thứ {clan.ClanBoss.Level + 1}] thời gian:", 200, ((int)(clan.ClanBoss.Time - ServerUtils.CurrentTimeMillis()) / 1000)));
                                        break;
                                    }
                                    if (clan.ClanBoss.Status == Model.Clan.ClanBoss.ClanBoss_Status.END)
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, "Bạn đã chiến thắng hôm nay, mai hãy quay lại nhé ", new List<string> { "OK" }, character.InfoChar.Gender));
                                        character.TypeMenu = 1;
                                        break;
                                    }
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextGiuMa[1], MenuNpc.Gi().MenuGiuMa[2 + clan.ClanBoss.Count >= 2 ? 2 : clan.ClanBoss.Count], character.InfoChar.Gender));
                                    character.TypeMenu = 1;
                                    break;
                                }
                            case 1:
                                {
                                    if (!character.InfoChar.isDiemDanh)
                                    {
                                        character.InfoChar.isDiemDanh = true;

                                        ClanManager.Get(character.ClanId).Capsule_Bang++;
                                        var me = ClanManager.Get(character.ClanId).Thành_viên.FirstOrDefault(i => i.Id == character.Id);
                                        me.Capsule_Cá_Nhân++;
                                        me.Capsule_Bang++;
                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được 1 Capsule Bang"));
                                        break;
                                    }
                                    var clan = ClanManager.Get(character.ClanId);
                                    clan.ClanZone.Map.OutZone(character);
                                    character.InfoMore.TransportMapId = 156;
                                    character.CharacterHandler.SendMessage(Service.Transport(3, 1));
                                }
                                break;
                            case 2:
                                {
                                    if (!character.InfoChar.isDiemDanh)
                                    {
                                        var clan = ClanManager.Get(character.ClanId);
                                        clan.ClanZone.Map.OutZone(character);
                                        character.InfoMore.TransportMapId = 156;
                                        character.CharacterHandler.SendMessage(Service.Transport(3, 1));
                                    }
                                    break;
                                }
                        }
                    break;
                }   
            }
        }
        private static void ConfirmNgoKhong(Character character, short npcId, int select)
        {
            short idDao = 0;
            switch (select)
            {
                case 0:
                    idDao = 541;//id quả hồng đào
                    break;
                case 1:
                    idDao = 542;//id quả hồng đào chín
                    break;
            }
            if (character.CharacterHandler.GetItemBagById(idDao) == null)
            {
                character.CharacterHandler.SendMessage(Service.ServerMessage($"Cần 1 {ItemCache.ItemTemplate(idDao).Name}"));
            }
            else
            {
                character.CharacterHandler.RemoveItemBagById(idDao, 1);
                character.CharacterHandler.SendMessage(Service.SendBag(character));
                var itemMap = LeaveItemHandler.LeaveBuaNguHanhSon(character.Id);
                itemMap.X = (short)(character.InfoChar.X + ServerUtils.RandomNumber(-20, 20));
                character.Zone.ZoneHandler.LeaveItemMap(itemMap);
            }
        }
        
        private static void ConfirmDuongTang(Character character, short npcId, int select)
        {
            var map = MapManager.Get(character.InfoChar.MapId);
            if (map == null) return;
           IMap mapJoin = null;
            switch (character.TypeMenu)
            {
                case 3://đổi điểm công duck
                    switch (select)
                    {

                    }
                    break;
                //Menu chính
                case 0:
                    {
                       switch (select)
                        {
                            case 0:
                                if (character.InfoChar.Power >= 1_500_000)
                                {
                                    mapJoin = MapManager.Get(123);
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Nhân vật cần đạt 1tr5 sức mạnh mới đủ trình độ giải cứu ngộ không"));

                                }
                                break;
                            case 1:
                                if (character.CharacterHandler.GetItemBagById(543) == null)
                                {
                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Cần Vòng Kim Cô mua tại Santa ở Đảo Kame"));
                                }
                                break;
                            case 3:
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextDuongTang[3], MenuNpc.Gi().MenuDuongTang[3], character.InfoChar.Gender));
                                    character.TypeMenu = 3;
                                }
                                break;
                        }
                    }
                    if (mapJoin == null) return;
                    var zoneJoin = mapJoin.GetZoneNotMaxPlayer();
                    if (zoneJoin != null)
                    {
                        character.CharacterHandler.SendZoneMessage(Service.SendTeleport(character.Id, 2));
                        map.OutZone(character);
                        character.InfoChar.X = 106;
                        character.InfoChar.Y = 384;
                        zoneJoin.ZoneHandler.JoinZone(character, false, false, 2);
                    }
                    else
                    {
                        character.CharacterHandler.SendMessage(Service.OpenUiSay(5, TextServer.gI().MAX_NUMCHARS, false, character.InfoChar.Gender));
                    }
                    break;
                case 1:
                    {
                        switch (select)
                        {
                            case 0:
                                mapJoin = MapManager.Get(0);
                                break;
                        }
                        
                    }
                    if (mapJoin == null) return;
                    var zoneJoin2 = mapJoin.GetZoneNotMaxPlayer();
                    if (zoneJoin2 != null)
                    {
                        character.CharacterHandler.SendZoneMessage(Service.SendTeleport(character.Id,2));
                        map.OutZone(character);
                        zoneJoin2.ZoneHandler.JoinZone(character, false, true, 2);
                    }
                    else
                    {
                        character.CharacterHandler.SendMessage(Service.OpenUiSay(5, TextServer.gI().MAX_NUMCHARS, false, character.InfoChar.Gender));
                    }
                    break;

                case 2:
                    {
                        switch (select)
                        {
                            case 1  :
                                character.CharacterHandler.SendMessage(Server.Gi().BangXepHang.ListTopInfoEvent());
                                break;
                            case 0:
                                //if (character.CharacterHandler.GetItemBagById(457) == null || character.CharacterHandler.GetItemBagById(457).Quantity < 50)
                                //{
                                //    character.CharacterHandler.SendMessage(Service.ServerMessage("yêu cầu 50 thỏi vàng"));
                                //    return;
                                //}
                                var giai = character.CharacterHandler.GetItemBagById(537);
                                var khai = character.CharacterHandler.GetItemBagById(538);
                                var phong = character.CharacterHandler.GetItemBagById(539);
                                var an = character.CharacterHandler.GetItemBagById(540);
                                if (giai != null && khai != null && phong != null && an != null) {
                                    if (giai.Quantity >= 10 && khai.Quantity >= 10 && phong.Quantity >= 10 && an.Quantity >= 10)
                                    {
                                        for (short dball = 537; dball <= 540; dball++)
                                        {
                                            character.CharacterHandler.RemoveItemBagById(dball, 10, "Ngũ Hành Sơn");
                                        }
                                        var randomRate = ServerUtils.RandomNumber(0.0, 100.0);
                                        var random = ServerUtils.RandomNumber(0.0, 100.0);
                                        var itemAdd2 = ItemCache.GetItemDefault(1);

                                        if (randomRate <= 20.0)
                                        {
                                            itemAdd2 = ItemCache.GetItemDefault(544);
                                        }
                                        else if (randomRate <= 40.0)
                                        {
                                            itemAdd2 = ItemCache.GetItemDefault(545);   
                                        }
                                        else if (randomRate <= 60.0)
                                        {
                                            itemAdd2 = itemAdd2 = ItemCache.GetItemDefault(546);
                                        }
                                      
                                        else
                                        {
                                            itemAdd2 = itemAdd2 = ItemCache.GetItemDefault(543);
                                        }
                                        itemAdd2.Reason = "Ngũ Hành Sơn";
                                        itemAdd2.Options.Add(new OptionItem()
                                       {
                                            Id = 77,
                                            Param = ServerUtils.RandomNumber(15, 25),
                                        }) ;
                                        itemAdd2.Options.Add(new OptionItem()
                                        {
                                            Id = 103,
                                            Param = ServerUtils.RandomNumber(15, 25),
                                        });
                                        itemAdd2.Options.Add(new OptionItem()
                                        {
                                            Id = 50,
                                            Param = ServerUtils.RandomNumber(15, 25),
                                        });
                                        itemAdd2.Options.Add(new OptionItem()
                                        {
                                            Id = 101,
                                            Param = ServerUtils.RandomNumber(15,25),
                                        });
                                        if (random <= 10)
                                        {

                                            itemAdd2.Options.Add(new OptionItem()
                                            {
                                                Id = 95,
                                                Param = ServerUtils.RandomNumber(10, 25),
                                            });
                                        } else if (random <= 30)
                                        {
                                            itemAdd2.Options.Add(new OptionItem()
                                            {
                                                Id = 96,
                                                Param = ServerUtils.RandomNumber(10, 25),
                                            });
                                        }
                                        itemAdd2.Quantity = 1;
                                        character.CharacterHandler.AddItemToBag(true, itemAdd2, "SuKien");
                                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                                        var template2 = ItemCache.ItemTemplate(itemAdd2.Id);
                                        character.CharacterHandler.SendMessage(
                                        Service.ServerMessage(string.Format(TextServer.gI().ADD_ITEM,
                                         $"x{itemAdd2.Quantity} {template2.Name}")));
                                        //character.CharacterHandler.RemoveItemBagById(457, 50);
                                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                                        character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                        break;
                                    } else
                                    {
                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Không đủ Vật Phẩm"));
                                    }
                                } else
                                {
                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn Không Có Bùa"));
                                }
                                break;
                        }
                    }
                    break;
            }
        }
        private static void ComfirmBabiday(Character character, short npcId, int select)
        {
            switch (select)
            {
                case 0:
                    break;
                case 1:
                    character.MineDiamond(1);
                    break;
                case 2:
                    if (character.PPower >= 20)
                    {
                        if (character.InfoChar.MapId == 115) MapManager.JoinMap(character, 117, ServerUtils.RandomNumber(20), false, false, 0);
                        else if (character.InfoChar.MapId == 120) MapManager.JoinMap(character, 52, ServerUtils.RandomNumber(20), false, false, 0);
                        else MapManager.JoinMap(character,  character.InfoChar.MapId + 1, ServerUtils.RandomNumber(20), false, false, 0);
                        character.PPower = 0;
                    }
                    else
                    {
                        character.CharacterHandler.SendMessage(Service.ServerMessage("Không đủ TL"));
                    }
                    break;
                case 3:
                    MapManager.JoinMap(character, 52, ServerUtils.RandomNumber(20), false, false, 0);
                    break;
            }
        }
        private static void ConfirmOsin(Character character, short npcId, int select)
        {
            var map = MapManager.Get(character.InfoChar.MapId);
            if (map == null) return;
           IMap mapJoin = null;
            switch (character.TypeMenu)
            {
                //Menu chính
                case 0:
                    {
                        switch (select)
                        {
                            case 0:
                                switch (character.InfoChar.MapId)
                                {
                                    //case 0 or 7 or 14:
                                    //    character.CharacterHandler.SendMessage(Server.Gi().BangXepHang.ListTopInfoEvent());
                                    //    break;
                                    case 50:
                                        MapManager.GetMapOffline(48).JoinZone(character, character.Id);

                                        break;
                                    case 155:
                                        {
                                            MapManager.GetMapOffline(154).JoinZone(character, character.Id);

                                        
                                        }
                                        break;

                                    case 127:
                                        {
                                            if (character.DataEnchant.PhuHoMabu2h)
                                            {
                                                character.CharacterHandler.SendMessage(Service.ServerMessage("Ngươi đã được phù hộ rồi !"));
                                            }
                                            else
                                            {
                                                character.DataEnchant.PhuHoMabu2h = true;
                                                character.CharacterHandler.SetUpInfo();
                                                character.CharacterHandler.SendMessage(Service.MeLoadPoint(character));
                                            }
                                            break;
                                        }
                                    default:
                                        if (Mabu12hConfig.Status == Mabu12hStatus.DURING)
                                        {
                                            Mabu12h.gI().JoinMap(character);
                                        }
                                        else if (Mabu2hConfig.Status == Mabu2hStatus.DURING)
                                        {
                                            mapJoin = MapManager.Get(127);
                                        }
                                        break;
                                }
                                break;
                            case 1:
                                switch (character.InfoChar.MapId)
                                {
                                    case 50:
                                        {
                                            if (character.InfoSet.IsFullSetHuyDiet)
                                            {
                                                MapManager.GetMapOffline(154).JoinZone(character, character.Id);
                                            }
                                            else
                                            {
                                                character.CharacterHandler.SendMessage(Service.ServerMessage("Nơi đây là nơi ở của thần hủy diệt, ngươi hãy mặc đủ bộ hủy diệt rồi tới đây tìm ta"));
                                            }
                                        }
                                        break;
                                    case 154:
                                        {
                                            mapJoin = MapManager.Get(155);

                                        }
                                        break;
                                    case 127:

                                        break;
                                    default:
                                        character.MineDiamond(1);
                                        break;
                                }
                                break;
                            case 2:
                                switch (character.InfoChar.MapId)
                                {
                                    case 127:
                                        {
                                            mapJoin = MapManager.Get(52);
                                        }
                                        break;
                                    default:
                                        if (character.PPower >= 20)
                                        {
                                            if (character.InfoChar.MapId == 115) MapManager.JoinMap(character, 117, ServerUtils.RandomNumber(20), false, false, 0);
                                            else if (character.InfoChar.MapId == 120) MapManager.JoinMap(character, 52, ServerUtils.RandomNumber(20), false, false, 0);
                                            else MapManager.JoinMap(character, character.InfoChar.MapId + 1, ServerUtils.RandomNumber(20), false, false, 0);
                                            character.PPower = 0;
                                        }
                                        else
                                        {
                                            character.CharacterHandler.SendMessage(Service.ServerMessage("Không đủ TL"));
                                        }
                                        break;
                                }
                                break;
                        }
                    }
                    if (mapJoin == null) return;
                    var zoneJoin = mapJoin.GetZoneNotMaxPlayer();
                    if (zoneJoin != null)
                    {
                        character.CharacterHandler.SendZoneMessage(Service.SendTeleport(character.Id, character.InfoChar.Teleport));
                        map.OutZone(character);
                        zoneJoin.ZoneHandler.JoinZone(character, false, true, character.InfoChar.Teleport);
                    }
                    else
                    {
                        character.CharacterHandler.SendMessage(Service.OpenUiSay(5, TextServer.gI().MAX_NUMCHARS, false, character.InfoChar.Gender));
                    }
                    break;
                case 1:
                    break;

                case 2:
                   
                    break;
            }
        }
        private static void ConfirmQuaTrung(Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                case 0://Chua đủ thời gian
                {
                    switch(select)
                    {
                        case 0://Chờ, bỏ qua
                        {
                            break;
                        }
                        case 1://Dùng tiền để nở trứng
                        {
                            var disciple = character.Disciple;
                            if (disciple != null)
                            {
                                var itemDiscipleBody = disciple.ItemBody.FirstOrDefault(item => item != null);

                                if (itemDiscipleBody != null)
                                {
                                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().PLEASE_EMPTY_DISCIPLE_BODY));
                                    return;
                                }
                            }
                            // Kiểm tra trạng thái hợp thể
                            if (character.InfoChar.Fusion.IsFusion)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().PLEASE_NOT_FUSION));
                                return;
                            }

                            if (character.InfoChar.Gold < DataCache.GIA_NO_TRUNG_MA_BU)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_GOLD));
                                return;
                            }

                            // Kiểm tra sức mạnh đệ tử 20 tỷ
                            if (character.Disciple != null && character.Disciple.InfoChar.Power < 160000000)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().DISCIPLE_NOT_ENOUGH_POWER_TO_OPEN_EGG));
                                return;
                            }

                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextQuaTrung[1], MenuNpc.Gi().MenuQuaTrung[2], character.InfoChar.Gender));
                            character.TypeMenu = 2;
                            break;
                        }
                    }
                    break;
                }
                case 1: //Menu đủ thời gian
                {
                    switch(select)
                    {
                        case 0: //Nở trứng
                        {
                            var disciple = character.Disciple;
                            if (disciple != null)
                            {
                                var itemDiscipleBody = disciple.ItemBody.FirstOrDefault(item => item != null);

                                if (itemDiscipleBody != null)
                                {
                                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().PLEASE_EMPTY_DISCIPLE_BODY));
                                    return;
                                }
                            }
                            // Kiểm tra trạng thái hợp thể
                            if (character.InfoChar.Fusion.IsFusion)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().PLEASE_NOT_FUSION));
                                return;
                            }

                            if (character.Disciple != null && character.Disciple.InfoChar.Power < 160000000)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().DISCIPLE_NOT_ENOUGH_POWER_TO_OPEN_EGG));
                                return;
                            }

                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextQuaTrung[1], MenuNpc.Gi().MenuQuaTrung[2], character.InfoChar.Gender));
                            character.TypeMenu = 3;
                            break;
                        }
                    }
                    break;
                }
                case 2:
                {
                    character.MineGold(DataCache.GIA_NO_TRUNG_MA_BU);
                    CreateDiscipleMabu(character, (sbyte)select);
                    break;
                }
                case 3:
                {
                    CreateDiscipleMabu(character, (sbyte)select);
                    break;
                }
            }
        }
        
        private static void ConfirmBill(Character character, short npcId, int select)
        {
            switch (character.TypeMenu)
            {
                case 0:
                {
                    if (select != 0) return;
                    var fullThucAn = character.ItemBag.FirstOrDefault(item => DataCache.ListThucAn.Contains(item.Id) && item.Quantity >= 99);

                    if (character.InfoSet.IsFullSetThanLinh && fullThucAn != null)
                    {
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextBill[2], MenuNpc.Gi().MenuBill[2], character.InfoChar.Gender));
                        character.TypeMenu = 2;
                    }
                    else 
                    {   
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextBill[1], MenuNpc.Gi().MenuBill[1], character.InfoChar.Gender));
                        character.TypeMenu = 1;
                    }
                    break;
                }   
                case 2:
                {
                    if (select != 0) return;
                    character.CharacterHandler.SendMessage(Service.Shop(character, 3, 21));
                    character.ShopId = 21;
                    character.TypeShop = 3;
                    break;
                }
            }
        }
        private static void ConfirmNoiBanh(Character character, short npcId, int select)
        {   
            switch (character.TypeMenu)
            {
                case 0:
                {
                    switch (select)
                    {
                            case 0:
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, MenuNpc.Gi().TextNoiBanh[4], MenuNpc.Gi().MenuNoiBanh[6], character.InfoChar.Gender));
                                    character.TypeMenu = 4;
                                    //Tết là TypeMenu 1;
                                    //Hùng Vương là TypeMenu = 4;
                                }
                                break;
                        //case 0:
                        //{ // nấu bằng ngọc type menu 1
                        //    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId,
                        //        MenuNpc.Gi().TextNoiBanh[1], MenuNpc.Gi().MenuNoiBanh[1], character.InfoChar.Gender));
                        //    character.TypeMenu = 1;
                        //    break;
                        //}
                        //case 1:
                        //{ // nấu bằng vàng type menu 2
                        //    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId,
                        //        MenuNpc.Gi().TextNoiBanh[2], MenuNpc.Gi().MenuNoiBanh[1], character.InfoChar.Gender));
                        //    character.TypeMenu = 2;
                        //    break;
                        //}
                    }

                    break;
                    }   
                case 1: // xử lý client đã chọn nấu, sự kiện Tết
                {
                        switch (select)
                        {
                            case 0:
                                {
                                    var text = DoiThuongHandler.DoiThuong(character, "Bạn muốn nấu bánh tét?", new List<int> { 748, 749, 750, 751, -1 }, new List<int> { 10, 10, 10, 10, 5000000 }, (int)DoiThuongType.NAU_BANH_TET);
                                    var menu = character.TypeDoiThuong == (int)DoiThuongType.NAU_BANH_TET ? MenuNpc.Gi().MenuNoiBanh[1] : MenuNpc.Gi().MenuNoiBanh[2];
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, text, menu, character.InfoChar.Gender));
                                    character.TypeMenu = 2;
                                    break;
                                }
                            case 1:
                                {
                                    var text = DoiThuongHandler.DoiThuong(character, "Bạn muốn nấu bánh chưng?", new List<int> { 748, 749, 750, 751, 886, -1 }, new List<int> { 10, 10, 10, 10, 1, 5000000 }, (int)DoiThuongType.NAU_BANH_CHUNG);
                                    var menu = character.TypeDoiThuong == (int)DoiThuongType.NAU_BANH_CHUNG ? MenuNpc.Gi().MenuNoiBanh[1] : MenuNpc.Gi().MenuNoiBanh[2];
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, text, menu, character.InfoChar.Gender));
                                    character.TypeMenu = 2;
                                    break;
                                }
                        }

                        break;
                }
                case 4: // xử lý client đã chọn nấu, sự kiện Tết
                    {
                        switch (select)
                        {
                            case 0:
                                {
                                    var text = DoiThuongHandler.DoiThuong(character, "Bạn muốn nấu bánh dầy?", new List<int> { 1595, 1596, 1594, 1593, -1 }, new List<int> { 99, 5, 2, 1, 1000000 }, (int)DoiThuongType.NAU_BANH_DAY);
                                    var menu = character.TypeDoiThuong == (int)DoiThuongType.NAU_BANH_DAY ? MenuNpc.Gi().MenuNoiBanh[1] : MenuNpc.Gi().MenuNoiBanh[2];
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, text, menu, character.InfoChar.Gender));
                                    character.TypeMenu = 2;
                                    break;
                                }
                            case 1:
                                {
                                    var text = DoiThuongHandler.DoiThuong(character, "Bạn muốn nấu bánh chưng Lang Liêu?", new List<int> { 1595, 1597, 1598, -1 }, new List<int> { 99, 2, 2, 1000000 }, (int)DoiThuongType.NAU_BANH_CHUNG_LANG_LIEU);
                                    var menu = character.TypeDoiThuong == (int)DoiThuongType.NAU_BANH_CHUNG_LANG_LIEU ? MenuNpc.Gi().MenuNoiBanh[1] : MenuNpc.Gi().MenuNoiBanh[2];
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(npcId, text, menu, character.InfoChar.Gender));
                                    character.TypeMenu = 2;
                                    break;
                                }
                        }

                        break;
                    }
                case 2: //xử lý client đã chọn nấu bằng vàng
                {
                    switch (character.TypeDoiThuong)
                    {
                            case (int)DoiThuongType.NAU_BANH_TET:
                                character.CharacterHandler.RemoveItemBagById(748, 10);
                                character.CharacterHandler.RemoveItemBagById(749, 10);
                                character.CharacterHandler.RemoveItemBagById(750, 10);
                                character.CharacterHandler.RemoveItemBagById(751, 10);
                                character.MineGold(5000000);
                                Server.Gi().GameCache.ThemBanhDangNau(character.Id, false, ServerUtils.CurrentTimeMillis() + DataCache._1MINUTES * 3, 752);
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Bánh đang được nấu, bánh sẽ chín sau 3 phút, nhớ quay lại đây lấy bánh"));
                                character.CharacterHandler.SendMessage(Service.SendBag(character));
                                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                break;
                            case (int)DoiThuongType.NAU_BANH_CHUNG:
                                character.CharacterHandler.RemoveItemBagById(748, 10);
                                character.CharacterHandler.RemoveItemBagById(749, 10);
                                character.CharacterHandler.RemoveItemBagById(750, 10);
                                character.CharacterHandler.RemoveItemBagById(751, 10);
                                character.CharacterHandler.RemoveItemBagById(886, 1);
                                character.MineGold(5000000);
                                Server.Gi().GameCache.ThemBanhDangNau(character.Id, false, ServerUtils.CurrentTimeMillis() + DataCache._1MINUTES * 3, 753);
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Bánh đang được nấu, bánh sẽ chín sau 3 phút, nhớ quay lại đây lấy bánh"));
                                character.CharacterHandler.SendMessage(Service.SendBag(character));
                                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                break;
                            case (int)DoiThuongType.NAU_BANH_CHUNG_LANG_LIEU:
                                break;
                            case (int)DoiThuongType.NAU_BANH_DAY:
                                break;
                        }

                        break;
                }
                case 3:
                    {
                        switch (select)
                        {
                            case 0:
                                var idBanh = Server.Gi().GameCache.GetBanhDaNauXong(character.Id);
                                List<short> ItemId;
                                switch (idBanh)
                                {
                                    case 753://banh chung
                                        ItemId = new List<short>() { 1173, 1084, 1085, 1086, 1074, 1075, 1076, 1077, 1078, 1079, 1080, 1081, 1082, 1083, 730, 731, 732 };
                                        break;
                                    default://banh tet
                                        ItemId = new List<short>() { 933, 934, 935, 964, 965, 441, 442, 443, 444, 445, 446, 447, 878, 1529, 1530, 1531, 1532, 1543, 1544, 1545, 1521, 1528  };
                                        break;
                                }
                                var itemAdd = ItemCache.GetItemDefault(ItemId[ServerUtils.RandomNumber(ItemId.Count)]);
                                var itemTemplate = ItemCache.ItemTemplate(itemAdd.Id);
                                if (itemTemplate.Type == 11 || itemTemplate.Type == 5 || (itemTemplate.Type == 27 && DataCache.ListPetID.Contains(itemAdd.Id)))
                                {
                                    ItemHandler.HandleExpireItem(itemAdd);
                                }
                                character.CharacterHandler.AddItemToBag(true, itemAdd);

                                character.CharacterHandler.AddItemToBag(true, ItemCache.GetItemDefault((short)idBanh));
                                Server.Gi().GameCache.XoaBanhDaNauXong(character.Id);
                                character.CharacterHandler.SendMessage(Service.SendBag(character));
                                character.CharacterHandler.SendMessage(Service.NpcChat(npcId, "Bánh đã ra lò, đây là phần thưởng của bạn"));
                                break;
                        }
                    }
                    break;
            }
        }
        #endregion
        #region Menu NOT COFIRM

        private static void MenuDauThan(Character character, int npcId, int menuId, int optionId)
        {
            var magicTree = MagicTreeManager.Get(character.Id);
            if(magicTree == null) return;
            lock (magicTree)
            {
                switch (menuId)
                {
                    //Thu hoạch // Dùng ngọc nâng cấp
                    case 0:
                    {
                        if (magicTree.IsUpdate)
                        {
                            var ngoc = magicTree.Diamond;
                            if (character.AllDiamond() < ngoc)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_DIAMOND));
                                return;
                            }
                            character.MineDiamond(ngoc);
                            
                            magicTree.IsUpdate = false;
                            magicTree.Level++;
                            switch (magicTree.Level)
                            {
                                case < 8:
                                    magicTree.NpcId++;
                                    break;
                                case >= 10:
                                    magicTree.Level = 10;
                                    break;
                            }

                            magicTree.MaxPea += 2; 
                            magicTree.Peas = magicTree.MaxPea;
                            magicTree.Seconds = 0;
                            magicTree.Diamond = 0;
                            // MagicTreeDB.Update(magicTree);

                            magicTree.MagicTreeHandler.HandleNgoc();
                            character.CharacterHandler.SendMessage(Service.MagicTree0(magicTree));
                            character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                            MagicTreeDB.Update(magicTree);
                        }
                        else
                        {
                            if(magicTree.Peas == 0) return;
                            var quantityPea = magicTree.Peas;
                            var emptyBag = 10 - character.GetTotalDauThanBag();
                            var emptyBox = 20 - character.GetTotalDauThanBox();
                            var totalEmpty = emptyBag + emptyBox;
                            if (emptyBag <= 0 && emptyBox <= 0)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().MAX_PEAS));
                                return;
                            }
                            if(quantityPea > 0 && emptyBag > 0) {
                                if(quantityPea < emptyBag) {
                                    emptyBag = quantityPea;
                                    quantityPea = 0;
                                } else {
                                    quantityPea -= emptyBag;
                                }
                                var item = ItemCache.GetItemDefault((short)DataCache.IdDauThan[magicTree.Level - 1], emptyBag);
                                if(character.CharacterHandler.AddItemToBag(true, item, "Thu hoạch đậu")) {
                                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                                }
                            }
                            if(quantityPea > 0 && emptyBox > 0) {
                                if(quantityPea < emptyBox) {
                                    emptyBox = quantityPea;
                                    quantityPea = 0;
                                } else {
                                    quantityPea -= emptyBox;
                                }
                                var item = ItemCache.GetItemDefault((short)DataCache.IdDauThan[magicTree.Level - 1], emptyBox);
                                if(character.CharacterHandler.AddItemToBox(true, item)) {
                                    character.CharacterHandler.SendMessage(Service.SendBox(character));
                                }
                            }

                            if (totalEmpty > 0)
                            {
                                character.CharacterHandler.SendMessage(Service.MagicTree0(magicTree));
                            }
                            magicTree.Peas = quantityPea;
                             
                            magicTree.Seconds = 60000 * magicTree.Level + ServerUtils.CurrentTimeMillis();
                            magicTree.IsUpdate = false;
                            magicTree.MagicTreeHandler.HandleNgoc();
                            character.CharacterHandler.SendMessage(Service.MagicTree2(quantityPea, magicTree.Level));
                        }
                        break;
                    }
                    //Nâng cấp
                    case 1:
                    {
                        if (magicTree.Level == 10)
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiSay(5, "Đậu thần đã đạt đến cấp độ tối đa", false, character.InfoChar.Gender));
                            return;
                        }
                        if (magicTree.IsUpdate)
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, MenuNpc.Gi().TextMeo[1], MenuNpc.Gi().MenuMeo[0], character.InfoChar.Gender));
                            character.TypeMenu = 2;
                        }
                        else
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, MenuNpc.Gi().TextMeo[0], MenuNpc.Gi().MenuMeo[0], character.InfoChar.Gender));
                            character.TypeMenu = 1;
                        }
                        break;
                    }
                    //Kết hạt nhanh
                    case 2:
                    {
                        if(magicTree.IsUpdate || magicTree.Peas == magicTree.MaxPea) return;
                        var ngoc = magicTree.Diamond;
                        if (character.AllDiamond() < ngoc)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_DIAMOND));
                            return;
                        }
                        character.MineDiamond(ngoc);
                        magicTree.Peas = magicTree.MaxPea;
                        magicTree.Seconds = 0;
                        magicTree.IsUpdate = false;
                        magicTree.MagicTreeHandler.HandleNgoc();
                        character.CharacterHandler.SendMessage(Service.MagicTree0(magicTree));
                        character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                        break;
                    }
                }
            }
            
        }
        

        #endregion
    
        #region Function
        public static void NauBanhChung()
        {
                
        }
        public static void NauBanhTet()
        {

        }
        public static void NhanBanhChung()
        {

        }
        public static void NhanBanhTet()
        {

        }
        public static void CreatePetNormal(Character character)
        {
            var detu = new Disciple();
            detu.CreateNewDisciple(character, ServerUtils.RandomNumber(0,2));
            detu.Player = character.Player;
            detu.CharacterHandler.SetUpInfo();
            
            character.Disciple = detu;
            character.InfoChar.IsHavePet = true;
            character.CharacterHandler.SendMessage(Service.Disciple(1, null));
            character.Zone.ZoneHandler.AddDisciple(detu);
            DiscipleDB.Create(detu);
        }
        private static void CreateDiscipleCumber(Character character, sbyte gender)
        {
            // Nếu có đệ thì đổi đệ
            var oldDisciple = character.Disciple;
            if (oldDisciple != null || DiscipleDB.IsAlreadyExist(-character.Id))
            {
                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, "Bạn có muốn đổi đệ tử hiện tại để đổi lấy đệ tử Cumber?", MenuNpc.Gi().MenuQuyLao[3], character.InfoChar.Gender));
                character.TypeMenu = 27;
                
            }
            // không có thì tạo mới
            else
            {
                var disciple = new Disciple();
                disciple.CreatePet(character, 3,gender);
                disciple.Player = character.Player;
                disciple.CharacterHandler.SetUpInfo();
                character.Disciple = disciple;
                character.InfoChar.IsHavePet = true;
                character.CharacterHandler.SendMessage(Service.Disciple(1, null));
                DiscipleDB.Create(disciple);
            }
        }
        private static void CreateDiscipleBilly(Character character, sbyte gender)
        {
            // Nếu có đệ thì đổi đệ
            var oldDisciple = character.Disciple;
            if (oldDisciple != null || DiscipleDB.IsAlreadyExist(-character.Id))
            {
                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, "Bạn có muốn đổi đệ tử hiện tại để đổi lấy đệ tử Billy?", MenuNpc.Gi().MenuQuyLao[3], character.InfoChar.Gender));
                character.TypeMenu = 29;

            }
            // không có thì tạo mới
            else
            {
                var disciple = new Disciple();
                disciple.CreatePet(character, 4, gender);
                disciple.Player = character.Player;
                disciple.CharacterHandler.SetUpInfo();
                character.Disciple = disciple;
                character.InfoChar.IsHavePet = true;
                character.CharacterHandler.SendMessage(Service.Disciple(1, null));
                DiscipleDB.Create(disciple);
            }
        }
        private static void CreateDiscipleMabu(Character character, sbyte gender)
        {
            // Nếu có đệ thì đổi đệ
            var oldDisciple = character.Disciple;
            if (oldDisciple != null || DiscipleDB.IsAlreadyExist(-character.Id))
            {
                oldDisciple = new Disciple();
                oldDisciple.CreateNewMaBuDisciple(character, gender);
                oldDisciple.Player = character.Player;
                oldDisciple.CharacterHandler.SetUpInfo();
                character.Disciple = oldDisciple;
                DiscipleDB.Update(oldDisciple);
            }
            // không có thì tạo mới
            else
            {
                var disciple = new Disciple();
                disciple.CreateNewMaBuDisciple(character, gender);
                disciple.Player = character.Player;
                disciple.CharacterHandler.SetUpInfo();
                character.Disciple = disciple;
                character.InfoChar.IsHavePet = true;
                character.CharacterHandler.SendMessage(Service.Disciple(1, null));
                DiscipleDB.Create(disciple);
            }
            
            // var oldDisciple = character.Disciple;
            // if (oldDisciple != null)
            // {
            //     DiscipleDB.Delete(oldDisciple.Id);
            //     character.CharacterHandler.SendMessage(Service.Disciple(0, null)); 
            //     character.InfoChar.IsHavePet = false;
            //     character.Disciple = null;
            // }
            character.CharacterHandler.SendMessage(Service.NoTrungMaBu());
            character.InfoChar.ThoiGianTrungMaBu = 0;

            // Thread.Sleep(3000);
            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().GET_NEW_MABU_DISCIPLE));
            
        }
        #endregion
    }
}