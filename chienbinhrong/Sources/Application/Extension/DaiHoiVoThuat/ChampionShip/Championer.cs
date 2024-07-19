using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.MainTasks;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.Model.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.DaiHoiVoThuat
{
    public class Championer
    {
        public int Posistion { get; set; }
        public int TeamId { get; set; }
        public int Enemy { get; set; }
        public ChampionerCS23_Status Status = ChampionerCS23_Status.NORMAL;
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                        