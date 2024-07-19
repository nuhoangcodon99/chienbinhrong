using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.DaiHoiVoThuat
{
    public class ChampionerCS23
    {
        public int Zone { get; set; } = 0;
        public int Round { get; set; } = 0;
        public long Delay { get; set; } = 0;
        public int Count { get; set; } = 0;
        public ChampionerCS23_Status Status = ChampionerCS23_Status.NORMAL;
        public int WoodChestLevel { get; set; } = 0;
        public bool WoodChestCollect { get; set; } = false;
        public ChampionShip23_Handler Handler = new ChampionShip23_Handler();

    }
}