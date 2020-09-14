using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace MonoRPG
{
    public class TilemapEntityLoader
    {
        EntityManager entityManager;

        public TilemapEntityLoader(EntityManager _entityManager)
        {
            entityManager = _entityManager;
        }
        
        ///<summary>
        /// Loads an entity by string name, also sets Entity's inital position and any values that are set in Ogmo for the entity
        ///</summary>
        public void LoadEntity(string _entityName, Vector2 _position, Level.LevelEntityValues _entityValues)
        {
            try
            {
                entityManager.AddEntity(GetEntity(_entityName), _position, _entityValues);
            }
            catch
            {
                Console.Error.WriteLine("ERROR: Entity name '" + _entityName + "' not found.");
            }
        }

        ///<summary>
        /// Returns a new instance of an entity by string name
        ///</summary>
        private Entity GetEntity(string _name)  // TODO: A poor method I think. Come up with something better some day.
        {
            switch(_name)
            {
                case "player":
                    return new Player();
                case "chest":
                    return new Chest();
                case "enemy":
                    return new Enemy();
                default:
                    return null;
            }
        }
    }
}