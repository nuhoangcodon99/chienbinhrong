﻿using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.Interfaces.Monster;

namespace NgocRongGold.Model.Info.Skill
{
    public class MeTroi
    {
        public long DelayStart { get; set; }
        public bool IsMeTroi { get; set; }
        public long TimeTroi { get; set; }
        public IMonster Monster { get; set; }
        public ICharacter Character { get; set; }

        public MeTroi()
        {
            DelayStart = -1;
            IsMeTroi = false;
            TimeTroi = -1;
            Monster = null; 
            Character = null; 
        }
    }
}