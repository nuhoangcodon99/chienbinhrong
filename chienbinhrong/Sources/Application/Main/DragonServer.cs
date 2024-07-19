using System;
using Microsoft.Extensions.Configuration;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Threading;
using NgocRongGold.Logging;
using NgocRongGold.DatabaseManager;
using NgocRongGold.DatabaseManager.Player;
using NgocRongGold.Application.Manager;

namespace NgocRongGold.Application.Main
{
    public partial class DragonBall
    {
        private static bool keepRunning = true;
        public static bool findErr = false;
        static void Main(string[] args)
        {

            IServerLogger logger = new ServerLogger();
              var configBuilder = new ConfigurationBuilder().SetBasePath(ServerUtils.ProjectDir("")).AddJsonFile("config.json");
            var configurationRoot = configBuilder.Build();
            DatabaseManager.ConfigManager.CreateManager(configurationRoot);

            Server.Gi().StartServer(true, logger, configurationRoot, false);

            Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e) {
                e.Cancel = true;
                DragonBall.keepRunning = false;
            };
            while (keepRunning)
            {

                var type = Console.ReadLine();
                if (type != null && type.Contains("baotri"))
                {
                    var time = 60;
                    try
                    {
                        time = Int32.Parse(type.Replace("baotri", ""));
                    }
                    catch (Exception)
                    {
                        // ignored
                    }

                    if (Maintenance.Gi().IsStart)
                    {
                        logger.Print($"Server is Maintained, time Left: {Maintenance.Gi().TimeCount} minutes...");
                    }
                    else
                    {
                        Maintenance.Gi().Start(time);
                        logger.Print($"Server will be under Maintenance Later: {time} minutes...");
                    }

                }
                else if (type == "restart")
                {
                    logger.Print("Server restarting...");
                    configBuilder = new ConfigurationBuilder().SetBasePath(ServerUtils.ProjectDir(""))
                        .AddJsonFile("config.json");
                    configurationRoot = configBuilder.Build();
                    DatabaseManager.ConfigManager.CreateManager(configurationRoot);
                    Server.Gi().RestartServer();
                }
                else if (type == "shutdown")
                {
                    logger.Print("Server stopping...");
                    Server.Gi().StopServer();
                }else if (type == "timloi")
                {
                    findErr = !findErr;
                    logger.Print("Find Error: " + findErr);
                }
                else
                {
                    ClientManager.Gi().SendMessage(Service.ServerMessageVip(type));
                }
            }
            logger.Print("Server stopping...");
            Server.Gi().StopServer();

        }
    }
}