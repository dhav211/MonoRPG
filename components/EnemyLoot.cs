using System.Collections.Generic;

namespace MonoRPG
{
    public class EnemyLoot : Component
    {
        public List<Item> Loot { get; private set; }

        public EnemyLoot(Entity _owner) : base(_owner)
        {
            owner.AddComponent<EnemyLoot>(this);
        }

        public void SetLoot(List<Item> _loot)
        {
            Loot = _loot;
        }

        public void LootEnemy()
        {
            if (Loot.Count > 0)
            {
                TakeLootBox takeLootBox = new TakeLootBox(Loot, owner.entityManager.Inventory, owner);
                EntityCreator.CreateUIEntity<TakeLootBox>(takeLootBox);
            }
        }
    }
}