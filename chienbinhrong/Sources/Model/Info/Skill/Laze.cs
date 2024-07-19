namespace NgocRongGold.Model.Info.Skill
{
    public class Laze
    {
        public bool Hold { get; set; }
        public long Time { get; set; }
        public bool isLaze { get; set; }
        public Laze()
        {
            isLaze = false;
            Hold = false;
            Time = -1;
        }
    }
}