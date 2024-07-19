using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Constants;
using Org.BouncyCastle.Math.Field;
using NgocRongGold.Model;
using NgocRongGold.Model.Character;
using NgocRongGold.DatabaseManager;
using NgocRongGold.Application.Threading;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.DatabaseManager.Player;
using NgocRongGold.Model.Map;
using Org.BouncyCastle.Asn1.X509;
using NgocRongGold.Application.Extension.Bosses.Xinbato;
using NgocRongGold.Application.Extension.Bosses.SoiHecQuyn;
using Application.Interfaces.Zone;
using System.ComponentModel;
/// Code by baodubai dep trai vai lon
namespace NgocRongGold.Application.Threading
{

    public class ABoss
    {
        private static ABoss Instance { get; set; } = null;
        public bool IsStop = false;
        public static ABoss gI()
        {
            return Instance ??= new ABoss();
        }
        public int CountColer = 1;
        public Boss Mabu2 = null;
        public int IdMabu = -1;
        public bool isMabuSpawn = false;
        public string TileMapMabu = "";
        public int OldMapMabu = -1;
        public int OldZoneMabu = -1;
        public long DelayMabu = 16000 + ServerUtils.CurrentTimeMillis();
        #region XY
        public int XKuKu = 0;
        public int YKuku = 0;
        public int XRambo = 0;
        public int YRambo = 0;
        public int XMapDauDinh = 0;
        public int YMapDauDinh = 0;
        public Zone zoneKuku;
        #endregion
        #region CountBoss
        public int countOngGiaNoel = 0;
        public int countSo4 = 0;
        public int countSo3 = 0;
        public int countSo1 = 0;
        public int countTieuDoiTruong = 0;
        public int countKuku = 0;
        public int countMapDauDinh = 0;
        public int countRambo = 0;
        public int countXen1 = 0;
        public int countXen2 = 0;
        public int countXenHoanThien = 0;
        public int countAnd19 = 0;
        public int countAnd20 = 0;
        public int countPic = 0;
        public int countPoc = 0;
        public int countKingKong = 0;
        public int countAnd13 = 0;
        public int countAnd14 = 0;
        public int countAnd15 = 0;
        public int countChilled = 0;
        public int countChilled2 = 0;
        public int countBlackGoku = 0;
        public int countSuperBlackGoku = 0;
        public int countBroly = 0;
        public int countSuperBroly = 0;
        public int countNoONo = 0;
        #endregion

        #region IdBoss
        public int idBroly = -1;
        public int idOngGiaNoel = -1;
        public int idSo4 = -1;
        public int idSo2 = -1;
        public int idSo3 = -1;
        public int idSo1 = -1;
        public int idTieuDoiTruong = -1;
        public int idKuku = -1;
        public int idMapDauDinh = -1;
        public int idRambo = -1;
        public int idXen1 = -1;
        public int idXen2 = -1;
        public int idXenHoanThien = -1;
        public int idXenHoanThien2 = -1;
        public int idAnd19 = -1;
        public int idAnd20 = -1;
        public int idPic = -1;
        public int idPoc = -1;
        public int idKingKong = -1;
        public int idAnd13 = -1;
        public int idAnd14 = -1;
        public int idAnd15 = -1;
        public int idChilled = -1;
        public int idChilled2 = -1;
        public int idBlackGoku = -1;
        public int idSuperBlackGoku = -1;
        public int idFide1 = -1;
        public int idFide2 = -1;
        public int idFide3 = -1;
        public int idColer1 = -1;
        public int idColer2 = -1;
        #endregion

        #region Spawn
        public bool So4Spawn = false;
        public bool So3Spawn = false;

        public bool So2Spawn = false;
        public bool So1Spawn = false;
        public bool TieuDoiTruongSpawn = false;
        public bool KukuSpawn = false;
        public bool MapDauDinhSpawn = false;
        public bool RamboSpawn = false;
        public bool Xen1Spawn = false;
        public bool Xen2Spawn = false;
        public bool XenHoanThienSpawn = false;
        public bool XenHoanThienSpawn2 = false;
        public bool And19Spawn = false;
        public bool And20Spawn = false;
        public bool PicSpawn = false;
        public bool PocSpawn = false;
        public bool KingkongSpawn = false;
        public bool And13Spawn = false;
        public bool And14Spawn = false;
        public bool And15Spawn = false;
        public bool ChilledSpawn = false;
        public bool Chilled2Spawn = false;
        public bool BlackGokuSpawn = false;
        public bool SuperBlackGokuSpawn = false;
        public bool Fide1Spawn = false;
        public bool Fide2Spawn = false;
        public bool Fide3Spawn = false;
        public bool Coler1Spawn = false;
        public bool Coler2Spawn = false;
        #endregion

        #region mapSpawm
        public List<int> CoolerMaps = new List<int> { 107, 108, 110 };
        public List<int> NappaMaps = new List<int> { 68, 69, 70, 71, 72 };
        public List<int> BlackGokuMaps = new List<int> { 92, 93, 94 };
        public List<int> TDSTMaps = new List<int> { 82, 83, 79 };
        public List<int> FideMaps = new List<int> { 79, 80 };
        public List<int> ChilledMaps = new List<int> { 161 };
        public List<int> BaMapDauTuongLai = new List<int> { 92, 93, 94 };
        public List<int> PicPocKKMaps = new List<int> { 97, 98, 99 };
        public List<int> CellMaps = new List<int> { 100 };
        public List<int> PerfectCellMaps = new List<int> { 103 };
        public List<int> SanSauSieuThi = new List<int> { 104 };
        public List<int> mapTraiDat = new List<int> { 3, 4, 27, 28, 29, 30, 6, 10 };
        public List<List<int>> PosistionBroly = new List<List<int>> { new List<int> { 777, 408 }, new List<int> { 701, 360 }, new List<int> { 836, 336 }, new List<int> { 669, 312 }, new List<int> { 488, 288 }, new List<int> { 484, 336 }, new List<int> { 326, 288 }, new List<int> { 570, 360 } };
        #endregion

        #region Notify
        public bool So4Notify = false;
        public bool So3Notify = false;
        public bool So1Notify = false;
        public bool TieuDoiTruongNotify = false;
        public bool KukuNotify = false;
        public bool MapDauDinhNotify = false;
        public bool RamboNotify = false;
        public bool Xen1Notify = false;
        public bool Xen2Notify = false;
        public bool XenHoanThienNotify = false;
        public bool And19Notify = false;
        public bool And20Notify = false;
        public bool PicNotify = false;
        public bool PocNotify = false;
        public bool KingkongNotify = false;
        public bool And13Notify = false;
        public bool And14Notify = false;
        public bool And15Notify = false;
        public bool ChilledNotify = false;
        public bool Chilled2Notify = false;
        public bool BlackGokuNotify = false;
        public bool SuperBlackGokuNotify = false;
        public bool Fide1Notify = false;
        public bool Fide2Notify = false;
        public bool Fide3Notify = false;
        #endregion

        #region oldMap
        public int oldMapOngGiaNoel = -1;
        public int oldMapSo4 = -1;
        public int oldMapSo3 = -1;
        public int oldMapSo2 = -1;

        public int oldMapSo1 = -1;
        public int oldMapTieuDoiTruong = -1;
        public int oldMapKuku = -1;
        public int oldMapMapDauDinh = -1;
        public int oldMapRambo = -1;
        public int oldMapXen1 = -1;
        public int oldMapXen2 = -1;
        public int oldMapXenHoanThien = -1;
        public int oldMapXenHoanThien2 = -1;
        public int oldMapAnd19 = -1;
        public int oldMapAnd20 = -1;
        public int oldMapPic = -1;
        public int oldMapPoc = -1;
        public int oldMapKingKong = -1;
        public int oldMapAnd13 = -1;
        public int oldMapAnd14 = -1;
        public int oldMapAnd15 = -1;
        public int oldMapChilled = -1;
        public int oldMapChilled2 = -1;
        public int oldMapBlackGoku = -1;
        public int oldMapSuperBlackGoku = -1;
        public int oldMapFide1 = -1;
        public int oldMapFide2 = -1;
        public int oldMapFide3 = -1;
        public int oldMapBroly = -1;
        public int oldMapColer1 = -1;
        public int oldMapColer2 = -1;

        #endregion

        #region oldZone
        public int oldZoneOngGiaNoel = -1;
        public int oldZoneSo4 = -1;
        public int oldZoneSo2 = -1;
        public int oldZoneSo3 = -1;
        public int oldZoneSo1 = -1;
        public int oldZoneTieuDoiTruong = -1;
        public int oldZoneKuku = -1;
        public int oldZoneMapDauDinh = -1;
        public int oldZoneRambo = -1;
        public int oldZoneXen1 = -1;
        public int oldZoneXen2 = -1;
        public int oldZoneXenHoanThien = -1;
        public int oldZoneXenHoanThien2 = -1;
        public int oldZoneAnd19 = -1;
        public int oldZoneAnd20 = -1;
        public int oldZonePic = -1;
        public int oldZonePoc = -1;
        public int oldZoneKingKong = -1;
        public int oldZoneAnd13 = -1;
        public int oldZoneAnd14 = -1;
        public int oldZoneAnd15 = -1;
        public int oldZoneChilled = -1;
        public int oldZoneChilled2 = -1;
        public int oldZoneBlackGoku = -1;
        public int oldZoneSuperBlackGoku = -1;
        public int oldZoneFide1 = -1;
        public int oldZoneFide2 = -1;
        public int oldZoneFide3 = -1;
        public int oldZoneBroly = -1;
        #endregion

        #region Boss
        public Boss NoONo = null;
        public Boss Broly = null;
        public List<Boss> ListBroly = new List<Boss>();

        public Boss OngGiaNoel = null;
        public Boss So4 = null;
        public Boss So3 = null;
        public Boss So2 = null;
        public Boss So1 = null;
        public Boss TieuDoiTruong = null;
        public Boss Kuku = null;
        public Boss MapDauDinh = null;
        public Boss Rambo = null;
        public Boss Xen1 = null;
        public Boss Xen2 = null;
        public Boss XenHoanThien = null;
        public Boss XenHoanThien2 = null;
        public Boss And19 = null;
        public Boss And20 = null;
        public Boss Pic = null;
        public Boss Poc = null;
        public Boss KingKong = null;
        public Boss And13 = null;
        public Boss And14 = null;
        public Boss And15 = null;
        public Boss Chilled = null;
        public Boss Chilled2 = null;
        public Boss BlackGoku = null;
        public Boss SuperBlackGoku = null;
        public Boss Fide1 = null;
        public Boss Fide2 = null;
        public Boss Fide3 = null;
        public Boss SuperBroly = null;
        public Boss Mabu = null;
        public Boss Kidbu = null;
        public Boss BuTenk = null;
        public Boss GohanBu = null;

        #endregion

        #region Delay
        public long DelayOngGiaNoel = 0 + ServerUtils.CurrentTimeMillis();
        public long DelaySo4 = 300000 + ServerUtils.CurrentTimeMillis();
        public long DelaySo2 = 300000 + ServerUtils.CurrentTimeMillis();
        public long DelaySo3 = 300000 + ServerUtils.CurrentTimeMillis();
        public long DelaySo1 = 300000 + ServerUtils.CurrentTimeMillis();
        public long DelayTieuDoiTruong = 300000 + ServerUtils.CurrentTimeMillis();
        public long DelayKuku = 10000 + ServerUtils.CurrentTimeMillis();
        public long DelayMapDauDinh = 100000 + ServerUtils.CurrentTimeMillis();
        public long DelayRambo = 100000 + ServerUtils.CurrentTimeMillis();
        public long DelayXen1 = 20000 + ServerUtils.CurrentTimeMillis();
        public long DelayXen2 = 400000 + ServerUtils.CurrentTimeMillis();
        public long DelayXenHoanThien = 400000 + ServerUtils.CurrentTimeMillis();
        public long DelayXenHoanThien2 = 21000 + ServerUtils.CurrentTimeMillis();
        public long DelayAnd19 = 120000 + ServerUtils.CurrentTimeMillis();
        public long DelayAnd20 = 120000 + ServerUtils.CurrentTimeMillis();
        public long DelayPic = 21000 + ServerUtils.CurrentTimeMillis();
        public long DelayPoc = 21000 + ServerUtils.CurrentTimeMillis();
        public long DelayKingKong = 21000 + ServerUtils.CurrentTimeMillis();
        public long DelayAnd13 = 0 + ServerUtils.CurrentTimeMillis();
        public long DelayAnd14 = 0 + ServerUtils.CurrentTimeMillis();
        public long DelayAnd15 = 0 + ServerUtils.CurrentTimeMillis();
        public long DelayChilled = 15000 + ServerUtils.CurrentTimeMillis();
        public long DelayChilled2 = 15000 + ServerUtils.CurrentTimeMillis();
        public long DelayBlackGoku = 10000 + ServerUtils.CurrentTimeMillis();
        public long DelaySuperBlackGoku = 150000 + ServerUtils.CurrentTimeMillis();
        public long DelayBroly = 11000 + ServerUtils.CurrentTimeMillis();
        public long DelayFide1 = 60000 + ServerUtils.CurrentTimeMillis();
        public long DelayFide2 = 63000 + ServerUtils.CurrentTimeMillis();
        public long DelayFide3 = 66000 + ServerUtils.CurrentTimeMillis();
        public long DelaySuperBroly = 10000 + ServerUtils.CurrentTimeMillis();
        public long DelayNoONo = 32000 + ServerUtils.CurrentTimeMillis();
        #endregion

        #region oldTileMap
        public string TileMapOngGiaNoel = "";
        public string TileMapSo4 = "";
        public string TileMapSo3 = "";
        public string TileMapSo2 = "";

        public string TileMapSo1 = "";
        public string TileMapTieuDoiTruong = "";
        public string TileMapKuku = "";
        public string TileMapMapDauDinh = "";
        public string TileMapRambo = "";
        public string TileMapXen1 = "";
        public string TileMapXen2 = "";
        public string TileMapXenHoanThien = "";
        public string TileMapAnd19 = "";
        public string TileMapAnd20 = "";
        public string TileMapPic = "";
        public string TileMapPoc = "";
        public string TileMapKingKong = "";
        public string TileMapAnd13 = "";
        public string TileMapAnd14 = "";
        public string TileMapAnd15 = "";
        public string TileMapChilled = "";
        public string TileMapChilled2 = "";
        public string TileMapBlackGoku = "";
        public string TileMapSuperBlackGoku = "";
        public string TileMapFide1 = "";
        public string TileMapFide2 = "";
        public string TileMapFide3 = "";
        public string TileMapBroly = "";
        public string TileMapXenHoanThien2 = "";
        #endregion



        public void SendBossServerChat(string thongbao)
        {
            ClientManager.Gi().SendMessageCharacter(Service.ServerChat($"BOSS {thongbao}"));
        }
        public void Output(string output)
        {
            Server.Gi().Logger.Print(output, "green");
        }
        public Zone PickMapSpawn(int mapId, int zoneId = 0)
        {
            if (zoneId == 0) zoneId = ServerUtils.RandomNumber(20);
            return (Zone)MapManager.Get(mapId).Zones[zoneId];
        }
        public Zone PickMapSpawnNotHasBosses(int mapId)
        {
            return (Zone)MapManager.Get(mapId).GetZoneNotBoss();
        }
        public Boss SetUpBoss(Boss boss, int type, short x = 0, short y = 0)
        {
            boss = new Boss();
            if (x != 0 || y != 0)
            {
                boss.CreateBoss(type, x, y);
            }
            else
            {
                boss.CreateBoss(type);
            }
            boss.CharacterHandler.SetUpInfo();
            return boss;
        }
        public Boss SetUpBossNoAttack(Boss boss, int type, short x = 0, short y = 0)
        {
            boss = new Boss();
            if (x != 0 || y != 0)
            {
                boss.CreateBoss(type, x, y);
            }
            else
            {
                boss.CreateBossNoAttack(type);
            }
            boss.CharacterHandler.SetUpInfo();
            return boss;
        }
        public readonly long _15MINUTES = 90000;
        public readonly long _5MINUTES = 300000;

        public readonly long _15SECONDS = 15000;
        public readonly long _45SECONDS = 45000;

        public List<Boss> ListBossTDST = new List<Boss>() { null, null, null, null, null };
        public List<int> IdBossesTDST = new List<int>() { 16, 17, 93, 18, 19 };
        public bool SpawnTDST = false;
        public long DelayTDST = 12000 + ServerUtils.CurrentTimeMillis();
        public int oldMapTDST = -1;
        public int oldZoneTDST = -1;
        public string TileMapTDST = "";

        public List<Boss> ListBossSatThu3 = new List<Boss> { null, null, null };
        public List<int> IDBossesSatThu3 = new List<int> { 28, 27, 29 };
        public bool SpawnSatThu3 = false;
        public long DelaySatThu3 = 15000 + ServerUtils.CurrentTimeMillis();
        public int oldMapSatThu3 = -1;
        public int oldZoneSatThu3 = -1;
        public string TileMapSatThu3 = "";

        public List<Boss> ListBossSatThu2 = new List<Boss> { null, null, null };
        public List<int> IDBossesSatThu2 = new List<int> { 34, 33, 32 };
        public bool SpawnSatThu2 = false;
        public long DelaySatThu2 = 14000 + ServerUtils.CurrentTimeMillis();
        public int oldMapSatThu2 = -1;
        public int oldZoneSatThu2 = -1;
        public string TileMapSatThu2 = "";

        public List<Boss> ListBossSatThu1 = new List<Boss> { null, null };
        public List<int> IDBossesSatThu1 = new List<int> { 30, 31 };
        public bool SpawnSatThu1 = false;
        public long DelaySatThu1 = 13000 + ServerUtils.CurrentTimeMillis();
        public int oldMapSatThu1 = -1;
        public int oldZoneSatThu1 = -1;
        public string TileMapSatThu1 = "";

        public List<Boss> ListBossXenBoHung = new List<Boss> { null, null, null };
        public List<int> IDBossesXenBoHung = new List<int> { 7, 8, 9 };
        public bool SpawnXenBoHung = false;
        public long DelayXenBoHung = 14000 + ServerUtils.CurrentTimeMillis();
        public int oldMapXenBoHung = -1;
        public int oldZoneXenBoHung = -1;
        public string TileMapXenBoHung = "";

        public Boss Basil = new Boss();
        public Boss Lavender = new Boss();
        public Boss Bergamo = new Boss();
        public bool SpawnBasil = false;
        public long DelayBasil = 15000 + ServerUtils.CurrentTimeMillis();
        public int oldMapBasil = -1;
        public int oldZoneBasil = -1;
        public string TileMapBasil = "";

        public bool SpawnLavender = false;
        public long DelayLavender = 16000 + ServerUtils.CurrentTimeMillis();
        public int oldMapLavender = -1;
        public int oldZoneLavender = -1;
        public string TileMapLavender = "";

        public bool SpawnBergamo = false;
        public long DelayBergamo = 17000 + ServerUtils.CurrentTimeMillis();
        public int oldMapBergamo = -1;
        public int oldZoneBergamo = -1;
        public string TileMapBergamo = "";

        public long DelayColer1 = 18000 + ServerUtils.CurrentTimeMillis();
        public long DelayColer2 = 19000 + ServerUtils.CurrentTimeMillis();
        public Boss Coler1 = null;
        public Boss Coler2 = null;
        public int oldZoneColer1 = -1;
        public int oldZoneColer2 = -1;
        public string TileMapColer1 = "";
        public string TileMapColer2 = "";

        public Boss Cumber = null;
        public Boss SuperCumber = null;
        public bool CumberSpawn = false;
        public long DelayCumber = 20000 + ServerUtils.CurrentTimeMillis();
        public int oldZoneCumber = -1;
        public int oldMapCumber = -1;
        public string TileMapCumber = "";

        public Boss Beerus = null;
        public bool BeerusSpawn = false;
        public long DelayBeerus = 20000 + ServerUtils.CurrentTimeMillis();
        public int oldZoneBeerus = -1;
        public int oldMapBeerus = -1;
        public string TileMapBeerus = "";

        public Boss MabuCom = null;
        public bool MabuComSpawn = false;
        public long DelayMabuCom = 20000 + ServerUtils.CurrentTimeMillis();
        public int oldZoneMabuCom = -1;
        public int oldMapMabuCom = -1;
        public string TileMapMabuCom = "";

        public Boss Zamasu = null;
        public bool ZamasuSpawn = false;
        public long DelayZamasu = 20000 + ServerUtils.CurrentTimeMillis();
        public int oldZoneZamasu = -1;
        public int oldMapZamasu = -1;
        public string TileMapZamasu = "";

        public Boss XeTrieuDinh = null;
        public bool XeTrieuDinhSpawn = false;
        public long DelayXeTrieuDinh = 20000 + ServerUtils.CurrentTimeMillis();
        public int oldZoneXeTrieuDinh = -1;
        public int oldMapXeTrieuDinh = -1;
        public string TileMapXeTrieuDinh = "";

        public Boss ThoDaiCa = null;
        public bool ThoDaiCaSpawn = false;
        public long DelayThoDaiCa = 20000 + ServerUtils.CurrentTimeMillis();
        public int oldZoneThoDaiCa = -1;
        public int oldMapThoDaiCa = -1;
        public string TileMapThoDaiCa = "";

        public int CountXinbato = 0;
        public long DelayXinbato = 10000 + ServerUtils.CurrentTimeMillis();

        public int CountSoiHecQuyn = 0;
        public long DelaySoiHecQuyn = 10000 + ServerUtils.CurrentTimeMillis();
        public void Start()
        {
            new Thread(new ThreadStart(AutoBossAsync)).Start();

        }
        public readonly List<int> IdMapOngGiaNoel = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
        public void AutoBossAsync()
        {
            while (Server.Gi().IsRunning)
            {
                try
                {
                    var timeserver = ServerUtils.CurrentTimeMillis();
                    //if (DelayOngGiaNoel < timeserver)
                    //{
                    //    DelayOngGiaNoel = 180000 + timeserver;
                    //    for(int i = 0; i < 6; i++)
                    //    {
                    //        var boss = new Boss();
                    //        boss.CreateBossNoAttack(40);
                    //        var zone = PickMapSpawn(IdMapOngGiaNoel[ServerUtils.RandomNumber(IdMapOngGiaNoel.Count)], 0);
                    //        zone.ZoneHandler.AddBoss(boss);
                    //    }
                    //}
                    if (DelayXinbato < timeserver)
                    {
                        DelayXinbato = (DataCache._1MINUTES * 10) + timeserver;
                        if (CountXinbato < 5)
                        {
                            var boss = new Boss();
                            boss.CreateBossNoAttack(115);
                            boss.CharacterHandler.SetUpInfo();
                            boss.CharacterHandler = new XinbatoHandler(boss);
                            var zone = PickMapSpawn(IdMapOngGiaNoel[ServerUtils.RandomNumber(IdMapOngGiaNoel.Count)], 0);
                            zone.ZoneHandler.AddBoss(boss);
                            Output($"Xinbato Spawn: [Mapid: {zone.Map.Id}, Zone: {zone.Id}");
                        }
                    }
                    if (DelaySoiHecQuyn < timeserver)
                    {
                        DelaySoiHecQuyn = (DataCache._1MINUTES * 10) + timeserver;
                        if (DelaySoiHecQuyn < 5)
                        {
                            var boss = new Boss();
                            boss.CreateBossNoAttack(116);
                            boss.CharacterHandler.SetUpInfo();
                            boss.CharacterHandler = new SoiHecQuynHandler(boss);
                            var zone = PickMapSpawn(IdMapOngGiaNoel[ServerUtils.RandomNumber(IdMapOngGiaNoel.Count)], 0);
                            zone.ZoneHandler.AddBoss(boss);
                            Output($"Soi Hec Quyn Spawn: [Mapid: {zone.Map.Id}, Zone: {zone.Id}");
                        }
                    }
                    //if (DelayThoDaiCa < timeserver)
                    //{
                    //    DelayThoDaiCa = _15MINUTES + timeserver;
                    //    if (!ThoDaiCaSpawn)
                    //    {
                    //        ThoDaiCaSpawn = true;
                    //        ThoDaiCa = SetUpBoss(ThoDaiCa, 13);
                    //        var zone = PickMapSpawn(mapTraiDat[ServerUtils.RandomNumber(mapTraiDat.Count)]);
                    //        zone.ZoneHandler.AddBoss(ThoDaiCa);
                    //        oldMapThoDaiCa = zone.Map.Id;
                    //        oldZoneThoDaiCa = zone.Id;
                    //        TileMapThoDaiCa = zone.Map.TileMap.Name;
                    //        SendBossServerChat("Thỏ Đại Ca vừa xuất hiện tại " + zone.Map.TileMap.Name);
                    //        Output($"ThoDaiCa Spawn: [Mapid: {oldMapThoDaiCa}, Zone: {oldZoneThoDaiCa}]");

                    //    }
                    //    else
                    //    {
                    //        SendBossServerChat("Thỏ Đại Ca vừa xuất hiện tại " + TileMapThoDaiCa);
                    //        Output($"ThoDaiCa ReSpawn: [Mapid: {oldMapThoDaiCa}, Zone: {oldZoneThoDaiCa}]");
                    //    }
                    //}
                    if (DelayMabuCom < timeserver)
                    {
                        DelayMabuCom = _15MINUTES + timeserver;
                        if (!MabuComSpawn)
                        {
                            MabuComSpawn = true;
                            MabuCom = SetUpBoss(MabuCom, 109);
                            var zone = PickMapSpawn(ServerUtils.RandomNumber(167, 168));
                            zone.ZoneHandler.AddBoss(MabuCom);
                            oldMapMabuCom = zone.Map.Id;
                            oldZoneMabuCom = zone.Id;
                            TileMapMabuCom = zone.Map.TileMap.Name;
                            SendBossServerChat("Mabư Còm vừa xuất hiện tại " + zone.Map.TileMap.Name);
                            Output($"MabuCom Spawn: [Mapid: {oldMapMabuCom}, Zone: {oldZoneMabuCom}]");

                        }
                        else
                        {
                            SendBossServerChat("Mabư Còm vừa xuất hiện tại " + TileMapMabuCom);
                            Output($"MabuCom ReSpawn: [Mapid: {oldMapMabuCom}, Zone: {oldZoneMabuCom}]");
                        }
                    }
                    if (DelayZamasu < timeserver)
                    {
                        DelayZamasu = _15MINUTES + timeserver;
                        if (!ZamasuSpawn)
                        {
                            ZamasuSpawn = true;
                            Zamasu = SetUpBoss(Zamasu, 110);
                            var zone = PickMapSpawn(BaMapDauTuongLai[ServerUtils.RandomNumber(BaMapDauTuongLai.Count)]);
                            zone.ZoneHandler.AddBoss(Zamasu);
                            oldMapZamasu = zone.Map.Id;
                            oldZoneZamasu = zone.Id;
                            TileMapZamasu = zone.Map.TileMap.Name;
                            SendBossServerChat("Zamasu vừa xuất hiện tại " + zone.Map.TileMap.Name);
                            Output($"Zamasu Spawn: [Mapid: {oldMapZamasu}, Zone: {oldZoneZamasu}]");

                        }
                        else
                        {
                            SendBossServerChat("Zamasu vừa xuất hiện tại " + TileMapZamasu);
                            Output($"Zamasu ReSpawn: [Mapid: {oldMapZamasu}, Zone: {oldZoneZamasu}]");
                        }
                    }
                    if (DelayXeTrieuDinh < timeserver)
                    {
                        DelayXeTrieuDinh = _15MINUTES + timeserver;
                        if (!XeTrieuDinhSpawn)
                        {
                            XeTrieuDinhSpawn = true;
                            XeTrieuDinh = SetUpBoss(XeTrieuDinh, 111);
                            var zone = PickMapSpawn(ServerUtils.RandomNumber(42, 44));
                            zone.ZoneHandler.AddBoss(XeTrieuDinh);
                            oldMapXeTrieuDinh = zone.Map.Id;
                            oldZoneXeTrieuDinh = zone.Id;
                            TileMapXeTrieuDinh = zone.Map.TileMap.Name;
                            SendBossServerChat("Xe Triều Đình vừa xuất hiện tại " + zone.Map.TileMap.Name);
                            Output($"XeTrieuDinh Spawn: [Mapid: {oldMapXeTrieuDinh}, Zone: {oldZoneXeTrieuDinh}]");

                        }
                        else
                        {
                            SendBossServerChat("Xe Triều Đình vừa xuất hiện tại " + TileMapXeTrieuDinh);
                            Output($"XeTrieuDinh ReSpawn: [Mapid: {oldMapXeTrieuDinh}, Zone: {oldZoneXeTrieuDinh}]");
                        }
                    }
                    if (DelayBeerus < timeserver)
                    {
                        DelayBeerus = _15MINUTES + timeserver;
                        if (!BeerusSpawn)
                        {
                            BeerusSpawn = true;
                            Beerus = new Boss();
                            Beerus.CreateBoss(108);
                            Beerus.CharacterHandler.SetUpInfo();
                            var zone = PickMapSpawn(ServerUtils.RandomNumber(42, 44));
                            zone.ZoneHandler.AddBoss(Beerus);
                            oldMapBeerus = zone.Map.Id;
                            oldZoneBeerus = zone.Id;
                            TileMapBeerus = zone.Map.TileMap.Name;
                            SendBossServerChat("Beerus vừa xuất hiện tại " + zone.Map.TileMap.Name);
                            Output($"Beerus Spawn: [Mapid: {zone.Map.Id}, Zone: {zone.Id}]");

                        }
                        else
                        {
                            SendBossServerChat("Beerus vừa xuất hiện tại " + TileMapBeerus);
                            Output($"Beerus ReSpawn: [Mapid: {oldMapBeerus}, Zone: {oldZoneBeerus}]");
                        }
                    }
                    if (DelayBasil < timeserver)
                    {
                        DelayBasil = _5MINUTES + timeserver;
                        if (!SpawnBasil)
                        {
                            SpawnBasil = true;
                            Basil = SetUpBoss(Basil, 100);
                            var zone = PickMapSpawn(BaMapDauTuongLai[ServerUtils.RandomNumber(BaMapDauTuongLai.Count)]);
                            zone.ZoneHandler.AddBoss(Basil);
                            oldMapBasil = zone.Map.Id;
                            oldZoneBasil = zone.Id;
                            TileMapBasil = zone.Map.TileMap.Name;
                            SendBossServerChat("Basil vừa xuất hiện tại " + zone.Map.TileMap.Name);
                            Output($"Basil Wolf Spawn: [Mapid: {oldMapBasil}, Zone: {oldZoneBasil}]");

                        }
                        else
                        {
                            SendBossServerChat("Basil vừa xuất hiện tại " + TileMapBasil);
                            Output($"Basil Wolf ReSpawn: [Mapid: {oldMapBasil}, Zone: {oldZoneBasil}]");
                        }
                    }
                    if (DelayLavender < timeserver)
                    {
                        DelayLavender = _5MINUTES + timeserver;
                        if (!SpawnLavender)
                        {
                            SpawnLavender = true;
                            Lavender = SetUpBoss(Lavender, 101);
                            var zone = PickMapSpawn(BaMapDauTuongLai[ServerUtils.RandomNumber(BaMapDauTuongLai.Count)]);
                            zone.ZoneHandler.AddBoss(Lavender);
                            oldMapLavender = zone.Map.Id;
                            oldZoneLavender = zone.Id;
                            TileMapLavender = zone.Map.TileMap.Name;
                            SendBossServerChat("Lavender vừa xuất hiện tại " + zone.Map.TileMap.Name);
                            Output($"Lavender Wolf Spawn: [Mapid: {oldMapLavender}, Zone: {oldZoneLavender}]");
                        }
                        else
                        {
                            SendBossServerChat("Lavender vừa xuất hiện tại " + TileMapLavender);
                            Output($"Lavender Wolf ReSpawn: [Mapid: {oldMapLavender}, Zone: {oldZoneLavender}]");
                        }
                    }
                    if (DelayBergamo < timeserver)
                    {
                        DelayBergamo = _5MINUTES + timeserver;
                        if (!SpawnBergamo)
                        {
                            SpawnBergamo = true;
                            Bergamo = SetUpBoss(Bergamo, 102);
                            var zone = PickMapSpawn(BaMapDauTuongLai[ServerUtils.RandomNumber(BaMapDauTuongLai.Count)]);
                            zone.ZoneHandler.AddBoss(Bergamo);
                            oldMapBergamo = zone.Map.Id;
                            oldZoneBergamo = zone.Id;
                            TileMapBergamo = zone.Map.TileMap.Name;
                            SendBossServerChat("Bergamo vừa xuất hiện tại " + TileMapBergamo);
                            Output($"Bergamo Wolf Spawn: [Mapid: {oldMapBergamo}, Zone: {oldZoneBergamo}]");
                        }
                        else
                        {
                            SendBossServerChat("Bergamo vừa xuất hiện tại " + TileMapBergamo);
                            Output($"Bergamo Wolf ReSpawn: [Mapid: {oldMapBergamo}, Zone: {oldZoneBergamo}]");
                        }
                    }
                    if (DelayCumber < timeserver)
                    {
                        DelayCumber = _5MINUTES + timeserver;
                        if (!CumberSpawn)
                        {
                            CumberSpawn = true;
                            Cumber = SetUpBoss(Cumber, 105);
                            var zone = PickMapSpawn(155);
                            zone.ZoneHandler.AddBoss(Cumber);
                            oldMapCumber = zone.Map.Id;
                            oldZoneCumber = zone.Id;
                            TileMapCumber = zone.Map.TileMap.Name;
                            Output($"Cumber Spawn: [Mapid: {oldMapCumber}, Zone: {oldZoneCumber}");
                            SendBossServerChat($"Cumber vừa xuất hiện tại {TileMapCumber}");
                        }
                        else
                        {
                            Output($"Cumber ReSpawn: [Mapid: {oldMapCumber}, Zone: {oldZoneCumber}");
                            SendBossServerChat($"Cumber vừa xuất hiện tại {TileMapCumber}");
                        }
                    }
                    if (DelayColer1 < timeserver)
                    {
                        DelayColer1 = _5MINUTES + timeserver;
                        if (!Coler1Spawn)
                        {
                            Coler1Spawn = true;
                            var zone = PickMapSpawn(CoolerMaps[ServerUtils.RandomNumber(CoolerMaps.Count)]);
                            Coler1 = SetUpBoss(Coler1, 10);
                            zone.ZoneHandler.AddBoss(Coler1);
                            oldMapColer1 = zone.Map.Id;
                            oldZoneColer1 = zone.Id;
                            TileMapColer1 = zone.Map.TileMap.Name;
                            Output($"Coler 1 Spawn: [Mapid: {oldMapColer1}, Zone: {oldZoneColer1}");
                            SendBossServerChat($"Coler 1 vừa xuất hiện tại {TileMapColer1}");
                        }
                        else
                        {
                            Output($"Coler 1 ReSpawn: [Mapid: {oldMapColer1}, Zone: {oldZoneColer1}");
                            SendBossServerChat($"Coler 1 vừa xuất hiện tại {TileMapColer1}");
                        }
                    }
                    if (DelayColer2 < timeserver)
                    {
                        DelayColer2 = _5MINUTES + timeserver;
                        if (!Coler2Spawn)
                        {
                            Coler2Spawn = true;
                            var zone = PickMapSpawn(CoolerMaps[ServerUtils.RandomNumber(CoolerMaps.Count)]);
                            Coler2 = SetUpBoss(Coler1, 10);
                            zone.ZoneHandler.AddBoss(Coler2);
                            oldMapColer2 = zone.Map.Id;
                            oldZoneColer2 = zone.Id;
                            TileMapColer2 = zone.Map.TileMap.Name;
                            Output($"Coler 2 Spawn: [Mapid: {oldMapColer2}, Zone: {oldZoneColer2}");
                            SendBossServerChat($"Coler 2 vừa xuất hiện tại {TileMapColer2}");
                        }
                        else
                        {
                            Output($"Coler 2 ReSpawn: [Mapid: {oldMapColer2}, Zone: {oldZoneColer2}");
                            SendBossServerChat($"Coler 2 vừa xuất hiện tại {TileMapColer2}");
                        }
                    }
                    if (DelayChilled < timeserver)
                    {
                        DelayChilled = _5MINUTES + timeserver;
                        if (!ChilledSpawn)
                        {
                            ChilledSpawn = true;
                            var zone = PickMapSpawn(ChilledMaps[ServerUtils.RandomNumber(ChilledMaps.Count)]);
                            Chilled = SetUpBoss(Chilled, 14);
                            zone.ZoneHandler.AddBoss(Chilled);
                            TileMapChilled = zone.Map.TileMap.Name;
                            oldMapChilled = zone.Map.
                                Id;
                            oldZoneChilled = zone.Id;
                            Output($"Chilled 1 Spawn: [Mapid: {oldMapChilled}, Zone: {oldZoneChilled}");
                            SendBossServerChat($"Chilled 1 vừa xuất hiện tại {TileMapChilled}");
                        }
                        else
                        {
                            Output($"Chilled 1 ReSpawn: [Mapid: {oldMapChilled}, Zone: {oldZoneChilled}");
                            SendBossServerChat($"Chilled 1 vừa xuất hiện tại {TileMapChilled}");
                        }
                    }
                    if (DelayChilled2 < timeserver)
                    {
                        DelayChilled2 = _5MINUTES + timeserver;
                        if (!Chilled2Spawn)
                        {
                            Chilled2Spawn = true;
                            var zone = PickMapSpawn(ChilledMaps[ServerUtils.RandomNumber(ChilledMaps.Count)]);
                            Chilled2 = SetUpBoss(Chilled, 15);
                            zone.ZoneHandler.AddBoss(Chilled2);
                            TileMapChilled2 = zone.Map.TileMap.Name;
                            oldMapChilled2 = zone.Map.
                                Id;
                            oldZoneChilled2 = zone.Id;
                            Output($"Chilled 2 Spawn: [Mapid: {oldMapChilled2}, Zone: {oldZoneChilled2}");
                            SendBossServerChat($"Chilled 2 vừa xuất hiện tại {TileMapChilled2}");
                        }
                        else
                        {
                            Output($"Chilled 2 ReSpawn: [Mapid: {oldMapChilled2}, Zone: {oldZoneChilled2}");
                            SendBossServerChat($"Chilled 2 vừa xuất hiện tại {TileMapChilled2}");
                        }
                    }
                    if (DelayXenHoanThien2 < timeserver)
                    {
                        DelayXenHoanThien2 = _5MINUTES + timeserver;
                        if (!XenHoanThienSpawn2)
                        {
                            var zone = PickMapSpawn(103);
                            XenHoanThienSpawn2 = true;
                            XenHoanThien2 = SetUpBoss(XenHoanThien2, 63);
                            zone.ZoneHandler.AddBoss(XenHoanThien2);
                            TileMapXenHoanThien2 = zone.Map.TileMap.Name;
                            oldZoneXenHoanThien2 = zone.Id;
                            oldMapXenHoanThien2 = zone.Map.Id;
                            Output($"Super Cell Spawn: [Mapid: {TileMapXenHoanThien2}, Zone: {oldZoneXenHoanThien2}");
                            SendBossServerChat($"Siêu Bọ Hung vừa xuất hiện tại {TileMapXenHoanThien2}");
                        }
                        else
                        {
                            Output($"Super Cell ReSpawn: [Mapid: {TileMapXenHoanThien2}, Zone: {oldZoneXenHoanThien2}");
                            SendBossServerChat($"Siêu Bọ Hung vừa xuất hiện tại {TileMapXenHoanThien2}");
                        }
                    }
                    //if (delayPicHe < timeserver)
                    //{
                    //    delayPicHe = _5MINUTES + timeserver;
                    //    if (!SpawnPiche)
                    //    {
                    //        var zone = PickMapSpawn(mapTraiDat[ServerUtils.RandomNumber(mapTraiDat.Count)]);
                    //        SpawnPiche = true;
                    //        Piche = SetUpBoss(Piche, 99);
                    //        zone.ZoneHandler.AddBoss(Piche);
                    //        tileMapPicHe = zone.Map.TileMap.Name;
                    //        ZonePicHe = Piche.Zone.Id;
                    //        MapPicHe = zone.Map.Id;
                    //        Output($"Summer Pic Spawn: [Mapid: {MapPicHe}, Zone: {ZonePicHe}]");
                    //        ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Pic hè vừa xuất hiện tại " + tileMapPicHe));
                    //    }
                    //    else
                    //    {
                    //        Output($"Summer Pic ReSpawn: [Mapid: {MapPicHe}, Zone: {ZonePicHe}]");
                    //        ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Pic hè vừa xuất hiện tại " + tileMapPicHe));

                    //    }
                    //}
                    //if (delayKingKongHe < timeserver)
                    //{
                    //    delayKingKongHe = _5MINUTES + timeserver;
                    //    if (!SpawnKKHe)
                    //    {
                    //        SpawnKKHe = true;
                    //        var zone = PickMapSpawn(mapTraiDat[ServerUtils.RandomNumber(mapTraiDat.Count)]);
                    //        KingKongHe = SetUpBoss(KingKongHe, 99);
                    //        zone.ZoneHandler.AddBoss(KingKongHe);
                    //        ZoneKKhe = KingKongHe.Zone.Id;
                    //        tileMapKingKongHe = zone.Map.TileMap.Name;
                    //        MapKKHe = zone.Map.Id;
                    //        Output($"Summer Kingkong Spawn: [Mapid: {MapKKHe}, Zone: {ZoneKKhe}]");
                    //        ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Kingkong Hè vừa xuất hiện tại " + tileMapKingKongHe));
                    //    }
                    //    else
                    //    {
                    //        Output($"Summer Kingkong ReSpawn: [Mapid: {MapKKHe}, Zone: {ZoneKKhe}]");
                    //        ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Kingkong Hè vừa xuất hiện tại " + tileMapKingKongHe));
                    //    }
                    //}
                    if (DelayXenBoHung < timeserver)
                    {
                        DelayXenBoHung = _5MINUTES + timeserver;
                        if (!SpawnXenBoHung)
                        {
                            SpawnXenBoHung = true;
                            var zone = PickMapSpawn(CellMaps[ServerUtils.RandomNumber(CellMaps.Count)]);
                            for (int i = 0; i < ListBossXenBoHung.Count; i++)
                            {
                                ListBossXenBoHung[i] = SetUpBoss(ListBossXenBoHung[i], IDBossesXenBoHung[i]);
                            }
                            zone.ZoneHandler.AddBoss(ListBossXenBoHung[0]);
                            ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Xên Bọ Hung 1 vừa xuất hiện tại " + zone.Map.TileMap.Name));
                            oldMapXenBoHung = zone.Map.Id;
                            oldZoneXenBoHung = zone.Id;
                            TileMapXenBoHung = zone.Map.TileMap.Name;
                            Output($"Cell 1 Spawn: [Mapid: {oldMapXenBoHung}, Zone: {oldZoneXenBoHung}]");

                        }
                        else
                        {
                            Output($"Cell 1 ReSpawn: [Mapid: {oldMapXenBoHung}, Zone: {oldZoneXenBoHung}]");
                            ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Xên Bọ Hung 1 vừa xuất hiện tại " + TileMapXenBoHung));

                        }
                    }
                    if (DelaySatThu3 < timeserver)
                    {
                        DelaySatThu3 = _5MINUTES + timeserver;
                        if (!SpawnSatThu3)
                        {
                            SpawnSatThu3 = true;
                            var zone = PickMapSpawn(PicPocKKMaps[ServerUtils.RandomNumber(PicPocKKMaps.Count)]);
                            for (int i = 0; i < ListBossSatThu3.Count; i++)
                            {
                                if (i == 0) ListBossSatThu3[i] = SetUpBoss(ListBossSatThu3[i], IDBossesSatThu3[i]);
                                else ListBossSatThu3[i] = SetUpBossNoAttack(ListBossSatThu3[i], IDBossesSatThu3[i]);
                                zone.ZoneHandler.AddBoss(ListBossSatThu3[i]);
                            }
                            ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Poc vừa xuất hiện tại " + zone.Map.TileMap.Name));
                            ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Pic vừa xuất hiện tại " + zone.Map.TileMap.Name));
                            ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Kingkong vừa xuất hiện tại " + zone.Map.TileMap.Name));
                            oldMapSatThu3 = zone.Map.Id;
                            oldZoneSatThu3 = zone.Id;
                            TileMapSatThu3 = zone.Map.TileMap.Name;
                            Output($"Poc Spawn: [Mapid: {oldMapSatThu3}, Zone: {oldZoneSatThu3}]");
                            Output($"Pic Spawn: [Mapid: {oldMapSatThu3}, Zone: {oldZoneSatThu3}]");
                            Output($"Kingkong Spawn: [Mapid: {oldMapSatThu3}, Zone: {oldZoneSatThu3}]");
                        }
                        else
                        {
                            ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Poc vừa xuất hiện tại " + TileMapSatThu3));
                            ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Pic vừa xuất hiện tại " + TileMapSatThu3));
                            ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Kingkong vừa xuất hiện tại " + TileMapSatThu3));
                            Output($"Poc ReSpawn: [Mapid: {oldMapSatThu3}, Zone: {oldZoneSatThu3}]");
                            Output($"Pic ReSpawn: [Mapid: {oldMapSatThu3}, Zone: {oldZoneSatThu3}]");
                            Output($"Kingkong ReSpawn: [Mapid: {oldMapSatThu3}, Zone: {oldZoneSatThu3}]");
                        }
                    }
                    if (DelaySatThu2 < timeserver)
                    {
                        DelaySatThu2 = _5MINUTES + timeserver;
                        if (!SpawnSatThu2)
                        {
                            SpawnSatThu2 = true;
                            var zone = PickMapSpawn(SanSauSieuThi[0]);
                            for (int i = 0; i < ListBossSatThu2.Count; i++)
                            {
                                if (i == 0) ListBossSatThu2[i] = SetUpBoss(ListBossSatThu2[i], IDBossesSatThu2[i]);
                                else ListBossSatThu2[i] = SetUpBossNoAttack(ListBossSatThu2[i], IDBossesSatThu2[i]);
                                zone.ZoneHandler.AddBoss(ListBossSatThu2[i]);
                            }
                            ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Android 15 vừa xuất hiện tại " + zone.Map.TileMap.Name));
                            ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Android 14 vừa xuất hiện tại " + zone.Map.TileMap.Name));
                            ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Android 13 vừa xuất hiện tại " + zone.Map.TileMap.Name));
                            oldMapSatThu2 = zone.Map.Id;
                            oldZoneSatThu2 = zone.Id;
                            TileMapSatThu2 = zone.Map.TileMap.Name;
                            Output($"Android 15 Spawn: [Mapid: {oldMapSatThu2}, Zone: {oldZoneSatThu2}]");
                            Output($"Android 14 Spawn: [Mapid: {oldMapSatThu2}, Zone: {oldZoneSatThu2}]");
                            Output($"Android 13 Spawn: [Mapid: {oldMapSatThu2}, Zone: {oldZoneSatThu2}]");
                        }
                        else
                        {
                            ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Android 15 vừa xuất hiện tại " + TileMapSatThu2));
                            ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Android 14 vừa xuất hiện tại " + TileMapSatThu2));
                            ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Android 13 vừa xuất hiện tại " + TileMapSatThu2));
                            Output($"Android 15 ReSpawn: [Mapid: {oldMapSatThu2}, Zone: {oldZoneSatThu2}]");
                            Output($"Android 14 ReSpawn: [Mapid: {oldMapSatThu2}, Zone: {oldZoneSatThu2}]");
                            Output($"Android 13 ReSpawn: [Mapid: {oldMapSatThu2}, Zone: {oldZoneSatThu2}]");
                        }
                    }
                    if (DelaySatThu1 < timeserver)
                    {
                        DelaySatThu1 = _5MINUTES + timeserver;
                        if (!SpawnSatThu1)
                        {
                            SpawnSatThu1 = true;
                            var zone = PickMapSpawn(BaMapDauTuongLai[ServerUtils.RandomNumber(BaMapDauTuongLai.Count)]);
                            for (int i = 0; i < ListBossSatThu1.Count; i++)
                            {
                                if (i == 0) ListBossSatThu1[i] = SetUpBoss(ListBossSatThu1[i], IDBossesSatThu1[i]);
                                else ListBossSatThu1[i] = SetUpBossNoAttack(ListBossSatThu1[i], IDBossesSatThu1[i]);
                                zone.ZoneHandler.AddBoss(ListBossSatThu1[i]);
                            }
                            ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Android 19 vừa xuất hiện tại " + zone.Map.TileMap.Name));
                            ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Android 20 vừa xuất hiện tại " + zone.Map.TileMap.Name));
                            oldMapSatThu1 = zone.Map.Id;
                            oldZoneSatThu1 = zone.Id;
                            TileMapSatThu1 = zone.Map.TileMap.Name;
                            Output($"Android 19 Spawn: [Mapid: {oldMapSatThu1}, Zone: {oldZoneSatThu1}]");
                            Output($"Android 20 Spawn: [Mapid: {oldMapSatThu1}, Zone: {oldZoneSatThu1}]");
                        }
                        else
                        {
                            ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Android 19 vừa xuất hiện tại " + TileMapSatThu1));
                            ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Android 20 vừa xuất hiện tại " + TileMapSatThu1));
                            Output($"Android 19 ReSpawn: [Mapid: {oldMapSatThu1}, Zone: {oldZoneSatThu1}]");
                            Output($"Android 20 ReSpawn: [Mapid: {oldMapSatThu1}, Zone: {oldZoneSatThu1}]");
                        }
                    }
                    if (DelayMapDauDinh < timeserver)
                    {
                        DelayMapDauDinh = _5MINUTES + timeserver;
                        if (!MapDauDinhSpawn)
                        {
                            MapDauDinhSpawn = true;
                            MapDauDinh = SetUpBoss(MapDauDinh, 25);
                            var zone = PickMapSpawn(NappaMaps[ServerUtils.RandomNumber(NappaMaps.Count)]);
                            zone.ZoneHandler.AddBoss(MapDauDinh);
                            ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Mập Đầu Đinh vừa xuất hiện tại " + zone.Map.TileMap.Name));
                            oldMapMapDauDinh = zone.Map.Id;
                            oldZoneMapDauDinh = zone.Id;
                            TileMapMapDauDinh = zone.Map.TileMap.Name;
                            Output($"Map Dau Dinh Spawn: [Mapid: {oldMapMapDauDinh}, Zone: {oldZoneMapDauDinh}]");
                        }
                        else
                        {
                            ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Mập Đầu Đinh vừa xuất hiện tại " + TileMapMapDauDinh));
                            Output($"Map Dau Dinh ReSpawn: [Mapid: {oldMapMapDauDinh}, Zone: {oldZoneMapDauDinh}]");

                        }
                    }
                    if (DelayRambo < timeserver)
                    {
                        DelayRambo = _5MINUTES + timeserver;
                        if (!RamboSpawn)
                        {
                            RamboSpawn = true;
                            Rambo = SetUpBoss(Rambo, 26);
                            var zone = PickMapSpawn(NappaMaps[ServerUtils.RandomNumber(NappaMaps.Count)]);
                            zone.ZoneHandler.AddBoss(Rambo);
                            ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Rambo vừa xuất hiện tại " + zone.Map.TileMap.Name));
                            oldMapRambo = zone.Map.Id;
                            oldZoneRambo = zone.Id;
                            TileMapRambo = zone.Map.TileMap.Name;
                            Output($"Rambo Spawn: [Mapid: {oldMapRambo}, Zone: {oldZoneRambo}]");
                        }
                        else
                        {
                            ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Rambo vừa xuất hiện tại " + TileMapRambo));
                            Output($"Rambo ReSpawn: [Mapid: {oldMapRambo}, Zone: {oldZoneRambo}]");

                        }
                    }
                    if (DelayKuku < timeserver)
                    {
                        DelayKuku = _5MINUTES + timeserver;
                        if (!KukuSpawn)
                        {
                            KukuSpawn = true;
                            Kuku = SetUpBoss(Kuku, 24);
                            var zone = PickMapSpawn(NappaMaps[ServerUtils.RandomNumber(NappaMaps.Count)]);
                            zone.ZoneHandler.AddBoss(Kuku);
                            ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Kuku vừa xuất hiện tại " + zone.Map.TileMap.Name + ", khu " + zone.Id));
                            oldMapKuku = zone.Map.Id;
                            oldZoneKuku = zone.Id;
                            TileMapKuku = zone.Map.TileMap.Name;
                            Output($"Kuku Spawn: [Mapid: {zone.Map.Id}, Zone: {zone.Id}]");
                        }
                        else
                        {
                            ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Kuku vừa xuất hiện tại " + TileMapKuku + ", khu " + oldZoneKuku));
                            Output($"Kuku ReSpawn: [Mapid: {oldMapKuku}, Zone: {oldZoneKuku}]");

                        }
                    }
                    if (DelayFide1 < timeserver)
                    {
                        DelayFide1 = _5MINUTES + timeserver;
                        if (!Fide1Spawn)
                        {
                            Fide1Spawn = true;
                            Fide1 = SetUpBoss(Fide1, 4);
                            var zone = PickMapSpawn(FideMaps[ServerUtils.RandomNumber(FideMaps.Count)]);
                            zone.ZoneHandler.AddBoss(Fide1);
                            ClientManager.Gi().SendMessageCharacter(Service.ServerChat("BOSS Fide Đại ca 1 vừa xuất hiện tại " + zone.Map.TileMap.Name + ", khu " + zone.Id));
                            oldMapFide1 = zone.Map.Id;
                            oldZoneFide1 = zone.Id;
                            TileMapFide1 = zone.Map.TileMap.Name;
                            Output($"Fide Spawn: [Mapid: {oldMapFide1}, Zone: {oldZoneFide1}]");

                        }
                        else
                        {
                            ClientManager.Gi().SendMessageCharacter(Service.ServerChat("BOSS Fide Đại ca 1 vừa xuất hiện tại " + TileMapFide1));
                            Output($"Fide ReSpawn: [Mapid: {oldMapFide1}, Zone: {oldZoneFide1}]");
                        }
                    }
                    if (DelayBlackGoku < timeserver)
                    {
                        DelayBlackGoku = _5MINUTES + timeserver;
                        if (!BlackGokuSpawn)
                        {
                            BlackGokuSpawn = true;
                            BlackGoku = SetUpBoss(BlackGoku, 2);
                            var zone = PickMapSpawn(BaMapDauTuongLai[ServerUtils.RandomNumber(BaMapDauTuongLai.Count)]);
                            zone.ZoneHandler.AddBoss(BlackGoku);
                            ClientManager.Gi().SendMessageCharacter(Service.ServerChat("BOSS Black Goku vừa xuất hiện tại " + zone.Map.TileMap.Name));
                            oldMapBlackGoku = zone.Map.Id;
                            oldZoneBlackGoku = zone.Id;
                            TileMapBlackGoku = zone.Map.TileMap.Name;
                            Output($"Black Goku Spawn: [Mapid: {oldMapBlackGoku}, Zone: {oldZoneBlackGoku}]");
                        }
                        else
                        {
                            ClientManager.Gi().SendMessageCharacter(Service.ServerChat("BOSS Black Goku vừa xuất hiện tại " + TileMapBlackGoku));
                            Output($"Black Goku ReSpawn: [Mapid: {oldMapBlackGoku}, Zone: {oldZoneBlackGoku}]");
                        }
                    }
                    if (DelayBroly < timeserver)
                    {
                        DelayBroly = _45SECONDS + timeserver;
                        if (ABoss.gI().countBroly < 20)
                        {
                            Broly = SetUpBoss(Broly, 41);
                            var zone = PickMapSpawnNotHasBosses(mapTraiDat[ServerUtils.RandomNumber(mapTraiDat.Count)]);
                            zone.ZoneHandler.AddBoss(Broly);
                            //ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Broly vừa xuất hiện tại " + zone.Map.TileMap.Name));
                            oldMapBroly = zone.Map.Id;
                            oldZoneBroly = zone.Id;
                            TileMapBroly = zone.Map.TileMap.Name;
                            Output($"Broly Spawn: [Mapid: {zone.Map.Id}, Zone: {zone.Id}]");
                            ABoss.gI().countBroly++;
                            //ListBroly.Add(Broly);
                        }

                    }
                    if (DelayTDST < timeserver)
                    {
                        DelayTDST = _5MINUTES + timeserver;
                        if (!SpawnTDST)
                        {
                            var zone = PickMapSpawn(TDSTMaps[ServerUtils.RandomNumber(TDSTMaps.Count)]);
                            SpawnTDST = true;
                            for (int i = 0; i < 5; i++)
                            {
                                if (i == 0) ListBossTDST[i] = SetUpBoss(ListBossTDST[i], IdBossesTDST[i]);
                                else ListBossTDST[i] = SetUpBossNoAttack(ListBossTDST[i], IdBossesTDST[i]);

                                ListBossTDST[i].BasePositionX += (short)(i * 17);
                                zone.ZoneHandler.AddBoss(ListBossTDST[i]);
                            }
                            oldMapTDST = zone.Map.Id;
                            oldZoneTDST = zone.Id;
                            TileMapTDST = zone.Map.TileMap.Name;
                            ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Tiểu đội sát thủ vừa xuất hiện tại " + TileMapTDST + ", khu " + zone.Id));
                            Output($"TDST Spawn: [Mapid: {oldMapTDST}, {oldZoneTDST}]");
                        }
                        else
                        {
                            ClientManager.Gi().SendMessage(Service.ServerChat("BOSS Tiểu đội sát thủ vừa xuất hiện tại " + TileMapTDST));
                            Output($"TDST ReSpawn: [Mapid: {oldMapTDST}, {oldZoneTDST}]");
                        }
                    }
                    Thread.Sleep(1000);
                }

                catch (Exception e)
                {
                    Server.Gi().Logger.Print("Aboss Error: " + e.Message + "\n" + e.StackTrace, "cyan");

                }
                IsStop = true;
            }
            Server.Gi().Logger.Print("Close Auto Boss Success", "red");
        }

        public void BossDied(Boss Bossist)
        {
            try
            {
                var timeserver = ServerUtils.CurrentTimeMillis();
                switch (Bossist.Type)
                {
                    case 108:
                        BeerusSpawn = false;
                        oldMapBeerus = -1;
                        oldZoneBeerus = -1;
                        TileMapBeerus = "";
                        DelayBeerus = 30000 + timeserver;
                        break;
                    case 109:
                        MabuComSpawn = false;
                        break;
                    case 110:
                        ZamasuSpawn = false;
                        break;
                    case 111:
                        XeTrieuDinhSpawn = false;
                        break;
                    case 1:

                        break;
                    case 41:
                        var countBroly = ABoss.gI().countBroly;
                        if (Bossist.HpFull >= 1500000)
                        {
                            ABoss.gI().countBroly--;
                            var SuperBroly = new Boss();
                            SuperBroly.CreateBossSuperBroly();
                            SuperBroly.CharacterHandler.SetUpInfo();
                            Bossist.Zone.ZoneHandler.AddBoss(SuperBroly);
                            ClientManager.Gi().SendMessageCharacter(Service.ServerChat("BOSS Super Broly vừa xuất hiện tại " + Bossist.Zone.Map.TileMap.Name));
                            Server.Gi().Logger.Print("[Spawn Super Broly] Map: " + Bossist.Zone.Map.Id + " | K: " + Bossist.Zone.Id, "yellow");
                        }
                        else if (ServerUtils.RandomNumber(100) < 15)
                        {
                            var SuperBroly = new Boss();
                            SuperBroly.CreateBossSuperBroly();
                            SuperBroly.CharacterHandler.SetUpInfo();
                            Bossist.Zone.ZoneHandler.AddBoss(SuperBroly);
                            ClientManager.Gi().SendMessageCharacter(Service.ServerChat("BOSS Super Broly vừa xuất hiện tại " + Bossist.Zone.Map.TileMap.Name));
                            Server.Gi().Logger.Print("[Spawn Super Broly] Map: " + Bossist.Zone.Map.Id + " | K: " + Bossist.Zone.Id, "yellow");
                            ABoss.gI().countBroly--;
                        }
                        else
                        {
                            ABoss.gI().countBroly--;

                            //var zone = Bossist.Zone;
                            //async void ActionRespawn()
                            //{
                            //    await Task.Delay(15000);

                            //        var broly = new Boss();
                            //        broly.CreateBoss(41);
                            //        broly.InfoChar.Hp = broly.HpFull = broly.HpFull + (broly.HpFull * 10 / 100);
                            //        broly.CharacterHandler.SetUpInfo();
                            //        zone.ZoneHandler.AddBoss(broly);


                            //}
                            //var task = new Task(ActionRespawn);
                            //task.Start();
                        }
                        Server.Gi().Logger.Print($"Count Broly : ({countBroly})old, ({ABoss.gI().countBroly})new");
                        break;
                    case 105:
                        Cumber = null;
                        DelayCumber = _5MINUTES + timeserver;
                        SuperCumber = SetUpBoss(SuperCumber, 106);
                        Bossist.Zone.ZoneHandler.AddBoss(SuperCumber);
                        break;
                    case 106:
                        CumberSpawn = false;
                        DelayCumber = _5MINUTES + timeserver;
                        break;
                    case 10:
                        Coler1 = null;
                        Coler1Spawn = false;
                        break;
                    case 11:
                        Coler2 = null;
                        Coler2Spawn = false;
                        break;
                    case 14:
                        Chilled = null;
                        ChilledSpawn = false;
                        break;
                    case 15:
                        Chilled2 = null;
                        Chilled2Spawn = false;
                        break;
                    case 63:
                        XenHoanThien2 = null;
                        XenHoanThienSpawn2 = false;
                        break;
                    case 100:
                        Basil = null;
                        DelayBasil = 120000 + timeserver;
                        SpawnBasil = false;
                        break;
                    case 101:
                        Lavender = null;
                        DelayLavender = 120000 + timeserver;
                        SpawnLavender = false;
                        break;
                    case 102:
                        Bergamo = null;
                        DelayBergamo = 120000 + timeserver;
                        SpawnBergamo = false;
                        break;
                    case 24:
                        Kuku = null;
                        KukuSpawn = false;
                        DelayKuku = 20000 + timeserver;
                        break;
                    case 26:
                        Rambo = null;

                        RamboSpawn = false;
                        DelayRambo = 20000 + timeserver;
                        break;
                    case 25:
                        MapDauDinh = null;
                        MapDauDinhSpawn = false;
                        DelayMapDauDinh = 20000 + timeserver;
                        break;
                    case 98:
                        Piche = null;
                        SpawnPiche = false;
                        break;
                    case 99:
                        KingKongHe = null;
                        SpawnKKHe = false;
                        break;

                    case 7:
                        {
                            var boss = new Boss();
                            boss = SetUpBoss(boss, IDBossesXenBoHung[1], Bossist.InfoChar.X, Bossist.InfoChar.Y);
                            boss.InfoDelayBoss.DelayRemove = 900000 + timeserver;
                            Bossist.Zone.ZoneHandler.AddBoss(boss);
                        }
                        break;
                    case 8:
                        {
                            var boss = new Boss();
                            boss = SetUpBoss(boss, IDBossesXenBoHung[2], Bossist.InfoChar.X, Bossist.InfoChar.Y);
                            boss.InfoDelayBoss.DelayRemove = 900000 + timeserver;
                            Bossist.Zone.ZoneHandler.AddBoss(boss);
                        }
                        break;
                    case 9:
                        DelayXenBoHung = 129000 + timeserver;
                        SpawnXenBoHung = false;
                        break;
                    case 2:
                        DelayBlackGoku = 120000 + timeserver;
                        BlackGokuSpawn = false;
                        BlackGoku = null;
                        SuperBlackGoku = SetUpBoss(SuperBlackGoku, 3, Bossist.InfoChar.X, Bossist.InfoChar.Y);
                        Bossist.Zone.ZoneHandler.AddBoss(SuperBlackGoku);
                        break;
                    case 4:
                        Fide1 = null;
                        Fide2 = SetUpBoss(Fide2, 5, Bossist.InfoChar.X, Bossist.InfoChar.Y);
                        Fide2.InfoDelayBoss.DelayRemove = 900000 + timeserver;
                        Bossist.Zone.ZoneHandler.AddBoss(Fide2);
                        break;
                    case 5:
                        Fide2 = null;
                        Fide3 = SetUpBoss(Fide3, 6, Bossist.InfoChar.X, Bossist.InfoChar.Y);
                        Fide3.InfoDelayBoss.DelayRemove = 900000 + timeserver;
                        Bossist.Zone.ZoneHandler.AddBoss(Fide3);
                        break;
                    case 6:
                        Fide3 = null;
                        Fide1Spawn = false;
                        DelayFide1 = 122000 + timeserver;
                        break;
                    case 16:
                        {
                            var boss = GetBoss(Bossist.Zone, IdBossesTDST[1]);
                            boss.InfoDelayBoss.DelayRemove = 900000 + timeserver;
                            boss.Status = 0;
                            boss.InfoChar.TypePk = 5;
                            Bossist.Zone.ZoneHandler.SendMessage(Service.ChangeTypePk(boss.Id, 5));
                            break;
                        }
                    case 17:
                        {
                            var boss = GetBoss(Bossist.Zone, IdBossesTDST[2]);
                            boss.InfoDelayBoss.DelayRemove = 900000 + timeserver;
                            boss.InfoChar.TypePk = 5;
                            Bossist.Zone.ZoneHandler.SendMessage(Service.ChangeTypePk(boss.Id, 5));
                            boss.Status = 0;
                            break;
                        }
                    case 93:
                        {
                            var boss = GetBoss(Bossist.Zone, IdBossesTDST[3]);
                            boss.InfoDelayBoss.DelayRemove = 900000 + timeserver;
                            boss.InfoChar.TypePk = 5;
                            Bossist.Zone.ZoneHandler.SendMessage(Service.ChangeTypePk(boss.Id, 5));
                            boss.Status = 0;
                            break;
                        }
                    case 18:
                        {
                            var boss = GetBoss(Bossist.Zone, IdBossesTDST[4]);
                            boss.InfoDelayBoss.DelayRemove = 900000 + timeserver;
                            boss.InfoChar.TypePk = 5;
                            Bossist.Zone.ZoneHandler.SendMessage(Service.ChangeTypePk(boss.Id, 5));
                            boss.Status = 0;
                            break;
                        }
                    case 19:
                        DelayTDST = 15000 + timeserver;
                        SpawnTDST = false;
                        break;
                    case 28:
                        {
                            var boss = GetBoss(Bossist.Zone, IDBossesSatThu3[1]);
                            boss.InfoDelayBoss.DelayRemove = 900000 + timeserver;
                            boss.InfoChar.TypePk = 5;
                            Bossist.Zone.ZoneHandler.SendMessage(Service.ChangeTypePk(boss.Id, 5));
                            boss.Status = 0;
                            break;
                        }
                    case 27:
                        {
                            var boss = GetBoss(Bossist.Zone, IDBossesSatThu3[2]);
                            boss.InfoDelayBoss.DelayRemove = 900000 + timeserver;
                            boss.InfoChar.TypePk = 5;
                            Bossist.Zone.ZoneHandler.SendMessage(Service.ChangeTypePk(boss.Id, 5));
                            boss.Status = 0;
                            break;
                        }
                    case 29:
                        DelaySatThu3 = 126000 + timeserver;
                        SpawnSatThu3 = false;
                        break;
                    case 30:
                        {
                            var boss = GetBoss(Bossist.Zone, IDBossesSatThu1[1]);
                            boss.InfoDelayBoss.DelayRemove = 900000 + timeserver;
                            boss.InfoChar.TypePk = 5;
                            Bossist.Zone.ZoneHandler.SendMessage(Service.ChangeTypePk(boss.Id, 5));
                            boss.Status = 0;
                            break;
                        }
                    case 31:
                        DelaySatThu1 = 127000 + timeserver;
                        SpawnSatThu1 = false;
                        break;
                    case 34:
                        {
                            var boss = GetBoss(Bossist.Zone, IDBossesSatThu2[1]);
                            boss.InfoDelayBoss.DelayRemove = 900000 + timeserver;
                            boss.InfoChar.TypePk = 5;
                            Bossist.Zone.ZoneHandler.SendMessage(Service.ChangeTypePk(boss.Id, 5));
                            boss.Status = 0;
                            break;
                        }
                    case 33:
                        {
                            var boss = GetBoss(Bossist.Zone, IDBossesSatThu2[2]);
                            boss.InfoDelayBoss.DelayRemove = 900000 + timeserver;
                            boss.InfoChar.TypePk = 5;
                            Bossist.Zone.ZoneHandler.SendMessage(Service.ChangeTypePk(boss.Id, 5));
                            boss.Status = 0;
                            break;
                        }
                    case 32:
                        DelaySatThu2 = 128000 + timeserver;
                        SpawnSatThu2 = false;
                        break;
                }
            }
            catch (Exception e)
            {
                Server.Gi().Logger.Print("BossDiedError:" + e.Message + "\n" + e.StackTrace);
            }
        }
        public Boss GetBoss(IZone zone, int id)
        {
            return ((Boss)(zone.ZoneHandler.GetBossInMap(id)[0]));
        }
        public Boss KingKongHe = null;
        public Boss Piche = null;
        public long delayKingKongHe = -1;
        public long delayPicHe = -1;
        public string tileMapKingKongHe = "";
        public string tileMapPicHe = "";
        public int CountKKHe = 0;
        public int CountPicHe = 0;
        public bool SpawnKKHe = false;
        public bool SpawnPiche = false;
        public int IdKKHe = -1;
        public int IdPicHe = -1;
        public int ZoneKKhe = -1;
        public int ZonePicHe = -1;
        public int MapKKHe = -1;
        public int MapPicHe = -1;



    }
}
