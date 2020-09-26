namespace MonoRPG
{
    public class KeyItem : Item
    {
        public bool IsResuable { get; set; } = false;
        public KeyItem()
        {
            Item_Type = ItemType.KEY;
        }

    }
    public class SimpleKey : KeyItem
    {
        public SimpleKey()
        {
            Name = "Simple Key";
            Description = "A simple key for a simple lock, locked by a simple person";
            Weight = 0.1f;
            IsResuable = false;
        }
    }

    public class SkeletonKey : KeyItem
    {
        public SkeletonKey()
        {
            Name = "Skeleton Key";
            Description = "It gives you the shivers just holding it, do you really want to see what it opens?";
            Weight = 0.2f;
            IsResuable = true;
        }
    }
}