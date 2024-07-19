using NgocRongGold.DatabaseManager;
using NgocRongGold.Application.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NgocRongGold.Model.Character;
using NgocRongGold.Application.Main;

namespace NgocRongGold.Application.Extension.SideQuest.HangNgay
{
    public class HangNgayQuest_Handler
    {
        public static void DoTask(Character character, int count, HangNgayQuest_Type type, int idObject)
        {
            if (character.DataSideTask.HaveTask() && character.DataSideTask.CheckType(type) && (idObject == character.DataSideTask.Object.Id))
            {
                
                var currentTask = character.DataSideTask.Quest;
                if (character.DataSideTask.Progress < currentTask.MaxProgress)
                {
                    character.DataSideTask.Progress = (character.DataSideTask.Progress + count > currentTask.MaxProgress ? currentTask.MaxProgress : character.DataSideTask.Progress + count);

                    if (character.DataSideTask.Progress >= currentTask.MaxProgress)

                         
                    {
                        character.DataSideTask.Status = HangNgayQuest_Status.FINISHED;
                        character.CharacterHandler.SendMessage(Service.ServerMessage($"Nhiệm vụ đã hoàn thành Nói chuyện với Bò Mộng để nhận thưởng"));
                    }
                    else
                    {
                        character.CharacterHandler.SendMessage(Service.ServerMessage($"Nhiệm vụ đã hoàn thành {character.DataSideTask.Progress}/{currentTask.MaxProgress}"));
                    }
                }
            }
        }
        public static void AccecptTask(Character character,HangNgayQuest_Difficult difficult)
        {
            int type = ServerUtils.RandomNumber(0, 2);

            var task = new HangNgayQuest_Quest()
            {
                Type = (HangNgayQuest_Type)type,
                Difficult = difficult,
            };
            character.DataSideTask.Quest = task;
            character.DataSideTask.Count--;
            character.DataSideTask.Progress = 0;
            character.DataSideTask.TimeAccecpt = ServerUtils.CurrentTimeMillis();
            character.DataSideTask.GetObj(character);
            character.DataSideTask.GetMaxProcess();
        }
        
    }
}
