using NgocRongGold.Application.Handlers.Character;
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
    public class DirtyMineHp : IEffectTemporary
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public long EffectiveTime { get; set; }
        public bool isEffective { get; set; }
        public Model.Character.Character Character { get; set; }
        public int Param { get; set; }
        public Model.Character.Character CharacterTemp { get; set; }
        public DirtyMineHp(Model.Character.Character character, Model.Character.Character CharacterMakeEffect, int param)
        {
            ID = (int)EffectTemporaryTemplate.DIRTY;
            Param = param;
            Name = $"{CharacterMakeEffect.Name}: Ở dơ, làm mất {param}% HP mọi người ở gần";
            Character = character;
            CharacterTemp = CharacterMakeEffect;
            EffectiveTime = 10000 + ServerUtils.CurrentTimeMillis();
            isEffective = true;
            Setup();
            Character.CharacterHandler.SendZoneMessage(Service.PublicChat(Character.Id, "Hôi quá, tránh xa ta ra"));
        }

        public void Setup()
        {
            Character.CharacterHandler.SendMessage(Service.ItemTimeWithMessage(Name, ID, 10));
            Character.CharacterHandler.MineHp(Character.HpFull * Param / 100);
            Character.CharacterHandler.SendZoneMessage(Service.PlayerLevel(Character));
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
            
            Character.EffectTemporaries.Remove(this);
        }
        public void Dispose()
        {
            // Implement disposal logic here
            GC.SuppressFinalize(this);
        }
    }
}
