using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Threading;
using NgocRongGold.Model.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.Practice.Whis
{
    public class Whis_Practice
    {
        public static Whis_Practice instance;
        public static Whis_Practice gI()
        {
            if (instance == null) instance = new Whis_Practice();
            return instance;
        }
        public void Kill(Character character)
        {
            double totalTimePractice = ServerUtils.CurrentTimeMillis() - character.DataPractice.Whis.Time;
            character.DataPractice.Whis.Level += character.DataPractice.Whis.Level < 100 ? 10 : 1;
            if (character.DataPractice.Whis.HighScore > totalTimePractice || character.DataPractice.Whis.HighScore is 0)
            {
                character.DataPractice.Whis.HighScore = Math.Round((double)(totalTimePractice/1000), 2);
                character.DataPractice.Whis.TimeSetScore = ServerUtils.CurrentTimeMillis();
            }
            character.CharacterHandler.SendMessage(Service.HideNpc(56, false));
        }
        public void Killed(Character character)
        {
            character.Zone.ZoneHandler.BossInMap()[0].CharacterHandler.SendDie();
            character.DataPractice.Whis.Status = Whis_Status.DIED;
            character.CharacterHandler.LeaveFromDead(true);
            character.CharacterHandler.SendMessage(Service.HideNpc(56, false));
        }
        public void Practice(Character character, int level)
        {
            //  character.DataTraining.DataWhis.Count++;
            character.DataPractice.Whis.Time = ServerUtils.CurrentTimeMillis();
            character.CharacterHandler.SendMessage(Service.HideNpc(56, true));
            var boss = new Boss();
            boss.CreateBoss(113, 339, 360);
            boss.InfoChar.OriginalHp = boss.InfoChar.Hp = 550000 * level;
            boss.InfoChar.OriginalMp = boss.InfoChar.Mp = 550000 * level;
            boss.InfoChar.OriginalDamage = 1000 * level;
            boss.CharacterHandler.SetUpInfo();
            character.Zone.ZoneHandler.AddBoss(boss);
            character.InfoChar.X = 493;
            character.InfoChar.Y = 360;
            character.CharacterHandler.SendMessage(Service.SendPos(character, 0));
        }
    }
}
