using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame_2DPlatformer;
using MonoGame_2DPlatformer.Core;

namespace MonoGame_2DPlatformer
{
    class Level
    {

        Texture2D clouds;
        Texture2D mountains;

        Player player;
        TestActor testActor;

        Ray ray = new Ray();
        
        List<ItemTile> mapItems;
        List<Coins> mapCoins;

        GUI coinFont = new GUI();

        int coins = 0;

        //level loader
        public void LoadLevel(string name)
        {

            clouds = Game1.content.Load<Texture2D>("Sprites/clouds");
            mountains = Game1.content.Load<Texture2D>("Sprites/mountains");

            coinFont.Load("Fonts/70sPixel_20");
            coinFont.Color = Color.Yellow;

            mapItems = new List<ItemTile>();
            mapCoins = new List<Coins>();

            string filePath = Game1.content.RootDirectory.ToString() + "\\Levels\\" + name;

            int x = 0;
            int y = 0;

            foreach (string line in File.ReadAllLines(filePath))
            {
                x = 0;

                foreach(char token in line)
                {
                    switch(token)
                    {
                        // Blank space
                        case '.':
                            mapItems.Add(new ItemTile(new Vector2(x * 32, y * 32), 0f, ItemTileType.Blank));
                            break;

                        case '#':
                            mapItems.Add(new ItemTile(new Vector2(x * 32, y * 32), 1f, ItemTileType.Block));
                            break;

                        case 'c':
                            //mapItems.Add(new ItemTile(new Vector2(x * 32, y * 32), 1f, ItemTileType.Coin));
                           mapCoins.Add(new Coins(new Vector2(x * 32, y * 32)));
                            break;

                        case 'p':
                            player = new Player(new Vector2(x * 32, y * 32));
                            testActor = new TestActor();
                            break;

                        // Unknown tile type character
                        default:
                            throw new Exception(String.Format("Wrong Char"));
                    }
                    x++;
                }

                y++;
            }

        }

        public void Update(GameTime gameTime)
        {

            coinFont.Text("Coins: " + coins);

            player.Update(gameTime);
            CheckCollision(gameTime);

       //     Collision(player.TileBoundingBox);

        }

        public bool Collision(Rectangle rectangleToCheck)
        {
                foreach (var obj in mapItems)
            {
                if (obj.TileBoundingBox.Intersects(rectangleToCheck))
                {
                    return false;
                    GameDebug.Log("Moo");
                }
            }
            return true;
        }

        private void CheckCollision(GameTime gameTime)
        {

            testActor.Update();

       //     ray.Position = new Vector3(player.Position.X + player.Rect.Width/2, player.Position.Y + player.Rect.Height, 0);
            ray.Direction = new Vector3(0, 1, 0);
            
            bool intersect = false;

            foreach (ItemTile o in mapItems)
            {

                o.UpdateCollision(gameTime);

                /*
                if (player.TileBoundingBox.Intersects(o.TileBoundingBox) && o.Type == ItemTileType.Block)
                {
                    intersect = true;
                    player.isGrounded = intersect;
                }
                else if (!intersect)
                {
                    player.isGrounded = intersect;
                }
                
                float distance = 5 + player.Velocity.Y * Time.DeltaTime;

                var result = ray.Intersects(o.BoundingBox);

                if (result.HasValue && result.Value < distance && o.Type == ItemTileType.Block)
                {
                    intersect = true;
                    player.isGrounded = intersect;
                }
                else if (!intersect)
                {
                    player.isGrounded = intersect;
                }
                */
            }
            /*
            for (int i = mapCoins.Count - 1; i >= 0; i--)
            {
                mapCoins[i].Update(gameTime);

                if (mapCoins[i].TileBoundingBox.Intersects(player.TileBoundingBox))
                {
                    coins += 1;
                    mapCoins.RemoveAt(i);
                }
            }
            */
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            
           // batch.GraphicsDevice.SamplerStates[1].Filter = TextureFilter.Point;
            Rectangle screenRectangle = new Rectangle(0, 0, Screen.width, Screen.height);
            spriteBatch.Draw(clouds, screenRectangle, Color.White);
            spriteBatch.Draw(mountains, screenRectangle, Color.White);

            coinFont.Draw(spriteBatch);

            foreach (ItemTile it in mapItems)
            {
                it.Draw(spriteBatch);
            }

            foreach (Coins c in mapCoins)
            {
              c.Draw(spriteBatch);
            }

            player.Draw(spriteBatch);
            testActor.Draw(spriteBatch);

            SpriteBatchEx.GraphicsDevice = Game1.graphics.GraphicsDevice;
            spriteBatch.DrawLine(new Vector2(ray.Position.X, ray.Position.Y), new Vector2(ray.Position.X + ray.Direction.X, ray.Position.Y + ray.Direction.Y), Color.Red);
        }

    }
}

