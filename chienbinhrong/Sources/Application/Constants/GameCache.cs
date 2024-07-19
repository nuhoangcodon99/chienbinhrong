using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Interfaces.Character;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Application.Threading;
using NgocRongGold.DatabaseManager;
using NgocRongGold.Model.Template;
using Org.BouncyCastle.Crypto.Modes.Gcm;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using static Google.Protobuf.WellKnownTypes.Field.Types;

namespace Application.Constants
{
    public enum BANNER_ENUM
    {
        Recharge = 0,
        WishDragon = 1,
        DefeatSuperBoss = 2,
        UpgradeItem = 3,
        Championer = 4,
        SideQuest = 5,
        WolfPet = 6,
        Xinbato = 7,
        Farmer = 8,
        Thief = 9,
        ODo = 10,
    }
    public class GameCache
    {
        public GameCache()
        {
            LoadData();
        }
        public static GameCache instance;
        public static GameCache gI()
        {
            if (instance == null) instance = new GameCache();
            return instance;
        }
        #region DictionaryPath
        public string PathDataDecorative = "GameCache - _decorative";
        public string PathDataExp = "GameCache - _exp";
        #endregion
        #region Propeties
        public Dictionary<int, int> WishDragons = new Dictionary<int, int>();//<id người chơi, lần ước>
        public Dictionary<int, long> Recharges = new Dictionary<int, long>();//<id người chơi, tiền nạp hoặc giá trị quy đổi>
        public Dictionary<int, int> AnTromKill = new Dictionary<int, int>();//<id người chơi, số ăn trộm bị giết>
        public Dictionary<int, int> ODoKill = new Dictionary<int, int>();//<id người chơi, số ở dơ bị giết>
        public Dictionary<int, int> PickItems = new Dictionary<int, int>();//<id người chơi, số trang bị đã nhặt>
        public Dictionary<int, int> DefeatSoiHecQuyn = new Dictionary<int, int>();//<id người chơi, số lần khiến sói héc quyn khuất phục>
        public Dictionary<int, int> FinishExtremeSideQuest = new Dictionary<int, int>();//<id người chơi, số lần hoàn thành nhiệm vụ hằng ngày siêu khó ở bò mộng>
        public Dictionary<int, int> XinbatoXinNuoc = new Dictionary<int, int>();//<id người chơi, số lần cho nước Xinbato>
        public Dictionary<int, int> SuperBossKill = new Dictionary<int, int>();//<id người chơi, số lần giết Cumber,BlackGoku,Xên,Cooler>
        
        public ConcurrentDictionary<int, Tuple<bool, long, int, int>> BanhDangNau = new ConcurrentDictionary<int, Tuple<bool, long, int, int>>();
        public ConcurrentDictionary<int, int> BanhDaNauXong = new ConcurrentDictionary<int, int>();
        public Task HandleUpdate;
        public Task HandleUpdateNauBanh;
        #endregion


        #region Data
        public Decorative _Decorative;
        public Exp _Exp;
        public class Decorative
        {
            public long cDecorative = 0;
            public int DecorativeOrnament = 0;
        }
        public class Exp
        {
            public int currentUpExp = 0;
            public long timeUpExp = 0;
        }
        #endregion
        #region OtherFunction
        /*Dùng các vật phẩm để trang trí, nếu 2.000 lượt tặng sẽ x2 kinh nghiệm 12h cho toàn Server
          Dùng các vật phẩm để trang trí, nếu 4.000 lượt tặng sẽ x2 kinh nghiệm 24h cho toàn Server
          Dùng các vật phẩm để trang trí, nếu 7.000 lượt tặng sẽ x3 kinh nghiệm 24h cho toàn Server
          Dùng các vật phẩm để trang trí, nếu 10.000 lượt tặng sẽ x3 kinh nghiệm 36h cho toàn Server
          Dùng các vật phẩm để trang trí, nếu 18.000 lượt tặng sẽ x4 kinh nghiệm 48h cho toàn Server
        */
        public void Decorativity()
        {
            _Decorative.cDecorative++;
            var oldDecorativeOrnament = _Decorative.DecorativeOrnament;
            switch (_Decorative.cDecorative)
            {
                case >= 18000:
                    _Decorative.DecorativeOrnament = 5;
                    break;
                case >= 10000:
                    _Decorative.DecorativeOrnament = 4;
                    break;
                case >= 7000:
                    _Decorative.DecorativeOrnament = 3;
                    break;
                case >= 4000:
                    _Decorative.DecorativeOrnament = 2;
                    break;
                case >= 2000:
                    _Decorative.DecorativeOrnament = 1;
                    break;
            }
            if (oldDecorativeOrnament != _Decorative.DecorativeOrnament)
            {
                SetDecorativeOrnament();
            }
        }
        public void SetDecorativeOrnament()
        {
            var timeServer = ServerUtils.CurrentTimeMillis();
            switch (_Decorative.DecorativeOrnament)
            {
                case 1:
                    _Exp.currentUpExp = 2;
                    _Exp.timeUpExp = DataCache._1DAY / 2 + timeServer;
                    break;
                case 2:
                    _Exp.currentUpExp = 2;
                    _Exp.timeUpExp = DataCache._1DAY + timeServer;
                    break;
                case 3:
                    _Exp.currentUpExp = 3;
                    _Exp.timeUpExp = DataCache._1DAY + timeServer;
                    break;
                case 4:
                    _Exp.currentUpExp = 3;
                    _Exp.timeUpExp = DataCache._1DAY + (DataCache._1DAY / 2) + timeServer;
                    break;
                case 5:
                    _Exp.currentUpExp = 4;
                    _Exp.timeUpExp = DataCache._1DAY * 2 + timeServer;
                    break;
            }
            ConfigManager.gI().ExpUp += _Exp.currentUpExp;
        }
        public bool isDangNau(int key)
        {
            return BanhDangNau.Keys.Contains(key);

        }
        public bool isDaNauXong(int key)
        {
            return BanhDaNauXong.Keys.Contains(key);
        }
        public void ThemBanhDangNau(int key, bool isNauXong, long thoiGianNau, int idBanh)
        {
            BanhDangNau.AddOrUpdate(key,
                _ => Tuple.Create(isNauXong, thoiGianNau, key, idBanh),
                (_, existingValue) => Tuple.Create(isNauXong, thoiGianNau, key, idBanh));
        }
        public int GetBanhDaNauXong(int key)
        {
            if (BanhDaNauXong.TryGetValue(key, out var n))
            {
                return n;
            }
            else
            {
                return -1;
            }
        }
        public long GetTimeBanhDangNau(int key)
        {
            if (BanhDangNau.TryGetValue(key, out var n))
            {
                return n.Item2;
            }
            else
            {
                return -1;
            }
        }
        public void XoaBanhDaNauXong(int key)
        {
            BanhDaNauXong.TryRemove(key, out _);
        }
        #endregion
        #region Function
        public void LoadData()
        {
            _Decorative = ServerUtils.readDataJson<Decorative>(PathDataDecorative) == null ? new Decorative() : ServerUtils.readDataJson<Decorative>(PathDataDecorative);
            // _Exp = new Exp();
            _Exp = ServerUtils.readDataJson<Exp>(PathDataExp) == null ? new Exp() : ServerUtils.readDataJson<Exp>(PathDataExp);
        }
        public void SaveData()
        {
            ServerUtils.WriteData("_exp", JsonConvert.SerializeObject(_Exp));
            ServerUtils.WriteData("_decorative", JsonConvert.SerializeObject(_Decorative));
            Server.Gi().Logger.Print("Save Data GameCache Success", "cyan");
        }
        public void Update()
        {
            async Task Action()
            {
                while (Server.Gi().IsRunning)
                {
                    var timeServer = ServerUtils.CurrentTimeMillis();

                    if (_Exp.timeUpExp < timeServer && _Exp.currentUpExp > 0)
                    {
                        _Exp.timeUpExp = 0;
                        _Exp.currentUpExp = 0;  
                    }
                    await UpdateNauBanh(timeServer);
                    await Task.Delay(1000);
                }
                await HandleUpdate;
                HandleUpdate = null;
                GC.SuppressFinalize(this);
            }
            HandleUpdate = Task.Run(Action);
        }
        public async Task UpdateNauBanh(long timeServer)
        {
            Server.Gi().Logger.Debug("Có: " + BanhDangNau.Count + " bánh đang nấu <----");
            var banhChuaNauXong = BanhDangNau.Where(tuple => !tuple.Value.Item1 && tuple.Value.Item2 < timeServer).ToList();
            foreach (var tuple in banhChuaNauXong)
            {

                if (BanhDaNauXong.TryAdd(tuple.Value.Item3, tuple.Value.Item4))
                {
                    Server.Gi().Logger.Debug("thêm bánh đã nấu xong <----");

                }
                if (BanhDangNau.TryRemove(tuple.Key, out _))
                {
                    Server.Gi().Logger.Debug("Xóa bánh đang nấu <----");
                }
            }

            await Task.CompletedTask;
        }
        public void Reset()//reset khi qua ngày mới 
        {
            WishDragons.Clear();
            Recharges.Clear();
            AnTromKill.Clear();
            ODoKill.Clear();
            PickItems.Clear();
            DefeatSoiHecQuyn.Clear();
            FinishExtremeSideQuest.Clear();
            XinbatoXinNuoc.Clear();
            SuperBossKill.Clear();
        }
        public void AddOrUpdate(Dictionary<int, int> Dictionary, int key, int value)
        {
            if (Dictionary.ContainsKey(key))
            {
                Dictionary[key] = value;
            }
            else
            {
                Dictionary.Add(key, value);
            }
        }
        public void AddOrUpdate(Dictionary<int, long> Dictionary, int key, int value)
        {
            if (Dictionary.ContainsKey(key))
            {
                Dictionary[key] = value;
            }
            else
            {
                Dictionary.Add(key, value);
            }
        }
        public void HandleBanner(int key, int value,BANNER_ENUM banner)
        {
            switch (banner)
            {
                case BANNER_ENUM.WishDragon:
                    AddOrUpdate(WishDragons, key, value);
                    if (WishDragons[key] >= 10)
                    {
                        var client = ClientManager.Gi().GetCharacter(key);
                        if (client != null)
                        {
                            HandleAddBanner(client, "Trùm ước rồng");
                        }
                    }
                    break;
            }
        }
        public void HandleAddBanner(ICharacter character, string name)
        {
            character.InfoChar.Roles1.Roles.Add(Cache.Gi().Role1Templates.FirstOrDefault(i => i.Name.Contains(name)));
            character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã nhận được danh hiệu " + name + " hãy đi đến Santa và kiểm tra"));
        }
        #endregion
    }
}
