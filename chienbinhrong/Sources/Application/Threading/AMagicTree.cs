using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Manager;
using Org.BouncyCastle.Math.Field;

namespace NgocRongGold.Application.Threading
{
    public class MagicTreeRunTime
    {
        public bool IsStop = false;
        public static int RunTimeUpdate1 = -1;
        public static bool IsRunTimeSave = true;

        public MagicTreeRunTime()
        {
            
        }
        public Task Runtime { get; set; }
        public void StartMagicTree()
        {
            Task.Factory.StartNew(MagicTree);
        }
        public void MagicTree()
        {
            
                while (Server.Gi().IsRunning)
                {
                    var now = ServerUtils.TimeNow();

                    if (now.Hour == 1 && now.Minute == 0 && IsRunTimeSave) 
                    {
                        IsRunTimeSave = false;
                        Parallel.ForEach(MagicTreeManager.Entrys.Values.ToList(), tree => tree.MagicTreeHandler.Update(0));
                    }
                    else if(RunTimeUpdate1 != now.Minute)
                    {
                        RunTimeUpdate1 = now.Minute;
                        Parallel.ForEach(MagicTreeManager.Entrys.Values.ToList(), tree => tree.MagicTreeHandler.Update(1));
                        if (now.Hour != 1 && !IsRunTimeSave) IsRunTimeSave = true;
                    }
                    Thread.Sleep(1000);
                }
                MagicTreeManager.Entrys.Values.ToList().ForEach(tree => tree.MagicTreeHandler.Flush());
                Server.Gi().Logger.Print("MagicTree Manager is close...","red");
                IsStop = true;
        }
    }
}