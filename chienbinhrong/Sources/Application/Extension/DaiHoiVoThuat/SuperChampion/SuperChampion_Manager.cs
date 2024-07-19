using NgocRongGold.Application.Extension.Yardat;
using NgocRongGold.Application.Threading;
using NgocRongGold.DatabaseManager.Player;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Clan;
using Org.BouncyCastle.Math.Field;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.DaiHoiVoThuat.SuperChampion
{
    public class SuperChampion_Manager
    {
        public static readonly ConcurrentDictionary<int, SuperChampion_Championer> Entrys = new ConcurrentDictionary<int, SuperChampion_Championer>();
        public static SuperChampion_Manager instance { get; set; }
        public static SuperChampion_Manager gI()
        {
            if (instance == null) instance = new SuperChampion_Manager();
            return instance;
        }
        public void CheckRank(SuperChampion_Championer superChampion_Championer)
        {

        }
        public SuperChampion_Championer Get(int id)
        {
           
            return Entrys.Values.FirstOrDefault(pl => pl.PlayerID == id);
        }
        public void Swap(SuperChampion_Championer cham1, SuperChampion_Championer cham2)
        {
        }
        public bool Add(SuperChampion_Championer champion_Championer)
        {
            return Entrys.TryAdd(Entrys.Count, champion_Championer);
        }
        public List<SuperChampion_Championer> GetList(params int[] top)
        {
            lock (Entrys)
            {
                if (top[1] <= 0)
                {
                    top[1] = 10;
                    top[0] = 1; 
                }

                var list = Entrys.Values
                    .Where(entry => entry.Top >= top[0] && entry.Top <= top[1])
                    .OrderBy(entry => entry.Top)
                    .ToList();

              

                return list;
            }
        }

        public Boss Clone(Character character,int id)
        {
            var characterClone = CharacterDB.GetById(id);
            if (characterClone == null) return null;
            var boss = new Boss();
            if (character != null)
            {
                boss.CreateBossClone(characterClone, characterClone.HpFull, characterClone.MpFull, characterClone.DamageFull, characterClone.DefenceFull);
                boss.CharacterHandler.SetUpInfo();
                character.Zone.ZoneHandler.AddBoss(boss);
            }
            else
            {
                boss.Dispose();
            }
            return boss;
        }
        public void RemoveClone(Boss boss)
        {
            boss.CharacterHandler.SendDie();
        }
    }
}
