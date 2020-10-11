using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

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


            // This bit should all be handled in a class called OnScreenGui, which will be created here and destroyed when scene changes
            Player player = EntityManager.GetEntityOfType<Player>();

            BottomBar bottomBar = new BottomBar();
            EntityCreator.CreateUIEntity<BottomBar>(bottomBar);

            int barSpawnY = bottomBar.DestinationRect.Y + 16;

            HealthBar health = new HealthBar(player, new Vector2(16 * 5, barSpawnY));
            EntityCreator.CreateUIEntity<HealthBar>(health);

            // Magic Bar goes here
            MagicBar magic = new MagicBar(player, new Vector2(Screen.Width - (16 * 5) - health.DestinationRect.Width, barSpawnY));
            EntityCreator.CreateUIEntity<MagicBar>(magic);

            SkillBar skillBar = new SkillBar(player);
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