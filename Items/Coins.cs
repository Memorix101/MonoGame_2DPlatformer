using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using MonoGame_2DPlatformer.Core;

namespace MonoGame_2DPlatformer
{
    class Coins : Sprite
    {
        Texture2D texture;
        //Vector2 p_pos;
        Rectangle coinRect;

        float speed = 10f;

        float time;
        float rectFrame = 1;

        public Coins(Vector2 p)
        {
            this.tile = texture = Game1.content.Load<Texture2D>("Sprites/coins_gold");
            this.Position = p;

        }

        public void Update(GameTime gameTime)
        {
            this.Rect = coinRect;
            time += speed * Time.DeltaTime;
            rectFrame += speed * Time.DeltaTime;

            if (time >= 8f)
            {
                rectFrame = 1f;
                time = 0F;
            }
           
           // GameDebug.Log("HIIII " + time);

            if (rectFrame >= 1f)
                coinRect = new Rectangle(0, 0, 32, 32);
            if (rectFrame >= 2f)
                coinRect = new Rectangle(32, 0, 32, 32);
            if (rectFrame >= 3f)
                coinRect = new Rectangle(64, 0, 32, 32);
            if (rectFrame >= 4f)
                coinRect = new Rectangle(96, 0, 32, 32);
            if (rectFrame >= 5f)
                coinRect = new Rectangle(128, 0, 32, 32);
            if (rectFrame >= 6f)
                coinRect = new Rectangle(160, 0, 32, 32);
            if (rectFrame >= 7f)
                coinRect = new Rectangle(192, 0, 32, 32);
            if (rectFrame >= 8f)
                coinRect = new Rectangle(224, 0, 32, 32);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, coinRect, Color.White);
            //base.Draw(spriteBatch);
        }
    }
}
