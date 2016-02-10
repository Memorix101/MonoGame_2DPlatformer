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
       // public static DebugViewXNA DebugView { get; private set; }
        /// <summary>
        /// Declare your stuff here
        /// </summary>

        SpriteBatch spriteBatch;

        Level level;

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
            this.Window.Title = "MonoGame 2D Platformer";

        }

        protected override void Initialize()
        {
            ///GameDebug.Log("Hello World");

            level = new Level();

            testText = new GUI();
            testText.Text(some_text);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ms_rainbow_ride = Content.Load<Song>("Music\\Rainbow_Ride");
            //  MediaPlayer.Play(ms_rainbow_ride);

            PhysicLoad();

            level.LoadLevel("test.txt");

            testText.Load("Fonts\\Pixel");
            testText.Position = new Vector2(Screen.width/2 - testText.Size.X/2, 0);
            testText.Color = Color.Red;

        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            _gameTime = gameTime;
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
                        
            world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));

            level.Update(gameTime);

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


            /// CURRENT FAKE CAMERA
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Game1.graphics.GraphicsDevice.Viewport.Width / 2, Game1.graphics.GraphicsDevice.Viewport.Height / 2, 0, 0, 1);
            Matrix view = Matrix.Identity;

            view = Matrix.CreateTranslation(16, 16, 0); // correction the view after Farseer pixels to meter convertation

            Matrix view2 = Matrix.CreateScale(32);
            view2 *= view;

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, view);
            level.Draw(spriteBatch);
            testText.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }


    }
}
