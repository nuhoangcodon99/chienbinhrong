using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Model.Info
{
    public class MuaGiaiTemplate
    {
        public int Count { get; set; }
        public string Name { get; set; }
        public List<int> Gift = new List<int>();
    }
    public class InfoMuaGiai
    {
        public Dictionary<int, MuaGiaiTemplate> Info { get; set; }//dictionary<mùa giải, vip 1,2,3>
        public InfoMuaGiai()
        {
            Info = new Dictionary<int, MuaGiaiTemplate>();
        }
        public void Add(int MuaGiai, int Gift)
        {
            MuaGiaiTemplate muaGiaiTemplate = null;
            if (!Info.ContainsKey(MuaGiai)){
                muaGiaiTemplate= new MuaGiaiTemplate();
            }
            else
            {
                muaGiaiTemplate = Get(MuaGiai);
                
            }
            muaGiaiTemplate.Gift.Add(Gift);
        }
        public MuaGiaiTemplate Get(int MuaGiai)
        {
            return Info[MuaGiai] != null ? Info[MuaGiai] : AddNewMuaGiai(MuaGiai);
        }
        public MuaGiaiTemplate AddNewMuaGiai(int MuaGiai, string Name = "")
        {
            var newMuaGiai = new MuaGiaiTemplate();
            newMuaGiai.Name = Name;
            newMuaGiai.Gift = new List<int>();
            Info.TryAdd(MuaGiai, newMuaGiai);
            return newMuaGiai;
        }
        
    }
}
