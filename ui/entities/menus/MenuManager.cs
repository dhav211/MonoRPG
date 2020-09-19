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

        public MenuManager(Inventory _inventory, EntityManager _entityManager)
        {
            inventory = _inventory;
            entityManager = _entityManager;
        }

        public void Update()
        {
            if (Input.IsKeyJustPressed(Keys.I) && CurrentMenuOpen != Menu.INVENTORY)
            {
                if (CurrentMenuOpen != Menu.NONE)
                    close();

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
    }
}