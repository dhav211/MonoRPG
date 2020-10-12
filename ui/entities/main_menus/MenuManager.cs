using Microsoft.Xna.Framework.Input;
using System;

namespace MonoRPG
{
    public class MenuManager
    {
        //public enum State { }
        public enum Menu { INVENTORY, PLAYER, OBJECTIVES, MAP, NONE }
        public Menu CurrentMenuOpen { get; private set; } = Menu.NONE;
        
        Action close;
        Inventory inventory;
        EntityManager entityManager;
        UIEntityManager uIEntityManager;

        public MenuManager(Inventory _inventory, EntityManager _entityManager, UIEntityManager _uiEntityManager)
        {
            inventory = _inventory;
            entityManager = _entityManager;
            uIEntityManager = _uiEntityManager;
        }

        public void Update()
        {
            if (Input.IsKeyJustPressed(Keys.I) && CurrentMenuOpen != Menu.INVENTORY)
            {
                if (CurrentMenuOpen != Menu.NONE)
                    close();

                CloseEnemyStatPopupMenus();
                CurrentMenuOpen = Menu.INVENTORY;
                InventoryMenu inventoryMenu = new InventoryMenu(inventory, entityManager);
                EntityCreator.CreateUIEntity<InventoryMenu>(inventoryMenu);
                close = inventoryMenu.Close;
            }
            else if (Input.IsKeyJustPressed(Keys.I) && CurrentMenuOpen == Menu.INVENTORY)
            {
                CurrentMenuOpen = Menu.NONE;
                close();
            }
        }

        private void CloseEnemyStatPopupMenus()
        {
            for(int i = 0; i < uIEntityManager.Entities.Count; ++i)
            {
                if (uIEntityManager.Entities[i] is EnemyStatPopup)
                {
                    uIEntityManager.RemoveEntity<UIEntity>(uIEntityManager.Entities[i]);
                }
            }
        }
    }
}