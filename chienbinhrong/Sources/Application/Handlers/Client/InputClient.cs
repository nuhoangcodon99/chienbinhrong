﻿using System;
using System.Collections.Generic;
using NgocRongGold.Application.Menu;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Threading;
using NgocRongGold.Application.Manager;
using NgocRongGold.DatabaseManager;
using NgocRongGold.DatabaseManager.Player;
using NgocRongGold.Model.Template;
using NgocRongGold.Model.Option;
using NgocRongGold.Model.Character;
using System.Runtime.CompilerServices;
using System.Linq;
using NgocRongGold.Application.Extension;
using Sources.Database;
using NgocRongGold.Application.Extension.ConSoMayMan;
using Sources;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Handlers.Client
{
    public static class InputClient
    {
       
        public static readonly List<List<int>> Lists = new List<List<int>>
            {
                //bo 1s, thoi vang, other
                new List<int>{10, 2500},
                new List<int>{7, 2000},
                new List<int>{5, 1000},
                new List<int>{3, 800},
                new List<int>{2,  500},
                new List<int>{2,   500},//6-10
                new List<int>{1,   400},//11-30
            };
        public static void TopGift(Model.Character.Character character,int top)
        {
           
            switch (top)
            {
                case 1:
                    {
                        for (int bo1s = 0; bo1s <= 6; bo1s++)
                        {
                            var ngocrong = ItemCache.GetItemDefault((short)(14 + bo1s));
                            var index = character.ItemGift.Count;
                            ngocrong.IndexUI = index;
                            ngocrong.Quantity = Lists[0][0];
                            character.ItemGift.Add(ngocrong);
                        }
                        var thoivang = ItemCache.GetItemDefault(457);
                        thoivang.Quantity = Lists[0][1];
                        thoivang.IndexUI = character.ItemGift.Count;
                        character.ItemGift.Add(thoivang);
                        var cumber = ItemCache.GetItemDefault(1279);
                        cumber.Options.ForEach(opt =>
                        {
                            opt.Param = 60;
                        });
                        cumber.IndexUI = character.ItemGift.Count;
                        character.ItemGift.Add(cumber);
                        var skh = ItemCache.GetItemDefault(1190);
                        skh.IndexUI = character.ItemGift.Count;
                        character.ItemGift.Add(skh);
                        break;
                    }
                case 2:
                    {
                        for (int bo1s = 0; bo1s <= 6; bo1s++)
                        {
                            var ngocrong = ItemCache.GetItemDefault((short)(14 + bo1s));
                            var index = character.ItemGift.Count;
                            ngocrong.IndexUI = index;
                            ngocrong.Quantity = Lists[1][0];
                            character.ItemGift.Add(ngocrong);
                        }
                        var thoivang = ItemCache.GetItemDefault(457);
                        thoivang.Quantity = Lists[1][1];
                        thoivang.IndexUI = character.ItemGift.Count;
                        character.ItemGift.Add(thoivang);
                        var cumber = ItemCache.GetItemDefault(1279);
                        cumber.Options.ForEach(opt =>
                        {
                            opt.Param = 55;
                        });
                        cumber.IndexUI = character.ItemGift.Count;
                        character.ItemGift.Add(cumber);
                        var skh = ItemCache.GetItemDefault(1190);
                        skh.IndexUI = character.ItemGift.Count;
                        character.ItemGift.Add(skh);
                        
                        break;
                    }
                case 3:
                    {
                        for (int bo1s = 0; bo1s <= 6; bo1s++)
                        {
                            var ngocrong = ItemCache.GetItemDefault((short)(14 + bo1s));
                            var index = character.ItemGift.Count;
                            ngocrong.IndexUI = index;
                            ngocrong.Quantity = Lists[2][0];
                            character.ItemGift.Add(ngocrong);
                        }
                        var thoivang = ItemCache.GetItemDefault(457);
                        thoivang.Quantity = Lists[2][1];
                        thoivang.IndexUI = character.ItemGift.Count;
                        character.ItemGift.Add(thoivang);
                        var cumber = ItemCache.GetItemDefault(1279);
                        cumber.Options.ForEach(opt =>
                        {
                            opt.Param = 50;
                        });
                        cumber.IndexUI = character.ItemGift.Count;
                        character.ItemGift.Add(cumber);
                        var thanlinh = ItemCache.GetItemDefault(1269);
                        thanlinh.IndexUI = character.ItemGift.Count;
                        character.ItemGift.Add(thanlinh);
                        break;
                    }
                case 4:
                    {
                        for (int bo1s = 0; bo1s <= 6; bo1s++)
                        {
                            var ngocrong = ItemCache.GetItemDefault((short)(14 + bo1s));
                            var index = character.ItemGift.Count;
                            ngocrong.IndexUI = index;
                            ngocrong.Quantity = Lists[3][0];
                            character.ItemGift.Add(ngocrong);
                        }
                        var thoivang = ItemCache.GetItemDefault(457);
                        thoivang.Quantity = Lists[3][1];
                        thoivang.IndexUI = character.ItemGift.Count;
                        character.ItemGift.Add(thoivang);
                        var cumber = ItemCache.GetItemDefault(1279);
                        cumber.Options.ForEach(opt =>
                        {
                            opt.Param = 45;
                        });
                        cumber.IndexUI = character.ItemGift.Count;
                        character.ItemGift.Add(cumber);
                        var thanlinh = ItemCache.GetItemDefault(1269);
                        thanlinh.IndexUI = character.ItemGift.Count;
                        character.ItemGift.Add(thanlinh);
                        break;
                    }
                case 5:
                    {
                        for (int bo1s = 0; bo1s <= 6; bo1s++)
                        {
                            var ngocrong = ItemCache.GetItemDefault((short)(14 + bo1s));
                            var index = character.ItemGift.Count;
                            ngocrong.IndexUI = index;
                            ngocrong.Quantity = Lists[4][0];
                            character.ItemGift.Add(ngocrong);
                        }
                        var thoivang = ItemCache.GetItemDefault(457);
                        thoivang.Quantity = Lists[4][1];
                        thoivang.IndexUI = character.ItemGift.Count;
                        character.ItemGift.Add(thoivang);
                        var cumber = ItemCache.GetItemDefault(1279);
                        cumber.Options.ForEach(opt =>
                        {
                            opt.Param = 45;
                        });
                        cumber.IndexUI = character.ItemGift.Count;
                        character.ItemGift.Add(cumber);
                        var thanlinh = ItemCache.GetItemDefault(1269);
                        thanlinh.IndexUI = character.ItemGift.Count;
                        character.ItemGift.Add(thanlinh);
                        break;
                    }
                case >= 6 and <= 10:
                    {
                        for (int bo1s = 0; bo1s <= 6; bo1s++)
                        {
                            var ngocrong = ItemCache.GetItemDefault((short)(14 + bo1s));
                            var index = character.ItemGift.Count;
                            ngocrong.IndexUI = index;
                            ngocrong.Quantity = Lists[5][0];
                            character.ItemGift.Add(ngocrong);
                        }
                        var thoivang = ItemCache.GetItemDefault(457);
                        thoivang.Quantity = Lists[5][1];
                        thoivang.IndexUI = character.ItemGift.Count;
                        character.ItemGift.Add(thoivang);
                        var cumber = ItemCache.GetItemDefault(1279);
                        cumber.Options.ForEach(opt =>
                        {
                            opt.Param = 45;
                        });
                        cumber.IndexUI = character.ItemGift.Count;
                        character.ItemGift.Add(cumber);
                        break;
                    }
                case >= 11 and <= 30:
                    {
                        for (int bo1s = 0; bo1s <= 6; bo1s++)
                        {
                            var ngocrong = ItemCache.GetItemDefault((short)(14 + bo1s));
                            var index = character.ItemGift.Count;
                            ngocrong.IndexUI = index;
                            ngocrong.Quantity = Lists[5][0];
                            character.ItemGift.Add(ngocrong);
                        }
                        var thoivang = ItemCache.GetItemDefault(457);
                        thoivang.Quantity = Lists[6][1];
                        thoivang.IndexUI = character.ItemGift.Count;
                        character.ItemGift.Add(thoivang);
                        var cumber = ItemCache.GetItemDefault(611);
                        cumber.IndexUI = character.ItemGift.Count;
                        character.ItemGift.Add(cumber);
                        break;
                    }
            }
        }
        public static void HanleInputClient(Model.Character.Character character, Message message)
        {
            if(message == null) return;
            try
            {
                var lengthInput = message.Reader.ReadByte();
                var listInput = new List<string>();
                for (var i = 0; i < lengthInput; i++)
                {
                    listInput.Add(message.Reader.ReadUTF());
                }
                if(listInput.Count <= 0) return;
                switch (character.TypeInput)
                {
                    case 116:
                        {
                            var number = listInput[0];
                            if (!Int32.TryParse(number, out _))
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().INPUT_CORRECT_NUMBER));
                                return;
                            }
                            var number2 = Int32.Parse(number);
                            if (number2 < 1 || number2 >= 100)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().INPUT_CORRECT_NUMBER));
                                return;
                            }
                            ConSoMayManHandler.gI().BuyConSoMayMan(character, number2);
                            break;
                        }
                    case 115:
                        var soluongg = listInput[0];
                        if (!Int32.TryParse(soluongg, out _))
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().INPUT_CORRECT_NUMBER));
                            return;
                        }
                        var soluongsd = Int32.Parse(soluongg);
                        if (soluongsd <= 0 || soluongsd > character.CharacterHandler.GetItemBagById(1036).Quantity)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().INPUT_CORRECT_NUMBER));
                            return;
                        }
                        character.DiemSuKien += soluongsd * 10;
                        character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + soluongsd * 10 + " điểm Đua Top Pháo Hoa"));
                        ClientManager.Gi().SendMessage(EffectCharacter.addEff(62, character.InfoChar.X, (short)(character.InfoChar.Y - 20), 0, 10, 30));
                        character.CharacterHandler.RemoveItemBagById(1036, soluongsd);
                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                        break;
                    case 114:
                        {
                            var charrr = ClientManager.Gi().GetPlayerByUserName(listInput[0]);
                            var top = int.Parse(listInput[1]);
                            if (charrr != null)
                            {
                                TopGift((Model.Character.Character)charrr.Character, top);
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Thưởng Top Thành Công"));
                            }
                            else
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Không tìm thấy người chơi"));
                            }
                            break;
                        }
                    case 0://Nạp thẻ
                        {
                            var soSeriText = listInput[0];
                            var maPinText = listInput[1];

                            Console.WriteLine("Loai the " + character.NapTheTemp.LoaiThe + " menh gia " + character.NapTheTemp.MenhGia + " So Seri " + soSeriText + " ma pin " + maPinText);
                            RechargeCard.Exchange(character, soSeriText, maPinText, (int)character.NapTheTemp.MenhGia);
                            //GachThe.SendCard(character, character.NapTheTemp.LoaiThe, character.NapTheTemp.MenhGia, soSeriText, maPinText);
                            break;
                        }
                    case 1://Gift code 
                        {
                            var codeInput = listInput[0];
                            GiftcodeDataBase.RewardGiftcode(character, codeInput);
                            break;
                        }
                    case 2://đổi mật khẩu
                        {
                            var timeServer = ServerUtils.CurrentTimeMillis();
                            var oldPass = listInput[0];
                            var newPass = listInput[1];
                            // var sdt = listInput[2];
                            var checkData = UserDB.CheckBeforeChangePass(character.Player.Id, oldPass);
                            if (!checkData)
                            {
                                character.CharacterHandler.SendMessage(Service.OpenUiSay((short)character.ShopId, "Thông tin tài khoản không chính xác, vui lòng nhập lại."));
                                return;
                            }
                            UserDB.DoiMatKhau(character.Player.Id, newPass);
                            character.CharacterHandler.SendMessage(Service.OpenUiSay((short)character.ShopId, "Đổi mật khẩu thành công, vui lòng thoát game và đăng nhập lại"));
                            break;
                        }
                    case 3: //khoa tai khoan
                        {
                            var banReason = listInput[0];
                            MenuAdminRecode.gI().HandlerBanAccount(banReason);
                            break;
                        }
                    case 4:
                        var id = Int32.Parse(listInput[0]);
                        var quantity = int.Parse(listInput[1]);
                        character.CharacterHandler.SendMessage(Service.ServerMessage("Buff thành công"));
                        MenuAdminRecode.gI().BuffItem(id, quantity);
                        break;
                    case 5:
                        MenuAdminRecode.gI().CallBoss(int.Parse(listInput[0]));
                        break;
                    case 112:
                        {
                            var username = listInput[0];
                            var thoivan = int.Parse(listInput[1]);
                            if (UserDB.PlusThoiVang(username, thoivan))
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Đã cộng thành công cho tài khoản " + username + " " + thoivan + " thỏi vàng"));
                            }
                            else
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Không tìm thấy tài khoản"));

                            }
                            break;
                        }
                    case 113:
                        {
                            var username = listInput[0];
                            var mone = int.Parse(listInput[1]);
                            if (UserDB.PlusVND(username, mone))
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Đã cộng thành công cho tài khoản " + username + " " + mone));
                            }
                            else
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Không tìm thấy tài khoản"));

                            }
                            break;
                        }
                    case 7:
                        MenuAdminRecode.gI().BuffPotenial(int.Parse(listInput[0]), long.Parse(listInput[1]));
                        break;
                    case 8:

                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(64, "Would You Want? SET OR PLUS ?", MenuNpc.Gi().MenuAdmin[3], character.InfoChar.Gender));
                        character.TypeMenu = 3;
                        break;
                    case 9:
                        var idTask = Int32.Parse(listInput[0]);
                        var index = Int32.Parse(listInput[1]);
                        var count = Int32.Parse(listInput[2]);
                        MenuAdminRecode.gI().BuffTask(idTask, index, count);
                        break;
                    case 10:
                        var getCharSelect = ClientManager.Gi().GetPlayerByUserName(MenuAdminRecode.gI().NameCharSelect).Character;

                        break;
                    case 11:
                        //ingored teleport to nrsd
                        break;
                    case 12:
                        var code = listInput[0];
                        MenuAdminRecode.gI().CheckGiftcode(character, code);

                        break;
                    case 13:
                        var money = Int32.Parse(listInput[0]);
                        MenuAdminRecode.gI().BuffMoney(money);
                        break;
                    case 14:
                        // ingored input level dungeon
                        var levele = Int32.Parse(listInput[0]);
                        int l2;
                        var isNumberr = Int32.TryParse(listInput[0], out l2);

                        if (levele > 110 || !isNumberr || levele <= 0)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerChat("Level không hợp lệ, vui lòng chọn lại !"));
                            return;
                        }
                        var clan = ClanManager.Get(character.ClanId);
                        if (character.InfoChar.MapId == 48)
                        {
                            if (clan.ClanDungeon.ConDuongRanDoc.Count <= 0 && clan.ClanDungeon.ConDuongRanDoc.CheckClose())
                            {
                                character.CharacterHandler.SendMessage(Service.ServerChat("Bạn đã hết số lần tham gia trong ngày, vui lòng quay lại vào ngày mai !"));
                                return;
                            }
                            clan.ClanDungeon.ConDuongRanDoc.Level = levele;
                            clan.ClanDungeon.ConDuongRanDoc.Open(clan);
                            MapManager.Get(143).GetZoneById(clan.Id).ZoneHandler.JoinZone(character, false, false, 0);

                        }
                        else
                        {
                            if (clan.ClanDungeon.BanDoKhoBau.Count <= 0 && clan.ClanDungeon.BanDoKhoBau.CheckClose())
                            {
                                character.CharacterHandler.SendMessage(Service.ServerChat("Bạn đã hết số lần tham gia trong ngày, vui lòng quay lại vào ngày mai !"));
                                return;
                            }
                            clan.ClanDungeon.BanDoKhoBau.Level = levele;
                            clan.ClanDungeon.BanDoKhoBau.Open(clan);
                            character.InfoChar.X = 78;
                            character.InfoChar.Y = 336;
                            MapManager.Get(135).GetZoneById(clan.Id).ZoneHandler.JoinZone(character, false, false, 0);
                            character.CharacterHandler.RemoveItemBagById(611, 1);
                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                        } 
                        break;
                    case 15:
                        var ok = listInput[0];
                        if (ok == "ok" || ok == "OK")
                        {
                            var disciple = character.Disciple;
                            if (disciple == null)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().DONT_FIND_DISCIPLE));
                                return;
                            }

                            var itemDiscipleBody = disciple.ItemBody.FirstOrDefault(item => item != null);

                            if (itemDiscipleBody != null)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().PLEASE_EMPTY_DISCIPLE_BODY));
                                return;
                            }

                            var oldStatus = disciple.Status;

                            if (oldStatus < 3)
                            {
                                character.Zone.ZoneHandler.RemoveDisciple(character.Disciple);
                            }

                            disciple = new Disciple();
                            switch (character.Disciple.Type)
                            {
                                case 2:
                                    disciple.CreateNewMaBuDisciple(character, character.InfoChar.Gender);
                                    break;
                                case 3:
                                    disciple.CreateNewCumberDisciple(character, character.InfoChar.Gender);
                                    break;
                                default:
                                    if (character.Disciple.InfoChar.Gender == 0)
                                    {
                                        disciple.CreateNewDisciple(character, 1);
                                    }
                                    else if (character.Disciple.InfoChar.Gender == 1)
                                    {
                                        disciple.CreateNewDisciple(character, 2);
                                    }
                                    else
                                    {
                                        disciple.CreateNewDisciple(character, 0);
                                    }
                                    break;
                            }
                            disciple.Player = character.Player;
                            disciple.Zone = character.Zone;
                            disciple.CharacterHandler.SetUpInfo();
                            character.Disciple = disciple;

                            if (!character.InfoChar.Fusion.IsFusion && oldStatus < 3)
                            {
                                character.Zone.ZoneHandler.AddDisciple(disciple);
                            }
                            else
                            {
                                character.CharacterHandler.SetUpInfo();
                                character.CharacterHandler.SendMessage(Service.MeLoadPoint(character));
                                character.CharacterHandler.SendMessage(Service.SendHp((int)character.InfoChar.Hp));
                                character.CharacterHandler.SendMessage(Service.SendMp((int)character.InfoChar.Mp));
                                character.CharacterHandler.SendZoneMessage(Service.PlayerLevel(character));
                            }
                            character.CharacterHandler.RemoveItemBagById(401, 1, reason: "Dùng đổi đệ tử");
                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                            DiscipleDB.Update(disciple);
                        }
                        break;
                    case 210: // thỏi vàng to hũ vàng
                        {
                            int n;
                            bool isNumeric = int.TryParse(listInput[0], out n);
                            if (!isNumeric)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().INPUT_CORRECT_NUMBER));
                                break;
                            }
                            var inputValue = Int32.Parse(listInput[0]);

                            if (inputValue < 0)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().INPUT_CORRECT_NUMBER));
                                break;
                            }
                            var thoivang = character.CharacterHandler.GetAllQuantityItemBagById(457);
                            if (inputValue > thoivang / 1200 || thoivang < 1200)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().INPUT_CORRECT_NUMBER));
                                break;
                            }
                            character.CharacterHandler.RemoveItemBagById(457, inputValue * 1200, "Quy đổi hũ vàng từ thỏi vàng");
                            character.CharacterHandler.AddItemToBag(true, ItemCache.GetItemDefault(1549, inputValue));
                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                            character.CharacterHandler.SendMessage(Service.ServerMessage($"Quy đổi {inputValue} hũ vàng thành công"));
                        }
                        break;
                    case 211:// hũ vàng to thỏi vàng
                        {
                            int n;
                            bool isNumeric = int.TryParse(listInput[0], out n);
                            if (!isNumeric)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().INPUT_CORRECT_NUMBER));
                                return;
                            }
                            var inputValue = Int32.Parse(listInput[0]);

                            if (inputValue < 0)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().INPUT_CORRECT_NUMBER));
                                return;
                            }
                            var huvang = character.CharacterHandler.GetAllQuantityItemBagById(1549);
                            if (huvang == 0)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().INPUT_CORRECT_NUMBER));
                                break;
                            }
                            if (inputValue > huvang)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().INPUT_CORRECT_NUMBER));
                                break;
                            }
                            character.CharacterHandler.RemoveItemBagById(1549, inputValue, "Quy đổi hũ vàng sang thỏi vàng");
                            character.CharacterHandler.AddItemToBag(true, ItemCache.GetItemDefault(457, inputValue *900));
                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                            character.CharacterHandler.SendMessage(Service.ServerMessage($"Quy đổi {inputValue * 900} thỏi vàng thành công"));

                        }
                        break;
                    case 212:// hũ vàng to hồng ngọc
                        {
                            int n;
                            bool isNumeric = int.TryParse(listInput[0], out n);
                            if (!isNumeric)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().INPUT_CORRECT_NUMBER));
                                return;
                            }
                            var inputValue = Int32.Parse(listInput[0]);

                            if (inputValue < 0)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().INPUT_CORRECT_NUMBER));
                                return;
                            }
                            var huvang = character.CharacterHandler.GetAllQuantityItemBagById(1549);
                            if (huvang == 0)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().INPUT_CORRECT_NUMBER));
                                break;
                            }
                            if (inputValue > huvang)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().INPUT_CORRECT_NUMBER));
                                break;
                            }

                            character.CharacterHandler.RemoveItemBagById(1549, inputValue, "Quy đổi hũ vàng sang hồng ngọc");
                            character.PlusDiamondLock(inputValue * 3);
                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                            character.CharacterHandler.SendMessage(Service.BuyItem(character));

                            character.CharacterHandler.SendMessage(Service.ServerMessage($"Quy đổi {inputValue} hồng ngọc thành công"));
                        }
                        break;


                    case 19:
                        {
                            string Name = listInput[0];
                            character.Name = Name;
                            character.CharacterHandler.SendMessage(Service.MeLoadAll(character));
                            break;
                        }
                    case 20:
                        {
                            string Name = listInput[0];
                            character.Disciple.Name = Name;
                            async void Action()
                            {
                                character.CharacterHandler.SendZoneMessage(Service.PublicChat(character.Disciple.Id, "Cảm ơn sư phụ đã đặt tên cho con là " + Name));
                                await Task.Delay(1000);
                                character.Zone.ZoneHandler.RemoveDisciple(character.Disciple);
                                character.Zone.ZoneHandler.AddDisciple(character.Disciple);
                            }
                            var task = new Task(Action);
                            task.Start();
                            break;
                        }
                    case 21:
                        var passNow = listInput[0];
                        var passChange = listInput[1];
                        var confirmPassChange = listInput[2];
                        if (UserDB.GetPassword(character.Player).Contains(passNow))
                        {
                            // dung pass thuc hien change
                            if (confirmPassChange == passChange)
                            {

                                if (UserDB.ChangePassword(character.Player, passChange))
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiSay(5, "Bạn đã đổi mật khẩu thành: " + passChange));
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiSay(5, "Vui lòng liên hệ Admin để đổi mật khẩu!"));

                                }
                            }
                            else
                            {
                                // ko confirm pass doi dung
                                character.CharacterHandler.SendMessage(Service.OpenUiSay(5, "Nhập lại mật khẩu cần thay đổi phải đúng !"));
                            }
                        }
                        else
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiSay(5, "Sai mật khẩu hiện tại!"));
                            // sai mk hien tai
                        }
                        break;
                    case 22:
                        var clan2 = ClanManager.Get(character.ClanId);
                        int m;
                        bool isNumer = int.TryParse(listInput[0], out m);
                        if (!isNumer)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().INPUT_CORRECT_NUMBER));
                            return;
                        }
                        if (clan2 == null)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn không có bang hội?"));
                            return;
                        }
                        var level = Int32.Parse(listInput[0]);
                        if (level <= 0 || level > 110)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Chỉ có thể nhập từ cấp 0 -> cấp 110"));
                            return;
                        }
                        clan2.ClanDungeon.KhiGasHuyDiet.Level = level;
                        clan2.ClanDungeon.KhiGasHuyDiet.Open(clan2);

                        var mapOld = MapManager.Get(character.InfoChar.MapId);
                        mapOld.OutZone(character);
                        character.InfoChar.X = 121;
                        character.InfoChar.Y = 336;
                        clan2.ClanDungeon.KhiGasHuyDiet.JoinMap(character, 149);


                        break;
                    case 23:
                        var pl = listInput[0];
                        var allPlayerInSever = "\nALL SESSION : " + ServerUtils.GetMoneys(ClientManager.Gi().Sessions.Count) + " | ALL PLAYER: " + ServerUtils.GetMoneys(ClientManager.Gi().Characters.Count);
                        if (ClientManager.Gi().GetCharacter(pl) == null)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("NOT FOUND PLAYER !"));
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(41, "Xin Chào " + character.Name + ", " + allPlayerInSever + "\n|0|Sever Status: [ON]", new List<string> { "ME", "FIND PLAYER:\n" + MenuAdminRecode.gI().NameCharSelect }, character.InfoChar.Gender));
                            character.TypeMenu = 0;
                            return;
                        }
                        MenuAdminRecode.gI().NameCharSelect = pl;
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(41, "Xin Chào "+character.Name+", " + allPlayerInSever + "\n|0|Sever Status: [ON]", MenuNpc.Gi().MenuAdmin[0], character.InfoChar.Gender));
                        character.TypeMenu = 0;
                        break;
                    case 24: // set point
                        var hp = Int32.Parse(listInput[0]);
                        var mp = Int32.Parse(listInput[1]);
                        var sd = Int32.Parse(listInput[2]);
                        var crit = Int32.Parse(listInput[3]);
                        var amor = Int32.Parse(listInput[4]);
                        MenuAdminRecode.gI().HandlerPlusOrignalPoint(0, hp, mp, sd, amor, crit);
                        break;
                    case 25: // plus point
                        var hp2 = Int32.Parse(listInput[0]);
                        var mp2 = Int32.Parse(listInput[1]);
                        var sd2 = Int32.Parse(listInput[2]);
                        var crit2 = Int32.Parse(listInput[3]);
                        var amor2 = Int32.Parse(listInput[4]);
                        MenuAdminRecode.gI().HandlerPlusOrignalPoint(1, hp2, mp2, sd2, amor2, crit2);
                        break;
                    case 31:
                        if (listInput[0].Length > 10)
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiSay(5, "Tối đa 10 kí tự"));
                            return;
                        }
                        ClanManager.Get(character.ClanId).shortName = listInput[0];
                        break;
                    case 26:
                        {
                            int n;
                            bool isNumeric = int.TryParse(listInput[0], out n);
                            if (!isNumeric)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().INPUT_CORRECT_NUMBER));
                                return;
                            }
                            var inputValue = Int32.Parse(listInput[0]);

                            if (inputValue < 0)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().INPUT_CORRECT_NUMBER));
                                return;
                            }
                            // Kiểm tra có đủ VNĐ không
                            if (character.CharacterHandler.GetItemBagById(457).Quantity < inputValue || character.CharacterHandler.GetItemBagById(457).Quantity < 0 || character.CharacterHandler.GetItemBagById(457) == null)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Không đủ thỏi vàng !"));
                                return;
                            }
                            long GoldGet = (long)((long)inputValue * 500000000);
                            if (GoldGet + character.InfoChar.Gold >= DataCache.LITMIT_GOLD)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Con số bạn nhập đã vượt quá giới hạn vàng của bạn"));
                                return;
                            }
                            character.CharacterHandler.RemoveItemBagById(457, inputValue);
                            character.InfoChar.Gold += GoldGet;
                            character.CharacterHandler.SendMessage(Service.BuyItem(character));
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + ServerUtils.GetMoneys(GoldGet) + " vàng"));
                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                            break;
                        }
                    case 40:
                        {
                            var @char = ClientManager.Gi().GetPlayerByUserName(listInput[0]);
                            var item = ItemCache.GetItemDefault((short)(int.Parse(listInput[1])), int.Parse(listInput[2]));
                            @char.Character.CharacterHandler.AddItemToBag(true, item);
                            @char.Character.CharacterHandler.SendMessage(Service.SendBag(character));
                            @char.Character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được x" + listInput[2] + " " + ItemCache.ItemTemplate(item.Id).Name + " từ Admin"));
                        }
                        break;
                    case 1999: //đổi vnd sang vàng
                    {
                        // kiểm tra có phải là số không
                        int n;
                        bool isNumeric = int.TryParse(listInput[0], out n);
                        if (!isNumeric) 
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().INPUT_CORRECT_NUMBER));
                            return;
                        }
                        var inputValue = Int32.Parse(listInput[0]);

                        if (inputValue < 0)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().INPUT_CORRECT_NUMBER));
                            return;
                        }
                        // Kiểm tra có đủ VNĐ không
                        int vnd = UserDB.GetVND(character.Player);
                        if (vnd < inputValue)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_VND));
                            return;
                        }
                        // Kiểm tra giới hạn vàng trên người
                        long quyDoi = inputValue*550;
                        if (character.InfoChar.Gold + quyDoi > character.InfoChar.LimitGold)
                        {
                            var quyDoiToiDa = (character.InfoChar.LimitGold - character.InfoChar.Gold)/550;
                            character.CharacterHandler.SendMessage(Service.ServerMessage(string.Format(TextServer.gI().VND_TO_GOLD_LIMIT, ServerUtils.GetMoneys(quyDoiToiDa))));
                            return;
                        }
                        // Oke hết thì trừ VNĐ và cộng vàng
                        if (UserDB.MineVND(character.Player, inputValue))
                        {
                            character.PlusGold(quyDoi);
                            character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));

                            if (inputValue >= 20000 && !character.InfoChar.IsPremium)
                            {
                                character.InfoChar.IsPremium = true;
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().UPGRADE_TO_PREMIUM));
                            }
                        }
                        character.TypeInput = 0;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error HanleInputClient in Service.cs: {e.Message} \n {e.StackTrace}", e);
            }
            finally
            {
                message?.CleanUp();
            }
        }
        
        public static void HandleNapThe(Model.Character.Character character, Message message)
        {
            var gender = character.InfoChar.Gender;
            character.CharacterHandler.SendMessage(Service.OpenUiSay(5, string.Format("Hãy đến gặp {0} để nạp thẻ bạn nhé.", TextTask.NameNpc[gender]), false, gender));
        }
    }
}