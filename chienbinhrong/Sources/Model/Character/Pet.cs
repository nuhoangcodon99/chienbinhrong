using System;
using System.Linq;
using Linq.Extras;
using NgocRongGold.Application.Constants;
using NgocRongGold.Application.Handlers.Character;
using NgocRongGold.Application.IO;
using NgocRongGold.DatabaseManager;
using NgocRongGold.Model.ModelBase;
using NgocRongGold.Model.Data;
using NgocRongGold.Model.Map;

namespace NgocRongGold.Model.Character
{
    public class Pet : CharacterBase
    {
        public Character Character { get; set; }
        public int PetId { get; set; } //item id
        public long DelayAutoMove { get; set; }

        public Pet(int petId, Character character)
        {
            Id = -(character.Id + 1000);
            PetId = petId;
            Name = "";
            Character = character;
            Player = character.Player;
            Zone = character.Zone;
            InfoChar.OriginalHp = InfoChar.Hp = 100;
            InfoChar.Speed = 5;
            CharacterHandler = new PetHandler(this);
            DelayAutoMove = ServerUtils.CurrentTimeMillis();
        }
        
        public Pet(Zone zone,int petId)
        {
            Id = -(petId + 1000);
            PetId = petId;
            Name = "";
            Character = null;
            Player = null;
            Zone = zone;
            InfoChar.OriginalHp = InfoChar.Hp = 100;
            InfoChar.Speed = 5;
            CharacterHandler = new PetHandler(this);
            DelayAutoMove = ServerUtils.CurrentTimeMillis();
        }
        public Pet()
        {
            PetId = -1;
            Name = "";
            CharacterHandler = new PetHandler(this);
            DelayAutoMove = ServerUtils.CurrentTimeMillis();
        }

        public override short GetHead(bool isMonkey = true)
        {
            return PetId switch
            {
                892 => (short) 882,//Thỏ xám
                893 => (short) 885,//Thỏ trắng
                908 => (short) 891,//Ma phong ba
                909 => (short) 897,//Thần chết cute
                910 => (short) 894,//Bí ngô nhí nhảnh
                916 => (short) 931,//Lính Tam Giác
                917 => (short) 928,//lính vuông
                918 => (short) 925,//lính tròn
                919 => (short) 934,//búp bê
                936 => (short) 718,//tuần lộc nhí
                942 => (short) 966,//hổ mặp vàng
                943 => (short) 969,//hổ mặp trắng
                944 => (short) 972,//hỏ mặp xanh
                967 => (short) 1050,//sao la
                1008 => (short) 1074,//cua đỏ
                1039 => (short) 1089,//Thỏ ốm
                1040 => (short) 1092,//Thỏ mập
                1046 => (short) 95,//Khỉ bong bóng
                1107 =>   (short) 1155, //  Bí Ma Zương
                1114 => (short) 1158, // Phù thủy da zàng
                1188 => (short)1183,
                1202 => (short)1183,
                1203 => (short)1201,
                1230 => (short)1227,
                1231 => (short)1233,
                1232 => (short)1230,
                1250 => (short)1245,
                1251 => (short)1248,
                1292 => (short) 1276,
                1359 => (short)1320,
                1388 => (short) 0x80,
                1455 => (short)1362,
                1476 => (short)0x80,
                1493 => (short)1386,
                1499 => (short)1389,
                1404 => (short)1404,//PIKA
                1542 => (short)1422,
                1527 => (short)1419,
                1599 => (short) 1460,
                1600 => (short)1457,
                _ => (short) 882
            };
        }

        public override short GetBody(bool isMonkey = true)
        {
            return (short)(GetHead() + 1);
        }

        public override short GetLeg(bool isMonkey = true)
        {
            return (short)(GetHead() + 2);
        }
    }
}