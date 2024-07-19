﻿using System.Collections;
using System.Collections.Generic;

namespace NgocRongGold.Model.Data
{
    public class Arrow
    {
        public short Id { get; set; }
        public List<short> Data { get; set; }

        public Arrow()
        {
            Data = new List<short>();
        }
    }
}