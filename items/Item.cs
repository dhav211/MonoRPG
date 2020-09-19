using Microsoft.Xna.Framework.Graphics;

namespace MonoRPG
{
    public class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Texture2D Texture { get; set; }
        public float Weight { get; set; }

        public enum ItemType { CONSUMABLE, EQUIPMENT, KEY, BATTLE}
        public ItemType Item_Type { get; set; }

        public void Use() { }
    }
}