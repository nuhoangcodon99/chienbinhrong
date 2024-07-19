﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using Newtonsoft.Json;
using NgocRongGold.Application.Threading;
using NgocRongGold.DatabaseManager;
using NgocRongGold.DatabaseManager.Player;
using NgocRongGold.Model.BangXepHang;

namespace NgocRongGold.DatabaseManager
{
    public class BXHDatabase{
        public static void SelectTopTask(int limit)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    var bxh = Server.Gi().BangXepHang;
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return;
                    command.CommandText = $"SELECT `Name`, JSON_EXTRACT(InfoTask, '$.Id') AS Ida,JSON_EXTRACT(InfoTask, '$.Index') AS i,JSON_EXTRACT(InfoTask, '$.Count') AS c,JSON_EXTRACT(Me, '$.Head'), JSON_EXTRACT(Me, '$.Body'), JSON_EXTRACT(Me, '$.Leg'),`id`, JSON_EXTRACT(InfoTask, '$.Time') as Time FROM `character` WHERE (JSON_EXTRACT(InfoTask, '$.Id') > 1) ORDER BY CAST(Ida as Int) DESC, CAST(i as Int) DESC , Cast(c as Int), CAST(Time as Int) ASC LIMIT {limit};";
                    using var reader = command.ExecuteReader();
                    if (!reader.HasRows) return;
                    int i = 1;
                    while (reader.Read())
                    {
                        bxh.TopTasks.Add(new BangXepHang.TopTask()
                        {
                            I = i,
                            Name = reader.GetString(0),
                            TaskId = reader.GetInt16(1),
                            Index   = reader.GetInt16(2),
                            Count = reader.GetInt16(3),
                            Head = reader.GetInt32(4),
                            Body = reader.GetInt32(5),
                            Leg = reader.GetInt32(6),
                            Id = reader.GetInt32(7),
                            Time =reader.GetInt64(8)
                        }) ;
                       // Server.Gi().Logger.Print("Top: " + i + " - Id: " + reader.GetInt16(1) + " - Index: " + reader.GetInt16(2) + " - C: " + reader.GetInt16(3), "red");
                        i++;
                    }
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Error bxh {e.Message}\n{e.StackTrace}");
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }
            }
        }
        public static void SelectTopWhis(int limit)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    var bxh = Server.Gi().BangXepHang;
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return;
                    command.CommandText = $"SELECT `id`,`Name`, JSON_VALUE(DataPractice,'$.Whis.Level') as Level, JSON_VALUE(DataPractice,'$.Whis.HighScore') as Score,JSON_EXTRACT(Me, '$.Head'), JSON_EXTRACT(Me, '$.Body'), JSON_EXTRACT(Me, '$.Leg'),JSON_EXTRACT(DataPractice, '$.Whis.TimeSetScore') FROM `character` WHERE JSON_VALUE(DataPractice,'$.Whis.Level') >= 1 ORDER BY Cast(`Level` as int) DESC, Cast(`Score` as int) limit {limit}";
                    using var reader = command.ExecuteReader();
                    if (!reader.HasRows) return;
                    int i = 1;
                    while (reader.Read())
                    {
                        bxh.TopWhises.Add(new BangXepHang.TopWhis()
                        {
                            Top = i,
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Level = reader.GetInt32(2),
                            Score   = reader.GetDouble(3),
                            Head = reader.GetInt32(4),
                            Body = reader.GetInt32(5),
                            Leg = reader.GetInt32(6),
                            TimeSetScore = reader.GetInt64(7),
                        }) ;
                        i++;
                    }
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Error bxh {e.Message}\n{e.StackTrace}");
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }
            }
        }
        public static void SelectTopClan(int limit)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    var bxh = Server.Gi().BangXepHang;
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return;
                    command.CommandText = $"SELECT `Name`, `LeaderName`, `Capsule Bang` as `Capsule`, `Level` as `Level`, `ImgId`, `Thành viên hiện tại`, `Thành viên tối đa`,JSON_EXTRACT(Leader, '$.Head'),JSON_EXTRACT(Leader, '$.Body'),JSON_EXTRACT(Leader, '$.Leg') FROM clan ORDER BY CAST(Level as INT) DESC, CAST(Capsule as INT) DESC LIMIT {limit}";
                    using var reader = command.ExecuteReader();
                    if (!reader.HasRows) return;
                    int i = 1;
                    while (reader.Read())
                    {
                        bxh.TopClans.Add(new BangXepHang.TopClan()
                        {
                            Top = i,
                            ClanName = reader.GetString(0),
                            LeaderName = reader.GetString(1),
                            Capsule = reader.GetInt32(2),
                            Level = reader.GetInt32(3),
                            ImageId = reader.GetInt32(4),
                            CurrentMember = reader.GetInt32(5),
                            MaxMember = reader.GetInt32(6),
                            Head = reader.GetInt32(7),
                            Body = reader.GetInt32(8),
                            Leg = reader.GetInt32(9),
                        }) ;
                        i++;
                    }
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Error bxh {e.Message}\n{e.StackTrace}");
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }
            }
        }
        public static void SelectTopKhiGas(int limit)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    var bxh = Server.Gi().BangXepHang;
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return;
                    command.CommandText = $"SELECT `Name`, `LeaderName`,JSON_EXTRACT(Leader, '$.Head'),JSON_EXTRACT(Leader, '$.Body'),JSON_EXTRACT(Leader, '$.Leg'), JSON_EXTRACT(ClanDungeon, '$.KhiGasHuyDiet.Record') as lv,JSON_EXTRACT(ClanDungeon, '$.KhiGasHuyDiet.Record') as rc, JSON_EXTRACT(ClanDungeon, '$.KhiGasHuyDiet.LastTimeRecord') as ltr FROM clan ORDER BY CAST(lv as INT) DESC, CAST(rc as INT), CAST(ltr as INT) DESC LIMIT {limit}";
                    using var reader = command.ExecuteReader();
                    if (!reader.HasRows) return;
                    int i = 1;
                    while (reader.Read())
                    {
                        bxh.TopClanKhiGas.Add(new BangXepHang.TopClanKhiGas()
                        {
                            Top = i,
                            ClanName = reader.GetString(0),
                            LeaderName = reader.GetString(1),
                            Head = reader.GetInt32(2),
                            Body = reader.GetInt32(3),
                            Leg = reader.GetInt32(4),
                            Level = reader.GetInt32(5),
                            Record = reader.GetInt64(6),
                            LastTimeRecord = reader.GetInt64(7),
                        });
                        i++;
                    }
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Error bxh {e.Message}\n{e.StackTrace}");
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }
            }
        }
        public static void SelectTopDisciple(int limit)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    var bxh = Server.Gi().BangXepHang;
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return;
                    command.CommandText = $"SELECT detu.`Name`, JSON_EXTRACT(detu.InfoChar, '$.Power') as Power, JSON_EXTRACT(detu.Info, '$.Head') as Head,JSON_EXTRACT(detu.Info, '$.Body') as Body,JSON_EXTRACT(detu.Info, '$.Leg') as Leg,Master.Name as MasterName, Master.id FROM disciple as detu CROSS JOIN `character` As `Master` ON Master.id = -`detu`.id ORDER BY CAST(Power as Int) DESC LIMIT {limit};";
                    using var reader = command.ExecuteReader();
                    if (!reader.HasRows) return;
                    int i = 1;
                    while (reader.Read())
                    {
                        bxh.TopDisciples.Add(new BangXepHang.TopDisciple()
                        {
                            I = i,
                            Name = reader.GetString(0),
                            Power = reader.GetInt64(1),
                            Head = reader.GetInt32(2),
                            Body = reader.GetInt32(3),
                            Leg = reader.GetInt32(4),
                            MasterName = reader.GetString(5),
                            Id = reader.GetInt32(6),
                        }) ;
                        // Server.Gi().Logger.Print("Top: " + i + " - Id: " + reader.GetInt16(1) + " - Index: " + reader.GetInt16(2) + " - C: " + reader.GetInt16(3), "red");
                        i++;
                    }
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Error bxh {e.Message}\n{e.StackTrace}");
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }
            }
        }

        public static void SelectTopEvent(int limit)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    var bxh = Server.Gi().BangXepHang;
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return;
                    command.CommandText = $"SELECT `Name`, JSON_EXTRACT(Me, '$.Head'), JSON_EXTRACT(Me, '$.Body'), JSON_EXTRACT(Me, '$.Leg'), DataEvent AS CharPower, `id` FROM `character` WHERE DataEvent > 0 ORDER BY CAST(CharPower as Int) DESC LIMIT 10;";
                    using var reader = command.ExecuteReader();
                    if (!reader.HasRows) return;
                    int i = 1;
                    while (reader.Read())
                    {

                        bxh.TopEvent.Add(new BangXepHang.TopEvent()
                        {
                            Top = i,
                            Name = reader.GetString(0),
                            Point = reader.GetInt32(4),
                            Head = reader.GetInt32(1),
                            Body = reader.GetInt32(2),
                            Leg = reader.GetInt32(3),
                            PlId = reader.GetInt32(5)
                        });
                        i++;
                    }
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Error bxh {e.Message}\n{e.StackTrace}");
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }
            }
        }
        public static void SelectTopNapThe(int limit)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    var bxh = Server.Gi().BangXepHang;
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return;
                    command.CommandText = "SELECT player.`Name`, account.`tongnap`, JSON_EXTRACT(player.Me, '$.Head'), JSON_EXTRACT(player.Me, '$.Body'), JSON_EXTRACT(player.Me,'$.Leg'), player.`id` FROM `user` AS account CROSS JOIN `character` AS player ON account.`character` = player.id WHERE account.tongnap > 0 ORDER BY Cast(account.`tongnap` as Int) DESC LIMIT 100;";
                    using var reader = command.ExecuteReader();
                    if (!reader.HasRows) return;
                    int i = 1;
                    while (reader.Read())
                    {

                        bxh.TopNaps.Add(new BangXepHang.TopNap()
                        {
                            I = i,
                            Name = reader.GetString(0),
                            TongNap = reader.GetInt32(1),
                            Head = reader.GetInt32(2),
                            Body = reader.GetInt32(3),
                            Leg = reader.GetInt32(4),
                            Id = reader.GetInt32(5)
                        });
                        i++;
                    }
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Error bxh {e.Message}\n{e.StackTrace}");
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }
            }
        }
        public static void SelectTopQuayThuong(int limit)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    var bxh = Server.Gi().BangXepHang;
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return;
                    command.CommandText = $"SELECT `Name`, JSON_EXTRACT(Me, '$.Head'), JSON_EXTRACT(Me, '$.Body'), JSON_EXTRACT(Me, '$.Leg'), JSON_EXTRACT(InfoChar, '$.PointQuayThuong') AS p, `id` FROM `character` WHERE JSON_EXTRACT(InfoChar, '$.PointQuayThuong') > 0 ORDER BY CAST(p as INT) DESC LIMIT {limit};";
                    using var reader = command.ExecuteReader();
                    if (!reader.HasRows) return;
                    int i = 1;
                    while (reader.Read())
                    {

                        bxh.TopQuayThuong.Add(new BangXepHang.TopQuayThuong()
                        {
                            Top = i,
                            Name = reader.GetString(0),
                            Point = reader.GetInt32(4),
                            Head = reader.GetInt32(1),
                            Body = reader.GetInt32(2),
                            Leg = reader.GetInt32(3),
                            PlId = reader.GetInt32(5)
                        });
                        i++;
                    }
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Error bxh {e.Message}\n{e.StackTrace}");
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }
            }
        }
        public static void SelectTopSanBoss(int limit)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    var bxh = Server.Gi().BangXepHang;
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return;
                    command.CommandText = $"SELECT `Name`, JSON_EXTRACT(Me, '$.Head'), JSON_EXTRACT(Me, '$.Body'), JSON_EXTRACT(Me, '$.Leg'), JSON_EXTRACT(InfoChar, '$.PointSanBoss') AS CharPower, `id` FROM `character` WHERE  JSON_EXTRACT(InfoChar, '$.PointSanBoss') > 0 ORDER BY Cast(CharPower as Int) DESC LIMIT {limit};";
                    using var reader = command.ExecuteReader();
                    if (!reader.HasRows) return;
                    int i = 1;
                    while (reader.Read())
                    {

                        bxh.TopSanBoss.Add(new BangXepHang.TopSanBoss()
                        {
                            Top = i,
                            Name = reader.GetString(0),
                            Point = reader.GetInt32(4),
                            Head = reader.GetInt32(1),
                            Body = reader.GetInt32(2),
                            Leg = reader.GetInt32(3),
                            PlId = reader.GetInt32(5)
                        });
                        i++;
                    }
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Error bxh {e.Message}\n{e.StackTrace}");
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }
            }
        }
        public static void SelectTopPower(int limit)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToAccount();
                    var bxh = Server.Gi().BangXepHang;
                    using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                    if (command == null) return;
                    command.CommandText = $"SELECT `Name`, JSON_EXTRACT(Me, '$.Head'), JSON_EXTRACT(Me, '$.Body'), JSON_EXTRACT(Me, '$.Leg'), JSON_EXTRACT(InfoChar, '$.Power') AS CharPower, `id`, JSON_EXTRACT(InfoChar, '$.Potential') AS CharPower2 FROM `character` WHERE (JSON_EXTRACT(InfoChar, '$.Power') > 1200) ORDER BY CAST(CharPower as Int) DESC, CAST(CharPower2 as Int) DESC LIMIT 100;";
                    using var reader = command.ExecuteReader();
                    if (!reader.HasRows) return;
                    int i = 1;
                    while (reader.Read())
                    {

                        bxh.TopPowers.Add(new BangXepHang.TopPower()
                        {
                            I = i,
                            Name = reader.GetString(0),
                            Power = reader.GetInt64(4),
                            Head = reader.GetInt32(1),
                            Body = reader.GetInt32(2),
                            Leg = reader.GetInt32(3),
                            Id = reader.GetInt32(5),
                            TotalPotential = reader.GetInt64(6)
                        }) ;
                        i++;
                    }
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Error bxh {e.Message}\n{e.StackTrace}");
                }
                finally
                {
                    DbContext.gI()?.CloseConnect();
                }
            }
        }
    }
}