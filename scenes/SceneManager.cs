using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MonoRPG
{
    public static class SceneManager
    {
        public static Scene CurrentScene { get; private set; }
        static LevelLoader LevelLoader;
        static ContentManager ContentManager;
        static EntityManager EntityManager;
        static UIEntityManager UIEntityManager;
        static TilemapRenderer TilemapRenderer;

        public static void LoadScene(string _sceneName)
        {
            if (CurrentScene != null)
            {
                ClearScene();
            }

            Scene sceneToLoad = GetScene(_sceneName);
            LevelLoader.LoadLevel(sceneToLoad.LevelAddress, ContentManager.Load<Texture2D>(sceneToLoad.TilesetAddress));
            sceneToLoad.SetEntities(EntityManager);
            CurrentScene = sceneToLoad;

            SkillBar skillBar = new SkillBar(EntityManager.GetEntityOfType<Player>());
            EntityCreator.CreateUIEntity<SkillBar>(skillBar);
            // TODO: The skill bar should probably be added to the ui entity manager for player equipping skills.
        }

        public static void SetSceneManager(LevelLoader _levelLoader, ContentManager _contentManager, EntityManager _entityManager, UIEntityManager _uiEntityManager, TilemapRenderer _tilemapRender)
        {
            LevelLoader = _levelLoader;
            ContentManager = _contentManager;
            EntityManager = _entityManager;
            UIEntityManager = _uiEntityManager;
            TilemapRenderer = _tilemapRender;
        }

        private static void ClearScene()
        {
            TilemapRenderer.ClearTilemap();
            EntityManager.RemoveAllEntities();
            UIEntityManager.RemoveAllEntities();
        }

        private static Scene GetScene(string _sceneName)
        {
            switch(_sceneName)
            {
                case "TestDungeonL1":
                    return new TestDungeonL1();
            }
            return new Scene();
        }
    }
}