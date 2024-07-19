  using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Threading;
using NgocRongGold.DatabaseManager;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Item;

namespace NgocRongGold.Application.Extension.Ký_gửi
{
    public class KyGUIService{
        public static List<string> TabsName = new List<string>{"Trang bị","Phụ kiện", "Hỗ trợ", "Linh tinh", "Hành trang nhân vật"};
        public static List<int> Items = new List<int> { 1066, 1067, 1068, 1069, 1070, 1316, 1480, 1464 };
        public static List<int> ItemsTabPhuKien = new List<int> { 1066, 1067, 1068, 1069, 1070 };

        public static List<KyGUIItem> getItemKyGui(byte tab, byte page = 0, sbyte gender = 3)
        {
            var kyGUIItems = Cache.Gi().KY_GUI_ITEMS
                .Where(i => i.Tab == tab && !i.isBuy &&
                            ((ItemCache.ItemTemplate(i.Id).Gender == gender || ItemCache.ItemTemplate(i.Id).Gender == 3) || gender == 3)).ToList();

            var lReturn = new List<KyGUIItem>();
            for (int i = 20 * page; i < 20 + 20 * page; i++)
            {
                if (i >= kyGUIItems.Count) break;
                lReturn.Add(kyGUIItems[i]);
            }
            return lReturn;
        }
        public static int GetIndexTab(int typeItem, int idItem)
        {
            switch (idItem)
            {
                case int i when DataCache.IdManhAngel.Contains(i):
                    return 1;
                case 1316://trang sách cũ
                    return 1;
                case 1480 or 1464 or 1481:// đá mài, Hematite, rương spl
                    return 2;
                default:
                    {
                        switch (typeItem)
                        {
                            case >= 0 and <= 4://trang bị
                                return 0;
                            case 12://ngọc rồng
                            case 33://thẻ sưu tầm
                           // case 27:
                                return 1;
                            case 14://đá nâng cấp
                            case 15://mảnh đá vụn
                            case 29://item buff (cuồng nộ, v,v)
                            case 30://all sao pha lê
                                return 2;
                            case 6:// đậu thần
                                return 3;
                        }
                        break;
                    }
            } 
            return -1;
        }
        public static int GetIndexTab2(int typeItem, int idItem)
        {

            switch (typeItem)
            {
                case >= 0 and <= 4://trang bị
                    return 0;
                case 12://ngọc rồng
                case 33://thẻ sưu tầm
                        // case 27:
                    return 1;
                case 14://đá nâng cấp
                case 15://mảnh đá vụn
                case 29://item buff (cuồng nộ, v,v)
                case 30://all sao pha lê
                    return 2;
                case 31:// các vật phẩm sự kiện sử dụng loại buff
                    return 3;
                default:
                    return 3;
            }
        }
        public static List<KyGUIItem> getItemKyGui(byte tab, sbyte gender = 3)
        {
            var kyGUIItems = Cache.Gi().KY_GUI_ITEMS
                .Where(i => i.Tab == tab && !i.isBuy &&
                            ((ItemCache.ItemTemplate(i.Id).Gender == gender || ItemCache.ItemTemplate(i.Id).Gender == 3) || gender == 3));

            return new List<KyGUIItem>(kyGUIItems);
        }
        
        public static bool ItemCanKyGui(Item item)
        {
            var itemTemplate = ItemCache.ItemTemplate(item.Id);
            switch (itemTemplate.Type)
            {
                case 27:
                    return Items.Contains(item.Id);
                case >= 0 and <= 4:
                    return item.isHaveOption(86) || item.isHaveOption(87);
                case 14:
                case 12:
                case 29:
                case 33:
                case 6:
                case 15://mảnh đá vụn
                    return true;
                default:
                    return item.isHaveOption(86) || item.isHaveOption(87);

            }
        }
        public static List<KyGUIItem> getItemCanKyGui(Character character, byte tab, byte page = 0)
        {
            List<KyGUIItem> kyGUIItems = new List<KyGUIItem>();
            character.ItemBag.Where(i => i != null && ItemCanKyGui(i)).ToList().ForEach(item =>
            {
                kyGUIItems.Add(new KyGUIItem()
                {
                    Id = item.Id,
                    ItemId = item.IndexUI,
                    quantity = item.Quantity,
                    isBuy = false,
                    Cost = 0,
                    Tab = 0,
                    BuyType = 0,
                    IdPlayerSell = character.Id,
                    Options = item.Options,
                    IsUpTop = false,
                    Page = 0,
                }) ;
            });
            kyGUIItems.AddRange(Cache.Gi().KY_GUI_ITEMS.Where(i => i.IdPlayerSell == character.Id));
            return kyGUIItems;
        }
        public static Message LoadPage(Character character, byte tab,byte page) //
        {
            var allInTab = getItemKyGui((byte)tab, character.InfoChar.Gender);
            var version = int.Parse(character.Player.Session.Version.Replace(".", ""));
            var temp = getItemKyGui(tab, page, character.InfoChar.Gender);
            var msg = new Message(-100);
            var maxPage = (byte)(allInTab.Count / 20 > 0 ? allInTab.Count / 20 : 1);
            if (20 * maxPage < allInTab.Count)
            {
                maxPage++;
            }
            if (page < 0 || page >= maxPage) return null;
            msg.Writer.WriteByte(tab);
            msg.Writer.WriteByte(maxPage); // max page  (true)
            msg.Writer.WriteByte(page);
            var nItem = 20; 
            //if (page == maxPage - 1)    
            //{
            //    nItem = allInTab.Count - 20 * page;
            //}
            //else
            //{
            //    nItem = 20;
            //}
            msg.Writer.WriteByte(nItem); // count item (true)
            temp.ForEach(item =>
            {
                msg.Writer.WriteShort(item.Id); // id item
                msg.Writer.WriteShort(item.ItemId);//count
                msg.Writer.WriteInt(item.BuyType == 0 ? item.Cost : -1); // vàng
                msg.Writer.WriteInt(item.BuyType == 1 ? item.Cost : -1); // ngọc
                msg.Writer.WriteByte(0);
                if (version >= 222)
                {
                    msg.Writer.WriteInt(item.quantity);
                }
                else
                {
                    msg.Writer.WriteByte(item.quantity);
                }
                msg.Writer.WriteByte(item.IdPlayerSell == character.Id ? 1 : 0); // isMe
                msg.Writer.WriteByte(item.Options.Count);
                item.Options.ForEach(opt =>
                {
                    msg.Writer.WriteByte(opt.Id);
                    msg.Writer.WriteShort(opt.Param);
                });
                msg.Writer.WriteByte(0);
                if (version >= 237)
                {
                    msg.Writer.WriteUTF(item.PlayerName);
                }

            });
            //for (int i = 0; i < 20 - temp.Count; i++)
            //{
            //    msg.Writer.WriteShort(-1); // id item

            //}
            return msg;
        }

        public static Message OpenShopKiGui(Character character) // 0 = ki gui trang bi || 1 = ki gui vp su kien
        {
            var items = getItemCanKyGui(character, 0);
            var version = int.Parse(character.Player.Session.Version.Replace(".", ""));
            var msg = new Message(-44);
            msg.Writer.WriteByte(2); // type shop (2 == shop ki gui) (true)
            msg.Writer.WriteByte(5); // so tab (true)
            for (int i = 0; i < 5; i++)
            {
                if (i == 4)
                {
                    msg.Writer.WriteUTF(TabsName[i]);
                    msg.Writer.WriteByte(0); // max page  (true)
                    msg.Writer.WriteByte(items.Count); // count item
                    items.ForEach(item =>
                    {
                        msg.Writer.WriteShort(item.Id);
                        msg.Writer.WriteShort(item.ItemId);
                        msg.Writer.WriteInt(item.BuyType == 0 ? item.Cost : -1);
                        msg.Writer.WriteInt(item.BuyType == 1 ? item.Cost : -1);
                        msg.Writer.WriteByte(item.isBuy ? 2 : (item.Cost > 0) ? 1 : 0);
                        if (version >= 222)
                        {
                            msg.Writer.WriteInt(item.quantity);
                        }
                        else
                        {
                            msg.Writer.WriteByte(item.quantity);
                        }
                        msg.Writer.WriteByte(1); // isMe
                        msg.Writer.WriteByte(item.Options.Count);
                        item.Options.ForEach(opt =>
                        {
                            msg.Writer.WriteByte(opt.Id);
                            msg.Writer.WriteShort(opt.Param);
                        });
                        msg.Writer.WriteByte(0);
                        msg.Writer.WriteByte(0);
                        if (version >= 237)

                        {
                            msg.Writer.WriteUTF(character.Name);
                        }
                    });
                }
                else
                {
                    var temp = getItemKyGui((byte)i, 0, character.InfoChar.Gender);
                    var allInTab = getItemKyGui((byte)i, character.InfoChar.Gender);
                    var maxPage = (byte)(allInTab.Count / 20 < 1 ? 1 : allInTab.Count / 20);
                    if (20 * maxPage < allInTab.Count)
                    {
                        maxPage++;
                    }
                    msg.Writer.WriteUTF(TabsName[i]);
                    msg.Writer.WriteByte(maxPage); // max page  (true)
                    msg.Writer.WriteByte(temp.Count); // count item (true)
                    temp.ForEach(item =>
                    {
                        msg.Writer.WriteShort(item.Id); // id item
                        msg.Writer.WriteShort(item.ItemId);//count
                        msg.Writer.WriteInt(item.BuyType == 0 ? item.Cost : -1); // vàng
                        msg.Writer.WriteInt(item.BuyType == 1 ? item.Cost : -1); // ngọc
                        msg.Writer.WriteByte(0);
                        if (version >= 222)
                        {
                            msg.Writer.WriteInt(item.quantity);
                        }
                        else
                        {
                            msg.Writer.WriteByte(item.quantity);
                        }
                        msg.Writer.WriteByte(item.IdPlayerSell == character.Id ? 1 : 0); // isMe
                        msg.Writer.WriteByte(item.Options.Count);
                        item.Options.ForEach(opt =>
                        {
                            msg.Writer.WriteByte(opt.Id);
                            msg.Writer.WriteShort(opt.Param);
                        });
                        msg.Writer.WriteByte(0);
                        msg.Writer.WriteByte(0);
                        if (version >= 237)
                        {
                            msg.Writer.WriteUTF(item.PlayerName);
                        }
                    });
                }
            }
            return msg;
        }
    }
}