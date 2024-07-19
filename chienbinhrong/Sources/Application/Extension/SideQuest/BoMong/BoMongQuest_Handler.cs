using NgocRongGold.Application.Constants;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Model.Character;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.SideQuest.BoMong
{
    public class BoMongQuest_Handler
    {
        public static BoMongQuest_Handler instance;
        public static BoMongQuest_Handler gI()
        {
            if (instance == null) instance = new BoMongQuest_Handler();
            return instance;
        }
        public Message OpenHubTask(Character character)
        {
            
            var message = new Message(-76);
            message.Writer.WriteByte(0);
            message.Writer.WriteByte(character.DataBoMong.Quests.Count);
            character.DataBoMong.Load(character);
            foreach (var quest in character.DataBoMong.Quests)
            {
                message.Writer.WriteUTF(quest.Name);
                message.Writer.WriteUTF(string.Format(quest.Description, ServerUtils.GetMoney(quest.Progress), ServerUtils.GetMoney(quest.MaxProgress)));
                message.Writer.WriteShort(quest.Reward);
                message.Writer.WriteBoolean(quest.IsFinish());  
                message.Writer.WriteBoolean(quest.IsReward());
               

            }

            return message;
        }
        
        public BoMongQuest_Quest GetTask(Character character,int id)// GetTask(enum)
        {
            return character.DataBoMong.Quests[id] != null ? character.DataBoMong.Quests[id] : null;//return character.List.FirstOrDefault or List[id];
        }
        public void Reward(Character character,int id)
        {
            var task = GetTask(character, id);
            if (task is null) return;
            if (task.Reward_Status is BoMongQuest_RewardStatus.COLLECTED || task.Quest_Status is BoMongQuest_Status.UN_FINISHED) return;
            var item = ItemCache.GetItemDefault(1519);
            character.CharacterHandler.AddItemToBag(true, item);

            item = ItemCache.GetItemDefault(1178);
            character.CharacterHandler.AddItemToBag(true, item);
            character.CharacterHandler.SendMessage(Service.SendBag(character));
            //handle
        }
        public void PlusSubTask(Character character, BoMongQuest_Template template)
        {
            var task = GetTask(character, (int)template);
            if (task is null) return;
            if (task.Quest_Status is BoMongQuest_Status.FINISHED) return;
            if (task.Progress >= task.MaxProgress)
            {
                task.Quest_Status = BoMongQuest_Status.FINISHED;
            }
            else
            {
                task.Progress++;
            }
        }
    }
}
