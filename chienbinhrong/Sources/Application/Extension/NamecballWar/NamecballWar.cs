using NgocRongGold.Application.IO;
using NgocRongGold.Application.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.NamecballWar
{
    public class NamecballWar
    {
        public static readonly List<string> NameTeam = new List<string> { "Ca Đíc", "Fide"}; // +248 || 144 , - 2218, 600 
        public static readonly List<long> CurrDaysOpen = new List<long> { 3, 5, 7 };
        public static readonly long currTimeRegister = 25; // 18 gio 25
        public static readonly long currTimeStart = 30; // 18 gio 30
        public static readonly List<List<String>> ListMenus = new List<List<String>>() { new List<string> { "Tranh ngọc\nrồng Namec" }, { new List<string> { "Hướng dẫn", "Đổi điểm\nthưởng\n[{0}]", "Bảng\nxếp hạng", "Từ chối" } } };
        public static NamecballWar_Info Cadic = new NamecballWar_Info();
        public static NamecballWar_Info Fide = new NamecballWar_Info();
        public static NamecballWar_Status_Server Status_Server = NamecballWar_Status_Server.CLOSE;
        public static NamecballWar instance;
        public static NamecballWar gI()
        {
            if (instance == null) instance = new NamecballWar();
            return instance;
        }
        public void Runtime()
        {
            while (Server.Gi().IsRunning)
            {
                switch (Status_Server)
                {
                    case NamecballWar_Status_Server.CLOSE:
                        if (CanOpen())
                        {
                            Status_Server = NamecballWar_Status_Server.REGISTER;
                        }
                        break;
                    case NamecballWar_Status_Server.REGISTER:
                        if (CanStart())
                        {
                            Status_Server = NamecballWar_Status_Server.OPEN;
                        }
                        break;
                    case NamecballWar_Status_Server.OPEN:
                        if (!CanOpen())
                        {
                            Status_Server = NamecballWar_Status_Server.CLOSE;
                        }
                        break;
                }
                Thread.Sleep(1000);
            }
        }
        public bool CanStart()
        {
            var now = ServerUtils.TimeNow();
            return (now.DayOfWeek is (DayOfWeek.Tuesday or DayOfWeek.Thursday or DayOfWeek.Saturday)) && (now.Hour is 18) && (now.Minute >= currTimeStart);
        }
        public bool CanOpen()
        {
            var now = ServerUtils.TimeNow();
            return (now.DayOfWeek is (DayOfWeek.Tuesday or DayOfWeek.Thursday or DayOfWeek.Saturday)) && (now.Hour is 18) && (now.Minute > currTimeRegister);
        }
       
    }
}
