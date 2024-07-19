using NgocRongGold.Application.Threading;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.DaiHoiVoThuat.SuperChampion
{
    public class SuperChampion_Championer
    {
        public int Top { get; set; }
        public int PlayerID { get; set; }
        public SuperChampion_Championer_Status Status { get; set; } = SuperChampion_Championer_Status.NORMAL;
        public int Price { get; set; } 
        public int Ticket { get; set; } 
        public int Win { get; set; }
        public int Lose { get; set; }
        public InfoFriend Championer { get; set; }// point
        public List<SuperChampion_Championer_History> Historys { get; set; }// lich su dau
        public long Time { get; set; }
        public SuperChampion_Championer_Handler Handler { get; set; }
        public int PlayerChallenge { get; set; }
        public DateTime DateGetRuby { get; set; }
        public SuperChampion_Championer()
        {
            Championer = null;
            Historys = new List<SuperChampion_Championer_History>();
            Handler = new SuperChampion_Championer_Handler();
            Time = -1;
            Win = 0;
            Lose = 0;
            Price = 0;
            Ticket = 3;
            DateGetRuby = DateTime.Now;
             PlayerID = -1 ;
           
        }
        
        public void UpdateData(InfoFriend me, bool newDay = false)
        {
            Championer = me;
            Time = -1;
            if (newDay) Ticket = 3;
            Price = 0;
            PlayerID = me.Id;
        }
        public int GetRuby()
        {
            int ruby = 0;
            ruby = Top switch
            {
                1 => 1000,
                <= 10 => 100,
                <= 100 => 10,
                _ => 0,
            };
            return ruby;
        }
    }
}
