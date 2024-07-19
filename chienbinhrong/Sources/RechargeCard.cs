using Application.Constants;
using Newtonsoft.Json;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Threading;
using NgocRongGold.DatabaseManager;
using NgocRongGold.Model.Character;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sources
{
    public class RechargeCard
    {
        public enum Value
        {
            CARD_10_000 = 0,
            CARD_20_000 = 1,
            CARD_50_000 = 2,
            CARD_100_000 = 3,
            CARD_200_000 = 4,
            CARD_500_000 = 5,
        }
        public static string GetRandomVarchar(int count)
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < count; i++)
            {
                stringBuilder.Append(DataCache.varchars[ServerUtils.RandomNumber(DataCache.varchars.Count)]);
            }

            return stringBuilder.ToString();
        }
        public static void Exchange(Character character,string serial, string pin, int value)
        {
            if (!CardExist(serial, pin, value))
            {
                //Thẻ không tồn tại hoặc đã được sử dụng
                character.CharacterHandler.SendMessage(Service.ServerMessage("Thẻ không tồn tại hoặc đã được sử dụng"));
                return;
            }
            int menhgia = value;
            value = value * 2 / 666;//số hồng ngọc nhận được
            character.PlusDiamondLock(value);
            character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn nhận được " + value + " hồng ngọc"));
            character.CharacterHandler.SendMessage(Service.BuyItem(character));
            GameCache.gI().AddOrUpdate(GameCache.gI().Recharges, character.Id, value);
            SaveHistory(character.Player.Id, character, menhgia, serial, pin);
        }
        public static bool CardExist(string serial, string pin, int value)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return false;
                    command.CommandText = $"SELECT `Serial`,`Pin` FROM `recharge_card` WHERE `Serial`= '{serial}' AND `Pin` = '{pin}' AND `Value` = '{value}' AND `Used` = '0'";
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
            return false;
        }            
        public static void SaveHistory(int transId, Character character, int value, string Serial, string Pin)//transid là id của tài khoản
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    command.CommandText = $"insert into `recharge_card_history` (`Trans_id`, `TaiKhoan`,`Nhanvat`,`Value`, `Serial`, `Pin`, `Time`, `Name`) VALUES ('{transId}','{character.Player.Username}','{character.Name}', '{value}', '{Serial}', '{Pin}', CURRENT_TIMESTAMP,'Thẻ World {value}')";
                    command.ExecuteNonQuery();
                    command.CommandText = $"update `recharge_card` SET `Used` = '1' WHERE `Serial` = '{Serial}' AND `Pin` = '{Pin}' AND `value` = '{value}'";
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Saver History Card error: {e.Message}\n{e.StackTrace}");
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }
            }
        }
            public static void Create()
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    for (int i = 0; i < 131; i++)
                    {
                        var cardValue = 10000;
                        if (i >= 100) cardValue = 500000;
                        else if (i >= 80) cardValue = 200000;
                        else if (i >= 60) cardValue = 100000;
                        else if (i >= 40) cardValue = 50000;
                        else if (i >= 20) cardValue = 20000;    
                        command.CommandText = $"insert into `recharge_card` (`Name`, `Value`, `Serial`, `Pin`, `Exchange`, `Used`) VALUES ('Thẻ World {cardValue}', '{cardValue}', '{GetRandomVarchar(8)}', '{GetRandomVarchar(16)}', '{(cardValue/666)*2}', '0')";
                        command.ExecuteNonQuery();
                    } 
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Exchange Card error: {e.Message}\n{e.StackTrace}");
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }
            }
        }
    }
}
