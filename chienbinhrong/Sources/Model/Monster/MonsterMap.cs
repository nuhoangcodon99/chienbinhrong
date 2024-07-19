
using NgocRongGold.Application.Handlers.Monster;
using NgocRongGold.Application.Interfaces.Monster;
using NgocRongGold.Model.ModelBase;

namespace NgocRongGold.Model.Monster
{
    public class MonsterMap : MonsterBase
    {
        public MonsterMap()
        {
            IsMobMe = false;
            IsDie = false;
            MonsterHandler = new MonsterMapHandler(this);
        }
        public MonsterMap(int type)//0 new mod, 1 bigboss
        {
            IsMobMe = false;
            IsDie = false;
            switch (type)
            {
                default:
                    MonsterHandler = new NewMobHandler(this);
                    break;
            }
        }
    }
}