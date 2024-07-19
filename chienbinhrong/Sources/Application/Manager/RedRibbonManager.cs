using Application.Interfaces.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chiến_Binh_Rồng.Sources.Application.Manager
{
    public class RedRibbonManager
    {
        public int countRedRibbon = 0;
        public static RedRibbonManager Instance { get; set; }
        public static RedRibbonManager gI()
        {
            if (Instance == null) Instance = new RedRibbonManager();
            return Instance;
        }

    }
}
