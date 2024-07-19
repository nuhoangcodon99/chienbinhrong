using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Handlers.Character;
using NgocRongGold.Application.Handlers.Client;
using NgocRongGold.Application.Handlers.Item;
using NgocRongGold.Application.Handlers.Skill;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.Interfaces.Client;
using NgocRongGold.Application.Interfaces.Map;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.DatabaseManager;
using NgocRongGold.DatabaseManager.Player;
using NgocRongGold.Application.Menu;
using NgocRongGold.Model;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Clan;
using NgocRongGold.Model.Info;
using NgocRongGold.Model.Map;
using NgocRongGold.Model.SkillCharacter;
using NgocRongGold.Model.Monster;
using NgocRongGold.Model.Item;
using NgocRongGold.Model.Template;
using NgocRongGold.Model.Option;
using Org.BouncyCastle.Math.Field;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using NgocRongGold.Application.MainTasks;
using NgocRongGold.Application.Extension;
using NgocRongGold.Application.Extension.Chẵn_Lẻ_Momo;
using NgocRongGold.Application.Extension.ChampionShip;
using NgocRongGold.Application.Extension.Bosses;
using NgocRongGold.Application.Extension.Dragon;
using NgocRongGold.Application.Extension.BlackballWar;
using System.Threading;
using NgocRongGold.Application.Extension.Bosses.Mabu2Gio;
using NgocRongGold.Application.Extension.Bosses.Mabu12Gio;
using NgocRongGold.Application.Extension.Ký_gửi;
using NgocRongGold.Model.Data;
using NgocRongGold.Application.Extension.Super_Champion;
using NgocRongGold.Application.Extension.Bo_Mong;
using System.Diagnostics;
using NgocRongGold.Application.Extension.NamecballWar;
using Sources.Database;
using NgocRongGold.Application.Extension.DaiHoiVoThuat;
using NgocRongGold.Application.Extension.DaiHoiVoThuat.SuperChampion;
using NgocRongGold.Application.Extension.Practice;
using Serilog.Settings.Configuration;
using NgocRongGold.Application.Extension.SideQuest.BoMong;
using Application.Interfaces.Zone;
using System.Data.Common;
using Sources;
using Application.Interfaces.Map;
using Application.Map;
using static NgocRongGold.Application.Threading.APet2;
using System.Globalization;

namespace NgocRongGold.Application.Main   
{
    public class Controller : IMessageHandler
    {
        private readonly ISession_ME _session;

        public Controller(ISession_ME client)
        {
            _session = client;
        }

        public void OnConnectionFail(ISession_ME client, bool isMain)
        {
            //throw new System.NotImplementedException();
        }

        public void OnConnectOK(ISession_ME client, bool isMain)
        {
            //throw new System.NotImplementedException();
        }

        public void OnDisconnected(ISession_ME client, bool isMain)
        {
            //throw new System.NotImplementedException();
        }
        
        public async Task OnMessage(Message message)
        {
            try
            {
                if (message == null) return;
                var command = message.Command;
              //  Server.Gi().Logger.Debug($"Client: {_session.Id} - Command >>>>> {command}");
              //  Server.Gi().Logger.Debug($"Zom Level: {_session.ZoomLevel}");

                //var characterLog = _session?.Player?.Character;
                //if (characterLog != null)
                //{
                    
                //    if (DataCache.LogTheoDoi.Contains(characterLog.Id))
                //    {
                //        ServerUtils.WriteTraceLog( characterLog.Id + "_" + characterLog.Name, "Command: " + command);
                //    }
                //}
                switch (command)
                {

                    //TopInfo ActionS
                    case -118:
                        {
                            var plId = message.Reader.ReadInt();
                            if (plId == -1) return;
                            var character = (Character)_session.Player.Character;
                            switch (character.InfoChar.MapId)
                            {
                                case 130:
                                    character.DataSieuHang.Handler.SendChallenge(character, plId);
                                    break;
                            }
                            break;
                        }

                    //Ki gui
                    case -100:
                        {
                            var action = message.Reader.ReadByte();
                            var character = (Character)_session.Player.Character;
                            if (character == null) return;
                            switch (action)
                            {
                                case 0:
                                    {
                                        var itemId = message.Reader.ReadShort();
                                        var moneyType = message.Reader.ReadByte();
                                        var money = message.Reader.ReadInt();
                                        var quantity = message.Reader.ReadInt();
                                        KyGUIHandler.KyGui(character, itemId, money, moneyType, quantity);
                                        break;
                                    }
                                case 1:
                                case 2:
                                    {
                                        var itemId = message.Reader.ReadShort();
                                        KyGUIHandler.ClaimMoneyOrDeleteItem(character, action, itemId);
                                        break;
                                    }
                                case 3:
                                    {
                                        var itemId = message.Reader.ReadShort();
                                        KyGUIHandler.BuyItem(character, itemId);
                                        break;
                                    }
                                case 4:
                                    {
                                        var tab = message.Reader.ReadByte();
                                        var page = message.Reader.ReadByte();
                                        character.CharacterHandler.SendMessage(KyGUIService.LoadPage(character, (byte)tab, (byte)page));

                                        break;
                                    }
                                case 5:
                                    {
                                        var itemId = message.Reader.ReadShort();
                                        KyGUIHandler.UpTop(character, itemId);
                                        break;
                                    }
                            }
                           // _session.SendMessage(Service.BuyItem(_session.Player.Character));
                            break;


                            //var action = message.Reader.ReadByte();
                            //var character = (Character)_session.Player.Character;

                            //    var itemId = message.Reader.ReadShort();
                            //    var moneyType = message.Reader.ReadByte();
                            //    var money = message.Reader.ReadInt();
                            //    var quantity = message.Reader.ReadInt();
                            //    // kí gửi
                            //        break;
                            //    case 1:
                            //    var itemId1 = message.Reader.ReadShort();
                            //    // xóa item
                            //        Server.Gi().Logger.Print("ItemId: " + itemId1, "cyan");
                            //    break;
                            //    case 2:
                            //    var itemId2 = message.Reader.ReadShort();
                            //   // nhận tiền
                            //        Server.Gi().Logger.Print("ItemId: " + itemId2, "cyan");
                            //    break;
                            //    case 3:
                            //    var itemId3 = message.Reader.ReadShort();
                            //    var moneyType3 = message.Reader.ReadByte();
                            //    var money3 = message.Reader.ReadInt();
                            //    // mua
                            //        Server.Gi().Logger.Print("ItemId: " + itemId3 + " moneyType: " + moneyType3 + " money: " + money3,"cyan");
                            //        break;
                            //    case 4:
                            //    var moneyType4 = message.Reader.ReadByte();
                            //    var money4 = message.Reader.ReadInt();
                            //        Server.Gi().Logger.Print("moneyType: " + moneyType4 + " money: " + money4, "cyan");
                            //        break;
                            //    case 5:
                            //        var itemId5 = message.Reader.ReadShort();
                            //        Server.Gi().Logger.Print("ItemId: " + itemId5, "cyan");
                            //        break;
                            //}
                            //break;
                        }
                    // Transport
                    case -105:
                    {
                        var character = (Character) _session?.Player?.Character;
                        if (character == null) break;  
                        try
                        {
                                
                            var mapId = character.InfoMore.TransportMapId;
                            if (mapId == -1)
                            {
                                JoinMapKarin(21 + character.InfoChar.Gender, true, true, character.InfoChar.Teleport);
                                break;
                            }

                            var @char = character;
                            var mapOld = MapManager.Get(@char.InfoChar.MapId);
                            //if (DataCache.IdMapCustom.Contains(@char.InfoChar.MapId))
                            //{
                            //    mapOld = MapManager.GetMapCustom(@char.InfoChar.MapCustomId)
                            //        .GetMapById(@char.InfoChar.MapId);
                            //}

                            IMap mapNext;
                            if (DataCache.IdMapCustom.Contains(mapId))
                            {
                                _session.SendMessage(
                                    Service.SendTeleport(character.Id, character.InfoChar.Teleport));
                                mapOld.OutZone(character);
                                @char.MapIdOld = mapOld.Id;
                                @char.SetOldMap();
                                switch (mapId)
                                {
                                    case 21:
                                    case 22:
                                    case 23:
                                    {
                                        JoinMapKarin(mapId,true, true, character.InfoChar.Teleport);
                                        return;
                                    }
                                    case 47:
                                    {
                                                JoinMapKarin(47, true, true, character.InfoChar.Teleport);
                                        return;
                                    }
                                    case 45:
                                    {
                                                JoinMapKarin(45, true, true, character.InfoChar.Teleport);
                                        return;
                                    }
                                    case 48:
                                    {
                                                JoinMapKarin(48, true, true, character.InfoChar.Teleport);
                                        return;
                                    }
                                    case 111:
                                    {
                                                JoinMapKarin(111, true, true, character.InfoChar.Teleport);
                                        return;
                                    }
                                        case 154:
                                            JoinMapKarin(154, true, true, character.InfoChar.Teleport);
                                            return;
                                    }
                                  
                            }
                            else
                            {
                                mapNext = MapManager.Get(mapId);
                                var zoneNext = mapNext.GetZoneNotMaxPlayer();
                                if (zoneNext == null)
                                {
                                    _session.SendMessage(Service.OpenUiSay(5, TextServer.gI().MAX_NUMCHARS,
                                        false,
                                        character.InfoChar.Gender));
                                    //JoinHome(true, true, character.InfoChar.Teleport);
                                }
                                else
                                {
                                    _session.SendMessage(Service.SendTeleport(character.Id,
                                        character.InfoChar.Teleport));
mapOld.OutZone(character);
                                    @char.MapIdOld = mapOld.Id;
                                    @char.SetOldMap();
                                    zoneNext.ZoneHandler.JoinZone((Character) character, true, true,
                                        character.InfoChar.Teleport);
                                }
                            }
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                        break;
                    }
                    // Special Skill
                    case 112:
                    {
                        var character = (Character) _session?.Player?.Character;
                        if (character == null) return;
                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, MenuNpc.Gi().TextNoiTai[0],
                                    MenuNpc.Gi().MenuNoiTai[0], character.InfoChar.Gender));
                        character.TypeMenu = 10;
                        break;
                    }
                    //Luck roll
                    case -127:
                    {
                        var character = (Character) _session?.Player?.Character;
                        if (character == null) return;
                        var type = message.Reader.ReadByte();
                        var soluong = 0;
                        if (message.Reader.Available() > 0)
                        {
                            soluong = message.Reader.ReadByte();
                        }

                           // Server.Gi().Logger
                           // .Debug(
                           //     $"Client: {_session.Id} - Luck roll -------------------- type: {type} - soluong: {soluong}");
                            switch (type)
                            {
                                case 0:
                                    {
                                        if (soluong == 0)
                                        {
                                            if (character.LuckyBox.Count >= DataCache.LIMIT_SLOT_RUONG_PHU_THUONG_DE)
                                            {
                                                character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().FULL_LUCKY_BOX));
                                                break;
                                            }
                                            character.CharacterHandler.SendMessage(Service.LuckRoll0());
                                            character.ShopId = 0;
                                        }
                                        break;
                                    }
                                case 1:
                                    {
                                        if (soluong == 0)
                                        {
                                            character.CharacterHandler.SendMessage(Service.LuckRoll0());
                                            break;
                                        }
                                        break;
                                    }
                                case 2:
                                    {
                                        var charReal = (Character)character;

                                        // Kiểm tra số lượng vé quay trước
                                        var itemVeQuay = character.ItemBag.FirstOrDefault(i => i.Id is 821);
                                        var soLuongVeQuay = itemVeQuay != null ? itemVeQuay.Quantity : 0;
                                        var soLuongNgocRong = soluong;
                                        // Nếu số lượng vé dùng cao hơn số lượng vé trong túi 
                                        if (soLuongNgocRong > soLuongVeQuay)
                                        {
                                            // Thì số lượng vé sử dụng bằng tổng số lượng vé quay
                                            // soLuongNgocRong ngọc quay trừ đi số lượng vé quay    
                                            soLuongNgocRong -= soLuongVeQuay;
                                        }
                                        // nếu số lượng vé cần dùng ít hơn hoặc bằng số lượng vé trong túi
                                        else
                                        {
                                            soLuongVeQuay = soLuongNgocRong;
                                            soLuongNgocRong = 0;
                                        }

                                        if (soLuongNgocRong * DataCache.CRACK_BALL_PRICE_DIAMOND_LOCK > charReal.AllDiamondLock())
                                        {
                                            //Ingored
                                            character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn còn thiếu " + (soLuongNgocRong * DataCache.CRACK_BALL_PRICE_DIAMOND_LOCK - charReal.AllDiamondLock()) + " hồng ngọc nữa"));
                                            break;
                                        }

                                        if (itemVeQuay != null && soLuongVeQuay > 0)
                                        {
                                            charReal.CharacterHandler.RemoveItemBagByIndex(itemVeQuay.IndexUI, soLuongVeQuay, reason: "qvmm");
                                            charReal.CharacterHandler.SendMessage(Service.SendBag(character));
                                        }

                                        //charReal.MineGold(soLuongNgocRong * DataCache.CRACK_BALL_PRICE);
                                        charReal.MineDiamond(soLuongNgocRong * DataCache.CRACK_BALL_PRICE_DIAMOND_LOCK, 2);
                                        charReal.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                                        var list = new List<short>();
                                        var timeServer = ServerUtils.CurrentTimeSecond();
                                        for (var i = 0; i < soluong; i++)
                                        {
                                            var randomRate = ServerUtils.RandomNumber(0.0, 100.0);
                                            var randomTiLe = ServerUtils.RandomNumber(0, 100);
                                            var listRandomItem = DataCache.LuckyBoxRare;
                                            if (ServerUtils.RandomNumber(100) < 40)
                                            {
                                                listRandomItem = DataCache.LuckyBoxItems;
                                            }
                                            int expireDay = 0;
                                            int expireTime = 0;
                                            
                                            switch(randomRate){
                                                case < 20:
                                                    break;
                                                case < 50:
                                                    expireDay = DataCache.LuckyBoxItemExpire[ServerUtils.RandomNumber(DataCache.LuckyBoxItemExpire.Count - 2, DataCache.LuckyBoxItemExpire.Count)];
                                                    break;
                                                case < 70:
                                                    expireDay = DataCache.LuckyBoxItemExpire[ServerUtils.RandomNumber(3, 5)];
                                                    break;
                                                default:
                                                expireDay = DataCache.LuckyBoxItemExpire[ServerUtils.RandomNumber(0, 3)];
                                                break;
                                            }

                                            if (expireDay > 0)
                                            {
                                                expireTime = (int)(timeServer + (expireDay * DataCache._1DAYBYSECOND));
                                            }
                                            Server.Gi().Logger.Debug(expireTime + "s | " + expireDay + "d | " + " | total: " + (expireTime - timeServer));
                                            int itemRandomIndex = ServerUtils.RandomNumber(listRandomItem.Count);
                                            var itemNew = ItemCache.GetItemDefault(listRandomItem[itemRandomIndex]);
                                            if (randomTiLe > 30)
                                            {
                                                itemNew = ItemCache.GetItemDefault(190);
                                            }
                                            var template = ItemCache.ItemTemplate(itemNew.Id);
                                                var indexList = charReal.LuckyBox.Count;
                                                itemNew.Reason = "Ngọc Rồng Online";
                                                itemNew.IndexUI = indexList;

                                                if (expireDay > 0 && template.Type != 29 && !DataCache.ListSaoPhaLe.Contains(itemNew.Id) && !DataCache.ListDragonball.Contains(itemNew.Id))
                                                {
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
                                                }

                                                if (template.Type == 9) // vàng
                                                {
                                                    itemNew.Options.Add(new OptionItem()
                                                    {
                                                        Id = 171,
                                                        Param = ServerUtils.RandomNumber(10, 20)
                                                    });
                                                }

                                                list.Add(template.IconId);
                                                charReal.LuckyBox.Add(itemNew);
                                            
                                           
                                        }

                                        character.CharacterHandler.SendMessage(Service.LuckRoll1(list));
                                        break;
                                    }
                            }

                        break;
                    }
                    //Input Client
                    case -125:
                    {
                        var character = _session?.Player?.Character;
                        if (character == null) return;
                        InputClient.HanleInputClient((Character) character, message);
                        break;
                    }
                    case -111: 
                    {
                        break;
                    }
                    //Change on skill
                    case -113:
                    {
                        var character = _session?.Player?.Character;
                        if (character == null) return;
                         
                        for (int i = 0; i < 10; i++)
                            {
                                character.InfoChar.OSkill[i] = message.Reader.ReadByte();
                            }
                        //var count = 0;
                        //while (message.Reader.Available() > 0)
                        //{
                        //    try
                        //    {
                        //        character.InfoChar.OSkill[count++] = message.Reader.ReadByte();
                        //    }
                        //    catch (Exception)
                        //    {
                        //        // ignored
                        //    }
                        //}

                        break;
                    }
                    //Status Đệ Tử
                    case -108:
                    {
                        var character = (Character) _session?.Player?.Character;
                        if (character?.Disciple == null || !character.InfoChar.IsHavePet) return;

                        if (character.IsDontMove())
                        {
                            character.CharacterHandler.SendMessage(
                                Service.ServerMessage(TextServer.gI().DO_NOT_ACTION_DISCIPLE));
                            return;
                        }

                        var status = message.Reader.ReadByte();
                        var disciple = character.Disciple;
                        lock (disciple)
                        {
                            //Server.Gi().Logger
                            //    .Debug(
                            //        $"Client: {_session.Id} ----------------------- status pet: {character.Disciple.Status}");
                            switch (status)
                            {
                                case 0:
                                {
                                    if (character.InfoChar.Fusion.IsFusion || disciple.Status >= 4)
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service.ServerMessage(TextServer.gI().DO_NOT_ACTION_DISCIPLE));
                                                break;
                                    }

                                    if (disciple.Status == 3)
                                    {
                                        async void Action()
                                        {
                                            await Task.Delay(2000);
                                            character.Zone.ZoneHandler.AddDisciple(disciple);
                                            character.CharacterHandler.SendMessage(Service.PublicChat(disciple.Id,
                                            "Bái kiến sư phụ"));
                                        }

                                        var task = new Task(Action);
                                        task.Start();
                                    } 
                                    else 
                                    {
                                        character.CharacterHandler.SendMessage(Service.PublicChat(disciple.Id,
                                            "Ok, con đi theo sư phụ"));
                                        
                                    }
                                    disciple.Status = 0;
                                    break;
                                }
                                case 1:
                                {
                                    if (character.InfoChar.Fusion.IsFusion || disciple.Status >= 3)
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service.ServerMessage(TextServer.gI().DO_NOT_ACTION_DISCIPLE));
                                        break;
                                    }

                                    character.CharacterHandler.SendMessage(Service.PublicChat(disciple.Id,
                                        "Ok, con sẽ bảo vệ sư phụ"));
                                    disciple.Status = 1;
                                    break;
                                }
                                case 2:
                                {
                                    if (character.InfoChar.Fusion.IsFusion || disciple.Status > 3)
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service.ServerMessage(TextServer.gI().DO_NOT_ACTION_DISCIPLE));
                                                break;
                                    }

                                    character.CharacterHandler.SendMessage(Service.PublicChat(disciple.Id,
                                        "Ok, sư phụ cứ để con lo cho"));
                                    disciple.Status = 2;
                                    break;
                                }
                                case 3:
                                {
                                    if (character.InfoChar.Fusion.IsFusion || disciple.Status == 4)
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service.ServerMessage(TextServer.gI().DO_NOT_ACTION_DISCIPLE));
                                                break;
                                    }

                                    character.CharacterHandler.SendMessage(Service.PublicChat(disciple.Id,
                                        "Bibi sư phụ..."));

                                    async void Action()
                                    {
                                        await Task.Delay(2000);
                                        try
                                        {
                                            if (disciple.Zone != null && disciple.Zone.ZoneHandler != null)
                                            {
                                                disciple.Zone.ZoneHandler.RemoveDisciple(disciple);
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                                    Server.Gi().Logger.Error($"Error disciple.Zone.ZoneHandler.RemoveDisciple in Controller.cs: {e.Message} \n {e.StackTrace}", e);
                                        }
                                    }

                                    disciple.Status = 3;
                                    var task = new Task(Action);
                                    task.Start();
                                    break;
                                }
                                //Hợp thể 10 phút
                                case 4:
                                {
                                    

                                    var timeServer = ServerUtils.CurrentTimeMillis();
                                    if (character.InfoChar.Fusion.IsFusion)
                                    {
                                        disciple.CharacterHandler.SetUpPosition(isRandom: true);
                                        character.Zone.ZoneHandler.AddDisciple(disciple);
                                        character.CharacterHandler.SendZoneMessage(Service.Fusion(character.Id, 1));
                                        lock (character.InfoChar.Fusion)
                                        {
                                            character.InfoChar.Fusion.IsFusion = false;
                                            character.InfoChar.Fusion.IsPorata = false;
                                            character.InfoChar.Fusion.IsPorata2 = false;
                                            character.InfoChar.Fusion.TimeStart = timeServer;
                                            character.InfoChar.Fusion.DelayFusion = timeServer + 600000;
                                            character.InfoChar.Fusion.TimeUse = 0;
                                        }

                                        disciple.Status = 0;
                                    }
                                    else
                                    {
                                        if (disciple.InfoChar.IsDie || disciple.Status >= 3)
                                        {
                                            character.CharacterHandler.SendMessage(
                                                Service.ServerMessage(TextServer.gI().CAN_NOT_USE_FUSION));
                                            return;
                                        }

                                        if (character.InfoChar.Fusion.DelayFusion > timeServer)
                                        {
                                            var delay = (character.InfoChar.Fusion.DelayFusion - timeServer) / 60000;
                                            if (delay < 1)
                                            {
                                                delay = 1;
                                            }

                                            character.CharacterHandler.SendMessage(
                                                Service.ServerMessage(string.Format(TextServer.gI().DELAY_FUSION_10M,
                                                    delay)));
                                            return;
                                        }

                                        if (disciple.InfoSkill.HuytSao.IsHuytSao)
                                        {
                                            SkillHandler.RemoveHuytSao(disciple);
                                        }

                                        if (disciple.InfoSkill.Monkey.MonkeyId == 1)
                                        {
                                            SkillHandler.HandleMonkey(disciple,false);
                                        }

                                        disciple.Zone.ZoneHandler.RemoveDisciple(disciple);
                                        character.CharacterHandler.SendZoneMessage(Service.Fusion(character.Id, 4));
                                        lock (character.InfoChar.Fusion)
                                        {
                                            character.InfoChar.Fusion.IsFusion = true;
                                            character.InfoChar.Fusion.IsPorata = false;
                                            character.InfoChar.Fusion.IsPorata2 = false;
                                            character.InfoChar.Fusion.TimeStart = timeServer;
                                            character.InfoChar.Fusion.TimeUse = 600000;
                                        }

                                        disciple.Status = 4;
                                    }

                                    character.CharacterHandler.SetUpInfo();
                                    character.CharacterHandler.PlusHp((int) character.HpFull);
                                    character.CharacterHandler.PlusMp((int) character.MpFull);
                                    character.CharacterHandler.SendZoneMessage(Service.UpdateBody(character));
                                    character.CharacterHandler.SendMessage(Service.PlayerLoadSpeed(character));
                                    character.CharacterHandler.SendMessage(Service.MeLoadPoint(character));
                                    character.CharacterHandler.SendMessage(Service.SendHp((int) character.HpFull));
                                    character.CharacterHandler.SendMessage(Service.SendMp((int) character.MpFull));
                                    character.CharacterHandler.SendZoneMessage(Service.PlayerLevel(character));
                                    break;
                                }
                                //Hợp thể vĩnh viễn
                                case 5:
                                {
                                    // if (character.InfoChar.Gender != 1) return;
                                    // if (character.Zone.Map.IsMapCustom())
                                    // {
                                    //     character.CharacterHandler.SendMessage(
                                    //         Service.ServerMessage(TextServer.gI().DONT_NOT_ACTION_DISCIPLE_HERE));
                                    //     return;
                                    // }

                                    // if (disciple.InfoChar.IsDie || disciple.Status >= 3)
                                    // {
                                    //     character.CharacterHandler.SendMessage(
                                    //         Service.ServerMessage(TextServer.gI().CAN_NOT_USE_FUSION));
                                    //     return;
                                    // }

                                    // if (DiscipleDB.Delete(disciple.Id))
                                    // {
                                    //     disciple.Status = 0;
                                    //     //Cleare disciple
                                    //     disciple.Zone.ZoneHandler.RemoveDisciple(disciple);
                                    //     disciple.CharacterHandler.Clear();

                                    //     character.CharacterHandler.SendZoneMessage(Service.Fusion(character.Id, 6));
                                    //     character.CharacterHandler.SendMessage(
                                    //         Service.ServerMessage(TextServer.gI().FUSION_88));

                                    //     character.InfoChar.Potential += disciple.InfoChar.Điểm_thành_tích;
                                    //     character.CharacterHandler.SendMessage(Service.MeLoadPoint(character));

                                    //     character.InfoChar.IsHavePet = false;
                                    //     character.Disciple = null;
                                    //     character.CharacterHandler.SendMessage(Service.Disciple(0, null));
                                    // }
                                    // else
                                    // {
                                    //     character.CharacterHandler.SendMessage(
                                    //         Service.ServerMessage(TextServer.gI().ERROR_SERVER));
                                    // }
                                    character.CharacterHandler.SendMessage(
                                            Service.ServerMessage(TextServer.gI().ERROR_SERVER));

                                    break;
                                }
                            }
                        }

                        break;
                    }
                    //Đệ tử
                    case -107:
                    {
                        var character = (Character) _session?.Player?.Character;
                        if (character == null) return;
                        if (character.Disciple != null)
                        {
                            character.CharacterHandler.SendMessage(Service.Disciple(2, character.Disciple));
                        }
                        else 
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiSay(5, TextServer.gI().NOT_FOUND_DISCIPLE, false,character.InfoChar.Gender));
                        }
                        break;
                    }
                    //Set LOCK
                    case -104:
                    {
                        var character = (Character) _session?.Player?.Character;
                        if (character == null) return;
                        try
                        {
                            var pass = message.Reader.ReadInt();
                            if (pass.ToString().Length != 6) return;
                            if (character.InfoChar.LockInventory.Pass == -1)
                            {
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5,
                                    string.Format(MenuNpc.Gi().TextMeo[8], pass), MenuNpc.Gi().MenuMeo[1],
                                    character.InfoChar.Gender));
                                character.TypeMenu = 8;
                                character.InfoChar.LockInventory.PassTemp = pass;
                            }
                            else
                            {
                                if (pass != character.InfoChar.LockInventory.Pass)
                                {
                                    character.CharacterHandler.SendMessage(
                                        Service.ServerMessage(TextServer.gI().INVALID_LOCK_INVENTORY));
                                    return;
                                }

                                var text = MenuNpc.Gi().TextMeo[9];
                                if (!character.InfoChar.LockInventory.IsLock)
                                {
                                    text = MenuNpc.Gi().TextMeo[10];
                                }

                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, text,
                                    MenuNpc.Gi().MenuMeo[1], character.InfoChar.Gender));
                                character.TypeMenu = 9;
                            }
                        }
                        catch (Exception)
                        {
                            return;
                        }

                        break;
                    }
                    //CHANGE_FLAG
                    case -103:
                    {
                        var character = (Character) _session?.Player?.Character;
                        if (character == null) return;
                        {
                            if (DataCache.IdMapCustom.Contains(character.InfoChar.MapId))
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.ServerMessage(TextServer.gI().NOT_CHANGE_FLAG));
                                return;
                            }

                            var action = message.Reader.ReadByte();
                            switch (action)
                            {
                                case 0:
                                {
                                    character.CharacterHandler.SendMessage(Service.ChangeFlag0());
                                    break;
                                }
                                case 1:
                                {
                                    var type = message.Reader.ReadByte();
                                    var @char = (Character) character;
                                    var delayChange = @char.Delay.ChangeFlag;
                                    var timeServe = ServerUtils.CurrentTimeMillis();
                                    if (type != 0 && delayChange > timeServe)
                                    {
                                        var timeDelay = (delayChange - timeServe) / 1000;
                                        character.CharacterHandler.SendMessage(
                                            Service.ServerMessage(string.Format(TextServer.gI().DELAY_CHANGEFLAG,
                                                timeDelay)));
                                        return;
                                    }
                                            if (BlackballCache.ListMapNRSD.Contains(@char.InfoChar.MapId))
                                            {
                                                character.CharacterHandler.SendMessage(Service.ServerMessage("Không thể đổi cờ ở khu vực này !"));
                                                return;
                                            }

                                    if (type is 9 or 10 && !DataCache.IdMapMabu.Contains(@char.InfoChar.MapId))
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service.ServerMessage(TextServer.gI().NOT_FLAG));
                                        return;
                                    }

                                    character.Flag = type;
                                    character.CharacterHandler.SendZoneMessage(Service.ChangeFlag1(character.Id, type));

                                    if (character.Disciple != null && character.InfoChar.IsHavePet)
                                    {
                                        character.Disciple.Flag = type;
                                        if (!character.InfoChar.Fusion.IsFusion)
                                        {
                                            character.CharacterHandler.SendZoneMessage(
                                                Service.ChangeFlag1(character.Disciple.Id, type));
                                        }
                                    }

                                    if (type != 0)
                                    {
                                        @char.Delay.ChangeFlag = 60000 + timeServe;
                                    }

                                    break;
                                }
                                case 2:
                                {
                                    var type = message.Reader.ReadByte();
                                            if (type < 0 || type > 12) return;
                                    character.CharacterHandler.SendMessage(Service.ChangeFlag2(type));
                                    break;
                                }
                            }
                        }
                        break;
                    }
                    //Guest Player
                    case -101:
                        {
                            var usernameSet = "@copyright_KhanhDTK_" + ServerUtils.CurrentTimeMillis();
                            _session.SendMessage(Service.PlayGuest(usernameSet));
                            UserDB.CreateUser(usernameSet, "a", 0, 0);
                            break;
                        }
                    //List enemy 
                    case -99:
                    {
                        var character = _session?.Player?.Character;
                        if (character == null) return;
                        var @charReal = (Character) character;
                        if (@charReal.Enemies == null)
                        {
                            @charReal.Enemies = new List<InfoFriend>();
                        }

                        var action = message.Reader.ReadByte();
                        switch (action)
                        {
                            //Danh sách
                            case 0:
                            {
                                character.CharacterHandler.SendMessage(Service.ListEmeny((Character)character,@charReal.Enemies));
                                break;
                            }
                            //Trả thù
                            case 1:
                            {
                                var charId = message.Reader.ReadInt();
                                if (charId == character.Id) return;
                                var @char = (Character) ClientManager.Gi().GetCharacter(charId);
                                if (@char != null)
                                {
                                    var enemy = @charReal.Enemies.FirstOrDefault(enemy => enemy.Id == charId);
                                    if (enemy == null)
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service.ServerMessage(TextServer.gI().NOT_FOUND_CHAR_IN_MAP));
                                        return;
                                    }

                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5,
                                        string.Format(MenuNpc.Gi().TextMeo[4], @char.Name), MenuNpc.Gi().MenuMeo[1],
                                        character.InfoChar.Gender));
                                    @charReal.TypeMenu = 5;
                                    @charReal.EnemyTemp = enemy;
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(
                                        Service.ServerMessage(TextServer.gI().USER_OFFLINE));
                                }

                                break;
                            }
                            //Xoá thù địch
                            case 2:
                            {
                                var charId = message.Reader.ReadInt();
                                var info = @charReal.Enemies.FirstOrDefault(enemy => enemy.Id == charId);
                                if (info != null)
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5,
                                        string.Format(MenuNpc.Gi().TextMeo[7], info.Name), MenuNpc.Gi().MenuMeo[1],
                                        character.InfoChar.Gender));
                                    @charReal.TypeMenu = 7;
                                    @charReal.EnemyTemp = info;
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(
                                        Service.ServerMessage(TextServer.gI().FRIEND_NOT_FOUND));
                                }

                                break;
                            }
                        }

                        break;
                    }
                    //MAP_TRANSPOT
                    case -91:
                    {
                        var character = _session?.Player?.Character;
                        if (character == null) return;
                        {
                            

                                try
                                {
                                    var @char = (Character)character;
                                    
                                    var mapTranspot = @char.MapTranspots[message.Reader.ReadByte()];
                                    if (mapTranspot == null) return;
                                    var mapOld = MapManager.Get(@char.InfoChar.MapId);
                                    bool notSaveMapOld = false;
                                    if (@char.DataNgocRongNamek.AlreadyPick(@char))
                                    {
                                        var itm = new ItemMap(-1, ItemCache.GetItemDefault((short)(@char.DataNgocRongNamek.IdNamekBall)));
                                        itm.X = @char.InfoChar.X;
                                        itm.Y = @char.InfoChar.Y;
                                        @char.Zone.ZoneHandler.LeaveItemMap(itm);
                                        @char.InfoChar.TypePk = 0;
                                        @char.CharacterHandler.SendZoneMessage(Service.ChangeTypePk(@char.Id, 0));
                                        @char.DataNgocRongNamek.IdNamekBall = -1;
                                        @char.InfoChar.Bag = ClanManager.Get(@char.ClanId) != null ? (sbyte)ClanManager.Get(@char.ClanId).ImgId : (sbyte)-1;
                                        @char.CharacterHandler.UpdatePhukien();
                                    }
                                    switch (mapTranspot.Id)
                                    {

                                        case 165:
                                            {
                                                var clan = ClanManager.Get(character.ClanId);
                                                clan.ClanBoss.Join(@char, mapOld.Id, true);
                                            }
                                            break;
                                        case 153:
                                            {
                                                var clan = ClanManager.Get(character.ClanId);
                                                clan.ClanZone.Join(@char, mapOld.Id, true);
                                            }
                                            break;
                                        case 154:
                                            if (!character.InfoSet.IsFullSetHuyDiet)
                                            {
                                                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn phải có đủ Set Hủy Diệt"));
                                                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                                return;
                                            }
                                            else
                                            {
                                                character.CharacterHandler.SendZoneMessage(Service.SendTeleport(character.Id, character.TypeTeleport));
                                                MapManager.GetMapOffline(mapTranspot.Id).JoinZone((Character)character, character.Id, true, true, character.TypeTeleport);
                                                return;
                                            }
                                            break;
                                        case int i when DataCache.IdMapKarin.Contains(i):
                                            {

                                                character.CharacterHandler.SendZoneMessage(Service.SendTeleport(character.Id, character.TypeTeleport));
                                                MapManager.GetMapOffline(mapTranspot.Id).JoinZone((Character)character, character.Id, true, true, character.TypeTeleport);
                                                return;
                                            }
                                        case int i when DataCache.IdMapReddot.Contains(i):
                                            {
                                                character.CharacterHandler.SendZoneMessage(Service.SendTeleport(character.Id, character.TypeTeleport));
                                                MapManager.Get(mapTranspot.Id).JoinZone((Character)character, character.ClanId, true, true, character.TypeTeleport);
                                                return;
                                            }
                                        case int i when DataCache.IdMapBDKB.Contains(i):
                                            {
                                                character.CharacterHandler.SendZoneMessage(Service.SendTeleport(character.Id, character.TypeTeleport));
                                                MapManager.Get(mapTranspot.Id).JoinZone((Character)character, character.ClanId, true, true, character.TypeTeleport);
                                                return;
                                            }
                                        case int i when DataCache.IdMapCDRD.Contains(i):
                                            {
                                                character.CharacterHandler.SendZoneMessage(Service.SendTeleport(character.Id, character.TypeTeleport));
                                                MapManager.Get(mapTranspot.Id).JoinZone((Character)character, character.ClanId, true, true, character.TypeTeleport);
                                                return;
                                            }
                                        case int i when DataCache.IdMapGas.Contains(i):
                                            {
                                                var clan = ClanManager.Get(character.ClanId);
                                                clan.ClanDungeon.KhiGasHuyDiet.JoinMap(@char, mapTranspot.Id);
                                                return;
                                            }
                                    }
                                    switch (mapOld.Id)
                                    {

                                        case 165:
                                            {
                                                var clan = ClanManager.Get(character.ClanId);
                                                mapOld = clan.ClanBoss.Map;
                                                notSaveMapOld = true;
                                            }
                                            break;
                                        case 153:
                                            {
                                                var clan = ClanManager.Get(character.ClanId);
                                                mapOld = clan.ClanZone.Map;
                                                notSaveMapOld = true;
                                            }
                                            break;
                                        
                                        case int i when DataCache.IdMapGas.Contains(i):
                                            {
                                                var clan = ClanManager.Get(character.ClanId);
                                                mapOld = clan.ClanDungeon.KhiGasHuyDiet.GetMap(i);
                                                notSaveMapOld = true;
                                                break;
                                            }
                                    }
                                    IMap mapNext;
                                    mapNext = MapManager.Get(mapTranspot.Id);
                                    switch (mapNext.Id)
                                    {
                                        case int i when DataCache.IdMapCold.Contains(i):
                                            {
                                                if (character.InfoTask.Id < 24)
                                                {
                                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn phải làm nhiệm vụ [Chú bé đến từ tương lai] trước"));
                                                    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                                    return;
                                                }
                                                break;
                                            }
                                        case 79:
                                            {
                                                if (character.InfoTask.Id < 23)
                                                {
                                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn phải làm nhiệm vụ [Nhiệm vụ Tiểu Đội Sát Thủ] trước"));
                                                    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                                    return;
                                                }
                                                break;
                                            }
                                        case 5:
                                            if (character.InfoTask.Id < 12)
                                            {
                                                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn phải làm nhiệm vụ [Nhiệm vụ tiên học lễ] trước"));
                                                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                                return;
                                            }
                                            break;
                                        case 19:
                                            if (character.InfoTask.Id < 18)
                                            {
                                                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn phải làm nhiệm vụ [Tiêu diệt quái tinh nhuệ] trước"));
                                                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                                return;
                                            }
                                            break;
                                        case 20:
                                            if (character.InfoTask.Id < 14)
                                            {
                                                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn phải làm nhiệm vụ [Nhiệm vụ bang hội lần 1] trước"));
                                                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                                return;
                                            }
                                            break;
                                        
                                    }
                                    var zoneNext = mapNext.GetZoneNotMaxPlayer();
                                    if (zoneNext == null)
                                    {
                                        _session.SendMessage(Service.OpenUiSay(5, TextServer.gI().MAX_NUMCHARS, false,
                                            character.InfoChar.Gender));
                                    }
                                    else
                                    {
                                        character.CharacterHandler.SendZoneMessage(
                                            Service.SendTeleport(character.Id, character.InfoChar.Teleport));
    mapOld.OutZone(character);
                                        if (!notSaveMapOld)
                                        {
                                            @char.MapIdOld = mapOld.Id;
                                            @char.SetOldMap();
                                        }
                                        zoneNext.ZoneHandler.JoinZone((Character)character, true, true,
                                            character.InfoChar.Teleport);
                                        if (@char.ItemCapsuleId is 193)
                                        {
                                            character.CharacterHandler.RemoveItemBagById(193, 1, reason: "Dùng cs");
                                            _session.SendMessage(Service.SendBag(character));
                                        }
                                        if (DataCache.IdMapNRSD.Contains(zoneNext.Map.Id))
                                        {
                                            var flag = ClanManager.Get(character.ClanId).ClanFlag;
                                            character.Flag = (sbyte)flag;
                                            character.CharacterHandler.SendZoneMessage(Service.ChangeFlag1(character.Id, flag));
                                        }
                                    }
                                    
                                    
                                }
                                catch (Exception)
                                {
                                    break;
                                }
                        }
                        break;
                    }
                    //GIAO DỊCH
                    case -86:
                    {
                        var character = (Character) _session?.Player?.Character;
                        if (character == null || character.InfoChar.IsDie || character.InfoChar.TypePk != 0)
                            return;
                        if (!character.CheckLockInventory()) return;
                        
                        var timeServer = ServerUtils.CurrentTimeMillis();

                        if ((Server.Gi().StartServerTime+300000) > timeServer)
                        {
                            var delay = ((Server.Gi().StartServerTime+300000) - timeServer) / 1000;
                            if (delay < 1)
                            {
                                delay = 1;
                            }

                            character.CharacterHandler.SendMessage(Service.DialogMessage(string.Format(TextServer.gI().DELAY_RESTART_SEC,
                                    delay)));
                            return;
                        }

                        if (Maintenance.Gi().IsStart)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Máy chủ đang tiến hành bảo trì, không thể giao dịch ngay lúc này"));
                            return;
                        }

                            //if (character.Delay.InvAction > timeServer)
                            //{
                            //    character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn thao tác quá nhanh, chậm lại nhé"));
                            //    return;
                            //}

                            if (!character.Player.IsActive)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Vui lòng kích hoạt tài khoản để sử dụng chức năng này "));
                                return;
                            }

                            ItemHandler.TradeItem(message, (Character) character);
                        break;
                    }
                    //COMBINNE
                    case -81:
                    {
                        var character = _session?.Player?.Character;
                        if (character == null) return;
                        Combinne(message, (Character) character);
                        break;
                    }
                    //ADD FRIEND
                    case -80:
                    {
                        var character = _session?.Player?.Character;
                        if (character == null) return;
                        var action = message.Reader.ReadByte();
                        var @charReal = (Character) character;
                            var version = int.Parse(_session.Version.Replace(".", ""));
                        switch (action)
                        {
                            //Danh sách
                            case 0:
                            {
                                character.CharacterHandler.SendMessage(Service.ListFriend0(version,@charReal.Friends));
                                break;
                            }
                            //Chấp nhập Kết bạn YES/NO
                            case 1:
                            {
                                var charId = message.Reader.ReadInt();
                                if (charId == character.Id) return;
                                var @char = (Character) ClientManager.Gi().GetCharacter(charId);
                                if (@char != null)
                                {
                                    if (@charReal.Friends.FirstOrDefault(friend => friend.Id == charId) != null)
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service.ServerMessage(TextServer.gI().FRIEND_DUPLICATE));
                                        return;
                                    }

                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5,
                                        string.Format(MenuNpc.Gi().TextMeo[2], @char.Name), MenuNpc.Gi().MenuMeo[1],
                                        character.InfoChar.Gender));
                                    @charReal.TypeMenu = 3;
                                    @charReal.FriendTemp = @char.Me;
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(
                                        Service.ServerMessage(TextServer.gI().USER_OFFLINE));
                                }

                                break;
                            }
                            //Xoá bạn bè
                            case 2:
                            {
                                var charId = message.Reader.ReadInt();
                                var info = @charReal.Friends.FirstOrDefault(friend => friend.Id == charId);
                                if (info != null)
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5,
                                        string.Format(MenuNpc.Gi().TextMeo[3], info.Name), MenuNpc.Gi().MenuMeo[1],
                                        character.InfoChar.Gender));
                                    @charReal.TypeMenu = 4;
                                    @charReal.FriendTemp = info;
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(
                                        Service.ServerMessage(TextServer.gI().FRIEND_NOT_FOUND));
                                }

                                break;
                            }
                        }

                        break;
                    }

                    //Get PLayer menu
                    case -79:
                    {
                        var character = _session?.Player?.Character;
                        if (character == null) return;
                        var charId = message.Reader.ReadInt();
                        if (!DataCache.IdMapCustom.Contains(character.InfoChar.MapId))
                        {
                            var zone = character.Zone;
                            if (zone != null)
                            {   
                                var @charCheck = zone.ZoneHandler.GetCharacter(charId);
                                if (@charCheck != null)
                                {
                                        var levels = Cache.Gi().LEVELS.Where(x => x.Gender == @charCheck.InfoChar.Gender).Select(x => x.Name).ToList()[@charCheck.InfoChar.Level - 1];
                                        character.CharacterHandler.SendMessage(Service.MenuPlayer(charId, @charCheck.InfoChar.Power, levels));

                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(
                                        Service.ServerMessage(TextServer.gI().NOT_FOUND_CHAR_IN_MAP));
                                }
                            }
                            else
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.ServerMessage(TextServer.gI().NOT_FOUND_CHAR_IN_MAP));
                            }
                        }
                        else
                        {
                            character.CharacterHandler.SendMessage(
                                Service.ServerMessage(TextServer.gI().NOT_FOUND_CHAR_IN_MAP));
                        }

                        break;
                    }
                    //Get Image Resource
                    case -74:
                    {
                        if (!DatabaseManager.ConfigManager.gI().IsDownloadServer)
                        {
                            return;
                        }

                        ////Check DDOS
                        //IpTime check = null;
                        //var block = false;
                        //var ipv4 = _session.IpV4;
                        //if (FireWall.IpTimes.ContainsKey(ipv4))
                        //{
                        //    check = FireWall.IpTimes[ipv4];
                        //   //     Server.Gi().Logger
                        //    //    .Info($"Ip: {ipv4} ----------- Call -74. Checking..... Count: {check.Count}");
                        //}
                        //else
                        //{
                        //    check = new IpTime()
                        //    {
                        //        Ip = ipv4,
                        //        Time = ServerUtils.CurrentTimeMillis(),
                        //        Count = 1,
                        //    };
                        //    FireWall.IpTimes.TryAdd(ipv4, check);
                        //}
                        

                        var action = message.Reader.ReadByte();
                         //   Server.Gi().Logger
                         //   .Debug(
                         //       $"Client: {_session.Id} - Ip: {_session.IpV4} ----------- Call -74 with action: {action}");
                        switch (action)
                        {
                            case 1:
                            {
                                _session.SendMessage(Service.SendImageResource1(_session.ZoomLevel));
                                break;
                            }
                            case 2:
                            {
                                        //  block = true;
                                await Service.SendImageResource2Async(_session);
                                //Service.SendImageResource2(_session);
                                Service.SendImageResource3(_session);
                                break;
                            }
                        }

                        //if (block && check.Time - ServerUtils.CurrentTimeMillis() < 1000 && check.Count > 10)
                        //{
                        //    check.Count++;
                        //    _session.CloseMessage();
                        //        Server.Gi().Logger.Info($"Ip: {ipv4} ----------- Call -74 qua nhieu, block IP.....");
                        //    FireWall.BanIp(ipv4);
                        //}

                        break;
                    }
                        //CHAT PRIVATE
                    case -72:
                    {
                        var character = _session?.Player?.Character;
                            if (!character.Player.IsActive)
                            {
                                character.CharacterHandler.SendMeMessage(Service.ServerMessage("Vui lòng kích hoạt tài khoản để sử dụng chức năng này !"));
                                return;
                        }
                        if (character == null) return;
                        var charReal = (Character) character;
                        var charId = message.Reader.ReadInt();
                        if (charId == character.Id) return;
                        var text = ServerUtils.FilterWords(message.Reader.ReadUTF());
                        var charTemp = ClientManager.Gi().GetCharacter(charId);
                        var info = charReal.Friends.FirstOrDefault(friend => friend.Id == charId);
                        if (charTemp != null)
                        {
                            if (info != null)
                            {
                                character.CharacterHandler.SendMessage(Service.ListFriend3(info.Id, true));
                            }

                            charTemp.CharacterHandler.SendMessage(Service.WorldChat((Character) character, text, 1));
                        }
                        else
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().USER_OFFLINE));
                            if (info != null)
                            {
                                character.CharacterHandler.SendMessage(Service.ListFriend3(info.Id, false));
                            }
                        }

                        break;
                    }
                    //CHAT THẾ GIỚI
                    case -71:
                    {
                        var character = _session?.Player?.Character;
                        if (character == null) return;
                        {
                            var @char = (Character) character;
                                if (!character.Player.IsActive)
                                {
                                    //character.InfoChar.ThoiGianChatTheGioi = 300000 + timeServer;
                                    character.CharacterHandler.SendMeMessage(Service.ServerMessage("Vui lòng kích hoạt tài khoản để sử dụng chức năng này !"));
                                    return;
                                }
                                var delayChat = @char.InfoChar.ThoiGianChatTheGioi;
                            var timeServer = ServerUtils.CurrentTimeMillis();
                            if (delayChat > timeServer)
                            {
                                var time = (delayChat - timeServer) / 1000;
                                character.CharacterHandler.SendMessage(
                                    Service.ServerMessage(string.Format(TextServer.gI().DELAY_CHAT_TG, time)));
                                return;
                            }

                            if (@char.AllDiamond() < 5)
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.ServerMessage(TextServer.gI().NOT_ENOUGH_DIAMOND));
                                return;
                            }

                                

                            var noiDung = message.Reader.ReadUTF();
                            if (noiDung.Length > 128)
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.ServerMessage(TextServer.gI().MESSAGE_TO_LONG));
                                return;
                            }
                            character.InfoChar.ThoiGianChatTheGioi = 300000 + timeServer;
                            @char.MineDiamond(5);
                            character.CharacterHandler.SendMessage(Service.BuyItem(@char));
                            ClientManager.Gi().SendMessageCharacter(Service.WorldChat(@char,
                                ServerUtils.FilterWords(noiDung), 0));
                        }
                        break;
                    }
                    //Send Icon Resources
                    case -67:
                    {
                        _session.SendMessage(Service.SendIcon(_session.ZoomLevel, message.Reader.ReadInt()));
                        break;
                    }
                    //Send Effect Resources
                    case -66:
                        {
                            var character = _session.Player.Character;
                            short effId = message.Reader.ReadShort();
                            if (Cache.Gi().Role1Templates.FirstOrDefault(i => i.Temp == effId) != null)
                            {
                                if (character.InfoChar.Roles1.Roles.FirstOrDefault(i => i.Temp == effId) == null)
                                {
                                    ServerUtils.WriteLog("RacKeoTrom.txt", character.Id + " Get " + effId);
                                    break;
                                }
                            }
                            _session.SendMessage(Service.SendEffect(int.Parse(_session.Version.Replace(".", "")), _session.ZoomLevel, effId, effId, _session.Player.Character.TypeDragon));
                            break;
                        }
                    //GET CLAN IMAGE

                    case -63:
                    {
                        var character = _session?.Player?.Character;
                        if (character == null) return;
                        var imgId = message.Reader.ReadByte();
                        if (imgId == -1) return;
                        var clanImage = Cache.Gi().CLAN_IMAGES.FirstOrDefault(clan => clan.Id == imgId);
                        if (clanImage != null)
                        {
                            character.CharacterHandler.SendMessage(Service.GetImageBag(clanImage));
                        }

                        break;
                    }
                    //Send Clan Image
                    case -62:
                    {
                        var character = _session?.Player?.Character;
                        if (character == null) return;
                        var imgId = message.Reader.ReadByte();
                        var clanImage = Cache.Gi().CLAN_IMAGES.FirstOrDefault(clan => clan.Id == imgId);
                        if (clanImage != null)
                        {
                            character.CharacterHandler.SendMessage(Service.SendClanImage(clanImage));
                        }

                        break;
                    }
                    //PLAYER_ATTACK_PLAYER
                    case -60:
                    {
                        var character = _session?.Player?.Character;
                        if (character == null || character.IsDontMove()) return;
                        SkillHandler.AttackPlayer(character, message);
                        break;
                    }
                    //thách đấu
                    case -59:
                    {
                        var character = _session?.Player?.Character;
                        if (character == null || character.InfoChar.IsDie) return;
                       // character.CharacterHandler.SendMessage(Service.DialogMessage("Chức năng hiện tại đang bảo trì"));                        
                         var @char = (Character) character;

                        // if (!real.InfoChar.IsPremium)
                        // {
                        //     character.CharacterHandler.SendMessage(
                        //         Service.ServerMessage(TextServer.gI().NOT_PREMIUM));
                        //     return;
                        // }

                         var action = message.Reader.ReadByte();
                         var type = message.Reader.ReadByte();
                         var idChar = message.Reader.ReadInt();
                            Server.Gi().Logger.Debug($"Invite to TEST --------------- ------- action: {action} - type: {type} - idChar: {idChar}");
                         switch (action)
                         {
                             case 0:
                                    {
                                        switch (type)
                                        {
                                            //             //Select Menu
                                            case 3:
                                                {
                                                    if (@char.Challenge.isChallenge)
                                                    {
                                                        character.CharacterHandler.SendMessage(
                                                            Service.ServerMessage(TextServer.gI().NOT_TEST_ME));
                                                        return;
                                                    }

                                                    var player = (Character)character.Zone?.ZoneHandler?.GetCharacter(idChar);
                                                    if (player == null || player.Challenge == null)
                                                    {
                                                        character.CharacterHandler.SendMessage(
                                                            Service.ServerMessage(TextServer.gI().NOT_FOUND_CHAR_IN_MAP));
                                                        return;
                                                    }

                                                    if (player.Challenge.isChallenge)
                                                    {
                                                        character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_TEST));
                                                        return;
                                                    }
                                                    @char.Challenge.PlayerChallengeID = player.Id;
                                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, string.Format(MenuNpc.Gi().TextMeo[6], player.Name,ServerUtils.GetPower(player.InfoChar.Power)), MenuNpc.Gi().MenuMeo[2], character.InfoChar.Gender));
                                                    @char.TypeMenu = 0;
                                                    break;
                                                }
                                        }
                                        break;
                                    }
                             case 1:
                             {
                                 switch (type)
                                 {
                        //             //Accept
                                     case 3:
                                     {
                                                    if (@char.Challenge.isChallenge)
                                                    {
                                                        character.CharacterHandler.SendMessage(
                                                            Service.ServerMessage(TextServer.gI().NOT_TEST_ME));
                                                        return;
                                                    }

                                                    var player = (Character)@char.Zone.ZoneHandler.GetCharacter(idChar);
                                                    if (player == null)
                                                    {
                                                        character.CharacterHandler.SendMessage(
                                                            Service.ServerMessage(TextServer.gI().NOT_FOUND_CHAR_IN_MAP));
                                                        return;
                                                    }

                                                    if (player.Challenge.isChallenge)
                                                    {
                                                        character.CharacterHandler.SendMessage(
                                                            Service.ServerMessage(TextServer.gI().NOT_TEST));
                                                        return;
                                                    }

                                                    if (@char.InfoChar.Gold < @char.Challenge.Gold)
                                                    {
                                                        character.CharacterHandler.SendMessage(
                                                            Service.ServerMessage(TextServer.gI().NOT_ENOUGH_GOLD));
                                                        
                                                        return;
                                                    }

                                                    @char.MineGold(@char.Challenge.Gold);
                                                    @char.Challenge.isChallenge = true;
                                                    @char.Challenge.PlayerChallengeID = player.Id;
                                                    @char.InfoChar.TypePk = 3;

                                                    player.MineGold(player.Challenge.Gold);
                                                    player.Challenge.isChallenge = true;
                                                    player.Challenge.PlayerChallengeID = @char.Id;
                                                    player.InfoChar.TypePk = 3;

                                                    @char.CharacterHandler.SendMessage(Service.BuyItem(@char));
                                                    player.CharacterHandler.SendMessage(Service.BuyItem(player));

                                                    @char.CharacterHandler.SendZoneMessage(Service.ChangeTypePk(@char.Id, 3));
                                                    player.CharacterHandler.SendZoneMessage(Service.ChangeTypePk(player.Id, 3));
                                                    
                                                    break;
                                     }
                                 }

                                 break;
                             }
                         }

                        break;
                    }
                    //Invite to clan
                    case -57:
                    {
                        var character = _session?.Player?.Character;
                        if (character == null) return;

                        if (DatabaseManager.ConfigManager.gI().IsVIPServer)
                        {
                            character.CharacterHandler.SendMessage(Service.DialogMessage(TextServer.gI().DOT_IT_ON_NORMAL));
                            return;
                        }

                        var @charReal = (Character) character;
                        var action = message.Reader.ReadByte();
                            Server.Gi().Logger.Debug($"Invite to Clan --------------- ------- action: {action}");
                        var map = MapManager.Get(character.InfoChar.MapId);
                        var zone = map?.GetZoneById(character.InfoChar.ZoneId);
                        if (zone == null) return;
                        switch (action)
                        {
                            case 0:
                                    {

                                        if (@charReal.ClanId == -1) return;
                                        var clan = ClanManager.Get(@charReal.ClanId);
                                        if (clan == null) return;
                                        if (clan.Thành_viên_hiện_tại >= clan.Tối_đa_thành_viên)
                                        {
                                            character.CharacterHandler.SendMessage(
                                                Service.ServerMessage(TextServer.gI().MAX_MEMBER));
                                            break;
                                        }

                                        var playerId = message.Reader.ReadInt();
                                        var getCharZone = (Character)zone.ZoneHandler.GetCharacter(playerId);
                                        if (getCharZone == null)
                                        {
                                            character.CharacterHandler.SendMessage(
                                                Service.ServerMessage(TextServer.gI().NOT_FOUND_CHAR_IN_MAP));
                                            return;
                                        }

                                        if (clan.ClanHandler.GetMember(playerId) != null || getCharZone.ClanId != -1)
                                        {
                                            character.CharacterHandler.SendMessage(
                                                Service.ServerMessage(TextServer.gI().IN_CLAN_2));
                                            return;
                                        }

                                        var code = int.Parse($"{ServerUtils.RandomNumber(1000, 9999)}{character.Id}");
                                        charReal.CodeInviteClan = code;
                                        var invite = string.Format(TextServer.gI().INVITE_CLAN, character.Name, charReal.Name);
                                        getCharZone.CharacterHandler.SendMessage(Service.InviteClan(invite, clan.Id, code));
                                        break;
                                    }
                            case 1:
                                    {
                                        if (charReal.ClanId != -1)
                                        {
                                            character.CharacterHandler.SendMessage(
                                                Service.ServerMessage(TextServer.gI().IN_CLAN));
                                            return;
                                        }

                                        var clanId = message.Reader.ReadInt();
                                        var code = message.Reader.ReadInt();
                                        code = int.Parse(code.ToString().Substring(4));

                                        var clan = ClanManager.Get(clanId);
                                        if (clan == null)
                                        {
                                            return;
                                        }

                                        if (clan.ClanHandler.GetMember(character.Id) != null)
                                        {
                                            return;
                                        }

                                        if (clan.ClanHandler.AddMember(charReal, 2))
                                        {
                                            if (TaskHandler.CheckTask(charReal, 13, 0)) TaskHandler.gI().PlusSubTask(charReal, 1);

                                            charReal.ClanId = clanId;
                                            charReal.InfoChar.Bag = (sbyte)clan.ImgId;
                                            var img = Cache.Gi().CLAN_IMAGES.FirstOrDefault(i => i.Id == clan.ImgId);
                                            charReal.CharacterHandler.SendMessage(Service.GetImageBag(img));
                                            if (charReal.InfoChar.PhukienPart == -1) charReal.CharacterHandler.SendZoneMessage(
                                                Service.SendImageBag(charReal.Id, clan.ImgId));
                                            charReal.CharacterHandler.SendMessage(
                                                Service.ServerMessage(string.Format(TextServer.gI().ACCEPT_INVITE_CLAN,
                                                    clan.Name)));
                                            charReal.CharacterHandler.SendMessage(Service.MyClanInfo(charReal));
                                            charReal.CharacterHandler.SendZoneMessage(
                                                Service.UpdateClanId(charReal.Id, clan.Id));
                                            clan.ClanHandler.UpdateClanId();
                                            CharacterDB.Update(charReal);
                                            ClanDB.Update(clan);
                                        }

                                        break;
                                    }
                        }

                        break;
                    }
                    //Change Leader
                    case -56:
                        {
                            var character = _session?.Player?.Character;
                            if (character == null) return;
                            
                            var @charReal = (Character)character;
                            var clan = ClanManager.Get(charReal.ClanId);
                            if (clan == null) return;
                            var playerId = message.Reader.ReadInt();
                            var role = message.Reader.ReadByte();
                            Server.Gi().Logger
                                .Debug($"Clan Change Leader --------------- ------- playerId: {playerId} - role: {role}");
                            if (playerId == character.Id) return;
                            var me = clan.ClanHandler.GetMember(character.Id);
                            var member = clan.ClanHandler.GetMember(playerId);
                            if (me == null || member == null) return;
                            var lastMess = clan.Messages.LastOrDefault();
                            var id = lastMess != null ? lastMess.Id + 1 : 0;
                            switch (role)
                            {
                                //Loại
                                case -1:
                                    {
                                        if (me.Role == 2) return;
                                        if (clan.ClanHandler.RemoveMember(member.Id))
                                        {
                                            clan.ClanHandler.Chat(new ClanMessage()
                                            {
                                                Type = 0,
                                                Id = id,
                                                PlayerId = -1,
                                                PlayerName = "Thông báo",
                                                Role = me.Role,
                                                Time = (int)(ServerUtils.CurrentTimeMillis() / 1000),
                                                Text = string.Format(TextServer.gI().REMOVE_MEMBER, member.Name),
                                                Color = 1,
                                                NewMessage = true,
                                            });
                                            var charRemove = (Character)ClientManager.Gi().GetCharacter(member.Id);
                                            if (charRemove != null)
                                            {
                                                charRemove.ClanId = -1;
                                                charRemove.InfoChar.Bag = -1;
                                                if (character.InfoChar.PhukienPart == -1) character.CharacterHandler.SendZoneMessage(
                                                    Service.SendImageBag(character.Id, -1));
                                                character.CharacterHandler.SendMessage(Service.GetImageBag(null));
                                                charRemove.CharacterHandler.SendMessage(
                                                    Service.ServerMessage(TextServer.gI().KICKED_CLAN));
                                                charRemove.CharacterHandler.SendMessage(Service.MyClanInfo());
                                                charRemove.CharacterHandler.SendZoneMessage(
                                                    Service.UpdateClanId(charRemove.Id, -1));
                                                clan.ClanHandler.UpdateClanId();
                                                clan.ClanHandler.Flush();
                                                CharacterDB.Update(charRemove);
                                            }
                                            else
                                            {
                                                CharacterDB.Update(member.Id);
                                            }
                                        }

                                    ;
                                        break;
                                    }
                                case 0:
                                    {
                                        if (me.Role != 0) return;
                                        me.Role = 2;
                                        member.Role = 0;
                                        clan.ClanHandler.Chat(new ClanMessage()
                                        {
                                            Type = 0,
                                            Id = id,
                                            PlayerId = -1,
                                            PlayerName = "Thông báo",
                                            Role = 0,
                                            Time = (int)(ServerUtils.CurrentTimeMillis() / 1000),
                                            Text = string.Format(TextServer.gI().CHANGED_LEADER_CLAN, member.Name),
                                            Color = 1,
                                            NewMessage = true,
                                        });
                                        clan.ClanHandler.SendMessage(Service.ChangeMemberClan(me));
                                        clan.ClanHandler.SendMessage(Service.ChangeMemberClan(member));
                                        clan.ClanHandler.Flush();
                                        break;
                                    }
                                case 1:
                                    {
                                        if (me.Role == 2) return;
                                        member.Role = 1;
                                        clan.ClanHandler.Chat(new ClanMessage()
                                        {
                                            Type = 0,
                                            Id = id,
                                            PlayerId = -1,
                                            PlayerName = "Thông báo",
                                            Role = me.Role,
                                                Time = (int)(ServerUtils.CurrentTimeMillis() / 1000),
                                            Text = string.Format(TextServer.gI().CHANGED_SUBLEADER_CLAN, member.Name),
                                            Color = 1,
                                            NewMessage = true,
                                        });
                                        clan.ClanHandler.SendMessage(Service.ChangeMemberClan(member));
                                        clan.ClanHandler.Flush();
                                        break;
                                    }
                                case 2:
                                    {
                                        if (me.Role != 0) return;
                                        member.Role = 2;
                                        clan.ClanHandler.Chat(new ClanMessage()
                                        {
                                            Type = 0,
                                            Id = id,
                                            PlayerId = -1,
                                            PlayerName = "Thông báo",
                                            Role = me.Role,
                                                Time = (int)(ServerUtils.CurrentTimeMillis() / 1000),
                                            Text = string.Format(TextServer.gI().REMOVE_SUBLEADER_CLAN, member.Name),
                                            Color = 1,
                                            NewMessage = true,
                                        });
                                        clan.ClanHandler.SendMessage(Service.ChangeMemberClan(member));
                                        clan.ClanHandler.Flush();
                                        break;
                                    }
                            }

                            break;
                        }
                    //Leave Clan
                    case -55:
                        {
                            var character = _session?.Player?.Character;
                            if (character == null) return;
                            
                            var @charReal = (Character)character;
                            var clan = ClanManager.Get(charReal.ClanId);
                            if (clan == null) return;
                            var me = clan.ClanHandler.GetMember(character.Id);
                            if (me.Role == 0)
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.ServerMessage(TextServer.gI().DONT_LEAVE_CLAN));
                                return;
                            }
                            else
                            {
                                character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5,
                                    MenuNpc.Gi().TextMeo[5], MenuNpc.Gi().MenuMeo[1],
                                    character.InfoChar.Gender));
                                @charReal.TypeMenu = 6;
                            }

                            break;
                        }
                    //Cho đậu
                    case -54:
                        {
                            var character = _session?.Player?.Character;
                            if (character == null) return;
                           
                            var @charReal = (Character)character;

                            var timeServer = ServerUtils.CurrentTimeMillis();

                            if (Server.Gi().StartServerTime > timeServer)
                            {
                                var delay = (Server.Gi().StartServerTime - timeServer) / 1000;
                                if (delay < 1)
                                {
                                    delay = 1;
                                }

                                character.CharacterHandler.SendMessage(Service.DialogMessage(string.Format(TextServer.gI().DELAY_RESTART_SEC,
                                        delay)));
                                return;
                            }

                            if (Maintenance.Gi().IsStart)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Máy chủ đang tiến hành bảo trì, không thể thao tác ngay lúc này, vui lòng thoát game"));
                                return;
                            }

                            if (charReal.Delay.InvAction > timeServer)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn thao tác quá nhanh, chậm lại nhé"));
                                return;
                            }

                            var clan = ClanManager.Get(charReal.ClanId);
                            if (clan == null) return;
                            var id = message.Reader.ReadInt();
                            Server.Gi().Logger.Debug($"Clan Donate --------------- ------- id: {id}");
                            if (!character.CheckLockInventory()) return;
                            lock (clan.Messages)
                            {
                                var msg = clan.ClanHandler.GetMessage(id);
                                if (msg is not { Type: 1 } || msg.Recieve >= msg.MaxCap) return;
                                if (msg.PlayerId == character.Id) return;
                                var itemPea =
                                    character.ItemBox.FirstOrDefault(item => DataCache.IdDauThan.Contains(item.Id));
                                if (itemPea == null)
                                {
                                    character.CharacterHandler.SendMessage(
                                        Service.ServerMessage(TextServer.gI().NOT_FOUND_PEA_IN_BOX));
                                    return;
                                }
                                var playerMe = clan.ClanHandler.GetMember(msg.PlayerId);
                                if (playerMe == null) return;

                                character.CharacterHandler.RemoveItemBoxByIndex(itemPea.IndexUI, 1);
                                var itemNew = ItemHandler.Clone(itemPea);
                                itemNew.Quantity = 1;

                                var memMe = clan.ClanHandler.GetMember(character.Id);
                                memMe.Cho_đậu++;

                                playerMe.Nhận_đậu++;

                                var charPlus = ClientManager.Gi().GetCharacter(msg.PlayerId);
                                if (charPlus != null)
                                {
                                    if (charPlus.CharacterHandler.AddItemToBag(true, itemNew, "Clan cho đậu"))
                                    {
                                        charPlus.CharacterHandler.SendMessage(Service.SendBag(charPlus));
                                    }

                                    ;
                                    charPlus.CharacterHandler.SendMessage(Service.ServerMessage(
                                        string.Format(TextServer.gI().RECEIVE_PEA_CLAN,
                                            ItemCache.ItemTemplate(itemNew.Id).Name, character.Name)));
                                }
                                else
                                {
                                    clan.ClanHandler.AddCharacterPea(new CharacterPea()
                                    {
                                        PeaId = itemNew.Id,
                                        PlayerGive = character.Name,
                                        PlayerRevice = msg.PlayerId,
                                        Quantity = 1
                                    });
                                }

                                msg.Recieve++;
                                clan.ClanHandler.Chat(msg);
                            }

                            break;
                        }
                    //Chat clan
                    case -51:
                        {
                            var character = _session?.Player?.Character;
                            if (character == null) return;
                            var @charReal = (Character)character;
                            var action = message.Reader.ReadByte();
                            Server.Gi().Logger.Debug($"Chat Clan --------------- ------- action: {action}");
                            switch (action)
                            {
                                case 0:
                                    {
                                        var clan = ClanManager.Get(charReal.ClanId);
                                        var member = clan?.ClanHandler.GetMember(character.Id);
                                        if (member == null) return;
                                        var text = ServerUtils.FilterWords(message.Reader.ReadUTF());
                                        var lastMess = clan.Messages.LastOrDefault();
                                        var id = lastMess != null ? lastMess.Id + 1 : 0;
                                        clan.ClanHandler.Chat(new ClanMessage()
                                        {
                                            Type = 0,
                                            PlayerId = character.Id,
                                            PlayerName = character.Name,
                                            Time = (int)(ServerUtils.CurrentTimeMillis() / 1000),
                                            Text = text,
                                            Role = member.Role,
                                            Id = id,
                                            Color = 0,
                                        });
                                        break;
                                    }
                                //Xin đậu
                                case 1:
                                    {
                                        var clan = ClanManager.Get(charReal.ClanId);
                                        var member = clan?.ClanHandler.GetMember(character.Id);
                                        if (member == null) return;
                                        if (member.DelayPea > ServerUtils.CurrentTimeMillis())
                                        {
                                            character.CharacterHandler.SendMessage(
                                                Service.ServerMessage(TextServer.gI().DELAY_REVICE_PEA));
                                            return;
                                        }

                                        member.DelayPea = ServerUtils.CurrentTimeMillis() + 300000;

                                        var messCheck =
                                            clan.Messages.FirstOrDefault(m => m.Type == 1 && m.PlayerId == character.Id);
                                        ClanMessage newMes;
                                        if (messCheck == null)
                                        {
                                            var lastMess = clan.Messages.LastOrDefault();
                                            var id = lastMess != null ? lastMess.Id + 1 : 0;
                                            newMes = new ClanMessage()
                                            {
                                                Type = 1,
                                                PlayerId = character.Id,
                                                PlayerName = character.Name,
                                                    Time = (int)(ServerUtils.CurrentTimeMillis() / 1000),
                                                Role = member.Role,
                                                Id = id,
                                                NewMessage = true,
                                                Color = 0,
                                                Recieve = 0,
                                                MaxCap = 5
                                            };
                                        }
                                        else
                                        {
                                            messCheck.Recieve = 0;
                                            messCheck.Time = (int)(ServerUtils.CurrentTimeMillis() / 1000);
                                            newMes = messCheck;
                                        }

                                        clan.ClanHandler.Chat(newMes);
                                        break;
                                    }
                                case 2:
                                    {
                                        var clan = ClanManager.Get(message.Reader.ReadInt());
                                        if (clan == null) return;

                                        if (clan.Thành_viên_hiện_tại >= clan.Tối_đa_thành_viên)
                                        {
                                            character.CharacterHandler.SendMessage(
                                                Service.ServerMessage(TextServer.gI().MAX_MEMBER));
                                            break;
                                        }

                                        var messCheck =
                                            clan.Messages.FirstOrDefault(m => m.Type == 2 && m.PlayerId == character.Id);
                                        if (messCheck == null)
                                        {
                                            var lastMess = clan.Messages.LastOrDefault();
                                            var id = lastMess != null ? lastMess.Id + 1 : 0;
                                            messCheck = new ClanMessage()
                                            {
                                                Type = 2,
                                                PlayerId = character.Id,
                                                PlayerName = character.Name,
                                                    Time = (int)(ServerUtils.CurrentTimeMillis() / 1000),
                                                Role = 0,
                                                Id = id,
                                                NewMessage = true,
                                                Color = 1,
                                                Text = string.Format(TextServer.gI().PLEASE_INVITE_CLAN, character.Name)
                                            };
                                            clan.ClanHandler.Chat(messCheck);
                                        }

                                        break;
                                    }
                            }

                            break;
                        }
                    //View List Member Clan
                    case -50:
                        {
                            var character = _session?.Player?.Character;
                            if (character == null) return;
                            var clanId = message.Reader.ReadInt();
                            var clan = ClanManager.Get(clanId);
                            if (clan == null)
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.ServerMessage(TextServer.gI().ERROR_FIND_CLAN));
                                return;
                            }

                            character.CharacterHandler.SendMessage(Service.ClanMember(clan.Thành_viên, int.Parse(_session.Version.Replace(".", ""))));
                            break;
                        }
                    //Accept Please JoinMap Clan
                    case -49:
                        {
                            var character = _session?.Player?.Character;
                            
                            if (character == null) return;
                            var @charReal = (Character)character;
                            var clan = ClanManager.Get(charReal.ClanId);
                            if (clan == null) return;
                            var memMe = clan.ClanHandler.GetMember(character.Id);
                            if (memMe.Role != 0) return;
                            var id = message.Reader.ReadInt();
                            var action = message.Reader.ReadByte();
                            var msg = clan.ClanHandler.GetMessage(id);
                            if (msg == null) return;
                            switch (action)
                            {
                                //Đồng ý
                                case 0:
                                    {
                                        var text = "";
                                        if (clan.ClanHandler.GetMember(msg.PlayerId) != null)
                                        {
                                            text = string.Format(TextServer.gI().IN_CLAN_3, msg.PlayerName);
                                        }
                                        else
                                        {
                                            var checkChar = (Character)ClientManager.Gi().GetCharacter(msg.PlayerId);
                                            if (checkChar != null)
                                            {
                                                if (checkChar.ClanId != -1)
                                                {
                                                    text = string.Format(TextServer.gI().IN_CLAN_4, msg.PlayerName);
                                                }
                                                else
                                                {
                                                    if (clan.ClanHandler.AddMember(checkChar, 2))
                                                    {
                                                        text = string.Format(TextServer.gI().JOINED_CLAN, msg.PlayerName);
                                                        checkChar.ClanId = clan.Id;
                                                        checkChar.InfoChar.Bag = (sbyte)clan.ImgId;
                                                        if (TaskHandler.CheckTask(checkChar, 13, 0)) TaskHandler.gI().PlusSubTask(checkChar, 1);

                                                        var img = Cache.Gi().CLAN_IMAGES
                                                            .FirstOrDefault(i => i.Id == clan.ImgId);
                                                        checkChar.CharacterHandler.SendMessage(Service.GetImageBag(img));
                                                        if (checkChar.InfoChar.PhukienPart == -1) checkChar.CharacterHandler.SendZoneMessage(
                                                            Service.SendImageBag(checkChar.Id, clan.ImgId));
                                                        checkChar.CharacterHandler.SendMessage(
                                                            Service.ServerMessage(
                                                                string.Format(TextServer.gI().ACCEPT_INVITE_CLAN, clan.Name)));
                                                        checkChar.CharacterHandler.SendMessage(Service.MyClanInfo(checkChar));
                                                        checkChar.CharacterHandler.SendZoneMessage(
                                                            Service.UpdateClanId(checkChar.Id, clan.Id));
                                                        clan.ClanHandler.UpdateClanId();
                                                        CharacterDB.Update(checkChar);
                                                        ClanDB.Update(clan);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                lock (Server.SQLLOCK)
                                                {
                                                    var charCheckDb = CharacterDB.GetById(msg.PlayerId);

                                                    if (charCheckDb != null)
                                                    {
                                                        if (charCheckDb.ClanId != -1)
                                                        {
                                                            text = string.Format(TextServer.gI().IN_CLAN_4, msg.PlayerName);
                                                        }
                                                        else if (clan.ClanHandler.AddMember(charCheckDb, 2))
                                                        {
                                                            text = string.Format(TextServer.gI().JOINED_CLAN, msg.PlayerName);
                                                            charCheckDb.ClanId = clan.Id;
                                                            charCheckDb.InfoChar.Bag = (sbyte)clan.ImgId;
                                                            clan.ClanHandler.UpdateClanId();
                                                            ClanDB.Update(clan);
                                                            CharacterDB.Update(charCheckDb);
                                                        }
                                                    }

                                                }
                                            }
                                        }

                                        msg = new ClanMessage()
                                        {
                                            Type = 0,
                                            PlayerId = -1,
                                            PlayerName = "Thông báo",
                                            Time = (int)(ServerUtils.CurrentTimeMillis() / 1000),
                                            Text = text,
                                            Role = 0,
                                            Id = msg.Id,
                                            Color = 1,
                                        };
                                        clan.ClanHandler.Chat(msg);
                                        break;
                                    }
                                //Từ chối
                                case 1:
                                    {
                                        msg = new ClanMessage()
                                        {
                                            Type = 0,
                                            PlayerId = -1,
                                            PlayerName = "Thông báo",
                                                Time = (int)(ServerUtils.CurrentTimeMillis() / 1000),
                                            Text = $"Từ chối xin gia nhập của {msg.PlayerName}",
                                            Role = 0,
                                            Id = msg.Id,
                                            Color = 1,
                                        };
                                        clan.ClanHandler.Chat(msg);
                                        break;
                                    }
                            }

                            break;
                        }
                    //Search Clan
                    case -47:
                        {
                            var character = _session?.Player?.Character;
                            if (character == null) return;
                            var charReal = (Character)character;
                            if (charReal.ClanId != -1)
                            {
                                character.CharacterHandler.SendMessage(Service.MyClanInfo(charReal));
                                return;
                            }

                            var name = message.Reader.ReadUTF();
                            name = name.ToLower();
                            List<Clan> clan;
                            clan = name.Equals("") ? ClanManager.Entrys.Values.ToList() : ClanManager.GetList(name);
                            character.CharacterHandler.SendMessage(Service.ClanSearch(clan));
                            break;
                        }
                    //Create Clan
                    case -46:
                        {
                            var character = _session?.Player?.Character;
                            if (character == null) return;
                            
                            var @charReal = (Character)character;
                            var action = message.Reader.ReadByte();
                            Server.Gi().Logger.Debug($"Create Clan --------------- ------- action: {action}");
                            switch (action)
                            {
                                case 0:
                                    {
                                        break;
                                    }
                                case 1:
                                    {
                                        var clan = ClanManager.Entrys.Values.ToList();
                                        character.CharacterHandler.SendMessage(Service.ClanSearch(clan));
                                        character.CharacterHandler.SendMessage(Service.CreateClan(1));
                                        break;
                                    }
                                //Tạo mới clan
                                case 2:
                                    {
                                        if (@charReal.ClanId != -1)
                                        {
                                            character.CharacterHandler.SendMessage(
                                                Service.ServerMessage(TextServer.gI().IN_CLAN));
                                            return;
                                        }

                                        var id = message.Reader.ReadByte();
                                        var name = message.Reader.ReadUTF();
                                        name = name.ToLower();
                                        if (name.Length is < 3 or > 25)
                                        {
                                            character.CharacterHandler.SendMessage(
                                                Service.ServerMessage(TextServer.gI().ERROR_CREATE_CLAN));
                                            return;
                                        }

                                        if (ClanManager.Get(name) != null)
                                        {
                                            character.CharacterHandler.SendMessage(
                                                Service.ServerMessage(TextServer.gI().DUPLICATE_CLAN));
                                            return;
                                        }

                                        var image = Cache.Gi().CLAN_IMAGES.FirstOrDefault(im => im.Id == id);
                                        if (image == null || id >= 30)
                                        {
                                            character.CharacterHandler.SendMessage(
                                                Service.ServerMessage(TextServer.gI().ERROR_CREATE_CLAN));
                                            return;
                                        }


                                        if (image.Gold != -1)
                                        {
                                            if (image.Gold > character.InfoChar.Gold)
                                            {
                                                character.CharacterHandler.SendMessage(
                                                    Service.ServerMessage(TextServer.gI().NOT_ENOUGH_GOLD));
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            if (image.Diamond > @charReal.AllDiamond())
                                            {
                                                character.CharacterHandler.SendMessage(
                                                    Service.ServerMessage(TextServer.gI().NOT_ENOUGH_DIAMOND));
                                                return;
                                            }
                                        }

                                        var timeServer = ServerUtils.CurrentTimeSecond();
                                        var clanNew = new Clan()
                                        {
                                            Name = name,
                                            Khẩu_hiệu = "",
                                            ImgId = id,
                                            Điểm_Danh_Vọng = 0,
                                            LeaderName = character.Name,
                                            Thành_viên_hiện_tại = 0,
                                            Tối_đa_thành_viên = 10,
                                            Thời_gian_tạo_bang = timeServer,
                                            DateClanCreate = ServerUtils.TimeNow(),
                                            DateClanReset = ServerUtils.TimeNow(),
                                            Leader = new ClanLeader(character.GetHead(), character.GetBody(), character.GetLeg()),
                                        };
                                        clanNew.ClanHandler = new ClanHandler(clanNew);
                                        if (clanNew.ClanHandler.AddMember((Character)character, isFlush: false))
                                        {
                                            var clanId = ClanDB.Create(clanNew);
                                            if (clanId > 0)
                                            {
                                                clanNew.Id = clanId;
                                                ClanManager.Add(clanNew);
                                                if (image.Gold != -1)
                                                {
                                                    @charReal.MineGold(image.Gold);
                                                }
                                                else
                                                {
                                                    @charReal.MineDiamond(image.Diamond);
                                                }
                                                if (TaskHandler.CheckTask(charReal, 13, 0)) TaskHandler.gI().PlusSubTask(charReal, 1);
                                                @charReal.ClanId = clanId;
                                                character.InfoChar.Bag = id;
                                                if (character.InfoChar.PhukienPart == -1) character.CharacterHandler.SendZoneMessage(
                                                    Service.SendImageBag(character.Id, id));
                                                character.CharacterHandler.SendMessage(Service.GetImageBag(image));
                                                character.CharacterHandler.SendMessage(
                                                    Service.ServerMessage(string.Format(TextServer.gI().SUCCESS_CREATE_CLAN,
                                                        clanNew.Name)));
                                                character.CharacterHandler.SendMessage(Service.MeLoadInfo(@charReal));
                                                character.CharacterHandler.SendMessage(Service.ClosePanel());
                                                character.CharacterHandler.SendMessage(Service.MyClanInfo(charReal));
                                                CharacterDB.Update(charReal);
                                            }
                                            else
                                            {
                                                character.CharacterHandler.SendMessage(
                                                    Service.ServerMessage(TextServer.gI().ERROR_CREATE_CLAN));
                                                return;
                                            }
                                        }

                                        break;
                                    }
                                //Chọn biếu tượng clan
                                case 3:
                                    {
                                        character.CharacterHandler.SendMessage(Service.CreateClan(1));
                                        break;
                                    }
                                case 4:
                                    {
                                        var id = message.Reader.ReadByte();
                                        var name = message.Reader.ReadUTF();
                                        var clan = ClanManager.Get(@charReal.ClanId);
                                        var isUpdate = false;

                                        if (clan != null)
                                        {
                                            if (clan.ImgId != id)
                                            {
                                                var img = Cache.Gi().CLAN_IMAGES.FirstOrDefault(i => i.Id == id);
                                                if (img != null)
                                                {
                                                    if (img.Gold != -1)
                                                    {
                                                        if (img.Gold > character.InfoChar.Gold)
                                                        {
                                                            character.CharacterHandler.SendMessage(
                                                                Service.ServerMessage(TextServer.gI().NOT_ENOUGH_GOLD));
                                                            return;
                                                        }
                                                        else
                                                        {
                                                            charReal.MineGold(img.Gold);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (img.Diamond > @charReal.AllDiamond())
                                                        {
                                                            character.CharacterHandler.SendMessage(
                                                                Service.ServerMessage(TextServer.gI().NOT_ENOUGH_DIAMOND));
                                                            return;
                                                        }
                                                        else
                                                        {
                                                            charReal.MineDiamond(img.Diamond);
                                                        }
                                                    }

                                                    clan.ImgId = img.Id;
                                                    if (character.InfoChar.PhukienPart == -1) character.CharacterHandler.SendZoneMessage(Service.SendImageBag(character.Id, img.Id));
                                                    character.CharacterHandler.SendMessage(Service.GetImageBag(img));
                                                    character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                                                    isUpdate = true;
                                                }
                                            }

                                            if (!name.Equals(""))
                                            {
                                                clan.Khẩu_hiệu = ServerUtils.FilterWords(name.ToLower());
                                                isUpdate = true;
                                            }

                                            character.CharacterHandler.SendMessage(Service.UpdateClan(clan.ImgId, clan.Khẩu_hiệu));

                                            if (isUpdate)
                                            {
                                                ClanDB.Update(clan);
                                            }
                                        }

                                        break;
                                    }
                            }

                            break;
                        }
                    //Skill not focus
                    case -45:
                    {
                        var character = _session?.Player?.Character;
                        if (character == null || character.InfoChar.IsDie ||
                            character.InfoChar.MapId - 21 == character.InfoChar.Gender) return;
                        if (character.InfoChar.Stamina <= 0)
                        {
                            character.CharacterHandler.SendMessage(
                                Service.ServerMessage(TextServer.gI().NOT_ENOUGH_STAMINA));
                            return;
                        }
                        SkillHandler.SkillNotFocus(character, message);
                        break;
                    }

                    //USE_ITEM
                    case -43:
                    {
                        var character = (Character) _session?.Player?.Character;
                        if (character != null)
                        {
                            if (Maintenance.Gi().IsStart)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Máy chủ đang tiến hành bảo trì, không thể thao tác ngay lúc này, vui lòng thoát game"));
                                return;
                            }
                            var timeServer = ServerUtils.CurrentTimeMillis();
                            if (Server.Gi().StartServerTime > timeServer)
                            {
                                var delay = (Server.Gi().StartServerTime - timeServer) / 1000;
                                if (delay < 1)
                                {
                                    delay = 1;
                                }

                                character.CharacterHandler.SendMessage(Service.DialogMessage(string.Format(TextServer.gI().DELAY_RESTART_SEC,
                                        delay)));
                                return;
                            }
                           

                            if (character.Trade.IsTrade)
                            {
                                character.CharacterHandler.SendMessage(Service.DialogMessage("Không thể thực hiện thao tác này khi đang giao dịch"));
                                return;
                            }

                            var type = message.Reader.ReadByte();
                            var where = message.Reader.ReadByte();
                            var index = message.Reader.ReadByte();
                            short template = -1;
                            if (index == -1) template = message.Reader.ReadShort();

                                Server.Gi().Logger
                                .Debug(
                                    $"User Item --------------- Type: {type} - where: {where} - index: {index} - template: {template}");
                            if (!character.CheckLockInventory()) return;
                            switch (type)
                            {
                                //Use item
                                case 0:
                                {
                                    switch (where)
                                    {
                                        case 0: break;
                                        //In bag
                                        case 1:
                                        {
                                            ItemHandler.UseItemBag((Character) character, index, template);
                                            break;
                                        }
                                        //In Box
                                        case 2:
                                        {
                                            ItemHandler.UseItemBox((Character) character, index, template);
                                            break;
                                        }
                                    }

                                    break;
                                }
                                //Drop Item
                                case 1:
                                {
                                    if (DataCache.IdMapNotChangeZone.Contains(character.InfoChar.MapId))
                                    {
                                        _session.SendMessage(
                                            Service.ServerMessage(TextServer.gI().ITEM_CANNOT_BE_DROPPED_HERE));
                                        return;
                                    }

                                    var checkWp = MapManager.Get(character.InfoChar.MapId)?.TileMap.WayPoints
                                        .FirstOrDefault(waypoint => CheckTrueWaypoint(character, waypoint, 30));
                                    if (checkWp != null)
                                    {
                                        _session.SendMessage(
                                            Service.ServerMessage(TextServer.gI().ITEM_CANNOT_BE_DROPPED_NEAR_WP));
                                        return;
                                    }

                                    switch (where)
                                    {
                                        //Drop body
                                        case 0:
                                        {
                                            var itemDrop = character.ItemBody[index];
                                            if (itemDrop == null) return;
                                            if (DataCache.ItemNotDrop.Contains(itemDrop.Id)) return;

                                            var itemTemplate = ItemCache.ItemTemplate(itemDrop.Id);
                                            if (itemTemplate == null) return;

                                            if (DatabaseManager.ConfigManager.gI().IsDropAll)
                                            {
                                                _session.SendMessage(Service.UseItem(1, 2, index,
                                                    String.Format(TextServer.gI().CONFIRM_DROP_ITEM,
                                                        itemTemplate.Name)));
                                                return;
                                            }

                                            if ((!itemTemplate.IsDrop &&
                                                 !DataCache.TypeItemRemove.Contains(itemTemplate.Type)) ||
                                                (itemDrop.Id == 193 && itemDrop.Quantity == 99))
                                            {
                                                _session.SendMessage(
                                                    Service.ServerMessage(TextServer.gI().CANT_DROP_ITEM));
                                                return;
                                            }

                                            if (!itemTemplate.IsDrop &&
                                                DataCache.TypeItemRemove.Contains(itemTemplate.Type))
                                            {
                                                _session.SendMessage(Service.UseItem(1, 2, index,
                                                    String.Format(TextServer.gI().CONFIRM_DROP_ITEM,
                                                        itemTemplate.Name)));
                                                return;
                                            }

                                            character.CharacterHandler.DropItemBody(index);
                                            break;
                                        }
                                        //Drop bag
                                        case 1:
                                        {
                                            var itemDrop = character.CharacterHandler.GetItemBagByIndex(index);
                                            if (itemDrop == null)
                                            {
                                                return;
                                            }

                                                     //   if (DataCache.ItemNotDrop.Contains(itemDrop.Id)) return;

                                                        var itemTemplate = ItemCache.ItemTemplate(itemDrop.Id);
                                            if (itemTemplate == null) return;

                                            if (DatabaseManager.ConfigManager.gI().IsDropAll)
                                            {
                                                _session.SendMessage(Service.UseItem(1, 1, index,
                                                    String.Format(TextServer.gI().CONFIRM_DROP_ITEM,
                                                        itemTemplate.Name)));
                                                return;
                                            }


                                            if ((!itemTemplate.IsDrop &&
                                                 !DataCache.TypeItemRemove.Contains(itemTemplate.Type)) ||
                                                (itemDrop.Id == 193 && itemDrop.Quantity == 99))
                                            {
                                                _session.SendMessage(
                                                    Service.ServerMessage(TextServer.gI().CANT_DROP_ITEM));
                                                return;
                                            }

                                            if (!itemTemplate.IsDrop &&
                                                DataCache.TypeItemRemove.Contains(itemTemplate.Type))
                                            {
                                                _session.SendMessage(Service.UseItem(1, 1, index,
                                                    String.Format(TextServer.gI().CONFIRM_DROP_ITEM,
                                                        itemTemplate.Name)));
                                                return;
                                            }

                                            character.CharacterHandler.DropItemBag(index);
                                            break;
                                        }
                                    }

                                    break;
                                }
                                //Accept Use Item
                                case 2:
                                {
                                    switch (where)
                                    {
                                        case 1://Xác nhận xóa vật phẩm
                                        {
                                            character.CharacterHandler.RemoveItemBag(index, reason:"Tự Xóa vật phẩm");
                                            break;
                                        }
                                        case 2://Xác nhận xóa vật phẩm
                                        {
                                            character.CharacterHandler.RemoveItemBody(index);
                                            break;
                                        }
                                        case 3://Xác nhận dùng vật phẩm
                                        {
                                            ItemHandler.ConfirmUseItemBag((Character) character, index, template);
                                            break;
                                        }
                                    }

                                    break;
                                }
                            }
                        }

                        break;
                    }
                    //Cấp_Độ caption
                    case -41:
                    {
                        int gender = message.Reader.ReadByte();
                        _session.SendMessage(Service.SendLevelCaption(gender));
                        break;
                    }
                    //GET_ITEM
                    case -40:
                    {
                        var character = (Character) _session?.Player?.Character;
                        if (character == null) return;
                        if (Maintenance.Gi().IsStart)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Máy chủ đang tiến hành bảo trì, không thể thao tác ngay lúc này, vui lòng thoát game"));
                            return;
                        }

                        if (character.Trade.IsTrade)
                        {
                            character.CharacterHandler.SendMessage(Service.DialogMessage("Không thể thực hiện thao tác này khi đang giao dịch"));
                            return;
                        }

                        var timeServer = ServerUtils.CurrentTimeMillis();
                        if (Server.Gi().StartServerTime > timeServer)
                        {
                            var delay = (Server.Gi().StartServerTime - timeServer) / 1000;
                            if (delay < 1)
                            {
                                delay = 1;
                            }

                            character.CharacterHandler.SendMessage(Service.DialogMessage(string.Format(TextServer.gI().DELAY_RESTART_SEC,
                                    delay)));
                            return;
                        }
                        //if (character.Delay.InvAction > timeServer)
                        //{
                        //    character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn thao tác quá nhanh, chậm lại nhé"));
                        //    return;
                        //}

                        var type = message.Reader.ReadByte();
                        var id = message.Reader.ReadByte();
                            Server.Gi().Logger.Debug($"GET Item --------------- Type: {type} - Id: {id}");
                        switch (type)
                        {
                            //Item BOX to bag fix
                            case 0:
                                    {
                                        if (character.LengthBagNull() > 0)
                                        {
                                            var itemcheck = character.CharacterHandler.GetItemBoxByIndex(id);
                                            if (itemcheck == null) return;
                                            var itemclone = ItemHandler.Clone(itemcheck);

                                            if (character.CharacterHandler.AddItemToBag(true, itemclone, "Lấy từ rương qua người")) 
                                            if (TaskHandler.CheckTask(character, 0, 3))
                                            {
                                                TaskHandler.gI().PlusSubTask(character, 1);
                                            }
                                            character.CharacterHandler.RemoveItemBox(id);
                                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                                        }
                                        else
                                        {
                                            character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_BAG));
                                        }

                                        break;
                                    }
                            // item bag to box fix
                            case 1:
                            {
                                if (character.LengthBoxNull() > 0)
                                {
                                    var itemcheck = character.CharacterHandler.GetItemBagByIndex(id);
                                    if (itemcheck == null) return;
                                    var itemclone = ItemHandler.Clone(itemcheck);
                                    character.CharacterHandler.AddItemToBox(true, itemclone);
                                    character.CharacterHandler.RemoveItemBag(id, reason:"Cất vào rương");
                                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                                    character.CharacterHandler.SendMessage(Service.SendBox(character));
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_BOX));
                                }

                                break;
                            }





                            /*case 0:
                            {
                                var itemCheck = character.CharacterHandler.GetItemBoxByIndex(id);
                                if (itemCheck == null) return;
                                var itemTemplate = ItemCache.ItemTemplate(itemCheck.Id);
                                if (DataCache.IdDauThan.Contains(itemTemplate.Id))
                                {
                                    var @char = (Character) character;
                                    var countDauThan = @char.GetTotalDauThanBag();
                                    var plus = 10 - countDauThan;
                                    if (plus <= 0)
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service.ServerMessage(TextServer.gI().MAX_PEAS));
                                        return;
                                    }

                                    var itemClone = ItemHandler.Clone(itemCheck);
                                    if (plus < itemCheck.Quantity)
                                    {
                                        itemClone.Quantity = plus;
                                    }
                                    else if (plus >= itemCheck.Quantity)
                                    {
                                        itemClone.Quantity = itemClone.Quantity;
                                    }

                                    if (character.CharacterHandler.AddItemToBag(true, itemClone))
                                    {
                                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                                        if (plus == itemCheck.Quantity)
                                        {
                                            character.CharacterHandler.RemoveItemBox(id);
                                        }
                                        else
                                        {
                                            itemCheck.Quantity -= plus;
                                            character.CharacterHandler.SendMessage(Service.SendBox(character));
                                        }

                                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                                    }
                                }
                                else
                                {
                                    var indexBody = itemTemplate.Type == 32 ? 6 : itemTemplate.Type;
                                    if (itemTemplate.IsTypeBody() && itemTemplate.Require <= character.InfoChar.Điểm_thành_tích &&
                                        character.ItemBody[indexBody] == null)
                                    {
                                        character.CharacterHandler.RemoveItemBox(id);
                                        character.CharacterHandler.AddItemToBody(itemCheck, indexBody);
                                        character.CharacterHandler.UpdateInfo();
                                    }
                                    else if (character.CharacterHandler.AddItemToBag(true, itemCheck))
                                    {
                                        character.CharacterHandler.RemoveItemBox(id);
                                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                                    }
                                }

                                break;
                            }
                            //Item BAG to box
                            case 1:
                            {
                                var itemCheck = character.CharacterHandler.GetItemBagByIndex(id);
                                if (itemCheck == null) return;
                                var itemTemplate = ItemCache.ItemTemplate(itemCheck.Id);
                                if (DataCache.IdDauThan.Contains(itemCheck.Id))
                                {
                                    var @char = (Character) character;
                                    var countDauThan = @char.GetTotalDauThanBox();
                                    var plus = 20 - countDauThan;
                                    if (plus <= 0)
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service.ServerMessage(TextServer.gI().MAX_PEAS));
                                        return;
                                    }

                                    var itemClone = ItemHandler.Clone(itemCheck);
                                    if (plus < itemCheck.Quantity)
                                    {
                                        itemClone.Quantity = plus;
                                    }
                                    else if (plus >= itemCheck.Quantity)
                                    {
                                        itemClone.Quantity = itemCheck.Quantity;
                                    }

                                    if (character.CharacterHandler.AddItemToBox(true, itemClone))
                                    {
                                        character.CharacterHandler.SendMessage(Service.SendBag(character));
                                        if (plus == itemCheck.Quantity)
                                        {
                                            character.CharacterHandler.RemoveItemBag(id);
                                        }
                                        else
                                        {
                                            itemCheck.Quantity -= plus;
                                            character.CharacterHandler.SendMessage(Service.SendBag(character));
                                        }

                                        character.CharacterHandler.SendMessage(Service.SendBox(character));
                                    }
                                }
                                else
                                {
                                    if (character.CharacterHandler.AddItemToBox(true, itemCheck))
                                    {
                                        character.CharacterHandler.RemoveItemBag(id);
                                        character.CharacterHandler.SendMessage(Service.SendBox(character));
                                    }
                                }

                                break;
                            }*/
                            case 2:
                            {
                                break;
                            }
                            //Item body to box
                            case 3:
                            {
                                var itemBody = character.ItemBody[id];
                                if (itemBody == null) return;
                                if (Server.Gi().StartServerTime > timeServer)
                                {
                                    var delay = (Server.Gi().StartServerTime - timeServer) / 1000;
                                    if (delay < 1)
                                    {
                                        delay = 1;
                                    }

                                    character.CharacterHandler.SendMessage(Service.DialogMessage(string.Format(TextServer.gI().DELAY_RESTART_SEC,
                                            delay)));
                                    return;
                                }

                                if (Maintenance.Gi().IsStart)
                                {
                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Máy chủ đang tiến hành bảo trì, không thể thao tác ngay lúc này, vui lòng thoát game"));
                                    return;
                                }

                                if (character.Trade.IsTrade)
                                {
                                    character.CharacterHandler.SendMessage(Service.DialogMessage("Không thể thực hiện thao tác này khi đang giao dịch"));
                                    return;
                                }
                                if (character.CharacterHandler.AddItemToBox(false, itemBody))
                                {
                                    character.ItemBody[id] = null;
                                    character.CharacterHandler.UpdateInfo();
                                    character.CharacterHandler.SendMessage(Service.SendBox(character));

                                   // character.Delay.InvAction = timeServer + 1000;
                                    if ((character.InfoChar.ThoiGianDoiMayChu - timeServer) < 180000)
                                    {
                                        character.InfoChar.ThoiGianDoiMayChu = timeServer + 300000;
                                    }
                                    // character.Delay.SaveData += 1000;
                                    //TODO HANDLE SET INFO BODY
                                }

                                break;
                            }
                            //USE item bag for me
                            case 4:
                            {
                                ItemHandler.UseItemBag((Character) character, id);
                                break;
                            }
                            //Item BODY to bag
                            case 5:
                            {
                                var itemBody = character.ItemBody[id];
                                
                                if (itemBody == null) return;
                                // Vừa tháo giáp luyện tập ra khỏi người
                                if (ItemCache.ItemIsGiapLuyenTap(itemBody.Id))
                                {
                                    var @charRel = ((Character)character);
                                    @charRel.InfoMore.LastGiapLuyenTapItemId = itemBody.Id;
                                    @charRel.Delay.GiapLuyenTap = 60000 + ServerUtils.CurrentTimeMillis();
                                }
                                if (character.CharacterHandler.AddItemToBag(false, itemBody, "Lấy từ body vào hành trang"))
                                {
                                    character.CharacterHandler.RemoveItemBody(id);
                                    character.CharacterHandler.SendMessage(Service.SendBag(character));

//                                    character.Delay.InvAction = timeServer + 1000;
                                    
                                            switch (id)
                                            {
                                                case 0:
                                                    character.CharacterHandler.SendMessage(Service.PlayerLoadAo(character));
                                                    break;
                                                case 1:
                                                    character.CharacterHandler.SendMessage(Service.PlayerLoadQuan(character));
                                                    break;
                                                case 7:
                                                    character.CharacterHandler.UpdatePhukien();
                                                    break;
                                                case 8:
                                                    character.CharacterHandler.UpdateMountId();
                                                    break;
                                                case 9:
                                                    character.CharacterHandler.UpdatePet();
                                                    character.CharacterHandler.UpdateEffective();
                                                    break;
                                                case 10:
                                                    character.CharacterHandler.UpdateItem10();
                                                    break;
                                               
                                            }
                                            // character.Delay.SaveData += 1000;
                                            //TODO HANDLE SET INFO BODY

                                        }

                                break;
                            }
                            //USE ITEM FOR DISCIPLE
                            case 6:
                            {
                                ItemHandler.UseItemForDisciple((Character) character, id);
                                break;

                            }
                            // REMOVE ITEM FROM DISCIPLE
                            case 7:
                            {
                                var charReal = (Character) character;
                                var disciple = charReal.Disciple;
                                if (disciple == null)
                                {
                                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_GENDER));
                                    return;
                                }
                                var itemBody = disciple.ItemBody[id];
                                if (itemBody == null) return;
                                if (character.CharacterHandler.AddItemToBag(false, itemBody, "Lấy từ body đệ vào hành trang"))
                                {
                                    disciple.ItemBody[id] = null;
                                    disciple.CharacterHandler.UpdateInfo(true);
                                    if (disciple.Status == 0 || disciple.Status == 4) character.CharacterHandler.UpdateInfo();
                                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                                    character.CharacterHandler.SendMessage(Service.Disciple(2, disciple));
                                    //DiscipleDB.SaveInventory(disciple);
                                }
                                break;
                            }
                        }

                        break;
                    }
                    //Finish Load map
                    case -39:
                    {
                        var character = (Character) _session?.Player?.Character;
                        if (character == null) return;
                            if (character.IsNextMap)
                            {
                                var zone = character?.Zone;
                                if (zone != null)
                                {
                                    character.CharacterHandler.HandleJoinMap(zone);
                                    TaskHandler.gI().CheckTaskDoneGoToMap(character);
                                    //character.CharacterHandler.SendZoneMessage(Service.SendImageBag(character.Id, NgocRongGold.Sources.Application.Extension.Namecball.NamecBallHandler.SendBagNamecBall(character)));
                                }
                                
                            character.IsNextMap = false;
                        }

                        break;  
                    }
                    //Finish Update
                    case -38:
                    {
                            if (message.Reader.Available() > 0)
                            {
                                var ChooseCharId = message.Reader.ReadInt();
                                //await Login(ChooseCharId);
                            }
                        break;
                    }
                    //Magic tree
                    case -34:
                    {
                        var character = _session?.Player?.Character;
                        if (character == null) return;
                        {
                            var action = message.Reader.ReadByte();
                                Server.Gi().Logger
                                .Debug(
                                    $"Client {_session.Id} - Magic tree action ------------------------------- {action}");
                            switch (@action)
                            {
                                case 0:
                                case 1: break;
                                case 2:
                                {
                                    _session.SendMessage(Service.MagicTree0(character.Id));
                                    break;
                                }
                            }
                        }
                        break;
                    }
                    //Map Offline
                    case -33:
                    {
                        NextMap();
                        break;
                    }
                    //Background Template
                    case -32:
                    {
                            _session.SendMessage(Service.SendBgImg(_session.ZoomLevel, message.Reader.ReadShort()));
                        break;
                    }
                    case -30:
                    {
                        MessageSubCommand(message);
                        break;
                    }
                    case -29:
                    {
                        await MessageNotLogin(message);
                        break;
                    }
                    case -28:
                    {
                        MessageNotMap(message);
                        break;
                    }
                    case -27:
                        {
                            _session.HansakeMessage();
                            _session.SendMessage(Service.ServerList());
                            _session.SendMessage(Service.GetImageResource2());
                            _session.SendMessage(Service.GetImageResource());

                            break;
                        }
                    //Change map
                    case -23:
                    {
                        NextMap();
                  //      _session.Player.Character.CharacterHandler.SendMessage(Service.ServerMessage("ko cho qua"));
                        
                        break;
                    }
                    //Pick Item Map
                    case -20:
                    {
                        var character = _session?.Player?.Character;
                        character?.CharacterHandler.PickItemMap(message.Reader.ReadShort());
                        break;
                    }
                    //Back Home
                    case -15:
                    {
                        var character = _session?.Player?.Character;
                        if (character == null || !character.InfoChar.IsDie) return;
                        character.CharacterHandler.BackHome();
                        break;
                    }
                    //Leave from dead
                    case -16:
                    {
                        var character = _session?.Player?.Character;
                        if (character == null || !character.InfoChar.IsDie) return;
                        character.CharacterHandler.LeaveFromDead();
                        break;
                    }
                    //Character Move
                    case -7:
                    {
                        var character = _session?.Player?.Character;

                        if (character == null) return;
                        var type = message.Reader.ReadByte();
                        var toX = message.Reader.ReadShort();
                        var toY = character.InfoChar.Y;
                        if (message.Reader.Available() > 0)
                        {
                            toY = message.Reader.ReadShort();
                        }
                            // Console.WriteLine("X " + toX + " Y " + toY);
                        Server.Gi().Logger.Debug($"Move X: {toX}, Y: {toY}");
                        character.CharacterHandler.MoveMap(toX, toY, type);
                        break;
                    }
                    //ITEM_BUY
                    case 6:
                    {
                        var character = (Character) _session?.Player?.Character;
                        if (character != null)
                        {
                            if (Maintenance.Gi().IsStart)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Máy chủ đang tiến hành bảo trì, không thể thao tác ngay lúc này, vui lòng thoát game"));
                                return;
                            }

                            var timeServer = ServerUtils.CurrentTimeMillis();
                            
                            if (Server.Gi().StartServerTime > timeServer)
                            {
                                var delay = (Server.Gi().StartServerTime - timeServer) / 1000;
                                if (delay < 1)
                                {
                                    delay = 1;
                                }

                                character.CharacterHandler.SendMessage(Service.DialogMessage(string.Format(TextServer.gI().DELAY_RESTART_SEC,
                                        delay)));
                                return;
                            }

                            //if (character.Delay.InvAction > timeServer)
                            //{
                            //    character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn thao tác quá nhanh, chậm lại nhé"));
                            //        character.CharacterHandler.SendMessage(Service.BuyItem(character));
                            //        return;
                            //}

                            var type = message.Reader.ReadByte();
                            var id = message.Reader.ReadShort();
                            short quantity = 1;
                            if (message.Reader.Available() > 0) quantity = message.Reader.ReadShort();
                                Server.Gi().Logger
                                .Debug($"Buy Item --------------- Type: {type} - Id Item: {id} - Quantity: {quantity}");
                            if (!character.CheckLockInventory())
                            {
                                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                return;
                            }

                            ItemHandler.BuyItem((Character) character, type, id, quantity);
                        }

                        break;
                    }
                    //ITEM_SALE
                    case 7:
                    {
                        var character = (Character) _session?.Player?.Character;
                        if (character == null) return;
                        if (!character.CheckLockInventory()) return;

                        if (Maintenance.Gi().IsStart)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Máy chủ đang tiến hành bảo trì, không thể thao tác ngay lúc này, vui lòng thoát game"));
                            return;
                        }
                        if (character.Trade.IsTrade)
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Không thể bán đồ khi đang giao dịch"));
                                break;
                            }
                        var timeServer = ServerUtils.CurrentTimeMillis();

                        if (Server.Gi().StartServerTime > timeServer)
                        {
                            var delay = (Server.Gi().StartServerTime - timeServer) / 1000;
                            if (delay < 1)
                            {
                                delay = 1;
                            }

                            character.CharacterHandler.SendMessage(Service.DialogMessage(string.Format(TextServer.gI().DELAY_RESTART_SEC,
                                    delay)));
                            return;
                        }

                        //if (character.Delay.InvAction > timeServer)
                        //{
                        //    character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn thao tác quá nhanh, chậm lại nhé"));
                        //    return;
                        //}

                        ItemHandler.SellItem((Character) character, message.Reader.ReadByte(),
                            message.Reader.ReadByte(), message.Reader.ReadShort());
                        break;
                    }
                    //Requests Mob Template
                    case 11:
                    {
                        var character = (Character) _session?.Player?.Character;
                        if (character == null) break;
                        var monsterId = message.Reader.ReadByte();
                        
                        _session.SendMessage(Service
                            .SendMonsterTemplate(_session.ZoomLevel, monsterId));
                        break;
                    }
                    //GOTO_PLAYER
                    case 18:
                    {
                        var character = _session?.Player?.Character;
                        if (character == null) break;
                        var charReal = (Character) character;

                        var caiTrang = character.ItemBody[5];
                        if (caiTrang == null)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn cần đeo cải trang có khả năng dịch chuyển tức thời"));
                            break;
                        }

                        var optionDCTT = caiTrang.Options.FirstOrDefault(option => option.Id == 33);
                        
                        if (optionDCTT == null)
                        {
                            character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn cần đeo cải trang có khả năng dịch chuyển tức thời"));
                            break;
                        }


                        var charId = message.Reader.ReadInt();
                        var @char = charReal.Friends.FirstOrDefault(friend => friend.Id == charId);
                        var delay = charReal.Delay.TeleportToPlayer;
                        var timeServer = ServerUtils.CurrentTimeMillis();
                        charReal.Delay.TeleportToPlayer = timeServer + 5000;
                        if (@char != null)
                        {
                            var charTeleport = ClientManager.Gi().GetCharacter(@char.Id);
                            if (charTeleport != null)
                            {
                                character.CharacterHandler.SendMessage(Service.ListFriend3(@char.Id, true));
                                if (delay > timeServer)
                                {
                                    var time = (delay - timeServer) / 1000;
                                    character.CharacterHandler.SendMessage(
                                        Service.ServerMessage(string.Format(TextServer.gI().TELEPORT_DELAY, time)));
                                    break;
                                }

                                var mapId = charTeleport.InfoChar.MapId;
                                if (DataCache.IdMapKarin.Contains(mapId) || DataCache.IdMapSpecial.Contains(mapId))
                                {
                                    character.CharacterHandler.SendMessage(
                                        Service.ServerMessage(TextServer.gI().TELEPORT_ERROR));
                                    break;
                                }
                                var charTeleportReal = (Model.Character.Character) charTeleport;
                                if (charTeleportReal.InfoBuff.AnDanh || charTeleportReal.InfoBuff.AnDanh2)
                                    {
                                        character.CharacterHandler.SendMessage(
                                       Service.ServerMessage(TextServer.gI().TELEPORT_ERROR));  
                                        break;
                                    }
                                var mapTeleport = MapManager.Get(mapId);
                                if (mapTeleport != null)
                                {
                                   
                                        switch (mapTeleport.Id)
                                        {
                                            case int i when DataCache.IdMapCold.Contains(i):
                                                {
                                                    if (character.InfoTask.Id < 24)
                                                    {
                                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn phải làm nhiệm vụ [Chú bé đến từ tương lai] trước"));
                                                        character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                                        return;
                                                    }
                                                    break;
                                                }
                                            case 79:
                                                {
                                                    if (character.InfoTask.Id < 23)
                                                    {
                                                        character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn phải làm nhiệm vụ [Nhiệm vụ Tiểu Đội Sát Thủ] trước"));
                                                        character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                                        return;
                                                    }
                                                    break;
                                                }
                                            case 5:
                                                if (character.InfoTask.Id < 12)
                                                {
                                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn phải làm nhiệm vụ [Nhiệm vụ tiên học lễ] trước"));
                                                    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                                    return;
                                                }
                                                break;
                                            case 19:
                                                if (character.InfoTask.Id < 18)
                                                {
                                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn phải làm nhiệm vụ [Tiêu diệt quái tinh nhuệ] trước"));
                                                    return;
                                                }
                                                break;
                                            case 20:
                                                if (character.InfoTask.Id < 14)
                                                {
                                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn phải làm nhiệm vụ [Nhiệm vụ bang hội lần 1] trước"));
                                                    return;
                                                }
                                                break;
                                            case int i when DataCache.IdMapThucVat.Contains(i):
                                                if (character.CharacterHandler.GetItemBagById(992) == null)
                                                {
                                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn phải sỡ hữu nhẫn thời không sai lệch trước"));
                                                    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                                    return;
                                                }
                                                break;
                                            case int i when DataCache.IdMapThanhDia.Contains(i):
                                                if (ClanManager.Get(character.ClanId) == null)
                                                {
                                                    character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn phải có bang hội trước"));
                                                    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                                    return;
                                                }
                                                break;
                                        }
                                        var zone = charTeleport.Zone;
                                    if (zone == null) break;
                                    if (zone.Characters.Count >= mapTeleport.TileMap.MaxPlayers)
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service.ServerMessage(TextServer.gI().MAX_NUMCHARS));
                                        break;
                                    }

                                    character.InfoChar.X = charTeleport.InfoChar.X;
                                    character.InfoChar.Y = charTeleport.InfoChar.Y;
                                        _session.SendMessage(Service.SendTeleport(character.Id,
                                            2));
                                        character.Zone.Map.OutZone(character);
                                        zone.ZoneHandler.JoinZone((Character)character, false, false,
                                            2);
                                    }
                                else
                                {
                                    character.CharacterHandler.SendMessage(
                                        Service.ServerMessage(TextServer.gI().TELEPORT_ERROR));
                                    return;
                                }
                            }
                            else
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.ServerMessage(TextServer.gI().USER_OFFLINE));
                                character.CharacterHandler.SendMessage(Service.ListFriend3(@char.Id, false));
                            }
                        }
                        else
                        {
                            character.CharacterHandler.SendMessage(
                                Service.ServerMessage(TextServer.gI().FRIEND_NOT_FOUND));
                        }

                        break;
                    }
                    //CHANGE_ZONE
                    case 21:
                    {
                        var character = _session?.Player?.Character;
                        if (character == null) return;
                        var mapId = character.InfoChar.MapId;
                            var map = MapManager.Get(mapId);
                            if (DataCache.IdMapNotChangeZone.Contains(mapId))
                        {
                            character.CharacterHandler.SendMessage(
                                Service.ServerMessage(TextServer.gI().NOT_CHANGEZONE));
                            return;
                        }

                        var @char = (Character) character;
                        var delayChangeZone = @char.Delay.ChangeZone;
                        var timeServer = ServerUtils.CurrentTimeMillis();
                        if (delayChangeZone > timeServer && character.Player.Role == 0)
                        {
                            var timeDelay = (int) (delayChangeZone - timeServer) / 1000;
                            character.CharacterHandler.SendMessage(Service.OpenUiSay(5,
                                string.Format(TextServer.gI().DELAY_CHANGEZONE, timeDelay), false,
                                @char.InfoChar.Gender));
                            return;
                        }

                        if (map != null)
                        {
                            var zoneId = message.Reader.ReadByte();
                            IZone zoneNext;
                               
                                    if (zoneId == -1)
                            {
                                var charZoneId = character.InfoChar.ZoneId;
                                zoneNext = map.GetZoneById(map.Zones.Count - charZoneId > 3
                                    ? ServerUtils.RandomNumber(charZoneId + 1, map.Zones.Count)
                                    : ServerUtils.RandomNumber(0, charZoneId - 1));
                            }
                            else
                            {
                                zoneNext = map.GetZoneById(zoneId);
                            }

                            if (zoneNext != null)
                            {
                                if (zoneNext.Characters.Count < map.TileMap.MaxPlayers)
                                {
                                    map.OutZone(character);
                                    map.JoinZone((Character) character, zoneId);
                                        @char.Delay.ChangeZone = (character.InfoChar.TimeAutoPlay > 0 ? 5000 : 15000) + timeServer;
                                }
                                else
                                {
                                    character.CharacterHandler.SendMessage(Service.OpenUiSay(5,
                                        TextServer.gI().MAX_NUMCHARS, false, character.InfoChar.Gender));
                                    return;
                                }
                            }
                            else
                            {
                                character.CharacterHandler.SendMessage(Service.OpenUiSay(5,
                                    TextServer.gI().NOT_CHANGEZONE, false, character.InfoChar.Gender));
                                return;
                            }
                        }
                        else
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiSay(5, TextServer.gI().NOT_CHANGEZONE,
                                false, character.InfoChar.Gender));
                            return;
                        }

                        break;
                    }
                    //Menu
                    case 22:
                    {
                        var character = _session?.Player?.Character;
                        if (character == null) return;
                        Menu.Menu.MenuHandler(message, (Character) character);
                        break;
                    }
                    //OPEN_UI_ZONE
                    case 29:
                    {
                        var character = _session?.Player?.Character;
                        if (character == null) return;
                            var mapId = character.InfoChar.MapId;
                            if (DataCache.IdMapNotChangeZone.Contains(mapId))
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.ServerMessage(TextServer.gI().NOT_CHANGEZONE));
                                return;
                            }
                            var map = MapManager.Get(mapId);

                            if (map != null)
                        {
                            character.CharacterHandler.SendMessage(Service.OpenUiZone(map));
                        }
                        else
                        {
                            character.CharacterHandler.SendMessage(
                                Service.ServerMessage(TextServer.gI().NOT_CHANGEZONE));
                            return;
                        }

                        break;
                    }
                    //UI_CONFIRM
                    case 32:
                    {
                        var character = _session?.Player?.Character;
                        if (character != null) Menu.Menu.UiConfirm(message, (Character) character);
                        break;
                    }
                    //OPEN_UI_MENU
                    case 33:
                    {
                        var character = _session?.Player?.Character;
                        if (character != null) Menu.Menu.OpenUiMenu(message.Reader.ReadShort(), (Character) character);
                        break;
                    }
                    //Select skill
                    case 34:
                    {
                        var character = _session?.Player?.Character;
                        if (character == null) return;
                        character.InfoChar.CSkill = message.Reader.ReadShort();
                        break;
                    }
                    case -76:
                        {
                            // Bo mong
                            int indexTask = message.Reader.ReadByte();

                            var character = (Model.Character.Character)_session.Player.Character;
                            var currentTask = character.DataBoMong.Quests[indexTask];
                            if (!currentTask.IsFinish())
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Nhiệm vụ này chưa được hoàn thành"));
                                break;
                            }
                            else if (currentTask.IsReward())
                            {
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Nhiệm vụ này đã được nhận thưởng"));
                            }
                            else
                            {
                                currentTask.Reward_Status = Extension.SideQuest.BoMong.BoMongQuest_RewardStatus.COLLECTED;
                                character.InfoChar.DiamondLock += currentTask.Reward;
                                character.CharacterHandler.SendMessage(Service.BuyItem(character));
                                //character.CharacterHandler.AddItemToBag(true, ItemCache.GetItemDefault(457, currentTask.Reward));
                                character.CharacterHandler.SendMessage(BoMongQuest_Handler.gI().OpenHubTask((Character)character));
                                character.CharacterHandler.SendMessage(Service.ServerMessage("Chúc mừng bạn đã hoàn thành nhiệm vụ\nBạn nhận được " + currentTask.Reward + " hồng ngọc"));
                                //character.CharacterHandler.SendMessage(Service.SendBag(character));
                            }
                            break;
                        }
                    //Chat Map // Public chat
                    case 42:

                        break;
                    case 44:
                        {
                            var character = (Character)_session?.Player?.Character;
                            if (character == null) return;
                            var role = _session?.Player?.Role;
                            var text = message.Reader.ReadUTF();


                            if (!text.Equals(""))
                            {
                                switch (text)
                                {
                                    case string eff when text.Contains("effchar_"):
                                        {
                                            character.CharacterHandler.SendMessage(EffectCharacter.sendInfoEffChar(character.Id, int.Parse(eff.Replace("effchar_", "")), -1, -1, 10, 1));
                                            break;
                                        }
                                    case "zz":
                                        {
                                            character.CharacterHandler.SendMessage(EffectCharacter.addEff(17, (short)(character.InfoChar.X - 50), (short)(character.InfoChar.Y - 60),-1, -1, 10));
                                            character.CharacterHandler.SendMessage(EffectCharacter.addEff(25, (short)(character.InfoChar.X - 30), (short)(character.InfoChar.Y - 60), -1, -1, 10));
                                            character.CharacterHandler.SendMessage(EffectCharacter.addEff(50, (short)(character.InfoChar.X - 10), (short)(character.InfoChar.Y - 60), -1, -1, 10));
                                            character.CharacterHandler.SendMessage(EffectCharacter.addEff(51, (short)(character.InfoChar.X + 10), (short)(character.InfoChar.Y - 60), -1, -1, 10));
                                            character.CharacterHandler.SendMessage(EffectCharacter.addEff(59, (short)(character.InfoChar.X + 30), (short)(character.InfoChar.Y - 60), -1, -1, 10));
                                            character.CharacterHandler.SendMessage(EffectCharacter.addEff(60, (short)(character.InfoChar.X + 50), (short)(character.InfoChar.Y - 60), -1, -1, 10));
                                            character.CharacterHandler.SendMessage(EffectCharacter.addEff(61, (short)(character.InfoChar.X), (short)(character.InfoChar.Y - 60), -1, -1, 10));
                                            character.CharacterHandler.SendMessage(EffectCharacter.addEff(62, (short)(character.InfoChar.X - 30), (short)(character.InfoChar.Y), -1, -1, 10));
                                            character.CharacterHandler.SendMessage(EffectCharacter.addEff(63, (short)(character.InfoChar.X - 10), (short)(character.InfoChar.Y), -1, -1, 10));

                                            character.CharacterHandler.SendMessage(EffectCharacter.addEff(64, (short)(character.InfoChar.X + 10), (short)(character.InfoChar.Y), -1, -1, 10));
                                            character.CharacterHandler.SendMessage(EffectCharacter.addEff(65, (short)(character.InfoChar.X + 30), (short)(character.InfoChar.Y), -1, -1, 10));

                                        }
                                        break;
                                    case "tettet" or "tet tet":
                                        {
                                            var temp = character.Zone.Pets2.Values.FirstOrDefault(kilan => kilan.DelayDiTheo < ServerUtils.CurrentTimeMillis() && Math.Abs(character.InfoChar.X - kilan.InfoChar.X) <= 150);
                                            if (temp != null)
                                            {
                                                character.Pet2 = temp;
                                                temp.Character = character;
                                                temp.DelayDiTheo = 10000 + ServerUtils.CurrentTimeMillis();
                                            }
                                            break;
                                        }
                                    case "bien hinh":
                                        {
                                            var charReal = (Character)character;
                                            var disciple = charReal.Disciple;
                                            if (disciple != null && disciple.Type == 2)
                                            {
                                                disciple.IsBienHinh = !disciple.IsBienHinh;
                                                disciple.CharacterHandler.UpdateInfo();
                                                disciple.Character.CharacterHandler.SendMessage(Service.PublicChat(disciple.Id, "Biến hình chíu chíu"));
                                            }
                                            break;
                                        }
                                    case "di theo":
                                        {
                                            var charReal = (Character)character;
                                            var disciple = charReal.Disciple;
                                            if (disciple != null)
                                            {
                                                if (charReal.InfoChar.Fusion.IsFusion || disciple.Status >= 4)
                                                {
                                                    charReal.CharacterHandler.SendMessage(
                                                        Service.ServerMessage(TextServer.gI().DO_NOT_ACTION_DISCIPLE));
                                                    return;
                                                }

                                                if (disciple.Status == 3)
                                                {
                                                    async void Action()
                                                    {
                                                        await Task.Delay(2000);
                                                        charReal.Zone.ZoneHandler.AddDisciple(disciple);
                                                        charReal.CharacterHandler.SendMessage(Service.PublicChat(disciple.Id,
                                                        "Bái kiến sư phụ"));
                                                    }

                                                    var task = new Task(Action);
                                                    task.Start();
                                                }
                                                else
                                                {
                                                    charReal.CharacterHandler.SendMessage(Service.PublicChat(disciple.Id,
                                                        "Ok, con đi theo sư phụ"));

                                                }
                                                disciple.Status = 0;
                                            }
                                            break;
                                        }
                                    case "bao ve":
                                        {
                                            var charReal = (Character)character;
                                            var disciple = charReal.Disciple;
                                            if (charReal.InfoChar.Fusion.IsFusion || disciple.Status >= 3)
                                            {
                                                charReal.CharacterHandler.SendMessage(
                                                    Service.ServerMessage(TextServer.gI().DO_NOT_ACTION_DISCIPLE));
                                                return;
                                            }

                                            charReal.CharacterHandler.SendMessage(Service.PublicChat(disciple.Id,
                                                "Ok, con sẽ bảo vệ sư phụ"));
                                            disciple.Status = 1;
                                            break;
                                        }
                                    case "tan cong":
                                        {
                                            var charReal = (Character)character;
                                            var disciple = charReal.Disciple;
                                            if (charReal.InfoChar.Fusion.IsFusion || disciple.Status >= 3)
                                            {
                                                charReal.CharacterHandler.SendMessage(
                                                    Service.ServerMessage(TextServer.gI().DO_NOT_ACTION_DISCIPLE));
                                                return;
                                            }

                                            charReal.CharacterHandler.SendMessage(Service.PublicChat(disciple.Id,
                                                "Ok, sư phụ cứ để con lo cho"));
                                            disciple.Status = 2;
                                            break;
                                        }
                                    case "ve nha":
                                        {
                                            var charReal = (Character)character;
                                            var disciple = charReal.Disciple;
                                            if (charReal.InfoChar.Fusion.IsFusion || disciple.Status == 4)
                                            {
                                                charReal.CharacterHandler.SendMessage(
                                                    Service.ServerMessage(TextServer.gI().DO_NOT_ACTION_DISCIPLE));
                                                return;
                                            }

                                            charReal.CharacterHandler.SendMessage(Service.PublicChat(disciple.Id,
                                                "Bibi sư phụ..."));

                                            async void Action()
                                            {
                                                await Task.Delay(2000);
                                                try
                                                {
                                                    if (disciple.Zone != null && disciple.Zone.ZoneHandler != null)
                                                    {
                                                        disciple.Zone.ZoneHandler.RemoveDisciple(disciple);
                                                    }
                                                }
                                                catch (Exception e)
                                                {
                                                    Server.Gi().Logger.Error($"Error disciple.Zone.ZoneHandler.RemoveDisciple in Controller.cs: {e.Message} \n {e.StackTrace}", e);
                                                }
                                            }

                                            disciple.Status = 3;
                                            var task = new Task(Action);
                                            task.Start();
                                            break;
                                        }
                                }
                            }
                        
                        if (!text.Equals("") && character != null)
                        {
                            character.CharacterHandler.SendZoneMessage(Service.PublicChat(character.Id,
                                ServerUtils.FilterWords(text)));
                                Server.Gi().Logger.Debug(text);
                        }


                        if (!text.Equals("") && character != null && role == 1)
                        {
                                if (text.Contains("gasman"))
                                {
                                    text = text.Replace("gasman", "").Trim();
                                    if (int.TryParse(text, out var n))
                                    {
                                        Maintenance.Gi().Start(int.Parse(text));
                                    }
                                    else
                                    {
                                        Maintenance.Gi().Start(60);
                                    }
                                }
                                #region menuAdmin
                                if (text.Contains("adm"))
                                {
                                    var player = ClientManager.Gi().Players.Count;
                                    var session = ClientManager.Gi().Sessions.Count;
                                    var thread = System.Diagnostics.Process.GetCurrentProcess().Threads.Count;
                                    var allPlayerInSever = $"{ServerUtils.Color("green")}Online: {player} {ServerUtils.Color("blue")}Sessions: {session} {ServerUtils.Color("light-green")}Thread: {thread} ";
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(41, $"{ServerUtils.Color("red")}------YTB KhanhDTK------" + allPlayerInSever, MenuNpc.Gi().MenuAdmin[0], character.InfoChar.Gender));
                                    character.TypeMenu = 0;
                                }
                                if (text.Contains("kiki"))
                                {
                                    var zone = character.Zone;
                                    var HoMapVang = new Pet2(zone, (int)TemplatePET.HO_MAP_VANG);
                                    zone.ZoneHandler.AddPet(HoMapVang);
                                }
                                if (text.Contains("clearss"))
                                {
                                    ClientManager.Gi().ClearSessionNull();

                                }
                                if (text.Contains("rsskill"))
                                {
                                    character.DataDaiHoiVoThuat23.Handler.ReleaseSkill(character);

                                }
                                //if (text.Contains("1"))
                                //{
                                //    for (int i = 0; i < 1000; i++)
                                //    {
                                //        if (i == 30) continue;
                                //        UserDB.CreateUser("bot" + i, "passbot" + i, i);
                                //        CharacterDB.Create(CharacterDB.GetById(10000 + i), i);
                                //    }

                                //}
                                #endregion
                                #region Item

                                if (text.Contains("i"))
                            {
                                text = text.Replace("i", "").Trim();
                                try
                                {
                                    var arrItem = text.Split("sl");
                                    var itemAdd = ItemCache.GetItemDefault(short.Parse(arrItem[0]));
                                    var template = ItemCache.ItemTemplate(itemAdd.Id);
                                    var count = 1;
                                        if (template.IsUpToUp)
                                        {
                                            try
                                            {
                                                count = Math.Abs(int.Parse(arrItem[1]));
                                                
                                            }
                                            catch (Exception)
                                            {
                                                // ignored
                                            }
                                        }
                                        
                                        
                                    itemAdd.Options.Add(new OptionItem()
                                    {
                                        Id = 30,
                                        Param = 0,
                                    });


                                    itemAdd.Quantity = count;
                                       
                                    character.CharacterHandler.AddItemToBag(true, itemAdd, "item");
                                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                                    character.CharacterHandler.SendMessage(
                                        Service.ServerMessage(string.Format(TextServer.gI().ADD_ITEM,
                                            $"x{count} {template.Name}")));
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }
                           
                            #endregion

                            #region SKH
                            if (text.Contains("skh"))
                            {
                                text = text.Replace("skh", "").Trim();
                                try
                                {
                                    var arrItem = text.Split("set");
                                    var itemAdd = ItemCache.GetItemDefault(short.Parse(arrItem[0]));
                                    var template = ItemCache.ItemTemplate(itemAdd.Id);
                                    var skh = int.Parse(arrItem[1]);

                                    itemAdd.Options.Add(new OptionItem()
                                    {
                                        Id = skh,
                                        Param = 0,
                                    });

                                    itemAdd.Options.Add(new OptionItem()
                                    {
                                        Id = LeaveItemHandler.GetSKHDescOption(skh),
                                        Param = 0,
                                    });

                                    itemAdd.Options.Add(new OptionItem()
                                    {
                                        Id = 30,
                                        Param = 0,
                                    });
                                    itemAdd.Quantity = 1;
                                    character.CharacterHandler.AddItemToBag(true, itemAdd, "item");
                                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                                    character.CharacterHandler.SendMessage(
                                        Service.ServerMessage(string.Format(TextServer.gI().ADD_ITEM,
                                            $"{template.Name}")));
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }
                                #endregion
                                #region Map
                            else if (text == "nrsd")
                                {
                                    var clan = Application.Manager.ClanManager.Get(character.ClanId);
                                    if (clan.DataBlackBall.ListCurrentBlackball.Count >= 1)
                                    {
                                        var menu = new List<string>();
                                        character.ListCollectBlackBall.Clear();
                                        for (int i = 0; i < clan.DataBlackBall.ListCurrentBlackball.Count; i++)
                                        {
                                            if (!character.Blackball.CurrentListBuff.Contains(clan.DataBlackBall.ListCurrentBlackball[i]))
                                            {
                                                menu.Add($"Ngọc rồng\n{clan.DataBlackBall.ListCurrentBlackball[i].Star} sao");
                                                character.ListCollectBlackBall.Add(clan.DataBlackBall.ListCurrentBlackball[i]);
                                            }
                                        }
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm((short)29, "Bang hội của ngươi có vài phần thưởng này!\nNgươi có muốn nhận không?", menu, character.InfoChar.Gender));
                                        character.TypeMenu = 1;
                                    }
                                    else
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm((short)29, "Ta có thể giúp gì cho ngươi?", BlackballCache.Menus[1], character.InfoChar.Gender));
                                        character.TypeMenu = 2;
                                    }
                                }
                                else if (text.Contains("m"))
                            {
                                text = text.Replace("m", "").Trim();
                                    try
                                    {
                                        var mapId = int.Parse(text);
                                        var @char = (Character)character;

                                        var mapOld = MapManager.Get(character.InfoChar.MapId);
                                        var map = MapManager.Get(mapId);
                                        mapOld.OutZone(character);
                                        map.GetZoneNotMaxPlayer().ZoneHandler.JoinZone(@char, true, true, 0);
                                    }
                                    catch (Exception)
                                    {
                                        // ignored
                                    }

                                return;
                            }

                            #endregion
                        }
                            
                        break;
                    }
                    //Fight Monster
                    case 54:
                    {
                        var character = _session?.Player?.Character;
                        if (character == null || character.IsDontMove()) return;
                        SkillHandler.AttackMonster(character, message);
                        break;
                    }
                    //GET_IMG_BY_NAME
                    case 66:
                    {
                        var character = _session?.Player?.Character;
                        if (character == null) return;
                        var imageName = message.Reader.ReadUTF();
                            if (imageName.Contains("Skills_24_2") || imageName.Contains("Skills_24_3") ||
                                imageName.Contains("Skills_25_2") || imageName.Contains("Skills_25_3") ||
                                imageName.Contains("Skills_26_2") || imageName.Contains("Skills_26_3"))
                            {
                                if (character.InfoChar.NewSkill.TypePaint != int.Parse(imageName.Split("_")[2]))
                                {
                                    ServerUtils.WriteLog("RacKeoTrom.txt", character.Id + " Get " + imageName);
                                    break;
                                }
                            }
                            _session.SendMessage(Service.SendImgByName(_session.ZoomLevel, imageName));
                        break;
                    }
                    //RADAR
                    case 127:
                    {
                        var character = (Character) _session?.Player?.Character;
                        if (character == null) return;
                        var action = message.Reader.ReadByte();
                            Server.Gi().Logger.Debug($"Radar -----------<<<<<<<>>>>>>>>>----------action: {action}");
                        switch (action)
                        {
                            //danh sách
                            case 0:
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.Radar0(character.InfoChar.Cards.Values.ToList()));
                                break;
                            }
                            //Use card
                            case 1:
                            {
                                var id = message.Reader.ReadShort();
                                if (character.InfoChar.Cards.ContainsKey(id) && character.InfoChar.Cards[id] != null)
                                {
                                    if (character.InfoChar.Cards[id].Level == 0)
                                    {
                                                character.CharacterHandler.SendMessage(Service.ServerMessage("Thẻ cần đạt ít nhất Level 1 mới có thể kích hoạt"));
                                        break;
                                    }

                                    if (character.InfoChar.Cards[id].Used == 0)
                                    {
                                        character.InfoChar.Cards[id].Used = 1;

                                        var radarTemplate = Cache.Gi().RADAR_TEMPLATE.FirstOrDefault(r => r.Id == id);

                                        if (radarTemplate != null)
                                        {
                                            character.InfoChar.EffectAuraId = radarTemplate.AuraId;
                                                    character.CharacterHandler.SendMessage(Service.Radar4(character.Id, radarTemplate.AuraId));
                                        }
                                    }
                                    else
                                    {
                                        character.InfoChar.Cards[id].Used = 0;
                                        character.InfoChar.EffectAuraId = -1;
                                    }

                                            character.CharacterHandler.SendMessage(Service.Radar1(id,
                                        character.InfoChar.Cards[id].Used));
                                    character.CharacterHandler.SetUpInfo();
                                    character.CharacterHandler.SendMessage(Service.MeLoadPoint(character));
                                }

                                break;
                            }
                        }

                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error OnMessage in Controller.cs: {e.Message} {e.StackTrace}", e);
            }
            finally
            {
                message?.CleanUp();
            }

            return;
        }

        private void MessageSubCommand(Message message)
        {
            try
            {
                if (message.Reader.Available() <= 0) return;
                var command = message.Reader.ReadByte();
                Server.Gi().Logger.Debug($"Client: {_session.Id} - Message: -30 - Command: {command}");
                var characterLog = _session?.Player?.Character;
                if (characterLog != null)
                {
                    
                    if (DataCache.LogTheoDoi.Contains(characterLog.Id))
                    {
                        ServerUtils.WriteTraceLog( characterLog.Id + "_" + characterLog.Name, "MES: -30 - Command: " + command);
                    }
                }
                switch (command)
                {
                    case 64:
                        {
                            var id = message.Reader.ReadInt();
                            var select = message.Reader.ReadShort();
                            var character = _session?.Player?.Character;
                            if (character.Zone.ZoneHandler.GetCharacter(id) == null)
                            {
                                break;
                            }
                            var player = character.Zone.ZoneHandler.GetCharacter(id);
                            PlayerMenu.Action((Character)character, player, select);
                        }
                        break;
                    //Plus point
                    case 16:
                    {
                        var type = message.Reader.ReadByte();
                        var total = message.Reader.ReadShort();
                            Server.Gi().Logger.Debug($"-30_16 Plus point --------------- type: {type} - total: {total}");
                        var character = _session?.Player?.Character;
                        if (character != null)
                        {
                            LimitPower limitPower;
                            limitPower = Cache.Gi().LIMIT_POWERS[DataCache.MAX_LIMIT_POWER_LEVEL];
                                // (character.InfoChar.IsPower
                                //     ? Cache.gI().LIMIT_POWERS[character.InfoChar.LitmitPower]
                                //     : Cache.gI().LIMIT_POWERS[character.InfoChar.LitmitPower - 1]) ??
                                // Cache.gI().LIMIT_POWERS[DataCache.MAX_LIMIT_POWER_LEVEL-1];
                            var ppoint = character.InfoChar.Potential;
                            long minePoint = 0;
                            switch (type)
                            {
                                //Hp gốc
                                case 0:
                                {
                                    var hpOld = character.InfoChar.OriginalHp;
                                    switch (total)
                                    {
                                        case 1:
                                        {
                                            minePoint = hpOld + 1000;
                                            break;
                                        }
                                        case 10:
                                        {
                                            minePoint = 10 * (2 * (hpOld + 1000) + 180) / 2;
                                            break;
                                        }
                                        case 100:
                                        {
                                            minePoint = 100 * (2 * (hpOld + 1000) + 1980) / 2;
                                            break;
                                        }
                                        default:
                                        {
                                            _session.SendMessage(
                                                Service.DialogMessage(TextServer.gI().ERROR_VALUE_INPUT));
                                            return;
                                        }
                                    }

                                    if (ppoint < minePoint)
                                    {
                                        _session.SendMessage(Service.DialogMessage(TextServer.gI().NOT_ENOUGH_PPOINT));
                                        return;
                                    }

                                    var hpNew = hpOld + character.InfoChar.HpFrom1000 * total;
                                    if (hpNew > limitPower.Hp)
                                    {
                                        _session.SendMessage(Service.DialogMessage(TextServer.gI().MAX_POINT_POWER));
                                        return;
                                    }

                                    character.InfoChar.OriginalHp = hpNew;
                                    
                                    break;
                                }
                                //Mp gốc
                                case 1:
                                {
                                    var mpOld = character.InfoChar.OriginalMp;
                                    switch (total)
                                    {
                                        case 1:
                                        {
                                            minePoint = mpOld + 1000;
                                            break;
                                        }
                                        case 10:
                                        {
                                            minePoint = 10 * (2 * (mpOld + 1000) + 180) / 2;
                                            break;
                                        }
                                        case 100:
                                        {
                                            minePoint = 100 * (2 * (mpOld + 1000) + 1980) / 2;
                                            break;
                                        }
                                        default:
                                        {
                                            _session.SendMessage(
                                                Service.DialogMessage(TextServer.gI().ERROR_VALUE_INPUT));
                                            return;
                                        }
                                    }

                                    if (ppoint < minePoint)
                                    {
                                        _session.SendMessage(Service.DialogMessage(TextServer.gI().NOT_ENOUGH_PPOINT));
                                        return;
                                    }

                                    var mpNew = mpOld + character.InfoChar.MpFrom1000 * total;
                                    if (mpNew > limitPower.Ki)
                                    {
                                        _session.SendMessage(Service.DialogMessage(TextServer.gI().MAX_POINT_POWER));
                                        return;
                                    }

                                    character.InfoChar.OriginalMp = mpNew;
                                    break;
                                }
                                //Dam gốc
                                case 2:
                                {
                                    var damageOld = character.InfoChar.OriginalDamage;
                                    switch (total)
                                    {
                                        case 1:
                                        {
                                            minePoint = damageOld * 100;
                                            break;
                                        }
                                        case 10:
                                        {
                                            minePoint = 10 * (2 * damageOld + 9) / 2 * character.InfoChar.Exp;
                                            break;
                                        }
                                        case 100:
                                        {
                                            minePoint = 100 * (2 * damageOld + 99) / 2 * character.InfoChar.Exp;
                                            break;
                                        }
                                        default:
                                        {
                                            _session.SendMessage(
                                                Service.DialogMessage(TextServer.gI().ERROR_VALUE_INPUT));
                                            return;
                                        }
                                    }

                                    if (ppoint < minePoint)
                                    {
                                        _session.SendMessage(Service.DialogMessage(TextServer.gI().NOT_ENOUGH_PPOINT));
                                        return;
                                    }

                                    var damageNew = damageOld + character.InfoChar.DamageFrom1000 * total;
                                    if (damageNew > limitPower.Damage)
                                    {
                                        _session.SendMessage(Service.DialogMessage(TextServer.gI().MAX_POINT_POWER));
                                        return;
                                    }

                                    character.InfoChar.OriginalDamage = damageNew;
                                    break;
                                }
                                //Def gốc
                                case 3:
                                {
                                    var defOld = character.InfoChar.OriginalDefence;
                                            switch (total)
                                            {
                                                case 1:
                                                    minePoint = (long)(2 * (defOld + 5)) / 2L * 100000L;
                                                    break;
                                                case 10:
                                                    minePoint = 10L * (long)(2 * (defOld + 5) + 9) / 2L * 100000L;
                                                    break;
                                                case 100:
                                                    minePoint = 100L * (long)(2 * (defOld + 5) + 99) / 2L * 100000L;
                                                    break;
                                                default:
                                                    {
                                                        _session.SendMessage(
                                                            Service.DialogMessage(TextServer.gI().ERROR_VALUE_INPUT));
                                                        return;
                                                    }
                                            }                            
                                    if (ppoint < minePoint)
                                    {
                                        _session.SendMessage(Service.DialogMessage(TextServer.gI().NOT_ENOUGH_PPOINT));
                                        return;
                                    }

                                    if (defOld >= limitPower.Def)
                                    {
                                        _session.SendMessage(Service.DialogMessage(TextServer.gI().MAX_POINT_POWER));
                                        return;
                                    }

                                    character.InfoChar.OriginalDefence += total;
                                    break;
                                }
                                //Crit gốc
                                case 4:
                                {
                                    var critOld = character.InfoChar.OriginalCrit;
                                    if (critOld == 10)
                                    {
                                        _session.SendMessage(Service.DialogMessage(TextServer.gI().MAX_POINT));
                                        return;
                                    }

                                    minePoint = 50000000;
                                    for (var i = 0; i < critOld; i++)
                                    {
                                        minePoint *= 5;
                                    }

                                    if (ppoint < minePoint)
                                    {
                                        _session.SendMessage(Service.DialogMessage(TextServer.gI().NOT_ENOUGH_PPOINT));
                                        return;
                                    }

                                    if (critOld >= limitPower.Crit)
                                    {
                                        _session.SendMessage(Service.DialogMessage(TextServer.gI().MAX_POINT_POWER));
                                        return;
                                    }

                                    character.InfoChar.OriginalCrit += 1;
                                    break;
                                }
                            }

                            character.InfoChar.Potential -= minePoint;
                            character.InfoChar.TotalPotential += minePoint;
                            character.CharacterHandler.SetUpInfo();
                            _session.SendMessage(Service.MeLoadPoint(character));
                                var charRel = (Character)character;
                                if (TaskHandler.CheckTask(charRel, 3, 0)) TaskHandler.gI().PlusSubTask(charRel, 1);
                           
                         }
                            break;
                    }
                    //Menu player
                    case 63:
                    {
                        var IdPlayerInMap = message.Reader.ReadInt();
                        _session.SendMessage(Service.MenuPlayer(_session.Player.Character, IdPlayerInMap));
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error Message Subcommand in Controller.cs: {e.Message} {e.StackTrace}", e);
            }
            finally
            {
                message?.CleanUp();
            }
        }
        private Task Login(int id)
        {
            return Task.CompletedTask;
            //if (!_session.Player.CharId.Any(i => i == id) && id != 0)
            //{
            //    _session.SendMessage(Service.DialogMessage(TextServer.gI().INCORRECT_LOGIN));
            //    return Task.CompletedTask;
            //}
            
            //var timeServerSec = ServerUtils.CurrentTimeSecond();
            //var thoiGianDangNhap = UserDB.GetTimeOut(id);

            //if (thoiGianDangNhap > timeServerSec)
            //{
            //    var delay = (thoiGianDangNhap - timeServerSec);
            //    _session.SendMessage(Service.DialogMessage(string.Format("Bạn vừa thoát game, vui lòng đợi {0} giây nữa để vào lại game",
            //            delay)));
            //    return Task.CompletedTask; 
            //}
            //if (!DatabaseManager.ConfigManager.gI().IsPlayServer)
            //{
            //    _session.SendMessage(Service.DialogMessage("Vui lòng chọn máy chủ khác, máy chủ này chỉ dùng để tải dữ liệu\nNếu chọn máy chủ khác mà không vào được game\nvui lòng thoát game đăng nhập lại."));
            //    return Task.CompletedTask;
            //}
            //var player = _session.Player;
            //if (DatabaseManager.ConfigManager.gI().IsDevServer && player.Role == 0)
            //{
            //    _session.SendMessage(Service.DialogMessage("Máy chủ hiện đang bảo trì."));
            //    return Task.CompletedTask;
            //}


            //if (Maintenance.Gi().IsStart)
            //{
            //    _session.SendMessage(Service.DialogMessage("Máy chủ đang tiến hành bảo trì, không thể vào game ngay lúc này"));
            //    return Task.CompletedTask;
            //}

            //if (player.Ban >= 1)
            //{
            //    _session.SendMessage(Service.DialogMessage(TextServer.gI().USER_LOCK));
            //    UserDB.UpdateLogin(player.Id, 0);
            //    return Task.CompletedTask;
            //}
            //if (id is 0)
            //{
            //    _session.SendMessage(Service.LoadingCreateChar());
            //    return Task.CompletedTask;
            //}
            //var temp = ClientManager.Gi().GetPlayer(id);
            //if (temp != null)
            //{
            //    var character = temp.Character;
            //    temp.Session.SendMessage(Service.DialogMessage(TextServer.gI().DUPLICATE_LOGIN));
            //    ClientManager.Gi().KickSession(temp.Session);
            //    _session.SendMessage(Service.DialogMessage(TextServer.gI().DUPLICATE_LOGIN2));
            //    ClientManager.Gi().KickSession(_session);
            //    UserDB.UpdateLogin(temp.Session.Id, 0);
            //    return Task.CompletedTask;
            //}
            //player.CharacterChoose = _session.Player.CharactersChoose.FirstOrDefault(i => i != null && i.Id == id);
            //player.IsOnline = true;
            //UserDB.Update(player, _session.IpV4);
            //ClientManager.Gi().Add(player);
            //_session.SendMessage(Service.UpdateData());
            //return Task.CompletedTask;
        }
        private Task MessageNotLogin(Message message)
        {
            try
            {
                if (message.Reader.Available() <= 0) return Task.CompletedTask;
                var command = message.Reader.ReadByte();
                Server.Gi().Logger.Debug($"Client: {_session.Id} - Message: -29 - Command: {command}");
                
                switch (command)
                {
                    case 3://đăng nhập quản lí bằng manager
                        {
                            Server.Gi().Logger.Print("Hanasake Message to server by manager");
                            var msgAdmin = message.Reader.ReadUTF();
                            var username = message.Reader.ReadUTF();
                            var password = message.Reader.ReadUTF();
                            var player = UserDB.Login(username, password);
                            if (player != null && player.Role == 1)
                            {
                                if (msgAdmin is "servermanager")
                                {
                                    _session.SetupAdmin();
                                    Server.Gi().Logger.Print("Login to server by manager success: Player is admin");
                                    break;
                                }
                            }
                            else if (player == null)
                            {
                                Server.Gi().Logger.Print("Login to server by manager failed: Player is null");
                            }
                            else if (player.Role != 1)
                            {
                                Server.Gi().Logger.Print("Login to server by manager failed: Player not admin");

                            }
                        }   
                        

                        break;
                    case 1://đăng ký tài khoản từ tài khoản ảo
                        {
                            var username = message.Reader.ReadUTF();
                            var password = message.Reader.ReadUTF();
                            var usernameAo = message.Reader.ReadUTF();
                            var passwordAo = "a";//default
                            if (!UserDB.ExistUsername(username))
                            {
                                UserDB.UpdateUser(usernameAo, passwordAo, username, password);
                                _session.SendMessage(Service.DialogMessage($"Chúc mừng bạn đã kích hoạt tài khoản ảo thành công\nUsername: {username}\nPassword: {password}\nChúc bạn chơi game vui vẻ\nrobuen0.online"));
                            }
                            else
                            {

                            }
                        }
                        break;
                    //Login game
                    case 0:
                        {
                           

                            var username = message.Reader.ReadUTF().Replace("['\"\\\\]", "\\\\$0");
                            var password = message.Reader.ReadUTF().Replace("['\"\\\\]", "\\\\$0");
                            var c_version = message.Reader.ReadUTF();
                            var c_type = message.Reader.ReadByte();
                            switch (c_type)
                            {
                                case 0://đăng nhập
                                    {
                                        var timeServerSec = ServerUtils.CurrentTimeSecond();
                                        var thoiGianDangNhap = UserDB.GetTimeOut(username);

                                        if (thoiGianDangNhap > timeServerSec)
                                        {
                                            var delay = (thoiGianDangNhap - timeServerSec);
                                            _session.SendMessage(Service.DialogMessage(string.Format("Bạn vừa thoát game, vui lòng đợi {0} giây nữa để vào lại game",
                                                    delay)));
                                            return Task.CompletedTask;
                                        }

                                        if (_session.LoginGame(username, password, c_version, c_type, message))
                                        {
                                            if (_session.Player.Ban >= 1)
                                            {
                                                _session.SendMessage(Service.DialogMessage(TextServer.gI().USER_LOCK));
                                                UserDB.UpdateLogin(_session.Player.Id, 0);
                                                return Task.CompletedTask;
                                            }
                                            var temp = ClientManager.Gi().GetPlayer(_session.Player.Id);
                                            if (temp != null)
                                            {
                                                var character = temp.Character;
                                                temp.Session.SendMessage(Service.DialogMessage(TextServer.gI().DUPLICATE_LOGIN));
                                                ClientManager.Gi().KickSession(temp.Session);
                                                _session.SendMessage(Service.DialogMessage(TextServer.gI().DUPLICATE_LOGIN2));
                                                ClientManager.Gi().KickSession(_session);
                                                UserDB.UpdateLogin(temp.Session.Id, 0);
                                                return Task.CompletedTask;
                                            }
                                            _session.Player.IsOnline = true;
                                            UserDB.Update(_session.Player, _session.IpV4);
                                            ClientManager.Gi().Add(_session.Player);
                                            _session.SendMessage(Service.SendNewImage(_session.ZoomLevel));
                                            _session.SendMessage(Service.SendNewBackground());
                                            _session.SendMessage(Service.SendVersionMessage());
                                            _session.SendMessage(Service.SendItemBackgrounds());
                                            _session.SendMessage(Service.SendTileSet());
                                            _session.SendMessage(Service.UpdateData());
                                        }
                                        else
                                        {
                                            _session.SendMessage(Service.DialogMessage(TextServer.gI().INCORRECT_LOGIN));
                                        }
                                    }
                                    break;
                                case 1://chơi tiếp (khi đã có tài khoản từ msg -101)
                                    {

                                        var timeServerSec = ServerUtils.CurrentTimeSecond();
                                        var thoiGianDangNhap = UserDB.GetTimeOut(username);

                                        if (thoiGianDangNhap > timeServerSec)
                                        {
                                            var delay = (thoiGianDangNhap - timeServerSec);
                                            _session.SendMessage(Service.DialogMessage(string.Format("Bạn vừa thoát game, vui lòng đợi {0} giây nữa để vào lại game",
                                                    delay)));
                                            return Task.CompletedTask;
                                        }
                                        if (_session.LoginGame(username, "a", c_version, c_type, message))
                                        {
                                            if (_session.Player.Ban >= 1)
                                            {
                                                _session.SendMessage(Service.DialogMessage(TextServer.gI().USER_LOCK));
                                                UserDB.UpdateLogin(_session.Player.Id, 0);
                                                return Task.CompletedTask;
                                            }
                                            var temp = ClientManager.Gi().GetPlayer(_session.Player.Id);
                                            if (temp != null)
                                            {
                                                var character = temp.Character;
                                                temp.Session.SendMessage(Service.DialogMessage(TextServer.gI().DUPLICATE_LOGIN));
                                                ClientManager.Gi().KickSession(temp.Session);
                                                _session.SendMessage(Service.DialogMessage(TextServer.gI().DUPLICATE_LOGIN2));
                                                ClientManager.Gi().KickSession(_session);
                                                UserDB.UpdateLogin(temp.Session.Id, 0);
                                                return Task.CompletedTask;
                                            }
                                            _session.Player.IsOnline = true;
                                            UserDB.Update(_session.Player, _session.IpV4);
                                            ClientManager.Gi().Add(_session.Player);
                                            _session.SendMessage(Service.SendNewImage(_session.ZoomLevel));
                                            _session.SendMessage(Service.SendNewBackground());
                                            _session.SendMessage(Service.SendVersionMessage());
                                            _session.SendMessage(Service.SendItemBackgrounds());
                                            _session.SendMessage(Service.SendTileSet());
                                            _session.SendMessage(Service.UpdateData());
                                        }
                                        else
                                        {
                                            _session.SendMessage(Service.DialogMessage(TextServer.gI().ERROR_ADMIN));
                                        }

                                    }
                                    break;
                                    

                            }
                            break;
                           

                        }
                    //Set connect
                    case 2:
                        {
                            _session.SetConnect(message);
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error Message Not Login in Controller.cs: {e.Message}\n{e.StackTrace}", e);
            }
            finally
            {
                message?.CleanUp();
            }
            return Task.CompletedTask;
        }


        private void MessageNotMap(Message message)
        {
            try
            {
                if (message.Reader.Available() <= 0) return;
                var command = message.Reader.ReadByte();
                Server.Gi().Logger.Debug($"Client: {_session.Id} - Message: -28 - Command: {command}");
                var characterLog = _session?.Player?.Character;
                if (characterLog != null)
                {
                    
                    if (DataCache.LogTheoDoi.Contains(characterLog.Id))
                    {
                        ServerUtils.WriteTraceLog( characterLog.Id + "_" + characterLog.Name, "MES: -28 - Command: " + command);
                    }
                }
                switch (command)
                {
                    //-28_2: Create new Character
                    case 2:
                        {
                            if (_session.Player == null) return;
                            //Get data
                            var name = message.Reader.ReadUTF().ToLower().Trim();
                            var gender = message.Reader.ReadByte();
                            var hair = message.Reader.ReadByte();
                            //Check name
                            if (!Regex.IsMatch(name, "^[a-zA-Z0-9]+$") || name.Length is < 5 or > 15)
                            {
                                _session.SendMessage(Service.DialogMessage(TextServer.gI().INCORRECT_NAME));
                                return;
                            }

                            if (CharacterDB.IsAlreadyExist(name))
                            {
                                _session.SendMessage(Service.DialogMessage(TextServer.gI().DUPLICATE_CHAR));
                                return;
                            }

                            if (gender is < 0 or > 2 || !DataCache.DefaultHair.Contains(hair))
                            {
                                gender = 0;
                                hair = 64;
                            }

                            var character = new Character(_session.Player)
                            {
                                Name = name,
                            };
                            //  character.Player.Session.IsNewVersion
                            character.InfoChar.Gender = character.InfoChar.NClass = gender;
                            character.InfoChar.Hair = hair;
                            character.InfoChar.Bag = -1;
                            character.InfoChar.IsNewMember = true;

                            character.CharacterHandler.AddItemToBody(ItemCache.GetItemDefault(gender), 0);
                            character.CharacterHandler.AddItemToBody(ItemCache.GetItemDefault((short)(gender + 6)), 1);
                            character.CharacterHandler.AddItemToBox(false, ItemCache.GetItemDefault(12));
                            GiftNewGame(character);
                            character.Skills.Add(new SkillCharacter(gender * 2, gender * 14));
                            character.BoughtSkill.Add(gender != 0 ? gender != 1 ? 87 : 79 : 66);
                            character.InfoChar.OSkill = new List<sbyte>() { (sbyte)(gender * 2), -1, -1, -1, -1, -1, -1, -1, -1, -1 };
                            character.SpecialSkill = new SpecialSkill()
                            {
                                Id = -1,
                                Info = "Chưa có Nội Tại\nBấm vào để xem chi tiết",
                                SkillId = -1,
                                Value = 0,
                                Img = 5223,
                            };
                            var idCharCreate = CharacterDB.Create(character);
                            if (idCharCreate != 0)
                            {
                                try
                                {
                                    character.Id = idCharCreate;
                                    character.Me = new InfoFriend(character);
                                    var tempSieuHang = new SuperChampion_Championer();
                                    tempSieuHang.Top = SuperChampion_Manager.Entrys.Count + 1;
                                    tempSieuHang.UpdateData(character.Me, true);
                                    character.DataSieuHang = tempSieuHang;
                                    SuperChampion_Manager.Entrys.TryAdd(tempSieuHang.Top, tempSieuHang);
                                    var magicTree = new MagicTree(idCharCreate, gender)
                                    {
                                        Id = idCharCreate
                                    };
                                    MagicTreeDB.Create(magicTree);
                                    MagicTreeManager.Add(magicTree);
                                    _session.Player.Character = character;
                                    _session.Player.CharId = idCharCreate;
                                    ClientManager.Gi().Add(_session.Player.Character);
                                    if (UserDB.Update(_session.Player, _session.IpV4, isCreateChar: true))
                                    {
                                        _session.Player.Character.CharacterHandler.PlusHp(50);
                                        _session.Player.Character.CharacterHandler.SendInfo();

                                        CharacterDB.Update((Character)_session.Player.Character);
                                        switch (gender)
                                        {
                                            case 2:
                                                character.InfoChar.X = 142;
                                                character.InfoChar.Y = 384;
                                                break;
                                            case 0 or 1:
                                                character.InfoChar.X = 159;
                                                character.InfoChar.Y = 384;
                                                break;
                                        }
                                        MapManager.GetMapOffline(39 + gender).JoinZone(character, character.Id);
                                        _session.SendMessage(Service.OpenUiSay(5,
                                        string.Format(Cache.Gi().GAME_INFO_TEMPLATES[4].Content, name, TextTask.NameMob[gender]), false,
                                        gender));
                                    }
                                    else
                                    {
                                        _session.SendMessage(Service.DialogMessage(TextServer.gI().ERROR_CREATE_NEW_CHAR));
                                    }
                                }
                                catch (Exception e)
                                {
                                    CharacterDB.Delete(idCharCreate);
                                    Server.Gi().Logger.Error($"Error Create New Char in Controller.cs: {e.Message} \n {e.StackTrace}", e);
                                }
                            }
                            else
                            {
                                _session.SendMessage(Service.DialogMessage(TextServer.gI().ERROR_CREATE_NEW_CHAR));
                            }

                            break;
                        }
                    //Update map    
                    case 6:
                    {
                        _session.SendMessage(Service.UpdateMap());
                        break;
                    }
                    //Update skill    
                    case 7:
                    {
                        _session.SendMessage(Service.UpdateSkill());
                        break;
                    }
                    //Update item    
                    case 8:
                    {
                        _session.SendMessage(Service.UpdateItem(0));
                        _session.SendMessage(Service.UpdateItem(1));
                        _session.SendMessage(Service.UpdateItem(2));
                            _session.SendMessage(Service.UpdateItem(100));
                            break;
                    }
                    //-28_10
                    //Request Map Template
                    case 10:
                    {
                        int id = message.Reader.ReadByte();
                        var character = _session?.Player?.Character;
                        if (character != null)
                        {
                            if (id < 0)
                            {
                                id += 256;
                            }

                            var zone = character.Zone;
                            if (zone == null) return;
                            var tileMap = Cache.Gi().TILE_MAPS.FirstOrDefault(x => x.Id == id);
                            _session.SendMessage(Service.RequestMapTemplate(tileMap, zone, character));
                        }

                        break;
                    }
                    //Client OK
                    case 13:
                    {
                        ClientOk();
                        break;
                    }
                    case 16:
                    {
                        var character = _session?.Player?.Character;
                        if (character == null) return;
                        InputClient.HandleNapThe((Character) character, message);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error Message Not Map in Controller.cs: {e.Message}: \n{e.StackTrace}", e);
            }
            finally
            {
                message?.CleanUp();
            }
        }

        private void ClientOk()
        {
            var player = _session?.Player;
            if (player == null) return;
            var character = _session?.Player?.Character;
            if (character == null)
            {
                LoadCharacter();
            }

        }

        private void GiftNewGame(Character character)
        {
           // character.PlusGold(5000000000);
            //character.PlusDiamond(100000);
            //haracter.CharacterHandler.AddItemToBag(false, ItemCache.GetItemDefault(1343, 20));
           // var thoivang = ItemCache.GetItemDefault(457, 50);
            //thoivang.Options.Add(new OptionItem()
         //   {
         //       Id = 30,
          //      Param = 0,
         //   });
         //   character.CharacterHandler.AddItemToBag(true, thoivang);
        //    _session.SendMessage(Service.MeLoadInfo(character));                        
            character.SetUpTrungMaBuPosition();
        }

        private void LoadCharacter()
        {
            var player = _session?.Player;
            if (player == null) return;
            if (player.CharId == 0)
            {
                _session.SendMessage(Service.LoadingCreateChar());
            }
            else
            {
                var character = CharacterDB.GetById(player.CharId);
                character.CharacterHandler.SetUpFriend();
                character.SetUpTrungMaBuPosition();
                character.Player = _session.Player;
                _session.Player.Character = character;
                ClientManager.Gi().Add(_session.Player.Character);

                _session.Player.Character.CharacterHandler.SendInfo();
                if (character.InfoChar.IsDie || character.InfoChar.Hp <= 0)
                {
                    character.InfoChar.IsDie = false;
                    character.InfoChar.Hp = 1;
                    JoinMapKarin(21 + character.InfoChar.Gender);
                    character.CharacterHandler.SendMessage(Service.PlayerLevel(character));
                }
                else if (character.InfoChar.MapId - 39 == character.InfoChar.Gender)
                {
                    MapManager.GetMapOffline(character.InfoChar.MapId).JoinZone(character, character.Id, false, false, 0);
                    _session.SendMessage(Service.OpenUiSay(5,
                        string.Format(Cache.Gi().GAME_INFO_TEMPLATES[5].Content, character.Name,
                            TextTask.NameMob[character.InfoChar.Gender])));
                }
                else
                {
                    switch (character.InfoChar.MapId)
                    {
                        case 155:
                            {
                                if (!character.InfoSet.IsFullSetHuyDiet)
                                {
                                    MapManager.GetMapOffline(21 + character.InfoChar.Gender).JoinZone(character, character.Id, false, false, 0);

                                }
                                else
                                {
                                    var map2 = MapManager.Get(character.InfoChar.MapId);
                                    var zone2 = map2.GetZoneNotMaxPlayer();
                                    if (zone2 != null)
                                    {
                                        zone2.ZoneHandler.JoinZone(character, false, false, 0);
                                        var disciple = character.Disciple;
                                        if (disciple != null && disciple.Zone == null)
                                        {
                                            disciple.Zone = character.Zone;
                                            disciple.Status = 0;
                                        }

                                    }
                                }
                                break;
                            }
                        case 160 or 161 or 162 or 163:
                            {
                                if (character.CharacterHandler.GetItemBagById(992) == null)
                                {
                                    MapManager.GetMapOffline(21 + character.InfoChar.Gender).JoinZone(character, character.Id, false, false, 0);

                                }
                                else
                                {
                                    var map2 = MapManager.Get(character.InfoChar.MapId);
                                    var zone2 = map2.GetZoneNotMaxPlayer();
                                    if (zone2 != null)
                                    {
                                        zone2.ZoneHandler.JoinZone(character, false, false, 0);
                                        var disciple = character.Disciple;
                                        if (disciple != null && disciple.Zone == null)
                                        {
                                            disciple.Zone = character.Zone;
                                            disciple.Status = 0;
                                        }

                                    }
                                }
                            }
                            break;
                        case 153:
                            ClanManager.Get(character.ClanId).ClanZone.Join(character, character.InfoChar.MapId, true);
                            break;
                        case 164:
                            MapManager.GetMapOffline(character.InfoChar.MapId).JoinZone(character, character.Id, false, false, 0);
                            break;
                        case int i when DataCache.IdMapKarin.Contains(i):
                            MapManager.GetMapOffline(character.InfoChar.MapId).JoinZone(character, character.Id, false, false, 0);
                            break;
                        case int i when DataCache.IdMapSpecial.Contains(i):
                            MapManager.GetMapOffline(21 + character.InfoChar.Gender).JoinZone(character, character.Id, false, false, 0);
                            break;
                        default:
                            var map = MapManager.Get(character.InfoChar.MapId);
                            var zone = map.GetZoneNotMaxPlayer();
                            if (zone != null)
                            {
                                zone.ZoneHandler.JoinZone(character, false, false, 0);
                                var disciple = character.Disciple;
                                if (disciple != null && disciple.Zone == null)
                                {
                                    disciple.Zone = character.Zone;
                                    disciple.Status = 0;
                                }

                            }
                            else
                            {
                                MapManager.GetMapOffline(character.InfoChar.MapId).JoinZone(character, character.Id, false, false, 0);
                            }
                            break;
                    }
                }
                /*_session.SendMessage(Service.OpenUiSay(5,
                          string.Format(Cache.Gi().GAME_INFO_TEMPLATES[0].Content, character.Name), false,
                          character.InfoChar.Gender));

                ClientManager.Gi().SendMessage(Service.ServerMessageVip($"Bá chủ bát hoang {character.Name} đã xuất thế, chiếu cáo thiên hạ, yêu ma run sợ"));*/
            }
        }
        

        private void NextMap()
        {
            try
            {
                var character = _session?.Player?.Character;
                var @char = (Character)character;
                if (character == null) return;
                var mapOld = MapManager.Get(character.InfoChar.MapId);
                var wayPoint = mapOld?.TileMap.WayPoints
                    .FirstOrDefault(waypoint =>
                        CheckTrueWaypoint(character, waypoint));
                if (wayPoint == null) return;
                var task = character.InfoTask;
                IMap mapNext;




                mapNext = MapManager.Get(wayPoint.MapNextId);



                if (mapNext == null) return;
                if (DataCache.IdMapCold.Contains(mapNext.Id) && character.InfoTask.Id < 30)
                {
                    character.CharacterHandler.SendMessage(Service.OpenUiSay(5, "Bạn phải làm [nhiệm vụ Cuộc dạo chơi của Xên] để qua khu vực này", false, character.InfoChar.Gender));
mapOld.OutZone(character);
                    character.CharacterHandler.SetUpPosition(mapNext.Id, mapOld.Id);
                    mapOld.JoinZone((Character)character, character.InfoChar.ZoneId);
                    return;
                }
                if (@char.DataNgocRongNamek.AlreadyPick(@char) && @char.DataNgocRongNamek.DelayAction > ServerUtils.CurrentTimeMillis())
                {
                    character.CharacterHandler.SendMessage(Service.OpenUiSay(5, "Chưa thể đổi map lúc này !", false, character.InfoChar.Gender));
mapOld.OutZone(character);
                    character.CharacterHandler.SetUpPosition(mapNext.Id, mapOld.Id);
                    mapOld.JoinZone((Character)character, character.InfoChar.ZoneId);
                    return;
                }


                switch (task.Id, mapNext.Id)
                {
                    case (1, 44 or 15 or 43 or 8 or 42 or 1):
                        mapOld.OutZone(character);
                        character.CharacterHandler.SetUpPosition(mapNext.Id, mapOld.Id);
                        mapOld.JoinZone((Character)character, character.InfoChar.ZoneId);
                        character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn chưa thể đến khu vực này"));
                        return;

                    case ( < 9, 5 or 13 or 20):
                        mapOld.OutZone(character);
                        character.CharacterHandler.SetUpPosition(mapNext.Id, mapOld.Id);
                        mapOld.JoinZone((Character)character, character.InfoChar.ZoneId);
                        character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn phải làm nhiệm vụ để qua khu vực này"));
                        return;
                    case ( < 24, 80):
                        mapOld.OutZone(character);
                        character.CharacterHandler.SetUpPosition(mapNext.Id, mapOld.Id);
                        mapOld.JoinZone((Character)character, character.InfoChar.ZoneId);
                        character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn phải làm [nhiệm vụ chạm trái đệ tử của Fide] để qua khu vực này"));
                        return;
                    case ( < 22, 65):
                        return;
                }
                IZone zoneNext;
                switch (mapNext.Id)
                {

                    //case 0 or 14 or 7:
                    //    switch (mapOld.Id)
                    //    {

                    //        case 21 or 22 or 23:
                    //            character.MapPrivate.ExitMap(mapOld.Id, mapNext.Id);
                    //            break;
                    //        default:
                    //            var zoneNext3 = mapNext.GetZoneNotMaxPlayer();

                    //            if (zoneNext3 != null)
                    //            {
                    //                mapOld.OutZone(character, mapNext.Id);
                    //                character.CharacterHandler.SetUpPosition(mapOld.Id, mapNext.Id);
                    //                mapNext.JoinZone((Character)character, zoneNext3.Id);

                    //            }
                    //            else
                    //            {
                    //                mapOld.OutZone(character, mapOld.Id);
                    //                character.CharacterHandler.SetUpPosition(mapNext.Id, mapOld.Id);
                    //                mapOld.JoinZone((Character)character, character.InfoChar.ZoneId);
                    //                _session.SendMessage(Service.OpenUiSay(5, TextServer.gI().MAX_NUMCHARS, false,
                    //                    character.InfoChar.Gender));
                    //            }
                    //            break;
                    //    }
                    //    break;
                    case 24 or 25 or 26:
                        if (@char.DataNgocRongNamek.AlreadyPick(@char))
                        {
                            var itm = new ItemMap(-1, ItemCache.GetItemDefault((short)(@char.DataNgocRongNamek.IdNamekBall)));
                            itm.X = @char.InfoChar.X;
                            itm.Y = @char.InfoChar.Y;
                            @char.Zone.ZoneHandler.LeaveItemMap(itm);
                            @char.InfoChar.TypePk = 0;
                            @char.CharacterHandler.SendZoneMessage(Service.ChangeTypePk(@char.Id, 0));
                            @char.DataNgocRongNamek.IdNamekBall = -1;
                            @char.InfoChar.Bag = ClanManager.Get(@char.ClanId) != null ? (sbyte)ClanManager.Get(@char.ClanId).ImgId : (sbyte)-1;
                            @char.CharacterHandler.UpdatePhukien();
                        }
                       zoneNext = mapNext.GetZoneNotMaxPlayer();

                        if (zoneNext != null)
                        {
mapOld.OutZone(character);
                            character.CharacterHandler.SetUpPosition(mapOld.Id, mapNext.Id);
                            mapNext.JoinZone((Character)character, zoneNext.Id);

                        }
                        else
                        {
        mapOld.OutZone(character);
                            character.CharacterHandler.SetUpPosition(mapNext.Id, mapOld.Id);
                            mapOld.JoinZone((Character)character, character.InfoChar.ZoneId);
                            _session.SendMessage(Service.OpenUiSay(5, TextServer.gI().MAX_NUMCHARS, false,
                                character.InfoChar.Gender));
                        }
                        break;
                    case 1:
                        switch (mapOld.Id)
                        {
                            case 47:
                                if (@char.StatusCDRD is Character.StatusConDuongRanDoc.TALK_MEO_KARIN)
                                {
                                    var mapCDRD = MapManager.Get(144);
                                    mapCDRD.JoinZone(@char, character.ClanId); 
                                    return;
                                }
                                zoneNext = mapNext.GetZoneNotMaxPlayer();
                                if (zoneNext != null)
                                {
mapOld.OutZone(character);
                                    character.CharacterHandler.SetUpPosition(mapOld.Id, mapNext.Id);
                                    mapNext.JoinZone((Character)character, zoneNext.Id);

                                }
                                else
                                {
mapOld.OutZone(character);
                                    character.CharacterHandler.SetUpPosition(mapNext.Id, mapOld.Id);
                                    mapOld.JoinZone((Character)character, character.InfoChar.ZoneId);
                                    _session.SendMessage(Service.OpenUiSay(5, TextServer.gI().MAX_NUMCHARS, false,
                                        character.InfoChar.Gender));
                                }
                                break;
                            default:
                                zoneNext = mapNext.GetZoneNotMaxPlayer();

                                if (zoneNext != null)
                                {
mapOld.OutZone(character);
                                    character.CharacterHandler.SetUpPosition(mapOld.Id, mapNext.Id);
                                    mapNext.JoinZone((Character)character, zoneNext.Id);

                                }
                                else
                                {
mapOld.OutZone(character);
                                    character.CharacterHandler.SetUpPosition(mapNext.Id, mapOld.Id);
                                    mapOld.JoinZone((Character)character, character.InfoChar.ZoneId);
                                    _session.SendMessage(Service.OpenUiSay(5, TextServer.gI().MAX_NUMCHARS, false,
                                        character.InfoChar.Gender));
                                }
                                break;
                        }
                        break;
                    case int i when DataCache.IdMapKarin.Contains(i):
                        {
                            var mapOffline = MapManager.Get(i);
                            mapOffline.JoinZone(@char, character.Id);
                            break;
                        }
                    case int i when DataCache.IdMapReddot.Contains(i):
                        {
                            var mapRedRibbon = MapManager.Get(i);
                            mapRedRibbon.JoinZone(@char, character.ClanId) ;
                            break;
                        }
                    case int i when DataCache.IdMapBDKB.Contains(i):
                        {
                            var mapTreasure = MapManager.Get(i);
                            mapTreasure.JoinZone(@char, character.ClanId);
                            break;
                        }
                    case int i when DataCache.IdMapCDRD.Contains(i):
                        {
                            var mapCDRD = MapManager.Get(i);
                            mapCDRD.JoinZone(@char, character.ClanId);
                            break;
                        }
                    case int i when DataCache.IdMapGas.Contains(i):
                        {
                            var clan = ClanManager.Get(character.ClanId);
                            clan.ClanDungeon.KhiGasHuyDiet.JoinMap(@char, mapNext.Id);

                            break;
                        }
                    
                    default:
                        {
                            zoneNext = mapNext.GetZoneNotMaxPlayer();
                            if (zoneNext != null)
                            {
mapOld.OutZone(character);
                                character.CharacterHandler.SetUpPosition(mapOld.Id, mapNext.Id);
                                mapNext.JoinZone((Character)character, zoneNext.Id);

                            }
                            else
                            {
            mapOld.OutZone(character);
                                character.CharacterHandler.SetUpPosition(mapNext.Id, mapOld.Id);
                                mapOld.JoinZone((Character)character, character.InfoChar.ZoneId);
                                _session.SendMessage(Service.OpenUiSay(5, TextServer.gI().MAX_NUMCHARS, false,
                                    character.InfoChar.Gender));
                            }
                            break;
                        }
                 
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Print(e.Message + "\n" + e.StackTrace);
            }
        }

        private bool CheckTrueWaypoint(ICharacter character, WayPoint waypoint, int size = 0)
        {
            if (waypoint.IsEnter)
            {
                return character.InfoChar.X >= waypoint.MinX - size && character.InfoChar.X <= waypoint.MaxX + size &&
                       character.InfoChar.Y <= waypoint.MaxY && character.InfoChar.Y >= waypoint.MinY;
            }

            if (waypoint.MinX == 0)
            {
                return character.InfoChar.X <= waypoint.MaxX + 100 + size && character.InfoChar.Y <= waypoint.MaxY &&
                       character.InfoChar.Y >= waypoint.MinY;
            }

            return character.InfoChar.X >= waypoint.MinX - size && character.InfoChar.Y <= waypoint.MaxY &&
                   character.InfoChar.Y >= waypoint.MinY;
        }

       
        
        public void JoinMapKarin(int mapId, bool isDefaul = false, bool isTeleport = false, int typeTeleport = 0)
        {
            var charRel = (Character)_session.Player.Character;
            MapManager.GetMapOffline(mapId).JoinZone(charRel, charRel.Id, isDefaul, isTeleport, typeTeleport);
        }
        
      
        private static void Combinne(Message message, Character character)
        {
            var action = message.Reader.ReadByte();
            Server.Gi().Logger.Debug($"Combinne -81 ---------------------- action: {action}");
            switch (action)
            {
                case 1:
                {
                    var arrIndexUi = new List<int>();
                    var length = message.Reader.ReadByte();
                    for (var i = 0; i < length; i++)
                    {
                        arrIndexUi.Add(message.Reader.ReadByte());
                    }

                    switch (character.ShopId)
                        {
                            case 30:// giải pháp sư
                                {
                                    if (arrIndexUi.Count != 2)
                                    {

                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR_2));
                                    }
                                    var indexTrangBi = -1;
                                    var indexDaGiaiPhapSu = -1;
                                    arrIndexUi.ForEach(index =>
                                    {
                                        var itemIndex = character.CharacterHandler.GetItemBagByIndex(index);
                                        var itemTemplate = ItemCache.ItemTemplate(itemIndex.Id);
                                        if (itemTemplate.isItemCombine())
                                        {
                                            indexTrangBi = itemIndex.IndexUI;
                                        }
                                        else if (itemIndex.Id == 1320)
                                        {
                                            indexDaGiaiPhapSu = itemIndex.IndexUI;
                                        }

                                    });
                                    if (indexTrangBi == -1 || indexDaGiaiPhapSu == -1)
                                    {

                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage("Cần trang bị cần giải pháp sư và 1 đá giải pháp sư"));
                                        break;
                                    }
                                    var itemIndex = character.CharacterHandler.GetItemBagByIndex(indexTrangBi);
                                    var itemTemplate = ItemCache.ItemTemplate(itemIndex.Id);
                                    var text = $"{ServerUtils.ColorNotSpace("red")}Giải pháp sư Trang bị\n";
                                    text += $"{ServerUtils.ColorNotSpace("brown")}Trang bị được Giải pháp sư '{itemTemplate.Name}'\n";
                                    text += $"{ServerUtils.ColorNotSpace("green")}Sau khi Giải pháp sư: Về trang bị thường\n";
                                    text += $"{ServerUtils.ColorNotSpace("blue")}Tỉ lệ thành công 100%\n";
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(21, text, new List<string> { "Đồng ý", "Từ chối" }, character.InfoChar.Gender));
                                    character.TypeMenu = 45;
                                    character.CombinneIndex = new List<int>() { indexTrangBi, indexDaGiaiPhapSu };
                                }
                                break;
                            case 29:// pháp sư hóa
                                {
                                    if (arrIndexUi.Count != 2)
                                    {

                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR_2));
                                    }
                                    var indexTrangBi = -1;
                                    var indexDaPhapSu = -1;
                                    arrIndexUi.ForEach(index =>
                                    {
                                        var itemIndex = character.CharacterHandler.GetItemBagByIndex(index);
                                        var itemTemplate = ItemCache.ItemTemplate(itemIndex.Id);
                                        if (itemTemplate.isItemCombine())
                                        {
                                            indexTrangBi = itemIndex.IndexUI;
                                        }
                                        else if (itemIndex.Id == 1320)
                                        {
                                            indexDaPhapSu = itemIndex.IndexUI;
                                        }

                                    });
                                    if (indexTrangBi == -1 || indexDaPhapSu == -1)
                                    {

                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage("Cần trang bị để hóa pháp sư và 1 đá pháp sư"));
                                        break;
                                    }
                                    var itemIndex = character.CharacterHandler.GetItemBagByIndex(indexTrangBi);
                                    var itemTemplate = ItemCache.ItemTemplate(itemIndex.Id);
                                    var text = $"{ServerUtils.ColorNotSpace("red")}Pháp sư hóa Trang bị\n";
                                    text += $"{ServerUtils.ColorNotSpace("brown")}Trang bị được Pháp sư hóa '{itemTemplate.Name}'\n";
                                    text += $"{ServerUtils.ColorNotSpace("green")}Sau khi Giải pháp sư: +3% chỉ số pháp sư (Hiện tại đã hóa {itemIndex.GetParamOption(231)}/10)\n";
                                    text += $"{ServerUtils.ColorNotSpace("blue")}Tỉ lệ thành công 100%\n";
                                    bool enoughGold = CombineHandler.NeedGood(character, 100000000);
                                    text += enoughGold ? $"{ServerUtils.Color("blue")}Cần {ServerUtils.GetMoneys(100000000)} vàng" : $"{ServerUtils.Color("red")}Cần {ServerUtils.GetMoneys(100000000)} vàng";
                                    if (!enoughGold)
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(21, text, new List<string> { "Đóng" }, character.InfoChar.Gender));
                                        character.TypeMenu = 27;
                                        break;
                                    }
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(21, text, new List<string> { "Nâng cấp",  "Từ chối"}, character.InfoChar.Gender));
                                    character.TypeMenu = 44;
                                    character.CombinneIndex = new List<int>() { indexTrangBi, indexDaPhapSu };
                                }
                                break;
                            case 28:// nâng sách 2
                                {
                                    if (arrIndexUi.Count != 2)
                                    {

                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR_2));
                                    }
                                    var indexSach = -1;
                                    var indexKimBamGiay = -1;
                                    arrIndexUi.ForEach(index =>
                                    {
                                        var itemIndex = character.CharacterHandler.GetItemBagByIndex(index);
                                        if (DataCache.ListSach1.Contains(itemIndex.Id))
                                        {
                                            indexSach = itemIndex.IndexUI;
                                        }else if (itemIndex.Id == 1320)
                                        {
                                            indexKimBamGiay = itemIndex.IndexUI;
                                        }

                                    });
                                    if (indexSach == -1 || indexKimBamGiay == -1)
                                    {

                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage("Cần Sách Tuyệt Kỹ 1 và 10 kìm bấm giấy"));
                                        break;
                                    }
                                    var itemIndex = character.CharacterHandler.GetItemBagByIndex(indexSach);
                                    var KimBamGiay = character.CharacterHandler.GetItemBagByIndex(indexKimBamGiay);
                                    if (KimBamGiay.Quantity < 10)
                                    {
                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage("Cần Sách Tuyệt Kỹ 1 và 10 kìm bấm giấy"));
                                        break;
                                    }
                                    var text = $"{ServerUtils.ColorNotSpace("blue")}Nâng cấp sách tuyệt kỹ\n";
                                    text += $"{ServerUtils.ColorNotSpace("blue")}Cần 10 kìm bấm giấy\n";
                                    text += $"{ServerUtils.ColorNotSpace("blue")}Tỉ lệ thành công 10%\n";
                                    text += $"{ServerUtils.ColorNotSpace("blue")}Nâng cấp thất bại sẽ mất 10 kìm bấm giấy";
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(21, text, new List<string> { "Đồng ý", "Từ chối" }, character.InfoChar.Gender));
                                    character.TypeMenu = 42;
                                    character.CombinneIndex = new List<int>() { indexSach, indexKimBamGiay };
                                }
                                break;
                            case 27:// tẩy sách
                                {
                                    if (arrIndexUi.Count != 1)
                                    {

                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR_2));
                                    }
                                    var indexSach = -1;
                                    arrIndexUi.ForEach(index =>
                                    {
                                        var itemIndex = character.CharacterHandler.GetItemBagByIndex(index);
                                        if (DataCache.ListSach1.Contains(itemIndex.Id) || DataCache.ListSach2.Contains(itemIndex.Id))
                                        {
                                            indexSach = itemIndex.IndexUI;
                                        }
                                       
                                    });
                                    if (indexSach == -1)
                                    {

                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage("Cần Sách Tuyệt Kỹ"));
                                        break;
                                    }
                                    var itemIndex = character.CharacterHandler.GetItemBagByIndex(indexSach);
                                    var optionSoLanTay = itemIndex.Options.FirstOrDefault(i => i.Id is 219);
                                    if (optionSoLanTay != null && optionSoLanTay.Param == 0)
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiSay(21, "Sách đã hết tẩy được nữa vì đã tẩy quá 5 lần"));
                                        break;
                                    }
                                    if (itemIndex.Options.FirstOrDefault(i => i.Id is 217) is not null)
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiSay(21, "Giám định đi rồi tẩy tiếp"));
                                        break;
                                    }
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(21, $"{ServerUtils.ColorNotSpace}Tẩy sách tuyệt kỹ?", new List<string> { "Đồng ý", "Từ chối" }, character.InfoChar.Gender));
                                    character.TypeMenu = 41;
                                    character.CombinneIndex = new List<int>() { indexSach };
                                }
                                break;
                            case 26://giám định sách
                                {
                                    if (arrIndexUi.Count != 2)
                                    {

                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR_2));
                                    }
                                    var indexSach1 = -1;
                                    var indexBuaGiamDinh = -1;
                                    arrIndexUi.ForEach(index =>
                                    {
                                        var itemIndex = character.CharacterHandler.GetItemBagByIndex(index);
                                        if (DataCache.ListSach1.Contains(itemIndex.Id) || DataCache.ListSach2.Contains(itemIndex.Id))
                                        {
                                            indexSach1 = itemIndex.IndexUI;
                                        }
                                        else if (itemIndex.Id is 1319)
                                        {
                                            indexBuaGiamDinh = itemIndex.IndexUI;
                                        }
                                    });
                                    if (indexSach1 == -1 || indexBuaGiamDinh == -1)
                                    {

                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage("Cần Sách Tuyệt Kỹ cấp 1 và 1 bùa giám định"));
                                        break;
                                    }
                                    var itemIndex = character.CharacterHandler.GetItemBagByIndex(indexSach1);
                                    if (itemIndex.Options.FirstOrDefault(i => i.Id is 217) == null)
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiSay(21, "Tẩy sách đi rồi giám định tiếp"));
                                        break;
                                    }
                                    var optionGiamDinhCount = itemIndex.Options.FirstOrDefault(i => i.Id is 211);
                                    if (optionGiamDinhCount is null || optionGiamDinhCount.Param >= 5)
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiSay(21, "Đã đạt số lần giám định tối đa"));
                                        break;
                                    }
                                    var itemBuaGiamDinh = character.CharacterHandler.GetItemBagByIndex(indexBuaGiamDinh);
                                    var text = $"|1|Giám định {ItemCache.ItemTemplate(itemIndex.Id).Name}?";
                                    text += $"{ServerUtils.Color("blue")}Bùa giám định {itemBuaGiamDinh.Quantity}/1";
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(21, text, new List<string> { "Giám định", "Từ chối" }, character.InfoChar.Gender));
                                    character.TypeMenu = 40;
                                    character.CombinneIndex = new List<int>() { indexSach1, indexBuaGiamDinh };
                                    break;
                                }
                            case 25://đánh bóng sao pha lê
                                {
                                    if (arrIndexUi.Count != 2)
                                    {

                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR_2));
                                    }
                                    var indexSaoPhaLeCap2 = -1;
                                    var indexDaMai = -1;
                                    arrIndexUi.ForEach(index =>
                                    {
                                        var itemIndex = character.CharacterHandler.GetItemBagByIndex(index);
                                        if (DataCache.ListSaoPhaLeLevel2.Contains(itemIndex.Id))
                                        {
                                            indexSaoPhaLeCap2 = itemIndex.IndexUI;
                                        }else if (itemIndex.Id is 1480)
                                        {
                                            indexDaMai = itemIndex.IndexUI;
                                        }
                                    });
                                    if (indexSaoPhaLeCap2 == -1 || indexDaMai == -1)
                                    {

                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage("Cần 2 sao pha lê cấp 2 cùng loại và 1 đá mài"));
                                        break;
                                    }
                                    
                                    var itemIndex = character.CharacterHandler.GetItemBagByIndex(indexSaoPhaLeCap2);
                                    if (itemIndex.Quantity < 2)
                                    {
                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage("Cần 2 sao pha lê cấp 2 cùng loại và 1 đá mài"));
                                        break;
                                    }
                                    var text = "|2|Đánh bóng sao pha lê cấp 2";
                                    text += $"{ServerUtils.Color("green")}Cần 2 sao pha lê cấp 2";
                                    text += $"{ServerUtils.Color("orange")}{ItemCache.ItemTemplate(itemIndex.Id).Name}";
                                    text += $"{ServerUtils.Color("green")}Tỉ lệ thành công 50%";
                                    bool enoughGold = CombineHandler.NeedGood(character, 100000000);
                                    bool enoughDiamond = CombineHandler.NeedDiamond(character, 50);
                                    text += enoughGold ? $"{ServerUtils.Color("blue")}Cần {ServerUtils.GetMoneys(100000000)} vàng" : $"{ServerUtils.Color("red")}Cần {ServerUtils.GetMoneys(100000000)} vàng";
                                    text += enoughDiamond ? $"{ServerUtils.Color("blue")}Cần {ServerUtils.GetMoneys(50)} ngọc" : $"{ServerUtils.Color("red")}Cần {ServerUtils.GetMoneys(50)} ngọc";

                                    if (!enoughGold || !enoughDiamond)
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(21, text, new List<string> { "Đóng" }, character.InfoChar.Gender));
                                        character.TypeMenu = 27;
                                        break;
                                    }
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(21, text, new List<string> { "Nâng cấp" }, character.InfoChar.Gender));
                                    character.TypeMenu = 36;
                                    character.CombinneIndex = new List<int>() { indexSaoPhaLeCap2, indexDaMai };
                                    break;
                                }
                            case 24:// cường hóa sao pha lê
                                {
                                    if (arrIndexUi.Count != 3)
                                    {

                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR_2));
                                    }
                                    var indexItem = -1;
                                    var indexHematite = -1;
                                    var indexDuiDuc = -1;
                                    arrIndexUi.ForEach(index =>
                                    {
                                        var itemIndex = character.CharacterHandler.GetItemBagByIndex(index);
                                        if (ItemCache.ItemTemplate(itemIndex.Id).isItemCombine())
                                        {
                                            indexItem = itemIndex.IndexUI;
                                        }
                                        else if (itemIndex.Id is 1464)
                                        {
                                            indexHematite = itemIndex.IndexUI;
                                        }
                                        else if (itemIndex.Id is 1479)
                                        {
                                            indexDuiDuc = itemIndex.IndexUI;
                                        }
                                    });
                                    if (indexItem == -1 || indexHematite == -1 || indexDuiDuc == -1)
                                    {

                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage("Cần 1 trang bị cần cường hóa sao pha lê (> 7 sao pha lê) và 1 đá Hematite và 1 dùi đục"));
                                        break;
                                    }
                                    var itemIndex = character.CharacterHandler.GetItemBagByIndex(indexItem);
                                    if (itemIndex.GetParamOption(107) < 7)
                                    {
                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage("Cần 1 trang bị cần cường hóa sao pha lê (> 7 sao pha lê) và 1 đá Hematite và 1 dùi đục"));
                                        break;
                                    }
                                    if (itemIndex.GetParamOption(228) >= DataCache.MAX_LIMIT_SPL)
                                    {
                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage("Trang bị của bạn đã cường hóa tối đa ô sao pha lê"));
                                        break;
                                    }
                                    var text = "|2|Cường hóa\n|2|Ô sao pha lê thứ " + itemIndex.GetParamOption(107);
                                    text += $"{ServerUtils.Color("green")}Cần 1 đá Hematite";
                                    text += $"{ServerUtils.Color("green")}{ItemCache.ItemTemplate(itemIndex.Id).Name}";
                                    text += $"{ServerUtils.Color("green")}Tỉ lệ thành công 50%";
                                    bool enoughGold = CombineHandler.NeedGood(character, 100000000);
                                    bool enoughDiamond = CombineHandler.NeedDiamond(character, 50);
                                    text += enoughGold ? $"{ServerUtils.Color("blue")}Cần {ServerUtils.GetMoneys(100000000)} vàng" : $"{ServerUtils.Color("red")}Cần {ServerUtils.GetMoneys(100000000)} vàng";
                                    text += enoughDiamond ? $"{ServerUtils.Color("blue")}Cần {ServerUtils.GetMoneys(50)} ngọc" : $"{ServerUtils.Color("red")}Cần {ServerUtils.GetMoneys(50)} ngọc";

                                    if (!enoughGold || !enoughDiamond)
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(21, text, new List<string> { "Đóng" }, character.InfoChar.Gender));
                                        character.TypeMenu = 27;
                                        break;
                                    }
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(21, text, new List<string> { "Nâng cấp" }, character.InfoChar.Gender));
                                    character.TypeMenu = 35;
                                    character.CombinneIndex = new List<int>() { indexItem, indexHematite, indexDuiDuc };
                                    break;
                                }
                            case 23:// chế tạo sao pha lê cấp 2
                                {
                                    if (arrIndexUi.Count != 2)
                                    {

                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR_2));
                                    }
                                    var indexSplC1 = -1;
                                    var indexHematite = -1;
                                    arrIndexUi.ForEach(index =>
                                    {
                                        var itemIndex = character.CharacterHandler.GetItemBagByIndex(index);
                                        if (DataCache.ListSaoPhaLe.Contains(itemIndex.Id))
                                        {
                                            indexSplC1 = itemIndex.IndexUI;
                                        }else if (itemIndex.Id is 1464)
                                        {
                                            indexHematite = itemIndex.IndexUI;
                                        }
                                    });
                                    if (indexSplC1 == -1 || indexHematite == -1)
                                    {

                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage("Cần 1 viên sao pha lê cấp 1 và 1 đá Hematite"));
                                        break;
                                    }
                                    var itemIndex = character.CharacterHandler.GetItemBagByIndex(indexSplC1);
                                    
                                    var text = "|2|Nâng cấp sao pha lê lên cấp 2";
                                    text += $"{ServerUtils.Color("green")}Cần 1 đá Hematite";
                                    text += $"{ServerUtils.Color("green")}Cần 1 sao pha lê cấp 1";
                                    text += $"{ServerUtils.Color("green")}Tỉ lệ thành công 50%";
                                    bool enoughGold = CombineHandler.NeedGood(character, 100000000);
                                    bool enoughDiamond = CombineHandler.NeedDiamond(character, 50);
                                    text += enoughGold ? $"{ServerUtils.Color("blue")}Cần {ServerUtils.GetMoneys(100000000)} vàng" : $"{ServerUtils.Color("red")}Cần {ServerUtils.GetMoneys(100000000)} vàng";
                                    text += enoughDiamond ? $"{ServerUtils.Color("blue")}Cần {ServerUtils.GetMoneys(50)} ngọc" : $"{ServerUtils.Color("red")}Cần {ServerUtils.GetMoneys(50)} ngọc";

                                    if (!enoughGold || !enoughDiamond)
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(21, text, new List<string> { "Đóng" }, character.InfoChar.Gender));
                                        character.TypeMenu = 27;
                                        break;
                                    }
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(21, text, new List<string> { "Nâng cấp" }, character.InfoChar.Gender));
                                    character.TypeMenu = 34;
                                    character.CombinneIndex = new List<int>() { indexSplC1, indexHematite };
                                    break;
                                }
                            case 22:// chế tạo đá Hematite
                                {
                                    if (arrIndexUi.Count != 1)
                                    {

                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR_2));
                                    }
                                    var indexSplC2 = -1;
                                    arrIndexUi.ForEach(index =>
                                    {
                                        var itemIndex = character.CharacterHandler.GetItemBagByIndex(index);
                                        if (DataCache.ListSaoPhaLeLevel2.Contains(itemIndex.Id))
                                        {
                                            indexSplC2 = itemIndex.IndexUI;
                                        } 
                                    });
                                    if (indexSplC2 == -1)
                                    {

                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage("Cần 5 viên sao pha lê cấp 2"));
                                        break;
                                    }
                                    var itemIndex = character.CharacterHandler.GetItemBagByIndex(indexSplC2);
                                    if (itemIndex.Quantity < 5)
                                    {
                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage("Cần 5 viên sao pha lê cấp 2"));
                                        break;
                                    }
                                    var text = "|2|Ta sẽ phù phép";
                                    text += $"{ServerUtils.Color("blue")}Tạo đá Hematite";
                                    text += $"{ServerUtils.Color("green")}Cần 5 sao pha lê cấp 2";
                                    bool enoughGold = CombineHandler.NeedGood(character, 1000000);
                                    text += enoughGold ? $"{ServerUtils.Color("blue")}Cần {ServerUtils.GetMoneys(1000000)} vàng" : $"{ServerUtils.Color("red")}Cần {ServerUtils.GetMoneys(1000000)} vàng";
                                    if (!enoughGold)
                                    {
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(21, text, new List<string> { "Đóng" }, character.InfoChar.Gender));
                                        character.TypeMenu = 27;
                                        break;
                                    }
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(21, text, new List<string> { "Nâng cấp" }, character.InfoChar.Gender));
                                    character.TypeMenu = 33;
                                    character.CombinneIndex = new List<int>() { indexSplC2 };
                                    break;
                                }
                            case 21:
                                {
                                    if (arrIndexUi.Count != 2)
                                    {
                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR_2));
break;
                                    }
                                    var indexMatHonMang = -1;
                                    var indexDaNguSac = -1;
                                    arrIndexUi.ForEach(index =>
                                    {
                                        var itemIndex = character.CharacterHandler.GetItemBagByIndex(index);
                                        if (itemIndex.Id == 674) indexDaNguSac = itemIndex.IndexUI;
                                        if (ItemCache.ItemTemplate(itemIndex.Id).Type == 37) indexMatHonMang = itemIndex.IndexUI;
                                    });
                                    if (indexMatHonMang == -1 || indexDaNguSac == -1)
                                    {
                                        character.CharacterHandler.SendMessage(
                                       Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR_2));
                                        break;
                                    }
                                    var item = character.CharacterHandler.GetItemBagByIndex(indexMatHonMang);
                                    var optionCheck = item.Options.FirstOrDefault(i => i.Id == 72).Param;
                                    if (optionCheck >= 10)
                                    {

                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage("Vật phẩm đã đạt cấp độ tối đa"));
                                        break;

                                    }
                                    var dataCombine = DataCache.PercentEyes[optionCheck];
                                    var itemTeplate = ItemCache.ItemTemplate(item.Id);
                                    var text = "|0|Nâng cấp mắt hỗn mang";
                                    var itemUpgrade = ItemCache.ItemTemplate((short)(item.Id + 1));
                                    item.Options.ForEach(option =>
                                    {
                                        text += $"{ServerUtils.Color("green")} {ItemCache.ItemOptionTemplate(option.Id).Name.Replace("#", ""+option.Param)}";
                                    });
                                    text += (character.CharacterHandler.GetAllQuantityItemBagById(1549) >= dataCombine[0]) ? $"{ServerUtils.Color("blue")}Cần {ServerUtils.GetMoneys(dataCombine[0])} hũ vàng" : $"{ServerUtils.Color("red")}Cần {ServerUtils.GetMoneys(dataCombine[0])} hũ vàng";
                                    text += (character.InfoChar.DiamondLock >= dataCombine[2]) ? $"{ServerUtils.Color("blue")}Cần {ServerUtils.GetMoneys(dataCombine[2])} hồng ngọc" : $"{ServerUtils.Color("red")}Cần {ServerUtils.GetMoneys(dataCombine[2])} hồng ngọc";
                                    text += $"{ServerUtils.Color("blue")}Tỉ lệ thành công {dataCombine[1]}%";
                                    character.CharacterHandler.SendMessage(Service.OpenUiConfirm(21, text, new List<string> { "Nâng cấp" }, character.InfoChar.Gender));
                                    character.TypeMenu = 32;
                                    character.CombinneIndex = new List<int>() { indexMatHonMang, indexDaNguSac, dataCombine[0], dataCombine[1], dataCombine[2], optionCheck };
                                    break;
                                }
                            case 20:
                                {
                                    if (arrIndexUi.Count < 2 || arrIndexUi.Count > 4)
                                    {
                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR_2));
                                        return;
                                    }
                                    var arrIndex = arrIndexUi;
                                    var checkCongThuc = false;
                                    var checkAngelPiece = false;
                                    var checkDaNangCap = false;
                                    var checkDaMayMan = false;
                                    var indexPiece = 0;
                                    var indexCongThuc = 0;
                                    var indexDaMayMan = 0;
                                    var indexDaNangCap = 0;
                                    for (int i = 0; i < arrIndexUi.Count; i++)
                                    {
                                        var getItem = character.CharacterHandler.GetItemBagByIndex(arrIndexUi[i]);
                                        if (DataCache.IdManhAngel.Contains(getItem.Id))
                                        {
                                            checkAngelPiece = true;
                                            indexPiece = getItem.IndexUI;
                                        }
                                        else if (DataCache.IdAllCongThuc.Contains(getItem.Id))
                                        {
                                            checkCongThuc = true;
                                            indexCongThuc = getItem.IndexUI;
                                        }
                                        else if (DataCache.IdDaMayMan.Contains(getItem.Id))
                                        {
                                            checkDaMayMan = true;
                                            indexDaMayMan = getItem.IndexUI;
                                        }
                                        else if (DataCache.IdDaNangCap.Contains(getItem.Id))
                                        {
                                            checkDaNangCap = true;
                                            indexDaNangCap = getItem.IndexUI;
                                        }
                                    }
                                    if (!checkCongThuc || !checkAngelPiece)
                                    {
                                        character.CharacterHandler.SendMessage(
                                        Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR_2));
return;
                                    }
                                    var angelPiece = character.CharacterHandler.GetItemBagByIndex(indexPiece);
                                    //if (angelPiece.Quantity < 9999)
                                    //{
                                    //    character.CharacterHandler.SendMessage(
                                    //                                           Service.DialogMessage("Cần 9999 Mảnh thiên sứ !"));
                                    //    return Task.CompletedTask;
                                    //}
                                    var congthuc = character.CharacterHandler.GetItemBagByIndex(indexCongThuc);
                                    var setPercentNangCap = 35;
                                    var setPercentMayMan = 35;
                                    var enoughThoiVang = true;
                                    if (character.CharacterHandler.GetItemBagById(457) == null || character.CharacterHandler.GetItemBagById(457).Quantity < 20)
                                    {
                                        enoughThoiVang = false;
                                    }
                                    var text = $"{ServerUtils.Color("green")}Chế tạo {ItemCache.ItemTemplate(character.CharacterHandler.GetItemBagByIndex(indexPiece).Id).Name} {(ItemCache.ItemTemplate(character.CharacterHandler.GetItemBagByIndex(indexCongThuc).Id).Gender == 0 ? "Trái Đất" : ItemCache.ItemTemplate(character.CharacterHandler.GetItemBagByIndex(indexCongThuc).Id).Gender == 1 ? "Namec":"Xayda")}";
                                    if (angelPiece.Quantity < 9999)
                                    {
                                        text += $"{ServerUtils.Color("red")}Mảnh ghép {angelPiece.Quantity}/9999";
                                        if (checkDaNangCap)
                                        {
                                            var DaNangCap = character.CharacterHandler.GetItemBagByIndex(indexDaNangCap);
                                            var Template = ItemCache.ItemTemplate(DaNangCap.Id);
                                            setPercentNangCap += Template.Level * 10;
                                            text += $"{ServerUtils.Color("blue")}{Template.Name} (+{setPercentNangCap}% tỉ lệ thành công)";
                                        }
                                        else
                                        {
                                            text += $"{ServerUtils.Color("red")}Không có (Mặc định(35%) tỉ lệ thành công)";
                                        }
                                        if (checkDaMayMan)
                                        {
                                            var DaMayMan = character.CharacterHandler.GetItemBagByIndex(indexDaMayMan);
                                            setPercentMayMan += ItemCache.ItemTemplate(DaMayMan.Id).Level*10;
                                            text += $"{ServerUtils.Color("blue")}{ItemCache.ItemTemplate(DaMayMan.Id).Name} (+{setPercentMayMan}% tỉ lệ tối đa các chỉ số)";
                                        }
                                        else
                                        {
                                            text += $"{ServerUtils.Color("red")}Không có (Mặc định(35%) tỉ lệ tối đa các chỉ số)";
                                        }

                                        if (!enoughThoiVang)
                                        {
                                            text += $"{ServerUtils.Color("red")}Phí nâng cấp: 200 thỏi vàng";
                                        }
                                        else
                                        {
                                            text += $"{ServerUtils.Color("blue")}Phí nâng cấp: 200 thỏi vàng";
                                        }
                                        text += $"{ServerUtils.Color("blue")}Tỉ lệ thành công: {setPercentMayMan}%";
                                        character.CharacterHandler.SendMessage(
                                    Service
                                        .OpenUiConfirm(21, text,
                                            new List<string>() { "Đóng" }, character.InfoChar.Gender));
                                        character.TypeMenu = 27;
                                    }
                                    else
                                    {
                                        text += $"{ServerUtils.Color("green")}Mảnh ghép {angelPiece.Quantity}/9999";
                                        if (checkDaNangCap)
                                        {
                                            var DaNangCap = character.CharacterHandler.GetItemBagByIndex(indexDaNangCap);
                                            var Template = ItemCache.ItemTemplate(DaNangCap.Id);
                                            setPercentNangCap += Template.Level * 10;
                                            text += $"{ServerUtils.Color("blue")}{Template.Name} (+{setPercentNangCap}% tỉ lệ tối đa các chỉ số)";
                                        }
                                        else
                                        {
                                            text += $"{ServerUtils.Color("red")}Không có (Mặc định(35%) tỉ lệ thành công)";
                                        }
                                        if (checkDaMayMan)
                                        {
                                            var DaMayMan = character.CharacterHandler.GetItemBagByIndex(indexDaMayMan);
                                            setPercentMayMan += ItemCache.ItemTemplate(DaMayMan.Id).Level * 10;
                                            text += $"{ServerUtils.Color("blue")}{ItemCache.ItemTemplate(DaMayMan.Id).Name} (+{setPercentMayMan}% tỉ lệ tối đa các chỉ số)";
                                        }
                                        else
                                        {
                                            text += $"{ServerUtils.Color("red")}Không có (Mặc định(35%) tỉ lệ tối đa các chỉ số)";
                                        }
                                        if (!enoughThoiVang)
                                        {
                                            text += $"{ServerUtils.Color("red")}Phí nâng cấp: 200 thỏi vàng";
                                        }
                                        else
                                        {
                                            text += $"{ServerUtils.Color("blue")}Phí nâng cấp: 200 thỏi vàng";
                                        }
                                        text += $"{ServerUtils.Color("blue")}Tỉ lệ thành công: {setPercentMayMan}%";
                                        character.CharacterHandler.SendMessage(Service.OpenUiConfirm(21, text, enoughThoiVang ?
                                            new List<string>() { "Nâng cấp", "Đóng"} : new List<string>() {"Đóng" }, character.InfoChar.Gender));
                                        character.TypeMenu = enoughThoiVang ? 21 : 99; 
                                        character.CombinneIndex = new List<int> { indexPiece, indexCongThuc,checkDaNangCap ? 1 : 0, checkDaMayMan ? 1 : 0,indexDaNangCap, indexDaMayMan, setPercentNangCap, setPercentMayMan };
                                    }
                                    
                                    break;
                                }
                        //Nâng cấp vật phẩm
                        case 0:
                                {
                                    if (arrIndexUi.Count > 3)
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR_2));
                                        return;
                                    }

                                    var checkReturn = false;
                                    var index = -1;
                                    var checkIndex = 0;
                                    var checkLevel = 0;
                                    var textOption = "";
                                    var buanangcap = false;
                                    arrIndexUi.ForEach(ind =>
                                    {
                                        var itemBag = character.CharacterHandler.GetItemBagByIndex(ind);
                                        if (itemBag == null)
                                        {
                                            checkReturn = true;
                                            return;
                                        }
                                        if (itemBag.Id == 1277)
                                        {
                                            buanangcap = true;
                                        }
                                        var itemTemplate = ItemCache.ItemTemplate(itemBag.Id);
                                        if (!itemTemplate.IsTypeUpgrade())
                                        {
                                            character.CharacterHandler.SendMessage(
                                                Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR_2));
                                            checkReturn = true;
                                            return;
                                        }

                                        itemBag.Options.ForEach(option =>
                                        {
                                            switch (option.Id)
                                            {
                                                case 67:
                                                case 68:
                                                case 69:
                                                case 70:
                                                case 71:
                                                    {
                                                        checkIndex = ind;
                                                        break;
                                                    }
                                                case 72:
                                                    {
                                                        checkLevel = option.Param;
                                                        break;
                                                    }
                                                // Giáp
                                                case 47:
                                                case 6:
                                                case 27:
                                                case 0:
                                                case 7:
                                                case 28:
                                                case 14:
                                                    {
                                                        textOption += ServerUtils.Color("green") + ItemCache
                                                    .ItemOptionTemplate(option.Id).Name.Replace("#",
                                                        option.Param + option.Param * 10 / 100 + "");
                                                        break;
                                                    }
                                            }
                                        });
                                        index = arrIndexUi[0] == checkIndex ? arrIndexUi[1] : arrIndexUi[0];
                                    });
                                    if (checkReturn) return;
                                    if (checkLevel >= DataCache.MAX_LIMIT_UPGRADE || index == -1)
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service.DialogMessage(TextServer.gI().MAX_UPGRADE));
                                        return;
                                    }

                                    var item1 = character.CharacterHandler.GetItemBagByIndex(index);
                                    var item2 = character.CharacterHandler.GetItemBagByIndex(checkIndex);
                                    if (item1 == null || item2 == null) return;
                                    var itemTemplate1 = ItemCache.ItemTemplate(item1.Id);
                                    var itemTemplate2 = ItemCache.ItemTemplate(item2.Id);
                                    try
                                    {
                                        if (DataCache.CheckTypeUpgrade[itemTemplate1.Type] != item2.Id)
                                        {
                                            character.CharacterHandler.SendMessage(
                                                Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR_2));
                                            return;
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR_2));
                                        return;
                                    }

                                    var dataCombinne = DataCache.PercentUpgrade[checkLevel];
                                    var checkDa = checkLevel + dataCombinne[0] + itemTemplate1.Level;
                                    var checkGold = dataCombinne[1] + itemTemplate1.Level * 500000;
                                    var percent = dataCombinne[2] + (buanangcap ? 30 : 0);
                                    var info = $"{ServerUtils.Color("blue")}Hiện tại {itemTemplate1.Name} (+{checkLevel})";
                                    var optionDefault1 = "";
                                    var optionDefault2 = "";
                                    item1.Options.ForEach(option =>
                                    {
                                        if (option.Id != 107 && option.Id != 102 && option.Id != 72)
                                        {
                                            optionDefault1 += ServerUtils.Color("brown") + ItemCache
                                                .ItemOptionTemplate(option.Id)
                                                .Name.Replace("#", option.Param + "");
                                        }

                                        if (option.Id != 107 && option.Id != 102 && option.Id != 72 && option.Id != 47 &&
                                            option.Id != 6 && option.Id != 27 && option.Id != 0 && option.Id != 7 &&
                                            option.Id != 28 && option.Id != 14)
                                        {
                                            optionDefault2 += ServerUtils.Color("green") + ItemCache
                                                .ItemOptionTemplate(option.Id)
                                                .Name.Replace("#", option.Param + "");
                                        }
                                    });
                                    info +=
                                        $"{optionDefault1}\nSau khi nâng cấp (+{checkLevel + 1}){textOption}{optionDefault2}";
                                    info +=
                                        $"{ServerUtils.Color("blue")}{string.Format(TextServer.gI().PERCENT_UPGRADE, percent)}%";
                                    info +=
                                        $"{ServerUtils.Color(item2.Quantity < checkDa ? "red" : "blue")}Cần {checkDa} {itemTemplate2.Name}";
                                    info +=
                                        $"{ServerUtils.Color(character.InfoChar.Gold < checkGold ? "red" : "blue")}Cần {ServerUtils.GetPower(checkGold)} vàng";
                                    if (checkLevel != 0 && checkLevel % 2 == 0)
                                    {
                                        info += $"{ServerUtils.Color("blue")}{TextServer.gI().IF_FAIL}(+{checkLevel - 1})";
                                        info += $"{ServerUtils.Color("red")}Nếu dùng đá bảo vệ sẽ không bị rớt cấp";
                                    }
                                    bool dungDaBaoVeKhoa = false;
                                    var itemDaBaoVe = character.CharacterHandler.GetItemBagById(987);
                                    if (character.CharacterHandler.GetItemBagById(1143) != null)
                                    {
                                        itemDaBaoVe = character.CharacterHandler.GetItemBagById(987);
                                        dungDaBaoVeKhoa = true;
                                    }
                                    var dungDaBaoVe = false;
                                    if (checkLevel != 0 && checkLevel % 2 == 0 && itemDaBaoVe != null)
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service
                                                .OpenUiConfirm(21, info,
                                                    new List<string>() { "Nâng cấp\n" + ServerUtils.GetPower(checkGold) + "\nvàng", "Nâng cấp\ndùng đá\nbảo vệ", "Đóng" },
                                                    character.InfoChar.Gender));
                                        dungDaBaoVe = true;
                                    }
                                    else
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service
                                                .OpenUiConfirm(21, info,
                                                    new List<string>() { "Nâng cấp\n" + ServerUtils.GetPower(checkGold) + "\nvàng", "Đóng" },
                                                    character.InfoChar.Gender));
                                    }

                                    character.TypeMenu = 5;
                                    character.CombinneIndex = new List<int>()
                            {
                                index,
                                checkIndex,
                                checkDa,
                                checkGold,
                                percent,
                                (dungDaBaoVe == true ? 1 : 0),
                                (itemDaBaoVe != null ? itemDaBaoVe.IndexUI : -1),
                                buanangcap ? 1 : 0,
                                dungDaBaoVeKhoa ? 1 : 0,
                            };
                                    break;
                                }
                        //Ghép đá
                        case 1:
                        {
                            if (arrIndexUi.Count != 2)
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.DialogMessage(TextServer.gI().ERROR_DA_VUN));
                               return;
                            }

                            foreach (var itemBag in arrIndexUi.Select(i =>
                                character.CharacterHandler.GetItemBagByIndex(i)))
                            {
                                if (itemBag == null)
                                {
                                    character.CharacterHandler.SendMessage(
                                        Service.DialogMessage(TextServer.gI().ERROR_DA_VUN));
                                   return;
                                }

                                if (itemBag.Id is 225 or 226 &&
                                    (itemBag.Id != 225 || itemBag.Quantity >= 10) &&
                                    (itemBag.Id != 226 || itemBag.Quantity >= 1)) continue;
                                character.CharacterHandler.SendMessage(
                                    Service.DialogMessage(TextServer.gI().ERROR_DA_VUN));
                               return;
                            }

                            character.CharacterHandler.SendMessage(
                                Service
                                    .OpenUiConfirm(21, MenuNpc.Gi().TextBaHatMit[1], MenuNpc.Gi().MenuBaHatMit[7],
                                        character.InfoChar.Gender));
                            character.TypeMenu = 6;
                            character.CombinneIndex = new List<int>();
                            character.CombinneIndex.AddRange(arrIndexUi);
                            break;
                        }
                        //Nhập ngọc rồng
                        case 2: 
                        {
                            if (arrIndexUi.Count != 1)
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.DialogMessage("Yều cầu ngọc rồng"));
return;
                            }

                                    var index = -1;
                                    var existDragonBall = false;
                                    for (int inde = 0; inde < arrIndexUi.Count; inde++)
                                    {
                                        var ngocrong = character.CharacterHandler.GetItemBagByIndex(arrIndexUi[inde]);
                                        if (ngocrong.Id >= 14 && ngocrong.Id <= 20)
                                        {
                                            index = ngocrong.IndexUI;
                                            existDragonBall = true;
                                        }
                                    }
                                    if (!existDragonBall)
                                    {
                                        character.CharacterHandler.SendMessage(
                                    Service.DialogMessage("Yều cầu ngọc rồng 7 sao trở xuống"));
return;
                                    }
                                    if (character.CharacterHandler.GetItemBagByIndex(index).Quantity < 7)
                                    {
                                        character.CharacterHandler.SendMessage(
                                    Service.DialogMessage("Yều cầu 7 viên ngọc rồng cùng loại"));
return;
                                    }

                                    var id = character.CharacterHandler.GetItemBagByIndex(index).Id;
                            var ngocRong = ItemCache.ItemTemplate(id);
                            var ngocRongUp = ItemCache.ItemTemplate((short)CombineHandler.GetNgocRongUp(id));
                            var text = string.Format(TextServer.gI().SPLIT_BALL, ngocRong.Name, ngocRongUp.Name,
                                ngocRong.Name);
                            character.CharacterHandler.SendMessage(
                                Service
                                    .OpenUiConfirm(21, text, new List<string>() {"Làm phép", "Từ chối"},
                                        character.InfoChar.Gender));
                            character.TypeMenu = 7;
                            character.CombinneIndex = new List<int>();
                            character.CombinneIndex.Add(index);
                            break;
                        }
                        //Ép sao
                        case 3:
                                {
                                    if (arrIndexUi.Count != 2)
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR));
                                        return;
                                    }

                                    var checkIndex = -1;
                                    var index2 = -1;
                                    var countSpl = 0;
                                    var countOption107 = 0;
                                    var UpgradeUp = 0;
                                    foreach (var itemBag in arrIndexUi.Select(i =>
                                        character.CharacterHandler.GetItemBagByIndex(i)))
                                    {
                                        if (itemBag == null)
                                        {
                                            character.CharacterHandler.SendMessage(
                                                Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR));
                                            return;
                                        }

                                        var itemTemplate = ItemCache.ItemTemplate(itemBag.Id);
                                        if ((!itemTemplate.IsTypeNRKham() && !itemTemplate.IsTypeSPL() &&
                                             !itemTemplate.IsTypeBody()) || itemTemplate.Type == 5)
                                        {
                                            character.CharacterHandler.SendMessage(
                                                Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR));
                                            return;
                                        }

                                        var option107 = itemBag.Options.FirstOrDefault(option => option.Id == 107);
                                        var option102 = itemBag.Options.FirstOrDefault(option => option.Id == 102);
                                        var option228 = itemBag.Options.FirstOrDefault(option => option.Id == 228);
                                        if (option228 != null)
                                        {
                                            UpgradeUp = option228.Param;
                                        }
                                        if (option107 != null)
                                        {
                                            checkIndex = itemBag.IndexUI;
                                            countOption107 = option107.Param;
                                        }

                                        if (option102 != null)
                                        {
                                            countSpl = option102.Param;
                                        }

                                        if (checkIndex == -1) continue;
                                        index2 = arrIndexUi[0] == checkIndex
                                            ? arrIndexUi[1]
                                            : arrIndexUi[0]; //CheckIndex: trang bị, index2: sao pha lê
                                        break;
                                    }

                                    if (countOption107 == countSpl || countSpl >= DataCache.MAX_LIMIT_SPL)
                                    {
                                        character.CharacterHandler.SendMessage(Service.DialogMessage(TextServer.gI().NEED_SPL));
                                        return;
                                    }
                                    bool isUpgradeUp = false;
                                    if (countOption107 >= 8 && countSpl >= 7) {//khi đập 8 sao và đã ép 7 sao
                                        isUpgradeUp = true;
                                    }
                                    //Trang bị
                                    var item1 = character.CharacterHandler.GetItemBagByIndex(checkIndex);
                                    var item2 = character.CharacterHandler.GetItemBagByIndex(index2);
                                    if (item1 == null || item2 == null) return;
                                    var tempalte1 = ItemCache.ItemTemplate(item1.Id);
                                    var tempalte2 = ItemCache.ItemTemplate(item2.Id);
                                    if (!tempalte2.IsTypeSPL() && !tempalte2.IsTypeNRKham())
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR));
                                        return;
                                    }
                                    //khi đập 8 sao và đã ép 7 sao mà chưa cường hóa lỗ (lỗ <= số sao pha lê đã đập)
                                    if (isUpgradeUp && (DataCache.ListSaoPhaLeLevel2.Contains(tempalte2.Id) || DataCache.ListAllDragonball.Contains(tempalte2.Id)) && UpgradeUp <= countSpl)
                                    {
                                        character.CharacterHandler.SendMessage(
                                    Service.DialogMessage("Cần phải cường hóa lỗ trang bị mới có thể ép các loại chỉ số và ngọc rồng khác")) ;
                                        return;
                                    }
                                    var info = tempalte1.Name;
                                    List<int> optionBall;
                                    var paramGetPlus = 0;
                                    var checkDiamond = 10 < character.AllDiamond();
                                    var checkGold = character.InfoChar.Gold > 10_000_000 + (10_000_000 * countSpl);
                                    optionBall = DataCache.GetOptionSQL(item2.Id);//OPTION


                                    item1.Options.ForEach(option =>
                                    {
                                        if (option.Id != 107 && option.Id != 102 && option.Id != optionBall[0] &&
                                            option.Id != 72)
                                        {
                                            info +=
                                                $"{ServerUtils.Color("brown")}{ItemCache.ItemOptionTemplate(option.Id).Name.Replace("#", option.Param + "")}";
                                        }

                                        if (option.Id == optionBall[0])
                                        {
                                            paramGetPlus = option.Param;
                                        }
                                    });
                                    info +=
                                        $"{ServerUtils.Color("green")}{ItemCache.ItemOptionTemplate(optionBall[0]).Name.Replace("#", optionBall[1] + paramGetPlus + "")}";
                                    info +=
                                        $"{ServerUtils.Color(checkDiamond ? "blue" : "red")}{TextServer.gI().NEED_10_DIAMOND}";

                                    info +=
                                       $"{ServerUtils.Color(checkGold ? "blue" : "red")}{"Cần " + ServerUtils.GetMoneyParse(10_000_000 + (10_000_000 * countSpl)) + " vàng"}";
                                    character.CharacterHandler.SendMessage(
                                        Service
                                            .OpenUiConfirm(21, info,
                                                new List<string>() { checkDiamond ? checkGold ? "Đồng ý" : "Còn thiếu\nvàng" : "Còn thiếu\n10\nngọc" },
                                                character.InfoChar.Gender));
                                    character.TypeMenu = checkDiamond ? 8 : 99;
                                    character.CombinneIndex = new List<int>()
                            {
                                checkIndex,
                                index2,
                            };
                                    character.CombinneIndex.AddRange(optionBall);
                                    character.CombinneIndex.Add(isUpgradeUp ? 1 : 0);
                                    break;
                                }
                        //Pha lê hoá
                        case 4:
                        {
                            if (arrIndexUi.Count != 1)
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR));
                               return;
                            }

                            var itemBag = character.CharacterHandler.GetItemBagByIndex(arrIndexUi[0]);
                            if (itemBag == null)
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR));
                               return;
                            }

                            var itemTempalte = ItemCache.ItemTemplate(itemBag.Id);

                            if (!itemTempalte.isItemCombine() || itemTempalte.Type == 5)
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR));
                               return;
                            }

                            var info = $"{ServerUtils.Color("blue")}{itemTempalte.Name}";
                            var count = 0;
                            itemBag.Options.ForEach(option =>
                            {
                                if (option.Id == 107)
                                {
                                    count += option.Param;
                                }
                                else if (option.Id != 107 && option.Id != 102 && option.Id != 72)
                                {
                                    

                                    info +=
                                        $"{ServerUtils.Color(DataCache.TextColor[0])}{ItemCache.ItemOptionTemplate(option.Id).Name.Replace("#", option.Param + "")}";
                                }
                            });
                            if (count >= DataCache.MAX_LIMIT_SPL)
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR));
                               return;
                            }
                            var percentPhaLe = DataCache.PercentPhaLe[count];

                            info += $"{ServerUtils.Color("green")}{string.Format(TextServer.gI().PHA_LE_TRANG_BI, (count + 1))}";
                            info +=
                                $"{ServerUtils.Color("blue")}{string.Format(TextServer.gI().PHA_LE_TRANG_BI_2, percentPhaLe[1])}";
                            info +=
                                $"{ServerUtils.Color(DataCache.PercentPhaLe[count][0] * 1000000 < character.InfoChar.Gold ? "blue" : "red")}{string.Format(TextServer.gI().PHA_LE_TRANG_BI_3, percentPhaLe[0])}";

                                    character.CharacterHandler.SendMessage(
                                        Service
                                            .OpenUiConfirm(21, info,
                                                new List<string>()
                                                {
                                                                                        String.Format(TextServer.gI().PHA_LE_TRANG_BI_4, percentPhaLe[2]),
                                            string.Format(TextServer.gI().PHA_LE_TRANG_BI_5, percentPhaLe[2], 10),
                                                                                        string.Format(TextServer.gI().PHA_LE_TRANG_BI_5, percentPhaLe[2], 100),

                                            "Từ chối"
                                               }, character.InfoChar.Gender));
                            character.TypeMenu = 9;
                            character.CombinneIndex = new List<int>()
                            {
                                itemBag.IndexUI,
                                count
                            };
                            break;
                        }
                        //Chuyển hoá VÀNG / 5
                        //Chuyển hoá NGỌC / 6
                        case 5:
                        case 6:
                        {
                            if (arrIndexUi.Count != 2)
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR));
                               return;
                            }

                            var indexLuongLong = arrIndexUi[0];
                            var indexThan = arrIndexUi[1];

                            var itemLuongLong =
                                character.CharacterHandler.GetItemBagByIndex(indexLuongLong); //Trang bị level
                            var itemThan = character.CharacterHandler.GetItemBagByIndex(indexThan); //ĐỒ thần
                            if (itemLuongLong == null || itemThan == null)return;
                            var template1 = ItemCache.ItemTemplate(itemLuongLong.Id);
                            var template2 = ItemCache.ItemTemplate(itemThan.Id);
                            if (template1.Level != 12)
                            {
                                (itemThan, itemLuongLong) = (itemLuongLong, itemThan);
                                (template2, template1) = (template1, template2);
                            }

                            if (template1.Level != 12 && template1.Level != 13 ||
                                template2.Level != 12 && template2.Level != 13)
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.DialogMessage(TextServer.gI().NEED_LEVEL_ITEM));
                               return;
                            }

                            if (template1.Type != template2.Type || template1.Gender != template2.Gender)
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.DialogMessage(TextServer.gI().NEED_EQUIPMENT_SAME_KIND));
                               return;
                            }

                            var option72Ll = itemLuongLong.Options.FirstOrDefault(opt => opt.Id == 72); //Cấp lưỡng long
                            if (itemThan.Options.Count != template2.Options.Count || option72Ll == null ||
                                option72Ll.Param < 4)
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.DialogMessage(TextServer.gI().NEED_TRUE_EQUIPMENT));
                               return;
                            }

                            var levelLuongLong = option72Ll.Param;
                            var optionOld = "";

                            var listOptionLlGoc = itemLuongLong.Options
                                .Where(opt => DataCache.IdOptionGoc.Contains(opt.Id)).ToList();
                            var listOptionLl = itemLuongLong.Options
                                .Where(opt => opt.Id != 107 && opt.Id != 72 && opt.Id != 102).ToList();
                            var listOptionThan = itemThan.Options
                                .Where(opt => opt.Id != 107 && opt.Id != 72 && opt.Id != 102).ToList();
                            listOptionThan.ForEach(opt =>
                            {
                                optionOld +=
                                    $"{ServerUtils.Color("brown")}{ItemCache.ItemOptionTemplate(opt.Id).Name.Replace("#", opt.Param + "")}";
                            });
                            var info = $"{ServerUtils.Color("blue")}Hiện tại {template2.Name}{optionOld}";

                            var menu = new List<string>();
                            var checkGold = 0;

                            var optionUpgrade = "";
                            switch (character.ShopId)
                            {
                                case 5:
                                {
                                    levelLuongLong -= 1;

                                    listOptionThan.ForEach(opt =>
                                    {
                                        var paramNew = opt.Param;
                                        var optCheck = listOptionLlGoc.FirstOrDefault(o => o.Id == opt.Id);
                                        if (optCheck != null)
                                        {
                                            paramNew += optCheck.Param - optCheck.Param / 10;
                                        }

                                        optionUpgrade +=
                                            $"{ServerUtils.Color("green")}{ItemCache.ItemOptionTemplate(opt.Id).Name.Replace("#", paramNew + "")}";
                                    });
                                    var listCheckPlus = listOptionLl.Where(opt =>
                                        listOptionThan.FirstOrDefault(o => o.Id == opt.Id) == null).ToList();
                                    listCheckPlus.ForEach(opt =>
                                    {
                                        var paramNew = opt.Param;
                                        optionUpgrade +=
                                            $"{ServerUtils.Color("green")}{ItemCache.ItemOptionTemplate(opt.Id).Name.Replace("#", paramNew + "")}";
                                    });

                                    checkGold = (listOptionLl.Count * 50 + levelLuongLong * 250 + 100) * 100000;
                                    info += $"{ServerUtils.Color("blue")}Sau khi nâng cấp (+{levelLuongLong})";
                                    info += optionUpgrade;
                                    info += $"{ServerUtils.Color("green")}Chuyển qua tất cả sao pha lê";
                                    info +=
                                        $"{ServerUtils.Color(checkGold < character.InfoChar.Gold ? "blue" : "red")}Cần {ServerUtils.GetPower(checkGold)} vàng";

                                    menu = checkGold < character.InfoChar.Gold
                                        ? new List<string>()
                                            {$"Chuyển hoá\n{ServerUtils.GetMoney(checkGold)}\nvàng", "Từ chối"}
                                        : new List<string>()
                                        {
                                            $"Còn thiếu\n{ServerUtils.GetMoney(checkGold - character.InfoChar.Gold)}\nvàng"
                                        };

                                    character.TypeMenu = 10;
                                    break;
                                }
                                case 6:
                                {
                                    listOptionThan.ForEach(opt =>
                                    {
                                        var paramNew = opt.Param;
                                        var optCheck = listOptionLlGoc.FirstOrDefault(o => o.Id == opt.Id);
                                        if (optCheck != null)
                                        {
                                            paramNew += optCheck.Param;
                                        }

                                        optionUpgrade +=
                                            $"{ServerUtils.Color("green")}{ItemCache.ItemOptionTemplate(opt.Id).Name.Replace("#", paramNew + "")}";
                                    });
                                    var listCheckPlus = listOptionLl.Where(opt =>
                                        listOptionThan.FirstOrDefault(o => o.Id == opt.Id) == null).ToList();
                                    listCheckPlus.ForEach(opt =>
                                    {
                                        var paramNew = opt.Param;
                                        optionUpgrade +=
                                            $"{ServerUtils.Color("green")}{ItemCache.ItemOptionTemplate(opt.Id).Name.Replace("#", paramNew + "")}";
                                    });

                                    checkGold = listOptionLl.Count * 50 + levelLuongLong * 250 + 100;
                                    info += $"{ServerUtils.Color("blue")}Sau khi nâng cấp (+{levelLuongLong})";
                                    info += optionUpgrade;
                                    info += $"{ServerUtils.Color("green")}Chuyển qua tất  cả sao pha lê";
                                    info +=
                                        $"{ServerUtils.Color(checkGold < character.InfoChar.DiamondLock ? "blue" : "red")}Cần {checkGold} h.ngọc";

                                    menu = checkGold < character.InfoChar.Gold
                                        ? new List<string>() {$"Chuyển hoá\n{checkGold}\nhồng ngọc", "Từ chối"}
                                        : new List<string>()
                                            {$"Còn thiếu\n{checkGold - character.InfoChar.DiamondLock}\nhồng ngọc"};

                                    character.TypeMenu = 11;
                                    break;
                                }
                            }

                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(21, info, menu,
                                character.InfoChar.Gender));

                            character.CombinneIndex = new List<int>()
                            {
                                itemLuongLong.IndexUI,
                                itemThan.IndexUI,
                                levelLuongLong,
                                checkGold
                            };
                            break;
                        }
                        case 7: //Nâng cấp item porata
                        {
                            // chỉ có 2 loại vật phẩm bỏ vào
                            if (arrIndexUi.Count != 2)
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR_PORATA_2));
                               return;
                            }
                            // Bông tai cấp một
                            var itemBongTai = character.CharacterHandler.GetItemBagByIndex(arrIndexUi[0]);
                            if (itemBongTai == null || itemBongTai.Id != 454)
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR_PORATA_2_FIRST));
                               return;
                            }

                            // Mảnh vỡ bông tai
                            var itemManhVoBongTai = character.CharacterHandler.GetItemBagByIndex(arrIndexUi[1]);
                            if (itemManhVoBongTai == null || itemManhVoBongTai.Id != 933)
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR_PORATA_2_SECOND));
                               return;
                            }

                            var soLuongManhVoBongTai = itemManhVoBongTai.Options.FirstOrDefault(opt => opt.Id == 31); //Số lượng bông tai
                            if (itemManhVoBongTai == null || soLuongManhVoBongTai.Param < 9999)
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.DialogMessage(TextServer.gI().TRANG_BI_ERROR_PORATA_2_SECOND));
                               return;
                            }

                            var dataCombinne = DataCache.PercentUpgradePorata2;
                            var checkDiamond = dataCombinne[0] < character.AllDiamond();
                            var checkGold = dataCombinne[1] < character.InfoChar.Gold;

                            var info = $"{ServerUtils.Color("blue")}Bông tai Porata [+2]";
                            info += $"{ServerUtils.Color("blue")}{string.Format(TextServer.gI().PERCENT_UPGRADE, dataCombinne[2])}%";
                            info += $"{ServerUtils.Color("blue")}Cần 9999 Mảnh vỡ bông tai";
                            info +=
                                $"{ServerUtils.Color(checkGold ? "blue" : "red")}{string.Format(TextServer.gI().NEED_GOLD, ServerUtils.GetMoney(dataCombinne[1]))}";
                            info +=
                                $"{ServerUtils.Color(checkDiamond ? "blue" : "red")}{string.Format(TextServer.gI().NEED_DIAMOND, dataCombinne[0])}";
                            info += $"{ServerUtils.Color("red")}Thất bại -99 mảnh vỡ bông tai";

                            character.CharacterHandler.SendMessage(
                                Service
                                    .OpenUiConfirm(21, info,
                                        new List<string>() {"Nâng cấp\n" + ServerUtils.GetMoney(dataCombinne[1]) + " vàng\n" + dataCombinne[0] + " ngọc", "Từ chối"},
                                        character.InfoChar.Gender));
                            character.TypeMenu = 12;
                            character.CombinneIndex = new List<int>()
                            {
                                arrIndexUi[0],
                                arrIndexUi[1],
                                dataCombinne[0],
                                dataCombinne[1],
                                dataCombinne[2]
                            };
                           return;
                        }
                            case 10: // ghep 3 tb huy diet -> random tb vai tho -> than linh skh
                                {
                                    if (arrIndexUi.Count != 4)
                                    {
                                        // yeu cau 4 trang bi
                                        character.CharacterHandler.SendMessage(
                                                Service.DialogMessage("Yêu cầu Trang bị thiên sứ, hủy diệt, thần linh, 4 đá ngũ sắc"));
return;
                                    }
                                    if (character.InfoChar.Gold < 500000000)
                                    {
                                        // ko co du 500tr vang
                                        character.CharacterHandler.SendMessage(
                                                Service.DialogMessage("Yêu cầu phải có 500tr vàng"));
return;
                                    }
                                    var indexThienSu = -1;
                                    var indexHuyDiet = -1;
                                    var indexThanLinh = -1;
                                    var indexDa5Sac = -1;
                                    for (int i = 0; i < arrIndexUi.Count; i++)
                                    {
                                        var item = character.CharacterHandler.GetItemBagByIndex(arrIndexUi[i]);
                                        if (item.Id >= 650 && item.Id <= 662 && indexHuyDiet == -1)
                                        {
                                            indexHuyDiet = item.IndexUI;
                                        }
                                        if (item.Id >= 555 && item.Id <= 567 && indexThanLinh == -1){
                                            indexThanLinh = item.IndexUI;
                                        }
                                        if (item.Id >= 1047 && item.Id <= 1062 && indexThienSu == -1){
                                            indexThienSu = item.IndexUI;
                                        }
                                        if (item.Id == 674 && indexDa5Sac == -1){
                                            indexDa5Sac = item.IndexUI;
                                        }
                                    }
                                    if (indexHuyDiet == -1 || indexThanLinh == -1 || indexThienSu == -1 || indexDa5Sac == -1 || character.CharacterHandler.GetItemBagByIndex(indexDa5Sac).Quantity < 4)
                                    {
                                        // yeu cau 3 trang bi huy diet\
                                        character.CharacterHandler.SendMessage(
                                                Service.DialogMessage("Yêu cầu Trang bị thiên sứ, hủy diệt, thần linh, 4 đá ngũ sắc"));
return;
                                    }
                                    var info = $"{ServerUtils.Color("black")}Trang bị 'nhận được ngẫu nhiên từ thường -> thiên sứ'";
                                    info += $"{ServerUtils.Color("brown")}Ngẫu nhiên 1 trong 3 set kích hoạt";
                                    info += $"{ServerUtils.Color("brown")}Có tỉ lệ được đồ thần linh, hủy diệt, thiên sứ kích hoạt";
                                    info += $"{ServerUtils.Color("blue")}Tỷ lệ thành công: 100%";
                                                                        info += $"{ServerUtils.Color("blue")}Cần 4 Đá Ngũ Sắc";
                                    info += $"{ServerUtils.Color("blue")}Cần 500tr vàng";
                                    character.CharacterHandler.SendMessage(
                                        Service
                                            .OpenUiConfirm(21, info,
                                                new List<string>() { "Ghép", "Từ chối" },
                                                character.InfoChar.Gender));
                                    character.TypeMenu = 16;
                                    character.CombinneIndex = new List<int>()
                                {
                                    indexThienSu,
                                    indexHuyDiet,
                                    indexThanLinh,
                                    indexDa5Sac
                                };
                                }
                                break;

                            case 11: // 
                                {
                                    if (arrIndexUi.Count != 4)
                                    {
                                        // yeu cau 1 trang bi
                                        character.CharacterHandler.SendMessage(
                                                Service.DialogMessage("Yêu cầu 4 trang bị thần linh"));
return;
                                    }
                                    if (character.InfoChar.Gold < 500000000)
                                    {
                                        // ko co du 1ti vang
                                        character.CharacterHandler.SendMessage(
                                                Service.DialogMessage("Yêu cầu phải có 500tr vàng"));
return;
                                    }
                                    var countTb = 0;
                                    var indexItemHuyDiet = new List<int>();
                                    for (int i = 0; i < arrIndexUi.Count; i++)
                                    {
                                        var item = character.CharacterHandler.GetItemBagByIndex(arrIndexUi[i]);
                                        var itemTemplate = ItemCache.ItemTemplate(item.Id);
                                        if (itemTemplate.Level == 13)
                                        {
                                            countTb++;
                                            indexItemHuyDiet.Add(item.IndexUI);
                                        }
                                       
                                    }
                                    if (countTb != 4)
                                    {
                                        character.CharacterHandler.SendMessage(
                                                Service.DialogMessage("Yêu cầu 4 trang bị thần linh"));
                                        return;
                                    }
                                    var info = $"{ServerUtils.Color("black")}Trang bị được nâng cấp '" + ItemCache.ItemTemplate(character.CharacterHandler.GetItemBagByIndex(indexItemHuyDiet[0]).Id).Name + "'";
                                    info += $"{ServerUtils.Color("brown")}Ngẫu nhiên 1 trong 3 set kích hoạt";
                                    info += $"{ServerUtils.Color("brown")}Hành tinh của đồ sẽ là hành tinh của trang bị đầu tiên";
                                    info += $"{ServerUtils.Color("blue")}Tỷ lệ thành công: 100%";
                                    info += $"{ServerUtils.Color("blue")}Cần 500tr vàng";
                                    character.CharacterHandler.SendMessage(
                                        Service
                                            .OpenUiConfirm(21, info,
                                                new List<string>() { "Ghép", "Từ chối" },
                                                character.InfoChar.Gender));
                                    character.TypeMenu = 17;
                                    character.CombinneIndex = new List<int>()
                                {
                                    indexItemHuyDiet[0],
                                    indexItemHuyDiet[1],
                                    indexItemHuyDiet[2],
                                    indexItemHuyDiet[3],

                                };
                                    break;
                                }
                            case 12: // ghep 1 tb than linh -> huy diet
                                {
                                    if (arrIndexUi.Count != 1)
                                    {
                                        // yeu cau 1 trang bi
                                        character.CharacterHandler.SendMessage(
                                                Service.DialogMessage("Yêu cầu trang bị thần linh"));
return;
                                    }
                                    var count2 = 0;
                                    for (int i = 0; i < arrIndexUi.Count; i++)
                                    {
                                        var item = character.CharacterHandler.GetItemBagByIndex(arrIndexUi[i]);
                                        if (item.Id >= 555 && item.Id <= 567)
                                        {
                                            count2++;
                                        }
                                    }
                                    if (count2 != 1)
                                    {
                                        // yeu cau 1 trang bi than linh
                                        character.CharacterHandler.SendMessage(
                                                Service.DialogMessage("Yêu cầu trang bị thần linh"));
return;
                                    }
                                    if (character.InfoChar.Gold < 500000000)
                                    {
                                        // ko co du 500tr vang
                                        character.CharacterHandler.SendMessage(
                                                Service.DialogMessage("Yêu cầu phải có 500tr vàng"));
return;
                                    }
                                    var item2 = character.CharacterHandler.GetItemBagByIndex(arrIndexUi[0]);
                                    //var opt = item2.Options.FirstOrDefault(i => i.Id == 107);
                                    //if (opt == null || opt.Param < 7)
                                    //{
                                    //    // ko phai 7s
                                    //    character.CharacterHandler.SendMessage(
                                    //            Service.DialogMessage("Yêu cầu trang bị thần linh phải có 7 sao pha lê"));
                                    //   return;
                                    //}
                                    var temp = ItemCache.ItemTemplate(item2.Id);
                                    var info = temp.Name;
                                    for (int i = 0; i < item2.Options.Count; i++)
                                    {
                                        var tempOpt = Cache.Gi().ITEM_OPTION_TEMPLATES.FirstOrDefault(i2 => i2.Id == item2.Options[i].Id);
                                        // info += tempOpt.Name.Replace("#", ""+item2.Options[i].Param);
                                        info += $"{ServerUtils.Color("brown")}{tempOpt.Name.Replace("#", "?")}";
                                    }
                                    info += "Tỷ lệ thành công: 100%";
                                    info += "Cần 2 tỷ vàng";
                                    character.CharacterHandler.SendMessage(
                                       Service
                                           .OpenUiConfirm(21, info,
                                               new List<string>() { "Ghép", "Từ chối" },
                                               character.InfoChar.Gender));
                                    character.TypeMenu = 18;
                                    character.CombinneIndex = new List<int>()
                                    { 
                                        arrIndexUi[0]
                                    };
                                    break;
                                }
                            case 13:
                                {
                                    {
                                        if (arrIndexUi.Count != 2)
                                        {
                                            // yeu cau 1 trang bi
                                            character.CharacterHandler.SendMessage(
                                                    Service.DialogMessage("Yêu cầu trang bị và đá ngũ sắc"));
    return;
                                        }
                                        var item2 = character.CharacterHandler.GetItemBagByIndex(arrIndexUi[0]);
                                        var dns = character.CharacterHandler.GetItemBagByIndex(arrIndexUi[1]);
                                        
                                        if (!ItemCache.ItemTemplate(item2.Id).isItemCombine())
                                        {
                                            character.CharacterHandler.SendMessage(
                                                   Service.DialogMessage("Yêu cầu trang bị và đá ngũ sắc"));
    return;
                                        }
                                        if (dns.Id != 674)
                                        {
                                            character.CharacterHandler.SendMessage(
                                                  Service.DialogMessage("Yêu cầu trang bị và đá ngũ sắc"));
    return;
                                        }
                                        for (int i = 0; i < item2.Options.Count; i++)
                                        {
                                            if (item2.Options[i].Id == 34 || item2.Options[i].Id == 35 || item2.Options[i].Id == 36)
                                            {
                                                character.CharacterHandler.SendMessage(
                                                  Service.DialogMessage("Trang bị này đã tinh chế rồi !"));
        return;
                                            }
                                        }
                                        //var opt = item2.Options.FirstOrDefault(i => i.Id == 107);
                                        //if (opt == null || opt.Param < 7)
                                        //{
                                        //    // ko phai 7s
                                        //    character.CharacterHandler.SendMessage(
                                        //            Service.DialogMessage("Yêu cầu trang bị thần linh phải có 7 sao pha lê"));
                                        //   return;
                                        //}
                                        var temp = ItemCache.ItemTemplate(item2.Id);
                                        var info = temp.Name;
                                        for (int i = 0; i < item2.Options.Count; i++)
                                        {
                                            var tempOpt = Cache.Gi().ITEM_OPTION_TEMPLATES.FirstOrDefault(i2 => i2.Id == item2.Options[i].Id);
                                            // info += tempOpt.Name.Replace("#", ""+item2.Options[i].Param);
                                            info += $"{ServerUtils.Color("brown")}{tempOpt.Name.Replace("#", item2.Options[i].Param.ToString())}";
                                        }
                                        info += $"\n{ServerUtils.Color("green")}Tỷ lệ thành công: 100%\n";
                                        info += $"{ServerUtils.Color("green")}Khi nâng cấp ngẫu nhiên nhận được hiệu ứng đặc biệt";
                                        character.CharacterHandler.SendMessage(
                                           Service
                                               .OpenUiConfirm(21, info,
                                                   new List<string>() { "Nâng cấp", "Từ chối" },
                                                   character.InfoChar.Gender));
                                        character.TypeMenu = 20;
                                        character.CombinneIndex = new List<int>()
                                    {
                                        arrIndexUi[0],
                                        arrIndexUi[1]
                                    };
                                        break;
                                    }
                                }
                            case 14:
                                {
                                    {
                                        if (arrIndexUi.Count != 2)
                                        {
                                            // yeu cau 1 trang bi
                                            character.CharacterHandler.SendMessage(
                                                    Service.DialogMessage("Yêu cầu trang bị và 2 đá ngũ sắc"));
    return;
                                        }
                                        var item2 = character.CharacterHandler.GetItemBagByIndex(arrIndexUi[0]);
                                        var dns = character.CharacterHandler.GetItemBagByIndex(arrIndexUi[1]);

                                        if (!ItemCache.ItemTemplate(item2.Id).isItemCombine())
                                        {
                                            character.CharacterHandler.SendMessage(
                                                   Service.DialogMessage("Yêu cầu trang bị và đá ngũ sắc"));
    return;
                                        }
                                        if (dns.Id != 674 || dns.Quantity < 2)
                                        {
                                            character.CharacterHandler.SendMessage(
                                                  Service.DialogMessage("Yêu cầu trang bị và 2 đá ngũ sắc"));
    return;
                                        }
                                        bool tinhche = false;
                                        for (int i = 0; i < item2.Options.Count; i++)
                                        {
                                            if (item2.Options[i].Id == 34 || item2.Options[i].Id == 35 || item2.Options[i].Id == 36)
                                            {
                                                tinhche = true;
                                            }
                                        }
                                        if (!tinhche)
                                        {
                                            character.CharacterHandler.SendMessage(
                                                 Service.DialogMessage("Trang bị này chưa tinh chế !"));
    return;
                                        }
                                        //var opt = item2.Options.FirstOrDefault(i => i.Id == 107);
                                        //if (opt == null || opt.Param < 7)
                                        //{
                                        //    // ko phai 7s
                                        //    character.CharacterHandler.SendMessage(
                                        //            Service.DialogMessage("Yêu cầu trang bị thần linh phải có 7 sao pha lê"));
                                        //   return;
                                        //}
                                        var temp = ItemCache.ItemTemplate(item2.Id);
                                        var info = temp.Name;
                                        for (int i = 0; i < item2.Options.Count; i++)
                                        {
                                            var tempOpt = Cache.Gi().ITEM_OPTION_TEMPLATES.FirstOrDefault(i2 => i2.Id == item2.Options[i].Id);
                                            // info += tempOpt.Name.Replace("#", ""+item2.Options[i].Param);
                                            info += $"{ServerUtils.Color("brown")}{tempOpt.Name.Replace("#", item2.Options[i].Param.ToString())}";
                                        }
                                        info += $"{ServerUtils.Color("green")}Khi xóa ấn, mọi tác dụng của ấn cũ sẽ mất";
                                        character.CharacterHandler.SendMessage(
                                           Service
                                               .OpenUiConfirm(21, info,
                                                   new List<string>() { "Nâng cấp", "Từ chối" },
                                                   character.InfoChar.Gender));
                                        character.TypeMenu = 31;
                                        character.CombinneIndex = new List<int>()
                                    {
                                        arrIndexUi[0],
                                        arrIndexUi[1]
                                    };
                                        break;
                                    }
                                }
                            case 8://mở chỉ số item porata
                                {
                                    // chỉ có 3 loại vật phẩm bỏ vào
                                    if (arrIndexUi.Count != 3)
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service.DialogMessage(TextServer.gI().UPGRADE_OPTION_PORATA_2_ERROR_COUNT));
                                        break;
                                    }

                                    // Bông tai cấp 2
                                    var itemBongTai = character.CharacterHandler.GetItemBagByIndex(arrIndexUi[0]);
                                    if (itemBongTai == null || itemBongTai.Id != 921)
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service.DialogMessage(TextServer.gI().UPGRADE_OPTION_PORATA_2_FIRST));
                                        break;
                                    }

                                    // Mảnh hồn bông tai
                                    var itemManhVoBongTai = character.CharacterHandler.GetItemBagByIndex(arrIndexUi[1]);
                                    if (itemManhVoBongTai == null || itemManhVoBongTai.Id != 934 || itemManhVoBongTai.Quantity < 99)
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service.DialogMessage(TextServer.gI().UPGRADE_OPTION_PORATA_2_SECOND));
                                        break;
                                    }

                                    // Đá xanh lam
                                    var itemDaXanhLam = character.CharacterHandler.GetItemBagByIndex(arrIndexUi[2]);
                                    if (itemDaXanhLam == null || itemDaXanhLam.Id != 935)
                                    {
                                        character.CharacterHandler.SendMessage(
                                            Service.DialogMessage(TextServer.gI().UPGRADE_OPTION_PORATA_2_THIRD));
                                        break;
                                    }

                                    var dataCombinne = DataCache.PercentUpgradePorata2;
                                    var checkDiamond = dataCombinne[0] < character.AllDiamond();
                                    var checkGold = dataCombinne[1] < character.InfoChar.Gold;

                                    var info = $"{ServerUtils.Color("blue")}Bông tai Porata [+2]";
                                    info += $"{ServerUtils.Color("blue")}{string.Format(TextServer.gI().PERCENT_UPGRADE, dataCombinne[2])}%";
                                    info += $"{ServerUtils.Color("blue")}Cần 99 Mảnh hồn bông tai";
                                    info += $"{ServerUtils.Color("blue")}Cần 1 Đá xanh lam";
                                    info +=
                                        $"{ServerUtils.Color(checkGold ? "blue" : "red")}{string.Format(TextServer.gI().NEED_GOLD, ServerUtils.GetMoney(dataCombinne[1]))}";
                                    info +=
                                        $"{ServerUtils.Color(checkDiamond ? "blue" : "red")}{string.Format(TextServer.gI().NEED_DIAMOND, dataCombinne[0])}";
                                    info += $"{ServerUtils.Color("green")}+1 Chỉ số ngẫu nhiên";

                                    character.CharacterHandler.SendMessage(
                                        Service
                                            .OpenUiConfirm(21, info,
                                                new List<string>() { "Nâng cấp\n" + ServerUtils.GetMoney(dataCombinne[1]) + " vàng\n" + dataCombinne[0] + " ngọc", "Từ chối" },
                                                character.InfoChar.Gender));
                                    character.TypeMenu = 13;
                                    character.CombinneIndex = new List<int>()
                            {
                                arrIndexUi[0],
                                arrIndexUi[1],
                                arrIndexUi[2],
                                dataCombinne[0],
                                dataCombinne[1],
                                dataCombinne[2]
                            };
                                    break;
                                }
                            case 9://nở trứng pet
                        {
                            if (arrIndexUi.Count > 3 || arrIndexUi.Count < 2)
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.DialogMessage("Cần chọn đúng trứng linh thú, 99 hồn linh thú, (Thỏi vàng nếu muốn nở trứng sớm) theo đúng thứ tự"));
                               return;
                            }

                            // Trứng linh thú
                            var itemTrungLinhThu = character.CharacterHandler.GetItemBagByIndex(arrIndexUi[0]);
                            if (itemTrungLinhThu == null || itemTrungLinhThu.Id != 1152)
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.DialogMessage("Cần cho trứng linh thú vào đầu tiên"));
                               return;
                            }

                            // Hồn linh thú
                            var itemHonLinhThu = character.CharacterHandler.GetItemBagByIndex(arrIndexUi[1]);
                            if (itemHonLinhThu == null || itemHonLinhThu.Id != 1151 || itemHonLinhThu.Quantity < 99)
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.DialogMessage("Cần cho 99 hồn linh thú vào thứ 2"));
                               return;
                            }

                            var thoiGianNoTrung = itemTrungLinhThu.Options.FirstOrDefault(option => option.Id == 211);

                            if (thoiGianNoTrung != null)
                            {
                                if (arrIndexUi.Count != 3)
                                {
                                    character.CharacterHandler.SendMessage(
                                        Service.DialogMessage("Cần chọn đúng trứng linh thú, 99 hồn linh thú, 5 thỏi vàng theo đúng thứ tự"));
                                   return;
                                }

                                var itemThoiVang = character.CharacterHandler.GetItemBagByIndex(arrIndexUi[2]);
                                if (itemThoiVang == null || itemThoiVang.Id != 457 || itemThoiVang.Quantity < 5)
                                {
                                    character.CharacterHandler.SendMessage(
                                        Service.DialogMessage("Cần có 5 thỏi vàng để nở trứng sớm"));
                                   return;
                                }
                            }

                            if (character.LengthBagNull() <= 0)
                            {
                                character.CharacterHandler.SendMessage(
                                    Service.DialogMessage("Cần 1 ô trống hành trang để chứa linh thú"));
                               return;
                            }

                            var info = $"{ServerUtils.Color("red")}Nở trứng linh thú";
                            info += $"{ServerUtils.Color("blue")}Cần 1 Trứng linh thú";
                            info += $"{ServerUtils.Color("blue")}Cần 99 Hồn linh thú";

                            if (arrIndexUi.Count == 3)
                            {
                                info += $"{ServerUtils.Color("blue")}Cần 5 thỏi vàng để nở trứng sớm";
                            }
                            info += $"{ServerUtils.Color("green")}Sẽ nở ra linh thú cùng hạng của trứng";

                            character.CharacterHandler.SendMessage(
                                Service
                                    .OpenUiConfirm(21, info,
                                        new List<string>() {"Nở trứng", "Từ chối"},
                                        character.InfoChar.Gender));
                            character.TypeMenu = 15;

                            if (arrIndexUi.Count == 3)
                            {
                                character.CombinneIndex = new List<int>()
                                {
                                    arrIndexUi[0],
                                    arrIndexUi[1],
                                    arrIndexUi[2],
                                };
                            }
                            else 
                            {
                                character.CombinneIndex = new List<int>()
                                {
                                    arrIndexUi[0],
                                    arrIndexUi[1],
                                };
                            }

                           return;
                        }
                    }

                    break;
                }
            }

           return;
        }
    }
}