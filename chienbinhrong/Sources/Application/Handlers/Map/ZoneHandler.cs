    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Handlers.Skill;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.Interfaces.Map;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.DatabaseManager;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Item;
using NgocRongGold.Model.Map;
using NgocRongGold.Model.Monster;
using NgocRongGold.Model.Template;
using NgocRongGold.Model;
using static System.GC;
using NgocRongGold.Application.Extension;
using NgocRongGold.Application.Extension.BlackballWar;
using System.Threading;
using System.Runtime.InteropServices;
using NgocRongGold.Application.Extension.DaiHoiVoThuat;
using NgocRongGold.Application.Extension.DiedRing;
using NgocRongGold.Application.Extension.NamecballWar;
using NgocRongGold.Application.Extension.NamecBattlefield;
using System.Runtime.CompilerServices;
using NgocRongGold.Application.Extension.Practice;
using Org.BouncyCastle.Math.Field;
using Application.Interfaces.Zone;
using NgocRongGold.Application.Handlers.Monster;

namespace NgocRongGold.Application.Handlers.Map
{
    public class ZoneHandler : IZoneHandler
    {
        public IZone Zone { get; set; }

        public ZoneHandler(IZone zone)
        {
            Zone = zone;
        }
        //#region TypeTeleport
        //public static int AUTO_SPACE_SHIP = -1;
        //public static int NON_SPACE_SHIP = 0;
        //public static int DEFAULT_SPACE_SHIP = 1;
        //public static int TELEPORT_YARDRAT = 2;
        //public static int TENNIS_SPACE_SHIP = 3;
        //#endregion

        public int GetCountMob()
        {
            var count = 0;
            foreach (var mob in Zone.MonsterMaps)
            {
                if (!mob.IsDie)
                {
                    count++;
                }
            }
            return count;
        }
        public void JoinZone(Model.Character.Character character, bool isDefault, bool isTeleport, int typeTeleport)
        {
            if (character == null || character.GetType() != typeof(Model.Character.Character)) return;
            var zone = Zone;
            character.CharacterHandler.SendMessage(Service.SendImageBag(character.Id, character.GetBag()));
            character.TypeTeleport = typeTeleport;

            character.InfoChar.MapId = zone.Map.Id;
            character.InfoChar.ZoneId = zone.Id;
            character.CharacterHandler.SendMessage(Service.MapClear());
            character.CharacterHandler.SendMessage(Service.SendStamina(character.InfoChar.Stamina));
            if (isDefault && !isTeleport)
            {
                character.InfoChar.X = zone.Map.TileMap.toX;
                character.InfoChar.Y = zone.Map.TileMap.toY;
            }
            else if (isTeleport)
            {
                character.InfoChar.X = (short)ServerUtils.RandomNumber(250, 450);
                character.InfoChar.Y = 0;
            }
            character.UpdateOldMap();

            if (zone.Characters.TryAdd(character.Id, character))
            {
                zone.ICharacters.TryAdd(character.Id, character);
                var disciple = character.Disciple;
                bool checkHaveDisciple = false;
                if (disciple is { Status: < 3 } && character.InfoChar.IsHavePet && !character.InfoChar.Fusion.IsFusion && !disciple.InfoChar.IsDie)
                {
                    if (zone.Disciples.TryAdd(disciple.Id, disciple))
                    {
                        disciple.Zone = zone;
                        disciple.CharacterHandler.SetUpPosition(isRandom: true);
                        disciple.PlusPoint.RandomPoint(disciple);
                        checkHaveDisciple = true;
                    }
                }
                bool checkHavePet = false;

                var pet = character.Pet;
                if (pet != null && !DataCache.IdMapKarin.Contains(zone.Map.Id))
                {
                    if (Zone.Pets.TryAdd(pet.Id, pet))
                    {
                        pet.Zone = zone;
                        pet.CharacterHandler.SetUpPosition(isRandom: true);
                        checkHavePet = true;
                    }
                }
                bool checkHavePet2 = false;
                var pet2 = character.Pet2;
                if (pet2 != null && !DataCache.IdMapKarin.Contains(zone.Map.Id))
                {
                    if (Zone.Pets2.TryAdd(pet2.Id, pet2))
                    {
                        pet2.Zone = zone;
                        pet2.CharacterHandler.SetUpPosition(isRandom: true);
                        checkHavePet2 = true;
                    }
                }
                if (character.InfoSkill.Egg.Monster != null)
                {
                    if (Zone.MonsterPets.TryAdd(character.InfoSkill.Egg.Monster.IdMap, character.InfoSkill.Egg.Monster))
                    {
                        character.InfoSkill.Egg.Monster.Zone = zone;
                        SendMessage(Service.UpdateMonsterMe0(character.InfoSkill.Egg.Monster));
                    }
                    else
                    {
                        SkillHandler.RemoveMonsterPet(character);
                    }
                }
                bool checkRole = false;
                var role = character.InfoChar.Roles1.RoleUsed;
                if (role != null)
                {
                    checkRole = true;
                }

                bool checkPhanthans = false;
                if (character.PhanThans != null && character.PhanThans.Count > 0)
                {
                    checkPhanthans = true;
                }
                foreach (var @char in Zone.Characters.Values.Where(c => c.Id != character.Id))
                {
                    @char?.CharacterHandler.SendMessage(Service.PlayerAdd(character));
                    if (checkHaveDisciple) @char?.CharacterHandler.SendMessage(Service.PlayerAdd(disciple, "#"));
                    if (checkHavePet) @char?.CharacterHandler.SendMessage(Service.PlayerAdd(pet, "#"));
                    if (checkHavePet2) @char?.CharacterHandler.SendMessage(Service.PlayerAdd(pet2, "[Event]"));
                    if (checkRole) @char?.CharacterHandler.SendMessage(Service.SendRole(character.Id, role.Second, role.Temp));
                    if (checkPhanthans)
                    {
                        foreach (var phanthan in character.PhanThans)
                        {
                            @char?.CharacterHandler.SendMessage(Service.PlayerAdd(phanthan, ""));
                        }
                    }
                }

                character.Zone = zone;
                character.IsNextMap = true;
                character.CharacterHandler.SendMessage(Service.MapInfo(zone, character));
                //character.CharacterHandler.SetUpInfo();
                character.CharacterHandler.SendMessage(Service.UpdateBody(character));
                character.CharacterHandler.SendMessage(Service.ChangeFlag2(character.Flag));
               // character.CharacterHandler.UpdatePhukien();
                character.CharacterHandler.SendMessage(Service.PlayerLoadSpeed(character));
                character.CharacterHandler.SendMessage(Service.SendImgWater(11528));
                character.CharacterHandler.UpdateMask();

                if (!isTeleport || typeTeleport != 3) return;
                character.InfoChar.Hp = character.HpFull;
                character.InfoChar.Mp = character.MpFull;
                character.CharacterHandler.SendMessage(Service.SendHp((int)character.InfoChar.Hp));
                character.CharacterHandler.SendMessage(Service.SendMp((int)character.InfoChar.Mp));
                SendMessage(Service.PlayerLevel(character));

            }
            else
            {
                if (Zone.Characters.TryRemove(character.Id, out _))
                {
                    JoinZone(character, isDefault, isTeleport, typeTeleport);
                    Server.Gi().Logger.Print("Character Join Zone Again Success !", "red");

                }
                else
                {
                    Server.Gi().Logger.Print("Character Join Zone Again Failed !", "red");
                }
            }
        }


        public void AddDisciple(Disciple disciple)
        {
            
            if (!Zone.Disciples.TryAdd(disciple.Id, disciple)) return;
            disciple.Zone = Zone;

            SendMessage(Service.PlayerAdd(disciple, disciple.Character.Id + disciple.Id == 0 ? "$" : "#"));
        }
        public void AddPhanThan(PhanThan disciple)
        {

            if (!Zone.PhanThans.TryAdd(disciple.Id, disciple)) return;
            disciple.Zone = Zone;

            SendMessage(Service.PlayerAdd(disciple, disciple.Character.Id + disciple.Id == 0 ? "$" : "#"));
        }
        public void RemoveDisciple(Disciple disciple)
        {

            if (!Zone.Disciples.TryRemove(disciple.Id, out _)) return;

            SendMessage(Service.PlayerRemove(disciple.Id));
            if (disciple.InfoSkill.Egg.Monster == null) return;
            RemoveMonsterMe(disciple.InfoSkill.Egg.Monster.IdMap);
            SendMessage(Service.UpdateMonsterMe7(disciple.InfoSkill.Egg.Monster.IdMap));


        }
        public void RemovePhanThan(PhanThan disciple)
        {

            if (!Zone.PhanThans.TryRemove(disciple.Id, out _)) return;

            SendMessage(Service.PlayerRemove(disciple.Id));
            if (disciple.InfoSkill.Egg.Monster == null) return;
            RemoveMonsterMe(disciple.InfoSkill.Egg.Monster.IdMap);
            SendMessage(Service.UpdateMonsterMe7(disciple.InfoSkill.Egg.Monster.IdMap));


        }
        public void AddPet(Pet pet)
        {
          
            if (!Zone.Pets.TryAdd(pet.Id, pet)) return;
            pet.Zone = Zone;

            SendMessage(Service.PlayerAdd(pet, pet.Character.Id + pet.Id == 0 ? "$" : "#"));

        }

        public void RemovePet(Pet pet)
        {

            try
            {
                if (!Zone.Pets.TryRemove(pet.Id, out _)) return;
                SendMessage(Service.PlayerRemove(pet.Id));
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error RemovePet in ZoneHandler.cs {e.Message} \n {e.StackTrace}", e);
            }

        }
        public void AddPet(Pet2 pet)
        {
            
            if (!Zone.Pets2.TryAdd(pet.IdMap, pet)) return;
            pet.Zone = Zone;

            SendMessage(Service.PlayerAdd(pet, "[Event]"));

        }

        public void RemovePet(Pet2 pet)
        {

            try
            {
                if (!Zone.Pets2.TryRemove(pet.IdMap, out _)) return;
                SendMessage(Service.PlayerRemove(pet.Id));
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error RemovePet in ZoneHandler.cs {e.Message} \n {e.StackTrace}", e);
            }

        }
        public void AddBoss(Boss boss)
        {

            if (!Zone.Bosses.TryAdd(boss.Id, boss)) return;
            boss.Zone = Zone;
            boss.CharacterHandler.SetUpPosition(mapNew: Zone.Map.Id);
            SendMessage(Service.PlayerAdd(boss));
        }

        public void RemoveBoss(Boss boss)
        {

            try
            {
                if (!Zone.Bosses.TryRemove(boss.Id, out _)) return;
                SendMessage(Service.PlayerRemove(boss.Id));
                if (boss.InfoSkill.Egg.Monster == null) return;
                RemoveMonsterMe(boss.InfoSkill.Egg.Monster.IdMap);
                SendMessage(Service.UpdateMonsterMe7(boss.InfoSkill.Egg.Monster.IdMap));
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error RemoveBoss in ZoneHandler.cs: {e.Message} \n {e.StackTrace}", e);
            }

        }



        public void OutZone(ICharacter character)
        {
            var zone = Zone;
            try
            {
                var @charReal = (Model.Character.Character)character;
                var mapOld = character.InfoChar.MapId;

                if (zone.Characters.TryRemove(character.Id, out _))
                {
                    SendMessage(Service.PlayerRemove(character.Id), character.Id);

                    if (character.InfoSkill.Egg.Monster != null)
                    {
                        RemoveMonsterMe(character.InfoSkill.Egg.Monster.IdMap);
                        SendMessage(Service.UpdateMonsterMe7(character.InfoSkill.Egg.Monster.IdMap));
                    }

                    var disciple = @charReal.Disciple;
                    if (disciple != null && (disciple.Status < 3 || disciple.InfoChar.IsDie) && character.InfoChar.IsHavePet && !character.InfoChar.Fusion.IsFusion)
                    {
                        if (zone.Disciples.TryRemove(disciple.Id, out _))
                        {
                            //Zone.Disciples.Remove(disciple);
                            disciple.MonsterFocus = null;
                            SendMessage(Service.PlayerRemove(disciple.Id), character.Id);
                            if (disciple.InfoSkill.Egg.Monster != null)
                            {
                                RemoveMonsterMe(disciple.InfoSkill.Egg.Monster.IdMap);
                                SendMessage(Service.UpdateMonsterMe7(disciple.InfoSkill.Egg.Monster.IdMap));
                            }
                        }
                    }

                    var pet = @charReal.Pet;
                    if (pet != null)
                    {
                        if (zone.Pets.TryRemove(pet.Id, out _))
                        {
                            SendMessage(Service.PlayerRemove(pet.Id), character.Id);
                        }
                    }
                    var pet2 = @charReal.Pet2;
                    if (pet2 != null)
                    {
                        if (zone.Pets2.TryRemove(pet2.Id, out _))
                        {
                            SendMessage(Service.PlayerRemove(pet2.Id), character.Id);
                        }
                    }
                    var phanthan = @charReal.PhanThans;
                    if (phanthan != null)
                    {
                        foreach (var temp in @charReal.PhanThans)
                        {
                            if (zone.PhanThans.TryRemove(temp.Id, out _))
                            {
                                SendMessage(Service.PlayerRemove(temp.Id), character.Id);
                            }
                        }
                    }
                }

                if (@charReal.Trade.IsTrade)
                {
                    var charTemp = (Model.Character.Character)GetCharacter(@charReal.Trade.CharacterId);
                    if (charTemp != null && charTemp.Trade.CharacterId == character.Id)
                    {
                        charTemp.CloseTrade(true);
                        charTemp.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().CLOSE_TRADE));
                    }
                    @charReal.CloseTrade(false);
                }
                if (BlackballCache.ListMapNRSD.Contains(@charReal.InfoChar.MapId) && @charReal.Blackball.AlreadyPick(charReal))
                {
                    @charReal.Blackball.ExitMapOrDie(@charReal);
                }
                if (charReal.Challenge.isChallenge)
                {
                    var player = (Model.Character.Character)ClientManager.Gi().GetCharacter(@charReal.Challenge.PlayerChallengeID);
                    var gold = (player.Challenge.Gold - (@charReal.Challenge.Gold / 100)) + (player.Challenge.Gold - (@charReal.Challenge.Gold / 100));
                    player.CharacterHandler.SendMessage(Service.ServerMessage($"Đối thủ đã bỏ trốn, bạn đã nhận được {gold} vàng"));
                    player.PlusGold(gold);
                    player.CharacterHandler.SendMessage(Service.MeLoadInfo(player));
                    @charReal.Challenge.SetStatusEnd(@charReal);
                    player.Challenge.SetStatusEnd(player);
                }
                if (@charReal.DataVoDaiSinhTu.Status is DiedRing_Character_Status.FIGHTING)
                {
                    DiedRing_Handler.gI().OutMapOrDie(@charReal);
                }
                if (@charReal.DataDaiHoiVoThuat23.Status is ChampionerCS23_Status.FIGHITING)
                {
                    @charReal.DataDaiHoiVoThuat23.Handler.Outmap(@charReal, "Bạn đã thất bại vì vi phạm quy chế thi đấu");
                }
                if (@charReal.DataDaiHoiVoThuat.Status is ChampionerCS23_Status.FIGHITING)
                {
                    ChampionShip.gI().OutMap(@charReal);
                }
                if (@charReal.DataNamecBattlefield.Status is NamecBattlefield_Character_Status.FIGHTING)
                {
                    NamecBattlefield_Handler.OutOrDie(charReal);
                }
                if (charReal.DataPractice.Status is (Practice_Staus.PRACTICE or Practice_Staus.CHALLENGE))
                {
                    charReal.DataPractice.Status = Practice_Staus.NORMAL;
                    if (charReal.DataPractice.Status is (Practice_Staus.PRACTICE)) charReal.DataPractice.Handler.EndPractice(charReal);
                }
                if (@charReal.DataSieuHang.Status is Extension.DaiHoiVoThuat.SuperChampion.SuperChampion_Championer_Status.BATTLE)
                {
                    @charReal.DataSieuHang.Handler.OutMap(charReal);
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error OutZone in ZoneHandler.cs: {e.Message} \n {e.StackTrace}", e);
            }
        }

        public void InitMob()
        {
            var tileMap = Zone.Map.TileMap;
            MonsterTemplate monsterTemplate;
            var i = 0;
            tileMap.MonsterMaps.ForEach(monster =>
            {
                monsterTemplate = Cache.Gi().MONSTER_TEMPLATES.FirstOrDefault(x => x.Id == monster.Id);

                if (monsterTemplate != null)
                {
                    switch (monsterTemplate.Id)
                    {
                        case 77 or 85 or 76 or 71:
                            {
                                var MonsterMap = new MonsterMap()
                                {
                                    IdMap = i++,
                                    Id = monster.Id,
                                    X = monster.X,
                                    Y = monster.Y,
                                    Status = 0,
                                    IsRefresh = false,
                                    IsDie = true,
                                    Level = monster.Level,
                                    LvBoss = 200,
                                    IsBoss = false,
                                    Zone = Zone,
                                    OriginalHp = 10000000,
                                    LeaveItemType = monsterTemplate.LeaveItemType,
                                };
                                MonsterMap.MonsterHandler.SetUpMonster(MonsterMap.IsDie);
                                Zone.MonsterMaps.Add(MonsterMap);
                            }
                            break;
                        case 92 or 93 or 82 or 83 or 84:
                            {
                                var MonsterMap = new MonsterMap(0)
                                {
                                    IdMap = i++,
                                    Id = monster.Id,
                                    X = monster.X,
                                    Y = monster.Y,
                                    Status = 5,
                                    Level = monster.Level,
                                    LvBoss = 200,
                                    IsBoss = true,
                                    Zone = Zone,
                                    OriginalHp = monsterTemplate.Hp,
                                    LeaveItemType = monsterTemplate.LeaveItemType,
                                };
                                MonsterMap.MonsterHandler.SetUpMonster(MonsterMap.IsDie);
                                Zone.MonsterMaps.Add(MonsterMap);
                            }
                            break;
                        default:
                            {
                                var MonsterMap = new MonsterMap()
                                {
                                    IdMap = i++,
                                    Id = monster.Id,
                                    X = monster.X,
                                    Y = monster.Y,
                                    Status = 5,
                                    Level = monster.Level,
                                    LvBoss = 0,
                                    IsBoss = false,
                                    Zone = Zone,
                                    OriginalHp = monsterTemplate.Hp,
                                    LeaveItemType = monsterTemplate.LeaveItemType,
                                };
                                MonsterMap.MonsterHandler.SetUpMonster(MonsterMap.IsDie);
                                Zone.MonsterMaps.Add(MonsterMap);
                            }
                            break;
                    }
                }
            });
            
        }

        public async Task Update()
        {
            await Task.WhenAll(UpdatePlayer(), UpdateMonsterMap(), UpdateBoss(), UpdateItemMap(), UpdatePet2());
        }

        #region Update Zone
        private async Task UpdateMonsterMap()
        {
            try
            {
                if (Zone.MonsterMaps.Count > 0)
                {
                    foreach(var monster in Zone.MonsterMaps)
                    {
                        monster.MonsterHandler.Update();
                    }
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error UpdateMonsterMap in ZoneHandler.cs: {e.Message} \n {e.StackTrace}", e);
            }

            await Task.Delay(50);
        }
        private async Task UpdateNpc()
        {
            try
            {
                foreach(var npc in Zone.Npcs){
                    
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error Update Npc in ZoneHandler.cs: {e.Message} \n {e.StackTrace}", e);
            }

            await Task.Delay(50);
        }
      

        private async Task UpdateItemMap()
        {
            try
            {
                if (Zone.ItemMaps.Count > 0)
                {
                   foreach(var item in Zone.ItemMaps.Values)
                    {
                        item.ItemMapHandler.Update(Zone);
                    }
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error UpdateItemMap in Service.cs: {e.Message} \n {e.StackTrace}", e);
            }
            await Task.Delay(50);

        }
        private async Task UpdatePlayer()
        {
            try
            {
                if (Zone.Characters.Count > 0)
                {
                    foreach(var player in Zone.Characters.Values)
                    {
                        player.CharacterHandler.Update();
                    }
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error UpdatePlayer in Service.cs: {e.Message} \n {e.StackTrace}", e);
            }
            await Task.Delay(50);

        }
        private async Task UpdatePet2()
        {
            try
            {
                if (Zone.Pets2.Count > 0)
                {
                    foreach(var pet in Zone.Pets2.Values)
                    {
                        pet.CharacterHandler.Update();
                    }
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error UpdatePet2 in Service.cs: {e.Message} \n {e.StackTrace}", e);
            }
            await Task.Delay(50);

        }

        private async Task UpdateBoss()
        {
            try
            {
                foreach(var boss in Zone.Bosses.Values) 
                {
                    if (boss == null) return;
                    boss.CharacterHandler.Update();
                }


            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error UpdateBoss in Service.cs: {e.Message} \n {e.StackTrace}", e);
            }
            await Task.Delay(50);   
        }
        #endregion

        public void Close()
        {
            Zone.ItemMaps.Clear();
            Zone.MonsterMaps.Clear();
            Zone.Bosses.Clear();
            Zone.Pets2.Clear();
            Zone.Pets.Clear();
            Zone.ICharacters.Clear();
            Zone.PhanThans.Clear();
            Zone.Map = null;
            SuppressFinalize(this);
        }

        public void SendMessage(Message message, bool isSkillMessage = false)
        {

            Zone.Characters.Values.ToList().ForEach(character =>
            {
                if (isSkillMessage && !character.InfoChar.  HieuUngDonDanh) return;
                character?.CharacterHandler.SendMessage(message);
            });


        }
        public void SendMessage(Message message)
        {
            foreach (var character in Zone.Characters.Values)
            {
                character?.CharacterHandler.SendMessage(message);
            }
        }
        public void SendMessage(Message message, int id)
        {
            foreach (var character in Zone.Characters.Values)
            {
                character?.CharacterHandler.SendMessage(message);
            }
        }

        public void LeaveItemMap(ItemMap itemMap)
        {
            if (itemMap == null) return;
            try
            {

                if (itemMap?.Item == null) return;
                if (Zone.ItemMaps.Count > 500) RemoveItemMap(0);
                {
                    itemMap.Id = GetItemMapNotId();
                    itemMap.X = (short)ServerUtils.RandomNumber(itemMap.X - 15, itemMap.X + 15);
                    itemMap.Y = Zone.Map.TileMap.TouchY(itemMap.X, itemMap.Y);
                }
                if (Zone.ItemMaps.TryAdd(itemMap.Id, itemMap))
                {
                    SendMessage(Service.ItemMapAdd(itemMap));
                }

            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error LeaveItemMap in ZoneHandler.cs: {e.Message} \n {e.StackTrace}", e);
            }
        }

        public void LeaveItemMap(ItemMap itemMap, MonsterMap monster)
        {
            if(itemMap == null) return;
            try
            {

                if (itemMap?.Item == null) return;
                if (Zone.ItemMaps.Count > 500) RemoveItemMap(0);
                itemMap.Id = GetItemMapNotId();
                itemMap.X = (short)ServerUtils.RandomNumber(itemMap.X - 30, itemMap.X + 30);
                itemMap.Y = Zone.Map.TileMap.TouchY(itemMap.X, itemMap.Y);
                if (Zone.ItemMaps.TryAdd(itemMap.Id, itemMap))
                {
                    if (Cache.Gi().MONSTER_TEMPLATES[monster.Id].Type == 4)
                    {
                        SendMessage(Service.MonsterFlyLeaveItem(monster.IdMap, itemMap));
                    }
                    else
                    {
                        SendMessage(Service.ItemMapAdd(itemMap));
                    }
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error LeaveItemMap in ZoneHandler.cs: {e.Message} \n {e.StackTrace}", e);
            }
        }

        public void RemoveItemMap(int id)
        {
            
                if (Zone.ItemMaps.TryRemove(id, out var it))
                {
                    SendMessage(Service.ItemMapRemove(id));
                    it.Dispose();
                }
            
        }

        public int GetItemMapNotId()
        {
            if (Zone.ItemMapId > 500) Zone.ItemMapId = 0;
            return Zone.ItemMapId++;
        }

        public void RemoveCharacter()
        {
            
        }

        public void RemoveMonsterMe(int id)
        {
            
                Zone.MonsterPets.TryRemove(id, out _);
            
        }

        public bool AddMonsterPet(MonsterPet monsterPet)
        {
            
                return Zone.MonsterPets.TryAdd(monsterPet.IdMap, monsterPet);
            
        }
        public ICharacter GetDisciple(int id)
        {
            if (Zone.Disciples.TryGetValue(id, out var disciple))
            {
                return disciple;
            }
            return null;
        }
        public ICharacter GetCharacter(int id)
        {
            if (Zone.Characters.TryGetValue(id, out var pler))
            {
                return pler;
            }
            return null;
        }
        public ICharacter GetICharacter(int id)
        {
            if (Zone.Characters.TryGetValue(id, out var pler))
            {
                return pler;
            }
            if (Zone.Bosses.TryGetValue(id, out var pler2))
            {
                return pler2;
            }
            if (Zone.Disciples.TryGetValue(id, out var pler3))
            {
                return pler3;
            }
            return null;
        }
        public List<ICharacter> GetCharacterClanInMap(int idClan)
        {
            List<ICharacter> charactersClan = new List<ICharacter>();

           
                foreach (var character in Zone.Characters.Values)
                {
                    if (character.ClanId == idClan) {
                        charactersClan.Add(character);
                    } 
                }
            

            return charactersClan;
        }
        public List<ICharacter> GetCharacterClanHasNamecBallInMap(int idClan)
        {
            List<ICharacter> charactersClan = new List<ICharacter>();

            
                foreach (var character in Zone.Characters.Values)
                {
                    if (character.ClanId == idClan && character.DataNgocRongNamek.AlreadyPick(character))
                    {
                        charactersClan.Add(character);
                    }
                }
            

            return charactersClan;
        }

        public List<ICharacter> CharacterInMap()
        {
            List<ICharacter> characters = new List<ICharacter>();

            foreach (var @char in Zone.Characters.Values)
            {
                characters.Add(@char);
            }
            return characters;
        }
        public List<ICharacter> BossInMap(){
             List<ICharacter> bosses = new List<ICharacter>();
                foreach(var @boss in Zone.Bosses.Values){
                    bosses.Add(@boss);
                }
            
            return bosses;
        }
        public List<Boss> GetBossInMap(int type)
        {
            List<Boss> BossInMap = new List<Boss>();

                foreach (var boss in Zone.Bosses.Values)
                {
                    if (boss.Type == type)
                    {
                        BossInMap.Add(boss);
                    }
                }
            

            return BossInMap;
        }
        public List<ICharacter> GetBossInMap()
        {
            List<ICharacter> BossInMap = new List<ICharacter>();

            
                foreach (var boss in Zone.Bosses.Values)
                {
                    
                        BossInMap.Add(boss);
                    
                }
            

            return BossInMap;
        }
        public List<ItemMap> GetItemInMap(int type)
        {
            List<ItemMap> ItemInMap = new List<ItemMap>();

            
                foreach (var ItemMaps in Zone.ItemMaps.Values)
                {
                    if (ItemCache.ItemTemplate(ItemMaps.Item.Id).Type== type)
                    {
                        ItemInMap.Add(ItemMaps);
                    }
                }
            

            return ItemInMap;
        }
            public List<ItemMap> GetItemMapsByID(int id){
                List<ItemMap> ItemMaps = new List<ItemMap>();
                foreach (var itm in Zone.ItemMaps.Values){
                    if (itm.Item.Id == id){
                        ItemMaps.Add(itm);
                    }
                }
                return ItemMaps;
            }
      

        public ICharacter GetPet(int id)
        {
            return GetPetKeyValue(id).Value;
        }
        public List<ICharacter> GetPet2(int id)
        {
            List<ICharacter> chars = new List<ICharacter>();
            foreach (var @char in Zone.Pets2.Values)
            {
                var pet2 = (Pet2)@char;
                if (pet2.PetId == id) chars.Add(@char);
            }
            return chars;
        }
        public ICharacter GetBoss(int id)
        {
            return GetBossKeyValue(id).Value;
        }

        public MonsterMap GetMonsterMap(int id)
        {
                return Zone.MonsterMaps.FirstOrDefault(m => m.IdMap == id);
        }
        public MonsterMap GetMobId(int id)
        {
           
               return Zone.MonsterMaps.FirstOrDefault(m => m.Id == id);
            
        }

        public MonsterPet GetMonsterPet(int id)
        {
            
                return Zone.MonsterPets.GetValueOrDefault((short)id);
            
        }
       

        public KeyValuePair<int, Model.Character.Character> GetCharacterKeyValue(int id)
        {
           
                return Zone.Characters.FirstOrDefault(c => c.Key == id);
            
        }

        public KeyValuePair<int, Disciple> GetDiscipleKeyValue(int id)
        {
            
                return Zone.Disciples.FirstOrDefault(c => c.Key == id);
            
        }

        public KeyValuePair<int, Pet> GetPetKeyValue(int id)
        {
            
                return Zone.Pets.FirstOrDefault(c => c.Key == id);
            
        }
        
        public KeyValuePair<int, Boss> GetBossKeyValue(int id)
        {
            
                return Zone.Bosses.FirstOrDefault(c => c.Key == id);
            
        }
    }
}