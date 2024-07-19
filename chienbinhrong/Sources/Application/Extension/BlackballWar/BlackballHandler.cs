using Newtonsoft.Json;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
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

namespace NgocRongGold.Application.Extension.BlackballWar
{
    public class BlackBallHandler
    {
        public class ForPlayer
        {

            public Blackball CurrentBlackball { get; set; }
            public List<Blackball> CurrentListBuff { get; set; }
            public int CurrentPercentPlusHp { get; set; }
            public long DelayCollectBlackball { get; set; }
            public long DelaySendMessage { get; set; }
            public static ForPlayer instance;
            public static ForPlayer gI()
            {
                if (instance == null) instance = new ForPlayer();
                return instance;
            }
            public ForPlayer()
            {
                DelaySendMessage = 10000 + ServerUtils.CurrentTimeMillis();
                DelayCollectBlackball = -1;
                CurrentBlackball = new Blackball();
                CurrentListBuff = new List<Blackball>();

                CurrentPercentPlusHp = -1;
            }
            public void Dispose()
            {
                GC.SuppressFinalize(this);
            }
            public Boolean AlreadyPick(Character character)
            {
                return character.Blackball.CurrentBlackball.Star != -1;
            }
            public Boolean AlreadyPlusHp(Character character)
            {
                return character.Blackball.CurrentPercentPlusHp != -1;
            }


            public void PickBlackball(Character character, int itemId)
            {
                var timeserver = ServerUtils.CurrentTimeMillis();

                character.Blackball.CurrentBlackball.Star = itemId - 371;
                character.Blackball.DelayCollectBlackball = timeserver + 600000;
                character.Blackball.DelaySendMessage = timeserver + 10000;
                var second = (character.Blackball.DelayCollectBlackball - timeserver) / 1000;
                character.CharacterHandler.SendMessage(Service.ServerMessage($"{string.Format(BlackballCache.textWaitToWin, second)}"));
                character.InfoChar.Bag = 107;
                character.CharacterHandler.UpdatePhukien();
                character.InfoChar.TypePk = 5;
                character.CharacterHandler.SendZoneMessage(Service.ChangeTypePk(character.Id, 5));
            }
            public void ExitMapOrDie(Character character)
            {
                if (AlreadyPick(character))
                {
                    var Blackball2 = ItemCache.GetItemDefault((short)BlackballCache.ListNRSD[character.Blackball.CurrentBlackball.Star - 1]);
                    var blackball = new ItemMap(-1, Blackball2);
                    blackball.X = character.InfoChar.X;
                    blackball.Y = character.InfoChar.Y;
                    character.Zone.ZoneHandler.LeaveItemMap(blackball);
                    character.InfoChar.Bag = (sbyte)(ClanManager.Get(character.ClanId) != null ? (ClanManager.Get(character.ClanId).ImgId) : -1);

                    character.CharacterHandler.UpdatePhukien();
                    character.CharacterHandler.SendZoneMessage(character.ClanId == -1 || character.ClanId == -100
                   ? Service.SendImageBag(character.Id, -1)
                   : Service.SendImageBag(character.Id, character.InfoChar.Bag));
                }
                character.Blackball.CurrentBlackball.Star = -1;
                character.Blackball.CurrentPercentPlusHp = -1;
                character.InfoChar.TypePk = 0;
                character.CharacterHandler.SendZoneMessage(Service.ChangeTypePk(character.Id, 0));
            }
            public void Runtime(Character character, long timeserver)
            {
                if (BlackballCache.Status == BlackballStatus.CLOSE)
                {
                    if (character.Blackball.AlreadyPick(character))
                    {
                        ForClan.ClanManagerr.gI().Insert(character, character.Blackball.CurrentBlackball);
                    }
                    character.CharacterHandler.SendMessage(Service.ServerMessage(BlackballCache.textEnd));
                    ExitMapOrDie(character);
                    MapManager.JoinMap(character, 24 + character.InfoChar.Gender, ServerUtils.RandomNumber(19), true, true, character.TypeTeleport);
                    Server.Gi().Logger.Print("blackball status: end");
                }
                else
                {
                    if (AlreadyPick(character))
                    {
                        if (character.Blackball.DelayCollectBlackball < timeserver)
                        {

                            character.CharacterHandler.SendMessage(Service.ServerMessage($"{string.Format(BlackballCache.textClanReward, character.Blackball.CurrentBlackball.Star)}"));
                            ForClan.ClanManagerr.gI().Insert(character, character.Blackball.CurrentBlackball);
                            MapManager.JoinMap(character, 24 + character.InfoChar.Gender, ServerUtils.RandomNumber(19), true, true, character.TypeTeleport);
                            ExitMapOrDie(character);
                        }
                        else if (character.Blackball.DelaySendMessage < timeserver)
                        {
                            var second = (character.Blackball.DelayCollectBlackball - timeserver) / 1000;
                            character.CharacterHandler.SendMessage(Service.ServerMessage($"{string.Format(BlackballCache.textWaitToWin, second)}"));
                            character.Blackball.DelaySendMessage = 10000 + timeserver;

                        }
                    }
                }
            }
        }
        public class ForServer
        {
            public static ForServer instance;
            public static ForServer gI()
            {
                if (instance == null) instance = new ForServer();
                return instance;
            }
            public void InitBlackball()
            {
                //Application.Threading.Map mapInit;
                //var item = ItemCache.GetItemDefault(372);
                //for (int i = 0; i < 7; i++)
                //{
                //    mapInit = MapManager.Get(85 + i);
                //    for (int zone = 0; zone < 12; zone++)
                //    {
                //        var zoneInit = mapInit.GetZoneById(zone);
                //        item = ItemCache.GetItemDefault((short)(372 + i));
                //        if (zoneInit.ZoneHandler.GetItemMapsByID(item.Id).Count >= 1)
                //        {
                //            for (int nrsd = 0; nrsd < zoneInit.ZoneHandler.GetItemMapsByID(item.Id).Count; nrsd++)
                //            {
                //                zoneInit.ZoneHandler.RemoveItemMap(zoneInit.ZoneHandler.GetItemMapsByID(item.Id)[nrsd].Id);
                //                Thread.Sleep(50);
                //            }
                //        }
                //        if (i == 3)
                //        {
                //            zoneInit.ItemMaps.TryAdd(0, new ItemMap(-1)
                //            {
                //                Id = 0,
                //                PlayerId = -1,
                //                Item = item,
                //                X = 1031,
                //                Y = 336,
                //            });
                //        }
                //        else if (i == 5)
                //        {
                //            zoneInit.ItemMaps.TryAdd(0, new ItemMap(-1)
                //            {
                //                Id = 0,
                //                PlayerId = -1,
                //                Item = item,
                //                X = 760,
                //                Y = 240,
                //            });
                //        }
                //        else
                //        {
                //            zoneInit.ItemMaps.TryAdd(0, new ItemMap(-1)
                //            {
                //                Id = 0,
                //                PlayerId = -1,
                //                Item = item,
                //                X = 1031,
                //                Y = 360,
                //            });
                //        }
                //    }
                //}
            }
        }
       
        public class ForClan
        {

            public class ClanDatabase
            {
                public static ClanDatabase instance;
                public static ClanDatabase gI()
                {
                    if (instance == null) instance = new ClanDatabase();
                    return instance;
                }
                public static void Update(int ClanId)
                {
                    lock (Server.SQLLOCK)
                    {

                        var clan = Application.Manager.ClanManager.Get(ClanId);
                        var text = $"`DataBlackball` = '{JsonConvert.SerializeObject(clan.DataBlackBall)}'";
                        DbContext.gI()?.ConnectToAccount();
                        using DbCommand command = DbContext.gI()?.Connection.CreateCommand();
                        if (command == null) return;
                        command.CommandText = $"UPDATE `clan` SET {text}  WHERE `id` = {ClanId};";
                        command.ExecuteNonQuery();
                        DbContext.gI()?.CloseConnect();
                    }
                }
            }
            public class ClanManagerr
            {
                public List<Blackball> ListCurrentBlackball = new List<Blackball>();
                public static ClanManagerr instance;
                public static ClanManagerr gI()
                {
                    if (instance == null) instance = new ClanManagerr();
                    return instance;
                }
                public ClanManagerr()
                {
                    ListCurrentBlackball = new List<Blackball>();

                }
                public void Insert(Character character, Blackball ball)
                {
                    var clan = Manager.ClanManager.Get(character.ClanId);
                    if (clan.DataBlackBall.ListCurrentBlackball.Contains(ball) || ball.Star == -1) return;
                    ball.Time = DataCache._1DAY + ServerUtils.CurrentTimeMillis();
                    clan.DataBlackBall.ListCurrentBlackball.Add(new Blackball() { Star = ball.Star, Time = ball.Time});
                    ClanDatabase.Update(clan.Id);
                }


            }
        }
        public class ForNpc
        {
            public class Omega_Dragon
            {
              
                public static void OpenMenuOmega_Dragon(Character character, int npcId)
                {
                    var now = ServerUtils.TimeNow().Hour;
                    var @char = character.CharacterHandler;
                    if (now == BlackballCache.currTimeStartBlackBall && character.ClanId != -1)
                    {
                        @char.SendMessage(Service.OpenUiConfirm((short)npcId, "Đường đến với ngọc rồng sao đen đã mở, ngươi có muốn tham gia không?", BlackballCache.Menus[0], character.InfoChar.Gender));
                        character.TypeMenu = 0;
                    }
                    else if (now == 21 && character.ClanId != -1)
                    {
                        var clan = Application.Manager.ClanManager.Get(character.ClanId);
                        if (clan.DataBlackBall.ListCurrentBlackball.Count >= 1)
                        {
                            var menu = new List<string>();
                            character.ListCollectBlackBall.Clear();

                            var filteredBlackBalls = clan.DataBlackBall.ListCurrentBlackball
     .Where(bb => !character.Blackball.CurrentListBuff.Any(blackball => blackball.Star == bb.Star))
     .ToList();

                            filteredBlackBalls.ForEach(blackball =>
                            {

                                menu.Add($"Ngọc rồng\n{blackball.Star} sao");
                                character.ListCollectBlackBall.Add(blackball);

                            });
                        @char.SendMessage(Service.OpenUiConfirm((short)npcId, "Bang hội của ngươi có vài phần thưởng này!\nNgươi có muốn nhận không?", menu, character.InfoChar.Gender));
                            character.TypeMenu = 1;
                        }
                        else
                        {
                            @char.SendMessage(Service.OpenUiConfirm((short)npcId, "Ta có thể giúp gì cho ngươi?", BlackballCache.Menus[1], character.InfoChar.Gender));
                            character.TypeMenu = 2;
                        }
                    }
                    else
                    {
                        @char.SendMessage(Service.OpenUiConfirm((short)npcId, "Ta có thể giúp gì cho ngươi?", BlackballCache.Menus[1], character.InfoChar.Gender));
                        character.TypeMenu = 2;
                    }
                }

                public static void Confirm(Character character, int npcId, int select)
                {
                    switch (character.TypeMenu)
                    {
                        case 0:
                            switch (select)
                            {
                                case 0: // huong dan them

                                    character.CharacterHandler.SendMessage(Service.OpenUiSay((short)npcId, BlackballCache.Tutorial));
                                    break;
                                case 1: // open menu capsule 
                                    character.SetMapNRSD();
                                    character.CharacterHandler.SendMessage(Service.MapTranspot(character.MapTranspots));
                                    break;
                            }
                            break;
                        case 1:
                            var clan = Application.Manager.ClanManager.Get(character.ClanId);
                            var blackBall = character.ListCollectBlackBall[select];
                            character.Blackball.CurrentListBuff.Add(blackBall);
                            character.CharacterHandler.SetUpInfo(true);
                            character.CharacterHandler.SendMessage(Service.MeLoadPoint(character));
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Chúc mừng bạn đã nhận được Ngọc Rồng" + character.ListCollectBlackBall[select].Star + " sao"));
                            break;
                    }
                }
            }
            public class Rong_nSaoDen
            {
                public static List<List<String>> Menus = new List<List<String>>()
                {
                    new List<String> { "Phù hộ", "Từ chối" },
                    new List<String> { "X3 HP\n10 Ngọc", "X5 HP\n20 Ngọc", "X7 HP\n30 Ngọc", "Từ chối" },
                    new List<String> {  "Từ chối" },
                };
                public static void OpenMenuRong_nSaoDen(Character character, int npcId)
                {
                    if (character.Blackball.AlreadyPick(character))
                    {
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm((short)npcId, "Ta có thể giúp gì cho ngươi?", Menus[0], character.InfoChar.Gender));
                        character.TypeMenu = 0;

                    }
                    else
                    {
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm((short)npcId, "Ta có thể giúp gì cho ngươi?", Menus[2], character.InfoChar.Gender));
                        character.TypeMenu = 2;
                    }
                }
                public static void ConfirmRong_nSaoDen(Character character, int npcId, int select)
                {
                    switch (character.TypeMenu)
                    {
                        case 0:
                            switch (select)
                            {
                                case 0:
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm((short)npcId, "Ta sẽ giúp ngươi tăng HP và KI lên mức kinh hoàng,ngươi hãy chọn đi", Menus[1], character.InfoChar.Gender));
                                    character.TypeMenu = 1;
                                    break;
                            }
                            break;
                        case 1:
                            switch (select)
                            {
                                case 0:
                                case 1:
                                case 2:
                                    if (character.AllDiamond() < BlackballCache.CostToPlusHp[select])
                                    {
                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn còn thiếu " + (BlackballCache.CostToPlusHp[select] - character.AllDiamond()) + " ngọc nữa !"));
                                        return;
                                    }
                                    if (character.Blackball.CurrentPercentPlusHp == BlackballCache.PercentPlusHp[select])
                                    {
                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Ngươi đã phù hộ hủy diệt rồi !"));
                                        return;
                                    }
                                    character.MineGold(BlackballCache.CostToPlusHp[select]);
                                    character.Blackball.CurrentPercentPlusHp = BlackballCache.PercentPlusHp[select];
                                    character.CharacterHandler.SetUpInfo(true);
                                    character.CharacterHandler.SendMessage(Service.MeLoadPoint(character));
                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Hủy diệt tụi nó thôi nào ehehe !"));
                                    break;

                            }
                            break;
                    }
                }
            }
        }

    }
}
