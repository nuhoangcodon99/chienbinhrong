using NgocRongGold.Application.Constants;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.Model.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.NamecBattlefield
{
    public class NamecBattlefield_Server
    {
        public Task Task { get; set; }
        public long Time { get; set; }
        public NamecBattlefield_Info Cadic = new NamecBattlefield_Info() { TeamId = 1};
        public NamecBattlefield_Info Fide = new NamecBattlefield_Info() { TeamId = 2};

        public List<int> All = new List<int>();
        public NamecBattlefield_Status Status { get; set; }
        public void Start()
        {
            new Thread(new ThreadStart(Runtime)).Start();
        }
        public void Runtime()
        {
            while (Server.Gi().IsRunning)
            {
                var timeServer = ServerUtils.CurrentTimeMillis();
                switch (Status) 
                {
                    case NamecBattlefield_Status.CLOSE:
                        if (CanRegister())
                        {
                            Status = NamecBattlefield_Status.REGISTER;
                            All.Clear();
                            Cadic.Clear();
                            Fide.Clear();
                        }
                        break;
                    case NamecBattlefield_Status.REGISTER:
                        if (CanOpen())
                        {
                            if (Cadic.Characters.Count < 1 || Fide.Characters.Count < 1)
                            {
                                Status = NamecBattlefield_Status.NOT_ENOUGH_CHARACTER;
                                break;
                            }
                            foreach (var id in Cadic.Characters)
                            {
                                var Client = (Model.Character.Character)ClientManager.Gi().GetCharacter(id);
                                if (Client != null)
                                {
                                    Client.InfoChar.X = 248;
                                    Client.InfoChar.Y = 144;
                                    Client.Flag = 1;
                                    MapManager.JoinMap((Character)Client, 164, 0, false, false, 0);
                                    Client.CharacterHandler.SendMessage(NamecBattlefield_Service.newInfoPhuBan(164, "Ca đíc", "Fide", 7, 900, 7));
                                    Client.DataNamecBattlefield.Status = NamecBattlefield_Character_Status.FIGHTING;
                                    Client.DataNamecBattlefield.TeamId = 1;
                                }
                            }
                            foreach (var id in Fide.Characters)
                            {
                                var Client = (Model.Character.Character)ClientManager.Gi().GetCharacter(id);
                                if (Client != null)
                                {
                                    Client.InfoChar.X = 2218;
                                    Client.InfoChar.Y = 600;
                                    Client.Flag = 2;
                                    MapManager.JoinMap((Character)Client, 164, 0, false, false, 0);
                                    Client.CharacterHandler.SendMessage(NamecBattlefield_Service.newInfoPhuBan(164, "Ca đíc", "Fide", 7, 900, 7));
                                    Client.DataNamecBattlefield.Status = NamecBattlefield_Character_Status.FIGHTING;
                                    Client.DataNamecBattlefield.TeamId = 2;
                                }
                            }
                            Status = NamecBattlefield_Status.OPEN;
                            Time = DataCache._1MINUTES * 15 + timeServer;
                        }
                        break;
                    case NamecBattlefield_Status.OPEN:
                        if (CanClose())
                        {
                            Status = NamecBattlefield_Status.CLOSE;
                        }else if (Time < timeServer)
                        {
                            if (Cadic.Life == Fide.Life)
                            {
                                //NamecBattlefield_Handler.SendMessage(Cadic.Characters, NamecBattlefield_Service.AddEffectEnd(2));
                                //NamecBattlefield_Handler.SendMessage(Fide.Characters, NamecBattlefield_Service.AddEffectEnd(2));
                                foreach (var id in Cadic.Characters)
                                {
                                    var Client = (Model.Character.Character)ClientManager.Gi().GetCharacter(id);
                                    if (Client != null)
                                    {
                                        Client.CharacterHandler.SendMessage(NamecBattlefield_Service.AddEffectEnd(2));
                                        var point = (Client.DataNamecBattlefield.PointTemporary / 3) + 5;
                                        NamecBattlefield_Handler.GoBackAndGetPoint(Client, point);

                                    }
                                }
                                foreach (var id in Fide.Characters)
                                {
                                    var Client = (Model.Character.Character)ClientManager.Gi().GetCharacter(id);
                                    if (Client != null)
                                    {
                                        Client.CharacterHandler.SendMessage(NamecBattlefield_Service.AddEffectEnd(2));
                                        var point = (Client.DataNamecBattlefield.PointTemporary / 3) + 5;
                                        NamecBattlefield_Handler.GoBackAndGetPoint(Client, point);

                                    }
                                }
                            }
                            else if (Cadic.Life > Fide.Life)
                            {
                                //NamecBattlefield_Handler.SendMessage(Cadic.Characters, NamecBattlefield_Service.AddEffectEnd(1));
                                //NamecBattlefield_Handler.SendMessage(Fide.Characters, NamecBattlefield_Service.AddEffectEnd(0));
                                foreach (var id in Cadic.Characters)
                                {
                                    var Client = (Model.Character.Character)ClientManager.Gi().GetCharacter(id);
                                    if (Client != null)
                                    {
                                        Client.CharacterHandler.SendMessage(NamecBattlefield_Service.AddEffectEnd(1));
                                        var point = (Client.DataNamecBattlefield.PointTemporary) + 10;
                                        NamecBattlefield_Handler.GoBackAndGetPoint(Client, point);
                                    }
                                }
                                foreach (var id in Fide.Characters)
                                {
                                    var Client = (Model.Character.Character)ClientManager.Gi().GetCharacter(id);
                                    if (Client != null)
                                    {
                                        Client.CharacterHandler.SendMessage(NamecBattlefield_Service.AddEffectEnd(0));
                                        var point = (Client.DataNamecBattlefield.PointTemporary / 2) + 1;
                                        NamecBattlefield_Handler.GoBackAndGetPoint(Client, point);
                                    }
                                }
                            }
                            else
                            {
                                foreach (var id in Cadic.Characters)
                                {
                                    var Client = (Model.Character.Character)ClientManager.Gi().GetCharacter(id);
                                    if (Client != null)
                                    {
                                        Client.CharacterHandler.SendMessage(NamecBattlefield_Service.AddEffectEnd(0));
                                        var point = (Client.DataNamecBattlefield.PointTemporary / 2) + 1;
                                        NamecBattlefield_Handler.GoBackAndGetPoint(Client, point);

                                    }
                                }
                                foreach (var id in Fide.Characters)
                                {
                                    var Client = (Model.Character.Character)ClientManager.Gi().GetCharacter(id);
                                    if (Client != null)
                                    {
                                        Client.CharacterHandler.SendMessage(NamecBattlefield_Service.AddEffectEnd(1));
                                        var point = (Client.DataNamecBattlefield.PointTemporary) + 10;
                                        NamecBattlefield_Handler.GoBackAndGetPoint(Client, point);
                                    }
                                }

                            }
                            Status = NamecBattlefield_Status.CLOSE;
                        }
                        break;
                    case NamecBattlefield_Status.NOT_ENOUGH_CHARACTER:
                        //for (int i = 0; i < All.Count; i++)
                        //{
                        //    var character = ClientManager.Gi().GetCharacter(All[i]);
                        //    if (character != null)
                        //    {
                        //        character.CharacterHandler.SendMessage(Service.ServerMessage("Trận đấu đã bị huỷ do không đủ người chơi tham gia"));
                        //    }
                        //}
                        NamecBattlefield_Handler.SendMessage(All, Service.ServerMessage("Trận đấu đã bị huỷ do không đủ người chơi tham gia"));
                        Status = NamecBattlefield_Status.CLOSE;
                        break;
                }
                Thread.Sleep(1000);
            }
        }
        //public NamecBattlefield_Info SelectWon()
        //{
        //    return Cadic.Life >= Fide.Life ? AllTeam : Cadic.Life > Fide.Life ? Cadic : Fide;
        //}
        public bool CanAction()
        {
            return Status is NamecBattlefield_Status.REGISTER or NamecBattlefield_Status.OPEN;
        }
        public bool CanRegister()
        {
            var now = ServerUtils.TimeNow();
            return (now.DayOfWeek is (DayOfWeek.Tuesday or DayOfWeek.Thursday or DayOfWeek.Saturday)) && (now.Hour is 18) && (now.Minute >= 25);
        }
        public bool CanOpen()
        {
            var now = ServerUtils.TimeNow();
            return (now.DayOfWeek is (DayOfWeek.Tuesday or DayOfWeek.Thursday or DayOfWeek.Saturday)) && (now.Hour is 18) && (now.Minute > 35) ;
        }
        public bool CanClose()
        {
            var now = ServerUtils.TimeNow();
            return (now.DayOfWeek is (DayOfWeek.Tuesday or DayOfWeek.Thursday or DayOfWeek.Saturday)) && (now.Hour != 18);
        }
    }
}
