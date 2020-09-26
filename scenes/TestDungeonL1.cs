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
            chest1Items.Add(new SimpleKey());

            chest2Items.Add(new LargePotion());

            foreach (ChestComponent chest in chestComponents)
            {
                switch(chest.ChestID)
                {
                    case 0:
                    {
                        chest.SetItemsInside(chest1Items);
                        break;
                    }

                    case 1:
                    {
                        chest.SetItemsInside(chest2Items);
                        break;
                    }
                }
            }
        }

        public override void SetEnemies()
        {
            List<Enemy> enemies = entityManager.GetEntitiesOfType<Enemy>();

            List<Item> enemy0Items = new List<Item>();
            enemy0Items.Add(new SmallPotion());

            List<Item> enemy1Items = new List<Item>();
            enemy1Items.Add(new LargePotion());
            enemy1Items.Add(new MagicHerb());

            foreach (Enemy enemy in enemies)
            {
                EnemyLoot enemyLoot = enemy.GetComponent<EnemyLoot>();

                switch(enemy.EnemyID)
                {
                    case 0:
                        enemyLoot.SetLoot(enemy0Items);
                        break;
                    
                    case 1:
                        enemyLoot.SetLoot(enemy1Items);
                        break;

                    default:
                        break;
                }
            }
        }
    }
}