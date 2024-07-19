using System;
using Application.Interfaces.Zone;
using NgocRongGold.Model.Map;

namespace NgocRongGold.Application.Interfaces.Item
{
    public interface IItemMapHandler : IDisposable
    {
        void Update(IZone zone);
    }
}