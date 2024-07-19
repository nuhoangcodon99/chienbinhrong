using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Threading;
using NgocRongGold.Model.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.Dragon
{
    public class Shenlong : IDragon
    {
        public Character Character { get; set; }
        public string Info { get; set; }
        public long Delay { get; set; } = 0;
        public long TimeDisappearing { get; set; }
        public long TimeAppearing { get; set; }
        public bool isApprearing { get; set; } = false;
        public Shenlong(Character character)
        {
            Character = character;
        }
        public bool ConditionWish()
        {
            return !isApprearing && Delay < ServerUtils.CurrentTimeMillis();
        }
        public void Wish()
        {
            var timeServer = ServerUtils.CurrentTimeMillis();
            Delay = ConstDragon.ConstDelayShenlong + timeServer;
            isApprearing = true;
            TimeAppearing = timeServer;
            TimeDisappearing = ConstDragon.ConstDisappearingShenlong + timeServer; 
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public void Disappearing()
        {
            Character.CharacterHandler.SendZoneMessage(Service.CallDragon(1, 0, Character));
            Character.CharacterHandler.SendMessage(Service.OpenUiConfirm(5, "Ta buồn ngủ quá rồi\nHẹn gặp ngươi lần sau, ta đi đây, bái bai", new List<string>(), Character.InfoChar.Gender));
          //  Dispose();
        }
    }
    public class ShenlongDragon
    {
        public Dictionary<int, Shenlong> Shenlongs { get; set; } = new Dictionary<int, Shenlong>();
        public static ShenlongDragon instance;
        public Thread ThreadShenronDragon { get; set; }

        public void StartUpdate()
        {
            ThreadShenronDragon = new Thread(new ThreadStart(() =>
            {
                while (Server.Gi().IsRunning)
                {
                    Update();
                    Thread.Sleep(1000);
                }
            }));
            ThreadShenronDragon.Start();
        }
        public static ShenlongDragon gI()
        {
            if (instance == null) instance = new ShenlongDragon();
            return instance;
        }
        public void WishFinish(int id)
        {
            var timeServer = ServerUtils.CurrentTimeMillis();
            var shenlong = GetShenlong(id);
            shenlong.Delay = ConstDragon.ConstDelayShenlong + timeServer;
          //  shenlong.Disappearing();
        }
        public void Update()
        {
            var temp = Shenlongs.Values.ToList();
            var timeServer = ServerUtils.CurrentTimeMillis();
            for (int i = 0; i < temp.Count; i++)
            {
                var shenlong = temp[i];
                
                if (shenlong.isApprearing && shenlong.TimeDisappearing < timeServer)
                {
                    //handle shenlong disappearing
                    Remove(shenlong.Character.Id);
                    shenlong.Disappearing();
                    
                }
            }
        }
       
        public Shenlong GetShenlong(int key)
        {
            if (Shenlongs.TryGetValue(key, out var shenlong))
            {
                return shenlong;
            }
            return null;
        }
        public void Add(Character character)
        {
            var shenlong = GetShenlong(character.Id);
            if (shenlong == null)
            {
                shenlong = new Shenlong(character);
                if (Shenlongs.TryAdd(character.Id, shenlong))
                {
                   // shenlong.Wish();
                }
            }
        }
        public void Remove(int key)
        {
            Shenlongs.Remove(key);
        }
    }
}
