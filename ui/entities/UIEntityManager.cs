using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MonoRPG
{
    public class UIEntityManager
    {
        public List<UIEntity> Entities { get; private set; } = new List<UIEntity>();
        private List<UIEntity> scrollableEntities = new List<UIEntity>();
        private List<UIEntity> nonscrollableEntities = new List<UIEntity>();
        public SpriteBatch SpriteBatch { get; private set; }
        public ContentManager ContentManager { get; private set; }

        public UIEntityManager(SpriteBatch _spriteBatch, ContentManager _contentManager) 
        {
            SpriteBatch = _spriteBatch;
            ContentManager = _contentManager;
        }

        ///<summary>
        /// Adds selected entity to list, also seperates it into the seperate scrollable and nonscrollable lists.
        ///</summary>
        public void AddEntity<T>(T _uiEntity) where T : UIEntity
        {
            _uiEntity.Initialize(this);
            Entities.Add(_uiEntity);

            if (_uiEntity.IsScrollable)
                scrollableEntities.Add(_uiEntity);
            else
                nonscrollableEntities.Add(_uiEntity);
        }

        ///<summary>
        /// Removes ui entity from all involved lists.
        ///</summary>
        public void RemoveEntity<T>(T _uiEntity) where T : UIEntity
        {
            Entities.Remove(_uiEntity);

            if (_uiEntity.IsScrollable)
                scrollableEntities.Remove(_uiEntity);
            else
                nonscrollableEntities.Remove(_uiEntity);
        }

        public void Update(float deltaTime) 
        {
            for (int i = 0; i < Entities.Count; ++i)
            {
                Entities[i].Update(deltaTime);
            }
        }
        public void Draw(float deltaTime, bool _isScrollable) 
        {
            if (_isScrollable)
            {
                for (int i = 0; i < scrollableEntities.Count; ++i)
                {
                    scrollableEntities[i].Draw(deltaTime);
                }
            }
            else
            {
                for (int i = 0; i < nonscrollableEntities.Count; ++i)
                {
                    nonscrollableEntities[i].Draw(deltaTime);
                }
            }
        }
        public void DrawText(float deltaTime, bool _isScrollable) 
        {
            if (_isScrollable)
            {
                for (int i = 0; i < scrollableEntities.Count; ++i)
                {
                    scrollableEntities[i].DrawText(deltaTime);
                }
            }
            else
            {
                for (int i = 0; i < nonscrollableEntities.Count; ++i)
                {
                    nonscrollableEntities[i].DrawText(deltaTime);
                }
            }
        }
    }
}