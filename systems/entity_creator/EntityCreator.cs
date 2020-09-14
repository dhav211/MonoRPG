using Microsoft.Xna.Framework;

/*
This is a quick and dirty way to create both game entities and ui entities.
Since it is static it can be called anywhere at anytime, however it does need to be initialized with both managers in Game1.cs
*/

namespace MonoRPG
{
    public static class EntityCreator
    {
        static EntityManager entityManager;
        static UIEntityManager uiEntityManager;

        public static void Initialize(EntityManager _entityManager, UIEntityManager _uiEntityManager)
        {
            entityManager = _entityManager;
            uiEntityManager = _uiEntityManager;
        }

        public static void CreateEntity<T>(T _entity, Vector2 _position) where T : Entity
        {
            entityManager.AddEntity<T>(_entity, _position);
        }

        public static void CreateUIEntity<T>(T _uiEntity) where T : UIEntity
        {
            uiEntityManager.AddEntity<T>(_uiEntity);
        }
    }
}