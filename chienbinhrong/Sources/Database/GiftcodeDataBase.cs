    using Newtonsoft.Json;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Threading;
using NgocRongGold.DatabaseManager;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Item;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NgocRongGold.Model.Info;
using NgocRongGold.Application.Extension;
using System.Net.WebSockets;
namespace Sources.Database
{

    public class GiftcodeDataBase
    {

        public static void RewardGiftcode(Character character, string code)
        {
            code = code.ToLower();
            if (code == "") return;
            else if (code is "tan thu nro")
            {
                if (GetUsedCode(code).Contains(character.Id))
                {
                    character.CharacterHandler.SendMessage(Service.DialogMessage("Bạn đã sử dụng Giftcode này rồi !"));
                    return;
                }
                var itemTanThu = ItemCache.GetItemDefault(1340);//goku ssj4 rare
                character.CharacterHandler.AddItemToBag(false, itemTanThu, "quà tân thủ nro");
                itemTanThu = ItemCache.GetItemDefault(352, 30);//dau cap 8
                character.CharacterHandler.AddItemToBag(false, itemTanThu, "quà tân thủ nro");
                itemTanThu = ItemCache.GetItemDefault(396);//thu cuoi cuc vip
                character.CharacterHandler.AddItemToBag(false, itemTanThu, "quà tân thủ nro");
                itemTanThu = ItemCache.GetItemDefault(1329);//nong dan cham chi
                character.CharacterHandler.AddItemToBag(false, itemTanThu, "quà tân thủ nro");

                UseCode(code, character.Id);
                character.CharacterHandler.SendMessage(Service.SendBag(character));
                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, "|0|Nhập code tân thủ thành công\nChúc bạn chơi game vui vẻ", new List<string> { "OK"}, character.InfoChar.Gender));
                character.TypeMenu = 50;

                return;
            }
            else
            {
                if (!GetCode(code))
                {
                    character.CharacterHandler.SendMessage(Service.DialogMessage("Giftcode không tồn tại"));
                    return;
                }
                if (!CanUse(code))
                {
                    character.CharacterHandler.SendMessage(Service.DialogMessage("Giftcode đã hết lượt nhập hoặc hết hạn"));
                    return;
                }
                if (GetUsedCode(code).Contains(character.Id))
                {
                    character.CharacterHandler.SendMessage(Service.DialogMessage("Bạn đã sử dụng Giftcode này rồi !"));
                    return;
                }
                var text = "";
                var item = GetItem(code);
                var gold = GetThoiVang(code);
                if (character.LengthBagNull() < item.Count + (gold > 0 ?  1 : 0))
                {
                    character.CharacterHandler.SendMessage(Service.ServerMessage("Yêu cầu trống " + item.Count + " ô hành trang"));
                    return;
                }
                var gem = GetGem(code);
                var ruby = GetRuby(code);
                text += $"|0|Bạn đã nhập code: {code} thành công !\n";
                text += $"|7|Bạn nhận được: \n";
                for (int i = 0; i < item.Count; i++)
                {
                    var ItemDefault = ItemCache.GetItemDefault(item[i].Id, item[i].Options, item[i].Quantity);
                    var template = ItemCache.ItemTemplate(ItemDefault.Id);
                    var isTypeBody = template.IsTypeBody();
                    text += $"|2|{(isTypeBody ? "" : item[i].Quantity + " ")}{template.Name}";
                    if (!character.CharacterHandler.AddItemToBag(true, ItemDefault))
                    {
                        text += " (Đã thêm vào Rương)\n";
                        character.CharacterHandler.AddItemToBox(true, ItemDefault);
                    }
                    else
                    {
                        text += "\n";
                    }
                }
                //if (roles != null)
                //{
                //    foreach (var role in roles)
                //    {
                //        character.InfoChar.Roles.Add(role);
                //    }
                //    character.CharacterHandler.SendMessage(Epic_Pet.Clear_Role(character));
                //    character.CharacterHandler.SendMessage(Epic_Pet.Call_Role(character));
                //}
                if (gold > 0)
                {
                    var thoivang = ItemCache.GetItemDefault(457);
                    thoivang.Options.Add(new NgocRongGold.Model.Option.OptionItem()
                    {
                        Id = 30,
                        Param = 0,
                    });
                    thoivang.Quantity = gold;
                    //   character.CharacterHandler.AddItemToBox(true, thoivang);
                    if (!character.CharacterHandler.AddItemToBag(true, thoivang))
                    {
                        text += $"|1|{gold} thỏi vàng (đã thêm vào rương)\n";
                        character.CharacterHandler.AddItemToBox(true, thoivang);
                    }
                    else
                    {
                        text += $"|1|{gold} thỏi vàng (đã thêm vào hành trang)\n";
                    }
                }
                if (gem > 0)
                {
                    character.PlusDiamond(gem);
                    text += $"|1|{gem} Ngọc xanh\n";
                }
                if (ruby > 0)
                {
                    character.PlusDiamondLock(ruby);
                    text += $"|1|{ruby} Hồng ngọc\n";
                }
                UseCode(code, character.Id);
                character.CharacterHandler.SendMessage(Service.SendBag(character));
                character.CharacterHandler.SendMessage(Service.SendBox(character));
                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, $"{text}", new List<string> { "OK" }, character.InfoChar.Gender));
                character.TypeMenu = 50;
            }
        }

        public static void UseCode(string code, int charId)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    var ids = GetUsedCode(code);
                    ids.Add(charId);
                    var text = $"`used` = '{JsonConvert.SerializeObject(ids)}', `count` = '{GetCount(code) -1}'";
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return;
                    command.CommandText = $"UPDATE `giftcode` SET {text}  WHERE `code` = '{code}';";
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Update character error: {e.Message}\n{e.StackTrace}");
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }
            }
        }
        public static List<int> GetUsed(string code)
        {
            List<int> Ids = new List<int>();
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return Ids;
                    command.CommandText = $"SELECT `used` FROM `giftcode` WHERE `code` = '{code}'";
                    using var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        Ids = JsonConvert.DeserializeObject<List<int>>(reader.GetString(0));
                        return Ids;
                    }

                    return Ids;
                }
                catch 
                {

                    return Ids;
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }

            }
        }
        public static string GetRandomVarchar(int count)
        {
            var text = "";
            for (int i = 0; i < count; i++)
            {
                text += $"{DataCache.varchars[ServerUtils.RandomNumber(DataCache.varchars.Count)]}";
            }
            return text;
        }
        public static string CreatePrivateCode(int count)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return "";
                    var randomVarchar = GetRandomVarchar(15);
                    for (int i = 0; i < count; i++)
                    {
                        command.CommandText = $"INSERT INTO `giftcode` (code, count, time_expire, item, gold, gem, ruby, used, type, danhhieu) values ('{randomVarchar}', '1', '{ServerUtils.TimeNow().Date}', '[]', '100', '0', '0', '[]', 1, '[]')";
                        command.ExecuteNonQuery();
                    }
                    return randomVarchar;
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"CheckCharacterAlreadyUsedCode: {e.Message}\n{e.StackTrace}");
                    return "";
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }

            }
        }
        public static bool CanUse(string code)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return false;
                    command.CommandText = $"SELECT `code` FROM `giftcode` WHERE `code`= '{code}' AND count > 0";
                    using var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"CheckCharacterAlreadyUsedCode: {e.Message}\n{e.StackTrace}");
                    return false;
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }

            }
        }
        public static bool GetCode(string code)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return false;
                    command.CommandText = $"SELECT `code` FROM `giftcode` WHERE `code`= '{code}'";
                    using var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"CheckCharacterAlreadyUsedCode: {e.Message}\n{e.StackTrace}");
                    return false;
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }

            }
        }
      
        public static int GetThoiVang(string code)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return -1;
                    command.CommandText = $"SELECT `gold` FROM `giftcode` WHERE `code` = '{code}' AND count > 0";
                    using var reader = command.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                    return -1;
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Check CheckCodeValid: {e.Message}\n{e.StackTrace}");
                    return -1;
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }

            }
        }
        public static int GetGem(string code)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return -1;
                    command.CommandText = $"SELECT `gem` FROM `giftcode` WHERE `code` = '{code}' AND count > 0";
                    using var reader = command.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                    return -1;
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Check CheckCodeValid: {e.Message}\n{e.StackTrace}");
                    return -1;
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }

            }
        }

        public static int GetRuby(string code)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return -1;
                    command.CommandText = $"SELECT `ruby` FROM `giftcode` WHERE `code` = '{code}' AND count > 0 ";
                    using var reader = command.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                    return -1;
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Check CheckCodeValid: {e.Message}\n{e.StackTrace}");
                    return -1;
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }

            }
        }
        public static List<ItemGiftcode> GetItem(string code)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return null;
                    command.CommandText = $"SELECT `item` FROM `giftcode` WHERE `code` = '{code}'";
                    using var reader = command.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        return JsonConvert.DeserializeObject<List<ItemGiftcode>>(reader.GetString(0),
                                DataCache.SettingNull);
                    }
                    return null;
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Check CheckCodeValid: {e.Message}\n{e.StackTrace}");
                    return null;
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }

            }
        }
        public static List<int> GetUsedCode(string code)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return null;
                    command.CommandText = $"SELECT `used` FROM `giftcode` WHERE `code` = '{code}'";
                    using var reader = command.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        return JsonConvert.DeserializeObject<List<int>>(reader.GetString(0),
                                DataCache.SettingNull);
                    }
                    return null;
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Check CheckCodeValid: {e.Message}\n{e.StackTrace}");
                    return null;
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }

            }
        }
        public static int GetCount(string code)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return -1;
                    command.CommandText = $"SELECT `count` FROM `giftcode` WHERE `code` = '{code}'";
                    using var reader = command.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                    return -1;
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Check CheckCodeValid: {e.Message}\n{e.StackTrace}");
                    return -1;
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }

            }
        }
        public static DateTime GetTimeExpire(string code)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return DateTime.Now;
                    command.CommandText = $"SELECT `time_expire` FROM `giftcode` WHERE `code` = '{code}' AND time_expire >= CURRENT_TIMESTAMP";
                    using var reader = command.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        return reader.GetDateTime(0);
                    }
                    return DateTime.Now;
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Check CheckCodeValid: {e.Message}\n{e.StackTrace}");
                    return DateTime.Now;
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }

            }
        }
    }
}
