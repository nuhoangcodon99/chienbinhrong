using System;
using System.Collections.Generic;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.Item;
using NgocRongGold.Application.Threading;
using NgocRongGold.DatabaseManager;
using System.Data.Common;
using Newtonsoft.Json;
using System.Threading;
using NgocRongGold.DatabaseManager.Player;

namespace NgocRongGold.Application.Extension.BlackballWar
{
    public class Blackball
    {

        public static Blackball instance;

        public int Star { get; set; } = -1;
        public long Time { get; set; } = -1;

        public static Blackball gI()
        {
            if (instance == null) instance = new Blackball();
            return instance;
        }

    }
}