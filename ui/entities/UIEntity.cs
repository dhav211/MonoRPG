using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRPG
{
    public class UIEntity
    {
        protected UIEntityManager uiEntityManager;
        public Rectangle DestinationRect { get; set; }
        public bool IsScrollable { get; set; } = false;  // this bool affects with it moves with the camera or stays static on the screen
        public SpriteBatch SpriteBatch { get; private set; }

        public UIEntity(bool _isScrollable = false)
        {
            IsScrollable = _isScrollable;
        }

        public virtual void Update(float deltaTime) { }
        public virtual void Initialize(UIEntityManager _uiEntityManager) 
        { 
            uiEntityManager = _uiEntityManager;
            SpriteBatch = uiEntityManager.SpriteBatch;
        }
        public virtual void Draw(float deltaTime) { }
        public virtual void DrawText(float deltaTime) { }
        public void Destroy()
        {
            uiEntityManager.RemoveEntity<UIEntity>(this);
        }
    }
}