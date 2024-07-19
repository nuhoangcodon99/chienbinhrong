using MySql.Data.MySqlClient.Authentication;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Extension.Bosses.SoiHecQuyn;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.MainTasks;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Info.Radar;
using NgocRongGold.Model.Map;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.DaiHoiVoThuat
{

    public class ChampionShip
    {
        
        public readonly List<string> TextChampionShip = new List<string>() { "Không có", "Nhi đồng", "Siêu cấp 1", "Siêu cấp 2", "Siêu cấp 3", "Ngoại hạng" };
        public List<int> PlayersRegister = new List<int>();
        public List<int> PlayersFighting = new List<int>();
        public Dictionary<int, List<int>> PlayersVerus = new Dictionary<int, List<int>>();
        public Dictionary<int, List<string>> PlayersVerusName = new Dictionary<int, List<string>>();
        public List<int> PlayersLucky = new List<int>();
        public int TypeChampionShip = 0;
        public int Round = 0;
        public ChampionStatus Status = ChampionStatus.END;

        public long CurrentDelay = ServerUtils.CurrentTimeMillis();
        public long DelaySendThongBao = ServerUtils.CurrentTimeMillis();
        public void OutputInConsole(string status)
        {
            Server.Gi().Logger.Print("ChampionShip Set Status [" + Status.ToString() + "][" + status + "]", "red");
        }
        public void Register(Character character)
        {

            if (!CanRegister(character, character.InfoChar.Power) || !CanRegister(character, character.InfoChar.TotalPotential))
            { // vuot qua nguong suc manh thi dau
                character.CharacterHandler.SendMessage(Service.ServerMessage("Sức mạnh của bạn đã vượt ngưỡng sức mạnh giải đấu, vui lòng tham gia giải đấu khác"));
                return;
            }
            if (Exist(character.Id))// da dang ky roi
            {
                character.CharacterHandler.SendMessage(Service.ServerMessage("Tên của bạn đã có trong danh sách thí sinh tham gia thi đấu"));
                return;
            }
            if (!CheckStatus(ChampionStatus.WAIT_REGISTER))// khong the dang ky nua vi het thoi gian
            {
                character.CharacterHandler.SendMessage(Service.ServerMessage("Giải đấu đang diễn ra, đã hết thời gian đăng ký, vui lòng quay trở lại sau"));
                return;
            }
            if (!Paid(character))
            {// khong du tien de dang ky giai
                character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn không có đủ chi phí để đăng ký tham gia thi đấu"));
                return;
            }
            PlayersRegister.Add(character.Id);
            character.CharacterHandler.SendMessage(Service.ServerMessage($"Chúc mừng bạn đã đăng ký thành công, vui lòng có mặt tại đây lúc {ServerUtils.TimeNow().Hour}h30"));
        }
        public bool Exist(int idPlayer)
        {
            return PlayersRegister.Contains(idPlayer);
        }
        public bool CheckStatus(ChampionStatus status)
        {
            return Status == status;
        }
        public bool Paid(Character character)
        {
            switch (TypeChampionShip)
            {
                case 5:
                    if (character.AllDiamond() < GetCostChampionShip())
                    {
                        return false;
                    }
                    character.MineDiamond(GetCostChampionShip());
                    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                    return true;
                case 1:
                case 2:
                case 3:
                case 4:
                    if (character.InfoChar.Gold < GetCostChampionShip())
                    {
                        return false;
                    }
                    character.MineGold(GetCostChampionShip());
                    character.CharacterHandler.SendMessage(Service.BuyItem(character));
                    return true;
                default:
                    return false;
            }
        }
        public bool CanRegister(Character character, long value)
        {
            return (value >= 1_500_000 * Math.Pow(TypeChampionShip, 10)) || TypeChampionShip == 5;
        }
        public void PairPlayers()
        {
            int team = 0;
            int player = 1;
            List<int> validIndexes = new List<int>();

            // Collect valid indexes first
            for (int i = 0; i < PlayersFighting.Count; i++)
            {
                var character = (Model.Character.Character)ClientManager.Gi().GetCharacter(PlayersFighting[i]);
                if (character != null)
                {
                    validIndexes.Add(i);
                }
            }

            // Pair players using valid indexes
            for (int i = 0; i < validIndexes.Count; i++)
            {
                var index = validIndexes[i];
                var character = (Model.Character.Character)ClientManager.Gi().GetCharacter(PlayersFighting[index]);

                if (i % 2 == 0)
                {
                    if (i == validIndexes.Count - 1)
                    {
                        PlayersLucky.Add(PlayersFighting[index]);
                    }
                    else
                    {
                        InitializePlayer(character, team, player, index, validIndexes[i + 1]);
                        player++;
                    }
                }
                else
                {
                    InitializeOpponent(character, team, player, index, validIndexes[i - 1]);
                    player = 1;
                    team++;
                }
            }
        }

        private void InitializePlayer(Character character, int team, int player, int index, int enemyIndex)
        {
            PlayersVerusName.Add(team, new List<string> { character.Name });
            PlayersVerus.Add(team, new List<int> { PlayersFighting[index] });

            character.DataDaiHoiVoThuat.Posistion = player;
            character.DataDaiHoiVoThuat.TeamId = team;

            character.DataDaiHoiVoThuat.Enemy = PlayersFighting[enemyIndex];
        }

        private void InitializeOpponent(Character character, int team, int player, int index, int enemyIndex)
        {
            PlayersVerus[team].Add(PlayersFighting[index]);
            PlayersVerusName[team].Add(character.Name);

            character.DataDaiHoiVoThuat.Posistion = player;
            character.DataDaiHoiVoThuat.TeamId = team;
            character.DataDaiHoiVoThuat.Enemy = PlayersFighting[enemyIndex];
        }


        public void Fighting()
        {
            for (int i = 0; i < PlayersFighting.Count; i++)
            {
                var id = PlayersFighting[i];
                var character = (Model.Character.Character)ClientManager.Gi().GetCharacter(id);
                if (character == null)
                {
                    PlayersFighting.Remove(id);
                }else if (character.InfoChar.MapId != 52)
                {
                    Failed(character, "Bạn đã thất bại vì không có mặt ở Đại Hội Võ Thuật");
                    PlayersFighting.Remove(id);
                    if (id % 2 == 0)
                    {
                        PlayersLucky.Add(PlayersFighting[i + 1]);
                    }
                    else
                    {
                        PlayersLucky.Add(PlayersFighting[i - 1]);
                    }
                }
                else
                {
                    Teleport(character, 113, character.DataDaiHoiVoThuat.TeamId, character.DataDaiHoiVoThuat.Posistion);
                    character.DataDaiHoiVoThuat.Status = ChampionerCS23_Status.FIGHITING;
                }
            }
            
        }
        public void SetTypePk(Character character, sbyte typePk)
        {
            character.InfoChar.TypePk = typePk;
            character.CharacterHandler.SendZoneMessage(Service.ChangeTypePk(character.Id, typePk));
        }
        public void Teleport(Character character, int idMap, int zone, int player)
        {
            var @char = (Character)character;
            if (player == 1)
            {
                character.InfoChar.X = 242;
                character.InfoChar.Y = 264;
                MapManager.JoinMap(@char, idMap, zone, false, false, 0);
            }
            else if (player == 2)
            {
                character.InfoChar.X = 500;
                character.InfoChar.Y = 264;
                MapManager.JoinMap(@char, idMap, zone, false, false, 0);
            }
            else
            {
                MapManager.JoinMap(@char, idMap, zone, false, false, 0);
            }
            character.DataDaiHoiVoThuat.Posistion = player;

        }
        public void SetPosistion(Character character, int posistion)
        {
            switch (posistion)
            {
                case 1:
                    character.InfoChar.X = 242;
                    character.InfoChar.Y = 264;
                    break;
                case 2:
                    character.InfoChar.X = 500;
                    character.InfoChar.Y = 264;
                    break;
            }
            character.CharacterHandler.SendZoneMessage(Service.SendPos(character, 1));
        }

        public Task Runtime { get; set; }
        public static ChampionShip instance;
        public static ChampionShip gI()
        {
            if (instance == null)
            {
                instance = new ChampionShip();
            }
            return instance;
        }
        public string GetNameChampionShip()
        {
            return TextChampionShip[TypeChampionShip];
        }
        public int GetCostChampionShip()
        {
            switch (TypeChampionShip)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    return 2 * TypeChampionShip;
                default:
                    return 10000;
            }
        }
        public void Start()
        {
            new Thread(new ThreadStart(InitChampionShip)).Start();
        }
        public void InitChampionShip()
        {
            while (Server.Gi().IsRunning)
            {
                try {
                    var timeserver = ServerUtils.CurrentTimeMillis();
                    switch (Status)
                    {
                        case ChampionStatus.WAIT_REGISTER:
                            if (timeserver > CurrentDelay)
                            {
                                PlayersFighting.Clear();
                                if (PlayersRegister.Count > 1)
                                {
                                    foreach (var id in PlayersRegister)
                                    {
                                        var PlayerRegister = ClientManager.Gi().GetCharacter(id);
                                        if (PlayerRegister != null)
                                        {
                                            PlayersFighting.Add(PlayerRegister.Id);
                                        }
                                    }
                                    PlayersRegister.Clear();
                                    PlayersVerus = new Dictionary<int, List<int>>(); 
                                    PairPlayers();  
                                    OutputInConsole($"[READY][FIGHTING:{PlayersFighting.Count}][VERUS: {PlayersVerus.Count}]");
                                    Status = ChampionStatus.READY;
                                    CurrentDelay = DataCache._1MINUTES + timeserver;
                                }
                                else if (PlayersRegister.Count == 1)
                                {
                                    var PlayerRegister = ClientManager.Gi().GetCharacter(PlayersRegister[0]);
                                    if (PlayerRegister != null)
                                    {
                                        PlayerRegister.CharacterHandler.SendMessage(Service.ServerMessage("Giải đấu đã bị hủy do không đủ người tham gia"));
                                    }
                                    PlayersRegister.Remove(PlayersRegister[0]);
                                    Status = ChampionStatus.END;
                                }
                                else
                                {
                                    Status = ChampionStatus.END;
                                    OutputInConsole($"[END][NO REASON]");

                                }
                            }
                            break;
                        case ChampionStatus.READY:
                            if (timeserver > DelaySendThongBao)
                            {
                                DelaySendThongBao = DataCache._1SECOND * 5 + timeserver;
                                var timeSecond = (CurrentDelay - timeserver) / 1000;
                                if (PlayersFighting.Count != 1)
                                {
                                    foreach (var id in PlayersFighting)
                                    {
                                        var PlayerFighting = ClientManager.Gi().GetCharacter(id);
                                        if (PlayerFighting != null)
                                        {
                                            PlayerFighting.CharacterHandler.SendMessage(Service.ServerMessage("Trận đấu của bạn sẽ diễn ra trong " + timeSecond + "s nữa"));
                                        }
                                    }
                                }
                                else
                                {
                                    var PlayerWinner = (Model.Character.Character)ClientManager.Gi().GetCharacter(PlayersFighting[0]);
                                    if (PlayerWinner != null)
                                    {
                                        Winner(PlayerWinner);
                                    }
                                    PlayersFighting.Remove(PlayersFighting[0]);
                                    Status = ChampionStatus.END;
                                }
                                OutputInConsole($"[SEND MESSAGE][READY {timeSecond}s]");
                                //gui thong bao cho toan bo nhan vat dang ki
                            }
                            if (timeserver > CurrentDelay)
                            {
                                HandleSummonTrongTai();
                                Status = ChampionStatus.READY_FIGHT;
                                Fighting();
                                CurrentDelay = DataCache._1SECOND * 20 + timeserver;
                                //xep cap va dich chuyen
                            }
                            break;
                        case ChampionStatus.READY_FIGHT:
                            {
                                HandleTrongTaiChat();
                                HandleFightingPlayer();
                                CurrentDelay = DataCache._1MINUTES * 3 + timeserver;
                                Status = ChampionStatus.FIGHTING;
                            } 
                            break;
                        case ChampionStatus.FIGHTING:
                            if (timeserver > CurrentDelay)
                            {
                                //handle chua danh nhau xong
                                Finish();
                                OutputInConsole("FIGHTING END");
                                Status = ChampionStatus.READY;
                                CurrentDelay = DataCache._1MINUTES + timeserver;
                                break;
                            }
                            //update
                            Update();
                            break;
                        case ChampionStatus.END:
                            if (SetType())
                            {
                                var now = ServerUtils.TimeNow();
                                Round = 0;
                                Status = ChampionStatus.WAIT_REGISTER;
                                 CurrentDelay = DataCache._1HOUR / 2 + timeserver;
                                //CurrentDelay = 30000 + timeserver;
                                OutputInConsole("SetType new");
                            }
                            break;
                    }
                    Thread.Sleep(1000);
                }catch(Exception e)
                {
                    Server.Gi().Logger.Print("Error SuperChampion " + e.Message + "\n" + e.StackTrace);
                }
                }
            OutputInConsole("Close Success !!!");
        }
        public void HandleTrongTaiChat()
        {
            async void Action()
            {
                await Task.Delay(5000);
                var map = MapManager.Get(113);
                map.Zones.ForEach(zone =>
                {
                    if (zone.Characters.Count < 2) { return; };
                    var trongtai = zone.ZoneHandler.BossInMap()[0];
                    zone.ZoneHandler.SendMessage(Service.PublicChat(trongtai.Id, $"Trận đấu giữa {PlayersVerusName[zone.Id][0]} và {PlayersVerusName[zone.Id][1]} sắp diễn ra"));
                });
                await Task.Delay(5000);
                map.Zones.ForEach(zone =>
                {
                    if (zone.Characters.Count < 2) { return; };
                    var trongtai = zone.ZoneHandler.BossInMap()[0];
                    zone.ZoneHandler.SendMessage(Service.PublicChat(trongtai.Id, $"Xin quý vị khán giả cho 1 tràng pháo tay cỗ vũ cho 2 đớ thủ nào"));
                });
                await Task.Delay(1000);
                map.Zones.ForEach(zone =>
                {
                    if (zone.Characters.Count < 2) { return; };
                    var trongtai = zone.ZoneHandler.BossInMap()[0];
                    zone.ZoneHandler.SendMessage(Service.PublicChat(trongtai.Id, $"Mọi người hãy ổn định chỗ ngồi, trận đấu sẽ bắt đầu sau 3 giây nữa"));
                });
                await Task.Delay(1000);
                map.Zones.ForEach(zone => { if (zone.Characters.Count < 2) { return; }; var trongtai = zone.ZoneHandler.BossInMap()[0]; zone.ZoneHandler.SendMessage(Service.PublicChat(trongtai.Id, $"3")); });
                await Task.Delay(1000);
                map.Zones.ForEach(zone => { if (zone.Characters.Count < 2) { return; }; var trongtai = zone.ZoneHandler.BossInMap()[0]; zone.ZoneHandler.SendMessage(Service.PublicChat(trongtai.Id, $"2")); });
                await Task.Delay(1000);
                map.Zones.ForEach(zone => { if (zone.Characters.Count < 2) { return; }; var trongtai = zone.ZoneHandler.BossInMap()[0]; zone.ZoneHandler.SendMessage(Service.PublicChat(trongtai.Id, $"1")); });
            }
            var act = new Task(Action);
            act.Start();
        }
        public void HandleSummonTrongTai()
        {
            var map = MapManager.Get(113);
            for (int i = 0; i < map.TileMap.ZoneNumbers; i++)
            {
                if (map.Zones[i].Bosses.Count >= 1) break;
                var trongtai = new Boss();
                trongtai.CreateBossNoAttack(82, 387, 264);
                trongtai.CharacterHandler = new TrongTaiHandler(trongtai);
                trongtai.CharacterHandler.SetUpInfo();
                map.Zones[i].ZoneHandler.AddBoss(trongtai);
            }
        }
        public void KillPlayer(Character character)
        {
            lock (PlayersFighting)
            {
                var idPlayerVerus = character.DataDaiHoiVoThuat.Enemy;
                var playerVerus = (Model.Character.Character)ClientManager.Gi().GetCharacter(idPlayerVerus);
                if (playerVerus != null)
                {
                    Failed(playerVerus, Reason.FAIL_BECAUSE_HAS_KILLED);
                }
                WinRound(character, Reason.WIN_BECAUSE_ENEMY_DEAD);
                PlayersFighting.Remove(idPlayerVerus);
            }
        }
        public void HandleFightingPlayer()
        {
            for (int i = 0; i < PlayersFighting.Count; i++)
            {
                var id = PlayersFighting[i];
                var character = (Model.Character.Character)ClientManager.Gi().GetCharacter(id);
                if (character == null)
                {
                    //nếu người chơi không online
                    continue;
                }
                SetTypePk(character, 3);
                SetPosistion(character, character.DataDaiHoiVoThuat.Posistion);
            }
        }
        public void Finish()
        {
            
                Round++;
            for (int i = 0; i < PlayersFighting.Count; i++)
            {
                var character = (Model.Character.Character)ClientManager.Gi().GetCharacter(PlayersFighting[i]);
                if (character == null) continue;
                else if (IndexPair(i) >= PlayersFighting.Count)
                {
                    WinRound(character, Reason.WIN_BECAUSE_ENEMY_DEAD);
                    break;
                }
                var player = (Model.Character.Character)ClientManager.Gi().GetCharacter(PlayersFighting[IndexPair(i)]);
                if (player == null)
                {
                    WinRound(character, Reason.WIN_BECAUSE_ENEMY_DEAD);
                    PlayersFighting.Remove(PlayersFighting[IndexPair(i)]);
                }
                else if (character.InfoChar.Hp >= player.InfoChar.Hp)
                {
                    Failed(player, Reason.FAIL_BECAUSE_HAS_KILLED);
                    WinRound(character, Reason.WIN_BECAUSE_ENEMY_DEAD);
                    PlayersFighting.Remove(player.Id);
                }
            }
            PlayersVerus.Clear();
            PlayersVerusName.Clear();
            PairPlayers();
                //for (int i = 0; i < PlayersVerus.Count; i++)
                //{
                //    for (int j = 0; j < PlayersVerus[i].Count; j++)
                //    {
                //        var player = ClientManager.Gi().GetCharacter(PlayersVerus[i][j]);
                //        if (player == null)
                //        {
                //            //handle khi player out game giua tran
                //            PlayersFighting.Remove(PlayersVerus[i][j]);
                //            PlayersVerus[i].Remove(PlayersVerus[i][j]);
                //            var idPlayerWin = PlayersVerus[i][0];
                //            var playerWin = ClientManager.Gi().GetCharacter(idPlayerWin);
                //            if (playerWin != null)
                //            {
                //                WinRound(playerWin, "Bạn đã thắng vòng này, xin chờ tại đây ít phút để thi tiếp vòng sau");
                //            }
                //            PlayersVerus[i].Remove(PlayersVerus[i][0]);

                //            break;
                //        }
                //        else if (CheckPosistion(player))
                //        {
                //            Failed(player, "Bạn đã thất bại vì vi phạm quy chế thi đấu, hãy trở lại vào giải đấu lần sau");
                //            PlayersFighting.Remove(player.Id);
                //            PlayersVerus[i].Remove(player.Id);
                //            var idPlayerWin = PlayersVerus[i][0];
                //            var playerWin = ClientManager.Gi().GetCharacter(idPlayerWin);
                //            if (playerWin != null)
                //            {
                //                WinRound(playerWin, "Bạn đã thắng vòng này, xin chờ tại đây ít phút để thi tiếp vòng sau");
                //            }
                //            PlayersVerus[i].Remove(PlayersVerus[i][0]);

                //            break;
                //        }
                //        else
                //        {
                //            var playersVerus = PlayerWin(PlayersVerus[i]);
                //            var pWin = ClientManager.Gi().GetCharacter(playersVerus[0]);
                //            var pLose = ClientManager.Gi().GetCharacter(playersVerus[1]);
                //            if (pWin != null)
                //            {
                //                WinRound(pWin, "Bạn đã thắng vòng này, xin chờ tại đây ít phút để thi tiếp vòng sau");
                //            }
                //            PlayersVerus[i].Remove(playersVerus[0]);
                //            if (pLose != null)
                //            {
                //                Failed(pLose, "Bạn đã thua cuộc, hãy trở lại vào gỉải sau");
                //            }
                //            PlayersVerus[i].Remove(playersVerus[1]);
                //            PlayersFighting.Remove(playersVerus[1]);
                //            break;
                //        }
                //    }


                //}
            

        }
       
        public void Failed(Character character, string reason)
        {
            SetTypePk(character, 0);
            character.CharacterHandler.BackHome();
            character.CharacterHandler.SendMessage(Service.ServerMessage(reason));
            character.DataDaiHoiVoThuat.Status = ChampionerCS23_Status.NORMAL;
        }
        public void OutMap(Character character)
        {
            character.DataDaiHoiVoThuat.Status = ChampionerCS23_Status.NORMAL;
            //var player = ClientManager.Gi().GetCharacter(character.DataDaiHoiVoThuat.Enemy);
            //if (player != null)
            //{
            //    WinRound(character, Reason.WIN_BECAUSE_ENEMY_OUT);
            //}
            //character.CharacterHandler.SendMessage(Service.ServerMessage(Reason.FAIL_BECAUSE_OUT_MAP));
            SetTypePk(character, 0);
            //PlayersFighting.Remove(character.Id);
            Server.Gi().Logger.Print("outmap");
        }
        //chưa xử lí được out game khi đang đánh nhau
        public void Reset(Character character)
        {
            character.DataDaiHoiVoThuat.Status = ChampionerCS23_Status.NORMAL;
           
        }
        public void WinRound(Character character, string reason = "")
        {
            if (TaskHandler.CheckTask(character, 19, 1) && Round == 1)
            {
                TaskHandler.gI().PlusSubTask(character, 1);
            }
            SetTypePk(character, 0);
            character.DataDaiHoiVoThuat.Status = ChampionerCS23_Status.NORMAL;
            MapManager.JoinMap((Character)character, 52, MapManager.Get(52).GetZoneNotMaxPlayer().Id, false, false, 0);
            character.CharacterHandler.SendMessage(Service.ServerMessage(reason));
        }
        public void Winner(Character character)
        {
            SetTypePk(character, 0);
            MapManager.JoinMap((Character)character, 52, MapManager.Get(52).GetZoneNotMaxPlayer().Id, false, false, 0);
            character.CharacterHandler.SendMessage(Service.ServerMessage(string.Format(Reason.WINNER, GetNameChampionShip())));
            ClientManager.Gi().SendMessage(Service.ServerMessage(string.Format(Reason.CONGRATULATION_WINNER, GetNameChampionShip(), character.Name)));
        }
        public bool CheckPosistion(Character character)
        {
            return (character.InfoChar.X >= 35 && character.InfoChar.X <= 157) || (character.InfoChar.X >= 611 && character.InfoChar.X <= 733) || character.InfoChar.Y == 288;
        }
        public bool CheckOutmap(Character character)
        {
            return character == null|| character.InfoChar.MapId != 113 ;
        }
        public void Update()
        {
            if (PlayersLucky.Count >= 1)
            {
                for (int i = 0; i < PlayersLucky.Count; i++)
                {
                    var character = (Model.Character.Character)ClientManager.Gi().GetCharacter(PlayersLucky[i]);
                    if (character != null)
                    {
                        WinRound(character, Reason.WIN_BECAUSE_ENEMY_OUT);
                    }
                }
            }
            for (int i = 0; i < PlayersFighting.Count; i++)
            {
                
                var character = (Model.Character.Character)ClientManager.Gi().GetCharacter(PlayersFighting[i]);
                if (PlayersFighting.Count == 1)
                {
                    if (character != null)
                    {
                        // nếu thằng này online
                        Winner(character);
                    }
                    PlayersFighting.Remove(PlayersFighting[0]);
                    Status = ChampionStatus.END;
                    break;
                }
                if (IndexPair(i) >= PlayersFighting.Count)//nếu không có đối thủ
                {
                    if (character != null)
                    {
                        // nếu thằng này online
                        WinRound(character, Reason.WIN_BECAUSE_ENEMY_OUT);
                    }
                    else
                    {
                        //nếu nó đéo online
                        PlayersFighting.Remove(PlayersFighting[i]);
                    }
                    break;
                }
                var player = (Model.Character.Character)ClientManager.Gi().GetCharacter(PlayersFighting[IndexPair(i)]);
                if (CheckOutmap(character) || CheckPosistion(character))
                {
                    //khi nó out map hoặc out game
                    if (player != null)
                    {
                        WinRound(player, Reason.WIN_BECAUSE_ENEMY_OUT);
                        PlayersFighting.Remove(PlayersFighting[i]); // xóa id thằng character check out
                        continue;
                    }
                    else //nếu thằng đối thủ cũng out
                    {
                        PlayersFighting.Remove(PlayersFighting[IndexPair(i)]);
                        continue;
                    }
                }
                
            }
        }
        public int IndexPair(int index)
        {
            var indexReturn = 0;
            if (index % 2 == 0)
            {
                indexReturn = index + 1;
            }
            else
            {
                indexReturn = index - 1;
            }
            return indexReturn;
        }
        public bool SetType()
        {
            var h = ServerUtils.TimeNow().Hour;
            switch (h)
            {
                case 8 or 13 or 18:
                    TypeChampionShip = 1;
                    return true;
                case 9 or 14 or 19:
                    TypeChampionShip = 2;
                    return true;
                case 10 or 15 or 20:
                    TypeChampionShip = 3;
                    return true;
                case 11 or 16 or 21:
                    TypeChampionShip = 4;
                    return true;
                default:
                    TypeChampionShip = 5;
                    return true;
                
            }
        }
    }
}
