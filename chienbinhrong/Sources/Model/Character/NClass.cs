using System.Collections.Generic;
using NgocRongGold.Model.Template;

namespace NgocRongGold.Model
{
    public class NClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SkillTemplate> SkillTemplates { get; set; }

        public NClass()
        {
            
        }
    }
}