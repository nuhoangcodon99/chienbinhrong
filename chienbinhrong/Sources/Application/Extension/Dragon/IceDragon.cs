using NgocRongGold.Application.Constants;
using NgocRongGold.Application.IO;
using NgocRongGold.Application.Main;
using NgocRongGold.Application.Manager;
using NgocRongGold.DatabaseManager;
using NgocRongGold.Model.Character;
using NgocRongGold.Model.SkillCharacter;
using NgocRongGold.Model.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgocRongGold.Application.Extension.Dragon
{
    public class IceDragon
    {
        public static IceDragon instance;
        public static IceDragon gI()
        {
            if (instance == null)
            {
                instance = new IceDragon();
            }
            return instance;
        }
        public string textWish = "Ta sẽ ban cho ngươi một điều ước, ngươi có 5 phút, hãy chọn đi:\n" +
            "1)Đổi kỹ năng 3 và 4 của đệ tử\n(Lưu ý:Kỹ năng mới có cấp 1 và vẫn có thể trùng lại với kỹ năng vốn có).\n" +
            "2)Thay đổi nội tại\n" +
            "3)Cải trang siêu thần hạn dùng 90 ngày\n" +
            "4)Cải trang Black Gohan Rose hạn dùng 90 ngày";
            
        public List<string> textMenuWish = new List<string> { "Điều\nước 1","Điều\nước 2", "Điều\nước 3","Điều\nước 4" };
        public void OpenMenuWish(Character character)
        {
            for (int dball = 925; dball <= 931; dball++)
            {
                if (character.CharacterHandler.GetItemBagById(dball) == null)
                {
                    character.CharacterHandler.SendMessage(Service.ServerMessage(TextServer.gI().NOT_GENDER));
                    return;
                }
            }
            for (short dball = 925; dball <= 931; dball++)
            {
                character.CharacterHandler.RemoveItemBagById(dball, 1, reason: "Gọi rồng");
            }
            MapManager.SetDragonAppeared(true);
            character.CharacterHandler.SendMessage(Service.SendBag(character));
            character.CharacterHandler.SendZoneMessage(Service.CallDragon(character, 3));
            character.CharacterHandler.SendMessage(Service.OpenUiConfirm(24, textWish, textMenuWish, 3));
        }
        public void Wish(Character character, int select)
        {
             switch (select)
            {
                case 0:
                    
                    var randomSkill = DataCache.IdSkillDisciple3[ServerUtils.RandomNumber(DataCache.IdSkillDisciple3.Count)];
                    if (character.Disciple.Skills.Count >= 3)
                    character.Disciple.Skills.Add(new SkillCharacter()
                    {
                        Id = randomSkill,
                        SkillId = Disciple.GetSkillId(randomSkill),
                        Point = 1,
                    });
                    if (character.Disciple.Skills.Count >= 4)
                    {
                        randomSkill = DataCache.IdSkillDisciple4[ServerUtils.RandomNumber(DataCache.IdSkillDisciple4.Count)];
                        character.Disciple.Skills.Add(new SkillCharacter()
                        {
                            Id = randomSkill,
                            SkillId = Disciple.GetSkillId(randomSkill),
                            Point = 1,
                        });
                    }
                    break;
                case 1:
                    var specialSkillTemplate = Cache.Gi().SPECIAL_SKILL_TEMPLATES.FirstOrDefault(s => s.Key == character.InfoChar.Gender).Value;
                    if (specialSkillTemplate == null) return;
                    int RandomIndex = ServerUtils.RandomNumber(specialSkillTemplate.Count);
                    SpecialSkillTemplate SkillRandom = specialSkillTemplate[RandomIndex];

                    int ValueRandom = 0;


                    ValueRandom = ServerUtils.RandomNumber(SkillRandom.Min + 10, SkillRandom.Max + 1);

                    string InfoRandom = SkillRandom.InfoFormat.Replace("#", ValueRandom + "");

                    character.SpecialSkill.Id = SkillRandom.Id;
                    character.SpecialSkill.Info = InfoRandom;
                    character.SpecialSkill.Img = SkillRandom.Img;
                    character.SpecialSkill.SkillId = SkillRandom.SkillId;
                    character.SpecialSkill.Value = ValueRandom;
                    character.CharacterHandler.SendMessage(Service.ServerMessage("Bạn đã mở nội tại " + InfoRandom));
                    character.CharacterHandler.SendMessage(Service.SpeacialSkill(character, 0));
                    character.CharacterHandler.SendMessage(Service.MeLoadInfo(character));
                    break; 
                case 2: // sieu than 90 days
                    var SieuThan = ItemCache.GetItemDefault(905);
                    if (character.InfoChar.Gender == 1)
                    {
                        SieuThan = ItemCache.GetItemDefault(907);
                    }
                    if (character.InfoChar.Gender == 2)
                    {
                        SieuThan = ItemCache.GetItemDefault(911);
                    }
                    SieuThan.Options.Add(new Model.Option.OptionItem()
                    {
                        Id = 93,
                        Param = 90
                    });
                    character.CharacterHandler.AddItemToBag(false, SieuThan, "Uoc Rong Bang");
                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                    character.CharacterHandler.SendMessage(Service.ServerMessage("Chúc mừng bạn đã nhận được " + ItemCache.ItemTemplate(SieuThan.Id).Name));
                    break;
                case 3: // black gohan rose
                    var Black = ItemCache.GetItemDefault(883);
                    Black.Options.Add(new Model.Option.OptionItem()
                    {
                        Id = 93,
                        Param = 90
                    });
                    character.CharacterHandler.AddItemToBag(false, Black, "Uoc Rong Bang");
                    character.CharacterHandler.SendMessage(Service.SendBag(character));
                    character.CharacterHandler.SendMessage(Service.ServerMessage("Chúc mừng bạn đã nhận được " + ItemCache.ItemTemplate(Black.Id).Name));
                    break;
            }
            character.CharacterHandler.SendMessage(Service.CallDragon(1, 0, character));
            MapManager.SetDragonAppeared(false);
        }
    }
}
