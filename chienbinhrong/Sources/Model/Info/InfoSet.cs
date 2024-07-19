namespace NgocRongGold.Model.Info
{
    public class InfoSet
    {
        public bool IsFullSetThanLinh = false;
        public int CountSetThanLinh = 0;
        public bool IsFullSetKirin = false;
        public int CountSetKirin = 0;

        public bool IsFullSetSongoku = false;
        public int CountSetSongoku = 0;

        public bool IsFullSetThienXinHang = false;
        public int CountSetThienXinHang = 0;

        public bool IsFullSetNappa = false;
        public int CountSetNappa = 0;
        public bool IsFullSetKakarot = false;
        public int CountSetKakarot = 0;
        public bool IsFullSetCadic = false;
        public int CountSetCadic = 0;
        public bool IsFullSetOcTieu = false;
        public int CountSetOcTieu = 0;
        public bool IsFullSetPicolo = false;
        public int CountSetPicolo = 0;
        public bool IsFullSetPikkoro = false;
        public int CountSetPikkoro = 0;
        public bool IsFullSetZelot = false;
        public int CountSetZelot = 0;
        public bool IsFullSetTinhAn = false;
        public int CountSetTinhAn = 0;
        public bool IsFullSetNguyetAn = false;
        public int CountSetNguyetAn = 0;
        public bool IsFullSetNhatAn = false;
        public int CountSetNhatAn = 0;
        public bool IsFullSetHuyDiet = false;
        public int CountSetHuyDiet = 0;
        public bool IsQuanBoi = false;  
        
        public InfoSet()
        {
            IsFullSetThanLinh = false;
            IsFullSetHuyDiet = false;
            IsFullSetKirin = false;
            IsFullSetSongoku = false;
            IsFullSetThienXinHang = false;
            IsFullSetNappa = false;
            IsFullSetKakarot = false;
            IsFullSetCadic = false;
            IsQuanBoi = false;
            IsFullSetOcTieu = false;
            IsFullSetPicolo = false;
            IsFullSetPikkoro = false;
            IsFullSetZelot = false;
            IsFullSetNhatAn = false;
            IsFullSetTinhAn = false;
            IsFullSetNguyetAn = false;

            CountSetCadic = 0;
            CountSetHuyDiet = 0;
            CountSetKakarot = 0;
            CountSetKirin = 0;
            CountSetNappa = 0;
            CountSetNguyetAn = 0;
            CountSetNhatAn = 0;
            CountSetOcTieu = 0;
            CountSetPicolo = 0;
            CountSetPikkoro = 0;
            CountSetSongoku = 0;
            CountSetThanLinh = 0;
            CountSetThienXinHang = 0;
            CountSetTinhAn = 0;
            CountSetZelot = 0;
        }
        public void UpdateSetInfoCounter(int count, ref int counter, ref bool isFull)
        {
            counter += count;
            if (counter >= 5)
            {
                isFull = true;
            }
        }
        public void Reset()
        {
            IsFullSetThanLinh = false;

            IsFullSetKirin = false;
            IsFullSetSongoku = false;
            IsFullSetThienXinHang = false;
            IsQuanBoi = false;
            IsFullSetNappa = false;
            IsFullSetKakarot = false;
            IsFullSetCadic = false;
            IsFullSetHuyDiet = false;
            IsFullSetOcTieu = false;
            IsFullSetPicolo = false;
            IsFullSetPikkoro = false;
            IsFullSetZelot = false;
            IsFullSetNhatAn = false;
            IsFullSetTinhAn = false;
            IsFullSetNguyetAn = false;

            CountSetCadic = 0;
            CountSetHuyDiet = 0;
            CountSetKakarot = 0;
            CountSetKirin = 0;
            CountSetNappa = 0;
            CountSetNguyetAn = 0;
            CountSetNhatAn = 0;
            CountSetOcTieu = 0;
            CountSetPicolo = 0;
            CountSetPikkoro = 0;
            CountSetSongoku = 0;
            CountSetThanLinh = 0;
            CountSetThienXinHang = 0;
            CountSetTinhAn = 0;
            CountSetZelot = 0;
        }
    }
}