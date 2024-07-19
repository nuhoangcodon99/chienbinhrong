using NgocRongGold.Model.Monster;

namespace NgocRongGold.Model.Info.Skill
{
    public class Egg
    {
        public MonsterPet Monster{ get; set; }
        public long Time { get; set; }

        public Egg()
        {
            Monster = null;
            Time = -1;
        }
    }
}