﻿using NgocRongGold.Application.IO;

namespace NgocRongGold.Model.Info
{
    public class InfoNapThe
    {
        public string SoSeri { get; set; }
        public string MaPin { get; set; }
        public string LoaiThe { get; set; }
        public long MenhGia { get; set; }

        public InfoNapThe()
        {
            SoSeri = "";
            MaPin = "";
            LoaiThe = "";
            MenhGia = 0;
        }
    }
}