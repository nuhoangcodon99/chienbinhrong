using System;
using System.Collections.Generic;
using System.Linq;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Handlers.Item;
using NgocRongGold.Application.Handlers.Skill;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.Interfaces.Monster;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.DatabaseManager;
using NgocRongGold.DatabaseManager.Player;
using NgocRongGold.Model;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Info;
using NgocRongGold.Model.Item;
using NgocRongGold.Model.Map;
using NgocRongGold.Model.Monster;
using NgocRongGold.Model.Effect;
using NgocRongGold.Model.Option;
using static System.GC;
using Google.Protobuf.WellKnownTypes;
using NgocRongGold.Model.Template;
using NgocRongGold.Application.MainTasks;
using NgocRongGold.Application.Extension.Ấp_trứng;
using NgocRongGold.Application.Extension;
using NgocRongGold.Application.Extension.BlackballWar;
using NgocRongGold.Application.Extension.ChampionShip;
using System.Threading.Tasks;
using NgocRongGold.Application.Extension.Namecball;
using System.Runtime.InteropServices;
using NgocRongGold.Application.Extension.Bosses.Mabu2Gio;
using NgocRongGold.Application.Extension.Bosses.Mabu12Gio;
using NgocRongGold.Application.Extension.ConSoMayMan;
using NgocRongGold.Application.Extension.DaiHoiVoThuat;
using NgocRongGold.Application.Extension.NamecBattlefield;
using NgocRongGold.Application.Extension.Practice;  
using System.IO.IsolatedStorage;
using NgocRongGold.Application.Extension.SideQuest.BoMong;
using NgocRongGold.Application.Extension.SideQuest.HangNgay;
using Application.Interfaces.Zone;
using NgocRongGold.Model.Data;
using NgocRongGold.Model.Info.EffectTemporary;
using System.Runtime.CompilerServices;

namespace NgocRongGold.Application.Handlers.Character
{
    public class CharacterHandler : ICharacterHandler
    {
        public Model.Character.Character Character { get; set; }
        public void SetupAmulet()
        {

        }
        public CharacterHandler(Model.Character.Character character)
        {
            Character = character;
        }
        public void SetPlayer(Player player)
        {
            Character.Player = player;
        }

        public void SendMessage(Message message)
        {
            Character?.Player?.Session?.SendMessage(message);
        }

        public void SendZoneMessage(Message message)
        {
            Character?.Zone?.ZoneHandler?.SendMessage(message);
        }

        public void SendMeMessage(Message message)
        {
            Character?.Zone?.ZoneHandler?.SendMessage(message, Character.Id);
        }

        private void UpdateAntiChangeServerTime(string reason = "")
        {
            //var timeServer = ServerUtils.CurrentTimeMillis();
            //if ((Character.InfoChar.ThoiGianDoiMayChu - timeServer) < 180000)
            //{
            //    Character.InfoChar.ThoiGianDoiMayChu = timeServer + 300000;
            //}
            //DelayInventoryAction(timeServer, reason);
        }

        //private void DelayInventoryAction(long timeServer, string reason)
        //{
        //    if (reason == "Nhặt từ map" || reason == "CSKB" || reason == "CSTT" || reason == "Ăn bánh tt" || reason == "Dùng đá nâng cấp" || reason == "Dùng đá bảo vệ" || reason == "Bán cho shop")
        //    {
        //        Character.Delay.SaveInvData = 8000 + timeServer;
        //    }
        //    else
        //    {
        //        Character.Delay.InvAction = timeServer + 1000;
        //    }
        //}

        public void Close()
        {
            Character.Me.CloseAndUpdate(Character);
            CharacterDB.Update(Character);
            Character?.Disciple?.CharacterHandler.Close();
            Character?.Pet?.CharacterHandler.Close();
            Character?.Pet2?.Reset();
            //foreach(var phanthan in Character.PhanThans)
            //{
                
            //}
            if (Character.DataNgocRongNamek.AlreadyPick(Character))
            {
                var itm = new ItemMap(-1, ItemCache.GetItemDefault((short)(Character.DataNgocRongNamek.IdNamekBall)));
                itm.X = Character.InfoChar.X;
                itm.Y = Character.InfoChar.Y;
                Character.Zone.ZoneHandler.LeaveItemMap(itm);
                Character.InfoChar.TypePk = 0;
                Character.DataNgocRongNamek.IdNamekBall = -1;
                Character.InfoChar.Bag = ClanManager.Get(Character.ClanId) != null ? (sbyte)ClanManager.Get(Character.ClanId).ImgId : (sbyte)-1;
            }            
            Character.Zone?.ZoneHandler?.OutZone(Character);
            Character.Blackball.Dispose();
            Character.DataBoMong.Dispose();
            Character.DataPractice.Dispose();
            
            Clear();

        }

        public void UpdateInfo(bool QueryItem = false)
        {
            SetUpInfo(QueryItem);
            SendMessage(Service.SendBody(Character));
            SendZoneMessage(Service.UpdateBody(Character));
            SendMessage(Service.MeLoadPoint(Character));
            Character.Me = new InfoFriend(Character);
        }

        public void Update()
        {
            var timeServer = ServerUtils.CurrentTimeMillis();
            lock (Character)
            {
                try
                {
                    RemoveSkill(timeServer);
                    UpdateOther(timeServer);
                    UpdateEffect(timeServer);
                   
                    if (Character.Pet != null)
                    {
                        Character.Pet.CharacterHandler.Update();
                    }
                    if (Character.Disciple != null)
                    {
                        Character.Disciple.CharacterHandler.Update();
                    }
                    if (Character.InfoSkill.Egg.Monster != null)
                    {
                        Character.InfoSkill.Egg.Monster.MonsterHandler.Update();
                    }
                   
                    if (!Character.InfoChar.IsDie)
                    {
                        UpdateEffectTemporary(timeServer);
                        UpdateFusion(timeServer);
                        UpdateAutoPlay(timeServer);
                        UpdateVeTinh(timeServer);
                    }
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Error UpdatePlayer in CharacterHandler.cs: {e.Message} \n {e.StackTrace}", e);

                }
            }
        }
       
        public void RemoveSocolaMabu(ICharacter character)
        {
            var infoskill = character.InfoSkill;
            infoskill.socolaMabu.Time = -1;
            infoskill.socolaMabu.isSocola = false;
            Character.CharacterHandler.SendZoneMessage(Service.UpdateBody(character));
        }
        public void RemoveHoaDa(ICharacter character)
        {
            var infoSkill = character.InfoSkill;
            infoSkill.HoaDa.IsHoaDa = false;
            infoSkill.HoaDa.Time = -1;
            infoSkill.HoaDa.CharacterId = -1;
            infoSkill.HoaDa.Percent = -1;
            infoSkill.HoaDa.Fight = -1;
            character.CharacterHandler.SendZoneMessage(Service.UpdateBody(character));
            character.CharacterHandler.SendZoneMessage(Service.SkillEffectPlayer(character.Id, character.Id, 2, 42));

        }
        public void RemoveHoaBang(ICharacter character)
        {
            var infoSkill = character.InfoSkill;
            infoSkill.HoaBang.Time = -1;
            infoSkill.HoaBang.isHoaBang = false;
            character.CharacterHandler.SendZoneMessage(EffectCharacter.removeEffChar(character.Id, 202));
            character.CharacterHandler.SendZoneMessage(Service.UpdateBody(character));
            character.CharacterHandler.SendZoneMessage(Service.SkillEffectPlayer(character.Id, character.Id, 2, 42));

        }
        public void RemoveMaPhongBa(ICharacter character)
        {
            var infoSkill = character.InfoSkill;
            infoSkill.MaPhongBa.isMaPhongBa = false;
            infoSkill.MaPhongBa.timeMaPhongBa = -1;
            character.CharacterHandler.SendZoneMessage(Service.UpdateBody(character));
        }
        public void RemoveSkill(long timeServer, bool globalReset = false)
        {
            var infoSkill = Character.InfoSkill;
            if ((infoSkill.TaiTaoNangLuong.IsTTNL &&
                 infoSkill.TaiTaoNangLuong.DelayTTNL <= timeServer) || globalReset)
            {
                SkillHandler.RemoveTTNL(Character);
            }
            if (infoSkill.MaPhongBa.isMaPhongBa && infoSkill.MaPhongBa.timeMaPhongBa <= timeServer || globalReset)
            {
                RemoveMaPhongBa(Character);
            }
            if (infoSkill.Monkey.MonkeyId == 1 && infoSkill.Monkey.TimeMonkey <= timeServer || globalReset)
            {
                SkillHandler.HandleMonkey(Character, false);
            }
            if (infoSkill.HoaDa.IsHoaDa && infoSkill.HoaDa.Time <= timeServer || globalReset)
            {
                RemoveHoaDa(Character);
            }
            if (infoSkill.TrungMabu.Active is Model.Info.Skill.Active.ACTIVE && infoSkill.TrungMabu.Time <= timeServer || globalReset)
            {
                SendMessage(Mabu2hService.SendMabu0(Character.Id));
            }
            if (infoSkill.socolaMabu.isSocola && infoSkill.socolaMabu.Time <= timeServer || globalReset)
            {
                RemoveSocolaMabu(Character);
            }
            if ((infoSkill.HoaBang.isHoaBang && infoSkill.HoaBang.Time <= timeServer) || globalReset)
            {
                RemoveHoaBang(Character);
            }
            if ((infoSkill.Protect.IsProtect && infoSkill.Protect.Time <= timeServer) || globalReset)
            {
                SkillHandler.RemoveProtect(Character);
                if (globalReset && infoSkill.Protect.IsProtect)
                {
                    SendMessage(Service.ItemTime(DataCache.TimeProtect[0], 0));
                }
            }

            if (infoSkill.HuytSao.IsHuytSao && infoSkill.HuytSao.Time <= timeServer)
            {
                SkillHandler.RemoveHuytSao(Character);
            }
            if (infoSkill.MeTroi.IsMeTroi) // mình là người trói
            {
                var monsterMap = infoSkill.MeTroi.Monster;
                var charTemp = infoSkill.MeTroi.Character; // nhân vật bị mình trói
                
                if (globalReset)
                {
                    SkillHandler.RemoveTroi(Character);
                }
                if (monsterMap is { IsDie: true })
                {
                    SkillHandler.RemoveTroi(Character);
                }else if (charTemp == null)
                {
                    Character.CharacterHandler.SendZoneMessage(Service.SkillEffectPlayer(Character.Id, Character.Id, 2, 32));
                    Character.InfoSkill.MeTroi.IsMeTroi = false;
                    Character.InfoSkill.MeTroi.Monster = null;
                    Character.InfoSkill.MeTroi.Character = null;
                    Character.InfoSkill.MeTroi.DelayStart = -1;
                    Character.InfoSkill.MeTroi.TimeTroi = -1;
                }
                else if ((charTemp != null && charTemp.InfoChar.IsDie))
                {
                    SkillHandler.RemoveTroi(Character);
                }
                else if (infoSkill.MeTroi.TimeTroi <= timeServer || monsterMap is { IsDie: true } || charTemp != null && charTemp.InfoChar.IsDie)
                {
                    SkillHandler.RemoveTroi(Character);
                }
            }

            if (infoSkill.PlayerTroi.IsPlayerTroi || globalReset) // mình là người bị trói
            {
                if (globalReset && infoSkill.PlayerTroi.IsPlayerTroi)
                {
                    SendMessage(Service.ItemTime(DataCache.TimeTroi[0], 0));
                    for (int i = 0; i < infoSkill.PlayerTroi.PlayerId.Count; i++) {
                        SkillHandler.RemoveTroi(Character.Zone.ZoneHandler.GetCharacter(infoSkill.PlayerTroi.PlayerId[i]));
                }
                }
                else if (infoSkill.PlayerTroi.IsPlayerTroi && infoSkill.PlayerTroi.TimeTroi <= timeServer)
                {
                    infoSkill.PlayerTroi.IsPlayerTroi = false;
                    infoSkill.PlayerTroi.TimeTroi = -1;
                    infoSkill.PlayerTroi.PlayerId.Clear();
                    SkillHandler.RemoveTroi(Character);
                }
            }

            if (infoSkill.DichChuyen.IsStun && infoSkill.DichChuyen.Time <= timeServer || globalReset)
            {
                SkillHandler.RemoveDichChuyen(Character);
            }

            if (infoSkill.ThaiDuongHanSan.IsStun && infoSkill.ThaiDuongHanSan.Time <= timeServer || globalReset)
            {
                SkillHandler.RemoveThaiDuongHanSan(Character);
            }

            if (infoSkill.ThoiMien.IsThoiMien && infoSkill.ThoiMien.Time <= timeServer || globalReset)
            {
                SkillHandler.RemoveThoiMien(Character);
                if (globalReset && infoSkill.ThoiMien.IsThoiMien)
                {
                    SendMessage(Service.ItemTime(DataCache.TimeThoiMien[0], 0));
                }
            }

            if (infoSkill.Socola.IsSocola && infoSkill.Socola.Time <= timeServer || globalReset)
            {
                SkillHandler.RemoveSocola(Character);
                if (globalReset && infoSkill.Socola.IsSocola)
                {
                    SendMessage(Service.ItemTime(3780, 0));
                }
            }
        }

        public void UpdateAutoPlay(long timeServer)
        {
            if (Character.InfoChar.TimeAutoPlay > 0 && Character.Delay.AutoPlay <= timeServer)
            {
                if (Character.InfoChar.TimeAutoPlay < timeServer || Character.InfoChar.TimeAutoPlay < 0)
                {
                    Character.InfoChar.TimeAutoPlay = 0;
                    SendMessage(Service.ItemTime(4387, 0));
                    SendMessage(Service.AutoPlay(false));
                }
                Character.Delay.AutoPlay = 60000 + timeServer;
            }
        }

        public void UpdateOther(long timeServer)
        {   if (Character.InfoChar.MapId == 126) //Ra khỏi map khi hết giờ
                {
                    var now = ServerUtils.TimeNow().Hour;
                    if (now == 0||now == 1||now == 2||now == 3||now == 4||now == 5||now == 6||now == 7||now == 8||now == 9||now == 10||now == 11||now == 12||now == 13||now == 14||now == 15||now == 16||now == 17||now == 18||now == 19||now == 20||now == 21||now == 23||now == 24)
                    {
                       BackHome();
                       SendMessage(Service.ServerMessage("Hẹn bạn vào 22h hằng ngày nhé!")); 
                    }
                }
            //   #region Phó bản Clan
            if (Character.DataPractice.Status is Extension.Practice.Practice_Staus.PRACTICE)
            {
                Practice_Handler.gI().UpdatePractice(Character, timeServer);
            }
            if (Mabu2hConfig.Status != Mabu2hStatus.DURING && DataCache.IdMapMabu2.Contains(Character.InfoChar.MapId)){
                MapManager.JoinMap(Character, 24 + Character.InfoChar.Gender, ServerUtils.RandomNumber(20), true, true, Character.TypeTeleport);
            }
            if (Mabu12hConfig.Status != Mabu12hStatus.DURING && DataCache.IdMapMabu.Contains(Character.InfoChar.MapId))
            {
                MapManager.JoinMap(Character, 24 + Character.InfoChar.Gender, ServerUtils.RandomNumber(20), true, true, Character.TypeTeleport);
            }
            if (Character.DataDaiHoiVoThuat23.Handler.CheckStatusPk(Character))
            {
                Character.DataDaiHoiVoThuat23.Handler.Update(Character, timeServer);
            }
            if (Character.DataSieuHang.Status is Extension.DaiHoiVoThuat.SuperChampion.SuperChampion_Championer_Status.BATTLE)
            {
                Character.DataSieuHang.Handler.Update(Character, timeServer);
            }
            if (Character.Challenge.isChallenge && Character.Challenge.PlayerChallengeID != -1)
            {
                Character.Challenge.Runtime(Character, timeServer);
            }
            if (BlackballCache.ListMapNRSD.Contains(Character.InfoChar.MapId))
            {
                Character.Blackball.Runtime(Character, timeServer);
            }
            for (int i = 0; i < Character.EffectTemporaries.Count; i++)
            {
                Character.EffectTemporaries[i].Update(timeServer);
            }
        }
        public void UpdateVeTinh(long timeServer)
        {
            var effect = Character.Effect;
            // Vệ tinh trí lực

            if (Character.InfoMore.IsNearAuraTriLucItem && effect.AuraBuffKi30S.Time <= timeServer && Character.InfoChar.Mp < Character.MpFull)
            {
                PlusMp((int)(Character.MpFull * 5 / 100));
                SendMessage(Service.SendMp((int)Character.InfoChar.Mp));
                effect.AuraBuffKi30S.Time = 30000 + timeServer;
                Character.InfoMore.IsNearAuraTriLucItem = false;
            }

            if (Character.InfoMore.IsNearAuraSinhLucItem && effect.AuraBuffHp30S.Time <= timeServer && Character.InfoChar.Hp < Character.HpFull)
            {
                PlusHp((int)(Character.HpFull * 5 / 100));
                SendMessage(Service.SendHp((int)Character.InfoChar.Hp));
                SendZoneMessage(Service.PlayerLevel(Character));
                effect.AuraBuffHp30S.Time = 30000 + timeServer;
                Character.InfoMore.IsNearAuraSinhLucItem = false;
            }
            //if ()

            if (effect.BuffHp30S.Value > 0 && effect.BuffHp30S.Time <= timeServer && Character.InfoChar.Hp < Character.HpFull)
            {
                PlusHp(effect.BuffHp30S.Value);
                SendMessage(Service.SendHp((int)Character.InfoChar.Hp));
                SendZoneMessage(Service.PlayerLevel(Character));
                effect.BuffHp30S.Time = 30000 + timeServer;
            }
            if (effect.BuffKi30S.Value > 0 && effect.BuffKi30S.Time <= timeServer && Character.InfoChar.Mp < Character.MpFull)
            {
                PlusMp(effect.BuffKi30S.Value);
                SendMessage(Service.SendMp((int)Character.InfoChar.Mp));
                effect.BuffKi30S.Time = 30000 + timeServer;
            }

            if (effect.BuffKi1s.Value > 0 && effect.BuffKi1s.Time <= timeServer && Character.InfoChar.Mp < Character.MpFull)
            {
                PlusMp(effect.BuffKi1s.Value);
                SendMessage(Service.SendMp((int)Character.InfoChar.Mp));
                effect.BuffKi1s.Time = 1500 + timeServer;
            }
        }
        public void UpdateEffect(long timeServer)
        {
            
            // Effect giáp luyện tập
            // Nếu vừa tháo giáp luyện tập ra thì sẽ trừ thời gian
            if (Character.InfoMore.LastGiapLuyenTapItemId != 0 && Character.Delay.GiapLuyenTap != -1 && Character.Delay.GiapLuyenTap < timeServer)
            {
                var giapLuyenTap = GetItemBagById(Character.InfoMore.LastGiapLuyenTapItemId);
                if (giapLuyenTap != null && ItemCache.ItemIsGiapLuyenTap(giapLuyenTap.Id))
                {
                    var optionCheck = giapLuyenTap.Options.FirstOrDefault(option => option.Id == 9);
                    if ((optionCheck.Param - 1) > 0)
                    {
                        optionCheck.Param -= 1;
                        SendMessage(Service.SendBody(Character));
                        Character.Delay.GiapLuyenTap = 60000 + timeServer;
                    }
                    else
                    {
                        optionCheck.Param = 0;
                        Character.InfoMore.LastGiapLuyenTapItemId = 0;
                        SendMessage(Service.SendBody(Character));
                        UpdateInfo(true);
                        Character.Delay.GiapLuyenTap = -1;
                    }
                }
                else
                {
                    Character.InfoMore.LastGiapLuyenTapItemId = 0;
                    Character.Delay.GiapLuyenTap = -1;
                }
            }

            bool IsRemoveBuffEffect = false;
            // Effect thức ăn
            if (Character.InfoBuff.BanhTrungThuTime < timeServer && Character.InfoBuff.BanhTrungThuId > -1)
            {
                Character.InfoBuff.BanhTrungThuId = -1;
                IsRemoveBuffEffect = true;
            }
            if (Character.InfoBuff.timeBanhTet < timeServer && Character.InfoBuff.BanhTet)
            {
                Character.InfoBuff.BanhTet = false;
                IsRemoveBuffEffect = true;
            }
            if (Character.InfoBuff.timeBanhChung < timeServer && Character.InfoBuff.BanhChung)
            {
                Character.InfoBuff.BanhChung = false;
                IsRemoveBuffEffect = true;
            }
            if (Character.InfoBuff.KichDucX2Time < timeServer && Character.InfoBuff.KichDucX2)
            {
                Character.InfoBuff.KichDucX2 = false;
                IsRemoveBuffEffect = true;
            }
            if (Character.InfoBuff.KichDucX5Time < timeServer && Character.InfoBuff.KichDucX5)
            {
                Character.InfoBuff.KichDucX2 = false;
                IsRemoveBuffEffect = true;
            }
            if (Character.InfoBuff.KichDucX7Time < timeServer && Character.InfoBuff.KichDucX7)  
            {
                Character.InfoBuff.KichDucX2 = false;
                IsRemoveBuffEffect = true;
            }
            // Effect thức ăn
            if (Character.InfoBuff.ThucAnTime < timeServer && Character.InfoBuff.ThucAnId > -1)
            {
                Character.InfoBuff.ThucAnId = -1;
                IsRemoveBuffEffect = true;
            }
            if (Character.InfoBuff.timeEnchantCrit < timeServer && Character.InfoBuff.isEnchantCrit && Character.InfoBuff.isActiveCrit)
            {
                Character.InfoBuff.isEnchantCrit = false;
                IsRemoveBuffEffect = true;
            }
            if (Character.InfoChar.Roles1.RoleUsed != null)
            {
                var role = Character.InfoChar.Roles1.RoleUsed;
                if (role.Delay < timeServer)
                {
                    role.Delay = 10000 + timeServer;
                    Character.CharacterHandler.SendZoneMessage(Service.SendRole(Character.Id, role.Second, role.Temp));
                }
            }
            if (Character.InfoBuff.delayEnchantCrit < timeServer && !Character.InfoBuff.isEnchantCrit && Character.InfoBuff.isActiveCrit) 
            {
                Character.InfoBuff.delayEnchantCrit = 25000 + timeServer;
                Character.InfoBuff.timeEnchantCrit = 5000 + timeServer;
                Character.InfoBuff.isEnchantCrit = true;
                Character.CharacterHandler.SendMessage(Service.ItemTimeWithMessage($"Chí mạng +5%", 100, 5));
                IsRemoveBuffEffect = true;
            }

            if (Character.InfoBuff.timeEnchantGiap < timeServer && Character.InfoBuff.isEnchantGiap && Character.InfoBuff.isActiveGiap)
            {
                Character.InfoBuff.isEnchantGiap = false;
                IsRemoveBuffEffect = true;
            }
            if (Character.InfoBuff.delayEnchantGiap < timeServer && !Character.InfoBuff.isEnchantGiap && Character.InfoBuff.isActiveGiap)
            { 
                Character.InfoBuff.delayEnchantGiap = 30000 + timeServer;
                Character.InfoBuff.timeEnchantGiap = 10000 + timeServer;
                Character.InfoBuff.isEnchantGiap = true;
                Character.CharacterHandler.SendMessage(Service.ItemTimeWithMessage($"Giáp +15%", 100, 10));
                IsRemoveBuffEffect = true;
            }
            if (Character.InfoBuff.XiMuoiHoaDaoTime < timeServer && Character.InfoBuff.XiMuoiHoaDao)
            {
                Character.InfoBuff.XiMuoiHoaDao = false;
                IsRemoveBuffEffect = true;
            }
            if (Character.InfoBuff.XiMuoiHoaMaiTime < timeServer && Character.InfoBuff.XiMuoiHoaMai)
            {
                Character.InfoBuff.XiMuoiHoaMai = false;
                IsRemoveBuffEffect = true;
            }
            if (Character.InfoBuff.MayDoLinhHonTime < timeServer && Character.InfoBuff.MayDoLinhHon)
            {
                Character.InfoBuff.MayDoLinhHon = false;
            }
            if (Character.InfoBuff.KhauTrangTime < timeServer && Character.InfoBuff.KhauTrang)
            {
                Character.InfoBuff.KhauTrang = false;
            }
            // Effect cuồng nộ
            if (Character.InfoBuff.CuongNoTime < timeServer && Character.InfoBuff.CuongNo)
            {
                Character.InfoBuff.CuongNo = false;
                IsRemoveBuffEffect = true;
            }
            // Effect Bổ huyết
            if (Character.InfoBuff.BoHuyetTime < timeServer && Character.InfoBuff.BoHuyet)
            {
                Character.InfoBuff.BoHuyet = false;
                IsRemoveBuffEffect = true;
            }
            // Effect Bo Khi
            if (Character.InfoBuff.BoKhiTime < timeServer && Character.InfoBuff.BoKhi)
            {
                Character.InfoBuff.BoKhi = false;
                IsRemoveBuffEffect = true;
            }
            // Effect giap xen
            if (Character.InfoBuff.GiapXenTime < timeServer && Character.InfoBuff.GiapXen)
            {
                Character.InfoBuff.GiapXen = false;
                IsRemoveBuffEffect = true;
            }
            if (Character.InfoBuff.effRongXuongTime < timeServer && Character.InfoBuff.effRongXuong)
            {
                Character.InfoBuff.effRongXuong = false;
                IsRemoveBuffEffect = true;
            }
            if (Character.InfoBuff.BinhChuaCommesonTime < timeServer && Character.InfoBuff.BinhChuaCommeson)
            {
                Character.InfoBuff.BinhChuaCommeson = false;
                IsRemoveBuffEffect = true;
            }
            // Effect An danh
            if (Character.InfoBuff.AnDanhTime < timeServer && Character.InfoBuff.AnDanh)
            {
                Character.InfoBuff.AnDanh = false;
                IsRemoveBuffEffect = true;
            }
            if (Character.InfoBuff.CuongNoTime2 < timeServer && Character.InfoBuff.CuongNo2)
            {
                Character.InfoBuff.CuongNo2 = false;
                IsRemoveBuffEffect = true;
            }
            // Effect Bổ huyết
            if (Character.InfoBuff.BoHuyetTime2 < timeServer && Character.InfoBuff.BoHuyet2)
            {
                Character.InfoBuff.BoHuyet2 = false;
                IsRemoveBuffEffect = true;
            }
            // Effect Bo Khi
            if (Character.InfoBuff.BoKhiTime2 < timeServer && Character.InfoBuff.BoKhi2)
            {
                Character.InfoBuff.BoKhi2 = false;
                IsRemoveBuffEffect = true;
            }
            // Effect giap xen
            if (Character.InfoBuff.GiapXenTime2 < timeServer && Character.InfoBuff.GiapXen2)
            {
                Character.InfoBuff.GiapXen2 = false;
                IsRemoveBuffEffect = true;
            }
            // Effect An danh
            if (Character.InfoBuff.AnDanhTime2 < timeServer && Character.InfoBuff.AnDanh2)
            {
                Character.InfoBuff.AnDanh2 = false;
                IsRemoveBuffEffect = true;
            }
            // Effect Do CSKB
            if (Character.InfoBuff.MayDoCSKBTime < timeServer && Character.InfoBuff.MayDoCSKB)
            {
                Character.InfoBuff.MayDoCSKB = false;
            }
            
            if (IsRemoveBuffEffect)
            {
                SetUpInfo(false);
                SendMessage(Service.MeLoadPoint(Character));
            }
       
        }

        public void UpdateEffectTemporary(long timeServer)
        {
            var item = Character.ItemBody[5];
            if (item == null) return;
            var selectOptionEffectTemporary = item.Options.Where(i => DataCache.IdOptionSpecialEffectTemporary.Contains(i.Id)).ToList();
            for (int i = 0; i < selectOptionEffectTemporary.Count; i++)
            {
                var option = selectOptionEffectTemporary[i];
                switch (option.Id)
                {
                    case 109:
                        if (Character.Delay.DelayODo < timeServer)
                        {
                            Character.Delay.DelayODo = 11000 + timeServer;
                            Character.Zone.Characters.Values.ToList().ForEach(temp =>
                            {
                                if (temp.Id == Character.Id || Math.Abs(temp.InfoChar.X - Character.InfoChar.X) > 450) return;

                                if (!temp.EffectTemporaries.Any(effect => effect is DirtyMineHp))
                                {
                                    temp.EffectTemporaries.Add(new DirtyMineHp(temp, Character, option.Param));

                                }
                            });
                        }
                        break;
                    case 117:
                        Character.Zone.Characters.Values.ToList().ForEach(temp =>
                        {
                            if (!temp.EffectTemporaries.Any(effect => effect is BeautifulPlusDamage))
                            {
                                temp.EffectTemporaries.Add(new BeautifulPlusDamage(temp, Character, option.Param));
                                temp.CharacterHandler.SetUpInfo(true);
                                temp.CharacterHandler.SendMessage(Service.MeLoadPoint(temp));
                            }
                        });
                        break;
                    case 226:
                        Character.Zone.Characters.Values.ToList().ForEach(temp =>
                        {
                            if (!temp.EffectTemporaries.Any(effect => effect is CutePlusDamage))
                            {
                                temp.EffectTemporaries.Add(new CutePlusDamage(temp, Character, option.Param));
                                temp.CharacterHandler.SetUpInfo(true);
                                temp.CharacterHandler.SendMessage(Service.MeLoadPoint(temp));
                            }
                        });
                        break;
                }
            }           
        }
        public void UpdateMask()
        {
            bool sendMessage = false;
            bool setNormal = false;

            switch (Character.InfoChar.MapId)
            {
                case int i when DataCache.IdMapCold.Contains(i):
                    if (!Character.InfoOption.ChongLanh)
                    {
                        Character.isColer = true;
                        sendMessage = true;
                        Character.CharacterHandler.SetUpInfo(true);
                        Character.CharacterHandler.SendMessage(Service.MeLoadPoint(Character));
                    }
                    else
                    {
                        Character.isColer = false;
                        sendMessage = true;
                        Character.CharacterHandler.SetUpInfo(true);
                        Character.CharacterHandler.SendMessage(Service.MeLoadPoint(Character));
                    }
                    break;
                case int i when DataCache.IdMapTuongLai.Contains(i):
                    if (!Character.InfoBuff.KhauTrang)
                    {
                        Character.isFuture = true;
                        sendMessage = true;
                        Character.CharacterHandler.SetUpInfo(true);
                        Character.CharacterHandler.SendMessage(Service.MeLoadPoint(Character));
                    }
                    else
                    {
                        Character.isFuture = false;
                        sendMessage = true;
                        Character.CharacterHandler.SetUpInfo(true);
                        Character.CharacterHandler.SendMessage(Service.MeLoadPoint(Character));
                    }
                    break;
                default:
                    if (Character.DataEnchant.PhuHoMabu2h && Character.InfoChar.MapId != 127)
                    {
                        Character.DataEnchant.PhuHoMabu2h = false;
                        sendMessage = true;
                        Character.CharacterHandler.SetUpInfo(true);
                        Character.CharacterHandler.SendMessage(Service.MeLoadPoint(Character));
                    }
                    if (Character.isColer || Character.isFuture)
                    {
                        setNormal = true;
                        sendMessage = true;
                    }
                    break;
            }

            if (sendMessage)
            {
                Character.CharacterHandler.SendMessage(Service.ServerMessage("Mọi thứ đã trở lại bình thường"));
            }

            if (setNormal)
            {
                Character.isColer = false;
                Character.isFuture = false;
                Character.CharacterHandler.SetUpInfo(true);
                Character.CharacterHandler.SendMessage(Service.MeLoadPoint(Character));
            }
        }

        public void UpdateLuyenTap()
        {
            var timeServer = ServerUtils.CurrentTimeMillis();
            if (Character.Delay.TrainGiapLuyenTap > timeServer) return;
            var item = Character.ItemBody[6];
            if (item == null || !ItemCache.ItemIsGiapLuyenTap(item.Id)) return;
            var optionCheck = item.Options.FirstOrDefault(option => option.Id == 9);
            if ((optionCheck.Param + 1) <= ItemCache.GetGiapLuyenTapLimit(item.Id))
            {
                optionCheck.Param += 1;
                SendMessage(Service.SendBag(Character));
            }
            Character.Delay.TrainGiapLuyenTap = 60000 + timeServer;
        }

        private void UpdateFusion(long timeServer)
        {
            var fusion = Character.InfoChar.Fusion;
            var disciple = Character.Disciple;
            if (disciple is { Status: 4 } && fusion.IsFusion)
            {
                // Update đệ đang hợp thể
                // if (disciple.InfoSkill.HuytSao.IsHuytSao && disciple.InfoSkill.HuytSao.Time <= timeServer)
                // {
                //     disciple.InfoSkill.HuytSao.IsHuytSao = false;
                //     disciple.InfoSkill.HuytSao.Time = -1;
                //     disciple.InfoSkill.HuytSao.Percent = 0;
                //     disciple.CharacterHandler.SetHpFull();

                //     if (disciple.InfoChar.Hp >= disciple.HpFull)
                //     {
                //         disciple.InfoChar.Hp = disciple.HpFull;
                //     }

                //     SetHpFull();
                //     if (Character.InfoChar.Hp >= Character.HpFull)
                //     {
                //         Character.InfoChar.Hp = Character.HpFull;
                //     }
                //     SendMessage(Service.MeLoadPoint(Character));
                //     SendZoneMessage(Service.PlayerLevel(Character));
                // }

                if (disciple.InfoSkill.Monkey.MonkeyId == 1 && disciple.InfoSkill.Monkey.TimeMonkey <= timeServer)
                {
                    // reset lại máu sư phụ
                    disciple.InfoSkill.Monkey.MonkeyId = 0;
                    disciple.InfoSkill.Monkey.HeadMonkey = -1;
                    disciple.InfoSkill.Monkey.BodyMonkey = -1;
                    disciple.InfoSkill.Monkey.LegMonkey = -1;
                    disciple.InfoSkill.Monkey.TimeMonkey = -1;
                    disciple.CharacterHandler.SetUpInfo();
                    SetUpInfo();
                    if (Character.InfoChar.Hp >= Character.HpFull)
                    {
                        Character.InfoChar.Hp = Character.HpFull;
                    }
                    SendMessage(Service.MeLoadPoint(Character));
                    SendZoneMessage(Service.PlayerLevel(Character));
                }

                if (timeServer >= fusion.TimeStart + fusion.TimeUse && (!fusion.IsPorata && !fusion.IsPorata2))
                {
                    disciple.CharacterHandler.SetUpPosition(isRandom: true);
                    Character.Zone.ZoneHandler.AddDisciple(disciple);
                    SendZoneMessage(Service.Fusion(Character.Id, 1));
                    lock (Character.InfoChar.Fusion)
                    {
                        Character.InfoChar.Fusion.IsFusion = false;
                        Character.InfoChar.Fusion.IsPorata = false;
                        Character.InfoChar.Fusion.IsPorata2 = false;
                        Character.InfoChar.Fusion.TimeStart = timeServer;
                        Character.InfoChar.Fusion.DelayFusion = timeServer + 600000;
                        Character.InfoChar.Fusion.TimeUse = 0;
                    }

                    disciple.Status = 0;
                    SetUpInfo();
                    SendZoneMessage(Service.UpdateBody(Character));
                    SendMessage(Service.SendBody(Character));
                    SendMessage(Service.PlayerLoadSpeed(Character));
                    SendMessage(Service.MeLoadPoint(Character));
                    SendMessage(Service.SendHp((int)Character.InfoChar.Hp));
                    SendMessage(Service.SendMp((int)Character.InfoChar.Mp));
                    SendZoneMessage(Service.PlayerLevel(Character));
                }
            }
        }

        public void SetUpPosition(int mapOld, int mapNew, bool isRandom = false)
        {
            PositionHandler.SetUpPosition(Character, mapOld, mapNew);
        }
        public void SetPos(short x, short y)
        {
            PositionHandler.SetupPositionXy(Character, x, y);
        }
        private void CheckExpireItem(Model.Item.Item item, int timeServer, int type)
        {
           
        }
        public int GetThoiVangInBag()
        {
            var quantity = 0;
            Character.ItemBag.Where(item => item != null && item.Id == 457).ToList().ForEach(item =>
            {
                quantity += item.Quantity;
            });
            return quantity;
        }
        public int GetThoiVangInRuong()
        {
            return -1;
        }

        public void SendInfo()
        {
            //Check hạn sử dụng item
            // Body Item
            var Second = ServerUtils.CurrentTimeSecond();
            
            Character.ItemBody.Where(item => item != null).ToList().ForEach(item =>
            {
                SetEnhancedOption(item, Second, 0, true);

            });
            Character.ItemBag.Where(item => item != null).ToList().ForEach(item =>
            {
                SetEnhancedOption(item, Second, 1);

            });
            // Box Item
            Character.ItemBox.Where(item => item != null).ToList().ForEach(item =>
            {
                SetEnhancedOption(item, Second, 2);

            });


            // LuckyBox Item
            //Check đệ tử
            if (Character.InfoChar.IsHavePet)
            {
                Character.Disciple = DiscipleDB.GetById(-Character.Id);
                if (Character.Disciple != null)
                {
                    Character.Disciple.Status = 0;
                    Character.Disciple.Character = Character;
                    Character.Disciple.Player = Character.Player;
                    Character.Disciple.CharacterHandler.SetUpPosition(isRandom: true);
                    Character.Disciple.CharacterHandler.UpdateInfo(true);
                }
                else
                {
                    Character.InfoChar.IsHavePet = false;
                    Character.InfoChar.Fusion.Reset();
                }
            }
            var itemBody = Character.ItemBody[9];

            if (itemBody != null)
            {
                Character.Pet = new Pet(itemBody.Id, Character);
            }

            if (Character.ClanId == -1)
            {
                Character.InfoChar.Bag = -1;
            }
            else
            {
                var clan = ClanManager.Get(Character.ClanId);
                if (clan?.ClanHandler.GetMember(Character.Id) != null)
                {
                    Character.InfoChar.Bag = (sbyte)clan.ImgId;
                }
                else
                {
                    Character.ClanId = -1;
                    Character.InfoChar.Bag = -1;
                }
            }

            if (Character.InfoChar.OSkill.Count == 0)
            {
                Character.InfoChar.OSkill = new List<sbyte>() { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            }

            if (Character.InfoChar.KSkill.Count == 0)
            {
                Character.InfoChar.KSkill = new List<sbyte>() { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            }
                   //         Server.Gi().Logger.Print("Litmit: " +Character.InfoChar.LitmitPower ,"red");
            if (Character.InfoChar.LitmitPower >= DataCache.MAX_LIMIT_POWER_LEVEL){
                //Server.Gi().Logger.Print("SetLitmit: " +DataCache.MAX_LIMIT_POWER_LEVEL );
                Character.InfoChar.LitmitPower = DataCache.MAX_LIMIT_POWER_LEVEL;
            }
            var maxPower = Cache.Gi().LIMIT_POWERS[DataCache.MAX_LIMIT_POWER_LEVEL].Power;
            if (Character.InfoChar.Power > maxPower)
            {
                Character.InfoChar.Power = maxPower;
            }

            Character.InfoChar.Level = (sbyte)(Cache.Gi().EXPS.Count(exp => exp < Character.InfoChar.Power) - 1);
            if (maxPower > Character.InfoChar.Power)
            {
                Character.InfoChar.IsPower = true;
            }
            else
            {
                Character.InfoChar.IsPower = false;
            }
            if (Character.InfoChar.PhukienPart == -1) SendMessage(Service.SendImageBag(Character.Id, Character.InfoChar.Bag));
            SendZoneMessage(Service.PlayerLoadAll(Character));
            SendMessage(Service.SendBox(Character));
            SendMessage(Service.SendBag(Character));
            SendMessage(Service.SendBody(Character));
           // SendMessage(Service.MeLoadPoint(Character));

            switch (Character.InfoChar.LitmitPower)
            {
                //case >= 13:
                //    Character.InfoChar.idEff_Set_Item = 11;
                //    break;
                //case >= 11:
                //    Character.InfoChar.idEff_Set_Item = 10;
                //    break;
                //case >= 10:
                //    Character.InfoChar.idEff_Set_Item = 9;
                //    break;
                case >= 9:
                    Character.InfoChar.idEff_Set_Item = 8;
                    break;
                case >= 8:
                    Character.InfoChar.idEff_Set_Item = 7;
                    break;
                case >= 7:
                    Character.InfoChar.idEff_Set_Item = 6;
                    break;
                case >= 6:
                    Character.InfoChar.idEff_Set_Item = 5;
                    break;
                case >= 5:
                    Character.InfoChar.idEff_Set_Item = 4;
                    break;
            }
            SendMessage(Service.SendTask(Character));
            SendMessage(Service.SpeacialSkill(Character, 0));
            SendMessage(Service.MyClanInfo(Character));
            SendMessage(Service.MeLoadAll(Character));
            SendMessage(Service.SendStamina(Character.InfoChar.Stamina));
            SendMessage(Service.SendMaxStamina(Character.InfoChar.MaxStamina));
            SendMessage(Service.SendNangDong(Character.InfoChar.NangDong));
            SendMessage(Service.GameInfo());
            SendMessage(Service.UpdateCooldown(Character));
            SendMessage(Service.ChangeOnSkill(Character.InfoChar.OSkill));
            var skill = Character.Skills.FirstOrDefault(sks => sks.Id == (Character.InfoChar.Gender == 0 ? 24 : Character.InfoChar.Gender == 1 ? 25 : 26));
            if (skill != null)
            {
                SendMessage(Service.UpdateSkill0((short)skill.SkillId, skill.CurrExp));
            }
            SendZoneMessage(Service.UpdateBody(Character));
            if (Character.InfoChar.TypePk != 0)
            {
                Character.InfoChar.TypePk = 0;
                SendZoneMessage(Service.ChangeTypePk(Character.Id, 0));
            }
            Character.DataDaiHoiVoThuat23.Handler.Reset(Character);
            ChampionShip.gI().Reset(Character);
            if (Character.InfoChar.IsHavePet && Character.Disciple != null)
            {
                SendMessage(Service.Disciple(1, null));
            }
            else
            {
                SendMessage(Service.Disciple(0, null));
            }
            UpdateMountId();
            UpdatePhukien();
            UpdateHaoQuangDacBiet();
            UpdateEffectCharacter();
            UpdateItem10();
            UpdateEffective();
            Character.UpdateOldMap();
            Character.SetupAmulet();
            UpdateInfo(true);
            if (Character.InfoChar.LuckyNumber.Number.Count > 1)
            {
                if (Character.InfoChar.LuckyNumber.JoinId == ConSoMayManHandler.gI().Config.RoomId)
                {
                    var text = "";
                    for (int i = 0; i < Character.InfoChar.LuckyNumber.Number.Count; i++)
                    {
                        if (i == 0) text += $"{Character.InfoChar.LuckyNumber.Number[i]}";
                        else if (i == Character.InfoChar.LuckyNumber.Number.Count - 1) text += $"{Character.InfoChar.LuckyNumber.Number[i]}";
                        else text += $", {Character.InfoChar.LuckyNumber.Number[i]} ";
                    }
                    Character.CharacterHandler.SendMessage(Service.ShowYourNumber0(text));
                }
                else
                {
                    Character.InfoChar.LuckyNumber.Number.Clear();
                    Character.InfoChar.LuckyNumber.NumberWin = -1;
                }
            }


        }
        public void SetUpPhoBan()
        {   
            var clan = ClanManager.Get(Character.ClanId);
            if (clan != null)
            {
                if (clan.ClanDungeon.BanDoKhoBau.CheckOpen())
                {
                    clan.ClanDungeon.BanDoKhoBau.SendTextTime(null, Character);
                }
                if (clan.ClanDungeon.KhiGasHuyDiet.CheckOpen())
                {
                    clan.ClanDungeon.KhiGasHuyDiet.SendTextTime(null, Character);
                }
                if (clan.ClanDungeon.DoanhTraiDocNhan.CheckOpen())
                {
                    clan.ClanDungeon.DoanhTraiDocNhan.SendTextTime(null, Character);
                }
                if (clan.ClanDungeon.ConDuongRanDoc.CheckOpen())
                {
                    clan.ClanDungeon.ConDuongRanDoc.SendTextTime(null, Character);
                }
                if (clan.ClanBoss.Status is (Model.Clan.ClanBoss.ClanBoss_Status.OPEN or Model.Clan.ClanBoss.ClanBoss_Status.END))
                {
                    clan.ClanBoss.SendTextTime(null, Character);

                }
            }
        } 
        public void SendDie()
        {
            lock (Character)
            {
                RemoveSkill(ServerUtils.CurrentTimeMillis(), true);
                Character.InfoChar.IsDie = true;
                Character.InfoSkill.Monkey.MonkeyId = 0;
                SetUpInfo();
                SendMessage(Service.PlayerLoadSpeed(Character));
                SendMessage(Service.MeLoadPoint(Character));
                SendMessage(Service.MeLoadInfo(Character));
                long minePower = 0;
                if (DataCache.IdMapTuongLai.Contains(Character.InfoChar.MapId))
                {
                    minePower = (long)(Character.InfoChar.Power * 20 / 100);
                }
                SendMessage(Service.MeDie(Character, minePower));
                SendZoneMessage(Service.PlayerDie(Character));
                LeaveGold();
                if (Character.DataNgocRongNamek.AlreadyPick(Character))
                {
                    var itm = new ItemMap(-1, ItemCache.GetItemDefault((short)(Character.DataNgocRongNamek.IdNamekBall)));
                    itm.X = Character.InfoChar.X;
                    itm.Y = Character.InfoChar.Y;
                    Character.Zone.ZoneHandler.LeaveItemMap(itm);
                    Character.InfoChar.TypePk = 0;
                    Character.DataNgocRongNamek.IdNamekBall = -1;
                    Character.InfoChar.Bag = ClanManager.Get(Character.ClanId) != null ? (sbyte)ClanManager.Get(Character.ClanId).ImgId : (sbyte)-1;
                    UpdatePhukien();
                }
                if (Character.Blackball.AlreadyPick(Character)){
                    Character.Blackball.ExitMapOrDie(Character);
                }
                if (Character.Challenge.isChallenge){
                    var player = (Model.Character.Character)ClientManager.Gi().GetCharacter(Character.Challenge.PlayerChallengeID);
                    var gold = (player.Challenge.Gold - (Character.Challenge.Gold / 100)) + (player.Challenge.Gold - (Character.Challenge.Gold / 100));
                    player.CharacterHandler.SendMessage(Service.ServerMessage($"Đối thủ đã kiệt sức,bạn đã nhận được {gold} vàng"));
                    player.PlusGold(gold);
                    player.CharacterHandler.SendMessage(Service.MeLoadInfo(player));
                    Character.Challenge.SetStatusEnd(Character);
                    player.Challenge.SetStatusEnd(player);
                }

                if (Character.DataNamecBattlefield.Status == Extension.NamecBattlefield.NamecBattlefield_Character_Status.FIGHTING)
                {
                    NamecBattlefield_Handler.OutOrDie(Character);
                }
                if (Character.Trade.IsTrade)
                {
                    var charTemp = (Model.Character.Character)Character.Zone.ZoneHandler.GetCharacter(Character.Trade.CharacterId);
                    if (charTemp != null && charTemp.Trade.CharacterId == Character.Id)
                    {
                        charTemp.CloseTrade(true);
                        charTemp.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().CLOSE_TRADE));
                    }
                    Character.CloseTrade(true);
                }
                
            }
        }
        public void PlusDiamondLock(int diamond)
        {
            Character.PlusDiamondLock(diamond);
            SendMessage(Service.MeLoadInfo(Character));
        }
        public void ClearTest()
        {
           
        }

        public void RemoveTroi(int charId)
        {
            var infoSkill = Character.InfoSkill.PlayerTroi;
            if (infoSkill.IsPlayerTroi)
            {
                infoSkill.PlayerId.RemoveAll(i => i == charId);
                if (infoSkill.PlayerId.Count <= 0)
                {
                    infoSkill.IsPlayerTroi = false;
                    infoSkill.TimeTroi = -1;
                    infoSkill.PlayerId.Clear();
                    SendZoneMessage(Service.SkillEffectPlayer(charId, Character.Id, 2, 32));
                }
            }
        }

        private void LeaveGold()
        {
            var quantity = Character.InfoChar.Gold switch
            {
                > 1000 and <= 500000 => Character.InfoChar.Gold / 30,
                > 500000 and <= 1000000 => ServerUtils.RandomNumber(10000, 20000),
                > 1000000 and <= 100000000 => ServerUtils.RandomNumber(20000, 30000),
                > 100000000 => ServerUtils.RandomNumber(30000, 50000),
                _ => 1
            };
            if (!Character.Player.IsActive) quantity = 1;
            var itemMap = LeaveItemHandler.LeaveGoldPlayer(Character.Id, (int)quantity);
            itemMap.X = Character.InfoChar.X;
            itemMap.Y = Character.InfoChar.Y;
            Character.Zone.ZoneHandler.LeaveItemMap(itemMap);
        }

        public void UpdateMountId()
        {
            var itemBag = Character.ItemBody[8];
            if (itemBag != null)
            {
                //var id = itemBag.Id;
                //id = id switch
                //{
                //    733 => 30001,
                //    734 => 30002,
                //    735 => 30003,
                //    743 => 30004,
                //    744 => 30005,
                //    746 => 30006,
                //    795 => 30007,
                //    849 => 30008,
                //    897 => 30009,
                //    920 => 30010,
                //    1092 => 30011,
                //    1139 => 30012,
                //    1144 => 30013,
                //    1172 => 30014,
                //    1159 => 30015,
                //    1260 => 30016,
                //    _ => id
                //};

                //Character.InfoChar.MountId = id;
                Character.InfoChar.MountId = (short)(ItemCache.ItemTemplate(itemBag.Id).Part + 30000);
            }

            else
            {
                Character.InfoChar.MountId = -1;
            }
        }
        public void UpdateItem10()
        {
            UpdateSachKiNang();
            //var itemBody = Character.ItemBody[10];
            //if (itemBody != null)
            //{
            //    var Linh_Thú_Template = Cache.Gi().LinhThu.Values.FirstOrDefault(a => a.Id == itemBody.Id);
            //    if (Linh_Thú_Template != null)
            //    {
            //        //   Character.InfoChar.Linh_Thú_ID = Linh_Thú_Template.Id;
            //        //   Character.InfoChar.LinhThuFrame = Linh_Thú_Template.Frame;
            //        //   Character.InfoChar.LinhThuImage = Linh_Thú_Template.IdImage;
            //        Character.CharacterHandler.SendZoneMessage(Epic_Pet.Call_EpicPet(Character, Linh_Thú_Template.IdImage, Linh_Thú_Template.Frame));
            //    }

            //}
            //else
            //{
            //    //  Character.InfoChar.Linh_Thú_ID = -1;
            //    Character.CharacterHandler.SendMessage(Epic_Pet.Remove_EpicPet(Character));
            //}
        }
        public void UpdatePet()
        {
            if (Character != null)
            {
                var itemBody = Character.ItemBody[9];
                    
                if (itemBody != null)
                {
                    var pet = Character.Pet;
                    if (pet != null)
                    {
                        Character.Zone.ZoneHandler.RemovePet(pet);
                        pet = new Pet(itemBody.Id, Character);
                        Character.Pet = pet;
                        Character.Pet.CharacterHandler.SetUpPosition(isRandom: true);
                        Character.InfoChar.PetId = itemBody.Id;
                        Character.Zone.ZoneHandler.AddPet(pet);
                    }
                    else
                    {
                        pet = new Pet(itemBody.Id, Character);
                        Character.Pet = pet;
                        Character.Pet.CharacterHandler.SetUpPosition(isRandom: true);
                        Character.InfoChar.PetId = itemBody.Id;
                        Character.Zone.ZoneHandler.AddPet(pet);
                    }
                }
            }
        }
        public void UpdateHaoQuangDacBiet()
        {
            var auraId = Character.InfoChar.EffectAuraId;
            if (Character.ItemBody[5] != null)
            {
                switch (Character.ItemBody[5].Id)
                {
                    case 1548:
                        auraId = 5;
                        break;
                }
            }
            if (auraId == -1) return;
            Character.CharacterHandler.SendMessage(Service.Radar4(Character.Id, auraId));

        }
        public void UpdateEffective()
        {
            var itemBody = Character.ItemBody[9];
            if (itemBody != null)
            {
                var timeServer = ServerUtils.CurrentTimeMillis();
                switch (itemBody.Id)
                {
                    case 1202:
                    case 1203:
                    case 1207:
                   
                        Character.InfoBuff.delayEnchantCrit = 20000 + timeServer;
                        Character.InfoBuff.isEnchantCrit = false;
                        Character.InfoBuff.isActiveCrit = true;
                        break;
                    case 1230:
                    case 1231:
                    case 1232:
                        Character.InfoBuff.delayEnchantGiap = 30000 + timeServer;
                        Character.InfoBuff.isEnchantGiap = false;
                        Character.InfoBuff.isActiveGiap = true;
                        break;
                }
            }
        }
        public void UpdateSachKiNang()
        {
            var skillNew = Character.Skills.FirstOrDefault(i => i.Id is (24 or 25 or 26));
            short skillId = -1;
            if (skillNew != null)
            {
                skillId = (short)skillNew.SkillId;
            }
            var itemBody = Character.ItemBody[10];
            if (itemBody != null)
            {
                Character.InfoChar.NewSkill.TypePaint = ItemCache.ItemTemplate(itemBody.Id).Part;
                if (itemBody.Id is 1314)
                {
                    Character.InfoChar.NewSkill.TypeItem = 2;
                }
                Character.CharacterHandler.SendMessage(Service.UpdateSkill1(skillId, (byte)Character.InfoChar.NewSkill.TypePaint));
            }
            else
            {
                Character.InfoChar.NewSkill.TypePaint = 1;
            }

        }
        public void UpdatePhukien()
        {
           
            switch (Character.InfoChar.Bag)
            {
                case 107:
                case 108:
                    Character.InfoChar.PhukienPart = Character.InfoChar.Bag;
                    Character.CharacterHandler.SendZoneMessage(Service.SendImageBag(Character.Id, Character.InfoChar.Bag));
                    break;
                default:
                    var itemBody = Character.ItemBody[7];
                    if (itemBody != null)
                    {
                        var itemTemplate = ItemCache.ItemTemplate(itemBody.Id);
                        Character.InfoChar.PhukienPart = itemTemplate.Part;
                        Character.CharacterHandler.SendZoneMessage(Service.SendImageBag(Character.Id, itemTemplate.Part));

                    }
                    else
                    {
                        Character.InfoChar.PhukienPart = -1;
                        Character.CharacterHandler.SendZoneMessage(Character.ClanId == -1 || Character.ClanId == -100
                            ? Service.SendImageBag(Character.Id, -1)
                            : Service.SendImageBag(Character.Id, Character.InfoChar.Bag));
                    }
                    break;
            }
        }
        public void UpdateEffectCharacter()
        {
            
            if (Character.InfoSet.IsFullSetNhatAn) Character.CharacterHandler.SendMessage(EffectCharacter.sendInfoEffChar((short)Character.Id, (short)56, (byte)1, -1, (short)10, 1));
            if (Character.InfoSet.IsFullSetTinhAn) Character.CharacterHandler.SendMessage(EffectCharacter.sendInfoEffChar((short)Character.Id, (short)57, (byte)1, -1, (short)10, 1));
            if (Character.InfoSet.IsFullSetNguyetAn) Character.CharacterHandler.SendMessage(EffectCharacter.sendInfoEffChar((short)Character.Id, (short)58, (byte)1, -1, (short)10, 1));
            
        }
        public bool GetParamItemExist(int id)
        {
            var exist = false;
            Character.ItemBody.Where(item => item != null).ToList().ForEach(item =>
            {
                if (item.Options.Where(option => option.Id == id).ToList().Count > 0) exist = true;
            });
            
            return exist;
        }
        public int GetCountItemLevel(byte level)
        {
            var count = 0;
            var itemsBody = CollectionsMarshal.AsSpan(Character.ItemBody.Where(item => item != null).ToList());
            for (int i = 0; i < itemsBody.Length; i++)
            {
                var item = itemsBody[i];
                if (ItemCache.ItemTemplate(item.Id).Level == level) count++;

            }
            return count;
        }
        public int GetParamItemExistCount(int id)
        {
            var count = 0;
            var itemsBody = CollectionsMarshal.AsSpan(Character.ItemBody.Where(item => item != null).ToList());
            for (int i = 0; i < itemsBody.Length; i++)
            {
                var item = itemsBody[i];
                if (item.isHaveOption(id)) count++;

            }
            return count;
        }
        public int GetParamItem(int id)
        {
            var param = 0;
            Character.ItemBody.Where(item => item != null).ToList().ForEach(item =>
            {
                var option = item.Options.Where(option => option.Id == id).ToList();
                param += option.Sum(optionItem => optionItem.Param);
            });
            Character.InfoChar.Cards.Values.Where(r => r.Used == 1).ToList().ForEach(r =>
            {
                foreach (var optionRadar in r.Options.Where(optionRadar => optionRadar.Id == id))
                {
                    if (optionRadar.ActiveCard == r.Level)
                    {
                        param += optionRadar.Param;
                    }
                    else if (r.Level == -1 && optionRadar.ActiveCard == 0)
                    {
                        param += optionRadar.Param;
                    }
                }
            });
            return param;
        }
        
        public List<int> GetListParamItem(int id)
        {
            var param = new List<int>();
            Character.ItemBody.Where(item => item != null).ToList().ForEach(item =>
            {
                var option = item.Options.Where(option => option.Id == id).ToList();
                param.AddRange(option.Select(optionItem => optionItem.Param));
            });
            Character.InfoChar.Cards.Values.Where(r => r.Used == 1).ToList().ForEach(r =>
            {
                foreach (var optionRadar in r.Options.Where(optionRadar => optionRadar.Id == id))
                {
                    if (optionRadar.ActiveCard == r.Level)
                    {
                        param.Add(optionRadar.Param);
                    }
                    else if (r.Level == -1 && optionRadar.ActiveCard == 0)
                    {
                        param.Add(optionRadar.Param);
                    }
                }
            });
            return param;
        }

        public void SetUpFriend()
        {
            Character.Me = new InfoFriend(Character);
            Character.Friends.ForEach(friend =>
            {
                var charCheck = (Model.Character.Character)ClientManager.Gi().GetCharacter(friend.Id);
                friend = charCheck != null ? new InfoFriend(charCheck) : CharacterDB.GetInfoCharacter(friend.Id);
            });
        }
        public void SetInfoEffectTemporaries()
        {
            for (int i = 0; i < Character.EffectTemporaries.Count; i++)
            {
                var effectTemporary = Character.EffectTemporaries[i];
                if (!effectTemporary.isEffective) continue;
                effectTemporary.SetupInfo();
            }
        }
        public void SetUpInfo(bool queryItem =false)
        {
            if (queryItem)
            {
                QueryItem();
                SetInfoBlackball();
                SetEnhancedOptionCard();
                SetInfoEffectTemporaries();
                SetEnhancedOptionRole();
            }
            SetInfoBuff();
            SetHpFull();
            SetMpFull();
            SetDamageFull();
            SetDefenceFull();
            SetCritFull();
            SetSpeed();
            SetBuffMp1s();
            SetBuffHp5s();
            SetBuffHp10s();
            SetBuffHp30s();
            SetBuffMp30s();
            
        //    SetTuDongLuyenTap();
           // SetInfoRole();
        }

        private void SetupPetIndex()
        {
           
        }

        private void SetEnhancedOptionRole()
        {
            Character.InfoChar.Roles1.Roles.ForEach(role =>
            {
                role.Options.ForEach(option =>
                {
                    switch (option.Id)
                    {
                        case 50:
                            Character.InfoOption.PhanTramDamage += option.Param;
                            break;
                        case 77:
                            Character.InfoOption.PhanTramHp += option.Param;
                            break;
                        case 103:
                            Character.InfoOption.PhanTramKi += option.Param;
                            break;
                        case 14:
                            Character.InfoOption.Crit += option.Param;
                            break;

                    }
                });
            });
        }
        public void SetEnhancedOptionCard()
        {
            Character.InfoChar.Cards.Values.Where(i => i.Used == 1).ToList().ForEach(card =>
            {
                card.Options.ForEach(option =>
                {
                    switch (option.Id)
                    {
                        case 50:
                            Character.InfoOption.PhanTramDamage += option.Param;
                            break;
                        case 77:
                            Character.InfoOption.PhanTramHp += option.Param;
                            break;
                        case 103:
                            Character.InfoOption.PhanTramKi += option.Param;
                            break;
                       
                    }
                });
            });
        }
        public void RestPlusHpMpFromDamage()
        {
            Character.MpPlusFromDamage = 0;
            Character.HpPlusFromDamage = 0;
            Character.HpPlusFromDamageMonster = 0;
        }
      
       
        public void SetEnhancedOption(Model.Item.Item item, long Second = 0, int type = 0, bool plusOption = false)
        {
            if (!plusOption)
            {
                item.Options.ForEach(option =>
                {
                    switch (option.Id)
                    {
                        case 93:
                            HandleExpiredItem(item, option, Second, type);
                            break;
                    }
                });
                return;
            }
            var ItemTemplate = ItemCache.ItemTemplate(item.Id);
            switch (ItemTemplate.Level)
            {
                case 13:
                    Character.InfoSet.UpdateSetInfoCounter(1, ref Character.InfoSet.CountSetThanLinh, ref Character.InfoSet.IsFullSetThanLinh);
                    break;
                case 14:
                    Character.InfoSet.UpdateSetInfoCounter(1, ref Character.InfoSet.CountSetHuyDiet, ref Character.InfoSet.IsFullSetHuyDiet);
                    break;
            }
            item.Options.ForEach(option =>
            {
                switch (option.Id)
                {
                   
                    case 7:
                        Character.InfoOption.Ki += option.Param;
                        break;
                    case 48:
                        Character.InfoOption.HpMp += option.Param;
                        break;
                    case 0:
                        Character.InfoOption.Damage += option.Param;
                        break;
                    case 50:
                        Character.InfoOption.PhanTramDamage += option.Param;
                        break;
                    case 6:
                        Character.InfoOption.Hp += option.Param;
                        break;
                    case 77:
                        Character.InfoOption.PhanTramHp += option.Param;
                        break;
                    case 14:
                    case 192:
                        Character.InfoOption.Crit += option.Param;
                        break;
                    case 103:
                        Character.InfoOption.PhanTramKi += option.Param;
                        break;
                    case 95:
                        Character.HpPlusFromDamage += option.Param;
                        break;
                    case 96:
                        Character.MpPlusFromDamage += option.Param;
                        break;
                    case 47:
                        Character.InfoOption.Defence += option.Param;
                        break;
                    case 94:
                        Character.InfoOption.PhanTramDefence += option.Param;
                        break;
                    case 159:
                        Character.InfoOption.PercentPlusDameKamejoko += option.Param;
                        break;
                    case 19:
                        Character.InfoOption.PlusDameToMonster += option.Param;
                        break;
                    case 156:
                        Character.InfoOption.CongDonDam = true;
                        break;
                    case 106:
                        Character.InfoOption.ChongLanh = true;
                        break;
                    case 34:
                        Character.InfoSet.UpdateSetInfoCounter(1, ref Character.InfoSet.CountSetTinhAn, ref Character.InfoSet.IsFullSetTinhAn);
                        break;
                    case 35:
                        Character.InfoSet.UpdateSetInfoCounter(1, ref Character.InfoSet.CountSetNguyetAn, ref Character.InfoSet.IsFullSetNguyetAn);
                        break;
                    case 36:
                        Character.InfoSet.UpdateSetInfoCounter(1, ref Character.InfoSet.CountSetNhatAn, ref Character.InfoSet.IsFullSetNhatAn);
                        break;
                    case 127:
                        Character.InfoSet.UpdateSetInfoCounter(1, ref Character.InfoSet.CountSetThienXinHang, ref Character.InfoSet.IsFullSetThienXinHang);
                        break;
                    case 128:
                        Character.InfoSet.UpdateSetInfoCounter(1, ref Character.InfoSet.CountSetKirin, ref Character.InfoSet.IsFullSetKirin);
                        break;
                    case 129:
                        Character.InfoSet.UpdateSetInfoCounter(1, ref Character.InfoSet.CountSetSongoku, ref Character.InfoSet.IsFullSetSongoku);

                        break;
                    case 130:
                        Character.InfoSet.UpdateSetInfoCounter(1, ref Character.InfoSet.CountSetPicolo, ref Character.InfoSet.IsFullSetPicolo);
                        break;
                    case 131:
                        Character.InfoSet.UpdateSetInfoCounter(1, ref Character.InfoSet.CountSetOcTieu, ref Character.InfoSet.IsFullSetOcTieu);
                        break;
                    case 132:
                        Character.InfoSet.UpdateSetInfoCounter(1, ref Character.InfoSet.CountSetPikkoro, ref Character.InfoSet.IsFullSetPikkoro);
                        break;
                    case 133:
                        Character.InfoSet.UpdateSetInfoCounter(1, ref Character.InfoSet.CountSetKakarot, ref Character.InfoSet.IsFullSetKakarot);
                        break;
                    case 134:
                        Character.InfoSet.UpdateSetInfoCounter(1, ref Character.InfoSet.CountSetCadic, ref Character.InfoSet.IsFullSetCadic);
                        break;
                    case 135:
                        Character.InfoSet.UpdateSetInfoCounter(1, ref Character.InfoSet.CountSetNappa, ref Character.InfoSet.IsFullSetNappa);
                        break;
                    
                    //-----------------------------------------------------------------------------------------//
                    case 80:
                        Character.InfoOption.PhanTramPlusHp30Second += option.Param;
                        break;
                    case 81:
                        Character.InfoOption.PhanTramPlusMp30Second += option.Param;
                        break;
                    case 2:
                        Character.InfoOption.HundredHPMP += option.Param;
                        break;
                    case 27:
                        Character.InfoOption.PlusHp30Second += option.Param;
                        break;
                    case 28:
                        Character.InfoOption.PlusMp30Second += option.Param;
                        break;

                    case 162:
                        Character.InfoOption.PlusMpEverySecond += option.Param;
                        break;
                    case 22:
                        Character.InfoOption.ThoundsandHP += option.Param;
                        break;
                    case 23:
                        Character.InfoOption.ThoundsandMP += option.Param;
                        break;
                    
                 
                    case 147:
                        Character.InfoOption.PhanTramDamage2 += option.Param;
                        break;
                    case 97:
                        Character.InfoOption.PhanPercentSatThuong += option.Param;
                        break;
                    case 98:
                        Character.InfoOption.PhanTramXuyenGiapChuong += option.Param;
                        break;
                    case 99:
                        Character.InfoOption.PhanTramXuyenGiapCanChien += option.Param;
                        break;
                    case 108:
                        Character.InfoOption.PhanTramNeDon += option.Param;
                        break;
                    case 100:
                        Character.InfoOption.PhanTramVangTuQuai += option.Param;
                        break;
                    case 101:
                        Character.InfoOption.PhanTramTNSM += option.Param;
                        break;
                    case 10:
                        Character.InfoOption.PercentChinhXac += option.Param;
                        break;
                    
                    case 5:
                        Character.InfoOption.PhanTramSatThuongChiMang += option.Param;
                        break;
                    case 3:
                        Character.InfoOption.PhanTramXuyenGiapChuong += option.Param;
                        break;
                    
                    case 155:
                        Character.InfoOption.X2TiemNang = true;
                        break;                    
                    case 104:
                        Character.HpPlusFromDamageMonster += option.Param;
                        break;
                    case 109:
                        Character.InfoOption.PhanTramGiamHp += option.Param;
                        break;
                    case 148:
                    case 114:
                    case 16:
                        Character.InfoOption.PhanTramSpeed += option.Param;
                        break;
                    case 178:
                        Character.InfoOption.PhanTramPlusMp10Second += option.Param;
                        break;
                    case 93:
                        HandleExpiredItem(item, option, Second, type);
                        break;
                }
            });
        }

        private void HandleExpiredItem(Model.Item.Item item, OptionItem option, long Second, int type)
        {
            var optionTimeExpire = item.Options.FirstOrDefault(i => i.Id == 73);
            if (optionTimeExpire == null) return;
            Server.Gi().Logger.Debug(optionTimeExpire.Param + " | " + Second + " | " + (optionTimeExpire.Param < Second));
            if (optionTimeExpire.Param < Second)// quá hạn sử dụng
            {
                switch (type)
                {
                    case 0: //body
                        RemoveItemBody(item.IndexUI);
                        break;
                    case 1:
                        RemoveItemBagByIndex(item.IndexUI, item.Quantity, false, reason: "Item hết hạn sử dụng");
                        break;
                    // Thêm các cases khác tương tự...
                    case 2:
                        {
                            RemoveItemBoxByIndex(item.IndexUI, item.Quantity, false);
                            break;
                        }
                    case 3:
                        {
                            RemoveItemLuckyBox(item.IndexUI, false);
                            break;
                        }
                }
            }
            else
            {
                var leftTime = optionTimeExpire.Param - Second;
                optionTimeExpire.Param = (int)(Second + leftTime);
                option.Param = ServerUtils.ConvertSecondToDay((int)leftTime);
                Server.Gi().Logger.Debug("leftTime: " + (optionTimeExpire.Param - Second) + "s");

            }

        }
        

        public void SetInfoBlackball()
        {
            var timeserver = ServerUtils.CurrentTimeMillis();
            for(int i = Character.Blackball.CurrentListBuff.Count - 1; i >= 0; i--)
{
                var blackball = Character.Blackball.CurrentListBuff[i];

                if (blackball.Time < timeserver)
                {
                    Character.Blackball.CurrentListBuff.Remove(blackball);
                }

                switch (blackball.Star)
                {
                    case 1:
                        Character.InfoOption.PhanTramDamage += 21;
                        break;
                    case 2:
                        Character.InfoOption.PhanTramHp += 35;
                        break;
                    case 3:
                        Character.InfoOption.PhanTramKi += 35;
                        break;
                    case 4:
                        Character.InfoOption.PhanTramDefence += 14;
                        break;
                    case 5:
                        Character.InfoOption.PhanTramNeDon += 14;
                        break;
                    case 6:
                    case 7:
                        Character.InfoOption.PhanTramTNSM += 35;
                        break;
                }
            }
        }
        public void SetInfoRole()
        {
           
        }
        public void SetInfoBuff()
        {
            var timeServer = ServerUtils.CurrentTimeMillis();
            
            if (Character.InfoChar.TimeAutoPlay > 0)
            {
                var giayConLai = (Character.InfoChar.TimeAutoPlay - timeServer) / 1000;
                if (giayConLai <= 0) giayConLai = 0;
                SendMessage(Service.ItemTime(4387, (int)giayConLai));
                SendMessage(Service.AutoPlay(true));
            }
            if (Character.InfoBuff.ThucAnTime > timeServer && Character.InfoBuff.ThucAnId != -1)
            {
                var giayConLai = (Character.InfoBuff.ThucAnTime - timeServer) / 1000;
                if (giayConLai <= 0) giayConLai = 0;
                var template = ItemCache.ItemTemplate(Character.InfoBuff.ThucAnId);
                SendMessage(Service.ItemTime(template.IconId, (int)giayConLai));
            }
            // Effect banh trung thu
            if (Character.InfoBuff.BanhTrungThuTime > timeServer && Character.InfoBuff.BanhTrungThuId != -1)
            {
                var giayConLai = (Character.InfoBuff.BanhTrungThuTime - timeServer) / 1000;
                if (giayConLai <= 0) giayConLai = 0;
                var template = ItemCache.ItemTemplate(Character.InfoBuff.BanhTrungThuId);
                SendMessage(Service.ItemTime(template.IconId, (int)giayConLai));
            }
            if (Character.InfoBuff.KichDucX2Time > timeServer)
            {
                var giayConLai = (Character.InfoBuff.KichDucX2Time - timeServer) / 1000;
                if (giayConLai <= 0) giayConLai = 0;
                SendMessage(Service.ItemTime(9374, (int)giayConLai));
            }
            if (Character.InfoBuff.KichDucX5Time > timeServer)
            {
                var giayConLai = (Character.InfoBuff.KichDucX5Time - timeServer) / 1000;
                if (giayConLai <= 0) giayConLai = 0;
                SendMessage(Service.ItemTime(9443, (int)giayConLai));
            }
            if (Character.InfoBuff.KichDucX7Time > timeServer)
            {
                var giayConLai = (Character.InfoBuff.KichDucX7Time - timeServer) / 1000;
                if (giayConLai <= 0) giayConLai = 0;
                SendMessage(Service.ItemTime(9444, (int)giayConLai));
            }
            // Effect cuồng nộ
            if (Character.InfoBuff.CuongNoTime > timeServer)
            {
                var giayConLai = (Character.InfoBuff.CuongNoTime - timeServer) / 1000;
                if (giayConLai <= 0) giayConLai = 0;
                SendMessage(Service.ItemTime(2754, (int)giayConLai));
            }
            if (Character.InfoBuff.MayDoLinhHonTime > timeServer)
            {
                var giayConLai = (Character.InfoBuff.MayDoLinhHonTime - timeServer) / 1000;
                if (giayConLai <= 0) giayConLai = 0;
                SendMessage(Service.ItemTime(11493, (int)giayConLai));
            }
            if (Character.InfoBuff.XiMuoiHoaDaoTime > timeServer)
            {
                var giayConLai = (Character.InfoBuff.XiMuoiHoaDaoTime - timeServer) / 1000;
                if (giayConLai <= 0) giayConLai = 0;
                SendMessage(Service.ItemTime(10905, (int)giayConLai));
            }
            if (Character.InfoBuff.XiMuoiHoaMaiTime > timeServer)
            {
                var giayConLai = (Character.InfoBuff.XiMuoiHoaMaiTime - timeServer) / 1000;
                if (giayConLai <= 0) giayConLai = 0;
                SendMessage(Service.ItemTime(10904, (int)giayConLai));
            }
            if (Character.InfoBuff.BinhChuaCommesonTime > timeServer)
            {
                var giayConLai = (Character.InfoBuff.BinhChuaCommesonTime - timeServer) / 1000;
                if (giayConLai <= 0) giayConLai = 0;
                SendMessage(Service.ItemTime(5829, (int)giayConLai));
            }
            // Effect Bổ huyết
            if (Character.InfoBuff.BoHuyetTime > timeServer)
            {
                var giayConLai = (Character.InfoBuff.BoHuyetTime - timeServer) / 1000;
                if (giayConLai <= 0) giayConLai = 0;
                SendMessage(Service.ItemTime(2755, (int)giayConLai));
            }
            // Effect Bo Khi
            if (Character.InfoBuff.BoKhiTime > timeServer)
            {
                var giayConLai = (Character.InfoBuff.BoKhiTime - timeServer) / 1000;
                if (giayConLai <= 0) giayConLai = 0;
                SendMessage(Service.ItemTime(2756, (int)giayConLai));
            }
            // Effect giap xen
            if (Character.InfoBuff.GiapXenTime > timeServer)
            {
                var giayConLai = (Character.InfoBuff.GiapXenTime - timeServer) / 1000;
                if (giayConLai <= 0) giayConLai = 0;
                SendMessage(Service.ItemTime(2757, (int)giayConLai));
            }
            // Effect An danh
            if (Character.InfoBuff.AnDanhTime > timeServer)
            {
                var giayConLai = (Character.InfoBuff.AnDanhTime - timeServer) / 1000;
                if (giayConLai <= 0) giayConLai = 0;
                SendMessage(Service.ItemTime(2760, (int)giayConLai));
            }



            if (Character.InfoBuff.CuongNoTime2 > timeServer)
            {
                var giayConLai = (Character.InfoBuff.CuongNoTime2 - timeServer) / 1000;
                if (giayConLai <= 0) giayConLai = 0;
                SendMessage(Service.ItemTime(10716, (int)giayConLai));
            }
            // Effect Bổ huyết
            if (Character.InfoBuff.BoHuyetTime2 > timeServer)
            {
                var giayConLai = (Character.InfoBuff.BoHuyetTime2 - timeServer) / 1000;
                if (giayConLai <= 0) giayConLai = 0;
                SendMessage(Service.ItemTime(10715, (int)giayConLai));
            }
            // Effect Bo Khi
            if (Character.InfoBuff.BoKhiTime2 > timeServer)
            {
                var giayConLai = (Character.InfoBuff.BoKhiTime2 - timeServer) / 1000;
                if (giayConLai <= 0) giayConLai = 0;
                SendMessage(Service.ItemTime(10714, (int)giayConLai));
            }
            // Effect giap xen
            if (Character.InfoBuff.GiapXenTime2 > timeServer)
            {
                var giayConLai = (Character.InfoBuff.GiapXenTime2 - timeServer) / 1000;
                if (giayConLai <= 0) giayConLai = 0;
                SendMessage(Service.ItemTime(10712, (int)giayConLai));
            }
            // Effect An danh
            if (Character.InfoBuff.AnDanhTime2 > timeServer)
            {
                var giayConLai = (Character.InfoBuff.AnDanhTime2 - timeServer) / 1000;
                if (giayConLai <= 0) giayConLai = 0;
                SendMessage(Service.ItemTime(10717, (int)giayConLai));
            }
            // Effect Do CSKB
            if (Character.InfoBuff.MayDoCSKBTime > timeServer)
            {
                var giayConLai = (Character.InfoBuff.MayDoCSKBTime - timeServer) / 1000;
                if (giayConLai <= 0) giayConLai = 0;
                SendMessage(Service.ItemTime(2758, (int)giayConLai));
            }
            // Effect Cu Ca Rot
            if (Character.InfoBuff.CuCarotTime > timeServer)
            {
                var giayConLai = (Character.InfoBuff.CuCarotTime - timeServer) / 1000;
                if (giayConLai <= 0) giayConLai = 0;
                SendMessage(Service.ItemTime(4082, (int)giayConLai));
            }
            if (Character.InfoBuff.TimeSatThuongChuan > timeServer)
            {
                var icon = ItemCache.ItemTemplate((short)Character.InfoBuff.SatThuongChuanId).IconId;
                var giayConLai = (Character.InfoBuff.TimeSatThuongChuan - timeServer) / 1000;
                if (giayConLai <= 0) giayConLai = 0;
                SendMessage(Service.ItemTime(icon, (int)giayConLai));
            }
        }
        public void QueryItem()
        {
            Character.InfoOption.Reset();
            Character.InfoSet.Reset();
            RestPlusHpMpFromDamage();
            var Milis = ServerUtils.CurrentTimeSecond();
            Character.ItemBody.Where(item => item != null).ToList().ForEach(item =>
            {
                SetEnhancedOption(item, Milis, 0, true);
            });
        }


        public void SetTuDongLuyenTap()
        {
           
        }

        public void SetHpFull()
        {
            var hp = Character.InfoChar.OriginalHp;
            hp += Character.InfoOption.HundredHPMP * 100;
            hp += Character.InfoOption.Hp;
            hp += Character.InfoOption.ThoundsandHP * 1000;
            hp += Character.InfoOption.HpMp;
            hp += hp * (Character.InfoOption.PhanTramHp) / 100; 
            hp -= hp * Character.InfoOption.PhanTramGiamHp / 100;
         //   GetListParamItem(77).ForEach(param => hp += (hp * param) / (param + 100));
         //   GetListParamItem(109).ForEach(param => hp -= hp * (param/100));
            if (Character.DataEnchant.PhuHoMabu2h)
            {
                hp += 1000000;
            }
            if (Character.InfoSet.IsFullSetNguyetAn)
            {
                hp += hp * 30 / 100;
            }
            if (Character.InfoBuff.XiMuoiHoaMai)
            {
                hp += hp * 20 / 100;
            }
            if (Character.InfoOption.X2TiemNang) hp /= 2;
            if (Character.Blackball.CurrentPercentPlusHp != -1 && Character.Blackball.AlreadyPick(Character)){
                hp *= Character.Blackball.CurrentPercentPlusHp;
            }
            if (Character.InfoChar.Fusion.IsFusion && Character.Disciple != null) {
                // Đệ ma bư +120%
                var disHP = Character.Disciple.HpFull;

                if (Character.Disciple.Type == 2 || Character.Disciple.Type == 3)
                {
                    hp += (disHP + (disHP * 20 / 100));
                }
                else if (Character.Disciple.Type == 4)
                {
                    hp += (disHP + (disHP * 40 / 100));
                }
                else if (Character.Disciple.Type == 5)
                {
                    hp += disHP * 2;
                }

                // Đệ thường +100%
                else if (Character.Disciple.Type == 1)
                {
                    hp += disHP;
                }
                // Bông tai porata 2
                if (Character.InfoChar.Fusion.IsPorata2)
                {
                    var bongTaiPorata2 = GetItemBagById(921);
                    if (bongTaiPorata2 != null)
                    {
                        var optionCheck = bongTaiPorata2.Options.FirstOrDefault(option => option.Id != 72);
                        if (optionCheck != null && optionCheck.Id == 77)
                        {
                            hp += hp * optionCheck.Param / 100;
                        }
                    }
                    // bông tai pt2 tăng 10%
                    hp += hp * 10 / 100;
                }
            }
           
           
            // Nappa
            if (Character.InfoSet.IsFullSetNappa)
            {
                hp += hp * 80 / 100;
            }
            if (Character.isColer)
            {
                hp -= hp / 2;
            }
            if (Character.InfoSkill.Monkey.MonkeyId != 0)
            {
                hp += hp * Character.InfoSkill.Monkey.Hp / 100;
            }

            if (Character.InfoSkill.HuytSao.IsHuytSao)
            {
                hp += hp * Character.InfoSkill.HuytSao.Percent / 100;
            }
            // Bổ Huyết
            if (Character.InfoBuff.BoHuyet)
            {
                hp += hp;
            }
            if (Character.InfoBuff.BoHuyet2)
            {
                hp += hp + (hp * 20 / 100);
            }
            if (Character.InfoBuff.BanhTrungThuId != -1)
            {
                switch (Character.InfoBuff.BanhTrungThuId)
                {
                    case 465:
                        {
                            hp += hp * 5 / 100;
                            break;
                        }
                    case 466:
                        {
                            hp += hp * 10 / 100;
                            break;
                        }
                    case 472:
                        {
                            hp += hp * 15 / 100;
                            break;
                        }
                    case 473:
                        {
                            hp += hp * 20 / 100;
                            break;
                        }
                }
            }


            Character.HpFull = hp;
        }

        public void SetMpFull()
        {
            var mp = Character.InfoChar.OriginalMp;
            mp += Character.InfoOption.HundredHPMP * 100;
            mp += Character.InfoOption.Ki;
            mp += Character.InfoOption.ThoundsandMP * 1000;
            mp += Character.InfoOption.HpMp;
            mp += (Character.InfoOption.PhanTramKi)*mp / 100;
            //GetListParamItem(103).ForEach(param => mp += mp * param / (300 + param));
            if (Character.DataEnchant.PhuHoMabu2h)
            {
                mp += 1000000;
            }
            if (Character.InfoSet.IsFullSetNhatAn)
                {
                    mp += mp * 30 / 100;
                }

            if (Character.InfoOption.X2TiemNang) mp /= 2;
            if (Character.InfoChar.Fusion.IsFusion && Character.Disciple != null) {
                // Đệ ma bư +120%
                if (Character.Disciple.Type == 2 || Character.Disciple.Type == 3)
                {
                    mp += (Character.Disciple.MpFull + (Character.Disciple.MpFull * 20 / 100));
                }
                if (Character.Disciple.Type == 4)
                {
                    mp += (Character.Disciple.MpFull + (Character.Disciple.MpFull * 40 / 100));
                }
                // Đệ thường +100%
                else if (Character.Disciple.Type == 1)
                {
                    mp += Character.Disciple.MpFull;
                }
                else if (Character.Disciple.Type == 5)
                {
                    mp += Character.Disciple.MpFull * 2;
                }
                // Bông tai porata 2
                if (Character.InfoChar.Fusion.IsPorata2)
                {
                    var bongTaiPorata2 = GetItemBagById(921);
                    if (bongTaiPorata2 != null)
                    {
                        var optionCheck = bongTaiPorata2.Options.FirstOrDefault(option => option.Id != 72);
                        if (optionCheck != null && optionCheck.Id == 103)
                        {
                            mp += mp * optionCheck.Param / 100;
                        }
                    }

                    mp += mp * 10 / 100;
                }
            }
            
            if (Character.isColer)
            {
                mp -= mp / 2;
            }
            if (Character.InfoBuff.BanhTrungThuId != -1)
            {
                switch (Character.InfoBuff.BanhTrungThuId)
                {
                    case 465:
                        {
                            mp += mp * 5 / 100;
                            break;
                        }
                    case 466:
                        {
                            mp += mp * 10 / 100;
                            break;
                        }
                    case 472:
                        {
                            mp += mp * 15 / 100;
                            break;
                        }
                    case 473:
                        {
                            mp += mp * 20 / 100;
                            break;
                        }
                }
            }

            // Bổ khí
            if (Character.InfoBuff.BoKhi)
            {
                mp += mp;
            }
            if (Character.InfoBuff.BoKhi2)
            {
                mp += mp + (mp * 20 /100);
            }
            Character.MpFull = mp;
        }

        
        public void SetDamageFull()
        {
            var damage = Character.InfoChar.OriginalDamage;
            damage += (int)Character.InfoOption.Damage;
            damage += (Character.InfoOption.PhanTramDamage) * damage / 100;
            damage += Character.InfoOption.PhanTramDamage2 * damage /100;
            if (Character.DataEnchant.PhuHoMabu2h)
            {
                damage += 100000;
            }
            if (Character.InfoOption.X2TiemNang) damage /= 2;
            if (Character.InfoSet.IsFullSetTinhAn)
            {
                damage += damage * 30 / 100;
            }
            if (Character.DataEnchant.MiNuong)
            {
                damage += damage * 24 / 100;
            }
            if (Character.InfoChar.Fusion.IsFusion && Character.Disciple != null) {
                // Đệ ma bư +120%
                var disDmg = Character.Disciple.DamageFull;

                if (Character.Disciple.Type == 2 || Character.Disciple.Type == 3)
                {
                    damage += (disDmg + (disDmg * 20 / 100));
                }
                if (Character.Disciple.Type == 4)
                {
                    damage += (disDmg + (disDmg * 40 / 100));
                }
                // Đệ thường +100%
                if (Character.Disciple.Type == 1)
                {
                    damage += disDmg;
                }
                if (Character.Disciple.Type == 5)
                {
                    damage += disDmg * 2;
                }
                if (Character.InfoBuff.effRongXuong)
                {
                    damage += damage * 20 / 100;
                }
                // Bông tai porata 2
                if (Character.InfoChar.Fusion.IsPorata2)
                {
                    var bongTaiPorata2 = GetItemBagById(921);
                    if (bongTaiPorata2 != null)
                    {
                        var optionCheck = bongTaiPorata2.Options.FirstOrDefault(option => option.Id != 72);
                        if (optionCheck != null && optionCheck.Id == 50)
                        {
                            damage += damage * optionCheck.Param / 100;
                        }
                    }

                    damage += damage * 10 / 100;
                }
            }
            if (Character.InfoBuff.BanhChung)
            {
                damage += damage * 25 / 100;


            }
            if (Character.InfoBuff.BanhTet)
            {
                damage += damage * 15 / 100;


            }
            if (Character.InfoSkill.Monkey.MonkeyId != 0) damage += damage * (Character.InfoSkill.Monkey.Damage-100) / 100;
            // Cuồng nộ
            if (Character.InfoBuff.CuongNo)
            {
                damage += damage;
            }
            if (Character.InfoBuff.XiMuoiHoaDao)
            {
                damage += damage * 20 / 100;
            }
            if (Character.InfoBuff.CuongNo2)
            {
                damage += damage + (damage * 20 / 100);
            }
            if (Character.isColer)
            {
                damage -= damage / 2;
            }
            if (Character.isFuture)
            {
                damage -= damage * 20 / 100;
            }
            // Kiểm tra có mặc giáp luyện tập hay không

            // Thức ăn
            if (Character.InfoBuff.ThucAnId != -1)
            {
                damage += damage * 10 / 100;
            }

            if (Character.InfoBuff.BanhTrungThuId != -1)
            {
                switch (Character.InfoBuff.BanhTrungThuId)
                {
                    case 465:
                        {
                            damage += damage * 10 / 100;
                            break;
                        }
                    case 466:
                        {
                            damage += damage * 15 / 100;
                            break;
                        }
                    case 472:
                        {
                            damage += damage * 20 / 100;
                            break;
                        }
                    case 473:
                        {
                            damage += damage * 25 / 100;
                            break;
                        }
                }
            }
            var itemGiap = Character.ItemBody[6];
            if (itemGiap != null && ItemCache.ItemIsGiapLuyenTap(itemGiap.Id))
            {
                damage -= (damage * ItemCache.GetGiapLuyenTapPTSucManh(itemGiap.Id)) / 100;
            }

            // Kiểm tra xem có vừa tháo giáp tập luyện ra không
            if (Character.InfoMore.LastGiapLuyenTapItemId != 0)
            {
                var giapLuyenTap = GetItemBagById(Character.InfoMore.LastGiapLuyenTapItemId);
                if (giapLuyenTap != null && ItemCache.ItemIsGiapLuyenTap(giapLuyenTap.Id))
                {
                    var optionCheck = giapLuyenTap.Options.FirstOrDefault(option => option.Id == 9);
                    if (optionCheck.Param > 0)
                    {
                        damage += (damage * ItemCache.GetGiapLuyenTapPTSucManh(giapLuyenTap.Id)) / 100;
                    }
                }
                else
                {
                    Character.InfoMore.LastGiapLuyenTapItemId = 0;
                    Character.Delay.GiapLuyenTap = -1;
                }
            }

            Character.DamageFull = damage;
        }

        public void SetDefenceFull()
        {
            var defence = Character.InfoChar.OriginalDefence * 4;
            defence += Character.InfoOption.Defence;
            defence += defence * Character.InfoOption.PhanTramDefence / 100;
            if (Character.InfoBuff.isEnchantGiap) defence += defence * 15 / 100;
            if (Character.InfoChar.Fusion.IsFusion && Character.Disciple != null) {
                // Bông tai porata 2
                if (Character.InfoChar.Fusion.IsPorata2)
                {
                    var bongTaiPorata2 = GetItemBagById(921);
                    if (bongTaiPorata2 != null)
                    {
                        var optionCheck = bongTaiPorata2.Options.FirstOrDefault(option => option.Id != 72);
                        if (optionCheck != null && optionCheck.Id == 94)
                        {
                            defence += defence * optionCheck.Param / 100;
                        }
                    }

                    defence += defence * 10 / 100;
                }
            }
            Character.DefenceFull = Math.Abs(defence);
        }

        public void SetCritFull()
        {
            int crtCal;
            if (Character.InfoSkill.Monkey.MonkeyId != 0)
            {
                crtCal = 115;
            }
            else
            {
                crtCal = Character.InfoChar.OriginalCrit;
                crtCal += Character.InfoOption.Crit;
            }
            if (Character.InfoBuff.isEnchantCrit)
            {
                crtCal += 3;
            }
            if (Character.InfoBuff.BanhChung)
            {
                crtCal += 25;
            }
            if (Character.InfoBuff.BanhTet)
            {
                crtCal += 15;
            }
            if (Character.InfoChar.Fusion.IsFusion && Character.Disciple != null) {
                // Bông tai porata 2
                if (Character.InfoChar.Fusion.IsPorata2)
                {
                    var bongTaiPorata2 = GetItemBagById(921);
                    if (bongTaiPorata2 != null)
                    {
                        var optionCheck = bongTaiPorata2.Options.FirstOrDefault(option => option.Id != 72);
                        if (optionCheck != null && optionCheck.Id == 14)
                        {
                            crtCal += optionCheck.Param;
                        }
                    }
                }
            }
            Character.CritFull = crtCal;
        }

        public void SetHpPlusFromDamage()
        {
           //ingored
        }

        public void SetMpPlusFromDamage()
        {
            //ingored
        }

        public void SetSpeed()
        {
            var speed = 5;
            if (Character.InfoSkill.Monkey.MonkeyId != 0) speed = 8;
            if (Character.InfoChar.Fusion.IsFusion) speed = 7;
            if (Character.InfoChar.MapId == 117) speed = 2;
            var plus = speed * (Character.InfoOption.PhanTramSpeed) / 100;
            switch (plus)
            {
                case <= 1:
                    speed += 1;
                    break;
                case > 1 and <= 2:
                    speed += 2;
                    break;
                    // case > 2:
                    //     speed += plus;
                    //     break;
            }
            Character.InfoChar. Speed = (sbyte)speed;
        }

        public void SetBuffHp30s()
        {
            var hpPlus = Character.InfoOption.PlusHp30Second;
            hpPlus += hpPlus * (Character.InfoOption.PhanTramPlusHp30Second / 100);
            Character.Effect.BuffHp30S.Value = hpPlus;
            if (Character.Effect.BuffHp30S.Time == -1)
            {
                Character.Effect.BuffHp30S.Time = 30000 + ServerUtils.CurrentTimeMillis();
            }

        }
        public void SetBuffMp30s()
        {
            var mpPlus = Character.InfoOption.PlusMp30Second;
            mpPlus += mpPlus * (Character.InfoOption.PhanTramPlusMp30Second / 100);
            Character.Effect.BuffKi30S.Value = mpPlus;
            if (Character.Effect.BuffKi30S.Time == -1)
            {
                Character.Effect.BuffKi30S.Time = 30000 + ServerUtils.CurrentTimeMillis();
            }

        }

        public void SetBuffMp1s()
        {
            var mpPlus = (int)Character.MpFull * Character.InfoOption.PlusMpEverySecond / 100;
            Character.Effect.BuffKi1s.Value = mpPlus;
            if (Character.Effect.BuffKi1s.Time == -1) 
            {
                Character.Effect.BuffKi1s.Time = 1500 + ServerUtils.CurrentTimeMillis();
            }
        }

        public void SetBuffHp5s()
        {
            //TODO HANDLE PLUS HP 5s
        }

        public void SetBuffHp10s()
        {
            //TODO HANDLE PLUS HP 10s
        }

        public void Clear() => SuppressFinalize(this);


        public void BagSort()
        {
            var count = 0;
            Character.ItemBag.ForEach(item => item.IndexUI = count++);
        }

        public void Upindex(int index)
        {
            var itemBag = GetItemBagByIndex(index);
            if (itemBag == null) return;
            if (index >= Character.ItemBag.Count) return;
            var count = 0;
            Character.ItemBag.ForEach(item => item.IndexUI = count++);
        }
        public void BoxSort()
        {
            var count = 0;
            Character.ItemBox.ForEach(item => item.IndexUI = count++);
        }
        private void UpdateCharacter()
        {

            var dateNow = ServerUtils.TimeNow();
            int MinutesDifferent = (int)(dateNow - Character.LastLogin).TotalMinutes;
            if (MinutesDifferent > 30 && Character.DataPractice.isAutoTrain())
            {
                Practice_Handler.gI().Training(Character, MinutesDifferent);
            }
            int DayDifferent = (int)(dateNow - Character.LastLogin).TotalDays;
            if (DayDifferent > 0)
            {
                Character.LastLogin = ServerUtils.TimeNow();
                Character.InfoChar.IsNhanBua = true;
                Character.DataDaiHoiVoThuat23.Handler.Reset(Character, true);
                Character.DataPractice.Whis.Status = Extension.Practice.Whis.Whis_Status.LIVE;
                Character.DataSideTask.Reset();
                Character.DataSieuHang.Ticket = 3;
                Character.DataVoDaiSinhTu.Count = 0;
                //if ((dateNow - Character.DataSieuHang.DateGetRuby).TotalDays > 0)
                //{
                //    var ruby = Character.DataSieuHang.GetRuby();
                //    if (ruby <= 0) return;
                //    Character.PlusDiamondLock(ruby);
                //    Character.CharacterHandler.SendMessage(Service.BuyItem(Character));
                //    Character.CharacterHandler.SendMessage(Service.OpenUiSay(5, $"Bạn đã nhận được {ruby} hồng ngọc từ giải đấu siêu hạng của Server"));
                //}
            }
            Character.DataSieuHang.UpdateData(Character.Me, DayDifferent > 0);
            if (Character.InfoChar.IsNhanBua)
            {
                Character.CharacterHandler.SendMessage(Service.ItemTimeWithMessage("Nhận ngẫu nhiên bùa 1h mỗi ngày tại Bà Hạt Mít ở vách núi", (int)TextTime.NHAN_BUA_MIEN_PHI, 30));
            }
            Character.LastLogin = dateNow;
        }
        public void HandleJoinMap(IZone zone)
        {
            //  lock (zone.Characters)
            //    {
            if (Character.newLogin)
            {
                Character.newLogin = false;
                UpdateCharacter();
            }
                foreach (var c in zone.Characters.Values.Where(x => x.Id != Character.Id).ToList())
                {

                    //if (Character.InfoChar.Roles.Count > 1)
                    //{
                    //    c.CharacterHandler.SendMessage(Epic_Pet.Call_Role(c));
                    //}
                    SendMessage(Service.PlayerAdd(c));
                var role = c.InfoChar.Roles1.RoleUsed;
                if (role != null)
                {
                    SendMessage(Service.SendRole(role.Id, role.Second, role.Temp));
                }
                    //if (c.InfoChar.Roles.Count > 1)
                    //{

                    //    SendMessage(Epic_Pet.Call_Role(c));
                    //}
                    var infoSkill = c.InfoSkill;
                    if (infoSkill.MeTroi.IsMeTroi)
                    {
                        if (infoSkill.MeTroi.Monster != null)
                        {
                            SendMessage(Service.SkillEffectMonster(c.Id, infoSkill.MeTroi.Monster.IdMap, 1, 32));
                        }
                    }

                    if (infoSkill.PlayerTroi.IsPlayerTroi)
                    {
                        infoSkill.PlayerTroi.PlayerId.ForEach(o =>
                        {
                            SendMessage(Service.SkillEffectPlayer(o, c.Id, 1, 32));
                        });
                    }

                    if (infoSkill.ThaiDuongHanSan.IsStun)
                    {
                        SendMessage(Service.SkillEffectPlayer(c.Id, c.Id, 1, 40));
                    }

                    if (infoSkill.DichChuyen.IsStun)
                    {
                        SendMessage(Service.SkillEffectPlayer(c.Id, c.Id, 1, 40));
                    }

                    if (infoSkill.Protect.IsProtect)
                    {
                        SendMessage(Service.SkillEffectPlayer(c.Id, c.Id, 1, 33));
                    }

                    if (infoSkill.ThoiMien.IsThoiMien)
                    {
                        SendMessage(Service.SkillEffectPlayer(c.Id, c.Id, 1, 41));
                    }
                }
                // }

                // lock (zone.Disciples)
                // {
                foreach (var disciplesValue in zone.Disciples.Values)
                {
                    var text = "#";
                    if (Character.Id + disciplesValue.Id == 0) text = "$";
                    SendMessage(Service.PlayerAdd(disciplesValue, text));
                    var infoSkill = disciplesValue.InfoSkill;
                    if (infoSkill.MeTroi.IsMeTroi)
                    {
                        if (infoSkill.MeTroi.Monster != null)
                        {
                            SendMessage(Service.SkillEffectMonster(disciplesValue.Id, infoSkill.MeTroi.Monster.IdMap, 1, 32));
                        }
                    }

                    if (infoSkill.PlayerTroi.IsPlayerTroi)
                    {
                        infoSkill.PlayerTroi.PlayerId.ForEach(o =>
                        {
                            SendMessage(Service.SkillEffectPlayer(o, disciplesValue.Id, 1, 32));
                        });
                    }

                    if (infoSkill.ThaiDuongHanSan.IsStun)
                    {
                        SendMessage(Service.SkillEffectPlayer(disciplesValue.Id, disciplesValue.Id, 1, 40));
                    }

                    if (infoSkill.DichChuyen.IsStun)
                    {
                        SendMessage(Service.SkillEffectPlayer(disciplesValue.Id, disciplesValue.Id, 1, 40));
                    }

                    if (infoSkill.Protect.IsProtect)
                    {
                        SendMessage(Service.SkillEffectPlayer(disciplesValue.Id, disciplesValue.Id, 1, 33));
                    }

                    if (infoSkill.ThoiMien.IsThoiMien)
                    {
                        SendMessage(Service.SkillEffectPlayer(disciplesValue.Id, disciplesValue.Id, 1, 41));
                    }
                }
                //  }

                ////  lock (zone.Pets)
                //  {
                foreach (var petValue in zone.Pets.Values)
                {
                    var text = "#";
                    if ((Character.Id + 1000) + petValue.Id == 0) text = "$";
                    SendMessage(Service.PlayerAdd(petValue, text));
                }
            
            foreach (var petValue in zone.Pets2.Values)
            {
                SendMessage(Service.PlayerAdd(petValue, "[Event]"));
            }
            foreach (var phanthan in zone.PhanThans.Values)
            {
                var text = "#";
                if ((Character.Id + 1000) + phanthan.Id == 0) text = "$";
                SendMessage(Service.PlayerAdd(phanthan, text));
            }
            //  }
            //
            //  lock (zone.Bosses)
            //  {
            foreach (var bossesValue in zone.Bosses.Values)
                {
                    SendMessage(Service.PlayerAdd(bossesValue));
                    var infoSkill = bossesValue.InfoSkill;
                    if (infoSkill.MeTroi.IsMeTroi)
                    {
                        if (infoSkill.MeTroi.Monster != null)
                        {
                            SendMessage(Service.SkillEffectMonster(bossesValue.Id, infoSkill.MeTroi.Monster.IdMap, 1, 32));
                        }
                    }

                    if (infoSkill.PlayerTroi.IsPlayerTroi)
                    {
                        infoSkill.PlayerTroi.PlayerId.ForEach(o =>
                        {
                            SendMessage(Service.SkillEffectPlayer(o, bossesValue.Id, 1, 32));
                        });
                    }

                    if (infoSkill.ThaiDuongHanSan.IsStun)
                    {
                        SendMessage(Service.SkillEffectPlayer(bossesValue.Id, bossesValue.Id, 1, 40));
                    }

                    if (infoSkill.DichChuyen.IsStun)
                    {
                        SendMessage(Service.SkillEffectPlayer(bossesValue.Id, bossesValue.Id, 1, 40));
                    }

                    if (infoSkill.Protect.IsProtect)
                    {
                        SendMessage(Service.SkillEffectPlayer(bossesValue.Id, bossesValue.Id, 1, 33));
                    }

                    if (infoSkill.ThoiMien.IsThoiMien)
                    {
                        SendMessage(Service.SkillEffectPlayer(bossesValue.Id, bossesValue.Id, 1, 41));
                    }
                }
           // }

          //  lock (zone.MonsterPets)
           // {
               foreach(var m in zone.MonsterPets.Values.Where(m => m is { IsDie: false } && m.IdMap != Character.Id).ToList())
                {
                    SendMessage(Service.UpdateMonsterMe0(m));
                }
          //  }

           foreach(var m in  zone.MonsterMaps.Where(m => !m.IsDie).ToList())
            {
                var infoSkill = m.InfoSkill;
                if (infoSkill.ThaiDuongHanSan.IsStun)
                {
                    SendMessage(Service.SkillEffectMonster(-1, m.IdMap, 1, 40));
                }

                if (infoSkill.DichChuyen.IsStun)
                {
                    SendMessage(Service.SkillEffectMonster(-1, m.IdMap, 1, 40));
                }

                if (infoSkill.ThoiMien.IsThoiMien)
                {
                    SendMessage(Service.SkillEffectMonster(-1, m.IdMap, 1, 41));
                }
            }
            switch (Character.InfoChar.MapId)
            {
                case 21:
                case 22:
                case 23:
                    if (Character.InfoChar.ThoiGianTrungMaBu > 0)
                    {
                        SendMessage(Service.TrungMaBu(Character));
                    }
                    if (Character.InfoChar.ThoiGianDuaHau > 0)
                    {
                        SendMessage(Service.DuaHau(Character));
                    }
                    break;
                default:
                    if (Character.DataNgocRongNamek.AlreadyPick(Character))
                    {
                        Character.DataNgocRongNamek.DelayAction = 60000 + ServerUtils.CurrentTimeMillis();
                    }
                    break;
            }
            if (TaskHandler.CheckTask(Character, 3, 1) && Character.InfoChar.MapId == 42 + Character.InfoChar.Gender)
            {
                var embe = ItemCache.GetItemDefault(78);
                var itemMap = new ItemMap(Character.Id, embe);
                switch (Character.InfoChar.MapId)
                {
                    case 42:
                        itemMap.X = 86;
                        itemMap.Y = 288;
                        break;
                    case 43:
                        itemMap.X = 129;
                        itemMap.Y = 264;
                        break;
                    case 44:
                        itemMap.X = 151;
                        itemMap.Y = 288;
                        break;
                }
                Character.Zone.ZoneHandler.LeaveItemMap(itemMap);
            }
        }

        public void AddItemToBody(Model.Item.Item item, int index)
        {
            if (item == null) return;
            item.IndexUI = index;
            Character.ItemBody[index] = item;

          //  UpdateAntiChangeServerTime();
         //   Character.Delay.NeedToSaveBody = true;

        }

        #region ItemBag
        public Model.Item.Item GetItemBagByIndex(int index)
        {
            return Character.ItemBag.FirstOrDefault(item => item.IndexUI == index);
        }
        public Model.Item.Item GetItemBodyByIndex(int index)
        {
            return Character.ItemBody.FirstOrDefault(item=>item.IndexUI==index);
        }
        public Model.Item.Item GetItemBagById(int id)
        {
            return Character.ItemBag.FirstOrDefault(item => item.Id == id);
        }
        public int GetAllQuantityItemBagById(int id)    
        {
            var matchingItems = Character.ItemBag.Where(itemBag => itemBag.Id == id);
            if (matchingItems.Any())
            {
                return matchingItems.Sum(itemBag => itemBag.Quantity);
            }
            else
            {
                return 0;
            }
        }
        private int IndexItemBagNotMaxQuantity(short id)
        {
            var item = Character.ItemBag.FirstOrDefault(item => (item.Quantity < 99 || ItemCache.IsUnlimitItem(id)) && item.Id == id);
            return item?.IndexUI ?? -1;
        }

        public Model.Item.Item ItemBagNotMaxQuantity(short id)
        {
            return Character.ItemBag.FirstOrDefault(item => (item.Quantity < 99 || ItemCache.IsUnlimitItem(id)) && item.Id == id);
        }

        public Model.Item.Item ItemBagNotMaxQuantity(short id, int indexUi)
        {
            return Character.ItemBag.FirstOrDefault(item => item.IndexUI != indexUi && (item.Quantity < 99 || ItemCache.IsUnlimitItem(id)) && item.Id == id);
        }

        public bool AddItemToBag(bool isUpToUp, Model.Item.Item item, string reason = "")
        {
            try
            {
                if (item == null) return false;
                //item.LogOption();
                var index = IndexItemBagNotMaxQuantity(item.Id);
                var itemTemplate = ItemCache.ItemTemplate(item.Id);
                if (isUpToUp && itemTemplate.IsUpToUp && index != -1)
                {
                    Server.Gi().Logger.Debug("isUptoItem");
                    var itemBag = GetItemBagByIndex(index);
                    var quantity = itemBag.Quantity + item.Quantity;
                    if (quantity > 99 && !ItemCache.IsUnlimitItem(item.Id) && !ItemCache.IsSpecialAmountItem(item.Id))
                    {
                        Server.Gi().Logger.Debug("isNormalItem");
                        var itemClone = ItemHandler.Clone(itemBag);
                        itemClone.Quantity = quantity - 99;
                        if (!AddItemToBag(itemClone, reason)) return false;
                        quantity = 99;
                    }
                    else if (ItemCache.IsSpecialAmountItem(item.Id))
                    {
                        Server.Gi().Logger.Debug("isSpecialAmountItem");
                        var opt = item.Options.FirstOrDefault(i => i.Id == 31);

                        if (opt != null)
                        {
                            Server.Gi().Logger.Debug("opt plus: "+ opt.Param + " + " + item.Quantity + " = " + (opt.Param + item.Quantity));
                            opt.Param += item.Quantity;
                        }
                        else
                        {
                            item.Options.Add(new OptionItem()
                            {
                                Id = 31,
                                Param = item.Quantity
                            });
                            Server.Gi().Logger.Debug("opt new: " + item.Quantity);
                        }
                    }
                    ServerUtils.WriteLog("additem/" + Character.Id, $"BAG:{Character.Name} add {quantity}x{itemTemplate.Name} (old: {itemBag.Quantity}) lydo: " + reason);
                    if (!ItemCache.IsSpecialAmountItem(item.Id)) itemBag.Quantity = quantity;
                    else itemBag.Quantity = 1;

                 //   UpdateAntiChangeServerTime(reason);
                 //   Character.Delay.NeedToSaveBag = true;

                    return true;
                }
                else
                {
                    Server.Gi().Logger.Debug("is not UptoItem");
                    return AddItemToBag(item, reason);
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error AddItemToBag in Service.cs: {e.Message} \n {e.StackTrace}", e);
                return false;
            }
        }

        private bool AddItemToBag(Model.Item.Item item, string reason)
        {
            if (item != null)
            {
                if (Character.LengthBagNull() > 0)
                {
                    var itemTemplate = ItemCache.ItemTemplate(item.Id);
                    lock (Character.ItemBag)
                    {
                        var index = Character.ItemBag.Count;
                        item.IndexUI = index;
                        if (ItemCache.IsSpecialAmountItem(item.Id))
                        {
                            Server.Gi().Logger.Debug("isSpecialAmountItem2");
                            var opt = item.Options.FirstOrDefault(i => i.Id == 31);
                            if (opt != null)
                            {
                                Server.Gi().Logger.Debug("opt plus");
                                opt.Param += item.Quantity;
                            }
                            else
                            {
                                Server.Gi().Logger.Debug("opt new");
                                item.Options.Add(new OptionItem()
                                {
                                    Id = 31,
                                    Param = item.Quantity
                                });
                            }
                        }
                        Character.ItemBag.Add(item);

                        //item.LogOption();
                    }
                    ServerUtils.WriteLog("additem/" + Character.Id, $"BAG:{Character.Name} add {item.Quantity}x{itemTemplate.Name} lydo: " + reason);
                    //UpdateAntiChangeServerTime(reason);
                    //Character.Delay.NeedToSaveBag = true;

                    
                    return true;
                }
                else
                {
                    SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_BAG));
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void RemoveItemBagById(short id, int quantity, string reason = "")
        {
            var num = 0;
            var lengthOld = Character.ItemBag.Count;
            Character.ItemBag.ToList().ForEach(itemBag =>
            {
                if (itemBag == null || itemBag.Id != id) return;
                if (num + itemBag.Quantity >= quantity)
                {
                    RemoveItemBagByIndex(itemBag.IndexUI, quantity - num, false, reason);
                    id = -1;
                    return;
                }
                num += itemBag.Quantity;
                RemoveItemBagByIndex(itemBag.IndexUI, itemBag.Quantity, false, reason);
                var itemTemplate = ItemCache.ItemTemplate(itemBag.Id);
                
            });
            if (lengthOld == Character.ItemBag.Count) return;
            num = 0;
            Character.ItemBag.ForEach(item => item.IndexUI = num++);
        }

        public void RemoveItemBagByIndex(int index, int quantity, bool reset = true, string reason = "")
        {
            lock (Character.ItemBag)
            {
                var itemBag = GetItemBagByIndex(index);
                if (itemBag == null) return;
                itemBag.Quantity -= quantity;

                if (itemBag.Quantity <= 0) Character.ItemBag.RemoveAll(item => item.IndexUI == index);

              //  UpdateAntiChangeServerTime(reason);

             //   Character.Delay.NeedToSaveBag = true;

                var itemTemplate = ItemCache.ItemTemplate(itemBag.Id);
                ServerUtils.WriteLog("removeitem/" + Character.Id, $"BAG:{Character.Name} remove {quantity}x{itemTemplate.Name} lydo: " + reason);
               
                if (!reset || index >= Character.ItemBag.Count) return;
                {
                    var count = 0;
                    Character.ItemBag.ForEach(item => item.IndexUI = count++);
                }
              //  if (itemTemplate.Type is 23 or 24) UpdateMountId();
            }
        }
        public void RemoveItemBodyByIndex(int index, int quantity, bool reset = true, string reason = "")
        {
            lock (Character.ItemBag)
            {
                var itemBag = GetItemBagByIndex(index);
                if (itemBag == null) return;
                itemBag.Quantity -= quantity;

                if (itemBag.Quantity <= 0) Character.ItemBag.RemoveAll(item => item.IndexUI == index);

              //  UpdateAntiChangeServerTime(reason);

              //  Character.Delay.NeedToSaveBag = true;

                var itemTemplate = ItemCache.ItemTemplate(itemBag.Id);
                ServerUtils.WriteLog("removeitem/" + Character.Id, $"BAG:{Character.Name} remove {quantity}x{itemTemplate.Name} lydo: " + reason);

                if (!reset || index >= Character.ItemBag.Count) return;
                {
                    var count = 0;
                    Character.ItemBag.ForEach(item => item.IndexUI = count++);
                }
               // if (itemTemplate.Type is 23 or 24) UpdateMountId();
            }
        }
        public Model.Item.Item RemoveItemBag(int index, bool isReset = true, string reason = "")
        {
            var itemBag = GetItemBagByIndex(index);
            lock (Character.ItemBag)
            {
                if (itemBag == null) return null;
                Character.ItemBag.RemoveAll(item => item.IndexUI == index);
                if (isReset && index < Character.ItemBag.Count)
                {
                    var count = 0;
                    Character.ItemBag.ForEach(item => item.IndexUI = count++);
                }
                SendMessage(Service.SendBag(Character));
                var itemTemplate = ItemCache.ItemTemplate(itemBag.Id);
                ServerUtils.WriteLog("removeitem/" + Character.Id, $"BAG:{Character.Name} remove {itemTemplate.Name} lydo: " + reason);
                
            //    UpdateAntiChangeServerTime(reason);
           //     Character.Delay.NeedToSaveBag = true;

            }
            return itemBag;
        }
        #endregion

        #region Item Box
        public bool AddItemToBox(bool isUpToUp, Model.Item.Item item)
        {
            try
            {
                if (item == null) return false;
                var index = IndexItemBoxNotMaxQuantity(item.Id);
                var itemTemplate = ItemCache.ItemTemplate(item.Id);
                if (isUpToUp && itemTemplate.IsUpToUp && index != -1)
                {
                    var itemBox = GetItemBoxByIndex(index);
                    var quantity = itemBox.Quantity + item.Quantity;
                    if (quantity > 99)
                    {
                        var itemClone = ItemHandler.Clone(itemBox);
                        itemClone.Quantity = quantity - 99;
                        if (!AddItemToBox(itemClone)) return false;
                        quantity = 99;
                    }

                    itemBox.Quantity = quantity;
                  //  UpdateAntiChangeServerTime();
                 //   Character.Delay.NeedToSaveBox = true;

                    return true;
                }
                else
                {
                    return AddItemToBox(item);
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Error($"Error AddItemToBox in Service.cs: {e.Message} \n {e.StackTrace}", e);
                return false;
            }
        }

        private bool AddItemToBox(Model.Item.Item item)
        {
            if (item != null)
            {
                if (Character.LengthBoxNull() > 0)
                {
                    lock (Character.ItemBox)
                    {
                        var index = Character.ItemBox.Count;
                        item.IndexUI = index;
                        Character.ItemBox.Add(item);
                    }
               //    UpdateAntiChangeServerTime();
               //     Character.Delay.NeedToSaveBox = true;

                    return true;
                }
                else
                {
                    SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_BOX));
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public Model.Item.Item GetItemBoxByIndex(int index)
        {
            return Character.ItemBox.FirstOrDefault(item => item.IndexUI == index);
        }
        public Model.Item.Item GetItemLuckyBoxByIndex(int index)
        {
            return Character.LuckyBox.FirstOrDefault(item => item.IndexUI == index);
        }
        public Model.Item.Item GetItemClanBoxByIndex(int index)
        {
            return ClanManager.Get(Character.ClanId).ClanBox.FirstOrDefault(item => item.IndexUI == index);
        }
        public Model.Item.Item GetItemBoxById(int id)
        {
            return Character.ItemBox.FirstOrDefault(item => item.Id == id);
        }

        public void RemoveMonsterMe()
        {
            var skillEgg = Character.InfoSkill.Egg;
            if (skillEgg.Monster is { IsDie: true })
            {
                SendZoneMessage(Service.UpdateMonsterMe7(skillEgg.Monster.Id));
                Character.Zone.ZoneHandler.RemoveMonsterMe(skillEgg.Monster.Id);
                SkillHandler.RemoveMonsterPet(Character);
            }
        }

        public void PlusTiemNang(IMonster monster, int damage)
        {
            if (monster.Character != null) return;
            if (damage <= 0) return;
            var timeServer = ServerUtils.CurrentTimeMillis();
            long fixDmg = (long)((damage) + (monster.OriginalHp * 0.00125));
            long damagePlusPoint = fixDmg;
            //if (Character.InfoChar.Task.Id == 5 && Character.InfoChar.Task.Index == 0 && Character.InfoChar.Điểm_thành_tích >= 200000 * 100)
            //{
            //    Character.InfoChar.Task.Index++;
            //    Character.CharacterHandler.SendMessage(Service.SendTask(Character));
            //}
            //if (Character.InfoChar.Task.Id == 6 && Character.InfoChar.Task.Index == 0 && Character.InfoChar.Điểm_thành_tích >= 400000 * 100)
            //{
            //    Character.InfoChar.Task.Index++;
            //    Character.CharacterHandler.SendMessage(Service.SendTask(Character));
            //}
            if (Character.InfoBuff.KhauTrang) damagePlusPoint += damagePlusPoint * 5 / 100;
            if (Character.InfoChar.Power > DatabaseManager.ConfigManager.gI().LimitPowerExpUp)
            {
                damagePlusPoint /= 2;
            }
          //  var CacheLitmit = Cache.Gi().LIMIT_POWERS.FirstOrDefault(i => i.Key == Character.InfoChar.LitmitPower).Value;
            if(Character.InfoChar.LitmitPower >= 4)
            {
                damagePlusPoint /= (Character.InfoChar.LitmitPower);

            }
            //if (Character.InfoChar.LitmitPower >= 6 && Character.InfoChar.Power >= CacheLitmit.Power)
            //{
            //    damagePlusPoint /= 2;
            //}
            //if (Character.InfoChar.LitmitPower >= 8 && Character.InfoChar.Power >= CacheLitmit.Power)
            //{
            //    damagePlusPoint /= 2;
            //}
            //if (Character.InfoChar.LitmitPower >= 10 && Character.InfoChar.Power >= CacheLitmit.Power)
            //{
            //    damagePlusPoint /= 2;
            //}
            //if (Character.InfoChar.LitmitPower >= 13 && Character.InfoChar.Power >= CacheLitmit.Power)
            //{
            //    damagePlusPoint /= 2;
            //}
            //if (Character.InfoChar.LitmitPower >= 16 && Character.InfoChar.Power >= CacheLitmit.Power)
            //{
            //    damagePlusPoint /= 2;
            //}
            //if (Character.InfoChar.LitmitPower >= 18 && Character.InfoChar.Power >= CacheLitmit.Power)
            //{
            //    damagePlusPoint /= 2;
            //}
            //if (Character.InfoChar.LitmitPower >= 20 && Character.InfoChar.Power >= CacheLitmit.Power)
            //{
            //    damagePlusPoint /= 2;
            //}
            //if (Character.InfoChar.LitmitPower >= 21 && Character.InfoChar.Power >= CacheLitmit.Power)
            //{
            //    damagePlusPoint /= 2;
            //}

            if (damagePlusPoint <= 0)
            {
                damagePlusPoint = 1;
            }

            if (monster.Id != 0)
            {
                //var levelChar = Character.InfoChar.Level;
                //var levelMonster = monster.Level;
                //var checkLevel = Math.Abs(levelChar - levelMonster);
                //if ((checkLevel > 5 && levelChar > levelMonster) || (levelMonster > levelChar && levelMonster - levelChar > 5))
                //{
                //    damagePlusPoint = 1;
                //}
                if (monster.LvBoss > 1)
                {
                    damagePlusPoint = damage;
                }
            }
            else
            {
                damagePlusPoint = 1;//ServerUtils.RandomNumber(100) < 3 ? 1 : 0;
            }

            switch (Character.Flag)
            {
                case 0: break;
                case 8:
                    damagePlusPoint += damagePlusPoint * 10 / 100;
                    break;
                default:
                    damagePlusPoint += damagePlusPoint * 5 / 100;
                    break;
            }

            if (DatabaseManager.ConfigManager.gI().ExpUp > 1)
            {
                if (Character.InfoChar.Power < DatabaseManager.ConfigManager.gI().LimitPowerExpUp)
                {
                    damagePlusPoint *= DatabaseManager.ConfigManager.gI().ExpUp;
                }
                else
                {
                    damagePlusPoint *= (DatabaseManager.ConfigManager.gI().ExpUp / 2);
                }
            }

            // Nội tại tăng tiềm năng sức mạnh đánh quái
            var specialId = Character.SpecialSkill.Id;
            if (specialId != -1 && (specialId == 8 || specialId == 19 || specialId == 29))
            {
                damagePlusPoint += damagePlusPoint * Character.SpecialSkill.Value / 100;
            }
            // Option Sao pha lê
            var OptionPhanTramTNSM = Character.InfoOption.PhanTramTNSM;
            if (OptionPhanTramTNSM > 0)
            {
                damagePlusPoint += damagePlusPoint * OptionPhanTramTNSM / 100;
            }

            // // Bùa Trí Tuệ x4
            if (Character.InfoMore.BuaTriTueX4)
            {
                if (Character.InfoMore.BuaTriTueX4Time > timeServer)
                {
                    damagePlusPoint *= 4;
                }
                else
                {
                    Character.InfoMore.BuaTriTueX4 = false;
                }
            }
            else if (Character.InfoMore.BuaTriTueX3) // Bùa Trí Tuệ x3
            {
                if (Character.InfoMore.BuaTriTueX3Time > timeServer)
                {
                    damagePlusPoint *= 3;
                }
                else
                {
                    Character.InfoMore.BuaTriTueX3 = false;
                }
            }
            // Bùa Trí Tuệ 213
            else if (Character.InfoMore.BuaTriTue)
            {
                if (Character.InfoMore.BuaTriTueTime > timeServer)
                {
                    damagePlusPoint *= 2;
                }
                else
                {
                    Character.InfoMore.BuaTriTue = false;
                }
            }
            // Vệ tinh Trí tuệ
            if (Character.InfoMore.IsNearAuraTriTueItem)
            {
                if (Character.InfoMore.AuraTriTueTime > timeServer)
                {
                    damagePlusPoint += damagePlusPoint * 20 / 100;
                }
                else
                {
                    Character.InfoMore.IsNearAuraTriTueItem = false;
                }
            }
            if (DataCache.IdMapNguHanhSon.Contains(Character.InfoChar.MapId))
            {
                damagePlusPoint *= 2;
            }
            if (Character.InfoBuff.KichDucX2)
            {
                damagePlusPoint *= 2;
            }
            if (Character.InfoBuff.KichDucX5)
            {
                damagePlusPoint *= 5;
            }
            if (Character.InfoBuff.KichDucX7)
            {
                damagePlusPoint *= 7;
            }
            //if (Character.Blackball.CurrentListBuff.Contains(6)) damagePlusPoint += damagePlusPoint * 35 / 100;
            //if (Character.Blackball.CurrentListBuff.Contains(7)) damagePlusPoint += damagePlusPoint * 35 / 100;

            if (Character.InfoOption.X2TiemNang) damagePlusPoint *= 2;
            //if (damagePlusPoint >= 6000000)
            //{
            //    damagePlusPoint = 6000000;
            //}
            if (DataCache.IdMapBDKB.Contains(Character.InfoChar.MapId)) damagePlusPoint *= 3;
            if (Character.InfoChar.IsPower)
            {
                PlusTiemNang(damagePlusPoint, damagePlusPoint, true);
                TaskHandler.gI().CheckTaskDonePower(Character);
              //  TaskHandler.CheckPowerToAction(this.Character);
            }
            else
            {
                PlusTiemNang(0, damagePlusPoint, false);
                TaskHandler.gI().CheckTaskDonePower(Character);
                //  TaskHandler.CheckPowerToAction(this.Character);
            }

            foreach (var clanChar in Character.Zone.Characters.Values.ToList().Where(c => c.ClanId != -1 && c.ClanId == Character.ClanId && c.Id != Character.Id))
            {
                clanChar.CharacterHandler.PlusTiemNang(0, damagePlusPoint / 2, false);
            }
        }

        public void PlusTiemNang(long power, long potential, bool isAll)
        {
            // if (!Character.InfoChar.IsPremium && Character.InfoChar.Điểm_thành_tích >= DataCache.PREMIUM_LIMIT_UP_POWER)
            // {
            //     SendMessage(Service.ServerMessage(TextServer.gI().NOT_PREMIUM_LIMIT_POWER));
            //     return;
            // }

            if (isAll && power > 0 && potential > 0)
            {
                PlusPower(power);
                PlusPotential(potential);
                SendMessage(Service.UpdateExp(2, power));
            }
            else
            {
                if (power > 0)
                {
                    PlusPower(power);
                    SendMessage(Service.UpdateExp(0, power));
                }

                if (potential > 0)
                {
                    PlusPotential(potential);
                    SendMessage(Service.UpdateExp(1, potential));
                }
            }
        }

        public void LeaveFromDead(bool isHeal = false)
        {
            lock (Character)
            {
                if (!isHeal)
                {
                    Character.MineDiamond(1);
                }
                SendMessage(Service.MeLoadInfo(Character));
                Character.InfoChar.IsDie = false;
                Character.InfoChar.Hp = Character.HpFull;
                Character.InfoChar.Mp = Character.MpFull;
                SendMessage(Service.MeLive());
                SendZoneMessage(Service.ReturnPointMap(Character));
                SendZoneMessage(Service.PlayerLoadLive(Character));
                BoMongQuest_Handler.gI().PlusSubTask(Character, BoMongQuest_Template.THANH_HOI_SINH);
            }
        }

        public void BackHome()
        {
            lock (Character)
            {
                SendZoneMessage(Service.SendTeleport(Character.Id, Character.InfoChar.Teleport));
                Character.Zone.Map.OutZone(Character);
                Character.InfoChar.IsDie = false;
                Character.InfoChar.Hp = 1;
                SendMessage(Service.MeLive());
                SendMessage(Service.PlayerLevel(Character));
                SendMessage(Service.MeLoadInfo(Character));
                MapManager.GetMapOffline(21 + Character.InfoChar.Gender).JoinZone(Character, Character.Id, true, true, Character.InfoChar.Teleport);

            }
        }

        private int IndexItemBoxNotMaxQuantity(short id)
        {
            var item = Character.ItemBox.FirstOrDefault(item => item.Quantity < 99 && item.Id == id);
            return item?.IndexUI ?? -1;
        }

        public void RemoveItemBoxByIndex(int index, int quantity, bool reset = true)
        {
            lock (Character.ItemBox)
            {
                var itemBox = GetItemBoxByIndex(index);
                if (itemBox == null) return;
                itemBox.Quantity -= quantity;
                if (itemBox.Quantity <= 0) Character.ItemBox.RemoveAll(item => item.IndexUI == index);
            //    UpdateAntiChangeServerTime();
           //     Character.Delay.NeedToSaveBox = true;

                if (!reset || index >= Character.ItemBox.Count) return;
                {
                    var count = 0;
                    Character.ItemBox.ForEach(item => item.IndexUI = count++);
                }
            }
        }

        public Model.Item.Item RemoveItemBox(int index, bool isReset = true)
        {
            lock (Character.ItemBox)
            {
                var itemBox = Character.ItemBox.FirstOrDefault(item => item.IndexUI == index);
                if (itemBox == null) return null;
                Character.ItemBox.RemoveAll(item => item.IndexUI == index);
                if (isReset && index < Character.ItemBox.Count)
                {
                    var count = 0;
                    Character.ItemBox.ForEach(item => item.IndexUI = count++);
                }
                SendMessage(Service.SendBox(Character));
        //        UpdateAntiChangeServerTime();
             //   Character.Delay.NeedToSaveBox = true;

                return itemBox;
            }

        }
        public Model.Item.Item RemoveItemLuckyBox(int index, bool isReset = true)
        {
            lock (Character.LuckyBox)
            {
                var itemBox = Character.LuckyBox.FirstOrDefault(item => item.IndexUI == index);
                if (itemBox == null) return null;
                Character.LuckyBox.RemoveAll(item => item.IndexUI == index);
                if (isReset && index < Character.LuckyBox.Count)
                {
                    var count = 0;
                    Character.LuckyBox.ForEach(item => item.IndexUI = count++);
                }
             //   UpdateAntiChangeServerTime();
             //   Character.Delay.NeedToSaveLucky = true;

                return itemBox;
            }

        }
        public Model.Item.Item RemoveItemGiftBox(int index, bool isReset = true)
        {
            lock (Character.ItemGift)
            {
                var itemBox = Character.ItemGift.FirstOrDefault(item => item.IndexUI == index);
                if (itemBox == null) return null;
                Character.ItemGift.RemoveAll(item => item.IndexUI == index);
                if (isReset && index < Character.LuckyBox.Count)
                {
                    var count = 0;
                    Character.LuckyBox.ForEach(item => item.IndexUI = count++);
                }
                //   UpdateAntiChangeServerTime();
                //   Character.Delay.NeedToSaveLucky = true;

                return itemBox;
            }

        }
        public Model.Item.Item RemoveItemClanBox(int index, bool isReset = true)
        {
            lock (ClanManager.Get(Character.ClanId).ClanBox)
            {
                var itemBox = ClanManager.Get(Character.ClanId).ClanBox.FirstOrDefault(item => item.IndexUI == index);
                if (itemBox == null) return null;
                ClanManager.Get(Character.ClanId).ClanBox.RemoveAll(item => item.IndexUI == index);
                if (isReset && index < ClanManager.Get(Character.ClanId).ClanBox.Count)
                {
                    var count = 0;
                    ClanManager.Get(Character.ClanId).ClanBox.ForEach(item => item.IndexUI = count++);
                }
             //   UpdateAntiChangeServerTime();
             //   Character.Delay.NeedToSaveLucky = true;

                return itemBox;
            }

        }
        public void OpenUiSay(string say)
        {
            SendMessage(Service.OpenUiSay(5, say));
        }
        public void SendServerMessage(string say)
        {
            SendMessage(Service.ServerMessage(say));
        }
        public void MoveMap(short toX, short toY, int type = 0)
        {
            Character.InfoChar.X = toX;
            Character.InfoChar.Y = toY;
            if (type == 1)
            {
                var mpMine = (int)Character.InfoChar.OriginalMp / 100 *
                             (Character.InfoSkill.Monkey.MonkeyId > 0 ? 2 : 1);
                if (Character.InfoChar.Mp > mpMine)
                {
                    if (Character.InfoChar.MountId == -1)
                    {
                        MineMp(mpMine);

                    }
                    BoMongQuest_Handler.gI().PlusSubTask(Character, BoMongQuest_Template.KHINH_CONG_THANH_THAO);
                }
            }
            
            SendZoneMessage(Service.PlayerMove(Character.Id, Character.InfoChar.X, Character.InfoChar.Y));
            if (Character.InfoSkill.MeTroi.IsMeTroi)
            {
                
                SkillHandler.RemoveTroi(Character);
            }

            var disciple = Character.Disciple;
            if (disciple != null && Character.InfoChar.IsHavePet && !Character.InfoChar.Fusion.IsFusion)
            {
                if (disciple.Status == 0 || disciple.Status == 1 && Math.Abs(Character.InfoChar.X - disciple.InfoChar.X) > 60 || disciple.Status == 2 && Math.Abs(Character.InfoChar.X - disciple.InfoChar.X) > 300 || disciple.Status == 3 && Math.Abs(Character.InfoChar.X - disciple.InfoChar.X) > 600)
                {
                    Character.Disciple.CharacterHandler.MoveMap(Character.InfoChar.X, Character.InfoChar.Y);
                }
            }

            var pet = Character.Pet;
            if (pet != null)
            {
                if (Math.Abs(Character.InfoChar.X - pet.InfoChar.X) > 60)
                {
                    Character.Pet.CharacterHandler.MoveMap(Character.InfoChar.X, Character.InfoChar.Y);
                }
            }
        }

        #endregion

        public void PlusHp(int hp)
        {
            lock (Character.InfoChar)
            {
                if (Character.InfoChar.IsDie) return;
                Character.InfoChar.Hp += hp;
                if (Character.InfoChar.Hp >= Character.HpFull) Character.InfoChar.Hp = Character.HpFull;
            }
        }

        public void MineHp(long hp)
        {
            lock (Character.InfoChar)
            {
                if (Character.InfoChar.IsDie || hp <= 0) return;

                if (hp > Character.InfoChar.Hp)
                {
                    Character.InfoChar.Hp = 0;
                }
                else
                {
                    Character.InfoChar.Hp -= hp;
                }

                if (Character.InfoChar.Hp <= 0)
                {
                    Character.InfoChar.IsDie = true;
                    Character.InfoChar.Hp = 0;
                }
            }
        }

        public void PlusMp(int mp)
        {
            lock (Character.InfoChar)
            {
                if (Character.InfoChar.IsDie) return;
                Character.InfoChar.Mp += mp;
                if (Character.InfoChar.Mp >= Character.MpFull) Character.InfoChar.Mp = Character.MpFull;
            }
        }

        public void MineMp(int mp)
        {
            lock (Character.InfoChar)
            {
                if (Character.InfoChar.IsDie || mp < 0) return;
                Character.InfoChar.Mp -= mp;
                if (Character.InfoChar.Mp <= 0) Character.InfoChar.Mp = 0;
            }
        }

        public void PlusStamina(int stamina)
        {
            lock (Character.InfoChar)
            {
                Character.InfoChar.Stamina += (short)stamina;
                if (Character.InfoChar.Stamina > 10000) Character.InfoChar.Stamina = 10000;
            }
        }

        public void MineStamina(int stamina)
        {
            // Bùa Dẻo Dai 218
            if (Character.InfoMore.BuaDeoDai)
            {
                if (Character.InfoMore.BuaDeoDaiTime > ServerUtils.CurrentTimeMillis())
                {
                    return;
                }
                else
                {
                    Character.InfoMore.BuaDeoDai = false;
                }
            }
            // 
            lock (Character.InfoChar)
            {
                if (stamina < 0) return;
                Character.InfoChar.Stamina -= (short)stamina;
                if (Character.InfoChar.Stamina <= 0) Character.InfoChar.Stamina = 0;
            }
        }

        public void PlusPower(long power)
        {
            lock (Character.InfoChar)
            {
                Character.InfoChar.Power += power;
                Character.InfoChar.Level = (sbyte)(Cache.Gi().EXPS.Count(exp => exp < Character.InfoChar.Power) - 1);
                if (Cache.Gi().LIMIT_POWERS[Character.InfoChar.LitmitPower].Power > Character.InfoChar.Power)
                {
                    Character.InfoChar.IsPower = true;
                }
                else
                {
                    Character.InfoChar.IsPower = false;
                }
            }
        }

        public void PlusPotential(long potential)
        {
            lock (Character.InfoChar)
            {
                Character.InfoChar.Potential += potential;
            }
        }

        public Model.Item.Item RemoveItemBody(int index)
        {
            Model.Item.Item item;
            lock (Character.ItemBody)
            {
                item = Character.ItemBody[index];
                if (item == null) return null;
                Character.ItemBody[index] = null;
                UpdateInfo(true);
             //   UpdateAntiChangeServerTime();
             //   Character.Delay.NeedToSaveBody = true;
                SendMessage(Service.SendBody(Character));
               
            }
            return item;
        }

        public void DropItemBody(int index)
        {
            var item = RemoveItemBody(index);
            var zone = MapManager.Get(Character.InfoChar.MapId)?.GetZoneById(Character.InfoChar.ZoneId);
            if (item == null || zone == null) return;
            zone.ZoneHandler.LeaveItemMap(new ItemMap()
            {
                PlayerId = Character.Id,
                X = Character.InfoChar.X,
                Y = Character.InfoChar.Y,
                Item = item,
            });

        }

        public void DropItemBag(int index)
        {
            var item = RemoveItemBag(index, reason: "Vứt vật phẩm");
            var zone = MapManager.Get(Character.InfoChar.MapId)?.GetZoneById(Character.InfoChar.ZoneId);
            if (item == null || zone == null) return;
            zone.ZoneHandler.LeaveItemMap(new ItemMap()
            {
                PlayerId = Character.Id,
                X = Character.InfoChar.X,
                Y = Character.InfoChar.Y,
                Item = item,
            });

        }
        public void CreatePetNormal()
        {
            var detu = new Disciple();
            detu.CreateNewDisciple(Character, ServerUtils.RandomNumber(0, 2));
            detu.Player = Character.Player;
            detu.CharacterHandler.SetUpInfo();

            Character.Disciple = detu;
            Character.InfoChar.IsHavePet = true;
            Character.CharacterHandler.SendMessage(Service.Disciple(1, null));
            Character.Zone.ZoneHandler.AddDisciple(detu);
            DiscipleDB.Create(detu);
        }
        public void PickItemMap(short id)
        {
            // try
            // {
            var zone = MapManager.Get(Character.InfoChar.MapId)?.GetZoneById(Character.InfoChar.ZoneId);
            if (DataCache.IdMapSpecial.Contains(Character.InfoChar.MapId) || DataCache.IdMapKarin.Contains(Character.InfoChar.MapId))
            {
                zone = Character.Zone;
            }
            var itemMap = zone.ItemMaps.Values.FirstOrDefault(item => item.Id == id);

            if (itemMap == null) return;
            if (itemMap.PlayerId == -2) return;
            var canPick = true;
            if (itemMap.PlayerId != -1 && ((itemMap.PlayerId != Character.Id) || (Character.InfoSkill.Egg.Monster != null && Character.InfoSkill.Egg.Monster.Character.Id != itemMap.PlayerId)))
            {
                canPick = false;
            }
            
            if (!canPick)
            {
                SendMessage(Service.ServerMessage(TextServer.gI().ITEM_OF_ORTHER));
                return;
            }

            if (Math.Abs(itemMap.X - Character.InfoChar.X) >= 70 && !Character.InfoMore.BuaThuHut)
            {
                SendMessage(Service.ServerMessage(TextServer.gI().SO_FAR));
                return;
            }

            lock (zone.ItemMaps)
            {
                var itemNew = itemMap.Item;
                var itemTemplate = ItemCache.ItemTemplate(itemNew.Id);
                if (itemNew == null) return;

                switch (itemNew.Id)
                {
                    case 380:
                        if (TaskHandler.CheckTask(Character, 30, 1))
                        {
                            TaskHandler.gI().PlusSubTask(Character, 1);

                        }
                        if (AddItemToBag(true, itemNew, "nhiệm vụ"))
                        {
                            zone.ZoneHandler.SendMessage(Service.ItemMapMePick(itemMap.Id, itemNew.Quantity, "a"));
                            SendMessage(Service.ServerMessage("Bạn nhận được " + itemTemplate.Name));
                        }
                        break;
                    case 85:
                        if (TaskHandler.CheckTask(Character, 15, 1))
                        {
                            TaskHandler.gI().PlusSubTask(Character, 1);
                            if (AddItemToBag(itemNew, "nhiệm vụ"))
                            {
                                UpdatePhukien();
                                zone.ZoneHandler.SendMessage(Service.ItemMapMePick(itemMap.Id, itemNew.Quantity, "a"));
                                SendMessage(Service.ServerMessage("Bạn nhận được " + itemTemplate.Name));
                            }
                        }
                        break;
                    case 78:
                        TaskHandler.gI().PlusSubTask(Character, 1);
                        TaskHandler.gI().DoSendMessage(Character, "Wow, một cậu bé dễ thương\nHãy bế cậu bé về nhà!");
                        if (AddItemToBag(itemNew, "nhiệm vụ"))
                        {
                            UpdatePhukien();
                            zone.ZoneHandler.SendMessage(Service.ItemMapMePick(itemMap.Id, itemNew.Quantity, "a"));
                        }

                        break;
                    case 861:
                        Character.PlusDiamondLock(itemNew.Quantity);
                        //  Character.DataBoMong.Count[16]++;
                        // Character.InfoChar.DiamondLock += itemNew.Quantity;
                        SendMessage(Service.MeLoadInfo(Character));
                        // if (itemNew.Quantity > 32767)
                        // {
                        SendMessage(Service.ServerMessage("Bạn nhặt được " + ServerUtils.GetMoney(itemNew.Quantity) + " ruby"));
                        zone.ZoneHandler.SendMessage(Service.ItemMapMePick(itemMap.Id, itemNew.Quantity, "a"));
                        // }

                        break;
                    case 372:
                    case 373:
                    case 374:
                    case 375:
                    case 376:
                    case 377:
                    case 378:
                        var timeserver = ServerUtils.CurrentTimeMillis();

                        if (BlackballCache.currTimeCanPick > timeserver)
                        {
                            Character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn sẽ được lụm sau " + (BlackballCache.currTimeCanPick - timeserver) / 1000 + "s nữa"));
                            return;
                        }

                        Character.Blackball.PickBlackball(Character, itemNew.Id);
                        Character.CharacterHandler.SendMessage(Service.SendImageBag(Character.Id, 107));
                        break;
                    case 353:
                    case 354:
                    case 355:
                    case 356:
                    case 357:
                    case 358:
                    case 359:
                        if (Character.DataNgocRongNamek.AlreadyPick(Character))
                        {
                            Character.CharacterHandler.SendMessage(Service.ServerMessage("Chỉ được nhặt 1 viên ngọc rồng namec"));
                            break;
                        }
                        Character.DataNgocRongNamek.DelayAction = 60000 + ServerUtils.CurrentTimeMillis();
                        Character.DataNgocRongNamek.DelayWish = 600000 + ServerUtils.CurrentTimeMillis();
                        Character.DataNgocRongNamek.IdNamekBall = itemNew.Id;
                        Init.NamecBalls.FirstOrDefault(i => i.Id == itemNew.Id).PlayerPick = Character.Id;
                        Character.InfoChar.Bag = 108;
                        Character.CharacterHandler.UpdatePhukien();
                        Character.InfoChar.TypePk = 5;
                        Character.CharacterHandler.SendZoneMessage(Service.ChangeTypePk(Character.Id, 5));
                        Character.CharacterHandler.SendMessage(Service.ServerMessage($"Bạn nhận được {ItemCache.ItemTemplate(itemNew.Id).Name} !"));
                        break;


                    case 516:
                        {
                            PlusHp((int)Character.HpFull / 10);
                            PlusMp((int)Character.MpFull / 10);
                            SendMessage(Service.SendHp((int)Character.InfoChar.Hp));
                            SendMessage(Service.SendMp((int)Character.InfoChar.Mp));
                            zone.ZoneHandler.SendMessage(Service.PlayerLevel(Character), Character.Id);
                            zone.ZoneHandler.SendMessage(Service.ItemMapMePick(itemMap.Id, itemNew.Quantity, "a"));
                            SendMessage(Service.ServerMessage("Bạn nhận được " + itemTemplate.Name));

                            break;
                        }
                    case 20:
                        if (TaskHandler.CheckTask(Character, 9, 1))
                        {
                            TaskHandler.gI().PlusSubTask(Character, 1);
                            if (AddItemToBag(itemNew, "nhiệm vụ"))
                            {
                                zone.ZoneHandler.SendMessage(Service.ItemMapMePick(itemMap.Id, itemNew.Quantity, "a"));
                                SendMessage(Service.SendBag(Character));
                                SendMessage(Service.ServerMessage("Bạn nhận được " + itemTemplate.Name));

                            }
                        }
                        if (AddItemToBag(true, itemNew, "nhiệm vụ"))
                        {
                            Character.Zone.ZoneHandler.SendMessage(Service.ItemMapMePick(itemMap.Id, itemNew.Quantity, "a"));
                            SendMessage(Service.SendBag(Character));

                            SendMessage(Service.ServerMessage("Bạn nhận được " + itemTemplate.Name));

                        }
                        break;
                    case 74:
                        {

                            if (Character.InfoChar.MapId - Character.InfoChar.Gender != 21)
                            {
                                if (TaskHandler.CheckTask(Character, 2, 0))
                                {
                                    TaskHandler.gI().PlusSubTask(Character, 1);
                                    if (AddItemToBag(true, itemNew, "nhiệm vụ"))
                                    {
                                        Character.Zone.ZoneHandler.SendMessage(Service.ItemMapMePick(itemMap.Id, itemNew.Quantity, "a"));
                                        SendMessage(Service.SendBag(Character));
                                        SendMessage(Service.ServerMessage("Bạn nhận được " + itemTemplate.Name));
                                    }
                                }
                            }
                            else
                            {

                                PlusHp((int)Character.HpFull);
                                PlusMp((int)Character.MpFull);
                                PlusStamina((int)Character.InfoChar.MaxStamina);
                                SendMessage(Service.SendHp((int)Character.InfoChar.Hp));
                                SendMessage(Service.SendMp((int)Character.InfoChar.Mp));
                                SendMessage(Service.SendStamina(Character.InfoChar.Stamina));
                                Character.Zone.ZoneHandler.SendMessage(Service.PlayerLevel(Character), Character.Id);
                                Character.Zone.ZoneHandler.SendMessage(Service.ItemMapMePick(itemMap.Id, itemNew.Quantity, "a"));
                                SendMessage(Service.ServerMessage("Bạn nhận được " + itemTemplate.Name));

                            }
                            break;


                        }
                    case 568: //Trứng ma bư
                        {
                            // if (Character.InfoChar.ThoiGianTrungMaBu > 0)
                            // {
                            Character.InfoChar.ThoiGianTrungMaBu += (DataCache.TRUNG_MA_BU_TIME + ServerUtils.CurrentTimeMillis());
                            if (AddItemToBag(false, itemNew, "Nhặt từ map")) SendMessage(Service.SendBag(Character));
                            zone.ZoneHandler.SendMessage(Service.ItemMapMePick(itemMap.Id, itemNew.Quantity, "a"));
                            SendMessage(Service.ServerMessage("Bạn đã nhặt được một quả trứng Ma Bư\nHãy về nhà kiểm tra"));
                            // }

                            break;
                        }
                    case 933:
                        {
                            // if(Character.LengthBagNull() < 1) {
                            //     SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_BAG));
                            //     return;
                            // }
                            // mảnh vỡ
                            var itemManhVoBongTai = GetItemBagById(933);
                            if (itemManhVoBongTai != null)
                            {
                                var soLuongManhVoBongTaiHT = itemManhVoBongTai.Options.FirstOrDefault(opt => opt.Id == 31); //Số lượng bông tai
                                var soLuongManhVoBongTaiDrop = itemNew.Options.FirstOrDefault(opt => opt.Id == 31);
                                if (soLuongManhVoBongTaiHT != null && soLuongManhVoBongTaiDrop != null)
                                {
                                    soLuongManhVoBongTaiHT.Param += soLuongManhVoBongTaiDrop.Param;
                                }
                                else
                                {
                                    soLuongManhVoBongTaiHT.Param += 1;//default
                                }
                            }
                            else
                            {
                                if (!AddItemToBag(true, itemNew, "Nhặt từ map")) return;
                            }
                            SendMessage(Service.SendBag(Character));
                            SendMessage(Service.ItemMapMePick(itemMap.Id, itemNew.Quantity, TextServer.gI().EMPTY));
                            break;
                        }
                    case 992://nhẫn thời không
                        {
                            if (TaskHandler.CheckTask(Character, 32, 0))
                            {
                                TaskHandler.gI().PlusSubTask(Character, 1);
                            }
                            var itemNhanThoiKhongBag = Character.CharacterHandler.GetItemBagById(992);
                            var itemNhanThoiKhongBox = Character.CharacterHandler.GetItemBoxById(992);
                            if (itemNhanThoiKhongBag != null || itemNhanThoiKhongBox != null)
                            {
                                SendMessage(Service.ServerMessage("Bạn đã có Nhẫn thời không sai lệch, không thể nhặt thêm"));
                                return;
                            }
                            SendMessage(Service.ItemMapMePick(itemMap.Id, itemNew.Quantity, TextServer.gI().EMPTY));
                            if (AddItemToBag(false, itemNew, "Nhặt từ map")) SendMessage(Service.SendBag(Character));
                            SendMessage(Service.ServerMessage("Bạn nhận được " + itemTemplate.Name));

                            break;
                        }
                    case 1152://trứng linh thú
                        {
                            var timeServer = ServerUtils.CurrentTimeSecond();
                            var expireHours = 12;
                            var expireTime = timeServer + (expireHours * 3600);
                            itemNew.Options.Add(new OptionItem()
                            {
                                Id = 211,
                                Param = expireHours,
                            });
                            var optionHiden = itemNew.Options.FirstOrDefault(option => option.Id == 73);

                            if (optionHiden != null)
                            {
                                optionHiden.Param = expireTime;
                            }
                            else
                            {
                                itemNew.Options.Add(new OptionItem()
                                {
                                    Id = 73,
                                    Param = expireTime,
                                });
                            }
                            SendMessage(Service.ItemMapMePick(itemMap.Id, itemNew.Quantity, TextServer.gI().EMPTY));
                            if (AddItemToBag(false, itemNew, "Nhặt từ map")) SendMessage(Service.SendBag(Character));
                            SendMessage(Service.ServerMessage("Bạn nhận được " + itemTemplate.Name));

                            break;
                        }

                    default:
                        {
                            var text = TextServer.gI().EMPTY;
                            switch (itemTemplate.Type)
                            {
                                case 9:
                                    {
                                        Character.PlusGold(itemNew.Quantity);
                                        HangNgayQuest_Handler.DoTask(Character, itemNew.Quantity, HangNgayQuest_Type.PICK_GOLD, 180);
                                        // Character.InfoChar.Gold += itemNew.Quantity;
                                        SendMessage(Service.MeLoadInfo(Character));
                                        if (itemNew.Quantity > 32767)
                                        {
                                            text = "Bạn nhặt được " + ServerUtils.GetMoney(itemNew.Quantity) + " vàng";
                                        }
                                        break;
                                    }
                                case 10:
                                    {
                                        Character.PlusDiamond(itemNew.Quantity);
                                        //Character.DataBoMong.Count[16]++;
                                        BoMongQuest_Handler.gI().PlusSubTask(Character, BoMongQuest_Template.TRUM_NHAT_NGOC);
                                        // Character.InfoChar.Diamond += itemNew.Quantity;
                                        SendMessage(Service.MeLoadInfo(Character));
                                        //if (itemNew.Quantity > 32767)
                                        //{
                                        text = "Bạn nhặt được " + ServerUtils.GetMoney(itemNew.Quantity) + " ngọc";
                                        //}
                                        break;
                                    }
                                case 34:
                                    {
                                        Character.PlusDiamondLock(itemNew.Quantity);
                                        BoMongQuest_Handler.gI().PlusSubTask(Character, BoMongQuest_Template.TRUM_NHAT_NGOC);
                                        // Character.DataBoMong.Count[16]++;
                                        // Character.InfoChar.DiamondLock += itemNew.Quantity;
                                        SendMessage(Service.MeLoadInfo(Character));
                                        //  if (itemNew.Quantity > 32767)
                                        //  {
                                        text = "Bạn nhặt được " + ServerUtils.GetMoney(itemNew.Quantity) + " ruby";
                                        // }
                                        break;
                                    }
                                default:
                                    {

                                        if (Character.LengthBagNull() < 1)
                                        {
                                            SendMessage(Service.ServerMessage(TextServer.gI().NOT_ENOUGH_BAG));
                                            return;
                                        }
                                        if (AddItemToBag(true, itemNew, "Nhặt từ map"))
                                        {
                                            SendMessage(Service.SendBag(Character));
                                        }
                                        else return;
                                    }
                                    break;
                            }
                            SendMessage(Service.ItemMapMePick(itemMap.Id, itemNew.Quantity, text));

                            break;
                        }

                }
            }
            zone.ZoneHandler.SendMessage(Service.ItemMapPlayerPick(itemMap.Id, Character.Id), Character.Id);
            zone.ZoneHandler.RemoveItemMap(itemMap.Id);
            //    }
            //


            // catch (Exception e)
            // {
            //      Server.Gi().Logger.Error($"Error Send Handshake Message in Service.cs: {e.Message} \n {e.StackTrace}", e);
            //  }
        }


        public void LeaveItem(ICharacter character)
        {
            // Ignore
        }

        public void Dispose()
        {
            SuppressFinalize(this);
        }
    }
}

