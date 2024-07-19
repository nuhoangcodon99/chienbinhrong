using NgocRongGold.Model.Template;

namespace NgocRongGold.Model.Info.Skill
{
    public class TuSat
    {
        public long Delay { get; set; }
        public int Damage { get; set; }
        public bool isTuSat { get; set; }
        public SkillDataTemplate Template { get; set; }
        public TuSat()
        {
            isTuSat = false;
            Delay = -1;
            Damage = 0;
            Template = null;
        }
    }
}