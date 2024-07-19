using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Linq.Extras;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.Application.Handlers.Skill;
using NgocRongGold.DatabaseManager;
using NgocRongGold.DatabaseManager.Player;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Info;
using NgocRongGold.Model.Info.Radar;
using NgocRongGold.Model.Item;
using NgocRongGold.Model.Option;
using NgocRongGold.Model.SkillCharacter;
using NgocRongGold.Model.Template;
using NgocRongGold.Application.Helper;
using NgocRongGold.Application.Menu;
using NgocRongGold.Model.Map;
using System.Threading.Tasks;
using NgocRongGold.Application.Extension.Namecball;
using NgocRongGold.Application.Extension;
using NgocRongGold.Application.Extension.Dragon;
using System.Runtime.InteropServices;
using NgocRongGold.Application.Extension.SideQuest.BoMong;
using System.Threading.Channels;
using Application.Constants;
using NgocRongGold.Application.Interfaces.Character;
using System.Runtime.CompilerServices;
using System.Net.WebSockets;
using Org.BouncyCastle.Math.Field;
using System.Drawing;
using System.Net.Quic;
using NgocRongGold.Application.Extension.Event;

namespace NgocRongGold.Application.Handlers.Item
{
    public class ItemHandler
    {
        //public static Model.Item.Item ConvertItem(ItemShop itemShop)
        //{
        //    var itemTemplate = Cache.Gi().ITEM_TEMPLATES.Values.FirstOrDefault(item => item.Id == itemShop.Id);
        //    if (itemTemplate == null) return null;
        //    var item = new Model.Item.Item();
        //    item.Id = itemTemplate.Id;
        //    //item.Options= itemTemplate.Options;
        //    item.Quantity = itemShop.Quantity;
        //    item.BuyPotential = 0;
        //    item.SaleCoin = itemTemplate.SaleCoin;
        //    item.Options.AddRange(itemShop.Options.ToList());

        //    return ItemHandler.Clone(item);
        //}
        public static Model.Item.Item ConvertItem(ItemShop itemShop)
        {
            var item = new Model.Item.Item()
            {
                Id = itemShop.Id,
                SaleCoin = ItemCache.ItemTemplate(itemShop.Id).SaleCoin,
                BuyPotential = itemShop.Power,
                Quantity = itemShop.Quantity,
            };

            if (itemShop.Options.Count < 1)
            {
                item.Options.Add(new OptionItem()
                {
                    Id = 73,
                    Param = 0,
                });
            }
            else
            {
                item.Options = itemShop.Options.Copy();
            }
            return item;
        }
        
        public static bool CheckTrue(Model.Character.Character character, int itemId, int quantity)
        {
            if (character.CharacterHandler.GetItemBagById(itemId).Quantity >= quantity) return true;
            return false;
        }
        public static string TrueItem(Model.Character.Character character, List<int> ItemId, List<int> Quantitys, int TypeDoiThuong)
        {
            List<string> ListText = new List<string>();
            if (ItemId.Count != Quantitys.Count) return "";
            for (int i = 0; i < ItemId.Count; i++)
            {
                if (ItemId[i] == -1)
                {
                    var item = character.InfoChar.Gold;
                    if (item >= Quantitys[i])
                    {
                        ListText.Add($"{ServerUtils.Color("blue")}Giá vàng: {ServerUtils.GetMoney(item)}/{Quantitys[i]}");
                    }
                    else
                    {
                        TypeDoiThuong = -1;

                        ListText.Add($"{ServerUtils.Color("red")}Giá vàng: {ServerUtils.GetMoney(item)}/{Quantitys[i]}");
                    }
                }
                else if (ItemId[i] == -2)
                {
                    var item = character.InfoChar.Diamond;
                    if (item >= Quantitys[i])
                    {
                        ListText.Add($"{ServerUtils.Color("blue")}Giá ngọc: {ServerUtils.GetMoney(item)}/{Quantitys[i]}");
                    }
                    else
                    {
                        TypeDoiThuong = -1;
                        ListText.Add($"{ServerUtils.Color("red")}Giá ngọc: {ServerUtils.GetMoney(item)}/{Quantitys[i]}");
                    }
                }
                else
                {
                    if (character.CharacterHandler.GetItemBagById(ItemId[i]) != null)
                    {
                        var item = character.CharacterHandler.GetItemBagById(ItemId[i]);
                        if (item.Quantity >= Quantitys[i])
                        {
                            ListText.Add($"{ServerUtils.Color("blue")}{ItemCache.ItemTemplate((short)(ItemId[i])).Name} {item.Quantity}/{Quantitys[i]}");
                        }
                        else
                        {
                            TypeDoiThuong = -1;
                            ListText.Add($"{ServerUtils.Color("red")}{ItemCache.ItemTemplate((short)(ItemId[i])).Name} {item.Quantity}/{Quantitys[i]}");
                        }
                    }
                    else
                    {
                        ListText.Add($"{ServerUtils.Color("red")}{ItemCache.ItemTemplate((short)(ItemId[i])).Name} 0/{Quantitys[i]}");
                        TypeDoiThuong = -1;
                    }
                }
            }

            character.TypeDoiThuong = TypeDoiThuong;
            var Text = "";
            for (int j = 0; j < ListText.Count; j++)
            {
                Text += ListText[j];
            }
            return Text;
        }
        public static List<string> TrueListItem(Model.Character.Character character, List<int> ItemId, List<int> Quantitys)
        {
            List<string> Text = new List<string>();
            if (ItemId.Count != Quantitys.Count) return Text;
            for (int i = 0; i < ItemId.Count; i++)
            {
                if (ItemId[i] == -1)
                {
                    var item = character.InfoChar.Gold;
                    if (item >= Quantitys[i])
                    {
                        Text.Add($"{ServerUtils.Color("blue")}Giá vàng {ServerUtils.GetMoney(item)}/{Quantitys[i]}");
                    }
                    else
                    {
                        Text.Add($"{ServerUtils.Color("red")}Giá vàng {ServerUtils.GetMoney(item)}/{Quantitys[i]}");

                    }
                }
                else
                {
                    var item = character.CharacterHandler.GetItemBagById(ItemId[i]);
                    if (item.Quantity >= Quantitys[i])
                    {
                        Text.Add($"{ServerUtils.Color("blue")}{ItemCache.ItemTemplate((short)(ItemId[i])).Name} {item.Quantity}/{Quantitys[i]}");
                    }
                    else
                    {
                        Text.Add($"{ServerUtils.Color("red")}{ItemCache.ItemTemplate((short)(ItemId[i])).Name} {item.Quantity}/{Quantitys[i]}");

                    }
                }
            }
            return Text;
        }
        public static Model.Item.Item Clone(Model.Item.Item item)
        {
            var itemClone = new Model.Item.Item()
            {
                Id = item.Id,
                SaleCoin = item.Vang,
                BuyPotential = item.BuyPotential,
                Quantity = item.Quantity,
            };
            if (item.Options.Count < 1)
            {
                itemClone.Options.Add(new OptionItem()
                {
                    Id = 73,
                    Param = 0,
                });
            }
            else
            {
                itemClone.Options = item.Options.Copy();
            }

            return itemClone;
        }
        public static void KiGUI(Model.Character.Character character, sbyte action, int itemId, sbyte MoneyType, int money, int quantity)
        {
            //var items = Cache.Gi().SHOP_KY_GUI.FirstOrDefault(i =>i.)
            switch (action)
            {

                case 4:
                    // message.writer().writeByte(moneyType);
                    // message.writer().writeByte(money);
                    // Res.outz("currTab= " + moneyType + " page= " + money);
                    break;
                case 0: // kí gửi
                    // message.writer().writeShort(itemId);
                    // message.writer().writeByte(moneyType);
                    // message.writer().writeInt(money);
                    // message.writer().writeInt(quaintly);
                    break;
                case 2: // nhận tiền                   
                    // message.writer().writeShort(itemId);

                    break;
                case 3: // muaa 
                    // message.writer().writeShort(itemId);
                    // message.writer().writeByte(moneyType);
                    // message.writer().writeInt(money);
                    break;
                case 1: // Hủy Kí Gửi 
                        // message.writer().writeShort(itemId);

                    break;
                case 5: // Up lên Top 
                    // message.writer().writeShort(itemId);
                    break;
            }
        }
        public static void BuyItem(Model.Character.Character character, int typeBuy, short id, short quantity = 1)
        {
            try
            {
                //var timeserver = ServerUtils.CurrentTimeMillis();
                //if (character.Delay.InvAction > timeserver)
                //{
                //    character.CharacterHandler.SendMessage(Service.ServerMessage("Sống chậm thôi ..."));
                //    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                //    return;
                //}
                var shopTemplates = Cache.Gi().SHOP_TEMPLATES.FirstOrDefault(s => s.Key == character.ShopId).Value;

                switch (character.ShopId) {
                    case 4444:
                        {
                            ItemShop itemShop = null;
                            foreach (var shopTemplate in shopTemplates)
                            {
                                itemShop = shopTemplate.ItemShops.FirstOrDefault(item => item.Id == id);
                                if (itemShop != null) break;
                            }
                            if (itemShop == null) break;
                            var checkRole = character.InfoChar.Roles1.Roles.FirstOrDefault(i => i.Id == id);
                            var templateRole = Cache.Gi().Role1Templates.FirstOrDefault(i => i.Id == id);
                            if (checkRole != null)
                            {
                                character.CharacterHandler.SendZoneMessage(Service.SendRole(character.Id, checkRole.Second, checkRole.Temp));
                                character.InfoChar.Roles1.RoleUsed = checkRole;
                                character.CharacterHandler.SendMessage(Service.ServerMessage("" + templateRole.Name));
                                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                character.CharacterHandler.SendMessage(Service.ClosePanel());
                                break;
                            }
                            if (character.CharacterHandler.GetAllQuantityItemBagById(1549) < itemShop.Vang)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage($"Bạn còn thiếu {itemShop.Vang - character.CharacterHandler.GetAllQuantityItemBagById(1549)} hũ vàng nữa"));
                                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                character.CharacterHandler.SendMessage(Service.ClosePanel());
                                break;
                            }
                            character.CharacterHandler.RemoveItemBagById(1549, itemShop.Vang, "mua danh hiệu");
                            character.CharacterHandler.SendMessage(Service.SendBag(character));                            
                            var role = new Role1Template()
                            {
                                Id = templateRole.Id,
                                Name = templateRole.Name,
                                Temp = templateRole.Temp,
                                Delay = templateRole.Delay,
                                Second = templateRole.Second,
                                Options = templateRole.Options.Copy(),
                            };
                            character.CharacterHandler.SendZoneMessage(Service.SendRole(character.Id, role.Second, role.Temp));
                            character.InfoChar.Roles1.Roles.Add(role);
                            character.InfoChar.Roles1.RoleUsed = role;
                            character.CharacterHandler.SendMessage(Service.ServerMessage("" + templateRole.Name));
                            character.CharacterHandler.SendMessage(Service.BuyItem(character));
                            character.CharacterHandler.SendMessage(Service.ClosePanel());
                            character.CharacterHandler.SetUpInfo(true);
                        }
                        break;
                    case >= 7 and <= 11:
                        {
                            ItemShop itemShop = null;
                            foreach (var shopTemplate in shopTemplates)
                            {
                                itemShop = shopTemplate.ItemShops.FirstOrDefault(item => item.Id == id);
                                if (itemShop != null) break;
                            }
                            if (itemShop == null) return;
                            if (itemShop.Power > character.InfoChar.Potential)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_BUY_PPOINT));
                                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                return;
                            }
                            var itemTemplate = ItemCache.ItemTemplate(itemShop.Id);
                            if (itemTemplate.Require > character.InfoChar.Power)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_POWER));
                                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                return;
                            }

                            var itemNew = ConvertItem(itemShop);
                            var skillTemplate = UseSkill(character, itemNew);
                            if (skillTemplate == null) return;
                            var levelBook = itemTemplate.Level;
                            var timeStudy = "";
                            var timeLong = DataCache.TimeUseSkill[levelBook - 1];
                            timeStudy = levelBook switch
                            {
                                < 3 => ServerUtils.ConvertMilisecondToMinute(timeLong),
                                3 => ServerUtils.ConvertMilisecondToHour(timeLong),
                                _ => ServerUtils.ConvertMilisecondToDay(timeLong)
                            };
                            var money = ServerUtils.GetMoney((int)itemShop.Power);
                            character.InfoChar.LearnSkillTemp = new LearnSkill()
                            {
                                ItemSkill = itemNew,
                                Time = timeLong,
                                ItemTemplateSkillId = (short)skillTemplate.Id,
                            };
                            var menu = string.Format(TextServer.gI().DO_YOU_ADD_SKILL, skillTemplate.Name, money, timeStudy);
                            switch (character.TypeShop)
                            {
                                case 0:
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(13, menu, new List<string>() { "Đồng ý", "Từ chối" }, character.InfoChar.Gender));
                                        character.TypeMenu = 2;
                                        break;
                                    }
                                case 1:
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(14, menu, new List<string>() { "Đồng ý", "Từ chối" }, character.InfoChar.Gender));
                                        character.TypeMenu = 1;
                                        break;
                                    }
                                case 2:
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(15, menu, new List<string>() { "Đồng ý", "Từ chối" }, character.InfoChar.Gender));
                                        character.TypeMenu = 1;
                                        break;
                                    }
                            }
                            break;
                        }
                    case 1111:
                        {
                            switch (typeBuy)
                            {
                                //Lấy item từ luckyBox
                                case 0:
                                    {
                                        if (character.LengthBagNull() <= 0)
                                        {
                                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_BAG));
                                            return;
                                        }

                                        var itemcheck = character.CharacterHandler.GetItemLuckyBoxByIndex(id);
                                        if (itemcheck == null) return;

                                        using (var itemclone = ItemHandler.Clone(itemcheck))
                                        {
                                            var luckyBox = character.LuckyBox;
                                            var itemTemplate = ItemCache.ItemTemplate(itemclone.Id);
                                            if (itemTemplate.Type == 9)
                                            {
                                                var gold = itemclone.Options.FirstOrDefault(opt => opt.Id == 171);
                                                if (gold != null && gold.Param > 0)
                                                {
                                                    character.PlusGold(gold.Param * 1000);
                                                    character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                                                }
                                            }
                                            else
                                            {
                                                character.CharacterHandler.AddItemToBag(true, itemclone, "Lấy từ lucky box");
                                                character.CharacterHandler.SendMessage(Service.SendBag(character));
                                            }

                                            character.CharacterHandler.RemoveItemLuckyBox(id);
                                            character.CharacterHandler.SendMessage(Service.SubBox(luckyBox));
                                            character.ShopId = 1111;
                                        }
                                        break;
                                    }
                                // Xóa Item LuckyBox
                                case 1:
                                    {
                                        var itemcheck = character.CharacterHandler.GetItemLuckyBoxByIndex(id);
                                        if (itemcheck == null) return;
                                        var luckyBox = character.LuckyBox;
                                        character.CharacterHandler.RemoveItemLuckyBox(id);
                                        character.CharacterHandler.SendMessage(Service.SubBox(luckyBox));
                                        character.ShopId = 1111;
                                        break;
                                    }
                                case 2:
                                    {
                                        if (character.LengthBagNull() <= 0)
                                        {
                                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_BAG));
                                            return;
                                        }

                                        foreach (var item in character.LuckyBox.ToList())
                                        {
                                            if (item == null) return;
                                            using (var itemclone = ItemHandler.Clone(item))
                                            {
                                                var itemTemplate = ItemCache.ItemTemplate(itemclone.Id);
                                                if (itemTemplate.Type == 9)
                                                {
                                                    var gold = itemclone.Options.FirstOrDefault(opt => opt.Id == 171);
                                                    if (gold != null && gold.Param > 0)
                                                    {
                                                        character.PlusGold(gold.Param * 1000);
                                                        character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                                                    }
                                                }
                                                else
                                                {
                                                    if (character.LengthBagNull() <= 0)
                                                    {
                                                        character.CharacterHandler.SendMessage(Service.SubBox(character.LuckyBox));
                                                        character.ShopId = 1111;
                                                        character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_BAG));
                                                        return;
                                                    }
                                                    character.CharacterHandler.AddItemToBag(true, itemclone, "Lấy từ lucky box");
                                                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                                                }

                                                character.CharacterHandler.RemoveItemLuckyBox(item.IndexUI, false);
                                            }
                                        }
                                        var luckyBox = character.LuckyBox;
                                        character.CharacterHandler.SendMessage(Service.SubBox(luckyBox));
                                        character.ShopId = 1111;
                                        break;
                                    }
                                    //Ignore
                            }
                            break;
                        }
                    case 3333:
                        {
                            if (character.LengthBagNull() <= 0)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_BAG));
                                return;
                            }
                            var item = character.ItemSells.FirstOrDefault(i => i.Id == id);
                            if (item == null)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Lỗi rồi mau thông báo cho đại vương ngay !"));
                                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                character.CharacterHandler.SendMessage(Service.ClosePanel());
                                return;
                            }
                            if (character.InfoChar.Gold < item.Vang)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_GOLD));
                                return;
                            }
                            var itemTemplate = ItemCache.ItemTemplate(item.Id);
                            if (itemTemplate.Require > character.InfoChar.Power)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_POWER));
                                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                return;
                            }
                            character.MineGold(item.Vang);
                            character.ItemSells.Remove(item);
                            character.CharacterHandler.AddItemToBag(false, item);
                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                            character.CharacterHandler.SendMessage(Service.BuyItem(character));
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + itemTemplate.Name));
                            character.CharacterHandler.SendMessage(Service.ClosePanel());
                            break;
                        }
                    case 2222:
                        {
                            switch (typeBuy)
                            {
                                case 0:
                                    if (character.LengthBagNull() <= 0)
                                    {
                                        character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_BAG));
                                        return;
                                    }

                                    var itemcheck = character.ItemGift.FirstOrDefault(item => item.IndexUI == id);
                                    if (itemcheck == null) return;

                                    using (var itemclone = ItemHandler.Clone(itemcheck))
                                    {
                                        var luckyBox = character.ItemGift;
                                        var itemTemplate = ItemCache.ItemTemplate(itemclone.Id);
                                        if (itemTemplate.Type == 9)
                                        {
                                            var gold = itemclone.Options.FirstOrDefault(opt => opt.Id == 171);
                                            if (gold != null && gold.Param > 0)
                                            {
                                                character.PlusGold(gold.Param * 1000);
                                                character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                                            }
                                        }
                                        else
                                        {
                                            character.CharacterHandler.AddItemToBag(true, itemclone, "Lấy từ gift box");
                                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                                        }

                                        character.CharacterHandler.RemoveItemGiftBox(id);
                                        character.CharacterHandler.SendMessage(Service.GiftBox(luckyBox));
                                        character.ShopId = 2222;
                                    }
                                    break;
                            }
                            break;
                        }
                    default:
                        {
                            switch (character.TypeShop)
                            {
                                case 0:
                                    {
                                        if (DataCache.IdDauThanx30.Contains(id))
                                        {
                                            id = (short)DataCache.IdDauThanx30[0];
                                        }
                                        ItemShop itemShop = null;
                                        foreach (var shopTemplate in shopTemplates)
                                        {
                                            itemShop = shopTemplate.ItemShops.FirstOrDefault(item => item.Id == id);
                                            if (itemShop != null) break;
                                        }
                                        if (itemShop != null)
                                        {
                                            if (character.ShopId - 3 == character.InfoChar.Gender)
                                            {
                                                character.InfoChar.Hair = itemShop.HeadTemp;
                                                character.CharacterHandler.SendMessage(Service.ClosePanel());
                                                character.CharacterHandler.SendZoneMessage(Service.UpdateBody(character));
                                                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã được cắt quả đầu mới siêu cấp Vip Pro"));
                                                return;
                                            }

                                            var itemTemplate = ItemCache.ItemTemplate(itemShop.Id);
                                            if (itemTemplate.Require > character.InfoChar.Power)
                                            {
                                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_POWER));
                                                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                                return;
                                            }

                                            var bGold = itemShop.Vang;
                                            var bDiamond = itemShop.Ngoc;
                                            Model.Item.Item itemNew;
                                            if (DataCache.IdDauThanx30.Contains(itemShop.Id))
                                            {
                                                var levelMagic = MagicTreeManager.Get(character.Id).Level;
                                                if (levelMagic == 1) levelMagic = 2;
                                                var idNew = (short)DataCache.IdDauThanx30[levelMagic - 2];
                                                var index = DataCache.IdDauThanx30.IndexOf(idNew);
                                                itemNew = ItemCache.GetItemDefault((short)DataCache.IdDauThan[index]);
                                                itemTemplate = ItemCache.ItemTemplate(itemNew.Id);
                                                itemNew.Quantity = 30;
                                                if (index == 0) index = 1;
                                                bDiamond *= index;
                                            }
                                            else
                                            {
                                                itemNew = ConvertItem(itemShop);
    

                                                if (itemShop.Id == 193) itemNew.Quantity = 10;
                                            }


                                            switch (itemShop.Id)
                                            {
                                                //Kỹ năng đệ tử 1
                                                case 402:
                                                    {
                                                        if (character.Disciple != null)
                                                        {
                                                            bDiamond *= character.Disciple.Skills[0].Point;
                                                        }
                                                        break;
                                                    }
                                                //Kỹ năng đệ tử 2
                                                case 403:
                                                    {
                                                        if (character.Disciple != null)
                                                        {
                                                            if (character.Disciple.Skills.Count >= 2)
                                                            {
                                                                bDiamond *= character.Disciple.Skills[1].Point;
                                                            }
                                                        }
                                                        break;
                                                    }
                                                //Kỹ năng đệ tử 3
                                                case 404:
                                                    {
                                                        if (character.Disciple != null)
                                                        {
                                                            if (character.Disciple.Skills.Count >= 3)
                                                            {
                                                                bDiamond *= character.Disciple.Skills[2].Point;
                                                            }
                                                        }
                                                        break;
                                                    }
                                                //Kỹ năng đệ tử 4
                                                case 759:
                                                    {
                                                        if (character.Disciple != null)
                                                        {
                                                            if (character.Disciple.Skills.Count >= 4)
                                                            {
                                                                bDiamond *= character.Disciple.Skills[3].Point;
                                                            }
                                                        }
                                                        break;
                                                    }
                                                case 517:
                                                    {
                                                        if (character.PlusBag >= DataCache.MAX_PLUS_BAG)
                                                        {
                                                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().MAX_NUMBERS_BAG));
                                                            character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                                            return;
                                                        }
                                                        bGold *= (character.PlusBag + 1);
                                                        break;
                                                    }
                                                case 518:
                                                    {
                                                        if (character.PlusBox >= DataCache.MAX_PLUS_BAG)
                                                        {
                                                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().MAX_NUMBERS_BOX));
                                                            character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                                            return;
                                                        }
                                                        bGold *= (character.PlusBox + 1);
                                                        break;
                                                    }
                                                default:
                                                    {
                                                        if (character.ShopId > 2)
                                                        {
                                                            var itemCheck = character.ItemBag.FirstOrDefault(item =>
                                                                item.Id == itemTemplate.Id && item.Quantity + itemNew.Quantity < 99);
                                                            if ((!itemTemplate.IsUpToUp || itemCheck == null) && character.LengthBagNull() < 1)
                                                            {
                                                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_BAG));
                                                                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                                                return;
                                                            }
                                                        }
                                                        break;
                                                    }
                                            }

                                            if (bGold > character.InfoChar.Gold)
                                            {
                                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_GOLD));
                                                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                                return;
                                            }

                                            if (bDiamond > character.AllDiamond())
                                            {
                                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_DIAMOND));
                                                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                                return;
                                            }

                                            switch (character.ShopId)
                                            {
                                                case 0:
                                                case 1:
                                                case 2:
                                                    {
                                                        var timePlus = (character.ShopId != 0
                                                            ? character.ShopId != 1 ? DataCache._1MONTH : DataCache._8HOURS
                                                            : DataCache._1HOUR); ;
                                                        if (character.InfoChar.ItemAmulet.ContainsKey(itemNew.Id))
                                                        {
                                                            if (character.InfoChar.ItemAmulet[itemNew.Id] < ServerUtils.CurrentTimeMillis())
                                                            {
                                                                character.InfoChar.ItemAmulet[itemNew.Id] = timePlus + ServerUtils.CurrentTimeMillis();
                                                            }
                                                            else
                                                            {
                                                                character.InfoChar.ItemAmulet[itemNew.Id] += timePlus;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            character.InfoChar.ItemAmulet.TryAdd(itemNew.Id, timePlus + ServerUtils.CurrentTimeMillis());
                                                        }
                                                        // Setup bùa
                                                        // character.CharacterHandler.SendMessage(Service.ClosePanel());
                                                        character.CharacterHandler.SendMessage(Service.Shop(character, 0, character.ShopId));
                                                        character.SetupAmulet();
                                                        break;
                                                    }
                                                default:
                                                    {
                                                        switch (itemShop.Id)
                                                        {
                                                            case 453:
                                                                {
                                                                    character.InfoChar.Teleport = 3;
                                                                    break;
                                                                }
                                                            case 517:
                                                                {
                                                                    character.PlusBag += 1;
                                                                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                                                                    break;
                                                                }
                                                            case 518:
                                                                {
                                                                    character.PlusBox += 1;
                                                                    character.CharacterHandler.SendMessage(Service.SendBox(character));
                                                                    break;
                                                                }
                                                            // Xử lý mua mảnh vỡ ở đây
                                                            case 933:
                                                                {
                                                                    // Kiểm tra trong túi có item chưa
                                                                    var itemManhVoBongTai = character.CharacterHandler.GetItemBagById(933);
                                                                    if (itemManhVoBongTai != null)
                                                                    {
                                                                        var soLuongManhVoBongTaiHT = itemManhVoBongTai.Options.FirstOrDefault(opt => opt.Id == 31); //Số lượng bông tai
                                                                        soLuongManhVoBongTaiHT.Param += 10;//default
                                                                        
                                                                    }
                                                                    else
                                                                    {
                                                                        if (!character.CharacterHandler.AddItemToBag(true, itemNew, "Mua mảnh vỡ từ cửa hàng")) return;
                                                                    }
                                                                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                                                                    break;
                                                                }
                                                            // Xử lý mua tự động luyện tập
                                                            case 521:
                                                                {
                                                                    // Kiểm tra trong túi có item chưa
                                                                    var itemTuDongLuyenTap = character.CharacterHandler.GetItemBagById(521);
                                                                    if (itemTuDongLuyenTap != null)
                                                                    {
                                                                        var soLuongTuDongLuyenTap = itemTuDongLuyenTap.Options.FirstOrDefault(opt => opt.Id == 1); //Số lượng bông tai
                                                                        var soLuongTDLTCuaHang = itemNew.Options.FirstOrDefault(opt => opt.Id == 1);
                                                                        if (soLuongTuDongLuyenTap != null && soLuongTDLTCuaHang != null)
                                                                        {
                                                                            soLuongTuDongLuyenTap.Param += soLuongTDLTCuaHang.Param;
                                                                        }
                                                                        else
                                                                        {
                                                                            soLuongTuDongLuyenTap.Param += 20;//default
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (!character.CharacterHandler.AddItemToBag(true, itemNew, "Mua TDTL từ cửa hàng")) return;
                                                                    }
                                                                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                                                                    break;
                                                                }
                                                           
                                                            // 
                                                            default:
                                                                {
                                                                   
                        
                                                                    if (!character.CharacterHandler.AddItemToBag(true, itemNew, "Mua từ cửa hàng")) return;
                                                                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                                                                    break;
                                                                }
                                                        }
                                                        break;
                                                    }
                                            }

                                            switch (typeBuy)
                                            {
                                                case 0:
                                                    character.MineGold(bGold);
                                                    break;
                                                case 1:
                                                    character.MineDiamond(bDiamond);
                                                    break;
                                            }
                                            character.CharacterHandler.SendMessage(Service.ServerMessage(string.Format(TextServer.gI().ADD_ITEM, itemTemplate.Name)));
                                            character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                        }
                                        else
                                        {
                                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().ERROR_SERVER));
                                            character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                            return;
                                        }
                                        break;
                                    }
                                case 3:
                                    {
                                        switch (character.ShopId)
                                        {
                                            case 21://Shop Bill
                                                {
                                                    var fullThucAn = character.ItemBag.FirstOrDefault(item => DataCache.ListThucAn.Contains(item.Id) && item.Quantity >= 99);
                                                    if (fullThucAn == null)
                                                    {
                                                        character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_ITEM));
                                                        character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                                        return;
                                                    }
                                                    break;
                                                }
                                        }
                                        // special icon id

                                        ItemShop itemShop = null;
                                        foreach(var shop in shopTemplates)
                                        {
                                            itemShop = shop.ItemShops.FirstOrDefault(item => item.Id == id);
                                            if (itemShop != null) break;
                                        }
                                        if (itemShop == null) return;

                                        var itemTemplate = ItemCache.ItemTemplate(itemShop.Id);
                                        if (itemTemplate.Require > character.InfoChar.Power)
                                        {
                                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_POWER));
                                            character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                            return;
                                        }
                                        var price = itemShop.Vang;
                                        var typePrice = itemShop.Ngoc;
                                        var itemIdCheck = 0;
                                        switch (typePrice)
                                        {
                                            case 4028:
                                                {
                                                    itemIdCheck = 457;
                                                    break;
                                                }
                                            case 7743:
                                                {
                                                    itemIdCheck = -1;
                                                    break;
                                                }
                                        }
                                        switch (itemIdCheck)
                                        {
                                            case -1:
                                                if (character.AllDiamondLock() < price)
                                                {
                                                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_ITEM));
                                                    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                                    return;
                                                }
                                                break;
                                            default:
                                                if (character.CharacterHandler.GetItemBagById(itemIdCheck) == null || character.CharacterHandler.GetItemBagById(itemIdCheck).Quantity < price)
                                                {
                                                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_ITEM));
                                                    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                                    return;
                                                }
                                                break;
                                        }
                                        switch (itemIdCheck) {

                                            case -1:
                                                character.MineDiamond(price, 2);
                                                break;
                                            default:
                                                character.CharacterHandler.RemoveItemBagById((short)itemIdCheck, price, reason: "Mua đồ hủy diệt");
                                                break;
                                        }
                                        Model.Item.Item itemNew = ConvertItem(itemShop);
                                        var itemCheck = character.ItemBag.FirstOrDefault(item =>
                                        item.Id == itemTemplate.Id && item.Quantity + itemNew.Quantity < 99);

                                        if ((!itemTemplate.IsUpToUp || itemCheck == null) && character.LengthBagNull() < 1)
                                        {
                                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_BAG));
                                            character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                            return;
                                        }
                                        switch (character.ShopId)
                                        {
                                            case 21:
                                                {
                                                    var randomRate = ServerUtils.RandomNumber(0.0, 100.0);
                                                    var phanTramCongThem = 0;
                                                    if (randomRate <= 0.8) //14-15%
                                                    {
                                                        //0.8%
                                                        phanTramCongThem = ServerUtils.RandomNumber(14, 15);
                                                    }
                                                    else if (randomRate <= 1.6) //13-14%
                                                    {
                                                        //0.8%
                                                        phanTramCongThem = ServerUtils.RandomNumber(13, 14);
                                                    }
                                                    else if (randomRate <= 3.2) //11-12%
                                                    {
                                                        // 1.6%
                                                        phanTramCongThem = ServerUtils.RandomNumber(11, 12);
                                                    }
                                                    else if (randomRate <= 5.4) //9-10%
                                                    {
                                                        //2.2%
                                                        phanTramCongThem = ServerUtils.RandomNumber(9, 10);
                                                    }
                                                    else if (randomRate <= 8.4) //7-8%
                                                    {
                                                        // 3%
                                                        phanTramCongThem = ServerUtils.RandomNumber(7, 8);
                                                    }
                                                    else if (randomRate <= 18.4) //5-6%
                                                    {
                                                        // 10%
                                                        phanTramCongThem = ServerUtils.RandomNumber(5, 6);
                                                    }
                                                    else if (randomRate <= 38.4) //3-4% 
                                                    {
                                                        //20%
                                                        phanTramCongThem = ServerUtils.RandomNumber(3, 4);
                                                    }
                                                    else if (randomRate <= 92.4) //1-2% 
                                                    {
                                                        // 44%
                                                        phanTramCongThem = ServerUtils.RandomNumber(1, 2);
                                                    }
                                                    itemNew.Options.Where(option => DataCache.IdOptionGoc.Contains(option.Id)).ToList().ForEach(
                                                    option =>
                                                    {
                                                        option.Param += option.Param * phanTramCongThem / 100;
                                                    });
                                                    break;
                                                }
                                            default:
                                                break;
                                        }

                                        var optionHSD = itemNew.Options.FirstOrDefault(i => i.Id == 93);
                                        if (optionHSD != null)
                                        {
                                            var timeSecondServer = ServerUtils.CurrentTimeSecond();
                                            var expireTime = DataCache._1DAYBYSECOND * optionHSD.Param + timeSecondServer;
                                            var optionTimeHSD = itemNew.Options.FirstOrDefault(i => i.Id == 73);
                                            if (optionTimeHSD != null)
                                            {
                                                optionTimeHSD.Param = (int)expireTime;
                                                Server.Gi().Logger.Debug("buy item hsd: " + (optionTimeHSD.Param - timeSecondServer));
                                            }
                                            else
                                            {
                                                itemNew.Options.Add(new OptionItem()
                                                {
                                                    Id = 73,
                                                    Param = (int)expireTime,
                                                });
                                                Server.Gi().Logger.Debug("buy item hsd: " + (expireTime - timeSecondServer));
                                            }
                                        }
                                        character.CharacterHandler.AddItemToBag(true, itemNew, "Buy item spec");
                                        switch (character.ShopId)
                                        {
                                            case 21://Shop Bill
                                                {
                                                    var fullThucAn = character.ItemBag.FirstOrDefault(item => DataCache.ListThucAn.Contains(item.Id) && item.Quantity >= 99);
                                                    if (fullThucAn != null)
                                                    {
                                                        character.CharacterHandler.RemoveItemBagByIndex(fullThucAn.IndexUI, fullThucAn.Quantity, reason: "Mua đồ hủy diệt");
                                                    }
                                                    break;
                                                }
                                        }

                                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                                        character.CharacterHandler.SendMessage(Service.ServerMessage(string.Format(TextServer.gI().ADD_ITEM, itemTemplate.Name)));
                                        character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                        break;
                                    }

                                case 1: break;
                            }
                          //  character.Delay.InvAction = 350 + timeserver;
                            break;
                        }
                }
            }
            catch (Exception e)
            {

                Server.Gi().Logger.Print($"Error buy item in ItemHandler.cs: {e.Message} \n {e.StackTrace}");
                ClientManager.Gi().KickSession(character.Player.Session);
                var temp = ClientManager.Gi().GetPlayer(character.Player.Id);
                if (temp != null)
                {
                    ClientManager.Gi().KickSession(temp.Session);
                }
            }
        }

        public static void SellItem(Model.Character.Character character, int action, int type, short index)
        {
            try
            {
                Server.Gi().Logger.Debug($"Sell Item -------------------------- action: {action} - type: {type} - index: {index}");
                switch (action)
                {
                    //Hỏi bán item
                    case 0:
                        {
                            Model.Item.Item itemSell = null;
                            switch (type)
                            {
                                //Cải trang
                                case 0 when index == 5:
                                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().DO_NOT_SELL_ITEM));
                                    return;
                                //Body
                                case 0:
                                    itemSell = character.ItemBody[index];
                                    break;
                                //Bag
                                case 1:
                                    var version = int.Parse(character.Player.Session.Version.Replace(".", ""));
                                    if (version <= 221)
                                    {
                                        itemSell = character.CharacterHandler.GetItemBagByIndex(index - 5);
                                        index -= 5;
                                    }
                                    else
                                    {
                                        itemSell = character.CharacterHandler.GetItemBagByIndex(index);
                                    }
                                    break;
                            }
                            if (itemSell == null) return;
                            if (DataCache.ItemNotSell.Contains(itemSell.Id))
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().DO_NOT_SELL_ITEM));
                                return;
                            }
                            var template = ItemCache.ItemTemplate(itemSell.Id);
                            var gold = template.SaleCoin;
                            if (gold == -1 || template.Type == 5)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().DO_NOT_SELL_ITEM));
                                return;
                            }

                            if (itemSell.Id is 457)
                            {
                                //   gold = DataCache.GiaBanThoiVang;
                                var inputBanVang = new List<InputBox>();
                                var inputVang = new InputBox()
                                {
                                    Name = "(Nhập số thỏi vàng bạn muốn bán)",
                                    Type = 1,
                                };
                                inputBanVang.Add(inputVang);
                                character.CharacterHandler.SendMessage(Service.ShowInput("Bán thỏi vàng, 1 thỏi = 500tr", inputBanVang));
                                character.TypeInput = 26;
                                return;
                            }

                            var quantity = 1;
                            if (itemSell.Quantity > 1 && !ItemCache.IsItemSellOnlyOne(itemSell.Id))
                            {
                                quantity = itemSell.Quantity;
                            }
                            gold *= quantity;
                            var info = string.Format(TextServer.gI().DO_YOU_WANT, quantity, template.Name, ServerUtils.GetMoneys(gold));
                            character.CharacterHandler.SendMessage(Service.SellItem(type, index, info));
                            break;
                        }
                    //Đồng ý bán item
                    case 1:
                        {
                            Model.Item.Item itemSell = null;
                            switch (type)
                            {
                                //Cải trang
                                case 0 when index == 5:
                                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().DO_NOT_SELL_ITEM));
                                    return;
                                //Body
                                case 0:
                                    itemSell = character.ItemBody[index];
                                    break;
                                //Bag
                                case 1:
                                    itemSell = character.CharacterHandler.GetItemBagByIndex(index);
                                    break;
                            }
                            if (itemSell == null) return;
                            var template = ItemCache.ItemTemplate(itemSell.Id);
                            var gold = template.SaleCoin;
                            if (gold == -1 || template.Type == 5)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().DO_NOT_SELL_ITEM));
                                return;
                            }

                            if (itemSell.Id == 1343)
                            {
                                gold = DataCache.GiaBanThoiVang;
                            }

                            var quantity = 1;
                            if (itemSell.Quantity > 1 && !ItemCache.IsItemSellOnlyOne(itemSell.Id))
                            {
                                quantity = itemSell.Quantity;
                            }

                            gold *= quantity;


                            if (character.InfoChar.Gold + gold > character.InfoChar.LimitGold)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().GOLD_BAR_TO_GOLD_LIMIT));
                                return;
                            }

                            character.PlusGold(gold);

                            if (type == 0)
                            {
                                character.ItemBody[index] = null;
                                // character.Delay.SaveData += 1000;
                                var timeServer = ServerUtils.CurrentTimeMillis();
                                character.CharacterHandler.UpdateInfo(true);
                            }
                            else
                            {
                                character.CharacterHandler.RemoveItemBagByIndex(index, quantity, reason: "Bán cho shop");
                            }
                            var itemResell = itemSell;
                            var itemResells = character.ItemSells;
                            if (itemResells.FirstOrDefault(i => i.Id == itemResell.Id) != null)
                            {
                                var oldItemResell = itemResells.FirstOrDefault(i => i.Id == itemResell.Id);
                                itemResells[itemResells.IndexOf(oldItemResell)] = itemResell;
                            }
                            itemResell.Vang = gold + (gold * 10 / 100);
                            itemResell.Quantity = quantity;
                            itemResell.IndexUI = itemResells.Count;
                            character.ItemSells.Add(itemResell);
                            if (character.ItemSells.Count >= 20)
                            {
                                character.ItemSells.RemoveAt(0);
                            }
                            BoMongQuest_Handler.gI().PlusSubTask(character, BoMongQuest_Template.TRUM_NHAT_VE_CHAI);
                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                            character.CharacterHandler.SendMessage(Service.BuyItem(character));
                            character.CharacterHandler.SendMessage(Service.ServerMessage(string.Format(TextServer.gI().SELL_ITEM_GOLD, ServerUtils.GetMoneys(gold))));
                            break;
                        }
                }

            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error sell item in ItemHandler.cs: {e.Message} \n {e.StackTrace}", e);
                character.CharacterHandler.SendMessage(Service.ServerMessage("Lỗi rồi, mau báo cáo cho Đại Vương"));
                character.CharacterHandler.SendMessage(Service.BuyItem(character));

                //  UserDB.BanUser(character.Player.Id);
                //ClientManager.Gi().KickSession(character.Player.Session);
                //ServerUtils.WriteLog("hackbando", $"Tên tài khoản {character.Player.Username} (ID:{character.Player.Id}) hack hackbando");

                //var temp = ClientManager.Gi().GetPlayer(character.Player.Id);
                //if (temp != null)
                //{
                //    ClientManager.Gi().KickSession(temp.Session);
                //}
            }
        }

        public static void TradeItem(Message message, Model.Character.Character character)
        {
            try
            {
                var action = message.Reader.ReadByte();
                var map = character.Zone.Map;

                if (map != null)
                {
                    var zone = map.GetZoneById(character.InfoChar.ZoneId);
                    if (zone == null)
                    {
                        if (character.Trade.IsTrade)
                        {
                            var charTemp =
                                (Model.Character.Character)ClientManager.Gi()
                                    .GetCharacter(character.Trade.CharacterId);
                            if (charTemp != null && charTemp.Trade.CharacterId == character.Id)
                            {
                                charTemp.CloseTrade(false);
                            }

                            character.CloseTrade(false);
                        }

                        return;
                    }

                    switch (action)
                    {
                        //send invite trade me to player
                        case 0:
                            {
                                var delayTrade = character.Delay.Trade;
                                var timeServer = ServerUtils.CurrentTimeMillis();
                                if (delayTrade > timeServer)
                                {
                                    var time = (delayTrade - timeServer) / 1000;
                                    character.CharacterHandler.SendMessage(
                                        Service.ServerMessage(string.Format(TextServer.gI().DELAY_TRADE, time)));
                                    return;
                                }

                                if (character.Trade.IsTrade || character.InfoChar.IsDie)
                                {
                                    character.CharacterHandler.SendMessage(
                                        Service.ServerMessage(TextServer.gI().NOT_TRADE_WITH_PLAYER));
                                    return;
                                }

                                var charId = message.Reader.ReadInt();
                                var player = (Model.Character.Character)zone.ZoneHandler.GetCharacter(charId);
                                if (player != null)
                                {
                                    if (player.Trade.IsTrade || player.InfoChar.IsDie)
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service.ServerMessage(TextServer.gI().NOT_TRADE_WITH_PLAYER));
                                        return;
                                    }

                                    player.CharacterHandler.SendMessage(Service.Trade01(0, character.Id));
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(
                                        Service.ServerMessage(TextServer.gI().NOT_FOUND_CHAR_IN_MAP));
                                }

                                break;
                            }
                        //accept trade player to me
                        case 1:
                            {
                                if (character.Trade.IsTrade || character.InfoChar.IsDie)
                                {
                                    character.CharacterHandler.SendMessage(
                                        Service.ServerMessage(TextServer.gI().NOT_TRADE_WITH_PLAYER));
                                    return;
                                }

                                var charId = message.Reader.ReadInt();
                                var player = (Model.Character.Character)zone.ZoneHandler.GetCharacter(charId);
                                if (player != null)
                                {
                                    if (player.Trade.IsTrade || player.InfoChar.IsDie)
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service.ServerMessage(TextServer.gI().NOT_TRADE_WITH_PLAYER));
                                        return;
                                    }

                                    player.CharacterHandler.SendMessage(Service.Trade01(1, character.Id));
                                    character.CharacterHandler.SendMessage(Service.Trade01(1, player.Id));
                                    //setup trade me
                                    character.Trade.CharacterId = player.Id;
                                    character.Trade.IsTrade = true;
                                    character.Trade.IsTrade = true;

                                    //setup trade player
                                    player.Trade.CharacterId = character.Id;
                                    player.Trade.IsTrade = true;
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(
                                        Service.ServerMessage(TextServer.gI().NOT_FOUND_CHAR_IN_MAP));
                                }

                                break;
                            }
                        //Add item to trade
                        case 2:
                            {
                                var index = message.Reader.ReadByte();
                                var quantity = message.Reader.ReadInt();
                                Server.Gi().Logger
                                    .Debug(
                                        $"Client: {character.Player.Session.Id} -------------------- index: {index} - quantity: {quantity}");
                                try
                                {
                                    if (!character.Trade.IsTrade || character.Trade.IsLock) return;
                                    var player =
                                        (Model.Character.Character)zone.ZoneHandler.GetCharacter(character.Trade
                                            .CharacterId);
                                    if (player != null && player.Trade.CharacterId == character.Id)
                                    {
                                        if (index == -1)
                                        {
                                            if (character.InfoChar.Gold < quantity) return;
                                            // Acc thường không giao dịch vàng được quá 100tr 1 lần


                                            var checkItemGold =
                                                character.Trade.Items.FirstOrDefault(item => item.Id == 76);
                                            if (checkItemGold == null)
                                            {
                                                var itemGold = ItemCache.GetItemDefault(76);
                                                itemGold.Quantity = quantity;
                                                itemGold.IndexUI = index;
                                                character.Trade.Items.Add(itemGold);
                                            }
                                            else
                                            {
                                                checkItemGold.Quantity = quantity;
                                            }
                                        }
                                        else
                                        {
                                            var itemTrade = character.CharacterHandler.GetItemBagByIndex(index);
                                            if (itemTrade == null) return;

                                            if (itemTrade.Quantity < quantity)
                                            {
                                                //if (itemTrade.Id == 457)
                                                //{
                                                //    character.CharacterHandler.SendMessage(Service.DialogMessage(TextServer.gI().SPLIT_GOLD_FIRST));
                                                //}
                                                character.CharacterHandler.SendMessage(Service.Trade2(index));
                                                return;
                                            }

                                            var template = ItemCache.ItemTemplate(itemTrade.Id);
                                            //if (DataCache.IsIdItemNotTrade(itemTrade.Id) ||
                                            //    (!DataCache.TypeItemTrade.Contains(template.Type) && !DataCache.ItemPremiumTrade.Contains(itemTrade.Id) && !DataCache.ItemNormalTrade.Contains(itemTrade.Id)))
                                            //{
                                            //    character.CharacterHandler.SendMessage(Service.Trade2(index));
                                            //    character.CharacterHandler.SendMessage(
                                            //        Service.ServerMessage(TextServer.gI().NOT_TRADE_ITEM));
                                            //    return;
                                            //}
                                            // ko có VIP thì ko giao dịch được item có SPL
                                            //if (!character.InfoChar.IsPremium && itemTrade.Options.FirstOrDefault(option => option.Id == 107) != null && Server.Gi().LockCloneGiaoDich)
                                            //{
                                            //    character.CharacterHandler.SendMessage(Service.Trade2(index));
                                            //    character.CharacterHandler.SendMessage(
                                            //        Service.ServerMessage(TextServer.gI().NOT_PREMIUM_TRADE_SPL_ITEM));
                                            //    return;
                                            //}

                                            //if (!player.InfoChar.IsPremium && itemTrade.Options.FirstOrDefault(option => option.Id == 107) != null)
                                            //{
                                            //    character.CharacterHandler.SendMessage(Service.Trade2(index));
                                            //    character.CharacterHandler.SendMessage(
                                            //        Service.ServerMessage(TextServer.gI().PLAYER_NOT_PREMIUM_TRADE_SPL_ITEM));
                                            //    return;
                                            //}

                                            //// ko có VIP thì ko giao dịch được item có cấp bậc
                                            //if (!character.InfoChar.IsPremium && itemTrade.Options.FirstOrDefault(option => option.Id == 72) != null && Server.Gi().LockCloneGiaoDich)
                                            //{
                                            //    character.CharacterHandler.SendMessage(Service.Trade2(index));
                                            //    character.CharacterHandler.SendMessage(
                                            //        Service.ServerMessage(TextServer.gI().NOT_PREMIUM_TRADE_LEVEL_ITEM));
                                            //    return;
                                            //}

                                            //if (!player.InfoChar.IsPremium && itemTrade.Options.FirstOrDefault(option => option.Id == 72) != null)
                                            //{
                                            //    character.CharacterHandler.SendMessage(Service.Trade2(index));
                                            //    character.CharacterHandler.SendMessage(
                                            //        Service.ServerMessage(TextServer.gI().PLAYER_NOT_PREMIUM_TRADE_LEVEL_ITEM));
                                            //    return;
                                            //}

                                            //// ko có VIP thì ko giao dịch được thỏi vàng
                                            //if (!character.InfoChar.IsPremium && DataCache.ItemPremiumTrade.Contains(itemTrade.Id))
                                            //{
                                            //    character.CharacterHandler.SendMessage(Service.Trade2(index));
                                            //    character.CharacterHandler.SendMessage(
                                            //        Service.ServerMessage(String.Format(TextServer.gI().NOT_PREMIUM_TRADE_GOLD_BAR, template.Name)));
                                            //    return;
                                            //}
                                            //if (!player.InfoChar.IsPremium && DataCache.ItemPremiumTrade.Contains(itemTrade.Id))
                                            //{
                                            //    character.CharacterHandler.SendMessage(Service.Trade2(index));
                                            //    character.CharacterHandler.SendMessage(
                                            //        Service.ServerMessage(String.Format(TextServer.gI().PLAYER_NOT_PREMIUM_TRADE_GOLD_BAR, template.Name)));
                                            //    return;
                                            //}

                                            var itemOptionNotTrade = itemTrade.Options.FirstOrDefault(option => option.Id == 30);
                                            if (itemOptionNotTrade != null)
                                            {
                                                character.CharacterHandler.SendMessage(Service.Trade2(index));
                                                character.CharacterHandler.SendMessage(
                                                    Service.ServerMessage(TextServer.gI().NOT_TRADE_ITEM));
                                                return;
                                            }

                                            //var itemSKH = itemTrade.Options.FirstOrDefault(option => option.Id >= 127 && option.Id <= 135);
                                            //if (itemSKH != null)
                                            //{
                                            //    character.CharacterHandler.SendMessage(Service.Trade2(index));
                                            //    character.CharacterHandler.SendMessage(
                                            //        Service.ServerMessage(TextServer.gI().NOT_TRADE_ITEM));
                                            //    return;
                                            //}

                                            var checkItemCheck = character.Trade.Items.FirstOrDefault(item =>
                                            item.Id == itemTrade.Id && item.IndexUI == itemTrade.IndexUI);
                                            if (checkItemCheck == null)
                                            {
                                                var itemClone = ItemHandler.Clone(itemTrade);
                                                if (quantity == 0)
                                                {
                                                    quantity = 1;
                                                }
                                                itemClone.Quantity = quantity;
                                                itemClone.IndexUI = index;
                                                character.Trade.Items.Add(itemClone);
                                            }
                                            else
                                            {
                                                checkItemCheck.Quantity = quantity;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service.ServerMessage(TextServer.gI().NOT_FOUND_CHAR_IN_MAP));
                                        character.CloseTrade(true);
                                    }
                                }
                                catch (Exception)
                                {
                                    var charTemp =
                                        (Model.Character.Character)zone.ZoneHandler.GetCharacter(character.Trade
                                            .CharacterId);
                                    if (charTemp != null && charTemp.Trade.CharacterId == character.Id)
                                    {
                                        charTemp.CloseTrade(true);
                                    }
                                    character.CloseTrade(true);
                                }

                                break;
                            }
                        //Huỷ giao dịch
                        case 3:
                            {
                                if (!character.Trade.IsTrade) return;
                                var player =
                                    (Model.Character.Character)zone.ZoneHandler.GetCharacter(character.Trade.CharacterId);
                                if (player != null && player.Trade.CharacterId == character.Id)
                                {
                                    player.CloseTrade(true);
                                    player.CharacterHandler.SendMessage(
                                        Service.ServerMessage(TextServer.gI().CLOSE_TRADE));
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(
                                        Service.ServerMessage(TextServer.gI().NOT_FOUND_CHAR_IN_MAP));
                                }

                                character.CloseTrade(true);
                                break;
                            }
                        //Khoá giao dịch
                        case 5:
                            {
                                try
                                {
                                    if (!character.Trade.IsTrade) return;
                                    character.Trade.IsLock = true;
                                    var player =
                                        (Model.Character.Character)zone.ZoneHandler.GetCharacter(character.Trade
                                            .CharacterId);
                                    if (player != null && player.Trade.CharacterId == character.Id)
                                    {
                                        player.CharacterHandler.SendMessage(Service.Trade6(int.Parse(character.Player.Session.Version.Replace(".", "")), character.Trade.Items));
                                    }
                                    else
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service.ServerMessage(TextServer.gI().NOT_FOUND_CHAR_IN_MAP));
                                        character.CloseTrade(true);
                                    }
                                }
                                catch (Exception)
                                {
                                    var charTemp =
                                        (Model.Character.Character)zone.ZoneHandler.GetCharacter(character.Trade
                                            .CharacterId);
                                    if (charTemp != null && charTemp.Trade.CharacterId == character.Id)
                                    {
                                        charTemp.CloseTrade(true);
                                    }

                                    character.CloseTrade(true);
                                }

                                break;
                            }
                        //Hoàn thành giao dịch
                        case 7:
                            {
                                try
                                {
                                    var logPlayer = "";
                                    var logCharacter = "";
                                    var tongGiaoDichThoiVang = 0;
                                    if (!character.Trade.IsTrade) return;
                                    character.Trade.IsHold = true;
                                    var player =
                                        (Model.Character.Character)zone.ZoneHandler.GetCharacter(character.Trade
                                            .CharacterId);
                                    if (player != null && player.Trade.CharacterId == character.Id)
                                    {
                                        if (player.Trade.IsHold)
                                        {
                                            var itemGoldMe = character.Trade.Items.FirstOrDefault(item => item.IndexUI == -1);
                                            var itemGoldPlayer = player.Trade.Items.FirstOrDefault(item => item.IndexUI == -1);
                                            var goldMe = 0;
                                            var goldPlayer = 0;
                                            if (itemGoldMe != null)
                                            {
                                                goldMe = itemGoldMe.Quantity;
                                            }
                                            if (itemGoldPlayer != null)
                                            {
                                                goldPlayer = itemGoldPlayer.Quantity;
                                            }

                                            var listItemMe = character.Trade.Items.Where(item => item.IndexUI != -1).ToList();
                                            var listItemPlayer = player.Trade.Items.Where(item => item.IndexUI != -1).ToList();

                                            if (listItemMe.Count > player.LengthBagNull())
                                            {
                                                player.CharacterHandler.SendMessage(
                                                    Service.ServerMessage(TextServer.gI().NOT_ENOUGH_BAG));
                                                character.CharacterHandler.SendMessage(
                                                    Service.ServerMessage(TextServer.gI().TRADE_ERROR));
                                                player.CloseTrade(false);
                                                character.CloseTrade(true);
                                                return;
                                            }

                                            if (listItemPlayer.Count > character.LengthBagNull())
                                            {
                                                character.CharacterHandler.SendMessage(
                                                    Service.ServerMessage(TextServer.gI().NOT_ENOUGH_BAG));
                                                player.CharacterHandler.SendMessage(
                                                    Service.ServerMessage(TextServer.gI().TRADE_ERROR));
                                                player.CloseTrade(false);
                                                character.CloseTrade(true);
                                                return;
                                            }

                                            if (goldMe > character.InfoChar.Gold)
                                            {
                                                character.CharacterHandler.SendMessage(
                                                    Service.ServerMessage(TextServer.gI().NOT_ENOUGH_GOLD));
                                                player.CharacterHandler.SendMessage(
                                                    Service.ServerMessage(TextServer.gI().TRADE_ERROR));
                                                player.CloseTrade(false);
                                                character.CloseTrade(true);
                                                return;
                                            }

                                            if (goldPlayer > player.InfoChar.Gold)
                                            {
                                                player.CharacterHandler.SendMessage(
                                                    Service.ServerMessage(TextServer.gI().NOT_ENOUGH_GOLD));
                                                character.CharacterHandler.SendMessage(
                                                    Service.ServerMessage(TextServer.gI().TRADE_ERROR));
                                                player.CloseTrade(false);
                                                character.CloseTrade(true);
                                                return;
                                            }

                                            //Check error item trade

                                            #region Check error item trade
                                            var itemCheck = listItemMe.FirstOrDefault(item =>
                                            {
                                                var itemBag = character.CharacterHandler.GetItemBagByIndex(item.IndexUI);
                                                return itemBag.Id != item.Id || itemBag.Quantity < item.Quantity;
                                            });
                                            if (itemCheck != null)
                                            {
                                                character.CharacterHandler.SendMessage(
                                                    Service.ServerMessage(TextServer.gI().TRADE_ERROR));
                                                player.CharacterHandler.SendMessage(
                                                    Service.ServerMessage(TextServer.gI().TRADE_ERROR));
                                                player.CloseTrade(false);
                                                character.CloseTrade(true);
                                                return;
                                            }

                                            itemCheck = listItemPlayer.FirstOrDefault(item =>
                                            {
                                                var itemBag = player.CharacterHandler.GetItemBagByIndex(item.IndexUI);
                                                return itemBag.Id != item.Id || itemBag.Quantity < item.Quantity;
                                            });
                                            if (itemCheck != null)
                                            {
                                                character.CharacterHandler.SendMessage(
                                                    Service.ServerMessage(TextServer.gI().TRADE_ERROR));
                                                player.CharacterHandler.SendMessage(
                                                    Service.ServerMessage(TextServer.gI().TRADE_ERROR));
                                                player.CloseTrade(false);
                                                character.CloseTrade(true);
                                                return;
                                            }
                                            #endregion

                                            logPlayer = $"{player.Name} đã giao dịch với {character.Name}: ";
                                            logCharacter = $"{character.Name} đã giao dịch với {player.Name}: ";
                                            //Remove item
                                            listItemMe.ForEach(item =>
                                            {
                                                if (item == listItemMe.LastOrDefault())
                                                {
                                                    if (item != null)
                                                    {
                                                        var itemTemplate = ItemCache.ItemTemplate(item.Id);
                                                        logCharacter += $"cho: ({item.IndexUI}){item.Quantity}x{itemTemplate.Name},";
                                                        character.CharacterHandler.RemoveItemBagByIndex(item.IndexUI,
                                                            item.Quantity, reason: "Giao dịch với " + player.Name);
                                                    }
                                                }
                                                else
                                                {
                                                    var itemTemplate = ItemCache.ItemTemplate(item.Id);
                                                    logCharacter += $"cho: ({item.IndexUI}){item.Quantity}x{itemTemplate.Name},";
                                                    character.CharacterHandler.RemoveItemBagByIndex(item.IndexUI,
                                                        item.Quantity, reset: false, reason: "Giao dịch với " + player.Name);
                                                }
                                            });

                                            listItemPlayer.ForEach(item =>
                                            {
                                                if (item == listItemPlayer.LastOrDefault())
                                                {
                                                    if (item != null)
                                                    {
                                                        var itemTemplate = ItemCache.ItemTemplate(item.Id);
                                                        logPlayer += $"cho: ({item.IndexUI}){item.Quantity}x{itemTemplate.Name},";
                                                        player.CharacterHandler.RemoveItemBagByIndex(item.IndexUI,
                                                            item.Quantity, reason: "Giao dịch với " + character.Name);
                                                    }
                                                }
                                                else
                                                {
                                                    var itemTemplate = ItemCache.ItemTemplate(item.Id);
                                                    logPlayer += $"cho: ({item.IndexUI}){item.Quantity}x{itemTemplate.Name},";
                                                    player.CharacterHandler.RemoveItemBagByIndex(item.IndexUI,
                                                        item.Quantity, reset: false, reason: "Giao dịch với " + character.Name);
                                                }
                                            });

                                            //Add item
                                            listItemPlayer.ForEach(item =>
                                            {
                                                if (item.Id == 457)
                                                {
                                                    tongGiaoDichThoiVang += item.Quantity;
                                                }
                                                var itemTemplate = ItemCache.ItemTemplate(item.Id);
                                                logCharacter += $"nhận: {item.Quantity}x{itemTemplate.Name},";
                                                character.CharacterHandler.AddItemToBag(false, item, "(GD) Nhận từ " + player.Name);
                                            });

                                            listItemMe.ForEach(item =>
                                            {
                                                if (item.Id == 457)
                                                {
                                                    tongGiaoDichThoiVang += item.Quantity;
                                                }
                                                var itemTemplate = ItemCache.ItemTemplate(item.Id);
                                                logPlayer += $"nhận: {item.Quantity}x{itemTemplate.Name},";
                                                player.CharacterHandler.AddItemToBag(false, item, "(GD) Nhận từ " + character.Name);
                                            });

                                            if (goldMe > 0)
                                            {
                                                character.MineGold(goldMe);
                                                player.PlusGold(goldMe);
                                                logPlayer += $"+G: {goldMe},";
                                                logCharacter += $"-G: {goldMe},";
                                                player.CharacterHandler.SendMessage(Service.BuyItem(player));
                                                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                            }

                                            if (goldPlayer > 0)
                                            {
                                                player.MineGold(goldPlayer);
                                                character.PlusGold(goldPlayer);
                                                logPlayer += $"-G: {goldPlayer},";
                                                logCharacter += $"+G: {goldPlayer},";
                                                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                                player.CharacterHandler.SendMessage(Service.BuyItem(player));
                                            }
                                            var timeServer = ServerUtils.CurrentTimeMillis();

                                            //if (!character.Delay.IsSavingInventory)
                                            //{
                                            //    character.Delay.IsSavingInventory = true;
                                            //    character.Delay.SaveInvData = 10000 + timeServer;
                                            //    character.Delay.InvAction = timeServer + 15000;
                                            //    if (CharacterDB.SaveInventory(character, true, true, true))
                                            //    {
                                            //        character.Delay.InvAction = timeServer;
                                            //    }
                                            //    character.Delay.IsSavingInventory = false;
                                            //}

                                            //if (!player.Delay.IsSavingInventory)
                                            //{
                                            //    player.Delay.IsSavingInventory = true;
                                            //    player.Delay.SaveInvData = 10000 + timeServer;
                                            //    player.Delay.InvAction = timeServer + 15000;
                                            //    if (CharacterDB.SaveInventory(player, true, true, true))
                                            //    {
                                            //        player.Delay.InvAction = timeServer;
                                            //    }
                                            //    player.Delay.IsSavingInventory = false;
                                            //}


                                            character.InfoChar.ThoiGianGiaoDich = timeServer + DataCache.LIMIT_NOT_PREMIUM_TRADE_TIME;
                                            player.InfoChar.ThoiGianGiaoDich = timeServer + DataCache.LIMIT_NOT_PREMIUM_TRADE_TIME;
                                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                                            player.CharacterHandler.SendMessage(Service.SendBag(player));
                                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().TRADE_SUCCESS));
                                            player.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().TRADE_SUCCESS));
                                            character.CloseTrade(true);
                                            player.CloseTrade(false);
                                            // CharacterDB.Update(player);
                                            // CharacterDB.Update(character);
                                            ServerUtils.WriteTradeLog(logPlayer, tongGiaoDichThoiVang);
                                            ServerUtils.WriteTradeLog(logCharacter, tongGiaoDichThoiVang);
                                        }
                                        else
                                        {
                                            character.CharacterHandler.SendMessage(
                                                Service.ServerMessage(TextServer.gI().TRADE_HOLD));
                                        }
                                    }
                                    else
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service.ServerMessage(TextServer.gI().TRADE_ERROR));
                                        character.CloseTrade(true);
                                    }
                                }
                                catch (Exception)
                                {
                                    var charTemp =
                                        (Model.Character.Character)zone.ZoneHandler.GetCharacter(character.Trade
                                            .CharacterId);
                                    if (charTemp != null && charTemp.Trade.CharacterId == character.Id)
                                    {
                                        charTemp.CloseTrade(true);
                                    }

                                    character.CloseTrade(true);
                                }

                                break;
                            }
                    }
                }
                else
                {
                    character.CharacterHandler.SendMessage(
                        Service.ServerMessage(TextServer.gI().NOT_FOUND_CHAR_IN_MAP));
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error Trade Item in itemHandler.cs: {e.Message} \n {e.StackTrace}", e);
            }
            finally
            {
                message?.CleanUp();
            }
        }

        public static void ConfirmUseItemBag(Model.Character.Character character, int index, short template = -1)
        {
            Model.Item.Item itemUse;
            if (template != -1 && DataCache.IdDauThan.Contains(template))
            {
                itemUse = character.ItemBag.FirstOrDefault(item => item.Id == template);
                if (itemUse != null) UsePea(character, itemUse, 0);
                return;
            }

            itemUse = character.CharacterHandler.GetItemBagByIndex(index);
            if (itemUse == null) return;

            var itemTemplate = ItemCache.ItemTemplate(itemUse.Id);
            if (itemTemplate.Require > character.InfoChar.Power)
            {
                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_POWER));
                return;
            }

            if (itemTemplate.Gender != 3 && itemTemplate.Gender != character.InfoChar.Gender)
            {
                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_GENDER));
                return;
            }

            if (itemTemplate.Type == 22)
            {
                UseAuraItem(character, itemUse);
                return;
            }
        }
        
        public static void UseItemBag(Model.Character.Character character, int index, short template = -1)
        {
            try
            {
                Model.Item.Item itemUse;
                if (template != -1 && DataCache.IdDauThan.Contains(template))
                {
                    itemUse = character.ItemBag.FirstOrDefault(item => item.Id == template);
                    if (itemUse != null) UsePea(character, itemUse, 0);
                    return;
                }

                itemUse = character.CharacterHandler.GetItemBagByIndex(index);
                if (itemUse == null) return;

                var itemTemplate = ItemCache.ItemTemplate(itemUse.Id);
                if (itemTemplate.Require > character.InfoChar.Power)
                {
                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_POWER));
                    return;
                }

                if (itemTemplate.Gender != 3 && itemTemplate.Gender != character.InfoChar.Gender)
                {
                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_GENDER));
                    return;
                }

                var yeuCauSucManhTi = itemUse.Options.FirstOrDefault(opt => opt.Id == 21);
                if (yeuCauSucManhTi != null)
                {
                    if ((long)((long)yeuCauSucManhTi.Param * 1000000000) > character.InfoChar.Power)
                    {
                        character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_POWER));
                        return;
                    }
                }

                var forDisciple = DataCache.listAvatarForDisciple.Contains(itemUse.Id);
                if (forDisciple)
                {
                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().ONLY_DISCIPLE));

                    return;
                }

                //Use Card


                if (itemTemplate.IsTypeBody() || DataCache.ListPetID.Contains(itemTemplate.Id))
                {
                    // Nếu mặc vào giáp luyện tập sẽ xóa bỏ mọi hiệu ứng cộng thêm dmg
                    if (ItemCache.ItemIsGiapLuyenTap(itemUse.Id))
                    {
                        character.InfoMore.LastGiapLuyenTapItemId = 0;
                        character.Delay.GiapLuyenTap = -1;
                    }

                   
                    var indexInBody = itemTemplate.Type;
                    switch (itemTemplate.Type)
                    {
                        case 32:
                            indexInBody = 6;
                            break;
                        case 11:
                            indexInBody = 7;
                            break;
                        case 23:
                            indexInBody = 8;
                            break;
                        case 27:
                            indexInBody = 9;
                            break;
                        case 38:
                            indexInBody = 10;
                            break;
                        case 37 :
                            indexInBody = 11;
                            break;

                    }
                    var itemBody = character.ItemBody[indexInBody];
                    var itemClone = Clone(itemUse);
                    itemClone.IndexUI = indexInBody;
                    character.ItemBody[indexInBody] = itemClone;
                    if (itemBody != null)
                    {

                        itemBody.IndexUI = index;
                        character.ItemBag[index] = itemBody;
                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                    }
                    else
                    {
                        character.CharacterHandler.RemoveItemBag(index, reason: "Mặc vào người");
                    }
                    var timeServer = ServerUtils.CurrentTimeMillis();
                    character.CharacterHandler.UpdateInfo(true);
                    switch (itemTemplate.Type)
                    {
                        case 5:
                            character.CharacterHandler.UpdateMask();
                            break;
                        case 11:
                            character.CharacterHandler.UpdatePhukien();
                            break;
                        case 27:
                            character.CharacterHandler.UpdatePet();
                            character.CharacterHandler.UpdateEffective();
                            break;
                        case 23:
                            character.CharacterHandler.UpdateMountId();
                            break;
                        case 38:
                            character.CharacterHandler.UpdateItem10();
                            break;
                        default:
                            character.CharacterHandler.UpdateEffectCharacter();
                            break;

                    }


                    return;
                }



                switch (itemTemplate.Type)
                {
                    case 7:
                        if (UseBook(character, itemUse))
                        {
                            character.CharacterHandler.RemoveItemBag(index, reason: "Sách kĩ năng");
                            return;
                        }
                        break;
                    case 12:
                        switch (itemTemplate.Id)
                        {
                            case <= 20:
                                UseDragonBall(character, itemUse);
                                return;
                            case > 807 and <= 813:
                                BoneDragon.gI().Mở_Menu(character);

                                return;
                            case >= 925 and <= 931:
                                IceDragon.gI().OpenMenuWish(character);
                                return;

                        }
                        break;
                    case 33:
                        {
                            UseCard(character, itemUse);
                            return;
                        }
                    case 22:
                        {
                            character.CharacterHandler.SendMessage(Service.UseItem(1, 3, index, String.Format(TextServer.gI().CONFIRM_USE_ITEM, itemTemplate.Name)));
                            return;
                        }
                    case 36:
                        {
                            GameCache.gI().HandleAddBanner(character, itemTemplate.Name);
                            character.CharacterHandler.RemoveItemBagByIndex(itemUse.IndexUI, 1);
                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                            return;
                        }
                }

                switch (itemTemplate.Id)
                {
                    case 1589:
                        character.InfoChar.X = 371;
                        character.InfoChar.Y = 240; 
                        MapManager.JoinMap(character, 166, ServerUtils.RandomNumber(0, 19), false, false, 0);
                        character.CharacterHandler.RemoveItemBagByIndex(itemUse.IndexUI, 1);
                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                        break;
                    case 1579:
                        var randomSkill = DataCache.IdSkillDisciple3[ServerUtils.RandomNumber(DataCache.IdSkillDisciple3.Count)];
                        if (character.Disciple.Skills.Count >= 3)
                            character.Disciple.Skills.Add(new SkillCharacter()
                            {
                                Id = randomSkill,
                                SkillId = Disciple.GetSkillId(randomSkill),
                                Point = 1,
                            });
                        character.CharacterHandler.RemoveItemBagByIndex(itemUse.IndexUI, 1);
                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                        break;
                    case 1580:
                        if (character.Disciple.Skills.Count >= 4)
                        {
                            randomSkill = DataCache.IdSkillDisciple4[ServerUtils.RandomNumber(DataCache.IdSkillDisciple4.Count)];
                            character.Disciple.Skills.Add(new SkillCharacter()
                            {
                                Id = randomSkill,
                                SkillId = Disciple.GetSkillId(randomSkill),
                                Point = 1,
                            });
                        }
                        character.CharacterHandler.RemoveItemBagByIndex(itemUse.IndexUI, 1);
                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                        break;
                    case 1555:
                    case 1556:
                    case 1557:
                        SuKien8Thang3.OpenMenuGoiHopQua(character);
                        break;
                    case 758:
                        UseCapsuleTet2024(character, itemUse);
                        break;
                    case 1187:
                        UseHopQuaTet2024(character, itemUse);
                        break;
                    case 1538:
                        UsePhongBiTet2024(character, itemUse);
                        break;
                    case 1541:
                        UseThiepChucMungLongChau(character, itemUse);
                        break;
                    case 1184:
                        UseGoiQuaDacBiet2024(character, itemUse);
                        break;
                    case 648:
                        UseHopQuaGiangSinh(character, itemUse);
                        break;
                    case 1481:
                        UseRuongSaoPhaLeCap2(character, itemUse);
                        break;
                    case 1512:
                        GameCache.gI().HandleAddBanner(character, "Ai mà xinh thế");
                        break;
                        case 1321:
                        GameCache.gI().HandleAddBanner(character, "Kẻ thao túng sói");
                        break;
                    case 456:
                        ItemBossHandler.UseBinhNuocXinbato(character, itemUse);
                        break;
                    case 1006:
                        DoiThuongHandler.Reward(character, DoiThuongCache.RewardDoiCa);
                        character.CharacterHandler.RemoveItemBagById(1006, 1);
                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                        break;
                    case 1338:
                        //for (int i = 0; i < DataCache.SetDoTanThu[character.InfoChar.Gender].Count; i++)
                        //{
                        //    var temp = DataCache.SetDoTanThu[character.InfoChar.Gender][i];
                        //    var item = ItemCache.GetItemDefault(temp);
                        //    item.Options.Add(new OptionItem() { Id = 107, Param = 3 });
                        //    item.Options.Add(new OptionItem() { Id = 30, Param = 0 });
                        //    character.CharacterHandler.AddItemToBag(false, item);
                        //}
                        for (int i =0; i < 5; i++)
                        {
                            var item = ItemCache.GetItemDefault((short)(381 + i), 10);
                            character.CharacterHandler.AddItemToBag(true, item);
                        }
                        character.CharacterHandler.RemoveItemBagById(1338, 1);
                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                        character.CharacterHandler.SendMessage(Service.ServerMessage("Mở hộp thành công, hãy mở hành trang để kiểm tra"));
                        break;
                    case 1036://phao hoa
                        var inputPhaoHoa = new List<InputBox>();
                        var inputSoluong = new InputBox()
                        {
                            Name = "Nhập số pháo hoa muốn sử dụng",
                            Type = 1,
                        };
                        inputPhaoHoa.Add(inputSoluong);
                        character.CharacterHandler.SendMessage(Service.ShowInput("Sử dụng pháo hoa", inputPhaoHoa));
                        character.TypeInput = 115;
                        break;
                    case 1290:
                        if (character.LengthBagNull() < 5)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Yêu cầu hành trang còn 5 ô trống"));
                            return;
                        }
                        var menus = new List<String>();
                        switch (character.InfoChar.Gender)
                        {
                            case 0:
                                menus = new List<string>() { "Songoku", "Kirin", "Thiên xin\nhăng" };
                                break;
                            case 1: 
                                menus = new List<string>() { "Picolo", "Ốc tiêu", "Pikkoro\nDaimao" };
                                break;
                            case 2:
                                menus = new List<string>() { "Kakarot", "Ca Đíc", "Nappa" };
                                break;

                        }
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, "Lựa chọn tác dụng của set kích hoạt", menus, character.InfoChar.Gender));
                        character.TypeMenu = 28;
                        break;
                    case 1269:
                        if (character.LengthBagNull() < 5)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Yêu cầu hành trang còn 5 ô trống"));
                            return;
                        }
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, "Lựa chọn hành tinh của Set Thần Linh", new List<string> { "Trái đất", "Namec", "Xayda", "Đóng" }, character.InfoChar.Gender));
                        character.TypeMenu = 13;
                        break;
                    case 1268:
                        {
                            var listItem = new List<int> { 1205, 730 };
                            var item = ItemCache.GetItemDefault((short)(listItem[ServerUtils.RandomNumber(listItem.Count)]));
                            character.CharacterHandler.RemoveItemBagByIndex(index, 1);
                            character.CharacterHandler.AddItemToBag(false, item);
                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Chúc mừng bạn đã nhận được " + ItemCache.ItemTemplate(item.Id).Name));

                        }

                        break;
                    case 1237:
                        {
                            List<int> LinhThuId = new List<int> { 1208, 1209, 1210, 1211, 1212 };
                            var item = ItemCache.GetItemDefault((short)(LinhThuId[ServerUtils.RandomNumber(LinhThuId.Count)]));
                            item.Options.FirstOrDefault(i => i.Id == 50).Param = ServerUtils.RandomNumber(1, 7);
                            item.Options.FirstOrDefault(i => i.Id == 77).Param = ServerUtils.RandomNumber(1, 7);
                            item.Options.FirstOrDefault(i => i.Id == 103).Param = ServerUtils.RandomNumber(1, 7);
                            item.Options.Add(new OptionItem()
                            {
                                Id = 210,
                                Param = 1,
                            });
                            character.CharacterHandler.AddItemToBag(false, item);
                            character.CharacterHandler.RemoveItemBagByIndex(index, 1);
                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                            character.CharacterHandler.SendMessage(Service.SendCombinne4(itemTemplate.IconId));
                            break;
                        }
                    case 988:
                        {
                            character.CharacterHandler.RemoveItemBagByIndex(itemUse.IndexUI, 1);
                            character.PlusLimitGold(200000000);
                            character.CharacterHandler.SendMessage(Service.BuyItem(character));
                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                            character.CharacterHandler.SendMessage(Service.ServerMessage(string.Format(TextServer.gI().UPGRADE_LIMIT_GOLD, ServerUtils.GetMoneys(character.InfoChar.LimitGold))));
                            break;
                        }
                    // Gold Bar
                    //case 457:

                    //    character.PlusGold(500000000);
                    //    character.CharacterHandler.RemoveItemBagById(457, 1);
                    //    character.CharacterHandler.SendMessage(Service.SendBag(character));
                    //    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                    //        break;

                    case 570:
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Vật phẩm sẽ được mở khóa khi Open"));
                            //var hn = 10 * itemUse.Options.FirstOrDefault(i => i.Id == 72).Param;
                            //// Application.Extension.ChampionShip.ChampionShip_23.WoodChest.gI().Open(character, itemUse.Options.FirstOrDefault(i => i.Id == 72).Param);
                            //character.CharacterHandler.PlusDiamondLock(hn);
                            //character.CharacterHandler.SendMessage(Service.BuyItem(character));
                            //character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + hn+" hồng ngọc"));
                            break;
                        }
                    case 460:
                        if (character.Zone.ZoneHandler.GetBossInMap(85).Count >= 1)
                        {
                            character.CharacterHandler.RemoveItemBagById(itemUse.Id, 1);
                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                            var itemmap = new ItemMap(-1, itemUse);
                            itemmap.X = character.InfoChar.X;
                            itemmap.Y = character.InfoChar.Y;
                            character.Zone.ZoneHandler.LeaveItemMap(itemmap);
                            if (character.Zone.ZoneHandler.GetBoss(85) != null)
                            {
                                var NoONo = character.Zone.ZoneHandler.GetBoss(85);
                                NoONo.CharacterHandler.SendZoneMessage(Service.PublicChat(NoONo.Id, "Ế! miếng xương ngon quáa !"));
                                NoONo.InfoChar.X = itemmap.X;
                                NoONo.InfoChar.Y = itemmap.Y;
                                NoONo.CharacterHandler.SendZoneMessage(Service.PlayerMove(NoONo.Id, NoONo.InfoChar.X, NoONo.InfoChar.Y));
                                async void LumCucXuong()
                                {
                                    await System.Threading.Tasks.Task.Delay(2000);
                                    NoONo.CharacterHandler.SendZoneMessage(Service.ItemMapMePick(itemUse.Id, 1, "a"));
                                    NoONo.Zone.ZoneHandler.RemoveBoss((Boss)NoONo);
                                }
                                var task = new System.Threading.Tasks.Task(LumCucXuong);
                                task.Start();
                            }
                        }
                        break;
                    //Dau than
                    case 13:
                    case 60:
                    case 61:
                    case 62:
                    case 63:
                    case 64:
                    case 65:
                    case 352:
                    case 523:
                    case 595:
                        {
                            UsePea(character, itemUse, 0);
                            return;
                        }
                    //Capsule
                    case 193:
                    case 194:
                        {
                            if (character.DataNgocRongNamek.AlreadyPick(character))
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Không thể thực hiện lúc này."));
                                return;
                            }
                            character.CharacterHandler.SendMessage(Service.MapTranspot(character.MapTranspots));
                            character.ItemCapsuleId = itemUse.Id;
                            return;
                        }
                    case 361:
                        character.CharacterHandler.RemoveItemBagByIndex(itemUse.IndexUI, 1, reason: "Dùng may do ngoc rong namek");
                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                        var text = "";
                        for (int i = 0; i < Init.NamecBalls.Count; i++)
                        {
                            text += $"{i} Sao: {Init.NamecBalls[i].MapName}{(Init.NamecBalls[i].PlayerPick == -1 ? "" : "(" + ClientManager.Gi().GetCharacter(Init.NamecBalls[i].PlayerPick).Name + ")")}\n";
                            //text += $"{i} Sao: ";
                        }
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(64, text, new List<string> { "Đến ngay\nViên 1 sao\n10 ngọc", "Đến ngay\nViên 1 sao\n100k vàng", "Kết thúc" }, character.InfoChar.Gender));
                        character.TypeMenu = 6;
                        break;
                    //Đổi đệ tử
                    case 401:
                        {
                            var inputChangePet = new List<InputBox>();

                            var inputOk = new InputBox()
                            {
                                Name = "Ghi 'OK' hoặc 'ok'",
                                Type = 1,
                            };
                            inputChangePet.Add(inputOk);


                            character.CharacterHandler.SendMessage(Service.ShowInput("Bạn có chắc muốn đổi đệ tử ?\nLưu ý: vật phẩm đệ tử hiện tại đang mặc sẽ bị mất nếu đổi.\nNhập OK để xác nhận:", inputChangePet));
                            character.TypeInput = 15;
                            //var disciple = character.Disciple;
                            //if (disciple == null)
                            //{
                            //    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().DONT_FIND_DISCIPLE));
                            //    return;
                            //}

                            //var itemDiscipleBody = disciple.ItemBody.FirstOrDefault(item => item != null);

                            //if (itemDiscipleBody != null)
                            //{
                            //    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().PLEASE_EMPTY_DISCIPLE_BODY));
                            //    return;
                            //}

                            //var oldStatus = disciple.Status;

                            //if (oldStatus < 3)
                            //{
                            //    character.Zone.ZoneHandler.RemoveDisciple(character.Disciple);
                            //}

                            //disciple = new Disciple();
                            //disciple.CreateNewDisciple(character);
                            //disciple.Player = character.Player;
                            //disciple.Zone = character.Zone;
                            //disciple.CharacterHandler.SetUpInfo();
                            //character.Disciple = disciple;

                            //if (!character.InfoChar.Fusion.IsFusion && oldStatus < 3)
                            //{
                            //    character.Zone.ZoneHandler.AddDisciple(disciple);
                            //}
                            //else
                            //{
                            //    character.CharacterHandler.SetUpInfo();
                            //    character.CharacterHandler.SendMessage(Service.MeLoadPoint(character));
                            //    character.CharacterHandler.SendMessage(Service.SendHp((int)character.InfoChar.Hp));
                            //    character.CharacterHandler.SendMessage(Service.SendMp((int)character.InfoChar.Mp));
                            //    character.CharacterHandler.SendZoneMessage(Service.PlayerLevel(character));
                            //}
                            //character.CharacterHandler.RemoveItemBagByIndex(itemUse.IndexUI, 1, reason:"Dùng đổi đệ tử");
                            //character.CharacterHandler.SendMessage(Service.SendBag(character));
                            //DiscipleDB.Update(disciple);
                            //break;
                            break;
                        }
                    //Nâng kỹ năng đệ tử 1
                    case 402:
                    case 403:
                    case 404:
                    case 759:
                        {
                            var disciple = character.Disciple;
                            if (disciple == null)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().DONT_FIND_DISCIPLE));
                                return;
                            }
                            var skill1 = disciple.Skills[0];
                            switch (itemUse.Id)
                            {
                                case 403:
                                    {
                                        if (disciple.Skills.Count < 2)
                                        {
                                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_FOUND_SKILL_DISCIPLE));
                                            return;
                                        }

                                        skill1 = disciple.Skills[1];
                                        break;
                                    }
                                case 404:
                                    {
                                        if (disciple.Skills.Count < 3)
                                        {
                                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_FOUND_SKILL_DISCIPLE));
                                            return;
                                        }
                                        skill1 = disciple.Skills[2];
                                        break;
                                    }
                                case 759:
                                    {
                                        if (disciple.Skills.Count < 4)
                                        {
                                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_FOUND_SKILL_DISCIPLE));
                                            return;
                                        }
                                        skill1 = disciple.Skills[3];
                                        break;
                                    }
                            }

                            if (skill1.Point >= 7)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().MAX_POINT_SKILL_DISCIPLE));
                                return;
                            }
                            skill1.Point++;
                            skill1.CoolDown = -1;
                            skill1.SkillId++;
                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().SUCCESS_POINT_SKILL_DISCIPLE));
                            character.CharacterHandler.SendMessage(Service.ClosePanel());
                            // character.CharacterHandler.RemoveItemBag(index);
                            character.CharacterHandler.RemoveItemBagByIndex(itemUse.IndexUI, 1, reason: "Dùng sách kĩ năng đệ tử");
                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                            break;
                        }
                    //Bông tai
                    case 454:
                        {
                            var disciple = character.Disciple;
                            if (disciple == null)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().DONT_FIND_DISCIPLE));
                                return;
                            }

                           


                            var timeServer = ServerUtils.CurrentTimeMillis();

                            if (character.InfoChar.Fusion.IsFusion)
                            {
                                if (character.InfoChar.Fusion.IsPorata2)
                                {
                                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().PLEASE_USE_PORATA_2));
                                    // Hợp thể cấp 2 không thể mở bằng item cấp 1
                                    return;
                                }
                                disciple.CharacterHandler.SetUpPosition(isRandom: true);
                                character.Zone.ZoneHandler.AddDisciple(disciple);
                                character.CharacterHandler.SendZoneMessage(Service.Fusion(character.Id, 1));
                                lock (character.InfoChar.Fusion)
                                {
                                    character.InfoChar.Fusion.IsFusion = false;
                                    character.InfoChar.Fusion.IsPorata = false;
                                    character.InfoChar.Fusion.TimeStart = timeServer;
                                    character.InfoChar.Fusion.DelayFusion = timeServer + 600000;
                                    character.InfoChar.Fusion.TimeUse = 0;
                                }

                                disciple.Status = 1;
                                character.Delay.BongTaiPorata = 10000 + timeServer;
                            }
                            else
                            {
                                if (disciple.InfoChar.IsDie) //disciple.Status >= 3 || 
                                {
                                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().CAN_NOT_USE_FUSION));
                                    return;
                                }

                                if (character.Delay.BongTaiPorata > timeServer)
                                {
                                    var delay = (character.Delay.BongTaiPorata - timeServer) / 1000;
                                    if (delay < 1)
                                    {
                                        delay = 1;
                                    }

                                    character.CharacterHandler.SendMessage(
                                        Service.ServerMessage(string.Format(TextServer.gI().DELAY_FUSION_SEC,
                                            delay)));
                                    return;
                                }

                                if (disciple.InfoSkill.HuytSao.IsHuytSao)
                                {
                                    SkillHandler.RemoveHuytSao(disciple);
                                }

                                if (disciple.InfoSkill.Monkey.MonkeyId == 1)
                                {
                                    SkillHandler.HandleMonkey(disciple, false);
                                }

                                if (disciple.Status < 3 && disciple.Zone != null && disciple.Zone.ZoneHandler != null)
                                {
                                    disciple.Zone.ZoneHandler.RemoveDisciple(disciple);
                                }

                                character.CharacterHandler.SendZoneMessage(Service.Fusion(character.Id, 6));
                                lock (character.InfoChar.Fusion)
                                {
                                    character.InfoChar.Fusion.IsFusion = true;
                                    character.InfoChar.Fusion.IsPorata = true;
                                    character.InfoChar.Fusion.TimeStart = timeServer;
                                    character.InfoChar.Fusion.TimeUse = 600000;
                                }
                                disciple.Status = 4;
                            }
                            character.CharacterHandler.SetUpInfo();
                            character.CharacterHandler.PlusHp((int)character.HpFull);
                            character.CharacterHandler.PlusMp((int)character.MpFull);
                            character.CharacterHandler.SendZoneMessage(Service.UpdateBody(character));
                            character.CharacterHandler.SendMessage(Service.PlayerLoadSpeed(character));
                            character.CharacterHandler.SendMessage(Service.MeLoadPoint(character));
                            character.CharacterHandler.SendMessage(Service.SendHp((int)character.HpFull));
                            character.CharacterHandler.SendMessage(Service.SendMp((int)character.MpFull));
                            character.CharacterHandler.SendZoneMessage(Service.PlayerLevel(character));
                            break;
                        }
                    case 1300:
                    case 638:
                    case 764:
                    case 752:
                    case 753:
                    case 1565:
                    case 1566:
                        {
                            UseBuffItem(character, itemUse);
                            break;
                        }
                    //Bông tai cấp 2
                    case 921:
                        {
                            var disciple = character.Disciple;
                            if (disciple == null)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().DONT_FIND_DISCIPLE));
                                return;
                            }

                            

                            var timeServer = ServerUtils.CurrentTimeMillis();

                            if (character.InfoChar.Fusion.IsFusion)
                            {
                                if (character.InfoChar.Fusion.IsPorata)
                                {
                                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().PLEASE_USE_PORATA));
                                    // Hợp thể cấp 1 không thể mở bằng item cấp 2
                                    return;
                                }

                                disciple.CharacterHandler.SetUpPosition(isRandom: true);
                                character.Zone.ZoneHandler.AddDisciple(disciple);
                                character.CharacterHandler.SendZoneMessage(Service.Fusion(character.Id, 1));
                                lock (character.InfoChar.Fusion)
                                {
                                    character.InfoChar.Fusion.IsFusion = false;
                                    character.InfoChar.Fusion.IsPorata2 = false;
                                    character.InfoChar.Fusion.TimeStart = timeServer;
                                    character.InfoChar.Fusion.DelayFusion = timeServer + 600000;
                                    character.InfoChar.Fusion.TimeUse = 0;
                                }

                                disciple.Status = 0;
                                character.Delay.BongTaiPorata = 10000 + timeServer;
                            }
                            else
                            {
                                if (disciple.InfoChar.IsDie) //disciple.Status >= 3 || 
                                {
                                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().CAN_NOT_USE_FUSION));
                                    return;
                                }

                                if (character.Delay.BongTaiPorata > timeServer)
                                {
                                    var delay = (character.Delay.BongTaiPorata - timeServer) / 1000;
                                    if (delay < 1)
                                    {
                                        delay = 1;
                                    }

                                    character.CharacterHandler.SendMessage(
                                        Service.ServerMessage(string.Format(TextServer.gI().DELAY_FUSION_SEC,
                                            delay)));
                                    return;
                                }

                                if (disciple.InfoSkill.HuytSao.IsHuytSao)
                                {
                                    SkillHandler.RemoveHuytSao(disciple);
                                }

                                if (disciple.InfoSkill.Monkey.MonkeyId == 1)
                                {
                                    SkillHandler.HandleMonkey(disciple, false);
                                }

                                if (disciple.Status < 3 && disciple.Zone != null && disciple.Zone.ZoneHandler != null)
                                {
                                    disciple.Zone.ZoneHandler.RemoveDisciple(disciple);
                                }

                                character.CharacterHandler.SendZoneMessage(Service.Fusion(character.Id, 6));
                                lock (character.InfoChar.Fusion)
                                {
                                    character.InfoChar.Fusion.IsFusion = true;
                                    character.InfoChar.Fusion.IsPorata2 = true;
                                    character.InfoChar.Fusion.TimeStart = timeServer;
                                    character.InfoChar.Fusion.TimeUse = 600000;
                                }
                                disciple.Status = 4;
                            }
                            character.CharacterHandler.SetUpInfo();
                            character.CharacterHandler.PlusHp((int)character.HpFull);
                            character.CharacterHandler.PlusMp((int)character.MpFull);
                            character.CharacterHandler.SendZoneMessage(Service.UpdateBody(character));
                            character.CharacterHandler.SendMessage(Service.PlayerLoadSpeed(character));
                            character.CharacterHandler.SendMessage(Service.MeLoadPoint(character));
                            character.CharacterHandler.SendMessage(Service.SendHp((int)character.HpFull));
                            character.CharacterHandler.SendMessage(Service.SendMp((int)character.MpFull));
                            character.CharacterHandler.SendZoneMessage(Service.PlayerLevel(character));
                            break;
                        }
                    case 1205:
                    case 1219:
                    //case 758:
                        UseVatPhamTet(character, itemUse);
                        break;
                    case 521:
                        {
                            var timeServer = ServerUtils.CurrentTimeMillis();
                            // Kiểm tra xem đã có dùng chưa,
                            // nếu đã có dùng thì xóa hiệu ứng và trả lại thời gian
                            var itemOption = itemUse.Options.FirstOrDefault(option => option.Id == 1);
                            if (character.InfoChar.TimeAutoPlay > 0)
                            {
                                // đã có dùng
                                var giayConLai = (character.InfoChar.TimeAutoPlay - timeServer) / 1000;
                                itemUse.Quantity = 1;
                                if (giayConLai > 60)
                                {
                                    var phutConLai = giayConLai / 60;
                                    itemOption.Param = (int)phutConLai;
                                }
                                else
                                {
                                    itemOption.Param = 0;
                                }
                                character.InfoChar.TimeAutoPlay = 0;
                                character.CharacterHandler.SendMessage(Service.ItemTime(4387, 0));
                                character.CharacterHandler.SendMessage(Service.AutoPlay(false));
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().END_AUTO_PLAY));
                            }
                            else
                            {
                                var soPhutSuDung = itemOption.Param;
                                var soGiaySuDung = soPhutSuDung * 60;
                                character.InfoChar.TimeAutoPlay = timeServer + (soGiaySuDung * 1000);
                                itemOption.Param = 0;
                                character.CharacterHandler.SendMessage(Service.ItemTime(4387, soGiaySuDung));
                                character.CharacterHandler.SendMessage(Service.AutoPlay(true));
                                character.Delay.AutoPlay = 60000 + timeServer;
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().START_AUTO_PLAY));
                            }

                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                            break;
                        }
                    case 379:
                        {
                            UseMayDoCapsuleKiBi(character, itemUse);
                            break;
                        }
                    case 380:
                        {
                            UseCapsuleKiBi(character, itemUse);
                            break;
                        }
                    case 818:
                        {
                            UseCapsuleHalloween(character, itemUse);
                            break;
                        }
                    case 663:
                    case 664:
                    case 665:
                    case 666:
                    case 667:
                    case 670:
                        {
                            UseThucAn(character, itemUse);
                            break;
                        }
                    case 465:
                    case 466:
                    case 472:
                    case 473:
                        {
                            UseBanhTrungThuBuff(character, itemUse);
                            break;
                        }
                    case 381:
                        if (character.InfoBuff.CuongNo2)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Chỉ được sử dụng 1 thứ!"));
                            return;
                        }
                        UseBuffItem(character, itemUse);
                        break;
                    case 382:
                        if (character.InfoBuff.BoHuyet2)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Chỉ được sử dụng 1 thứ!"));
                            return;
                        }
                        UseBuffItem(character, itemUse);
                        break;
                    case 383:
                        if (character.InfoBuff.BoKhi2)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Chỉ được sử dụng 1 thứ!"));
                            return;
                        }
                        UseBuffItem(character, itemUse);
                        break;
                    case 384:
                        if (character.InfoBuff.GiapXen2)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Chỉ được sử dụng 1 thứ!"));
                            return;
                        }
                        UseBuffItem(character, itemUse);
                        break;
                    case 385:
                        if (character.InfoBuff.AnDanh2)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Chỉ được sử dụng 1 thứ!"));
                            return;
                        }
                        UseBuffItem(character, itemUse);
                        break;
                    case 462:
                        UseBuffItem(character, itemUse);
                        break;
                    case 1150:
                        if (character.InfoBuff.CuongNo)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Chỉ được sử dụng 1 thứ!"));
                            return;
                        }
                        UseBuffItem(character, itemUse);
                        break;
                    case 1151:
                        if (character.InfoBuff.BoKhi)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Chỉ được sử dụng 1 thứ!"));
                            return;
                        }
                        UseBuffItem(character, itemUse);
                        break;
                    case 1152:
                        if (character.InfoBuff.BoHuyet)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Chỉ được sử dụng 1 thứ!"));
                            return;
                        }
                        UseBuffItem(character, itemUse);
                        break;
                    case 1153:
                        if (character.InfoBuff.GiapXen)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Chỉ được sử dụng 1 thứ!"));
                            return;
                        }
                        UseBuffItem(character, itemUse);
                        break;
                    case 1154:
                        {
                            if (character.InfoBuff.AnDanh)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Chỉ được sử dụng 1 thứ!"));
                                return;
                            }
                            UseBuffItem(character, itemUse);
                            break;
                        }
                    case 1274:
                        if (character.InfoBuff.KichDucX5 || character.InfoBuff.KichDucX7)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Chỉ được sử dụng 1 thứ!"));
                            return;
                        }
                        UseBuffItem(character, itemUse);

                        break;

                    case 1275:
                        if (character.InfoBuff.KichDucX2 || character.InfoBuff.KichDucX7)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Chỉ được sử dụng 1 thứ!"));
                            return;
                        }
                        UseBuffItem(character, itemUse);

                        break;
                    case 1276:
                        if (character.InfoBuff.KichDucX2 || character.InfoBuff.KichDucX5)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Chỉ được sử dụng 1 thứ!"));
                            return;
                        }
                        UseBuffItem(character, itemUse);
                        break;
                    case 694:
                    case 1195:
                    case 1196:
                        UseBuffItem(character, itemUse);
                        break;
                    case 1171:
                        UseTui7ChuLun(character, itemUse);
                        break;
                    case 891:
                        {
                            UseBanhTrungThu(character, itemUse);
                            break;
                        }

                    case 737:
                        {
                            UseCapsuleTrungThu(character, itemUse);
                            break;
                        }
                    case 962:
                    case 963:
                        {
                            UseCapsuleCaiTrang(character, itemUse, itemUse.Id - 962);
                            break;
                        }
                        break;
                    case 992:
                        {
                            character.InfoMore.TransportMapId = 160;
                            character.CharacterHandler.SendMessage(Service.Transport(3, 1));
                            break;
                        }
                    case 400:
                        if (character.Disciple == null)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn phải có đệ tử để sử dụng vật phẩm"));
                            break;
                        }
                        var inputChangeNameDisciple = new List<InputBox>();
                        var inputNameChange = new InputBox()
                        {
                            Name = "Nhập tên cho đệ tử",
                            Type = 1,
                        };
                        inputChangeNameDisciple.Add(inputNameChange);
                        character.CharacterHandler.SendMessage(Service.ShowInput("Đổi tên đệ tử", inputChangeNameDisciple));
                        character.TypeInput = 20;
                        break;
                    case 1220:
                        var inputChangeNameCharacter = new List<InputBox>();
                        inputNameChange = new InputBox()
                        {
                            Name = "Nhập tên cho nhân vật",
                            Type = 19,
                        };
                        inputChangeNameCharacter.Add(inputNameChange);
                        character.CharacterHandler.SendMessage(Service.ShowInput("Đổi tên Nhân vật", inputChangeNameCharacter));
                        character.TypeInput = 21;
                        break;
                }


            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error Use item in ItemHandler.cs: {e.Message} \n {e.StackTrace}", e);
                throw;
            }

        }

        public static void UseItemBox(Model.Character.Character character, int index, short template = -1)
        {
            try
            {
                Model.Item.Item itemUse;
                if (template != -1) return;

                itemUse = character.CharacterHandler.GetItemBoxByIndex(index);
                if (itemUse == null) return;

                var itemTemplate = ItemCache.ItemTemplate(itemUse.Id);
                if (itemTemplate.Require > character.InfoChar.Power)
                {
                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_POWER));
                    return;
                }

                if (itemTemplate.Gender != 3 && itemTemplate.Gender != character.InfoChar.Gender)
                {
                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_GENDER));
                    return;
                }

                var yeuCauSucManhTi = itemUse.Options.FirstOrDefault(opt => opt.Id == 21);
                if (yeuCauSucManhTi != null)
                {
                    if ((long)((long)yeuCauSucManhTi.Param * 1000000000) > character.InfoChar.Power)
                    {
                        character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_POWER));
                        return;
                    }
                }

                if (itemTemplate.IsTypeBody())
                {
                    var indexInBody = itemTemplate.Type == 32 ? 6 : itemTemplate.Type;
                    var itemBody = character.ItemBody[indexInBody];
                    var itemClone = Clone(itemUse);
                    itemClone.IndexUI = indexInBody;
                    character.ItemBody[indexInBody] = itemClone;
                    if (itemBody != null)
                    {
                        itemBody.IndexUI = index;
                        character.ItemBox[index] = itemBody;
                        character.CharacterHandler.SendMessage(Service.SendBox(character));
                    }
                    else
                    {
                        character.CharacterHandler.RemoveItemBox(index);
                    }
                    character.Delay.NeedToSaveBody = true;
                    var timeServer = ServerUtils.CurrentTimeMillis();
                    character.Delay.InvAction = timeServer + 1000;
                    if ((character.InfoChar.ThoiGianDoiMayChu - timeServer) < 180000)
                    {
                        character.InfoChar.ThoiGianDoiMayChu = timeServer + 300000;
                    }
                    // character.Delay.SaveData += 1000;
                    character.CharacterHandler.UpdateInfo();
                    return;
                }

                //Use Card
                if (itemTemplate.Type == 33)
                {
                    UseCard(character, itemUse, true);
                    return;
                }

                if (itemTemplate.Type == 7 && UseBook(character, itemUse))
                {
                    character.CharacterHandler.RemoveItemBox(index);
                    return;
                }

                switch (itemTemplate.Id)
                {
                    //Dau than
                    case 13:
                    case 60:
                    case 61:
                    case 62:
                    case 63:
                    case 64:
                    case 65:
                    case 352:
                    case 523:
                    case 595:
                        {
                            UsePea(character, itemUse, 1);
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error Use item in ItemHandler.cs: {e.Message} \n {e.StackTrace}", e);
            }

        }

        public static void UseItemForDisciple(Model.Character.Character character, int index, short template = -1)
        {
            try
            {
                var disciple = character.Disciple;
                if (disciple == null || disciple.InfoChar.IsDie)
                {
                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_GENDER));
                    return;
                }

                Model.Item.Item itemUse;
                itemUse = character.CharacterHandler.GetItemBagByIndex(index);
                if (itemUse == null) return;

                var itemTemplate = ItemCache.ItemTemplate(itemUse.Id);
                if (disciple.InfoChar.Power < 1500000)
                {
                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().DISCIPLE_NOT_ENOUGH_POWER));
                    return;
                }
                if (itemTemplate.Gender != 3 && itemTemplate.Gender != disciple.InfoChar.Gender)
                {
                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_GENDER));
                    return;
                }

                if (itemTemplate.Require > disciple.InfoChar.Power)
                {
                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().DISCIPLE_NOT_ENOUGH_POWER));
                    return;
                }

                var yeuCauSucManhTi = itemUse.Options.FirstOrDefault(opt => opt.Id == 21);
                if (yeuCauSucManhTi != null)
                {
                    if ((long)((long)yeuCauSucManhTi.Param * 1000000000) > disciple.InfoChar.Power)
                    {
                        character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().DISCIPLE_NOT_ENOUGH_POWER));
                        return;
                    }
                }
                try
                {
                    if (itemTemplate.isDiscipleBody())
                    {
                        //    return;
                        var indexInBody = itemTemplate.Type == 32 ? 6 : itemTemplate.Type == 37 ? 7 : itemTemplate.Type;
                        var itemBody = disciple.ItemBody[indexInBody];
                        var itemClone = Clone(itemUse);
                        itemClone.IndexUI = indexInBody;
                        disciple.ItemBody[indexInBody] = itemClone;
                        if (itemBody != null)
                        {
                            itemBody.IndexUI = index;
                            character.ItemBag[index] = itemBody;
                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                        }
                        else
                        {
                            character.CharacterHandler.RemoveItemBag(index, reason: "Mặc cho đệ tử");
                        }
                        if (disciple.Status == 0 || disciple.Status == 4) character.CharacterHandler.UpdateInfo();
                        disciple.CharacterHandler.UpdateInfo(true);
                        character.CharacterHandler.SendMessage(Service.Disciple(2, disciple));
                        //  DiscipleDB.SaveInventory(disciple);
                    }
                } catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Error Use item in ItemHandler.cs: {e.Message} \n {e.StackTrace}", e);

                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error Use item in ItemHandler.cs: {e.Message} \n {e.StackTrace}", e);
            }
        }
        private static void UseRuongSaoPhaLeCap2(Model.Character.Character character, Model.Item.Item item)
        {
            var itemNew = ItemCache.GetItemDefault((short)(DataCache.ListItemSaoPhaLeUpgrade[ServerUtils.RandomNumber(DataCache.ListItemSaoPhaLeUpgrade.Count)]));
            var itemTemplateOld = ItemCache.ItemTemplate(item.Id);
            var itemTemplateNew = ItemCache.ItemTemplate(itemNew.Id);
            character.CharacterHandler.SendMessage(Service.SendCombinne6(itemTemplateOld.IconId, itemTemplateNew.IconId));
            character.CharacterHandler.RemoveItemBagByIndex(item.IndexUI, 1);
            character.CharacterHandler.AddItemToBag(true, itemNew);
            character.CharacterHandler.SendMessage(Service.SendBag(character));
            character.CharacterHandler.SendMessage(Service.ServerMessage($"Bạn đã nhận được " + ItemCache.ItemTemplate(itemNew.Id).Name)); 
        }
        private static void UseHopQuaGiangSinh(Model.Character.Character character, Model.Item.Item item)
        {
            var listItem = new List<short>();
            listItem.AddRange(DataCache.ListSaoPhaLe);
            listItem.AddRange(DataCache.ListDaNangCap);
            listItem.AddRange(DataCache.ListSaoPhaLeLevel2);
            var itemNew = ItemCache.GetItemDefault((short)(listItem[ServerUtils.RandomNumber(listItem.Count)]), ServerUtils.RandomNumber(1,4));
            character.CharacterHandler.RemoveItemBagByIndex(item.IndexUI, 1);
            character.CharacterHandler.AddItemToBag(true, itemNew);
            character.CharacterHandler.SendMessage(Service.SendBag(character));
            character.CharacterHandler.SendMessage(Service.ServerMessage($"Bạn đã nhận được " + ItemCache.ItemTemplate(itemNew.Id).Name));
        }
        public static void UseNhoTim(Model.Character.Character character, Model.Item.Item item)
        {
            character.CharacterHandler.PlusStamina(character.InfoChar.MaxStamina);
            character.CharacterHandler.SendMessage(Service.SendStamina(character.InfoChar.Stamina));
            character.CharacterHandler.RemoveItemBagByIndex(item.IndexUI, 1);
            character.CharacterHandler.SendMessage(Service.ServerMessage("Thể lực của bạn đã phục hồi 100%"));
            character.CharacterHandler.SendMessage(Service.SendBag(character));
            character.CharacterHandler.SendMessage(Service.SendBag(character));
        }
        public static void UsePea(Model.Character.Character character, Model.Item.Item item, int type)
        {
            if (character.Delay.UsePea > ServerUtils.CurrentTimeMillis()) return;
            if (character.HpFull == character.InfoChar.Hp &&
                character.MpFull == character.InfoChar.Mp &&
                (!character.InfoChar.IsHavePet || character.InfoChar.IsHavePet &&
                    character.Disciple.HpFull == character.Disciple.InfoChar.Hp &&
                    character.Disciple.MpFull == character.Disciple.InfoChar.Mp &&
                    character.Disciple.InfoChar.Stamina == character.Disciple.InfoChar.MaxStamina)) return;

            switch (type)
            {
                case 0:
                    character.CharacterHandler.RemoveItemBagByIndex(item.IndexUI, 1, reason: "Ăn đậu");
                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                    break;
                case 1:
                    character.CharacterHandler.RemoveItemBoxByIndex(item.IndexUI, 1);
                    character.CharacterHandler.SendMessage(Service.SendBox(character));
                    break;
            }

            var plus = item.GetParamOption(2) * 1000 + item.GetParamOption(48);
            if (character.InfoChar.Hp < character.HpFull)
            {
                character.CharacterHandler.PlusHp(plus);
                character.CharacterHandler.SendMessage(Service.SendHp((int)character.InfoChar.Hp));
                character.CharacterHandler.SendZoneMessage(Service.PlayerLevel(character));
            }

            if (character.InfoChar.Mp < character.MpFull)
            {
                character.CharacterHandler.PlusMp(plus);
                character.CharacterHandler.SendMessage(Service.SendMp((int)character.InfoChar.Mp));
            }
            character.Delay.UsePea = 10000 + ServerUtils.CurrentTimeMillis();
            if (character.InfoChar.Stamina < character.InfoChar.MaxStamina)
            {
                character.CharacterHandler.PlusStamina(100 * (DataCache.IdDauThan.IndexOf(item.Id) + 1));
            }
            var disciple = character.Disciple;
            if (disciple != null && !disciple.InfoChar.IsDie && disciple.Status < 3)
            {
                if (disciple.InfoChar.Hp < disciple.HpFull)
                {
                    disciple.CharacterHandler.PlusHp(plus);
                    disciple.CharacterHandler.SendZoneMessage(Service.PlayerLevel(disciple));
                }

                if (disciple.InfoChar.Mp < disciple.MpFull)
                {
                    disciple.CharacterHandler.PlusMp(plus);
                }

                if (disciple.InfoChar.Stamina < disciple.InfoChar.MaxStamina)
                {
                    disciple.CharacterHandler.PlusStamina(100 * (DataCache.IdDauThan.IndexOf(item.Id) + 1));
                }
            }
        }
        public static void UseRole(Model.Character.Character character, Model.Item.Item item, int type)
        {
            var role = Cache.Gi().Role1Templates.FirstOrDefault(i => i.Id == item.Id);
            if (character.InfoChar.Roles1.Roles.Contains(role))
            {
                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã sở hữu danh hiệu này rồi, ra Cửa hành danh hiệu Santa để đeo danh hiệu"));
                return;
            }
            var roleNew = new Role1Template()
            {
                Id = role.Id,
                Delay = role.Delay,
                Temp = role.Temp,
                Name = role.Name,
                Options = role.Options.Copy(),
                Second = role.Second,
            };
            character.InfoChar.Roles1.Roles.Add(roleNew);
            character.CharacterHandler.SendMessage(Service.ServerMessage("Sử dụng danh hiệu thành công, hãy ra Cửa Hàng Santa để đeo danh hiệu"));
            character.CharacterHandler.RemoveItemBagByIndex(item.IndexUI, 1);
            character.CharacterHandler.SendMessage(Service.SendBag(character));
        }
        private static bool UseBook(Model.Character.Character character, Model.Item.Item item)
        {
            var itemTemplate = ItemCache.ItemTemplate(item.Id);
            if (itemTemplate.Gender != character.InfoChar.Gender && itemTemplate.Gender != 3)
            {
                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().DONOT_USE_SKILL));
                return false;
            }

            var skillTemplate = Cache.Gi().SKILL_TEMPLATES.FirstOrDefault(skill => skill.Id == itemTemplate.Skill);
            if (skillTemplate == null) return false;
            {
                var levelSkillBook = itemTemplate.Level;
                var skillChar = character.Skills.FirstOrDefault(skill => skill.Id == skillTemplate.Id);
                if (skillChar != null)
                {
                    if (levelSkillBook <= skillChar.Point)
                    {
                        character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().DUPLICATE_USE_SKILL));
                        return false;
                    }
                    if (levelSkillBook - skillChar.Point != 1)
                    {
                        character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().CANT_YET_USE_SKILL));
                        return false;
                    }

                    var skillAdd =
                        skillTemplate.SkillDataTemplates.FirstOrDefault(option => option.Point == levelSkillBook);
                    skillChar.SkillId = skillAdd!.SkillId;
                    skillChar.CoolDown = 0;
                    skillChar.Point++;
                    character.CharacterHandler.SendMessage(Service.UpdateSkill((short)skillAdd.SkillId));
                    character.BoughtSkill.Add(item.Id);
                    return true;
                }

                if (character.BoughtSkill.Contains(item.Id))
                {
                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().DUPLICATE_USE_SKILL));
                    return false;
                }
                switch (itemTemplate.Skill)
                {
                    case 21:
                        {
                            var skilCharCheck = character.Skills.FirstOrDefault(skill => skill.Id == 13);
                            if (skilCharCheck == null || skilCharCheck.Point < 7)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().CANT_YET_USE_SKILL));
                                return false;
                            }
                            break;
                        }
                    case 18:
                        {
                            var skilCharCheck = character.Skills.FirstOrDefault(skill => skill.Id == 12);
                            if (skilCharCheck == null || (skilCharCheck?.Point < 7))
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().CANT_YET_USE_SKILL));
                                return false;
                            }
                            break;
                        }
                    case 22:
                        {
                            var skilCharCheck = character.Skills.FirstOrDefault(skill => skill.Id == 9);
                            if (skilCharCheck == null || (skilCharCheck.Point < 7))
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().CANT_YET_USE_SKILL));
                                return false;
                            }
                            break;
                        }
                }

                if (itemTemplate.Level == 1)
                {
                    var skillAdd =
                        skillTemplate.SkillDataTemplates.FirstOrDefault(option => option.Point == 1);
                    if (skillAdd == null)
                    {
                        return false;
                    }
                    character.Skills.Add(new SkillCharacter()
                    {
                        Id = skillTemplate.Id,
                        SkillId = skillAdd.SkillId,
                        CoolDown = 0,
                        Point = 1,
                    });
                    character.CharacterHandler.SendMessage(Service.AddSkill((short)skillAdd.SkillId));
                    character.BoughtSkill.Add(item.Id);
                    return true;
                }
                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().CANT_YET_USE_SKILL));
                return false;
            }
        }

        private static void UseTui7ChuLun(Model.Character.Character character, Model.Item.Item item)
        {
            character.CharacterHandler.RemoveItemBagByIndex(item.IndexUI, 1, reason: "Tui 7 Chu Lun");
            var p = ServerUtils.RandomNumber(120);
            if (100 > p)
            {
                var i = ItemCache.GetItemDefault(1161);
                if (100 > p)
                {
                    if (10 > p)
                    {
                        i = ItemCache.GetItemDefault(1161);
                    }
                    else if (20 > p)
                    {
                        i = ItemCache.GetItemDefault(1165);
                    }
                    else if (30 > p)
                    {
                        i = ItemCache.GetItemDefault(1166);
                    }
                    else if (40 > p)
                    {
                        i = ItemCache.GetItemDefault(1167);
                    }
                    else if (50 > p)
                    {
                        i = ItemCache.GetItemDefault(1168);
                    }
                    else if (60 > p)
                    {
                        i = ItemCache.GetItemDefault(1169);
                    }
                    else if (70 > p)
                    {
                        i = ItemCache.GetItemDefault(1170);
                    }
                    else
                    {
                        if (80 > p)
                        {
                            if (character.InfoChar.Gender == 0)
                            {
                                i = ItemCache.GetItemDefault(1171);
                            } else if (character.InfoChar.Gender == 1)
                            {
                                i = ItemCache.GetItemDefault(1172);
                            }
                            else
                            {
                                i = ItemCache.GetItemDefault(1173);
                            }
                        }
                        else
                        {
                            var r = ServerUtils.RandomNumber(8);
                            if (r == 1)
                            {
                                i = ItemCache.GetItemDefault(1174);
                            }
                            else if (r == 2)
                            {
                                i = ItemCache.GetItemDefault(1175);
                            } else if (r == 3)
                            {
                                i = ItemCache.GetItemDefault(1176);
                            }
                            else if (r == 4)
                            {
                                i = ItemCache.GetItemDefault(1177);
                            }
                            else if (r == 5)
                            {
                                i = ItemCache.GetItemDefault(1178);
                            }
                            else if (r == 6)
                            {
                                i = ItemCache.GetItemDefault(1179);
                            }
                            else if (r == 7)
                            {
                                i = ItemCache.GetItemDefault(1180);
                            }
                            else if (r == 8)
                            {
                                i = ItemCache.GetItemDefault(987);
                            }
                        }
                    }
                }
                var temp = ItemCache.ItemTemplate(i.Id);
                i.Quantity = 1;
                if ((i.Id >= 1174 && i.Id <= 1180) || (i.Id >= 1171 && i.Id <= 1173))
                {
                    i.Options.Add(new OptionItem()
                    {
                        Id = 50,
                        Param = ServerUtils.RandomNumber(15, 21),
                    });
                    i.Options.Add(new OptionItem()
                    {
                        Id = 77,
                        Param = ServerUtils.RandomNumber(15, 21),
                    });
                    i.Options.Add(new OptionItem()
                    {
                        Id = 103,
                        Param = ServerUtils.RandomNumber(15, 21),
                    });
                    i.Options.Add(new OptionItem()
                    {
                        Id = 14,
                        Param = ServerUtils.RandomNumber(8, 15),
                    });
                }
                character.CharacterHandler.AddItemToBag(false, i, "USE TUI 7 CHU LUN");
                character.CharacterHandler.SendMessage(Service.SendBag(character));
                character.CharacterHandler.SendMessage(Service.ServerMessage("Chúc mừng bạn đã gọt được " + temp.Name));
            }
        }
        private static void UseMayDoCapsuleKiBi(Model.Character.Character character, Model.Item.Item item)
        {
            var template = ItemCache.ItemTemplate(item.Id);
            character.InfoBuff.MayDoCSKB = true;
            character.InfoBuff.MayDoCSKBTime = ServerUtils.CurrentTimeMillis() + 1800000;
            character.CharacterHandler.SendMessage(Service.ItemTime(template.IconId, 1800));
            character.CharacterHandler.RemoveItemBagByIndex(item.IndexUI, 1, reason: "Dùng máy dò");
            character.CharacterHandler.SendMessage(Service.SendBag(character));
        }

        private static void UseCapsuleHalloween(Model.Character.Character character, Model.Item.Item item)
        {
            try
            {
                character.CharacterHandler.RemoveItemBagByIndex(item.IndexUI, 1, reason: "CSKB");
                var randomPercent = ServerUtils.RandomNumber(0, 100);
                if (randomPercent <= 0.3)
                {
                    // - đồ kích hoạt 0.3%
                    var gender = character.InfoChar.Gender;
                    // TD hiếm Găng và RADAR 21,
                    var listItem = new List<short>() { 0, 6, 27 };
                    // Sôn gô ku hiếm 129
                    var listSKH = new List<int>() { 127, 128, 127, 128, 129 };

                    if (gender == 1)
                    {
                        // NM hiếm Giầy và Radar 28
                        listItem = new List<short>() { 1, 7, 22 };
                        // bộ picolo hiếm
                        listSKH = new List<int>() { 131, 132, 130, 131, 132 };
                    }
                    else if (gender == 2)
                    {
                        //XD hiếm quần và Radar 8
                        listItem = new List<short>() { 2, 23, 29 };
                        // bộ nappa hiếm 135
                        listSKH = new List<int>() { 133, 134, 133, 135, 134 };
                    }

                    var itemAdd = ItemCache.GetItemDefault(listItem[ServerUtils.RandomNumber(listItem.Count)]);
                    itemAdd.Quantity = 1;
                    var idSKH = listSKH[ServerUtils.RandomNumber(listSKH.Count)];
                    itemAdd.Options.Add(new OptionItem()
                    {
                        Id = idSKH,
                        Param = 0,
                    });
                    itemAdd.Options.Add(new OptionItem()
                    {
                        Id = LeaveItemHandler.GetSKHDescOption(idSKH),
                        Param = 0,
                    });
                    itemAdd.Options.Add(new OptionItem()
                    {
                        Id = 30,
                        Param = 0,
                    });

                    character.CharacterHandler.AddItemToBag(true, itemAdd, "CSKB");
                    var template = ItemCache.ItemTemplate(itemAdd.Id);
                    character.CharacterHandler.SendMessage(Service.ServerMessage(string.Format(TextServer.gI().CSKB_GET, template.Name)));

                } else if (randomPercent <= 20.0)
                {
                    // - đồ thần linh 0.7%
                    var random = new Random();
                    int index = random.Next(DataCache.ListDoThanLinh.Count);
                    short idDoThanLinh = DataCache.ListDoThanLinh[index];
                    var itemAdd = ItemCache.GetItemDefault(idDoThanLinh);
                    itemAdd.Quantity = 1;
                    character.CharacterHandler.AddItemToBag(true, itemAdd, "CSKB");
                    var template = ItemCache.ItemTemplate(itemAdd.Id);
                    character.CharacterHandler.SendMessage(Service.ServerMessage(string.Format(TextServer.gI().CSKB_GET, template.Name)));

                } else if (randomPercent <= 15.0)
                {
                    // - đá bảo vệ 4%
                    var itemAdd = ItemCache.GetItemDefault(987);
                    itemAdd.Quantity = 1;
                    character.CharacterHandler.AddItemToBag(true, itemAdd, "CSKB");
                    var template = ItemCache.ItemTemplate(itemAdd.Id);
                    character.CharacterHandler.SendMessage(Service.ServerMessage(string.Format(TextServer.gI().CSKB_GET, template.Name)));

                } else if (randomPercent <= 40.0)
                {
                    // - ngọc rồng 7s-3s = 15%
                    var ListDo = new List<short>() { 16, 17, 18, 19, 20 };
                    var random = new Random();
                    int index = random.Next(ListDo.Count);
                    short idVatPham = ListDo[index];
                    var itemAdd = ItemCache.GetItemDefault(idVatPham);
                    itemAdd.Quantity = 1;
                    character.CharacterHandler.AddItemToBag(true, itemAdd, "CSKB");
                    var template = ItemCache.ItemTemplate(itemAdd.Id);
                    character.CharacterHandler.SendMessage(Service.ServerMessage(string.Format(TextServer.gI().CSKB_GET, template.Name)));

                }
                else if (randomPercent <= 40.0)
                {
                    // - quần , giày , rada , găng = 20%
                    var ListDo = new List<short>() { 0,1,2,3,4,5,33,34,41,42,49,50,
                     136,137,138,139,152,153,154,155,168,169,170,171,230,231
                         ,232,233,234,235,236,237,238,239,240,241,6,7,8,9,10,11,35,36,43,44,51,52,
                     140,141,142,143,156,157,158,159,172,173,174,175,
                     242,243,244,245,246,247,248,249,250,251,252,253,
                     253,256,257,258,259,260,261,262,263,264,265,21,22,23,24,25,26,37,38,45,46,53,54,
                     144,145,160,161,162,163,176,177,178,179,254,255,266,267,268,269,270,271,272,273,274,
                     275,276,277,27,28,29,30,31,32,39,40,47,48,55,56,149,150,151,164,165,166,167,180,181,
                     182,183,12,57,58,59 };
                    var random = new Random();
                    int index = random.Next(ListDo.Count);
                    short idVatPham = ListDo[index];
                    var itemAdd = ItemCache.GetItemDefault(idVatPham);
                    itemAdd.Quantity = 1;
                    character.CharacterHandler.AddItemToBag(true, itemAdd, "CSKB");
                    var template = ItemCache.ItemTemplate(itemAdd.Id);
                    character.CharacterHandler.SendMessage(Service.ServerMessage(string.Format(TextServer.gI().CSKB_GET, template.Name)));

                }
                else if (randomPercent <= 70.0)
                {
                    // - sao pha lê các loại  = 30%
                    var random = new Random();
                    int index = random.Next(DataCache.ListSaoPhaLe.Count);
                    short idSaoPhaLe = DataCache.ListSaoPhaLe[index];
                    var itemAdd = ItemCache.GetItemDefault(idSaoPhaLe);
                    itemAdd.Quantity = 1;
                    character.CharacterHandler.AddItemToBag(true, itemAdd, "CSKB");
                    var template = ItemCache.ItemTemplate(itemAdd.Id);
                    character.CharacterHandler.SendMessage(Service.ServerMessage(string.Format(TextServer.gI().CSKB_GET, template.Name)));
                }
                else if (randomPercent <= 80.0)
                {
                    // - đá nâng cấp = 30%
                    var random = new Random();
                    int index = random.Next(DataCache.ListDaNangCap.Count);
                    short idDaNangCap = DataCache.ListDaNangCap[index];
                    var itemAdd = ItemCache.GetItemDefault(idDaNangCap);
                    itemAdd.Quantity = 1;
                    character.CharacterHandler.AddItemToBag(true, itemAdd, "CSKB");
                    var template = ItemCache.ItemTemplate(itemAdd.Id);
                    character.CharacterHandler.SendMessage(Service.ServerMessage(string.Format(TextServer.gI().CSKB_GET, template.Name)));

                }
                character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                character.CharacterHandler.SendMessage(Service.SendBag(character));
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error UseCapsuleHalloween in ItemHandler.cs: {e.Message} \n {e.StackTrace}", e);
                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().ERROR_SERVER));
            }

        }
        private static void UseCapsuleKiBi(Model.Character.Character character, Model.Item.Item item)
        {
            character.CharacterHandler.RemoveItemBagByIndex(item.IndexUI, 1, reason: "CSKB");
            var randomPercent = ServerUtils.RandomNumber(0, 100);
            short itemId = 76;
            if (randomPercent > 45)
            {
                // itemId = 74;
                var template = ItemCache.ItemTemplate(itemId);
                character.PlusGold(ServerUtils.RandomNumber(17000, 32000));
                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                character.CharacterHandler.SendMessage(Service.ServerMessage(string.Format(TextServer.gI().CSKB_GET, template.Name)));
            }
            else
            {
                itemId = DataCache.listItemCSKB[ServerUtils.RandomNumber(DataCache.listItemCSKB.Count)];
                var template = ItemCache.ItemTemplate(itemId);
                var itemAdd = ItemCache.GetItemDefault(itemId);
                character.CharacterHandler.AddItemToBag(true, itemAdd);
                character.CharacterHandler.SendMessage(Service.ServerMessage(string.Format(TextServer.gI().CSKB_GET, template.Name)));
                character.CharacterHandler.SendMessage(Service.SendBag(character));
                character.CharacterHandler.SendMessage(Service.SendCombinne6(ItemCache.ItemTemplate(item.Id).IconId, template.IconId));

            }
           
        }

        private static void UseCapsuleTrungThu(Model.Character.Character character, Model.Item.Item item)
        {
            if (character.CharacterHandler.GetItemBagById(737) == null) return;
            character.CharacterHandler.RemoveItemBagByIndex(item.IndexUI, 1, reason: "CSTT");
            var tile = ServerUtils.RandomNumber(100);
            //50 % vàng, 20% nr, 20% item ngẫu nhiên, 8% cải trang, 2% cải trang v.v
            if (tile < 50)
            {
                var gold = ServerUtils.RandomNumber(50000000, 80000000);
                character.CharacterHandler.SendMessage(
                    Service.ServerMessage(string.Format($"Bạn nhận được {ServerUtils.GetMoney(gold)} vàng")));
                character.PlusGold(gold);
                character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
            }
            else if (tile < 70)
            {
                var listitem = new List<short>() { 16, 16, 17, 17, 17, 18, 18, 18, 18, 19, 19, 19, 19, 19 }; // ngoc rồng
                var itemrand = listitem[ServerUtils.RandomNumber(listitem.Count)];
                var itemAdd = ItemCache.GetItemDefault(itemrand);
                var temp = ItemCache.ItemTemplate(itemAdd.Id);
                character.CharacterHandler.AddItemToBag(true, itemAdd, "CSTT");
                character.CharacterHandler.SendMessage(
                    Service.ServerMessage(string.Format(TextServer.gI().ADD_ITEM,
                        $"{temp.Name}")));
            }
            else if (tile < 90)
            {
                var listitem = new List<short>() { 467, 468, 469, 470, 471, 800, 801, 802, 803, 804, 733, 734, 735, 993, 998, 999, 1000, 1001 }; // item ngẫu nhiên
                var itemrand = listitem[ServerUtils.RandomNumber(listitem.Count)];
                var itemAdd = ItemCache.GetItemDefault(itemrand);
                var timeServer = ServerUtils.CurrentTimeSecond();
                var expireDay = ServerUtils.RandomNumber(2, 5);
                var expireTime = timeServer + (expireDay * DataCache._1DAYBYSECOND);
                itemAdd.Options.Add(new OptionItem()
                {
                    Id = 93,
                    Param = expireDay
                });

                var optionHiden = itemAdd.Options.FirstOrDefault(option => option.Id == 73);
                if (optionHiden != null)
                {
                    optionHiden.Param =(int) expireTime;
                }
                else
                {
                    itemAdd.Options.Add(new OptionItem()
                    {
                        Id = 73,
                        Param = (int)expireTime,
                    });
                }

                itemAdd.Options.Add(new OptionItem()
                {
                    Id = 30,
                    Param = 0,
                });

                character.CharacterHandler.AddItemToBag(true, itemAdd, "CSTT");

                var temp = ItemCache.ItemTemplate(itemAdd.Id);
                character.CharacterHandler.SendMessage(
                    Service.ServerMessage(string.Format(TextServer.gI().ADD_ITEM,
                        $"{temp.Name}")));
            }
            else
            {
                var listitem = new List<short>() { 463, 464 }; // item ngẫu nhiên
                var itemrand = listitem[ServerUtils.RandomNumber(listitem.Count)];
                var itemAdd = ItemCache.GetItemDefault(itemrand);

                if (tile < 98)
                {
                    var timeServer = ServerUtils.CurrentTimeSecond();
                    var expireDay = ServerUtils.RandomNumber(2, 3);
                    var expireTime = timeServer + (expireDay * DataCache._1DAYBYSECOND);
                    itemAdd.Options.Add(new OptionItem()
                    {
                        Id = 93,
                        Param = expireDay
                    });

                    var optionHiden = itemAdd.Options.FirstOrDefault(option => option.Id == 73);
                    if (optionHiden != null)
                    {
                        optionHiden.Param = (int)expireTime;
                    }
                    else
                    {
                        itemAdd.Options.Add(new OptionItem()
                        {
                            Id = 73,
                            Param = (int)expireTime,
                        });
                    }
                }

                itemAdd.Options.Add(new OptionItem()
                {
                    Id = 30,
                    Param = 0,
                });

                character.CharacterHandler.AddItemToBag(true, itemAdd, "CSTT");

                var temp = ItemCache.ItemTemplate(itemAdd.Id);
                character.CharacterHandler.SendMessage(
                    Service.ServerMessage(string.Format(TextServer.gI().ADD_ITEM,
                        $"{temp.Name}")));
            }
            character.CharacterHandler.SendMessage(Service.SendBag(character));
        }
        public static void UseCapsuleCaiTrang(Model.Character.Character character, Model.Item.Item item, int type)
        {
            character.CharacterHandler.RemoveItemBagByIndex(item.IndexUI, 1);
            var itemNew = ItemCache.GetItemDefault((short)(DataCache.listItemCapsuleCaiTrang[ServerUtils.RandomNumber(DataCache.listItemCapsuleCaiTrang.Count)]));
            var expireDay = type == 1 ? 7 : 5;
            var expireTime = (int)((DataCache._1DAYBYSECOND * expireDay) + ServerUtils.CurrentTimeSecond());
            var optionHiden = itemNew.Options.FirstOrDefault(option => option.Id == 73);
            itemNew.Options.Add(new OptionItem()
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
                itemNew.Options.Add(new OptionItem()
                {
                    Id = 73,
                    Param = expireTime,
                });
            }
            character.CharacterHandler.SendMessage(Service.SendCombinne4((short)ItemCache.ItemTemplate(itemNew.Id).IconId));
            character.CharacterHandler.AddItemToBag(false, itemNew, "CSCT");

            var temp = ItemCache.ItemTemplate(itemNew.Id);
            character.CharacterHandler.SendMessage(
                Service.ServerMessage(string.Format(TextServer.gI().ADD_ITEM,
                    $"{temp.Name}")));
            character.CharacterHandler.SendMessage(Service.SendBag(character));

        }
        public static void UseVatPhamTet(Model.Character.Character character, Model.Item.Item item)
        {
            var id = item.Id;
            var item2 = ItemCache.GetItemDefault(1);
            switch (id)
            {
                case 758: // capsule tet
                    
                    var random = ServerUtils.RandomNumber(180);
                    if (random <= 20)
                    {
                        item2 = ItemCache.GetItemDefault(1219);
                    }else if (random<= 40)
                    {
                        item2 = ItemCache.GetItemDefault(849);
                    }
                    else if (random <= 60)
                    {
                        item2 = ItemCache.GetItemDefault(852);
                    }
                    else if (random <= 80)
                    {
                        item2 = ItemCache.GetItemDefault(941);
                    }
                    else if (random <= 100) {
                        item2 = ItemCache.GetItemDefault((short)(846+character.InfoChar.Gender));

                    }
                    else if (random <= 120)
                    {
                        item2 = ItemCache.GetItemDefault((short)(754 + character.InfoChar.Gender));
                    }
                    else if (random <= 140)
                    {
                        int[] listCaiTrangHo = { 941,946,947,948,952,953};
                        item2 = ItemCache.GetItemDefault((short)(listCaiTrangHo[ServerUtils.RandomNumber(listCaiTrangHo.Length)]));
                        item2.Options.Add(new OptionItem()
                        {
                            Id = 50,
                            Param = 12
                        });
                        item2.Options.Add(new OptionItem()
                        {
                            Id = 77,
                            Param = ServerUtils.RandomNumber(12, 20)
                        }) ;
                        item2.Options.Add(new OptionItem()
                        {
                            Id = 103,
                            Param = ServerUtils.RandomNumber(12, 20)
                        });
                        item2.Options.Add(new OptionItem()
                        {
                            Id = 12,
                            Param = ServerUtils.RandomNumber(12, 20)
                        });
                       

                    }
                    else if (random <= 160)
                    {
                        item2 = ItemCache.GetItemDefault(1205);
                    }
                    else if (random <= 180)
                    {
                        item2 = ItemCache.GetItemDefault(1219);
                    }

                    break;
                case 1219: // tui quy mao
                    random = ServerUtils.RandomNumber(100);
                    item2 = ItemCache.GetItemDefault(1);
                    if (random <= 60)
                    {
                        item2 = ItemCache.GetItemDefault((short)(1216+character.InfoChar.Gender));
                        item2.Options.Add(new OptionItem()
                        {
                            Id = 50,
                            Param = 12
                        });
                        item2.Options.Add(new OptionItem()
                        {
                            Id = 77,
                            Param = ServerUtils.RandomNumber(12, 20)
                        });
                        item2.Options.Add(new OptionItem()
                        {
                            Id = 103,
                            Param = ServerUtils.RandomNumber(12, 20)
                        });
                        item2.Options.Add(new OptionItem()
                        {
                            Id = 12,
                            Param = ServerUtils.RandomNumber(12, 20)
                        });
                    }
                    else if (random<= 70)
                    {
                        item2 = ItemCache.GetItemDefault(1213);
                    }else if (random<= 80)
                    {
                        item2 = ItemCache.GetItemDefault(1214);
                    }else if (random <= 85)
                    {
                        item2 = ItemCache.GetItemDefault(1203);

                    }else if (random <= 90)
                    {
                        item2 = ItemCache.GetItemDefault(1204);
                    }else if (random <= 95)
                    {
                        item2 = ItemCache.GetItemDefault(1206);
                    }else if (random <= 100)
                    {
                        item2 = ItemCache.GetItemDefault(1205);
                    }
                    break;
                case 1205:
                    random = ServerUtils.RandomNumber(100);
                    if (random <= 50)
                    {
                        item2 = ItemCache.GetItemDefault((short)(1191 + character.InfoChar.Gender));
                        item2.Options.Add(new OptionItem()
                        {
                            Id = 50,
                            Param = 22
                        });
                        item2.Options.Add(new OptionItem()
                        {
                            Id = 77,
                            Param = ServerUtils.RandomNumber(12, 20)
                        });
                        item2.Options.Add(new OptionItem()
                        {
                            Id = 103,
                            Param = ServerUtils.RandomNumber(12, 20)
                        });
                        item2.Options.Add(new OptionItem()
                        {
                            Id = 12,
                            Param = ServerUtils.RandomNumber(12, 20)
                        });
                    }
                    else if(random <= 100)
                    {
                        item2 = ItemCache.GetItemDefault((short)(1102));
                        item2.Options.Add(new OptionItem()
                        {
                            Id = 50,
                            Param = 36
                        });
                        item2.Options.Add(new OptionItem()
                        {
                            Id = 77,
                            Param = ServerUtils.RandomNumber(16, 30)
                        });
                        item2.Options.Add(new OptionItem()
                        {
                            Id = 103,
                            Param = ServerUtils.RandomNumber(16, 30)
                        });
                        item2.Options.Add(new OptionItem()
                        {
                            Id = 12,
                            Param = ServerUtils.RandomNumber(16, 30)
                        });
                    }
                    break;

            }
            character.CharacterHandler.AddItemToBag(false, item2, "Vat pham tet");
            character.CharacterHandler.SendMessage(Service.SendBag(character));
            character.CharacterHandler.SendMessage(Service.SendCombinne4((short)ItemCache.ItemTemplate(item2.Id).IconId));
          //  character.CharacterHandler.SendMessage(Service.ServerMessage("Chúc mừng bạn đã nhận được " + ItemCache.ItemTemplate(item2.Id).Name));
        }
        private static void UseBuffItem(Model.Character.Character character, Model.Item.Item item)
        {
            var ib = character.InfoBuff;
            long second = 0;
            switch(item.Id)
            {
                case 753:
                    ib.BanhChung = true;
                    ib.timeBanhChung = ServerUtils.CurrentTimeMillis() + DataCache._1HOUR;
                    second = DataCache._1HOUR / 1000;
                    break;
                case 752:
                    ib.BanhTet = true;
                    ib.timeBanhTet = ServerUtils.CurrentTimeMillis() + DataCache._1HOUR;
                    second = DataCache._1HOUR / 1000;
                    break;
                case 1300:
                   ib.MayDoLinhHon = true;
                    ib.MayDoLinhHonTime = ServerUtils.CurrentTimeMillis() + DataCache._1HOUR;
                    second = DataCache._1HOUR / 1000;
                    break;
                case 638:
                    ib.BinhChuaCommeson = true;
                    ib.BinhChuaCommesonTime = ServerUtils.CurrentTimeMillis() + 3600000;
                    second = 3600000 / 1000;
                    break;
                case 381:
                {
                   ib.CuongNo = true;
                   ib.CuongNoTime = ServerUtils.CurrentTimeMillis() + 600000;
                   second = 600000 / 1000;
                   break;
                }
                case 382:
                {
                    ib.BoHuyet = true;
                    ib.BoHuyetTime = ServerUtils.CurrentTimeMillis() + 600000;
                        second = 600000 / 1000;
                    break;
                }
                case 383:
                {
                    ib.BoKhi = true;
                    ib.BoKhiTime = ServerUtils.CurrentTimeMillis() + 600000;
                    second = 600000 / 1000;
                   break;
                }
                case 384:
                {
                    ib.GiapXen = true;
                    ib.GiapXenTime = ServerUtils.CurrentTimeMillis() + 600000;
                    second = 600000 / 1000;
                    break;
                }
                case 385:
                {
                    ib.AnDanh = true;
                    ib.AnDanhTime = ServerUtils.CurrentTimeMillis() + 600000;
                        second = 600000 / 1000;
                        break;
                }
                case 1150:
                    ib.CuongNo2 = true;
                    ib.CuongNoTime2 = ServerUtils.CurrentTimeMillis() + 600000;
                    second = 600000 / 1000;
                    break;
                case 1151:
                    ib.BoKhi2 = true;
                    ib.BoKhiTime2 = ServerUtils.CurrentTimeMillis() + 600000;
                    second = 600000 / 1000;
                    break;
                case 1152:
                    ib.BoHuyet2 = true;
                    ib.BoHuyetTime2 = ServerUtils.CurrentTimeMillis() + 600000;
                    second = 600000 / 1000;
                    break;
                case 1153:
                    ib.GiapXen2 = true;
                    ib.GiapXenTime2 = ServerUtils.CurrentTimeMillis() + 600000;
                    second = 600000 / 1000;
                    break;
                case 1154:
                    ib.AnDanh2 = true;
                    ib.AnDanhTime2 = ServerUtils.CurrentTimeMillis() + 600000;
                    second = 600000 / 1000;
                    break;
                case 462:
                {
                    ib.CuCarot = true;
                    ib.CuCarotTime = ServerUtils.CurrentTimeMillis() + 600000;
                        second = 600000 / 1000;
                        break;
                }
                case 691:
                    break;
                case 1195:
                    ib.XiMuoiHoaDao = true;
                    ib.XiMuoiHoaDaoTime = ServerUtils.CurrentTimeMillis() + 600000;
                    second = 600000 / 1000;
                    break;
                case 1196:
                    ib.XiMuoiHoaMai = true;
                    ib.XiMuoiHoaMaiTime = ServerUtils.CurrentTimeMillis() + 600000;
                    second = 600000 / 1000;
                    break;
                case 1274:
                    ib.KichDucX2 = true;
                    ib.KichDucX2Time = ServerUtils.CurrentTimeMillis() + 1800000;
                    second = 1800000 / 1000;
                    break;
                case 1275:
                    ib.KichDucX5 = true;
                    ib.KichDucX5Time = ServerUtils.CurrentTimeMillis() + 1800000;
                    second = 1800000 / 1000;
                    break;

                case 1276:
                    ib.KichDucX7 = true;
                    ib.KichDucX7Time = ServerUtils.CurrentTimeMillis() + 1800000;
                    second = 1800000 / 1000;
                    break;
                case 764:
                    ib.KhauTrang = true;
                    ib.KhauTrangTime = ServerUtils.CurrentTimeMillis() + 1800000;
                    second = 1800000 / 1000;
                    break;
                case 1565:
                case 1566:
                    ib.SatThuongChuanId = item.Id;
                    ib.TimeSatThuongChuan = ServerUtils.CurrentTimeMillis() + 600000;
                    second = 600000 / 1000;
                    break;
            }
            var template = ItemCache.ItemTemplate(item.Id);
            character.CharacterHandler.SendMessage(Service.ItemTime(template.IconId, (int)second));
            character.CharacterHandler.RemoveItemBagByIndex(item.IndexUI, 1, reason:"Dùng buff");
            character.CharacterHandler.SendMessage(Service.SendBag(character));
            character.CharacterHandler.SetUpInfo(true);
            character.CharacterHandler.SendMessage(Service.MeLoadPoint(character));
        }

        private static void UseBanhTrungThu(Model.Character.Character character, Model.Item.Item item)
        {
            if (character.CharacterHandler.GetItemBagById(891) == null) return;
            character.CharacterHandler.RemoveItemBagByIndex(item.IndexUI, 1, reason:"Ăn bánh tt");
            var tile = ServerUtils.RandomNumber(100);
            //50 % vàng,20% nr, 10% item ngẫu nhiên, 20% không ra gì
            if (tile < 50)
            {
                var gold = ServerUtils.RandomNumber(10000000,12000000);
                character.CharacterHandler.SendMessage(
                    Service.ServerMessage(string.Format($"Bạn nhận được {ServerUtils.GetMoney(gold)} vàng")));
                character.PlusGold(gold);
                character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
            } else if (tile < 65)
            {
                var listitem = new List<short>() {16,16,17,17,17,18,18,18,18,19,19,19,19,19}; // ngoc rồng
                var itemrand = listitem[ServerUtils.RandomNumber(listitem.Count)];
                var itemAdd = ItemCache.GetItemDefault(itemrand);
                var temp = ItemCache.ItemTemplate(itemAdd.Id);
                character.CharacterHandler.AddItemToBag(true, itemAdd, "CSTT");
                character.CharacterHandler.SendMessage(
                    Service.ServerMessage(string.Format(TextServer.gI().ADD_ITEM,
                        $"{temp.Name}")));
            } else if (tile < 75)
            {
                var listitem = new List<short>() {467,468,469,470,471,800,801,802,803,804,733,734,735}; // item ngẫu nhiên
                var itemrand = listitem[ServerUtils.RandomNumber(listitem.Count)];
                var itemAdd = ItemCache.GetItemDefault(itemrand);
                var timeServer = ServerUtils.CurrentTimeSecond();
                var expireDay = ServerUtils.RandomNumber(2,3);
                var expireTime = timeServer + (expireDay* DataCache._1DAYBYSECOND);
                itemAdd.Options.Add(new OptionItem()
                {
                    Id = 93,
                    Param = expireDay
                });

                var optionHiden = itemAdd.Options.FirstOrDefault(option => option.Id == 73);
                if (optionHiden != null) 
                {
                    optionHiden.Param = (int)expireTime;
                }
                else 
                {
                    itemAdd.Options.Add(new OptionItem()
                    {
                        Id = 73,
                        Param = (int)expireTime,
                    });
                }

                itemAdd.Options.Add(new OptionItem()
                {
                    Id = 30,
                    Param = 0,
                });

                character.CharacterHandler.AddItemToBag(true, itemAdd, "CSTT");

                var temp = ItemCache.ItemTemplate(itemAdd.Id);
                character.CharacterHandler.SendMessage(
                    Service.ServerMessage(string.Format(TextServer.gI().ADD_ITEM,
                        $"{temp.Name}")));
            }
            character.DiemSuKien += 1;
            character.CharacterHandler.SendMessage(
                Service.ServerMessage(string.Format("Bạn nhận được 1 điểm sự kiện trung thu")));
            character.CharacterHandler.SendMessage(Service.SendBag(character));
        }

        private static void UseThucAn(Model.Character.Character character, Model.Item.Item item)
        {
            // Nếu chưa có thức ăn, thì set thời gian
            if (character.InfoBuff.ThucAnId != -1)
            {
                var oldTemplate = ItemCache.ItemTemplate(character.InfoBuff.ThucAnId);
                character.CharacterHandler.SendMessage(Service.ItemTime(oldTemplate.IconId, 0));
            }
            // Nếu đã có thức ăn thì xóa item thức ăn cũ.
            var template = ItemCache.ItemTemplate(item.Id);
            character.InfoBuff.ThucAnId = item.Id;
            character.InfoBuff.ThucAnTime = ServerUtils.CurrentTimeMillis() + 600000;
            character.CharacterHandler.SendMessage(Service.ItemTime(template.IconId, (int)character.InfoBuff.ThucAnTime/1000));
            character.CharacterHandler.RemoveItemBagByIndex(item.IndexUI, 1, reason:"Ăn thức ăn");
            character.CharacterHandler.SendMessage(Service.SendBag(character));
            character.CharacterHandler.SetUpInfo();
            character.CharacterHandler.SendMessage(Service.MeLoadPoint(character));
        }

        private static void UseBanhTrungThuBuff(Model.Character.Character character, Model.Item.Item item)
        {
            // Nếu chưa có thức ăn, thì set thời gian
            if (character.InfoBuff.BanhTrungThuId != -1)
            {
                var oldTemplate = ItemCache.ItemTemplate(character.InfoBuff.BanhTrungThuId);
                character.CharacterHandler.SendMessage(Service.ItemTime(oldTemplate.IconId, 0));
            }
            // Nếu đã có thức ăn thì xóa item thức ăn cũ.
            var template = ItemCache.ItemTemplate(item.Id);
            character.InfoBuff.BanhTrungThuId = item.Id;
            character.InfoBuff.BanhTrungThuTime = ServerUtils.CurrentTimeMillis() + 3600000;
            character.CharacterHandler.SendMessage(Service.ItemTime(template.IconId, (int)character.InfoBuff.BanhTrungThuTime / 1000));
            character.CharacterHandler.RemoveItemBagByIndex(item.IndexUI, 1, reason:"Ăn bánh tt buff");
            character.CharacterHandler.SendMessage(Service.SendBag(character));
            character.CharacterHandler.SetUpInfo();
            character.CharacterHandler.SendMessage(Service.MeLoadPoint(character));
        }
        private static void HandleAmulet(Model.Character.Character character, short amulet)
        {
            if (character.InfoChar.ItemAmulet.ContainsKey(amulet))
            {
                if (character.InfoChar.ItemAmulet[amulet] < ServerUtils.CurrentTimeMillis())
                {
                    character.InfoChar.ItemAmulet[amulet] = DataCache._8HOURS + ServerUtils.CurrentTimeMillis();
                }
                else
                {
                    character.InfoChar.ItemAmulet[amulet] += DataCache._8HOURS;
                }
            }
            else
            {
                character.InfoChar.ItemAmulet.TryAdd(amulet, DataCache._1HOUR + ServerUtils.CurrentTimeMillis());
            }
            character.CharacterHandler.SetupAmulet();

        }
        private static int HandleRandGold(Model.Character.Character character, int minG, int maxG)
        {
            var gold = ServerUtils.RandomNumber(minG, maxG);
            character.PlusGold(gold);
            return gold;
        }
        public static void HandleExpireItem(Model.Item.Item itemExpire)
        {
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
            var timeServer = ServerUtils.CurrentTimeSecond();
            if (expireDay > 0)
            {
                expireTime = (int)(timeServer + (expireDay * DataCache._1DAYBYSECOND));
            }
            var optionHiden = itemExpire.Options.FirstOrDefault(option => option.Id == 73);
            itemExpire.Options.Add(new OptionItem()
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
                itemExpire.Options.Add(new OptionItem()
                {
                    Id = 73,
                    Param = expireTime,
                });
            }
        }
        private static void UseGoiQuaDacBiet2024(Model.Character.Character character, Model.Item.Item itemUse)
        {
            var item = ItemCache.GetItemDefault(ItemCache.ItemGoiQuaDacBiet2024[ServerUtils.RandomNumber(ItemCache.ItemGoiQuaDacBiet2024.Count)]);
            var itemTemplate = ItemCache.ItemTemplate(item.Id);
            if (ItemCache.ItemAmulet.Contains(item.Id))
            {
                HandleAmulet(character, item.Id);
                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + itemTemplate.Name + " 1 giờ"));
            }
            else if (item.Id is 76)
            {
                var g = HandleRandGold(character, 50000, 500000);
                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + g + " vàng"));
            }
            else
            {
                character.CharacterHandler.AddItemToBag(true, item, "Nhận được từ gói quà đặc biệt 2024");
                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + itemTemplate.Name));
            }

            if (itemTemplate.Type == 5 || itemTemplate.Type == 11 || (itemTemplate.Type == 27 && DataCache.ListPetID.Contains(item.Id))) { 
                    HandleExpireItem(item);
                }
            character.CharacterHandler.SendMessage(Service.SendCombinne6(ItemCache.ItemTemplate(itemUse.Id).IconId, itemTemplate.IconId));
            character.CharacterHandler.RemoveItemBagByIndex(itemUse.IndexUI, 1, reason: "Sử dụng gói quà đặc biệt 2024");
            character.CharacterHandler.SendMessage(Service.SendBag(character));

        }
        private static void UseThiepChucMungLongChau(Model.Character.Character character, Model.Item.Item itemUse)
        {
            var item = ItemCache.GetItemDefault(ItemCache.ItemThiepChucMungLongChau[ServerUtils.RandomNumber(ItemCache.ItemThiepChucMungLongChau.Count)]);
            var itemTemplate = ItemCache.ItemTemplate(item.Id);
            if (ItemCache.ItemAmulet.Contains(item.Id))
            {
                HandleAmulet(character, item.Id);
                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + itemTemplate.Name + " 1 giờ"));
            }

            else
            {
                character.CharacterHandler.AddItemToBag(true, item, "Nhận được từ thiệp chúc mừng long châu");
                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + itemTemplate.Name));
            }
            if (itemTemplate.Type == 5 || itemTemplate.Type == 11 || (itemTemplate.Type == 27 && DataCache.ListPetID.Contains(item.Id)))
            {
                HandleExpireItem(item);
            }
            character.CharacterHandler.SendMessage(Service.SendCombinne6(ItemCache.ItemTemplate(itemUse.Id).IconId, itemTemplate.IconId));
            character.CharacterHandler.RemoveItemBagByIndex(itemUse.IndexUI, 1, reason: "Sử dụng thiệp chúc mừng long châu");
            character.CharacterHandler.SendMessage(Service.SendBag(character));

        }
        private static void UsePhongBiTet2024(Model.Character.Character character, Model.Item.Item itemUse)
        {
            var item = ItemCache.GetItemDefault(ItemCache.ItemPhongBiTet2024[ServerUtils.RandomNumber(ItemCache.ItemPhongBiTet2024.Count)]);
            var itemTemplate = ItemCache.ItemTemplate(item.Id);
            character.InfoEvent.PhongBiTet2024++;
            if (ItemCache.ItemAmulet.Contains(item.Id))
            {
                HandleAmulet(character, item.Id);
                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + itemTemplate.Name + " 1 giờ"));
            }
            
            else
            {
                character.CharacterHandler.AddItemToBag(true, item, "Nhận được từ phong bì tết 2024");
                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + itemTemplate.Name));
            }

            if (itemTemplate.Type == 5 || itemTemplate.Type == 11 || (itemTemplate.Type == 27 && DataCache.ListPetID.Contains(item.Id)))
            {
                HandleExpireItem(item);
            }
            character.CharacterHandler.SendMessage(Service.SendCombinne6(ItemCache.ItemTemplate(itemUse.Id).IconId, itemTemplate.IconId));
            character.CharacterHandler.RemoveItemBagByIndex(itemUse.IndexUI, 1, reason: "Sử dụng phong bì tết 2024");
            character.CharacterHandler.SendMessage(Service.SendBag(character));

        }
        private static void UseCapsuleTet2024(Model.Character.Character character, Model.Item.Item itemUse)
        {
            var item = ItemCache.GetItemDefault(ItemCache.ItemCapsuleTet2024[ServerUtils.RandomNumber(ItemCache.ItemCapsuleTet2024.Count)]);
            var itemTemplate = ItemCache.ItemTemplate(item.Id);
            if (ItemCache.ItemAmulet.Contains(item.Id))
            {
                HandleAmulet(character, item.Id);
                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + itemTemplate.Name + " 1 giờ"));
            }
            else if (item.Id is 76)
            {
                var g = HandleRandGold(character, 50000, 500000);
                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + g + " vàng"));
            }
            else
            {
                character.CharacterHandler.AddItemToBag(true, item, "Nhận được từ capsule tết 2024");
                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + itemTemplate.Name));
            }

            if (itemTemplate.Type == 5 || itemTemplate.Type == 11 || (itemTemplate.Type == 27 && DataCache.ListPetID.Contains(item.Id)))
            {
                HandleExpireItem(item);
            }
            character.CharacterHandler.SendMessage(Service.SendCombinne6(ItemCache.ItemTemplate(itemUse.Id).IconId, itemTemplate.IconId));
            character.CharacterHandler.RemoveItemBagByIndex(itemUse.IndexUI, 1, reason: "Sử dụng capsule tết 2024");
            character.CharacterHandler.SendMessage(Service.SendBag(character));


        }
        private static void UseHopQuaTet2024(Model.Character.Character character, Model.Item.Item itemUse)
        {
            var itemId = ItemCache.ItemHopQuaTet2024[ServerUtils.RandomNumber(ItemCache.ItemHopQuaTet2024.Count)];
            if (itemId == 227)
            {
                itemId = (short)(227 + character.InfoChar.Gender);
            }
            var item = ItemCache.GetItemDefault(itemId);
            var itemTemplate = ItemCache.ItemTemplate(item.Id);
            if (ItemCache.ItemAmulet.Contains(item.Id))
            {
                HandleAmulet(character, item.Id);
                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + itemTemplate.Name + " 1 giờ"));
            }
            else if (DataCache.ListSaoPhaLe.Contains(item.Id))

            {
                item.Quantity = ServerUtils.RandomNumber(1, 5);
                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + itemTemplate.Name));

            }
            else
            {
                character.CharacterHandler.AddItemToBag(true, item, "Nhận được từ hộp quà tết 2024");
                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được " + itemTemplate.Name));
            }

            if (itemTemplate.Type == 5 || itemTemplate.Type == 11 || (itemTemplate.Type == 27 && DataCache.ListPetID.Contains(item.Id)))
            {
                HandleExpireItem(item);
            }
            character.CharacterHandler.RemoveItemBagByIndex(itemUse.IndexUI, 1, reason: "Sử dụng hộp quà tết 2024");
            character.CharacterHandler.SendMessage(Service.SendBag(character));

        }
        public static void UseAuraItem(Model.Character.Character character, Model.Item.Item item)
        {
            
            var itemMap = new ItemMap(-2, item);
            var vetinh = character.Zone.ZoneHandler.GetItemInMap(22).Count;
            if (vetinh >= 3) { character.CharacterHandler.SendMessage(Service.ServerMessage("Map đã đạt giới hạn đặt 3 vệ tinh")); return; }
            itemMap.AuraPlayerId = character.Id;
            itemMap.X = character.InfoChar.X;
            itemMap.Y = character.InfoChar.Y;
            itemMap.R = 200;
          //  character.InfoChar.VeTinh += 1;
            character.Zone.ZoneHandler.LeaveItemMap(itemMap);
            character.CharacterHandler.RemoveItemBagByIndex(item.IndexUI, 1, reason:"Dùng vệ tinh");
            character.CharacterHandler.SendMessage(Service.SendBag(character));
        }

        private static void UseDragonBall(Model.Character.Character character, Model.Item.Item item)
        {
            var timeServer = ServerUtils.CurrentTimeMillis();
            var dragon = ShenlongDragon.gI().GetShenlong(character.Id);
            if (dragon != null && !dragon.ConditionWish())
            {
                var delay = (dragon.Delay - timeServer) / 1000;
                if (delay < 1)
                {
                    delay = 1;
                }

                character.CharacterHandler.SendMessage(
                    Service.ServerMessage(string.Format(TextServer.gI().DELAY_CALL_DRAGON_SEC,
                        delay)));
                return;
            }else if (dragon == null)
            {
                ShenlongDragon.gI().Add(character);
                dragon = ShenlongDragon.gI().GetShenlong(character.Id);
            }
            

           

            //if (character.InfoChar.CountGoiRong >= DataCache.LIMIT_SO_LAN_GOI_RONG[(character.InfoChar.IsPremium ? 1 : 0)])
            //{
            //    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().LIMIT_GOI_RONG));
            //    return;
            //}
            // Kiểm tra có đứng ở đúng làng không
            if ((character.InfoChar.Gender == 0 && character.InfoChar.MapId != 0) ||
                (character.InfoChar.Gender == 1 && character.InfoChar.MapId != 7) ||
                (character.InfoChar.Gender == 2 && character.InfoChar.MapId != 14))
            {
                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().INVALID_PLACE_CALL_DRAGON));
                return;
            }

            for (int dball = 14; dball <= 20; dball++)
            {
                if (character.CharacterHandler.GetItemBagById(dball) == null)
                {
                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_GENDER));
                    return;
                }
            }
            for (short dball = 14; dball <= 20; dball++)
            {
                character.CharacterHandler.RemoveItemBagById(dball, 1, reason: "Gọi rồng");
            }
            // Gọi rồng thần
            //MapManager.IdPlayerCallDragon = character.Id;
            //MapManager.delayCallDragon = timeServer + 600000;
            //MapManager.timeUoc = 180000 + timeServer;
            //MapManager.SetDragonAppeared(true);
            dragon.Wish();
            character.CharacterHandler.SendMessage(Service.SendBag(character));
            character.CharacterHandler.SendZoneMessage(Service.CallDragon(0, 0, character));
            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(24, MenuNpc.Gi().TextRongThan, MenuNpc.Gi().MenuDieuUocRongThan, 3));
            character.TypeMenu = 0;
            GameCache.gI().HandleBanner(character.Id, 1, BANNER_ENUM.WishDragon);
        }
        private static void UsePetItem(Model.Character.Character character, Model.Item.Item item)
        {
            //if (character.Zone.Map.IsMapCustom())
            //{
            //    character.CharacterHandler.SendMessage(Service.ServerMessage(string.Format("Không thể sử dụng linh thú tại đây")));
            //    return;
            //}

            //var petImeiOption = item.Options.FirstOrDefault(option => option.Id == 73);

            //if (petImeiOption == null)
            //{
            //    character.CharacterHandler.SendMessage(Service.ServerMessage(string.Format("Linh thú đã bị lỗi, vui lòng liên hệ Admin")));
            //    return;
            //}

            //var pet = character.Pet;

            //if (pet != null)
            //{
            //    if (petImeiOption.Param == character.InfoChar.PetImei && item.Id == character.InfoChar.PetId)
            //    {
            //        Cất pet
            //        character.Zone.ZoneHandler.RemovePet(pet);
            //        character.InfoChar.PetId = -1;
            //        character.InfoChar.PetImei = -1;
            //        character.InfoMore.PetItemIndex = -1;
            //        character.Pet = null;
            //        character.CharacterHandler.SetUpInfo();
            //    }
            //    else
            //    {
            //        character.CharacterHandler.SendMessage(Service.ServerMessage(string.Format("Bạn đang có linh thú bên ngoài")));
            //    }
            //    return;
            //}

            //pet = new Pet(item.Id, character);
            //character.Pet = pet;
            //character.Zone.ZoneHandler.AddPet(pet);

            //character.InfoChar.PetId = item.Id;
            //character.InfoChar.PetImei = petImeiOption.Param;
            //character.InfoMore.PetItemIndex = item.IndexUI;
            //character.CharacterHandler.SetUpInfo();
        }

        private static SkillTemplate UseSkill(Model.Character.Character character, Model.Item.Item item)
        {
            var itemTemplate = ItemCache.ItemTemplate(item.Id);
            if (itemTemplate.Gender != character.InfoChar.Gender && itemTemplate.Gender != 3)
            {
                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().DONOT_USE_SKILL));
                return null;
            }

            var skillTemplate = Cache.Gi().SKILL_TEMPLATES.FirstOrDefault(skill => skill.Id == itemTemplate.Skill);
            if (skillTemplate == null) return null;
            {
                var levelSkillBook = itemTemplate.Level;
                var skillChar = character.Skills.FirstOrDefault(skill => skill.Id == skillTemplate.Id);
                if (skillChar != null)
                {
                    if (levelSkillBook <= skillChar.Point)
                    {
                        character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().DUPLICATE_USE_SKILL));
                        return null;
                    }
                    if (levelSkillBook - skillChar.Point != 1)
                    {
                        character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().CANT_YET_USE_SKILL));
                        return null;
                    }
                    return skillTemplate;
                }

                if (character.BoughtSkill.Contains(item.Id))
                {
                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().DUPLICATE_USE_SKILL));
                    return null;
                }
                switch (itemTemplate.Skill)
                    {
                        case 21:
                        {
                            var skilCharCheck = character.Skills.FirstOrDefault(skill => skill.Id == 13);
                            if (skilCharCheck == null || skilCharCheck.Point < 7)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().CANT_YET_USE_SKILL));
                                return null;
                            }
                            break;
                        }
                        case 18:
                        {
                            var skilCharCheck = character.Skills.FirstOrDefault(skill => skill.Id == 12);
                            if (skilCharCheck == null || (skilCharCheck?.Point < 7))
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().CANT_YET_USE_SKILL));
                                return null;
                            }
                            break;
                        }
                        case 22:
                        {
                            var skilCharCheck = character.Skills.FirstOrDefault(skill => skill.Id == 9);
                            if (skilCharCheck == null || (skilCharCheck.Point < 7))
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().CANT_YET_USE_SKILL));
                                return null;
                            }
                            break;
                        }
                    }

                if (itemTemplate.Level == 1)
                {
                    return skillTemplate;
                }
                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().CANT_YET_USE_SKILL));
                return null; 
            }
        }

        public static void AddLearnSkill(Model.Character.Character character, Model.Item.Item item, SkillTemplate skillTemplate)
        {
            var itemTemplate = ItemCache.ItemTemplate(item.Id);
            var levelBook = itemTemplate.Level;
            var skillChar = character.Skills.FirstOrDefault(skill => skill.Id == skillTemplate.Id);
            if (skillChar != null)
            {
                if (levelBook <= skillChar.Point) return;
                if (levelBook - skillChar.Point != 1) return;
                var skillAdd =
                    skillTemplate.SkillDataTemplates.FirstOrDefault(option => option.Point == levelBook);
                skillChar.SkillId = skillAdd!.SkillId;
                skillChar.CoolDown = 0;
                skillChar.Point++;
                character.CharacterHandler.SendMessage(Service.UpdateSkill((short)skillAdd.SkillId));
                character.BoughtSkill.Add(item.Id);
            }
            else
            {
                var skillAdd =
                    skillTemplate.SkillDataTemplates.FirstOrDefault(option => option.Point == 1);
                if (skillAdd == null) return;
                character.Skills.Add(new SkillCharacter()
                {
                    Id = skillTemplate.Id,
                    SkillId = skillAdd.SkillId,
                    CoolDown = 0,
                    Point = 1,
                });
                character.CharacterHandler.SendMessage(Service.AddSkill((short) skillAdd.SkillId));
                character.BoughtSkill.Add(item.Id);
            }
        }

        public static void UseCard(Model.Character.Character character, Model.Item.Item item, bool isBox = false)
        {
            var radarTemplate = Cache.Gi().RADAR_TEMPLATE.FirstOrDefault(r => r.Id == item.Id);
            if(radarTemplate == null) return;
            // kiểm tra require ở đây
            if (radarTemplate.Require != -1)
            {
                var radarRequireTemplate = Cache.Gi().RADAR_TEMPLATE.FirstOrDefault(r => r.Id == radarTemplate.Require); 
                if(radarRequireTemplate == null) return;
                var cardRequire = character.InfoChar.Cards.GetValueOrDefault(radarTemplate.Require);  // require_level là con cần require để nâng cấp con level cao hơn , SQL Require level là level con c1 để nâng con c2 
                if (cardRequire == null || cardRequire.Level < radarTemplate.RequireLevel)
                {
                    character.CharacterHandler.SendMessage(Service.DialogMessage(string.Format(TextServer.gI().RADAR_REQUIRE, radarRequireTemplate.Name, radarTemplate.RequireLevel))); // check coi con level 1 đủ điểu kiện nâng chưa
                    return;
                }

            }
            var card = character.InfoChar.Cards.GetValueOrDefault(item.Id);
            if (card == null)
            {
                var newCard = new Card()
                {
                    Id = item.Id,
                    Amount = 1,
                    MaxAmount = radarTemplate.Max,
                    Level = -1,
                    Options = radarTemplate.Options.Copy()
                };
                if (character.InfoChar.Cards.TryAdd(newCard.Id, newCard))
                {
                    character.CharacterHandler.SendMessage(Service.Radar3(newCard.Id, newCard.Amount, newCard.MaxAmount));
                    character.CharacterHandler.SendMessage(Service.Radar2(newCard.Id, newCard.Level));
                    if (isBox)
                    {
                        character.CharacterHandler.RemoveItemBoxByIndex(item.IndexUI, 1);
                        character.CharacterHandler.SendMessage(Service.SendBox(character));
                    }
                    else
                    {
                        character.CharacterHandler.RemoveItemBagByIndex(item.IndexUI, 1, reason:"Dùng thẻ sưu tầm");
                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                    }
                }
            }
            else
            {
                if (card.Level >= 2)
                {
                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().LEVEL_CARD_MAX));
                    return;
                }
                card.Amount++;
                if (card.Amount >= card.MaxAmount)
                {
                    card.Amount = 0;
                    if (card.Level == -1)
                    {
                        card.Level = 1;
                    }
                    else 
                    {
                        card.Level++;
                    }
                     
                    character.CharacterHandler.SetUpInfo();
                    character.CharacterHandler.SendMessage(Service.MeLoadPoint(character));
                    character.CharacterHandler.SendMessage(Service.Radar2(card.Id, card.Level));
                }
                character.CharacterHandler.SendMessage(Service.Radar3(card.Id, card.Amount, card.MaxAmount));
                if (isBox)
                {
                    character.CharacterHandler.RemoveItemBoxByIndex(item.IndexUI, 1);
                    character.CharacterHandler.SendMessage(Service.SendBox(character));
                }
                else
                {
                    character.CharacterHandler.RemoveItemBagByIndex(item.IndexUI, 1, reason:"Dùng thẻ sưu tầm");
                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                }
            }
        }
    }
}