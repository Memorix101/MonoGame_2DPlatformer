using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Dynamics;
using FarseerPhysics.DebugView;
using FarseerPhysics;
using System;
//My own imports
using MonoGame_2DPlatformer;
using MonoGame_2DPlatformer.Core;


namespace MonoGame_2DPlatformer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {

        /// <summary>
        ///  public static pass "stuff"       
        /// </summary>
        public static GameTime _gameTime { get; private set; }
        public static GraphicsDeviceManager graphics { get; private set; }
        public static ContentManager content { get; private set; }
        public static World world { get; private set; }
        public static Camera2D camera { get; private set; }
        public static PhysicsDebugCam DebugCam { get; private set; }
        public static DebugViewXNA DebugView { get; private set; }
        public static bool debugActive { get; private set; }
        public static string levelName = "menu";
        public static bool quit;
        /// <summary>
        /// Declare your stuff here
        /// </summary>

        SpriteBatch spriteBatch;
    //    static Level level;
        GUI testText;
        Song ms_rainbow_ride;


        string some_text = "EARLY PRE-ALPHA CODE";

        public Game1()
        {
            content = Content;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Screen.fullScreen = false;
            //Screen.resolution(1280, 720);
            this.Window.Title = "Super Fuzlib";

        }

        protected override void Initialize()
        {
            ///GameDebug.Log("Hello World");

            //  level = new Level();
            Level.Init();
            camera = new Camera2D();
            DebugCam = new PhysicsDebugCam();
            testText = new GUI(string.Empty);
            testText.Text(some_text);

            PhysicLoad();
            Level.LoadLevel(levelName);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ms_rainbow_ride = Content.Load<Song>("Music\\Rainbow_Ride");
          //  MediaPlayer.Play(ms_rainbow_ride);
            
            testText.Load("Fonts\\Pixel");
            testText.Position = new Vector2(Screen.width / 2 - testText.Size.X / 2, 0);
            testText.Color = Color.Red;
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            _gameTime = gameTime;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) || quit)
                Exit();
                        
            world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));

            camera.Update();
            DebugCam.Update();

            Menu.Update();

            Level.Update(gameTime);

            base.Update(gameTime);
        }


        private void PhysicLoad()
        {

            if (world == null)
            {

                // Farseer expects objects to be scaled to MKS (meters, kilos, seconds)
                ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);  // 1 meters equals 64 pixels here

                //Create a world with gravity.
                world = new World(new Vector2(0, 9.82f));
            }
            else
            {
                world.Clear();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.TransparentBlack);

         
            //Sky
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            Level.Sky(spriteBatch);
            spriteBatch.End();


            // Level stuff

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.view);
            Level.Draw(spriteBatch);
            spriteBatch.End();

          #if DEBUG
            DebugDraw();
          #endif

            //GUI Stuff

            spriteBatch.Begin();
            testText.Draw(spriteBatch);
            Level.HUD(spriteBatch);
            Menu.Draw(spriteBatch);
            spriteBatch.End();


            base.Draw(gameTime);
        }

        void DebugDraw()
        {
            Matrix view2 = Matrix.CreateScale(32); //default 32
            view2 *= Game1.DebugCam.view;

            debugActive = true;

            DebugView = new DebugViewXNA(Game1.world);
            DebugView.AppendFlags(FarseerPhysics.DebugViewFlags.DebugPanel);
            DebugView.DefaultShapeColor = Color.Red;
            DebugView.SleepingShapeColor = Color.Green;
            DebugView.StaticShapeColor = Color.Violet;
            DebugView.LoadContent(Game1.graphics.GraphicsDevice, Game1.content);
            DebugView.RenderDebugData(ref Game1.DebugCam.projection, ref view2);
        }


    }
}
