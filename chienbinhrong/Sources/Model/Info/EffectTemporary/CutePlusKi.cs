using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Threading;
using NgocRongGold.Info.EffectTemporary;
using NgocRongGold.Model.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Model.Info.EffectTemporary
{
    public class CutePlusKi : IEffectTemporary
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public long EffectiveTime { get; set; }
        public bool isEffective { get; set; }
        public Model.Character.Character Character { get; set; }
        public int Param { get; set; }
        public Model.Character.Character CharacterTemp { get; set; }

        public CutePlusKi(Model.Character.Character character, Model.Character.Character CharacterMakeEffect, int param)
        {
            ID = (int)EffectTemporaryTemplate.CUTE;
            Param = param;
            Name = $"{CharacterMakeEffect.Name}: Cute hồi {param}% KI bản thân và người xung quanh";
            Character = character;
            CharacterTemp = CharacterMakeEffect;
            EffectiveTime = 1000 + ServerUtils.CurrentTimeMillis();
            isEffective = true;
            Setup();
            Character.CharacterHandler.SendZoneMessage(Service.PublicChat(Character.Id, $"{CharacterMakeEffect.Name} Cute quá"));

        }

        public void Setup()
        {
            Character.CharacterHandler.SendMessage(Service.ItemTimeWithMessage(Name, ID, 1));
            var kiPlus = Character.MpFull * 2 / 100;
            if (Character.InfoChar.Mp + kiPlus > Character.MpFull)
            {
                kiPlus = Character.MpFull - Character.InfoChar.Mp;
            }
            Character.CharacterHandler.PlusMp((int)kiPlus);
            Character.CharacterHandler.SendMessage(Service.SendMp((int)Character.InfoChar.Mp));
        }
        public void SetupInfo()
        {
           
        }
        public void Update(long timeServer)
        {
            if (EffectiveTime < timeServer)
            {
                isEffective = false;
                Reset();
                Dispose();
            }
        }
        public void Reset()
        {
            Character.CharacterHandler.SetUpInfo(true);
            Character.CharacterHandler.SendMessage(Service.MeLoadPoint(Character));
            Character.EffectTemporaries.Remove(this);
        }
        public void Dispose()
        {
            // Implement disposal logic here
            GC.SuppressFinalize(this);
        }
    }
}
