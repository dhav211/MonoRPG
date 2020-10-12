using Microsoft.Xna.Framework;
using System;

namespace MonoRPG
{
    public class EnemyStatPopupController : Component
    {
        EnemyStatPopup enemyStatPopup = null;
        Stats stats;
        Transform transform;
        TakeDamage takeDamage;

        Action _onMouseEntered;
        Action _onMouseExited;
        Action _onPressed;
        Action _onTookDamage;

        public EnemyStatPopupController (Entity _owner) : base(_owner)
        {
            owner.AddComponent<EnemyStatPopupController>(this);

            _onMouseEntered = onMouseEntered;
            _onMouseExited = onMouseExited;
            _onPressed = onPressed;
            _onTookDamage = onTookDamage;

            owner.MouseEntered.Add("enemy_stat_popup_controller", _onMouseEntered);
            owner.MouseExited.Add("enemy_stat_popup_controller", _onMouseExited);
            owner.MousePressed.Add("enemy_stat_popup_controller", _onPressed);
        }

        public override void Initialize()
        {
            stats = owner.GetComponent<Stats>();
            transform = owner.GetComponent<Transform>();
            takeDamage = owner.GetComponent<TakeDamage>();

            takeDamage.TookDamage.Add("enemy_stat_popup_controller", _onTookDamage);
        }

        private void Open()
        {
            if (!owner.IsAlive)
                return;
            
            // Menu can only pop up if one doesn't exist and the game state allows it
            if (enemyStatPopup == null && GameState.CanPlayerMove())
            {
                enemyStatPopup = new EnemyStatPopup(transform.Position, stats);
                EntityCreator.CreateUIEntity<EnemyStatPopup>(enemyStatPopup);
            }
        }

        private void Close()
        {
            if (enemyStatPopup != null)
            {
                enemyStatPopup.Destroy();
                enemyStatPopup = null;
            }
        }

        public void onMouseEntered()
        {
            // check to see if pop is null
            // if yes then pop it up
            Open();
        }

        public void onMouseExited()
        {
            // check to see if pop is not null
            // if yes then remove it
            Close();
        }

        public void onPressed()
        {
            Close();
        }

        public void onTookDamage()
        {
            if (enemyStatPopup == null)
                return;
            
            enemyStatPopup.SetCurrentHP(stats.HP);
        }
    }
}