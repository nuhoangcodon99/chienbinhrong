using NgocRongGold.Application.IO;
using NgocRongGold.DatabaseManager;
using NgocRongGold.Model.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.SideQuest.HangNgay
{
    public class HangNgayQuest_Data
    {
        public int Count { get; set; }
        public int Progress { get; set; }
        public HangNgayQuest_Quest Quest { get; set; }
        public HangNgayQuest_Status Status { get; set; }
        public long TimeAccecpt { get; set; }
        public HangNgayQuest_Object Object { get; set; }
        public HangNgayQuest_Data()
        {
            Count = 10;
            Progress = 0;
            Quest = null;
            Object = null;
        }
        public void Reset()
        {
            Count = 10;
            Progress = 0;
            Quest = null;
            Object = null;
        }
        public bool CheckType(HangNgayQuest_Type tyoe)
        {
            return Quest != null && (Quest.Type == tyoe);
        }
        public string GetCurrentTask()
        {
            if (HaveTask())
            {
                var text = $"Nhiệm vụ cấp độ {GetDifficult()}: {GetActivity((int)Quest.Type)} {Quest.MaxProgress} {Object.Name}\n";
               // if (Quest.Type is 0) text += $"Địa điểm {GetSite()}\n";
                text += $"Tiến độ {Progress}/{Quest.MaxProgress}\n";
                text += $"Thời gian nhận nhiệm vụ: {ServerUtils.GetTimeInPast(ServerUtils.CurrentTimeMillis(), TimeAccecpt)}";
                return text;
            }
            return "";
        }
        public string GetActivity(int type)
        {
            switch (type) {
                case 0:
                    return "Hạ";
                case 1:
                    return "Giết";
                default:
                    return "Nhặt";

            }
        }
        public string GetSite()
        {
            return Object.Site;
        }
        public void SetNameObj(int id)
        {
            switch (Quest.Type)
            {
                case HangNgayQuest_Type.KILL_MOB:
                    switch (Object.Id)
                    {
                        default:
                            Object.Name = Cache.Gi().MONSTER_TEMPLATES[id].Name;
                            break;
                    }
                    break;
                case HangNgayQuest_Type.KILL_BOSS:
                    switch (Object.Id)
                    {
                        default:
                            Object.Name = Cache.Gi().BOSS_TEMPLATES[id].Name; 
                            break;
                    }
                    break;
            }
        }
        public void SetObj(params int[] id)
        {
            int idObj = id[0];
            if (id.Length > 1)
            {
                idObj = id[ServerUtils.RandomNumber(id.Length)];
            }
            Object = new HangNgayQuest_Object();
            Object.Id = idObj;
            SetNameObj(idObj);
        }
        public int GetMaxProcess()
        {
            var mProcess = 0;
            switch(Quest.Type)
            {
                case HangNgayQuest_Type.KILL_MOB:
                    switch (Quest.Difficult)
                    {
                        case HangNgayQuest_Difficult.Dễ:
                            mProcess = ServerUtils.RandomNumber(11, 15);
                            break;
                        case HangNgayQuest_Difficult.Thường:
                            mProcess = ServerUtils.RandomNumber(15, 19);
                            break;
                        case HangNgayQuest_Difficult.Khó:
                            mProcess = ServerUtils.RandomNumber(19, 26);
                            break;
                        case HangNgayQuest_Difficult.Hell:
                            mProcess = ServerUtils.RandomNumber(26, 35);
                            break;
                    }
                    break;
                case HangNgayQuest_Type.KILL_BOSS:
                    switch (Quest.Difficult)
                    {
                        case HangNgayQuest_Difficult.Dễ:
                            mProcess = ServerUtils.RandomNumber(1, 3);
                            break;
                        case HangNgayQuest_Difficult.Thường:
                            mProcess = ServerUtils.RandomNumber(2, 4);
                            break;
                        case HangNgayQuest_Difficult.Khó:
                            mProcess = ServerUtils.RandomNumber(3, 5);
                            break;
                        case HangNgayQuest_Difficult.Hell:
                            mProcess = ServerUtils.RandomNumber(4, 7);
                            break;
                    }
                    break;
                default:
                    switch (Quest.Difficult)
                    {
                        case HangNgayQuest_Difficult.Dễ:
                            mProcess = ServerUtils.RandomNumber(11000, 15000);
                            break;
                        case HangNgayQuest_Difficult.Thường:
                            mProcess = ServerUtils.RandomNumber(150000, 190000);
                            break;
                        case HangNgayQuest_Difficult.Khó:
                            mProcess = ServerUtils.RandomNumber(1900000, 2600000);
                            break;
                        case HangNgayQuest_Difficult.Hell:
                            mProcess = ServerUtils.RandomNumber(2600000, 3500000);
                            break;
                    }
                    break;
            }
            return mProcess;
        }
        public void GetObj(Character character)
        {
            switch (Quest.Type)
            {
                case HangNgayQuest_Type.KILL_MOB:
                    switch (character.InfoTask.Id)
                    {
                        case > 32://hanh tinh thuc vat
                            SetObj(80, 81);
                            break;
                        case > 29://xen cap 8
                            SetObj(65);
                            break;
                        case > 28://xen cap 5
                            SetObj(62);
                            break;
                        case > 26://xen cap 3
                            SetObj(60);
                            break;
                        case 25://xen cap 1
                            SetObj(58);

                            break;
                        case 24://khi long vang
                            SetObj(57);
                            break;
                        case 23://khi long den || khi long do
                            SetObj(54, 56);
                            break;
                        case > 21://nappa
                            SetObj(39, 40, 41, 42, 43);
                            break;
                        case 20:
                            SetObj(24, 25, 26);//drum
                            break;
                        case > 16://bulon
                            SetObj(22, 23, 24);
                            break;
                        case  15://oc muon hon
                            SetObj(13, 14, 15);
                            break;
                        case  14://heo rung
                            SetObj(16, 17, 18);
                            break;
                        case >= 0://moc nhan
                            SetObj(0);
                            break;
                    }
                    break;
                case HangNgayQuest_Type.KILL_BOSS:
                    switch (character.InfoTask.Id)
                    {
                        case >= 33://black goku or chilled
                            SetObj(14, 15, 2);
                            break;
                        case >= 31://mabu
                            SetObj(39);
                            break;
                        case >= 25:
                            SetObj(24, 25, 26, 4, 5, 6);
                            break;
                        case >= 23://fide
                            SetObj(4, 5, 6);
                            break;
                        case <= 22://kuku, rambo, map dau dinh
                            SetObj(24, 25, 26);
                            break;
                    }
                    break;
                case HangNgayQuest_Type.PICK_GOLD:
                    SetObj(188);
                    break;
            }
        }
        public bool HaveTask()
        {
            return Quest != null;
        }
        
        public int GetPercent()
        {
            return (Progress / Quest.MaxProgress) * 100;
        }
        public bool IsDone()
        {
            return Status is HangNgayQuest_Status.FINISHED;
        }
        public string GetDifficult()
        {
            return Quest.Difficult.ToString();
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
