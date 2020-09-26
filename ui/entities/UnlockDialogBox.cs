using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MonoRPG
{
    public class UnlockDialogBox : UIEntity
    {
        KeyItem keyRequired;
        Inventory inventory;
        ChestComponent chest;

        NineSpliceSprite nineSpliceSprite;
        Label text;
        TextButton unlockButton;
        TextButton closeButton;
        SpriteFont spriteFont;

        bool hasKey;

        // TODO add a new constructor for door, and of course add a variable for the door to unlock

        public UnlockDialogBox(KeyItem _keyRequired, Inventory _inventory, ChestComponent _chest)
        {
            keyRequired = _keyRequired;
            inventory = _inventory;
            chest = _chest;

            Point textBoxSize = new Point(Screen.Width / 4, Screen.Height / 4);
            DestinationRect = new Rectangle((Screen.Width / 2) - (textBoxSize.X / 2), (Screen.Height / 2) - (textBoxSize.Y / 2), textBoxSize.X, textBoxSize.Y);

            hasKey = DoesPlayerHaveKey();

            GameState.OpenMenu();
        }

        // -Create a new type of text box, it will be a new class called UnlockDialogBox. It will require different parameters from the normal text box.
        // -Firstly it will require the type of key required to open the box , a reference of the players inventory, and finally a reference of this chest.
        // -When the dialog box is initialized it does a quick loop thru your key items and checks to see if the required key is there.
        // The message and opitions will be different depending if you have the key or not.
        // if you have the key have it be like this
        /*
        You have the Simple Key.
                Unlock
                Close
        */
        // if you don't have the key it be like this
        /*
        The simple key is required.
                Close
        */
        // When unlock is pressed, check if the key is reusable, if not remove. Then Call the chests Unlock function again and close the dialog box.
        // This box will be used for chests and does, so that last bit will require a if/else statment to see if its a door or a chest.

        public override void Initialize(UIEntityManager _uiEntityManager)
        {
            base.Initialize(_uiEntityManager);

            spriteFont = uiEntityManager.ContentManager.Load<SpriteFont>("fonts/m5x7_16");

            SetNineSpliceSprite();
            SetText();
            SetButtons();
        }

        public override void Draw(float deltaTime)
        {
            nineSpliceSprite.Draw(deltaTime);
        }

        public override void DrawText(float deltaTime)
        {
            text.Draw(deltaTime);
            closeButton.Draw(deltaTime);

            if (hasKey)
                unlockButton.Draw(deltaTime);
        }

        public override void Update(float deltaTime)
        {
            closeButton.Update(deltaTime);

            if (hasKey)
                unlockButton.Update(deltaTime);
        }

        private void SetNineSpliceSprite()
        {
            nineSpliceSprite = new NineSpliceSprite(this);
            nineSpliceSprite.Initialize(uiEntityManager.ContentManager.Load<Texture2D>("ui/9splicesprite"), new Rectangle());
        }

        private void SetText()
        {
            text = new Label(this);
            Rectangle textRect = new Rectangle(8, 8, DestinationRect.Width - 16, DestinationRect.Height / 2);
            text.Initialize(spriteFont, textRect, "", Color.White);

            if (hasKey)
            {
                text.Text = "You have the " + keyRequired.Name;
            }
            else
            {
                text.Text = "The " + keyRequired.Name + " is needed to unlock.";
            }
        }

        private void SetButtons()
        {
            Vector2 closeTextSize = spriteFont.MeasureString("Close");
            closeTextSize /= Screen.Scale;
            Vector2 initialPosition = new Vector2(DestinationRect.Width / 2, DestinationRect.Height - (closeTextSize.Y * 2));
            Vector2 closeButtonPosition = new Vector2(initialPosition.X, initialPosition.Y);

            closeButton = new TextButton(this);
            closeButton.Initialize(spriteFont, "Close", closeButtonPosition, Color.White);
            Action closeAction = Close;
            closeButton.Pressed.Add("unlock_dialog_box", closeAction);

            if (hasKey)
            {
                unlockButton = new TextButton(this);
                unlockButton.Initialize(spriteFont, "Unlock", new Vector2(initialPosition.X, initialPosition.Y - closeTextSize.Y), Color.White);
                Action unlockAction = Unlock;
                unlockButton.Pressed.Add("unlock_dialog_box", unlockAction);
            }
        }

        public void Close()
        {
            GameState.CloseMenu();
            uiEntityManager.RemoveEntity<UnlockDialogBox>(this);
        }

        public void Unlock()
        {
            // Set chest to unlocked, then run chests open command once more.
            // remove key from inventory if not reusable
            // run close function
            Close();

            if (chest != null)
            {
                chest.Unlock();
            }

            if (!keyRequired.IsResuable)
                RemoveKeyFromInventory();
        }

        private bool DoesPlayerHaveKey()
        {
            Type keyRequiredType = keyRequired.GetType();

            foreach (KeyItem keyItem in inventory.KeyItems)
            {
                if (keyRequiredType == keyItem.GetType())
                {
                    return true;
                }
            }

            return false;
        }

        private void RemoveKeyFromInventory()
        {
            Type keyRequiredType = keyRequired.GetType();

            for (int i = 0; i < inventory.KeyItems.Count; ++i)
            {
                if (keyRequiredType == inventory.KeyItems[i].GetType())
                {
                    inventory.RemoveItem(inventory.KeyItems[i]);
                }
            }
        }
    }
}