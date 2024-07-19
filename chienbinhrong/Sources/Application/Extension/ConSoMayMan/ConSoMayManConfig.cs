using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.ConSoMayMan
{
    public class ConSoMayManConfig
    {
        public readonly int PercentReward = 2;
        public readonly string FormUpConSo = "0-1000";
        public int RoomId = 1;
        //action variable
        public List<int> PlayersGame = new List<int>();
        public ConSoMayManStatus ConSoMayManStatus = ConSoMayManStatus.WAIT ;
        public TypeReward TypeReward = new TypeReward();
        public Dictionary<int ,ICharacter> LastPlayersWinGame = new Dictionary<int, ICharacter>();
        public string LastPlayersNameWinGame = "";
        public int LastResult = -1;
        public List<long> Joins = new List<long>() { 0, 0};///tong tham gia, tong dat cuoc
        public long TimeResult = ServerUtils.CurrentTimeMillis();// hien thi ket qua (trao qua)
        public long timeRemain = 30000 + ServerUtils.CurrentTimeMillis(); // 5 phut thoi gian delay moi lan

    }
}