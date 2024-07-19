using System;
using System.Linq;
using Newtonsoft.Json;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.DatabaseManager.Player;
using NgocRongGold.Model.Clan;
using NgocRongGold.Model.Info;
using Org.BouncyCastle.Math.Field;

namespace NgocRongGold.Application.Handlers.Character
{
    public class ClanHandler : IClanHandler
    {
        public Clan Clan { get; set; }

        public ClanHandler(Clan clan)
        {
            Clan = clan;
        }

        public void Update(int id)
        {
            lock (Clan)
            {
                switch (id)
                {
                    case 0:
                        {
                            Clan.Messages.Clear();
                            SendUpdateClan();
                            Clan.Reset();
                            Flush();
                            break;
                        }
                    //Check Pea
                    case 1:
                        {
                            if (Clan.CharacterPeas.Count > 0)
                            {
                                Clan.CharacterPeas.ToList().ForEach(cp =>
                                {
                                    if (Clan.Thành_viên.FirstOrDefault(check => check.Id == cp.PlayerRevice) != null) return;
                                    var mem = ClientManager.Gi().GetCharacter(cp.PlayerRevice);
                                    if (mem == null) return;
                                    var itemNew = ItemCache.GetItemDefault((short)cp.PeaId);
                                    itemNew.Quantity = cp.Quantity;
                                    if (mem.CharacterHandler.AddItemToBag(true, itemNew, "Nhận đậu từ clan"))
                                    {
                                        mem.CharacterHandler.SendMessage(Service.SendBag(mem));
                                    };
                                    mem.CharacterHandler.SendMessage(Service.ServerMessage(string.Format(TextServer.gI().RECEIVE_PEA_CLAN, ItemCache.ItemTemplate(itemNew.Id).Name, cp.PlayerGive)));
                                    Clan.CharacterPeas.Remove(cp);
                                });
                            }
                            break;
                        }
                    //Update clan
                    case 2:
                        {
                            SendUpdateClan();
                            break;
                        }
                    case 3:
                        try
                        {
                            var timeServer = ServerUtils.CurrentTimeMillis();

                            var dateTime = ServerUtils.TimeNow();

                            if (Clan.ClanDungeon.ConDuongRanDoc.Status is PhoBanStatus.OPEN)
                            {
                                Clan.ClanDungeon.ConDuongRanDoc.Update(timeServer);
                            }
                            if (Clan.ClanDungeon.BanDoKhoBau.Status is PhoBanStatus.OPEN)
                            {
                                Clan.ClanDungeon.BanDoKhoBau.Update(timeServer);
                            }
                            if (Clan.ClanDungeon.KhiGasHuyDiet.Status is PhoBanStatus.OPEN)
                            {
                                Clan.ClanDungeon.KhiGasHuyDiet.Update(timeServer);
                            }
                            if (Clan.ClanDungeon.DoanhTraiDocNhan.Status is PhoBanStatus.OPEN)
                            {
                                Clan.ClanDungeon.DoanhTraiDocNhan.Update(timeServer);
                            }
                            if (Clan.ClanBoss.Status is Model.Clan.ClanBoss.ClanBoss_Status.OPEN)
                            {
                                Clan.ClanBoss.Update(timeServer);
                            }
                        }
                        catch (Exception e)
                        {
                            Server.Gi().Logger.Print(e.Message + "\n" + e.StackTrace);
                        }
                        break;
                    
                }
            }
        }

        public void Flush()
        {
            ClanDB.Update(Clan);
        }

        public bool AddMember(Model.Character.Character character, int role = 0, bool isFlush = true)
        {

            lock (Clan.Thành_viên)
            {
                if (Clan.Thành_viên.FirstOrDefault(m => m.Id == character.Id) == null)
                {
                    var member = new ClanMember()
                    {
                        Id = character.Id,
                        Name = character.Name,
                        Head = character.GetHead(false),
                        Leg = character.GetLeg(false),
                        Body = character.GetBody(false),
                        Role = role,
                        Power = character.InfoChar.Power,
                        Cho_đậu = 0,
                        Nhận_đậu = 0,
                        Capsule_Bang = 0,
                        Capsule_Cá_Nhân = 0,
                        LastRequest = 0,
                        JoinTime = ServerUtils.CurrentTimeSecond(),
                        DateJoin = ServerUtils.TimeNow().Date
                    };
                    Clan.Thành_viên.Add(member);
                    Clan.Thành_viên_hiện_tại++;
                    SendMessage(Service.AddMemberClan(member));
                    if(isFlush) Flush();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void AddCharacterPea(CharacterPea characterPea)
        {
            lock (Clan.CharacterPeas)
            {
                var check = Clan.CharacterPeas.FirstOrDefault(c =>
                    c.PlayerRevice == characterPea.PlayerRevice && c.PlayerGive.Equals(characterPea.PlayerGive) && c.PeaId == characterPea.PeaId);
                if (check == null)
                {
                    Clan.CharacterPeas.Add(characterPea);
                }
                else
                {
                    check.Quantity += characterPea.Quantity;
                }
            }
        }

        public bool RemoveMember(int id)
        {
            lock (Clan.Thành_viên)
            {
                var mem = Clan.Thành_viên.FirstOrDefault(m => m.Id == id);
                if (mem != null)
                {
                    var index = Clan.Thành_viên.IndexOf(mem);
                    Clan.Thành_viên.RemoveAt(index);
                    Clan.Thành_viên_hiện_tại-=1;
                    SendMessage(Service.RemoveMemberClan(index));
                    Flush();
                    return true;
                }
                return false;
            }
        }

        public ClanMember GetMember(int id)
        {
            return Clan.Thành_viên.FirstOrDefault(member => member.Id == id);
        }

        public ClanMessage GetMessage(int id)
        {
            return Clan.Messages.FirstOrDefault(message => message.Id == id);
        }

        public void SendMessage(Message message)
        {
            lock (Clan.Thành_viên)
            {
                Clan.Thành_viên.ToList().ForEach(member =>
                {
                    var charMem = (Model.Character.Character)ClientManager.Gi().GetCharacter(member.Id);
                    charMem?.CharacterHandler.SendMessage(message);
                });
            }
        }

        public void UpdateClanId()
        {
            lock (Clan.Thành_viên)
            {
                Clan.Thành_viên.ToList().ForEach(member =>
                {
                    var charMem = (Model.Character.Character)ClientManager.Gi().GetCharacter(member.Id);
                    charMem?.CharacterHandler.SendZoneMessage(Service.UpdateClanId(charMem.Id, Clan.Id));
                });
            }
        }

        public void SendUpdateClan()
        {
            lock (Clan.Thành_viên)
            {
                Clan.Thành_viên.ToList().ForEach(member =>
                {
                    var charMem = (Model.Character.Character)ClientManager.Gi().GetCharacter(member.Id);
                    if (charMem != null)
                    {
                        member.Head = charMem.GetHead(false);
                        member.Body = charMem.GetBody(false);
                        member.Leg = charMem.GetLeg(false);
                        member.Power = charMem.InfoChar.Power;
                        charMem.CharacterHandler.SendMessage(Service.MyClanInfo(charMem));
                    }
                });
            }
        }

        public void Chat(ClanMessage message)
        {
            lock (Clan.Messages)
            {
                var check = Clan.Messages.FirstOrDefault(msg => msg.Id == message.Id);
                if (check != null)
                {
                    var index = Clan.Messages.IndexOf(check);
                    Clan.Messages.RemoveAt(index);
                    if (message.Recieve < message.MaxCap)
                    {
                        Clan.Messages.Insert(Clan.Messages.Count, message);
                    }
                }   
                else
                {
                    Clan.Messages.Add(message);
                    if (Clan.Messages.Count < 4)
                    {
                        SendUpdateClan();
                    }
                    else if(Clan.Messages.Count > 15)
                    {
                        var list = Clan.Messages.FirstOrDefault(msg => msg.Type != 1);
                        Clan.Messages.Remove(list);
                    }
                }
                SendMessage(Service.ClanMessage(message));
            }
        }
    }
}