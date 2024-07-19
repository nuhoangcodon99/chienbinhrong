using Application.Interfaces.Zone;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Manager;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Template;
using Org.BouncyCastle.Math.Field;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Threading
{
    public class APet2
    {
        public enum TemplatePET
        {
            HO_MAP_VANG = 942,
            HO_MAP_TRANG = 943,
            HO_MAP_XANH = 944,
            KI_LAN = 763,
        }
        public long delayHoMapVang = -1;
        public int countHoMapVang = 0;
        public long delayHoMapTrang = -1;
        public int countHoMapTrang = 0;
        public long delayHoMapXanh = -1;
        public int countHoMapXanh = 0;
        public List<int> mapTraiDat = new List<int> { 0,1,2,3, 4, 5,27, 28, 29, 30 };
        public IZone getZone(List<int> maps, int ZoneSelect, int mapSelect = -1)
        {
            var map = mapSelect;
            if (map == -1 && maps != null)
            {
                map = maps[ServerUtils.RandomNumber(maps.Count)];
            }
            return MapManager.Get(map).GetZoneById(RandZone(ZoneSelect));
        }
        public int RandZone(int zoneSelect = -1)
        {
            return zoneSelect != -1 ? zoneSelect : ServerUtils.RandomNumber(0, 19);
        }
        public void StartUpdate()
        {
            Task.Run(() =>
            {
                while (Server.Gi().IsRunning)
                {
                    var timeServer = ServerUtils.CurrentTimeMillis();
                    if (timeServer > delayHoMapVang)
                    {
                        delayHoMapVang = 30000 + ServerUtils.CurrentTimeMillis();
                        if (countHoMapVang > 5)
                        {
                            continue;
                        }
                        countHoMapVang++;
                        var zone = getZone(mapTraiDat, 0);
                        //var HoMapVang = new Pet2(zone, (int)TemplatePET.HO_MAP_VANG, "Hổ Mập Golden");
                        var HoMapVang = new Pet2(zone, (int)TemplatePET.KI_LAN, "Kì Lân Con");
                        zone.ZoneHandler.AddPet(HoMapVang);
                        
                    }
                    if (timeServer > delayHoMapTrang)
                    {
                        delayHoMapTrang = 30000 + ServerUtils.CurrentTimeMillis();
                        if (countHoMapTrang > 5)
                        {
                            continue;
                        }
                        countHoMapTrang++;
                        var zone = getZone(mapTraiDat, 0);
                        //var HoMapTrang = new Pet2(zone, (int)TemplatePET.HO_MAP_TRANG, "Hổ Mập Sliver");
                        var HoMapTrang = new Pet2(zone, (int)TemplatePET.KI_LAN, "Kì Lân Con");

                        zone.ZoneHandler.AddPet(HoMapTrang);

                    }
                    if (timeServer > delayHoMapXanh)
                    {
                        delayHoMapXanh = 30000 + ServerUtils.CurrentTimeMillis();
                        if (countHoMapXanh > 5)
                        {
                            continue;
                        }
                        countHoMapXanh++;
                        var zone = getZone(mapTraiDat, 0);
 //                       var HoMapXanh = new Pet2(zone, (int)TemplatePET.HO_MAP_XANH,  "Hổ Mập Diamond");
                        var HoMapXanh = new Pet2(zone, (int)TemplatePET.KI_LAN, "Kì Lân Con");

                        zone.ZoneHandler.AddPet(HoMapXanh);

                    }
                    Thread.Sleep(1000);
                }
                Server.Gi().Logger.Print("Close Auto Pet2", "red");
            });
        }
    }
}
