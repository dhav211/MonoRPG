using System.Collections.Generic;
using System;

namespace MonoRPG
{
    public class Inventory
    {
        public List<Item> Items { get; private set; } = new List<Item>();
        public List<ConsumableItem> ConsumableItems { get; private set; } = new List<ConsumableItem>();
        public List<Equipment> Equipment { get; private set; } = new List<Equipment>();
        public List<KeyItem> KeyItems { get; private set; } = new List<KeyItem>();
        public List<BattleItem> BattleItems { get; private set; } = new List<BattleItem>();

        public void AddItem(Item _item)
        {
            Items.Add(_item);

            switch(_item.Item_Type)
            {
                case Item.ItemType.CONSUMABLE:
                {
                    ConsumableItems.Add(_item as ConsumableItem);
                    break;
                }
                case Item.ItemType.EQUIPMENT:
                {
                    Equipment.Add(_item as Equipment);
                    break;
                }
                case Item.ItemType.KEY:
                {
                    KeyItems.Add(_item as KeyItem);
                    break;
                }
                case Item.ItemType.BATTLE:
                {
                    BattleItems.Add(_item as BattleItem);
                    break;
                }
            }
        }

        public void RemoveItem(Item _item)
        {
            Items.Remove(_item);

            switch(_item.Item_Type)
            {
                case Item.ItemType.CONSUMABLE:
                {
                    ConsumableItems.Remove(_item as ConsumableItem);
                    break;
                }
                case Item.ItemType.EQUIPMENT:
                {
                    Equipment.Remove(_item as Equipment);
                    break;
                }
                case Item.ItemType.KEY:
                {
                    KeyItems.Remove(_item as KeyItem);
                    break;
                }
                case Item.ItemType.BATTLE:
                {
                    BattleItems.Remove(_item as BattleItem);
                    break;
                }
            }
        }
    }
}