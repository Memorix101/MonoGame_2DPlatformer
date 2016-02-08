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
        /// 
        public static GameTime _gameTime { get; private set; }
        public static GraphicsDeviceManager graphics { get; private set; }
        public static ContentManager content { get; private set; }
        public static World world { get; private set; }
       // public static DebugViewXNA DebugView { get; private set; }
        /// <summary>
        /// Declare your stuff here
        /// </summary>

        Camera2D Camera;

        SpriteBatch spriteBatch;

        Level level;

        GUI testText;

        Song ms_rainbow_ride;

        string some_text = "2D Platformer";

        public Game1()
        {
            content = Content;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Screen.fullScreen = false;
            //     Screen.resolution(1280, 720);
            
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
            Camera.Update(gameTime);

            base.Update(gameTime);
        }


        private void PhysicLoad()
        {

            if (world == null)
            {
                world = new World(new Vector2(0, 1));
            }
            else
            {
                world.Clear();
            }

Camera = new Camera2D(graphics.GraphicsDevice);

            /*
            if (DebugView == null)
            {

                Console.WriteLine("DEBUUUUUUUUUUUG!!!!");
                
                //projection = Matrix.CreateOrthographicOffCenter(0, ConvertUnits.ToSimUnits(Game1.graphics.GraphicsDevice.Viewport.Width), ConvertUnits.ToSimUnits(Game1.graphics.GraphicsDevice.Viewport.Height), 0, 0, 1);
                //view = Matrix.Identity;
                //Debug view
                DebugView = new DebugViewXNA(world);
                DebugView.RemoveFlags(DebugViewFlags.Shape);
                DebugView.RemoveFlags(DebugViewFlags.DebugPanel);
                DebugView.RemoveFlags(DebugViewFlags.Joint);
                DebugView.DefaultShapeColor = Color.White;
                DebugView.SleepingShapeColor = Color.LightGray;
                DebugView.LoadContent(graphics.GraphicsDevice, Content);
            }

            */
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.TransparentBlack);

            // spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            //spriteBatch.DrawString(testText, some_text, new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - testText.MeasureString(some_text).X * 3, 0), Color.White);
            
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

          //  spriteBatch.Begin();

            //DebugView.RenderDebugData(ref Camera.SimProjection, ref Camera.SimView);


            level.Draw(spriteBatch);
      //      Matrix proj = Matrix.CreateOrthographicOffCenter(0f, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height, 0f, 0f, 1f);
             
            spriteBatch.End();

              
            
            /*
            spriteBatch.Begin();
            testText.Draw(spriteBatch);
            spriteBatch.End();
            */

            base.Draw(gameTime);
        }


    }
}
