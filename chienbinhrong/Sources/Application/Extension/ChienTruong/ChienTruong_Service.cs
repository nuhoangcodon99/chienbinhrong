using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.ChienTruong
{
    public class ChienTruong_Service
    {
        /*
         * else if ((int)b == 5)
		{
			string text = "\n|ChienTruong|Log: ";
			sbyte b3 = msg.reader().readByte();
			if ((int)b3 == 0)
			{
				GameScr.nCT_team = msg.reader().readUTF();
				GameScr.nCT_TeamA = (GameScr.nCT_TeamB = (int)msg.reader().readByte());
				GameScr.nCT_nBoyBaller = GameScr.nCT_TeamA * 2;
				GameScr.isPaint_CT = false;
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"\tsub    0|  nCT_team= ",
					GameScr.nCT_team,
					"|nCT_TeamA =",
					GameScr.nCT_TeamA,
					"  isPaint_CT=false \n"
				});
			}
			else if ((int)b3 == 1)
			{
				int num4 = msg.reader().readInt();
				sbyte b4 = msg.reader().readByte();
				GameScr.nCT_floor = b4;
				GameScr.nCT_timeBallte = (long)(num4 * 1000) + mSystem.currentTimeMillis();
				GameScr.isPaint_CT = true;
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"\tsub    1 floor= ",
					b4,
					"|timeBallte= ",
					num4,
					"isPaint_CT=true \n"
				});
			}
			else if ((int)b3 == 2)
			{
				GameScr.nCT_TeamA = (int)msg.reader().readByte();
				GameScr.nCT_TeamB = (int)msg.reader().readByte();
				GameScr.res_CT.removeAllElements();
				sbyte b5 = msg.reader().readByte();
				for (int i = 0; i < (int)b5; i++)
				{
					string text3 = string.Empty;
					text3 = text3 + msg.reader().readByte() + "|";
					text3 = text3 + msg.reader().readUTF() + "|";
					text3 = text3 + msg.reader().readShort() + "|";
					text3 += msg.reader().readInt();
					GameScr.res_CT.addElement(text3);
				}
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"\tsub   2|  A= ",
					GameScr.nCT_TeamA,
					"|B =",
					GameScr.nCT_TeamB,
					"  isPaint_CT=true \n"
				});
			}
			else if ((int)b3 == 3)
			{
				Service.gI().sendCT_ready(b, b3);
				GameScr.nCT_floor = 0;
				GameScr.nCT_timeBallte = 0L;
				GameScr.isPaint_CT = false;
				text += "\tsub    3|  isPaint_CT=false \n";
			}
			else if ((int)b3 == 4)
			{
				GameScr.nUSER_CT = (int)msg.reader().readByte();
				GameScr.nUSER_MAX_CT = (int)msg.reader().readByte();
			}
			text += "END LOG CT.";
			Res.err(text);
        */
    }
}
