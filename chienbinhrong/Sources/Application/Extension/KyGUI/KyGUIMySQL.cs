using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Threading;
using NgocRongGold.DatabaseManager;
using NgocRongGold.Model.Option;

namespace NgocRongGold.Application.Extension.Ký_gửi
{
    

    public class KyGUIMySQL{
        public static bool UpdateAllItem()
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToData();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return false;
                    command.CommandText = "TRUNCATE TABLE `shop kí gửi`";
                    command.ExecuteNonQuery();
                    var listItem = CollectionsMarshal.AsSpan(Cache.Gi().KY_GUI_ITEMS.ToList());
                    for (int i = 0; i < listItem.Length; i++)
                    {
                        var item = listItem[i];
                        command.CommandText = $"insert into `shop kí gửi` (`item_id`, `player_id`, `tab`, `page`,`id`, `buyType`, `Cost`, `quantity`, `itemOptions`, `isUpTop`, `isBuy`, `NamePlayer`) values ('{item.ItemId}', '{item.IdPlayerSell}', '{item.Tab}', '{item.Page}','{item.Id}', '{item.BuyType}', '{item.Cost}', '{item.quantity}', '{JsonConvert.SerializeObject(item.Options)}', '0', '{item.isBuy}', '{item.PlayerName}');";
                        command.ExecuteNonQuery();
                    }
                    return true;
                }catch(Exception e)
                {
                    Server.Gi().Logger.Error($"Updateallitem ki gui error: {e.Message}\n{e.StackTrace}");
                    return false;
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }
            }
        }
       
        
        
    }
}