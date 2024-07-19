using NgocRongGold.Application.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.NamecBattlefield
{
    partial class NamecBattlefield_Service
    {
        public static Message newInfoPhuBan(short idMap, string nameTeam, string nameTeam2, int maxPoint, short second, byte maxLife)
        {
            var msg = new Message(20);
            msg.Writer.WriteByte(0);
            msg.Writer.WriteByte(0);
            msg.Writer.WriteShort(idMap);
            msg.Writer.WriteUTF(nameTeam);
            msg.Writer.WriteUTF(nameTeam2);
            msg.Writer.WriteInt(maxPoint);
            msg.Writer.WriteShort(second);
            msg.Writer.WriteByte(maxLife);
            return msg;
        }
        public static Message updatePoint(int pointTeam1, int pointTeam2)
        {
            var msg = new Message(20);
            msg.Writer.WriteByte(0);
            msg.Writer.WriteByte(1);
            msg.Writer.WriteInt(pointTeam1);
            msg.Writer.WriteInt(pointTeam2);
            return msg;
        }
        public static Message AddEffectEnd(int type)
        {
            var msg = new Message(20);
            msg.Writer.WriteByte(0);
            msg.Writer.WriteByte(2);
            msg.Writer.WriteByte(type);
            return msg;
        }
        public static Message updateTime(short second)
        {
            var msg = new Message(20);
            msg.Writer.WriteByte(0);
            msg.Writer.WriteByte(5);
            msg.Writer.WriteShort(second);
            return msg;
        }
        public static Message updateLife(byte lifeTeam1, byte lifeTeam2)
        {
            var msg = new Message(20);
            msg.Writer.WriteByte(0);
            msg.Writer.WriteByte(4);
            msg.Writer.WriteByte(lifeTeam1);
            msg.Writer.WriteByte(lifeTeam2);
            return msg;
        }
    }
}
