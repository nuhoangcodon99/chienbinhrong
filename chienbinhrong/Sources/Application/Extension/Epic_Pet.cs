using NgocRongGold.Application.IO;
using NgocRongGold.DatabaseManager;
using NgocRongGold.Model.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Threading;

namespace NgocRongGold.Application.Extension
{
    public class Epic_Pet
    {
        public int Id { get; set; }
        public short IdImage { get; set; }
        public int Frame { get; set; }
        public int hImg { get; set; }
        public int xImg { get; set; }
        public Epic_Pet()
        {

        }
        public static Message Call_Role(Character character)
        {
            //var msg = new Message(31);
            //msg.Writer.WriteInt(character.Id);
            //msg.Writer.WriteByte(2);
            //msg.Writer.WriteByte(0);
            //msg.Writer.WriteInt(character.InfoChar.Roles.Count);
            //character.InfoChar.Roles.ForEach(role =>
            //{
            //    msg.Writer.WriteShort(role.Small);
            //    msg.Writer.WriteInt(role.Frame);
            //    msg.Writer.WriteShort(role.Dx);
            //    msg.Writer.WriteShort(role.Dy);
            //});
           
            return null;
        }
        public static Message Clear_Role(Character character)
        {
            var msg = new Message(31);
            msg.Writer.WriteInt(character.Id);
            msg.Writer.WriteByte(2);
            msg.Writer.WriteByte(1);
            
            return msg;
        }
        public static Message Call_ListRole(Character character, params int[] idImage)
        {
            var msg = new Message(31);
            msg.Writer.WriteInt(character.Id);
            msg.Writer.WriteByte(3);// type
            msg.Writer.WriteInt(idImage.Length);
            for (int i = 0; i < idImage.Length; i++)
            {
                msg.Writer.WriteShort(idImage[i]);//id icon
                msg.Writer.WriteInt(6);// frame
            }
            return msg;
        }
        public static Message Call_EpicPet(Character nhân_vật, int idImage, int frame)
        {
            var msg = new Message(31);
            msg.Writer.WriteInt(nhân_vật.Id); // char id
            msg.Writer.WriteByte(1); // 0 la remove 1 la sed
            msg.Writer.WriteShort(idImage); // id image pet folow
            msg.Writer.WriteByte(1); // 0 la frame 3 mac dinh
            if (frame > 0)
            {
                msg.Writer.WriteByte(frame); // so frame
                for (int i = 0; i < frame; i++)
                {
                    msg.Writer.WriteByte(i);
                }
                var cache = Cache.Gi().LinhThu.Values.FirstOrDefault(i => i.IdImage == idImage);
                //msg.Writer.WriteShort(idImage == 15067 ? 65 : 75); // do dai img
                //msg.Writer.WriteShort(idImage == 15067 ? 65 : 75); // do rong img
                msg.Writer.WriteShort(cache.hImg);
                msg.Writer.WriteShort(cache.xImg);
            }
            
            return msg;
        }
        public static Message Remove_EpicPet(Character nhân_vật)
        {
            var msg = new Message(31);
            msg.Writer.WriteInt(nhân_vật.Id); // char id
            msg.Writer.WriteByte(0); // 0 la remove 1 la sed
            return msg;
        }
    }
}
