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
using NgocRongGold.Application.Main;
using Application.Interfaces.Zone;
using System.ComponentModel.DataAnnotations;

namespace NgocRongGold.Model.Character
{
    public class Pet2 : CharacterBase
    {
        public int PetId { get; set; } //item id
        public long DelayAutoMove { get; set; }
        public long DelayDiTheo { get; set; }
        public int IdMap { get; set; }
        public Character Character { get; set; }

        public Pet2(IZone zone,int petId, string NamePet = "")
        {
            Character = null;
            PetId = petId;
            Name = NamePet;
            Player = null;
            Zone = zone;
            IdMap = zone.Pets2.Count;
            Id = -(petId * 10 + IdMap);
            InfoChar.OriginalHp = InfoChar.Hp = 100;
            Character = null;
            InfoChar.Speed = 5;
            CharacterHandler = new Pet2Handler(this);
            DelayAutoMove = DelayDiTheo = ServerUtils.CurrentTimeMillis();
            SetPosistion();
        }
        public Pet2()
        {
            PetId = -1;
            Name = "";
            CharacterHandler = new Pet2Handler(this);
            DelayAutoMove = DelayDiTheo = ServerUtils.CurrentTimeMillis();
        }
        public void Reset()
        {
            Character = null;
            Zone.ZoneHandler.SendMessage(Service.PlayerAdd(this, "[Event]"));
        }
        public void ChangePosistion(short x, short y)
        {
            InfoChar.X = x;
            InfoChar.Y = y;

        }
        public void SetPosistion()
        {
            switch (Zone.Map.Id)
            {
                case 0:
                    ChangePosistion((short)ServerUtils.RandomNumber(402, 1054), 432);
                    break;
                case 1:
                    ChangePosistion((short)ServerUtils.RandomNumber(168, 1377), 360);
                    break;
                case 2:
                    ChangePosistion((short)ServerUtils.RandomNumber(734, 1054), 208);
                    break;
                case 3:
                    ChangePosistion((short)ServerUtils.RandomNumber(170, 674), 408);
                    break;
                case 4:
                    ChangePosistion(216, 312);

                    break;
                case 5:
                    ChangePosistion((short)ServerUtils.RandomNumber(965, 1208), 408);
                    break;
                case 27:
                    ChangePosistion((short)ServerUtils.RandomNumber(538, 1087), 336);
                    break;
                case 28:
                    ChangePosistion((short)ServerUtils.RandomNumber(466, 1087), 264);
                    break;
                case 29:
                    ChangePosistion(981, 360);
                    break;
                case 30:
                    ChangePosistion(475, 288);
                    break;
               
            }
         

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
                1207 => (short)1077,
                1230 => (short)1227,
                1231 => (short)1233,
                1232 => (short)1230,
                1250 => (short)1245,
                1251 => (short)1248,
                1291 => (short) 1276,
                763 => (short) 763,
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