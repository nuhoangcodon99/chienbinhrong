using System.Collections.Generic;
using NgocRongGold.Model;
using NgocRongGold.Model.Data;
using NgocRongGold.Model.Map;
using NgocRongGold.Model.Template;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Item;
using NgocRongGold.Sources.Base.Template;
using NgocRongGold.Model.Info;
using NgocRongGold.Application.Extension.Ký_gửi;
using NgocRongGold.Application.Extension.DaiHoiVoThuat.SuperChampion;
using NgocRongGold.Application.Extension.SideQuest.HangNgay;

namespace NgocRongGold.DatabaseManager
{
    public class Cache
    {
        private static Cache Instance;
        public static readonly int MaxIcon = 30000;
        public readonly List<string> RegexText = new List<string>();
        public readonly Dictionary<string, List<ImageByName>> ImgByName = new Dictionary<string, List<ImageByName>>();
        public readonly List<Arrow> NRArrows = new List<Arrow>();
        public readonly List<Role1Template> Role1Templates = new List<Role1Template>();
        public readonly List<Dart> NRDarts = new List<Dart>();
        public readonly List<Effect> NREffects = new List<Effect>();
        public readonly List<Image> NRImages = new List<Image>();
        public readonly List<Part> NRParts = new List<Part>();
        public readonly List<Skill> NRSkills = new List<Skill>();
        
        public readonly Dictionary<int, int[]> TASKS = new Dictionary<int, int[]>();
        public readonly Dictionary<int, int[]> MAPTASKS = new Dictionary<int, int[]>();
        public readonly Dictionary<int, HangNgayQuest_Quest> SideTask_HangNgay = new Dictionary<int, HangNgayQuest_Quest>();

        // public readonly Dictionary<short, TaskTemplate> TASK_TEMPLATES = new Dictionary<short, TaskTemplate>();
        public readonly Dictionary<short, TaskTemplate> TASK_TEMPLATES_0 = new Dictionary<short, TaskTemplate>();
        public readonly Dictionary<short, TaskTemplate> TASK_TEMPLATES_1 = new Dictionary<short, TaskTemplate>();
        public readonly Dictionary<short, TaskTemplate> TASK_TEMPLATES_2 = new Dictionary<short, TaskTemplate>();
        public readonly Dictionary<short, DiscipleTemplate> DISCIPLE_TEMPLATE = new Dictionary<short, DiscipleTemplate>();
        public readonly Dictionary<short, NgocRongGold.Application.Extension.Bo_Mong.BoMong_Task_Template> TASK_BO_MONG = new Dictionary<short, NgocRongGold.Application.Extension.Bo_Mong.BoMong_Task_Template>();
        public readonly List<long> EXPS = new List<long>();
        public readonly Dictionary<int, LimitPower> LIMIT_POWERS = new Dictionary<int, LimitPower>();
        public readonly List<Level> LEVELS = new List<Level>();
        public readonly Dictionary<int, int> AVATAR = new Dictionary<int, int>();
        public readonly List<BackgroundItemTemplate> BACKGROUND_ITEM_TEMPLATES = new List<BackgroundItemTemplate>();
        public readonly List<MonsterTemplate> MONSTER_TEMPLATES = new List<MonsterTemplate>();
        public readonly List<NpcTemplate> NPC_TEMPLATES = new List<NpcTemplate>();
        public readonly List<BossTemplate> BOSS_TEMPLATES = new List<BossTemplate>();
        public readonly List<ItemOptionTemplate> ITEM_OPTION_TEMPLATES = new List<ItemOptionTemplate>();
        public readonly Dictionary<int, ItemTemplate> ITEM_TEMPLATES = new Dictionary<int, ItemTemplate>();
        public readonly List<SkillTemplate> SKILL_TEMPLATES = new List<SkillTemplate>();
        public readonly List<SkillOption> SKILL_OPTIONS = new List<SkillOption>();
        public readonly List<TileMap> TILE_MAPS = new List<TileMap>();
        public readonly Dictionary<int, SuperChampion_Championer> SuperChampioner = new Dictionary<int, SuperChampion_Championer>();
        public readonly Dictionary<short, Application.Extension.CaiTrangTemplate> CaiTrangTemplate = new Dictionary<short, Application.Extension.CaiTrangTemplate>();
        public readonly Dictionary<short, Application.Extension.Epic_Pet> LinhThu = new Dictionary<short, Application.Extension.Epic_Pet>();

        public readonly Dictionary<int, List<ShopTemplate>> SHOP_TEMPLATES = new Dictionary<int, List<ShopTemplate>>();
        public readonly List<KyGUIItem> KY_GUI_ITEMS = new List<KyGUIItem>();

        public readonly Dictionary<int, Application.Extension.Ký_gửi.KyGUIItem> kyGUIItems2 = new Dictionary<int, Application.Extension.Ký_gửi.KyGUIItem>();

        //public readonly List<NgocRongGold.Sources.Application.Extension.ShopKyGui> SHOP_KY_GUI = new List<NgocRongGold.Sources.Application.Extension.ShopKyGui>();
        public readonly Dictionary<int, List<SpecialSkillTemplate>> SPECIAL_SKILL_TEMPLATES = new Dictionary<int, List<SpecialSkillTemplate>>();
        public readonly List<GameInfoTemplate> GAME_INFO_TEMPLATES = new List<GameInfoTemplate>();
        public readonly List<NClass> NClasses = new List<NClass>();
        public readonly Dictionary<short, short> PARTS = new Dictionary<short, short>();

        public byte[][] DATA_MONSTERS_X1 = new byte[100][];
        public byte[][] DATA_MONSTERS_X2 = new byte[100][];
        public byte[][] DATA_MONSTERS_X3 = new byte[100][];
        public byte[][] DATA_MONSTERS_X4 = new byte[100][];
        public byte[][] IMAGE_MONSTERS_X1 = new byte[100][];
        public byte[][] IMAGE_MONSTERS_X2 = new byte[100][];
        public byte[][] IMAGE_MONSTERS_X3 = new byte[100][];
        public byte[][] IMAGE_MONSTERS_X4 = new byte[100][];
        public byte[][][] DATA_MONSTERS_ALL = new byte[byte.MaxValue][][];

        public readonly byte[][] DATA_ICON_X1 = new byte[MaxIcon][];
        public readonly byte[][] DATA_ICON_X2 = new byte[MaxIcon][];
        public readonly byte[][] DATA_ICON_X3 = new byte[MaxIcon][];
        public readonly byte[][] DATA_ICON_X4 = new byte[MaxIcon][];

        public readonly byte[][] DATA_BgItem_X1 = new byte[348][];
        public readonly byte[][] DATA_BgItem_X2 = new byte[348][];
        public readonly byte[][] DATA_BgItem_X3 = new byte[348][];
        public readonly byte[][] DATA_BgItem_X4 = new byte[348][];

        public readonly sbyte[] DATA_VERSION_BgItem_X1 = new sbyte[348];
        public readonly sbyte[] DATA_VERSION_BgItem_X2 = new sbyte[348];
        public readonly sbyte[] DATA_VERSION_BgItem_X3 = new sbyte[348];
        public readonly sbyte[] DATA_VERSION_BgItem_X4 = new sbyte[348];

        public readonly byte[][][] DATA_ICON_ALL = new byte[byte.MaxValue][][];
        public readonly sbyte[][] VERSION_ICON_ALL = new sbyte[sbyte.MaxValue][];


        public readonly Dictionary<int, List<List<int>>> DATA_MAGICTREE = new Dictionary<int, List<List<int>>>();
        public readonly List<ClanImage> CLAN_IMAGES = new List<ClanImage>();
        public readonly List<RadarTemplate> RADAR_TEMPLATE = new List<RadarTemplate>();
        public readonly List<ABossTemplate> AUTOBOSS_TEMPLATE = new List<ABossTemplate>();
        public int[][] TILE_TYPES;
        public int[][][] TILE_INDEXS;

        public readonly List<sbyte> NR_DART = new List<sbyte>();
        public readonly List<sbyte> NR_ARROW= new List<sbyte>();
        public readonly List<sbyte> NR_IMAGE = new List<sbyte>();
        public readonly List<sbyte> NR_EFFECT = new List<sbyte>();
        public readonly List<sbyte> NR_PART = new List<sbyte>();
        public readonly List<sbyte> NR_SKILL = new List<sbyte>();
        public readonly List<sbyte> NRMAP = new List<sbyte>();
        public readonly List<sbyte> NRSKILL = new List<sbyte>();
        public readonly List<sbyte> DataItemTemplateOld = new List<sbyte>();
        public readonly List<sbyte> DataItemTemplateNew = new List<sbyte>();

        public readonly List<int[]> DataArrHead = new List<int[]>();

        public readonly sbyte[] VersionIconX1 = new sbyte[MaxIcon];
        public readonly sbyte[] VersionIconX2 = new sbyte[MaxIcon];
        public readonly sbyte[] VersionIconX3 = new sbyte[MaxIcon];
        public readonly sbyte[] VersionIconX4 = new sbyte[MaxIcon];
        public readonly List<sbyte> VersionIcon = new List<sbyte>();
        public readonly List<sbyte> NR_PART2 = new List<sbyte>();
        public static Cache Gi()
        {
            return Instance ??= new Cache();
        }

        public static void ClearCache()
        {
            Instance = null;
        }
    }
}