using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using MonoGame_2DPlatformer;
using MonoGame_2DPlatformer.Core;

using FarseerPhysics.DebugView;

namespace MonoGame_2DPlatformer
{
    static class Level
    {
        //Sky and Background
        static Texture2D sky, clouds, clouds2, mountains;
        static float cloudMove, cloudMove2;
        static Vector2 cloudOrigin, cloudOrigin2;

        static Player player;
        static Exit exit;
        static TestActor testActor;

        static List<ItemTile> mapItems;
        static List<Coins> mapCoins;
        static List<Enemy> mapEnemies;

        static GUI coinFont = new GUI(string.Empty);

        static int coins = 0;

        static bool levelDone;

        static SoundEffect sn_coin;
        static SoundEffect sn_level_exit;

        public static void Dispose()
        {
            foreach (ItemTile t in mapItems)
            {
                t.GetRigidbody.Dispose();
            }

            foreach (Coins c in mapCoins)
            {
                c.GetRigidbody.Dispose();
            }

            foreach (Enemy e in mapEnemies)
            {
                e.GetRigidbody.Dispose();
            }

            // Yes, I know a list would be faster :P 
            if (player != null)
            {
                player = null;
            }

            if (exit != null)
            {
                exit = null;
            }

            if (testActor != null)
            {
                testActor.body.Dispose();
                testActor = null;
            }

            Game1.camera.Reset();

            mapEnemies.Clear();
            mapItems.Clear();
            mapCoins.Clear();
        }

        public static void Init()
        {
            //     sky = Game1.content.Load<Texture2D>("Sprites/kenney_32x32");
            sky = Game1.content.Load<Texture2D>("Sprites/sky");
            clouds = Game1.content.Load<Texture2D>("Sprites/clouds");
            clouds2 = Game1.content.Load<Texture2D>("Sprites/clouds");
            mountains = Game1.content.Load<Texture2D>("Sprites/mountains");

            sn_coin = Game1.content.Load<SoundEffect>("Sounds/Pickup_Coin");
            sn_level_exit = Game1.content.Load<SoundEffect>("Sounds/magic_01");

            coinFont.Load("Fonts/70sPixel_20");
            coinFont.Position = new Vector2(5, 5);
            coinFont.Color = Color.OrangeRed;
        }

        //level loader
        public static void LoadLevel(string name)
        {
            if (mapItems == null)
                mapItems = new List<ItemTile>();

            if (mapCoins == null)
                mapCoins = new List<Coins>();

            if (mapEnemies == null)
                mapEnemies = new List<Enemy>();

            levelDone = false;

            coins = 0;

            string filePath = Game1.content.RootDirectory.ToString() + "\\Levels\\" + name + ".map";

            int x = 0;
            int y = 0;

            foreach (string line in File.ReadAllLines(filePath))
            {
                x = 0;

                foreach (char token in line)
                {
                    switch (token)
                    {
                        // Blank space
                        case '.':
                            // mapItems.Add(new ItemTile(new Vector2(x * 32, y * 32), 0f, ItemTileType.Blank));
                            break;

                        case '#':
                            mapItems.Add(new ItemTile(new Vector2(x * 32, y * 32), 1f, ItemTileType.Block));
                            break;

                        case '+':
                            mapItems.Add(new ItemTile(new Vector2(x * 32, y * 32), 1f, ItemTileType.BlockC));
                            break;

                        case '-':
                            mapItems.Add(new ItemTile(new Vector2(x * 32, y * 32), 1f, ItemTileType.DarkBrick));
                            break;

                        case 'c':
                            //mapItems.Add(new ItemTile(new Vector2(x * 32, y * 32), 1f, ItemTileType.Coin));
                            mapCoins.Add(new Coins(new Vector2(x * 32, y * 32)));
                            break;

                        case 'p':
                            player = new Player(new Vector2(x * 32, y * 32));
                            break;

                        case 'q':
                            testActor = new TestActor(new Vector2(x * 32, y * 32));
                            break;

                        case 'x':
                            exit = new Exit(new Vector2(x * 32, y * 32));
                            break;

                        case 'e':
                            mapEnemies.Add(new Enemy(new Vector2(x * 32, y * 32)));
                            break;

                            // Unknown tile type character
                            //    default:
                            //      throw new Exception(String.Format("Wrong Char"));
                    }
                    x++;
                }

                y++;
            }

        }

        public static void Update(GameTime gameTime)
        {

            coinFont.Text("Coins: " + coins);

            if (player != null)
            {
                if (!levelDone)
                {
                    player.Update(gameTime);
                }
                else
                {
                    GameUI.Display(GameUIState.Clear);
                }

                if (player.isDead)
                {
               //     player = null;

                    //some UI here
                    GameUI.Display(GameUIState.Died);
                }
            }

            MoveClouds();

            CheckCollision(gameTime);

        }
        private static void CheckCollision(GameTime gameTime)
        {
            if (testActor != null)
                testActor.Update();

            if (player != null && exit != null) // I guess we are save now ^-^
            {
                exit.Update(gameTime);


                if (exit.TileBoundingBox.Intersects(player.TileBoundingBox) && !exit.rigidbody.IsDisposed)
                {
                    exit.rigidbody.Dispose();
                    levelDone = true;
                    sn_level_exit.Play();
                }
            }


            for (int i = mapEnemies.Count - 1; i >= 0; i--)
            {

                mapEnemies[i].Update(gameTime);
                
                if (mapEnemies[i].Bite && mapEnemies[i] != null)
                {
                    if (player != null)
                    {
                        player.ReceiveDamage();
                    }
                }

            }

            for (int i = mapEnemies.Count - 1; i >= 0; i--)
            {
                if (mapEnemies[i].IsKilled && mapEnemies[i] != null)
                {
                    mapEnemies[i].rigidbody.Dispose();
                    //        mapEnemies.RemoveAt(i);
                }
                    
            }

            for (int i = mapCoins.Count - 1; i >= 0; i--)
            {
                mapCoins[i].Update(gameTime);

                if (player != null)
                {
                    if (mapCoins[i].TileBoundingBox.Intersects(player.TileBoundingBox))
                    {
                        coins += 1;
                        sn_coin.Play();
                        mapCoins[i].rigidbody.Dispose();
                        mapCoins.RemoveAt(i);
                    }
                }
            }
            
        }

        public static void HUD(SpriteBatch spriteBatch)
        {
            if (player != null)
                coinFont.Draw(spriteBatch);
        }

        public static void Sky(SpriteBatch spriteBatch)
        {
            Rectangle screenRectangle = new Rectangle(0, 0, Screen.width, Screen.height);
            //   Rectangle skyRect = new Rectangle(930, 34, 16*32, 8*32);
            // spriteBatch.Draw(sky, screenRectangle, skyRect, Color.White);
            spriteBatch.Draw(sky, screenRectangle, Color.White);
            spriteBatch.Draw(clouds, screenRectangle, null, Color.WhiteSmoke, 0f, cloudOrigin, SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(clouds2, screenRectangle, null, Color.White, 0f, cloudOrigin2, SpriteEffects.None, 0);
            spriteBatch.Draw(mountains, screenRectangle, Color.White);
        }

        static void MoveClouds()
        {

            // This stuff is ugly !!!!

            const float speed = 1f;
            const float offset = 300f;
            cloudOrigin = new Vector2(cloudMove, 0);
            cloudOrigin2 = new Vector2(cloudMove2 + offset, 0);

            cloudMove += speed * Time.DeltaTime;
            cloudMove2 += speed * Time.DeltaTime;

            if (cloudOrigin.X >= clouds.Width)
            {
                cloudMove = -clouds.Width;
            }

            if (cloudOrigin2.X >= clouds2.Width)
            {
                cloudMove2 = -clouds2.Width - offset;
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (ItemTile it in mapItems)
            {
                it.Draw(spriteBatch);
            }

            foreach (Coins c in mapCoins)
            {
              c.Draw(spriteBatch);
            }

            foreach (Enemy e in mapEnemies)
            {
                e.Draw(spriteBatch);
            }

            if (exit != null)
                exit.Draw(spriteBatch);

            if (player != null)
                player.Draw(spriteBatch);

            if (testActor != null)
                testActor.Draw(spriteBatch);

            SpriteBatchEx.GraphicsDevice = Game1.graphics.GraphicsDevice;
          //  spriteBatch.DrawLine(new Vector2(ray.Position.X, ray.Position.Y), new Vector2(ray.Position.X + ray.Direction.X, ray.Position.Y + ray.Direction.Y), Color.Red);
        }

    }
}

