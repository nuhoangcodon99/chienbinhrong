using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Effect
{
    public class EffectHandler
    {
        public NgocRongGold.Model.Info.Effect.Effect Effect { get; set; }
        public EffectHandler(NgocRongGold.Model.Info.Effect.Effect effect)
        {
            Effect = effect;
        }
        public void Update()
        {
        }
        public void Dispose()
        {
            lock (Effect)
            {
                Effect = null;
                GC.SuppressFinalize(this);
            }
        }
    }
}
