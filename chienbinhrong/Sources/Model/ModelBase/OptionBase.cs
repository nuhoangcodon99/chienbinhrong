﻿using System;

namespace NgocRongGold.Model.ModelBase
{
    public class OptionBase : ICloneable
    {
        public int Id { get; set; }
        public int Param { get; set; }

        public virtual object Clone()
        {
            return new OptionBase()
            {
                Id = Id,
                Param = Param
            };
        }
    }
}