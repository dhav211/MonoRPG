namespace MonoRPG
{
    public class ConsumableItem : Item
    {
        public ConsumableItem()
        {
            Item_Type = ItemType.CONSUMABLE;
        }

        public int Strength { get; set; }

        public virtual void Use(Entity _entity) { }

        public void ChangeHP(Stats _entityStats, int _amount)
        {
            _entityStats.HP += _amount;
        }

        public void ChangeMP(Stats _entityStats, int _amount)
        {
            _entityStats.MP += _amount;
        }
    }

    public class SmallPotion : ConsumableItem
    {
        public SmallPotion()
        {
            Name = "Small Potion";
            Description = "A bitter potion hardly worth drinking.";
            Strength = 15;
            Weight = 1;
        }

        public override void Use(Entity _entity)
        {
            Stats entityStats = _entity.GetComponent<Stats>();
            ChangeHP(entityStats, Strength);
        }
    }

    public class LargePotion : ConsumableItem
    {
        public LargePotion()
        {
            Name = "Large Potion";
            Description = "Just as bitter, but open your throat and it will flow down quick.";
            Strength = 50;
            Weight = 3;
        }

        public override void Use(Entity _entity)
        {
            Stats entityStats = _entity.GetComponent<Stats>();
            ChangeHP(entityStats, Strength);
        }
    }

    public class MagicHerb : ConsumableItem
    {
        public MagicHerb()
        {
            Name = "Magic Herb";
            Description = "Smells potent, but honestly you won't notice too much of an effect.";
            Strength = 10;
            Weight = .25f;
        }

        public override void Use(Entity _entity)
        {
            Stats entityStats = _entity.GetComponent<Stats>();
            ChangeMP(entityStats, Strength);
        }
    }
}