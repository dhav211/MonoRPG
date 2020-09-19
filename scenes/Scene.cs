namespace MonoRPG
{
    public class Scene
    {
        public string Name { get; set; }
        public string TilesetAddress { get; set; }
        public string LevelAddress { get; set; }
        
        protected EntityManager entityManager;

        public void SetEntities(EntityManager _entityManager)
        {
            entityManager = _entityManager;
            SetChests();
        }

        public virtual void SetChests() {}
    }
}