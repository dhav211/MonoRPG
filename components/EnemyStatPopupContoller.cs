using Microsoft.Xna.Framework;
using System;

namespace MonoRPG
{
    public class EnemyStatPopupController : Component
    {
        EnemyStatPopup enemyStatPopup = null;
        Stats stats;
        Transform transform;

        Action _onMouseEntered;
        Action _onMouseExited;
        Action _onPressed;

        public EnemyStatPopupController (Entity _owner) : base(_owner)
        {
            owner.AddComponent<EnemyStatPopupController>(this);

            _onMouseEntered = onMouseEntered;
            _onMouseExited = onMouseExited;
            _onPressed = onPressed;

            owner.MouseEntered.Add("enemy_stat_popup_controller", _onMouseEntered);
            owner.MouseExited.Add("enemy_stat_popup_controller", _onMouseExited);
            owner.MousePressed.Add("enemy_stat_popup_controller", _onPressed);
        }

        public override void Initialize()
        {
            stats = owner.GetComponent<Stats>() as Stats;
            transform = owner.GetComponent<Transform>() as Transform;
        }

        private void Open()
        {
            if (!owner.IsAlive)
                return;
            
            if (enemyStatPopup == null)
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
    }
}