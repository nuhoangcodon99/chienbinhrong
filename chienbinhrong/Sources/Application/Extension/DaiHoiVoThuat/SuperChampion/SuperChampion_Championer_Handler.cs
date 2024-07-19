using Linq.Extras;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Model;
using NgocRongGold.Model.Character;
using Org.BouncyCastle.Asn1.CryptoPro;
using Org.BouncyCastle.Math.Field;
using Serilog.Settings.Configuration;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using static NgocRongGold.Model.BangXepHang.BangXepHang;

namespace NgocRongGold.Application.Extension.DaiHoiVoThuat.SuperChampion
{
    public class SuperChampion_Championer_Handler
    {
        public void SendChallenge(Character character, int playerId)
        {
            character.CharacterHandler.SendMessage(Service.ServerMessage("Vui lòng chờ ít phút để lên võ đài"));
            var player = SuperChampion_Manager.gI().Get(playerId);
            var me = SuperChampion_Manager.gI().Get(character.Id);
            if(player.Top <= 10 && me.Top >= 100)// không thể thách đấu top 10 nếu không trong top 100
            {
                character.CharacterHandler.SendMessage(Service.ServerMessage("Không thể khiêu chiến top 10 nếu bạn không trong top 100"));
                return;
            }
            if (player.Status == SuperChampion_Championer_Status.BATTLE)// không thể thách đấu nếu dối phương đang thách đấu
            {
                character.CharacterHandler.SendMessage(Service.ServerMessage("Đối phương đang thách đấu"));
                return;
            }
            if (character.Zone.Bosses.Count > 0)
            {
                var map = character.Zone.Map;
                if (map.Zones.FirstOrDefault(zone => zone.Characters.Count <= 0) != null)
                {
                    var zone = map.Zones.FirstOrDefault(zone => zone.Characters.Count <= 0);
                    map.OutZone(character);
                    zone.ZoneHandler.JoinZone(character, false, false, 0);
                }
                else
                {
                    character.CharacterHandler.SendMessage(Service.ServerMessage("Vui lòng đợi 1 tí nữa"));
                    return;
                }
            }
            //if (not zone count == 0)
            var boss = SuperChampion_Manager.gI().Clone(character, playerId);
            if (boss is null)
            {
                character.CharacterHandler.SendMessage(Service.ServerMessage("Vui lòng đợi 1 tí nữa"));
                return;
            }
            me.Status = SuperChampion_Championer_Status.BATTLE;
            me.Time = DataCache._1MINUTES*180+ServerUtils.CurrentTimeMillis();
            me.PlayerChallenge = playerId;
            async void Action()
            {
                var boss = SuperChampion_Manager.gI().Clone(character,playerId);
                boss.InfoChar.TypePk = 0;
                boss.BasePositionX = 497;
                boss.BasePositionY = 264;
                boss.CharacterHandler.SetUpInfo();
                character.Zone.ZoneHandler.AddBoss(boss);
                boss.CharacterHandler.SendZoneMessage(Service.PublicChat(boss.Id, "Sẵn sàng chưa?"));
                SendPosistion(character, 334, 264);
                SetTypePk(character, 0);
                await Task.Delay(1000);
                SetTypePk(boss, 3);
                SetTypePk(character, 3);
               
            }
            var task = new Task(Action);
            task.Start();
        }
        public void SendPosistion(ICharacter character, short toX, short toY)
        {
            character.InfoChar.X = toX;
            character.InfoChar.Y = toY;
            character.Zone.ZoneHandler.SendMessage(Service.SendPos(character, 0));
        }
        public void OutMap(Character character)
        {
            Lose(character);
            if (character.InfoChar.MapId == 130)
            {
                SendPosistion(character, 389, 360);
            }
            SetTypePk(character, 0);
        }
       
        public void SetTypePk(ICharacter character, sbyte typePk)
        {
            character.InfoChar.TypePk = typePk;
            character.CharacterHandler.SendZoneMessage(Service.ChangeTypePk(character.Id, typePk));
        }
        public void ThoiMien(ICharacter character)
        {
            character.InfoSkill.ThoiMien.IsThoiMien = true;
            character.InfoSkill.ThoiMien.Time = ChampionShip23_Cache.TimeThoiMien + ServerUtils.CurrentTimeMillis();
            character.CharacterHandler.SendMessage(Service.ItemTime(DataCache.TimeThoiMien[0], ChampionShip23_Cache.TimeThoiMien / 1000));
            character.CharacterHandler.SendMessage(Service.SkillEffectPlayer(character.Id, character.Id, 1, 40));
        }
        public void Win(Character character)
        {
            var me = SuperChampion_Manager.gI().Get(character.Id);
            var player = SuperChampion_Manager.gI().Get(me.PlayerChallenge);

            AddHistory(me, player, SuperChampion_Result.Thắng, SuperChampion_Result.Thua);
            me.Status = SuperChampion_Championer_Status.NORMAL;
            player.Status = SuperChampion_Championer_Status.NORMAL;
            var playerTop = player.Top;
            player.Top = me.Top;
            me.Top = playerTop;
            SendPosistion(character, 389, 360);
            character.CharacterHandler.SendMessage(Service.PublicChat(character.Id, $"Thắng rồi, lên hạng {player.Top} rồi hahahah"));
        }
        public void Lose(Character character)
        {
            var me = SuperChampion_Manager.gI().Get(character.Id);
            var player = SuperChampion_Manager.gI().Get(me.PlayerChallenge);

            AddHistory(me, player, SuperChampion_Result.Thua, SuperChampion_Result.Thắng);
            me.Status = SuperChampion_Championer_Status.NORMAL;
            player.Status = SuperChampion_Championer_Status.NORMAL;
            character.DataSieuHang.Ticket -= 1;
            RemoveBoss(character);
        }
        public void RemoveBoss(Character character)
        {
            foreach(var boss in character.Zone.Bosses)
            {
                boss.Value.CharacterHandler.SendDie();
            }

        }
        public void TopInfo(Character character)
        {

            var message = new Message(-96);
            message.Writer.WriteByte(0);
            message.Writer.WriteUTF("Top Cường Giả");
            var me = SuperChampion_Manager.gI().Get(character.Id);
            var players = SuperChampion_Manager.gI().GetList(me.Top, me.Top - 10);
            if ((players is null) || (players.Count <= 0))
            {
                message.CleanUp();
                message = null;
                character.CharacterHandler.SendMessage(Service.ServerMessage("Đang trống, không thể thách đấu"));
                return;
            }
            message.Writer.WriteByte(players.Count);
            players.ForEach(i =>
           {
               //var i = character.DataSieuHang;

               //AddHistory(character.DataSieuHang, i, SuperChampion_Result.Thắng, SuperChampion_Result.Thua);

               message.Writer.WriteInt(i.Top); // rank
               message.Writer.WriteInt(i.PlayerID); // pl id
               message.Writer.WriteShort(i.Championer.Head); // head id
               message.Writer.WriteShort(-1); // head icon
               message.Writer.WriteShort(i.Championer.Body); // body
               message.Writer.WriteShort(i.Championer.Leg); // leg
               message.Writer.WriteUTF(i.Championer.Name);  // name
               message.Writer.WriteUTF(getPrice(i.Top)); // name
               message.Writer.WriteUTF(getFullInfo(i)); // info 3
                                                        // });

           });
            character.CharacterHandler.SendMessage(message);
        }
        public void TopInfo100(Character character)
        {

            var message = new Message(-96);
            message.Writer.WriteByte(0);
            message.Writer.WriteUTF("Top 100 Cường Giả");
            var me = SuperChampion_Manager.gI().Get(character.Id);
            var players = SuperChampion_Manager.gI().GetList(1, 100);   
            message.Writer.WriteByte(players.Count);
            players.ForEach(i =>
            {
                //var i = character.DataSieuHang;

                //AddHistory(character.DataSieuHang, i, SuperChampion_Result.Thắng, SuperChampion_Result.Thua);

                message.Writer.WriteInt(i.Top); // rank
                message.Writer.WriteInt(i.PlayerID); // pl id
                message.Writer.WriteShort(i.Championer.Head); // head id
                message.Writer.WriteShort(-1); // head icon
                message.Writer.WriteShort(i.Championer.Body); // body
                message.Writer.WriteShort(i.Championer.Leg); // leg
                message.Writer.WriteUTF(i.Championer.Name);  // name
                message.Writer.WriteUTF(getPrice(i.Top)); // name
                message.Writer.WriteUTF(getFullInfo(i)); // info 3
                                                         // });

            });
            character.CharacterHandler.SendMessage(message);
        }
        public void AddHistory(SuperChampion_Championer _championer, SuperChampion_Championer enemy_championer, SuperChampion_Result result, SuperChampion_Result result2)
        {
            var dateNow = ServerUtils.TimeNow().Date;
            if (_championer != null)
            {
                _championer.Historys.Add(new SuperChampion_Championer_History()
                {
                    Text = $"{result} {enemy_championer.Championer.Name} [{enemy_championer.Top}]",
                    Date = dateNow,
                });
            }
            if (enemy_championer != null)
            {
                enemy_championer.Historys.Add(new SuperChampion_Championer_History()
                {
                    Text = $"{result2} {_championer.Championer.Name} [{_championer.Top}]",
                    Date = dateNow,
                });
            }
        }
        public string getFullInfo(SuperChampion_Championer championer)
        {
            var info = "";
            var dateNow = ServerUtils.TimeNow().Date;
            info += getPoint(championer);
            if (championer.Historys.Count < 1) 
            {
                info += $"Thắng\\Thua: {championer.Win}\\{championer.Lose}\n";
                return info;
            }
            info += $"{championer.Win}:{championer.Lose}\n";

            for (int i = 0; i < championer.Historys.Count; i++)
            {
                var history = championer.Historys[i];
                if ((dateNow - history.Date).TotalDays >= 1)
                {
                    championer.Historys.RemoveAt(i);
                    continue;
                }
                else
                {
                    int time = (int)((dateNow - history.Date).TotalSeconds);
                    var timeUtils = ServerUtils.GetTimeInPast(time);
                    if (i == 0 && i != championer.Historys.Count - 1)
                    {
                        info += history.Text + $" {timeUtils}\n";
                        continue;
                    }
                    else if (i == 0)
                    { 
                        info += history.Text + $" {timeUtils}\n"; ;
                        continue;
                    }
                    else if (i == championer.Historys.Count - 1)
                    {
                        info += history.Text + $" {timeUtils}"; ;
                        continue;
                    }
                    else info += history.Text + $" {timeUtils}\n";
                }
            }
            return info;
        }
        public string getPoint(SuperChampion_Championer championer)
        {
            var info = "";
            info += $"HP {ServerUtils.GetMoneys(championer.Championer.HpFull)}\n";
            info += $"Sức đánh {ServerUtils.GetMoneys(championer.Championer.DamageFull)}\n";
            info += $"Giáp {ServerUtils.GetMoneys(championer.Championer.DefenceFull)}\n";

            return info;
        }
        public string getPrice(int top)
        {
            switch (top)
            {
                case 1:
                    return "+1000 ngọc/ngày";
                case 2:
                    return "+500 ngọc/ngày";
                case 3:
                    return "+200 ngọc/ngày";
                case >= 4 and <= 10:
                    return "+100 ngọc/ngày";
                case >= 11 and <= 30:
                    return "+50 ngọc/ngày";
                case >= 31 and <= 50:
                    return "+20 ngọc/ngày";
                case >= 51 and <= 100:
                    return "+10 ngọc/ngày";
                default:
                    return "";

            }
        }
        public int getRuby(int top)
        {
            switch (top)
            {
                case 1:
                    return 1000;
                case 2:
                    return 500;
                case 3:
                    return 200;
                case >= 4 and <= 10:
                    return 100;
                case >= 11 and <= 30:
                    return 50;
                case >= 31 and <= 50:
                    return 20;
                case >= 51 and <= 100:
                    return 10;
                default:
                    return 0;

            }
        }
        public void Challenge(int id)
        {

        }
        public void Update(Character character, long timeServer)
        {
            switch (character.DataSieuHang.Status)
            {
                case SuperChampion_Championer_Status.BATTLE:
                    if (character.DataSieuHang.Time < timeServer)
                    {
                        Lose(character);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
