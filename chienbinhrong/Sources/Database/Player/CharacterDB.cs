using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using Newtonsoft.Json;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Threading;
using NgocRongGold.Model.BangXepHang;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Info;
using NgocRongGold.Model.Item;
using NgocRongGold.Model.SkillCharacter;
using NgocRongGold.Application.Extension.Chẵn_Lẻ_Momo;
using NgocRongGold.Model.Task;
using static NgocRongGold.Application.Extension.Super_Champion.SieuHang;
using System.IO;
using static NgocRongGold.Application.Extension.BlackballWar.Blackball;
using NgocRongGold.Application.Extension.BlackballWar;
using NgocRongGold.Application.Extension.DaiHoiVoThuat;
using NgocRongGold.Application.Extension.DaiHoiVoThuat.SuperChampion;
using NgocRongGold.Application.Manager;
using NgocRongGold.Logging;
using NgocRongGold.Application.Extension.Practice;
using NgocRongGold.Application.Extension.SideQuest.BoMong;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.Extension.DiedRing;
using NgocRongGold.Application.Extension.SideQuest.HangNgay;

namespace NgocRongGold.DatabaseManager.Player
{
    public class CharacterDB
    {
        public static bool IsAlreadyExist(string nameCheck)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return false;
                    command.CommandText = $"SELECT `id` FROM `character` WHERE `name` = '{nameCheck}'";
                    using var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Check Already Exist Name Character: {e.Message}\n{e.StackTrace}");
                    return false;
                }
                finally
                {
                    DbContext.gI()?.CloseConnect(); 
                }
                
            }
        }
        public static DateTime GetCreateDateById(int id)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return ServerUtils.TimeNow();

                    command.CommandText =
                        $"SELECT `CreateDate` FROM `character` WHERE `id` = {id};";
                    using var reader = command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        return ServerUtils.TimeNow();
                    }
                    while (reader.Read())
                    {
                        
                        return reader.GetDateTime(0);
                    }
                    return ServerUtils.TimeNow();
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"GetById character error: {e.Message}\n{e.StackTrace}");
                    return ServerUtils.TimeNow();
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }
            }
        }
        public static int Create(Character character)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command != null)
                    {
                        var createDate = ServerUtils.TimeNow();
                        command.CommandText =
                        $"INSERT INTO `character` (`Name`, `Skills`, `ItemBody`, `ItemBag`, `ItemBox`, `InfoChar`, `BoughtSkill`, `InfoTask`,`LuckyBox`, `LastLogin`, `CreateDate`, `SpecialSkill`, `DataEvent`,`InfoBuff`, `DataBlackBall`,`DataBoMong`, " +
                        $"`DataDaiHoiVoThuat`, `DataPractice`, `DataSieuHang`, `Me`, `Friends`, `Enemies`, `ItemGift`, `InfoMuaGiai`) VALUES ('{character.Name}', '{JsonConvert.SerializeObject(character.Skills)}', '{JsonConvert.SerializeObject(character.ItemBody)}' , '{JsonConvert.SerializeObject(character.ItemBag)}', '{JsonConvert.SerializeObject(character.ItemBox)}', '{JsonConvert.SerializeObject(character.InfoChar)}', '{JsonConvert.SerializeObject(character.BoughtSkill)}', '{JsonConvert.SerializeObject(character.InfoTask)}'," +
                        $"'{JsonConvert.SerializeObject(character.LuckyBox)}', '{character.LastLogin:yyyy-MM-dd HH:mm:ss}' , '{createDate:yyyy-MM-dd HH:mm:ss}', '{JsonConvert.SerializeObject(character.SpecialSkill)}', '0','{JsonConvert.SerializeObject(character.InfoBuff)}', '[]','{JsonConvert.SerializeObject(character.DataBoMong)}','{JsonConvert.SerializeObject(character.DataDaiHoiVoThuat23)}', '{JsonConvert.SerializeObject(character.DataPractice)}', " +
                        $"'{JsonConvert.SerializeObject(character.DataSieuHang)}', '{JsonConvert.SerializeObject(character.Me)}', " +
                            $"'[]', '[]', '[]', '{JsonConvert.SerializeObject(character.InfoMuaGiai)}'); SELECT LAST_INSERT_ID();";
                        var reader = int.Parse(command.ExecuteScalar()?.ToString() ?? "0");
                        return reader;
                    }
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Create new character error: {e.Message}\n{e.StackTrace}");
                    return 0;
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }
                return 0;
            }
        }
         public static int Create(Character character, int id)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command != null)
                    {
                        var createDate = ServerUtils.TimeNow();
                        command.CommandText =
                            $"INSERT INTO `character` (`id`,`Name`, `Skills`, `ItemBody`, `ItemBag`, `ItemBox`, `InfoChar`, `BoughtSkill`, `InfoTask`,`LuckyBox`, `LastLogin`, `CreateDate`, `SpecialSkill`, `DataEvent`,`InfoBuff`, `DataBlackBall`,`DataBoMong`, " +
                            $"`DataDaiHoiVoThuat`, `DataPractice`, `DataSieuHang`, `Me`, `Friends`, `Enemies`, `ItemGift`, `DataVoDaiSinhTu`, `DataSideTask`, `InfoMuaGiai`) VALUES " +
                            $"('{id}','{character.Name}{id}', '{JsonConvert.SerializeObject(character.Skills)}', '{JsonConvert.SerializeObject(character.ItemBody)}' , '{JsonConvert.SerializeObject(character.ItemBag)}', '{JsonConvert.SerializeObject(character.ItemBox)}', '{JsonConvert.SerializeObject(character.InfoChar)}', '{JsonConvert.SerializeObject(character.BoughtSkill)}', '{JsonConvert.SerializeObject(character.InfoTask)}'," +
                            $"'{JsonConvert.SerializeObject(character.LuckyBox)}', '{character.LastLogin:yyyy-MM-dd HH:mm:ss}' , '{createDate:yyyy-MM-dd HH:mm:ss}', '{JsonConvert.SerializeObject(character.SpecialSkill)}', '0','{JsonConvert.SerializeObject(character.InfoBuff)}','[]','{JsonConvert.SerializeObject(character.DataBoMong)}','{JsonConvert.SerializeObject(character.DataDaiHoiVoThuat23)}', '{JsonConvert.SerializeObject(character.DataPractice)}', " +
                            $"'{JsonConvert.SerializeObject(character.DataSieuHang)}', '{JsonConvert.SerializeObject(character.Me)}', " +
                            $"'[]', '[]', '[]', '{JsonConvert.SerializeObject(character.DataVoDaiSinhTu)}','{JsonConvert.SerializeObject(character.DataSideTask)}');";
                        var reader = int.Parse(command.ExecuteScalar()?.ToString() ?? "0");
                        return reader;
                    }
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Create new character error: {e.Message}\n{e.StackTrace}");
                    return 0;
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }
                return 0;
            }
        }
        public static void Delete(int idChar)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return;
                    command.CommandText =
                        $"DELETE FROM `character` WHERE `id` = {idChar};";
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Create new character error: {e.Message}\n{e.StackTrace}");
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }
            }
        }

        public static bool SaveInventory(Character character, bool saveBag = false,bool saveBody = false, bool saveBox = false, bool saveLuckyBox = false)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    var text = $"`ItemBag` = '{JsonConvert.SerializeObject(character.ItemBag)}'";
                    if (saveBag)
                    {
                        character.Delay.NeedToSaveBag = false;

                    }
                    ////if (character.CharacterHandler.GetItemBagById(457) != null)
                    ////{
                    ////    text += $", `thoivang` = '{character.CharacterHandler.GetItemBagById(457).Quantity}'";
                    ////}
                    if (saveBody)
                    {
                        text += $", `ItemBody` = '{JsonConvert.SerializeObject(character.ItemBody)}'";
                        character.Delay.NeedToSaveBody = false;
                    }

                    if (saveBox)
                    {
                        text += $", `ItemBox` = '{JsonConvert.SerializeObject(character.ItemBox)}'";
                        character.Delay.NeedToSaveBox = false;
                    }

                    if (saveLuckyBox)
                    {
                        text += $", `LuckyBox` = '{JsonConvert.SerializeObject(character.LuckyBox)}'";
                        Server.Gi().Logger.Print("Save Luckybox character id: " + character.Id + " | name: " + character.Name + " success !", "cyan");
                        character.Delay.NeedToSaveLucky = false;
                    }
                    Server.Gi().Logger.Print("Save Inventory character id: " + character.Id + " | name: " + character.Name + " success !", "cyan");
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return false;
                    command.CommandText = $"UPDATE `character` SET {text} WHERE `id` = {character.Id};";
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Update SaveInventory error: {e.Message}\n{e.StackTrace}");
                    return false;
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }
            }
        }

        public static void Update(Character character)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    if (character.InfoChar.Fusion.TimeUse > 0)
                    {
                        character.InfoChar.Fusion.TimeUse = (int)(ServerUtils.CurrentTimeMillis() - character.InfoChar.Fusion.TimeStart);
                    }
                    var text = $"`name` = '{character.Name}'";
                    text += $", `Skills` = '{JsonConvert.SerializeObject(character.Skills)}'";
                    text += $", `ItemBody` = '{JsonConvert.SerializeObject(character.ItemBody)}'";
                    text += $", `ItemBag` = '{JsonConvert.SerializeObject(character.ItemBag)}'";
                    text += $", `ItemBox` = '{JsonConvert.SerializeObject(character.ItemBox)}'";
                    text += $", `InfoChar` = '{JsonConvert.SerializeObject(character.InfoChar)}'";
                    text += $", `BoughtSkill` = '{JsonConvert.SerializeObject(character.BoughtSkill)}'";
                    text += $", `InfoTask` = '{JsonConvert.SerializeObject(character.InfoTask)}'";
                    text += $", `PlusBag` = {character.PlusBag}";
                    text += $", `PlusBox` = {character.PlusBox}";
                    text += $", `Friends` = '{JsonConvert.SerializeObject(character.Friends)}'";
                    text += $", `Enemies` = '{JsonConvert.SerializeObject(character.Enemies)}'";
                    text += $", `Me` = '{JsonConvert.SerializeObject(character.Me)}'";
                    text += $", `ClanId` = {character.ClanId}";
                    text += $", `LuckyBox` = '{JsonConvert.SerializeObject(character.LuckyBox)}'";
                    text += $", `LastLogin` = '{character.LastLogin:yyyy-MM-dd HH:mm:ss}'";
                    text += $", `SpecialSkill` = '{JsonConvert.SerializeObject(character.SpecialSkill)}'";
                    text += $", `InfoBuff` = '{JsonConvert.SerializeObject(character.InfoBuff)}'";
                    text += $", `DataEvent` = '{JsonConvert.SerializeObject(character.DiemSuKien)}'";
                    text += $", `DataBlackBall` = '{JsonConvert.SerializeObject(character.Blackball.CurrentListBuff)}'";
                    text += $", `DataBoMong` = '{JsonConvert.SerializeObject(character.DataBoMong)}'";
                    text += $", `DataDaiHoiVoThuat` = '{JsonConvert.SerializeObject(character.DataDaiHoiVoThuat23)}'";
                    text += $", `DataPractice` = '{JsonConvert.SerializeObject(character.DataPractice)}'";
                    text += $", `DataSieuHang` = '{JsonConvert.SerializeObject(character.DataSieuHang)}'";
                    text += $", `ItemGift` = '{JsonConvert.SerializeObject(character.ItemGift)}'";
                    text += $", `DataVoDaiSinhTu` = '{JsonConvert.SerializeObject(character.DataVoDaiSinhTu)}'";
                    text += $", `DataSideTask` = '{JsonConvert.SerializeObject(character.DataSideTask)}'";
                    text += $", `InfoMuaGiai` = '{JsonConvert.SerializeObject(character.InfoMuaGiai)}'";
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return;
                    command.CommandText = $"UPDATE `character` SET {text}  WHERE `id` = {character.Id};";
                    command.ExecuteNonQuery();
                    Server.Gi().Logger.Print("Save character " + character.Name + ", id: " + character.Id + " success", "cyan");
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Print("Save character " + character.Name + " | id: " + character.Id + " failed !", "red");
                    ServerUtils.WriteLog("savecharacter/" + character.Name, $"Update character error: {e.Message}\n{e.StackTrace}");
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }
            }
        }

        public static void Update(int charId)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    var text = $"`ClanId` = -1";
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return;
                    command.CommandText = $"UPDATE `character` SET {text}  WHERE `id` = {charId};";
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

        public Character GetByName(string name)
        {
            return null;
        }
        
        public static Character GetById(int id)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return null;
                    command.CommandText =
                        $"SELECT * FROM `character` WHERE `id` = {id};";
                    using var reader = command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        return null;
                    }
                    while (reader.Read())
                    {
                        var character = new Character
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        };
                        character.Skills.AddRange(
                            JsonConvert.DeserializeObject<List<SkillCharacter>>(reader.GetString(2)));
                        character.ItemBody =
                            JsonConvert.DeserializeObject<List<Item>>(reader.GetString(3),
                                DataCache.SettingNull);
                        character.ItemBag = JsonConvert.DeserializeObject<List<Item>>(reader.GetString(4));
                        character.ItemBox = JsonConvert.DeserializeObject<List<Item>>(reader.GetString(5));
                        character.InfoChar =
                            JsonConvert.DeserializeObject<InfoChar>(reader.GetString(6),
                                DataCache.SettingNull);
                        character.BoughtSkill.AddRange(
                            JsonConvert.DeserializeObject<List<int>>(reader.GetString(7)));
                        character.InfoTask = JsonConvert.DeserializeObject<TaskInfo>(reader.GetString(8));
                        character.PlusBag = reader.GetInt32(9);
                        character.PlusBox = reader.GetInt32(10);
                        character.Friends = JsonConvert.DeserializeObject<List<InfoFriend>>(reader.GetString(11)) ?? new List<InfoFriend>();
                        character.Enemies = JsonConvert.DeserializeObject<List<InfoFriend>>(reader.GetString(12)) ?? new List<InfoFriend>();
                        character.Me = JsonConvert.DeserializeObject<InfoFriend>(reader.GetString(13)) ?? new InfoFriend();
                        //12
                        character.ClanId = reader.GetInt32(14);
                        character.LuckyBox = JsonConvert.DeserializeObject<List<Item>>(reader.GetString(15));
                        character.LastLogin = reader.GetDateTime(16);
                        // character.CharacterHandler.SetUpInfo();
                        character.SpecialSkill = JsonConvert.DeserializeObject<SpecialSkill>(reader.GetString(18),
                                DataCache.SettingNull);

                        character.InfoBuff = JsonConvert.DeserializeObject<InfoBuff>(reader.GetString(19),
                                DataCache.SettingNull);
                        character.DiemSuKien = reader.GetInt32(20);
                      
                        character.Blackball.CurrentListBuff = JsonConvert.DeserializeObject<List<Blackball>>(reader.GetString(21),
                                DataCache.SettingNull);
                        character.DataBoMong = JsonConvert.DeserializeObject<BoMongQuest_Data>(reader.GetString(22),
                                DataCache.SettingNull); 
                        character.DataDaiHoiVoThuat23 = JsonConvert.DeserializeObject<ChampionerCS23>(reader.GetString(23),
                                DataCache.SettingNull);
                        character.DataPractice = JsonConvert.DeserializeObject<Practice_Data>(reader.GetString(24),
                                DataCache.SettingNull);
                       // character.DataKyGUI = JsonConvert.DeserializeObject<List<Application.Extension.Ký_gửi.KyGUIItem>>(reader.GetString(27),
                       //         DataCache.SettingNull);
                        character.DataSieuHang = JsonConvert.DeserializeObject<SuperChampion_Championer >(reader.GetString(25));
                        character.ItemGift = JsonConvert.DeserializeObject<List<Item>>(reader.GetString(26));
                        character.DataVoDaiSinhTu = JsonConvert.DeserializeObject<DiedRing_Character>(reader.GetString(27), DataCache.SettingNull);
                        character.DataSideTask = JsonConvert.DeserializeObject<HangNgayQuest_Data>(reader.GetString(28), DataCache.SettingNull);
                        character.InfoMuaGiai = JsonConvert.DeserializeObject<InfoMuaGiai>(reader.GetString(29), DataCache.SettingNull);

                        character.SpecialSkill.ClearTemp();
                        return character;
                    }
                    return null;
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Print($"GetById character error: {e.Message}\n{e.StackTrace}", "yellow");
                    return null;
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }
            }
        }
        public static InfoFriend GetInfo(int id)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return null;
                    command.CommandText =
                        $"SELECT * FROM `character` WHERE `id` = {id};";
                    using var reader = command.ExecuteReader();
                    if (!reader.HasRows) return null;
                    while (reader.Read())
                    {
                        return JsonConvert.DeserializeObject<InfoFriend>(reader.GetString(12),
                            DataCache.SettingNull);
                    }

                    return null;
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Get info character error: {e.Message}\n{e.StackTrace}");
                    return null;
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }
            }
        }
        public static InfoChar GetInfoCharById(int id)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return null;
                    command.CommandText =
                        $"SELECT * FROM `character` WHERE `id` = {id};";
                    using var reader = command.ExecuteReader();
                    if (!reader.HasRows) return null;
                    while (reader.Read())
                    {
                        return JsonConvert.DeserializeObject<InfoChar>(reader.GetString(6),
                            DataCache.SettingNull);
                    }

                    return null;
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Get info character error: {e.Message}\n{e.StackTrace}");
                    return null;
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }
            }
        }

        public static void Update(InfoChar info, int id)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    var text = $"`InfoChar` = '{JsonConvert.SerializeObject(info)}'";
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return;
                    command.CommandText = $"UPDATE `character` SET {text}  WHERE `id` = {id};";
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Update character Infocharr error: {e.Message}\n{e.StackTrace}");
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }
            }
        }

        public static InfoFriend GetInfoCharacter(int id)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return null;
                    command.CommandText =
                        $"SELECT * FROM `character` WHERE `id` = {id};";
                    using var reader = command.ExecuteReader();
                    if (!reader.HasRows) return null;
                    while (reader.Read())
                    {
                        return JsonConvert.DeserializeObject<InfoFriend>(reader.GetString(11),
                            DataCache.SettingNull);
                    }
                    return null;
                }
                catch (Exception )
                {
                   // Server.Gi().Logger.Error($"Create new character error: {e.Message}\n{e.StackTrace}");
                    return null;
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }
            }
        }
        
        
        
        
    }
}