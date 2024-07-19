using Newtonsoft.Json.Linq;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.Practice
{
    public class Practice_Handler
    {
        public static Practice_Handler Instance { get; set; }
        public static Practice_Handler gI()
        {
            if (Instance == null)Instance = new Practice_Handler();
            return Instance;
        }
        public void Challenge(Character character, Practice_Progress progress)
        {
            character.DataPractice.Status = Practice_Staus.CHALLENGE;
            var zone = MapManager.GetMapOffline(character.InfoChar.MapId).GetZoneById(character.Id);
            switch (progress)
            {
                case Practice_Progress.THAN_MEO_KARIN:
                    {
                        character.CharacterHandler.SendMessage(Service.HideNpc(18, true));
                        var meokarin = new Boss();
                        meokarin.CreateBoss(87, 204, 408);
                        meokarin.CharacterHandler.SetUpInfo();
                        zone.ZoneHandler.AddBoss(meokarin);
                    }
                    break;
                case Practice_Progress.YAJIRO:
                    {
                        character.CharacterHandler.SendMessage(Service.HideNpc(18, true));
                        var boss = character.Zone.ZoneHandler.GetBossInMap(88)[0];
                        boss.InfoChar.TypePk = 0;
                        boss.Zone.ZoneHandler.SendMessage(Service.ChangeTypePk(boss.Id, 0));
                        boss.CharacterHandler.BackHome();
                    }
                    break;
                case Practice_Progress.MR_POPO:
                    {
                        character.CharacterHandler.SendMessage(Service.HideNpc(19, true));
                        var boss = character.Zone.ZoneHandler.GetBossInMap(90)[0];
                        boss.InfoChar.TypePk = 0;
                        boss.Zone.ZoneHandler.SendMessage(Service.ChangeTypePk(boss.Id, 0));
                        boss.CharacterHandler.BackHome();
                    }
                    break;
                case Practice_Progress.THUONG_DE:
                    {
                        character.CharacterHandler.SendMessage(Service.HideNpc(19, true));
                        MapManager.GetMapOffline(49).JoinZone(character, character.Id);
                        var boss = character.Zone.ZoneHandler.GetBossInMap(92)[0];
                        boss.InfoChar.TypePk = 0;
                        boss.Zone.ZoneHandler.SendMessage(Service.ChangeTypePk(boss.Id, 0));
                        boss.CharacterHandler.BackHome();
                        //handle
                    }
                    break;
                case Practice_Progress.KHI_BUBBLES:
                    {
                        character.CharacterHandler.SendMessage(Service.HideNpc(20, true));
                        var boss = character.Zone.ZoneHandler.GetBossInMap(91)[0];
                        boss.InfoChar.TypePk = 0;
                        boss.Zone.ZoneHandler.SendMessage(Service.ChangeTypePk(boss.Id, 0));
                        boss.CharacterHandler.BackHome();
                    }
                    break;
                case Practice_Progress.KAIO:
                    {
                        character.CharacterHandler.SendMessage(Service.HideNpc(20, true));
                        var kario = new Boss();
                        kario.CreateBoss(92, 357, 432);
                        kario.CharacterHandler.SetUpInfo();
                        zone.ZoneHandler.AddBoss(kario);
                    }
                    break;
                case Practice_Progress.KAIOSHIN:
                    //handle
                    break;
            }
        }
        public void Practice(Character character, Practice_Progress progress)
        {
            character.DataPractice.Status = Practice_Staus.PRACTICE;
            character.DataPractice.Practice_Progress = progress;
            character.DataPractice.Potential = character.DataPractice.GetPotenial((int)progress);
            character.CharacterHandler.SendMessage(Service.ItemTimeWithMessage($"Luyện tập nhận {character.DataPractice.Potential} sau ", (int)TextTime.LUYEN_TAP, 60));
            var zone = MapManager.GetMapOffline(character.InfoChar.MapId).GetZoneById(character.Id);
            switch (progress)
            {
                case Practice_Progress.THAN_MEO_KARIN:
                    {
                        character.CharacterHandler.SendMessage(Service.HideNpc(18, true));
                        var meokarin = new Boss();
                        meokarin.CreateBoss(87, 204, 408);
                        meokarin.CharacterHandler.SetUpInfo();
                        zone.ZoneHandler.AddBoss(meokarin);
                    }
                    break;
                case Practice_Progress.YAJIRO:
                    {
                        character.CharacterHandler.SendMessage(Service.HideNpc(18, true));
                        var boss = character.Zone.ZoneHandler.GetBossInMap(88)[0];
                        boss.InfoChar.TypePk = 5;
                        boss.Zone.ZoneHandler.SendMessage(Service.ChangeTypePk(boss.Id, 5));
                    }
                    break;
                case Practice_Progress.MR_POPO:
                    {
                        character.CharacterHandler.SendMessage(Service.HideNpc(19, true));
                        var boss = character.Zone.ZoneHandler.GetBossInMap(90)[0];
                        boss.InfoChar.TypePk = 5;
                        boss.Zone.ZoneHandler.SendMessage(Service.ChangeTypePk(boss.Id, 5));
                    }
                    break;
                case Practice_Progress.THUONG_DE:
                    {
                        character.CharacterHandler.SendMessage(Service.HideNpc(19, true));
                        MapManager.GetMapOffline(49).JoinZone(character, character.Id);
                        var boss = character.Zone.ZoneHandler.GetBossInMap(89)[0];
                        boss.InfoChar.TypePk = 5;
                        boss.Zone.ZoneHandler.SendMessage(Service.ChangeTypePk(boss.Id, 5));
                        //handle
                    }
                    break;
                case Practice_Progress.KHI_BUBBLES:
                    {
                        character.CharacterHandler.SendMessage(Service.HideNpc(20, true));
                        var boss = character.Zone.ZoneHandler.GetBossInMap(91)[0];
                        boss.InfoChar.TypePk = 5;
                        boss.Zone.ZoneHandler.SendMessage(Service.ChangeTypePk(boss.Id, 5));
                    }   
                    break;
                case Practice_Progress.KAIO:
                    {
                        character.CharacterHandler.SendMessage(Service.HideNpc(20, true));
                        var kario = new Boss();
                        kario.CreateBoss(92, 357, 432);
                        kario.CharacterHandler.SetUpInfo();
                        zone.ZoneHandler.AddBoss(kario);
                    }
                    break;
                case Practice_Progress.KAIOSHIN:
                    //handle
                    break;
            }
        }
        public void Kill(Boss boss, Character character, int type)
        {
            Practice_Progress practice_Progress = character.DataPractice.Progress;
            if (character.DataPractice.Status is Practice_Staus.CHALLENGE && CanNextProgress(practice_Progress, boss))
            {
                character.DataPractice.Progress = (Practice_Progress)((int)practice_Progress++);
            }
            if (character.DataPractice.Status is Practice_Staus.PRACTICE)
            {
                EndPractice(character);
                practice_Progress = character.DataPractice.Practice_Progress;
            }
            switch(practice_Progress, boss.Type)
            {
                case (Practice_Progress.THAN_MEO_KARIN, 87):
                    character.CharacterHandler.SendMessage(Service.HideNpc(18, false));
                    break;
                case (Practice_Progress.YAJIRO, 88):
                    character.CharacterHandler.SendMessage(Service.HideNpc(18, false));
                    break;
                case (Practice_Progress.MR_POPO, 90):
                    
                    character.CharacterHandler.SendMessage(Service.HideNpc(19, false));
                    
                    break;
                case (Practice_Progress.THUONG_DE, 89):
                    character.CharacterHandler.SendMessage(Service.HideNpc(19, false));
                    break;
                case (Practice_Progress.KHI_BUBBLES, 91):
                    character.CharacterHandler.SendMessage(Service.HideNpc(20, false));
                    break;
                case (Practice_Progress.KAIO, 92):
                    character.CharacterHandler.SendMessage(Service.HideNpc(20, false));
                    break;
                case (Practice_Progress.KAIOSHIN, 0):
                    break;
            }
        }
        public bool CanNextProgress(Practice_Progress practice_Progress, Boss boss)
        {
            switch (practice_Progress, boss.Type)
            {
                case (Practice_Progress.THAN_MEO_KARIN, 87):
                case (Practice_Progress.YAJIRO, 88):
                case (Practice_Progress.MR_POPO, 90):
                case (Practice_Progress.THUONG_DE, 89):
                case (Practice_Progress.KHI_BUBBLES, 91):
                case (Practice_Progress.KAIO, 92):
                case (Practice_Progress.KAIOSHIN, 0):
                    return true;
                default:
                    return false;
            }
        }
        public void EndPractice(Character character)
        {
            character.DataPractice.Status = Practice_Staus.NORMAL;
            character.CharacterHandler.SendMessage(Service.ItemTimeWithMessage($"Luyện tập nhận {character.DataPractice.Potential} sau ", (int)TextTime.LUYEN_TAP, 0));
            character.CharacterHandler.SendMessage(Service.ServerMessage("Luyện tập hoàn thành"));

        }
        public void Killed(Boss boss,Character character, Practice_Progress practice_Progress)
        {
            switch (practice_Progress)
            {
                case Practice_Progress.THAN_MEO_KARIN:
                    character.CharacterHandler.SendMessage(Service.HideNpc(18, false));
                    break;
                case Practice_Progress.YAJIRO:
                    character.CharacterHandler.SendMessage(Service.HideNpc(18, false));
                    boss.InfoChar.TypePk = 0;
                    boss.Zone.ZoneHandler.SendMessage(Service.ChangeTypePk(boss.Id, 0));
                    boss.CharacterHandler.BackHome();
                    SetPosBoss(boss, 300, 408);
                    break;
                case Practice_Progress.MR_POPO:
                    character.CharacterHandler.SendMessage(Service.HideNpc(19, false));
                    boss.InfoChar.TypePk = 0;
                    boss.Zone.ZoneHandler.SendMessage(Service.ChangeTypePk(boss.Id, 0));
                    boss.CharacterHandler.BackHome();
                    SetPosBoss(boss, 301, 408);
                    break;
                case Practice_Progress.THUONG_DE:
                    character.CharacterHandler.SendMessage(Service.HideNpc(19, false));
                    boss.InfoChar.TypePk = 0;
                    boss.Zone.ZoneHandler.SendMessage(Service.ChangeTypePk(boss.Id, 0));
                    boss.CharacterHandler.BackHome();
                    SetPosBoss(boss, 829, 456);
                    break;
                case Practice_Progress.KHI_BUBBLES:
                    character.CharacterHandler.SendMessage(Service.HideNpc(20, false));
                    boss.InfoChar.TypePk = 0;
                    boss.Zone.ZoneHandler.SendMessage(Service.ChangeTypePk(boss.Id, 0));
                    boss.CharacterHandler.BackHome();
                    SetPosBoss(boss, 315, 240);
                    break;
                case Practice_Progress.KAIO:
                    character.CharacterHandler.SendMessage(Service.HideNpc(20, false));
                    break;
                case Practice_Progress.KAIOSHIN:
                    break;
            }
        }
        public void SetPosBoss(Boss boss,int x, int y)
        {
            boss.InfoChar.X = (short)x;
            boss.InfoChar.Y = (short)y;
            boss.Zone.ZoneHandler.SendMessage(Service.SendPos(boss, 0));
        }
        public void UpdatePractice(Character character, long timeServer)
        {
            var dataPractice = character.DataPractice;
            if (timeServer - dataPractice.Time > 60000)
            {
                var Potential = dataPractice.Potential;
                dataPractice.Time = timeServer;
                UpPotential(character,Potential);
                character.CharacterHandler.SendMessage(Service.ItemTimeWithMessage($"Luyện tập nhận {character.DataPractice.Potential} sau ", (int)TextTime.LUYEN_TAP, 60));
            }
        }
        public void UpPotential(Character character, long Potential)
        {
            character.CharacterHandler.PlusPotential(Potential);
            character.CharacterHandler.PlusPower(Potential);
            character.CharacterHandler.SendMessage(Service.UpdateExp(2, Potential));
        }
        public void Training(Character character, int Minutes)
        {
            try { 
            if (Minutes < 30 && character.DataPractice.TrainStatus !=  AutoTrain_Status.AUTO_TRAIN) return;

                switch (character.InfoChar.MapId)
                {
                    case 45 or 46 or 47 or 50 or 111 or 116 or 48 or 49:
                        {
                            var Potential = character.DataPractice.GetPotenial() * Minutes;
                            UpPotential(character, Potential);
                            character.CharacterHandler.SendMessage(Service.OpenUiSay(5, $"Bạn tăng được {ServerUtils.GetMoneys(Potential)} sức mạnh trong thời gian {Minutes}' tập luyện Offline"));
                        }
                        break;
                    case int i when DataCache.IdMapKarin.Contains(i):
                        {

                            if (character.AllDiamondLock() < 1)
                            {
                                break;
                            }
                            character.MineDiamond(1, 2);

                            var Potential = character.DataPractice.GetPotenial() * Minutes;
                            UpPotential(character, Potential);
                            character.DataPractice.MapOldId = character.InfoChar.MapId;
                            MapManager.Get(character.DataPractice.MapOldId).OutZone(character);
                            character.Zone.ZoneHandler.SendMessage(Service.SendTeleport(character.Id, character.TypeTeleport));
                            MapManager.GetMapOffline(character.DataPractice.MapPracticeId).JoinZone(character, 0, true, true);
                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(19, $"Bạn tăng được {ServerUtils.GetMoneys(Potential)} sức mạnh trong thời gian {Minutes}' tập luyện Offline", new List<string> { "Ở\nLại đây", "Về\nChỗ cũ" }, character.InfoChar.Gender));
                        }
                        break;

                    default:
                        {
                            if (character.AllDiamondLock() < 1)
                            {
                                break;
                            }
                            character.MineDiamond(1, 2);
                            var Potential = character.DataPractice.GetPotenial() * Minutes;
                            UpPotential(character, Potential);
                            character.DataPractice.MapOldId = character.InfoChar.MapId;
                            MapManager.Get(character.DataPractice.MapOldId).OutZone(character);

                            character.Zone.ZoneHandler.SendMessage(Service.SendTeleport(character.Id, character.TypeTeleport));
                            MapManager.GetMapOffline(character.DataPractice.MapPracticeId).JoinZone(character, 0, true, true);

                            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(19, $"Bạn tăng được {ServerUtils.GetMoneys(Potential)} sức mạnh trong thời gian {Minutes}' tập luyện Offline", new List<string> { "Ở\nLại đây", "Về\nChỗ cũ" }, character.InfoChar.Gender));

                        }
                        break;
                }
            }catch(Exception e)
            {

            }
        }

    }
}
