namespace MonoRPG
{
    public class ConsumableItem : Item
    {
        public ConsumableItem()
        {
            Item_Type = ItemType.CONSUMABLE;
        }

        public int Strength { get; set; }

        public virtual void Use() { }

        public void ChangeHP(Entity _entity, int _amount)
        {
            Stats stats = _entity.GetComponent<Stats>() as Stats;
            stats.HP += _amount;
        }
    }

    public class SmallPotion : ConsumableItem
    {
        public SmallPotion()
        {
            Name = "Small Potion";
            Description = "A bitter potion hardly worth drinking.";
            Strength = 15;
        }
    }

    public class LargePotion : ConsumableItem
    {
        public LargePotion()
        {
            Name = "Large Potion";
            Description = "Just as bitter, but open your throat and it will flow down quick.";
            Strength = 50;
        }
    }

    public class MagicHerb : ConsumableItem
    {
        public MagicHerb()
        {
            Name = "Magic Herb";
            Description = "Smells potent, but honestly you won't notice too much of an effect.";
            Strength = 10;
        }
    }
}