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
        }

        public static void SetSceneManager(LevelLoader _levelLoader, ContentManager _contentManager, EntityManager _entityManager, TilemapRenderer _tilemapRender)
        {
            LevelLoader = _levelLoader;
            ContentManager = _contentManager;
            EntityManager = _entityManager;
            TilemapRenderer = _tilemapRender;
        }

        private static void ClearScene()
        {
            TilemapRenderer.ClearTilemap();
            EntityManager.RemoveAllEntities();
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