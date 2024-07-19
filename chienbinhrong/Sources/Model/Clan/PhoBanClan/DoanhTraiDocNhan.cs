
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
using NgocRongGold.Model.Item;
using Application.Interfaces.Map;
using Model.Zone;
using Chiến_Binh_Rồng.Sources.Application.Manager;

namespace NgocRongGold.Model.Clan
{
    public class DoanhTraiDocNhan
    {
        public long Time { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int Level { get; set; }
        public PhoBanStatus Status { get; set; } // trang thai pho ban
        public long Record { get; set; }
        public List<long> LastRecord { get; set; } // max 5
        public long LastTimeRecord { get; set; } // thoi gian lap ki luc
        public int PercentHp { get; set; }
        public DoanhTraiDocNhan()
        {
            Time = -1;
            Name = "Doanh trại độc nhãn";
            Status = PhoBanStatus.CLOSE;
            Record = -1;
            LastRecord = null;
            LastTimeRecord = -1;
            Count = 1;
            PercentHp = 0;
        }
        
      
       
        public void Win(Clan clan)
        {
            for (int i = 0; i < DataCache.IdMapReddot.Count;i++)
            {
                var mapId = DataCache.IdMapReddot[i];
                var zone = (ZoneRedRibbon)MapManager.Get(mapId).GetZoneById(clan.Id);
                var itemMap = new ItemMap(-1, ItemCache.GetItemDefault((short)(ServerUtils.RandomNumber(17, 21))), true);
                var index = i;
                itemMap.X = (short)DataCache.listXDoanhTrai[index];
                itemMap.Y = (short)DataCache.listYDoanhTrai[index];
                zone.ZoneHandler.LeaveItemMap(itemMap);
                zone. Time = DataCache._1MINUTES * 5 + ServerUtils.CurrentTimeMillis();
            }
            Time = DataCache._1MINUTES * 5 + ServerUtils.CurrentTimeMillis();
            SendTextTime(clan);
            Status = PhoBanStatus.SPECIAL;
        }
        public void Open(Clan clan)
        {
            PercentHp = 0;
            clan.Thành_viên.ForEach(member =>
            {
                var imember = ClientManager.Gi().GetCharacter(member.Id);
                if (imember != null)
                {
                    PercentHp += (int)imember.HpFull/50000;
                }
            });
            Count--;
            Time = DataCache._1MINUTES * 30 + ServerUtils.CurrentTimeMillis();
            Status = PhoBanStatus.OPEN;
        }
       
        public void Reset(bool newDay = false)
        {
            Time = -1;
            Name = "Doanh trại độc nhãn";
            Status = PhoBanStatus.CLOSE;
            if (newDay)
            {
                Count = 1;
            }
            PercentHp = 0;
        }
        public void SendTextTime(Clan clan = null, Character.Character character = null)
        {
            if (clan != null)
            {
                foreach (var clanMember in clan.Thành_viên)
                {
                    var member = ClientManager.Gi().GetCharacter(clanMember.Id);
                    if (member == null) continue;
                    member.CharacterHandler.SendMessage(Service.ItemTimeWithMessage(Name, (int)TextTime.DOANH_TRAI_DOC_NHAN, (int)((Time - ServerUtils.CurrentTimeMillis()) / 1000)));
                }
            }
            if (character != null) character.CharacterHandler.SendMessage(Service.ItemTimeWithMessage(Name, (int)TextTime.DOANH_TRAI_DOC_NHAN, (int)((Time - ServerUtils.CurrentTimeMillis()) / 1000)));
        }
        public void Update(long timeserver)
        {
            switch (Status)
            {

                case PhoBanStatus.OPEN:
                case PhoBanStatus.WIN:
                case PhoBanStatus.SPECIAL:
                    if (timeserver > Time)
                    {
                        Close();
                    }
                    break;

            }
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
            RedRibbonManager.gI().countRedRibbon--;
            Reset(false);
        }


        public bool CheckOpen()
        {
            return Status is (PhoBanStatus.OPEN or PhoBanStatus.WIN or PhoBanStatus.SPECIAL);
        }
        public bool CheckClose()
        {
            return Status is PhoBanStatus.CLOSE;
        }
       
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}