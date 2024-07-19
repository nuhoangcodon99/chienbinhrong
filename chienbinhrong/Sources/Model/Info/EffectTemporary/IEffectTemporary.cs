using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Info.EffectTemporary
{
    public interface IEffectTemporary : IDisposable
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public long EffectiveTime { get; set; }
        public bool isEffective { get; set; }
        public int Param { get; set; }
        public Model.Character.Character CharacterTemp { get; set; }
        public Model.Character.Character Character { get; set; }

        public void Update(long timeServer);
        public void Setup();
        public void SetupInfo();
        public void Reset();
    }
}
