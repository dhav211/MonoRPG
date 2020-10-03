using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRPG
{
    public class EntityManager
    {
        List<Entity> entities = new List<Entity>();

        public ContentManager ContentManager { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }
        public MapGrid Grid { get; private set; }
        public Camera Camera { get; private set; }
        public TurnManager TurnManager { get; private set; }
        public Inventory Inventory { get; private set; }

        public EntityManager(ContentManager _content, SpriteBatch _spriteBatch, MapGrid _grid, Camera _camera, TurnManager _turnManager, Inventory _inventory)
        {
            ContentManager = _content;
            SpriteBatch = _spriteBatch;
            Grid = _grid;
            Camera = _camera;
            TurnManager = _turnManager;
            Inventory = _inventory;
        }

        public void Update(float deltaTime)
        {
            for (int i = 0; i < entities.Count; ++i)
            {
                entities[i].Update(deltaTime);
            }
        }

        public void Draw(float deltaTime)
        {
            for (int i = 0; i < entities.Count; ++i)
            {
                entities[i].Draw(deltaTime);
            }
        }

        public void RunPostInitialzation()
        {
            foreach (Entity entity in entities)
            {
                entity.PostInitialize();
                foreach (Component component in entity.Components)
                {
                    component.PostInitialize();
                }
            }
        }

        ///<summary>
        /// Adds selected entity to scene and to entity list
        ///</summary>
        public void AddEntity<E>(E _entity, Vector2 _position = new Vector2(), Level.LevelEntityValues _entityValues = null) where E : Entity
        {
            entities.Add(_entity);
            _entity.Initialize(this, _position, _entityValues);
        }

        ///<summary>
        /// Returns the first entity in list of given type
        ///</summary>
        public T GetEntityOfType<T>() where T : Entity  // TODO: Wrap these in try catch statements
        {
            foreach (Entity entity in entities)
            {
                if (entity is T)
                    return entity as T;
            }

            return null;
        }

        ///<summary>
        /// Returns all of the entities in list of given type as a List<Entity>
        ///</summary>
        public List<T> GetEntitiesOfType<T>() where T : Entity
        {
            List<T> entitiesToReturn = new List<T>();

            foreach (Entity entity in entities)
            {
                if (entity is T)
                    entitiesToReturn.Add(entity as T);
            }

            return entitiesToReturn;
        }

        public List<E> GetEntitiesWithComponent<T,E>() where T : Component where E : Entity
        {
            List<E> entitiesToReturn = new List<E>();

            for (int i = 0; i < entities.Count; ++i)
            {
                if (entities[i].HasComponent<T>())
                {
                    entitiesToReturn.Add(entities[i] as E);
                }
            }

            return entitiesToReturn;
        }

        public List<Entity> GetEntitiesByTag(string _tag)
        {
            //run a for loop thru entities checking each ones tag, if it lines up with parameter then return it
            return new List<Entity>();
        }


        public List<T> GetComponents<T>() where T : Component
        {
            List<T> componentsToReturn = new List<T>();

            for (int i = 0; i < entities.Count; ++i)
            {
                if (entities[i].HasComponent<T>())
                {
                    componentsToReturn.Add(entities[i].GetComponent<T>());
                }
            }

            return componentsToReturn;
        }

        ///<summary>
        /// Removes given entity from scene
        ///</summary>
        public void RemoveEntity(Entity _entity)
        {
            entities.Remove(_entity);
        }

        ///<summary>
        /// Removes all entities from the scene
        ///</summary>
        public void RemoveAllEntities()
        {
            entities = new List<Entity>();
        }
    }
}