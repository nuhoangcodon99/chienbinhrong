    using System;
using System.Linq;
using System.Collections.Generic;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.IO;
using NgocRongGold.Model.Item;
using NgocRongGold.Model.Option;
using Org.BouncyCastle.Math.Field;
using System.Net.WebSockets;
using NgocRongGold.Application.MainTasks;
using NgocRongGold.DatabaseManager;
using NgocRongGold.Model.Info.Radar;
using NgocRongGold.Application.Threading;
using NgocRongGold.Application.Extension.DaiHoiVoThuat;

namespace NgocRongGold.Application.Handlers.Item
{
    public static class LeaveItemHandler
    {
        public static ItemMap LeaveGold(int charId, int quantity)
        {
            if (ServerUtils.RandomNumber(100) > 25) return null;
            var item = ItemCache.GetItemDefault(76);
            switch (quantity)
            {
                case >= 350 and < 5500:
                    item = ItemCache.GetItemDefault(188);
                    break;
                case >= 5500 and < 15000:
                    item = ItemCache.GetItemDefault(189);
                    break;
                case > 15000:
                    item = ItemCache.GetItemDefault(190);
                    break;
            }

            item.Quantity = quantity;
            return new ItemMap(charId, item);
        }

        public static ItemMap LeaveGoldPlayer(int charId, int quantity)
        {
            if (quantity == 0) return null;
            var item = ItemCache.GetItemDefault(76);
            item.Quantity = quantity;
            return new ItemMap(charId, item);
        }
        public static ItemMap LeaveManhBongTai(int charId)
        {
            var item = ItemCache.GetItemDefault(DataCache.ItemThanhDia[ServerUtils.RandomNumber(DataCache.ItemThanhDia.Count)]);
            item.Quantity = 1;
            return new ItemMap(charId, item);
        }
        public static ItemMap LeaveSuKienHe(int charId)
        {
            List<int> ListItem = new List<int>() { 695, 696, 697, 698 };
            var item = ItemCache.GetItemDefault((short)(ListItem[ServerUtils.RandomNumber(ListItem.Count)]));
            return new ItemMap(charId, item);
        }
        public static ItemMap LeaveSuKienTet(int charId)
        {
            List<int> ListItem = new List<int>() { 1177, 1178, 1179, 1180, 1181, 748, 749, 750, 751 };
            var item = ItemCache.GetItemDefault((short)(ListItem[ServerUtils.RandomNumber(ListItem.Count)]));
            return new ItemMap(charId, item);
        }
        public static ItemMap LeaveSuKien8Thang3(int charId)
        {
            List<int> ListItem = new List<int>() {1553, 1554, 1555, 1556, 1557  };
            var item = ItemCache.GetItemDefault((short)(ListItem[ServerUtils.RandomNumber(ListItem.Count)]));
            return new ItemMap(charId, item);
        }
        public static ItemMap Hirudegarn(int charId)
        {
            List<int> ListItem = new List<int>() { 15, 16, 17, 17, 17, 17, 17, 380, 381, 382, 383, 384, 384, 1074, 1074, 1075, 1075, 1076, 1077, 1078, 1079, 1080, 1081, 1082, 1083}; //568
            var item = ItemCache.GetItemDefault((short)(ListItem[ServerUtils.RandomNumber(ListItem.Count)]));
            return new ItemMap(charId, item);
        }
        public static ItemMap LeaveRuaCon(int charId)
        {
            var item = ItemCache.GetItemDefault(874);
            return new ItemMap(charId, item);
        }
        public static ItemMap LeaveBuaNguHanhSon(int charId)
        {
                var item = ItemCache.GetItemDefault((short)DataCache.ListBuaNguHanhSon[ServerUtils.RandomNumber(DataCache.ListBuaNguHanhSon.Count)]);
                item.Quantity = 1;
                return new ItemMap(charId, item);
            
        }
        public static ItemMap LeaveManhQuai(int charId, int monsterId)
        {
            short itemdrop = 76;
            switch (monsterId)
            {
                case >= 1 and <= 9:
                    itemdrop = (short)(827 + monsterId);
                    break;
            }
            if (itemdrop == 76) return LeaveGold(charId, ServerUtils.RandomNumber(2000, 3000));
            var item = ItemCache.GetItemDefault((short)itemdrop);
            item.Quantity = 1;
            return new ItemMap(charId, item);
        }
        public static ItemMap LeaveMonsterItemRecode(ICharacter character, int leaveItemType, int goldPlusPercent = 0, int mapId = 0, short monsterId = 0)
        {
            var charId = Math.Abs(character.Id);
            var item = ItemCache.GetItemDefault(1);
            var percentNotDrop = ServerUtils.RandomNumber(50);
            var percentSuccess = ServerUtils.RandomNumber(100);
            if (percentNotDrop < 26) return null;
            switch (leaveItemType) 
            {
                case 0:
                    if (percentSuccess > 30)
                    {
                        return LeaveGold(character, ServerUtils.RandomNumber(2000, 3000), goldPlusPercent);
                    }
                    return null;
                case 1:
                    if (character.Id < 0) break;
                    if (TaskHandler.CheckTask((Model.Character.Character)character,2, 0))
                    {
                       
                            item = ItemCache.GetItemDefault(74);
                            item.Quantity = 1;
                            return new ItemMap(charId, item);
                       
                        //else
                        //{
                        //    if (percentSuccess < 30)
                        //    {
                        //        var ListDragonBall = new List<int> { 19, 20 };
                        //        short randomDragonBall = (short)ListDragonBall[ServerUtils.RandomNumber(ListDragonBall.Count)];
                        //        item = ItemCache.GetItemDefault(randomDragonBall);
                        //        item.Quantity = 1;
                        //        return new ItemMap(charId, item);
                        //    }
                        //    else if (percentSuccess < 50)
                        //    {
                        //        int index = ServerUtils.RandomNumber(DataCache.ListDaNangCap.Count);
                        //        short idDaNangCap = DataCache.ListDaNangCap[index];
                        //        item = ItemCache.GetItemDefault(idDaNangCap);
                        //        item.Quantity = 1;
                        //        return new ItemMap(charId, item);
                        //    }
                        //    else if (percentSuccess < 70)
                        //    {
                        //        var CaiTrangDSPL = character.ItemBody[5];
                        //        if (CaiTrangDSPL == null || CaiTrangDSPL?.Options?.FirstOrDefault(option => option.Id == 110) == null) return null;
                        //        int index = ServerUtils.RandomNumber(DataCache.ListSaoPhaLe.Count);
                        //        short idSaoPhaLe = DataCache.ListSaoPhaLe[index];
                        //        item = ItemCache.GetItemDefault(idSaoPhaLe);
                        //        item.Quantity = 1;
                        //        return new ItemMap(charId, item);
                        //    }
                        //    else
                        //    {
                        //        return LeaveGold(character, ServerUtils.RandomNumber(2000, 3000), goldPlusPercent);
                        //    }
                        //}
                    }
                    else
                    {
                        var percentDropSetKichHoat = ServerUtils.RandomNumber(0.0000, 100.000) < 4;
                        if (percentDropSetKichHoat)
                        {
                            return LeaveSKH(character, mapId);
                        }
                        else
                        {
                            if (percentSuccess < 20)
                            {
                                return LeaveManhQuai(charId, monsterId);
                            }
                            else  if (percentSuccess < 30)
                            {
                                short randomDragonBall = (short)DataCache.ListDragonball[ServerUtils.RandomNumber(DataCache.ListDragonball.Count)];

                                item = ItemCache.GetItemDefault(randomDragonBall);
                                item.Quantity = 1;
                                return new ItemMap(charId, item);
                            }
                            else if (percentSuccess < 50)
                            {
                                int index = ServerUtils.RandomNumber(DataCache.ListDaNangCap.Count);
                                short idDaNangCap = DataCache.ListDaNangCap[index];
                                item = ItemCache.GetItemDefault(idDaNangCap);
                                item.Quantity = 1;
                                return new ItemMap(charId, item);
                            }
                            else if (percentSuccess < 70)
                            {
                                var CaiTrangDSPL = character.ItemBody[5];
                                if (CaiTrangDSPL == null || CaiTrangDSPL?.Options?.FirstOrDefault(option => option.Id == 110) == null) return null;
                                int index = ServerUtils.RandomNumber(DataCache.ListSaoPhaLe.Count);
                                short idSaoPhaLe = DataCache.ListSaoPhaLe[index];
                                item = ItemCache.GetItemDefault(idSaoPhaLe);
                                item.Quantity = 1;
                                return new ItemMap(charId, item);
                            }
                            else
                            {
                                return LeaveGold(character, ServerUtils.RandomNumber(2000, 3500), goldPlusPercent);
                            }
                        }
                    }
                case 2:
                    if (character.Id < 0) break;
                    if (TaskHandler.CheckTask((Model.Character.Character)character, 8, 1))
                    {
                        if (monsterId == 10 || monsterId == 11 || monsterId == 12)
                        {
                            
                                item = ItemCache.GetItemDefault(20);
                                item.Quantity = 1;
                                return new ItemMap(charId, item);
                            
                           
                        }
                        else
                        {
                            return LeaveGold(character, ServerUtils.RandomNumber(2100, 3600), goldPlusPercent);
                        }
                    }
                    else
                    {
                        if (percentSuccess < 30)
                        {
                            short randomDragonBall = (short)DataCache.ListDragonball[ServerUtils.RandomNumber(DataCache.ListDragonball.Count)];

                            item = ItemCache.GetItemDefault(randomDragonBall);
                            item.Quantity = 1;
                            return new ItemMap(charId, item);
                        }
                        else if (percentSuccess < 50)
                        {
                            int index = ServerUtils.RandomNumber(DataCache.ListDaNangCap.Count);
                            short idDaNangCap = DataCache.ListDaNangCap[index];
                            item = ItemCache.GetItemDefault(idDaNangCap);
                            item.Quantity = 1;
                            return new ItemMap(charId, item);
                        }
                        else if (percentSuccess < 70)
                        {
                            var CaiTrangDSPL = character.ItemBody[5];
                            if (CaiTrangDSPL == null || CaiTrangDSPL?.Options?.FirstOrDefault(option => option.Id == 110) == null) return null;
                            int index = ServerUtils.RandomNumber(DataCache.ListSaoPhaLe.Count);
                            short idSaoPhaLe = DataCache.ListSaoPhaLe[index];
                            item = ItemCache.GetItemDefault(idSaoPhaLe);
                            item.Quantity = 1;
                            return new ItemMap(charId, item);
                        }

                        else
                        {
                            return LeaveGold(character, ServerUtils.RandomNumber(2200, 3700), goldPlusPercent);
                        }
                    }
                    break;
                case 3:
                    if (TaskHandler.CheckTask((Model.Character.Character) character, 15, 1))
                    {
                        var percentDropTruyenDoremon = ServerUtils.RandomNumber(100) < 30;
                        if (monsterId == 13 || monsterId == 14 || monsterId == 15)
                        {
                            if (percentDropTruyenDoremon)
                            {
                                item = ItemCache.GetItemDefault(85);
                                item.Quantity = 1;
                                return new ItemMap(charId, item);
                            }
                        }
                        else
                        {
                            if (percentSuccess < 30)
                            {
                                short randomDragonBall = (short)DataCache.ListDragonball[ServerUtils.RandomNumber(DataCache.ListDragonball.Count)];
                                item = ItemCache.GetItemDefault(randomDragonBall);
                                item.Quantity = 1;
                                return new ItemMap(charId, item);
                            }
                            else if (percentSuccess < 50)
                            {
                                int index = ServerUtils.RandomNumber(DataCache.ListDaNangCap.Count);
                                short idDaNangCap = DataCache.ListDaNangCap[index];
                                item = ItemCache.GetItemDefault(idDaNangCap);
                                item.Quantity = 1;
                                return new ItemMap(charId, item);
                            }
                            else if (percentSuccess < 70)
                            {
                                var CaiTrangDSPL = character.ItemBody[5];
                                if (CaiTrangDSPL == null || CaiTrangDSPL?.Options?.FirstOrDefault(option => option.Id == 110) == null) return null;
                                int index = ServerUtils.RandomNumber(DataCache.ListSaoPhaLe.Count);
                                short idSaoPhaLe = DataCache.ListSaoPhaLe[index];
                                item = ItemCache.GetItemDefault(idSaoPhaLe);
                                item.Quantity = 1;
                                return new ItemMap(charId, item);
                            }
                            else
                            {
                                return LeaveGold(character, ServerUtils.RandomNumber(2400, 3800), goldPlusPercent);
                            }
                        }
                    }
                    else
                    {
                        if (percentSuccess < 30)
                        {
                            short randomDragonBall = (short)DataCache.ListDragonball[ServerUtils.RandomNumber(DataCache.ListDragonball.Count)];
                            item = ItemCache.GetItemDefault(randomDragonBall);
                            item.Quantity = 1;
                            return new ItemMap(charId, item);
                        }
                        else if (percentSuccess < 50)
                        {
                            int index = ServerUtils.RandomNumber(DataCache.ListDaNangCap.Count);
                            short idDaNangCap = DataCache.ListDaNangCap[index];
                            item = ItemCache.GetItemDefault(idDaNangCap);
                            item.Quantity = 1;
                            return new ItemMap(charId, item);
                        }
                        else 
                        {


                            return LeaveGold(character, ServerUtils.RandomNumber(2500, 3900), goldPlusPercent);
                        }
                    }
                    break;
                case 9:
                    {
                        var charReal = (NgocRongGold.Model.Character.Character)character;
                        var percent = ServerUtils.RandomNumber(100);
                        if (charReal.InfoBuff.MayDoCSKB)
                        {
                            var percentDropCapsuleKiBi = ServerUtils.RandomNumber(100);
                            if (percent < 60)
                            {
                                item = ItemCache.GetItemDefault(1235);
                                item.Quantity = 1;
                                return new ItemMap(charId, item);
                            }
                            else if (percent < 70)
                            {
                                item = ItemCache.GetItemDefault(380);
                                item.Quantity = 1;
                                return new ItemMap(charId, item);
                            }else if (percent < 80)
                            {
                                item = ItemCache.GetItemDefault((short)ServerUtils.RandomNumber(933,934));
                                item.Quantity = 1;
                                return new ItemMap(charId, item);
                            }
                            else if (percent < 90)
                            {
                                int index = ServerUtils.RandomNumber(DataCache.ListDaNangCap.Count);
                                short idDaNangCap = DataCache.ListDaNangCap[index];
                                item = ItemCache.GetItemDefault(idDaNangCap);
                                item.Quantity = 1;
                                return new ItemMap(charId, item);
                            }
                            else
                            {
                                // drop vang
                                return LeaveGold(character, ServerUtils.RandomNumber(13000, 15000), goldPlusPercent);
                            }
                        }
                        else
                        {
                            if (percent < 30)
                            {
                                item = ItemCache.GetItemDefault(1235);
                                item.Quantity = 1;
                                return new ItemMap(charId, item);
                            }
                            else if (percent < 50)
                            {
                                // drop spl
                                var CaiTrangDSPL = character.ItemBody[5];
                                if (CaiTrangDSPL == null || CaiTrangDSPL?.Options?.FirstOrDefault(option => option.Id == 110) == null) return null;
                                int index = ServerUtils.RandomNumber(DataCache.ListSaoPhaLe.Count);
                                short idSaoPhaLe = DataCache.ListSaoPhaLe[index];
                                item = ItemCache.GetItemDefault(idSaoPhaLe);
                                item.Quantity = 1;
                                return new ItemMap(charId, item);
                            }
                            else if (percent < 70)
                            {
                                // drop ngoc rong

                                short randomDragonBall = (short)DataCache.ListDragonball[ServerUtils.RandomNumber(DataCache.ListDragonball.Count)];
                                item = ItemCache.GetItemDefault(randomDragonBall);
                                item.Quantity = 1;
                                return new ItemMap(charId, item);
                            }
                            else
                            {
                                // drop vang
                                return LeaveGold(character, ServerUtils.RandomNumber(13000, 15000), goldPlusPercent);
                            }

                        }
                    }
                case 11:
                    {
                        if (character.Id > 0)
                        {
                            var charRel = (Model.Character.Character)character;
                            var random = ServerUtils.RandomNumber(150);
                            if (random <= 15 && charRel.InfoBuff.MayDoLinhHon)
                            {
                                item = ItemCache.GetItemDefault(1294);
                                return new ItemMap(character.Id, item);
                            }
                            else
                            {
                                return LeaveGold(character.Id, ServerUtils.RandomNumber(10000, 50000));
                            }
                        }
                        else
                        {
                           return LeaveGold(character.Id, ServerUtils.RandomNumber(10000, 50000));
                        }
                    }
                case 10:
                    {
                        if (DataCache.IdMapCold.Contains(mapId) || DataCache.IdMapThucVat.Contains(mapId))
                        {
                            var percent = ServerUtils.RandomNumber(0.0000, 200.0000);
                            // drop thuc an
                            var charReal = (Model.Character.Character)character;
                            if (charReal.InfoSet.IsFullSetThanLinh)
                            {
                                if (percent <= 15)
                                {
                                    int index = ServerUtils.RandomNumber(DataCache.ListThucAn.Count);
                                    short idThucHan = DataCache.ListThucAn[index];
                                    item = ItemCache.GetItemDefault(idThucHan);
                                    item.Quantity = 1;

                                    item.Options.Add(new OptionItem()
                                    {
                                        Id = 30,
                                        Param = 1
                                    });
                                    return new ItemMap(charId, item);
                                }
                                else
                                {
                                    return LeaveGold(character.Id,ServerUtils.RandomNumber(2000,5000));
                                }
                            }
                            else
                            {
                                if (percent < 4)
                                {
                                    return LeaveGodItem(character, "đánh quái");
                                }
                                else if (percent < 0.03)
                                {
                                    return LeaveDoII(character);
                                }
                                else if (percent < 40)
                                {


                                    int index = ServerUtils.RandomNumber(DataCache.ListDaNangCap.Count);
                                    short idDaNangCap = DataCache.ListDaNangCap[index];
                                    item = ItemCache.GetItemDefault(idDaNangCap);
                                    item.Quantity = 1;
                                    return new ItemMap(charId, item);

                                }
                                else if (percent < 50)
                                {
                                    // drop spl
                                    var CaiTrangDSPL = character.ItemBody[5];
                                    if (CaiTrangDSPL == null || CaiTrangDSPL?.Options?.FirstOrDefault(option => option.Id == 110) == null) return null;
                                    int index = ServerUtils.RandomNumber(DataCache.ListSaoPhaLe.Count);
                                    short idSaoPhaLe = DataCache.ListSaoPhaLe[index];
                                    item = ItemCache.GetItemDefault(idSaoPhaLe);
                                    item.Quantity = 1;
                                    return new ItemMap(charId, item);
                                }
                                else if (percent < 70)
                                {
                                   // var random = ServerUtils.RandomNumber(100);

                                    // drop ngoc rong
                                    //if (random < 50)
                                    //{
                                    //    item = ItemCache.GetItemDefault(1003);
                                    //    item.Quantity = 1;
                                    //    return new ItemMap(charId, item);
                                    //}
                                    //else
                                    //{
                                        short randomDragonBall = (short)DataCache.ListDragonball[ServerUtils.RandomNumber(DataCache.ListDragonball.Count)];
                                        item = ItemCache.GetItemDefault(randomDragonBall);
                                        item.Quantity = 1;
                                        return new ItemMap(charId, item);
                                    //}
                                }
                                else
                                {
                                    // drop vang
                                    return LeaveGold(character, ServerUtils.RandomNumber(13000, 15000), goldPlusPercent);
                                }
                            }
                        }else if (mapId == 155)
                        {
                            var percentManhThienSu = ServerUtils.RandomNumber(100);
                            if (percentManhThienSu <= 60)
                            {
                                var charReal = (Model.Character.Character)character;
                                if (charReal.InfoSet.IsFullSetHuyDiet)
                                {
                                    int index = ServerUtils.RandomNumber(DataCache.ListManhAngel.Count);
                                    short idManhAngel = DataCache.ListManhAngel[index];
                                    item = ItemCache.GetItemDefault(idManhAngel);
                                    item.Quantity = 1;

                                    item.Options.Add(new OptionItem()
                                    {
                                        Id = 30,
                                        Param = 1
                                    });
                                    return new ItemMap(charId, item);
                                }
                                else
                                {
                                    int index = ServerUtils.RandomNumber(DataCache.ListDaNangCap.Count);
                                    short idDaNangCap = DataCache.ListDaNangCap[index];
                                    item = ItemCache.GetItemDefault(idDaNangCap);
                                    item.Quantity = 1;
                                    return new ItemMap(charId, item);
                                }
                            }
                            else if (percentManhThienSu <= 80)
                            {
                                short randomDragonBall = (short)DataCache.ListDragonball[ServerUtils.RandomNumber(DataCache.ListDragonball.Count)];
                                item = ItemCache.GetItemDefault(randomDragonBall);
                                item.Quantity = 1;
                                return new ItemMap(charId, item);
                            }
                            else
                            {
                                return LeaveGold(character, ServerUtils.RandomNumber(13000, 15000), goldPlusPercent);
                            }
                        }
                        
                    }
                    break;
                default:
                    if (percentSuccess < 30)
                    {
                        short randomDragonBall = (short)DataCache.ListDragonball[ServerUtils.RandomNumber(DataCache.ListDragonball.Count)];
                        item = ItemCache.GetItemDefault(randomDragonBall);
                        item.Quantity = 1;
                        return new ItemMap(charId, item);
                    }
                    else if (percentSuccess < 50)
                    {
                        int index = ServerUtils.RandomNumber(DataCache.ListDaNangCap.Count);
                        short idDaNangCap = DataCache.ListDaNangCap[index];
                        item = ItemCache.GetItemDefault(idDaNangCap);
                        item.Quantity = 1;
                        return new ItemMap(charId, item);
                    } 
                    else if (percentSuccess < 70)
                    {
                        var CaiTrangDSPL = character.ItemBody[5];
                        if (CaiTrangDSPL == null || CaiTrangDSPL?.Options?.FirstOrDefault(option => option.Id == 110) == null) return null;
                        int index = ServerUtils.RandomNumber(DataCache.ListSaoPhaLe.Count);
                        short idSaoPhaLe = DataCache.ListSaoPhaLe[index];
                        item = ItemCache.GetItemDefault(idSaoPhaLe);
                        item.Quantity = 1;
                        return new ItemMap(charId, item);   
                    }
                    else
                    {
                        return LeaveGold(character, ServerUtils.RandomNumber(8000, 9000), goldPlusPercent);
                    }
            }
            return new ItemMap(charId, item);
        }
         
        public static ItemMap LeaveGold(ICharacter character, int gold, int percentPlusGold)
        {
            var vang = ItemCache.GetItemDefault(189);
            if (gold > 30000)
            {
                vang = ItemCache.GetItemDefault(190);
            }
            vang.Quantity = gold;
            if (percentPlusGold > 0)
            {
                vang.Quantity += vang.Quantity * percentPlusGold / 100;
            }
            return new ItemMap(character.Id, vang);
        }
        public static ItemMap LeaveGodItem(ICharacter character, string reason = "")
        {
            var index = ServerUtils.RandomNumber(DataCache.ListDoThanLinh.Count);
            short idDoHuyDiet = DataCache.ListDoThanLinh[index];
            var item = ItemCache.GetItemDefault(idDoHuyDiet);
            var itemTemplate = ItemCache.ItemTemplate(item.Id);
            var typeItem = ItemCache.ItemTemplate(item.Id).Type;
            var optionItem = item.Options[0];
            var optionConfig = DataCache.OptionGodItem.TryGetValue(optionItem.Id, out var _option) ;
            int minParam;
            switch (itemTemplate.Gender)
            {
                case 0:
                    minParam = _option[0];
                    break;
                case 1:
                    minParam = _option[2];
                    break;
                default:
                    minParam = _option[4];
                    break;
            }
            var randomPercentPlus = ServerUtils.RandomNumber(1, 15);
            if (typeItem == 4)
            {
                randomPercentPlus = ServerUtils.RandomNumber(1, 7);
                optionItem.Param = minParam + randomPercentPlus;
            }
            else
            {
                optionItem.Param = minParam + (minParam * randomPercentPlus / 100);
            }
            if (ServerUtils.RandomNumber(50) < 20)
            {
                item.Options.Add(new OptionItem()
                {
                    Id = 107,
                    Param = ServerUtils.RandomNumber(1, 3),
                });
            }
            if (reason.Contains("quái"))
            {
                item.Options.Add(new OptionItem()
                {
                    Id = 206,
                    Param = randomPercentPlus,
                });
            }
            else
            {
                item.Options.Add(new OptionItem()
                {
                    Id = 207,
                    Param = randomPercentPlus,
                });
            }
            item.Options.Add(new OptionItem()
            {
                Id = ServerUtils.RandomNumber(86, 88),
                Param = 0,
            });
            item.Quantity = 1;

            return new ItemMap(character.Id, item);
        }
        public static ItemMap LeaveDoII(ICharacter character, string reason = "")
        {
            var index = ServerUtils.RandomNumber(DataCache.ListDoHiem.Count);
            short idDoHuyDiet = DataCache.ListDoHiem[index];
            var item = ItemCache.GetItemDefault(idDoHuyDiet);
            var typeItem = ItemCache.ItemTemplate(item.Id).Type;
            var option = item.Options[0];
            var randomPercentPlus = ServerUtils.RandomNumber(1, 15);
            option.Param = option.Param + (option.Param * randomPercentPlus / 100);
            if (ServerUtils.RandomNumber(50) < 20)
            {
                item.Options.Add(new OptionItem()
                {
                    Id = 107,
                    Param = ServerUtils.RandomNumber(1, 3),
                });

            }
            item.Options.Add(new OptionItem()
            {
                Id = ServerUtils.RandomNumber(86, 88),
                Param = 0,
            });
            if (reason.Contains("quái"))
            {
                item.Options.Add(new OptionItem()
                {
                    Id = 206,
                    Param = randomPercentPlus,
                });
            }
            else
            {
                item.Options.Add(new OptionItem()
                {
                    Id = 207,
                    Param = randomPercentPlus,
                });
            }
            item.Quantity = 1;

            return new ItemMap(character.Id, item);
        }
        public static ItemMap LeaveItemStar(ICharacter character, List<short> listIdItem, int maxStar)
        {
            var item = ItemCache.GetItemDefault(listIdItem[ServerUtils.RandomNumber(listIdItem.Count)]);
            item.Options.Add(new OptionItem()
            {
                Id = 107,
                Param = ServerUtils.RandomNumber(maxStar)
            });
            item.Quantity = 1;
            return new ItemMap(character.Id, item);
        }
        public static ItemMap LeaveSKH(ICharacter character, int mapId = 0)
        {
            if (character.Id < 0) return null;
            if (mapId != 1 && mapId != 2 && mapId != 3 && mapId != 8 && mapId != 9 && mapId != 10 
            && mapId != 15 && mapId != 16 && mapId != 17) return null;
            var gender = character.InfoChar.Gender;          
            var listItem = new List<short>(){0,6,12,21,27};
            var listSKH =    new List<int>(){127,128,129};
            if (gender == 1)
            {
                listItem = new List<short>() { 1, 7, 12, 22, 28 };
                listSKH = new List<int>() { 131, 132, 130 };
            }
            else if (gender == 2)
            {
                listItem = new List<short>() { 2, 8, 12, 23, 29 };
                listSKH = new List<int>() { 133, 134, 135 };
            }
            var item = ItemCache.GetItemDefault(listItem[ServerUtils.RandomNumber(listItem.Count)]);
            item.Quantity = 1;
            if (ServerUtils.RandomNumber(100) < 30)
            {
                item.Options.Add(new OptionItem()
                {
                    Id = 107,
                    Param = ServerUtils.RandomNumber(3)
                });
            }
            else
            {
                var idSKH = listSKH[ServerUtils.RandomNumber(listSKH.Count)];
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
                //item.Options.Add(new OptionItem()
                //{
                //    Id = 30,
                //    Param = 0,
                //});
            }
            return new ItemMap(character.Id, item);
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
    }
}