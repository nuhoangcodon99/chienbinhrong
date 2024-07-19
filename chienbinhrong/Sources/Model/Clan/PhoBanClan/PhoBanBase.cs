using Application.Interfaces.Map;
using NgocRongGold.Application.Threading;
using NgocRongGold.Model.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace NgocRongGold.Model.Clan
{
    public interface PhoBanBase 
    {
        public long Time { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public PhoBanStatus Status { get; set; } // trang thai pho ban
        public List<Boss> Bosses { get; set; }
        public long Record { get; set; }
        public List<long> LastRecord { get; set; } // max 5
        public long LastTimeRecord { get; set; } // thoi gian lap ki luc
        public IList<IMap> Maps { get; set; }
        public IMap GetMap(int obj);
        public void JoinMap(Character.Character character, int MapNext, bool isCapsule = false);
        public void KickPlayer(Character.Character character);  
        public void Reset(bool newDay = false);
        public void SendTextTime(Clan clan = null, Character.Character character = null);
        public void Update(long timeserver);
        public void Close();
        public bool CheckOpen();
        public bool CheckClose();
        public void Open(Clan clan);
        public void InitMob(Clan clan);
        public void InitBoss(Clan clan);
    }
}
