namespace MonoRPG
{
    public enum EquipPosition { R_HAND, L_HAND, BOTH, TWO_HANDED, HEAD, BODY, LEGS, FEET, ACESSORY }
    public enum EquipmentType { DAGGER, SWORD, GREAT_SWORD, SHIELD, LIGHT_ARMOR, HEAVY_ARMOR, BOOTS, HELMET, RING, BRACELET }

    public class Equipment : Item
    {
        public int HP { get; private set; }
        public int MP { get; private set; }
        public int ATK { get; private set; }
        public int DEF { get; private set; }
        public int INT { get; private set; }
        public int RES { get; private set; }
        public int LUK { get; private set; }
        public int SPD { get; private set; }
        public bool IsEquipped { get; set; }
        public EquipPosition EquipPosition { get; private set; }

        public Equipment()
        {
            Item_Type = ItemType.EQUIPMENT;
        }

        public class RustyDagger : Equipment
        {
            public RustyDagger()
            {
                Name = "Rusty Dagger";
                Description = "It can barely stab and oddly can't even give tetanus. Why do you have this?";
                ATK = 2;
                EquipPosition = EquipPosition.BOTH;
                Weight = 3;
            }
        }

        public class WoodenSword : Equipment
        {
            public WoodenSword()
            {
                Name = "Wooden Sword";
                Description = "A good training sword for a child. Should be able to bash a rat's head in though.";
                ATK = 4;
                EquipPosition = EquipPosition.R_HAND;
            }
        }

        public class PotLid : Equipment
        {
            public PotLid()
            {
                Name = "Pot Lid";
                Description = "It still smells like soup!";
                DEF = 2;
                EquipPosition = EquipPosition.L_HAND;
            }
        }

        public class WoodenShield : Equipment
        {
            public WoodenShield()
            {
                Name = "Wooden Shield";
                Description = "It's better than using a lid, just don't expose it to fire.";
                DEF = 5;
                EquipPosition = EquipPosition.R_HAND;
            }
        }

        public class WalkingShoes : Equipment
        {
            public WalkingShoes()
            {
                Name = "Walking Shoes";
                Description = "Great for a stroll through the market, not so much through a dungeon";
                DEF = 1;
                SPD = 2;
            }
        }

        public class LuckyShoes : Equipment
        {
            public LuckyShoes()
            {
                Name = "Lucky Shoes";
                Description = "Some kid got his first kiss in these shoes, what kind of luck will they bring you?";
                DEF = 3;
                SPD = 3;
                LUK = 10;
            }
        }

        public class SimpleHat : Equipment
        {
            public SimpleHat()
            {
                Name = "Simple Hat";
                Description = "Designed to prevent the wind from runing your hair, pretty simple huh?";
                DEF = 1;
            }
        }

        public class ApprenticeWizardsCap : Equipment
        {
            public ApprenticeWizardsCap()
            {
                Name = "Apprentice Wizards Cap";
                Description = "Just learning the art of magic? Better look the part, that's step one!";
                DEF = 2;
                INT = 3;
                RES = 2;
            }
        }

        public class ClassicTrousers : Equipment
        {
            public ClassicTrousers()
            {
                Name = "Classic Trousers";
                Description = "A snazzy pair of pants your grandfather would wear.";
                DEF = 2;
                SPD = 1;
            }
        }
    }    
}