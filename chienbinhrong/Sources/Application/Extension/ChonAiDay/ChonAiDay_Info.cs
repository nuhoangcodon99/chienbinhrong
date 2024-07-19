using NgocRongGold.Application.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.ChonAiDay
{
    public class ChonAiDay_Info
    {
        public List<long> TongGiaiThuongVang = new List<long> { 0, 0, };//normal, vip
        public List<List<long>> TongThamGiaVang = new List<List<long>> { new List<long>(), new List<long>() };//normal, vip
        public List<long> TongGiaiThuongNgocXanh = new List<long> { 0, 0, };//normal, vip
        public List<List<long>> TongThamGiaNgocXanh = new List<List<long>> { new List<long>(), new List<long>() };//normal, vip
        public List<long> TongGiaiThuongNgocDo = new List<long> { 0, 0 };//normal, vip
        public List<List<long>> TongThamGiaNgocDo = new List<List<long>> { new List<long>(), new List<long>() };//normal, vip
        public ChonAiDay_Status Status = ChonAiDay_Status.PICK;//PICK
        public long TimeStart = ServerUtils.CurrentTimeMillis();
        public long TimeEnd = 300000 + ServerUtils.CurrentTimeMillis();
        public string Name = "";
    }
}
