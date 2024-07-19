using NgocRongGold.Application.Extension.Practice.Whis;
using NgocRongGold.Model.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.Practice
{
    public class Practice_Data
    {
        public int Potential { get; set; } = 0;
        public Practice_Progress Progress = Practice_Progress.THAN_MEO_KARIN;
        public Practice_Progress Practice_Progress = Practice_Progress.THAN_MEO_KARIN;
        public Practice_Handler Handler = new Practice_Handler();
        public long Time { get; set; } = 0;// thời gian luyện tập 
        public Practice_Staus Status = Practice_Staus.NORMAL;
        public AutoTrain_Status TrainStatus = AutoTrain_Status.NORMAL;
        public Whis_Data Whis = new Whis_Data();
        public int MapOldId = 0;
        public int MapPracticeId = 0;
        
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public int GetPotenial()
        {
            return (int)(Math.Pow(2, (int)Progress) * 10);//Pow (lũy thừa) 
        }
        public int GetPotenial(int value)
        {
            return (int)(Math.Pow(2, (int)value) * 10);//Pow (lũy thừa) 
        }
        public bool isAutoTrain()
        {
            return TrainStatus.Equals(AutoTrain_Status.AUTO_TRAIN);
        }
    }
}
