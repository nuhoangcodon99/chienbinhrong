﻿
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Manager;

namespace NgocRongGold.Application.Threading
{
    public class ClanRunTime
    {
        public bool IsStop = false;
        public static bool IsRunTimeSave = true;
        public static int TimeUpdate = -1;
        public static int TimeUpdate2 = -1;
        public Task RunTime { get; set; }

        public ClanRunTime()
        {
            
        }
        public void StartClan()
        {
            new Thread(new ThreadStart(Clan)).Start() ;
        }
        public void Clan()
        {
           
                while (Server.Gi().IsRunning)
                {
                    var now = ServerUtils.TimeNow();
                    if (now.Hour == 1 && now.Minute == 0)
                    {
                        if (IsRunTimeSave)
                        {
                            IsRunTimeSave = false;
                            Parallel.ForEach(ClanManager.Entrys.Values.ToList(), clan => clan.ClanHandler.Update(0));
                        }
                    }
                    else if(now.Hour != 1)
                    {
                        if (!IsRunTimeSave) IsRunTimeSave = true;
                    }

                    if (TimeUpdate != now.Minute)
                    {
                        TimeUpdate = now.Minute;
                        Parallel.ForEach(ClanManager.Entrys.Values.ToList(), clan => clan.ClanHandler.Update(1));
                    }

                    if (now.Minute % 10 == 0 && TimeUpdate2 != now.Minute)
                    {
                        TimeUpdate2 = now.Minute;
                        Parallel.ForEach(ClanManager.Entrys.Values.ToList(), clan => clan.ClanHandler.Update(2));
                    }
                    
                    Parallel.ForEach(ClanManager.Entrys.Values.ToList(), clan => clan.ClanHandler.Update(3));
                      Thread.Sleep(1000);
                }
                ClanManager.Entrys.Values.ToList().ForEach(tree => tree.ClanHandler.Flush());
                Server.Gi().Logger.Print("Clan Manager is close...", "red");
                IsStop = true;
        }
    }
}