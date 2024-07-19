using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using NgocRongGold.Application.Interfaces.Character;

namespace NgocRongGold.Application.Interfaces.Map
{
    public interface IMapCustom
    {
        int Id { get; set; }
        IList<Threading.Map> Maps { get; set; }
        public long Time { get; set; }
    }
}