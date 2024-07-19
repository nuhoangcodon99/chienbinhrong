using Newtonsoft.Json;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Manager;
using NgocRongGold.DatabaseManager.Player;
using NgocRongGold.DatabaseManager;
using NgocRongGold.Model.Character;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Messaging;
using System.Runtime.InteropServices;

namespace NgocRongGold.Application.Threading
{
    
    public class Backup
    {
        public Backup()
        {
        }
        public long delayBackup = DataCache._1MINUTES + ServerUtils.CurrentTimeMillis();
        private readonly object _lock = new object();
        public void Start()
        {
            new Thread(new ThreadStart(AutoBackupData)).Start();
        }
        public void AutoBackupData()
        {
            while (Server.Gi().IsRunning)
            {
                try
                {
                    var timeServer = ServerUtils.CurrentTimeMillis();
                    if (timeServer > delayBackup)
                    {
                        delayBackup = DataCache._1MINUTES + ServerUtils.CurrentTimeMillis();
                        var Temp = ClientManager.Gi().Characters.Values.ToList();
                        var textBackup = "";
                        for (int i = 0; i < Temp.Count; i++)
                        {
                            var character = ClientManager.Gi().GetCharacter(Temp[i].Id);
                            if (character == null) continue;
                            var charReal = (Character)character;
                            textBackup += UpdateStatement(charReal) + "\n";
                            if (charReal.InfoChar.IsHavePet)
                            {
                                textBackup += UpdateStatement(charReal.Disciple) + "\n";
                            }
                        }

                        ServerUtils.WriteLogBackup($"backup character and disciple {DateTime.Now.ToString("MM-dd-yyyy")}", textBackup);
                        ServerUtils.WriteLogBackup($"backup ki gui {DateTime.Now.ToString("MM-dd-yyyy")}", UpdateStatementKiGui());

                    }
                    Thread.Sleep(1000);
                }catch(Exception e)
                {
                    Server.Gi().Logger.Print("Error Backup at " + e.StackTrace + "\n" + e.Message);
                }
            }
        }
        public string UpdateStatement(Character character)
        {
            var text = $"`name` = '{character.Name}'";
            text += $", `Skills` = '{JsonConvert.SerializeObject(character.Skills)}'";
            text += $", `ItemBody` = '{JsonConvert.SerializeObject(character.ItemBody)}'";
            text += $", `ItemBag` = '{JsonConvert.SerializeObject(character.ItemBag)}'";
            text += $", `ItemBox` = '{JsonConvert.SerializeObject(character.ItemBox)}'";
            text += $", `InfoChar` = '{JsonConvert.SerializeObject(character.InfoChar)}'";
            text += $", `BoughtSkill` = '{JsonConvert.SerializeObject(character.BoughtSkill)}'";
            text += $", `InfoTask` = '{JsonConvert.SerializeObject(character.InfoTask)}'";
            text += $", `PlusBag` = {character.PlusBag}";
            text += $", `PlusBox` = {character.PlusBox}";
            text += $", `Friends` = '{JsonConvert.SerializeObject(character.Friends)}'";
            text += $", `Enemies` = '{JsonConvert.SerializeObject(character.Enemies)}'";
            text += $", `Me` = '{JsonConvert.SerializeObject(character.Me)}'";
            text += $", `ClanId` = {character.ClanId}";
            text += $", `LuckyBox` = '{JsonConvert.SerializeObject(character.LuckyBox)}'";
            text += $", `LastLogin` = '{character.LastLogin:yyyy-MM-dd HH:mm:ss}'";
            text += $", `SpecialSkill` = '{JsonConvert.SerializeObject(character.SpecialSkill)}'";
            text += $", `InfoBuff` = '{JsonConvert.SerializeObject(character.InfoBuff)}'";
            text += $", `DataEvent` = '{JsonConvert.SerializeObject(character.DiemSuKien)}'";
            text += $", `DataBlackBall` = '{JsonConvert.SerializeObject(character.Blackball.CurrentListBuff)}'";
            text += $", `DataBoMong` = '{JsonConvert.SerializeObject(character.DataBoMong)}'";
            text += $", `DataDaiHoiVoThuat` = '{JsonConvert.SerializeObject(character.DataDaiHoiVoThuat23)}'";
            text += $", `DataPractice` = '{JsonConvert.SerializeObject(character.DataPractice)}'";
            text += $", `DataSieuHang` = '{JsonConvert.SerializeObject(character.DataSieuHang)}'";
            text += $", `ItemGift` = '{JsonConvert.SerializeObject(character.ItemGift)}'";
            text += $", `DataVoDaiSinhTu` = '{JsonConvert.SerializeObject(character.DataVoDaiSinhTu)}'";
            text += $", `DataSideTask` = '{JsonConvert.SerializeObject(character.DataSideTask)}'";
            return $"UPDATE `character` SET {text}  WHERE `id` = {character.Id};"; ;
        }
        public string UpdateStatement(Disciple disciple)
        {
            var text = $"`name` = '{disciple.Name}'";
            text += $", `Status` = '{disciple.Status}'";
            text += $", `ItemBody` = '{JsonConvert.SerializeObject(disciple.ItemBody)}'";
            text += $", `Skills` = '{JsonConvert.SerializeObject(disciple.Skills)}'";
            text += $", `InfoChar` = '{JsonConvert.SerializeObject(disciple.InfoChar)}'";
            text += $", `Type` = '{disciple.Type}'";
            text += $", `Info` = '{JsonConvert.SerializeObject(disciple.Info)}'";
           
            return $"UPDATE `disciple` SET {text}  WHERE `id` = {disciple.Id};";
        }
        public string UpdateStatementKiGui()
        {
            var text = "TRUNCATE TABLE `shop kí gửi`\n";
            var listItem = CollectionsMarshal.AsSpan(Cache.Gi().KY_GUI_ITEMS.ToList());
            for (int i = 0; i < listItem.Length; i++)
            {
                var item = listItem[i];
                text += $"insert into `shop kí gửi` (`item_id`, `player_id`, `tab`, `page`,`id`, `buyType`, `Cost`, `quantity`, `itemOptions`, `isUpTop`, `isBuy`, `NamePlayer`) values ('{item.ItemId}', '{item.IdPlayerSell}', '{item.Tab}', '{item.Page}','{item.Id}', '{item.BuyType}', '{item.Cost}', '{item.quantity}', '{JsonConvert.SerializeObject(item.Options)}', '0', '{item.isBuy}', '{item.PlayerName}');\n";
            }
            return text;
        }
    }
}
