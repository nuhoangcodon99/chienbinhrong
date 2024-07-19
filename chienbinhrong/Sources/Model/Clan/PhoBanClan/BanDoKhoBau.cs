
using System.Collections.Concurrent;
using System.Collections.Generic;
using NgocRongGold.Application.Handlers.Character;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.IO;
using NgocRongGold.Model.Info;
using NgocRongGold.Model.Clan;
using NgocRongGold.Application.Threading;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Constants;
using NgocRongGold.Model.Character;
using System;
using System.Linq;
using NgocRongGold.Model.Template;
using Application.Interfaces.Map;

namespace NgocRongGold.Model.Clan
{
    public class BanDoKhoBau
    {
        public long Time { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int Level { get; set; }
        public PhoBanStatus Status { get; set; } // trang thai pho ban
        public long Record { get; set; }
        public List<long> LastRecord { get; set; } // max 5
        public long LastTimeRecord { get; set; } // thoi gian lap ki luc
        public BanDoKhoBau()
        {
            Time = -1;
            Name = "Bản đồ kho báu";
            Status = PhoBanStatus.CLOSE;
            Record = -1;
            LastRecord = null;
            LastTimeRecord = -1;
            Count = 1;
        }
       
       
        
       
        public void Reset(bool newDay = false)
        {
            Time = -1;
            Name = "Bản đồ kho báu";
            Status = PhoBanStatus.CLOSE;
            if (newDay)
            {
                Count = 1;
            }
        }
        public void SendTextTime(Clan clan = null, Character.Character character = null)
        {
            if (clan != null)
            {
                foreach (var clanMember in clan.Thành_viên)
                {
                    var member = ClientManager.Gi().GetCharacter(clanMember.Id);
                    if (member == null) continue;
                    member.CharacterHandler.SendMessage(Service.ItemTimeWithMessage(Name, (int)TextTime.BAN_DO_KHO_BAU, (int)((Time - ServerUtils.CurrentTimeMillis()) / 1000)));
                }
            }
            if (character != null) character.CharacterHandler.SendMessage(Service.ItemTimeWithMessage(Name, (int)TextTime.BAN_DO_KHO_BAU, (int)((Time - ServerUtils.CurrentTimeMillis()) / 1000)));
        }
        public void Update(long timeserver)
        {
            switch (Status)
            {

                case PhoBanStatus.OPEN:
                case PhoBanStatus.WIN:
                    if (timeserver > Time)
                    {
                        Close();
                    }
                    break;

            }
        }
        public void KickPlayer(Character.Character character)
        {
        }
        public void Close()
        {
            if (Status == PhoBanStatus.WIN)
            {
                var timeServer = ServerUtils.CurrentTimeMillis();
                if (Time - timeServer > Record)
                {
                    Record = Time - timeServer;
                }
                if (LastRecord.Count >= 5) LastRecord.RemoveAt(0);
                LastRecord.Add(Record);
                LastTimeRecord = timeServer;
            }
          
            Reset(false);
        }
        public void Open(Clan clan)
        {
            Count -= 1;
            Time = (DataCache._1MINUTES * 30) + ServerUtils.CurrentTimeMillis();
            Status = PhoBanStatus.OPEN;
           
            SendTextTime(clan, null);
        }
        public bool CheckOpen()
        {
            return Status is (PhoBanStatus.OPEN or PhoBanStatus.WIN or PhoBanStatus.SPECIAL);
        }
        public bool CheckClose()
        {
            return Status is PhoBanStatus.CLOSE;
        }
        
       

    }
}