using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.Model.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.ConSoMayMan
{
    public class ConSoMayManHandler
    {
        public static ConSoMayManHandler instance;
        public static readonly string HuongDanThem = "Diễn ra mọi lúc, mọi nơi\nMỗi lượt được chọn từ 0-> 99\nthời gian mỗi lượt là 5 phút";
        public static ConSoMayManHandler gI()
        {
            if (instance == null) instance = new ConSoMayManHandler();
            return instance;
        }
        public ConSoMayMan.ConSoMayManConfig Config = new ConSoMayManConfig();
        public bool Result;
        public void Reward(ICharacter character, int value)
        {
            if (character.InfoChar.LuckyNumber.JoinId == Config.RoomId)
            {
                switch (Config.TypeReward)
                {
                    case TypeReward.THOIVANG:
                        var thoivang = ItemCache.GetItemDefault(457, value);
                        character.CharacterHandler.AddItemToBag(true, thoivang);
                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                        break;
                    case TypeReward.GREEN_DIAMOND:
                        ((Character)character).PlusDiamond(value);
                        character.CharacterHandler.SendMessage(Service.BuyItem(character));
                        break;
                    case TypeReward.RED_DIAMOND:
                        ((Character)character).PlusDiamondLock(value);
                        character.CharacterHandler.SendMessage(Service.BuyItem(character));
                        break;
                }
            }
            character?.CharacterHandler.SendMessage(Service.ServerMessage($"Chúc mừng {character.Name} đã thắng {value} thỏi vàng với con số may mắn {character.InfoChar.LuckyNumber.NumberWin}"));

        }
        public void BuyConSoMayMan(Character character, int value)
        {
            if (Config.ConSoMayManStatus == ConSoMayManStatus.DONE) return;
            if (!character.Player.IsActive)
            {
                character.CharacterHandler.SendMessage(Service.ServerMessage("Vui lòng mở thành viên để kích hoạt chức năng này"));
                return;
            }
            if (character.CharacterHandler.GetItemBagById(457) == null || character.CharacterHandler.GetThoiVangInBag() < 5)
            {
                character.CharacterHandler.SendMessage(Service.ServerMessage("Cần ít nhất 5 thỏi vàng trong hành trang"));
                return; 
            }
            if (!Config.PlayersGame.Contains(character.Id))
            {
                Config.PlayersGame.Add(character.Id);
            }
            character.InfoChar.LuckyNumber.Number.Add(value.ToString());
            var text = "";
            for (int i = 0; i < character.InfoChar.LuckyNumber.Number.Count; i++)
            {
                if (i == 0) text += $"{character.InfoChar.LuckyNumber.Number[i]}";
                else if (i == character.InfoChar.LuckyNumber.Number.Count - 1) text += $"{character.InfoChar.LuckyNumber.Number[i]}";
                else text += $", {character.InfoChar.LuckyNumber.Number[i]} ";
            }
            character.CharacterHandler.SendMessage(Service.ShowYourNumber0(text));
            character.CharacterHandler.RemoveItemBagById(457, 5);
            character.CharacterHandler.SendMessage(Service.SendBag(character));
            //character.CharacterHandler.SendMessage(Service.ShowWinNumber0(character.InfoChar.LuckyNumber.Number));
            Config.Joins[1] += 5;
            character.CharacterHandler.SendMessage(Service.ServerMessage($"{value}"));
        }
        public void Start()
        {
            new Thread(new ThreadStart(AutoResult2)).Start();
        }
        public void AutoResult2()
        {
            while (Server.Gi().IsRunning)
            {
                var timeserver = ServerUtils.CurrentTimeMillis();
                switch (Config.ConSoMayManStatus)
                {
                    case ConSoMayManStatus.WAIT:
                        if (Config.timeRemain < timeserver)
                        {
                            Config.LastResult = ServerUtils.RandomNumber(100);
                            Config.ConSoMayManStatus = ConSoMayManStatus.RESULT;
                            Server.Gi().Logger.Print($"SETUP CON SO MAY MAN: [{Config.LastResult}] | STATUS: WAIT, SUCCESS !", "red");
                        }
                        break;
                    case ConSoMayManStatus.RESULT:
                        // Config.PlayersGame.ForEach(id =>
                        // {
                        if (Result) break;
                        Result = true;
                        if (Config.PlayersGame.Count < 1)
                        {
                            Config.timeRemain = 60000 + timeserver;
                            Config.ConSoMayManStatus = ConSoMayManStatus.DONE;
                            Server.Gi().Logger.Print($"SETUP CON SO MAY MAN | STATUS: RESULT, FAILED ! (PlayersGame <= 0)", "red");
                            Result = false;
                            break;
                        }
                        foreach (var id in Config.PlayersGame)
                        {
                            var player = ClientManager.Gi().GetCharacter(id);
                            if (player != null)
                            {
                                player.CharacterHandler.SendMessage(Service.ShowWinNumber1(Config.LastResult.ToString()));
                                if (player.InfoChar.LuckyNumber.Number.Count < 1) continue;
                                foreach (var number in player.InfoChar.LuckyNumber.Number)
                                {
                                    if (!int.TryParse(number, out _))
                                    {
                                        continue;
                                    }
                                    if (int.Parse(number) == Config.LastResult)
                                    {
                                        Config.LastPlayersNameWinGame += $"{player.Name}";
                                        player.InfoChar.LuckyNumber.NumberWin = Config.LastResult;
                                        if (!Config.LastPlayersWinGame.ContainsKey(player.Id))
                                        {
                                            Config.LastPlayersWinGame.TryAdd(player.Id, player);
                                        }
                                    }
                                }
                                player.InfoChar.LuckyNumber.Number.Clear();
                            }
                        }
                        Config.PlayersGame.Clear();
                        if (Config.LastPlayersWinGame.Count < 1)
                        {
                            Config.timeRemain = 60000 + timeserver;
                            Config.ConSoMayManStatus = ConSoMayManStatus.DONE;
                            Server.Gi().Logger.Print($"SETUP CON SO MAY MAN | STATUS: RESULT, FAILED ! (PlayersWin <= 0)", "red");
                            Result = false;
                            break;
                        }
                        //await Task.Delay(11000);
                        var valueGet = Math.Abs((int)Config.Joins[1] - (Config.Joins[1] / 100 * 10) / Config.LastPlayersWinGame.Count) ;
                        foreach (var character in Config.LastPlayersWinGame)
                        {
                            var player = ClientManager.Gi().GetCharacter(character.Value.Id);
                            if (player != null)
                            {
                                Reward(player, (int)valueGet);
                            }
                        }
                        Config.timeRemain = 60000 + timeserver;
                        Config.ConSoMayManStatus = ConSoMayManStatus.DONE;
                        Server.Gi().Logger.Print($"SETUP CON SO MAY MAN | STATUS: RESULT, SUCCESS !", "red");
                        Result = false;
                       // });
                        break;
                    case ConSoMayManStatus.DONE:
                        if (Config.timeRemain < timeserver)
                        {
                            Config.RoomId++;
                            Config.Joins[0] = Config.Joins[1] = 0;
                            Config.LastPlayersWinGame.Clear();
                            Config.timeRemain = 30000 + timeserver;
                            Config.ConSoMayManStatus = ConSoMayManStatus.WAIT;
                            Server.Gi().Logger.Print($"SETUP CON SO MAY MAN | STATUS: DONE, SUCCESS !", "red");
                        }
                        //Server.Gi().Logger.Print($"SETUP CON SO MAY MAN | STATUS: DONE, SUCCESS !", "red");
                        break;
                }
                Thread.Sleep(1000);
            }
            Server.Gi().Logger.Print("Close Con So May Man Success !", "red");
        }
        //public void AutoResultOld(long timeserver)
        //{
        //    switch (Config.ConSoMayManStatus)
        //    {
        //        case ConSoMayManStatus.WAIT:
        //            if (Config.timeRemain < timeserver)
        //            {
        //                var number = Config.FormUpConSo.Split("_");
        //                Config.LastResult = ServerUtils.RandomNumber(int.Parse(number[0]), int.Parse(number[1]));
        //                Config.ConSoMayManStatus = ConSoMayManStatus.RESULT;
        //                Server.Gi().Logger.Print("SETUP CONSO: " + Config.LastResult + " | STATUS: RESULT", "red");
        //            }
        //            break;
        //        case ConSoMayManStatus.RESULT:
        //            Config.LastPlayersWinGame.Clear();
        //            if (Config.PlayersGame.Count < 1) return;
        //            Config.PlayersGame.ForEach(playerid =>
        //            {
        //                //send message quay quay
        //                var player = ClientManager.Gi().GetCharacter(playerid);
        //                if (player != null && player.InfoChar.LuckyNumber.JoinId == Config.RoomId)
        //                {
        //                    player.CharacterHandler.SendMessage(Service.ShowWinNumber1($"{Config.LastResult}"));
        //                    player.InfoChar.LuckyNumber.Number.ForEach(number =>
        //                    {
        //                        var numberSplit = number?.Split("-");
        //                        if (int.Parse(numberSplit[0]) >= Config.LastResult && int.Parse(numberSplit[1]) <= Config.LastResult)
        //                        {
        //                            if (!Config.LastPlayersWinGame.ContainsKey(player.Id))
        //                            {
        //                                player.InfoChar.LuckyNumber.NumberWin = Config.LastResult;
        //                                Config.LastPlayersWinGame.TryAdd(player.Id, player);
        //                            }
        //                        }
        //                    });
        //                    //if (player.InfoChar.LuckyNumber.Number.Contains(","))
        //                    //{
        //                    //    player.InfoChar.LuckyNumber.Number.Split(",").ToList().ForEach(number =>
        //                    //    {
        //                    //        var numberSplit = number.Split("-");
        //                    //        if (int.Parse(numberSplit[0]) >= Config.LastResult && int.Parse(numberSplit[1]) <= Config.LastResult)
        //                    //        {
        //                    //            if (!Config.LastPlayersWinGame.ContainsKey(player.Id))
        //                    //            {
        //                    //                player.InfoChar.LuckyNumber.NumberWin = Config.LastResult;
        //                    //                Config.LastPlayersWinGame.TryAdd(player.Id, player);
        //                    //            }
        //                    //        }
        //                    //    });
        //                    //}
        //                    //else
        //                    //{
        //                    //    var numberSplit = player.InfoChar.LuckyNumber.Number.Split("-");
        //                    //    if (int.Parse(numberSplit[0]) >= Config.LastResult && int.Parse(numberSplit[1]) <= Config.LastResult)
        //                    //    {
        //                    //        if (!Config.LastPlayersWinGame.ContainsKey(player.Id))
        //                    //        {
        //                    //            player.InfoChar.LuckyNumber.NumberWin = Config.LastResult;
        //                    //            Config.LastPlayersWinGame.TryAdd(player.Id, player);
        //                    //        }
        //                    //    }
        //                    //}
        //                    player.InfoChar.LuckyNumber.Number.Clear();
        //                    player.InfoChar.LuckyNumber.JoinId = -1;
        //                    player.CharacterHandler.SendMessage(Service.ShowWinNumber0(player.InfoChar.LuckyNumber.Number));
        //                }
        //            });
        //            Config.TimeResult = 11000 + timeserver;
        //            Config.ConSoMayManStatus = ConSoMayManStatus.DONE;
        //            Server.Gi().Logger.Print("SETUP CONSO | STATUS: DONE", "red");

        //            break;
        //        case ConSoMayManStatus.DONE:
        //            if (Config.TimeResult > timeserver)
        //            {
        //                Config.RoomId++;
        //                Config.PlayersGame.Clear();
        //                if (Config.LastPlayersWinGame.Count < 1) return;

        //                var rewardValue = Math.Abs((int)Config.Joins[1] / Config.LastPlayersWinGame.Count);

        //                Config.LastPlayersWinGame.ToList().ForEach(playerWin =>
        //                {
        //                    Reward(playerWin.Value, rewardValue);
        //                });

        //                Config.TimeResult = 300000 + timeserver;
        //                Config.ConSoMayManStatus = ConSoMayManStatus.WAIT;
        //            }
        //            Server.Gi().Logger.Print("SETUP CONSO | STATUS: WAIT", "red");
        //            break;
        //    }
        //}
    }
}
