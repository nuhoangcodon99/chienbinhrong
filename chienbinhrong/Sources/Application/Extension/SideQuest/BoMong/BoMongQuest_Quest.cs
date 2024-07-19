using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.SideQuest.BoMong
{
    public enum BoMongQuest_Status
    {
        UN_FINISHED = 0,
        FINISHED = 1,
    }
    public enum BoMongQuest_RewardStatus
    {
        UN_COLLECTED = 0,
        COLLECTED = 1,  
    }
    public class BoMongQuest_Quest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Progress { get; set; }
        public long MaxProgress { get;set; }
        public BoMongQuest_Status Quest_Status { get; set; }
        public BoMongQuest_RewardStatus Reward_Status { get; set; }
        public int Reward { get; set; }
        public bool IsFinish()
        {
            return Quest_Status is BoMongQuest_Status.FINISHED || Progress >= MaxProgress;
        }
        public bool IsReward()
        {
            return Reward_Status is BoMongQuest_RewardStatus.COLLECTED;
        }
    }
}
