using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;

namespace MonoRPG
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D renderTarget;
        private RenderTarget2D guiRenderTarget;
        private RenderTarget2D textRenderTarget;
        private EntityManager entityManager;
        private UIEntityManager uIEntityManager;
        private TilemapRenderer tilemapRenderer;
        private LevelLoader levelLoader;
        private MapGrid grid;
        private Camera camera;
        private TurnManager turnManager;

        TextBox textBox;
        TextBox textBox2;

        // TODO this won't be consts as time goes on since the menu will allow resoultion change.
        // Detect user resolution and set screen width as that, then set vitrual screen width based on that
        // Example, at 1080P the virtual resolution is scaled down by 4, whereas in 720P it would be scaled at 3
        const int SCREEN_WIDTH = 1920;
        const int SCREEN_HEIGHT = 1080;
        const int VIRTUAL_SCREEN_WIDTH = 480;
        const int VIRTUAL_SCREEN_HEIGHT = 270;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.IsFullScreen = true;
        }
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            _graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            _graphics.ApplyChanges();

            renderTarget = new RenderTarget2D(GraphicsDevice, VIRTUAL_SCREEN_WIDTH, VIRTUAL_SCREEN_HEIGHT);
            guiRenderTarget = new RenderTarget2D(GraphicsDevice, VIRTUAL_SCREEN_WIDTH, VIRTUAL_SCREEN_HEIGHT);
            textRenderTarget = new RenderTarget2D(GraphicsDevice, SCREEN_WIDTH, SCREEN_HEIGHT);

            camera = new Camera(GraphicsDevice.Viewport);
            turnManager = new TurnManager();

            Screen.SetWidthHeight(VIRTUAL_SCREEN_WIDTH, VIRTUAL_SCREEN_HEIGHT);
            Screen.SetScale(4);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            grid = new MapGrid();
            entityManager = new EntityManager(this.Content, _spriteBatch, grid, camera, turnManager);
            uIEntityManager = new UIEntityManager(_spriteBatch, Content);
            EntityCreator.Initialize(entityManager, uIEntityManager);
            tilemapRenderer = new TilemapRenderer(_spriteBatch);
            levelLoader = new LevelLoader(tilemapRenderer, new TilemapEntityLoader(entityManager), grid, entityManager);
            SceneManager.SetSceneManager(levelLoader, Content, entityManager, tilemapRenderer);

            SceneManager.LoadScene("TestDungeonL1");
            //levelLoader.LoadLevel("levels/test_dungeon.json", this.Content.Load<Texture2D>("tilesets/temp_tileset"));
            // TODO: use this.Content to load your game content here
        }
  
        protected override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Time.Update(deltaTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            camera.Update(deltaTime);
            entityManager.Update(deltaTime);
            uIEntityManager.Update(deltaTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            DrawGameToRenderTarget(deltaTime);
            DrawGuiToRenderTarget(deltaTime);

            DrawGameRenderTargetToScreen();
            DrawGUIRenderTargetToScreen();

            base.Draw(gameTime);
        }

        void DrawGameToRenderTarget(float deltaTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(transformMatrix: camera.TransformationMatrix);
            tilemapRenderer.Draw();
            entityManager.Draw(deltaTime);
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
        }

        void DrawGameRenderTargetToScreen()
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);

            _spriteBatch.Draw(renderTarget, new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), new Rectangle(0, 0, VIRTUAL_SCREEN_WIDTH, VIRTUAL_SCREEN_HEIGHT), Color.White);
            
            _spriteBatch.End();
        }

        void DrawGuiToRenderTarget(float deltaTime)
        {
            //TODO: This will later have two loops draw scrollable objects (such has floating text and various popup menus)
            // and non scrollable (such as bottom menu, inventory, main menu, etc)

            // Draw GUI texture elements
            GraphicsDevice.SetRenderTarget(guiRenderTarget);
            GraphicsDevice.Clear(Color.Transparent);

            _spriteBatch.Begin(transformMatrix: camera.TransformationMatrix);
            uIEntityManager.Draw(deltaTime, true);
            _spriteBatch.End();

            _spriteBatch.Begin();
            uIEntityManager.Draw(deltaTime, false);
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            // Draw GUI text elements
            GraphicsDevice.SetRenderTarget(textRenderTarget);
            GraphicsDevice.Clear(Color.Transparent);

            _spriteBatch.Begin(transformMatrix: camera.TextTransformationMatrix);
            uIEntityManager.DrawText(deltaTime, true);
            _spriteBatch.End();

            _spriteBatch.Begin();
            uIEntityManager.DrawText(deltaTime, false);
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
        }

        void DrawGUIRenderTargetToScreen()
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);

            _spriteBatch.Draw(guiRenderTarget, new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), new Rectangle(0, 0, VIRTUAL_SCREEN_WIDTH, VIRTUAL_SCREEN_HEIGHT), Color.White);
            _spriteBatch.Draw(textRenderTarget, new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT), Color.White);

            _spriteBatch.End();
        }
    }
}
