using NgocRongGold.Application.Main;
using NgocRongGold.Model.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Handlers.Item
{
    public class ItemBossHandler
    {
        public static void UseBinhNuocXinbato(Model.Character.Character character, Model.Item.Item itemUse)
        {
            if (itemUse.Quantity < 99)
            {
                character.CharacterHandler.SendMessage(Service.ServerMessage("Thu nhập 99 bình nước để đưa cho Xinbato"));
                return;
            }
            character.CharacterHandler.RemoveItemBagByIndex(itemUse.IndexUI, 99);
            character.CharacterHandler.SendMessage(Service.SendBag(character));
            if (character.Zone.Bosses.Count < 0)
            {
                return;
            }
            if (character.Zone.ZoneHandler.GetBossInMap(115).Count < 0)
            {
                return;
            }
            var xinbato = (Model.Character.Boss)character.Zone.ZoneHandler.GetBossInMap(115)[0];
            async void Action()
            {
                xinbato.Zone.ZoneHandler.SendMessage(Service.PublicChat(xinbato.Id, "Cảm ơn " + character.Name + " đã cho tôi 99 bình nước"));
                await Task.Delay(1000);
                xinbato.CharacterHandler.LeaveFromDead();
            }
            var task = new Task(Action);
            task.Start();
        }

        public static void UseCucXuongSoiHecQuyn(Model.Character.Character character, Model.Item.Item itemUse)
        {

            character.CharacterHandler.RemoveItemBagByIndex(itemUse.IndexUI, 1);
            character.CharacterHandler.SendMessage(Service.SendBag(character));
            if (character.Zone.Bosses.Count < 0)
            {
                return;
            }
            if (character.Zone.ZoneHandler.GetBossInMap(116).Count < 0)
            {
                return;
            }
            var xinbato = (Model.Character.Boss)character.Zone.ZoneHandler.GetBossInMap(116)[0];
            async void Action()
            {
                xinbato.Zone.ZoneHandler.SendMessage(Service.PublicChat(xinbato.Id, "Cảm ơn " + character.Name + " đã cho tôi 99 bình nước"));
                await Task.Delay(1000);
                xinbato.CharacterHandler.LeaveFromDead();
            }
            var task = new Task(Action);
            task.Start();
        }
    }
}
