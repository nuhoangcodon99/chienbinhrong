using System.Collections.Generic;
using NgocRongGold.Model.ModelBase;

namespace NgocRongGold.Model.Template
{
    public class TaskTemplate : TaskBase
    {
        public string Name { get; set; }
        public string Detail { get; set; }

        public TaskTemplate() : base()
        {
        }
    }
}