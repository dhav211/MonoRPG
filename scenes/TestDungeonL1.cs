using System.Collections.Generic;

namespace MonoRPG
{
    public class TestDungeonL1 : Scene
    {
        public TestDungeonL1()
        {
            Name = "TestDungeonL1";
            LevelAddress = "levels/test_dungeon.json";
            TilesetAddress = "tilesets/temp_tileset";
        }

        public override void SetChests()
        {
            List<ChestComponent> chestComponents = entityManager.GetComponents<ChestComponent>();

            List<Item> chest1Items = new List<Item>();
            List<Item> chest2Items = new List<Item>();

            chest1Items.Add(new SmallPotion());
            chest1Items.Add(new SmallPotion());
            chest1Items.Add(new SmallPotion());
            chest1Items.Add(new MagicHerb());

            foreach (ChestComponent chest in chestComponents)
            {
                switch(chest.ChestID)
                {
                    case 0:
                    {
                        chest.SetItemsInside(chest1Items);
                        break;
                    }
                }
            }
        }
    }
}