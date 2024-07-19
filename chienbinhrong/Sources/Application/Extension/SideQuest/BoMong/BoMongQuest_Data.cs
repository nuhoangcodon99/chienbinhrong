using NgocRongGold.Application.IO;
using NgocRongGold.Application.Manager;
using NgocRongGold.DatabaseManager;
using NgocRongGold.DatabaseManager.Player;
using NgocRongGold.Model.Character;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.SideQuest.BoMong
{
    public class BoMongQuest_Data
    {
        public List<BoMongQuest_Quest> Quests { get; set; }
        public BoMongQuest_Data()
        {
            Quests = new List<BoMongQuest_Quest>();
            //Create();
        }
        public void Create()
        {
            foreach (var task in Cache.Gi().TASK_BO_MONG.Values.ToList())
            {
                Quests.Add(new BoMongQuest_Quest()
                {
                    Id = task.Id,
                    Description = task.TaskDescription,
                    Name = task.TaskName,
                    Progress = 0,
                    MaxProgress = task.Count,
                    Reward_Status = BoMongQuest_RewardStatus.UN_COLLECTED,
                    Quest_Status = BoMongQuest_Status.UN_FINISHED,
                    Reward = task.GemCollect,
                });
            }
        }
        public void Load(Character character)
        {
            var booleanClan = ClanManager.Get(character.ClanId) != null;

            character.DataBoMong.Quests[0].Progress = character.InfoChar.Power;
            character.DataBoMong.Quests[1].Progress = character.InfoChar.Power;
            character.DataBoMong.Quests[2].Progress = MagicTreeManager.Get(character.Id)?.Level ?? 0;
            character.DataBoMong.Quests[9].Progress = (long)(ServerUtils.TimeNow() - CharacterDB.GetCreateDateById(character.Id)).TotalHours;

            if (booleanClan)
            {
                var clanManager = ClanManager.Get(character.ClanId);
                var clanHandler = clanManager?.ClanHandler;
                character.DataBoMong.Quests[10].Progress = clanHandler?.GetMember(character.Id)?.Cho_đậu ?? 0;
            }

            character.DataBoMong.Quests[16].Progress = character.InfoChar.Power;
        }
        public void Dispose()
        {
            foreach (var task in Quests)
            {
                GC.SuppressFinalize(task);

            }
            GC.SuppressFinalize(this);
        }
    }
}
