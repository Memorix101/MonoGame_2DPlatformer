using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

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

        /// <summary>
        /// Declare your stuff here
        /// </summary>

        SpriteBatch spriteBatch;

        Sprite sp_tilesheet;
        // ItemTile it;

        MapLoader ml;

        Level level;

        GUI testText; 

        Song ms_rainbow_ride;

        float timeTest;

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
            GameDebug.Log("Hello World");

            //sheetLoader = new SheetLoader();

            ml = new MapLoader();
            level = new Level();

            testText = new GUI();
            testText.Text(some_text);

            sp_tilesheet = new Sprite();
            sp_tilesheet.Init(Vector2.Zero, new Rectangle(0, 0, 32, 32), Color.White);

           // it = new ItemTile();
           // it.Init(Vector2.Zero, 1);


            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ms_rainbow_ride = Content.Load<Song>("Music\\Rainbow_Ride");
            //  MediaPlayer.Play(ms_rainbow_ride);

           // sp_tilesheet.Load("Sprites\\mini");

            level.LoadLevel("test.txt");

            //  SheetLoader.LoadSheet(this);
            //ml.LoadSheet(this);

          //  it.LoadBlock();

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

            // timeTest += 1 *Time.DeltaTime;

            // GameDebug.Log(timeTest.ToString());
            // TODO: Add your update logic here

            level.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.TransparentBlack);
            
            //  ml.Draw(spriteBatch);

            // spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            //spriteBatch.DrawString(testText, some_text, new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - testText.MeasureString(some_text).X * 3, 0), Color.White);

            // sp_tilesheet.Draw(spriteBatch);
            //it.Draw(spriteBatch);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            level.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();

            testText.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }


    }
}
