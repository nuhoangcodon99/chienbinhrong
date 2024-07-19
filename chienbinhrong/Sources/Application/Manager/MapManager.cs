using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NgocRongGold.Application.Interfaces.Map;
using NgocRongGold.DatabaseManager;
using NgocRongGold.Model.Character;
using NgocRongGold.Application.Interfaces.Character;
using System.Threading.Tasks;
using Application.Map;
using Application.Interfaces.Map;
using NgocRongGold.Application.Constants;

namespace NgocRongGold.Application.Manager
{
    public class MapManager
    {
        public static int IdBase = 0;
        public static readonly ConcurrentDictionary<int, IMapCustom> Enrtys = new ConcurrentDictionary<int, IMapCustom>();

        private static readonly List<IMap> Maps = new List<IMap>();

        public static bool isDragonHasAppeared = false;
        public static long delayCallDragon = 600000;
        public static int IdPlayerCallDragon = -1;
        public static long timeUoc = -1;
        public static IMap Get(int id)
        {

            return Maps.FirstOrDefault(x => x.Id == id);
        }
        public static MapOffline GetMapOffline(int id)
        {
            return (MapOffline)Maps.FirstOrDefault(x => x.Id == id);

        }


        public static void InitMapServer()
        {
            Cache.Gi().TILE_MAPS.ForEach(x =>
            {
                switch (x.Id)
                {
                    case int i when DataCache.IdMapReddot.Contains(i):
                        Maps.Add(new MapRedRibbon(x.Id));
                        break;
                    case int i when DataCache.IdMapKarin.Contains(i):
                        Maps.Add(new MapOffline(x.Id));
                        break;
                    case int i when DataCache.IdMapBDKB.Contains(i):
                        Maps.Add(new MapTreasure(x.Id));
                        break;
                    case int i when DataCache.IdMapCDRD.Contains(i):
                        Maps.Add(new MapSnakeRoad(x.Id));
                        break;
                    default:
                        Maps.Add(new Threading.Map(x.Id, x));
                        break;
                }
            });
        }

        public static void JoinMap(Character @char, int mapId, int zoneId, bool isDefault, bool isTeleport, int typeTeleport)
        {
            OutMap(@char, mapId);
            var map = Get(mapId);
            map?.JoinZone(@char, zoneId, isDefault, isTeleport, typeTeleport );
        }

        public static void OutMap(Character @char, int mapNextId)
        {
            var map = Get(@char.InfoChar.MapId);
            map?.OutZone(@char);
        }

        // For only once dragon apprea
        public static void SetDragonAppeared(bool toggle)
        {
            isDragonHasAppeared = toggle;
        }

        public static bool IsDragonHasAppeared()
        {
            return isDragonHasAppeared;
        }
    }
}