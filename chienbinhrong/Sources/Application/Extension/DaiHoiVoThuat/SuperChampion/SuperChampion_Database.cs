using Newtonsoft.Json;
using NgocRongGold.Application.Threading;
using NgocRongGold.DatabaseManager;
using NgocRongGold.Model.Character;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.DaiHoiVoThuat.SuperChampion
{
    public class SuperChampion_Database
    {
        public static void Update()
        {
            if (TRUNCATE())
            {
                foreach (var championer in SuperChampion_Manager.Entrys)
                {
                    if (Create(championer.Key, championer.Value))
                    {
                        // Server.Gi().Logger.Print("Save Success !", "cyan");
                    }

                }
            }
        }
        public static bool TRUNCATE()
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToData();
                    using (DbCommand command = DbContext.gI()?.Connection.CreateCommand())
                    {
                        command.CommandText =
                            $"TRUNCATE TABLE `siêu hạng`";

                        command.ExecuteNonQuery();

                        return true;
                    }
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Error TRUNCATE SIEU HANG: {e.Message}\n{e.StackTrace}");
                    return false;
                }
                finally
                {

                }
            }
        }
        public static bool Create(int top, SuperChampion_Championer superChampion_Championer)
        {
            lock (Server.SQLLOCK)
            {
                try
                {
                    DbContext.gI()?.ConnectToData();
                    using (DbCommand command = DbContext.gI()?.Connection.CreateCommand())
                    {
                        command.CommandText =
                            $"INSERT INTO `siêu hạng` (`Top`, `PlayerId`, `Championer`) VALUES ('{top}', '{superChampion_Championer.PlayerID}', '{JsonConvert.SerializeObject(superChampion_Championer)}')";

                        command.ExecuteNonQuery();

                        return true;
                    }
                }
                catch (Exception e)
                {
                    Server.Gi().Logger.Error($"Error Login User: {e.Message}\n{e.StackTrace}");
                    return false;
                }
                finally
                {
                    
                }
            }
        }
    }
}
