using System.Collections.Generic;
using System.Linq;
using NgocRongGold.Model.Option;
using NgocRongGold.Model.Item;
using NgocRongGold.Application.Threading;

namespace NgocRongGold.Model.ModelBase
{
    public class ItemBase
    {
        public short Id { get; set; }
        public int Vang
        { get; set; } = 0;
        public int Ngoc { get; set; } = 0;
        public int Quantity { get; set; } = 1;
        public string Reason { get; set; }
        public List<OptionItem> Options { get; set; }

        public ItemBase()
        {
            Reason = "";
            Options = new List<OptionItem>();
        }

        public int GetParamOption(int id)
        {
            var option = Options.FirstOrDefault(op => op.Id == id);
            return option != null ? option.Param : 0;
        }
        public void LogOption(string reason = "")
        {
            var text = $"{reason}\n";
            Options.ForEach(opt =>
            {
                text += $"{opt.Id} | {opt.Param}\n";
            });
            text += "\n-------------------------";
            Server.Gi().Logger.Debug(text);
        }
        public OptionItem GetOption(int id) { return Options.FirstOrDefault(i => i.Id == id); }
        public bool isHaveOption(int id)
        {
            var option = Options.FirstOrDefault(op => op.Id == id);
            return option != null ? true : false;
        }
    }
}